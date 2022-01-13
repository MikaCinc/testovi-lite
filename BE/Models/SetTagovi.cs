using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Models
{
    [Table("SetTagovi")]
    public class SetTagovi
    {
        [Key]
        public int ID { get; set; }

        [JsonIgnore]
        public Set Set { get; set; }
        public Tag Tag { get; set; }
    }
}