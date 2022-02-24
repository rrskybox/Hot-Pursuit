using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AstroImage;

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
            this.Text = "Image Stack "+ fitsNames.Count.ToString();
        }
    }
}
