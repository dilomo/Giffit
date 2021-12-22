/* Copyright (C) 2021, Anton D. Kerezov, All rights reserved.
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Anton Kerezov <ankere@gmail.com>, December 2021
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

 namespace BasicGiffer
{
    public partial class InfoForm : Form
    {
        public string urlAdress = "";
        public InfoForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void InfoForm_Load(object sender, EventArgs e)
        {
           
        }

        private void linkSupport_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(new ProcessStartInfo { FileName = urlAdress, UseShellExecute = true });
        }
    }
}
