
using BlockfrostLibrary.ADO.Models.Collection;

namespace RimeLibrary.Classes
{
    public class StatsContainer
    {
        public BlockfrostCollection collection { get; set; }
        public List<string> traitsIncluded { get; set; } = new List<string>();
        public bool includeTraitCount { get; set; } = false;
    }
}
