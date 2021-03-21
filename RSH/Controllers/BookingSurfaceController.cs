using RSH.Models;
using RSH.Utility;
using System;
using System.Runtime.Caching;
using System.Web.Mvc;
using Umbraco.Web.Mvc;

namespace RSH.Controllers
{
    public class BookingSurfaceController : SurfaceController
    {
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Submit(BookingFormSubmission submission, int nodeId = 0)
        {
            if (!ModelState.IsValid)
            {
                TempData["ModalTitle"] = "Feil i skjemaet";
                TempData["ModalBody"] = "Det ser ut som at skjemaet ikke har blitt riktig fylt ut. Prøv på nytt.";
                return RedirectToUmbracoPage(nodeId);
            }

            var booking = new Booking
            {
                Requested = DateTime.UtcNow,
                From = submission.From,
                To = submission.To ?? submission.From,
                Area = submission.Area,
                Telephone = submission.Telephone.Substring(0, Math.Min(254, submission.Telephone.Length)),
                Name = submission.Name.Substring(0, Math.Min(254, submission.Name.Length)),
                Address = submission.Address.Substring(0, Math.Min(254, submission.Address.Length)),
                Email = submission.Email.Substring(0, Math.Min(254, submission.Email.Length)),
                Comment = submission.Comment,
                TimeOfDay = submission.TimeOfDay.Substring(0, Math.Min(254, submission.TimeOfDay.Length)),
                Purpose = submission.Purpose.Substring(0, Math.Min(254, submission.Purpose.Length))
            };

            try
            {
                BookingHelper.New(booking);
            }
            catch (Exception e)
            {
                LogHelper.Error($"Feil under booking for [{submission.Name}, {submission.Telephone}]", e);
                TempData["ModalTitle"] = "Feil i skjemaet";
                TempData["ModalBody"] = "Det ser ut som at skjemaet ikke har blitt riktig fylt ut. Prøv på nytt.";
            }

            TempData["ModalTitle"] = "Vi har mottatt din forespørsel.";
            TempData["ModalBody"] = "Vi kommer til å ta kontakt i løpet av de nærmeste dager for å bekrefte din reservasjon.";
            return RedirectToUmbracoPage(nodeId);
        }

        [HttpGet]
        public string Reserver([Bind(Prefix = "t")]string token, int id)
        {
            if (TokenHelper.Validate(token, id))
            {
                var booking = BookingHelper.Get(id);
                if (booking != null)
                {
                    booking.Reserved = true;
                    BookingHelper.Save(booking);
                }
            }
            MemoryCache.Default.Remove("bookings");

            return "Bookingen har blitt markert som \"Reservert\"";
        }

    }
}