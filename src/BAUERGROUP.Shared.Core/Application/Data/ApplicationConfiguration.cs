using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAUERGROUP.Shared.Core.Application.Data
{
    [Serializable]
    public class ApplicationConfiguration : IApplicationConfiguration
    {
        protected ApplicationConfiguration()
        {
            UID = Guid.Empty;
            Timestamp = DateTime.UtcNow;
        }

        public Guid UID { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
