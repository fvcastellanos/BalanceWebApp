using System.Collections.Generic;
using BalanceWebApp.Model.Domain;

namespace BalanceWebApp.Model.Dao
{
    public interface ITransactionTypeDao
    {
        List<TransactionType> GetAll();
        TransactionType GetById(long id);
        long New(TransactionType transactionType);
        TransactionType Update(TransactionType transactionType);
        int Delete(long id);
    }
}