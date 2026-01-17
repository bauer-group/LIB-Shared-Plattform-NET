using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BAUERGROUP.Shared.Core.Images
{
    public class IndependentImage
    {
        public IndependentImage()
        {
            Content = null;
            Timestamp = DateTime.UtcNow;
        }

        public Byte[]? Content { get; set; }
        public DateTime Timestamp { get; set; }

        public IndependentImageFormat Format { get { return GetFormat(Content); } }
        public Int32 Size { get { return Content == null ? 0 : Content.Length; } }

        public static IndependentImage FromFile(String filename)
        {
            throw new NotImplementedException();
        }

        public static IndependentImage FromStream(Stream stream)
        {
            throw new NotImplementedException();
        }

        public static IndependentImage FromURL(String url)
        {
            throw new NotImplementedException();
        }

        public void Save(Stream stream, IndependentImageFormat format = IndependentImageFormat.PNG)
        {
            throw new NotImplementedException();
        }

        public void Save(String filename, IndependentImageFormat format = IndependentImageFormat.PNG)
        {
            throw new NotImplementedException();
        }

        private static IndependentImageFormat GetFormat(Byte[]? rawData)
        {
            if (rawData == null || rawData.Length == 0)
                return IndependentImageFormat.Unkown;

            var bitmap = Encoding.ASCII.GetBytes("BM");
            var gif = Encoding.ASCII.GetBytes("GIF");
            var png = new Byte[] { 137, 80, 78, 71 };
            var tiff1 = new Byte[] { 73, 73, 42 };
            var tiff2 = new Byte[] { 77, 77, 42 };
            var jpeg1 = new Byte[] { 255, 216, 255, 224 };
            var jpeg2 = new Byte[] { 255, 216, 255, 225 };

            if (bitmap.SequenceEqual(rawData.Take(bitmap.Length)))
                return IndependentImageFormat.BMP;

            if (gif.SequenceEqual(rawData.Take(gif.Length)))
                return IndependentImageFormat.GIF;

            if (png.SequenceEqual(rawData.Take(png.Length)))
                return IndependentImageFormat.PNG;

            if (tiff1.SequenceEqual(rawData.Take(tiff1.Length)))
                return IndependentImageFormat.TIFF;

            if (tiff2.SequenceEqual(rawData.Take(tiff2.Length)))
                return IndependentImageFormat.TIFF;

            if (jpeg1.SequenceEqual(rawData.Take(jpeg1.Length)))
                return IndependentImageFormat.JPEG;

            if (jpeg2.SequenceEqual(rawData.Take(jpeg2.Length)))
                return IndependentImageFormat.JPEG;

            return IndependentImageFormat.Unkown;
        }
    }
}
