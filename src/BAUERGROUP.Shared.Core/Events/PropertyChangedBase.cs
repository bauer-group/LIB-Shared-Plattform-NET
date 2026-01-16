using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace BAUERGROUP.Shared.Core.Events
{
    public class PropertyChangedBase : INotifyPropertyChanged
    {
        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        
        protected void OnPropertyChanged(String sName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(sName));
        #endregion
    }
}
