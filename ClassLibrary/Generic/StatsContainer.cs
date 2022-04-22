
using WenRarityLibrary.ADO.Blockfrost.Models;

namespace WenRarityLibrary.Generic
{
    public class StatsContainer
    {
        public Collection collection { get; set; }
        public List<string> traitsIncluded { get; set; } = new List<string>();
        public bool includeTraitCount { get; set; } = false;
    }
}
