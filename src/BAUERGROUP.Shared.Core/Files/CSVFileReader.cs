using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace BAUERGROUP.Shared.Core.Files
{
    public class CSVFileReader : IDisposable
    {
        protected StreamReader? _sr;
        protected CsvReader? _cr;

        public CSVFileReader(String fileName, Encoding? encoding = null, String delimiter = @";", Char quote = '"', Boolean quoteAllFields = true, CultureInfo? culture = null, Boolean hasHeader = true)
        {
            _sr = new StreamReader(fileName, encoding == null ? Encoding.Unicode : encoding, true);
            _cr = new CsvReader(_sr, new CsvConfiguration(culture == null ? CultureInfo.CurrentCulture : culture)
            {
                HasHeaderRecord = hasHeader,
                Comment = '#',
                AllowComments = true,
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
            if (_sr == null)
                return;

            _sr.Close();

            _cr?.Dispose();
            _sr.Dispose();

            _cr = null;
            _sr = null;
        }

        public void Close()
        {
            Dispose();
        }

        public IEnumerable<T> ReadRecords<T>()
        {
            return _cr?.GetRecords<T>() ?? Enumerable.Empty<T>();
        }
    }
}
