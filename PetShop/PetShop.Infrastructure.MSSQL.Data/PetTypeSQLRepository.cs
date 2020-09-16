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
