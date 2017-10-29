using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGH.PaymentGateway
{
    //https://qiita.com/Ippoippo@github/items/c623364b32fb432cf3fb
    interface IPaymentGateway
    {
        /// <summary>
        /// get gateway's default parameters, such as name
        /// </summary>
        PaymentGatewayParameters Parameters { get; }
        /// <summary>
        /// initialize the payment gateway, such as passing gateway URLs, API keys, etc
        /// </summary>
        /// <param name="options"></param>
        void Init(PaymentGatewaySettings options);
        /// <summary>
        /// call this after Init to indicate this is a test
        /// </summary>
        /// <param name="env">some gateway support multiple testing environment, if the gateway only support one, ignore this parameter</param>
        void Test(string env = "");
        /// <summary>
        /// Perform a combined AUTH/CAPTURE, with specified amount, using supplied Card 
        /// </summary>
        void Purchase(Money money, CreditCard card);
        /// <summary>
        /// Perform an AUTH, for a specified amount, using supplied Card
        /// </summary>
        void Authorize(Money money, CreditCard card);
        /// <summary>
        /// Capture a previous AUTH
        /// </summary>
        void Capture(Money money, string transRef);
        /// <summary>
        /// Cancel a previous SALE, AUTH, or CAPTURE (if gateway supports it)
        /// </summary>
        void Void(string transRef);
        /// <summary>
        /// Refund a pervious transaction, partial refund is possible
        /// </summary>
        void Refund(Money money, string transRef);
        /// <summary>
        /// which is like `refund`, but allows you to add money back to a card without having to reference a previous transaction.
        /// </summary>
        /// <param name="money"></param>
        /// <param name="card"></param>
        void Credit(Money money, CreditCard card);
        /// <summary>
        /// Some Gateways support verification of card details
        /// </summary>
        void Verify(CreditCard card);
        /// <summary>
        /// Use supplied card details, and obtain a card token.
        /// </summary>
        void Store(CreditCard card);
        /// <summary>
        /// to void a card token
        /// </summary>
        void UnStore();
    }

    public class PaymentGatewaySettings
    {

    }

    public class PaymentGatewayParameters
    {
        public string Name { get; internal set; }
        /// <summary>
        /// the gateway's class name must be: ShortName+'Gateway'
        /// </summary>
        public string ShortName { get; internal set; }
    }
}
