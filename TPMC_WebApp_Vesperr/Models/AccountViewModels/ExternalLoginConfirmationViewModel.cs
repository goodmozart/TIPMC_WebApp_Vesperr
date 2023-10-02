using System.ComponentModel.DataAnnotations;

namespace TPMC_WebApp_Vesperr.Models.AccountViewModels
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
