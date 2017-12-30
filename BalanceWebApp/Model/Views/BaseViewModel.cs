using System.Collections.Generic;

namespace BalanceWebApp.Model.Views
{
    public abstract class BaseViewModel
    {
        public string Message { get; set; }
        
        public IEnumerable<Option> Options { get; set; }        
    }
}