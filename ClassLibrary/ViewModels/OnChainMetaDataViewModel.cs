﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WenRarityLibrary.ADO.Rime.Models.OnChainMetaData;
using WenRarityLibrary.ADO.Rime.Models.OnChainMetaData.Token;

namespace WenRarityLibrary.ViewModels
{
    public class OnChainMetaDataViewModel
    {
        #region Model Attributes
        public string name { get; set; }
        public string image { get; set; }
        public string mediaType { get; set; }
        public string policy_id { get; set; }
        public string asset { get; set; }
        #endregion
        //public Type type { get; set; } = typeof(DefaultOnChainMetaData);
        public Dictionary<string, string> attributes { get; set; } = new Dictionary<string, string>();
        public List<OnChainFilesViewModel> files { get; set; }

        public OnChainMetaData model { get; set; }

        public dynamic AsModel(Type type)
        {
            switch (type.Name)
            {
                //##_:
                case "KBot": return (KBot)model;
                default: return model;
            }
        }
    }
}