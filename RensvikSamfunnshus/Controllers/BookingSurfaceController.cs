using RensvikSamfunnshus.Models;
using System.Web.Mvc;
using Umbraco.Web.Mvc;

namespace RensvikSamfunnshus.Controllers
{
    public class BookingSurfaceController : SurfaceController
    {
        [HttpPost]
        [ValidateAntiForgeryToken]
        public bool Submit(Booking booking)
        {
            return false;
        }

    }
}