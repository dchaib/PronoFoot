using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PronoFoot.Filters
{
    public class AnalyticsFilterAttribute : ActionFilterAttribute
    {
        private string analyticsTrackingId;
        private string analyticsDomain;

        public AnalyticsFilterAttribute(string analyticsTrackingId, string analyticsDomain)
        {
            this.analyticsTrackingId = analyticsTrackingId;
            this.analyticsDomain = analyticsDomain;
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var result = filterContext.Result as ViewResultBase;
            if (result != null)
            {
                if (!string.IsNullOrWhiteSpace(analyticsTrackingId) && !string.IsNullOrWhiteSpace(analyticsDomain))
                {
                    result.ViewBag.UseAnalytics = true;
                    result.ViewBag.AnalyticsTrackingId = analyticsTrackingId;
                    result.ViewBag.AnalyticsDomain = analyticsDomain;
                }
                else
                {
                    result.ViewBag.UseAnalytics = false;
                }
            }
            base.OnActionExecuted(filterContext);
        }
    }
}