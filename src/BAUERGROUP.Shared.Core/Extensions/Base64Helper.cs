using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BAUERGROUP.Shared.Core.Extensions
{
    public static class Base64Helper
    {
        public static String Base64Encode(this String sValue)
        {
            return Convert.ToBase64String(Encoding.Default.GetBytes(sValue));
        }

        public static String Base64Decode(this String sValue)
        {
            return Encoding.Default.GetString(Convert.FromBase64String(sValue));
        }

        public static String FileToBase64(String sFilename)
        {
            return Convert.ToBase64String(BinaryHelper.ReadBytesFromFile(sFilename));
        }

        public static void Base64ToFile(String sBase64, string sFilename)
        {
            BinaryHelper.WriteBytesToFile(sFilename, Convert.FromBase64String(sBase64));
        }

        public static String ByteToBase64(Byte[] sBytes)
        {
            return Convert.ToBase64String(sBytes);
        }

        public static Byte[] Base64ToByte(String sBase64)
        {
            return Convert.FromBase64String(sBase64);
        }

        public static Byte[] Base64ToByte(Byte[] bBase64)
        {
            return Convert.FromBase64String(Encoding.ASCII.GetString(bBase64));
        }
    }
}
