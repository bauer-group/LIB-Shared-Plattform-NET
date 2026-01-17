using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAUERGROUP.Shared.Core.Utilities
{
    public class SimpleLockingList: LockingList<DateTime>
    {
        public SimpleLockingList(String name)
            : base(name)
        {

        }

        public void Add(String code, TimeSpan expiration)
        {
            this.Add(code, DateTime.UtcNow.Add(expiration), expiration, true);
        }

        public DateTime GetUnlockTime(String code)
        {
            return this.Get(code)!;
        }
    }
}
