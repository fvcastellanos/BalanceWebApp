namespace BalanceWebApp.Model.Domain
{
    public class TransactionType
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool Credit { get; set; }
    }
}