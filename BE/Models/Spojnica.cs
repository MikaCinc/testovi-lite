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
        // [DefaultValue("Nova spojnica")]
        public string Title { get; set; }

        [Required]
        // [DefaultValue(false)]
        public bool Archived { get; set; }

        [Required]
        // [DefaultValue(false)]
        public bool Highlighted { get; set; }

        [Required]
        [Range(1, 3)]
        // [DefaultValue(2)]
        public int Priority { get; set; }

        [Required]
        // [Range(1, 3)]
        // [DefaultValue(0)]
        public int NumberOfGames { get; set; }
        public System.DateTime DateCreated { get; set; }

        public List<SpojniceTagovi> Tagovi { get; set; }
        public List<SpojnicePitanja> Pitanja { get; set; }
    }
}