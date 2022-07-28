using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Userservices.Models;

namespace Userservices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserLoginController : ControllerBase
    {
        private IConfiguration _config;
        public UserLoginController(IConfiguration config)
        {
            _config = config;
        }

        [Route("Users")]
        [HttpGet]
        public IList<UserCredentials> GetUsers()
        {
            using (var db = new UserDBContext())
            {
                return db.UserCredentials.ToList();
            }
        }

        [Route("UserLogin")]
        [HttpPost]
        public IActionResult UserLogin([FromBody] UserCredentials credentialsObj)
        {
            if (credentialsObj == null)
            {
                return NotFound(new
                {
                    success = 0,
                    message = "Please check your Credentials"
                });
            }
            else
            {
                var db = new UserDBContext();
                var user = db.UserCredentials.Where(a =>
                    a.Email == credentialsObj.Email
                    && a.Password == credentialsObj.Password
                    ).FirstOrDefault();
                var token = GenerateTokenUsingJwt(credentialsObj.Email);
                if (user != null)
                {
                    return Ok(new
                    {
                        success = 1,
                        message = "Logged In Successfully",
                        emailId = user.Email,
                        token = token
                    });
                }
                else
                {
                    return NotFound(new
                    {
                        success = 0,
                        message = "Please check your Credentials"
                    });
                }
            }
        }

        /// <summary>
        /// JWT Authentication
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public string GenerateTokenUsingJwt(string userName)
        {
            var signinngKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var signingCreds = new SigningCredentials(signinngKey, SecurityAlgorithms.HmacSha256);

            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Issuer"],
                claims: claims,
                signingCredentials: signingCreds,
                expires: DateTime.Now.AddMinutes(15)
            );

            var Jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return Jwt;
        }
    }
}
