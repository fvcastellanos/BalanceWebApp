using System;
using System.Collections.Generic;
using BalanceWebApp.Model.Domain;

namespace BalanceWebApp.Tests.Fixture
{
    public static class ModelFixture
    {
        public static long CalculateRandomId()
        {
            var random = new Random();
            return random.Next(1, 300);
        }
        
        public static ICollection<Account> BuildAccountList()
        {
            var list = new List<Account> { BuildAccount() };

            return list;
        }

        public static Account BuildAccount()
        {
            return new Account()
            {
                AccountNumber = "123",
                AccountType = "type",
                AccountTypeId = 0,
                Balance = 100,
                Id = 0,
                Name = "name",
                Provider = "provider",
                ProviderCountry = "GT",
                ProviderId = 0
            };
        }

        public static Provider BuildProvider()
        {
            return new Provider()
            {
                Id = 10,
                Name = "provider",
                Country = "GT"
            };
        }

        public static IList<Provider> BuildProviderList()
        {
            return new List<Provider>() { BuildProvider() };
        }

        public static AccountType BuildAccountType()
        {
            return new AccountType()
            {
                Id = 10,
                Name = "account type"
            };
        }

        public static IList<AccountType> BuildAccountTypeList()
        {
            return new List<AccountType>() { BuildAccountType() };
        }

        public static TransactionType BuildTransactionType()
        {
            return new TransactionType()
            {
                Id = CalculateRandomId(),
                Name = "transaction type",
                Credit = false
            };
        }

        public static List<TransactionType> BuildTransactionTypeList()
        {
            return new List<TransactionType> { BuildTransactionType() };
        }
    }
}