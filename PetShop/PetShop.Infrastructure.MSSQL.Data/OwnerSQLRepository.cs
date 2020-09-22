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
            ctx.Attach(owner).State = EntityState.Added;
            ctx.SaveChanges();
            return owner;
        }

        public IEnumerable<Owner> ReadOwners()
        {
            return ctx.Owners.AsEnumerable();
        }

        public IEnumerable<Owner> ReadOwnersFilterSearch(Filter filter)
        {
            IQueryable<Owner> owners = ctx.Owners.AsQueryable();

            if (!string.IsNullOrEmpty(filter.Sorting) && filter.Sorting.ToLower().Equals("asc"))
            {
                owners = from x in owners orderby x.FirstName + x.LastName select x;
            }
            else if (!string.IsNullOrEmpty(filter.Sorting) && filter.Sorting.ToLower().Equals("desc"))
            {
                owners = from x in owners orderby x.FirstName + x.LastName descending select x;
            }

            return owners;
        }

        public Owner GetOwnerByID(int ID)
        {
            return ctx.Owners.AsNoTracking().FirstOrDefault(x => x.ID == ID);
        }

        public Owner GetOwnerByIDIncludePets(int ID)
        {
            return ctx.Owners.Include(owner => owner.Pets).ThenInclude(pet => pet.Type).FirstOrDefault(x => x.ID == ID);
        }

        public Owner UpdateOwner(Owner owner)
        {
            ctx.Attach(owner).State = EntityState.Modified;
            ctx.SaveChanges();
            return owner;
        }

        public Owner DeleteOwner(int ID)
        {
            /*var petsToUnregister = ctx.Pets.Where(pet => pet.Owner.ID == ID).ToList();
            petsToUnregister.ForEach(pet => pet.Owner = null);
            ctx.Pets.UpdateRange(petsToUnregister);*/

            var deletedOwner = ctx.Owners.Remove(GetOwnerByID(ID));
            ctx.SaveChanges();
            return deletedOwner.Entity;
        }
    }
}
