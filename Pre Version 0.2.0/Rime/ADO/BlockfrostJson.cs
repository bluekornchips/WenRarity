using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rime.ADO
{
    public class BlockfrostJson
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string json { get; set; }
        public string policy_id { get; set; }
        public string policy_name { get; set; }
        public string asset { get; set; }
    }
}
