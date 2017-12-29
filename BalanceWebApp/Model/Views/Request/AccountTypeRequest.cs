using System.ComponentModel.DataAnnotations;

namespace BalanceWebApp.Model.Views.Request
{
    public class AccountTypeRequest
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
    }
}