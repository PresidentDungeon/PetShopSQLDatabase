using PetShop.Core.Search;
using System.Collections.Generic;

namespace PetShop.Core.Entities
{
    public class PetType: ISearchAble
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public List<Pet> Pets { get; set; }

        public string searchValue()
        {
            return Name;
        }
    }
}
