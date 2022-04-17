using Rarity.Controller;
using Rime.ADO;
using Rime.Builders;
using Rime.ViewModels.Asset;
using Rime.ViewModels.Collection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WenRarityLibrary;

namespace Rarity.Builders
{
    internal class RarityBuilder
    {
        private static RarityBuilder _instance;
        public static RarityBuilder Instance => _instance ?? (_instance = new RarityBuilder());
        private static Ducky _ducky = Ducky.Instance;
        private static RimeController _rimeController = RimeController.Instance;
        private List<OnChainMetaData> _tokens = new();
        //private CollectionViewModel _collection;

        private RarityBuilder() { }

        public void Build()
        {
            string policyId = "3f00d83452b4ead45cf5e0ca811fe8da561dfc45a5e414c88c4d8759";
            _rimeController.Get_CollectionById(policyId, out Collection collection);

            //_collection = new CollectionViewModel()
            //{
            //    PolicyId = collection.PolicyId,
            //    Name = collection.Name,
            //    NamePlural = collection.NamePlural
            //};

            if (collection == null)
            {
                _ducky.Error("RarityBuilder", "Build", $"No collection found for {policyId}.");
                return;
            }
            else
            {
                _rimeController.Get_TokensFromCollection(collection, out _tokens);
                Generate(_tokens);
            }
        }


        private void Generate(List<OnChainMetaData> tokens)
        {
            var collectionType = tokens.FirstOrDefault().GetType();
            if (collectionType != null)
            {
                switch (collectionType.Name)
                {
                    case "GrandmasterAdventurer": Handle_GMA(tokens); break;
                    case "KBot": Handle_KBot(tokens); break;
                    default:
                        break;
                }
            }
            //AttributeCounts();
        }

        private void Handle_GMA(List<OnChainMetaData> tokens)
        {
            Dictionary<string, GrandmasterAdventurer> gmas = new();
            foreach (var item in tokens) gmas.Add(item.asset, (GrandmasterAdventurer)item);


        }

        private void Handle_KBot(List<OnChainMetaData> tokens)
        {
            Dictionary<string, KBot> kbots = new();
            Dictionary<string, int> pets = new();
            foreach (var item in tokens)
            {
                KBot bot = (KBot)item;
                kbots.Add(item.asset, bot);
                if (pets.ContainsKey(bot.Pet)) ++pets[bot.Pet]; else pets.Add(bot.Pet, 1);
            }

            //foreach (var item in kbots)
            //{
            //    KBotRarity botRarity = new KBotRarity()
            //    {
            //        asset = item.asset,
            //        Pet = item.
            //    };
            //}
        }

        //private void AttributeCounts()
        //{
        //    foreach (var token in _tokens)
        //    {
        //        if(LocalJson(token.asset, out AssetViewModel avm))
        //        {
        //            Console.WriteLine();
        //        }
        //    }
        //}

        //private bool LocalJson(string asset, out AssetViewModel avm)
        //{
        //    avm = new();
        //    _rimeController.Get_BlockfrostJson(asset, out string json);
        //    if (json != "")
        //    {
        //        AssetBuilder ab = AssetBuilder.Instance;
        //        ab.Parse(_collection, json, out avm);
        //        //_ducky.Info($"Using local Json for {avm.onchain_metadata.name}.");
        //        return true;
        //    }
        //    return false;
        //}

        //private void AttributeCounts(Dictionary<string, int> attributes, OnChainMetaData ocmd)
        //{

        //}
    }
}
