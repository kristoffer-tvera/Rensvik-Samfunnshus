using System.Collections.Generic;
using System.Linq;
using Umbraco.Core;
using Umbraco.Core.Persistence;
using RSH.Models;
using System.Configuration;
using System.Net.Configuration;

namespace RSH.Utility
{
    public static class EmailHelper
    {

        public static IEnumerable<string> GetSummaryEmails()
        {
            var dbContext = ApplicationContext.Current.DatabaseContext;
            var db = dbContext.Database;

            var sql = new Sql()
               .Select("*")
               .From<SummaryEmail>(dbContext.SqlSyntax);
               
            var emails = db.Fetch<SummaryEmail>(sql);

            return emails.Select(x => x.Email);
        }

        public static IEnumerable<string> GetNewBookingEmails()
        {
            var dbContext = ApplicationContext.Current.DatabaseContext;
            var db = dbContext.Database;

            var sql = new Sql()
               .Select("*")
               .From<NewBookingEmail>(dbContext.SqlSyntax);

            var emails = db.Fetch<NewBookingEmail>(sql);

            return emails.Select(x => x.Email);
        }
        
        public static string GetSmtpUsername()
        {
            var smtpSection = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");
            string username = smtpSection.From;
            return username;
        }
    }
}