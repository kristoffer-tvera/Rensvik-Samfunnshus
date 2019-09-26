using RSH.Models;
using RSH.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Umbraco.Web.Models;

namespace RSH.Controllers
{
    public class RentController : Umbraco.Web.Mvc.RenderMvcController
    {
        public override ActionResult Index(RenderModel model)
        {
            var viewModel = new RentViewModel(model.Content, model.CurrentCulture);

            var currentBookings = BookingHelper.Get();

            var dateList = new List<DateTime>();
            foreach (var booking in currentBookings.Where(booking => booking.Approved))
            {
                if (booking.From == booking.To || booking.From > booking.To)
                {
                    dateList.Add(booking.From);
                }
                else
                {
                    var to = booking.To;
                    var from = booking.From;
                    var safeguard = 150;

                    while (from <= to)
                    {
                        dateList.Add(from);
                        from = from.AddDays(1);

                        if (safeguard-- < 0) break;
                    }
                }

            }

            viewModel.CurrentBookings = dateList;
            return CurrentTemplate(viewModel);
        }
    }
}