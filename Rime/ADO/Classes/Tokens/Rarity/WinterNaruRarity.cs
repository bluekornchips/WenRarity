using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rime.ADO.Classes.Tokens.Rarity
{
    public class WinterNaruRarity : Rarity
    {
        public double Background { get; set; }
        public double Body { get; set; }
        public double Face { get; set; }
        public double Headwear { get; set; }

        public WinterNaruRarity()
        {

        }
    }
}
