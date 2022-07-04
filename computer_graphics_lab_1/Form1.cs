using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace сomputer_graphics_lab_1
{
    public partial class MainForm : Form
    {
        Bitmap image;
        public MainForm()
        {
            InitializeComponent();
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Image file | *.png; *.jpg; *.bmp | All Files | *.*";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                image = new Bitmap(dialog.FileName);
                PictureBoxImage.Image = image;
                PictureBoxImage.Refresh();
            }
        }

        private void инверсияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap resultImage = Filter.Filter.InvertExecute(image);
            PictureBoxImage.Image = resultImage;
            image = resultImage;
            PictureBoxImage.Refresh();
        }

        private void оттенкиСерогоToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap resultImage = Filter.Filter.GrayScaleExecute(image);
            PictureBoxImage.Image = resultImage;
            image = resultImage;
            PictureBoxImage.Refresh();
        }

        private void сепияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap resultImage = Filter.Filter.SepiaExecute(image);
            PictureBoxImage.Image = resultImage;
            image = resultImage;
            PictureBoxImage.Refresh();
        }

        private void увеличениеЯркостиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap resultImage = Filter.Filter.BrightnessExecute(image);
            PictureBoxImage.Image = resultImage;
            image = resultImage;
            PictureBoxImage.Refresh();
        }

        private void сдвигToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap resultImage = Filter.Filter.ShiftExecute(image);
            PictureBoxImage.Image = resultImage;
            image = resultImage;
            PictureBoxImage.Refresh();
        }

        private void тиснениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap resultImage = Filter.Filter.EmbossingExecute(image);
            PictureBoxImage.Image = resultImage;
            image = resultImage;
            PictureBoxImage.Refresh();
        }

        private void размытиеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap resultImage = Filter.Filter.MotionBlurExecute(image);
            PictureBoxImage.Image = resultImage;
            image = resultImage;
            PictureBoxImage.Refresh();
        }

        private void серыйМирToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap resultImage = Filter.Filter.GrayWorldExecute(image);
            PictureBoxImage.Image = resultImage;
            image = resultImage;
            PictureBoxImage.Refresh();
        }

        private void растяжениеКонтрастностиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap resultImage = Filter.Filter.AutoLevelsExecute(image);
            PictureBoxImage.Image = resultImage;
            image = resultImage;
            PictureBoxImage.Refresh();
        }

        private void идеальныйОтражательToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap resultImage = Filter.Filter.PerfectReflectorExecute(image);
            PictureBoxImage.Image = resultImage;
            image = resultImage;
            PictureBoxImage.Refresh();
        }

        private void dilationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap resultImage = Filter.Filter.DilationExecute(image);
            PictureBoxImage.Image = resultImage;
            image = resultImage;
            PictureBoxImage.Refresh();
        }

        private void erosionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap resultImage = Filter.Filter.ErosionExecute(image);
            PictureBoxImage.Image = resultImage;
            image = resultImage;
            PictureBoxImage.Refresh();
        }

        private void медианныйФильтрToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap resultImage = Filter.Filter.MedianExecute(image);
            PictureBoxImage.Image = resultImage;
            image = resultImage;
            PictureBoxImage.Refresh();
        }

        private void фильтрСобеляToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap resultImage = Filter.Filter.SobelExecute(image);
            PictureBoxImage.Image = resultImage;
            image = resultImage;
            PictureBoxImage.Refresh();
        }

        private void фильтрЩарраToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap resultImage = Filter.Filter.ScharrExecute(image);
            PictureBoxImage.Image = resultImage;
            image = resultImage;
            PictureBoxImage.Refresh();
        }
    }
}
