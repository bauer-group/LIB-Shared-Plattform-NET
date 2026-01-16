#nullable enable

using System.Text;

namespace BAUERGROUP.Shared.Core.Files;

/// <summary>
/// Monitors a text file and provides tail functionality.
/// </summary>
public class TailTextFile : FileChangesMonitor
{
    private readonly string _fileName;

    /// <summary>
    /// Creates a new tail text file monitor.
    /// </summary>
    /// <param name="fileName">The file to monitor.</param>
    public TailTextFile(string fileName) : base(fileName)
    {
        _fileName = fileName;
    }

    /// <summary>
    /// Reads the last bytes from the file.
    /// </summary>
    /// <param name="length">Number of bytes to read from the end.</param>
    /// <returns>The tail content of the file.</returns>
    public string ReadTail(ushort length = 1024)
    {
        using var fileStream = File.Open(_fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        fileStream.Seek(0 - length, SeekOrigin.End);
        var bytes = new byte[length];
        fileStream.Read(bytes, 0, length);
        return Encoding.Default.GetString(bytes);
    }
}
