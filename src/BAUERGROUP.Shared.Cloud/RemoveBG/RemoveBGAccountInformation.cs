#nullable enable

using System.Text.Json.Serialization;

namespace BAUERGROUP.Shared.Cloud.RemoveBG;

/// <summary>
/// Account information from the Remove.bg API.
/// </summary>
public record RemoveBGAccountInformation
{
    /// <summary>Credit balance information.</summary>
    [JsonPropertyName("credits")]
    public CreditInfo? Credits { get; init; }

    /// <summary>API usage information.</summary>
    [JsonPropertyName("api")]
    public ApiInfo? Api { get; init; }

    /// <summary>
    /// Credit balance details.
    /// </summary>
    public record CreditInfo
    {
        /// <summary>Total available credits.</summary>
        [JsonPropertyName("total")]
        public int Total { get; init; }

        /// <summary>Subscription credits.</summary>
        [JsonPropertyName("subscription")]
        public int Subscription { get; init; }

        /// <summary>Pay-as-you-go credits.</summary>
        [JsonPropertyName("payg")]
        public int PayAsYouGo { get; init; }

        /// <summary>Enterprise credits.</summary>
        [JsonPropertyName("enterprise")]
        public int Enterprise { get; init; }
    }

    /// <summary>
    /// API usage details.
    /// </summary>
    public record ApiInfo
    {
        /// <summary>Number of free API calls remaining.</summary>
        [JsonPropertyName("free_calls")]
        public int FreeCalls { get; init; }

        /// <summary>Available image sizes.</summary>
        [JsonPropertyName("sizes")]
        public string? Sizes { get; init; }
    }
}
