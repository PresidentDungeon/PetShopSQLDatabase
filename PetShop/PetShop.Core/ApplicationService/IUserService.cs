using PetShop.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PetShop.Core.ApplicationService
{
    public interface IUserService
    {
        string GenerateHash(string password, string salt);

        string GenerateSalt();

        User Login(User user);

        User CreateUser(string userName, string password, string userRole);

        User AddUser(User user);

        List<User> GetAllUsers();

        User GetUserByID(int ID);

        User UpdateUser(User user, int ID);

        User DeleteUser(int ID);
    }
}
