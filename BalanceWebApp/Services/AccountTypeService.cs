using System;
using System.Collections.Generic;
using BalanceWebApp.Model.Dao;
using BalanceWebApp.Model.Domain;
using Microsoft.Extensions.Logging;

namespace BalanceWebApp.Services
{
    public class AccountTypeService : BaseService
    {
        private readonly ILogger<AccountTypeService> _logger;
        private readonly AccountTypeDao _accountTypeDao;

        public AccountTypeService(ILogger<AccountTypeService> logger, AccountTypeDao accountTypeDao)
        {
            _accountTypeDao = accountTypeDao;
            _logger = logger;
        }

        public Result<string, List<AccountType>> GetAccountTypes()
        {
            try
            {
                _logger.LogInformation("Getting all the account types");
                var list = _accountTypeDao.FindAll();

                return Result<string, List<AccountType>>.ForSuccess(list);
            }
            catch(Exception ex)
            {
                _logger.LogError("Unable to get the account types, {0}", ex);
                return Result<string, List<AccountType>>.ForFailure("Can't get the account types");
            }
        }

        public Result<string, AccountType> GetAccountTypeById(long id)
        {
            try
            {
                _logger.LogInformation("Getting Account Type for id: {0}", id);
                var accountTypeHolder = _accountTypeDao.FindById(id);
                
                if (accountTypeHolder == null) return Result<string, AccountType>.ForFailure("Account type not found");

                return Result<string, AccountType>.ForSuccess(accountTypeHolder);

            } catch(Exception ex)
            {
                _logger.LogError("Unable to get the account type: {0}, due: {1}", id, ex);
                return Result<string, AccountType>.ForFailure("Can't get the account type requested");
            }
        }

        public Result<string, AccountType> New(string name)
        {
            try
            {
                _logger.LogInformation("Adding new account type: {0}", name);
                var value = _accountTypeDao.AddNew(name);

                var accountTypeHolder = _accountTypeDao.FindById(value);
                
                return Result<string, AccountType>.ForSuccess(accountTypeHolder);
            }
            catch(Exception ex)
            {
                _logger.LogError("Unable to create a new account type due: {0}", ex);
                return Result<string, AccountType>.ForFailure("Can't create the account type");
            }
        }

        public Result<string, int> DeleteAccountType(long id)
        {
            try
            {
                _logger.LogInformation("Trying to delete account type with id: {0}", id);
                var rows = _accountTypeDao.Delete(id);

                if (rows > 0)
                {
                    return Result<string, int>.ForSuccess(rows);
                }

                return Result<string, int>.ForFailure("No account type with id: {0} was deleted");
            }
            catch(Exception ex) {
                _logger.LogError("Unable to delete account type with id: {0}, due: {1}", id, ex);
                return Result<string, int>.ForFailure("Can't delete account type");
            }
        }

        public Result<string, AccountType> UpdateAccountType(AccountType accountType) {
            try {
                _logger.LogInformation("Updating account type: {0}", accountType);
                var at = _accountTypeDao.Update(accountType);
                return Result<string, AccountType>.ForSuccess(at);
            } catch(Exception ex) {
                _logger.LogError("Unable to update account type with id: {0}, due: {1}", accountType, ex);
                return Result<string, AccountType>.ForFailure("Can't update account type");
            }
            
        }
    }
}
