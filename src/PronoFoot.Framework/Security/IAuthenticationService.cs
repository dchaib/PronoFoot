using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using PronoFoot.Security;

namespace PronoFoot.Security
{
    public interface IAuthenticationService
    {
        void SignIn(HttpContextBase httpContext, User user, bool createPersistentCookie);
        void SignOut();
        User GetAuthenticatedUser(HttpContextBase httpContext);
    }
}