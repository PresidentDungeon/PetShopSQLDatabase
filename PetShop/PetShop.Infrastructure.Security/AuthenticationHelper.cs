using Microsoft.IdentityModel.Tokens;
using PetShop.Core.DomainService;
using PetShop.Core.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PetShop.Infrastructure.Security
{
    public class AuthenticationHelper : IAuthenticationHelper
    {
        private byte[] SecretBytes;

        public AuthenticationHelper(byte[] secretBytes)
        {
            SecretBytes = secretBytes;
        }

        public string GenerateJWTToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(SecretBytes);
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.UserRole)
            };

            var token = new JwtSecurityToken(
               new JwtHeader(credentials),
               new JwtPayload(null, // issuer - not needed (ValidateIssuer = false)
                              null, // audience - not needed (ValidateAudience = false)
                              claims,
                              DateTime.Now,               // notBefore
                              DateTime.Now.AddMinutes(5)));  // expires

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
