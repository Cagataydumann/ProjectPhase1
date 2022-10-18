using System.Threading.Tasks;

namespace ShoppApp.WebApp.EmailServices
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string htmlMessage);

    }
}