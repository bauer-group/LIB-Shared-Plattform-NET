#nullable enable

using Stimulsoft.Report;
using Stimulsoft.Report.Components;
using System.Drawing.Printing;
using System.IO;
using System.Reflection;
using System.Security.Principal;
using System.Windows.Controls;

namespace BAUERGROUP.Shared.Desktop.Reporting.Reporting;

/// <summary>
/// Base class for Stimulsoft reports.
/// </summary>
public class CommonReport : IDisposable
{
    private static bool _licenseApplied;
#if NET9_0_OR_GREATER
    private static readonly Lock _licenseLock = new();
#else
    private static readonly object _licenseLock = new();
#endif

    /// <summary>
    /// Gets the underlying report.
    /// </summary>
    protected StiReport? Report { get; private set; }

    /// <summary>
    /// Gets the report settings.
    /// </summary>
    protected CommonReportSettings Settings { get; }

    /// <summary>
    /// Creates a new report instance.
    /// </summary>
    /// <param name="control">Optional control for context.</param>
    /// <param name="settings">Report settings.</param>
    /// <exception cref="InvalidOperationException">Thrown when license has not been applied. Call <see cref="ApplyLicense"/> first.</exception>
    public CommonReport(Control? control = null, CommonReportSettings? settings = null)
    {
        if (!_licenseApplied)
            throw new InvalidOperationException(
                "Stimulsoft license has not been applied. Call CommonReport.ApplyLicense() at application startup.");

        Settings = settings ?? new CommonReportSettings();
        SetupReportBehaviour();
        Report = new StiReport();
        UpdateAssemblies();
    }

    /// <summary>
    /// Applies the Stimulsoft license key. Must be called once at application startup.
    /// </summary>
    /// <param name="licenseKey">The Stimulsoft license key.</param>
    /// <exception cref="ArgumentException">Thrown when licenseKey is null or empty.</exception>
    public static void ApplyLicense(string licenseKey)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(licenseKey, nameof(licenseKey));

