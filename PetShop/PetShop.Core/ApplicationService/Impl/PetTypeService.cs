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

        public PetType GetPetTypeByID(int ID)
        {
            return PetTypeRepository.GetPetTypeByID(ID);
        }

        public List<PetType> GetPetTypeByName(string searchTitle)
        {
            return SearchEngine.Search<PetType>(GetAllPetTypes(), searchTitle);
        }

        public List<PetType> GetPetTypesFilterSearch(Filter filter)
        {
            IEnumerable<PetType> types = PetTypeRepository.ReadTypes();

            if (!string.IsNullOrEmpty(filter.Name))
            {
                types = SearchEngine.Search<PetType>(types.ToList(), filter.Name);

            }
            if (!string.IsNullOrEmpty(filter.Sorting) && filter.Sorting.ToLower().Equals("asc"))
            {
                types = from x in types orderby x.Name select x;
            }
            else if (!string.IsNullOrEmpty(filter.Sorting) && filter.Sorting.ToLower().Equals("desc"))
            {
                types = from x in types orderby x.Name descending select x;
            }

            return types.ToList();
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
