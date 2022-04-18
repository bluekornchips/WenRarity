using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WenRarityLibrary.ADO.Rime.Models.OnChainMetaData
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
        [NotMapped]
        public Dictionary<string, string> attributes { get; set; }
    }

    public class DefaultOnChainMetaData : OnChainMetaData { }
}
