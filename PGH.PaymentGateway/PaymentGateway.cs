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
                request.UseDefaultCredentials = true;
                request.Proxy.Credentials = CredentialCache.DefaultNetworkCredentials;
            }
            return request;
        }
        protected string HttpGet(string url, string contentType = "application/json")
        {
            HttpWebRequest request = CreateWebRequest(url);
            request.ContentType = contentType;
            WriteTraceLog("HTTP", string.Format("GET URL={0}", url));
       
            WriteTraceLog("HTTP", string.Format("ContentType={0}", contentType));
            return GetResponse(request.GetResponse());
        }
        private static object obj = new object();
        protected static void WriteTraceLog(string category, string msg)
        {

            if (System.Web.HttpContext.Current != null)
            {
                System.Web.HttpContext.Current.Trace.Write(category, msg);
                if (Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["EnableTraceLog"]))
                {
                    lock (obj)
                    {
                        string path = System.Web.HttpContext.Current.Server.MapPath("~/log");
                        if (!System.IO.Directory.Exists(path))
                        {
                            System.IO.Directory.CreateDirectory(path);
                        }
                        path = System.IO.Path.Combine(path, DateTime.Now.ToString("yyyyMMdd") + ".txt");
                        string line = string.Format("{0} {1}\n {2}\n" , DateTime.Now.ToString(), category, msg);
                        System.IO.File.AppendAllText(path, line);
                    }
                }
            }
            else
            {
                if (Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["EnableTraceLog"]))
                {
                    lock (obj)
                    {
                        string path = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "log");
                        if (!System.IO.Directory.Exists(path))
                        {
                            System.IO.Directory.CreateDirectory(path);
                        }
                        path = System.IO.Path.Combine(path, DateTime.Now.ToString("yyyyMMdd") + ".txt");
                        string line = string.Format("{0} {1}\n {2}\n", DateTime.Now.ToString(), category, msg);
                        System.IO.File.AppendAllText(path, line);
                    }
                }
            }
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
            WriteTraceLog("HTTP", string.Format("{1} URL={0}", url, method));
            WriteTraceLog("HTTP", string.Format("ContentType={0}", contentType));
            if (!string.IsNullOrEmpty(postData))
            {
                byte[] dataBuffer = System.Text.Encoding.UTF8.GetBytes(postData);
                request.ContentLength = dataBuffer.Length;
                using (var webStream = request.GetRequestStream())
                {
                    webStream.Write(dataBuffer, 0, dataBuffer.Length);
                }
                WriteTraceLog("HTTP", string.Format("PostData={0}", postData));
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
            WriteTraceLog("HTTP", string.Format("RESPONSE CODE={0}", (int)((HttpWebResponse) webResponse).StatusCode));
            using (var webStream = webResponse.GetResponseStream())
            {
                if (webStream != null)
                {
                    using (var responseReader = new System.IO.StreamReader(webStream))
                    {

                        var r = responseReader.ReadToEnd();
                        WriteTraceLog("HTTP", string.Format("RESPONSE={0}", r));
                        return r;
                    }
                }
                else
                {
                    WriteTraceLog("HTTP", "RESPONSE=EMPTY");
                    return string.Empty;
                }
            }
        }
        #endregion
    }
}
