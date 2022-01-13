using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Table("Set")]
    public class Set
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        public List<SetTagovi> SetTagovi { get; set; }
        public List<SetSpojnice> SetSpojnice { get; set; }
        public List<SetPitanja> SetPitanja { get; set; }
    }
}