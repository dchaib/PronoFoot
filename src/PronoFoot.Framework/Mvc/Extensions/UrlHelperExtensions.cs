using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace PronoFoot.Mvc.Extensions
{
    public static class UrlHelperExtensions
    {
        public static string AbsoluteAction(this UrlHelper urlHelper, string action, string controllerName, object routeValues)
        {
            string url = urlHelper.Action(action, controllerName, routeValues,
                urlHelper.RequestContext.HttpContext.Request.Url.Scheme);
            return url;
        }

        //private static string MakeAbsolute(this UrlHelper urlHelper, string relativeUrl)
        //{
        //    string t = urlHelper.RequestContext.HttpContext.
        //    return relativeUrl;
        //}
    }
}
