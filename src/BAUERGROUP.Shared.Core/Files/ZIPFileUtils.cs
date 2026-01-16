using BAUERGROUP.Shared.Core.Internet;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using BAUERGROUP.Shared.Core.Streams;

namespace BAUERGROUP.Shared.Core.Files
{
    public class ZIPFileUtils: IDisposable
    {
        private ZipInputStream ZIP { get; set; }

        public ZIPFileUtils(Stream streamZIPFile)
        {
            ZIP = new ZipInputStream(streamZIPFile);         
        }

        public ZIPFileUtils(Uri oURL) :
            this(DownloadUtils.DownloadContentToStream(oURL.AbsoluteUri))
        {            

        }

        public List<byte[]> GetFilesAsByteList()
        {
            var r = new List<byte[]>(0);

            foreach (var fileStream in GetFilesAsStreamList())
            {
                r.Add(fileStream.CopyToBytes());
                fileStream.Close();                
            }

            return r;
        }

        public List<Stream> GetFilesAsStreamList()
        {
            var r = new List<Stream>(0);

            ZipEntry zipEntry;
            while ((zipEntry = ZIP.GetNextEntry()) != null)
                r.Add(ZIP.CopyToMemoryStream());

            return r;
        }

        public void Dispose()
        {
            ZIP.Close();
        }
    }
}
