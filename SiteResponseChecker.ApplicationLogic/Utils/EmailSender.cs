using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using SiteResponseChecker.Domain;

namespace SiteResponseChecker.ApplicationLogic.Utils
{
    public class EmailSender
    {
        public void SendSnapshotEmail(User user, string htmlSnapshot)
        {
            string subject = String.Format("Site Response Checker Snapshot for: {0}", user.UserName);
            SendEmail(user.Email, subject, htmlSnapshot);
        }

        public void SendEmail(string to, string subject, string body)
        {
            MailMessage mailMessage = new MailMessage();
            // "siteresponsechecker@ivancantalice.com", to, subject, body);
            //mailMessage.From = new MailAddress(smtp)
            mailMessage.To.Add(to);
            mailMessage.Subject = subject;
            mailMessage.Body = body;
            mailMessage.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Send(mailMessage);
        }
    }
}
