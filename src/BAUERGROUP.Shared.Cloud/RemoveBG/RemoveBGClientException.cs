#nullable enable

using System.Net;

namespace BAUERGROUP.Shared.Cloud.RemoveBG;

/// <summary>
/// Exception thrown when a Remove.bg API operation fails.
/// </summary>
public class RemoveBGClientException : Exception
{
    /// <summary>
    /// Gets the HTTP status code associated with this exception, if any.
    /// </summary>
    public HttpStatusCode? StatusCode { get; }

    /// <summary>
    /// Creates a new exception with a message.
    /// </summary>
    public RemoveBGClientException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Creates a new exception with a message and inner exception.
    /// </summary>
    public RemoveBGClientException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    /// <summary>
    /// Creates a new exception with an HTTP status code and message.
    /// </summary>
    public RemoveBGClientException(HttpStatusCode statusCode, string message)
        : base(message)
    {
        StatusCode = statusCode;
    }

    /// <summary>
    /// Creates a new exception with an HTTP status code, message, and inner exception.
    /// </summary>
    public RemoveBGClientException(HttpStatusCode statusCode, string message, Exception innerException)
        : base(message, innerException)
    {
        StatusCode = statusCode;
    }
}
