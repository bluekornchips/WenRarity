
using WenRarityLibrary.ADO.Blockfrost.Models;

namespace WenRarityLibrary.Generic
{
    public class StatsContainer
    {
        public Collection collection { get; set; }
        public List<string> traitsIncluded { get; set; }
        public bool includeTraitCount { get; set; } = false;
    }
}
