using System.Threading.Tasks;

namespace ChatApp.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
