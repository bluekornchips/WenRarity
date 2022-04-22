using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WenRarityLibrary.ADO.Blockfrost.Models.OnChainMetaData
{
    public abstract class OnChainMetaData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Required]
        [MaxLength(150)]
        public string name { get; set; }

        [Required]
        [MaxLength(150)]
        public string image { get; set; }

        [Required]
        [MaxLength(150)]
        public string mediaType { get; set; }

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
        public int traitCount { get; set; }

        [NotMapped]
        public Dictionary<string, string> attributes { get; set; } = new Dictionary<string, string>();
    }

    public class DefaultOnChainMetaData : OnChainMetaData
    {
        public DefaultOnChainMetaData()
        {
            attributes = new();
        }
    }
}
