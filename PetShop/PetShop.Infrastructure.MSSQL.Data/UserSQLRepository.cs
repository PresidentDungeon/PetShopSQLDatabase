using Microsoft.EntityFrameworkCore;
using PetShop.Core.DomainService;
using PetShop.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PetShop.Infrastructure.SQLLite.Data
{
    public class UserSQLRepository : IUserRepository
    {
        private PetShopContext ctx;

        public UserSQLRepository(PetShopContext ctx)
        {
            this.ctx = ctx;
        }

        public User AddUser(User user)
        {
            ctx.Attach(user).State = EntityState.Added;
            ctx.SaveChanges();
            return user;
        }
        public IEnumerable<User> ReadUsers()
        {
            return ctx.Users.AsEnumerable();
        }
      
        public User GetUserByID(int ID)
        {
            return ctx.Users.AsNoTracking().FirstOrDefault(x => x.ID == ID);
        }
        public User UpdateUser(User user)
        {
            ctx.Attach(user).State = EntityState.Modified;
            ctx.SaveChanges();
            return user;
        }
        public User DeleteUser(int ID)
        {
            var deletedUser = ctx.Users.Remove(GetUserByID(ID));
            ctx.SaveChanges();
            return deletedUser.Entity;
        }
    }
}
