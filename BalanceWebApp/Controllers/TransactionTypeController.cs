using System.Collections.Generic;
using BalanceWebApp.Model.Domain;
using BalanceWebApp.Model.Views.TransactionTypes;
using BalanceWebApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BalanceWebApp.Controllers
{
    [Route("/transaction-types")]
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
            }

            model = new IndexViewModel()
            {
                TransactionTypes = result.GetPayload()
            };
            
            return View(model);
        }

        [Route("new")]
        public IActionResult New()
        {
            return View();
        }
    }
}