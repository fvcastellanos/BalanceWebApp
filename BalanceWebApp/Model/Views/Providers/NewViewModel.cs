using System.ComponentModel.DataAnnotations;

namespace BalanceWebApp.Model.Views.Providers
{
    public class NewViewModel : BaseViewModel
    {
        [Required]
        [MaxLength(150)]
        public string Name { get; set; }
        
        [Required]
        [MaxLength(2)]
        public string CountryCode { get; set; }
    }
}