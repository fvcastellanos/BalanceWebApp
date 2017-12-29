namespace BalanceWebApp.Model.Views.Response
{
    public class ProviderResponse
    {
        public long Id { get; }
        public string Name { get; }
        public string Country { get; }

        public ProviderResponse(long id, string name, string country)
        {
            Id = id;
            Name = name;
            Country = country;
        }
    }
}