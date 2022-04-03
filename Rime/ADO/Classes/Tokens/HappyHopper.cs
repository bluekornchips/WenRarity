using System.ComponentModel.DataAnnotations;

namespace Rime.ADO.Classes.Tokens
{
    public class HappyHopper : Asset
    {
        [Key]
        public int Id { get; set; }
        public string Background { get; set; }
        public string Eyes { get; set; }
        public string Hands { get; set; }
        public string Hat { get; set; }
        public string Mouth { get; set; }
        public string Skin { get; set; }
        public string Wings { get; set; }
    }
}
