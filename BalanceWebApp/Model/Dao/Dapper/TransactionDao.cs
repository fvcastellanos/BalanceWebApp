using System;
using System.Collections.Generic;
using Dapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BalanceWebApp.Model.Dao.Dapper
{
    public class TransactionDao : BaseDao
    {
        public TransactionDao(ILogger<TransactionDao> logger, ConnectionFactory connectionFactory) : base(logger, connectionFactory)
        {
        }

        public IList<Transaction> GetTransactions(long accountId, DateTime start, DateTime end)
        {
            using (var db = GetConnection())
            {
                var sql = "select t.id, t.transaction_type_id transactionTypeId, tt.name transactionType, t.account_id AccountId, " +
                          "  a.account_number accountNumber, a.name AccountName,	t.date, t.description, t.amount, t.currency " +
                          " from transaction t " +
                          "  inner join transaction_type tt on t.transaction_type_id = tt.id " +
                          "  inner join account a on t.account_id = a.id " +
                          " where a.id = @AccountId " +
                          "  and DATE(t.date) between DATE(@Start) and DATE(@End) ";

                var query = db.Query<Transaction>(sql, new {AccountId = accountId, Start = start, End = end});

                return query.AsList();
            }
        }

        public Transaction GetTransaction(long id)
        {
            using (var db = GetConnection())
            {
                var sql = "select t.id, t.transaction_type_id transactionTypeId, tt.name transactionType, t.account_id AccountId, " +
                        "  a.account_number accountNumber, a.name AccountName,	t.date, t.description, t.amount, t.currency " +
                        " from transaction t " +
                        "  inner join transaction_type tt on t.transaction_type_id = tt.id " +
                        "  inner join account a on t.account_id = a.id " +
                        " where t.id = @Id ";
                          

                var query = db.QueryFirstOrDefault<Transaction>(sql, new { Id = id });

                return query;
            }
            
        }
        
        public long Add(Transaction transaction)
        {
            using (var db = GetConnection())
            {
                var sql = "insert into transaction " +
                    " (transaction_type_id, account_id, date, description, amount, currency) " +
                    " values (@TransactionTypeId, @AccountId, @Date, @Description, @Amount, @Currency)";
                
                db.Execute(sql, new { Id = transaction.Id, @TransactionTypeId = transaction.TransactionTypeId,
                    AccountId = transaction.AccountId, Date = transaction.Date, Description = transaction.Description,
                    Amount = transaction.Amount, Currency = transaction.Currency });

                return GetLasInsertedId();
            }
        }

        public void Update(Transaction transaction)
        {
            using (var db = GetConnection())
            {
                var sql = " update transaction " +
                          " set transaction_type_id = @TransactionTypeId, " +
                          "  account_id = @AccountId, " +
                          "  date = @Date, " +
                          "  description = @Description, " +
                          "  amount = @Amount, " +
                          "  currency = @Currency " +
                          " where id = @Id";

                db.Execute(sql, new
                {
                    TransactionTypeId = transaction.TransactionTypeId, 
                    AccountId = transaction.AccountId,
                    Date = transaction.Date,
                    Description = transaction.Description,
                    Amount = transaction.Amount,
                    Currency = transaction.Currency,
                    Id = transaction.Id
                });
            }
        }
        
        public void Delete(long id)
        {
            using (var db = GetConnection())
            {
                db.Execute("delete from transaction where id = @Id", new { Id = id });
            }
        }
    }
}