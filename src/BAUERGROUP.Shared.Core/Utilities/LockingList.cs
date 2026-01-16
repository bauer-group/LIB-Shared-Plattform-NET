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
        public event EventHandler Updated;

        public LockingList(String sName)
        {
            _mc = new MemoryCache(sName);            
        }
        
        public void Dispose()
        {
            _mc.Dispose();
        }

        public bool Add(String sCode, T oValue, TimeSpan tsExpiration, Boolean bOverwrite = false)
        {
            var bResult = false;

            if (bOverwrite)
            {
                _mc.Set(sCode, oValue, new CacheItemPolicy() { AbsoluteExpiration = DateTime.UtcNow.Add(tsExpiration) });
                bResult = true;
            }
            else
            {
                bResult = _mc.Add(sCode, oValue, new CacheItemPolicy() { AbsoluteExpiration = DateTime.UtcNow.Add(tsExpiration) });
            }

            UpdatedListEvent();
            return bResult;
        }

        public bool IsExists(String sCode)
        {
            return _mc.Contains(sCode);
        }

        public T Get(String sCode)
        {
            return ((T)_mc.Get(sCode));
        }

        public T Remove(String sCode)
        {
            T oResult = default(T);

            if (IsExists(sCode))
                oResult = (T)_mc.Remove(sCode);

            UpdatedListEvent();
            return oResult;
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
