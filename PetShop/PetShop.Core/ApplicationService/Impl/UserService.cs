using PetShop.Core.DomainService;
using PetShop.Core.Entities;
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

        public string GenerateHash(string password, string salt)
        {
            StringBuilder hash = new StringBuilder();

            String saltedPassword = salt + password;

                HashAlgorithm sha = SHA256.Create();
                byte[] hashedBytes = sha.ComputeHash(Encoding.ASCII.GetBytes(saltedPassword));

                for (int idx = 0; idx < hashedBytes.Length; ++idx)
                {
                    byte b = hashedBytes[idx];
                    hash.Append(((b & 0xf0) >> 4).ToString("X"));
                    hash.Append((b & 0x0f).ToString("X")); 
                }

            return hash.ToString();
        }

        public string GenerateSalt()
        {
            StringBuilder hash = new StringBuilder();
            Random r = new Random();
            byte[] salt = new byte[6];
            r.NextBytes(salt);

            foreach (byte b in salt)
            {
                int value = (b & 0xf0) >> 4;
                hash.Append(value.ToString("X"));
            }

            return hash.ToString();
        }

        public User Login(User user)
        {
            if (user.UserName == null || user.Password == null)
            {
                throw new UnauthorizedAccessException();
            }

            User foundUser = GetAllUsers().Where(u => u.UserName.Equals(user.UserName)).FirstOrDefault();

            if (foundUser == null)
            {
                throw new UnauthorizedAccessException();
            }

            string hashedPassword = GenerateHash(user.Password, foundUser.Salt);

            if (!hashedPassword.Equals(foundUser.Password))
            {
                throw new UnauthorizedAccessException();
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

            string generatedSalt = GenerateSalt();

            return new User
            {
                UserName = userName,
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
