using PetShop.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PetShop.Core.ApplicationService
{
    public interface IPetService
    {
        Pet CreatePet(string petName, PetType type, DateTime birthDate, List<PetColor> petColors, double price);

        Pet AddPet(Pet pet);

        List<Pet> GetAllPets();

        List<Pet> GetPetsFilterSearch(Filter filter);

        List<Pet> GetAllPetsIncludeOwner();

        Pet GetPetByID(int ID);

        Pet UpdatePet(Pet pet, int ID);

        Pet DeletePet(int ID);

    }
}
