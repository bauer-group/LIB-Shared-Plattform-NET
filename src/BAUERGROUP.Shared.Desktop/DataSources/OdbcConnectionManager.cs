using Microsoft.Win32;
using System;
using System.Collections;
using System.Data.Odbc;
using System.Linq;
using System.Text;

namespace BAUERGROUP.Shared.Desktop.DataSources
{
    public class OdbcConnectionManager
    {
        public enum DataSourceType
        {
            System = 0,
            User = 1
        }

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
