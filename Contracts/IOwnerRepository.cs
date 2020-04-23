namespace Contracts
{
    using System;
    using System.Collections.Generic;
    using Entities.Helpers;
    using Entities.Models;

    public interface IOwnerRepository : IRepositoryBase<Owner>
    {
        PagedList<Owner> GetOwners(OwnerParameters ownerParameters);
        IEnumerable<Owner> GetAllOwners();
        Owner GetOwnerById(Guid ownerId);
        Owner GetOwnerWithDetails(Guid ownerId);
        void CreateOwner(Owner owner);
        void UpdateOwner(Owner owner);
        void DeleteOwner(Owner owner);
    }
}
