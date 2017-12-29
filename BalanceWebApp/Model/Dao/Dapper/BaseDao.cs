using System.Data;
using System.Linq;
using MySql.Data.MySqlClient;
using Dapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BalanceWebApp.Model.Dao.Dapper
{
    public abstract class BaseDao
    {
        private static string LastInsertId = "select LAST_INSERT_ID()";

        private readonly ILogger _logger;
        private AppSettings _settings { get; }

        public BaseDao(IOptions<AppSettings> appSettings, ILogger logger)
        {
            _settings = appSettings.Value;
            _logger = logger;
        }

        protected IDbConnection GetConnection()
        {
            _logger.LogInformation("Getting database connection");
            return new MySqlConnection(_settings.ConnectionString);
        }

        protected long GetLasInsertedId()
        {
            _logger.LogInformation("Getting last inserted Id");
            return GetConnection().Query<long>(LastInsertId).Single();
        }        
    }
}