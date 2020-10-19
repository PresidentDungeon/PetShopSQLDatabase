using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using PetShop.Core.DomainService;
using PetShop.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

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

            IQueryable<Pet> pets = ctx.Pets.Include(pet => pet.Type).Include(pet => pet.petColors).ThenInclude(p => p.Color).AsQueryable();

            if (!string.IsNullOrEmpty(filter.Name))
            {
                pets = from x in pets where x.Name.Contains(filter.Name) select x;
            }
            if (!string.IsNullOrEmpty(filter.PetType))
            {
               // ctx.Pets.Where(p => p.Type.Name.ToLower().Equals(filter.PetType.ToLower()));
                pets = from x in pets where x.Type.Name.ToLower().Equals(filter.PetType.ToLower()) select x;
            }
            if (!string.IsNullOrEmpty(filter.Sorting) && filter.Sorting.ToLower().Equals("asc"))
            {
                //ctx.Pets.Where(p => p.Owner == null).OrderBy(p => p.Price);
                pets = from x in pets where x.Owner == null orderby x.Price select x;
            }
            else if (!string.IsNullOrEmpty(filter.Sorting) && filter.Sorting.ToLower().Equals("desc"))
            {
                //ctx.Pets.Where(p => p.Owner == null).OrderByDescending(p => p.Price);
                pets = from x in pets where x.Owner == null orderby x.Price descending select x;
            }

            return pets;
        }

        public IEnumerable<Pet> ReadPetsIncludeOwners()
        {
            return ctx.Pets.Include(pet => pet.Type).Include(pet => pet.Colors).Include(pet => pet.Owner).AsEnumerable();
        }

        public Pet GetPetByID(int ID)
        {
            return ctx.Pets.AsNoTracking().Include(pet => pet.Type).Include(pet => pet.petColors).ThenInclude(c => c.Color)
                .Include(pet => pet.Colors)
                .Include(pet => pet.Owner)
                .FirstOrDefault(x => x.ID == ID);
        }

        public Pet UpdatePet(Pet pet)
        {
            List<PetColor> petColors = ctx.PetColors.AsNoTracking().ToList();

            //fjerne alle der ikke bruges
            List<PetColor> color = petColors.Where(p => pet.petColors.All(p2 => p2.ColorID != p.ColorID) && p.PetID == pet.ID).ToList();
            ctx.PetColors.RemoveRange(color);

            //tilføje alle nye
            List<PetColor> colorsToAdd = pet.petColors.Where(p => petColors.All(p2 => p2.ColorID != p.ColorID || p2.PetID != pet.ID)).ToList();
            colorsToAdd.ForEach(x => x.PetID = pet.ID);
            ctx.PetColors.AddRange(colorsToAdd);
            //ctx.SaveChanges();

            pet.petColors = null;

            ctx.Attach(pet).State = EntityState.Modified;
            ctx.Entry(pet).Reference(pet => pet.Owner).IsModified = true;
            ctx.Entry(pet).Reference(pet => pet.Type).IsModified = true;

            ctx.SaveChanges();

            
            return GetPetByID(pet.ID);
        }

        public Pet DeletePet(int ID)
        {
            var removedPet = ctx.Pets.Remove(GetPetByID(ID));
            ctx.SaveChanges();
            return removedPet.Entity;
        }
    }
}
