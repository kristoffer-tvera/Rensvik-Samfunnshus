using Microsoft.Owin;
using Owin;
using RensvikSamfunnshus.Hangfire;
using Umbraco.Web;

[assembly: OwinStartup("Startup", typeof(RensvikSamfunnshus.Startup))]
namespace RensvikSamfunnshus
{
    public class Startup : UmbracoDefaultOwinStartup
    {
        public override void Configuration(IAppBuilder app)
        {
            base.Configuration(app);
            HangfireConfig.Init(app);
            HangfireTasks.Initialize();
        }
    }
}