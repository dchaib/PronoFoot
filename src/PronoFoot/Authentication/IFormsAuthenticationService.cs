using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace PronoFoot.Authentication
{
    public interface IFormsAuthenticationService
    {
        FormsAuthenticationTicket Decrypt(string encryptedTicket);
        void SetAuthCookie(HttpContextBase httpContext, FormsAuthenticationTicket authenticationTicket);
        void SignOut();
    }
}