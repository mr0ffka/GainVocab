using GainVocab.API.Core.Interfaces;
using GainVocab.API.Core.Models.Email;
using Microsoft.Extensions.Configuration;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using Microsoft.AspNetCore.Identity;
using GainVocab.API.Data.Models;
using System.Web;

namespace GainVocab.API.Core.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration Configuration;
        private readonly UserManager<APIUser> UserManager;

        public EmailService(IConfiguration config, UserManager<APIUser> userManager)
        {
            Configuration = config;
            UserManager = userManager;
        }

        public void SendEmail(EmailSendModel request)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(Configuration["EmailHandlers:SMTP:Email"]));
            email.To.Add(MailboxAddress.Parse(request.To));
            email.Subject = request.Subject;
            email.Body = new TextPart(TextFormat.Html) { Text = request.Body };

            using var smtp = new SmtpClient();
            smtp.Connect(Configuration["EmailHandlers:SMTP:Host"], int.Parse(Configuration["EmailHandlers:SMTP:Port"]), SecureSocketOptions.StartTls);
            smtp.Authenticate(Configuration["EmailHandlers:SMTP:Email"], Configuration["EmailHandlers:SMTP:Password"]);
            smtp.Send(email);
            smtp.Disconnect(true);
        }

        public Task SendEmailConfirmationEmail(APIUser user, string code)
        {
            var model = new EmailSendModel();

            var emailBody = "Please confirm your email address <a target=\"_blank\" href=\"#URL#\">Click here!</a>";
            var link = Configuration["OriginUrl"] + "/auth/verifyemail?userid=" + user.Id + "&code=" + code;
            var body = emailBody.Replace("#URL#", link);

            model.To = user.Email;
            model.Subject = "GainVocab: Email confirmation";
            model.Body = body;

            SendEmail(model);
            return Task.CompletedTask;
        }

        public Task SendForgotPasswordEmail(APIUser user, string code)
        {
            var model = new EmailSendModel();

            var emailBody = "Reset your password <a target=\"_blank\" href=\"#URL#\">Click here!</a>";
            var link = Configuration["OriginUrl"] + "/auth/resetpassword?userid=" + user.Id + "&code=" + code;
            var body = emailBody.Replace("#URL#", link);

            model.To = user.Email;
            model.Subject = "GainVocab: Reset password";
            model.Body = body;

            SendEmail(model);
            return Task.CompletedTask;
        }
    }
}
