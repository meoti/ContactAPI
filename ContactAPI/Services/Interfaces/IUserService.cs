using System.Threading.Tasks;

using ContactAPI.Data.Models;
using ContactAPI.Models;

namespace ContactAPI.Services.Interfaces
{
    public interface IUserService
    {
        Task<Results<User>> RegisterUser(User user, string password);

        Task<Results<User>> Login(string email, string password);

    }
}
