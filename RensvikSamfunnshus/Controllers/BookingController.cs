using RensvikSamfunnshus.Models;
using System.Collections.Generic;
using System.Web.Http;
using Umbraco.Core.Persistence;
using Umbraco.Web.Editors;

namespace RensvikSamfunnshus.Controllers
{
    public class BookingController : UmbracoAuthorizedJsonController
    {
        [HttpPost]
        public void Save(Booking booking)
        {
            var dbContext = ApplicationContext.DatabaseContext;

            var db = dbContext.Database;
            var sql = new Sql()
                .Select("*")
                .From<Booking>(dbContext.SqlSyntax)
                .Where<Booking>(f => f.Id == booking.Id, dbContext.SqlSyntax);
            var correctBooking = db.FirstOrDefault<Booking>(sql);

            db.Update(booking);
        }

        [HttpGet]
        public IEnumerable<Booking> Load()
        {
            var dbContext = ApplicationContext.DatabaseContext;

            var db = dbContext.Database;
            var sql = new Sql()
                .Select("*")
                .From<Booking>(dbContext.SqlSyntax);
            var bookings = db.Fetch<Booking>(sql);

            return bookings;

            //var bookings = new List<Booking>();
            //bookings.Add(new Booking
            //{
            //    Requested = new DateTime(2019, 4, 1),
            //    From = new DateTime(2019, 4, 10),
            //    To = new DateTime(2019, 4, 12),
            //    Area = "Stor sal",
            //    Telephone = "123 45 678",
            //    Email = "test@example.com",
            //    Comment = "Ingen kommentar",
            //    Wash = false,
            //    Approved = true,
            //    Payment = null
            //});

            //bookings.Add(new Booking
            //{
            //    Requested = new DateTime(2019, 4, 2),
            //    From = new DateTime(2019, 4, 15),
            //    To = new DateTime(2019, 4, 17),
            //    Area = "Lille sal",
            //    Telephone = "123 45 678",
            //    Email = "test@example.com",
            //    Comment = "Ingen kommentar",
            //    Wash = true,
            //    Approved = false,
            //    Payment = null
            //});

            //bookings.Add(new Booking
            //{
            //    Requested = new DateTime(2019, 4, 2),
            //    From = new DateTime(2019, 4, 15),
            //    To = new DateTime(2019, 4, 17),
            //    Area = "Hele huset",
            //    Telephone = "123 45 678",
            //    Email = "test@example.com",
            //    Wash = true,
            //    Approved = false,
            //    Payment = new DateTime(2019, 2, 2)
            //});

            //return bookings;
        }

    }
}