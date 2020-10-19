using Microsoft.AspNetCore.Mvc;
using PetShop.Core.ApplicationService;
using PetShop.Core.DomainService;
using PetShop.Core.Entities;
using PetShop.Core.Entities.Security;
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
        public ActionResult Login([FromBody] LoginInputModel inputModel)
        {
            try
            {
                User foundUser = UserService.Login(inputModel);

                if (foundUser == null)
                {
                    return Unauthorized();
                }

                var tokenString = AuthenticationHelper.GenerateJWTToken(foundUser);

                return Ok(new
                {
                    ID = foundUser.ID,
                    Username = foundUser.Username,
                    Role = foundUser.UserRole,
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
