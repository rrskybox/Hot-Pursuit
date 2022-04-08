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
            FitsFile af = Stack.StraightStack(fitsNames.ToArray());
            ap = new AstroPic(af);
            ap.LinearStretch();
            //Add target cross
            //target cross hairs
            Point target;
            double pixSize = 1;
            if (af.FocalLength != 0)
                pixSize = (206.265 / af.FocalLength) * af.XpixSz;
            target = af.RADECtoImageXY(af.RA, af.Dec);
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
