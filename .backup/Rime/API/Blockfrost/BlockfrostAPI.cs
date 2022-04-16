using BlockfrostQuery.Util;
using System;
using System.Net.Http;

namespace Rime.API.Blockfrost
{
    public class BlockfrostAPI : API
    {

        //public static readonly string _project_id = "mainnet8N69TWR2dOwpjRXFAaA7xfl7LnazepBQ"; // backup
        private static readonly string _asset = $"{_mainNet}/assets/";

        #region GET
        public static string Asset_One(string assetName)
        {
            string a = "";
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("project_id", _queryToken);
                try
                {
                    a = client.GetStringAsync($"{_asset}{assetName}").Result;
                }
                catch (Exception ex)
                {
                    Logger.Error("API", "Asset_One", ex.Message);
                }
            }
            return a;
        }

        /// <summary>
        /// Retrieves assets from the policy, in a paginated form.
        /// Default page size is 100.
        /// Data returned is in chronological order of mint date.
        /// </summary>
        /// <param name="policy"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public static string Assets_ByPolicy(string policy, int page)
        {
            string a;
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("project_id", _queryToken);
                try
                {
                    //a = client.GetStringAsync($"{_asset}/policy/{policy}?page={page}&order=desc").Result;
                    a = client.GetStringAsync($"{_asset}/policy/{policy}?page={page}").Result;
                }
                catch (Exception ex)
                {
                    Logger.Error("API", "Assets_ByPolicy", ex.Message);
                    return null;
                }
            }
            return a;
        }
        #endregion
    }
}
