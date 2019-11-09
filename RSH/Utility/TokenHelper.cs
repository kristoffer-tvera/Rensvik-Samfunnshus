using RSH.Models;
using System;
using System.Linq;
using Umbraco.Core;
using Umbraco.Core.Persistence;

namespace RSH.Utility
{
    public static class TokenHelper
    {
        public static void Add(Token token)
        {
            var dbContext = ApplicationContext.Current.DatabaseContext;
            var db = dbContext.Database;
            db.Insert(token);
        }

        public static bool Validate(string key, int bookingId)
        {
            var dbContext = ApplicationContext.Current.DatabaseContext;
            var db = dbContext.Database;

            var sql = new Sql()
               .Select("*")
               .From<Token>(dbContext.SqlSyntax)
               .Where<Token>(t => t.Key == key, dbContext.SqlSyntax)
               .Where<Token>(t => t.BookingId == bookingId, dbContext.SqlSyntax);

            var token = db.FirstOrDefault<Token>(sql);
            if (token == null) return false;

            if (token.Expiration >= DateTime.UtcNow)
            {
                db.Delete(token);
                return true;
            }

            db.Delete(token);
            return false;
        }
    }
}