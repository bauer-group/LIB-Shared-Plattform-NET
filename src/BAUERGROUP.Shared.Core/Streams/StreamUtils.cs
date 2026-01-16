using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BAUERGROUP.Shared.Core.Streams
{
    public static class StreamUtils
    {
        public static MemoryStream CopyToMemoryStream(this Stream inputStream)
        {
            var memoryStream = new MemoryStream();
            byte[] chunkBuffer = new byte[4096];
            int bytesRead;
            while ((bytesRead = inputStream.Read(chunkBuffer, 0, chunkBuffer.Length)) > 0)
                memoryStream.Write(chunkBuffer, 0, bytesRead);

            //Important, set pointer to begin of stream
            memoryStream.Seek(0, SeekOrigin.Begin);

            return memoryStream;
        }

        public static Byte[] CopyToBytes(this Stream inputStream)
        {
            var fileBytes = new byte[inputStream.Length];
            inputStream.Read(fileBytes, 0, (int)inputStream.Length);            

            return fileBytes;
        }

        public static Stream GetRessourceAsStream(String sRessource, Type oType, String sRessourcePath = @"Resources")
        {
            var oAssembly = Assembly.GetAssembly(oType);
            var sRessourceName = String.Format(@"{0}.{1}.{2}", oAssembly.GetName().Name, sRessourcePath, sRessource);

            return oAssembly.GetManifestResourceStream(sRessourceName);
        }

        public static Boolean CopyRessourceToFile(String sRessource, String sFilename, Type oType, Boolean bAlwaysOverwriteFile = false, String sRessourcePath = @"Resources")
        {
            if (File.Exists(sFilename) && !bAlwaysOverwriteFile)
                return true;

            using (var rs = GetRessourceAsStream(sRessource, oType, sRessourcePath))
            {
                if (rs == null)
                    return false;

                FileStream fs = new FileStream(sFilename, FileMode.Create);
                rs.CopyTo(fs);
                fs.Close();
            }

            return true;
        }
    }
}
