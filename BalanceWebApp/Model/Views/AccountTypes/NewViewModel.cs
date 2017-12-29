using System.ComponentModel.DataAnnotations;

namespace BalanceWebApp.Model.Views.AccountTypes
{
    public class NewViewModel : BaseViewModel
    {
        [Required]
        [MaxLength(150)]
        public string Name { get; set; }
    }
}