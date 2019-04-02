using RensvikSamfunnshus.Models;

using System;
using System.Collections.Generic;
using System.Web.Http;
using Umbraco.Web.Editors;

namespace RensvikSamfunnshus.Controllers
{
    public class BookingController : UmbracoAuthorizedJsonController
    {
        [HttpPost]
        public void Save(IEnumerable<Booking> bookings)
        {
            System.Diagnostics.Debugger.Break();
        }

        [HttpGet]
        public IEnumerable<Booking> Load()
        {
            var bookings = new List<Booking>();
            bookings.Add(new Booking
            {
                Requested = new DateTime(2019, 4, 1),
                From = new DateTime(2019, 4, 10),
                To = new DateTime(2019, 4, 12),
                Area = "Stor sal",
                Telephone = "123 45 678",
                Email = "test@example.com",
                Comment = "Ingen kommentar",
                Wash = false,
                Approved = true,
                Payment = null
            });

            bookings.Add(new Booking
            {
                Requested = new DateTime(2019, 4, 2),
                From = new DateTime(2019, 4, 15),
                To = new DateTime(2019, 4, 17),
                Area = "Lille sal",
                Telephone = "123 45 678",
                Email = "test@example.com",
                Comment = "Ingen kommentar",
                Wash = true,
                Approved = false,
                Payment = null
            });

            bookings.Add(new Booking
            {
                Requested = new DateTime(2019, 4, 2),
                From = new DateTime(2019, 4, 15),
                To = new DateTime(2019, 4, 17),
                Area = "Hele huset",
                Telephone = "123 45 678",
                Email = "test@example.com",
                Wash = true,
                Approved = false,
                Payment = new DateTime(2019, 2, 2)
            });

            return bookings;
        }

    }
}