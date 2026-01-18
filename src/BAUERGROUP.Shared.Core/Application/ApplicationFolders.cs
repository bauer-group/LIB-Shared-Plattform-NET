using System;
using System.IO;

namespace BAUERGROUP.Shared.Core.Application
{
    /// <summary>
    /// Provides access to common application folder paths.
    /// </summary>
    public static class ApplicationFolders
    {
        /// <summary>
        /// Gets the roaming application data folder path for the current user.
        /// </summary>
        public static String ApplicationData
        {
            get
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            }
        }

        /// <summary>
        /// Gets the directory containing the application binary.
        /// </summary>
        public static String? ApplicationBinary
        {
            get
            {
                return Path.GetDirectoryName(ApplicationProperties.AutomaticAssembly.Location);
            }
        }

        /// <summary>
        /// Gets the application file name without the extension.
        /// </summary>
        public static String ApplicationFileNameWithoutExtension
        {
            get
            {
                return Path.GetFileNameWithoutExtension(ApplicationProperties.AutomaticAssembly.Location);
            }
        }

        /// <summary>
        /// Gets the application-specific folder within the roaming application data directory.
        /// </summary>
        public static String ExecutionApplicationDataFolder
        {
            get
            {
                return Path.Combine(ApplicationData, ApplicationFileNameWithoutExtension);
            }
        }

        /// <summary>
        /// Gets the common application data folder path shared by all users.
        /// </summary>
        public static String CommonApplicationData
        {
            get
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            }
        }

        /// <summary>
        /// Gets the application-specific folder within the common application data directory.
        /// </summary>
        public static String ExecutionCommonApplicationDataFolder
        {
            get
            {
                return Path.Combine(CommonApplicationData, ApplicationFileNameWithoutExtension);
            }
        }

        /// <summary>
        /// Gets the directory containing the currently executing assembly.
        /// </summary>
        public static String? ApplicationExecuting
        {
            get
            {
                return Path.GetDirectoryName(ApplicationProperties.ExecutingAssembly.Location);
            }
        }

        /// <summary>
        /// Gets the local (non-roaming) application data folder path for the current user.
        /// </summary>
        public static String LocalApplicationData
        {
            get
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            }
        }

        /// <summary>
        /// Gets the current working directory of the application.
        /// </summary>
        public static String CurrentDirectory
        {
            get
            {
                return Environment.CurrentDirectory;
            }
        }

        /// <summary>
        /// Gets the name of the environment variable that forces data storage in the user's roaming profile.
        /// </summary>
        /// <remarks>
        /// When this environment variable is set to "TRUE", application data is stored in the roaming profile.
        /// </remarks>
        public static String ForceApplicationDataFolderInUserProfileEnvironmentVariable
        {
            get
            {
                return @"BAUERGROUP_ROAMINGAPPLICATIONDATA"; //Environment Variable must be Present and be TRUE
            }
        }

        /// <summary>
        /// Gets the appropriate application data folder based on the BAUERGROUP_ROAMINGAPPLICATIONDATA environment variable.
        /// </summary>
        /// <remarks>
        /// Returns <see cref="ExecutionApplicationDataFolder"/> if the environment variable is set to "TRUE",
        /// otherwise returns <see cref="ExecutionCommonApplicationDataFolder"/>.
        /// </remarks>
        public static String ExecutionAutomaticApplicationDataFolder
        {
            get
            {
                var dataFolderControl = EnvironmentProperties.GetEnvironmentVariable(ForceApplicationDataFolderInUserProfileEnvironmentVariable);

                if (!String.IsNullOrEmpty(dataFolderControl) && dataFolderControl?.ToUpper() == "TRUE")
                    return ExecutionApplicationDataFolder; //Variable for Storing Configuration in Users Roaming Profile is present

                return ExecutionCommonApplicationDataFolder; //Variable for Storing Configuration in Users Roaming Profile is absent
            }
        }

        /// <summary>
        /// Gets the user's personal documents directory (My Documents).
        /// </summary>
        public static String UserPersonalDocumentsDirectory
        {
            get
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            }
        }
    }
}
