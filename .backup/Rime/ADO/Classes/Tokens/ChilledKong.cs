using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rime.ADO.Classes
{
    public class ChilledKong : Asset
    {
        [Key]
        public int Id { get; set; }
        public string Background { get; set; }
        public string Body { get; set; }
        public string Clothes { get; set; }
        public string Earrings { get; set; }
        public string Eyes { get; set; }
        public string Hat { get; set; }
        public string Mouth { get; set; }
        public string Special { get; set; }
    }
}
