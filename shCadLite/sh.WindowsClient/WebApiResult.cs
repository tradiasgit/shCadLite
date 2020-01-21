using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sh.WindowsClient
{
    public class WebApiResult
    {
        internal WebApiResult(IRestResponse res)
        {
            ErrorMessage = res.ErrorMessage;
            ResponseStatus = res.ResponseStatus.ToString();
            ResponseUri = res.ResponseUri;
            ErrorException = res.ErrorException;
            IsSuccessful = res.IsSuccessful;
            StatusCode = (int)res.StatusCode;
            Content = res.Content;
            ContentEncoding = res.ContentEncoding;
            ContentLength = res.ContentLength;
            ContentType = res.ContentType;
            StatusDescription = res.StatusDescription;
            ProtocolVersion = res.ProtocolVersion;
        }


        public string ErrorMessage { get; set; }
        public string ResponseStatus { get; set; }
        public Uri ResponseUri { get; set; }
        public Exception ErrorException { get; set; }
        public byte[] RawBytes { get; set; }
        public bool IsSuccessful { get; }
        public int StatusCode { get; set; }
        public string Content { get; set; }
        public string ContentEncoding { get; set; }
        public long ContentLength { get; set; }
        public string ContentType { get; set; }
        public string StatusDescription { get; set; }
        public Version ProtocolVersion { get; set; }
    }
}
