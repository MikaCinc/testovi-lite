using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Models
{
    [Table("IspitniRok")]
    public class IspitniRok
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [MaxLength(50)]
        public string Naziv { get; set; }

        [JsonIgnore]
        public List<Spoj> StudentiPredmeti { get; set; }
    }
}