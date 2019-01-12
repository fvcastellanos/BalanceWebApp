using System;
using System.Collections.Generic;
using BalanceWebApp.Model.Dao;
using BalanceWebApp.Model.Domain;
using Microsoft.Extensions.Logging;

namespace BalanceWebApp.Services
{
    public class AccountService : BaseService
    {
        private readonly IAccountDao _accountDao;
        private readonly IProviderDao _providerDao;
        private readonly IAccountTypeDao _accountTypeDao;
        private readonly ILogger _logger;

        public AccountService(ILogger<AccountService> logger, IAccountDao accountDao,
                IProviderDao providerDao, IAccountTypeDao accountTypeDao)
        {
            _accountDao = accountDao;
            _providerDao = providerDao;
            _accountTypeDao = accountTypeDao;
            _logger = logger;
        }
        
        public Result<string, ICollection<Account>> GetAll()
        {
            try
            {
                _logger.LogInformation("Getting accounts");
                var accounts = _accountDao.GetAll();

                return Result<string, ICollection<Account>>.ForSuccess(accounts);
            }
            catch (Exception ex)
            {
                _logger.LogError("Can't get accounts", ex);
                return Result<string, ICollection<Account>>.ForFailure("Can't get accounts");
            }
        }

        public Result<string, Account> GetById(long id)
        {
            try 
            {
                _logger.LogInformation("Getting account using id: {0}", id);
                var accountHolder = _accountDao.GetById(id);
                
                if (accountHolder == null) return Result<string, Account>.ForFailure("Account not found");

                return Result<string, Account>.ForSuccess(accountHolder);
            }
            catch (Exception ex)
            {
                _logger.LogError("Can't get account by id: {0}", id, ex);
                return Result<string, Account>.ForFailure("Can't get account");
            }
        }

        public Result<string, Account> AddNew(long accountTypeId, long providerId, string name, string number)
        {
            try
            {
                var accountHolder = _accountDao.GetAccount(accountTypeId, providerId, number);

                if (accountHolder != null) return Result<string, Account>.ForFailure("Looks like the account already exists");

                var id = _accountDao.CreateAccount(accountTypeId, providerId, name, number);

                var createdAccount = _accountDao.GetById(id);
                return Result<string, Account>.ForSuccess(createdAccount);
            }
            catch(Exception ex)
            {
                _logger.LogError("Can't create a new account: ", ex);
                return Result<string, Account>.ForFailure("Can't create new account");
            }
        }

        public Result<string, Account> Update(Account account)
        {
            try 
            {
                _logger.LogInformation("Getting account: {0] - {1}", account.Id, account.AccountNumber);
                var storedAccountHolder = _accountDao.GetById(account.Id);

                if (storedAccountHolder == null) 
                {
                    _logger.LogError("Getting account: {0] - {1} not found", account.Id, account.AccountNumber);
                    return Result<string, Account>.ForFailure("Account not found");
                }

                if (!ProviderExist(account.ProviderId)) return Result<string, Account>.ForFailure("Provider not found");

                if (!AccountTypeExist(account.AccountTypeId))
                    return Result<string, Account>.ForFailure("Account type not found");
                
                _logger.LogInformation("Updating account: {0} - {1}", account.Id, account.Name);
                _accountDao.Update(account);

                return Result<string, Account>.ForSuccess(account);
            }
            catch (Exception ex)
            {
                _logger.LogError("Can't update account: {0}", account.Id, ex);
                return Result<string, Account>.ForFailure("Can't update account");
            }
        }

        private bool ProviderExist(long id)
        {
            _logger.LogInformation("Getting provider: {0}", id);
            var providerHolder = _providerDao.GetById(id);

            return providerHolder != null ? true : false;
        }

        private bool AccountTypeExist(long id)
        {
            _logger.LogInformation("Getting account type: {0}", id);
            var accountTypeHolder = _accountTypeDao.FindById(id);

            return accountTypeHolder != null ? true : false;
        }
    }
}
