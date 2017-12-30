using System.Collections.Generic;
using BalanceWebApp.Model.Domain;

namespace BalanceWebApp.Model.Views.TransactionTypes
{
    public class IndexViewModel : BaseViewModel
    {
        public IEnumerable<TransactionType> TransactionTypes { get; set; }
    }
}