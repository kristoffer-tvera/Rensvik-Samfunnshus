using Hangfire;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using RSH.Models;
using RSH.Utility;
using System;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

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
            var emails = EmailHelper.GetSummaryEmails();
            if (emails == null || !emails.Any())
                return "No summary email recipients";

            var bookings = BookingHelper.Get();
            var nextWeekBookings = bookings.Where(booking =>
                (booking.Confirmed || booking.Reserved) &&
                booking.From >= DateTime.Now.AddDays(-1) &&
                booking.From <= DateTime.Now.AddDays(7)).ToList();

            var weekNumber = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(DateTime.UtcNow,
                CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            var sb = new StringBuilder();
            sb.AppendLine($"Kommende uke ({weekNumber}) \r\n\r\n");

            if (nextWeekBookings.Any())
            {
                foreach (var booking in nextWeekBookings.OrderBy(b => b.From))
                {
                    sb.AppendLine($"Navn: {booking.Name}");
                    sb.AppendLine($"Telefon: - {booking.Telephone}");

                    if (booking.From == booking.To)
                    {
                        sb.AppendLine($"Dato: {booking.From:dd.MM}");
                    }
                    else
                    {
                        sb.AppendLine($"Dato: {booking.From:dd.MM} - {booking.To:dd.MM}");
                    }

                    sb.AppendLine($"Område - {booking.Area}");
                    sb.AppendLine($"Melding: {booking.Comment}");
                    sb.AppendLine(); // Blank line between bookings
                }
            }
            else
            {
                sb.AppendLine("Ingen bookinger for kommende uke");
            }

            // Create the email once
            var message = new MimeMessage();

            // From address — must be a verified sender in Resend
            message.From.Add(new MailboxAddress("Rensvik Samfunnshus", "rensvik.samfunnshus@bzl.no"));

            // Add all recipients
            foreach (var recipient in emails)
            {
                message.To.Add(MailboxAddress.Parse(recipient));
            }

            // Subject
            message.Subject = $"Rensvik Samfunnshus, uke {weekNumber}";

            // Body (HTML in this example)
            message.Body = new TextPart("html")
            {
                Text = sb.ToString()
            };

            // Send via Resend
            using (var client = new SmtpClient())
            {
                client.ServerCertificateValidationCallback = (s, c, h, e) => true; // Dev mode — disable in production

                client.Connect("smtp.resend.com", 587, SecureSocketOptions.StartTls);
                var apiKey = ConfigurationManager.AppSettings["ResendApiKey"];
                client.Authenticate("resend", apiKey);

                client.Send(message);
                client.Disconnect(true);
            }

            var sentEmails = emails.Count();
            return $"Sent {sentEmails} emails, to following list of recipients: {string.Join(", ", emails)}";
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

            var emailRecipientsList = EmailHelper.GetNewBookingEmails();

            // Build the email
            var message = new MimeMessage();

            // From address — must be verified in Resend
            message.From.Add(new MailboxAddress("Rensvik Samfunnshus", "rensvik.samfunnshus@bzl.no"));

            // Add all recipients
            foreach (var emailRecipient in emailRecipientsList)
            {
                message.To.Add(MailboxAddress.Parse(emailRecipient));
            }

            message.Subject = $"Ny booking ({DateTime.UtcNow:dd.MM HH:mm})";

            // Build body
            var bodyBuilder = new StringBuilder();
            bodyBuilder.AppendLine($"Navn: {booking.Name}");
            bodyBuilder.AppendLine($"Telefon: {booking.Telephone}");
            bodyBuilder.AppendLine($"Område: {booking.Area}");

            if (booking.From == booking.To)
            {
                bodyBuilder.AppendLine($"Dato: {booking.From:dd.MM}");
            }
            else
            {
                bodyBuilder.AppendLine($"Dato: {booking.From:dd.MM} - {booking.To:dd.MM}");
            }

            bodyBuilder.AppendLine($"Tid: {booking.TimeOfDay}");
            bodyBuilder.AppendLine($"Formål: {booking.Purpose}");
            bodyBuilder.AppendLine($"Kommentar: {booking.Comment}");
            bodyBuilder.AppendLine();
            bodyBuilder.AppendLine("Marker denne bookingen som reservert ved å klikke lenken nedenfor");
            bodyBuilder.AppendLine(Settings.NewBookingEmailLinkTarget + $"?t={token.Key}&id={id}");

            // Use plain text since it looks like a text-based email
            message.Body = new TextPart("plain")
            {
                Text = bodyBuilder.ToString()
            };

            var apiKey = ConfigurationManager.AppSettings["ResendApiKey"];

            using (var client = new SmtpClient())
            {
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                client.Connect("smtp.resend.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                client.Authenticate("resend", apiKey);

                client.Send(message);
                client.Disconnect(true);
            }

            return "New booking actions complete";
        }

    }
}