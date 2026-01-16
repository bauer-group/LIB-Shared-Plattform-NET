#nullable enable

using System.Net.Http.Headers;
using System.Text.Json;

namespace BAUERGROUP.Shared.Cloud.RemoveBG;

/// <summary>
/// Client for the Remove.bg background removal API.
/// </summary>
/// <remarks>
/// For DI registration, use IHttpClientFactory:
/// <code>
/// services.AddHttpClient&lt;IRemoveBGClient, RemoveBGClient&gt;();
/// </code>
/// </remarks>
public class RemoveBGClient : IRemoveBGClient
{
    private const string BaseUrl = "https://api.remove.bg/v1.0";
    private const string ApiKeyHeader = "X-Api-Key";

    private readonly HttpClient _httpClient;
    private readonly RemoveBGConfiguration _configuration;
    private readonly JsonSerializerOptions _jsonOptions;
    private bool _disposed;

    /// <summary>
    /// Creates a new Remove.bg client with the specified configuration.
    /// </summary>
    /// <param name="httpClient">The HTTP client to use (injected by IHttpClientFactory).</param>
    /// <param name="configuration">The API configuration.</param>
    public RemoveBGClient(HttpClient httpClient, RemoveBGConfiguration configuration)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

        _httpClient.DefaultRequestHeaders.Add(ApiKeyHeader, _configuration.ApiKey);
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    /// <summary>
    /// Creates a new Remove.bg client with a new HttpClient instance.
    /// </summary>
    /// <param name="configuration">The API configuration.</param>
    /// <remarks>
    /// Prefer using the constructor with HttpClient injection for better resource management.
    /// </remarks>
    public RemoveBGClient(RemoveBGConfiguration configuration)
        : this(new HttpClient(), configuration)
    {
    }

    /// <inheritdoc />
    public async Task<RemoveBGAccountInformation> GetAccountInformationAsync(CancellationToken cancellationToken = default)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        try
        {
            using var response = await _httpClient.GetAsync($"{BaseUrl}/account", cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
                throw new RemoveBGClientException(response.StatusCode, errorContent);
            }

            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            var wrapper = JsonSerializer.Deserialize<AccountResponseWrapper>(content, _jsonOptions);

            return wrapper?.Data?.Attributes
                ?? throw new RemoveBGClientException("Invalid response format from API");
        }
        catch (HttpRequestException ex)
        {
            throw new RemoveBGClientException("HTTP request to Remove.bg failed", ex);
        }
        catch (TaskCanceledException ex) when (!cancellationToken.IsCancellationRequested)
        {
            throw new RemoveBGClientException("Request to Remove.bg timed out", ex);
        }
        catch (JsonException ex)
        {
            throw new RemoveBGClientException("Failed to parse Remove.bg response", ex);
        }
    }

    /// <inheritdoc />
    public async Task RemoveBackgroundAsync(
        string sourceFileName,
        string targetFileName,
        RemoveBGClientOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        ArgumentException.ThrowIfNullOrWhiteSpace(sourceFileName);
        ArgumentException.ThrowIfNullOrWhiteSpace(targetFileName);

        if (!File.Exists(sourceFileName))
        {
            throw new FileNotFoundException("Source file not found", sourceFileName);
        }

        var imageData = await File.ReadAllBytesAsync(sourceFileName, cancellationToken);
        var resultData = await RemoveBackgroundAsync(imageData, Path.GetFileName(sourceFileName), options, cancellationToken);

        await using var targetStream = new FileStream(
            targetFileName,
            FileMode.Create,
            FileAccess.Write,
            FileShare.None,
            bufferSize: 4096,
            useAsync: true);

        await targetStream.WriteAsync(resultData, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<byte[]> RemoveBackgroundAsync(
        byte[] imageData,
        string fileName,
        RemoveBGClientOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        ArgumentNullException.ThrowIfNull(imageData);
        ArgumentException.ThrowIfNullOrWhiteSpace(fileName);

        options ??= new RemoveBGClientOptions();

        try
        {
            using var content = CreateMultipartContent(imageData, fileName, options);
            using var response = await _httpClient.PostAsync($"{BaseUrl}/removebg", content, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
                throw new RemoveBGClientException(response.StatusCode, errorContent);
            }

            return await response.Content.ReadAsByteArrayAsync(cancellationToken);
        }
        catch (HttpRequestException ex)
        {
            throw new RemoveBGClientException("HTTP request to Remove.bg failed", ex);
        }
        catch (TaskCanceledException ex) when (!cancellationToken.IsCancellationRequested)
        {
            throw new RemoveBGClientException("Request to Remove.bg timed out", ex);
        }
    }

    private static MultipartFormDataContent CreateMultipartContent(
        byte[] imageData,
        string fileName,
        RemoveBGClientOptions options)
    {
        var content = new MultipartFormDataContent
        {
            { new ByteArrayContent(imageData), "image_file", fileName },
            { new StringContent(options.SizeValue), "size" },
            { new StringContent(options.TypeValue), "type" },
            { new StringContent(options.FormatValue), "format" }
        };

        if (!string.IsNullOrEmpty(options.BackgroundColor))
        {
            content.Add(new StringContent(options.BackgroundColor), "bg_color");
        }

        return content;
    }

    /// <inheritdoc />
    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    /// <inheritdoc />
    public ValueTask DisposeAsync()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
        return ValueTask.CompletedTask;
    }

    /// <summary>
    /// Disposes managed resources.
    /// </summary>
    protected virtual void Dispose(bool disposing)
    {
        if (_disposed) return;

        if (disposing)
        {
            // Note: HttpClient is typically managed by IHttpClientFactory
            // and should not be disposed by this class in that case.
        }

        _disposed = true;
    }

    // Internal wrapper classes for JSON deserialization
    private record AccountResponseWrapper
    {
        public AccountDataWrapper? Data { get; init; }
    }

    private record AccountDataWrapper
    {
        public RemoveBGAccountInformation? Attributes { get; init; }
    }
}
