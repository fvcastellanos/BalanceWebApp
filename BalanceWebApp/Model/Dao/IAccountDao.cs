using System.Collections.Generic;
using BalanceWebApp.Model.Domain;

namespace BalanceWebApp.Model.Dao
{
    public interface IAccountDao
    {
        ICollection<Account> GetAll();
        Account GetById(long id);
        Account GetByAccountNumber(string number);
        Account GetAccount(long accountTypeId, long providerId, string accountNumber);
        long CreateAccount(long accountTypeId, long providerId, string name, string accountNumber);
        void Update(Account account);
    }
}