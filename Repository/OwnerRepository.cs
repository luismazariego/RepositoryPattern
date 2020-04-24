﻿namespace Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Contracts;

    using Entities;
    using Entities.Helpers;
    using Entities.Models;
    using Microsoft.EntityFrameworkCore;

    public class OwnerRepository : RepositoryBase<Owner>, IOwnerRepository
    {
        public OwnerRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public PagedList<Owner> GetOwners(OwnerParameters ownerParameters)
        {
            var owners = FindByCondition(o => o.DateOfBirth.Year >= ownerParameters.MinYearOfBirth &&
								o.DateOfBirth.Year <= ownerParameters.MaxYearOfBirth)
							.OrderBy(on => on.Name);

            SearchByName(ref owners, ownerParameters.Name);

            return PagedList<Owner>.ToPagedList(owners,
                ownerParameters.PageNumber,
                ownerParameters.PageSize);
        }

        private void SearchByName(ref IOrderedQueryable<Owner> owners, string name)
        {
            if (!owners.Any() || string.IsNullOrWhiteSpace(name))
		        return;
 
	        owners = (IOrderedQueryable<Owner>)owners
                .Where(o => o.Name.ToLower().Contains(name.Trim().ToLower()));
        }        

        public IEnumerable<Owner> GetAllOwners()
        {
            return FindAll()
                .OrderBy(ow => ow.Name)
                .ToList();
        }

        public Owner GetOwnerById(Guid ownerId)
        {
            return FindByCondition(owner => owner.Id.Equals(ownerId))
                    .FirstOrDefault();
        }

        public Owner GetOwnerWithDetails(Guid ownerId)
        {
            return FindByCondition(owner => owner.Id.Equals(ownerId))
                .Include(ac => ac.Accounts)
                .FirstOrDefault();
        }

        public void CreateOwner(Owner owner)
        {
            Create(owner);
        }

        public void UpdateOwner(Owner owner)
        {
            Update(owner);
        }

        public void DeleteOwner(Owner owner)
        {
            Delete(owner);
        }
    }
}
