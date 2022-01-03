using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization; 

namespace Models
{
    [Table("Pitanje")]
    public class Pitanje
    {
        [Key]
        public int ID { get; set; }

        /* [Required]
        [Range(10000, 20000)]
        public int question { get; set; } */

        [Required]
        [MaxLength(500)]
        //[DefaultValue("")]
        public string Question { get; set; }

        [Required]
        [MaxLength(500)]
        public string Answer { get; set; }

        [Required]
        // [DefaultValue(false)]
        public bool isArchived { get; set; }
        // public List<Spoj> StudentPredmet { get; set; }
    }
}