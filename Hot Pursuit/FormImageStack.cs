using AstroImage;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Hot_Pursuit
{
    public partial class FormImageStack : Form
    {
        public FormImageStack(List<FitsFile> fitsNames)
        {
            InitializeComponent();
            FitsFile af = Stack.StraightStack(fitsNames.ToArray());
            AstroPic ap = new AstroPic(af);
            Image baseImage = ap.ResizeImage(ImageBox.Size, true);
            ImageBox.Image = baseImage;
            this.Text = "Image Stack " + fitsNames.Count.ToString();
        }
    }
}
