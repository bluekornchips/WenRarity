using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rime.ADO
{
    public partial class Collection
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string PolicyId { get; set; }
        public string Name { get; set; }
        public string NamePlural { get; set; }
    }
}
