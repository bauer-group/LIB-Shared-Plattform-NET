using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace BAUERGROUP.Shared.Desktop.Application
{
    public static class DesktopEnvironmentProperties
    {
        public static WindowsIdentity ProcessIdentity
        {
            get
            {
                return WindowsIdentity.GetCurrent();
            }
        }
    }
}
