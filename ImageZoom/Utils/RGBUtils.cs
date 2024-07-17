using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageZoom.Utils
{
    public static class RGBUtils
    {
        public static unsafe Bitmap ToBitmap(byte[] rgbCollection, int width, int height)
        {
            if (rgbCollection == null)
                throw new ArgumentNullException(nameof(rgbCollection), "RGB collection is null");

            if (width <= 0 || height <= 0)
                throw new ArgumentException("Width and height must be greater than zero");

            Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, bitmap.PixelFormat);

            int bytesPerPixel = Image.GetPixelFormatSize(bitmap.PixelFormat) / 8;
            byte* ptr = (byte*)bitmapData.Scan0;

            fixed (byte* rgbPtr = rgbCollection)
            {
                    for (int i = 0; i < width * height * 3; i += 3)
                    {
                    if (i >= rgbCollection.Length - 3) continue;

                        byte r = rgbPtr[i];
                        byte g = rgbPtr[i + 1];
                        byte b = rgbPtr[i + 2];

                        int pixelIndex = (i / 3) * bytesPerPixel;

                        ptr[pixelIndex] = b;
                        ptr[pixelIndex + 1] = g;
                        ptr[pixelIndex + 2] = r;
                        ptr[pixelIndex + 3] = 255;
                    }
            }

            bitmap.UnlockBits(bitmapData);

            return bitmap;
        }
    }
}
