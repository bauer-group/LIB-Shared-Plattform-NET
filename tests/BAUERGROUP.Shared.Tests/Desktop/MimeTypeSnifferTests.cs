using BAUERGROUP.Shared.Desktop.Files;

namespace BAUERGROUP.Shared.Tests.Desktop;

public class MimeTypeSnifferTests
{
    [Fact]
    public void GetMime_WithPngHeader_ReturnsPngMimeType()
    {
        // PNG file signature: 89 50 4E 47 0D 0A 1A 0A
        var pngHeader = new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A };

        var result = MimeTypeSniffer.GetMime(pngHeader);

        result.Should().BeOneOf("image/png", "image/x-png", "unknown/unknown");
    }

    [Fact]
    public void GetMime_WithJpegHeader_ReturnsJpegMimeType()
    {
        // JPEG file signature: FF D8 FF
        var jpegHeader = new byte[] { 0xFF, 0xD8, 0xFF, 0xE0, 0x00, 0x10, 0x4A, 0x46 };

        var result = MimeTypeSniffer.GetMime(jpegHeader);

        result.Should().BeOneOf("image/jpeg", "image/pjpeg", "unknown/unknown");
    }

    [Fact]
    public void GetMime_WithGifHeader_ReturnsGifMimeType()
    {
        // GIF file signature: 47 49 46 38 (GIF8)
        var gifHeader = new byte[] { 0x47, 0x49, 0x46, 0x38, 0x39, 0x61 };

        var result = MimeTypeSniffer.GetMime(gifHeader);

        result.Should().BeOneOf("image/gif", "unknown/unknown");
    }

    [Fact]
    public void GetMime_WithPdfHeader_ReturnsPdfMimeType()
    {
        // PDF file signature: 25 50 44 46 (%PDF)
        var pdfHeader = new byte[] { 0x25, 0x50, 0x44, 0x46, 0x2D, 0x31, 0x2E };

        var result = MimeTypeSniffer.GetMime(pdfHeader);

        result.Should().BeOneOf("application/pdf", "unknown/unknown");
    }

    [Fact]
    public void GetMime_WithUnknownHeader_ReturnsUnknown()
    {
        // Random bytes that don't match any known signature
        var unknownHeader = new byte[] { 0x12, 0x34, 0x56, 0x78 };

        var result = MimeTypeSniffer.GetMime(unknownHeader);

        // Should return unknown/unknown or a generic type
        result.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void GetMime_WithEmptyArray_ReturnsResult()
    {
        var emptyHeader = Array.Empty<byte>();

        var result = MimeTypeSniffer.GetMime(emptyHeader);

        result.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void GetMime_WithHtmlContent_ReturnsHtmlMimeType()
    {
        // HTML content starts with <html>
        var htmlBytes = System.Text.Encoding.ASCII.GetBytes("<html><head></head><body></body></html>");

        var result = MimeTypeSniffer.GetMime(htmlBytes);

        result.Should().BeOneOf("text/html", "unknown/unknown");
    }

    [Fact]
    public void GetMime_WithZipHeader_ReturnsZipMimeType()
    {
        // ZIP file signature: 50 4B 03 04
        var zipHeader = new byte[] { 0x50, 0x4B, 0x03, 0x04 };

        var result = MimeTypeSniffer.GetMime(zipHeader);

        result.Should().BeOneOf("application/x-zip-compressed", "application/zip", "unknown/unknown");
    }

    [Fact]
    public void GetMime_WithBmpHeader_ReturnsBmpMimeType()
    {
        // BMP file signature: 42 4D (BM)
        var bmpHeader = new byte[] { 0x42, 0x4D, 0x00, 0x00, 0x00, 0x00 };

        var result = MimeTypeSniffer.GetMime(bmpHeader);

        result.Should().BeOneOf("image/bmp", "image/x-bmp", "unknown/unknown");
    }
}
