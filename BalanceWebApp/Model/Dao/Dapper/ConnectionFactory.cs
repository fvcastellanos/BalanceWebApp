using System;
using System.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace BalanceWebApp.Model.Dao.Dapper
{
    public class ConnectionFactory
    {
        private readonly ILogger _logger;
        private AppSettings _settings { get; }

        public ConnectionFactory(IOptions<AppSettings> appSettings, ILogger<ConnectionFactory> logger)
        {
            _settings = appSettings.Value;
            _logger = logger;
        }

        public IDbConnection GetConnection()
        {
            var connectionString = BuildFromEnv();

            if ("".Equals(connectionString))
            {
                connectionString = BuildFromConfig();
            }
            
            _logger.LogInformation("Getting database connection");
            return new MySqlConnection(connectionString);
        }

        private string BuildFromEnv()
        {
            _logger.LogInformation("Get connection from env");
            var dbHost = Environment.GetEnvironmentVariable("DB_HOST");
            var dbName = Environment.GetEnvironmentVariable("DB_NAME");
            var dbUser = Environment.GetEnvironmentVariable("DB_USER");
            var dbPwd = Environment.GetEnvironmentVariable("DB_PWD");

            if (dbHost == null || dbName == null || dbUser == null || dbPwd == null)
            {
                _logger.LogInformation("No connection info found in env");
                return "";
            }

            var connectionString = "Server=" + dbHost + ";Database=" + dbName + ";Uid=" + dbUser + ";Pwd=" + dbPwd +
                                   ";SslMode=None";
            
            _logger.LogInformation("Connection info found in env");
            return connectionString;
        }

        private string BuildFromConfig()
        {
            _logger.LogInformation("Loading connection from settings");
            return _settings.ConnectionString;
        }           
    }
}