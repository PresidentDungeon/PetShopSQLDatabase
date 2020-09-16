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
            var petTypeCreated = ctx.PetTypes.Add(type);
            ctx.SaveChanges();
            return petTypeCreated.Entity;
        }
        public IEnumerable<PetType> ReadTypes()
        {
            return ctx.PetTypes.AsEnumerable();
        }

        public IEnumerable<PetType> ReadTypesFilterSearch(Filter filter)
        {
            IEnumerable<PetType> types = ctx.PetTypes.AsEnumerable();

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
            return ctx.PetTypes.FirstOrDefault(x => x.ID == ID);
        }

        public PetType UpdatePetType(PetType type)
        {
            var petToUpdate = ctx.PetTypes.Update(type);
            ctx.SaveChanges();
            return petToUpdate.Entity;
        }

        public PetType DeletePetType(int ID)
        {
            var deletedPet = ctx.PetTypes.Remove(GetPetTypeByID(ID));
            ctx.SaveChanges();
            return deletedPet.Entity;
        }
    }
}
