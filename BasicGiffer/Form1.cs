using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace BasicGiffer
{
    public partial class BasicGiffer : Form
    {
        List<Image> images = new List<Image>();
        List<Image> freshImages = new List<Image>();
        bool loop = true;
        KeyboardHook hook = new KeyboardHook();

        public BasicGiffer()
        {
            InitializeComponent();

            // register the event that is fired after the key press.
            hook.KeyPressed +=
                new EventHandler<KeyPressedEventArgs>(hook_KeyPressed);
            // register the ONLY space combination as hot key.         
            hook.RegisterHotKey( Modifiers.NoRepeat , Keys.Space);

        }
        void hook_KeyPressed(object sender, KeyPressedEventArgs e)
        {
            if (tAnimation.Enabled)
                Stop();
            else Play();
        }


        private void tbFrames_ValueChanged(object sender, EventArgs e)
        {
           SetFrame();
        }

        public void SetFrame()
        {
            if (images.Count > 0)
            {
                pbImage.Image = images[tbFrames.Value - 1];
                lblCurFrame.Text = tbFrames.Value.ToString();
            }
        }

        private void tableLayoutPanel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (saveGIF.ShowDialog() == DialogResult.OK)
            {

               

                Thread saveGifThread = new Thread(new ThreadStart(SaveGif));
                saveGifThread.Start();
                while (saveGifThread.IsAlive)
                {
                    Application.DoEvents();
                    Thread.Sleep(0);
                }


                lblResult.Text = $"File was saved!";
            }
        }


        private bool GetFilename(out string[] filenames, DragEventArgs e)
        {
            bool ret = true;
            List<string> names = new List<string>();
            if ((e.AllowedEffect & DragDropEffects.Copy) == DragDropEffects.Copy)
            {
                Array data = ((IDataObject)e.Data).GetData("FileDrop") as Array;
                if (data != null)
                {
                    for (int i = 0; i < data.Length; i++)
                    {
                        if (data.GetValue(0) is String)
                        {
                            names.Add(((string[])data)[i]);
                            string ext = Path.GetExtension(names.Last()).ToLower();
                            if ((ext != ".jpg") && (ext != ".png") && (ext != ".bmp"))
                            {
                                ret = false;
                            }
                        }
                    }
                }
            }

            if (names.Count == 0)
                ret = false;

            names.Sort();
            filenames = names.ToArray();
            return ret;
        }


        protected bool validData;
        protected string[] filenames;
        protected Thread getImageThread;

        private void BasicGiffer_DragEnter(object sender, DragEventArgs e)
        {
            freshImages.Clear();

            validData = GetFilename(out filenames, e);
            if (validData)
            {
                getImageThread = new Thread(new ThreadStart(LoadImages));
                getImageThread.Start();
                e.Effect = DragDropEffects.Copy;
                lblResult.Text = filenames.Count().ToString() + " are loading ...";
            }
            else
            {
                lblResult.Text = "";
                e.Effect = DragDropEffects.None;
            }

        }

        protected void LoadImages()
        {
            string path;
            foreach (var name in filenames)
            {
                path = name;
                freshImages.Add(new Bitmap(path));
            }
        }

        protected void SaveGif()
        {
            lblResult.Text = $"Writing file 0%";
            var imageArray = images.ToArray();
            double time = 1000 / (double)nudFPS.Value;
            double percentMultiplier = 100 / images.Count;

            using (var stream = new MemoryStream())
            {
                using (var encoder = new BumpKit.GifEncoder(stream, null, null, (int)nudRepeat.Value))
                {
                    for (int i = 0; i < imageArray.Length; i++)
                    {
                        var image = new Bitmap((imageArray[i] as Bitmap));
                        encoder.AddFrame(image, 0, 0, TimeSpan.FromMilliseconds(time));
                        lblResult.Text = $"Writing file {i * percentMultiplier}%";
                    }

                    stream.Position = 0;
                    using (var fileStream = new FileStream(saveGIF.FileName, FileMode.Create, FileAccess.Write, FileShare.None, 4096, false))
                    {
                        stream.WriteTo(fileStream);
                    }
                }
            }
        }

        private void BasicGiffer_DragDrop(object sender, DragEventArgs e)
        {
            if (validData)
            {
                while (getImageThread.IsAlive)
                {
                    Application.DoEvents();
                    Thread.Sleep(0);
                }

                images = freshImages;
                tbFrames.Value = 1;
                tbFrames.Maximum = images.Count();
                UpdateInfo();
                pbImage.Image = images.First();
                btnSave.Enabled = true;
                btnLoop.Enabled = true;
                btnPlay.Enabled = true;
                btnStop.Enabled = true;
                tbFrames.Enabled = true;
            }

        }

        public void UpdateInfo()
        {
            lblResult.Text = $"x{tbFrames.Maximum} frames = {tbFrames.Maximum / nudFPS.Value:F} sec";
            tAnimation.Interval = (int) Math.Round(1000/nudFPS.Value, MidpointRounding.AwayFromZero);
        }

        private void nudFPS_ValueChanged(object sender, EventArgs e)
        {
            UpdateInfo();
        }


        private void tAnimation_Tick(object sender, EventArgs e)
        {
            if (tbFrames.Value == images.Count)
            {
                if (loop)
                {
                    tbFrames.Value = 1;
                }
                else
                {
                    Stop();
                }
                }
            else
            {
                tbFrames.Value++;
            }
        }


        private void playToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Play();

        }

        private void pauseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Stop();
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            Play();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            Stop();
        }

        private void btnLoop_Click(object sender, EventArgs e)
        {
            Loop();
        }

        public void Loop()
        {
            loop = !loop;
            btnLoop.BackColor = loop ? System.Drawing.SystemColors.ControlDark : System.Drawing.SystemColors.Control;
            repeatToolStripMenuItem.Checked = loop;
        }

        public void Play()
        {
            if (tbFrames.Value == images.Count)
                tbFrames.Value = 1;
            tAnimation.Enabled = true;
            btnPlay.BackColor = System.Drawing.SystemColors.ControlDark;
            btnStop.BackColor = System.Drawing.SystemColors.Control;
        }

        public void Stop()
        {
            tAnimation.Enabled = false;
            btnPlay.BackColor = System.Drawing.SystemColors.Control;
            btnStop.BackColor = System.Drawing.SystemColors.ControlDark;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (images.Count > 0)
                MessageBox.Show($"Images loaded: {tbFrames.Maximum} frames\n" +
                    $"First frame size: {images[0].Size.Width}x{images[0].Size.Height}px.\n\n" +
                    $"{Application.ProductName} version: {Application.ProductVersion}\n" +
                    $"Contact: www.ankere.co\n" +
                    $"License: (c) All rights reserved, 2021 Anton Kerezov", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show($"Image info will be availabe after you load frames.\n\n" +
                    $"{Application.ProductName} version: {Application.ProductVersion}\n" +
                    $"Contact: www.ankere.co\n" +
                    $"License: (c) All rights reserved, 2021 Anton Kerezov", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void repeatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Loop();
        }

        private void pbImage_DoubleClick(object sender, EventArgs e)
        {
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                freshImages.Clear();

                filenames = ofd.FileNames;

                getImageThread = new Thread(new ThreadStart(LoadImages));
                getImageThread.Start();
                lblResult.Text = filenames.Count().ToString() + " are loading ...";

                while (getImageThread.IsAlive)
                {
                    Application.DoEvents();
                    Thread.Sleep(0);
                }

                images = freshImages;
                tbFrames.Value = 1;
                tbFrames.Maximum = images.Count();
                UpdateInfo();
                pbImage.Image = images.First();
                btnSave.Enabled = true;
                btnLoop.Enabled = true;
                btnPlay.Enabled = true;
                btnStop.Enabled = true;
                tbFrames.Enabled = true;
            }
        }
    }
}
