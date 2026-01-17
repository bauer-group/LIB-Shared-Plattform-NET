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

        public FileChangesMonitor(String path, String filter = @"*.*", Boolean includeSubdirectories = false)
        {
            _fsw = new FileSystemWatcher(path, filter);
            _fsw.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size;
            _fsw.IncludeSubdirectories = includeSubdirectories;
            _fsw.Changed += OnFileChanged;
            _fsw.EnableRaisingEvents = true;
        }

        public FileChangesMonitor(String fileName):
            this(Path.GetDirectoryName(fileName) ?? ".", Path.GetFileName(fileName))
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
