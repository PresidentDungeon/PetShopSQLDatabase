using PetShop.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PetShop.Core.DomainService
{
    public interface IAuthenticationHelper
    {
        public string GenerateJWTToken(User user);
    }
}
