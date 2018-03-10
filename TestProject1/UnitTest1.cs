using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using LoadTester;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestBitArray()
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

        [TestMethod]
        public void TestSamplingHistogrammDataFactory1()
        {
            SampleHistogrammDataFactory.ChartSize = 5;
            var uniques = SampleHistogrammDataFactory.GetSortedUniques(new double[] {1, 2, 3, 4, 5, 1});
            Assert.AreEqual(5, uniques.Length);

            int index = 0;

            Assert.AreEqual(1, uniques[index].Value);
            Assert.AreEqual(2, uniques[index].Count);

            ++index;
            Assert.AreEqual(2, uniques[index].Value);
            Assert.AreEqual(1, uniques[index].Count);

            ++index;
            Assert.AreEqual(3, uniques[index].Value);
            Assert.AreEqual(1, uniques[index].Count);

            ++index;
            Assert.AreEqual(4, uniques[index].Value);
            Assert.AreEqual(1, uniques[index].Count);

            ++index;
            Assert.AreEqual(5, uniques[index].Value);
            Assert.AreEqual(1, uniques[index].Count);
        }

        [TestMethod]
        public void TestSamplingHistogrammDataFactory2()
        {
            SampleHistogrammDataFactory.ChartSize = 5;
            var uniques = SampleHistogrammDataFactory.GetSortedUniques(new double[] { 1, 2, 3, 4, 5, 1 });
            var resultedValues = SampleHistogrammDataFactory.GetResultedValues(uniques);
            Assert.IsTrue(Enumerable.SequenceEqual(uniques, resultedValues));
        }


        [TestMethod]
        public void TestSamplingHistogrammDataFactory3()
        {
            SampleHistogrammDataFactory.ChartSize = 3;
            var uniques = SampleHistogrammDataFactory.GetSortedUniques(new double[] { 1, 2, 3, 4, 5, 1 });
            var resultedValues = SampleHistogrammDataFactory.GetResultedValues(uniques);

            Assert.AreEqual(3, resultedValues.Length);

            int index = 0;

            Assert.AreEqual(1, resultedValues[index].Value);
            Assert.AreEqual(2, resultedValues[index].Count);

            ++index;
            Assert.AreEqual(2, resultedValues[index].Value);
            Assert.AreEqual(1, resultedValues[index].Count);

            ++index;
            Assert.AreEqual(3, resultedValues[index].Value);
            Assert.AreEqual(1, resultedValues[index].Count);
        }
    }
}
