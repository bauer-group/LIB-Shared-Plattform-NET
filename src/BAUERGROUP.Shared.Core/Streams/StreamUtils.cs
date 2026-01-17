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
            ReadExactly(inputStream, fileBytes, 0, (int)inputStream.Length);

            return fileBytes;
        }

        public static Stream? GetRessourceAsStream(String ressource, Type type, String ressourcePath = @"Resources")
        {
            var assembly = Assembly.GetAssembly(type);
            if (assembly == null) return null;
            var ressourceName = String.Format(@"{0}.{1}.{2}", assembly.GetName().Name, ressourcePath, ressource);

            return assembly.GetManifestResourceStream(ressourceName);
        }

        public static Boolean CopyRessourceToFile(String ressource, String filename, Type type, Boolean alwaysOverwriteFile = false, String ressourcePath = @"Resources")
        {
            if (File.Exists(filename) && !alwaysOverwriteFile)
                return true;

            using (var resourceStream = GetRessourceAsStream(ressource, type, ressourcePath))
            {
                if (resourceStream == null)
                    return false;

                FileStream fileStream = new FileStream(filename, FileMode.Create);
                resourceStream.CopyTo(fileStream);
                fileStream.Close();
            }

            return true;
        }

        private static void ReadExactly(Stream stream, byte[] buffer, int offset, int count)
        {
#if NETSTANDARD2_0
            int totalRead = 0;
            while (totalRead < count)
            {
                int bytesRead = stream.Read(buffer, offset + totalRead, count - totalRead);
                if (bytesRead == 0)
                {
                    throw new EndOfStreamException("Unexpected end of stream.");
                }
                totalRead += bytesRead;
            }
#else
            stream.ReadExactly(buffer, offset, count);
#endif
        }
    }
}
