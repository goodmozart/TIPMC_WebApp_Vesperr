using System.Threading.Tasks;

namespace TPMC_WebApp_Vesperr.Services
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }
}
