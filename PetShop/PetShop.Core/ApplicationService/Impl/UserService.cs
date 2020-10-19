using PetShop.Core.DomainService;
using PetShop.Core.Entities;
using PetShop.Core.Entities.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace PetShop.Core.ApplicationService.Impl
{
    public class UserService: IUserService
    {
        IUserRepository UserRepository;

        public UserService(IUserRepository userRepository)
        {
            this.UserRepository = userRepository;
        }

        public byte[] GenerateHash(string password, byte[] salt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(salt))
            {
                return hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public byte[] GenerateSalt()
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                return hmac.Key;
            }
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

            byte[] hashedPassword = GenerateHash(inputModel.Password, foundUser.Salt);
            byte[] storedPassword = foundUser.Password;

            for (int i = 0; i < storedPassword.Length; i++)
            {
                if (storedPassword[i] != hashedPassword[i])
                {
                    throw new UnauthorizedAccessException("Entered password is incorrect");
                }
            }

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

            byte[] generatedSalt = GenerateSalt();

            return new User
            {
                Username = userName,
                Salt = generatedSalt,
                Password = GenerateHash(password, generatedSalt),
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
