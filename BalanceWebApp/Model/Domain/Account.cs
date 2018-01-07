
namespace BalanceWebApp.Model.Domain
{
    public class Account
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string AccountNumber { get; set; }
        public double Balance { get; set; }

        public long AccountTypeId { get; set; }
        public string AccountType { get; set; }

        public long ProviderId { get; set; }
        public string Provider { get; set; }
        public string ProviderCountry { get; set; }
    }
}