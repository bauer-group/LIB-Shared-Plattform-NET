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
    /// <summary>
    /// A thread-safe, persistent key-value dictionary backed by SQLite storage.
    /// </summary>
    /// <typeparam name="TKey">The type of keys in the dictionary. Must implement <see cref="IComparable{TKey}"/>.</typeparam>
    /// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
    public class ConcurrentPersistentDictionary<TKey, TValue> : IDisposable where TKey : IComparable<TKey>
    {
        /// <summary>
        /// Gets the file path of the SQLite database.
        /// </summary>
        protected string FileName { get; private set; } = null!;

        /// <summary>
        /// Gets the name of the table within the database.
        /// </summary>
        protected string TableName { get; private set; } = null!;

        /// <summary>
        /// Gets the SQLite database connection.
        /// </summary>
        protected SQLiteConnection? Connection { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConcurrentPersistentDictionary{TKey, TValue}"/> class.
        /// </summary>
        /// <param name="dataStorageDirectory">Optional directory for the database file. Defaults to the application database folder.</param>
        /// <param name="databaseName">Optional name for the database file. Defaults to "Database".</param>
        /// <param name="tableName">Optional name for the table. Defaults to the database name.</param>
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

        /// <summary>
        /// Releases all resources used by the <see cref="ConcurrentPersistentDictionary{TKey, TValue}"/>.
        /// </summary>
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

        /// <summary>
        /// Creates or overwrites an entry with the specified key and value.
        /// </summary>
        /// <param name="key">The key of the entry.</param>
        /// <param name="value">The value to store.</param>
        public void Create(TKey key, TValue value)
        {
            Delete(key);
            Connection!.Insert(new ConcurrentPersistentDictionaryStorage(TableName, ConvertKey2StorageFormat(key), ConvertValue2StorageFormat(value)));
        }

        /// <summary>
        /// Updates an existing entry or creates a new one with the specified key and value.
        /// </summary>
        /// <param name="key">The key of the entry.</param>
        /// <param name="value">The value to store.</param>
        public void Update(TKey key, TValue value)
        {
            Create(key, value);
        }

        /// <summary>
        /// Deletes the entry with the specified key.
        /// </summary>
        /// <param name="key">The key of the entry to delete.</param>
        public void Delete(TKey key)
        {
            var storageKey = ConvertKey2StorageFormat(key);
            var query = Connection!.Table<ConcurrentPersistentDictionaryStorage>().Where(p => p.Container == TableName).Where(p => p.Key == storageKey);
            foreach (var item in query)
                Connection!.Delete<ConcurrentPersistentDictionaryStorage>(item.ID);
        }

        /// <summary>
        /// Reads the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key to look up.</param>
        /// <returns>The value if found; otherwise, the default value for <typeparamref name="TValue"/>.</returns>
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

        /// <summary>
        /// Reads all values in the dictionary.
        /// </summary>
        /// <returns>A list of all values.</returns>
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

        /// <summary>
        /// Reads all entries as a dictionary of key-value pairs.
        /// </summary>
        /// <returns>A dictionary containing all key-value pairs.</returns>
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

        /// <summary>
        /// Determines whether the dictionary contains an entry with the specified key.
        /// </summary>
        /// <param name="key">The key to check.</param>
        /// <returns>True if the key exists; otherwise, false.</returns>
        public bool Exists(TKey key)
        {
            var storageKey = ConvertKey2StorageFormat(key);
            return Connection!.Table<ConcurrentPersistentDictionaryStorage>().Where(p => p.Container == TableName).Where(p => p.Key == storageKey).Count() > 0;
        }

        /// <summary>
        /// Gets the number of entries in the dictionary.
        /// </summary>
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
