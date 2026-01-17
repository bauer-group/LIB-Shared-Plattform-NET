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
        IEnumerable<T> Load<T>(Expression<Func<T, Boolean>> filter, Int32 skip = 0, Int32 limit = Int32.MaxValue);
        T Load<T>(T value) where T : class, IApplicationDataStoreEntry;
        void Save<T>(T value) where T : class, IApplicationDataStoreEntry;

        void Delete<T>() where T : class, IApplicationDataStoreEntry;
        void Delete<T>(Expression<Func<T, Boolean>> filter);
        void Delete<T>(T value) where T : class, IApplicationDataStoreEntry;

        void Optimize();
    }
}
