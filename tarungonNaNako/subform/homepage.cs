using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace tarungonNaNako.subform
{
    public partial class homepage : Form
    {
        private Image originalImage;
        private bool isArrowDown = true;
        public homepage()

        {
            InitializeComponent();
            LoadPngImage();
        }

        private void LoadPngImage()
        {
            originalImage = Image.FromFile("C:\\Users\\teche\\source\\repos\\tarungonNaNako\\tarungonNaNako\\Assets (images)\\dropdown-select.png");
            pictureBox2.Image = originalImage;
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox3.Image = originalImage;
            pictureBox3.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            // Toggle visibility of flowLayoutPanel1
            flowLayoutPanel1.Visible = !flowLayoutPanel1.Visible;

            isArrowDown = !isArrowDown; // Toggle between arrow down and arrow right
            float rotationAngle = isArrowDown ? 0 : -90;
            pictureBox2.Image = RotateImage(originalImage, rotationAngle);
        }
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            panel5.Visible = !panel5.Visible;
            isArrowDown = !isArrowDown; // Toggle between arrow down and arrow right
            float rotationAngle = isArrowDown ? 0 : -90;
            pictureBox3.Image = RotateImage(originalImage, rotationAngle);
        }

        private Image RotateImage(Image img, float rotationAngle)
        {
            // Create a new empty bitmap to hold rotated image
            Bitmap bmp = new Bitmap(img.Width, img.Height);
            bmp.SetResolution(img.HorizontalResolution, img.VerticalResolution);

            // Make a graphics object from the empty bitmap
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.Transparent); // Clear the background to avoid overlapping
                // Move rotation point to center of image
                g.TranslateTransform((float)img.Width / 2, (float)img.Height / 2);
                // Rotate
                g.RotateTransform(rotationAngle);
                // Move image back
                g.TranslateTransform(-(float)img.Width / 2, -(float)img.Height / 2);
                // Draw passed in image onto graphics object
                g.DrawImage(img, new Point(0, 0));
            }

            return bmp;
        }

    }
}
