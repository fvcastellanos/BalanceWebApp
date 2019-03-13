using System;
using BalanceWebApp.Model.Dao;
using BalanceWebApp.Services;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

using static BalanceWebApp.Tests.Fixture.ModelFixture;

namespace BalanceWebApp.Tests.Services
{
    public class TransactionTypeServiceTest : BaseServiceTest
    {
        private Mock<ITransactionTypeDao> _transactionTypeMock;

        private TransactionTypeService _transactionTypeService;

        [SetUp]
        public void Setup()
        {
            _transactionTypeMock = new Mock<ITransactionTypeDao>();
            
            var logger = new Logger<TransactionTypeService>(new LoggerFactory());
            
            _transactionTypeService = new TransactionTypeService(logger, _transactionTypeMock.Object);
        }

        [Test]
        public void testGetAll()
        {
            var transactionTypes = BuildTransactionTypeList();

            _transactionTypeMock.Setup(dao => dao.GetAll(user))
                .Returns(transactionTypes);
            
            var result = _transactionTypeService.GetAll(user);

            Assert.True(result.IsSuccess());
            Assert.NotNull(result.GetPayload());
            
            _transactionTypeMock.Verify(dao => dao.GetAll(user));
            _transactionTypeMock.VerifyNoOtherCalls();
        }

        [Test]
        public void testGetAllThrowsException()
        {
            _transactionTypeMock.Setup(dao => dao.GetAll(user))
                .Throws(new Exception("expected exception"));
            
            var result = _transactionTypeService.GetAll(user);

            Assert.True(result.HasErrors());
            Assert.AreEqual("Can't get the transactions types", result.GetFailure());
            
            _transactionTypeMock.Verify(dao => dao.GetAll(user));
            _transactionTypeMock.VerifyNoOtherCalls();
            
        }
        
        
    }
}