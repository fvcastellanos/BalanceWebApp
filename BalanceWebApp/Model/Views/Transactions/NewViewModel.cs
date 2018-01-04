using System;
using System.ComponentModel.DataAnnotations;

namespace BalanceWebApp.Model.Views.Transactions
{
    public class NewViewModel : BaseViewModel
    {
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        
        [Required]
        public long TransactionTypeId { get; set; }
        
        [Required]
        public long AccountId { get; set; }
        
        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        
        [MaxLength(150)]
        public string Description { get; set; }
        
        [Required]
        [DataType(DataType.Currency)]
        public double Amount { get; set; }
        
        [Required]
        [MaxLength(3)]
        public string Currency { get; set; }
    }
}