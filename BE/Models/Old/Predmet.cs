using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Models
{
    [Table("Predmet")]
    public class Predmet
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [Range(1, 5)]
        public int Godina { get; set; }

        [Required]
        [MaxLength(50)]
        public string Naziv { get; set; }

        [JsonIgnore]
        public List<Spoj> PredmetStudent { get; set; }
    }
}