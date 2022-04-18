using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WenRarityLibrary.ADO;

namespace WenRarityLibrary.API
{
    public class BlockfrostAPI
    {
        private static BlockfrostAPI instance;
        public static BlockfrostAPI Instance => instance ?? (instance = new BlockfrostAPI());
        private BlockfrostAPI() { }

        private static Ducky _ducky = Ducky.Instance;

        private static readonly string _mainNet = "https://cardano-mainnet.blockfrost.io/api/v0/";
        private static readonly string _queryToken = "mainnetqW1HNm5UljEVmlVm5Rr7hdseDMaMZccB";
        private static readonly string _asset = $"{_mainNet}/assets/";

        public void Asset(string asset, out string json)
        {
            json = "";
            using HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("project_id", _queryToken);
            try
            {
                json = client.GetStringAsync($"{_asset}{asset}").Result;
            }
            catch (Exception ex)
            {
                _ducky.Error("BlockfrostAPI", "Asset", ex.Message);
            }
        }

        public void Assets_ByPolicy(string policyId, int page, out string json)
        {
            json = "";
            using HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("project_id", _queryToken);
            try
            {
                //a = client.GetStringAsync($"{_asset}/policy/{policy}?page={page}&order=desc").Result;
                json = client.GetStringAsync($"{_asset}/policy/{policyId}?page={page}").Result;
            }
            catch (Exception ex)
            {
                _ducky.Error("API", "Assets_ByPolicy", ex.Message);
            }
        }
    }
}
