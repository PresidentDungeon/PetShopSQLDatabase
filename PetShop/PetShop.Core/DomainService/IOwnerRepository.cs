﻿using PetShop.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PetShop.Core.DomainService
{
    public interface IOwnerRepository
    {
        Owner AddOwner(Owner owner);
        IEnumerable<Owner> ReadOwners();
        Owner GetOwnerByID(int ID);
        Owner UpdateOwner(Owner owner);
        Owner DeleteOwner(int id);
    }
}