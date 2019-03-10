using System.Collections.Generic;
using System.Linq;
using BalanceWebApp.Model.Domain;
using Dapper;

namespace BalanceWebApp.Model.Dao
{
    public class TransactionTypeDao : BaseDao, ITransactionTypeDao
    {
        public TransactionTypeDao(ConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }

        public List<TransactionType> GetAll(string user)
        {
            return GetConnection().Query<TransactionType>("select * from transaction_type where tenant = @User",
                    new { User = user }).AsList();
        }

        public TransactionType GetById(long id)
        {
            return GetConnection().Query<TransactionType>("select * from transaction_type where id = @Id",
                new { Id = id }).SingleOrDefault();
        }

        public long New(TransactionType transactionType, string user)
        {
            long id = 0;

            var rows = GetConnection().Execute("insert into transaction_type (name, credit, tenant) values (@Name, @Credit, @User)",
                new {transactionType.Name, transactionType.Credit, User = user });
                
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