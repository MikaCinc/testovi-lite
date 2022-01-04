using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Models
{
    [Table("SpojniceTagovi")]
    public class SpojniceTagovi
    {
        [Key]
        public int ID { get; set; }

        [JsonIgnore]
        public Spojnica Spojnica { get; set; }
        public Tag Tag { get; set; }
    }
}