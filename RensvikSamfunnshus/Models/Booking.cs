using System;

namespace RensvikSamfunnshus.Models
{
    public class Booking
    {
        public DateTime Requested { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public string Area { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }
        public string Comment { get; set; }
        public bool Wash { get; set; }
        public bool Approved { get; set; }
        public DateTime? Payment { get; set; }

    }
}