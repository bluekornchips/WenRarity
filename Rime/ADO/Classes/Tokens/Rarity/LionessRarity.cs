using System.ComponentModel.DataAnnotations;

namespace Rime.ADO
{
    public class LionessRarity
    {
        [Key]
        public int Id { get; set; }
        public string Fingerprint { get; set; }
        public double Background { get; set; }
        public double Clothes { get; set; }
        public double Expression { get; set; }
        public double Eyewear { get; set; }
        public double Fur { get; set; }
        public double Headwear { get; set; }
        public double Mouth { get; set; }
        public double TraitCount { get; set; }
        public double Weighting { get; set; }

        public LionessRarity()
        {

        }
    }
}