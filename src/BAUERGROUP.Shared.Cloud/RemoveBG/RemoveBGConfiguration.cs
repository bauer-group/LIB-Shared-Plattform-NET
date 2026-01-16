#nullable enable

namespace BAUERGROUP.Shared.Cloud.RemoveBG;

/// <summary>
/// Configuration for the Remove.bg API client.
/// </summary>
/// <param name="ApiKey">The API key for authentication with Remove.bg service.</param>
public record RemoveBGConfiguration(string ApiKey)
{
    /// <summary>
    /// Creates a new configuration with an empty API key.
    /// </summary>
    public RemoveBGConfiguration() : this(string.Empty) { }
}
