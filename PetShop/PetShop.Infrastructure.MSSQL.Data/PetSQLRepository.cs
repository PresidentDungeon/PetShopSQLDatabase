﻿using Microsoft.EntityFrameworkCore;
using PetShop.Core.DomainService;
using PetShop.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PetShop.Infrastructure.SQLLite.Data
{
    public class PetSQLRepository : IPetRepository
    {
        private PetShopContext ctx;

        public PetSQLRepository(PetShopContext ctx)
        {
            this.ctx = ctx;
        }

        public Pet AddPet(Pet pet)
        {
            /* if(pet.Owner != null && ctx.ChangeTracker.Entries<Owner>().FirstOrDefault(ce => ce.Entity.ID == pet.Owner.ID) == null)
             {
                 ctx.Attach(pet.Owner);
             }

             var petCreated = ctx.Pets.Add(pet);
             ctx.SaveChanges();
             return petCreated.Entity; */

            ctx.Attach(pet).State = EntityState.Added;
            ctx.SaveChanges();

            return pet;

        }

        public IEnumerable<Pet> ReadPets()
        {
            return ctx.Pets.Include(pet => pet.Type).AsEnumerable();
        }

        public IEnumerable<Pet> ReadPetsFilterSearch(Filter filter)
        {
            IEnumerable<Pet> pets = ctx.Pets.Include(pet => pet.Type).AsEnumerable();

            if (!string.IsNullOrEmpty(filter.PetType))
            {
                pets = from x in pets where x.Type.Name.ToLower().Equals(filter.PetType.ToLower()) select x;
            }
            if (!string.IsNullOrEmpty(filter.Sorting) && filter.Sorting.ToLower().Equals("asc"))
            {
                pets = from x in pets where x.Owner == null orderby x.Price select x;
            }
            else if (!string.IsNullOrEmpty(filter.Sorting) && filter.Sorting.ToLower().Equals("desc"))
            {
                pets = from x in pets where x.Owner == null orderby x.Price descending select x;
            }

            return pets.AsEnumerable();
        }

        public IEnumerable<Pet> ReadPetsIncludeOwners()
        {
            return ctx.Pets.Include(pet => pet.Type).Include(pet => pet.Owner).AsEnumerable();
        }

        public Pet GetPetByID(int ID)
        {
            return ctx.Pets.AsNoTracking().Include(pet => pet.Type).Include(pet => pet.Owner).FirstOrDefault(x => x.ID == ID);
        }

        public Pet UpdatePet(Pet pet)
        {
            /* if(pet.Owner != null && ctx.ChangeTracker.Entries<Owner>().FirstOrDefault(ce => ce.Entity.ID == pet.Owner.ID) == null)
             {
                 ctx.Attach(pet.Owner);
             }
             else
             {
                 ctx.Entry(pet).Reference(pet => pet.Owner).IsModified = true;
             }

             var updatedPet = ctx.Pets.Update(pet);
             ctx.SaveChanges();
             return updatedPet.Entity;
            */

            ctx.Attach(pet).State = EntityState.Modified;
            ctx.Entry(pet).Reference(pet => pet.Owner).IsModified = true;
            ctx.Entry(pet).Reference(pet => pet.Type).IsModified = true;
            ctx.SaveChanges();
            return pet;
        }

        public Pet DeletePet(int ID)
        {
            var removedPet = ctx.Pets.Remove(GetPetByID(ID));
            ctx.SaveChanges();
            return removedPet.Entity;

        }
    }
}
