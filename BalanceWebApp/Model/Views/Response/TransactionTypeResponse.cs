namespace BalanceWebApp.Model.Views.Response
{
    public class TransactionTypeResponse
    {
        public long Id { get; }
        public string Name { get; }
        public bool Credit { get; }

        public TransactionTypeResponse(long id, string name, bool credit)
        {
            Id = id;
            Name = name;
            Credit = credit;
        }
    }
}