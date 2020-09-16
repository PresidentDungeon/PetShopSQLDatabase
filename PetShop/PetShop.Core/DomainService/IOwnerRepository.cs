using PetShop.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PetShop.Core.DomainService
{
    public interface IOwnerRepository
    {
        Owner AddOwner(Owner owner);
        IEnumerable<Owner> ReadOwners();
        IEnumerable<Owner> ReadOwnersFilterSearch(Filter filter);
        Owner GetOwnerByID(int ID);
        Owner GetOwnerByIDIncludePets(int ID);
        Owner UpdateOwner(Owner owner);
        Owner DeleteOwner(int id);
    }
}
