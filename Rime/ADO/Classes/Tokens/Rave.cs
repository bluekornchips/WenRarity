using System.ComponentModel.DataAnnotations;

namespace Rime.ADO.Classes
{
    public class Rave : Asset
    {
        [Key]
        public int Id { get; set; }
        public string Background { get; set; }
        public string Wing { get; set; }
        public string Body { get; set; }
        public string Beak { get; set; }
        public string Eye { get; set; }
    }
}
