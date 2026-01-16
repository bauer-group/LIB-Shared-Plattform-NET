using BAUERGROUP.Shared.Core.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BAUERGROUP.Shared.Core.Files
{
    public class FileProcessing
    {
        public String JobDirectory { get; private set; }
        public String JobDirectory_Processed { get; private set; }
        public String JobDirectory_Unprocessed { get; private set; }

        public String JobFileFilter { get; private set; }

        public FileProcessing(String sJobDirectory, String sJobFileFilter)
        {
            if (sJobDirectory == null)
                throw new FileProcessingException("FileProcessing() -> Argument sJobDirectory cannot be NULL.", new ArgumentNullException("sDirectory cannot be NULL."));

            if (sJobFileFilter == null)
                throw new FileProcessingException("FileProcessing() -> Argument sJobFileFilter cannot be NULL.", new ArgumentNullException("sJobFileFilter cannot be NULL."));

            JobDirectory = sJobDirectory;
            JobFileFilter = sJobFileFilter;

            JobDirectory_Processed = Path.Combine(JobDirectory, @"Verarbeitet");
            JobDirectory_Unprocessed = Path.Combine(JobDirectory, @"Unverarbeitet");

            if (!Directory.Exists(JobDirectory))
                throw new FileProcessingException($"Folder '{JobDirectory}' does not exists.", new ArgumentException($"Folder '{JobDirectory}' does not exists."));
            
            if (!Directory.Exists(JobDirectory_Processed))
                Directory.CreateDirectory(JobDirectory_Processed);

            if (!Directory.Exists(JobDirectory_Unprocessed))
                Directory.CreateDirectory(JobDirectory_Unprocessed);
        }

        public String[] GetFiles()
        {
            if (!Directory.Exists(JobDirectory))
                throw new FileProcessingException($"Folder '{JobDirectory}' does not exists.");

            return Directory.GetFiles(JobDirectory, JobFileFilter, SearchOption.TopDirectoryOnly).OrderBy(p => new FileInfo(p).Name).ToArray();
        }

        public Int32 GetFilesCount()
        {
            return GetFiles().Length;
        }

        public void MoveFile(String sFile, FileProcessingStatus eStatus = FileProcessingStatus.Processed, Boolean bMoveConnectedFiles = true)
        {
            var sTargetFolder = eStatus == FileProcessingStatus.Processed ? JobDirectory_Processed : JobDirectory_Unprocessed;

            //Move primary File
            if (File.Exists(sFile))
                File.Move(sFile, Path.Combine(sTargetFolder, Path.GetFileName(GetTargetFilename(sFile))));

            //Move connected Files
            if (bMoveConnectedFiles)
            {
                var sConnectedFiles = Directory.GetFiles(JobDirectory, $"{Path.GetFileNameWithoutExtension(sFile)}.*", SearchOption.TopDirectoryOnly);
                foreach (var sConnectedFile in sConnectedFiles)
                    File.Move(sConnectedFile, Path.Combine(sTargetFolder, Path.GetFileName(GetTargetFilename(sConnectedFile))));
            }
        }

        private string GetTargetFilename(string sSourceFilename)
        {
            return Path.Combine(Path.GetDirectoryName(sSourceFilename), String.Format("[{0:yyyy-MM-dd HHmmss}] {1}", DateTime.Now, Path.GetFileName(sSourceFilename)));
        }
    }
}
