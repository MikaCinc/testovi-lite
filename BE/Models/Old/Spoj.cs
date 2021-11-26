using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Models
{
    [Table("Spoj")]
    public class Spoj
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [Range(5, 10)]
        public int Ocena { get; set; }
        public IspitniRok IspitniRok { get; set; }

        [JsonIgnore]
        public Student Student { get; set; }
        public Predmet Predmet { get; set; }
    }
}