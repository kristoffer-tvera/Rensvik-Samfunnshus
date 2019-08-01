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
        public bool Submit(BookingFormSubmission submission)
        {
            if (!ModelState.IsValid) return false;

            var booking = new Booking
            {
                Requested = DateTime.UtcNow,
                From = submission.From,
                To = submission.To,
                Area = submission.Area,
                Telephone = submission.Telephone,
                Email = submission.Email,
                Comment = submission.Comment,
                Wash = submission.Wash
            };

            var dbContext = ApplicationContext.DatabaseContext;
            var db = dbContext.Database;
            db.Insert(booking);

            return true;
        }

    }
}