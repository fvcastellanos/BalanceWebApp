using System.Collections.Generic;
using System.Linq;
using BalanceWebApp.Model.Domain;
using BalanceWebApp.Model.Views.Accounts;
using BalanceWebApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using static BalanceWebApp.Controllers.Routes;

namespace BalanceWebApp.Controllers
{
    [Route(Accounts)]
    public class AccountController : BaseController
    {
        private readonly AccountService _accountService;
        private readonly ProviderService _providerService;
        private readonly AccountTypeService _accountTypeService;
        private readonly ILogger _logger;

        public AccountController(AccountService accountService, 
            ProviderService providerService, 
            AccountTypeService accountTypeService, 
            ILogger<AccountController> logger)
        {
            _accountService = accountService;
            _providerService = providerService;
            _accountTypeService = accountTypeService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var result = _accountService.GetAll();

            IndexViewModel model;
            if (result.HasErrors())
            {
                model = new IndexViewModel()
                {
                    Message = result.GetFailure()
                };
                
                return View(model);
            }

            model = new IndexViewModel()
            {
                Accounts = result.GetPayload()
            };
            
            return View(model);
        }

        [Route(Routes.New)]
        public IActionResult New()
        {
            var model = BuildNewViewModel();

            return View(model);
        }


        [HttpPost]
        [Route(Routes.New)]
        public IActionResult Save(NewViewModel model)
        {
            model.Providers = GetProviders();
            model.AccountTypes = GetAccountTypes();

            if (!ModelState.IsValid)
            {
                return View("New", model);
            }

            var result = _accountService.AddNew(model.AccountTypeId, model.ProviderId, model.Name, model.Number);

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
            var model = BuildUpdateViewModel(id);

            var result = _accountService.GetById(id);

            if (result.HasErrors())
            {
                model.Message = result.GetFailure();
                return View(model);
            }

            var account = result.GetPayload();

            model.AccountTypeId = account.AccountTypeId;
            model.ProviderId = account.ProviderId;
            model.Name = account.Name;
            model.Number = account.AccountNumber;
            
            return View(model);
        }

        [HttpPost]
        [Route("{id}")]
        public IActionResult Update(long id, UpdateViewModel model)
        {
            model.AccountTypes = GetAccountTypes();
            model.Providers = GetProviders();

            if (!ModelState.IsValid)
            {
                return View("Load", model);
            }

            var account = new Account()
            {
                Id = id,
                AccountTypeId = model.AccountTypeId,
                ProviderId = model.ProviderId,
                Name = model.Name,
                AccountNumber = model.Number
            };

            var result = _accountService.Update(account);

            if (result.HasErrors())
            {
                model.Message = result.GetFailure();
                return View("Load", model);
            }

            return RedirectToAction("Index");
        }

        private NewViewModel BuildNewViewModel()
        {
            return new NewViewModel()
            {
                Providers = GetProviders(),
                AccountTypes = GetAccountTypes()
            };
            
        }

        private UpdateViewModel BuildUpdateViewModel(long id)
        {
            return new UpdateViewModel()
            {
                Id = id,
                Providers = GetProviders(),
                AccountTypes = GetAccountTypes()
            };
        }

        private IList<Item> GetProviders()
        {
            var result = _providerService.GetAll();

            if (result.IsSuccess())
            {
                var list = from p in result.GetPayload()
                    select new Item(p.Id.ToString(), p.Country + " - " + p.Name);

                return list.ToList();
            }
            
            return new List<Item>();
        }

        private IList<Item> GetAccountTypes()
        {
            var result = _accountTypeService.GetAccountTypes();

            if (result.IsSuccess())
            {
                var list = from at in result.GetPayload()
                    select new Item(at.Id.ToString(), at.Name);

                return list.ToList();
            }

            return new List<Item>();
        }
    }
}