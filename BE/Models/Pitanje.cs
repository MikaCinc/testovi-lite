using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [Def]
        public string Question { get; set; }

        [Required]
        [MaxLength(500)]
        public string Answer { get; set; }

        [Required]
        [DefaulValue(false)]
        public bool isArchived { get; set; }
        // public List<Spoj> StudentPredmet { get; set; }
    }
}