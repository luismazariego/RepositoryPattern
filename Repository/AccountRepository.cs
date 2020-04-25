namespace Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Contracts;

    using Entities;
    using Entities.Helpers;
    using Entities.Models;

    public class AccountRepository : RepositoryBase<Account>, IAccountRepository
    {
        private readonly ISortHelper<Account> _sortHelper;

        public AccountRepository(RepositoryContext repositoryContext,
            ISortHelper<Account> sortHelper)
            : base(repositoryContext)
        {
            _sortHelper = sortHelper;
        }

        public IEnumerable<Account> AccountsByOwner(Guid ownerId)
        {
            return FindByCondition(a => a.OwnerId.Equals(ownerId)).ToList();
        }

        public PagedList<Account> GetAccountsByOwner(Guid ownerId, AccountParameters parameters)
        {
            var accounts = FindByCondition(a => a.OwnerId.Equals(ownerId));

            return PagedList<Account>.ToPagedList(accounts,
                parameters.PageNumber,
                parameters.PageSize);
        }

        public Account GetAccountByOwner(Guid ownerId, Guid id)
        {
            return FindByCondition(a => a.OwnerId.Equals(ownerId) && a.Id.Equals(id)).SingleOrDefault();
        }
    }
}
