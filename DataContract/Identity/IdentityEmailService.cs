using DataContract.Interfaces;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace DataContract.Identity
{
    class IdentityEmailService : IIdentityMessageService
    {
        public IdentityEmailService(IEmailConfiguration configuration)
        {
            this.emailConfiguration = configuration;
        }

        private IEmailConfiguration emailConfiguration;

        public Task SendAsync(IdentityMessage message)
        {
            string userName = emailConfiguration.UserName;
            string display = emailConfiguration.DisplayName;
            var from = new MailAddress(emailConfiguration.UserName, emailConfiguration.DisplayName);

            SmtpClient client = new SmtpClient(emailConfiguration.SmtpServer, emailConfiguration.Port);

            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(from.Address, emailConfiguration.Password);
            client.EnableSsl = true;

            //Creating email
            var mail = new MailMessage(from, new MailAddress(message.Destination));
            mail.Subject = message.Subject;
            mail.Body = message.Body;
            mail.IsBodyHtml = true;

            return client.SendMailAsync(mail);
        }

    }
}
