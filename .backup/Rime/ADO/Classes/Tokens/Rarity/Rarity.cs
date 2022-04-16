using System.ComponentModel.DataAnnotations;

namespace Rime.ADO.Classes.Tokens.Rarity
{
    public class Rarity
    {
        [Key]
        public int Id { get; set; }
        public string Fingerprint { get; set; }
        public double TraitCount { get; set; }
        public double Weighting { get; set; }
    }
}
