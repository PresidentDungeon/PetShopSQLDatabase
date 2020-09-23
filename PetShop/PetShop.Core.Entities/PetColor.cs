using System;
using System.Collections.Generic;
using System.Text;

namespace PetShop.Core.Entities
{
    public class PetColor
    {
        public int PetID{get;set;}
        public Pet Pet { get; set; }

        public int ColorID { get; set; }
        public Color Color { get; set; }
    }
}
