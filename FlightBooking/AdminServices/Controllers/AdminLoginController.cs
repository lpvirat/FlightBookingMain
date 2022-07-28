using AdminServices.Models;
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

namespace AdminServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminLoginController : ControllerBase
    {
        private IConfiguration _config;
        public AdminLoginController(IConfiguration config)
        {
            _config = config;
        }

        [Route("Admins")]
        [HttpGet]
        public IList<AdminCredentials> GetAdmins()
        {
            using (var db = new AdminDBContext())
            {
                return db.AdminCredentials.ToList();
            }
        }

        [Route("AdminLogin")]
        [HttpPost]
        public IActionResult LoginAdmin([FromBody] AdminCredentials credObj)
        {
            if (credObj == null)
                return NotFound(new
                {
                    success = 0,
                    message = "LogIn Failed"
                });
            else
            {
                var db = new AdminDBContext();
                var admin = db.AdminCredentials.Where(a =>
                    a.Email == credObj.Email
                    && a.Password == credObj.Password).FirstOrDefault();
                if (admin != null)
                {
                    var token = GenerateTokenUsingJwt(credObj.Email);

                    return Ok(new
                    {
                        success = 1,
                        message = "Logged In Successfully",
                        token = token
                    });
                }
                else
                    return NotFound(new
                    {
                        success = 0,
                        message = "LogIn Failed"
                    });
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
