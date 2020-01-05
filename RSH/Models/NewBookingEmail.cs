using System;
using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.DatabaseAnnotations;

namespace RSH.Models
{
    [TableName("NewBookingEmail")]
    [PrimaryKey("Id", autoIncrement = true)]
    public class NewBookingEmail
    {
        [Column("Id")]
        [PrimaryKeyColumn(AutoIncrement = true)]
        public int Id { get; set; }

        [Column("Email")]
        public string Email { get; set; }
    }
}