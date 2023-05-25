using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Eob_Web.Core.Services
{
    public class Email_Service : IEmailSender, IBulkEmailSender
    {
        private IConfiguration _configuration;

        private string _email;
        private string _pass;
        private string _smtp;
        private int _port;

        public Email_Service(IConfiguration configuration)
        {
            _configuration = configuration;

            _email = _configuration.GetValue<string>($"Email:Email");
            _pass = _configuration.GetValue<string>($"Email:Pass");
            _smtp = _configuration.GetValue<string>($"Email:Smtp");
            _port = _configuration.GetValue<int>($"Email:Port");
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var mail = new MailMessage(_email, email) 
            { 
                Subject = subject,
                Body = message,
                BodyEncoding = Encoding.UTF8,
                IsBodyHtml = true
            };


            var client = new SmtpClient(_smtp, _port)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_email, _pass)
            };

            await client.SendMailAsync(mail);
        }

        public async Task SendBulkEmailAsync(List<string> emails, string subject, string message)
        {
            var mail = new MailMessage
            {
                From = new MailAddress(_email),
                Subject = subject,
                Body = message,
                BodyEncoding = Encoding.UTF8,
                IsBodyHtml = true
            };

            foreach (string email in emails)
            {
                mail.To.Add(email);
            }

            var client = new SmtpClient(_smtp, _port)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_email, _pass)
            };

            await client.SendMailAsync(mail);
        }
    }
}
