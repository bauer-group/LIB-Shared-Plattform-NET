using System;
using System.Collections.Generic;
using System.Text;

namespace BAUERGROUP.Shared.API.API
{
    /// <summary>
    /// Represents errors that occur during API client operations.
    /// </summary>
    public class GenericAPIClientException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GenericAPIClientException"/> class.
        /// </summary>
        public GenericAPIClientException()
            : base()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericAPIClientException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public GenericAPIClientException(String message)
            : base(message)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericAPIClientException"/> class with a specified error message and a reference to the inner exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public GenericAPIClientException(String message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
