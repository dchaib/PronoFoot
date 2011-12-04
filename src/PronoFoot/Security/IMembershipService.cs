using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;

namespace PronoFoot.Security
{
    public interface IMembershipService
    {
        MembershipCreateStatus CreateUser(string userName, string password, string email);
        bool ValidateUser(string userName, string password);
        bool ChangePassword(string userName, string oldPassword, string newPassword);
    }
}
