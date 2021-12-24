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
 

        public frmSettings()
        {
            InitializeComponent();        
        }

        private void btnStore_Click(object sender, EventArgs e)
        {

        }
        private void btnReset_Click(object sender, EventArgs e)
        {
            tbSize.Value = 100;
            cbStyle.SelectedIndex = cbStyle.Items.Count -1;
            
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
    }
}
