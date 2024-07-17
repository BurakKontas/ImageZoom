using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageZoom.Adapters
{
    public static class PictureBoxAdapter
    {
        public static byte[] ToRgbArray(this PictureBox pictureBox)
        {
            if (pictureBox.Image == null)
            {
                throw new ArgumentNullException(nameof(pictureBox), "PictureBox.Image is null");
            }

            Bitmap bitmap = new Bitmap(pictureBox.Image);
            int width = bitmap.Width;
            int height = bitmap.Height;
            byte[] result = new byte[width * height * 3];

            for (int i = 0; i < width * height * 3; i += 3)
            {
                int x = (i == 0) ? 0 : ((int)(i / 3)) % (width);
                int y = ((int)(i / 3)) / (width);

                Color pixelColor = bitmap.GetPixel(x, y);
                byte r = pixelColor.R;
                byte g = pixelColor.G;
                byte b = pixelColor.B;

                result[i] = r;
                result[i + 1] = g;
                result[i + 2] = b;
            }

            return result;
        }
    }
}
