using System.Collections.Generic;
using BalanceWebApp.Model.Domain;

namespace BalanceWebApp.Model.Views.Providers
{
    public class IndexViewModel : BaseViewModel
    {
        public IEnumerable<Provider> Providers { get; set; }
    }
}