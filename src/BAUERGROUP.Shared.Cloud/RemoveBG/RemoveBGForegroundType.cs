#nullable enable

namespace BAUERGROUP.Shared.Cloud.RemoveBG;

/// <summary>
/// Specifies the type of foreground object for background removal.
/// </summary>
public enum RemoveBGForegroundType
{
    /// <summary>Automatic detection of foreground type.</summary>
    Auto = 0,

    /// <summary>Optimize for person/portrait images.</summary>
    Person,

    /// <summary>Optimize for product images.</summary>
    Product,

    /// <summary>Optimize for car images.</summary>
    Car
}
