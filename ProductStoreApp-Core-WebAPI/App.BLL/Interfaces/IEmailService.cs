using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace App.BLL.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string email, string subject, string message);
        Task SendEmail(string email, IFormFile excelFile);
    }
}
