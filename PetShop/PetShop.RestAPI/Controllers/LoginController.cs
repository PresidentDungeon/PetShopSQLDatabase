using Microsoft.AspNetCore.Mvc;
using PetShop.Core.ApplicationService;
using PetShop.Core.DomainService;
using PetShop.Core.Entities;
using System;

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
                    ID = user.ID,
                    Username = user.Username,
                    Role = user.UserRole,
                    Token = tokenString
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }
    }
}
