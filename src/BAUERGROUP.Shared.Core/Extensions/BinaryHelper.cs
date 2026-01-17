using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BAUERGROUP.Shared.Core.Extensions
{
    public static class BinaryHelper
    {
        public static void WriteBytesToFile(String filename, Byte[] data)
        {
            File.WriteAllBytes(filename, data);
        }

        public static Byte[] ReadBytesFromFile(String filename)
        {
            return File.ReadAllBytes(filename);
        }
    }
}
