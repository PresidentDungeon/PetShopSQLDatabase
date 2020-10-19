using PetShop.Core.Entities;
using PetShop.Core.Entities.Security;

namespace PetShop.Core.DomainService
{
    public interface IAuthenticationHelper
    {
        public byte[] GenerateHash(string password, byte[] salt);
        public byte[] GenerateSalt();
        public void ValidateLogin(User userToValidate, LoginInputModel inputModel);
        public string GenerateJWTToken(User user);
    }
}
