using System.Collections.Generic;

namespace RimeTwo
{
    public class OnChainMetaData
    {
        public string name { get; set; }
        public string image { get; set; }
        public List<OnChainFiles> files { get; set; }
        public string mediaType { get; set; }
        //public string Id { get; set; }
        //public string copyright { get; set; }
        //public string website { get; set; }
        //public string royalties { get; set; }
        //public string collection { get; set; }
        public Dictionary<string,string> attributes { get; set; }
        public Dictionary<string,string> inlineAttributes { get; set; }
    }
}