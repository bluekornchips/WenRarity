using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rime.ADO
{
    public partial class OnChainMetaData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string name { get; set; }
        public string image { get; set; }
        public string mediaType { get; set; }
        public string policy_id { get; set; }
        public string asset { get; set; }
    }
}
