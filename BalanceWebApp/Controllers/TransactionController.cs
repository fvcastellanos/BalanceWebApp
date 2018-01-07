using System;
using System.Collections.Generic;
using System.Linq;
using BalanceWebApp.Model;
using BalanceWebApp.Model.Domain;
using BalanceWebApp.Model.Views;
using BalanceWebApp.Model.Views.Transactions;
using BalanceWebApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using static BalanceWebApp.Controllers.Routes;

namespace BalanceWebApp.Controllers
{
    [Route(Accounts)]
    public class TransactionController : BaseController
    {
        private readonly TransactionTypeService _transactionTypeService;
        private readonly TransactionService _transactionService;
        private readonly AccountService _accountService;        
        private readonly ILogger _logger;

        public TransactionController(TransactionTypeService transactionTypeService, 
            TransactionService transactionService, 
            AccountService accountService, 
            ILogger<TransactionController> logger)
        {
            _transactionTypeService = transactionTypeService;
            _transactionService = transactionService;
            _accountService = accountService;
            _logger = logger;
        }

        [Route("{accountId}" + Transactions)]
        public IActionResult Index(long accountId)
        {
            var model = BuildTransactionModel(accountId, null, null);
            var result = _transactionService.GetTransactions(accountId, model.Start, model.End);

            if (result.HasErrors())
            {
                model.Message = result.GetFailure();
                return View(model);
            }

            model.Transactions = result.GetPayload();
            return View(model);
        }

        [HttpPost]
        [Route("{accountId}" + Transactions)]
        public IActionResult GetTransactions(long accountId, IndexViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            var account = GetAccount(accountId);
            model.AccountName = account.Name;
            model.AccountNumber = account.AccountNumber;
            
            var result = _transactionService.GetTransactions(accountId, model.Start, model.End);

            if (result.HasErrors())
            {
                model.Message = result.GetFailure();
                return View("Index", model);
            }

            model.Transactions = result.GetPayload();
            return View("Index", model);
        }
        
        [Route("{accountId}" + Transactions + "/" + Routes.New)]
        public IActionResult New(long accountId)
        {
            var account = GetAccount(accountId);
            
            var model = new NewViewModel()
            {
                AccountId = accountId,
                AccountName = account.Name,
                AccountNumber = account.AccountNumber,
                Date = DateTime.Today,
                Options = GetTransactionTypes()
            };
            
            return View("New", model);
        }

        [HttpPost]
        [Route("{accountId}" + Transactions + "/" + Routes.New)]
        public IActionResult Save(long accountId, NewViewModel model)
        {
            var account = GetAccount(accountId);
            model.AccountId = accountId;
            model.AccountName = account.Name;
            model.AccountNumber = account.AccountNumber;
            model.Options = GetTransactionTypes();

            if (!ModelState.IsValid)
            {
                return View("New", model);
            }

            var transaction = new Transaction()
            {
                AccountId = accountId,
                Date = model.Date,
                TransactionTypeId = model.TransactionTypeId,
                Description = model.Description,
                Currency = model.Currency,
                Amount = model.Amount
            };
            
            var result = _transactionService.Add(transaction);

            if (result.HasErrors())
            {
                model.Message = result.GetFailure();
                return View("New", model);
            }

            return RedirectToAction("Index");
        }

        [Route("{accountId}" + Transactions + "/{id}")]
        public IActionResult Load(long accountId, long id)
        {
            var model = new UpdateViewModel()
            {
                Options = GetTransactionTypes()
            };

            var result = _transactionService.GetById(id);

            if (result.HasErrors())
            {
                model.Message = result.GetFailure();
                return View(model);
            }

            var transaction = result.GetPayload();
            model.AccountId = accountId;
            model.AccountName = transaction.AccountName;
            model.AccountNumber = transaction.AccountNumber;
            model.TransactionTypeId = transaction.TransactionTypeId;
            model.Date = transaction.Date;
            model.Description = transaction.Description;
            model.Currency = transaction.Currency;
            model.Amount = transaction.Amount;

            return View(model);
        }

        [HttpPost]
        [Route("{accountId}" + Transactions + "/{id}")]
        public IActionResult Update(long accountId, long id, UpdateViewModel model)
        {
            model.Options = GetTransactionTypes();

            if (!ModelState.IsValid)
            {
                return View("Load", model);
            }

            var transaction = new Transaction()
            {
                Id = id,
                AccountId = accountId,
                TransactionTypeId = model.TransactionTypeId,
                Date = model.Date,
                Description = model.Description,
                Currency = model.Currency,
                Amount =  model.Amount
            };

            var result = _transactionService.Update(transaction);

            if (result.HasErrors())
            {
                model.Message = result.GetFailure();
                return View("Load", model);
            }

            return RedirectToAction("Index");
        }

        [Route("{accountId}" + Transactions + "/{id}/" + Routes.Delete)]
        public IActionResult Delete(long accountId, long id)
        {
            var model = BuildTransactionModel(accountId, null, null);
            
            var result = _transactionService.Delete(id);

            if (result.HasErrors())
            {
                model.Message = result.GetFailure();
                return View("Index", model);
            }

            return RedirectToAction("Index");
        }
                
        private IndexViewModel BuildTransactionModel(long accountId, DateTime ? start, DateTime  ? end)
        {
            var model = new IndexViewModel();

            var account = GetAccount(accountId);
            model.AccountName = account.Name;
            model.AccountNumber = account.AccountNumber;

            var today = DateTime.Today;
            var initDate = today.AddMonths(-2);
            var startDate = new DateTime(initDate.Year, initDate.Month, 1);
            var endDate = today.AddMonths(1).AddDays(-1);

            if (start.HasValue) startDate = start.Value;
            if (end.HasValue) endDate = end.Value;

            model.Start = startDate;
            model.End = endDate;
            
            return model;
        }

        private Account GetAccount(long id)
        {
            var result = _accountService.GetById(id);

            if (result.IsSuccess())
            {
                var account = result.GetPayload();
                return account;
            }
            
            return new Account();
        }
        
        private IEnumerable<Option> GetTransactionTypes()
        {
            var result = _transactionTypeService.GetAll();

            if (result.IsSuccess())
            {
                var list = from tt in result.GetPayload()
                    select new Option(tt.Id.ToString(), tt.Name);

                return list.ToList();
            }

            return new List<Option>();
        }        
    }        
}