using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using PetShop.Core.DomainService;
using PetShop.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PetShop.Infrastructure.SQLLite.Data
{
    public class OwnerSQLRepository : IOwnerRepository
    {

        private PetShopContext ctx;

        public OwnerSQLRepository(PetShopContext ctx)
        {
            this.ctx = ctx;
        }

        public Owner AddOwner(Owner owner)
        {
            var createdOwner = ctx.Owners.Add(owner);
            ctx.SaveChanges();
            return createdOwner.Entity;
        }

        public IEnumerable<Owner> ReadOwners()
        {
            return ctx.Owners.AsEnumerable();
        }

        public Owner GetOwnerByID(int ID)
        {
            return ctx.Owners.FirstOrDefault(x => x.ID == ID);
        }

        public Owner GetOwnerByIDIncludePets(int ID)
        {
            return ctx.Owners.Include(owner => owner.Pets).ThenInclude(pet => pet.Type).FirstOrDefault(x => x.ID == ID);
        }

        public Owner UpdateOwner(Owner owner)
        {
            var updatedOwner = ctx.Update(owner);
            ctx.SaveChanges();
            return updatedOwner.Entity;
        }

        public Owner DeleteOwner(int ID)
        {
            var deletedOwner = ctx.Owners.Remove(GetOwnerByID(ID));
            ctx.SaveChanges();
            return deletedOwner.Entity;
        }
    }
}
