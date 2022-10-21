using System;
using System.Net.Mail;
using System.Net;

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
            client.Credentials = new System.Net.NetworkCredential("lucifermorningstar231087@gmail.com", "Lucifer@6969");
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
