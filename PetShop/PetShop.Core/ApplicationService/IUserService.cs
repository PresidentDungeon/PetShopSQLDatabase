using PetShop.Core.Entities;
using PetShop.Core.Entities.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace PetShop.Core.ApplicationService
{
    public interface IUserService
    {
        byte[] GenerateHash(string password, byte[] salt);

        byte[] GenerateSalt();

        User Login(LoginInputModel inputModel);

        User CreateUser(string userName, string password, string userRole);

        User AddUser(User user);

        List<User> GetAllUsers();

        User GetUserByID(int ID);

        User UpdateUser(User user, int ID);

        User DeleteUser(int ID);
    }
}
