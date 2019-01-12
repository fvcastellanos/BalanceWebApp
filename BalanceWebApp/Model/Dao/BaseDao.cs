using System.Data;
using System.Linq;
using Dapper;
using Microsoft.Extensions.Logging;

namespace BalanceWebApp.Model.Dao
{
    public abstract class BaseDao
    {
        private const string LastInsertId = "select LAST_INSERT_ID()";

        private readonly ConnectionFactory _connectionFactory;

        protected BaseDao(ConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        protected IDbConnection GetConnection()
        {
            return _connectionFactory.GetConnection();
        }

        protected long GetLasInsertedId()
        {
            return GetConnection().Query<long>(LastInsertId).Single();
        }        
    }
}