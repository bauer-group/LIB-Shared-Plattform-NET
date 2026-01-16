#nullable enable

using System.IO;
using System.Windows.Controls;

namespace BAUERGROUP.Shared.Desktop.Reporting.Reporting;

/// <summary>
/// Manages multiple reports in a folder.
/// </summary>
public class CommonReportManager : CommonReport
{
    private const string DisabledPrefix = @"[DISABLED]";
    private const string ReportExtension = @".mrt";
    private const string ReportSearchPattern = @"*.mrt";

    /// <summary>
    /// Gets the folder containing reports.
    /// </summary>
    public string Folder { get; }

    /// <summary>
    /// Creates a new report manager.
    /// </summary>
    /// <param name="reportFilesPath">Path to the reports folder.</param>
    /// <param name="control">Optional control for context.</param>
    /// <param name="settings">Report settings.</param>
    public CommonReportManager(string reportFilesPath, Control? control = null, CommonReportSettings? settings = null)
        : base(control, settings)
    {
        Folder = reportFilesPath;
    }

    /// <inheritdoc />
    public override void Dispose()
    {
        base.Dispose();
    }

    /// <summary>
    /// Gets the list of available reports.
    /// </summary>
    public IEnumerable<string> Reports => ListReports(false, true, false);

    /// <summary>
    /// Lists reports in the folder.
    /// </summary>
    protected IEnumerable<string> ListReports(bool includeSubfolders = false, bool fileNamesOnlyWithoutExtension = false, bool onlyEnabledReports = false)
    {
        var files = Directory.GetFiles(Folder, ReportSearchPattern, includeSubfolders ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);

        IEnumerable<string> reports = fileNamesOnlyWithoutExtension
            ? files.Select(f => Path.GetFileNameWithoutExtension(f) ?? string.Empty)
            : files;

        return onlyEnabledReports
            ? reports.Where(p => !p.Contains(DisabledPrefix))
            : reports;
    }

    /// <summary>
    /// Gets the full path for a report name.
    /// </summary>
    protected string GetFullReportFilename(string name)
    {
        return Path.Combine(Folder, $"{name.Trim()}{ReportExtension}");
    }

    /// <summary>
    /// Disables a report.
    /// </summary>
    public void Disable(string name)
    {
        var fullName = GetFullReportFilename(name);

        if (!File.Exists(fullName))
            throw new NotSupportedException($"Report '{name}' cannot be disabled. The corresponding report file '{fullName}' was not found.");

        if (name.Contains(DisabledPrefix))
            return;

        Rename(name, $"{DisabledPrefix} {name}");
    }

    /// <summary>
    /// Enables a report.
    /// </summary>
    public void Enable(string name)
    {
        var fullName = GetFullReportFilename(name);

        if (!File.Exists(fullName))
            throw new NotSupportedException($"Report '{name}' cannot be enabled. The corresponding report file '{fullName}' was not found.");

        if (!name.Contains(DisabledPrefix))
            return;

        Rename(name, name.Replace(DisabledPrefix, ""));
    }

    /// <summary>
    /// Creates a new report.
    /// </summary>
    public void Create(string name)
    {
        var fullName = GetFullReportFilename(name);

        if (File.Exists(fullName))
            throw new NotSupportedException($"Report '{name}' cannot be created. The corresponding report file '{fullName}' already exists.");

        Reset();
        Save(name);
    }

    /// <summary>
    /// Renames a report.
    /// </summary>
    public void Rename(string oldName, string newName)
    {
        if (!File.Exists(GetFullReportFilename(oldName)))
            throw new NotSupportedException($"Report '{oldName}' cannot be renamed. The corresponding report file '{GetFullReportFilename(oldName)}' was not found.");

        File.Move(GetFullReportFilename(oldName), GetFullReportFilename(newName));
        OnReportChanged();
    }

    /// <summary>
    /// Copies a report.
    /// </summary>
    public void Copy(string oldName, string newName)
    {
        if (!File.Exists(GetFullReportFilename(oldName)))
            throw new NotSupportedException($"Report '{oldName}' cannot be copied. The corresponding report file '{GetFullReportFilename(oldName)}' was not found.");

        File.Copy(GetFullReportFilename(oldName), GetFullReportFilename(newName));
        OnReportChanged();
    }

    /// <summary>
    /// Deletes a report.
    /// </summary>
    public void Delete(string name)
    {
        if (!File.Exists(GetFullReportFilename(name)))
            throw new NotSupportedException($"Report '{name}' cannot be deleted. The corresponding report file '{GetFullReportFilename(name)}' was not found.");

        File.Delete(GetFullReportFilename(name));
        OnReportChanged();
    }

    /// <inheritdoc />
    public override void Load(string filename)
    {
        if (!File.Exists(GetFullReportFilename(filename)))
            throw new NotSupportedException($"Report '{filename}' cannot be loaded. The corresponding report file '{GetFullReportFilename(filename)}' was not found.");

        base.Load(GetFullReportFilename(filename));
    }

    /// <inheritdoc />
    public override void Save(string filename, bool updateReportName = true)
    {
        base.Save(GetFullReportFilename(filename));
        OnReportChanged();
    }

    /// <summary>
    /// Prints all enabled reports.
    /// </summary>
    public void PrintAll(bool showPrintDialog = false, short copies = 1)
    {
        base.PrintAll(ListReports(false, false, true), showPrintDialog, copies);
    }

    /// <summary>
    /// Occurs when a report is changed.
    /// </summary>
    public event EventHandler<EventArgs>? ReportChanged;

    /// <summary>
    /// Raises the ReportChanged event.
    /// </summary>
    protected virtual void OnReportChanged()
    {
        ReportChanged?.Invoke(this, EventArgs.Empty);
    }
}
