using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BalanceWebApp.Model.Views.Transactions
{
    public class IndexViewModel : BaseViewModel
    {
        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
        public IEnumerable<Transaction> Transactions { get; set; }

        [Required]
        public DateTime Start { get; set; }

        [Required]
        public DateTime End { get; set; }
    }
}