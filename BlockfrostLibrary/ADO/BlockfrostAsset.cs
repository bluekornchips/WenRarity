using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlockfrostLibrary.ADO
{
    [Table("BlockfrostAsset")]
    public class BlockfrostAsset
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string policy_id { get; set; }

        [Required]
        [MaxLength(150)]
        public string asset { get; set; }
        
        [Required]
        [MaxLength(150)]
        public string fingerprint { get; set; }
        
        [Required]
        public int quantity { get; set; }

        [Required]
        [MaxLength(150)]
        public string initial_mint_tx_hash { get; set; }

        [Required]
        public int mint_or_burn_count { get; set; }

        [Required]
        [MaxLength(150)]
        public string metadata { get; set; }
    }
}