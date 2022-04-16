using RimeTwo.ADO.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RimeTwo.ViewModels.Collection
{
    internal class CollectionViewModel
    {
        public string PolicyId { get; set; }
        public string Name { get; set; }
        public Type AssetType { get; set; }

        public CollectionModel AsModel()
        {
            CollectionModel model = new CollectionModel();
            model.PolicyId = PolicyId;
            model.Name = Name;
            return model;
        }
    }
}
