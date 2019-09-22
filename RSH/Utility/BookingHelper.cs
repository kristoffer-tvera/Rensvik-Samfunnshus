using RSH.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Web;
using Umbraco.Core;
using Umbraco.Core.Persistence;

namespace RSH.Utility
{
    public static class BookingHelper
    {
        public static IEnumerable<Booking> Load()
        {
            if(MemoryCache.Default.Get("bookings") is List<Booking> bookings)
            {
                return bookings.FindAll(element => element.To >= DateTime.UtcNow.AddMonths(-2));
            }

            var dbContext = ApplicationContext.Current.DatabaseContext;
            var db = dbContext.Database;
            var sql = new Sql()
                .Select("*")
                .From<Booking>(dbContext.SqlSyntax);
            bookings = db.Fetch<Booking>(sql);

            MemoryCache.Default.Add("bookings", bookings, DateTimeOffset.UtcNow.AddDays(7));

            return bookings.FindAll(element => element.To >= DateTime.UtcNow.AddMonths(-2));
        }

        public static IEnumerable<Booking> LoadOld()
        {
            if (MemoryCache.Default.Get("bookings") is List<Booking> bookings)
            {
                return bookings.FindAll(element => element.To <= DateTime.UtcNow.AddMonths(-2));
            }

            var dbContext = ApplicationContext.Current.DatabaseContext;

            var db = dbContext.Database;
            var sql = new Sql()
                .Select("*")
                .From<Booking>(dbContext.SqlSyntax);
            bookings = db.Fetch<Booking>(sql);

            MemoryCache.Default.Add("bookings", bookings, DateTimeOffset.UtcNow.AddDays(7));

            return bookings.FindAll(element => element.To <= DateTime.UtcNow.AddMonths(-2));
        }

        public static void Save(Booking booking)
        {
            var dbContext = ApplicationContext.Current.DatabaseContext;
            var db = dbContext.Database;

            db.Update(booking);
            MemoryCache.Default.Remove("bookings");
        }

        public static void New(Booking booking)
        {
            var dbContext = ApplicationContext.Current.DatabaseContext;
            var db = dbContext.Database;

            booking.Requested = DateTime.UtcNow;

            db.Insert(booking);
            MemoryCache.Default.Remove("bookings");
        }
    }
}