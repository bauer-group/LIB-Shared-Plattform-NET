using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BAUERGROUP.Shared.Core.Extensions
{
    public static class FilenameHelper
    {
        public static String ToValidFilename(this String sInput, Char sReplacement = '_')
        {
            foreach (var cInvalid in Path.GetInvalidFileNameChars())
            {
                sInput = sInput.Replace(cInvalid, sReplacement);
            }

            return sInput;
        }
    }
}
