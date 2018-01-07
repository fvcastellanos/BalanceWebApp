using System.ComponentModel.DataAnnotations;

namespace BalanceWebApp.Model.Views.Providers
{
    public class UpdateViewModel : BaseViewModel
    {
        [Required]
        public long Id { get; set; }
        
        [Required]
        [MaxLength(150)]
        public string Name { get; set; }
        
        [Required]
        [MaxLength(2)]
        public string CountryCode { get; set; }
    }
}