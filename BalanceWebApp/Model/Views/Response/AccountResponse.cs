namespace BalanceWebApp.Model.Views.Response
{
    public class AccountResponse
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string AccountNumber { get; set; }
        public double Balance { get; set; }

        public AccountTypeResponse AccountType { get; set; }
        public ProviderResponse Provider { get; set; }
    }
}