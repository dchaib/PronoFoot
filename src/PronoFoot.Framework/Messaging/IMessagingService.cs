using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;

namespace PronoFoot.Messaging
{
    public interface IMessagingService
    {
        void SendMessage(MailMessage message);
    }
}
