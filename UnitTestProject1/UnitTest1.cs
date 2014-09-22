using System;
using System.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sunshine;
using System.Collections.Generic;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void RectSubstract_Outside()
        {
            List<Rect> result;
            Rect src = new Rect(100,100,10,10);

            result = src.Substract(new Rect(0, 0, 10, 10));
            Assert.AreEqual(result.Count, 1);
            Assert.AreEqual(src, result[0]);

            result = src.Substract(new Rect(110, 100, 10, 10));
            Assert.AreEqual(result.Count, 1);
            Assert.AreEqual(src, result[0]);

            result = src.Substract(new Rect(110, 110, 10, 10));
            Assert.AreEqual(result.Count, 1);
            Assert.AreEqual(src, result[0]);

            result = src.Substract(new Rect(100, 0, 10, 10));
            Assert.AreEqual(result.Count, 1);
            Assert.AreEqual(src, result[0]);

            result = src.Substract(new Rect(100, 110, 10, 10));
            Assert.AreEqual(result.Count, 1);
            Assert.AreEqual(src, result[0]);
        }

        [TestMethod]
        public void RectSubstract_2Parts()
        {
            List<Rect> result;
            Rect src = new Rect(100, 100, 10, 10);

            result = src.Substract(new Rect(90, 90, 15, 15));
            Assert.AreEqual(result.Count, 2);
            Assert.AreEqual(new Rect(105,100,5,10), result[0]);
            Assert.AreEqual(new Rect(100, 105, 5, 5), result[1]);

            result = src.Substract(new Rect(105, 105, 15, 15));
            Assert.AreEqual(result.Count, 2);
            Assert.AreEqual(new Rect(100, 100, 5, 10), result[0]);
            Assert.AreEqual(new Rect(105, 100, 5, 5), result[1]);

            result = src.Substract(new Rect(90, 102, 30, 6));
            Assert.AreEqual(result.Count, 2);
            Assert.AreEqual(new Rect(100, 100, 10, 2), result[0]);
            Assert.AreEqual(new Rect(100, 108, 10, 2), result[1]);

            result = src.Substract(new Rect(102, 90, 6, 30));
            Assert.AreEqual(result.Count, 2);
            Assert.AreEqual(new Rect(100, 100, 2, 10), result[0]);
            Assert.AreEqual(new Rect(108, 100, 2, 10), result[1]);

        }

        [TestMethod]
        public void RectSubstract_3Parts()
        {
            List<Rect> result;
            Rect src = new Rect(100, 100, 10, 10);

            result = src.Substract(new Rect(90, 102, 12, 6));
            Assert.AreEqual(result.Count, 3);
            Assert.AreEqual(new Rect(102, 100, 8, 10), result[0]);
            Assert.AreEqual(new Rect(100, 100, 2, 2), result[1]);
            Assert.AreEqual(new Rect(100, 108, 2, 2), result[2]);

        }

        [TestMethod]
        public void RectSubstract_4Parts()
        {
            List<Rect> result;
            Rect src = new Rect(100, 100, 10, 10);

            result = src.Substract(new Rect(102, 102, 6, 6));
            Assert.AreEqual(result.Count, 4);
            Assert.AreEqual(new Rect(100, 100, 2, 10), result[0]);
            Assert.AreEqual(new Rect(108, 100, 2, 10), result[1]);
            Assert.AreEqual(new Rect(102, 100, 6, 2), result[2]);
            Assert.AreEqual(new Rect(102, 108, 6, 2), result[3]);

        }


    }
}
