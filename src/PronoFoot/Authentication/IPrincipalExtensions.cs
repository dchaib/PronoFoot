using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Principal;
using PronoFoot.Models;

namespace PronoFoot.Authentication
{
    public static class IPrincipalExtensions
    {
        public static PronoFootIdentity GetPronoFootIdentity(this IPrincipal principal)
        {
            return (PronoFootIdentity)principal.Identity;
        }
    }
}