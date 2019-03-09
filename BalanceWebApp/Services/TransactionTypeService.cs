using System;
using System.Collections.Generic;
using BalanceWebApp.Model.Dao;
using BalanceWebApp.Model.Domain;
using Microsoft.Extensions.Logging;

namespace BalanceWebApp.Services
{
    public class TransactionTypeService : BaseService
    {
        private readonly ILogger _logger;

        private readonly ITransactionTypeDao _transactionTypeDao;

        public TransactionTypeService(ILogger<TransactionTypeService> logger, ITransactionTypeDao transactionTypeDao)
        {
            _logger = logger;
            _transactionTypeDao = transactionTypeDao;
        }

        public Result<string, List<TransactionType>> GetAll(string user) 
        {
            try 
            {
                _logger.LogInformation("Getting all the transaction types");
                var list = _transactionTypeDao.GetAll(user);

                return Result<string, List<TransactionType>>.ForSuccess(list); 
            }
            catch(Exception ex)
            {
                _logger.LogError("Can't get the transactions types: ", ex);
                return Result<string, List<TransactionType>>.ForFailure("Can't get the transactions types");
            }
        }

        public Result<string, TransactionType> GetById(long id)
        {
            try 
            {
                _logger.LogInformation("Getting transaction type with id: {0}", id);
                var transactionType = _transactionTypeDao.GetById(id);

                return Result<string, TransactionType>.ForSuccess(transactionType);
            }
            catch(Exception ex)
            {
                _logger.LogError("Can't get transaction type", ex);
                return Result<string, TransactionType>.ForFailure("Can't get transaction type");
            }
        }

        public Result<string, TransactionType> New(TransactionType transactionType, string user)
        {
            try 
            {
                _logger.LogInformation("Adding a new transaction type");
                var id = _transactionTypeDao.New(transactionType, user);
                
                if (id == 0) return Result<string, TransactionType>.ForFailure("Can't create new account type");

                var value = _transactionTypeDao.GetById(id);
                return Result<string, TransactionType>.ForSuccess(value);
            }
            catch(Exception ex)
            {
                _logger.LogError("Can't create new account type: ", ex);
                return Result<string, TransactionType>.ForFailure("Can't create new account type");
            }
        }

        public Result<string, TransactionType> Update(TransactionType transactionType) {
            try
            {
                _logger.LogInformation("Updating transaction type: {0}", transactionType.Name);
                var transactionTypeOld = _transactionTypeDao.GetById(transactionType.Id);

                if (transactionTypeOld == null) return Result<string, TransactionType>.ForFailure("Transaction Type not found");

                var updated = _transactionTypeDao.Update(transactionType);

                return Result<string, TransactionType>.ForSuccess(updated);
            }
            catch(Exception ex)
            {
                _logger.LogInformation("Can't update transaction type: ", ex);
                return Result<string, TransactionType>.ForFailure("Can't update transaction type");
            }
        }

        public Result<string, int> Delete(long id)
        {
            try
            {
                _logger.LogInformation("Deleting Transaction Type with id: {0}", id);
                
                var rows = _transactionTypeDao.Delete(id);
                _logger.LogInformation("Transaction Type with Id: {0} was deleted", id);

                return Result<string, int>.ForSuccess(rows);
            }
            catch (Exception ex)
            {
                _logger.LogError("Unable to delete Transaction type with id: {0} -> {1}", id, ex);
                return Result<string, int>.ForFailure("Can't delete transaction type");
            }
        }

    }
}