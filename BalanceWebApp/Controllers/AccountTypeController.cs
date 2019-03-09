using System.Collections.Generic;
using BalanceWebApp.Model.Domain;
using BalanceWebApp.Model.Views.AccountTypes;
using BalanceWebApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using static BalanceWebApp.Controllers.Routes;

namespace BalanceWebApp.Controllers
{
    [Authorize]
    [Route(AccountTypes)]
    public class AccountTypeController : BaseController
    {
        private readonly AccountTypeService _accountTypeService;

        public AccountTypeController(ILogger<AccountTypeController> logger, AccountTypeService accountTypeService)
        {
            _accountTypeService = accountTypeService;
        }

        public IActionResult Index()
        {
            var user = GetAuthenticatedUserId();
            var result = _accountTypeService.GetAccountTypes(user);

            IndexViewModel model;

            if (result.HasErrors())
            {
                model = BuildErrorModel(result.GetFailure());
                return View(model);
            }
            
            model = BuildModel(result.GetPayload());
            return View(model);
        }

        [Route(Routes.New)]
        public IActionResult New()
        {
            return View(new NewViewModel());
        }

        [Route(Routes.New)]
        [HttpPost]
        public IActionResult Save(NewViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("New", model);
            }

            var user = GetAuthenticatedUserId();
            var result = _accountTypeService.New(model.Name, user);

            if (result.HasErrors())
            {
                model.Message = result.GetFailure();
                return View("New", model);
            }

            return RedirectToAction("Index");
        }

        [Route("{id}")]
        public IActionResult Load(long id)
        {
            var result = _accountTypeService.GetAccountTypeById(id);

            UpdateViewModel model;
            
            if (result.HasErrors())
            {
                model = new UpdateViewModel() {
                    Message = result.GetFailure()
                };

                return View(model);
            }
            
            model = new UpdateViewModel()
            {
                Id = id,
                Name = result.GetPayload().Name
            };

            return View(model);
        }

        [Route("{id}")]
        [HttpPost]
        public IActionResult Update(UpdateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Load", model);
            }

            var accountType = new AccountType()
            {
                Id = model.Id,
                Name = model.Name
            };

            var result = _accountTypeService.UpdateAccountType(accountType);

            if (result.HasErrors())
            {
                model.Message = result.GetFailure();
                return View("Load", model);
            }

            return RedirectToAction("Index");
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