using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAUERGROUP.Shared.Core.Utilities
{
    /// <summary>
    /// Provides a thread-safe, lazy-initialized singleton pattern implementation.
    /// </summary>
    /// <typeparam name="T">The type of the singleton instance. Must be a class with a parameterless constructor.</typeparam>
    public sealed class SimpleSingleton<T> where T : class, new()
    {
        private static readonly Lazy<T> _oSingleObject = new Lazy<T>(() => new T());

        private SimpleSingleton()
        {
        }

        /// <summary>
        /// Gets the singleton instance of type <typeparamref name="T"/>.
        /// </summary>
        public static T Instance
        {
            get
            {
                return _oSingleObject.Value;
            }
        }
    }
}
