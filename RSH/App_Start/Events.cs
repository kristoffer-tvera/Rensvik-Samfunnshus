using RSH.Models;
using Umbraco.Core;
using Umbraco.Core.Persistence;

namespace RSH.App_Start
{
    public class Events : ApplicationEventHandler
    {
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            var dbContext = applicationContext.DatabaseContext;
            var db = new DatabaseSchemaHelper(dbContext.Database, applicationContext.ProfilingLogger.Logger, dbContext.SqlSyntax);

            if (!db.TableExist<Booking>())
            {
                db.CreateTable<Booking>();
            }

            if (!db.TableExist<Token>())
            {
                db.CreateTable<Token>();
            }

            if (!db.TableExist<SummaryEmail>())
            {
                db.CreateTable<SummaryEmail>();
            }

            if (!db.TableExist<NewBookingEmail>())
            {
                db.CreateTable<NewBookingEmail>();
            }
        }
    }
}