using PetShop.Core.Search;
using System;

namespace PetShop.Core.Entities
{
    public class Pet: ISearchAble
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public PetType Type { get; set; }
        public DateTime Birthdate { get; set; }
        public DateTime SoldDate { get; set; }
        public string Color { get; set; }
        public Owner? Owner { get; set; }
        public double Price { get; set; }

        public string searchValue()
        {
            return Name;
        }

        public override string ToString()
        {
            return $"Name: {Name} (ID {ID})\nAnimal Type: {Type}\nColor: {Color}\nBirthDate: {Birthdate.ToString("dd/MM/yyyy")}" +
                $"\nPrice: {Price.ToString("n2")} DKK\nStatus: {((Owner == null) ? "NOT SOLD" : $"SOLD TO {Owner.FirstName} {Owner.LastName} ({SoldDate})")}";
        }
    }
}
