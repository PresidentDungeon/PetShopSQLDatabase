using PetShop.Core.Search;
using System;
using System.Collections.Generic;

namespace PetShop.Core.Entities
{
    public class Pet: ISearchAble
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public PetType Type { get; set; }
        public DateTime Birthdate { get; set; }
        public DateTime SoldDate { get; set; }
        public Owner? Owner { get; set; }
        public double Price { get; set; }
        public List<Color> Colors { get; set; }

        public List<PetColor> petColors { get; set; }

        public string searchValue()
        {
            return Name;
        }
    }
}
