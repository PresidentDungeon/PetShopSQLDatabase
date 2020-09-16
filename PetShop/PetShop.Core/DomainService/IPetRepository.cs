using PetShop.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PetShop.Core.DomainService
{
    public interface IPetRepository
    {
        Pet AddPet(Pet pet);
        IEnumerable<Pet> ReadPets();
        IEnumerable<Pet> ReadPetsIncludeOwners();
        Pet GetPetByID(int ID);
        Pet UpdatePet(Pet pet);
        Pet DeletePet(int ID);
    }
}
