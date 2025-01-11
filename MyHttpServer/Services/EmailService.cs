/*using System.Net.Mail;
using System.Net;
using HttpServerLibrary.Configurations;
using HttpServerLibrary;

namespace MyHttpServer.Services
{


    internal sealed class EmailService : IEmailService
    {
        private readonly AppConfig _config;

        public EmailService(AppConfig config)
        {
            _config = config;
        }

            
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            string fromEmail = _config.SmtpUser; // Получаем адрес отправителя из конфигурации

            // отправитель - устанавливаем адрес и отображаемое в письме имя
            MailAddress from = new MailAddress(fromEmail, "Tom");
            // кому отправляем
            MailAddress to = new MailAddress(email);
            // создаем объект сообщения
            MailMessage m = new MailMessage(from, to);
            // тема письма
            m.Subject = subject;
            // текст письма
            m.Body = message;
            // письмо представляет код html
            m.IsBodyHtml = true;

            // адрес smtp-сервера и порт, с которого будем отправлять письмо
            SmtpClient smtp = new SmtpClient(_config.SmtpServer, _config.SmtpPort);
            // логин и пароль
            smtp.Credentials = new NetworkCredential(fromEmail, _config.SmtpPassword);
            smtp.EnableSsl = true;

            smtp.Send(m);
        }

        public async Task SendEmailWithAttachmentAsync(string toEmail, string subject, string body, string attachmentPath)
        {
            using (var smtpClient = new SmtpClient("smtp.mail.ru", 587))
            {
                smtpClient.Credentials = new NetworkCredential("andrey051407@mail.ru", "cWj423B54kJm37uM6N2v");
                smtpClient.EnableSsl = true;

                using (var mailMessage = new MailMessage())
                {
                    mailMessage.From = new MailAddress("andrey051407@mail.ru");
                    mailMessage.To.Add(toEmail);
                    mailMessage.Subject = subject;
                    mailMessage.Body = body;
                    mailMessage.IsBodyHtml = true;

                    // Прикрепляем файл
                    if (File.Exists(attachmentPath))
                    {
                        var attachment = new Attachment(attachmentPath);
                        mailMessage.Attachments.Add(attachment);
                    }

                    await smtpClient.SendMailAsync(mailMessage);
                }
            }
        }

    }
}
*/