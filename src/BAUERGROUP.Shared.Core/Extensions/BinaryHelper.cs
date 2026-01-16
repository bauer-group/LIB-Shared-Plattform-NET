using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BAUERGROUP.Shared.Core.Extensions
{
    public static class BinaryHelper
    {
        public static void WriteBytesToFile(String sFilename, Byte[] bData)
        {
            File.WriteAllBytes(sFilename, bData);
        }

        public static Byte[] ReadBytesFromFile(String sFilename)
        {
            return File.ReadAllBytes(sFilename);
        }
    }
}
