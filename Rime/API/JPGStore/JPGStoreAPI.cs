using BlockfrostQuery.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Rime.API.JPGStore
{
    public class JPGStoreAPI : API
    {
        //jpg(dot)store/api/policy/[id]/listings - listings for a given policy
        private static string _JPGStorePath = "https://jpg.store/api/policy/";
        public string ListingsForPolicyById(string policyId)
        {
            string a = "";
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    a = client.GetStringAsync($"{_JPGStorePath}{policyId}/listings").Result;
                }
                catch (Exception ex)
                {
                    Logger.Error("JPGStoreAPI", "ListingsForPolicyById", ex.Message);
                }
            }
            return a;
        }
    }
}
