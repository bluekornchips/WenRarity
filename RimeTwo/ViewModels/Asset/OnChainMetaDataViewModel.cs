using RimeTwo.ADO.Asset;
using RimeTwo.ViewModels.Asset;
using System;
using System.Collections.Generic;

namespace RimeTwo.ViewModels
{
    public class OnChainMetaDataViewModel
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public string MediaType { get; set; }
        public Dictionary<string, string> attributes { get; set; } = new();
        public List<OnChainFilesViewModel> files { get; set; }

        public OnChainMetaDataModel AsModel()
        {
            OnChainMetaDataModel onChainMetaData = new OnChainMetaDataModel();
            onChainMetaData.name = Name;
            onChainMetaData.image = Image;
            onChainMetaData.mediaType = MediaType;
            return onChainMetaData;
        }
    }
}
