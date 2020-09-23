using PetShop.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication.ExtendedProtection;
using System.Threading.Tasks;

namespace PetShop.RestAPI.DTO
{
    public class PetDTO
    {
        public PetDTO(Pet pet)
        {
            ID = pet.ID;
            Name = pet.Name;
            Type = pet.Type.Name;
            Birthdate = pet.Birthdate;
            SoldDate = pet.SoldDate;
            Owner = pet.Owner;
            Price = pet.Price;
            Colors = new List<Color>();

            foreach (PetColor petColor in pet.petColors)
            {
                Color color = petColor.Color;
                color.PetColors = null;
                Colors.Add(color);
            }
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public DateTime Birthdate { get; set; }
        public DateTime SoldDate { get; set; }
        public Owner? Owner { get; set; }
        public double Price { get; set; }
        public List<Color> Colors { get; set; }
    }
}
