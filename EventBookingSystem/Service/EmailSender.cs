using System.Net.Mail;
using System.Net.Mime;
using System.Net;
using EventBookingSystem.Service.Interface;

namespace EventBookingSystem.Service
{
    public class EmailSender : IEmailSender
    {
        public void SendEmailWithAttachment(string toAddress, string subject, string body, byte[] attachmentData, string attachmentFileName)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient smtpServer = new SmtpClient("smtp.live.com"); 

                mail.From = new MailAddress("eventbookingsystem@outlook.com"); 
                mail.To.Add(toAddress);
                mail.Subject = subject;
                mail.Body = body;

                Attachment attachment = new Attachment(new MemoryStream(attachmentData), attachmentFileName, MediaTypeNames.Application.Pdf);
                mail.Attachments.Add(attachment);

                smtpServer.Port = 587; 
                smtpServer.Credentials = new NetworkCredential("eventbookingsystem@outlook.com", "JoãoPaulo123!"); // Substitua pelo seu e-mail e senha
                smtpServer.EnableSsl = true;

                smtpServer.Send(mail);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while send email: " + ex.Message);
            }
        }
    }
}
