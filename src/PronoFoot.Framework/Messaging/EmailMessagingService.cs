using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;

namespace PronoFoot.Messaging
{
    public class EmailMessagingService : IMessagingService
    {

        public void SendMessage(MailMessage message)
        {
            using (SmtpClient smtpClient = new SmtpClient())
            {
                if (message.To.Count == 0)
                {
                    //Logger.Error("Recipient is missing an email address");
                    return;
                }

                //message.From = new MailAddress(smtpClient.Address);
                //context.MailMessage.IsBodyHtml = context.MailMessage.Body != null && context.MailMessage.Body.Contains("<") && context.MailMessage.Body.Contains(">");

                try
                {
                    smtpClient.Send(message);
                    //Logger.Debug("Message sent to {0}: {1}", context.MailMessage.To[0].Address, context.Type);
                }
                catch (Exception ex)
                {
                    //Logger.Error(e, "An unexpected error while sending a message to {0}: {1}", context.MailMessage.To[0].Address, context.Type);
                }
            }
        }
    }
}
