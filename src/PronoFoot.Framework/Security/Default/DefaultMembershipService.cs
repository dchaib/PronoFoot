using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Xml.Linq;
using System.Globalization;
using System.Text;
using PronoFoot.Messaging;
using System.Net.Mail;
using PronoFoot.Logging;

namespace PronoFoot.Security
{
    public class DefaultMembershipService : IMembershipService
    {
        private static TimeSpan DelayToResetPassword = new TimeSpan(1, 0, 0, 0);

        private readonly IEncryptionService encryptionService;
        private readonly IMessagingService messagingService;
        private readonly ILogger logger;

        public bool PasswordResetEnabled { get { return Membership.EnablePasswordReset; } }

        public DefaultMembershipService(IEncryptionService encryptionService, IMessagingService messagingService, ILogger logger)
        {
            this.encryptionService = encryptionService;
            this.messagingService = messagingService;
            this.logger = logger;
        }

        public MembershipCreateStatus CreateUser(string userName, string password, string email)
        {
            MembershipCreateStatus createStatus;
            Membership.CreateUser(userName, password, email, null, null, true, null, out createStatus);
            return createStatus;
        }

        public bool ValidateUser(string userName, string password)
        {
            return Membership.ValidateUser(userName, password);
        }

        public bool ChangePassword(string userName, string oldPassword, string newPassword)
        {
            MembershipUser currentUser = Membership.GetUser(userName, true /* userIsOnline */);
            return currentUser.ChangePassword(oldPassword, newPassword);
        }

        public bool ResetPassword(string userName, string newPassword)
        {
            MembershipUser currentUser = Membership.GetUser(userName);
            currentUser.UnlockUser();
            return currentUser.ChangePassword(currentUser.ResetPassword(), newPassword);
        }

        public bool SendRequestResetPasswordEmail(string userName, Func<string, string> createUrl)
        {
            MembershipUser user = Membership.GetUser(userName);

            if (user != null)
            {
                string nonce = CreateNonce(userName, DelayToResetPassword);
                string url = createUrl(nonce);

                messagingService.SendMessage(CreateRequestResetPasswordMessage(user, url));
                return true;
            }

            return false;
        }

        public string ValidateResetPassword(string nonce)
        {
            string username;
            DateTime validateByUtc;

            if (!DecryptNonce(nonce, out username, out validateByUtc))
            {
                return null;
            }

            if (validateByUtc < DateTime.UtcNow)
                return null;

            var user = Membership.GetUser(username);
            if (user == null)
                return null;

            return user.UserName;
        }




        private MailMessage CreateRequestResetPasswordMessage(MembershipUser user, string url)
        {
            string subject = "[PronoFoot] Mot de passe oublié";
            string textBody = string.Format("Cher {0},\r\n\r\nVous avez demandé à réinitialiser votre mot de passe. Suivez le lien ci-dessous pour changer votre mot de passe (ce lien est valide pendant 24 heures) :\r\n{1}\r\n\r\nL'équipe de PronoFoot", user.UserName, url);
            string htmlBody = string.Format("<!DOCTYPE HTML><html><head><meta http-equiv=Content-Type content=\"text/html; charset=iso-8859-1\"></head><body><p>Cher {0},</p><p>Vous avez demandé à réinitialiser votre mot de passe. Suivez le lien ci-dessous pour changer votre mot de passe (ce lien est valide pendant 24 heures) :<br/><a href={1}>{1}</a></p><p>L'équipe de PronoFoot</p></body></html>", user.UserName, url);

            var message = new MailMessage();
            message.Subject = subject;
            message.Body = textBody;
            message.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(htmlBody, new System.Net.Mime.ContentType("text/html")));
            message.To.Add(user.Email);
            return message;
        }

        private string CreateNonce(string userName, TimeSpan delay)
        {
            var challengeToken = new XElement("n", new XAttribute("un", userName), new XAttribute("utc", DateTime.UtcNow.Add(delay).ToString(CultureInfo.InvariantCulture))).ToString();
            var data = Encoding.UTF8.GetBytes(challengeToken);
            return Convert.ToBase64String(encryptionService.Encode(data));
        }

        private bool DecryptNonce(string nonce, out string username, out DateTime validateByUtc)
        {
            username = null;
            validateByUtc = DateTime.UtcNow;

            try
            {
                var data = encryptionService.Decode(Convert.FromBase64String(nonce));
                var xml = Encoding.UTF8.GetString(data);
                var element = XElement.Parse(xml);
                username = element.Attribute("un").Value;
                validateByUtc = DateTime.Parse(element.Attribute("utc").Value, CultureInfo.InvariantCulture);
                return DateTime.UtcNow <= validateByUtc;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error when decrypting nonce");
                return false;
            }
        }
    }
}