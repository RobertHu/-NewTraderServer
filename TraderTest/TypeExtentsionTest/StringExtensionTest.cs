using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Trader.Server.TypeExtension;
using System.Diagnostics;

namespace TraderTest.TypeExtentsionTest
{
    [TestFixture]
    public class StringExtensionTest
    {
        [Test]
        public void ToJoinStringTest()
        {
            string[] source = {"one","two","three","four" };
            string result = source.ToJoinString();
            Assert.IsNotNullOrEmpty(result);
            Debug.WriteLine(result);
        }

        [Test]
        public void ToGuidArrayTest()
        {
            string[] source = { Guid.NewGuid().ToString(),Guid.NewGuid().ToString(),Guid.NewGuid().ToString(),Guid.NewGuid().ToString()};
            string oneStr = source.ToJoinString();
            Guid[] result = oneStr.ToGuidArray();
            Assert.NotNull(result);
            Assert.AreEqual(4, result.Length);
            foreach (Guid item in result)
            {
                Debug.WriteLine(item);
            }
        }

        [Test]
        public void To2DArray()
        {
            string source = "a,b,c;1,2,3;x,y,z;aa,bb,cc,dd";
            string[][] result = source.To2DArray();
            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.Length);
            for (int i = 0; i < result.Length; i++)
            {
                for (int j = 0; j < result[i].Length; j++)
                {
                    Debug.Write(result[i][j] + "  ");
                }
                Debug.WriteLine("");
            }
        }
    }
}
