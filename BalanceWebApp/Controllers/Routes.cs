namespace BalanceWebApp.Controllers
{
    public class Routes
    {
        // public const string Root = "/balance";
        public const string Root = "";

        // Assets
        public const string AssetsRoute = Root + "/assets";
        public const string Css = AssetsRoute + "/css";
        public const string Images = AssetsRoute + "/img";
        
        // Common routes
        public const string New = "new";
        public const string Delete = "delete";
        
        // Controller routes
        public const string Accounts = "/accounts";
        public const string AccountTypes = "/account-types";
        public const string Providers = "/providers";
        public const string ScheduledTransactions = "/scheduled-transactions";
        public const string Transactions = "/transactions";
        public const string TransactionTypes = "/transaction-types";
    }
}