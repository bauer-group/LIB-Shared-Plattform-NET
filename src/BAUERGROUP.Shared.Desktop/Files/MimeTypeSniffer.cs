using System;
using System.Collections.Generic;
using System.Linq;

namespace BAUERGROUP.Shared.Desktop.Files
{
    /// <summary>
    /// Cross-platform MIME type detection based on file magic bytes (file signatures).
    /// </summary>
    public static class MimeTypeSniffer
    {
        private const string UNKNOWN = "unknown/unknown";
        private const string OCTET_STREAM = "application/octet-stream";

        private static readonly List<MimeSignature> Signatures = new()
        {
            // Images
            new MimeSignature("image/jpeg", new byte[] { 0xFF, 0xD8, 0xFF }),
            new MimeSignature("image/png", new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A }),
            new MimeSignature("image/gif", new byte[] { 0x47, 0x49, 0x46, 0x38, 0x37, 0x61 }), // GIF87a
            new MimeSignature("image/gif", new byte[] { 0x47, 0x49, 0x46, 0x38, 0x39, 0x61 }), // GIF89a
            new MimeSignature("image/bmp", new byte[] { 0x42, 0x4D }), // BM
            new MimeSignature("image/webp", new byte[] { 0x52, 0x49, 0x46, 0x46 }, 8, new byte[] { 0x57, 0x45, 0x42, 0x50 }), // RIFF....WEBP
            new MimeSignature("image/tiff", new byte[] { 0x49, 0x49, 0x2A, 0x00 }), // Little endian
            new MimeSignature("image/tiff", new byte[] { 0x4D, 0x4D, 0x00, 0x2A }), // Big endian
            new MimeSignature("image/x-icon", new byte[] { 0x00, 0x00, 0x01, 0x00 }), // ICO
            new MimeSignature("image/vnd.microsoft.icon", new byte[] { 0x00, 0x00, 0x01, 0x00 }), // ICO alternative
            new MimeSignature("image/svg+xml", System.Text.Encoding.UTF8.GetBytes("<svg")),
            new MimeSignature("image/svg+xml", System.Text.Encoding.UTF8.GetBytes("<?xml"), 0, null, data => ContainsString(data, "<svg")),
            new MimeSignature("image/avif", new byte[] { 0x00, 0x00, 0x00 }, 4, new byte[] { 0x66, 0x74, 0x79, 0x70, 0x61, 0x76, 0x69, 0x66 }), // ftyp avif
            new MimeSignature("image/heic", new byte[] { 0x00, 0x00, 0x00 }, 4, new byte[] { 0x66, 0x74, 0x79, 0x70, 0x68, 0x65, 0x69, 0x63 }), // ftyp heic

            // Documents
            new MimeSignature("application/pdf", new byte[] { 0x25, 0x50, 0x44, 0x46 }), // %PDF
            new MimeSignature("application/rtf", new byte[] { 0x7B, 0x5C, 0x72, 0x74, 0x66 }), // {\rtf
            new MimeSignature("application/postscript", new byte[] { 0x25, 0x21, 0x50, 0x53 }), // %!PS

            // Microsoft Office (legacy)
            new MimeSignature("application/msword", new byte[] { 0xD0, 0xCF, 0x11, 0xE0, 0xA1, 0xB1, 0x1A, 0xE1 }), // DOC, XLS, PPT (OLE)

