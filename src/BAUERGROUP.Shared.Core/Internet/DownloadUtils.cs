using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using BAUERGROUP.Shared.Core.Streams;

namespace BAUERGROUP.Shared.Core.Internet
{
    public static class DownloadUtils
    {
        private static readonly HttpClient DefaultHttpClient = new();

        public static Stream DownloadContentToStream(string url, bool acceptInvalidCertificates = false)
        {
            return DownloadContentToStreamAsync(url, acceptInvalidCertificates).GetAwaiter().GetResult();
        }

        public static async Task<Stream> DownloadContentToStreamAsync(string url, bool acceptInvalidCertificates = false)
        {
            HttpClient client;

            if (acceptInvalidCertificates)
            {
                var handler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (_, _, _, _) => true
                };
                client = new HttpClient(handler);
            }
            else
            {
                client = DefaultHttpClient;
            }

            try
            {
                using var response = await client.GetAsync(url).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();

                var memoryStream = new MemoryStream();
                await response.Content.CopyToAsync(memoryStream).ConfigureAwait(false);
                memoryStream.Seek(0, SeekOrigin.Begin);

                return memoryStream;
            }
            finally
            {
                if (acceptInvalidCertificates)
                {
                    client.Dispose();
                }
            }
        }

        public static Stream DownloadContentToStream(Uri url)
        {
            return DownloadContentToStream(url.AbsoluteUri);
        }

        public static Task<Stream> DownloadContentToStreamAsync(Uri url, bool acceptInvalidCertificates = false)
        {
            return DownloadContentToStreamAsync(url.AbsoluteUri, acceptInvalidCertificates);
        }
    }
}
