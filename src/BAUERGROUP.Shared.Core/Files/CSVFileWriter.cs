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
        protected StreamWriter _sw;
        protected CsvWriter _cw;

        public CSVFileWriter(String sFileName, Encoding oEncoding = null, Boolean bAppendFile = false, String sDelimiter = @";", Char sQuote = '"', Boolean bQuoteAllFields = true, CultureInfo oCulture = null, Boolean bHasHeader = true)
        {
            _sw = new StreamWriter(sFileName, bAppendFile, oEncoding == null ? Encoding.Unicode : oEncoding);
            _cw = new CsvWriter(_sw, new CsvConfiguration(oCulture == null ? CultureInfo.CurrentCulture : oCulture)
            {
                HasHeaderRecord = bHasHeader,                
                Comment = '#',
                AllowComments = false,
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
            if (_sw == null)
                return;

            _sw.Close();

            _cw.Dispose();
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
            _cw.WriteHeader<T>();
        }

        public void WriteRecord<T>(T oEntry)
        {
            _cw.WriteRecord(oEntry);
        }
        
        public void WriteRecords(IEnumerable oEntries)
        {
            _cw.WriteRecords(oEntries);
        }

        public void WriteField<T>(T oEnty)
        {
            _cw.WriteField(oEnty);
        }

        public void NextRecord()
        {
            _cw.NextRecord();
        }
    }
}
