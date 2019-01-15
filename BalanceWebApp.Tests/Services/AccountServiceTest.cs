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
        private Mock<IProviderDao> _providerDao;

        private Mock<IAccountTypeDao> _accountTypeDao;
        
        [SetUp]
        public void SetUp()
        {
            _accountDao = new Mock<IAccountDao>();
            _providerDao = new Mock<IProviderDao>();
            _accountTypeDao = new Mock<IAccountTypeDao>();

            var logger = new Logger<AccountService>(new LoggerFactory());
            
            _accountService = new AccountService(logger, _accountDao.Object, _providerDao.Object, _accountTypeDao.Object);
        }

        [Test]
        public void TestGetAll()
        {
            ExpectAccountList();

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
            
            var id = ExpectSuccessAccountCreation();
            ExpectAccountForId(expectedAccount, id);

            _accountDao.Setup(dao => dao.GetAccount(0, 0, "123"));
            
            var result = _accountService.AddNew(0, 0, "name", "123");
            
            Assert.True(result.IsSuccess());
            Assert.AreEqual(expectedAccount, result.GetPayload());

            _accountDao.Verify(dao => dao.CreateAccount(It.IsAny<long>(), It.IsAny<long>(),
                It.IsAny<string>(), It.IsAny<string>()));

            _accountDao.Verify(dao => dao.GetAccount(0, 0, "123"));
            _accountDao.Verify(dao => dao.GetById(id));

            _accountDao.VerifyNoOtherCalls();
        }

        [Test]
        public void TestAddNewWithExistingAccount()
        {
            var account = BuildAccount();

            _accountDao.Setup(dao => dao.GetAccount(0, 0, "123"))
                .Returns(account);

            var result = _accountService.AddNew(0, 0, "name", "123");
            
            Assert.True(result.HasErrors());
            Assert.AreEqual("Looks like the account already exists", result.GetFailure());

            _accountDao.Verify(dao => dao.GetAccount(0, 0, "123"));
            _accountDao.VerifyNoOtherCalls();
        }

        [Test]
        public void TestAddNewThrowsException()
        {
            _accountDao.Setup(dao => dao.GetAccount(0, 0, "123"))
                .Throws(new Exception("expected exception"));
            
            var result = _accountService.AddNew(0, 0, "name", "123");
            
            Assert.True(result.HasErrors());
            Assert.AreEqual("Can't create new account", result.GetFailure());

            _accountDao.Verify(dao => dao.GetAccount(0, 0, "123"));
            _accountDao.VerifyNoOtherCalls();
        }

        [Test]
        public void TestUpdateNonExistingAccount()
        {
            var account = BuildAccount();

            _accountDao.Setup(dao => dao.GetById(It.IsAny<long>()));

            var result = _accountService.Update(account);
            
            Assert.True(result.HasErrors());
            Assert.AreEqual("Account not found", result.GetFailure());
         
            _accountDao.Verify(dao => dao.GetById(It.IsAny<long>()));
            _accountDao.VerifyNoOtherCalls();
            _providerDao.VerifyNoOtherCalls();
            _accountTypeDao.VerifyNoOtherCalls();
        }

        [Test]
        public void TestUpdateNonExistingProvider()
        {
            var account = BuildAccount();

            _accountDao.Setup(dao => dao.GetById(It.IsAny<long>()))
                .Returns(account);

            _providerDao.Setup(dao => dao.GetById(It.IsAny<long>()));
            
            var result = _accountService.Update(account);
            
            Assert.True(result.HasErrors());
            Assert.AreEqual("Provider not found", result.GetFailure());
         
            _accountDao.Verify(dao => dao.GetById(It.IsAny<long>()));
            _providerDao.Verify(dao => dao.GetById(It.IsAny<long>()));
            
            _accountDao.VerifyNoOtherCalls();
            _providerDao.VerifyNoOtherCalls();
            _accountTypeDao.VerifyNoOtherCalls();
        }
        
        [Test]
        public void TestUpdateNonExistingAccountType()
        {
            var account = BuildAccount();
            var provider = BuildProvider();

            _accountDao.Setup(dao => dao.GetById(It.IsAny<long>()))
                .Returns(account);

            _providerDao.Setup(dao => dao.GetById(It.IsAny<long>()))
                .Returns(provider);

            _accountTypeDao.Setup(dao => dao.FindById(It.IsAny<long>()));

            var result = _accountService.Update(account);

            Assert.True(result.HasErrors());
            Assert.AreEqual("Account type not found", result.GetFailure());

            _accountDao.Verify(dao => dao.GetById(It.IsAny<long>()));
            _providerDao.Verify(dao => dao.GetById(It.IsAny<long>()));
            _accountTypeDao.Verify(dao => dao.FindById(It.IsAny<long>()));

            _accountDao.VerifyNoOtherCalls();
            _providerDao.VerifyNoOtherCalls();
            _accountTypeDao.VerifyNoOtherCalls();
        }

        [Test]
        public void TestUpdate()
        {
            var account = BuildAccount();
            var provider = BuildProvider();
            var accountType = BuildAccountType();

            _accountDao.Setup(dao => dao.GetById(It.IsAny<long>()))
                .Returns(account);

            _accountDao.Setup(dao => dao.Update(It.IsAny<Account>()));

            _providerDao.Setup(dao => dao.GetById(It.IsAny<long>()))
                .Returns(provider);

            _accountTypeDao.Setup(dao => dao.FindById(It.IsAny<long>()))
                .Returns(accountType);

            var result = _accountService.Update(account);

            Assert.True(result.IsSuccess());
            Assert.AreEqual(account, result.GetPayload());

            _accountDao.Verify(dao => dao.GetById(It.IsAny<long>()));
            _accountDao.Verify(dao => dao.Update(It.IsAny<Account>()));
            _providerDao.Verify(dao => dao.GetById(It.IsAny<long>()));
            _accountTypeDao.Verify(dao => dao.FindById(It.IsAny<long>()));

            _accountDao.VerifyNoOtherCalls();
            _providerDao.VerifyNoOtherCalls();
            _accountTypeDao.VerifyNoOtherCalls();

        }
        // ------------------------------------------------------------------------------------------------------------

        private static long CalculateRandomId()
        {
            var random = new Random();
            return random.Next(1, 300);
        }

        private void ExpectAccountList()
        {
            _accountDao.Setup(dao => dao.GetAll())
                .Returns(BuildAccountList);
        }

        private long ExpectSuccessAccountCreation()
        {
            var id = CalculateRandomId();

            _accountDao.Setup(dao => dao.CreateAccount(It.IsAny<long>(), It.IsAny<long>(),
                    It.IsAny<string>(), It.IsAny<string>()))
                .Returns(id);

            return id;
        }

        private void ExpectAccountForId(Account account, long id)
        {
            _accountDao.Setup(dao => dao.GetById(id))
                .Returns(account);
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

        private static Provider BuildProvider()
        {
            return new Provider()
            {
                Id = 10,
                Name = "provider",
                Country = "GT"
            };
        }

        private static AccountType BuildAccountType()
        {
            return new AccountType()
            {
                Id = 10,
                Name = "account type"
            };
        }
    }
}