using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace sh.WindowsClient
{

    
    public class WebApiClient
    {



        //该方法用于验证服务器证书是否合法，当然可以直接返回true来表示验证永远通过。服务器证书具体内容在参数certificate中。可根据个人需求验证
        //该方法在request.GetResponse()时触发
        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true; //总是接受  
        }

        private static readonly string DefaultUserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";


        public static WebApiResult GetResult(string url, string method, string json = "")
        {
            var m = Method.GET;
            if (Enum.TryParse(method, out m)) return new WebApiResult(GetResponse(url, m, json));
            else throw new Exception("传入的http方法不正确：" + method);
        }



        internal static IRestResponse GetResponse(string url, Method method, string json = "")
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
            var request = new RestRequest(method);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("User-Agent", DefaultUserAgent);
            request.AddHeader("Accept", "*/*");
            //request.AddHeader("Cache-Control", "no-cache");
            //request.AddHeader("Postman-Token", "ff233143-9594-4aa3-9990-fa039b92de05");
            //request.AddHeader("Host", "localhost:44371");
            //request.AddHeader("Accept-Encoding", "gzip, deflate");
            //request.AddHeader("Content-Length", "14");
            request.AddHeader("Connection", "keep-alive");
            request.AddParameter("application/json", json, ParameterType.RequestBody);
            return client.Execute(request);
        }
    }
}
