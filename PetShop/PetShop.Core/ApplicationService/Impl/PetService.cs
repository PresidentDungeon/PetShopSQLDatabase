using PetShop.Core.DomainService;
using PetShop.Core.Entities;
using PetShop.Core.Search;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PetShop.Core.ApplicationService.Impl
{
    public class PetService : IPetService
    {
        private IPetRepository PetRepository;
        private IPetTypeRepository PetTypeRepository;
        private ISearchEngine SearchEngine;

        public PetService(IPetRepository petRepository, IPetTypeRepository petTypeRepository, ISearchEngine searchEngine)
        {
            this.PetRepository = petRepository;
            this.PetTypeRepository = petTypeRepository;
            this.SearchEngine = searchEngine;
        }

        public Pet CreatePet(string petName, PetType type, DateTime birthDate, List<PetColor> petColors, double price)
        {
            if (string.IsNullOrEmpty(petName))
            {
                throw new ArgumentException("Entered pet name too short");
            }
            if (petColors.Count == 0)
            {
                throw new ArgumentException("Entered color description too short");
            }
            if(price < 0)
            {
                throw new ArgumentException("Pet price can't be negative");
            }
            if(type == null )
            {
                throw new ArgumentException("The type of pet is invalid");
            }
            else
            {
                if(PetTypeRepository.ReadTypes().Where((x) => { return x.ID == type.ID; }).FirstOrDefault() == null)
                {
                    throw new ArgumentException("No pet type with such an ID found");
                }
                else
                {
                    type = PetTypeRepository.ReadTypes().Where((x) => { return x.ID == type.ID; }).FirstOrDefault();
                }
            }
            if((DateTime.Now.Year - birthDate.Year) > 150 || (DateTime.Now.Year - birthDate.Year) < 0)
            {
                throw new ArgumentException("Invalid birthdate selected");
            }

            return new Pet { Name = petName, Type = type, Birthdate = birthDate, petColors = petColors, Price = price };
        }

        public Pet AddPet(Pet pet)
        {
            if(pet != null)
            {
                return PetRepository.AddPet(pet);
            }
            return null;
        }

        public List<Pet> GetAllPets()
        {
            return PetRepository.ReadPets().ToList();
        }

        public List<Pet> GetPetsFilterSearch(Filter filter)
        {
            if (filter.CurrentPage < 0 || filter.ItemsPrPage < 0)
            {
                throw new InvalidDataException("Page or items per page must be above zero");
            }

            IEnumerable<Pet> pets = PetRepository.ReadPetsFilterSearch(filter);

            if (filter.CurrentPage > 0 && filter.ItemsPrPage > 0)
            {
                pets = pets.Skip((filter.CurrentPage - 1) * filter.ItemsPrPage).Take(filter.ItemsPrPage);
                if (pets.Count() == 0)
                {
                    throw new InvalidDataException("Index out of bounds");
                }
            }

            return pets.ToList();
        }

        public List<Pet> GetAllPetsIncludeOwner()
        {
            return PetRepository.ReadPetsIncludeOwners().ToList();
        }

        public Pet GetPetByID(int ID)
        {
            return PetRepository.GetPetByID(ID);
        }

        public Pet UpdatePet(Pet pet, int ID)
        {
            if (GetPetByID(ID) == null)
            {
                throw new ArgumentException("No pet with such ID found");
            }
            if(pet == null)
            {
                throw new ArgumentException("Updating pet does not excist");
            }
            pet.ID = ID;
            return PetRepository.UpdatePet(pet);
        }

        public Pet DeletePet(int ID)
        {
            if (ID <= 0)
            {
                throw new ArgumentException("Incorrect ID entered");
            }
            if (GetPetByID(ID) == null)
            {
                throw new ArgumentException("No pet with such ID found");
            }
            return PetRepository.DeletePet(ID);
        }

    }
}
