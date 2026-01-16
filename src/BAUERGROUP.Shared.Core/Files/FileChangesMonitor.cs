using BAUERGROUP.Shared.Core.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BAUERGROUP.Shared.Core.Files
{
    public class FileChangesMonitor : PropertyChangedBase, IDisposable
    {
        protected FileSystemWatcher _fsw;

        public FileChangesMonitor(String sPath, String sFilter = @"*.*", Boolean bIncludeSubdirectories = false)
        {
            _fsw = new FileSystemWatcher(sPath, sFilter);
            _fsw.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size;
            _fsw.IncludeSubdirectories = bIncludeSubdirectories;
            _fsw.Changed += OnFileChanged;
            _fsw.EnableRaisingEvents = true;
        }

        public FileChangesMonitor(String sFileName):
            this(Path.GetDirectoryName(sFileName), Path.GetFileName(sFileName))
        {

        }

        private void OnFileChanged(Object sender, FileSystemEventArgs e)
        {
            _fsw.EnableRaisingEvents = false;
            OnPropertyChanged(e.FullPath);
            _fsw.EnableRaisingEvents = true;
        }

        public void Dispose()
        {
            _fsw.Dispose();
        }
    }
}
