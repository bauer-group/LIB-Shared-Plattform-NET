using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;

namespace BAUERGROUP.Shared.Core.Utilities
{
    public class LockingList<T> : IDisposable where T : new()
    {
        private MemoryCache _mc;
        public event EventHandler? Updated;

        public LockingList(String name)
        {
            _mc = new MemoryCache(name);
        }

        public void Dispose()
        {
            _mc.Dispose();
        }

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

        public bool IsExists(String code)
        {
            return _mc.Contains(code);
        }

        public T? Get(String code)
        {
            return ((T?)_mc.Get(code));
        }

        public T? Remove(String code)
        {
            T? result = default;

            if (IsExists(code))
                result = (T?)_mc.Remove(code);

            UpdatedListEvent();
            return result;
        }

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
