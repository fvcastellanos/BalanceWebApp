using System.ComponentModel.DataAnnotations;

namespace BalanceWebApp.Model.Views.Request
{
    public class AccountRequest
    {
        [Required]
        public long AccountTypeId { get; set; }

        [Required]
        public long ProviderId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        public string AccountNumber { get; set; }
    }
}