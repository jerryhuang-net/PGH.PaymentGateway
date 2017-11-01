using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PGH.PaymentGateway
{
    public abstract class PaymentGateway : IPaymentGateway
    {
        public PaymentGateway()
        {
            ServicePointManager.DefaultConnectionLimit = 999999;
        }
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

        public abstract PaymentResult Authorize(Money money, CreditCard card, PaymentOptions options);
        public abstract PaymentResult Capture(Money money, string transRef, PaymentOptions options);
        public abstract PaymentResult Credit(Money money, CreditCard card, PaymentOptions options);
        public abstract void Init(PaymentGatewaySettings options);
        public abstract PaymentResult Purchase(Money money, CreditCard card, PaymentOptions options);
        public abstract PaymentResult Refund(Money money, string transRef, PaymentOptions options);
        public abstract string Store(CreditCard card, PaymentOptions options);
        public abstract void Test(string env = "");
        public abstract void UnStore(CreditCard card, PaymentOptions options);
        public abstract PaymentResult Verify(CreditCard card, PaymentOptions options);
        public abstract PaymentResult Void(string transRef, PaymentOptions options);

        #region http helper

        private  HttpWebRequest CreateWebRequest(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.KeepAlive = false;
            if (ConfigurationManager.AppSettings.AllKeys.Contains("HttpProxy"))
            {
                request.Proxy = new WebProxy(ConfigurationManager.AppSettings["HttpProxy"]
                    , int.Parse(ConfigurationManager.AppSettings["HttpProxyPort"]));
            }
            return request;
        }
        protected string HttpGet(string url, string contentType = "application/json")
        {
            HttpWebRequest request = CreateWebRequest(url);
            request.ContentType = contentType;
            return GetResponse(request.GetResponse());
        }

        protected T HttpGet<T>(string url, string contentType = "application/json")
        {
            var str = HttpGet(url, contentType);
            return JsonConvert.DeserializeObject<T>(str);
        }

        protected string HttpPost(string url, string postData, string method = "POST", string contentType = "application/json")
        {
            HttpWebRequest request = CreateWebRequest(url);
            request.ContentType = contentType;
            request.Method = method;
            if (!string.IsNullOrEmpty(postData))
            {
                byte[] dataBuffer = System.Text.Encoding.UTF8.GetBytes(postData);
                request.ContentLength = dataBuffer.Length;
                using (var webStream = request.GetRequestStream())
                {
                    webStream.Write(dataBuffer, 0, dataBuffer.Length);
                }
            }
            return GetResponse(request.GetResponse());
        }

        protected  T HttpPost<T>(string url, string postData, string method = "POST", string contentType = "application/json")
        {
            var str = HttpPost(url, postData, method, contentType);
            return JsonConvert.DeserializeObject<T>(str);
        }

        private static string GetResponse(WebResponse webResponse)
        {
            using (var webStream = webResponse.GetResponseStream())
            {
                if (webStream != null)
                {
                    using (var responseReader = new System.IO.StreamReader(webStream))
                    {

                        return responseReader.ReadToEnd();
                    }
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        #endregion
    }
}
