using BalanceWebApp.Model.Dao.Dapper;
using Dapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BalanceWebApp.Model.Dao
{
    public class TransactionDao : BaseDao
    {
        public TransactionDao(IOptions<AppSettings> appSettings, ILogger logger) : base(appSettings, logger)
        {
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

        public void Delete(long id)
        {
            using (var db = GetConnection())
            {
                db.Execute("delete from transaction where id = @Id", new { Id = id });
            }
        }
    }
}