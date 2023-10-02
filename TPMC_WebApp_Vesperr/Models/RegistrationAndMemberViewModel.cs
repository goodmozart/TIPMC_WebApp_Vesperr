using TPMC_WebApp_Vesperr.Models.AccountViewModels;

namespace TPMC_WebApp_Vesperr.Models
{
    public class RegistrationAndMemberViewModel
    {
        public RegisterViewModel RegisterModel { get; set; }
        public TPMC_WebApp_Vesperr.Models.POS.Member MemberModel { get; set; }
    }
}
