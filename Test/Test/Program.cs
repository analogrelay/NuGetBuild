using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Adder;

namespace BetterNuGetBuild
{
    class Program
    {
        static void Main(string[] args)
        {
            var farter = new SuperFarter.SuperFarter();
            farter.Fart(Class1.Add(2, 3));
        }
    }
}
