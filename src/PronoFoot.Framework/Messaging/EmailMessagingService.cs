using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using PronoFoot.Logging;

namespace PronoFoot.Messaging
{
    public class EmailMessagingService : IMessagingService
    {
        private readonly ILogger logger;
        private readonly string fromAddress;

        public EmailMessagingService(ILogger logger, string fromAddress)
        {
            this.logger = logger;
            this.fromAddress = fromAddress;
        }

        public void SendMessage(MailMessage message)
        {
            using (SmtpClient smtpClient = new SmtpClient())
            {
                if (message.To.Count == 0)
                {
                    logger.Error("Recipient is missing an email address");
                    return;
                }

                if (message.From == null || string.IsNullOrWhiteSpace(message.From.Address))
                    message.From = new MailAddress(fromAddress);

                try
                {
                    smtpClient.Send(message);
                    logger.Debug("Message sent to {0}: \"{1}\"", message.To[0].Address, message.Subject);
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "An unexpected error while sending a message to {0}: \"{1}\"", message.To[0].Address, message.Subject);
                }
            }
        }
    }
}
