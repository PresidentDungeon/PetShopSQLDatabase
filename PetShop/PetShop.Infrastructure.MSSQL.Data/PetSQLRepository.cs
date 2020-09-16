using Microsoft.EntityFrameworkCore;
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
            var petCreated = ctx.Pets.Add(pet);
            ctx.SaveChanges();
            return petCreated.Entity;
        }

        public IEnumerable<Pet> ReadPets()
        {
            return ctx.Pets.Include(pet => pet.Type).AsEnumerable();
        }

        public IEnumerable<Pet> ReadPetsIncludeOwners()
        {
            return ctx.Pets.Include(pet => pet.Type).Include(pet => pet.Owner).AsEnumerable();
        }

        public Pet GetPetByID(int ID)
        {
            return ctx.Pets.Include(pet => pet.Type).Include(pet => pet.Owner).FirstOrDefault(x => x.ID == ID);
        }

        public Pet UpdatePet(Pet pet)
        {
            var updatedPet = ctx.Pets.Update(pet);
            ctx.SaveChanges();
            return updatedPet.Entity;
        }

        public Pet DeletePet(int ID)
        {
            var removedPet = ctx.Pets.Remove(GetPetByID(ID));
            ctx.SaveChanges();
            return removedPet.Entity;

        }
    }
}
