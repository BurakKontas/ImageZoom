using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ImageZoom.Adapters
{
    public class BitmapConverter
    {
        public static Bitmap ByteArrayToBitmap(byte[] byteArray)
        {
            using (MemoryStream ms = new MemoryStream(byteArray))
            {
                return new Bitmap(ms);
            }
        }

        public static Bitmap PixelArrayToBitmap(byte[] pixelData, int width, int height)
        {
            Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);

            Rectangle rect = new Rectangle(0, 0, width, height);
            BitmapData bmpData = bitmap.LockBits(rect, ImageLockMode.WriteOnly, bitmap.PixelFormat);

            Marshal.Copy(pixelData, 0, bmpData.Scan0, pixelData.Length);
            bitmap.UnlockBits(bmpData);

            return bitmap;
        }

        public static byte[] BitmapToByteArray(Bitmap bitmap)
        {
            int width = bitmap.Width;
            int height = bitmap.Height;

            PixelFormat format = PixelFormat.Format32bppArgb;

            BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, format);

            int bytes = Math.Abs(bitmapData.Stride) * height;

            byte[] rgbValues = new byte[bytes];

            Marshal.Copy(bitmapData.Scan0, rgbValues, 0, bytes);

            bitmap.UnlockBits(bitmapData);

            return rgbValues;
        }
    }
}
