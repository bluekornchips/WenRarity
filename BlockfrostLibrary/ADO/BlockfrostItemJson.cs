using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlockfrostLibrary.ADO
{
    [Table("BlockfrostItemJson")]

    public class BlockfrostItemJson
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Required]
        public string json { get; set; }

        [Required]
        [MaxLength(150)]
        public string policy_id { get; set; }

        [Required]
        [MaxLength(150)]
        public string policy_name { get; set; }

        [Required]
        [MaxLength(150)]
        public string asset { get; set; }
    }
}