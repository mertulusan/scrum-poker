using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;

namespace ScrumPoker.Common
{
    public class ScreenShareHelper
    {
        public static byte[] TakeScreenshot()
        {
            using var bitmap = new Bitmap(1920, 1080);

            using (var g = Graphics.FromImage(bitmap))
            using (MemoryStream stream = new MemoryStream())

            {
                g.CopyFromScreen(0, 0, 0, 0, bitmap.Size, CopyPixelOperation.SourceCopy);
                
                return ResizeImage(bitmap, 1280, 720);
            }

            //using var bitmap = new Bitmap(1920, 1080);
            //using (var g = Graphics.FromImage(bitmap))
            //using (MemoryStream stream = new MemoryStream())
            //{
            //    g.CopyFromScreen(0, 0, 0, 0, bitmap.Size, CopyPixelOperation.SourceCopy);

            //    bitmap.Save(stream, ImageFormat.Jpeg);

            //    return stream.ToArray();
            //}
        }

        public static byte[] ResizeImage(Bitmap bitmap, int width, int height)
        {
            using (Image image = bitmap)
            using (Image newImage = new Bitmap(image, width, height))
            using (MemoryStream stream = new MemoryStream())
            {
                newImage.Save(stream, ImageFormat.Jpeg);
                return stream.ToArray();
            }
        }
    }
}
