using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Table("Tag")]
    public class Tag
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        // public List<Spoj> StudentPredmet { get; set; }
    }
}