using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;

namespace BAUERGROUP.Shared.Core.Utilities
{
    /// <summary>
    /// A thread-safe cache-backed list with automatic expiration support.
    /// </summary>
    /// <typeparam name="T">The type of items to store in the list.</typeparam>
    public class LockingList<T> : IDisposable where T : new()
    {
        private MemoryCache _mc;

        /// <summary>
        /// Occurs when the list is modified (item added or removed).
        /// </summary>
        public event EventHandler? Updated;

        /// <summary>
        /// Initializes a new instance of the <see cref="LockingList{T}"/> class.
        /// </summary>
        /// <param name="name">The name of the underlying memory cache.</param>
        public LockingList(String name)
        {
            _mc = new MemoryCache(name);
        }

        /// <summary>
        /// Releases all resources used by the <see cref="LockingList{T}"/>.
        /// </summary>
        public void Dispose()
        {
            _mc.Dispose();
        }

        /// <summary>
        /// Adds an item to the list with the specified expiration time.
        /// </summary>
        /// <param name="code">The unique key for the item.</param>
        /// <param name="value">The value to store.</param>
        /// <param name="expiration">The time span after which the item expires.</param>
        /// <param name="overwrite">If true, overwrites an existing item with the same key.</param>
        /// <returns>True if the item was added successfully; false if the key already exists and overwrite is false.</returns>
        public bool Add(String code, T value, TimeSpan expiration, Boolean overwrite = false)
        {
            var result = false;

            if (overwrite)
            {
                _mc.Set(code, value, new CacheItemPolicy() { AbsoluteExpiration = DateTime.UtcNow.Add(expiration) });
                result = true;
            }
            else
            {
                result = _mc.Add(code, value, new CacheItemPolicy() { AbsoluteExpiration = DateTime.UtcNow.Add(expiration) });
            }

            UpdatedListEvent();
            return result;
        }

        /// <summary>
        /// Determines whether an item with the specified key exists in the list.
        /// </summary>
        /// <param name="code">The key to check.</param>
        /// <returns>True if the key exists; otherwise, false.</returns>
        public bool IsExists(String code)
        {
            return _mc.Contains(code);
        }

        /// <summary>
        /// Retrieves an item from the list by its key.
        /// </summary>
        /// <param name="code">The key of the item to retrieve.</param>
        /// <returns>The item if found; otherwise, the default value for type <typeparamref name="T"/>.</returns>
        public T? Get(String code)
        {
            return ((T?)_mc.Get(code));
        }

        /// <summary>
        /// Removes an item from the list by its key.
        /// </summary>
        /// <param name="code">The key of the item to remove.</param>
        /// <returns>The removed item if found; otherwise, the default value for type <typeparamref name="T"/>.</returns>
        public T? Remove(String code)
        {
            T? result = default;

            if (IsExists(code))
                result = (T?)_mc.Remove(code);

            UpdatedListEvent();
            return result;
        }

        /// <summary>
        /// Gets the number of items currently in the list.
        /// </summary>
        public Int64 Count
        {
            get
            {
                return _mc.GetCount();
            }
        }

        private void UpdatedListEvent()
        {
            if (Updated == null)
                return;

            Updated(this, new EventArgs());
        }
    }
}
