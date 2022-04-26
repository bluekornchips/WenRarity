using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlockfrostLibrary.ADO.Models.Collection
{
    public class BlockfrostCollection
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string PolicyId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        public string RealName { get; set; }
        [Required]
        [StringLength(100)]
        public string DatabaseName { get; set; }
    }
}
