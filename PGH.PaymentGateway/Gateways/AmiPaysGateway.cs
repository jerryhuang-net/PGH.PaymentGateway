using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGH.PaymentGateway.Gateways
{
    public class AmiPaysGateway : PaymentGateway
    {
        public override void Init(PaymentGatewaySettings options)
        {
            if (options == null)
                throw new ArgumentNullException("options");
            Parameters.ShortName = "AmiPays";
            if (string.IsNullOrEmpty(options.BaseUrl))
                options.BaseUrl = "https://www.Amipays.com/Api/Payin/";
            Parameters.Options = options;
        }
        public override void Test(string env = "")
        {
            if (Parameters.Options == null)
                throw new ArgumentException("Please call Init(options) first.");
            Parameters.Options.BaseUrl = "http://test.amipays.com/Api/TestPayin/";
        }

        public override void Authorize(Money money, CreditCard card, PaymentOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Capture(Money money, string transRef, PaymentOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Credit(Money money, CreditCard card, PaymentOptions options)
        {
            throw new NotImplementedException();
        }

      

        public override void Purchase(Money money, CreditCard card, PaymentOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Refund(Money money, string transRef, PaymentOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Store(CreditCard card, PaymentOptions options)
        {
            throw new NotImplementedException();
        }

       

        public override void UnStore(CreditCard card, PaymentOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Verify(CreditCard card, PaymentOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Void(string transRef, PaymentOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
