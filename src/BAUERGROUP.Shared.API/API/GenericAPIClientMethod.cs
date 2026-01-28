using System;
using System.Collections.Generic;
using System.Text;

namespace BAUERGROUP.Shared.API.API
{
    /// <summary>
    /// Specifies the HTTP method to use for API requests.
    /// </summary>
    public enum GenericAPIClientMethod
    {
        /// <summary>
        /// HTTP GET method for retrieving resources.
        /// </summary>
        GET,

        /// <summary>
        /// HTTP POST method for creating resources.
        /// </summary>
        POST,

        /// <summary>
        /// HTTP PUT method for updating resources.
        /// </summary>
        PUT,

        /// <summary>
        /// HTTP DELETE method for removing resources.
        /// </summary>
        DELETE
    }
}
