using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BAUERGROUP.Shared.Core.DataManagement
{
    public class FilteredStringCollector
    {
        private HashSet<String> _RecordsRead;

        private String FilterRegExPattern { get; set; }

        public FilteredStringCollector(String sFilterRegEx = null)
        {
            _RecordsRead = new HashSet<String>();
            FilterRegExPattern = sFilterRegEx;
        }

        public Boolean Add(String sEntry)
        {
            //Check for Empty Data
            if (String.IsNullOrWhiteSpace(sEntry))
                return false;

            //Do Processing of Data
            _RecordsRead.Add(sEntry);

            //Fire Event
            var bIsMatch = FilterRegExPattern == null ? true : Regex.IsMatch(sEntry, FilterRegExPattern);
            OnRecordAdded(sEntry, bIsMatch);

            return true;
        }

        public Int32 AllRecordsCount => _RecordsRead.Count;
        public IEnumerable<String> AllRecords => _RecordsRead;
        public String FirstRecord => AllRecords.FirstOrDefault();
        public String LastRecord => AllRecords.LastOrDefault();

        public Int32 MatchingRecordsCount => (FilterRegExPattern == null) ? _RecordsRead.Count() : _RecordsRead.Count(p => Regex.IsMatch(p, FilterRegExPattern));
        public IEnumerable<String> MatchingRecords => (FilterRegExPattern == null) ? _RecordsRead : _RecordsRead.Where(p => Regex.IsMatch(p, FilterRegExPattern));
        public String FirstMatchingRecord => MatchingRecords.FirstOrDefault();
        public String LastMatchingRecord => MatchingRecords.LastOrDefault();

        public Boolean Contains(String sEntry)
        {
            return _RecordsRead.Contains(sEntry);
        }

        public void Clear()
        {
            _RecordsRead.Clear();
            _RecordsRead.TrimExcess();
        }

        public event EventHandler<FilteredStringCollectorEventArgs> RecordAdded;

        protected void OnRecordAdded(FilteredStringCollectorEventArgs eventArgs) => RecordAdded?.Invoke(this, eventArgs);

        protected void OnRecordAdded(String sEntry, Boolean bMatch) => RecordAdded?.Invoke(this, new FilteredStringCollectorEventArgs(sEntry, bMatch));
    }
}
