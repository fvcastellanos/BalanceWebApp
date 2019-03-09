using System.Collections.Generic;
using BalanceWebApp.Model.Domain;

namespace BalanceWebApp.Model.Dao
{
    public interface IProviderDao
    {
        IList<Provider> GetAll(string user);
        IList<Provider> GetByCountry(string country, string user);
        Provider GetById(long id);
        Provider FindProvider(string name, string country, string user);
        long New(string name, string country, string user);
        int Delete(long id);
        Provider Update(Provider provider);
    }
}