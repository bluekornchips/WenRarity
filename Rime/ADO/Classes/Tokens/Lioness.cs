using System.ComponentModel.DataAnnotations;

namespace Rime.ADO.Classes
{
    public class Lioness : Asset
    {
        [Key]
        public int Id { get; set; }
        public string Background { get; set; }
        public string Clothes { get; set; }
        public string Expression { get; set; }
        public string Eyewear { get; set; }
        public string Fur { get; set; }
        public string Headwear { get; set; }
        public string Mouth { get; set; }
    }
}
