using System.Collections.Generic;
using System.Linq;
using BalanceWebApp.Model.Domain;
using Dapper;

namespace BalanceWebApp.Model.Dao
{
    public class AccountDao : BaseDao, IAccountDao
    {
        public AccountDao(ConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }

        public ICollection<Account> GetAll(string user)
        {
            var query = "select a.id, a.account_type_id AccountTypeId, at.name AccountType, a.provider_id ProviderId, p.name Provider, " +
                "  a.name, a.account_number AccountNumber, p.country ProviderCountry " +
                " from account a " +
                "   inner join account_type at on a.account_type_id = at.id " +
                "   inner join provider p on a.provider_id = p.id " +
                " where a.tenant = @User";

            var accounts = GetConnection().Query<Account>(query, new { User = user }).AsList();

            return accounts;
        }

        public Account GetById(long id)
        {
            var query = "select a.id, a.account_type_id AccountTypeId, at.name AccountType, a.provider_id ProviderId, p.name Provider, " +
                "  a.name, a.account_number AccountNumber, a.balance, p.country ProviderCountry " +
                " from account a " +
                "   inner join account_type at on a.account_type_id = at.id " +
                "   inner join provider p on a.provider_id = p.id " +
                " where a.id = @Id";

            var account = GetConnection().Query<Account>(query, new { Id = id }).SingleOrDefault<Account>();

            return account;
        }

        public Account GetByAccountNumber(string number, string user)
        {
            var query = "select a.id, a.account_type_id AccountTypeId, at.name AccountType, a.provider_id ProviderId, p.name Provider, " +
                "  a.name, a.account_number AccountNumber, a.balance, p.country ProviderCountry " +
                " from account a " +
                "   inner join account_type at on a.account_type_id = at.id " +
                "   inner join provider p on a.provider_id = p.id " +
                " where a.account_number = @Number and a.tenant = @User";

            var account = GetConnection().Query<Account>(query, new { Number = number, User = user }).SingleOrDefault<Account>();

            return account;
        }

        public Account GetAccount(long accountTypeId, long providerId, string accountNumber, string user)
        {
            var query = "select a.id, a.account_type_id AccountTypeId, at.name AccountType, a.provider_id ProviderId, p.name Provider, " +
                "  a.name, a.account_number AccountNumber, a.balance, p.country ProviderCountry " +
                " from account a " +
                "   inner join account_type at on a.account_type_id = at.id " +
                "   inner join provider p on a.provider_id = p.id " +
                " where a.account_number = @Number and a.account_type_id = @AccountTypeId and a.provider_id = @ProviderId and a.tenant = @User";

            var account = GetConnection().Query<Account>(query, new { Number = accountNumber, AccountTypeId = accountTypeId, ProviderId = providerId, User = user })
                    .SingleOrDefault<Account>();

            return account;
        }

        public long CreateAccount(long accountTypeId, long providerId, string name, string accountNumber, string user)
        {
            var query = "insert into account (account_type_id, provider_id, name, account_number, tenant) " +
                "  values (@AccountTypeId, @ProviderId, @Name, @AccountNumber, @User) ";

            var rows = GetConnection().Execute(query, new { AccountNumber = accountNumber, AccountTypeId = accountTypeId, ProviderId = providerId, Name = name, User = user });
            
            return rows > 0 ? GetLasInsertedId() : 0;
        }

        public void Update(Account account)
        {
            var query = "update account set account_type_id = @AccountTypeId, provider_id = @ProviderId, name = @Name, " +
                " account_number = @AccountNumber where id = @Id";

            GetConnection().Execute(query, new
            {
                AccountTypeId = account.AccountTypeId, 
                ProviderId = account.ProviderId,
                Name = account.Name,
                AccountNumber = account.AccountNumber,
                Id = account.Id
            });
        }
    }
}