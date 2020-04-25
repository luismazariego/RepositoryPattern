﻿namespace Repository
{
    using Contracts;
    using Entities;
    using Entities.Helpers;
    using Entities.Models;

    public class RepositoryWrapper : IRepositoryWrapper
    {
        private RepositoryContext _repoContext;
        private IOwnerRepository _owner;
        private IAccountRepository _account;
        private ISortHelper<Owner> _ownerSortHelper;
	    private ISortHelper<Account> _accountSortHelper;
        private IDataShaper<Account> _accountDataShaper;
        private IDataShaper<Owner> _ownerDataShaper;

        public RepositoryWrapper(RepositoryContext repositoryContext,
            ISortHelper<Owner> ownerSortHelper,
		    ISortHelper<Account> accountSortHelper,
            IDataShaper<Owner> ownerDataShaper,
            IDataShaper<Account> accountDataShaper
            )
        {
            _repoContext = repositoryContext;
            _ownerSortHelper = ownerSortHelper;
            _accountSortHelper = accountSortHelper;
            _ownerDataShaper = ownerDataShaper;
		    _accountDataShaper = accountDataShaper;
        }

        public IOwnerRepository Owner
        {
            get
            {
                if (_owner == null)
                {
                    _owner = new OwnerRepository(_repoContext, _ownerSortHelper, _ownerDataShaper);
                }

                return _owner;
            }
        }

        public IAccountRepository Account
        {
            get
            {
                if (_account == null)
                {
                    _account = new AccountRepository(_repoContext, _accountSortHelper);
                }

                return _account;
            }
        }

        public void Save() => _repoContext.SaveChanges();
    }
}
