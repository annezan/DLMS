namespace DLMS_DAL.Services
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(string toEmail, string subject, string body);
        Task<bool> SendPasswordNotificationAsync(string toEmail, string userName, string password);
    }
}

