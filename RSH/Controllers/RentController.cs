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

            var dateList = new List<Tuple<DateTime, bool>>();
            foreach (var booking in currentBookings.Where(booking => booking.Confirmed || booking.Reserved))
            {
                if (booking.From == booking.To || booking.From > booking.To)
                {
                    dateList.Add(new Tuple<DateTime, bool>(booking.From, booking.Confirmed));
                }
                else
                {
                    /*
                     * Bookings over multiple days requires custom handling. The easiest is
                     * to just start on the booking-start-date, and increment until we reach
                     * booking end date. If we've incremented more than N times, something is wrong. 
                     */
                    var to = booking.To;
                    var from = booking.From;
                    var safeguard = 0;
                    var tempDateList = new List<Tuple<DateTime, bool>>();
                    while (from <= to && safeguard < Settings.MaxBookingLength)
                    {
                        tempDateList.Add(new Tuple<DateTime, bool>(from, booking.Confirmed));
                        from = from.AddDays(1);
                        safeguard++;
                    }

                    if (safeguard > Settings.MaxBookingLength)
                    {
                        dateList.AddRange(tempDateList);
                    }
                    else
                    {
                        Logger.Warn(typeof(RentController), $"There is a booking that exceeds Max Booking Length ({Settings.MaxBookingLength}).");
                    }
                }
            }

            viewModel.CurrentBookings = dateList;
            return CurrentTemplate(viewModel);
        }
    }
}