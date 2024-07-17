using ImageZoom.Adapters;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Numerics;

namespace ImageZoom.Utils
{
    public static class ZoomUtils
    {
        public static Bitmap ZoomBitmap(Bitmap originalImage, int k)
        {
            int width = originalImage.Width;
            int height = originalImage.Height;

            int zoomedWidth = width * k;
            int zoomedHeight = height * k;

            Bitmap zoomedImage = new Bitmap(zoomedWidth, zoomedHeight);

            using(Graphics g = Graphics.FromImage(zoomedImage))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;

                g.DrawImage(originalImage, new Rectangle(0, 0, zoomedWidth, zoomedHeight));
            }

            return zoomedImage;
        }

        public static unsafe byte[] Zoom(byte[] imageData, int width, int height, int zoomFactor)
        {
            int newWidth = zoomFactor * (width - 1) + 1;
            int newHeight = zoomFactor * (height - 1) + 1;
            byte[] zoomedImageData = new byte[newWidth * newHeight * 3];

            fixed (byte* pOriginal = imageData)
            fixed (byte* pZoomed = zoomedImageData)
            {
                // Row-wise zooming
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width - 1; x++)
                    {
                        int originalIndex = (y * width + x) * 3;
                        int zoomedStartIndex = (y * zoomFactor * newWidth + x * zoomFactor) * 3;

                        byte* pSrc = pOriginal + originalIndex;

                        for (int i = 0; i < 3; i++)
                        {
                            byte originalPixel = pSrc[i];
                            byte nextPixel = pSrc[i + 3];

                            int diff = nextPixel - originalPixel;
                            int step = diff / zoomFactor;

                            pZoomed[zoomedStartIndex + i] = originalPixel;

                            for (int k = 1; k < zoomFactor; k++)
                            {
                                pZoomed[zoomedStartIndex + i + k * 3] = (byte)(originalPixel + step * k);
                            }
                        }
                    }
                }

                // Column-wise zooming
                for (int x = 0; x < newWidth; x++)
                {
                    for (int y = 0; y < height - 1; y++)
                    {
                        int originalIndex = (y * width + x / zoomFactor) * 3;
                        int zoomedStartIndex = (y * zoomFactor * newWidth + x) * 3;

                        byte* pSrc = pOriginal + originalIndex;

                        for (int i = 0; i < 3; i++)
                        {
                            byte originalPixel = pSrc[i];
                            byte nextPixel = pOriginal[originalIndex + width * 3];

                            int diff = nextPixel - originalPixel;
                            int step = diff / zoomFactor;

                            pZoomed[zoomedStartIndex + i] = originalPixel;

                            for (int k = 1; k < zoomFactor; k++)
                            {
                                pZoomed[zoomedStartIndex + k * newWidth * 3 + i] = (byte)(originalPixel + step * k);
                            }
                        }
                    }
                }
            }

            return zoomedImageData;
        }


        public static unsafe byte[] ZoomOut2(byte[] zoomedData, int zoomedWidth, int zoomedHeight, int zoomFactor)
        {
            int originalWidth = ((zoomedWidth - 1) / zoomFactor + 1);
            int originalHeight = (zoomedHeight - 1) / zoomFactor + 1;
            byte[] zoomedoutImageData = new byte[originalWidth * originalHeight * 3];

            fixed (byte* pZoomed = zoomedData)
            fixed (byte* pZoomedout = zoomedoutImageData)
            {
                int imageIndex = 0;
                for(int y = 0; y < zoomedHeight ; y+=(zoomFactor))
                {
                    for(int x = 0; x < zoomedWidth; x+=(zoomFactor))
                    {
                        int index = (y * zoomedWidth * 3) + x * 3;
                        zoomedoutImageData[imageIndex++] = zoomedData[index++];
                        zoomedoutImageData[imageIndex++] = zoomedData[index++];
                        zoomedoutImageData[imageIndex++] = zoomedData[index++];
                     }
                }
            }

            return zoomedoutImageData;
        }
        public static unsafe byte[] ZoomOut(byte[] zoomedImageData, int zoomedWidth, int zoomedHeight, int zoomFactor)
        {
            int originalWidth = zoomedWidth / zoomFactor;
            int originalHeight = zoomedHeight / zoomFactor;
            byte[] originalImageData = new byte[originalWidth * originalHeight * 3];

            fixed (byte* pZoomed = zoomedImageData)
            fixed (byte* pOriginal = originalImageData)
            {
                for (int y = 0; y < originalHeight; y++)
                {
                    for (int x = 0; x < originalWidth; x++)
                    {
                        int zoomedX = x * zoomFactor;
                        int zoomedY = y * zoomFactor;

                        int zoomedIndex = (zoomedY * zoomedWidth + zoomedX) * 3;
                        int originalIndex = (y * originalWidth + x) * 3;

                        byte* pSrc = pZoomed + zoomedIndex;
                        byte* pDest = pOriginal + originalIndex;

                        for (int i = 0; i < 3; i++)
                        {
                            pDest[i] = pSrc[i];
                        }
                    }
                }
            }

            return originalImageData;
        }
    }
}
