using System;
using System.IO;
using System.Reflection;

namespace BAUERGROUP.Shared.Core.Application
{
    public static class ApplicationProperties
    {
        public static Assembly EntryAssembly
        {
            get
            {
                return Assembly.GetEntryAssembly();
            }
        }

        public static Assembly CallingAssembly
        {
            get
            {
                return Assembly.GetCallingAssembly();
            }
        }

        public static Assembly ExecutingAssembly
        {
            get
            {
                return Assembly.GetExecutingAssembly();
            }
        }

        public static Assembly AutomaticAssembly
        {
            get
            {
                return EntryAssembly == null ? (CallingAssembly == null ? ExecutingAssembly : CallingAssembly) : EntryAssembly;
            }
        }

        public static String Title
        {
            get
            {
                return ((AssemblyTitleAttribute)(AutomaticAssembly.GetCustomAttributes(typeof(AssemblyTitleAttribute), false)[0])).Title;
            }
        }

        public static Version Version
        {
            get
            {
                return AutomaticAssembly.GetName().Version;
            }
        }

        public static String Name
        {
            get
            {
                return AutomaticAssembly.GetName().Name;
            }
        }

        public static String CommandLine
        {
            get
            {
                return Environment.CommandLine;
            }
        }

        public static Boolean Is64BitProcess
        {
            get
            {
                return Environment.Is64BitProcess;
            }
        }
    }
}
