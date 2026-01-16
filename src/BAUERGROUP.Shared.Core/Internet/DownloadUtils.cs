using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using BAUERGROUP.Shared.Core.Streams;

namespace BAUERGROUP.Shared.Core.Internet
{
    public static class DownloadUtils
    {
        public static Stream DownloadContentToStream(String sURL, Boolean bAcceptInvalidCertificates = false)
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(sURL);
            httpWebRequest.AllowAutoRedirect = true;
            httpWebRequest.AllowWriteStreamBuffering = true;

            if (bAcceptInvalidCertificates)
                httpWebRequest.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

            HttpWebResponse httpWebReponse = (HttpWebResponse)httpWebRequest.GetResponse();
            var httpStream = httpWebReponse.GetResponseStream();
            
            //Download the content and copy it to an memory stream, to have an seekable stream
            var memoryStream = httpStream.CopyToMemoryStream();

            //Close not anymore reqired objects
            httpStream.Close();
            httpWebReponse.Close();

            return memoryStream;
        }

        public static Stream DownloadContentToStream(Uri oURL)
        {
            return DownloadContentToStream(oURL.AbsoluteUri);
        }
    }
}
