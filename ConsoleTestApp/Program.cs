using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestProject1;

namespace ConsoleTestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var unitTest = new UnitTest1();
            unitTest.TestSamplingHistogrammDataFactory1();
            Console.ReadLine();
        }
    }
}
