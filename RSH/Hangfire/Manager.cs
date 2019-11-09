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
                RecurringJob.AddOrUpdate("SummaryEmail", () => SummaryEmail(),
                    Cron.Weekly(DayOfWeek.Monday, 6), TimeZoneInfo.Utc); //Weekly
            }
            catch (Exception ex)
            {
                LogHelper.Error(LogHelper.MethodInfo(MethodBase.GetCurrentMethod()), ex);
            }
        }

        [Queue("default")]
        [AutomaticRetry(Attempts = 2)]
        public static string SummaryEmail()
        {
            var summaryEmailRecipients = Settings.SummaryEmailRecipients;
            if (string.IsNullOrWhiteSpace(summaryEmailRecipients))
                return "Invalid settings, \"SummaryEmailRecipients\"";

            var emails = summaryEmailRecipients.Split(',');

            var bookings = BookingHelper.Get();
            var nextWeekBookings = bookings.Where(booking =>
                (booking.Confirmed || booking.Reserved) &&
                booking.From >= DateTime.Now.AddDays(-1) &&
                booking.From <= DateTime.Now.AddDays(7)).ToList();

            var weekNumber = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(DateTime.UtcNow,
                CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            var emailBody = $"Kommende uke ({weekNumber}) \r\n\r\n";

            if (nextWeekBookings.Any())
            {
                foreach (var booking in nextWeekBookings)
                {
                    var str = $"Navn: {booking.Name}\r\n" +
                              $"Dato: {booking.From:dd.MM}" + (booking.From == booking.To
                                  ? "\r\n"
                                  : $" - {booking.To:dd.MM} \r\n") +
                              $"Melding: {booking.Comment}\r\n\r\n";
                    emailBody += str;
                }
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
                    var mail = new MailMessage("debug@bzl.no", recipient, $"Rensvik Samfunnshus, uke {weekNumber}",
                        emailBody);
                    client.Send(mail);
                    sentEmails++;
                }
            }

            return $"Sent {sentEmails} emails, to following list of recipients: {summaryEmailRecipients}";
        }
    }
}