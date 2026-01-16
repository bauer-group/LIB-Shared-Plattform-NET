#nullable enable

namespace BAUERGROUP.Shared.Cloud.RemoveBG;

/// <summary>
/// Interface for the Remove.bg background removal service client.
/// </summary>
public interface IRemoveBGClient : IAsyncDisposable, IDisposable
{
    /// <summary>
    /// Gets account information including credit balance.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Account information.</returns>
    Task<RemoveBGAccountInformation> GetAccountInformationAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes the background from an image file.
    /// </summary>
    /// <param name="sourceFileName">Path to the source image file.</param>
    /// <param name="targetFileName">Path where the result should be saved.</param>
    /// <param name="options">Processing options.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task RemoveBackgroundAsync(
        string sourceFileName,
        string targetFileName,
        RemoveBGClientOptions? options = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes the background from image data.
    /// </summary>
    /// <param name="imageData">The source image data.</param>
    /// <param name="fileName">Original file name (for format detection).</param>
    /// <param name="options">Processing options.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The processed image data.</returns>
    Task<byte[]> RemoveBackgroundAsync(
        byte[] imageData,
        string fileName,
        RemoveBGClientOptions? options = null,
        CancellationToken cancellationToken = default);
}
