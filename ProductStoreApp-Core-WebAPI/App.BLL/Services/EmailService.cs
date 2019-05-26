using System.IO;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http; 
using App.BLL.Interfaces;
using MimeKit;

namespace App.BLL.Services
{
    public class EmailService : IEmailService
    {
        private readonly IHostingEnvironment _appEnvironment;

        public EmailService(IHostingEnvironment appEnvironment)
        {
            _appEnvironment = appEnvironment;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Site administration", "messendertest@mail.ru"));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.yandex.ru", 587, true);
                await client.AuthenticateAsync("a.pakholko.vironit@vironit.ru", "25032019pas");
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
        }

        public async Task SendEmail(string email, IFormFile excelFile)
        {
            var path = "/Excel/" + excelFile.FileName;
            using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
            {
                await excelFile.CopyToAsync(fileStream);
            }

            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Site administration", "messendertest@mail.ru"));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = "Excel file";

            var builder = new BodyBuilder();
            builder.Attachments.Add("D:/Study/VironIT/Apps/Core2.1-WebAPI-Angular/ProductStoreApp-Core2.1-WebAPI-Angular/ProductStoreApp-Core-WebAPI/App.API/wwwroot/" + path);
            emailMessage.Body = builder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.mail.ru", 587, false);
                await client.AuthenticateAsync("messendertest@mail.ru", "15975310895623Vladimir!");
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
        }
    }
}
