using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BAUERGROUP.Shared.Core.Extensions
{
    public static class Base64Helper
    {
        public static String Base64Encode(this String value)
        {
            return Convert.ToBase64String(Encoding.Default.GetBytes(value));
        }

        public static String Base64Decode(this String value)
        {
            return Encoding.Default.GetString(Convert.FromBase64String(value));
        }

        public static String FileToBase64(String filename)
        {
            return Convert.ToBase64String(BinaryHelper.ReadBytesFromFile(filename));
        }

        public static void Base64ToFile(String base64, string filename)
        {
            BinaryHelper.WriteBytesToFile(filename, Convert.FromBase64String(base64));
        }

        public static String ByteToBase64(Byte[] bytes)
        {
            return Convert.ToBase64String(bytes);
        }

        public static Byte[] Base64ToByte(String base64)
        {
            return Convert.FromBase64String(base64);
        }

        public static Byte[] Base64ToByte(Byte[] base64Bytes)
        {
            return Convert.FromBase64String(Encoding.ASCII.GetString(base64Bytes));
        }
    }
}
