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
using PetShop.Core.DomainService;
using PetShop.Core.Entities;

namespace PetShop.RestAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private IUserService UserService;
        private IAuthenticationHelper AuthenticationHelper;

        public LoginController(IUserService userService, IAuthenticationHelper authenticationHelper)
        {
            UserService = userService;
            AuthenticationHelper = authenticationHelper;
        }

        [HttpPost]
        public ActionResult Login([FromBody] User user)
        {
            try
            {
                User foundUser = UserService.Login(user);

                if (foundUser == null)
                {
                    return Unauthorized();
                }

                var tokenString = AuthenticationHelper.GenerateJWTToken(foundUser);

                return Ok(new
                {
                    username = user.UserName,
                    token = tokenString,
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized();
            }
            
        }
    }
}
