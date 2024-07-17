using ImageZoom.Adapters;
using ImageZoom.Utils;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ImageZoom
{
    public partial class Form1 : Form
    {
        private bool IsDrawing = false;
        private Point startPoint;
        private Rectangle rect;
        private float X_Scale = 1;
        private float Y_Scale = 1;

        public Form1()
        {
            InitializeComponent();
            image_box.SizeMode = PictureBoxSizeMode.Zoom;
            cropped_image.SizeMode = PictureBoxSizeMode.CenterImage;
        }

        private void UploadButtonClick(object sender, EventArgs e)
        {
            UploadImage(image_box, true);
            if (image_box.Image != null)
            {
                X_Scale = (float)image_box.Image.Width / image_box.ClientSize.Width;
                Y_Scale = (float)image_box.Image.Height / image_box.ClientSize.Height;
            }
        }

        private void ImageMouseDown(object sender, MouseEventArgs e)
        {
            if (image_box.Image == null) return;

            IsDrawing = true;
            startPoint = e.Location;
        }

        private void ImageMouseMove(object sender, MouseEventArgs e)
        {
            if (IsDrawing)
            {
                int width = Math.Abs(e.X - startPoint.X);
                int height = Math.Abs(e.Y - startPoint.Y);
                int x = Math.Min(e.X, startPoint.X);
                int y = Math.Min(e.Y, startPoint.Y);
                rect = new Rectangle(x, y, width, height);
                image_box.Invalidate();
            }
        }

        private void ImageMouseUp(object sender, MouseEventArgs e)
        {
            IsDrawing = false;

            var croppedImage = CropImage(image_box, rect);

            cropped_image.Image = croppedImage;

        }

        private void ImagePaint(object sender, PaintEventArgs e)
        {
            if (rect != null && rect.Width > 0 && rect.Height > 0)
            {
                e.Graphics.DrawRectangle(Pens.Red, rect);
            }
        }

        private Bitmap CropImage(PictureBox pictureBox, Rectangle cropRect)
        {
            if (pictureBox.Image == null || cropRect.Width <= 0 || cropRect.Height <= 0)
                return null;

            int scaledX = (int)(cropRect.X * X_Scale);
            int scaledY = (int)(cropRect.Y * Y_Scale);
            int scaledWidth = (int)(cropRect.Width * X_Scale);
            int scaledHeight = (int)(cropRect.Height * Y_Scale);

            Rectangle scaledRect = new Rectangle(scaledX, scaledY, scaledWidth, scaledHeight);

            Bitmap sourceBitmap = new Bitmap(pictureBox.Image);
            Bitmap croppedBitmap = new Bitmap(scaledRect.Width, scaledRect.Height);

            using (Graphics g = Graphics.FromImage(croppedBitmap))
            {
                g.DrawImage(sourceBitmap, new Rectangle(0, 0, croppedBitmap.Width, croppedBitmap.Height),
                            scaledRect,
                            GraphicsUnit.Pixel);
            }

            return croppedBitmap;
        }

        private void ResizeImage(int scale)
        {
            if (cropped_image.Image == null) return;

            Bitmap result = ZoomUtils.ZoomBitmap((Bitmap)cropped_image.Image, scale);
            
            cropped_image.Image = result;
        }

        private void ZoomOutImage(int scale)
        {
            if (cropped_image.Image == null) return;

            byte[] image = cropped_image.ToRgbArray();

            byte[] newImage = ZoomUtils.ZoomOut(image, cropped_image.Image.Width, cropped_image.Image.Height, scale);

            cropped_image.Image = RGBUtils.ToBitmap(newImage, cropped_image.Image.Width / scale, cropped_image.Image.Height / scale);
        }

        private void ResizeImage2(int scale)
        {
            if (cropped_image.Image == null) return;

            byte[] data = cropped_image.ToRgbArray();

            byte[] result = ZoomUtils.Zoom(data, cropped_image.Image.Width, cropped_image.Image.Height, scale);

            int newWidth = scale * (cropped_image.Image.Width - 1) + 1;
            int newHeight = scale * (cropped_image.Image.Height - 1) + 1;

            Bitmap image = RGBUtils.ToBitmap(result, newWidth, newHeight);

            cropped_image.Image = image;
        }

        private void ZoomOutImage2(int scale)
        {
            if (cropped_image.Image == null) return;

            byte[] image = cropped_image.ToRgbArray();

            byte[] newImage = ZoomUtils.ZoomOut2(image, cropped_image.Image.Width, cropped_image.Image.Height, scale);
            cropped_image.Image = RGBUtils.ToBitmap(newImage, ((cropped_image.Image.Width - 1) / scale + 1), (cropped_image.Image.Height - 1) / scale + 1);
        }


        private void Zoom_button_Click(object sender, EventArgs e)
        {
            //ResizeImage(3);
            ResizeImage2(3);
        }
        private void ZoomOut_button_Click(object sender, EventArgs e)
        {
            //ZoomOutImage(3);
            ZoomOutImage2(3);
        }
        private void UploadImage(PictureBox pictureBox, bool resizeBox = false)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.jpg, *.jpeg, *.png, *.gif, *.bmp)|*.jpg; *.jpeg; *.png; *.gif; *.bmp|All files (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = true
            };

            if (openFileDialog.ShowDialog() != DialogResult.OK) return;

            try
            {
                var selectedFile = openFileDialog.FileName;

                pictureBox.Image = Image.FromFile(selectedFile);

                if (resizeBox) pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Resim yüklenirken bir hata oluştu: " + ex.Message);
            }
        }

        private byte[] ImageToPixelArray(Image imageIn)
        {
            using (Bitmap bitmap = new Bitmap(imageIn))
            {
                int width = bitmap.Width;
                int height = bitmap.Height;
                byte[] pixelData = new byte[width * height * 4];

                Rectangle rect = new Rectangle(0, 0, width, height);
                BitmapData bmpData = bitmap.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

                Marshal.Copy(bmpData.Scan0, pixelData, 0, pixelData.Length);

                bitmap.UnlockBits(bmpData);

                return pixelData;
            }
        }

    }
}
