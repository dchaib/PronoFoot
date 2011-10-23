using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Principal;
using System.Web.Security;

namespace PronoFoot.Models
{
    public class PronoFootIdentity : IIdentity
    {
        public PronoFootIdentity(FormsAuthenticationTicket ticket)
            : this(ticket.Name, UserInfo.FromString(ticket.UserData))
        {
            if (ticket == null) throw new ArgumentNullException("ticket");
        }

        public PronoFootIdentity(string name, UserInfo userInfo)
            : this(name, userInfo.DisplayName, userInfo.UserId)
        {
            if (userInfo == null) throw new ArgumentNullException("userInfo");
            this.UserId = userInfo.UserId;
        }

        public PronoFootIdentity(string name, string displayName, int userId)
        {
            this.Name = name;
            this.DisplayName = displayName;
            this.UserId = userId;
        }

        public string AuthenticationType
        {
            get { return "PronoFoot"; }
        }

        public bool IsAuthenticated
        {
            get { return true; }
        }

        public string Name { get; private set; }
        public string DisplayName { get; private set; }
        public int UserId { get; private set; }

    }
}