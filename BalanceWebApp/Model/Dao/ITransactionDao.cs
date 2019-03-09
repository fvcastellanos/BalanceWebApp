using System;
using System.Collections.Generic;

namespace BalanceWebApp.Model.Dao
{
    public interface ITransactionDao
    {
        IList<Transaction> GetTransactions(long accountId, DateTime start, DateTime end);
        Transaction GetTransaction(long id);
        long Add(Transaction transaction, string user);
        void Update(Transaction transaction);
        void Delete(long id);
    }
}