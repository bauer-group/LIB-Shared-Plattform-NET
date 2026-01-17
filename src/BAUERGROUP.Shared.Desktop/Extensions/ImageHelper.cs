using BAUERGROUP.Shared.Core.Internet;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Net;

namespace BAUERGROUP.Shared.Desktop.Extensions
{
    public static class ImageHelper
    {
        public static Image FromURL(string url)
        {
            return FromURL(null!, url);
        }

        public static Image FromURL(this Image? image, string url)
        {
            Stream imageStream = DownloadUtils.DownloadContentToStream(url);

            var returnImage = Image.FromStream(imageStream);

            imageStream.Close();

            return returnImage;
        }

        public static Image? ResizeImage(this Image? image, Size sizeTarget, bool centerImage = false)
        {
            if (image == null || sizeTarget.IsEmpty)
                return null;

            int originalWidth = image.Size.Width;
            int originalHeight = image.Size.Height;

            int targetWidth = sizeTarget.Width;
            int targetHeight = sizeTarget.Height;

            var targetBitmap = new Bitmap(targetWidth, targetHeight);

            var targetGraphic = Graphics.FromImage(targetBitmap);

            targetGraphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
            targetGraphic.SmoothingMode = SmoothingMode.HighQuality;
            targetGraphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
            targetGraphic.CompositingQuality = CompositingQuality.HighQuality;

            double targetRatioX = (double)targetWidth / (double)originalWidth;
            double targetRatioY = (double)targetHeight / (double)originalHeight;
            double targetRatio = targetRatioX < targetRatioY ? targetRatioX : targetRatioY;

            int newHeight = Convert.ToInt32(originalHeight * targetRatio);
            int newWidth = Convert.ToInt32(originalWidth * targetRatio);

            int posX = Convert.ToInt32((targetWidth - (image.Width * targetRatio)) / 2);
            int posY = Convert.ToInt32((targetHeight - (image.Height * targetRatio)) / 2);

            if (!centerImage)
            {
                posX = 0;
                posY = 0;
            }

            targetGraphic.Clear(Color.White);

            targetGraphic.DrawImage(image, posX, posY, newWidth, newHeight);

            var imageCodeInfo = ImageCodecInfo.GetImageEncoders();
            var encoderParameters = new EncoderParameters(1);
            encoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, 100L);

            var targetStream = new System.IO.MemoryStream();

            targetBitmap.Save(targetStream, imageCodeInfo[1], encoderParameters);

            return Image.FromStream(targetStream);
        }
    }
}
