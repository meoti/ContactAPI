using System;
using System.Linq;
using System.Threading.Tasks;

using ContactAPI.Data.Models;
using ContactAPI.Data.Repository;
using ContactAPI.Models;
using ContactAPI.Services.Interfaces;

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ContactAPI.Services.Implementation
{
    public class UserService : IUserService
    {
        //private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        public UserService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public async Task<Results<User>> Login(string email, string password)
        {
            if ( string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) )
            {
                return new Results<User> { Errors = new[] { "Invalid Email or Password" }, Succeded = false };
            }

            User user = await FindUserAsync(email);

            Results<User> results = VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt);
            results.Value = user;
            return results;
        }

        private async Task<User> FindUserAsync(string email)
        {
            return await _repositoryWrapper.Users
                            .FindByConditionNoTracking(x => x.Email == email)
                            .SingleOrDefaultAsync();
        }

        public async Task<Results<User>> RegisterUser(User user, string password)
        {
            Results<User> results = new Results<User>() { Errors = Array.Empty<string>(), Succeded = false };
            if ( string.IsNullOrWhiteSpace(password) )
            {
                results.Errors = results.Errors.Append("Password cannot be null");
                return results;
            }

            User dbUser = await FindUserAsync(user.Email);
            if ( dbUser != null )
            {
                results.Errors = results.Errors.Append("User with email address already exists");
                return results;
            }

            (user.PasswordHash, user.PasswordSalt) = CreatePasswordHash(password);
            user.Id = Guid.NewGuid();
            _repositoryWrapper.Users.Create(user);
            int ItemSavedCount = await _repositoryWrapper.SaveAsync();

            if ( ItemSavedCount < 0 )
            {
                results.Errors = results.Errors.Append("A database error was produced trying to save data");
            }

            results.Succeded = true;
            results.Value = user;
            return results;
        }

        private static Results<User> VerifyPasswordHash(string password, string storedHash, string storedSalt)
        {
            Results<User> results = new Results<User>() { Errors = Array.Empty<string>() };
            if ( string.IsNullOrWhiteSpace(password) )
            {
                results.Errors = results.Errors.Append("Password cannot be empty");
                return results;
            }

            using ( System.Security.Cryptography.HMACSHA512 hmac = new System.Security.Cryptography.HMACSHA512(Convert.FromBase64String(storedSalt)) )
            {
                string computedHash = Convert.ToBase64String(hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password)));
                if ( computedHash.Equals(storedHash, StringComparison.Ordinal) )
                {
                    results.Succeded = true;
                }
                else
                {
                    results.Errors = results.Errors.Append("Passowrd hash mismatch");
                }
            }

            return results;
        }

        private static (string passwordHash, string passwordSalt) CreatePasswordHash(string password)
        {
            if ( string.IsNullOrWhiteSpace(password) )
            {
                throw new ArgumentException("Value cannot be empty or whitespace only string.", nameof(password));
            }

            string passwordHash;
            string passwordSalt;
            using ( System.Security.Cryptography.HMACSHA512 hmac = new System.Security.Cryptography.HMACSHA512() )
            {
                passwordSalt = Convert.ToBase64String(hmac.Key);
                passwordHash = Convert.ToBase64String(hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password)));
            }
            return (passwordHash, passwordSalt);
        }
    }
}
