using System.Collections;
using System.Collections.Generic;
using BalanceWebApp.Model.Domain;

namespace BalanceWebApp.Model.Views.AccountTypes
{
    public class IndexViewModel : BaseViewModel
    {
        public IEnumerable<AccountType> AccountTypes { get; set; }
    }
}