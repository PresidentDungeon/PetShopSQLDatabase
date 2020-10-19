using PetShop.Core.Entities;
using System.Collections.Generic;

namespace PetShop.Core.DomainService
{
    public interface IUserRepository
    {
        User AddUser(User user);
        IEnumerable<User> ReadUsers();
        User GetUserByID(int ID);
        User UpdateUser(User user);
        User DeleteUser(int ID);
    }
}
