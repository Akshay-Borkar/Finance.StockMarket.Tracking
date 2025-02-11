using Finance.StockMarket.Application.Contracts.Email;
using Finance.StockMarket.Application.Models.Email;
using Microsoft.Extensions.Options;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.StockMarket.Infrastructure.EmailService
{
    public class EmailSender : IEmailSender
    {
        public EmailSettings _emailSettings { get; }
        public EmailSender(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }
        public async Task<bool> SendEmail(EmailMessage emailMessage)
        {
            var client = new SendGrid.SendGridClient(_emailSettings.ApiKey);
            var from = new EmailAddress(_emailSettings.FromAddress, _emailSettings.FromName);
            var subject = emailMessage.Subject;
            var to = new EmailAddress(emailMessage.To);
            var plainTextContent = emailMessage.Content;
            var htmlContent = emailMessage.Content;
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
            return response.IsSuccessStatusCode;
        }
    }
}
