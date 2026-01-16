#nullable enable

namespace BAUERGROUP.Shared.Cloud.RemoveBG;

/// <summary>
/// Specifies the output image format for background removal.
/// </summary>
public enum RemoveBGImageFormat
{
    /// <summary>Automatic format selection.</summary>
    Auto = 0,

    /// <summary>PNG format with transparency support.</summary>
    PNG,

    /// <summary>JPG format.</summary>
    JPG,

    /// <summary>ZIP archive containing the result.</summary>
    ZIP
}
