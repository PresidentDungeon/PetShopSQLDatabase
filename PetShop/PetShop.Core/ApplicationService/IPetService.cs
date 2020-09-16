using PetShop.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PetShop.Core.ApplicationService
{
    public interface IPetService
    {
        Pet CreatePet(string petName, PetType type, DateTime birthDate, string color, double price);

        Pet AddPet(Pet pet);

        List<Pet> GetAllPets();

        List<Pet> GetAllPetsIncludeOwner();

        List<Pet> GetAllPetsByPrice();

        List<Pet> GetAllAvailablePetsByPrice();

        Pet GetPetByID(int ID);

        List<Pet> GetPetByType (PetType type);

        List<Pet> GetPetByName(string searchTitle);

        List<Pet> GetPetByBirthdate(DateTime date);

        List<Pet> GetPetsFilterSearch(Filter filter);

        Pet UpdatePet(Pet pet, int ID);

        Pet DeletePet(int ID);

    }
}
