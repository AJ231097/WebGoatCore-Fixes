using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;

namespace WebGoatCore.Models
{
    public class EmailHelper
    {
        public bool SendEmail(string userEmail, string confirmationLink)
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("securecoding231087@outlook.com");
            mailMessage.To.Add(new MailAddress(userEmail));

            mailMessage.Subject = "Confirm your email";
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = confirmationLink;

            SmtpClient client = new SmtpClient("smtp.office365.com");
            client.UseDefaultCredentials = true;
            client.Credentials = new System.Net.NetworkCredential("securecoding231087@outlook.com", "securecodinG@143","outlook.com");
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            //client.Host = "smtpout.secureserver.net";
            client.Port = 587;
            client.EnableSsl= true;

            try
            {
                client.Send(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Email sending failed", ex.Message);
            }
            return false;
        }
    }
}
