using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BAUERGROUP.Shared.Core.Files
{
    public static class FileManagementUtilities
    {
        public static void MoveDirectoryContents(String sSourceDirectory, String sDestinationDirectory)
        {
            Directory.Move(sSourceDirectory, sDestinationDirectory);
        }

        public static void CopyDirectoryContents(String sSourceDirectory, String sDestinationDirectory)
        {
            if (sSourceDirectory == sDestinationDirectory)
                throw new ArgumentException("Source and destination cannot be the same directory.");

            foreach (var sPath in Directory.GetDirectories(sSourceDirectory, "*", SearchOption.AllDirectories))
                Directory.CreateDirectory(sPath.Replace(sSourceDirectory, sDestinationDirectory));

            foreach (var sPath in Directory.GetFiles(sSourceDirectory, "*.*", SearchOption.AllDirectories))
                File.Copy(sPath, sPath.Replace(sSourceDirectory, sDestinationDirectory), true);
        }
    }
}
