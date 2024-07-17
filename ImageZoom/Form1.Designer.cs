using System.Windows.Forms;

namespace ImageZoom
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Upload_Button = new System.Windows.Forms.Button();
            this.image_box = new System.Windows.Forms.PictureBox();
            this.cropped_image = new System.Windows.Forms.PictureBox();
            this.Zoom_button = new System.Windows.Forms.Button();
            this.ZoomOut_button = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.image_box)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cropped_image)).BeginInit();
            this.SuspendLayout();
            // 
            // Upload_Button
            // 
            this.Upload_Button.Location = new System.Drawing.Point(556, 250);
            this.Upload_Button.Name = "Upload_Button";
            this.Upload_Button.Size = new System.Drawing.Size(75, 23);
            this.Upload_Button.TabIndex = 0;
            this.Upload_Button.Text = "Upload";
            this.Upload_Button.UseVisualStyleBackColor = true;
            this.Upload_Button.Click += new System.EventHandler(this.UploadButtonClick);
            // 
            // image_box
            // 
            this.image_box.Location = new System.Drawing.Point(13, 12);
            this.image_box.Name = "image_box";
            this.image_box.Size = new System.Drawing.Size(537, 426);
            this.image_box.TabIndex = 1;
            this.image_box.TabStop = false;
            this.image_box.Paint += new System.Windows.Forms.PaintEventHandler(this.ImagePaint);
            this.image_box.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ImageMouseDown);
            this.image_box.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ImageMouseMove);
            this.image_box.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ImageMouseUp);
            // 
            // cropped_image
            // 
            this.cropped_image.Location = new System.Drawing.Point(556, 12);
            this.cropped_image.Name = "cropped_image";
            this.cropped_image.Size = new System.Drawing.Size(232, 232);
            this.cropped_image.TabIndex = 2;
            this.cropped_image.TabStop = false;
            // 
            // Zoom_button
            // 
            this.Zoom_button.Location = new System.Drawing.Point(637, 250);
            this.Zoom_button.Name = "Zoom_button";
            this.Zoom_button.Size = new System.Drawing.Size(75, 23);
            this.Zoom_button.TabIndex = 3;
            this.Zoom_button.Text = "Zoom";
            this.Zoom_button.UseVisualStyleBackColor = true;
            this.Zoom_button.Click += new System.EventHandler(this.Zoom_button_Click);
            // 
            // ZoomOut_button
            // 
            this.ZoomOut_button.Location = new System.Drawing.Point(718, 250);
            this.ZoomOut_button.Name = "ZoomOut_button";
            this.ZoomOut_button.Size = new System.Drawing.Size(70, 23);
            this.ZoomOut_button.TabIndex = 4;
            this.ZoomOut_button.Text = "Zoom Out";
            this.ZoomOut_button.UseVisualStyleBackColor = true;
            this.ZoomOut_button.Click += new System.EventHandler(this.ZoomOut_button_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.ZoomOut_button);
            this.Controls.Add(this.Zoom_button);
            this.Controls.Add(this.cropped_image);
            this.Controls.Add(this.image_box);
            this.Controls.Add(this.Upload_Button);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.image_box)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cropped_image)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Upload_Button;
        private System.Windows.Forms.PictureBox image_box;
        private PictureBox cropped_image;
        private Button Zoom_button;
        private Button ZoomOut_button;
    }
}

