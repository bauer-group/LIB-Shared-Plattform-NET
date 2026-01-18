using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace BAUERGROUP.Shared.Desktop.Application
{
    /// <summary>
    /// Provides Windows-specific environment properties for desktop applications.
    /// </summary>
    public static class DesktopEnvironmentProperties
    {
        /// <summary>
        /// Gets the Windows identity of the current process.
        /// </summary>
        public static WindowsIdentity ProcessIdentity
        {
            get
            {
                return WindowsIdentity.GetCurrent();
            }
        }
    }
}
