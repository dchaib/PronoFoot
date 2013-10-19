using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using PronoFoot.Security;

namespace PronoFoot.Security
{
    public class FormsAuthenticationService : IAuthenticationService
    {
        private User user;

        public void SignIn(HttpContextBase httpContext, User user, bool createPersistentCookie)
        {
            var now = DateTime.Now;
            var userData = ConvertToUserData(user);

            var ticket = new FormsAuthenticationTicket(
                2 /*version*/,
                user.Name,
                now,
                now.Add(FormsAuthentication.Timeout),
                createPersistentCookie,
                userData,
                FormsAuthentication.FormsCookiePath);

            var encryptedTicket = FormsAuthentication.Encrypt(ticket);

            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket)
            {
                HttpOnly = true,
                Secure = FormsAuthentication.RequireSSL,
                Path = FormsAuthentication.FormsCookiePath
            };

            if (FormsAuthentication.CookieDomain != null)
            {
                cookie.Domain = FormsAuthentication.CookieDomain;
            }

            if (createPersistentCookie)
            {
                cookie.Expires = ticket.Expiration;
            }

            httpContext.Response.Cookies.Add(cookie);

            this.user = user;
        }

        public void SignOut()
        {
            user = null;
            FormsAuthentication.SignOut();
        }

        public User GetAuthenticatedUser(HttpContextBase httpContext)
        {
            if (user != null)
                return user;

            if (httpContext == null || !httpContext.Request.IsAuthenticated || !(httpContext.User.Identity is FormsIdentity))
            {
                return null;
            }

            var formsIdentity = (FormsIdentity)httpContext.User.Identity;
            var userData = formsIdentity.Ticket.UserData;
            return ConvertFromUserData(userData);
        }

        private static string ConvertToUserData(User user)
        {
            if (user == null)
                return null;

            return user.Id.ToString();
        }

        private static User ConvertFromUserData(string userData)
        {
            int userId;
            if (!int.TryParse(userData, out userId))
            {
                //Logger.Fatal("User id not a parsable integer");
                return null;
            }
            return new User { Id = userId };
        }
    }
}