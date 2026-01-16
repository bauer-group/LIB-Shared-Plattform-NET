using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAUERGROUP.Shared.Core.Utilities
{
    public class SimpleLockingList: LockingList<DateTime>
    {
        public SimpleLockingList(String sName)
            : base(sName)
        {

        }

        public void Add(String sCode, TimeSpan tsExpiration)
        {
            this.Add(sCode, DateTime.UtcNow.Add(tsExpiration), tsExpiration);
        }

        public DateTime GetUnlockTime(String sCode)
        {
            return this.Get(sCode);
        }
    }
}
