using System;
using System.Collections.Generic;
using System.Linq;
using BalanceWebApp.Model.Domain;
using Dapper;
using Microsoft.Extensions.Logging;

namespace BalanceWebApp.Model.Dao.Dapper
{
    public class AccountTypeDao : BaseDao
    {
        private readonly ILogger<AccountTypeDao> _logger;

        public AccountTypeDao(ILogger<AccountTypeDao> logger, ConnectionFactory connectionFactory) : base(logger, connectionFactory)
        {
            _logger = logger;
        }

        public List<AccountType> FindAll()
        {
            try
            {
                _logger.LogInformation("Getting account types");
                return GetConnection().Query<AccountType>("select id, name from account_type").AsList();
            }
            catch(Exception ex)
            {
                _logger.LogError("Unable to get the account types", ex);
                throw;
            }
        }

        public AccountType FindById(long id)
        {
            try
            {
                _logger.LogInformation("Getting account type with Id: {0}", id);
                var accountType = GetConnection().QuerySingleOrDefault<AccountType>("select id, name from account_type " +
                                                                     " where id = @Id", new { Id = id }); 
                return accountType;
            }
            catch(Exception ex)
            {
                _logger.LogError("Unable to perform the query", ex);
                throw;
            }
        }

        public AccountType FindByName(string name)
        {
            try
            {
                _logger.LogInformation("Getting account type with name: {0}", name);
                var accountType = GetConnection().QuerySingleOrDefault<AccountType>("select id, name from account_type " +
                    " where name = @Name", new { Name = name });
                
                return accountType;
            }
            catch (Exception ex)
            {
                _logger.LogError("Unable to perform the query", ex);
                throw;
            }
        }

        public long AddNew(string name) {
            try {
                long id = 0;
                var rows = GetConnection().Execute("insert into account_type (name) values (@Name)", new { Name = name });
                if (rows > 0) {
                    id = GetConnection().Query<long>("select LAST_INSERT_ID()").Single();
                }

                return id;
            } catch(Exception ex) {
                _logger.LogError("Unable to create an account type of name: {0}", name, ex.StackTrace);
                throw;
            }
        }

        public int Delete(long id) {
            try {
                var rows = GetConnection().Execute("delete from account_type where id = @Id", new {Id = id});
                return rows;
            } catch(Exception ex) {
                _logger.LogError("Unable to delete account type with id: {0}", id, ex.StackTrace);
                throw;
            }
        }

        public AccountType Update(AccountType accountType) {
            try {
                GetConnection().Execute("update account_type set name = @Name where id = @Id", 
                    new { Name = accountType.Name, Id = accountType.Id });

                return FindById(accountType.Id);
            } catch(Exception ex) {
                _logger.LogError("Unable to update account type due: {0}", ex.Message);
                throw;
            }
        }
    }
}
