using System;
using System.Collections.Generic;
using BalanceWebApp.Model;
using BalanceWebApp.Model.Dao;
using BalanceWebApp.Model.Domain;
using Microsoft.Extensions.Logging;

namespace BalanceWebApp.Services
{
    public class TransactionService
    {
        private readonly ITransactionDao _transactionDao;
        private readonly ITransactionTypeDao _transactionTypeDao;
        private readonly IAccountDao _accountDao;
        private readonly ILogger _logger;

        public TransactionService(ILogger<TransactionService> logger, ITransactionDao transactionDao, ITransactionTypeDao transactionTypeDao, IAccountDao accountDao)
        {
            _logger = logger;
            _transactionDao = transactionDao;
            _transactionTypeDao = transactionTypeDao;
            _accountDao = accountDao;
        }

        public Result<string, IList<Transaction>> GetTransactions(long accountId, DateTime start, DateTime end)
        {
            try
            {
                if (!AccountExists(accountId))
                {
                    _logger.LogError("account with id: {0} doesn't exists", accountId);
                    return Result<string, IList<Transaction>>.ForFailure("Account doesn't exists");
                }
                
                _logger.LogInformation("getting transactions from: {0} to: {1}, accountId: {3}", start, end, accountId);
                var transactions = _transactionDao.GetTransactions(accountId, start, end);
                
                return Result<string, IList<Transaction>>.ForSuccess(transactions);
            }
            catch (Exception ex)
            {
                _logger.LogError("Can't get transactions: {0}", ex.StackTrace);
                return Result<string, IList<Transaction>>.ForFailure("Can't get transactions");
            }
        }

        public Result<string, Transaction> GetById(long id)
        {
            try
            {
                _logger.LogInformation("getting transaction with id: {0}", id);
                var transaction = _transactionDao.GetTransaction(id);
                
                if (transaction == null) return Result<string, Transaction>.ForFailure("Transaction not found"); 
                    
                return Result<string, Transaction>.ForSuccess(transaction);
            }
            catch (Exception ex)
            {
                _logger.LogError("Can't get transaction: {0}", ex.StackTrace);
                return Result<string, Transaction>.ForFailure("Can't get transaction");
            }
        }

        public Result<string, Transaction> Add(Transaction transaction)
        {
            try
            {
                _logger.LogInformation("creating transaction : {0} - {1} - {2} - {3}", transaction.AccountId, transaction.TransactionTypeId, transaction.Amount, transaction.Currency);
                
                if (!TransactionTypeExists(transaction.TransactionTypeId))
                    return Result<string, Transaction>.ForFailure("Transaction type not found");
                
                if (!AccountExists(transaction.AccountId))
                    return Result<string, Transaction>.ForFailure("Account not found");

                var id = _transactionDao.Add(transaction);
                
                var savedTransaction = _transactionDao.GetTransaction(id);
                
                if (savedTransaction == null) return Result<string, Transaction>.ForFailure("Transaction not created"); 

                return Result<string, Transaction>.ForSuccess(savedTransaction);
            }
            catch (Exception ex)
            {
                _logger.LogError("Can't create transaction: {0}", ex.StackTrace);
                return Result<string, Transaction>.ForFailure("Can't create transaction");
            }
        }

        public Result<string, bool> Update(Transaction transaction)
        {
            try
            {
                _logger.LogInformation("updating transaction : {0} - {1} - {2} - {3}", transaction.AccountId, transaction.TransactionTypeId, transaction.Amount, transaction.Currency);
                
                if (!TransactionTypeExists(transaction.TransactionTypeId))
                    return Result<string, bool>.ForFailure("Transaction type not found");
                
                if (!AccountExists(transaction.AccountId))
                    return Result<string, bool>.ForFailure("Account not found");
                
                _transactionDao.Update(transaction);
                
                return Result<string, bool>.ForSuccess(true);
            }
            catch (Exception ex)
            {
                _logger.LogError("Can't update transaction: {0}", ex.StackTrace);
                return Result<string, bool>.ForFailure("Can't update transaction");
            }
        }
        
        public Result<string, bool> Delete(long id)
        {
            try
            {
                _logger.LogInformation("deleting transaction with id: {0}", id);
                _transactionDao.Delete(id);
                
                return Result<string, bool>.ForSuccess(true);
            }
            catch (Exception ex)
            {
                _logger.LogError("Can't delete transaction: {0}", ex.StackTrace);
                return Result<string, bool>.ForFailure("Can't delete transaction");
            }
        }

        private bool TransactionTypeExists(long id)
        {
            var transactionType = _transactionTypeDao.GetById(id);

            return transactionType != null ? true : false;
        }

        private bool AccountExists(long id)
        {
            var account = _accountDao.GetById(id);

            return account != null ? true : false;
        }
    }
}