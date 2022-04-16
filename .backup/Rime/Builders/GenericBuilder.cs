using BlockfrostQuery.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rime.ADO;
using Rime.ADO.Classes;
using Rime.API.Blockfrost;
using Rime.Controller;
using Rime.Util;
using Rime.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rime.Builders
{
    public class GenericBuilder
    {
        public string PolicyId { get; set; }
        public string PolicyName { get; set; }
        public string OutputDir { get; set; }
        public string[] Attributes { get; set; }
        public static Dictionary<string, TokenViewModel> Tokens { get; set; }
        public static List<JToken> jtokens { get; set; }
        public static Func<JToken, Asset> Cleaner { get; set; }


        protected GenericBuilder(string policyName, string policyId)
        {
            PolicyId = policyId;
            PolicyName = policyName;
            Tokens = new Dictionary<string, TokenViewModel>();
            jtokens = new List<JToken>();
            OutputDir = Setup.DataDir + PolicyName;
        }

        public void Build()
        {
            RetrieveAllTokensFrom_DB();
            RetriveAllFrom_Blockfrost();
        }

        public void DeleteTableData()
        {
            Logger.Info($"Clearing Table Data for {PolicyName}.");
            RimeDBContextController.ClearTokens(PolicyName, PolicyId);
        }

        private void RetriveAllFrom_Blockfrost()
        {
            Logger.Info($"Retrieving {PolicyName} assets from Blockfrost.");
            bool getPolicy = true;
            int scans = 1, maxScans = 100;
            int page = 0;
            int matchCount = 0;

            bool build = true;
            if (build)
            {
                do
                {
                    string policyRequestResults = BlockfrostAPI.Assets_ByPolicy(PolicyId, ++page);
                    if (policyRequestResults == "[]")
                        getPolicy = false;

                    if (getPolicy)
                    {
                        JToken deserialized = (JToken)JsonConvert.DeserializeObject(policyRequestResults);

                        var jsonAssets = deserialized.Children().Values("asset");
                        matchCount = 0;
                        TokenViewModel builtTokenViewModel = null;
                        foreach (var asset in jsonAssets)
                        {
                            if (!Tokens.ContainsKey(asset.ToString()))
                            {
                                builtTokenViewModel = BuildOneAsync(asset.ToString());
                                if(builtTokenViewModel != null && builtTokenViewModel.Token.AssetName != null)
                                {
                                    if(Tokens.TryAdd(builtTokenViewModel.Token.AssetName, builtTokenViewModel))
                                    {
                                        // Add to Database
                                        if (RimeDBContextController.Add(builtTokenViewModel)) Logger.Debug("GenericBuilder", "RetriveAllFrom_Blockfrost", $"Added {builtTokenViewModel.Asset.Name}");
                                        else Logger.Debug("GenericBuilder", "RetriveAllFrom_Blockfrost", $"Unable to add asset: {builtTokenViewModel.Asset.Name}");
                                    }
                                    else
                                    {
                                        Logger.Debug("GenericBuilder", "RetriveAllFrom_Blockfrost", $"Duplicate asset exists for: {builtTokenViewModel.Asset.Name}");
                                    }
                                }
                            }
                            else
                            {
                                ++matchCount;
                                //Logger.Info($"{asset} already exists.");
                            }
                        }
                        if (matchCount == 100)
                        {
                            //getPolicy = false;
                        }
                        ++scans;
                    }
                } while (getPolicy && scans < maxScans);
            }
            Logger.Info($"Completed build for {PolicyName}.");
        }

        private TokenViewModel BuildOneAsync(string name)
        {
            string built = BlockfrostAPI.Asset_One(name);
            JToken serialized = (JToken)JsonConvert.DeserializeObject(built);
            jtokens.Add(serialized);
            //OutputUtil.WriteJson(OutputDir, jtokens);
            return BuildOneFromJToken(serialized);
        }

        private TokenViewModel BuildOneFromJToken(JToken jToken)
        {
            TokenViewModel tokenViewModel = new TokenViewModel();

            try
            {
                // OnChainMetaData
                tokenViewModel.Token._Asset = (string)(jToken["asset"] ?? jToken["asset"].ToString());
                tokenViewModel.Token.PolicyID = (string)(jToken["policy_id"] ?? jToken["policy_id"].ToString());
                tokenViewModel.Token.AssetName = (string)(jToken["asset_name"] ?? jToken["asset_name"].ToString());
                tokenViewModel.Token.Fingerprint = (string)(jToken["fingerprint"] ?? jToken["fingerprint"].ToString());
                tokenViewModel.Token.Quantity = (int)(jToken["quantity"] ?? int.Parse(jToken["quantity"].ToString()));
                tokenViewModel.Token.InitialMintTxHash = (string)(jToken["initial_mint_tx_hash"] ?? jToken["initial_mint_tx_hash"].ToString());
                tokenViewModel.Token.MintOrBurnCount = (int)(jToken["mint_or_burn_count"] ?? int.Parse(jToken["mint_or_burn_count"].ToString()));

                // MetaDataFiles
                JToken onChain = jToken["onchain_metadata"];

                if (tokenViewModel.Token.MintOrBurnCount == 1)
                {
                    if (CleanJToken(onChain, tokenViewModel.Token.Fingerprint, out Asset builtToken))
                    {
                        tokenViewModel.Token.PolicyName = PolicyName;
                        tokenViewModel.Asset = builtToken;
                    }
                    else
                    {
                        Logger.Debug("GenericBuilder", "BuildOneFromJToken", "Unable to create token from Blockfrost Request.");
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, "GenericBuilder", "BuildOneFromJToken");
            }
            return tokenViewModel;
        }

        private bool CleanJToken(JToken jToken, string fingerprint, out Asset builtToken)
        {
            try
            {
                builtToken = Cleaner(jToken);
                builtToken.Fingerprint = fingerprint;
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void RetrieveAllTokensFrom_DB()
        {
            Logger.Info($"Retrieving {PolicyName} tokens from database.");
            Tokens = new Dictionary<string, TokenViewModel>();

            using(var db = new RimeContext())
            {
                var query = from t in db.Tokens.Where(token => token.PolicyID == PolicyId)  select t as Token;
                foreach (var item in query)
                {
                    Tokens.Add(item._Asset, new TokenViewModel(item));
                }
            }
        }


        public void OutputWithWeights()
        {
            StringBuilder sb = new StringBuilder();
            string tableName = PolicyName;
            string headers = "Rank,Name,";
            if (PolicyName.ElementAt(PolicyName.Length - 1) != 's') tableName = PolicyName + "s";

            sb.Append("SELECT [TokenTable].[Name],");

            foreach (string field in Attributes)
            {
                sb.Append($"[TokenTable].[{field}],[TokenTableRarity].[{field}],");
                headers += $"{field},{field}Weight,";
            }
            headers += "TraitCount,Weighting";

            sb.Append("[TokenTable].[TraitCount], " +
                " [TokenTableRarity].[Weighting]" +
                $" FROM [{tableName}] [TokenTable]" +
                $" INNER JOIN [{PolicyName}Rarities] [TokenTableRarity] ON" +
                $" [TokenTable].[Fingerprint] = [TokenTableRarity].[Fingerprint]" +
                $" ORDER BY [TokenTableRarity].[Weighting] DESC");
            RimeDBContextController.SelectWithRarity(sb.ToString(), out List<string[]> rowsReturned);
            List<string> results = new List<string>() { headers };
            string result = "";
            int rank = 0;
            foreach (string[] row in rowsReturned)
            {
                result = $"{++rank},";
                for (int i = 0; i < row.Length - 1; i++)
                    result += row[i] + ",";
                result += row[row.Length - 1];
                results.Add(result);
            }
            OutputUtil.Write(OutputDir + "Weightings", results);
        }

        #region old
        public void OutputRarityToCSV()
        {
            StringBuilder sb = new StringBuilder();
            string tableName = PolicyName;
            string headers = "Rank,Name,";
            if(PolicyName.ElementAt(PolicyName.Length - 1) != 's') tableName = PolicyName + "s";

            sb.Append("SELECT [TokenTable].[Name],");

            foreach (string field in Attributes)
            {
                sb.Append($"[TokenTable].[{ field}], ");
                headers += field + ",";
            }
            headers += "TraitCount,Weighting";
            
            sb.Append("[TokenTable].[TraitCount], " +
                " [TokenTableRarity].[Weighting]" +
                $" FROM [{tableName}] [TokenTable]" +
                $" INNER JOIN [{PolicyName}Rarities] [TokenTableRarity] ON" +
                $" [TokenTable].[Fingerprint] = [TokenTableRarity].[Fingerprint]" +
                $" ORDER BY [TokenTableRarity].[Weighting] DESC");
            RimeDBContextController.SelectWithRarity(sb.ToString(), out List<string[]> rowsReturned);
            List<string> results = new List<string>() { headers};
            string result = "";
            int rank = 0;
            foreach (string[] row in rowsReturned)
            {
                result = $"{++rank},";
                for(int i = 0; i < row.Length - 1; i++)
                    result += row[i] + ",";
                result += row[row.Length - 1];
                results.Add(result);
            }
            OutputUtil.Write(OutputDir, results);
        }
        #endregion
    }
}
