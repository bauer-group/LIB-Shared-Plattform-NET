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
    public CommonReport(Control? control = null, CommonReportSettings? settings = null)
    {
        ApplyLicense();
        Settings = settings ?? new CommonReportSettings();
        SetupReportBehaviour();
        Report = new StiReport();
        UpdateAssemblies();
    }

    private void ApplyLicense()
    {
        Stimulsoft.Base.StiLicense.Key = "6vJhGtLLLz2GNviWmUTrhSqnOItdDwjBylQzQcAOiHnqkeggb/VhgQmKwOs4flbv5DZTaU+jqS5BybbTk1Fi06HsDc" +
                                         "fRtqH1mCKYHwCQrbySRxk6h6AhdJ9UdaN/e1VyVrce2pxzRVIkcpjivDhAQMBRtkdpRj9LJCCJz8CeGoMbuL55zoyl" +
                                         "sl1ZsDNyqwb3+wnZp3iQ577I9RsoAHkmVeDjDxWYJLi4ZPbDEBPPkCAvMD8KebAjihELEs/wU55U5DjxpERYh6r+Se" +
                                         "YMGohZ7bl9r6jh6b3owmqRfJKn6rjK0OzHv257rk8rClNmExl97et8G/FjVz6lcCEu7sHbeMv/Iy61iMbQq7m0I0aY" +
                                         "lDZO2kgiKudKc1vtMEndM3P6taPER+U+xsWhsT6+UhR688s196Wah0N1QOPd0ClqFOpbSLaAZn3dBHQWZnflPzc8hn" +
                                         "9LU+gJI4OyjypMHA0jW8CT1Cy3NHzLKrWPGDmB305MLedT3cc9SSY1Y0fCSbOTNU/QszSJ9b7TQ8v1Wr8sgpukkn4E" +
                                         "mVOp7INJkG6tzvL8Gt2fYIlNLhxqkz7f4uTt";
    }

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
