namespace BalanceWebApp.Controllers
{
    public class Routes
    {
        public const string Root = "balance";
        
        // Common routes
        public const string New = "new";
        
        // Controller routes
        public const string Accounts = "/accounts";
        public const string AccountTypes = "/account-types";
        public const string Providers = "/providers";
        public const string ScheduledTransactions = "/scheduled-transactions";
        public const string Transactions = "/transactions";
        public const string TransactionTypes = "/transaction-types";
    }
}