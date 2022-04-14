using RimeTwo.API;
using System;
using System.Collections.Generic;

namespace RimeTwo
{
    internal class FrameworkBuilder
    {
        private static FrameworkBuilder _instance;
        public static FrameworkBuilder Instance => _instance ?? (_instance = new FrameworkBuilder());
        private RimeWriter _rimeWriter = RimeWriter.Instance;
        private static BlockfrostAPI blockfrostAPI = BlockfrostAPI.Instance;
        private FrameworkBuilder() { }
        public void Build()
        {
            //Console.WriteLine("Enter Policy ID: ");
            //string policyId = Console.ReadLine();
            string policyId = "3f00d83452b4ead45cf5e0ca811fe8da561dfc45a5e414c88c4d8759";

            //Console.WriteLine("Enter Collection Name: ");
            //string collectionName = Console.ReadLine();
            string collectionName = "KBot";
            CreateNew(policyId, collectionName);
            //BuildAttributes(out List<string[]> attributes);
            //_rimeWriter.Build(collectionName[0].ToString().ToUpper() + collectionName.Substring(1), attributes);
        }

        private void CreateNew(string policyId, string className)
        {
            int page = 0;
            // Check the policy
            blockfrostAPI.Assets_ByPolicy(policyId, page, out List<BlockfrostPolicyItem> items);
            if (items != null)
            {
                // Use the first item in sequence that has a quantity greater than 0.
                int index = 0;
                while (items[index].Quantity == 0) ++index;
                BlockfrostPolicyItem item = items[index];
                blockfrostAPI.Asset_One(item.Asset, out Asset asset);
                _rimeWriter.Build(className, asset);
            }
        }

        //private void BuildAttributes(out List<string[]> attributes)
        //{
        //    int inputCount = 0;
        //    attributes = new();
        //    Console.WriteLine("Format: [TYPE] [NAME]");
        //    do
        //    {
        //        Console.WriteLine("Enter Type and Attribute Name:");
        //        string[] attribute = Console.ReadLine().Split(new char[] { });
        //        inputCount = attribute.Length;
        //        if (inputCount != 1) attributes.Add(attribute);
        //    } while (inputCount != 1);
        //}
    }
}