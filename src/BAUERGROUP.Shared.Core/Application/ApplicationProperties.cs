using System;
using System.IO;
using System.Reflection;

namespace BAUERGROUP.Shared.Core.Application
{
    /// <summary>
    /// Provides access to application assembly information and runtime properties.
    /// </summary>
    public static class ApplicationProperties
    {
        /// <summary>
        /// Gets the entry assembly for the current process, or null if not available.
        /// </summary>
        public static Assembly? EntryAssembly
        {
            get
            {
                return Assembly.GetEntryAssembly();
            }
        }

        /// <summary>
        /// Gets the assembly that called into the current code.
        /// </summary>
        public static Assembly CallingAssembly
        {
            get
            {
                return Assembly.GetCallingAssembly();
            }
        }

        /// <summary>
        /// Gets the assembly containing the currently executing code.
        /// </summary>
        public static Assembly ExecutingAssembly
        {
            get
            {
                return Assembly.GetExecutingAssembly();
            }
        }

        /// <summary>
        /// Gets the most appropriate assembly reference, preferring EntryAssembly, then CallingAssembly, then ExecutingAssembly.
        /// </summary>
        public static Assembly AutomaticAssembly
        {
            get
            {
                return EntryAssembly == null ? (CallingAssembly == null ? ExecutingAssembly : CallingAssembly) : EntryAssembly;
            }
        }

        /// <summary>
        /// Gets the title of the application from the assembly's <see cref="AssemblyTitleAttribute"/>.
        /// </summary>
        public static String Title
        {
            get
            {
                return ((AssemblyTitleAttribute)(AutomaticAssembly.GetCustomAttributes(typeof(AssemblyTitleAttribute), false)[0])).Title;
            }
        }

        /// <summary>
        /// Gets the version of the application assembly.
        /// </summary>
        public static Version? Version
        {
            get
            {
                return AutomaticAssembly.GetName().Version;
            }
        }

        /// <summary>
        /// Gets the name of the application assembly.
        /// </summary>
        public static String? Name
        {
            get
            {
                return AutomaticAssembly.GetName().Name;
            }
        }

        /// <summary>
        /// Gets the command line arguments passed to the application.
        /// </summary>
        public static String CommandLine
        {
            get
            {
                return Environment.CommandLine;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the current process is running as 64-bit.
        /// </summary>
        public static Boolean Is64BitProcess
        {
            get
            {
                return Environment.Is64BitProcess;
            }
        }
    }
}
