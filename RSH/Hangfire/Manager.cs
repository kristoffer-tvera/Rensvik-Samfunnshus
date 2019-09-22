using Hangfire;
using RSH.Utility;
using System;
using System.Reflection;

namespace RSH.Hangfire
{
    public static class Manager
    {
        public static void InitializeJobs()
        {
            try
            {
                RecurringJob.AddOrUpdate("Debug", () => Debug(),
                    Cron.Daily(4), TimeZoneInfo.Utc); //04:00 UTC

                RecurringJob.AddOrUpdate("SummaryEmail", () => SummaryEmail(),
                    Cron.Weekly(), TimeZoneInfo.Utc); //Weekly
            }
            catch (Exception ex)
            {
                LogHelper.Error(LogHelper.MethodInfo(MethodBase.GetCurrentMethod()), ex);
            }
        }

        [Queue("default")]
        [AutomaticRetry(Attempts = 2)]
        public static string Debug()
        {
            return "Hello world";
        }

        [Queue("default")]
        [AutomaticRetry(Attempts = 2)]
        public static string SummaryEmail()
        {
            var summaryEmailRecipients = Settings.SummaryEmailRecipients;
            if (string.IsNullOrWhiteSpace(summaryEmailRecipients)) return "Invalid settings, \"SummaryEmailRecipients\"";

            var emails = summaryEmailRecipients.Split(',');

            return "Hello world";
        }
    }
}