
using System.Collections.Generic;
using System.Linq;
using BalanceWebApp.Model.Domain;
using Dapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BalanceWebApp.Model.Dao.Dapper
{
    public class TransactionTypeDao : BaseDao
    {
        private readonly ILogger<TransactionTypeDao> _logger;
        public TransactionTypeDao(IOptions<AppSettings> appSettings, ILogger<TransactionTypeDao> logger) : base(appSettings, logger)
        {
            _logger = logger;
        }

        public List<TransactionType> GetAll()
        {
            _logger.LogInformation("Getting the transactions types from DB");
            return GetConnection().Query<TransactionType>("select * from transaction_type").AsList();
        }

        public TransactionType GetById(long id)
        {
            return GetConnection().Query<TransactionType>("select * from transaction_type where id = @Id",
                new { Id = id }).SingleOrDefault();
        }

        public long New(TransactionType transactionType)
        {
            long id = 0;
            _logger.LogInformation("Adding a new transaction type with name: {0} and type: {1}", transactionType.Name, transactionType.Credit);

            var rows = GetConnection().Execute("insert into transaction_type (name, credit) values (@Name, @Credit)",
                new {transactionType.Name, transactionType.Credit });
                
            if(rows > 0)
            {
                id = GetLasInsertedId();
            }

            return id;
        }

        public TransactionType Update(TransactionType transactionType)
        {
            GetConnection().Execute("update transaction_type set name = @Name, credit = @Credit where id = @Id",
                new {transactionType.Name, transactionType.Credit, transactionType.Id });
                
            return GetById(transactionType.Id);
        }

        public int Delete(long id)
        {
            var rows = GetConnection().Execute("delete from transaction_type where id = @Id",
                new { Id = id });

            return rows;
        }
    }
}