using PetShop.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PetShop.Core.ApplicationService
{
    public interface IPetTypeService
    {
        PetType CreatePetType(string type);

        PetType AddPetType(PetType petType);

        List<PetType> GetAllPetTypes();

        List<PetType> GetPetTypesFilterSearch(Filter filter);

        PetType GetPetTypeByID(int ID);

        PetType UpdatePetType(PetType type, int ID);

        PetType DeletePetType(int ID);
    }
}
