using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace update
{
    public class FilePackage
    {
        public string Url { get; set; }

        public List<string> Files { get; set; }



        public void Download(DirectoryInfo target,string FileName)
        {
            var url = Url + FileName;
            var targetfile = new FileInfo($@"{target.FullName}\{FileName}");
            if (!targetfile.Directory.Exists) targetfile.Directory.Create();
            FtpWebRequest req = (FtpWebRequest)WebRequest.Create(url);
            req.Method = WebRequestMethods.Ftp.DownloadFile;
            req.KeepAlive = false;
            req.UseBinary = true;
            using (var response = req.GetResponse())
            {
                using (var reader = response.GetResponseStream())
                {
                    using (var writer = targetfile.OpenWrite())
                    {
                        var buffLength = 2048;
                        var buff = new byte[buffLength];
                        var contentLen = reader.Read(buff, 0, buffLength);
                        while (contentLen != 0)
                        {
                            writer.Write(buff, 0, contentLen);
                            contentLen = reader.Read(buff, 0, buffLength);
                        }
                    }
                }
            }

        }

    }

}
