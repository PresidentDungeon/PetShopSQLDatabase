using PetShop.Core.Entities;
using System.Collections.Generic;

namespace PetShop.Core.DomainService
{
    public interface IPetTypeRepository
    {
        PetType AddPetType(PetType type);
        IEnumerable<PetType> ReadTypes();
        IEnumerable<PetType> ReadTypesFilterSearch(Filter filter);
        PetType GetPetTypeByID(int ID);
        PetType UpdatePetType(PetType type);
        PetType DeletePetType(int ID);
    }
}
