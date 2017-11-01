using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PGH.PaymentGateway.Utils
{
    public class HttpHelper
    {
        private static HttpWebRequest CreateWebRequest(string url)
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
        public static string HttpGet(string url, string contentType="application/json")
        {
            HttpWebRequest request = CreateWebRequest(url);
            request.ContentType = contentType;
            return GetResponse(request.GetResponse());
        }

        public static T HttpGet<T>(string url, string contentType = "application/json")
        {
            var str = HttpGet(url, contentType);
            return JsonConvert.DeserializeObject<T>(str);
        }

        public static string HttpPost(string url, string postData, string method="POST", string contentType = "application/json")
        {
            HttpWebRequest request = CreateWebRequest(url);
            request.ContentType = contentType;
            request.Method = method;
            if (!string.IsNullOrEmpty(postData))
            {
                byte[] dataBuffer = System.Text.Encoding.UTF8.GetBytes(postData);
                request.ContentLength = dataBuffer.Length;
                using (Stream webStream = request.GetRequestStream())
                {
                    webStream.Write(dataBuffer, 0, dataBuffer.Length);
                }
            }
            return GetResponse(request.GetResponse());
        }

        public static T HttpPost<T>(string url, string postData, string method = "POST", string contentType = "application/json")
        {
            var str = HttpPost(url, postData, method, contentType);
            return JsonConvert.DeserializeObject<T>(str);
        }
     
        private static string GetResponse(WebResponse webResponse)
        {
            using (Stream webStream = webResponse.GetResponseStream())
            {
                if (webStream != null)
                {
                    using (StreamReader responseReader = new StreamReader(webStream))
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

    }
}
