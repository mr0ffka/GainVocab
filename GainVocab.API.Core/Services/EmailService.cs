﻿using GainVocab.API.Core.Interfaces;
using GainVocab.API.Core.Models.Email;
using Microsoft.Extensions.Configuration;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using Microsoft.AspNetCore.Identity;
using GainVocab.API.Data.Models;
using Microsoft.EntityFrameworkCore.Migrations.Internal;

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
            email.From.Add(MailboxAddress.Parse(Configuration["EmailHandlers:SMTP:Ethereal:Email"]));
            email.To.Add(MailboxAddress.Parse(request.To));
            email.Subject = request.Subject;
            email.Body = new TextPart(TextFormat.Html) { Text = request.Body };

            using var smtp = new SmtpClient();
            smtp.Connect(Configuration["EmailHandlers:SMTP:Ethereal:Host"], int.Parse(Configuration["EmailHandlers:SMTP:Ethereal:Port"]), SecureSocketOptions.StartTls);
            smtp.Authenticate(Configuration["EmailHandlers:SMTP:Ethereal:Email"], Configuration["EmailHandlers:SMTP:Ethereal:Password"]);
            smtp.Send(email);
            smtp.Disconnect(true);
        }

        public async Task SendEmailConfirmation(APIUser user, string callbackUrl)
        {
            var model = new EmailSendModel();

            var code = await UserManager.GenerateEmailConfirmationTokenAsync(user);
            var emailBody = "Please confirm your email address <a href=\"#URL#\">Click here!</a>";
            var link = callbackUrl + "code=" + code;
            var body = emailBody.Replace("#URL#", System.Text.Encodings.Web.HtmlEncoder.Default.Encode(link));

            model.To = user.Email;
            model.Subject = "GainVocab: Email confirmation";
            model.Body = body;

            SendEmail(model);
        }
    }
}