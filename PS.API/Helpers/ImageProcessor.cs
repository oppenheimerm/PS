using SkiaSharp;
using System.Drawing;
using System.IO;
using System.Security.Policy;
using static PS.API.Helpers.Constants;

namespace PS.API.Helpers
{
    public class ImageProcessor
    {
        const int Quality = 68;
        const int imageSize = 720;


        public FileInfo CreateImage(Stream imageStream, string filePath)
        {
            FileStream fileStream;

            string dir = Path.Combine(Path.GetDirectoryName(filePath));
            string displayName = Path.GetFileNameWithoutExtension(filePath);
            string ext = Path.GetExtension(filePath);

            //Directory.CreateDirectory(dir);

            var format = GetFormat(filePath);

            using (var inputStream = new SKManagedStream(imageStream))
            using (var codec = SKCodec.Create(inputStream))
            using (var original = SKBitmap.Decode(codec))
            using (var image = HandleOrientation(original, codec.EncodedOrigin))
            {
                int width = (int)Constants.ImageType.Cover;
                int height = (int)Math.Round(width * ((float)image.Height / image.Width));

                string thumbnailPath = Path.Combine(dir, $"{displayName}-{width}x{height}{ext}");
                var info = new SKImageInfo(width, height);

                using (var resized = image.Resize(info, SKBitmapResizeMethod.Lanczos3))
                using (var thumb = SKImage.FromBitmap(resized))
                using (var fs = new FileStream(thumbnailPath, FileMode.CreateNew, FileAccess.ReadWrite))
                {
                    thumb.Encode(format, Quality).SaveTo(fs);

                    fileStream = fs;
                    //fs.n
                    return new FileInfo(fs.Name);
                }
            }
        }

        private static SKEncodedImageFormat GetFormat(string fileName)
        {
            string ext = Path.GetExtension(fileName.ToLowerInvariant());

            switch (ext)
            {
                case ".gif":
                    return SKEncodedImageFormat.Gif;
                case ".png":
                    return SKEncodedImageFormat.Png;
                case ".webp":
                    return SKEncodedImageFormat.Webp;
            }

            return SKEncodedImageFormat.Jpeg;
        }

        // Got the code from https://stackoverflow.com/a/45620498/1074470
        private static SKBitmap HandleOrientation(SKBitmap bitmap, SKEncodedOrigin orientation)
        {
            SKBitmap rotated;
            switch (orientation)
            {
                case SKEncodedOrigin.BottomRight:

                    using (var surface = new SKCanvas(bitmap))
                    {
                        surface.RotateDegrees(180, bitmap.Width / 2, bitmap.Height / 2);
                        surface.DrawBitmap(bitmap.Copy(), 0, 0);
                    }

                    return bitmap;

                case SKEncodedOrigin.RightTop:
                    rotated = new SKBitmap(bitmap.Height, bitmap.Width);

                    using (var surface = new SKCanvas(rotated))
                    {
                        surface.Translate(rotated.Width, 0);
                        surface.RotateDegrees(90);
                        surface.DrawBitmap(bitmap, 0, 0);
                    }

                    return rotated;

                case SKEncodedOrigin.LeftBottom:
                    rotated = new SKBitmap(bitmap.Height, bitmap.Width);

                    using (var surface = new SKCanvas(rotated))
                    {
                        surface.Translate(0, rotated.Height);
                        surface.RotateDegrees(270);
                        surface.DrawBitmap(bitmap, 0, 0);
                    }

                    return rotated;

                default:
                    return bitmap;
            }
        }
    }
}
