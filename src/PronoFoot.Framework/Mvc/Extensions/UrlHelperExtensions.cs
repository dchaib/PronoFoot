using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace PronoFoot.Mvc.Extensions
{
    public static class UrlHelperExtensions
    {
        public static string ToPublicUrl(this UrlHelper urlHelper, string relativeUri)
        {
            var httpContext = urlHelper.RequestContext.HttpContext;

            //HACK port is forced to 80 (because of AppHarbour)
            var uriBuilder = new UriBuilder
            {
                Host = httpContext.Request.Url.Host,
                Path = "/",
                Port = 80,
                Scheme = "http",
            };

            if (httpContext.Request.IsLocal)
            {
                uriBuilder.Port = httpContext.Request.Url.Port;
            }

            return new Uri(uriBuilder.Uri, relativeUri).AbsoluteUri;
        }
    }
}
