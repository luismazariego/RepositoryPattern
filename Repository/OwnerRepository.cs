﻿namespace Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Dynamic.Core;
    using System.Reflection;
    using System.Text;
    using Contracts;

    using Entities;
    using Entities.Helpers;
    using Entities.Models;
    using Microsoft.EntityFrameworkCore;

    public class OwnerRepository : RepositoryBase<Owner>, IOwnerRepository
    {
        private readonly ISortHelper<Owner> _sortHelper;

        public OwnerRepository(RepositoryContext repositoryContext,
            ISortHelper<Owner> sortHelper)
            : base(repositoryContext)
        {
            _sortHelper = sortHelper;
        }

        public PagedList<Owner> GetOwners(OwnerParameters ownerParameters)
        {
            var owners = FindByCondition(o => o.DateOfBirth.Year >= ownerParameters.MinYearOfBirth &&
								o.DateOfBirth.Year <= ownerParameters.MaxYearOfBirth);
							

            SearchByName(ref owners, ownerParameters.Name);

            var sortedOwners = _sortHelper.ApplySort(owners, ownerParameters.OrderBy);

            return PagedList<Owner>.ToPagedList(sortedOwners,
                ownerParameters.PageNumber,
                ownerParameters.PageSize);
        }

        private void SearchByName(ref IQueryable<Owner> owners, string name)
        {
            if (!owners.Any() || string.IsNullOrWhiteSpace(name))
		        return;
 
	        owners = (IQueryable<Owner>)owners
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

        private void ApplySort(ref IOrderedQueryable<Owner> owners, string orderByQueryString)
        {
            if (!owners.Any())
                return;

            if (string.IsNullOrWhiteSpace(orderByQueryString))
            {
                owners = owners.OrderBy(x => x.Name);
                return;
            }

            var orderParams = orderByQueryString.Trim().Split(',');
            var propertyInfos = typeof(Owner).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var orderQueryBuilder = new StringBuilder();

            foreach (var param in orderParams)
            {
                if (string.IsNullOrWhiteSpace(param))
                    continue;

                var propertyFromQueryName = param.Split(" ")[0];
                var objectProperty = propertyInfos.FirstOrDefault(pi => pi.Name.Equals(propertyFromQueryName, StringComparison.InvariantCultureIgnoreCase));

                if (objectProperty == null)
                    continue;

                var sortingOrder = param.EndsWith(" desc") ? "descending" : "ascending";

                orderQueryBuilder.Append($"{objectProperty.Name.ToString()} {sortingOrder}, ");
            }

            var orderQuery = orderQueryBuilder.ToString().TrimEnd(',', ' ');

            if (string.IsNullOrWhiteSpace(orderQuery))
            {
                owners = owners.OrderBy(x => x.Name);
                return;
            }

            owners = owners.OrderBy(orderQuery);
        }
    }
}
