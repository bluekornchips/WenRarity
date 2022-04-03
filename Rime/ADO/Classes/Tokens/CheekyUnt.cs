using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rime.ADO.Classes.Tokens
{
    public class CheekyUnt : Asset
    {
        [Key]
        public int Id { get; set; }
        public string Unt { get; set; }
        public string Eyes { get; set; }
        public string Face { get; set; }
        public string Lefty { get; set; }
        public string Mouth { get; set; }
        public string Righty { get; set; }
        public string Accessory { get; set; }
        public string Hat { get; set; }
        public string Location { get; set; }
        public string Special { get; set; }
        public string Season { get; set; }
    }
}
