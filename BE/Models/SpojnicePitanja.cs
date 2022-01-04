using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Models
{
    [Table("SpojnicePitanja")]
    public class SpojnicePitanja
    {
        [Key]
        public int ID { get; set; }

        [JsonIgnore]
        public Spojnica Spojnica { get; set; }
        public Pitanje Pitanje { get; set; }
    }
}