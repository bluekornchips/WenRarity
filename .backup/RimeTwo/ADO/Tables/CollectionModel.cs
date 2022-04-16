using System.ComponentModel.DataAnnotations;

namespace RimeTwo.ADO.Tables
{
    public class CollectionModel
    {
        [Key]
        [DataType(DataType.Text)]
        public string PolicyId { get; set; }
        [DataType(DataType.Text)]
        public string Name { get; set; }
        public CollectionDataModel collectionData { get; set; } = new();
    }

    public class CollectionDataModel
    {
        public double assets { get; set; } = 0;
        public double floor_overall { get; set; } = 0;
        public double ceiling_overall { get; set; } = 0;
        public double sale_highest { get; set; } = 0;
        public double sale_lowest { get; set; } = 0;
    }
}
