using Microsoft.Win32;
using System;
using System.Collections;
using System.Data.Odbc;
using System.Linq;
using System.Text;

namespace BAUERGROUP.Shared.Desktop.DataSources
{
    /// <summary>
    /// Provides utilities for managing ODBC data source connections on Windows.
    /// </summary>
    public class OdbcConnectionManager
    {
        /// <summary>
        /// Specifies the type of ODBC data source.
        /// </summary>
        public enum DataSourceType
        {
            /// <summary>
            /// System DSN, available to all users on the machine.
            /// </summary>
            System = 0,

            /// <summary>
            /// User DSN, available only to the current user.
            /// </summary>
            User = 1
        }

        /// <summary>
        /// Gets all ODBC data source names (both User and System DSNs).
        /// </summary>
        /// <returns>A sorted list of DSN names and their types.</returns>
        public static SortedList GetDataSourceNames()
        {
            SortedList userDSNList = GetDataSourceNames(DataSourceType.User);
            SortedList systemDSNList = GetDataSourceNames(DataSourceType.System);

            for (int i = 0; i < systemDSNList.Count; i++)
            {
                var dsnName = systemDSNList.GetKey(i) as String;
                var dsnTypeValue = systemDSNList.GetByIndex(i);

                if (dsnName == null || dsnTypeValue == null)
                    continue;

                DataSourceType dsnType = (DataSourceType)dsnTypeValue;

                try
                {
                    userDSNList.Add(dsnName, dsnType);
                }
                catch
                {
                    // An exception can be thrown if the key being added is a duplicate. So we just catch it here and have to ignore it.
                }
            }

            return userDSNList;
        }

        /// <summary>
        /// Gets ODBC data source names of the specified type.
        /// </summary>
        /// <param name="type">The type of DSN to retrieve (User or System).</param>
        /// <returns>A sorted list of DSN names and their types.</returns>
        public static SortedList GetDataSourceNames(DataSourceType type)
        {
            SortedList dsnList = new SortedList();

            var softwareKey = (type == DataSourceType.System ? Registry.LocalMachine : Registry.CurrentUser).OpenSubKey("Software");

            if (softwareKey != null)
            {
                var odbcKey = softwareKey.OpenSubKey("ODBC");

                if (odbcKey != null)
                {
                    var odbcIniKey = odbcKey.OpenSubKey("ODBC.INI");

                    if (odbcIniKey != null)
                    {
                        var odbcDsnKey = odbcIniKey.OpenSubKey("ODBC Data Sources");

                        if (odbcDsnKey != null)
                        {
                            foreach (string name in odbcDsnKey.GetValueNames())
                                dsnList.Add(name, type);

                            odbcDsnKey.Close();
                        }

                        odbcIniKey.Close();
                    }

                    odbcKey.Close();
                }

                softwareKey.Close();
            }

            return dsnList;
        }

        /// <summary>
        /// Builds an ODBC connection string for the specified DSN with credentials.
        /// </summary>
        /// <param name="dsn">The data source name.</param>
        /// <param name="username">The username for authentication.</param>
        /// <param name="password">The password for authentication.</param>
        /// <returns>A formatted ODBC connection string.</returns>
        public static String GetConnectionString(String dsn, String username, String password)
        {
            var odbcConnection = new OdbcConnectionStringBuilder();

            odbcConnection.Add(@"DSN", dsn);
            odbcConnection.Add(@"UID", username);
            odbcConnection.Add(@"PWD", password);

            return odbcConnection.ConnectionString;
        }
    }
}

//Hints:
/*
 * http://thecodeventures.blogspot.de/2012/12/c-adding-32-bit-odbc-dsn-in-windows-64.html
 * http://azuliadesigns.com/adding-odbc-system-dsn/
 * http://stackoverflow.com/questions/334939/how-do-i-create-an-odbc-dsn-entry-using-c
 * http://www.devtoolshed.com/content/get-list-odbc-data-source-names-programatically-using-c
*/
