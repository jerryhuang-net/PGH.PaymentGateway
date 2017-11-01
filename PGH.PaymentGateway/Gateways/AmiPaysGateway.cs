using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGH.PaymentGateway.Gateways
{
    public class AmiPaysGateway : PaymentGateway
    {
        public override PaymentResult Authorize(Money money, CreditCard card, PaymentOptions options)
        {
            throw new NotImplementedException();
        }

        public override PaymentResult Capture(Money money, string transRef, PaymentOptions options)
        {
            throw new NotImplementedException();
        }

        public override PaymentResult Credit(Money money, CreditCard card, PaymentOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Init(PaymentGatewaySettings options)
        {
            if (options == null)
                throw new ArgumentNullException("options");
            Parameters.ShortName = "AmiPays";
            if (string.IsNullOrEmpty(options.BaseUrl))
                options.BaseUrl = "https://www.Amipays.com/Api/Payin/";
            Parameters.Options = options;
        }

        public override PaymentResult Purchase(Money money, CreditCard card, PaymentOptions options)
        {
            throw new NotImplementedException();
        }

        public override PaymentResult Refund(Money money, string transRef, PaymentOptions options)
        {
            throw new NotImplementedException();
        }

        public override string Store(CreditCard card, PaymentOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Test(string env = "")
        {
            if (Parameters.Options == null)
                throw new ArgumentException("Please call Init(options) first.");
            Parameters.Options.BaseUrl = "http://test.amipays.com/Api/TestPayin/";
        }

        public override void UnStore(CreditCard card, PaymentOptions options)
        {
            throw new NotImplementedException();
        }

        public override PaymentResult Verify(CreditCard card, PaymentOptions options)
        {
            throw new NotImplementedException();
        }

        public override PaymentResult Void(string transRef, PaymentOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
