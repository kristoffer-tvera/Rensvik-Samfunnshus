using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RensvikSamfunnshus.Models
{
    public class BookingFormSubmission
    {
        [Required]
        [DisplayName("Fra")]
        public DateTime From { get; set; }

        [Required]
        [DisplayName("Til")]
        public DateTime To { get; set; }

        [Required]
        [DisplayName("Område")]
        public string Area { get; set; }

        [Required]
        [DisplayName("Telephone number")]
        public string Telephone { get; set; }

        [DisplayName("Email")]
        public string Email { get; set; }

        [DisplayName("Comment")]
        public string Comment { get; set; }

        [DisplayName("Wash")]
        public bool Wash { get; set; }
    }
}