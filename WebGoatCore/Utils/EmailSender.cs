using System;
using System.Net.Mail;

namespace WebGoatCore
{
    public class EmailSender
    {
        public static void Send(string to, string subject, string messageBody)
        {
            var message = new MailMessage("lucifermorningstar231087@gmail.com", to)
            {
                Subject = subject,
                IsBodyHtml = true,
                Body = messageBody,
            };
            var client = new SmtpClient("smtp.gmail.com",587) { EnableSsl = true };
            client.UseDefaultCredentials = false;
            try
            {
                client.Send(message);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Send(MailMessage message)
        {
            var client = new SmtpClient("smtp.gmail.com", 587) { EnableSsl = true };
            try
            {
                client.Send(message);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
