using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace BAUERGROUP.Shared.Core.Files
{
    public class CSVFileWriter : IDisposable
    {
        protected StreamWriter? _sw;
        protected CsvWriter? _cw;

        public CSVFileWriter(String fileName, Encoding? encoding = null, Boolean appendFile = false, String delimiter = @";", Char quote = '"', Boolean quoteAllFields = true, CultureInfo? culture = null, Boolean hasHeader = true)
        {
            _sw = new StreamWriter(fileName, appendFile, encoding == null ? Encoding.Unicode : encoding);
            _cw = new CsvWriter(_sw, new CsvConfiguration(culture == null ? CultureInfo.CurrentCulture : culture)
            {
                HasHeaderRecord = hasHeader,
                Comment = '#',
                AllowComments = false,
                Delimiter = delimiter,
                Encoding = encoding == null ? Encoding.Unicode : encoding,
                Quote = quote,
                ShouldQuote = (q) => quoteAllFields,
                ShouldSkipRecord = (record) => record.Row.Parser.Record?.All(String.IsNullOrWhiteSpace) ?? false,
                TrimOptions = TrimOptions.Trim | TrimOptions.InsideQuotes,
                IgnoreBlankLines = true,
            });
        }

        public void Dispose()
        {
            if (_sw == null)
                return;

            _sw.Close();

            _cw?.Dispose();
            _sw.Dispose();

            _cw = null;
            _sw = null;
        }

        public void Close()
        {
            Dispose();
        }

        public void WriteHeader<T>()
        {
            _cw?.WriteHeader<T>();
        }

        public void WriteRecord<T>(T entry)
        {
            _cw?.WriteRecord(entry);
        }

        public void WriteRecords(IEnumerable entries)
        {
            _cw?.WriteRecords(entries);
        }

        public void WriteField<T>(T entry)
        {
            _cw?.WriteField(entry);
        }

        public void NextRecord()
        {
            _cw?.NextRecord();
        }
    }
}
