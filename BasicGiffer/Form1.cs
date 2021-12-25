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
        List<Image> previewImages = new List<Image>();
        List<Image> previewImagesLoopBack = new List<Image>();
        System.Collections.Specialized.StringCollection recentFolders 
            = new System.Collections.Specialized.StringCollection();
        bool loopback = false;
        bool preview = true;
        bool preserveStyle = false;
        bool zoom = true;
        bool oneToOne = false;
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
            if (previewImages.Count > 0)
            {
                pbImage.Image = previewImages[tbFrames.Value - 1];
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

            PreviewEffects(true);

            tbFrames.Value = 1;
            tbFrames.Maximum = previewImages.Count();
            UpdateInfo();
            pbImage.Image = previewImages.First();
            pbImage.BackgroundImage = null;
            btnSave.Enabled = true;
            btnSettings.Enabled = true;
            btnLoop.Enabled = true;
            btnPlay.Enabled = true;
            btnStop.Enabled = true;
            tbFrames.Enabled = true;
            btnPreview.Enabled = true;
            copyStripMenuItem.Enabled = true;
            saveGIFToolStripMenuItem.Enabled = true;
            addToolStripMenuItem.Enabled = true;
            UpdateTitleBar("");
            AdjustWindowToImage();
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
            tbFrames.Maximum = previewImages.Count();
            PreviewEffects(true);
            UpdateInfo();
            UpdateTitleBar("");
        }
        private void OpenWithDialog()
        {
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                filenames = ofd.FileNames;
                OpenFiles();
            }
        }
        private void AddWithDialog()
        {
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                filenames = ofd.FileNames;
                AddFiles();
            }
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
            int i = previewImages.Count;
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
                    previewImages.Add(img);
                }
                i++;
            }

        }
        protected void DisposeImages()
        {
            foreach (var img in previewImages)
            {
                img.Dispose();
            }
            previewImages.Clear();
            foreach (var img in previewImagesLoopBack)
            {
                img.Dispose();
            }
            previewImagesLoopBack.Clear();
            foreach (var img in originalImages)
            {
                img.Dispose();
            }
            originalImages.Clear();
        }
        protected void SaveGifAnimation()
        {
            var imageArray = previewImages.ToArray();
            double time = 1000 / (double)nudFPS.Value;
            double percentMultiplier = 100 / previewImages.Count;

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
        protected void SaveGifFrame()
        {
            using (var stream = new MemoryStream())
            {
                using (var encoder = new BumpKit.GifEncoder(stream, null, null, (int)-1))
                {
                    var image = new Bitmap((pbImage.Image as Bitmap));
                    encoder.AddFrame(image, 0, 0, TimeSpan.FromMilliseconds(0));

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

            if (preset.StyleIndex != preset.DefaultStyle)
                image = image.ConvertPixelFormat(preset.pixFormat, preset.quantizer, preset.ditherer);
 
            return image;
        }
        public void PreviewEffects(bool active)
        {
            btnSettings.Image = Giffit.Properties.Resources.spinner_20;
            Application.DoEvents();
            Stop();
            btnPlay.Enabled = false;

            var previewThread = new Thread(() => GeneratePreview(active));
            previewThread.Start();
            while (previewThread.IsAlive)
            {
                Application.DoEvents();
                Thread.Sleep(0);
            }


            btnPlay.Enabled = true;
            btnSettings.Image = Giffit.Properties.Resources.Settings;
            UpdateInfo();
            SetFrame();
        }

        private void GeneratePreview(bool active)
        {
            if (previewImages.Count > 0)
            {
                foreach (var img in previewImages)
                    img.Dispose();
                previewImages.Clear();
            }
            if (previewImagesLoopBack.Count > 0)
            {
                foreach (var img in previewImagesLoopBack)
                    img.Dispose();
                previewImagesLoopBack.Clear();
            }


            if (active)
            {
                var imagesNew = new ConcurrentBag<Image>();
                Parallel.ForEach(originalImages, frame =>
                {
                    CloneAdd(imagesNew, frame, settings);

                });
                previewImages = imagesNew.OrderBy(bm => (int)bm.Tag).ToList();

                if (loopback)
                {
                    var imagesNewLB = new ConcurrentBag<Image>();
                    Parallel.ForEach(originalImagesLoopBack, frame =>
                    {
                        CloneAdd(imagesNewLB, frame, settings);
                    });
                    previewImagesLoopBack = imagesNewLB.OrderBy(bm => (int)bm.Tag).ToList();

                    previewImages.AddRange(previewImagesLoopBack);
                }
            }
            else
            {
                foreach (var frame in originalImages)
                    CloneAdd(previewImages, frame);

                if (loopback)
                {
                    foreach (var frame in originalImagesLoopBack)
                        CloneAdd(previewImagesLoopBack, frame);

                    previewImages.AddRange(previewImagesLoopBack);
                }
            }
        }

        private static void CloneAdd(List<Image> list, Image frame)
        {
            var mod = new Bitmap(frame);
            mod.Tag = frame.Tag;
            list.Add(mod);
        }
        private static void CloneAdd(List<Image> list, Image frame, Giffit.GiffitPreset style, bool applyQuantizer = true)
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
        private static void CloneAdd(ConcurrentBag<Image> list, Image frame, Giffit.GiffitPreset style, bool applyQuantizer = true)
        {
            Image mod = new Bitmap(frame);
            mod = ApplyEffects(mod, style);
            mod.Tag = frame.Tag;
            list.Add(mod);
        }
        public static Image ScaleImage(Image image, int Width, int Height)
        {
            var ratioX = (double)Width / image.Width;
            var ratioY = (double)Height / image.Height;
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
        public void UpdateInfo(string text)
        {
            if (this.InvokeRequired)
            {
                // Call this same method but append THREAD2 to the text
                Action safeWrite = delegate { UpdateInfo(text); };
                this.Invoke(safeWrite);
            }
            else
                lblResult.Text = text;
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
                UpdateInfo("Creating loopback ...");
                Application.DoEvents();

                // creat loopbacks for the two sets of data
                previewImagesLoopBack = previewImages.Select(i => (Image)new Bitmap(i)).ToList();
                previewImagesLoopBack.Reverse();
                for (int i = 0; i < previewImages.Count; i++)
                    previewImagesLoopBack[i].Tag = previewImages[i].Tag;
             

                // and now for the originasl
                originalImagesLoopBack = originalImages.Select(i => (Image)new Bitmap(i)).ToList();
                originalImagesLoopBack.Reverse();
                for (int i = 0; i < originalImages.Count; i++)
                    originalImagesLoopBack[i].Tag = originalImages[i].Tag;
                
                previewImages.AddRange(previewImagesLoopBack);
            }
            else
            {
                UpdateInfo ("Removing loopback ...");
                Application.DoEvents();
                foreach (var img in previewImagesLoopBack)
                    img.Dispose();
                previewImagesLoopBack.Clear();
                foreach (var img in originalImagesLoopBack)
                    img.Dispose();
                originalImagesLoopBack.Clear();

                previewImages.RemoveRange(previewImages.Count / 2, previewImages.Count / 2);
                if (tbFrames.Value > previewImages.Count)
                     tbFrames.Value = previewImages.Count;
            }          
            tbFrames.Maximum = previewImages.Count();
            btnLoop.BackColor = loopback ? System.Drawing.SystemColors.ControlDark : System.Drawing.SystemColors.Control;
            UpdateInfo();
        }
        public void Play()
        {
            if (tbFrames.Value == previewImages.Count)
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
        private void AdjustWindowToImage()
        {
            oneToOne = false;

            // contol size to add later
            var width = this.Width - pbImage.Width;
            var height = this.Height - pbImage.Height;
            var maxW = this.MaximumSize.Width - width;
            var maxH = this.MaximumSize.Height - height;


            // new total size we aim for
            var W = pbImage.Image.Size.Width;
            var H = pbImage.Image.Size.Height;

            this.AutoSize = true;
            if ((H <= maxH && W <= maxW))
            {
                this.Size = new Size(W+width, H+height);
                if (pbImage.SizeMode == PictureBoxSizeMode.Zoom)
                {
                    oneToOne = true;
                    btnPreview.PerformClick();
                }
            }
            else if (this.MinimumSize.Height - height > H && this.MinimumSize.Width - width > W)
            {
                var w = MinimumSize.Width - W;
                var h = MinimumSize.Height - H;
                var ratio = (double)Math.Min(w, h) / Math.Max(W, H);

                this.Size = new Size((int)Math.Truncate(W * ratio), (int)Math.Truncate(H * ratio));

                if (pbImage.SizeMode == PictureBoxSizeMode.CenterImage)
                {
                    btnPreview.PerformClick();
                }
            }
            else if (H >maxH && W <= maxW)
            {
                var ratio = (double)W / H;
                this.Size = new Size((int)Math.Truncate((MaximumSize.Height - height) * ratio) + width, MaximumSize.Height);
                if (pbImage.SizeMode == PictureBoxSizeMode.CenterImage)
                {
                    btnPreview.PerformClick();
                }
            }
            else if (H <= maxH && W > maxW)
            {
                var ratio = (double)H / W;
                this.Size = new Size(MaximumSize.Width, (int)Math.Truncate((MaximumSize.Width - width) * ratio) + height);
                if (pbImage.SizeMode == PictureBoxSizeMode.CenterImage)
                {
                    btnPreview.PerformClick();
                }
            }
            else if (H > maxH && W > maxW)
            {
                // horizontal
                if (W > H)
                {
                    var ratio = (double)W / H;
                    this.Size = new Size((int)Math.Truncate((MaximumSize.Height - height) * ratio) + width, MaximumSize.Height);
                    //var ratio = (double)H / W;
                    //this.Size = new Size(MaximumSize.Width, (int)Math.Truncate((MaximumSize.Width - width) * ratio) + height);
                }
                else
                {
                    var ratio = (double)W / H;
                    this.Size = new Size((int)Math.Truncate((MaximumSize.Height - height) * ratio) + width, MaximumSize.Height);   
                }
                if (pbImage.SizeMode == PictureBoxSizeMode.CenterImage)
                {
                    btnPreview.PerformClick();
                }
            }

            this.AutoSize = false;
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
                if (saveGIF.FilterIndex == 1)
                {
                    Thread saveGifThread = new Thread(new ThreadStart(SaveGifAnimation));
                    saveGifThread.Start();
                    while (saveGifThread.IsAlive)
                    {
                        Application.DoEvents();
                        Thread.Sleep(0);
                    }
                }
                else
                {
                    SaveGifFrame();
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
                this.Activate();
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
            if (tbFrames.Value == previewImages.Count)
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
            OpenWithDialog();
        }


        protected override bool ProcessCmdKey(ref Message message, Keys keys)
        {
            switch (keys)
            {
                case Keys.Space:
                    if (previewImages.Count > 0)
                    {
                        if (tAnimation.Enabled)
                            Stop();
                        else Play();
                    }
                    return true; // signal that we've processed this key

                case Keys.Control | Keys.S:
                    if (previewImages.Count > 0)
                    {
                        btnSave.PerformClick();
                    }
                    return true;
                case Keys.Control | Keys.O:
                    OpenWithDialog();
                    return true;
                case Keys.Control | Keys.I:
                    AddWithDialog();
                    return true;
                case Keys.Control | Keys.C:
                    if (previewImages.Count > 0)
                        Clipboard.SetDataObject(pbImage.Image);
                    return true;
            }

            // run base implementation
            return base.ProcessCmdKey(ref message, keys);
        }


        private void nudFPS_KeyUp(object sender, KeyEventArgs e)
        {
            nudFPS.Focus();
        }
        private void nudRepeat_KeyUp(object sender, KeyEventArgs e)
        {
            nudRepeat.Focus();
        }
        private void nudFPS_Leave(object sender, EventArgs e)
       {


        }
        private void Gifit_Load(object sender, EventArgs e)
        {
            nudRepeat.Value = (int)Giffit.Properties.Settings.Default["Repeats"];
            nudFPS.Value = (int)Giffit.Properties.Settings.Default["FPS"];
            recentFolders = (System.Collections.Specialized.StringCollection)Giffit.Properties.Settings.Default["Recents"];
            settings.Scaling = (decimal)Giffit.Properties.Settings.Default["Scaling"];
            preserveStyle = (bool)Giffit.Properties.Settings.Default["KeepStyle"];
            settings.StyleIndex = (int)Giffit.Properties.Settings.Default["StyleIndex"];

            if (!preserveStyle)
                settings.StyleIndex = settings.DefaultStyle;

            if (recentFolders.Count > 0)
                clearrecentToolStripMenuItem.Enabled = true;

            lblResult.Text = "";
        }
        private void Gifit_FormClosing(object sender, FormClosingEventArgs e)
        {
            Giffit.Properties.Settings.Default["Repeats"] = (int) nudRepeat.Value;
            Giffit.Properties.Settings.Default["FPS"] = (int) nudFPS.Value;
            Giffit.Properties.Settings.Default["Recents"] = recentFolders;
            Giffit.Properties.Settings.Default["Scaling"] = settings.Scaling;
            Giffit.Properties.Settings.Default["StyleIndex"] = settings.StyleIndex;
            Giffit.Properties.Settings.Default["KeepStyle"] = preserveStyle;
            Giffit.Properties.Settings.Default.Save(); // Saves settings in application configuration file
        }
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var info = "";
            if (previewImages.Count > 0)
                info += $"1 of {tbFrames.Maximum} frames is {previewImages[0].Size.Width}x{previewImages[0].Size.Height}px\n" +
                    $"Encoding: {previewImages[0].PixelFormat.ToString()}\n\n";
            else
                info += $"Image info will be availabe after you load frames.\n\n";

                info += $"{Application.ProductName} version: {Application.ProductVersion}\n" +
                $"KGySoft module: { System.Reflection.Assembly.GetAssembly(typeof(KGySoft.Drawing.Imaging.AnimatedGifConfiguration)).GetName().Version.ToString()}\n\n" +
                $"License: Freeware";

            // $"©2021 Anton Kerezov, All Rights Reserved.";

            InfoForm frm = new InfoForm();
            frm.urlAdress = "https://www.ankere.co/news/giffit-the-animated-gif-tool-for-architects/";
            frm.lblInfo.Text = info;
            frm.ShowDialog();

        }
        private void nudRepeat_ValueChanged(object sender, EventArgs e)
        {
            repeats = 1;
        }
        private void btnSettings_Click(object sender, EventArgs e)
        {
            if (previewImages.Count == 0)
            { MessageBox.Show("Processing images, please wait ..."); return; };

            Giffit.frmSettings sets = new Giffit.frmSettings();

            sets.cbStyle.Items.AddRange(Giffit.GiffitPreset.StyleNames.ToArray());  
            sets.cbStyle.SelectedIndex = settings.StyleIndex;
            sets.tbSize.Value = (int) Math.Ceiling(settings.Scaling * 100);
            sets.cbPersistent.Checked = preserveStyle;
            sets.h = previewImages[0].Size.Height;
            sets.w = previewImages[0].Size.Width;

            if (sets.ShowDialog() == DialogResult.OK)
            {
                if (settings.StyleIndex != sets.cbStyle.SelectedIndex || settings.Scaling != (decimal)sets.tbSize.Value / 100 || sets.cbPersistent.Checked != preserveStyle)
                {
                    settings.StyleIndex = sets.cbStyle.SelectedIndex;
                    settings.Scaling = (decimal)sets.tbSize.Value / 100;
                    preserveStyle = sets.cbPersistent.Checked;
                    PreviewEffects(preview);
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
            AddWithDialog();
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
        private void Gifit_SizeChanged(object sender, EventArgs e)
        {
            if (oneToOne)
            {
                oneToOne = false;
                btnPreview.PerformClick();
            }
        }
        private void copyStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject(pbImage.Image);
        }

        bool tok = false;
        private void tStatus_Tick(object sender, EventArgs e)
        {
            tok = !tok;

            if (tok)
              UpdateInfo("Applying image settings 　・");
            else
              UpdateInfo("Applying image settings ・　");

            Application.DoEvents();
        }
    }
}