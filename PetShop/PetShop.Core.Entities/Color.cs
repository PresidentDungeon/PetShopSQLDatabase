using PetShop.Core.Search;
using System;
using System.Collections.Generic;
using System.Text;

namespace PetShop.Core.Entities
{
    public class Color : ISearchAble
    {
        public int ID { get; set; }
        public string ColorDescription { get; set; }
        public List<PetColor> PetColors { get; set; }

        public string searchValue()
        {
            return ColorDescription;
        }
    }
}
