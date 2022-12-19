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
using NuGet.Common;

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

        private void SendEmail(EmailSendModel request)
        {
            try
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
            catch (Exception)
            {

                throw;
            }
        }

        public Task SendEmailConfirmationEmail(APIUser user, string token)
        {
            var model = new EmailSendModel();

            var emailBody = "Please confirm your email address <a target=\"_blank\" href=\"{0}\">Click here!</a>";
            var link = Configuration["OriginUrl"] + "/auth/verifyemail?userid=" + user.Id + "&token=" + token;
            var body = String.Format(emailBody, link);

            model.To = user.Email;
            model.Subject = "GainVocab: Email confirmation";
            model.Body = body;

            SendEmail(model);
            return Task.CompletedTask;
        }

        public Task SendForgotPasswordEmail(APIUser user, string token)
        {
            var model = new EmailSendModel();

            var emailBody = "Reset your password <a target=\"_blank\" href=\"{0}\">Click here!</a>";
            var link = Configuration["OriginUrl"] + "/auth/resetpassword?userid=" + user.Id + "&token=" + token;
            var body = String.Format(emailBody, link);

            model.To = user.Email;
            model.Subject = "GainVocab: Reset password";
            model.Body = body;

            SendEmail(model);
            return Task.CompletedTask;
        }

        public Task SendNewIssueNotificationEmail(List<string> adminEmails, string issueId)
        {
            var model = new EmailSendModel();

            var emailBody = "New issue <a target=\"_blank\" href=\"{0}\">Click here!</a>";
            var link = Configuration["OriginUrl"] + "/admin/support?issueId=" + issueId;
            var body = String.Format(emailBody, link);

            model.Subject = "GainVocab Support - New Issue!";
            model.Body = body;
            foreach (var admin in adminEmails)
            {
                model.To = admin;
                SendEmail(model);
            }

            return Task.CompletedTask;
        }

        public Task SendResolvedIssueNotificationEmail(APIUser user)
        {
            var model = new EmailSendModel();

            var body = "Issue has been resolved!";

            model.To = user.Email;
            model.Subject = "GainVocab Support - Your issue has been resolved!";
            model.Body = body;
            SendEmail(model);

            return Task.CompletedTask;
        }
    }
}
