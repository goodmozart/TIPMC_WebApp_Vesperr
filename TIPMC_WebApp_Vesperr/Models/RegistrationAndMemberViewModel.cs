using TIPMC_WebApp_Vesperr.Models.AccountViewModels;

namespace TIPMC_WebApp_Vesperr.Models
{
    public class RegistrationAndMemberViewModel
    {
        public RegisterViewModel RegisterModel { get; set; }
        public TIPMC_WebApp_Vesperr.Models.POS.Member MemberModel { get; set; }
    }
}
