﻿using Hangfire;
using RSH.Models;
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

            var emailBody = $"Kommende uke ({weekNumber}) \r\n\r\n";

            if (nextWeekBookings.Any())
            {
                foreach (var booking in nextWeekBookings.OrderBy(b => b.From))
                {
                    var str = $"Navn: {booking.Name}\r\n" +
                              $"Telefon: - {booking.Telephone} \r\n" +
                              $"Dato: {booking.From:dd.MM}" + (booking.From == booking.To
                                  ? "\r\n"
                                  : $" - {booking.To:dd.MM} \r\n") +
                              $"Område - {booking.Area} \r\n" +
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
                        BodyEncoding = System.Text.Encoding.UTF8,
                        From = new MailAddress(EmailHelper.GetSmtpUsername(), "Rensvik Samfunnshus")
                    };
                    mailMessage.To.Add(recipient);

                    client.Send(mailMessage);
                    sentEmails++;
                }
            }

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

            var mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(EmailHelper.GetSmtpUsername(), "Rensvik Samfunnshus");

            foreach (var emailRecipient in emailRecipientsList)
            {
                mailMessage.To.Add(emailRecipient);
            }
            mailMessage.Subject = $"Ny booking ({DateTime.UtcNow:dd.MM HH:mm})";
            mailMessage.BodyEncoding = System.Text.Encoding.UTF8;

            var body = $"Navn: {booking.Name} \r\n";
            body += $"Telefon: {booking.Telephone} \r\n";
            body += $"Område: {booking.Area} \r\n";
            if (booking.From == booking.To)
            {
                body += $"Dato: {booking.From:dd.MM} \r\n";
            }
            else
            {
                body += $"Dato: {booking.From:dd.MM} - {booking.To:dd.MM} \r\n";
            }

            body += $"Tid: {booking.TimeOfDay} \r\n";
            body += $"Formål: {booking.Purpose} \r\n";
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