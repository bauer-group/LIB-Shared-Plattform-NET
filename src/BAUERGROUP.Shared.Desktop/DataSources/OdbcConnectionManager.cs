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
                String sDSNName = systemDSNList.GetKey(i) as String;
                DataSourceType eDSNType = (DataSourceType)systemDSNList.GetByIndex(i);                

                try
                {                    
                    userDSNList.Add(sDSNName, eDSNType);
                }
                catch
                {
                    // An exception can be thrown if the key being added is a duplicate. So we just catch it here and have to ignore it.
                }
            }            

            return userDSNList;            
        }

        public static SortedList GetDataSourceNames(DataSourceType eType)
        {
            SortedList dsnList = new SortedList();

            var rkSoftware = (eType == DataSourceType.System ? Registry.LocalMachine : Registry.CurrentUser).OpenSubKey("Software");

            if (rkSoftware != null)
            {
                var rkODBC = rkSoftware.OpenSubKey("ODBC");

                if (rkODBC != null)
                {
                    var rkODBCINI = rkODBC.OpenSubKey("ODBC.INI");

                    if (rkODBCINI != null)
                    {
                        var rkODBCDSN = rkODBCINI.OpenSubKey("ODBC Data Sources");

                        if (rkODBCDSN != null)
                        {
                            foreach (string sName in rkODBCDSN.GetValueNames())
                                dsnList.Add(sName, eType);

                            rkODBCDSN.Close();
                        }

                        rkODBCINI.Close();
                    }

                    rkODBC.Close();
                }

                rkSoftware.Close();
            }

            return dsnList;
        }

        public static String GetConnectionString(String sDSN, String sUsername, String sPassword)
        {
            var odbcConnection = new OdbcConnectionStringBuilder();

            odbcConnection.Add(@"DSN", sDSN);
            odbcConnection.Add(@"UID", sUsername);
            odbcConnection.Add(@"PWD", sPassword);

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
