using System.Collections.Generic;
using BalanceWebApp.Model.Domain;
using BalanceWebApp.Model.Views;
using BalanceWebApp.Model.Views.TransactionTypes;
using BalanceWebApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using static BalanceWebApp.Controllers.Routes;

namespace BalanceWebApp.Controllers
{
    [Authorize]
    [Route(TransactionTypes)]
    public class TransactionTypeController : BaseController
    {
        private readonly ILogger _logger;
        private readonly TransactionTypeService _transactionTypeService;
        
        public TransactionTypeController(ILogger<TransactionTypeController> logger, TransactionTypeService transactionTypeService)
        {
            _logger = logger;
            _transactionTypeService = transactionTypeService;
        }

        public IActionResult Index()
        {
            var result = _transactionTypeService.GetAll();

            IndexViewModel model;
            if (result.HasErrors())
            {
                model = new IndexViewModel()
                {
                    TransactionTypes = new List<TransactionType>(),
                    Message = result.GetFailure()
                };

                return View(model);
            }

            model = new IndexViewModel()
            {
                TransactionTypes = result.GetPayload()
            };
            
            return View(model);
        }

        [Route(Routes.New)]
        public IActionResult New()
        {
            var model = new NewViewModel()
            {
                Options = BuildTypes()
            };
            
            return View(model);
        }

        [HttpPost]
        [Route(Routes.New)]
        public IActionResult Save(NewViewModel model)
        {
            model.Options = BuildTypes();
            
            if (!ModelState.IsValid)
            {
                return View("New", model);
            }

            var transactionType = new TransactionType()
            {
                Name = model.Name,
                Credit = model.Type.Equals("C") ? true : false
            };

            var result = _transactionTypeService.New(transactionType);

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
            var result = _transactionTypeService.GetById(id);

            var model = new UpdateViewModel()
            {
                Id = id,
                Options = BuildTypes()
            };
            
            if (result.HasErrors())
            {
                model.Message = result.GetFailure();

                return View(model);
            }

            var transactionType = result.GetPayload();
            model.Name = transactionType.Name;
            model.Type = transactionType.Credit ? "C" : "D";

            return View(model);
        }

        [Route("{id}")]
        [HttpPost]
        public IActionResult Update(long id, UpdateViewModel model)
        {
            model.Options = BuildTypes();
            if (!ModelState.IsValid)
            {
                return View("Load", model);
            }

            var transactionType = new TransactionType()
            {
                Id = id,
                Name = model.Name,
                Credit = model.Equals("C") ? true : false
            };

            var result = _transactionTypeService.Update(transactionType);

            if (result.HasErrors())
            {
                model.Message = result.GetFailure();
                return View("Load", model);
            }

            return RedirectToAction("Index");
        }

        private IList<Option> BuildTypes()
        {
            var types = new List<Option>();
            
            types.Add(new Option("C", "Credit"));
            types.Add(new Option("D", "Debit"));

            return types;
        }
    }
}