#nullable enable

namespace BAUERGROUP.Shared.Desktop.Browser.Internet;

/// <summary>
/// Represents an external web resource job with optional data.
/// </summary>
/// <typeparam name="T">The type of data associated with the job.</typeparam>
public record class ExternalWebRessourceJob<T> where T : class
{
    /// <summary>
    /// Creates a new empty job.
    /// </summary>
    public ExternalWebRessourceJob()
        : this(string.Empty, ExternalWebRessourceMode.Unspecified, default)
    {
    }

    /// <summary>
    /// Creates a new job with the specified parameters.
    /// </summary>
    /// <param name="url">The URL to process.</param>
    /// <param name="mode">The processing mode.</param>
    /// <param name="data">Optional data for URL parameter substitution.</param>
    public ExternalWebRessourceJob(string url, ExternalWebRessourceMode mode = ExternalWebRessourceMode.Unspecified, T? data = default)
    {
        Mode = mode;
        URL = url;
        Data = data;
    }

    /// <summary>
    /// Gets or sets the processing mode.
    /// </summary>
    public ExternalWebRessourceMode Mode { get; set; }

    /// <summary>
    /// Gets or sets the URL.
    /// </summary>
    public string URL { get; set; }

    /// <summary>
    /// Gets or sets the associated data.
    /// </summary>
    public T? Data { get; set; }
}
