using AstroImage;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Hot_Pursuit
{
    public partial class FormImageStack : Form
    {
        private int zoomDistance;
        private AstroPic ap;

        public FormImageStack(List<FitsFile> fitsNames)
        {
            InitializeComponent();
            Stack iStack = new Stack(fitsNames.ToArray());
            ap = new AstroPic(iStack.FitsStack);
            ap.LinearStretch();
            //Add target cross
            //target cross hairs
            Point target;
            double pixSize = 1;
            if (iStack.FitsStack.FocalLength != 0)
                pixSize = (206.265 / iStack.FitsStack.FocalLength) * iStack.FitsStack.XpixSz;
            target = iStack.FitsStack.RADECtoImageXY(iStack.FitsStack.ObjectRA, iStack.FitsStack.ObjectDec);
            ap.AddCrossHair(target, 400, 5);
            //Create image
            Image baseImage = ap.ResizeImage(ImageBox.Size, true);
            ImageBox.Image = baseImage;
            this.Text = "Image Stack " + fitsNames.Count.ToString();
        }

        private void MouseWheel_Handler(object sender, MouseEventArgs e)
        {
            zoomDistance += e.Delta / 3;
            Size subSize = new Size(ImageBox.Size.Width + zoomDistance, ImageBox.Size.Height + zoomDistance);
            Image baseImage = ap.ResizeImage(subSize, true);
            ImageBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            ImageBox.Image = baseImage;
            return;
        }
    }
}
