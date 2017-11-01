
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PGH.PaymentGateway.Gateways
{
    
    public class PayDollarGateway : PaymentGateway
    {
        public override void Init(PaymentGatewaySettings options)
        {//https://test.paydollar.com/b2cDemo/eng/payment/payForm.jsp
            if (options == null)
                throw new ArgumentNullException("options");
            Parameters.ShortName = "PayDollor";
            if (string.IsNullOrEmpty(options.BaseUrl))
                options.BaseUrl = "https://www.paydollar.com/b2c2/eng";
            Parameters.Options = options;

            System.Net.ServicePointManager.SecurityProtocol = (System.Net.SecurityProtocolType)3072;
        }
        public override void Test(string env = "")
        {
            if (Parameters.Options == null)
                throw new ArgumentException("Please call Init(options) first.");
            Parameters.Options.BaseUrl = "https://test.paydollar.com/b2cDemo/eng";
        }
        
        public override string Store(CreditCard card, PaymentOptions options)
        {
            throw new NotImplementedException();
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

        public override PaymentResult Purchase(Money money, CreditCard card, PaymentOptions options)
        {
            if (options.GetType() != typeof(PayDollarPaymentOptions))
                throw new ArgumentException("options parameter needs to be type of PayDollarPaymentOptions.");
            PayDollarPaymentOptions opt = (PayDollarPaymentOptions)options;
            string url = string.Format("{0}/directPay/payComp.jsp", Parameters.Options.BaseUrl);
            opt.Amount = money.Amount;
            opt.Currency = money.Currency;
            
            string hash = string.Empty;
            //apply to authorized merchant only
            if (!string.IsNullOrEmpty(opt.SecretKey))
                hash = GetSecureHash(opt);
            var sb = new StringBuilder();
            sb.Append("merchantId=");
            sb.Append(opt.MerchantId);
            sb.Append("&loginId=");
            sb.Append(opt.LoginId);
            sb.Append("&password=");
            sb.Append(opt.Password);
            sb.Append("&orderRef=");
            sb.Append(opt.MerchantRef);
            sb.Append("&amount=");
            sb.Append(money.Amount);
            sb.Append("&currCode=");
            sb.Append(money.Currency);
            sb.Append("&pMethod=");
            sb.Append(opt.PaymentMethod);
            sb.Append("&epMonth=");
            sb.Append(card.ExpiryMonth);
            sb.Append("&epYear=");
            sb.Append(card.ExpiryYear);
            sb.Append("&cardNo=");
            sb.Append(card.CardNo);
            sb.Append("&cardHolder=");
            sb.Append(card.HolderName);
            sb.Append("&securityCode=");
            sb.Append(card.CVV);
            sb.Append("&remark=");
            sb.Append(opt.TransactionRemark);
            if (!string.IsNullOrEmpty(hash))
            {
                sb.Append("&secureHash=");
                sb.Append(hash);
            }
            var result = HttpPost(url, sb.ToString(), "POST", "application/x-www-form-urlencoded");
            var q = System.Web.HttpUtility.ParseQueryString(result);
            var r = new PayDollarPaymentResult();
            r.SuccessCode = q["successcode"];
            r.Message = q["errMsg"];
            r.GatewayRef = q["PayRef"];
            r.MerchantRef = q["Ref"];
            r.Src = q["src"];
            r.Prc = q["prc"];
            if (!string.IsNullOrWhiteSpace(q["Amt"]))
                r.Amount = decimal.Parse(q["Amt"]);
            r.Holder = q["Holder"];
            r.TransactionTime = DateTime.Parse(q["TxTime"]);
            r.BankRef = q["ord"];
            r.ApprovalCode = q["AuthId"];
            r.Currency = q["Cur"];
            return r;
        }
        private string GetSecureHash(PayDollarPaymentOptions opt)
        {
            var hash = GetSHA1Hash(string.Format("{0}|{1}|{2}|{3}|{4}|{5}",
              opt.MerchantId, opt.MerchantRef, opt.Currency, opt.Amount, opt.PaymentType, opt.SecretKey));
            return hash;
        }
        private string GetSHA1Hash(string input)
        {
            using (SHA1Managed sha1 = new SHA1Managed())
            {
                var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(input));
                var sb = new StringBuilder(hash.Length * 2);
                foreach (byte b in hash)
                {
                    //"x2": lowercase, X2 upppercase
                    sb.Append(b.ToString("x2"));
                }
                return sb.ToString();
            }

        }
        public override PaymentResult Refund(Money money, string transRef, PaymentOptions options)
        {
            throw new NotImplementedException();
        }
    }

    public class PayDollarPaymentOptions : PaymentOptions
    {
        public string LoginId { get; set; }
        public string Password { get; set; }
        public string Language { get; set; }
        /// <summary>
        /// VISA, Master, Diners, JCB, AMEX, etc
        /// </summary>
        public string PaymentMethod { get; set; }
        /// <summary>
        /// H: hold, equals ot AUTH; N: normal payment
        /// </summary>
        public string PaymentType { get; set; }
        public string TransactionRemark { get; set; }
        public string SecretKey { get; set; }
    }

    public class PayDollarPaymentResult: PaymentResult
    {
        /// <summary>
        /// 0: success; non-zero: failed
        /// </summary>
        public string SuccessCode { get; set; }
        public string Message { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string Src { get; set; }
        public string Prc { get; set; }
        public string BankRef { get; set; }
        public string Holder { get; set; }
        public string ApprovalCode { get; set; }
        public DateTime TransactionTime { get; set; }
    }
}
