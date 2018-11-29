using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Library;

namespace LibraryUnitTestProject
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void IsCheckInOutHelper()
        {
            string card = "123";
            string isbn = "1234";

            var cioh = new CheckInOutHelper(card, isbn);

            Assert.IsInstanceOfType(cioh, typeof(CheckInOutHelper));
        }
    }
}
