using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Net;
using System.Net.Mail;
namespace ToolExportVideo.Common
{
    public static class EmailHelper
    {
        private static string smtpPass = "kbkb pchg rilw slgi";
        private static string fromEmail = "itteckcore@gmail.com";
        public static void SendEmail(string toEmail, string subject, string message)
        {
            try
            {
                var fromAddress = new MailAddress(fromEmail, "Thế giới việc làm");
                var toAddress = new MailAddress(toEmail, toEmail);
                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, smtpPass)
                };
                using (var mailMessage = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = message
                })
                {
                    smtp.Send(mailMessage);
                }
            }
            catch (Exception)
            {
            }
        }
    }
}
