using RensvikSamfunnshus.Models;
using System;
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

            db.Update(booking);
        }

        [HttpPost]
        public void New(Booking booking)
        {
            var dbContext = ApplicationContext.DatabaseContext;
            var db = dbContext.Database;

            booking.Requested = DateTime.UtcNow;

            db.Insert(booking);
            //db.Update(booking);
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

            return bookings.FindAll(element => element.To >= DateTime.UtcNow.AddMonths(-2));

        }

        [HttpGet]
        public IEnumerable<Booking> LoadOld()
        {
            var dbContext = ApplicationContext.DatabaseContext;

            var db = dbContext.Database;
            var sql = new Sql()
                .Select("*")
                .From<Booking>(dbContext.SqlSyntax);
            var bookings = db.Fetch<Booking>(sql);

            return bookings.FindAll(element => element.To <= DateTime.UtcNow.AddMonths(-2));

        }

    }
}