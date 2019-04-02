using Hangfire;
using Hangfire.SqlServer;
using Owin;
using RensvikSamfunnshus.Utility;
using System;

namespace RensvikSamfunnshus.Hangfire
{
    public class HangfireConfig
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
                    Authorization = new[] { new HangfireAuthorization() },
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
                LogHelper.Error(ex.Message, ex);
            }

        }
    }
}