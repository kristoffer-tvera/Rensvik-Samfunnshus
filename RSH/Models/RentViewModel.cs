using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using Umbraco.Core.Models;
using Umbraco.Web.Models;

namespace RSH.Models
{
    public class RentViewModel : RenderModel
    {
        public RentViewModel(IPublishedContent content, CultureInfo culture) : base(content, culture)
        {
        }

        public IEnumerable<Tuple<DateTime, bool>> CurrentBookings { get; set; }
        public string ModalTitle { get; set; }
        public string ModalBody { get; set; }

    }
}