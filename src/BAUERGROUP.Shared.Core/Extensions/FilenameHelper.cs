using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BAUERGROUP.Shared.Core.Extensions
{
    public static class FilenameHelper
    {
        public static String ToValidFilename(this String input, Char replacement = '_')
        {
            foreach (var invalidChar in Path.GetInvalidFileNameChars())
            {
                input = input.Replace(invalidChar, replacement);
            }

            return input;
        }
    }
}
