using System.Linq;
using System.Web;
using Hangfire.Dashboard;
using Umbraco.Core.Security;
using Umbraco.Web;

namespace RSH.Hangfire
{
    public class AuthorizationFilter : IDashboardAuthorizationFilter
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