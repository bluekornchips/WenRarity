using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RimeLibrary.ADO.Rime.Models.Rarity
{
    public abstract class OnChainRarity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Required]
        [MaxLength(150)]
        public string asset { get; set; }

        [Required]
        [MaxLength(150)]
        public string fingerprint { get; set; }

        [Required]
        [MaxLength(150)]
        public string name { get; set; }
        [Required]
        public double traitCount { get; set; }

        [Required]
        public double weighting { get; set; }

        [NotMapped]
        public Dictionary<string, string> attributes { get; set; }
    }

    public class DefaultOnChainMetaData : OnChainRarity { }
}
