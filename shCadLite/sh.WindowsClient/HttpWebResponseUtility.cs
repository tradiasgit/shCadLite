using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace sh.WindowsClient
{
    /// <summary>  
    /// 有关HTTP请求的辅助类  
    /// </summary>  
    class HttpWebResponseUtility
    {
        private static readonly string DefaultUserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";

        private static HttpWebRequest CreateHttpRequest(string url,string method="GET", string json=null, int? timeout = 5000, string userAgent = null, Encoding requestEncoding = null, CookieCollection cookies = null)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }
            if (requestEncoding == null)
            {
                requestEncoding = Encoding.UTF8;
            }
            //HttpWebRequest request = null;
            //如果是发送HTTPS请求  
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                // 这里设置了协议类型。
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //(SecurityProtocolType)768 | (SecurityProtocolType)3072
                ServicePointManager.CheckCertificateRevocationList = true;
                ServicePointManager.DefaultConnectionLimit = 100;
                ServicePointManager.Expect100Continue = false;
            }

            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.KeepAlive = false;
            request.ProtocolVersion = HttpVersion.Version10;
            request.Method = method;    //使用get方式发送数据
            //request.ContentType = "application/json";
            request.ContentType = "application/json, text/javascript, */*";
            request.Referer = "";
            request.AllowAutoRedirect = true;            
            request.Accept = "*/*";
            request.Method = method;
            
            if (!string.IsNullOrEmpty(userAgent))
            {
                request.UserAgent = userAgent;
            }
            else
            {
                request.UserAgent = DefaultUserAgent;
            }
            if (timeout.HasValue)
            {
                request.Timeout = timeout.Value;
            }
            if (cookies != null)
            {
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(cookies);
            }
            //如果需要POST数据  
            if (!(json == null || json.Length == 0))
            {
                request.ContentLength = json.Length;
                byte[] data = requestEncoding.GetBytes(json);
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
                
            }
            else request.ContentLength = 0;
            //获取网页响应结果
            return request;
        }




        public static IRestResponse  s(string url)
        {
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                // 这里设置了协议类型。
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //(SecurityProtocolType)768 | (SecurityProtocolType)3072
                ServicePointManager.CheckCertificateRevocationList = true;
                ServicePointManager.DefaultConnectionLimit = 100;
                ServicePointManager.Expect100Continue = false;
            }
            var client = new RestClient(url);
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("User-Agent", "PostmanRuntime/7.21.0");
            request.AddHeader("Accept", "*/*");
            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("Postman-Token", "c8606d54-2bb5-4ded-8426-bbc992252f4d");
            request.AddHeader("Host", "localhost:44371");
            request.AddHeader("Accept-Encoding", "gzip, deflate");
            request.AddHeader("Connection", "keep-alive");
            request.AddParameter("application/json", "", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            return response;
        }






        public static HttpWebResponse CreateGetHttpResponse(string url)
        {
            var req = CreateHttpRequest(url, "GET");
            return req.GetResponse() as HttpWebResponse;
        }
        public static HttpWebResponse CreatePostHttpResponse(string url, string json)
        {
            var req = CreateHttpRequest(url, "POST",json);
            return req.GetResponse() as HttpWebResponse;
        }


        public static HttpWebResponse CreatePutHttpResponse(string url, string json)
        {
            var req = CreateHttpRequest(url, "PUT",json);
            return req.GetResponse() as HttpWebResponse;
        }
        public static HttpWebResponse CreateDeleteHttpResponse(string url)
        {
            var req = CreateHttpRequest(url, "DELETE");
            return req.GetResponse() as HttpWebResponse;
        }

        //该方法用于验证服务器证书是否合法，当然可以直接返回true来表示验证永远通过。服务器证书具体内容在参数certificate中。可根据个人需求验证
        //该方法在request.GetResponse()时触发
        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true; //总是接受  
        }


    }
}
