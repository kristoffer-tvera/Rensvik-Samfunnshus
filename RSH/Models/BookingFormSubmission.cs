using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RSH.Models
{
    public class BookingFormSubmission
    {
        [Required]
        [DisplayName("Fra")]
        public DateTime From { get; set; }

        [DisplayName("Til")]
        public DateTime? To { get; set; }

        [Required]
        [DisplayName("Område")]
        public string Area { get; set; }

        [Required]
        [DisplayName("Telephone number")]
        public string Telephone { get; set; }

        [Required]
        [DisplayName("Telephone number")]
        public string Address { get; set; }

        [DisplayName("Email")]
        public string Email { get; set; }

        [DisplayName("Name")]
        public string Name { get; set; } = "";

        [DisplayName("Comment")]
        public string Comment { get; set; } = "";

        [DisplayName("TimeOfDay")]
        public string TimeOfDay { get; set; } = "";

        [DisplayName("Purpose")]
        public string Purpose { get; set; } = "";
    }
}