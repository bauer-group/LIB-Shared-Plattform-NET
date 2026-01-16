using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using BAUERGROUP.Shared.Data.Application.Data;
using BAUERGROUP.Shared.Data.Application.Interface;

namespace BAUERGROUP.Shared.Data.Application.Interface
{
    public interface IApplicationDataStore
    {
        IEnumerable<T> Load<T>();
        IEnumerable<T> Load<T>(Expression<Func<T, Boolean>> oFilter, Int32 iSkip = 0, Int32 iLimit = Int32.MaxValue);
        T Load<T>(T oValue) where T : class, IApplicationDataStoreEntry;
        void Save<T>(T oValue) where T : class, IApplicationDataStoreEntry;

        void Delete<T>() where T : class, IApplicationDataStoreEntry;
        void Delete<T>(Expression<Func<T, Boolean>> oFilter);
        void Delete<T>(T oValue) where T : class, IApplicationDataStoreEntry;

        void Optimize();
    }
}