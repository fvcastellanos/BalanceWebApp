using BalanceWebApp.Model.Dao;
using NUnit.Framework;
using Moq;
using BalanceWebApp.Services;
using Microsoft.Extensions.Logging;
using System;

using static BalanceWebApp.Tests.Fixture.ModelFixture;

namespace BalanceWebApp.Tests.Services
{
    public class ProviderServiceTest
    {
        private Mock<IProviderDao> _providerDaoMock;

        private ProviderService _providerService;

        [SetUp]
        public void SetUp()
        {
            _providerDaoMock = new Mock<IProviderDao>();

            var logger = new Logger<ProviderService>(new LoggerFactory());
            _providerService = new ProviderService(logger, _providerDaoMock.Object);
        }

        [Test]
        public void GetProvidersThrowsExceptionTest()
        {
            _providerDaoMock.Setup(dao => dao.GetAll())
                .Throws(new Exception("expected exception"));

            var result = _providerService.GetAll();

            Assert.True(result.HasErrors());
            Assert.AreEqual("Can't get providers", result.GetFailure());

            _providerDaoMock.Verify(dao => dao.GetAll());
            _providerDaoMock.VerifyNoOtherCalls();
        }

        [Test]
        public void GetProvidersTest()
        {
            var providerList = BuildProviderList();

            _providerDaoMock.Setup(dao => dao.GetAll())
                .Returns(providerList);

            var result = _providerService.GetAll();

            Assert.True(result.IsSuccess());
            Assert.NotNull(result.GetPayload());

            _providerDaoMock.Verify(dao => dao.GetAll());
            _providerDaoMock.VerifyNoOtherCalls();

        }
    }
}