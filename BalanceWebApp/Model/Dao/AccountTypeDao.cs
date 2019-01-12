using System.Collections.Generic;
using System.Linq;
using BalanceWebApp.Model.Domain;
using Dapper;

namespace BalanceWebApp.Model.Dao
{
    public class AccountTypeDao : BaseDao, IAccountTypeDao
    {
        public AccountTypeDao(ConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }

        public List<AccountType> FindAll()
        {
            return GetConnection().Query<AccountType>("select id, name from account_type").AsList();
        }

        public AccountType FindById(long id)
        {
            var accountType = GetConnection().QuerySingleOrDefault<AccountType>("select id, name from account_type " +
                                                                 " where id = @Id", new { Id = id }); 
            return accountType;
        }

        public AccountType FindByName(string name)
        {
            var accountType = GetConnection().QuerySingleOrDefault<AccountType>("select id, name from account_type " +
                " where name = @Name", new { Name = name });
            
            return accountType;
        }

        public long AddNew(string name)
        {
            long id = 0;
            var rows = GetConnection().Execute("insert into account_type (name) values (@Name)", new { Name = name });
            if (rows > 0) {
                id = GetConnection().Query<long>("select LAST_INSERT_ID()").Single();
            }

            return id;
        }

        public int Delete(long id) 
        {
            var rows = GetConnection().Execute("delete from account_type where id = @Id", new {Id = id});

            return rows;
        }

        public AccountType Update(AccountType accountType)
        {
            GetConnection().Execute("update account_type set name = @Name where id = @Id", 
                new { Name = accountType.Name, Id = accountType.Id });

            return FindById(accountType.Id);
        }
    }
}
