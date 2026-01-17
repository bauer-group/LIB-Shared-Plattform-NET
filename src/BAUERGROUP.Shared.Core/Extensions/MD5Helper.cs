using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace BAUERGROUP.Shared.Core.Extensions
{
    public static class MD5Helper
    {
        //Source: http://msdn.microsoft.com/de-de/library/system.security.cryptography.md5%28v=vs.110%29.aspx

        private static String PrepareMD5HashResult(Byte[] hashBytes)
        {
            //Create a new Stringbuilder to collect the bytes and create a string.
            StringBuilder stringBuilder = new StringBuilder();

            //Loop through each byte of the hashed data and format each one as a hexadecimal string.
            for (int i = 0; i < hashBytes.Length; i++)
            {
                stringBuilder.Append(hashBytes[i].ToString("x2", CultureInfo.InvariantCulture));
            }

            //Return the hexadecimal string
            return stringBuilder.ToString();
        }

        public static String GetMD5Hash(this String input, Encoding? encoding = null)
        {
            //Convert the input string to a byte array and compute the hash.
            Byte[] hashBytes = MD5.Create().ComputeHash(encoding == null ? Encoding.ASCII.GetBytes(input) : encoding.GetBytes(input));

            return PrepareMD5HashResult(hashBytes);
        }

        public static Boolean VerifyMD5Hash(String input, String md5Hash)
        {
            //Hash the input.
            String hashOfInput = GetMD5Hash(input);

            //Create a StringComparer an compare the hashes.
            StringComparer stringComparer = StringComparer.OrdinalIgnoreCase;

            if (0 == stringComparer.Compare(hashOfInput, md5Hash))
                return true;
            else
                return false;
        }

        public static Boolean VerifyMD5Hash(Stream input, String md5Hash)
        {
            //Hash the input.
            String hashOfInput = GetMD5Hash(input);

            //Create a StringComparer an compare the hashes.
            StringComparer stringComparer = StringComparer.OrdinalIgnoreCase;

            if (0 == stringComparer.Compare(hashOfInput, md5Hash))
                return true;
            else
                return false;
        }

        public static String GetMD5Hash(this Stream input)
        {
            //Convert the input string to a byte array and compute the hash.
            Byte[] hashBytes = MD5.Create().ComputeHash(input);

            return PrepareMD5HashResult(hashBytes);
        }

        public static String GetMD5HashFromFile(String filename)
        {
            using (var fileStream = File.OpenRead(filename))
            {
                return fileStream.GetMD5Hash();
            }
        }
    }
}
