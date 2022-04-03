using Rime.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rime.ViewModels
{
    public class ListingViewModel
    {
        public IListing store { get; set; }
        public TokenViewModel tokenViewModel { get; set; }
    }
}
