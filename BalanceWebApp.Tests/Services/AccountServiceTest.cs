using System.Collections.Generic;
using BalanceWebApp.Model.Dao;
using BalanceWebApp.Model.Domain;
using BalanceWebApp.Services;
using Moq;
using NUnit.Framework;

namespace BalanceWebApp.Tests.Services 
{
    public class AccountServiceTest
    {

        private AccountService accountService;

        private Mock<IAccountDao> accountDaoMock;

        [SetUp]
        public void SetUp()
        {
            accountDaoMock = new Mock<IAccountDao>();

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