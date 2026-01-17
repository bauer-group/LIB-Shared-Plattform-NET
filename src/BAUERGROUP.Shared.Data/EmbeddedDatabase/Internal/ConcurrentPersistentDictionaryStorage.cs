using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAUERGROUP.Shared.Data.EmbeddedDatabase.Internal
{
    [Serializable]
    [Table("PersistentDictionary")]
    internal class ConcurrentPersistentDictionaryStorage
    {
        public ConcurrentPersistentDictionaryStorage() :
            this (@"", @"", @"")
        {

        }

        public ConcurrentPersistentDictionaryStorage(String container, String key, String value)
        {
            Container = container;
            Key = key;
            Value = value;

            Timestamp = DateTime.UtcNow;
        }

        [PrimaryKey, AutoIncrement]
        public Int64 ID { get; set; }

        [Indexed, MaxLength(256)]
        public String Container { get; set; }

        [Indexed, MaxLength(8192)]
        public String Key { get; set; }

        [MaxLength(2147483647)]
        public String Value { get; set; }

        [Indexed]
        public DateTime Timestamp { get; set; }
    }
}
