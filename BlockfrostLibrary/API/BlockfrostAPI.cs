using WenRarityLibrary;

namespace BlockfrostLibrary.API
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

        /// <summary>
        /// Retrieve a single assets metadata.
        /// </summary>
        /// <param name="asset"></param>
        /// <param name="json"></param>
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

        /// <summary>
        /// Collect assets for the policy, default return size is 100 items.
        /// </summary>
        /// <param name="policyId"></param>
        /// <param name="page"></param>
        /// <param name="json"></param>
        public void Assets_ByPolicy(string policyId, int page, out string json)
        {
            json = "";
            using HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("project_id", _queryToken);
            try
            {
                //json = client.GetStringAsync($"{_asset}/policy/{policyId}?page={page}&order=desc").Result;
                json = client.GetStringAsync($"{_asset}/policy/{policyId}?page={page}").Result;
            }
            catch (Exception ex)
            {
                _ducky.Error("API", "Assets_ByPolicy", ex.Message);
            }
        }
    }
}
