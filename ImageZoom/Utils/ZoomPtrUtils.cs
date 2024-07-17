using ImageZoom.Adapters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageZoom.Utils
{
    public static class ZoomPtrUtils
    {
        public static unsafe (byte[], int, int) KTimesZooming(byte[] image, int width, int height, int k)
        {
            width = image.Length / height;
            int newWidth = k * (width - 1) + 1;
            int newHeight = (k * (height - 1) + 1);

            byte[] newImage = new byte[newWidth * newHeight];

            // Row Wide
            for (int y = 0; y < newHeight; y += k)
            {
                for (int x = 0; x < width - 1; x++)
                {
                    int left = (y / k * width + x) * 4;
                    int right = ((y / k) * width + (x + 1)) * 4;

                    if (left >= image.Length) continue;
                    if(right >= image.Length - 4) continue;
                    if ((x + 1) % k == 0) continue;

                    int baseIndex = (y * newWidth + x * k) * 4; 

                    if(baseIndex >= image.Length - x * k) continue;

                    byte aLeft = image[left + 3];
                    byte rLeft = image[left + 0];
                    byte gLeft = image[left + 1];
                    byte bLeft = image[left + 2];

                    byte aRight = image[right + 3];
                    byte rRight = image[right + 0];
                    byte gRight = image[right + 1];
                    byte bRight = image[right + 2];

                    byte opA = (byte)(Math.Abs(aLeft - aRight) / k);
                    byte opR = (byte)(Math.Abs(rLeft - rRight) / k);
                    byte opG = (byte)(Math.Abs(gLeft - gRight) / k);
                    byte opB = (byte)(Math.Abs(bLeft - bRight) / k);

                    newImage[baseIndex + 0] = aLeft;
                    newImage[baseIndex + 1] = rLeft;
                    newImage[baseIndex + 2] = gLeft;
                    newImage[baseIndex + 3] = bLeft;

                    newImage[baseIndex + (k - 1) * 4 + 0] = aRight;
                    newImage[baseIndex + (k - 1) * 4 + 1] = rRight;
                    newImage[baseIndex + (k - 1) * 4 + 2] = gRight;
                    newImage[baseIndex + (k - 1) * 4 + 3] = bRight;

                    for (int i = 0; i < k - 1; i++)
                    {
                        newImage[baseIndex + (i + 1) * 4 + 0] = (byte)(Math.Min(aLeft, aRight) + (i == 0 ? opA : opA * i));
                        newImage[baseIndex + (i + 1) * 4 + 1] = (byte)(Math.Min(rLeft, rRight) + (i == 0 ? opR : opR * i));
                        newImage[baseIndex + (i + 1) * 4 + 2] = (byte)(Math.Min(gLeft, gRight) + (i == 0 ? opG : opG * i));
                        newImage[baseIndex + (i + 1) * 4 + 3] = (byte)(Math.Min(bLeft, bRight) + (i == 0 ? opB : opB * i));
                    }
                }
            }

            // Column Wide
            for (int x = 0; x < newWidth; x += k)
            {
                for (int y = 0; y < newHeight - 2; y++)
                {
                    int baseIndex = (y * newWidth + x) * 4;

                    if (baseIndex >= image.Length) continue;

                    byte aTop = newImage[baseIndex + 3];
                    byte rTop = newImage[baseIndex + 0];
                    byte gTop = newImage[baseIndex + 1];
                    byte bTop = newImage[baseIndex + 2];

                    if (((y + 1) * newWidth + x) * 4 >= newImage.Length) continue;

                    byte aBottom = newImage[((y + 1) * newWidth + x) * 4 + 3];
                    byte rBottom = newImage[((y + 1) * newWidth + x) * 4 + 0];
                    byte gBottom = newImage[((y + 1) * newWidth + x) * 4 + 1];
                    byte bBottom = newImage[((y + 1) * newWidth + x) * 4 + 2];

                    byte opA = (byte)(Math.Abs(aTop - aBottom) / k);
                    byte opR = (byte)(Math.Abs(rTop - rBottom) / k);
                    byte opG = (byte)(Math.Abs(gTop - gBottom) / k);
                    byte opB = (byte)(Math.Abs(bTop - bBottom) / k);

                    // Assign top and bottom pixels
                    newImage[baseIndex + 0] = aTop;
                    newImage[baseIndex + 1] = rTop;
                    newImage[baseIndex + 2] = gTop;
                    newImage[baseIndex + 3] = bTop;

                    if (((y + k - 1) * newWidth + x) * 4 >= newImage.Length) continue;

                    newImage[((y + k - 1) * newWidth + x) * 4 + 0] = aBottom;
                    newImage[((y + k - 1) * newWidth + x) * 4 + 1] = rBottom;
                    newImage[((y + k - 1) * newWidth + x) * 4 + 2] = gBottom;
                    newImage[((y + k - 1) * newWidth + x) * 4 + 3] = bBottom;

                    // Interpolate between top and bottom pixels
                    for (int i = 0; i < k - 1; i++)
                    {
                        newImage[((y + i + 1) * newWidth + x) * 4 + 0] = (byte)(Math.Min(aTop, aBottom) + (i == 0 ? opA : opA * i));
                        newImage[((y + i + 1) * newWidth + x) * 4 + 1] = (byte)(Math.Min(rTop, rBottom) + (i == 0 ? opR : opR * i));
                        newImage[((y + i + 1) * newWidth + x) * 4 + 2] = (byte)(Math.Min(gTop, gBottom) + (i == 0 ? opG : opG * i));
                        newImage[((y + i + 1) * newWidth + x) * 4 + 3] = (byte)(Math.Min(bTop, bBottom) + (i == 0 ? opB : opB * i));
                    }
                }
            }

            return (newImage, newWidth, newHeight);
        }
    }
}
