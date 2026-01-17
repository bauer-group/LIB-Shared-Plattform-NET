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

        var actualLength = (int)Math.Min(length, fileStream.Length);
        if (actualLength == 0)
        {
            return string.Empty;
        }

        fileStream.Seek(-actualLength, SeekOrigin.End);
        var bytes = new byte[actualLength];
        ReadExactly(fileStream, bytes, 0, actualLength);
        return Encoding.Default.GetString(bytes);
    }

    private static void ReadExactly(Stream stream, byte[] buffer, int offset, int count)
    {
#if NETSTANDARD2_0
        int totalRead = 0;
        while (totalRead < count)
        {
            int bytesRead = stream.Read(buffer, offset + totalRead, count - totalRead);
            if (bytesRead == 0)
            {
                throw new EndOfStreamException("Unexpected end of stream.");
            }
            totalRead += bytesRead;
        }
#else
        stream.ReadExactly(buffer, offset, count);
#endif
    }
}
