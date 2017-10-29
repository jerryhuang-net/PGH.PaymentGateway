using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PGH.PaymentGateway.UnitTest
{
    [TestClass]
    public class CreditCardTest
    {
        [TestMethod]
        public void TestExpiryDate()
        {
            var cc = new CreditCard();
            cc.Month = 9;
            cc.Year = 2017;
            Assert.AreEqual(cc.ExpiryDate, "201709", "201709 not equals to " + cc.ExpiryDate);
            cc.Month = 12;
            cc.Year = 2017;
            Assert.AreEqual(cc.ExpiryDate, "201712", "201712 not equals to " + cc.ExpiryDate);

        

        }
    }
}
