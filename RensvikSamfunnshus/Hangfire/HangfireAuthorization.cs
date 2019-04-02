using Hangfire.Dashboard;
using System.Linq;
using System.Web;
using Umbraco.Core.Security;
using Umbraco.Web;
using Umbraco.Web.Mvc;

namespace RensvikSamfunnshus.Hangfire
{
    [UmbracoAuthorize]
    class HangfireAuthorization : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            var httpContextWrapper = new HttpContextWrapper(HttpContext.Current);
            var ticket = httpContextWrapper.GetUmbracoAuthTicket();
            httpContextWrapper.AuthenticateCurrentRequest(ticket, true);
            var user = UmbracoContext.Current.Security.CurrentUser;

            return user != null && user.AllowedSections.Contains("developer");
        }
    }
}