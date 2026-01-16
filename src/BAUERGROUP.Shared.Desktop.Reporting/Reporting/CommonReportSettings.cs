#nullable enable

using Stimulsoft.Report;

namespace BAUERGROUP.Shared.Desktop.Reporting.Reporting;

/// <summary>
/// Settings for the report engine.
/// </summary>
public class CommonReportSettings
{
    /// <summary>
    /// Creates new report settings with default values.
    /// </summary>
    public CommonReportSettings()
    {
        ReferenceAssemblies = true;
        ExportEmailAutoGenerateFilename = true;
        ExportPDFCreator = @"BAUER GROUP Shared Components - Reporting Engine (http://www.bauer-group.com/)";

        StiOptions.Dictionary.BusinessObjects.MaxLevel = 32;

        AdditionalAssemblies = new List<string>();
    }

    /// <summary>
    /// Gets or sets whether to reference assemblies automatically.
    /// </summary>
    public bool ReferenceAssemblies { get; set; }

    /// <summary>
    /// Gets or sets the designer window title.
    /// </summary>
    public string? DesignerTitle { get; set; }

    /// <summary>
    /// Gets or sets the viewer window title.
    /// </summary>
    public string? ViewerTitle { get; set; }

    /// <summary>
    /// Gets or sets additional assemblies to reference.
    /// </summary>
    public List<string> AdditionalAssemblies { get; set; }

    /// <summary>
    /// Gets or sets whether to auto-generate filename in email export.
    /// </summary>
    public bool ExportEmailAutoGenerateFilename
    {
        get => StiOptions.Export.AutoGenerateFileNameInSendEMail;
        set => StiOptions.Export.AutoGenerateFileNameInSendEMail = value;
    }

    /// <summary>
    /// Gets or sets the PDF creator string.
    /// </summary>
    public string ExportPDFCreator
    {
        get => StiOptions.Export.Pdf.CreatorString;
        set => StiOptions.Export.Pdf.CreatorString = value;
    }
}
