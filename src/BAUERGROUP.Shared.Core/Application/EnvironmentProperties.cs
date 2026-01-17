using System;
using System.Collections.Generic;
using System.Text;

namespace BAUERGROUP.Shared.Core.Application
{
    public static class EnvironmentProperties
    {
        public static String ClientName { get { return Environment.MachineName; } }

        public static String UserName { get { return Environment.UserName; } }
        public static String UserDomainName { get { return Environment.UserDomainName; } }
        public static String FullUserName { get { return String.Format(@"{0}\{1}", UserDomainName, UserName); } }

        public static Boolean UserIsInteractive { get { return Environment.UserInteractive; } }

        public static OperatingSystem OperatingSystem { get { return Environment.OSVersion; } }
        public static Boolean Is64BitOperatingSystem { get { return Environment.Is64BitOperatingSystem; } }

        public static Int32 ProcessorCount { get { return Environment.ProcessorCount; } }
        public static String SystemDirectory { get { return Environment.SystemDirectory; } }

        public static String? GetEnvironmentVariable(String name)
        {
            //Returns NULL if not found
            return Environment.GetEnvironmentVariable(name);
        }

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
