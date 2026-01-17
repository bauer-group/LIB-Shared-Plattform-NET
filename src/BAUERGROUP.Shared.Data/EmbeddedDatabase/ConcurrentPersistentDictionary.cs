using BAUERGROUP.Shared.Core.Application;
using BAUERGROUP.Shared.Data.EmbeddedDatabase.Internal;
using BAUERGROUP.Shared.Core.Extensions;
using System.Text.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BAUERGROUP.Shared.Data.EmbeddedDatabase
{
    public class ConcurrentPersistentDictionary<TKey, TValue> : IDisposable where TKey : IComparable<TKey>
    {
        protected string FileName { get; private set; } = null!;
        protected string TableName { get; private set; } = null!;
        protected SQLiteConnection? Connection { get; private set; }

        public ConcurrentPersistentDictionary(string? dataStorageDirectory = null, string? databaseName = null, string? tableName = null)
        {
            string directory = dataStorageDirectory ?? ApplicationDatabaseFolder;
            string dbName = databaseName ?? "Database";

            FileName = Path.Combine(directory, $"{dbName}.db");
            TableName = tableName ?? dbName;

            lock (ConcurrentPersistentDictionaryShared.GlobalLock)
            {
                Connection = new SQLiteConnection(FileName, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache);
                Connection.CreateTable<ConcurrentPersistentDictionaryStorage>();
            }
        }

        public void Dispose()
        {
            lock (ConcurrentPersistentDictionaryShared.GlobalLock)
            {
                if (Connection == null)
                    return;

                Connection.Close();
                Connection.Dispose();
                Connection = null;
            }
        }

        protected static String ApplicationDatabaseFolder
        {
            get
            {
                return Path.Combine(ApplicationFolders.ExecutionAutomaticApplicationDataFolder, @"Database");
            }
        }

        protected static void CreatePath(String path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }

        public void Create(TKey key, TValue value)
        {
            Delete(key);
            Connection!.Insert(new ConcurrentPersistentDictionaryStorage(TableName, ConvertKey2StorageFormat(key), ConvertValue2StorageFormat(value)));
        }

        public void Update(TKey key, TValue value)
        {
            Create(key, value);
        }

        public void Delete(TKey key)
        {
            var storageKey = ConvertKey2StorageFormat(key);
            var query = Connection!.Table<ConcurrentPersistentDictionaryStorage>().Where(p => p.Container == TableName).Where(p => p.Key == storageKey);
            foreach (var item in query)
                Connection!.Delete<ConcurrentPersistentDictionaryStorage>(item.ID);
        }

        public TValue? Read(TKey key)
        {
            try
            {
                var storageKey = ConvertKey2StorageFormat(key);
                var result = Connection!.Table<ConcurrentPersistentDictionaryStorage>().Where(p => p.Container == TableName).Where(p => p.Key == storageKey).Single();

                return JsonSerializer.Deserialize<TValue>(result.Value);
            }
            catch (InvalidOperationException)
            {
                return default;
            }
        }

        public List<TValue> Read()
        {
            var resultList = new List<TValue>();

            var result = Connection!.Table<ConcurrentPersistentDictionaryStorage>().Where(p => p.Container == TableName);

            foreach (var entry in result)
            {
                resultList.Add(ConvertValue2RuntimeFormat(entry.Value)!);
            }

            return resultList;
        }

        public Dictionary<TKey, TValue> ReadWithKeys()
        {
            var resultDictionary = new Dictionary<TKey, TValue>();
            var result = Connection!.Table<ConcurrentPersistentDictionaryStorage>().Where(p => p.Container == TableName);

            foreach (var entry in result)
            {
                resultDictionary.Add(ConvertKey2RuntimeFormat(entry.Key)!, ConvertValue2RuntimeFormat(entry.Value)!);
            }

            return resultDictionary;
        }

        public bool Exists(TKey key)
        {
            var storageKey = ConvertKey2StorageFormat(key);
            return Connection!.Table<ConcurrentPersistentDictionaryStorage>().Where(p => p.Container == TableName).Where(p => p.Key == storageKey).Count() > 0;
        }

        public Int64 Count
        {
            get
            {
                return Connection!.Table<ConcurrentPersistentDictionaryStorage>().Where(p => p.Container == TableName).Count();
            }
        }

        private String ConvertKey2StorageFormat(TKey key)
        {
            return ObjectHelper.SerializeToJSON<TKey>(key);
        }

        private TKey? ConvertKey2RuntimeFormat(String key)
        {
            return ObjectHelper.DeserializeFromJSON<TKey>(key);
        }

        private String ConvertValue2StorageFormat(TValue value)
        {
            return ObjectHelper.SerializeToJSON<TValue>(value);
        }

        private TValue? ConvertValue2RuntimeFormat(String value)
        {
            return ObjectHelper.DeserializeFromJSON<TValue>(value);
        }
    }
}
