namespace Contracts
{
    using System;

    using Entities.Helpers;
    using Entities.Models;

    public interface IAccountRepository : IRepositoryBase<Account>
    {
        PagedList<Account> GetAccountsByOwner(Guid ownerId, AccountParameters parameters);
        Account GetAccountByOwner(Guid ownerId, Guid id);
    }
}
