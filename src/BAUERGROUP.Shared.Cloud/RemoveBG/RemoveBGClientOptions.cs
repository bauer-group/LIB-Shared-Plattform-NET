#nullable enable

namespace BAUERGROUP.Shared.Cloud.RemoveBG;

/// <summary>
/// Options for background removal operations.
/// </summary>
/// <param name="IsPreview">If true, returns a smaller preview image (faster, uses fewer credits).</param>
/// <param name="Type">The type of foreground object to optimize for.</param>
/// <param name="Format">The desired output image format.</param>
/// <param name="BackgroundColor">Optional background color (e.g., "ff0000" for red). Null for transparent.</param>
public record RemoveBGClientOptions(
    bool IsPreview = false,
    RemoveBGForegroundType Type = RemoveBGForegroundType.Auto,
    RemoveBGImageFormat Format = RemoveBGImageFormat.Auto,
    string? BackgroundColor = null)
{
    /// <summary>
    /// Gets the API string value for the foreground type.
    /// </summary>
    internal string TypeValue => Type switch
    {
        RemoveBGForegroundType.Person => "person",
        RemoveBGForegroundType.Product => "product",
        RemoveBGForegroundType.Car => "car",
        _ => "auto"
    };

    /// <summary>
    /// Gets the API string value for the image format.
    /// </summary>
    internal string FormatValue => Format switch
    {
        RemoveBGImageFormat.PNG => "png",
        RemoveBGImageFormat.JPG => "jpg",
        RemoveBGImageFormat.ZIP => "zip",
        _ => "auto"
    };

    /// <summary>
    /// Gets the API string value for the size parameter.
    /// </summary>
    internal string SizeValue => IsPreview ? "preview" : "full";
}
