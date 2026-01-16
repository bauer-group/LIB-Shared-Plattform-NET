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
        protected StreamReader _sr;
        protected CsvReader _cr;

        public CSVFileReader(String sFileName, Encoding? oEncoding = null, String sDelimiter = @";", Char sQuote = '"', Boolean bQuoteAllFields = true, CultureInfo? oCulture = null, Boolean bHasHeader = true)
        {
            _sr = new StreamReader(sFileName, oEncoding == null ? Encoding.Unicode : oEncoding, true);
            _cr = new CsvReader(_sr, new CsvConfiguration(oCulture == null ? CultureInfo.CurrentCulture : oCulture)
            {
                HasHeaderRecord = bHasHeader,
                Comment = '#',
                AllowComments = true,
                Delimiter = sDelimiter,
                Encoding = oEncoding == null ? Encoding.Unicode : oEncoding,
                Quote = sQuote,
                ShouldQuote = (quote) => bQuoteAllFields,
                ShouldSkipRecord = (record) => record.Row.Parser.Record.All(String.IsNullOrWhiteSpace),
                TrimOptions = TrimOptions.Trim | TrimOptions.InsideQuotes,
                IgnoreBlankLines = true,                
            });
        }
        
        public void Dispose()
        {
            if (_sr == null)
                return;

            _sr.Close();

            _cr.Dispose();
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
            return _cr.GetRecords<T>();            
        }
    }
}
