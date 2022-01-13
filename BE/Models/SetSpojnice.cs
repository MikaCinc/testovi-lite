using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Models
{
    [Table("SetSpojnice")]
    public class SetSpojnice
    {
        [Key]
        public int ID { get; set; }

        [JsonIgnore]
        public Set Set { get; set; }
        public Spojnica Spojnica { get; set; }
    }
}