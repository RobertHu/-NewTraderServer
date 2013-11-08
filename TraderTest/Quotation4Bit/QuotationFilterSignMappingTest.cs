using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Trader.Server._4BitCompress;
namespace TraderTest.Quotation4Bit
{
    [TestFixture]
    public class QuotationFilterSignMappingTest
    {
        [Test]
        public void TestAddAndRemove()
        {
            string key1 = "aaaa";
            string key2 = "bbbb";
            string key3 = "cccc";
            string key4 = "aaaa";
            long seq1 = QuotationFilterSignMapping.AddSign(key1);
            var seq2 = QuotationFilterSignMapping.AddSign(key2);
            var seq3 = QuotationFilterSignMapping.AddSign(key3);
            var seq4 = QuotationFilterSignMapping.AddSign(key4);
            Assert.AreEqual(seq1, 1);
            Assert.AreEqual(seq2, 2);
            Assert.AreEqual(seq3, 3);
            Assert.AreEqual(seq4, 1);

            var r1 = QuotationFilterSignMapping.Remove(key1);
            var r2 = QuotationFilterSignMapping.Remove(key2);
            var r3 = QuotationFilterSignMapping.Remove(key3);
            var r4 = QuotationFilterSignMapping.Remove(key4);

            Assert.IsFalse(r1);
            Assert.IsTrue(r2);
            Assert.IsTrue(r3);
            Assert.IsTrue(r4);

           
        }
    }
}
