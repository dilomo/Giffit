using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Giffit
{
    public partial class frmSettings : Form
    {
        public int w = 640;
        public int h = 480;
        public int defI = 0;

        public frmSettings()
        {
            InitializeComponent();        
        }

        private void btnStore_Click(object sender, EventArgs e)
        {

        }
        private void btnReset_Click(object sender, EventArgs e)
        {
            cd.Color = Color.White;
            label1.BackColor = Color.White;
            tbSize.Value = 100;
            cbStyle.SelectedIndex = defI;
            cbUseDefault.Checked = false;
            cbDontPreview.Checked = true;

            var nw = w * tbSize.Value / 100;
            var nh = h * tbSize.Value / 100;
            lblsize.Text = $"{nw}x{nh}px ({tbSize.Value}%)";
        }

        private void tbSize_Scroll(object sender, EventArgs e)
        {
           var  nw = w * tbSize.Value / 100;
           var  nh = h * tbSize.Value / 100;
           lblsize.Text = $"{nw}x{nh}px ({tbSize.Value}%)";
        }

        private void frmSettings_Load(object sender, EventArgs e)
        {
            var nw = w * tbSize.Value / 100;
            var nh = h * tbSize.Value / 100;
            lblsize.Text = $"{nw}x{nh}px ({tbSize.Value}%)";
        }

        private void label1_Click(object sender, EventArgs e)
        {
            cd.ShowDialog();
            label1.BackColor = cd.Color;
        }

        private void label1_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        public Color Background
        {
            get {
                return cd.Color;
            }
            set {
                cd.Color = value;
                label1.BackColor = value;
            }
        }

        private void label1_MouseEnter(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.Cross;
        }
    }
}
