using System.Collections.Generic;
using BalanceWebApp.Model.Domain;

namespace BalanceWebApp.Model.Views.Accounts
{
    public class IndexViewModel : BaseViewModel
    {
        public IEnumerable<Account> Accounts { get; set; }
    }
}