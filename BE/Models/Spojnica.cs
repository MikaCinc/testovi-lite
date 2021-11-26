using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Table("Spojnica")]
    public class Spojnica
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [MaxLength(250)]
        [DefaulValue("Nova spojnica")]
        public string Title { get; set; }

        [Required]
        [DefaulValue(false)]
        public bool Archived { get; set; }
        
        [Required]
        [DefaulValue(false)]
        public bool Highlighted { get; set; }

        [Required]
        [Range(1, 3)]
        [DefaulValue(2)]
        public int Priority { get; set; }

        [Required]
        // [Range(1, 3)]
        [DefaulValue(0)]
        public int NumberOfGames { get; set; }

        // public List<Spoj> StudentPredmet { get; set; } TAGOVI
        // public List<Spoj> StudentPredmet { get; set; } PITANJA
    }
}