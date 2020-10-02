using System;
using System.Collections.Generic;
using System.Text;

namespace PetShop.Core.Entities
{
    public class User
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public string UserRole { get; set; }
    }
}
