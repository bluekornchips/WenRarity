using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blockfrost
{
    internal class FrameworkBuilder
    {
        private static FrameworkBuilder instance;
        public static FrameworkBuilder Instance => instance ?? (instance = new FrameworkBuilder());

        public void Build()
        {
            Console.WriteLine("Enter Policy ID: ");
            string policyId = Console.ReadLine();

            Console.WriteLine("Enter Collection Name: ");
            string collectionName = Console.ReadLine();
        }
    }
}
