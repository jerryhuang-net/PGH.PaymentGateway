using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PGH.PaymentGateway.Gateways;
using Newtonsoft.Json;

namespace PGH.PaymentGateway.UnitTest
{
    [TestClass]
    public class PayDollarTest
    {
        
        [TestMethod]
        public void TestPurchase()
        {
            var gw = new PayDollarGateway();
            var settings = new PaymentGatewaySettings();

            gw.Init(settings);
            gw.Test();
            //keep confidential info else where in purpose
            var o = System.IO.File.ReadAllText(@"D:\TestCase\PayDollar\trans1.json");
            var card = System.IO.File.ReadAllText(@"D:\TestCase\PayDollar\card.json");
           
            var m = new Money();
            m.Currency = "344";//hkd
            m.Amount = 1;
            var cc = JsonConvert.DeserializeObject<CreditCard>(card);
            var options = JsonConvert.DeserializeObject<PayDollarPaymentOptions>(o);
            var r = gw.Purchase(m, cc, options);
            PayDollarPaymentResult result = (PayDollarPaymentResult)r;
            Assert.AreEqual(result.SuccessCode, "0", "payDollar purchase failed");
        }

        [TestMethod]
        public void TestCapture()
        {
            var gw = new PayDollarGateway();
            var settings = new PaymentGatewaySettings();

            gw.Init(settings);
            gw.Test();
            //keep confidential info else where in purpose
            var o = System.IO.File.ReadAllText(@"D:\TestCase\PayDollar\trans2.json");
            var card = System.IO.File.ReadAllText(@"D:\TestCase\PayDollar\card.json");

            var m = new Money();
            m.Currency = "344";//hkd
            m.Amount = 1;
            var cc = JsonConvert.DeserializeObject<CreditCard>(card);
            var options = JsonConvert.DeserializeObject<PayDollarPaymentOptions>(o);
            var r = gw.Authorize(m, cc, options);
            PayDollarPaymentResult result = (PayDollarPaymentResult)r;
            Assert.AreEqual(result.SuccessCode, "0", "payDollar AUTH failed");

            var cr = gw.Capture(m, result.GatewayRef, options);
            PayDollarPaymentResult captureResult = (PayDollarPaymentResult)cr;
            Assert.AreEqual(captureResult.SuccessCode, "0", "payDollar Capture failed");
        }

        [TestMethod]
        public void TestRefund()
        {
            var gw = new PayDollarGateway();
            var settings = new PaymentGatewaySettings();

            gw.Init(settings);
            gw.Test();
            //keep confidential info else where in purpose
            var o = System.IO.File.ReadAllText(@"D:\TestCase\PayDollar\trans1.json");
            var card = System.IO.File.ReadAllText(@"D:\TestCase\PayDollar\card.json");

            var m = new Money();
            m.Currency = "344";//hkd
            m.Amount = 1;
            var cc = JsonConvert.DeserializeObject<CreditCard>(card);
            var options = JsonConvert.DeserializeObject<PayDollarPaymentOptions>(o);
            var r = gw.Purchase(m, cc, options);
            PayDollarPaymentResult result = (PayDollarPaymentResult)r;
            Assert.AreEqual(result.SuccessCode, "0", "payDollar Purchase failed");

            var cr = gw.Refund(m, result.GatewayRef, options);
            PayDollarPaymentResult refundResult = (PayDollarPaymentResult)cr;
            Assert.AreEqual(refundResult.SuccessCode, "0", "payDollar Refund failed");
        }
    }
}
