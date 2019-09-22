using Microsoft.Owin;
using Owin;
using Umbraco.Web;

[assembly: OwinStartup("Startup", typeof(RSH.Startup))]
namespace RSH
{
    public class Startup : UmbracoDefaultOwinStartup
    {
        public override void Configuration(IAppBuilder app)
        {
            base.Configuration(app);
            Hangfire.Config.Init(app);
            Hangfire.Manager.InitializeJobs();
        }
    }
}