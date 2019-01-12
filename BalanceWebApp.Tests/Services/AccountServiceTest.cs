using System;
using System.Collections.Generic;
using BalanceWebApp.Model.Dao;
using BalanceWebApp.Model.Domain;
using BalanceWebApp.Services;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace BalanceWebApp.Tests.Services 
{
    public class AccountServiceTest
    {
        private AccountService _accountService;

        private Mock<IAccountDao> _accountDao;
        
        [SetUp]
        public void SetUp()
        {
            _accountDao = new Mock<IAccountDao>();
            var logger = new Logger<AccountService>(new LoggerFactory());
            
            _accountService = new AccountService(logger, _accountDao.Object, null, null);
        }

        [Test]
        public void TestGetAll()
        {
            _accountDao.Setup(dao => dao.GetAll())
                .Returns(BuildAccountList);

            var result = _accountService.GetAll();
            
            Assert.True(result.IsSuccess());
            Assert.NotNull(result.GetPayload());
            
            _accountDao.Verify(dao => dao.GetAll());
            _accountDao.VerifyNoOtherCalls();
        }

        [Test]
        public void TestGetAllThrowsException()
        {
            _accountDao.Setup(dao => dao.GetAll())
                .Throws(new Exception("expected exception"));

            var result = _accountService.GetAll();
            
            Assert.True(result.HasErrors());
            Assert.AreEqual("Can't get accounts", result.GetFailure());
            
            _accountDao.Verify(dao => dao.GetAll());
            _accountDao.VerifyNoOtherCalls();
        }

        [Test]
        public void TestGetById()
        {
            var expectedAccount = BuildAccount();
            _accountDao.Setup(dao => dao.GetById(It.IsAny<long>()))
                .Returns(expectedAccount);

            var result = _accountService.GetById(0);
            
            Assert.True(result.IsSuccess());
            Assert.AreEqual(expectedAccount, result.GetPayload());
            
            _accountDao.Verify(dao => dao.GetById(It.IsAny<long>()));
            _accountDao.VerifyNoOtherCalls();
        }

        [Test]
        public void TestGetByIdException()
        {
            _accountDao.Setup(dao => dao.GetById(It.IsAny<long>()))
                .Throws(new Exception("expected exception"));

            var result = _accountService.GetById(0);
            
            Assert.True(result.HasErrors());
            Assert.AreEqual("Can't get account", result.GetFailure());
            
            _accountDao.Verify(dao => dao.GetById(It.IsAny<long>()));           
            _accountDao.VerifyNoOtherCalls();
        }

        [Test]
        public void TestAddNew()
        {
            var expectedAccount = BuildAccount();
            
            _accountDao.Setup(dao => dao.CreateAccount(It.IsAny<long>(), It.IsAny<long>(),
                    It.IsAny<string>(), It.IsAny<string>()))
                .Returns(0);

            _accountDao.Setup(dao => dao.GetById(0))
                .Returns(expectedAccount);

            _accountDao.Setup(dao => dao.GetAccount(0, 0, "123"));
            
            var result = _accountService.AddNew(0, 0, "name", "123");
            
            Assert.True(result.IsSuccess());
            Assert.AreEqual(expectedAccount, result.GetPayload());

            _accountDao.Verify(dao => dao.CreateAccount(It.IsAny<long>(), It.IsAny<long>(),
                It.IsAny<string>(), It.IsAny<string>()));

            _accountDao.Verify(dao => dao.GetAccount(0, 0, "123"));
            
            _accountDao.Verify(dao => dao.GetById(0));
            _accountDao.VerifyNoOtherCalls();
        }
        
        private static ICollection<Account> BuildAccountList()
        {
            var list = new List<Account> { BuildAccount() };

            return list;
        }

        private static Account BuildAccount()
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
    }
}