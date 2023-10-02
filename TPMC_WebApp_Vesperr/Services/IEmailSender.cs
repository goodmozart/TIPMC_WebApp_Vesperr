using Microsoft.Extensions.Configuration;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;

namespace TPMC_WebApp_Vesperr.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }

    //public class EmailSender : IEmailSender
    //{
    //    private readonly IConfiguration _configuration;

    //    public EmailSender(IConfiguration configuration)
    //    {
    //        _configuration = configuration;
    //    }

    //    public async Task SendEmailAsync(string email, string subject, string message)
    //    {
    //        var smtpServer = _configuration["EmailSettings:SmtpServer"];
    //        //var port = int.Parse(_configuration["EmailSettings:Port"]);
    //        //var username = _configuration["EmailSettings:Username"];
    //        //var password = _configuration["EmailSettings:Password"];

    //        using var client = new SmtpClient(smtpServer)
    //        {
    //            Credentials = new NetworkCredential(),
    //            EnableSsl = true,
    //        };

    //        var mailMessage = new MailMessage
    //        {
    //            From = new MailAddress("TPMC@toshiba.co.jp"),
    //            Subject = subject,
    //            Body = message,
    //            IsBodyHtml = true
    //        };
    //        mailMessage.To.Add(email);

    //        await client.SendMailAsync(mailMessage);
    //    }
    //}

}
