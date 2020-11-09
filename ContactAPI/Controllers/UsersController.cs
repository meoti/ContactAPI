using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;

using ContactAPI.Data.Models;
using ContactAPI.Models;
using ContactAPI.Services;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ContactAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        public IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IServiceWrapper _serviceWrapper;

        public UsersController(
            IConfiguration configuration,
            IServiceWrapper serviceWrapper,
            IMapper mapper
        )
        {
            _configuration = configuration;
            _mapper = mapper;
            _serviceWrapper = serviceWrapper;
        }


        [HttpPost("authenticate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<string>> Authenticate([FromBody] LoginModel loginData)
        {
            Results<User> results = await _serviceWrapper.UserService.Login(loginData.Email, loginData.Password);
            if ( !results.Succeded )
            {
                return BadRequest(results.Errors);
            }
            string token = GenerateJwtToken(results.Value);

            return Ok(token);

        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<string>> Register([FromBody] RegisterModel registerData)
        {
            User user = _mapper.Map<User>(registerData);
            // user service create user, hashing password
            Results<User> results = await _serviceWrapper.UserService.RegisterUser(user, registerData.Password);
            if ( !results.Succeded )
            {
                return BadRequest(results.Errors);
            }
            string token = GenerateJwtToken(results.Value);
            return Ok(token);
        }

        private string GenerateJwtToken(User user)
        {
            Claim[] claims = new[]
            {
                    new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    new Claim("Id", user.Id.ToString()),
                    new Claim("Email", user.Email)
            };
            SymmetricSecurityKey secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]));
            SigningCredentials signInCred = new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddHours(24), // for test purpose
                signingCredentials: signInCred
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
