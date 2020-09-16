using PetShop.Core.Entities;
using System.Collections.Generic;

namespace PetShop.Core.ApplicationService
{
    public interface IOwnerService
    {
        Owner CreateOwner(string firstName, string lastName, string address, string phoneNumber, string email);

        Owner AddOwner(Owner owner);

        List<Owner> GetAllOwners();

        Owner GetOwnerByID(int ID);

        Owner GetOwnerByIDIncludePets(int ID);

        Owner GetOwnerByIDWithPets(int ID);

        List<Owner> GetOwnerByName(string searchTitle);

        List<Owner> GetOwnersFilterSearch(Filter filter);

        Owner UpdateOwner(Owner owner, int ID);

        Owner DeleteOwner(int ID);
    }
}
