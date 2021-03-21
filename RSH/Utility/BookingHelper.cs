using Hangfire;
using RSH.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using Umbraco.Core;
using Umbraco.Core.Persistence;

namespace RSH.Utility
{
    public static class BookingHelper
    {
        public static IEnumerable<Booking> Get()
        {
            if (MemoryCache.Default.Get("bookings") is List<Booking> bookings)
            {
                return bookings
                    .FindAll(element => element.To >= DateTime.UtcNow.AddMonths(-2))
                    .OrderByDescending(b => b.From);
            }


            var dbContext = ApplicationContext.Current.DatabaseContext;
            var db = dbContext.Database;
            var sql = new Sql()
                .Select("*")
                .From<Booking>(dbContext.SqlSyntax);
            bookings = db.Fetch<Booking>(sql);

            MemoryCache.Default.Add("bookings", bookings, DateTimeOffset.UtcNow.AddDays(7));

            return bookings.FindAll(element => element.To >= DateTime.UtcNow.AddMonths(-2))
                .OrderByDescending(b => b.From).ToList();
        }

        public static Booking Get(int id)
        {
            if (MemoryCache.Default.Get("bookings") is List<Booking> bookings)
            {
                return bookings.FirstOrDefault(b => b.Id == id);
            }

            var dbContext = ApplicationContext.Current.DatabaseContext;
            var db = dbContext.Database;
            var sql = new Sql()
                .Select("*")
                .From<Booking>(dbContext.SqlSyntax);
            bookings = db.Fetch<Booking>(sql);

            MemoryCache.Default.Add("bookings", bookings, DateTimeOffset.UtcNow.AddDays(7));

            return bookings.FirstOrDefault(b => b.Id == id);
        }

        public static IEnumerable<Booking> GetOld()
        {
            if (MemoryCache.Default.Get("bookings") is List<Booking> bookings)
            {
                return bookings.FindAll(element => element.To <= DateTime.UtcNow.AddMonths(-2))
                    .OrderByDescending(b => b.From);
            }

            var dbContext = ApplicationContext.Current.DatabaseContext;

            var db = dbContext.Database;
            var sql = new Sql()
                .Select("*")
                .From<Booking>(dbContext.SqlSyntax);
            bookings = db.Fetch<Booking>(sql);

            MemoryCache.Default.Add("bookings", bookings, DateTimeOffset.UtcNow.AddDays(7));

            return bookings.FindAll(element => element.To <= DateTime.UtcNow.AddMonths(-2))
                .OrderByDescending(b => b.From);
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

            BackgroundJob.Enqueue(() => Hangfire.Manager.NewBooking(booking.Id));

            MemoryCache.Default.Remove("bookings");
        }

    }
}