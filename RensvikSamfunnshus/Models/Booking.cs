using System;
using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.DatabaseAnnotations;

namespace RensvikSamfunnshus.Models
{
    [TableName("Booking")]
    [PrimaryKey("Id", autoIncrement = true)]
    [ExplicitColumns]
    public class Booking
    {
        [Column("Id")]
        [PrimaryKeyColumn(AutoIncrement = true)]
        public int Id { get; set; }

        [Column("Requested")]
        public DateTime Requested { get; set; }

        [Column("From")]
        public DateTime From { get; set; }

        [Column("To")]
        public DateTime To { get; set; }

        [Column("Area")]
        public string Area { get; set; } = "";

        [Column("Telephone")]
        public string Telephone { get; set; } = "";

        [Column("Email")]
        [NullSetting(NullSetting = NullSettings.Null)]
        public string Email { get; set; } = "";

        [Column("Comment")]
        [NullSetting(NullSetting = NullSettings.Null)]
        public string Comment { get; set; } = "";

        [Column("Wash")]
        public bool Wash { get; set; }

        [Column("Approved")]
        public bool Approved { get; set; }

        [Column("Payment")]
        [NullSetting(NullSetting = NullSettings.Null)]
        public DateTime? Payment { get; set; }
    }
}