﻿/* Copyright (C) 2021, Anton D. Kerezov, All rights reserved.
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
using System.Threading;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Drawing.Imaging;
using KGySoft.Drawing;
using KGySoft.Drawing.Imaging;

namespace BasicGiffer
{
    public partial class Gifit : Form
    {
        List<Image> originalImages = new List<Image>();
        List<Image> originalImagesLoopBack = new List<Image>();
        List<Image> images = new List<Image>();
        List<Image> imagesLoopBack = new List<Image>();
        System.Collections.Specialized.StringCollection recentFolders 
            = new System.Collections.Specialized.StringCollection();
        bool loopback = false;
        bool preview = true;
        bool zoom = true;
        int repeats = 1; 
        protected bool validData;
        protected string[] filenames;
        protected Thread getImageThread;
        protected Giffit.GiffitPreset settings = new Giffit.GiffitPreset();

        public Gifit()
        {
            InitializeComponent();
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
                            var filename = ((string[])data)[i];
                            string ext = Path.GetExtension(filename).ToLower();
                            if ((ext != ".jpg") && (ext != ".png") && (ext != ".bmp"))
                            {
                                //ret = false;
                            }
                            else
                                names.Add(filename);
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
        private bool GetFilename(out string[] filenames, string folder)
        {
            bool ret = true;
            List<string> names = new List<string>();

            string[] data = Directory.GetFiles(folder);

            if (data != null)
            {
                for (int i = 0; i < data.Length; i++)
                {
                    var filename = data[i];
                    string ext = Path.GetExtension(filename).ToLower();
                    if ((ext != ".jpg") && (ext != ".png") && (ext != ".bmp"))
                    {
                        //ret = false;
                    }
                    else
                        names.Add(filename);
                }
            }

            if (names.Count == 0)
                ret = false;

            names.Sort();
            filenames = names.ToArray();
            return ret;
        }
        public void SetFrame()
        {
            if (images.Count > 0)
            {
                pbImage.Image = images[tbFrames.Value - 1];
                lblCurFrame.Text = tbFrames.Value.ToString();
            }
        }
        protected void OpenFiles()
        {
            DisposeImages();
            UpdateTitleBar($"- {filenames.Count().ToString()} files are loading ... ");
            AddRecent(filenames);

            getImageThread = new Thread(new ThreadStart(LoadImages));
            getImageThread.Start();

            while (getImageThread.IsAlive)
            {
                Application.DoEvents();
                Thread.Sleep(0);
            }

            tbFrames.Value = 1;
            tbFrames.Maximum = images.Count();
            UpdateInfo();
            pbImage.Image = images.First();
            pbImage.BackgroundImage = null;
            btnSave.Enabled = true;
            btnSettings.Enabled = true;
            btnLoop.Enabled = true;
            btnPlay.Enabled = true;
            btnStop.Enabled = true;
            tbFrames.Enabled = true;
            btnPreview.Enabled = true;
            saveGIFToolStripMenuItem.Enabled = true;
            addToolStripMenuItem.Enabled = true;
            UpdateTitleBar("");
        }
        protected void AddFiles()
        { 
            UpdateTitleBar($"- {filenames.Count().ToString()} files are loading ... ");
            AddRecent(filenames);

            getImageThread = new Thread(new ThreadStart(LoadImages));
            getImageThread.Start();

            while (getImageThread.IsAlive)
            {
                Application.DoEvents();
                Thread.Sleep(0);
            }
            tbFrames.Maximum = images.Count();
            UpdateInfo();
            UpdateTitleBar("");

        }
        private void AddRecent(string[] filenames)
        {
            clearrecentToolStripMenuItem.Enabled = true;
            var dir = filenames.First();
            if (!recentFolders.Contains(Path.GetDirectoryName(dir)))
                recentFolders.Add(Path.GetDirectoryName(dir));
        }
        protected void LoadImages()
        {
            int i = images.Count;
            foreach (var name in filenames)
            {
                Image img;
                Image imgOriginal;
                // release file lock
                using (var bmpTemp = new Bitmap(name))
                {
                    // store original files for non destructive editing
                    imgOriginal = new Bitmap(bmpTemp);
                    imgOriginal.Tag = i;
                    originalImages.Add(imgOriginal);
                    // store preview image
                    img = new Bitmap(bmpTemp);
                    img.Tag = i;
                    if (preview)
                        images.Add(ApplyEffects(img, settings));
                    else
                        images.Add(img);
                }
                i++;
            }

        }
        protected void DisposeImages()
        {
            foreach (var img in images)
            {
                img.Dispose();
            }
            images.Clear();
            foreach (var img in imagesLoopBack)
            {
                img.Dispose();
            }
            imagesLoopBack.Clear();
            foreach (var img in originalImages)
            {
                img.Dispose();
            }
            originalImages.Clear();
        }
        protected void SaveGif()
        {
            var imageArray = images.ToArray();
            double time = 1000 / (double)nudFPS.Value;
            double percentMultiplier = 100 / images.Count;

            using (var stream = new MemoryStream())
            {
                using (var encoder = new BumpKit.GifEncoder(stream, null, null, (int)-1))
                {
                    for (int i = 0; i < imageArray.Length; i++)
                    {
                        var image = new Bitmap((imageArray[i] as Bitmap));
                        encoder.AddFrame(image, 0, 0, TimeSpan.FromMilliseconds(time));      
                        UpdateTitleBar($" - Writing file { i * percentMultiplier}%");
                    }
                    stream.Position = 0;
                    using (var fileStream = new FileStream(saveGIF.FileName, FileMode.Create, FileAccess.Write, FileShare.None, 81920, false))
                    {
                        stream.WriteTo(fileStream);
                    }
                }
            }
        }
        public static Image ApplyEffects(Image image, Giffit.GiffitPreset preset)
        {
            if (preset.Scaling < 1)
                image = (Bitmap)ScaleImage(image, (int)(image.Width * preset.Scaling), (int)(image.Height * preset.Scaling));

            Bitmap quantizedBitmap;
            if (preset.StyleIndex < Giffit.GiffitPreset.StyleNames.Count - 1)
                quantizedBitmap = image.ConvertPixelFormat(preset.pixFormat, preset.quantizer, preset.ditherer);
            else quantizedBitmap = (Bitmap) image;

            return quantizedBitmap;
        }
        public void PreviewEffects(bool active)
        {
            Stop();
            btnPlay.Enabled = false;

            if (images.Count > 0)
            {
                foreach (var img in images)
                    img.Dispose();
                images.Clear();
            }
            if (imagesLoopBack.Count > 0)
            {
                foreach (var img in imagesLoopBack)
                    img.Dispose();
                imagesLoopBack.Clear();
            }


            if (active)
            {
                lblResult.Text = "Applying image settings ...";
                Application.DoEvents();

                var imagesNew = new ConcurrentBag<Image>();
                Parallel.ForEach(originalImages, frame =>
                {
                    CloneAdd(imagesNew, frame, settings);
                });
                images = imagesNew.OrderBy(bm => (int) bm.Tag).ToList();

                //foreach (var frame in originalImages)
                //    images.Add(ApplyEffects(new Bitmap(frame), settings));

                lblResult.Text = images[5].Tag.ToString();

                if (loopback)
                {
                    var imagesNewLB = new ConcurrentBag<Image>();
                    Parallel.ForEach(originalImagesLoopBack, frame =>
                    {
                        CloneAdd(imagesNew, frame, settings);
                    });
                    imagesLoopBack = imagesNewLB.OrderBy(bm => (int) bm.Tag).ToList();
                    //foreach (var frame in originalImagesLoopBack)
                    //    imagesLoopBack.Add(ApplyEffects(new Bitmap(frame), settings));

                    images.AddRange(imagesLoopBack);
                }
            }
            else
            {

                foreach (var frame in originalImages)
                    CloneAdd(images, frame);
                
                if (loopback)
                {
                    foreach (var frame in originalImagesLoopBack)
                        CloneAdd(imagesLoopBack, frame);

                    images.AddRange(imagesLoopBack);
                }
            }

            btnPlay.Enabled = true;
            UpdateInfo();
            SetFrame();
        }
        private static void CloneAdd(List<Image> list, Image frame)
        {
            var mod = new Bitmap(frame);
            mod.Tag = frame.Tag;
            list.Add(mod);
        }
        private static void CloneAdd(List<Image> list, Image frame, Giffit.GiffitPreset style)
        {
            Image mod = new Bitmap(frame);
            mod = ApplyEffects(mod, style);
            mod.Tag = frame.Tag;
            list.Add(mod);
        }
        private static void CloneAdd(ConcurrentBag<Image> list, Image frame)
        {
            var mod = new Bitmap(frame);
            mod.Tag = frame.Tag;
            list.Add(mod);
        }
        private static void CloneAdd(ConcurrentBag<Image> list, Image frame, Giffit.GiffitPreset style)
        {
            Image mod = new Bitmap(frame);
            mod = ApplyEffects(mod, style);
            mod.Tag = frame.Tag;
            list.Add(mod);
        }
        public static Image ScaleImage(Image image, int maxWidth, int maxHeight)
        {
            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);

            var newImage = new Bitmap(newWidth, newHeight);

            using (var graphics = Graphics.FromImage(newImage))
                graphics.DrawImage(image, 0, 0, newWidth, newHeight);

            return newImage;
        }
        public void UpdateTitleBar(string text)
        {
            if (this.InvokeRequired)
            {
                // Call this same method but append THREAD2 to the text
                Action safeWrite = delegate { UpdateTitleBar(text); };
                this.Invoke(safeWrite);
            }
            else
                this.Text = "Giffit " + text;
        }
        public void UpdateInfo()
        {
            lblResult.Text = $"of {tbFrames.Maximum} frames = {tbFrames.Maximum / nudFPS.Value:F} sec";
            tAnimation.Interval = (int)Math.Round(1000 / nudFPS.Value, MidpointRounding.AwayFromZero);
        }
        public void Loop( bool loop)
        {
            if (loop)
            {
                lblResult.Text = "Creating loopback ...";
                Application.DoEvents();
                imagesLoopBack = images.Select(i => (Image)new Bitmap(i)).ToList();
                for (int i = 0; i < images.Count; i++)
                     imagesLoopBack[i].Tag = images[i].Tag;
                imagesLoopBack.Reverse();
                originalImagesLoopBack = originalImages.Select(i => (Image)new Bitmap(i)).ToList();
                for (int i = 0; i < originalImages.Count; i++)
                    originalImagesLoopBack[i].Tag = originalImages[i].Tag;
                originalImagesLoopBack.Reverse();
                images.AddRange(imagesLoopBack);
            }
            else
            {
                lblResult.Text = "Removing loopback ...";
                Application.DoEvents();
                foreach (var img in imagesLoopBack)
                    img.Dispose();
                imagesLoopBack.Clear();
                foreach (var img in originalImagesLoopBack)
                    img.Dispose();
                originalImagesLoopBack.Clear();

                images.RemoveRange(images.Count / 2, images.Count / 2);
                if (tbFrames.Value > images.Count)
                     tbFrames.Value = images.Count;
            }          
            tbFrames.Maximum = images.Count();
            btnLoop.BackColor = loopback ? System.Drawing.SystemColors.ControlDark : System.Drawing.SystemColors.Control;
            UpdateInfo();
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


        private void tbFrames_ValueChanged(object sender, EventArgs e)
        {
            SetFrame();
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (saveGIF.ShowDialog() == DialogResult.OK)
            {
                UpdateTitleBar($" - Writing file ...");
                Thread saveGifThread = new Thread(new ThreadStart(SaveGif));
                saveGifThread.Start();
                while (saveGifThread.IsAlive)
                {
                    Application.DoEvents();
                    Thread.Sleep(0);
                }
                UpdateTitleBar($" - Saved to {Path.GetFileName(saveGIF.FileName)}");
            }
        }
        private void BasicGiffer_DragEnter(object sender, DragEventArgs e)
        {
            validData = GetFilename(out filenames, e);
            if (validData)
            {
                e.Effect = DragDropEffects.Copy;
                UpdateTitleBar($"- {filenames.Count().ToString()} files can be loaded");
            }
            else
            {
                UpdateTitleBar($"- none of the files dragged is supported");
                e.Effect = DragDropEffects.None;
            }
        }
        private void BasicGiffer_DragDrop(object sender, DragEventArgs e)
        {
            if (validData)
            {
                OpenFiles();
            }
        }
        private void Gifit_DragLeave(object sender, EventArgs e)
        {
            UpdateTitleBar("");
        }
        private void nudFPS_ValueChanged(object sender, EventArgs e)
        {
            UpdateInfo();
        }
        private void tAnimation_Tick(object sender, EventArgs e)
        {
            if (tbFrames.Value == images.Count)
            {
                if (nudRepeat.Value == 0)
                    tbFrames.Value = 1;
                else if (repeats < nudRepeat.Value)
                {
                    repeats++;
                    tbFrames.Value = 1;
                }
                else
                {
                    repeats = 1;
                    Stop();
                }
            }
            else
                tbFrames.Value++;
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
            loopback = !loopback;
            Loop(loopback);
        }
        private void pbImage_DoubleClick(object sender, EventArgs e)
        {
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                filenames = ofd.FileNames;
                OpenFiles();
            }
        }
        protected override bool ProcessCmdKey(ref Message message, Keys keys)
        {
            switch (keys)
            {
                case Keys.Space:
                    if (images.Count > 0)
                    {
                        if (tAnimation.Enabled)
                            Stop();
                        else Play();
                    }
                    return true; // signal that we've processed this key

                case Keys.Control | Keys.S:
                    btnSave.PerformClick();
                    return true;
            }

            // run base implementation
            return base.ProcessCmdKey(ref message, keys);
        }
        private void nudFPS_KeyUp(object sender, KeyEventArgs e)
        {
            //nudFPS.Value = nudFPS.Value;

            //if (nudFPS.Value >= 1000)
            //{
            //    nudFPS.Value = 1000;
            //}
            //else if ( nudFPS.Value <1)
            //{
            //    nudFPS.Value = 1;
            //}
        }
        private void Gifit_Load(object sender, EventArgs e)
        {
            nudRepeat.Value = (int)Giffit.Properties.Settings.Default["Repeats"];
            nudFPS.Value = (int)Giffit.Properties.Settings.Default["FPS"];
            recentFolders = (System.Collections.Specialized.StringCollection)Giffit.Properties.Settings.Default["Recents"];
            settings.Scaling = (decimal)Giffit.Properties.Settings.Default["Scaling"];
            //settings.StyleIndex = (int)Giffit.Properties.Settings.Default["StyleIndex"];
            // preview = (bool)Giffit.Properties.Settings.Default["Preview"];

            if (recentFolders.Count > 0)
                clearrecentToolStripMenuItem.Enabled = true;

            //btnPreview.BackColor = preview ? System.Drawing.SystemColors.ControlDark : System.Drawing.SystemColors.Control;

            lblResult.Text = "";
        }
        private void Gifit_FormClosing(object sender, FormClosingEventArgs e)
        {
            Giffit.Properties.Settings.Default["Repeats"] = (int) nudRepeat.Value;
            Giffit.Properties.Settings.Default["FPS"] = (int) nudFPS.Value;
            Giffit.Properties.Settings.Default["Recents"] = recentFolders;
            Giffit.Properties.Settings.Default["Scaling"] = settings.Scaling;
            Giffit.Properties.Settings.Default["StyleIndex"] = settings.StyleIndex;
            Giffit.Properties.Settings.Default["Preview"] = preview;
            Giffit.Properties.Settings.Default.Save(); // Saves settings in application configuration file
        }
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var info = "";
            if (images.Count > 0)
                info += $"Images loaded: {tbFrames.Maximum} frames\n" +
                    $"First frame size: {images[0].Size.Width}x{images[0].Size.Height}px.\n\n";
            else
                info += $"Image info will be availabe after you load frames.\n\n\n";

            info += $"{Application.ProductName} version: {Application.ProductVersion}\n" +
                $"KGySoft version: { System.Reflection.Assembly.GetAssembly(typeof(KGySoft.Drawing.Imaging.AnimatedGifConfiguration)).GetName().Version.ToString()}\n" +
                    $"Giffit License: Freeware\n" +   
                    $"©2021 Anton Kerezov, All Rights Reserved.";

            InfoForm frm = new InfoForm();
            frm.urlAdress = "https://www.ankere.co/news/gifit-the-animated-gif-tool-for-architects/";
            frm.lblInfo.Text = info;
            frm.ShowDialog();

        }
        private void nudRepeat_ValueChanged(object sender, EventArgs e)
        {
            repeats = 1;
        }


       

        private void btnSettings_Click(object sender, EventArgs e)
        {
            if (images.Count == 0)
            { MessageBox.Show("No data loaded"); return; };

            Giffit.frmSettings sets = new Giffit.frmSettings();

            sets.cbStyle.Items.AddRange(Giffit.GiffitPreset.StyleNames.ToArray());  
            sets.cbStyle.SelectedIndex = settings.StyleIndex;
            sets.tbSize.Value = (int) Math.Ceiling(settings.Scaling * 100);
            sets.h = images[0].Size.Height;
            sets.w = images[0].Size.Width;

            if (sets.ShowDialog() == DialogResult.OK)
            {
                if (settings.StyleIndex != sets.cbStyle.SelectedIndex || settings.Scaling != (decimal)sets.tbSize.Value / 100)
                {
                    settings.StyleIndex = sets.cbStyle.SelectedIndex;
                    settings.Scaling = (decimal)sets.tbSize.Value / 100;
                    if (preview)
                        PreviewEffects(true);
                }
            }
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            zoom = !zoom;

            if (zoom)
                pbImage.SizeMode = PictureBoxSizeMode.Zoom;
            else
                pbImage.SizeMode = PictureBoxSizeMode.CenterImage;

            btnPreview.BackColor = !zoom ? System.Drawing.SystemColors.ControlDark : System.Drawing.SystemColors.Control;
            
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                filenames = ofd.FileNames;
                OpenFiles();
            }
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                filenames = ofd.FileNames;
                AddFiles();
            }
        }

        private void saveGIFToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnSave.PerformClick();
        }

        private void clearrecentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            recentfoldersToolStripMenuItem.Enabled = false;
            recentFolders.Clear();
        }

        private void cmsActions_Opening_1(object sender, CancelEventArgs e)
        {
            cmsRecents.Items.Clear();
            if (recentFolders.Count > 0)
                recentfoldersToolStripMenuItem.Enabled = true;
            else
                recentfoldersToolStripMenuItem.Enabled = false;

            foreach (var folder in recentFolders)
            {
                if (Directory.Exists(folder) && Directory.GetFiles(folder).Length > 0)
                {
                    ToolStripMenuItem rf = new ToolStripMenuItem(Path.GetFileName(folder.TrimEnd(Path.DirectorySeparatorChar)));
                    rf.ToolTipText = folder;
                    rf.Click += Rf_Click;
                    cmsRecents.Items.Add(rf);
                }
            }
        }
        private void Rf_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem ts = (ToolStripMenuItem)sender;

            validData = GetFilename(out filenames, ts.ToolTipText);
            if (validData)
            {
                OpenFiles();
            }
            else
            {
                MessageBox.Show("No supported files were found in this recent directiry");
            }
        }
    }
}