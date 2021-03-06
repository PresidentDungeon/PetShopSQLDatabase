﻿using PetShop.Core.Entities;
using System.Collections.Generic;

namespace PetShop.Core.DomainService
{
    public interface IPetRepository
    {
        Pet AddPet(Pet pet);
        IEnumerable<Pet> ReadPets();
        IEnumerable<Pet> ReadPetsFilterSearch(Filter filter);
        IEnumerable<Pet> ReadPetsIncludeOwners();
        Pet GetPetByID(int ID);
        Pet UpdatePet(Pet pet);
        Pet DeletePet(int ID);
    }
}
