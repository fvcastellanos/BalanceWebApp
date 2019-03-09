using System;
using System.Collections.Generic;
using BalanceWebApp.Model.Dao;
using BalanceWebApp.Model.Domain;
using Microsoft.Extensions.Logging;

namespace BalanceWebApp.Services
{
    public class ProviderService : BaseService
    {
        private readonly ILogger _logger;

        private readonly IProviderDao _providerDao;

        public ProviderService(ILogger<ProviderService> logger, IProviderDao providerDao)
        {
            _logger = logger;
            _providerDao = providerDao;
        }

        public Result<string, IList<Provider>> GetAll(string user)
        {
            try
            {
                _logger.LogInformation("Getting all the providers");
                var providers = _providerDao.GetAll(user);

                return Result<string, IList<Provider>>.ForSuccess(providers);
            }
            catch(Exception ex)
            {
                _logger.LogError("Exception: ", ex);
                return Result<string, IList<Provider>>.ForFailure("Can't get providers");
            }
        }

        public Result<string, Provider> GetById(long id)
        {
            try
            {
                _logger.LogInformation("Getting provider with id: {0}", id);
                var provider = _providerDao.GetById(id);

                if (provider == null)
                {
                    _logger.LogInformation("Provider with id: {0} not found", id);
                    return Result<string, Provider>.ForFailure("Provider Not Found");
                }
                    
                return Result<string, Provider>.ForSuccess(provider);
            }
            catch (Exception ex)
            {
                _logger.LogError("Unable to get provider with id: {0}", id, ex);
                return Result<string, Provider>.ForFailure("Can't get the Provider");
            }
        }

        public Result<string, IList<Provider>> GetByCountry(string country, string user)
        {
            try
            {
                _logger.LogInformation("Getting provider for country: {0}", country);
                var providers = _providerDao.GetByCountry(country, user);

                return Result<string, IList<Provider>>.ForSuccess(providers);
            }
            catch(Exception ex)
            {
                _logger.LogError("Can't get providers by country: ", ex);
                return Result<string, IList<Provider>>.ForFailure("Can't get provider by selected country");
            }
        }

        public Result<string, Provider> New(Provider provider, string user) {
            try
            {
                var providerHolder = _providerDao.FindProvider(provider.Name, provider.Country, user);
                if (providerHolder != null)
                {
                    _logger.LogError("Provider: {0} - {1} already exists", provider.Name, provider.Country);
                    return Result<string, Provider>.ForFailure("Provider already exists");
                }
                
                _logger.LogInformation("Adding a new provider with name: {0} and country: {1}", provider.Name, provider.Country);
                var id = _providerDao.New(provider.Name, provider.Country, user);
                var newProviderHolder = _providerDao.GetById(id);

                return Result<string, Provider>.ForSuccess(newProviderHolder);
            }
            catch(Exception ex)
            {
                _logger.LogError("Unable to create a new provider due: {0}", ex);
                return Result<string, Provider>.ForFailure("Can't create provider");
            }
        }

        public Result<string, int> Delete(long id)
        {
            try
            {
                if (!ProviderExists(id))
                {
                    _logger.LogError("Provider with id: {0} not found", id);
                    return Result<string, int>.ForFailure("Provider not found");
                }
                
                _logger.LogInformation("Deleting provider with id {0}", id);
                var rows = _providerDao.Delete(id);
                _logger.LogInformation("Provider with id {0} deleted", id);

                return Result<string, int>.ForSuccess(rows);
            }
            catch(Exception ex)
            {
                _logger.LogError("Can't delete provider: {0}", ex.StackTrace);
                return Result<string, int>.ForFailure("Can't delete provider");
            }
        }

        public Result<string, Provider> Update(Provider provider)
        {
            try
            {
                if (!ProviderExists(provider.Id))
                {
                    _logger.LogError("Provider with id: {0} not found", provider.Id);
                    return Result<string, Provider>.ForFailure("Provider not found");
                    
                }

                _logger.LogInformation("Trying to update provider with id: {0}", provider.Id);
                var p = _providerDao.Update(provider);
                _logger.LogInformation("Provider udpated");

                return Result<string, Provider>.ForSuccess(p);
            }
            catch(Exception ex)
            {
                _logger.LogError("Can't update provider: {0}", ex.StackTrace);
                return Result<string, Provider>.ForFailure("Can't update provider");
            }
        }

        private bool ProviderExists(long id)
        {
            var providerHolder = _providerDao.GetById(id);

            return providerHolder != null ? true : false;
        }
        
    }
}
