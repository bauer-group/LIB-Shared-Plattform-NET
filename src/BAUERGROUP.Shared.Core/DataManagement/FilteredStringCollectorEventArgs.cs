using System;
using System.Collections.Generic;
using System.Text;

namespace BAUERGROUP.Shared.Core.DataManagement
{
    public class FilteredStringCollectorEventArgs : EventArgs
    {
        public FilteredStringCollectorEventArgs(String sEntry, Boolean bMatch)
        {
            Entry = sEntry;
            Match = bMatch;
        }

        public String Entry { get; set; }
        public Boolean Match { get; set; }
    }
}
