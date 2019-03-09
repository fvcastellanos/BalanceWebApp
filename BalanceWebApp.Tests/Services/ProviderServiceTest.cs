using BalanceWebApp.Model.Dao;
using NUnit.Framework;
using Moq;
using BalanceWebApp.Services;
using Microsoft.Extensions.Logging;
using System;
using BalanceWebApp.Model.Domain;

using static BalanceWebApp.Tests.Fixture.ModelFixture;
using System.Collections.Generic;

namespace BalanceWebApp.Tests.Services
{
    public class ProviderServiceTest
    {
        private Mock<IProviderDao> _providerDaoMock;

        private ProviderService _providerService;

        private string user;

        [SetUp]
        public void SetUp()
        {
            _providerDaoMock = new Mock<IProviderDao>();

            var logger = new Logger<ProviderService>(new LoggerFactory());
            _providerService = new ProviderService(logger, _providerDaoMock.Object);

            user = "super-user";
        }

        [Test]
        public void GetProvidersThrowsExceptionTest()
        {
            _providerDaoMock.Setup(dao => dao.GetAll(user))
                .Throws(new Exception("expected exception"));

            var result = _providerService.GetAll(user);

            Assert.True(result.HasErrors());
            Assert.AreEqual("Can't get providers", result.GetFailure());

            _providerDaoMock.Verify(dao => dao.GetAll(user));
            _providerDaoMock.VerifyNoOtherCalls();
        }

        [Test]
        public void GetProvidersTest()
        {
            var providerList = BuildProviderList();

            _providerDaoMock.Setup(dao => dao.GetAll(user))
                .Returns(providerList);

            var result = _providerService.GetAll(user);

            Assert.True(result.IsSuccess());
            Assert.NotNull(result.GetPayload());

            _providerDaoMock.Verify(dao => dao.GetAll(user));
            _providerDaoMock.VerifyNoOtherCalls();

        }

        [Test]
        public void GetNonExistingProviderTest()
        {
            var providerId = 10;
            _providerDaoMock.Setup(dao => dao.GetById(providerId));

            var result = _providerService.GetById(providerId);

            Assert.True(result.HasErrors());
            Assert.AreEqual("Provider Not Found", result.GetFailure());

            _providerDaoMock.Verify(dao => dao.GetById(providerId));
            _providerDaoMock.VerifyNoOtherCalls();
        }

        [Test]
        public void GetProviderThrowsExceptionTest()
        {
            var providerId = 10;
            _providerDaoMock.Setup(dao => dao.GetById(providerId))
                .Throws(new Exception("expected exception"));

            var result = _providerService.GetById(providerId);

            Assert.True(result.HasErrors());
            Assert.AreEqual("Can't get the Provider", result.GetFailure());

            _providerDaoMock.Verify(dao => dao.GetById(providerId));
            _providerDaoMock.VerifyNoOtherCalls();
        }

        [Test]
        public void GetProviderTest()
        {
            var expectedProvider = BuildProvider();

            _providerDaoMock.Setup(dao => dao.GetById(0))
                .Returns(expectedProvider);

            var result = _providerService.GetById(0);

            Assert.True(result.IsSuccess());
            Assert.AreEqual(expectedProvider, result.GetPayload());

            _providerDaoMock.Verify(dao => dao.GetById(0));
            _providerDaoMock.VerifyNoOtherCalls();
        }
        
        [Test]
        public void GetProvidersByCountryNotFoundTest()
        {
            var countryCode = "GT";
            _providerDaoMock.Setup(dao => dao.GetByCountry(countryCode, user))
                .Returns(new List<Provider>());

            var result = _providerService.GetByCountry(countryCode, user);

            Assert.True(result.IsSuccess());
            Assert.That(result.GetPayload().Count == 0);

            _providerDaoMock.Verify(dao => dao.GetByCountry(countryCode, user));
            _providerDaoMock.VerifyNoOtherCalls();
        }

        [Test]
        public void GetProvidersByCountryThrowsException()
        {
            var countryCode = "GT";
            _providerDaoMock.Setup(dao => dao.GetByCountry(countryCode, user))
                .Throws(new Exception("expected exception"));

            var result = _providerService.GetByCountry(countryCode, user);

            Assert.True(result.HasErrors());
            Assert.AreEqual("Can't get provider by selected country", result.GetFailure());

            _providerDaoMock.Verify(dao => dao.GetByCountry(countryCode, user));
            _providerDaoMock.VerifyNoOtherCalls();
        }

        [Test]
        public void GetProvidersByCountry()
        {
            var countryCode = "GT";
            _providerDaoMock.Setup(dao => dao.GetByCountry(countryCode, user))
                .Returns(BuildProviderList());

            var result = _providerService.GetByCountry(countryCode, user);

            Assert.True(result.IsSuccess());
            Assert.That(result.GetPayload().Count > 0);

            _providerDaoMock.Verify(dao => dao.GetByCountry(countryCode, user));
            _providerDaoMock.VerifyNoOtherCalls();
        }

        [Test]
        public void NewExistingProvider()
        {
            var expectedProvider = BuildProvider();

            _providerDaoMock.Setup(dao => dao.FindProvider(It.IsAny<string>(), It.IsAny<string>(), user))
                .Returns(expectedProvider);

            var result = _providerService.New(expectedProvider, user);

            Assert.True(result.HasErrors());
            Assert.AreEqual("Provider already exists", result.GetFailure());

            _providerDaoMock.Verify(dao => dao.FindProvider(It.IsAny<string>(), It.IsAny<string>(), user));
            _providerDaoMock.VerifyNoOtherCalls();
        }

        [Test]
        public void NewProviderThrowsException()
        {
            var expectedProvider = BuildProvider();

            _providerDaoMock.Setup(dao => dao.FindProvider(It.IsAny<string>(), It.IsAny<string>(), user))
                .Throws(new Exception("expected exception"));

            var result = _providerService.New(expectedProvider, user);

            Assert.True(result.HasErrors());
            Assert.AreEqual("Can't create provider", result.GetFailure());

            _providerDaoMock.Verify(dao => dao.FindProvider(It.IsAny<string>(), It.IsAny<string>(), user));
            _providerDaoMock.VerifyNoOtherCalls();
        }

        [Test]
        public void NewProvider()
        {
            var expectedProvider = BuildProvider();
            _providerDaoMock.Setup(dao => dao.FindProvider(It.IsAny<string>(), It.IsAny<string>(), user));
            _providerDaoMock.Setup(dao => dao.New(It.IsAny<string>(), It.IsAny<string>(), user))
                .Returns(0);

            _providerDaoMock.Setup(dao => dao.GetById(It.IsAny<long>()))
                .Returns(expectedProvider);

            var result = _providerService.New(expectedProvider, user);
            
            Assert.True(result.IsSuccess());
            Assert.AreEqual(expectedProvider, result.GetPayload());

            _providerDaoMock.Verify(dao => dao.FindProvider(It.IsAny<string>(), It.IsAny<string>(), user));
            _providerDaoMock.Verify(dao => dao.New(It.IsAny<string>(), It.IsAny<string>(), user));
            _providerDaoMock.Verify(dao => dao.GetById(It.IsAny<long>()));
            _providerDaoMock.VerifyNoOtherCalls();
        }
    }
}