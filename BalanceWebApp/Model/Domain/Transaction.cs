using System;

namespace BalanceWebApp.Model
{
    public class Transaction
    {
        public long Id { get; set; }
        public long TransactionTypeId { get; set; }
        public long AccountId { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }
        public string Currency { get; set; }
    }
}
