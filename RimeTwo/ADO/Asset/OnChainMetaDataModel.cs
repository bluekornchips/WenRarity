using System.ComponentModel.DataAnnotations;

namespace RimeTwo.ADO.Asset
{
    public class OnChainMetaDataModel
    {
        [Key]
        [DataType(DataType.Text)]
        public string name { get; set; }
        [DataType(DataType.Text)]
        public string image { get; set; }
        [DataType(DataType.Text)]
        public string mediaType { get; set; }
        [DataType(DataType.Text)]
        public string Collection { get; set; }
    }
}