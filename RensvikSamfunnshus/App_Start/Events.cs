using RensvikSamfunnshus.Models;
using Umbraco.Core;
using Umbraco.Core.Persistence;

namespace RensvikSamfunnshus.App_Start
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
        }
    }
}