            // Archives
            new MimeSignature("application/zip", new byte[] { 0x50, 0x4B, 0x03, 0x04 }), // PK
            new MimeSignature("application/zip", new byte[] { 0x50, 0x4B, 0x05, 0x06 }), // Empty archive
            new MimeSignature("application/zip", new byte[] { 0x50, 0x4B, 0x07, 0x08 }), // Spanned archive
            new MimeSignature("application/x-rar-compressed", new byte[] { 0x52, 0x61, 0x72, 0x21, 0x1A, 0x07 }), // Rar!
            new MimeSignature("application/gzip", new byte[] { 0x1F, 0x8B }),
            new MimeSignature("application/x-7z-compressed", new byte[] { 0x37, 0x7A, 0xBC, 0xAF, 0x27, 0x1C }), // 7z
            new MimeSignature("application/x-tar", new byte[] { 0x75, 0x73, 0x74, 0x61, 0x72 }, 257), // ustar at offset 257
            new MimeSignature("application/x-bzip2", new byte[] { 0x42, 0x5A, 0x68 }), // BZh
            new MimeSignature("application/x-xz", new byte[] { 0xFD, 0x37, 0x7A, 0x58, 0x5A, 0x00 }),
            new MimeSignature("application/zstd", new byte[] { 0x28, 0xB5, 0x2F, 0xFD }),

            // Audio
            new MimeSignature("audio/mpeg", new byte[] { 0xFF, 0xFB }), // MP3
            new MimeSignature("audio/mpeg", new byte[] { 0xFF, 0xFA }), // MP3
            new MimeSignature("audio/mpeg", new byte[] { 0xFF, 0xF3 }), // MP3
            new MimeSignature("audio/mpeg", new byte[] { 0xFF, 0xF2 }), // MP3
            new MimeSignature("audio/mpeg", new byte[] { 0x49, 0x44, 0x33 }), // ID3 tag
            new MimeSignature("audio/wav", new byte[] { 0x52, 0x49, 0x46, 0x46 }, 8, new byte[] { 0x57, 0x41, 0x56, 0x45 }), // RIFF....WAVE
            new MimeSignature("audio/flac", new byte[] { 0x66, 0x4C, 0x61, 0x43 }), // fLaC
            new MimeSignature("audio/ogg", new byte[] { 0x4F, 0x67, 0x67, 0x53 }), // OggS
            new MimeSignature("audio/midi", new byte[] { 0x4D, 0x54, 0x68, 0x64 }), // MThd
            new MimeSignature("audio/aac", new byte[] { 0xFF, 0xF1 }), // AAC ADTS
            new MimeSignature("audio/aac", new byte[] { 0xFF, 0xF9 }), // AAC ADTS

            // Video
            new MimeSignature("video/mp4", new byte[] { 0x00, 0x00, 0x00 }, 4, new byte[] { 0x66, 0x74, 0x79, 0x70 }), // ftyp
            new MimeSignature("video/webm", new byte[] { 0x1A, 0x45, 0xDF, 0xA3 }), // EBML (WebM/MKV)
            new MimeSignature("video/x-matroska", new byte[] { 0x1A, 0x45, 0xDF, 0xA3 }), // MKV
            new MimeSignature("video/avi", new byte[] { 0x52, 0x49, 0x46, 0x46 }, 8, new byte[] { 0x41, 0x56, 0x49, 0x20 }), // RIFF....AVI
            new MimeSignature("video/x-flv", new byte[] { 0x46, 0x4C, 0x56 }), // FLV
            new MimeSignature("video/quicktime", new byte[] { 0x00, 0x00, 0x00 }, 4, new byte[] { 0x6D, 0x6F, 0x6F, 0x76 }), // moov
            new MimeSignature("video/quicktime", new byte[] { 0x00, 0x00, 0x00 }, 4, new byte[] { 0x66, 0x72, 0x65, 0x65 }), // free
            new MimeSignature("video/mpeg", new byte[] { 0x00, 0x00, 0x01, 0xBA }), // MPEG PS
            new MimeSignature("video/mpeg", new byte[] { 0x00, 0x00, 0x01, 0xB3 }), // MPEG ES

