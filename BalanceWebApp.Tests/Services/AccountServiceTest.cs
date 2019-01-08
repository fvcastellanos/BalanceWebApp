
using System.Collections.Generic;
using BalanceWebApp.Model.Dao.Dapper;
using BalanceWebApp.Model.Domain;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace BalanceWebApp.Services 
{
    public class AccountServiceTest
    {

        private AccountService accountService;

        private Mock<AccountDao> accountDaoMock;

        [SetUp]
        public void SetUp()
        {
            accountDaoMock = new Mock<AccountDao>();

            accountService = new AccountService(null, accountDaoMock.Object, null, null);
        }

        [Test]
        public void Testito()
        {
            accountDaoMock.Setup(x => x.GetAll())
                .Returns(new List<Account>());

            var accounts = accountService.GetAll();

            Assert.IsNotNull(accounts);
        }
    }
}