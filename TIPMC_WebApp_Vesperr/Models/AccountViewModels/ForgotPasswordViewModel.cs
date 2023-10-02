using System.ComponentModel.DataAnnotations;

namespace TIPMC_WebApp_Vesperr.Models.AccountViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
