using System.ComponentModel.DataAnnotations;

namespace Rime.ADO.Classes
{
    public class ElMatador : Asset
    {
        [Key]
        public int Id { get; set; }
        public string Canvas { get; set; }
        public string Chaqueta { get; set; }
        public string Craneo { get; set; }
        public string Polvo { get; set; }
        public string Rociada { get; set; }
    }
}
