using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rime.ADO
{
    public partial class BlockfrostAsset
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string policy_id { get; set; }
        public string asset { get; set; }
        public string fingerprint { get; set; }
        public int quantity { get; set; }
        public string initial_mint_tx_hash { get; set; }
        public int mint_or_burn_count { get; set; }
        public string metadata { get; set; }
    }
}
