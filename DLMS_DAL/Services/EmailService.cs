using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace DLMS_DAL.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> SendEmailAsync(string toEmail, string subject, string body)
        {
            try
            {
                var smtpHost = _configuration["EmailSettings:SmtpHost"];
                var smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"]);
                var smtpUsername = _configuration["EmailSettings:SmtpUsername"];
                var smtpPassword = _configuration["EmailSettings:SmtpPassword"];
                var fromEmail = _configuration["EmailSettings:FromEmail"];
                var fromName = _configuration["EmailSettings:FromName"];
                var enableSsl = bool.Parse(_configuration["EmailSettings:EnableSsl"] ?? "true");

                using (var smtpClient = new SmtpClient(smtpHost, smtpPort))
                {
                    smtpClient.EnableSsl = enableSsl;
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(fromEmail, fromName),
                        Subject = subject,
                        Body = body,
                        IsBodyHtml = true
                    };

                    mailMessage.To.Add(toEmail);

                    await smtpClient.SendMailAsync(mailMessage);
                }

                return true;
            }
            catch (Exception ex)
            {
                // Log l'erreur
                Console.WriteLine($"Erreur lors de l'envoi de l'email: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> SendPasswordNotificationAsync(string toEmail, string userName, string password)
        {
            var subject = "Bienvenue - Vos identifiants de connexion";
            
            var body = $@"
                <html>
                <head>
                    <style>
                        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
                        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                        .header {{ background-color: #4CAF50; color: white; padding: 20px; text-align: center; }}
                        .content {{ background-color: #f9f9f9; padding: 20px; border-radius: 5px; margin-top: 20px; }}
                        .password-box {{ background-color: #fff; padding: 15px; border-left: 4px solid #4CAF50; margin: 15px 0; }}
                        .footer {{ text-align: center; margin-top: 20px; font-size: 12px; color: #666; }}
                        .warning {{ color: #ff5722; font-weight: bold; }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>
                            <h1>Bienvenue sur DLMS</h1>
                        </div>
                        <div class='content'>
                            <p>Bonjour <strong>{userName}</strong>,</p>
                            <p>Votre compte a été créé avec succès. Voici vos identifiants de connexion :</p>
                            
                            <div class='password-box'>
                                <p><strong>Email :</strong> {toEmail}</p>
                                <p><strong>Mot de passe temporaire :</strong> {password}</p>
                            </div>
                            
                            <p class='warning'>⚠️ Important :</p>
                            <ul>
                                <li>Ce mot de passe est temporaire</li>
                                <li>Vous devez le changer lors de votre première connexion</li>
                                <li>Ne partagez jamais votre mot de passe avec qui que ce soit</li>
                            </ul>
                            
                            <p>Pour des raisons de sécurité, nous vous recommandons de vous connecter dès que possible et de modifier votre mot de passe.</p>
                        </div>
                        <div class='footer'>
                            <p>Cet email a été envoyé automatiquement, merci de ne pas y répondre.</p>
                            <p>&copy; 2025 DLMS - Tous droits réservés</p>
                        </div>
                    </div>
                </body>
                </html>
            ";

            return await SendEmailAsync(toEmail, subject, body);
        }
    }
}

