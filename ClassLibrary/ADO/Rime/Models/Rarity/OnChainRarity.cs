﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WenRarityLibrary.ADO.Rime.Models.Rarity
{
    public abstract class OnChainRarity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Required]
        [MaxLength(150)]
        public string policy_id { get; set; }

        [Required]
        [MaxLength(150)]
        public string asset { get; set; }
        [NotMapped]
        public Dictionary<string, string> attributes { get; set; }
    }

    public class DefaultOnChainMetaData : OnChainRarity { }
}