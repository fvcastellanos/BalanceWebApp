using System.Collections.Generic;
using BalanceWebApp.Model.Domain;

namespace BalanceWebApp.Model.Dao
{
    public interface IAccountTypeDao
    {
        IList<AccountType> FindAll();
        AccountType FindById(long id);
        AccountType FindByName(string name);
        long AddNew(string name);
        int Delete(long id);
        AccountType Update(AccountType accountType);
    }
}