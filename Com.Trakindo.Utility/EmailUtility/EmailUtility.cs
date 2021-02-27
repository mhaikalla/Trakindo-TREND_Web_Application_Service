using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Net;

namespace Com.Trakindo.Utility
{
    public class EmailUtility
    {
        public static string SMTPHost = "smtp.gmail.com";
        public static int SMTPPort = 587;
        public static bool EnableSsl = true;
        public static string EmailAccountUsername = "hudabeybi@gmail.com";
        public static string EmailAccountPassword = "Rotikeju98";

        public static void SendEmail(string from, string[] tos, string subject, string emailMessage)
        {
            foreach (string to in tos)
            {
                MailMessage mail = new MailMessage(from, to);
                SmtpClient client = new SmtpClient();
                client.Port = SMTPPort;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(from, EmailAccountPassword);
                client.Host = EmailUtility.SMTPHost;
                client.EnableSsl = EnableSsl;
                mail.Subject = subject;
                mail.Body = emailMessage;
                mail.IsBodyHtml = true;
                client.Send(mail);
            }
        }


        public static void SendEmail(string senderName, string senderEmail, string[] tos, string subject, string emailMessage)
        {
            foreach (string to in tos)
            {
                MailMessage mail = new MailMessage(senderEmail, to);
                mail.Sender = new MailAddress(senderEmail, senderName);

                SmtpClient client = new SmtpClient();
                client.Port = SMTPPort;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(EmailAccountUsername, EmailAccountPassword);
                client.Host = EmailUtility.SMTPHost;
                client.EnableSsl = EnableSsl;
                mail.Subject = subject;
                mail.Body = emailMessage;
                mail.IsBodyHtml = true;
                client.Send(mail);
            }
        }
    }
}
