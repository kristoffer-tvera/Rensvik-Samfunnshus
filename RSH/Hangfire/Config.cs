using System;
using System.Reflection;
using Hangfire;
using Hangfire.SqlServer;
using Owin;
using Umbraco.Core.Logging;

namespace RSH.Hangfire
{
    public class Config
    {
        public static void Init(IAppBuilder app)
        {
            try
            {
                GlobalConfiguration.Configuration.UseSqlServerStorage("umbracoDbDSN", new SqlServerStorageOptions
                {
                    PrepareSchemaIfNecessary = true
                });

                GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute { Attempts = 2 });

                app.UseHangfireDashboard("/hangfire", new DashboardOptions
                {
                    Authorization = new[] { new AuthorizationFilter() },
                    AppPath = "/"
                });

                app.UseHangfireServer(new BackgroundJobServerOptions
                {
                    SchedulePollingInterval = TimeSpan.FromSeconds(5),
                    WorkerCount = Math.Max(Environment.ProcessorCount * 5 / 2, 20),
                    Queues = new[] { "critical", "default" }
                });
            }
            catch (Exception ex)
            {
                LogHelper.Error(MethodBase.GetCurrentMethod().DeclaringType, ex.Message, ex);
            }
        }
    }
}