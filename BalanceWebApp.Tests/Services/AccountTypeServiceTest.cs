using System;
using BalanceWebApp.Model.Dao;
using BalanceWebApp.Services;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

using static BalanceWebApp.Tests.Fixture.ModelFixture;

namespace BalanceWebApp.Tests.Services
{
    public class AccountTypeServiceTest
    {
        private AccountTypeService _accountTypeService;

        private Mock<IAccountTypeDao> _accountTypeDaoMock;

        [SetUp]
        public void SetUp()
        {
            _accountTypeDaoMock = new Mock<IAccountTypeDao>();
            var logger = new Logger<AccountTypeService>(new LoggerFactory());
            
            _accountTypeService = new AccountTypeService(logger, _accountTypeDaoMock.Object);
        }

        [Test]
        public void GetAccountTypesThrowsExceptionTest()
        {
            _accountTypeDaoMock.Setup(dao => dao.FindAll())
                .Throws(new Exception("expected exception"));

            var result = _accountTypeService.GetAccountTypes();
            
            Assert.True(result.HasErrors());
            Assert.AreEqual("Can't get the account types", result.GetFailure());

            _accountTypeDaoMock.Verify(dao => dao.FindAll());
            _accountTypeDaoMock.VerifyNoOtherCalls();
        }

        [Test]
        public void GetAccountTypesTest()
        {
            var expectedAccountTypeList = BuildAccountTypeList();

            _accountTypeDaoMock.Setup(dao => dao.FindAll())
                .Returns(expectedAccountTypeList);

            var result = _accountTypeService.GetAccountTypes();
            
            Assert.True(result.IsSuccess());
            Assert.NotNull(result.GetPayload());
        }
    }
}