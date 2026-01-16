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

        public Byte[] Content { get; set; }
        public DateTime Timestamp { get; set; }

        public IndependentImageFormat Format { get { return GetFormat(Content); } }
        public Int32 Size { get { return Content == null ? 0 : Content.Length; } }

        public static IndependentImage FromFile(String sFilename)
        {
            throw new NotImplementedException();
        }

        public static IndependentImage FromStream(Stream oStream)
        {
            throw new NotImplementedException();
        }

        public static IndependentImage FromURL(String sURL)
        {
            throw new NotImplementedException();
        }

        public void Save(Stream oStream, IndependentImageFormat eFormat = IndependentImageFormat.PNG)
        {
            throw new NotImplementedException();
        }

        public void Save(String sFilename, IndependentImageFormat eFormat = IndependentImageFormat.PNG)
        {
            throw new NotImplementedException();
        }

        private static IndependentImageFormat GetFormat(Byte[] oRawData)
        {
            if (oRawData == null || oRawData.Length == 0)
                return IndependentImageFormat.Unkown;

            var bBitmap = Encoding.ASCII.GetBytes("BM");
            var bGIF = Encoding.ASCII.GetBytes("GIF");
            var PNG = new Byte[] { 137, 80, 78, 71 };
            var bTIFF1 = new Byte[] { 73, 73, 42 };
            var bTIFF2 = new Byte[] { 77, 77, 42 };
            var bJPEG1 = new Byte[] { 255, 216, 255, 224 };
            var bJPEG2 = new Byte[] { 255, 216, 255, 225 };

            if (bBitmap.SequenceEqual(oRawData.Take(bBitmap.Length)))
                return IndependentImageFormat.BMP;

            if (bGIF.SequenceEqual(oRawData.Take(bGIF.Length)))
                return IndependentImageFormat.GIF;

            if (PNG.SequenceEqual(oRawData.Take(PNG.Length)))
                return IndependentImageFormat.PNG;

            if (bTIFF1.SequenceEqual(oRawData.Take(bTIFF1.Length)))
                return IndependentImageFormat.TIFF;

            if (bTIFF2.SequenceEqual(oRawData.Take(bTIFF2.Length)))
                return IndependentImageFormat.TIFF;

            if (bJPEG1.SequenceEqual(oRawData.Take(bJPEG1.Length)))
                return IndependentImageFormat.JPEG;

            if (bJPEG2.SequenceEqual(oRawData.Take(bJPEG2.Length)))
                return IndependentImageFormat.JPEG;

            return IndependentImageFormat.Unkown;
        }
    }
}
