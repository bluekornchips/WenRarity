using System.ComponentModel.DataAnnotations;

namespace Rime.ADO.Classes
{
    public class Pendulum : Asset
    {
        [Key]
        public int Id { get; set; }
        public string Background { get; set; }
        public string Body { get; set; }
        public string Ear { get; set; }
        public string Eye { get; set; }
        public string Eyes { get; set; }
        public string Head { get; set; }
        public string Mouth { get; set; }
        public string Skin { get; set; }
    }
}
