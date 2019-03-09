using System.Collections.Generic;
using BalanceWebApp.Model.Domain;

namespace BalanceWebApp.Model.Dao
{
    public interface IAccountDao
    {
        ICollection<Account> GetAll(string user);
        Account GetById(long id);
        Account GetByAccountNumber(string number, string user);
        Account GetAccount(long accountTypeId, long providerId, string accountNumber, string user);
        long CreateAccount(long accountTypeId, long providerId, string name, string accountNumber, string user);
        void Update(Account account);
    }
}