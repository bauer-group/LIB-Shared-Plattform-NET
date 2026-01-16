using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAUERGROUP.Shared.Core.Utilities
{
    public sealed class SimpleSingleton<T> where T : class, new()
    {
        private static readonly Lazy<T> _oSingleObject = new Lazy<T>(() => new T());

        private SimpleSingleton()
        {
        }
        
        public static T Instance
        {
            get
            {
                return _oSingleObject.Value;
            }
        }
    }
}
