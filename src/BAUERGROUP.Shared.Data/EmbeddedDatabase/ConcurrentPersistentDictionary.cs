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
        protected String FileName { get; private set; }
        protected String TableName { get; private set; }
        protected SQLiteConnection Connection { get; private set; }

        public ConcurrentPersistentDictionary(String sDataStorageDirectory = null, String sDatabaseName = null, String sTableName = null)
        {
            var sDirectory = String.IsNullOrEmpty(sDataStorageDirectory) ? ApplicationDatabaseFolder : sDataStorageDirectory;
            var sDBName = String.IsNullOrEmpty(sDatabaseName) ? @"Database" : sDatabaseName;
            var sTable = String.IsNullOrEmpty(sTableName) ? sDBName : sTableName;

            FileName = Path.Combine(sDirectory, String.Format("{0}.db", sDBName));
            TableName = sTable;

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

        protected static void CreatePath(String sPath)
        {
            if (!Directory.Exists(sPath))
                Directory.CreateDirectory(sPath);
        }

        public void Create(TKey oKey, TValue oValue)
        {            
            Delete(oKey);
            Connection.Insert(new ConcurrentPersistentDictionaryStorage(TableName, ConvertKey2StorageFormat(oKey), ConvertValue2StorageFormat(oValue)));            
        }

        public void Update(TKey oKey, TValue oValue)
        {            
            Create(oKey, oValue);           
        }

        public void Delete(TKey oKey)
        {
            var sKey = ConvertKey2StorageFormat(oKey);
            var oQuery = Connection.Table<ConcurrentPersistentDictionaryStorage>().Where(p => p.Container == TableName).Where(p => p.Key == sKey);
            foreach (var oItem in oQuery)
                Connection.Delete<ConcurrentPersistentDictionaryStorage>(oItem.ID);
        }

        public TValue Read(TKey oKey)
        {
            try
            {
                var sKey = ConvertKey2StorageFormat(oKey);
                var oResult = Connection.Table<ConcurrentPersistentDictionaryStorage>().Where(p => p.Container == TableName).Where(p => p.Key == sKey).Single();
                
                return JsonSerializer.Deserialize<TValue>(oResult.Value);                
            }
            catch (InvalidOperationException)
            {
                return default(TValue);
            }
        }

        public List<TValue> Read()
        {
            var oResultList = new List<TValue>();

            var oResult = Connection.Table<ConcurrentPersistentDictionaryStorage>().Where(p => p.Container == TableName);

            foreach (var oEntry in oResult)
            {
                oResultList.Add(ConvertValue2RuntimeFormat(oEntry.Value));
            }

            return oResultList;
        }

        public Dictionary<TKey, TValue> ReadWithKeys()
        {
            var oResultDictionary = new Dictionary<TKey, TValue>();
            var oResult = Connection.Table<ConcurrentPersistentDictionaryStorage>().Where(p => p.Container == TableName);

            foreach (var oEntry in oResult)
            {
                oResultDictionary.Add(ConvertKey2RuntimeFormat(oEntry.Key), ConvertValue2RuntimeFormat(oEntry.Value));                
            }
            
            return oResultDictionary;
        }

        public bool Exists(TKey oKey)
        {
            var sKey = ConvertKey2StorageFormat(oKey);
            return Connection.Table<ConcurrentPersistentDictionaryStorage>().Where(p => p.Container == TableName).Where(p => p.Key == sKey).Count() > 0;
        }

        public Int64 Count
        {
            get
            {
                return Connection.Table<ConcurrentPersistentDictionaryStorage>().Where(p => p.Container == TableName).Count();
            }
        }

        private String ConvertKey2StorageFormat(TKey oKey)
        {
            return ObjectHelper.SerializeToJSON<TKey>(oKey);
        }

        private TKey ConvertKey2RuntimeFormat(String sKey)
        {
            return ObjectHelper.DeserializeFromJSON<TKey>(sKey);          
        }

        private String ConvertValue2StorageFormat(TValue oValue)
        {
            return ObjectHelper.SerializeToJSON<TValue>(oValue);            
        }

        private TValue ConvertValue2RuntimeFormat(String sValue)
        {
            return ObjectHelper.DeserializeFromJSON<TValue>(sValue);        
        }
    }
}