using System;
using System.Collections.Generic;
using System.Text;

namespace BAUERGROUP.Shared.Core.Application
{
    /// <summary>
    /// Specifies the type of user session the application is running in.
    /// </summary>
    public enum EnvironmentSessionType
    {
        /// <summary>
        /// The session type could not be determined.
        /// </summary>
        Undetermined = 0,

        /// <summary>
        /// A local console session.
        /// </summary>
        Console = 1,

        /// <summary>
        /// A Citrix ICA (Independent Computing Architecture) remote session.
        /// </summary>
        ICA = 10,

        /// <summary>
        /// A Microsoft Remote Desktop Protocol (RDP) session.
        /// </summary>
        RDP = 11
    }
}
