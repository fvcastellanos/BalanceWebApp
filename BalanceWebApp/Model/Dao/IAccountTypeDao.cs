using System.Collections.Generic;
using BalanceWebApp.Model.Domain;

namespace BalanceWebApp.Model.Dao
{
    public interface IAccountTypeDao
    {
        IList<AccountType> FindAll(string user);
        AccountType FindById(long id);
        AccountType FindByName(string name, string user);
        long AddNew(string name, string user);
        int Delete(long id);
        AccountType Update(AccountType accountType);
    }
}