        lock (_licenseLock)
        {
            if (_licenseApplied)
                return;

            Stimulsoft.Base.StiLicense.Key = licenseKey;
            _licenseApplied = true;
        }
    }

    /// <summary>
    /// Gets a value indicating whether the license has been applied.
    /// </summary>
    public static bool IsLicenseApplied => _licenseApplied;

    private void SetupReportBehaviour()
    {
        if (!string.IsNullOrWhiteSpace(Settings.ViewerTitle))
            StiOptions.Viewer.ViewerTitle = Settings.ViewerTitle;

        if (!string.IsNullOrWhiteSpace(Settings.DesignerTitle))
            StiOptions.Designer.DesignerTitle = Settings.DesignerTitle;
    }

    private void UpdateAssemblies()
    {
        if (Report == null || !Settings.ReferenceAssemblies)
            return;

        var entryAssembly = Assembly.GetEntryAssembly();
        if (entryAssembly == null)
            return;

        var currentReportAssemblies = new List<string>(Report.ReferencedAssemblies);
        var runtimeAssemblies = entryAssembly.GetReferencedAssemblies()
            .Select(s => s.Name)
            .Where(n => n != null)
            .Cast<string>();

        var mergedAssemblies = new List<string>();
        mergedAssemblies.AddRange(currentReportAssemblies.Select(s => s.Replace(".dll", "")));
        mergedAssemblies.AddRange(runtimeAssemblies);

        var entryName = entryAssembly.GetName().Name;
        if (entryName != null)
            mergedAssemblies.Add(entryName);

        if (Settings.AdditionalAssemblies != null)
            mergedAssemblies.AddRange(Settings.AdditionalAssemblies);

        Report.ReferencedAssemblies = mergedAssemblies.Distinct().ToArray();
    }

    /// <inheritdoc />
    public virtual void Dispose()
    {
        if (Report == null)
            return;

        Report.Dispose();
        Report = null;
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Assigns a business object to the report.
    /// </summary>
    public void AssignBusinessObject<T>(string name, T data)
    {
        Report?.RegBusinessObject(name, data);
    }

    /// <summary>
    /// Assigns a business object using its type name.
    /// </summary>
    public void AssignBusinessObject<T>(T data)
    {
        AssignBusinessObject(typeof(T).Name, data);
    }

    /// <summary>
    /// Assigns data to the report.
    /// </summary>
    public void AssignData<T>(string name, T data)
    {
        Report?.RegData(name, data);
    }

    /// <summary>
    /// Assigns data using its type name.
    /// </summary>
    public void AssignData<T>(T data)
    {
        AssignData(typeof(T).Name, data);
    }

    /// <summary>
    /// Assigns both business object and data to the report.
    /// </summary>
    public void AssignBoth<T>(string name, T data)
    {
        AssignBusinessObject($"o{name}", data);
        AssignData(name, data);
    }

    /// <summary>
    /// Assigns both business object and data using the type name.
    /// </summary>
    public void AssignBoth<T>(T data)
    {
        AssignBoth(typeof(T).Name, data);
    }

    /// <summary>
    /// Resets the report to default state.
    /// </summary>
    public void Reset()
    {
        if (Report == null) return;

        Report.Pages.Clear();
        Report.Pages.Add(new StiPage
        {
            PageHeight = 29.7f,
            PageWidth = 21f,
            Name = "Page 1",
            Margins = new StiMargins(1f, 1f, 1f, 1f),
            PaperSize = PaperKind.A4
        });
        Report.Reset();
    }

    /// <summary>
    /// Clears all data from the report.
    /// </summary>
    public void Clear()
    {
        if (Report == null) return;

        Report.Dictionary.BusinessObjects.Clear();
        Report.Dictionary.Databases.Clear();
        Report.Dictionary.DataSources.Clear();
        Report.Dictionary.DataStore.Clear();
        Report.Dictionary.Synchronize();
    }

    /// <summary>
    /// Loads a report from file.
    /// </summary>
    public virtual void Load(string filename)
    {
        Report?.Load(filename);
        UpdateAssemblies();
    }

    /// <summary>
    /// Saves the report to file.
    /// </summary>
    public virtual void Save(string filename, bool updateReportName = true)
    {
        if (Report == null) return;

        if (updateReportName)
        {
            var name = Path.GetFileNameWithoutExtension(filename);
            Name = name;
            AliasName = name;
            Author = WindowsIdentity.GetCurrent().Name;
        }

        Report.Save(filename);
    }

    /// <summary>
    /// Shows the report viewer.
    /// </summary>
    public void Show()
    {
        Report?.ShowWithWpfRibbonGUI(true);
    }

    /// <summary>
    /// Opens the report designer.
    /// </summary>
    public void Design()
    {
        Report?.DesignWithWpf();
    }

    /// <summary>
    /// Prints the report.
    /// </summary>
    public void Print(bool showPrintDialog = false, short copies = 1, bool showProgress = true)
    {
        if (Report == null) return;

        Report.RenderWithWpf(showProgress);
        Report.PrintWithWpf(showPrintDialog, copies);
    }

    /// <summary>
    /// Prints multiple reports.
    /// </summary>
    public virtual void PrintAll(IEnumerable<string> reportFilenames, bool showPrintDialog = false, short copies = 1)
    {
        if (Report == null) return;

        foreach (var filename in reportFilenames)
        {
            Report.Load(filename);
            Print(showPrintDialog, copies, false);
        }
    }

    /// <summary>
    /// Gets or sets the report name.
    /// </summary>
    public string Name
    {
        get => Report?.ReportName ?? string.Empty;
        set { if (Report != null) Report.ReportName = value; }
    }

    /// <summary>
    /// Gets or sets the report description.
    /// </summary>
    public string Description
    {
        get => Report?.ReportDescription ?? string.Empty;
        set { if (Report != null) Report.ReportDescription = value; }
    }

    /// <summary>
    /// Gets or sets the report author.
    /// </summary>
    public string Author
    {
        get => Report?.ReportAuthor ?? string.Empty;
        set { if (Report != null) Report.ReportAuthor = value; }
    }

    /// <summary>
    /// Gets or sets the report alias name.
    /// </summary>
    public string AliasName
    {
        get => Report?.ReportAlias ?? string.Empty;
        set { if (Report != null) Report.ReportAlias = value; }
    }
}
