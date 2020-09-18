using PetShop.Core.DomainService;
using PetShop.Core.Entities;
using PetShop.Core.Search;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PetShop.Core.ApplicationService.Impl
{
    public class PetTypeService: IPetTypeService
    {
        private IPetTypeRepository PetTypeRepository;
        private ISearchEngine SearchEngine;

        public PetTypeService(IPetTypeRepository petTypeRepository, ISearchEngine searchEngine)
        {
            this.PetTypeRepository = petTypeRepository;
            this.SearchEngine = searchEngine;
        }

        public PetType CreatePetType(string type)
        {
            if (string.IsNullOrEmpty(type))
            {
                throw new ArgumentException("Entered pet name too short");
            }

            return new PetType {Name = type };
        }

        public PetType AddPetType(PetType type)
        {
            if (type != null)
            {
                if ((from x in GetAllPetTypes() where x.Name.ToLower().Equals(type.Name.ToLower()) select x).Count() > 0)
                {
                    throw new ArgumentException("Pet type with that name already exists");
                }
                return PetTypeRepository.AddPetType(type);
            }
            return null;
        }

        public List<PetType> GetAllPetTypes()
        {
            return PetTypeRepository.ReadTypes().ToList();
        }

        public List<PetType> GetPetTypesFilterSearch(Filter filter)
        {
            IEnumerable<PetType> types = PetTypeRepository.ReadTypesFilterSearch(filter);

            if (!string.IsNullOrEmpty(filter.Name))
            {
                return SearchEngine.Search<PetType>(types.ToList(), filter.Name);
            }

            return types.ToList();
        }

        public PetType GetPetTypeByID(int ID)
        {
            return PetTypeRepository.GetPetTypeByID(ID);
        }

        public PetType UpdatePetType(PetType type, int ID)
        {
            if (GetPetTypeByID(ID) == null)
            {
                throw new ArgumentException("No pet type with such ID found");
            }
            if (type == null)
            {
                throw new ArgumentException("Updating pet type does not excist");
            }
            type.ID = ID;
            return PetTypeRepository.UpdatePetType(type);
        }

        public PetType DeletePetType(int ID)
        {
            if (ID <= 0)
            {
                throw new ArgumentException("Incorrect ID entered");
            }
            if (GetPetTypeByID(ID) == null)
            {
                throw new ArgumentException("No pet type with such ID found");
            }
            return PetTypeRepository.DeletePetType(ID);
        }

    }
}