            // Executables
            new MimeSignature("application/x-msdownload", new byte[] { 0x4D, 0x5A }), // MZ (EXE/DLL)
            new MimeSignature("application/x-elf", new byte[] { 0x7F, 0x45, 0x4C, 0x46 }), // ELF
            new MimeSignature("application/x-mach-binary", new byte[] { 0xCF, 0xFA, 0xED, 0xFE }), // Mach-O 64-bit
            new MimeSignature("application/x-mach-binary", new byte[] { 0xCE, 0xFA, 0xED, 0xFE }), // Mach-O 32-bit

            // Fonts
            new MimeSignature("font/woff", new byte[] { 0x77, 0x4F, 0x46, 0x46 }), // wOFF
            new MimeSignature("font/woff2", new byte[] { 0x77, 0x4F, 0x46, 0x32 }), // wOF2
            new MimeSignature("font/otf", new byte[] { 0x4F, 0x54, 0x54, 0x4F }), // OTTO
            new MimeSignature("font/ttf", new byte[] { 0x00, 0x01, 0x00, 0x00 }),

            // Data formats
            new MimeSignature("application/json", new byte[] { 0x7B }, 0, null, data => IsJson(data)), // { with validation
            new MimeSignature("application/xml", new byte[] { 0x3C, 0x3F, 0x78, 0x6D, 0x6C }), // <?xml
            new MimeSignature("application/x-sqlite3", new byte[] { 0x53, 0x51, 0x4C, 0x69, 0x74, 0x65, 0x20, 0x66, 0x6F, 0x72, 0x6D, 0x61, 0x74, 0x20, 0x33 }), // SQLite format 3

            // Text/HTML (check last due to being less specific)
            new MimeSignature("text/html", System.Text.Encoding.UTF8.GetBytes("<!DOCTYPE html"), 0, null, null, StringComparison.OrdinalIgnoreCase),
            new MimeSignature("text/html", System.Text.Encoding.UTF8.GetBytes("<html"), 0, null, null, StringComparison.OrdinalIgnoreCase),
            new MimeSignature("text/html", System.Text.Encoding.UTF8.GetBytes("<head"), 0, null, null, StringComparison.OrdinalIgnoreCase),
            new MimeSignature("text/html", System.Text.Encoding.UTF8.GetBytes("<body"), 0, null, null, StringComparison.OrdinalIgnoreCase),
        };

        /// <summary>
        /// Returns the MIME type for the specified file header bytes.
        /// </summary>
        /// <param name="header">The file header bytes to examine.</param>
        /// <returns>The detected MIME type or "unknown/unknown" if not recognized.</returns>
        public static string GetMime(byte[] header)
        {
            if (header == null || header.Length == 0)
            {
                return UNKNOWN;
            }

            try
            {
                foreach (var signature in Signatures)
                {
                    if (signature.Matches(header))
                    {
                        return signature.MimeType;
                    }
                }

                // Check if it looks like text
                if (LooksLikeText(header))
                {
                    return "text/plain";
                }

                return OCTET_STREAM;
            }
            catch
            {
                return UNKNOWN;
            }
        }

        /// <summary>
        /// Returns the MIME type for the specified file header bytes.
        /// Returns application/octet-stream instead of unknown/unknown for unrecognized types.
        /// </summary>
        /// <param name="header">The file header bytes to examine.</param>
        /// <returns>The detected MIME type.</returns>
        public static string GetMimeOrDefault(byte[] header)
        {
            var mime = GetMime(header);
            return mime == UNKNOWN ? OCTET_STREAM : mime;
        }

        private static bool LooksLikeText(byte[] data)
        {
            if (data.Length == 0) return false;

            // Check for UTF-8 BOM
            if (data.Length >= 3 && data[0] == 0xEF && data[1] == 0xBB && data[2] == 0xBF)
            {
                return true;
            }

            // Check for UTF-16 BOM
            if (data.Length >= 2 && ((data[0] == 0xFF && data[1] == 0xFE) || (data[0] == 0xFE && data[1] == 0xFF)))
            {
                return true;
            }

            // Check if mostly printable ASCII/UTF-8
            int printable = 0;
            int length = Math.Min(data.Length, 512);

            for (int i = 0; i < length; i++)
            {
                byte b = data[i];
                if ((b >= 0x20 && b < 0x7F) || b == 0x09 || b == 0x0A || b == 0x0D || b >= 0x80)
                {
                    printable++;
                }
            }

            return (double)printable / length > 0.85;
        }

