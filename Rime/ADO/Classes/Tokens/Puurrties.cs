using System.ComponentModel.DataAnnotations;

namespace Rime.ADO.Classes
{
    public class Puurrties : Asset
    {
        [Key]
        public int Id { get; set; }
        public string Background { get; set; }
        public string Eyes { get; set; }
        public string Fur { get; set; }
        public string Hands { get; set; }
        public string Hat { get; set; }
        public string Mask { get; set; }
        public string Mouth { get; set; }
        public string Outfit { get; set; }
        public string Tail { get; set; }
        public string Wings { get; set; }
    }
}
