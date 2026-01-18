using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAUERGROUP.Shared.Desktop.Exceptions
{
    /// <summary>
    /// Represents errors that occur during Chrome browser operations.
    /// </summary>
    public class ChromeException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChromeException"/> class.
        /// </summary>
        public ChromeException()
            : base()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChromeException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public ChromeException(String message)
            : base(message)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChromeException"/> class with a specified error message and inner exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public ChromeException(String message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