        private static bool ContainsString(byte[] data, string search)
        {
            if (data.Length < search.Length) return false;

            var searchBytes = System.Text.Encoding.UTF8.GetBytes(search);
            int limit = Math.Min(data.Length - searchBytes.Length, 1024);

            for (int i = 0; i <= limit; i++)
            {
                bool found = true;
                for (int j = 0; j < searchBytes.Length; j++)
                {
                    if (data[i + j] != searchBytes[j])
                    {
                        found = false;
                        break;
                    }
                }
                if (found) return true;
            }

            return false;
        }

        private static bool IsJson(byte[] data)
        {
            if (data.Length < 2) return false;

            // Skip UTF-8 BOM if present
            int start = 0;
            if (data.Length >= 3 && data[0] == 0xEF && data[1] == 0xBB && data[2] == 0xBF)
            {
                start = 3;
            }

            // Skip whitespace
            while (start < data.Length && (data[start] == 0x20 || data[start] == 0x09 || data[start] == 0x0A || data[start] == 0x0D))
            {
                start++;
            }

            if (start >= data.Length) return false;

            // JSON must start with { or [
            return data[start] == 0x7B || data[start] == 0x5B;
        }

        private sealed class MimeSignature
        {
            public string MimeType { get; }
            private readonly byte[] _signature;
            private readonly int _offset;
            private readonly byte[]? _secondSignature;
            private readonly int _secondOffset;
            private readonly Func<byte[], bool>? _additionalCheck;
            private readonly StringComparison? _stringComparison;

            public MimeSignature(string mimeType, byte[] signature, int offset = 0, byte[]? secondSignature = null, Func<byte[], bool>? additionalCheck = null, StringComparison? stringComparison = null)
            {
                MimeType = mimeType;
                _signature = signature;
                _offset = offset;
                _secondSignature = secondSignature;
                _secondOffset = secondSignature != null ? offset + signature.Length : 0;
                _additionalCheck = additionalCheck;
                _stringComparison = stringComparison;
            }

            public bool Matches(byte[] data)
            {
                // Check if data is long enough
                int requiredLength = _offset + _signature.Length;
                if (_secondSignature != null)
                {
                    requiredLength = Math.Max(requiredLength, _secondOffset + _secondSignature.Length);
                }

                if (data.Length < requiredLength)
                {
                    return false;
                }

                // Case-insensitive string comparison for text-based signatures
                if (_stringComparison.HasValue)
                {
                    try
                    {
                        string dataStr = System.Text.Encoding.UTF8.GetString(data, _offset, Math.Min(_signature.Length + 10, data.Length - _offset));
                        string sigStr = System.Text.Encoding.UTF8.GetString(_signature);
                        if (!dataStr.StartsWith(sigStr, _stringComparison.Value))
                        {
                            return false;
                        }
                    }
                    catch
                    {
                        return false;
                    }
                }
                else
                {
                    // Check primary signature
                    for (int i = 0; i < _signature.Length; i++)
                    {
                        if (data[_offset + i] != _signature[i])
                        {
                            return false;
                        }
                    }
                }

                // Check secondary signature if present
                if (_secondSignature != null)
                {
                    for (int i = 0; i < _secondSignature.Length; i++)
                    {
                        if (data[_secondOffset + i] != _secondSignature[i])
                        {
                            return false;
                        }
                    }
                }

                // Run additional check if present
                if (_additionalCheck != null)
                {
                    return _additionalCheck(data);
                }

                return true;
            }
        }
    }
}
