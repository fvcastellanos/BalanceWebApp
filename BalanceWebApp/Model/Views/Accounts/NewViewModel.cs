using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BalanceWebApp.Model.Domain;

namespace BalanceWebApp.Model.Views.Accounts
{
    public class NewViewModel : BaseViewModel
    {
        [Required]
        [MaxLength(150)]
        public string Name { get; set; }

        [Required]
        [MaxLength(100)]
        public string Number { get; set; }
        
        [Required]
        public long AccountTypeId { get; set; }
        
        [Required]
        public long ProviderId { get; set; }
        
        public IEnumerable<Option> Providers { get; set; }
        
        public IEnumerable<Option> AccountTypes { get; set; }
    }
}
