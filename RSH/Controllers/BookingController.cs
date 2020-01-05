using RSH.Models;
using RSH.Utility;
using System.Collections.Generic;
using System.Web.Http;
using Umbraco.Core.Persistence;
using Umbraco.Web.Editors;

namespace RSH.Controllers
{
    public class BookingController : UmbracoAuthorizedJsonController
    {
        [HttpPost]
        public void Save(Booking booking)
        {
            BookingHelper.Save(booking);
        }

        [HttpPost]
        public void New(Booking booking)
        {
            BookingHelper.New(booking);
        }

        [HttpGet]
        public IEnumerable<Booking> Load()
        {
            return BookingHelper.Get();
        }

        [HttpGet]
        public IEnumerable<Booking> LoadOld()
        {
            return BookingHelper.GetOld();
        }

        [HttpGet]
        public IEnumerable<string> GetSummaryEmails()
        {
            var db = new Database("umbracoDbDSN");
            var sql = new Sql().Select("Email").From<SummaryEmail>(ApplicationContext.DatabaseContext.SqlSyntax);
            var summaryEmails = db.Fetch<string>(sql);
            return summaryEmails;
        }

        [HttpGet]
        public void AddSummaryEmail(string email)
        {
            var db = new Database("umbracoDbDSN");
            db.Insert(new SummaryEmail
            {
                Email = email
            });

            return;
        }

        [HttpGet]
        public void RemoveSummaryEmail(string email)
        {
            var pepe = DatabaseContext.SqlSyntax;

            var db = new Database("umbracoDbDSN");
            var sql = new Sql()
                .Select("*")
                .From<SummaryEmail>(DatabaseContext.SqlSyntax)
                .Where<SummaryEmail>(summaryEmail => summaryEmail.Email == email, DatabaseContext.SqlSyntax);
            var record = db.FirstOrDefault<SummaryEmail>(sql);
            if (record != null)
            {
                db.Delete<SummaryEmail>(record);
            }
        }

        [HttpGet]
        public IEnumerable<string> GetNewBookingEmails()
        {
            var db = new Database("umbracoDbDSN");
            var sql = new Sql().Select("Email").From<NewBookingEmail>(DatabaseContext.SqlSyntax);
            var summaryEmails = db.Fetch<string>(sql);
            return summaryEmails;
        }

        [HttpGet]
        public void AddNewBookingEmail(string email)
        {
            var db = new Database("umbracoDbDSN");
            db.Insert(new NewBookingEmail
            {
                Email = email
            });

            return;
        }

        [HttpGet]
        public void RemoveNewBookingEmail(string email)
        {
            var pepe = DatabaseContext.SqlSyntax;

            var db = new Database("umbracoDbDSN");
            var sql = new Sql()
                .Select("*")
                .From<NewBookingEmail>(DatabaseContext.SqlSyntax)
                .Where<NewBookingEmail>(summaryEmail => summaryEmail.Email == email, DatabaseContext.SqlSyntax);
            var record = db.FirstOrDefault<NewBookingEmail>(sql);
            if (record != null)
            {
                db.Delete<NewBookingEmail>(record);
            }
        }

    }
}