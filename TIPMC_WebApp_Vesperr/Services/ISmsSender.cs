using System.Threading.Tasks;

namespace TIPMC_WebApp_Vesperr.Services
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }
}
