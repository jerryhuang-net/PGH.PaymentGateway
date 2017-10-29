using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGH.PaymentGateway
{
    public abstract class PaymentGateway : IPaymentGateway
    {
        PaymentGatewayParameters _defaultParas;
        public PaymentGatewayParameters Parameters
        {
            get
            {
                if (_defaultParas == null)
                    _defaultParas = new PaymentGatewayParameters();
                return _defaultParas;
            }
        }

        public abstract void Authorize(Money money, CreditCard card, PaymentOptions options);
        public abstract void Capture(Money money, string transRef, PaymentOptions options);
        public abstract void Credit(Money money, CreditCard card, PaymentOptions options);
        public abstract void Init(PaymentGatewaySettings options);
        public abstract void Purchase(Money money, CreditCard card, PaymentOptions options);
        public abstract void Refund(Money money, string transRef, PaymentOptions options);
        public abstract void Store(CreditCard card, PaymentOptions options);
        public abstract void Test(string env = "");
        public abstract void UnStore(CreditCard card, PaymentOptions options);
        public abstract void Verify(CreditCard card, PaymentOptions options);
        public abstract void Void(string transRef, PaymentOptions options);
    }
}
