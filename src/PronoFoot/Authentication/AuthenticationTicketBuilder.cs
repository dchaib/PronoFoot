using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PronoFoot.Data.Model;
using System.Web.Security;
using PronoFoot.Models;

namespace PronoFoot.Authentication
{
    public static class AuthenticationTicketBuilder
    {
        public static FormsAuthenticationTicket CreateTicket(User user, bool isPersistent)
        {
            UserInfo userInfo = CreateUserInfoFromUser(user);

            var ticket = new FormsAuthenticationTicket(
                1,
                user.Login,
                DateTime.Now,
                DateTime.Now.Add(FormsAuthentication.Timeout),
                isPersistent,
                userInfo.ToString());

            return ticket;
        }

        private static UserInfo CreateUserInfoFromUser(User user)
        {
            var userInfo = new UserInfo
            {
                UserId = user.UserId,
                Login = user.Login,
                DisplayName = user.Name
            };

            return userInfo;
        }
    }
}