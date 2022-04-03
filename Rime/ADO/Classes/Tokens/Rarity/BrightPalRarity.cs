using Rime.ADO.Classes.Tokens.Rarity;
using System.ComponentModel.DataAnnotations;

namespace Rime.ADO.Classes
{
    public class BrightPalRarity : Rarity
    {
        public double Background { get; set; }
        public double Body { get; set; }
        public double Ears { get; set; }
        public double Face { get; set; }
        public double Hair { get; set; }
        public double Head { get; set; }

        public BrightPalRarity()
        {

        }
    }
}
