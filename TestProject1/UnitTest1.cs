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

            ulong value = bitArray.Value & UInt32.MaxValue;
            Assert.AreEqual(value, (ulong)0);

            bitArray[0] = true;
            value = bitArray.Value & UInt32.MaxValue;
            Assert.AreEqual(value, (ulong)1);

            bitArray[0] = false;
            value = bitArray.Value & UInt32.MaxValue;
            Assert.AreEqual(value, (ulong)0);


            bitArray[1] = true;
            value = bitArray.Value & UInt32.MaxValue;
            Assert.AreEqual(value, (ulong)2);

            bitArray[1] = false;
        }
    }
}
