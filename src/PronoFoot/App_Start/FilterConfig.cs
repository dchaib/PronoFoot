using PronoFoot.Filters;
using System.Configuration;
using System.Web;
using System.Web.Mvc;

namespace PronoFoot
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());

            string analyticsTrackingId = ConfigurationManager.AppSettings["AnalyticsTrackingId"];
            string analyticsDomain = ConfigurationManager.AppSettings["AnalyticsDomain"];
            filters.Add(new AnalyticsFilterAttribute(analyticsTrackingId, analyticsDomain));
        }
    }
}