using System;
using System.IO;

namespace BAUERGROUP.Shared.Core.Application
{
    public static class ApplicationFolders
    {
        public static String ApplicationData
        {
            get
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            }
        }

        public static String ApplicationBinary
        {
            get
            {
                return Path.GetDirectoryName(ApplicationProperties.AutomaticAssembly.Location);
            }
        }

        public static String ApplicationFileNameWithoutExtension
        {
            get
            {
                return Path.GetFileNameWithoutExtension(ApplicationProperties.AutomaticAssembly.Location);
            }
        }

        public static String ExecutionApplicationDataFolder
        {
            get
            {
                return Path.Combine(ApplicationData, ApplicationFileNameWithoutExtension);
            }
        }

        public static String CommonApplicationData
        {
            get
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            }
        }

        public static String ExecutionCommonApplicationDataFolder
        {
            get
            {
                return Path.Combine(CommonApplicationData, ApplicationFileNameWithoutExtension);
            }
        }

        public static String ApplicationExecuting
        {
            get
            {
                return Path.GetDirectoryName(ApplicationProperties.ExecutingAssembly.Location);
            }
        }

        public static String LocalApplicationData
        {
            get
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            }
        }

        public static String CurrentDirectory
        {
            get
            {
                return Environment.CurrentDirectory;
            }
        }

        public static String ForceApplicationDataFolderInUserProfileEnvironmentVariable
        {
            get
            {
                return @"BAUERGROUP_ROAMINGAPPLICATIONDATA"; //Environment Variable must be Present and be TRUE
            }
        }

        public static String ExecutionAutomaticApplicationDataFolder
        {
            get
            {
                var sDataFolderControl = EnvironmentProperties.GetEnvironmentVariable(ForceApplicationDataFolderInUserProfileEnvironmentVariable);

                if (!String.IsNullOrEmpty(sDataFolderControl) && sDataFolderControl.ToUpper() == "TRUE")
                    return ExecutionApplicationDataFolder; //Variable for Storing Configuration in Users Roaming Profile is present

                return ExecutionCommonApplicationDataFolder; //Variable for Storing Configuration in Users Roaming Profile is absent
            }
        }

        public static String UserPersonalDocumentsDirectory
        {
            get
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            }
        }
    }
}
