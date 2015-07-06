using Orange.Core.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Orange.Test
{
    [TestClass]
    public class ExtensionTest
    {
        [TestMethod]
        public void TrimToEllipsis()
        {
            string myString = "Michael programs way too much!";
            myString = myString.TrimToEllipsis(10);
            Assert.AreEqual(myString, "Michael...");
        }
    }
}
