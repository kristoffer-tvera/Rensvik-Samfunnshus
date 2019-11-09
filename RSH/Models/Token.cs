using System;
using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.DatabaseAnnotations;

namespace RSH.Models
{
    [TableName("Token")]
    [PrimaryKey("Id", autoIncrement = true)]
    [ExplicitColumns]
    public class Token
    {
        [Column("Id")]
        [PrimaryKeyColumn(AutoIncrement = true)]
        public int Id { get; set; }

        [Column("Key")]
        public string Key { get; set; }

        [Column("Expiration")]
        public DateTime Expiration { get; set; }

        [Column("BookingId")]
        public int BookingId { get; set; }
    }
}