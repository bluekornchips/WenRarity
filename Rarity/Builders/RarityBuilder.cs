using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rarity.Builders
{
    internal class RarityBuilder
    {
        private static RarityBuilder _instance;
        public static RarityBuilder Instance => _instance ?? (_instance = new RarityBuilder());
    }
}
