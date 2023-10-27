using EventBookingSystem.Service.Interface;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;

namespace EventBookingSystem.Service
{
    public class EmailSender : IEmailSender
    {
        private readonly ILogger<EmailSender> _logger;
        public EmailSender(ILogger<EmailSender> logger)
        {
            _logger = logger;
        }
        public bool SendEmailWithAttachment(string toAddress, string subject, string body, byte[] attachmentData, string attachmentFileName)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient smtpServer = new SmtpClient();

                mail.From = new MailAddress("paulocolt3@outlook.com");
                mail.To.Add(toAddress);
                mail.Subject = subject;
                mail.Body = body;

                Attachment attachment = new Attachment(new MemoryStream(attachmentData), attachmentFileName, MediaTypeNames.Application.Pdf);
                mail.Attachments.Add(attachment);

                smtpServer.Host = "smtp-mail.outlook.com";
                smtpServer.Port = 587;
                smtpServer.Credentials = new NetworkCredential("paulocolt3@outlook.com", "Joaopaulo123!");
                smtpServer.EnableSsl = true;
                smtpServer.UseDefaultCredentials = false;
                smtpServer.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error while send email: " + ex.Message);
                return false;
            }
        }
    }
}
