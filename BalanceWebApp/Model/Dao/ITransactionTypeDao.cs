using System.Collections.Generic;
using BalanceWebApp.Model.Domain;

namespace BalanceWebApp.Model.Dao
{
    public interface ITransactionTypeDao
    {
        List<TransactionType> GetAll(string user);
        TransactionType GetById(long id);
        long New(TransactionType transactionType, string user);
        TransactionType Update(TransactionType transactionType);
        int Delete(long id);
    }
}