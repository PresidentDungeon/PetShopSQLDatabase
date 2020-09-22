using Microsoft.EntityFrameworkCore;
using PetShop.Core.DomainService;
using PetShop.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PetShop.Infrastructure.SQLLite.Data
{
    public class PetTypeSQLRepository : IPetTypeRepository
    {
        private PetShopContext ctx;

        public PetTypeSQLRepository(PetShopContext ctx)
        {
            this.ctx = ctx;
        }

        public PetType AddPetType(PetType type)
        {
            ctx.Attach(type).State = EntityState.Added;
            ctx.SaveChanges();
            return type;
        }
        public IEnumerable<PetType> ReadTypes()
        {
            return ctx.PetTypes.AsEnumerable();
        }

        public IEnumerable<PetType> ReadTypesFilterSearch(Filter filter)
        {
            IQueryable<PetType> types = ctx.PetTypes.AsQueryable();

            if (!string.IsNullOrEmpty(filter.Sorting) && filter.Sorting.ToLower().Equals("asc"))
            {
                types = from x in types orderby x.Name select x;
            }
            else if (!string.IsNullOrEmpty(filter.Sorting) && filter.Sorting.ToLower().Equals("desc"))
            {
                types = from x in types orderby x.Name descending select x;
            }

            return types;
        }

        public PetType GetPetTypeByID(int ID)
        {
            return ctx.PetTypes.AsNoTracking().FirstOrDefault(x => x.ID == ID);
        }

        public PetType UpdatePetType(PetType type)
        {
            ctx.Attach(type).State = EntityState.Modified;
            ctx.SaveChanges();
            return type;
        }

        public PetType DeletePetType(int ID)
        {
            var deletedPet = ctx.PetTypes.Remove(GetPetTypeByID(ID));
            ctx.SaveChanges();
            return deletedPet.Entity;
        }
    }
}
