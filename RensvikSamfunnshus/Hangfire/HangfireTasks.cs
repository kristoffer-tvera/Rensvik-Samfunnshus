using Hangfire;
using RensvikSamfunnshus.Utility;
using System;

namespace RensvikSamfunnshus.Hangfire
{
    public class HangfireTasks
    {
        public static void Initialize()
        {
            try
            {
                RecurringJob.AddOrUpdate("Empty Task", () => EmptyTask(),
                    Cron.Daily(4), TimeZoneInfo.Utc); //04:00 UTC
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message, ex);
            }
        }

        [Queue("default")]
        [AutomaticRetry(Attempts = 2)]
        public static void EmptyTask()
        {
            Console.WriteLine("Hello World");
        }
    }
}