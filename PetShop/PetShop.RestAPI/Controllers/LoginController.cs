using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PetShop.Core.ApplicationService;
using PetShop.Core.Entities;

namespace PetShop.RestAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private IConfiguration Config;
        private IUserService UserService;


        public LoginController(IUserService userService, IConfiguration config)
        {
            UserService = userService;
            Config = config;
        }

        [HttpPost]
        public ActionResult Login([FromBody] User user)
        {
            User foundUser = UserService.GetAllUsers().Where(u => u.UserName.Equals(user.UserName)).FirstOrDefault();

            if (foundUser == null)
            {
                return Unauthorized();
            }

            string hashedPassword = UserService.GenerateHash(user.Password, foundUser.Salt);

            if (!hashedPassword.Equals(foundUser.Password))
            {
                return Unauthorized();
            }

            var tokenString = GenerateJWTToken(foundUser);

            return Ok(new
            {
                username = user.UserName,
                token = tokenString,
            });
        }

        string GenerateJWTToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Config["Jwt:SecretKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.UserRole)
            };

            var token = new JwtSecurityToken(
               new JwtHeader(credentials),
               new JwtPayload(Config["Jwt:Issuer"], // issuer - not needed (ValidateIssuer = false)
                              Config["Jwt:Audience"], // audience - not needed (ValidateAudience = false)
                              claims.ToArray(),
                              DateTime.Now,               // notBefore
                              DateTime.Now.AddMinutes(5)));  // expires

            return new JwtSecurityTokenHandler().WriteToken(token);



        }





    }
}
