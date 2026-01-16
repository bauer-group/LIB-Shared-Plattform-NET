using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAUERGROUP.Shared.Core.Application.Data
{
    public interface IApplicationConfiguration
    {
        Guid UID { get; set; }
        DateTime Timestamp { get; set; }
    }
}
