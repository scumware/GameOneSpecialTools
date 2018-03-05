using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var bitArray = new LoadTester.BitArray();
            bitArray.SetCount(sizeof(UInt32)*8);
            Assert.AreEqual(bitArray.Value, ulong.MaxValue);

            for (int i = 0; i < bitArray.Count; i++)
                bitArray[i] = false;

            var value = bitArray.Value & (
                (~((UInt64) UInt32.MaxValue))
                << (8*sizeof (UInt32))
                );
            Assert.AreEqual(value, 0);
        }
    }
}
