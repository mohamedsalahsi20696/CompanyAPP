using DataAccessLayer.Models;
using System.Net;
using System.Net.Mail;

namespace CompanyAPP.Helpers
{
    public class EmailSettings
    {
        public static void SendEmail(Email email)
        {
            // How to Use the Gmail SMTP Server to Send Emails for Free
            // Gmail SMTP server address: smtp.gmail.com
            // Gmail SMTP port (TLS): 587

            var client = new SmtpClient("smtp.gmail.com", 587);

            client.EnableSsl = true;
            // to sign in email
            client.Credentials = new NetworkCredential("mesho6567@gmail.com", "dwliismasbtbxxar"); // app password
            // to send email
            client.Send("mesho6567@gmail.com", email.To, email.Subject, email.Body);

        }
    }
}
