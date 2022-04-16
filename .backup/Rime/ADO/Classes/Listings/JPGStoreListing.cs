using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rime.ADO.Classes.Listings
{
    public class JPGStoreListing
    {
        [Key]
        public int Id { get; set; }
        public string asset { get; set; } = string.Empty;
        public string asset_display_name { get; set; } = string.Empty;
        public string price_lovelace { get; set; } = string.Empty;
        public string listed_at { get; set; } = string.Empty;
        public JPGStoreListing(string asset, string asset_display_name, string price_lovelace, string listed_at)
        {
            this.asset = asset;
            this.asset_display_name = asset_display_name;
            this.price_lovelace = price_lovelace;
            this.listed_at = listed_at;
        }

        public JPGStoreListing()
        {

        }
    }
}
