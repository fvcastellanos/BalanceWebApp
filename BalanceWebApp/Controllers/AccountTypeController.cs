using System.Collections.Generic;
using BalanceWebApp.Model.Domain;
using BalanceWebApp.Model.Views.AccountTypes;
using BalanceWebApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BalanceWebApp.Controllers
{
    [Route("/account-types")]
    public class AccountTypeController : BaseController
    {
        private AccountTypeService _accountTypeService;

        public AccountTypeController(ILogger<AccountTypeController> logger, AccountTypeService accountTypeService)
        {
            _accountTypeService = accountTypeService;
        }

        public IActionResult Index()
        {
            var result = _accountTypeService.GetAccountTypes();

            IndexViewModel model;

            if (result.HasErrors())
            {
                model = BuildErrorModel(result.GetFailure());
                return View(model);
            }
            
            model = BuildModel(result.GetPayload());
            return View(model);
        }

        private IndexViewModel BuildErrorModel(string message)
        {
            return new IndexViewModel() {
                AccountTypes = new List<AccountType>(),
                Message = message
            };
        }

        private IndexViewModel BuildModel(IEnumerable<AccountType> list)
        {
            return new IndexViewModel() {
                AccountTypes = list
            };
        }
    }
}