using Hangfire;
using RSH.Models;
using RSH.Utility;
using System;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Security.Cryptography;

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

            var _summaryEmailRecipients = EmailHelper.GetSummaryEmails();

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
                    var mailMessage = new MailMessage
                    {
                        Subject = $"Rensvik Samfunnshus, uke {weekNumber}",
                        Body = emailBody,
                        BodyEncoding = System.Text.Encoding.UTF8
                    };
                    mailMessage.To.Add(recipient);

                    client.Send(mailMessage);
                    sentEmails++;
                }
            }

            return $"Sent {sentEmails} emails, to following list of recipients: {summaryEmailRecipients}";
        }

        [Queue("default")]
        [AutomaticRetry(Attempts = 2)]
        public static string NewBooking(int id)
        {
            var token = new Token
            {
                BookingId = id,
                Expiration = DateTime.UtcNow.AddDays(1),
                Key = RandomHelper.RandomString(14)
            };

            TokenHelper.Add(token);
            var booking = BookingHelper.Get(id);
            if (booking == null) return "No valid booking registered";

            var emailRecipients = Settings.NewBookingEmailRecipients;
            if (string.IsNullOrWhiteSpace(emailRecipients)) return "Aborting early, no recipients configred";

            var emailRecipientsList = emailRecipients.Split(',');
            var mailMessage = new MailMessage();

            foreach (var emailRecipient in emailRecipientsList)
            {
                mailMessage.To.Add(emailRecipient);
            }
            mailMessage.Subject = $"Ny booking ({DateTime.UtcNow:dd.MM HH:mm})";
            mailMessage.BodyEncoding = System.Text.Encoding.UTF8;

            var body = $"Navn: {booking.Name} \r\n";
            body += $"Telefon: {booking.Telephone} \r\n";
            if (booking.From == booking.To)
            {
                body += $"Dato: {booking.From:dd.MM} \r\n";
            }
            else
            {
                body += $"Dato: {booking.From:dd.MM} - {booking.To:dd.MM} \r\n";
            }
            body += $"Kommentar: {booking.Comment} \r\n";

            body += $"Marker denne bookingen som reservert ved å klikke lenken nedenfor\r\n";
            body += Settings.NewBookingEmailLinkTarget + $"?t={token.Key}&id={id}";

            mailMessage.Body = body;

            using (var client = new SmtpClient())
            {
                client.Send(mailMessage);
            }

            return "New booking actions complete";
        }

    }
}