using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rime.Store
{
    public interface IListing
    {
        public void GetListings(string policy);
    }
}
