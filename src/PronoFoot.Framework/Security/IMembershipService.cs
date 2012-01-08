using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;

namespace PronoFoot.Security
{
    public interface IMembershipService
    {
        bool PasswordResetEnabled { get; }

        MembershipCreateStatus CreateUser(string userName, string password, string email);
        bool ValidateUser(string userName, string password);
        bool ChangePassword(string userName, string oldPassword, string newPassword);
        bool ResetPassword(string userName, string newPassword);

        bool SendRequestResetPasswordEmail(string userName, Func<string, string> createUrl);
        string ValidateResetPassword(string nonce);
    }
}
