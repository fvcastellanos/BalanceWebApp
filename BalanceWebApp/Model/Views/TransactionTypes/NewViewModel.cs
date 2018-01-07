using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BalanceWebApp.Model.Views.TransactionTypes
{
    public class NewViewModel : BaseViewModel
    {
        [Required]
        [MaxLength(150)]
        public string Name { get; set; }
        
        [Required]
        [MaxLength(1)]
        public string Type { get; set; }
    }
}