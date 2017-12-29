using System;
using System.Linq;
using System.Collections.Generic;
using Dapper;
using Microsoft.Extensions.Logging;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Options;
using BalanceWebApp.Model.Domain;

namespace BalanceWebApp.Model.Dao.Dapper
{
    public class AccountTypeDao : BaseDao
    {
        private readonly ILogger<AccountTypeDao> _logger;

        public AccountTypeDao(IOptions<AppSettings> settings, 
            ILogger<AccountTypeDao> logger) : base(settings, logger)
        {
            this._logger = logger;
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

        public Optional<AccountType> FindById(long id)
        {
            try
            {
                _logger.LogInformation("Getting account type with Id: {0}", id);
                var accountType = GetConnection().Query<AccountType>("select id, name from account_type " +
                                                                     " where id = @Id", new { Id = id }).SingleOrDefault<AccountType>(); 
                return new Optional<AccountType>(accountType);
            }
            catch(Exception ex)
            {
                _logger.LogError("Unable to perform the query", ex);
                throw ex;
            }
        }

        public Optional<AccountType> FindByName(string name)
        {
            try
            {
                _logger.LogInformation("Getting account type with name: {0}", name);
                var accountType = GetConnection().Query<AccountType>("select id, name from account_type " +
                    " where name = @Name", new { Name = name }).SingleOrDefault<AccountType>();
                
                return new Optional<AccountType>(accountType);
            }
            catch (Exception ex)
            {
                _logger.LogError("Unable to perform the query", ex);
                throw ex;
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
                _logger.LogError("Unable to create an account type of name: {0}", name);
                throw ex;
            }
        }

        public int Delete(long id) {
            try {
                var rows = GetConnection().Execute("delete from account_type where id = @Id", new {Id = id});
                return rows;
            } catch(Exception ex) {
                _logger.LogError("Unable to delete account type with id: {0}", id);
                throw ex;
            }
        }

        public AccountType Update(AccountType accountType) {
            try {
                GetConnection().Execute("update account_type set name = @Name where id = @Id", 
                    new { Name = accountType.Name, Id = accountType.Id });

                return FindById(accountType.Id).Value;
            } catch(Exception ex) {
                _logger.LogError("Unable to update account type due: {0}", ex.Message);
                throw ex;
            }
        }


    }
}
