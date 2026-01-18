using System;
using System.Collections.Generic;
using System.Text;

namespace BAUERGROUP.Shared.Core.Application
{
    /// <summary>
    /// Provides access to environment and system information.
    /// </summary>
    public static class EnvironmentProperties
    {
        /// <summary>
        /// Gets the NetBIOS name of the local computer.
        /// </summary>
        public static String ClientName { get { return Environment.MachineName; } }

        /// <summary>
        /// Gets the user name of the currently logged-in user.
        /// </summary>
        public static String UserName { get { return Environment.UserName; } }

        /// <summary>
        /// Gets the network domain name associated with the current user.
        /// </summary>
        public static String UserDomainName { get { return Environment.UserDomainName; } }

        /// <summary>
        /// Gets the full user name in DOMAIN\Username format.
        /// </summary>
        public static String FullUserName { get { return String.Format(@"{0}\{1}", UserDomainName, UserName); } }

        /// <summary>
        /// Gets a value indicating whether the current process is running in user interactive mode.
        /// </summary>
        public static Boolean UserIsInteractive { get { return Environment.UserInteractive; } }

        /// <summary>
        /// Gets the current operating system information.
        /// </summary>
        public static OperatingSystem OperatingSystem { get { return Environment.OSVersion; } }

        /// <summary>
        /// Gets a value indicating whether the current operating system is 64-bit.
        /// </summary>
        public static Boolean Is64BitOperatingSystem { get { return Environment.Is64BitOperatingSystem; } }

        /// <summary>
        /// Gets the number of processors available to the current process.
        /// </summary>
        public static Int32 ProcessorCount { get { return Environment.ProcessorCount; } }

        /// <summary>
        /// Gets the fully qualified path of the system directory.
        /// </summary>
        public static String SystemDirectory { get { return Environment.SystemDirectory; } }

        /// <summary>
        /// Retrieves the value of an environment variable.
        /// </summary>
        /// <param name="name">The name of the environment variable.</param>
        /// <returns>The value of the environment variable, or null if not found.</returns>
        public static String? GetEnvironmentVariable(String name)
        {
            //Returns NULL if not found
            return Environment.GetEnvironmentVariable(name);
        }

        /// <summary>
        /// Gets the type of session the application is running in (Console, RDP, ICA, or Undetermined).
        /// </summary>
        public static EnvironmentSessionType SessionType
        {
            get
            {
                var sessionName = GetEnvironmentVariable("SESSIONNAME");
                if (sessionName == null || sessionName.Length < 3)
                    return EnvironmentSessionType.Undetermined;

                switch (sessionName.ToUpper().Substring(0, 3))
                {
                    case "CON":
                        return EnvironmentSessionType.Console;

                    case "RDP":
                        return EnvironmentSessionType.RDP;

                    case "ICA":
                        return EnvironmentSessionType.ICA;

                    default:
                        return EnvironmentSessionType.Undetermined;
                }
            }
        }
    }
}
