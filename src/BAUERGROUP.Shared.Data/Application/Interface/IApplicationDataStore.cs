using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using BAUERGROUP.Shared.Data.Application.Data;
using BAUERGROUP.Shared.Data.Application.Interface;

namespace BAUERGROUP.Shared.Data.Application.Interface
{
    /// <summary>
    /// Defines the contract for an application data store that supports CRUD operations.
    /// </summary>
    public interface IApplicationDataStore
    {
        /// <summary>
        /// Loads all entries of the specified type.
        /// </summary>
        /// <typeparam name="T">The type of entries to load.</typeparam>
        /// <returns>An enumerable of all entries.</returns>
        IEnumerable<T> Load<T>();

        /// <summary>
        /// Loads entries matching the specified filter with optional pagination.
        /// </summary>
        /// <typeparam name="T">The type of entries to load.</typeparam>
        /// <param name="filter">The filter expression.</param>
        /// <param name="skip">Number of entries to skip.</param>
        /// <param name="limit">Maximum number of entries to return.</param>
        /// <returns>An enumerable of matching entries.</returns>
        IEnumerable<T> Load<T>(Expression<Func<T, Boolean>> filter, Int32 skip = 0, Int32 limit = Int32.MaxValue);

        /// <summary>
        /// Loads a specific entry by its identifier.
        /// </summary>
        /// <typeparam name="T">The type of entry to load.</typeparam>
        /// <param name="value">The entry with the UID to look up.</param>
        /// <returns>The matching entry.</returns>
        T Load<T>(T value) where T : class, IApplicationDataStoreEntry;

        /// <summary>
        /// Saves an entry to the data store (insert or update).
        /// </summary>
        /// <typeparam name="T">The type of entry to save.</typeparam>
        /// <param name="value">The entry to save.</param>
        void Save<T>(T value) where T : class, IApplicationDataStoreEntry;

        /// <summary>
        /// Deletes all entries of the specified type.
        /// </summary>
        /// <typeparam name="T">The type of entries to delete.</typeparam>
        void Delete<T>() where T : class, IApplicationDataStoreEntry;

        /// <summary>
        /// Deletes entries matching the specified filter.
        /// </summary>
        /// <typeparam name="T">The type of entries to delete.</typeparam>
        /// <param name="filter">The filter expression.</param>
        void Delete<T>(Expression<Func<T, Boolean>> filter);

        /// <summary>
        /// Deletes a specific entry from the data store.
        /// </summary>
        /// <typeparam name="T">The type of entry to delete.</typeparam>
        /// <param name="value">The entry to delete.</param>
        void Delete<T>(T value) where T : class, IApplicationDataStoreEntry;

        /// <summary>
        /// Optimizes the data store (e.g., compacting, indexing).
        /// </summary>
        void Optimize();
    }
}
