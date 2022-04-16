namespace Rime.ViewModels.Collection
{
    public class CollectionViewModel
    {
        public string PolicyId { get; set; }
        public string Name { get; set; }
        public string NamePlural { get; set; }
        public Type AssetType { get; set; }

        public ADO.Collection AsCollection()
        {
            ADO.Collection model = new ADO.Collection();
            model.PolicyId = PolicyId;
            model.Name = Name;
            model.NamePlural = NamePlural;
            return model;
        }
    }
}
