using PetShop.Core.DomainService;
using PetShop.Core.Entities;
using PetShop.Core.Entities.Security;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PetShop.Core.ApplicationService.Impl
{
    public class UserService: IUserService
    {
        IUserRepository UserRepository;
        IAuthenticationHelper AuthenticationHelper;

        public UserService(IUserRepository userRepository, IAuthenticationHelper authenticationHelper)
        {
            this.UserRepository = userRepository;
            this.AuthenticationHelper = authenticationHelper;
        }

        public User Login(LoginInputModel inputModel)
        {
            if (inputModel.Username == null || inputModel.Password == null)
            {
                throw new UnauthorizedAccessException("Username or Password is non-existing");
            }

            User foundUser = GetAllUsers().Where(u => u.Username.Equals(inputModel.Username)).FirstOrDefault();

            if (foundUser == null)
            {
                throw new UnauthorizedAccessException("No user registered with such a name");
            }

            AuthenticationHelper.ValidateLogin(foundUser, inputModel);

            return foundUser;
        }

        public User CreateUser(string userName, string password, string userRole)
        {
            int minPasswordLenght = 5;

            if (string.IsNullOrEmpty(userName))
            {
                throw new ArgumentException("Entered username too short");
            }
            if (string.IsNullOrEmpty(password) || password.Length < minPasswordLenght)
            {
                throw new ArgumentException("Entered password too short");
            }
            if (string.IsNullOrEmpty(userRole))
            {
                throw new ArgumentException("Entered user role too short");
            }

            byte[] generatedSalt = AuthenticationHelper.GenerateSalt();

            return new User
            {
                Username = userName,
                Salt = generatedSalt,
                Password = AuthenticationHelper.GenerateHash(password, generatedSalt),
                UserRole = userRole
            };
        }

        public User AddUser(User user)
        {
            if (user != null)
            {
                return UserRepository.AddUser(user);
            }
            return null;
        }

        public List<User> GetAllUsers()
        {
            return UserRepository.ReadUsers().ToList();
        }

        public User GetUserByID(int ID)
        {
            return UserRepository.GetUserByID(ID);
        }

        public User UpdateUser(User user, int ID)
        {
            if (GetUserByID(ID) == null)
            {
                throw new ArgumentException("No user with such ID found");
            }
            if (user == null)
            {
                throw new ArgumentException("Updating user does not excist");
            }
            user.ID = ID;
            return UserRepository.UpdateUser(user);
        }

        public User DeleteUser(int ID)
        {
            if (ID <= 0)
            {
                throw new ArgumentException("Incorrect ID entered");
            }
            if (GetUserByID(ID) == null)
            {
                throw new ArgumentException("No user with such ID found");
            }
            return UserRepository.DeleteUser(ID);
        }
    }
}
