using PetShop.Core.Search;
using System.Collections.Generic;

namespace PetShop.Core.Entities
{
    public class Owner: ISearchAble
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public List<Pet> Pets { get; set; }

        public string searchValue()
        {
            return FirstName + " " + LastName;
        }

        public override string ToString()
        {
            return $"Name: {FirstName} {LastName} (ID {ID})\nAddress: {Address}\nPhone number: {PhoneNumber}\nEmail: {(string.IsNullOrEmpty(Email) ? "Unknown email" : Email)}";
        }
    }
}
