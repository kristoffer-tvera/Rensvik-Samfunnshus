using Hangfire;
using RSH.Utility;
using System;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
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

            var bookings = BookingHelper.Get();
            var nextWeekBookings = bookings.Where(booking =>
            booking.Approved &&
            booking.From >= DateTime.Now.AddDays(-1) &&
            booking.From <= DateTime.Now.AddDays(7));

            var weekNumber = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(DateTime.UtcNow, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            var emailBody = $"Kommende uke ({weekNumber}) \r\n\r\n";

            if (nextWeekBookings.Any())
            {
                emailBody += nextWeekBookings.Select(booking =>
               $"Navn: {booking.Name}\r\n" +
               $"Dato: {booking.From} - {booking.To} \r\n" +
               $"Melding: {booking.Comment}\r\n\r\n");
            }
            else
            {
                emailBody += "Ingen bookinger";
            }

            var sentEmails = 0;
            using (var client = new SmtpClient())
            {
                foreach (var recipient in emails)
                {
                    var mail = new MailMessage("debug@bzl.no", recipient, $"Rensvik Samfunnshus, uke {weekNumber}", emailBody);
                    client.Send(mail);
                    sentEmails++;
                }
            }

            return $"Sent {sentEmails} emails, to following list of recipients: {summaryEmailRecipients}";
        }
    }
}