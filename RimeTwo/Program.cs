using RimeTwo.Util;
using System;

namespace RimeTwo
{
    internal class Program
    {
        private static Ducky _ducky;
        static void Main(string[] args)
        {
            Setup();
            FrameworkBuilder builder = FrameworkBuilder.Instance;
            builder.Build();
        }

        static void Setup()
        {
            _ducky = Ducky.Instance;
        }
    }
}
