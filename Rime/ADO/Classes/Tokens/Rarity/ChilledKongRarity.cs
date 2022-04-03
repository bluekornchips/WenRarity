using System.ComponentModel.DataAnnotations;

namespace Rime.ADO.Classes
{
    public class ChilledKongRarity {
        [Key]
        public int Id { get; set; }
        public string Fingerprint { get; set; }
        public double Background { get; set; }
        public double Body { get; set; }
        public double Clothes { get; set; }
        public double Earrings { get; set; }
        public double Eyes { get; set; }
        public double Hat { get; set; }
        public double Mouth { get; set; }
        public double Special { get; set; }
        public double TraitCount { get; set; }
        public double Weighting { get; set; }
    }
}
