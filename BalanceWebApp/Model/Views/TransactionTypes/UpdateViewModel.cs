using System;

namespace BalanceWebApp.Model.Views.TransactionTypes
{
    public class UpdateViewModel : BaseViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
    }
}