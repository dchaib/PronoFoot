using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace PronoFoot.Security
{
    public class DefaultMembershipService : IMembershipService
    {
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
    }
}