using System.Collections.Generic;
using BalanceWebApp.Model.Domain;
using BalanceWebApp.Model.Views.Providers;
using BalanceWebApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using static BalanceWebApp.Controllers.Routes;

namespace BalanceWebApp.Controllers
{
    [Authorize]
    [Route(Providers)]
    public class ProviderController : BaseController
    {
        private readonly ILogger _logger;
        private readonly ProviderService _providerService;

        public ProviderController(ILogger<ProviderController> logger, ProviderService providerService)
        {
            _logger = logger;
            _providerService = providerService;
        }
        
        // GET
        public IActionResult Index()
        {
            var user = GetAuthenticatedUserId();
            var result = _providerService.GetAll(user);

            IndexViewModel model;
            
            if (result.HasErrors())
            {
                model = new IndexViewModel()
                {
                    Providers = new List<Provider>(),
                    Message = result.GetFailure()
                };

                return View(model);
            }

            model = new IndexViewModel()
            {
                Providers = result.GetPayload()
            };

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

            var provider = new Provider()
            {
                Country = model.CountryCode,
                Name = model.Name
            };

            var user = GetAuthenticatedUserId();
            var result = _providerService.New(provider, user);

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
            var result = _providerService.GetById(id);

            UpdateViewModel model;
            if (result.HasErrors())
            {
                model = new UpdateViewModel()
                {
                    Message = result.GetFailure()
                };

                return View(model);
            }

            model = new UpdateViewModel()
            {
                Id = id,
                Name = result.GetPayload().Name,
                CountryCode = result.GetPayload().Country
            };

            return View(model);
        }

        [Route("{id}")]
        [HttpPost]
        public IActionResult Update(long id, UpdateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Load", model);
            }

            var provider = new Provider()
            {
                Id = model.Id,
                Name = model.Name,
                Country = model.CountryCode
            };

            var result = _providerService.Update(provider);

            if (result.HasErrors())
            {
                model.Message = result.GetFailure();
                return View("Load", model);
            }

            return RedirectToAction("Index");
        }
    }
}