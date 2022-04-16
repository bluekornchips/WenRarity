using System.ComponentModel.DataAnnotations;

namespace Rime.ADO.Classes
{
    public class BrightPal : Asset
    {
        [Key]
        public int Id { get; set; }
        public string Background { get; set; }
        public string Body { get; set; }
        public string Ears { get; set; }
        public string Face { get; set; }
        public string Hair { get; set; }
        public string Head { get; set; }
    }
}
