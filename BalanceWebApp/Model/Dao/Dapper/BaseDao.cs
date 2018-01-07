using System.Data;
using System.Linq;
using Dapper;
using Microsoft.Extensions.Logging;

namespace BalanceWebApp.Model.Dao.Dapper
{
    public abstract class BaseDao
    {
        private const string LastInsertId = "select LAST_INSERT_ID()";

        private readonly ILogger _logger;
        private readonly ConnectionFactory _connectionFactory;

        public BaseDao(ILogger logger, ConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        protected IDbConnection GetConnection()
        {
            return _connectionFactory.GetConnection();
        }

        protected long GetLasInsertedId()
        {
            _logger.LogInformation("Getting last inserted Id");
            return GetConnection().Query<long>(LastInsertId).Single();
        }        
    }
}