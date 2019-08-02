using RSH.Models;
using System;
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
                ViewData["FormError"] = "Error";
                return RedirectToUmbracoPage(nodeId);
            }

            var booking = new Booking
            {
                Requested = DateTime.UtcNow,
                From = submission.From,
                To = submission.To,
                Area = submission.Area,
                Telephone = submission.Telephone,
                Name = submission.Name,
                Comment = submission.Comment,
                Wash = submission.Wash
            };

            var dbContext = ApplicationContext.DatabaseContext;
            var db = dbContext.Database;
            db.Insert(booking);

            return RedirectToUmbracoPage(nodeId);
        }

    }
}