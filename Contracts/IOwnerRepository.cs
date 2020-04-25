namespace Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Dynamic;
    using Entities.Helpers;
    using Entities.Models;

    public interface IOwnerRepository : IRepositoryBase<Owner>
    {
        PagedList<ExpandoObject> GetOwners(OwnerParameters ownerParameters);
        IEnumerable<Owner> GetAllOwners();
        ExpandoObject GetOwnerById(Guid ownerId, string fields);
        Owner GetOwnerById(Guid ownerId);
        Owner GetOwnerWithDetails(Guid ownerId);
        void CreateOwner(Owner owner);
        void UpdateOwner(Owner owner);
        void DeleteOwner(Owner owner);
    }
}
