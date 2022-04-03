using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rime.ADO.Classes.Tokens
{
    public class WinterNaru : Asset
    {
        [Key]
        public int Id { get; set; }
        public string Background { get; set; }
        public string Body { get; set; }
        public string Face { get; set; }
        public string Headwear { get; set; }
    }
}
