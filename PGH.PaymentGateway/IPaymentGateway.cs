using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGH.PaymentGateway
{
    //https://qiita.com/Ippoippo@github/items/c623364b32fb432cf3fb
    //https://github.com/activemerchant/active_merchant/wiki/Contributing
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
        void Purchase(Money money, CreditCard card, PaymentOptions options);
        /// <summary>
        /// Perform an AUTH, for a specified amount, using supplied Card
        /// </summary>
        void Authorize(Money money, CreditCard card, PaymentOptions options);
        /// <summary>
        /// Capture a previous AUTH
        /// </summary>
        void Capture(Money money, string transRef, PaymentOptions options);
        /// <summary>
        /// Cancel a previous SALE, AUTH, or CAPTURE (if gateway supports it)
        /// </summary>
        void Void(string transRef, PaymentOptions options);
        /// <summary>
        /// Refund a pervious transaction, partial refund is possible
        /// </summary>
        void Refund(Money money, string transRef, PaymentOptions options);
        /// <summary>
        /// which is like `refund`, but allows you to add money back to a card without having to reference a previous transaction.
        /// </summary>
        /// <param name="money"></param>
        /// <param name="card"></param>
        void Credit(Money money, CreditCard card, PaymentOptions options);
        /// <summary>
        /// Some Gateways support verification of card details
        /// </summary>
        void Verify(CreditCard card, PaymentOptions options);
        /// <summary>
        /// Use supplied card details, and obtain a card token.
        /// </summary>
        void Store(CreditCard card, PaymentOptions options);
        /// <summary>
        /// to void a card token
        /// </summary>
        void UnStore(CreditCard card, PaymentOptions options);
    }

    public class PaymentGatewaySettings
    {
        public string BaseUrl { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ApiKey { get; set; }

        public string DefaultCurrency { get; internal set; }
        Dictionary<string, string> _dict;
        public Dictionary<string, string> OtherSettings
        {
            get
            {
                if (_dict == null)
                    _dict = new Dictionary<string, string>();
                return _dict;
            }
        }
    }

    public class PaymentGatewayParameters
    {
        /// <summary>
        /// display name of the gateway
        /// </summary>
        public string Name { get; internal set; }
        /// <summary>
        /// the gateway's class name must be: ShortName+'Gateway'
        /// </summary>
        public string ShortName { get; internal set; }
        public PaymentGatewaySettings Options { get; internal set; }
    }

    public class PaymentOptions
    {
        public string MerchantId { get; set; }
        /// <summary>
        /// merchant reference number which is normally unique per merchant
        /// </summary>
        public string MerchantRef { get; set; }

        public object OtherParameter { get; set; }

    }
}
