namespace BalanceWebApp.Model.Views.Response
{
    public class AccountTypeResponse
    {
        public long Id { get; }
        public string Name { get; }

        public AccountTypeResponse(long id, string name)
        {
            Id = id;
            Name = name;
        }

    }
}