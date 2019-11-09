using Newtonsoft.Json;
using System;
using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.DatabaseAnnotations;

namespace RSH.Models
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

        [Column("Name")]
        [NullSetting(NullSetting = NullSettings.Null)]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; } = "";

        [Column("Comment")]
        [NullSetting(NullSetting = NullSettings.Null)]
        [SpecialDbType(SpecialDbTypes.NTEXT)]
        public string Comment { get; set; } = "";

        [Column("Reserved")]
        public bool Reserved { get; set; }
        
        [Column("Confirmed")]
        public bool Confirmed { get; set; }
    }
}