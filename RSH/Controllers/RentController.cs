using RSH.Models;
using RSH.Utility;
using System.Linq;
using System.Web.Mvc;
using Umbraco.Web.Models;

namespace RSH.Controllers
{
    public class RentController : Umbraco.Web.Mvc.RenderMvcController
    {
        public ActionResult Index(RenderModel model)
        {
            var viewModel = new RentViewModel(model.Content, model.CurrentCulture);

            var currentBookings = BookingHelper.Load();

            viewModel.CurrentBookings = currentBookings.Where(booking => booking.Approved).Select(booking => booking.From);

            return CurrentTemplate(viewModel);
        }
    }
}