using RSH.Models;
using RSH.Utility;
using System.Collections.Generic;
using System.Web.Http;
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
            return BookingHelper.Load();
        }

        [HttpGet]
        public IEnumerable<Booking> LoadOld()
        {
            return BookingHelper.LoadOld();
        }

    }
}