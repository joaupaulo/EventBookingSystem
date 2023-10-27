namespace EventBookingSystem.Service.Interface
{
    public interface IEmailSender
    {
        public bool SendEmailWithAttachment(string toAddress, string subject, string body, byte[] attachmentData, string attachmentFileName);
    }
}
