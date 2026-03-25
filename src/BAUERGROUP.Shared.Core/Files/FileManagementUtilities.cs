using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BAUERGROUP.Shared.Core.Files
{
    public static class FileManagementUtilities
    {
        public static void MoveDirectoryContents(String sourceDirectory, String destinationDirectory)
        {
            Directory.Move(sourceDirectory, destinationDirectory);
        }

        public static void CopyDirectoryContents(String sourceDirectory, String destinationDirectory)
        {
            if (sourceDirectory == destinationDirectory)
                throw new ArgumentException("Source and destination cannot be the same directory.");

            // Normalize paths to ensure consistent trailing separator
            var normalizedSource = Path.GetFullPath(sourceDirectory)
                .TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
                + Path.DirectorySeparatorChar;

            foreach (var path in Directory.GetDirectories(sourceDirectory, "*", SearchOption.AllDirectories))
            {
                var relativePath = path.Substring(normalizedSource.Length);
                Directory.CreateDirectory(Path.Combine(destinationDirectory, relativePath));
            }

            foreach (var path in Directory.GetFiles(sourceDirectory, "*.*", SearchOption.AllDirectories))
            {
                var relativePath = path.Substring(normalizedSource.Length);
                File.Copy(path, Path.Combine(destinationDirectory, relativePath), true);
            }
        }
    }
}
