using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace DemoShop.BLL.Service
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("engdonia2002@gmail.com", "nxdh gpok palf uydz")
            };

            return client.SendMailAsync
                (
                new MailMessage(from: "engdonia2002@gmail.com",
                to: email,
                subject,
                message
                )
                { IsBodyHtml = true});
        }
    }
}
