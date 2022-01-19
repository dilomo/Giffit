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
        bool processing = false;
        bool preserveStyle = false;
        bool zoom = true;
        bool oneToOne = false;
        int repeats = 1; 
        protected bool validData;
        protected string[] filenames;
        protected Thread getImageThread;
        protected Giffit.GiffitPreset settings = new Giffit.GiffitPreset();
        string animationFolder = "animation";
        bool folderDrop = false;

        public Gifit()
        {
            InitializeComponent();
        }

      
        private bool GetFilename(out string[] filenames, DragEventArgs e)
        {
            bool result = true;
            List<string> names = null;
            if ((e.AllowedEffect & DragDropEffects.Copy) == DragDropEffects.Copy)
            {
                string[] data = ((IDataObject)e.Data).GetData("FileDrop") as string[];
                names = AllowedFiles(data, out folderDrop);
            }

            if (names == null)
                result = false;

            names.Sort();
            filenames = names.ToArray();
            return result;
        }
        private bool GetFilename(out string[] filenames, string folder)
        {
            bool result = true;
            List<string> names = null;

            names = AllowedFiles(new string[] { folder }, out folderDrop);

            if (names == null)
                result = false;
            names.Sort();
            filenames = names.ToArray();
            return result;
        }
        /// <summary>
        /// Get file names that can be loaded
        /// </summary>
        /// <param name="data"></param>
        /// <param name="suggestedFilename">A name suggestion based on context</param>
        /// <returns></returns>
        private static List<string> AllowedFiles(string[] data, out bool isFolder)
        {
            isFolder = false;
            List<string> names = new List<string>();
            if (data != null)
            {
                for (int i = 0; i < data.Length; i++)
                {
                    if (data.GetValue(0) is String)
                    {
                        var filename = ((string[])data)[i];
                        string[] files;

                        if (Directory.Exists(filename))
                        {
                            files = Directory.GetFiles(filename);
                            isFolder = true;
                        }
                        else
                            files = new string[] { filename };

                        foreach (var file in files)
                        {
                            string ext = Path.GetExtension(file).ToLower();
                            if ((ext != ".gif") && (ext != ".jpg") && (ext != ".png") && (ext != ".bmp") && (ext != ".tiff"))
                            {
                                //ret = false;
                            }
                            else
                                names.Add(file);
                        }
                    }
                }
            }

            if (names.Count == 0)
                return null;
           
            return names;
        }
        public void SetFrame()
        {
            if (previewImages.Count > 0)
            {
                pbImage.Image = previewImages[tbFrames.Value - 1];
                lblCurFrame.Text = tbFrames.Value.ToString();
            }
            else pbImage.Image = null;
        }
        protected void OpenFiles()
        {
          
            DisposeImages();
            DisableActions();
            UpdateInfo($"{filenames.Count().ToString()} files are loading ... ");
            AddRecent(filenames);

            animationFolder = Path.GetDirectoryName(filenames[0]);

            getImageThread = new Thread(new ThreadStart(LoadImages));
            getImageThread.Start();

            while (getImageThread.IsAlive)
            {
                Application.DoEvents();
                Thread.Sleep(0);
            }
      
            tbFrames.Value = 1;
            tbFrames.Maximum = previewImages.Count();
            pbImage.Image = null;
            btnSettings.Enabled = true;
            if (preview)
                  PreviewEffects(preview);
            pbImage.SizeMode = PictureBoxSizeMode.Zoom;
            pbImage.Image = previewImages.First();   
            AdjustWindowToImage();
            pbImage.BackgroundImage = null;
            tbFrames.Enabled = true;
            copyStripMenuItem.Enabled = true;
            saveGIFToolStripMenuItem.Enabled = true;
            addToolStripMenuItem.Enabled = true;
            UpdateInfo();
            EnableActions();
        }
        protected void AddFiles()
        {
            DisableActions();
            UpdateInfo($"{filenames.Count().ToString()} files are loading ... ");
            AddRecent(filenames);

            getImageThread = new Thread(new ThreadStart(LoadImages));
            getImageThread.Start();

            while (getImageThread.IsAlive)
            {
                Application.DoEvents();
                Thread.Sleep(0);
            }
            tbFrames.Maximum = previewImages.Count();
            PreviewEffects(preview);
            UpdateInfo();
            EnableActions();
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
                if (loopback)
                {
                    Loop(false);
                    AddFiles();
                    Loop(true);
                }
                else
                  AddFiles();
            }
        }
        private void AddRecent(string[] filenames)
        {
            clearrecentToolStripMenuItem.Enabled = true;
            var dir = filenames.First();
            if (!recentFolders.Contains(Path.GetDirectoryName(dir)))
                recentFolders.Add(Path.GetDirectoryName(dir));

            // add gif as recent entry 
            if (filenames.Count() == 1 && Path.GetExtension(filenames.First()) == ".gif")
            {
                if (!recentFolders.Contains(dir))
                    recentFolders.Add(dir);
            }
        }
        protected void LoadImages()
        {
            int i = previewImages.Count;
            foreach (var name in filenames)
            {
                Image previewImg;
                Image originalImg;
                // release file lock
                using (var temp = (Bitmap)Image.FromFile(name, true))
                {
                    var possibleFrames = BitmapExtensions.ExtractBitmaps(temp);

                    foreach (var tempFrame in possibleFrames)
                    {
                        // store original files for non destructive editing
                        originalImg = tempFrame.CloneCurrentFrame();
                        originalImg.Tag = i;
                        originalImages.Add(originalImg);

                        // store preview image
                        previewImg = tempFrame.CloneCurrentFrame();
                        previewImg.Tag = i;
                        previewImages.Add(previewImg);
                        i++;
                        UpdateInfo($"Adding frame {i}");
                    }
                }
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
            foreach (var img in originalImagesLoopBack)
            {
                img.Dispose();
            }
            originalImagesLoopBack.Clear();
            // get pointer to delete later
            this.Invoke(new MethodInvoker(delegate ()
            {
                if (pbImage.Image != null)
                {
                    pbImage.Image.Dispose();
                    pbImage.Image = null;
                }
            }));
        }
        protected void SaveGifAnimation(double time)
        {
            double percentMultiplier = 100 / previewImages.Count;
            IEnumerable<IReadableBitmapData> imageArray;

            if (settings.HighQuality || preview == false)
            {
                List<Image> allframes = new List<Image>();
                allframes.AddRange(originalImages);
                allframes.AddRange(originalImagesLoopBack);
                allframes = allframes.Select(i => ApplyEffects(i, settings)).ToList();
                imageArray = allframes.Select(i => BitmapExtensions.GetReadableBitmapData((Bitmap)i.Clone()));
            }
            else
                imageArray = previewImages.Select(i => BitmapExtensions.GetReadableBitmapData((Bitmap)i.Clone()));

            var agf = new AnimatedGifConfiguration(imageArray, TimeSpan.FromMilliseconds(time));
            agf.AnimationMode = (AnimationMode)nudRepeat.Value;
            agf.SizeHandling = AnimationFramesSizeHandling.Center;
            agf.ReportOverallProgress = true;
            agf.ReplaceZeroDelays = false;
            agf.AllowDeltaFrames = false;
            agf.EncodeTransparentBorders = true;

            if (settings.OptimisedQuantizer)
                agf.AllowDeltaFrames = true;

            // grayscale bugfix for now
            if (settings.StyleIndex == 9) agf.Quantizer = settings.quantizer;

            if (settings.HighQuality)
            {
                agf.AllowDeltaFrames = true;
                agf.Quantizer = settings.quantizer;
                agf.Ditherer = settings.ditherer;
            }
    

            using (var stream = new MemoryStream())
            {
                var progresseReporter = new TaskConfig();
                progresseReporter.Progress = new SaveProgress(saveGIF.FileName, UpdateInfo);

                GifEncoder.EncodeAnimationAsync(agf, stream, progresseReporter).Wait();

                stream.Position = 0;
                using (var fileStream = new FileStream(saveGIF.FileName, FileMode.Create, FileAccess.Write, FileShare.None, 131072, false))
                {
                    stream.WriteTo(fileStream);
                }
            }
        }
        private class SaveProgress : IDrawingProgress
        {
            string name;
            Action<string> show; 
            #region Methods
            public SaveProgress(string file, Action<string> displayFunc )
            {
                name = file;
                show = displayFunc;
            }
            
            void IDrawingProgress.Report(DrawingProgress progress) => show($"Saving {Convert.ToInt32((double)progress.CurrentValue/progress.MaximumValue*100)}% ...");
            void IDrawingProgress.New(DrawingOperation operationType, int maximumValue, int currentValue) => show($"Saving file ...");
            void IDrawingProgress.Increment() { }
            void IDrawingProgress.SetProgressValue(int value) => show($"({value})");
            void IDrawingProgress.Complete() => show($"File saved to {name}");
#if !(NET35 || NET40)
            void IProgress<DrawingProgress>.Report(DrawingProgress value) => ((IDrawingProgress)this).Report(value);
#endif

            #endregion
        }
        protected void SaveGifAnimation_BumpKit(double time)
        {
            var imageArray = previewImages.ToArray();
            double percentMultiplier = 100 / previewImages.Count;
            using (var stream = new MemoryStream())
            {
                using (var encoder = new BumpKit.GifEncoder(stream, null, null, (int)nudRepeat.Value))
                {
                    for (int i = 0; i < imageArray.Length; i++)
                    {
                        var image = new Bitmap(imageArray[i]);
                        encoder.AddFrame(image, 0, 0, TimeSpan.FromMilliseconds(time));
                        UpdateInfo($"Writing file { i * percentMultiplier}%");
                    }
                    stream.Position = 0;
                    using (var fileStream = new FileStream(saveGIF.FileName, FileMode.Create, FileAccess.Write, FileShare.None, 131072, false))
                    {
                        stream.WriteTo(fileStream);
                    }
                }
            }
        }
        protected void SaveGifFrame()
        {
            ImageExtensions.SaveAsGif(pbImage.Image, saveGIF.FileName);
        }
        protected void SavePNGFrame()
        {
            pbImage.Image.Save(saveGIF.FileName, ImageFormat.Png); 
        }
        protected void SaveJpgFrame()
        {
          //  var enc = new EncoderParameters();
          //  enc.Param = new EncoderParameter[1] { new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 80) };
            pbImage.Image.Save(saveGIF.FileName, ImageFormat.Jpeg);
        }
        protected void SaveTIFfFrame()
        {
            pbImage.Image.Save(saveGIF.FileName, ImageFormat.Tiff);
        }
        public static Image ApplyEffects(Image image, Giffit.GiffitPreset preset)
        {
            if (preset.Scaling < 1)
                image = ((Bitmap)image).Resize(new Size((int)(image.Width * preset.Scaling), (int)(image.Height * preset.Scaling)), ScalingMode.Lanczos2, true);

            if (!preset.HighQuality)
            {
                image = image.ConvertPixelFormat(preset.pixFormat, preset.quantizer,preset.ditherer);
            }  
            return image;
        }
        public static Image ApplyEffects32bpp(Image image, Giffit.GiffitPreset preset)
        {
            Bitmap mod = new Bitmap((int)(image.Width * preset.Scaling), (int)(image.Height * preset.Scaling));
            if (!preset.HighQuality)
            {
                image.DrawInto(mod, new Rectangle(0, 0, (int)(image.Width * preset.Scaling), (int)(image.Height * preset.Scaling)), preset.quantizer, preset.ditherer, ScalingMode.Auto);
                return mod;
            }

            if (preset.Scaling < 1)
                mod = (Bitmap)ScaleImage(image, (int)(image.Width * preset.Scaling), (int)(image.Height * preset.Scaling));
            else
                mod = (Bitmap)image;

            return mod;
        }

        public void PreviewEffects(bool active)
        {
            UpdateTitleBar("");
            Stop();
            DisableActions();

            Image currenticon = null;
            Color border = Color.Red;

            if (preview)
            {
                currenticon = btnSettings.Image;
                border = btnSettings.FlatAppearance.BorderColor;
                btnSettings.Image = Giffit.Properties.Resources.spinner_20;
                btnSettings.FlatAppearance.BorderColor = Color.OrangeRed;
                UpdateInfo($"Applying '{Giffit.GiffitPreset.StyleNames[settings.StyleIndex]}' ... ");
            }
            

            var previewThread = new Thread(() => GeneratePreview(active));
            previewThread.Start();
            while (previewThread.IsAlive)
            {
                Application.DoEvents();
                Thread.Sleep(0);
            }

            EnableActions();
            if (preview)
            {
                btnSettings.FlatAppearance.BorderColor = border;
                btnSettings.Image = currenticon;
            }

            SetFrame();
        }
        private void EnableActions()
        {
            btnSave.Enabled = true;
            btnPlay.Enabled = true;
            btnLoop.Enabled = true;
            cmsActions.Enabled = true;
            btnPreview.Enabled = true;
            btnStop.Enabled = true;
            btnSave.Enabled = true;
            tbFrames.Enabled = true;
            processing = false;
        }
        private void DisableActions()
        {
            btnPlay.Enabled = false;
            btnLoop.Enabled = false;
            btnPreview.Enabled = false;
            btnStop.Enabled = false;
            cmsActions.Enabled = false;
            btnSave.Enabled = false;
            tbFrames.Enabled = false;
            processing = true;
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
            {
                lblResult.Text = text;
            }
        }
        public void UpdateInfo()
        {
            string fullInfo = $"from {tbFrames.Maximum} frames = {tbFrames.Maximum / GetFPS():F}s";

            if (lblResult.Width < System.Windows.Forms.TextRenderer.MeasureText(fullInfo,
                 new Font(lblResult.Font.FontFamily, lblResult.Font.Size, lblResult.Font.Style)).Width)
            {
                lblResult.Text = $"x{tbFrames.Maximum}={tbFrames.Maximum / GetFPS():F1}s";
            }
            else
            {
                lblResult.Text = fullInfo;
            }
            tAnimation.Interval = (int)Math.Round(1000 / GetFPS(), MidpointRounding.AwayFromZero);
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
            if (pbImage.Image == null)
                return;

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
        private void MoveRight()
        {
            if (previewImages.Count > 0)
            {
                if (tbFrames.Value + 1 <= tbFrames.Maximum)
                    tbFrames.Value += 1;
            }
        }
        private void MoveLeft()
        {
            if (previewImages.Count > 0)
                if (tbFrames.Value - 1 >= tbFrames.Minimum)
                    tbFrames.Value -= 1;
        }
        private double GetFPS()
        {
            return double.Parse(cbFPS.Text);
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
                case Keys.Control | Keys.N:
                    OpenWithDialog();
                    return true;
                case Keys.Control | Keys.I:
                    AddWithDialog();
                    return true;
                case Keys.Control | Keys.C:
                    if (previewImages.Count > 0)
                        Clipboard.SetDataObject(pbImage.Image);
                    return true;
                case Keys.Z:
                    if (previewImages.Count > 0)
                        btnPreview.PerformClick();
                    return true;
                case Keys.S:
                    if (previewImages.Count > 0)
                        btnSettings.PerformClick();
                    return true;
                case Keys.Left:
                    MoveLeft();
                    return true;
                case Keys.Right:
                    MoveRight();
                    return true;
            }

            // run base implementation
            return base.ProcessCmdKey(ref message, keys);
        }

        private void tbFrames_ValueChanged(object sender, EventArgs e)
        {
            SetFrame();
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            Stop();
            saveGIF.FileName = Path.GetFileName(animationFolder);
            saveGIF.InitialDirectory = folderDrop? Directory.GetParent(animationFolder).FullName : animationFolder;

            if (saveGIF.ShowDialog() == DialogResult.OK)
            {
                if (preview)
                    UpdateInfo($"Writing file ...");
                else
                    UpdateInfo($"Applying '{Giffit.GiffitPreset.StyleNames[settings.StyleIndex]}' ...");

                DisableActions();
                if (saveGIF.FilterIndex == 1)
                {
                    Thread saveGifThread = null;
                    var fps = GetFPS();
                    saveGifThread = new Thread(() => SaveGifAnimation(1000 / fps));

                    saveGifThread.Start();
                    while (saveGifThread.IsAlive)
                    {
                        Application.DoEvents();
                        Thread.Sleep(0);
                    }
                }

                else if (saveGIF.FilterIndex == 2)
                    SaveGifFrame();
                else if (saveGIF.FilterIndex == 3)
                    SaveJpgFrame();
                else if (saveGIF.FilterIndex == 4)
                    SavePNGFrame();
                else
                    SaveTIFfFrame();

                animationFolder = saveGIF.FileName;
                UpdateInfo();
                UpdateTitleBar($"- Saved to {Path.GetFileName(saveGIF.FileName)}");
                EnableActions();
            }
        }
        private void BasicGiffer_DragEnter(object sender, DragEventArgs e)
        {
            validData = GetFilename(out filenames, e);
            if (validData)
            {
                e.Effect = DragDropEffects.Copy;
                UpdateInfo($"{filenames.Count().ToString()} files can be loaded");
            }
            else
            {
                UpdateInfo($"none of the files dragged is supported");
                e.Effect = DragDropEffects.None;
            }
        }
        private void BasicGiffer_DragDrop(object sender, DragEventArgs e)
        {
            if (validData && !processing)
            {
                OpenFiles();
                this.Activate();
            }
            else
                MessageBox.Show("The files are not supported or there is currently processing action in background that cannot be interrupted.");
        }
        private void Gifit_DragLeave(object sender, EventArgs e)
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
        private void nudRepeat_KeyUp(object sender, KeyEventArgs e)
        {
            nudRepeat.Focus();
        }
        private void Gifit_Load(object sender, EventArgs e)
        {
            nudRepeat.Value = (int)Giffit.Properties.Settings.Default["Repeats"];
            cbFPS.SelectedIndex = (int)Giffit.Properties.Settings.Default["FPS"];
            recentFolders = (System.Collections.Specialized.StringCollection)Giffit.Properties.Settings.Default["Recents"];
            settings.Scaling = (decimal)Giffit.Properties.Settings.Default["Scaling"];
            settings.Background = (Color)Giffit.Properties.Settings.Default["BackgroundC"];
            settings.StyleIndex = (int)Giffit.Properties.Settings.Default["StyleIndex"];
            preserveStyle = (bool)Giffit.Properties.Settings.Default["KeepStyle"];
            preview = (bool)Giffit.Properties.Settings.Default["DontPreview"];


            if (!preserveStyle)
            {
                settings.StyleIndex = settings.DefaultStyle;
                settings.Scaling = 1.0m;
                settings.Background = Color.White;
            }

            if (recentFolders.Count > 0)
                clearrecentToolStripMenuItem.Enabled = true;


            // adjust button size for better fit
            if (this.DeviceDpi <= 100)
            {
                tableLayoutPanel1.RowStyles[2].Height = 44;
                tableLayoutPanel1.RowStyles[3].Height = 37;
                tableLayoutPanel3.ColumnStyles[2].Width = 37;
                tableLayoutPanel3.ColumnStyles[3].Width = 37;
                tableLayoutPanel3.ColumnStyles[4].Width = 37;
                tableLayoutPanel3.ColumnStyles[5].Width = 37;
                tableLayoutPanel3.ColumnStyles[6].Width = 37;


            }
            else if (this.DeviceDpi > 100 && this.DeviceDpi < 190)
            {
                tableLayoutPanel1.RowStyles[2].Height = 52;
                tableLayoutPanel1.RowStyles[3].Height = 48;
                tableLayoutPanel3.ColumnStyles[2].Width = 42;
                tableLayoutPanel3.ColumnStyles[3].Width = 42;
                tableLayoutPanel3.ColumnStyles[4].Width = 42;
                tableLayoutPanel3.ColumnStyles[5].Width = 42;
                tableLayoutPanel3.ColumnStyles[6].Width = 42;
            }
            else
            {
                tableLayoutPanel1.RowStyles[2].Height = 72;
                tableLayoutPanel1.RowStyles[3].Height = 68;
                tableLayoutPanel3.ColumnStyles[2].Width = 64;
                tableLayoutPanel3.ColumnStyles[3].Width = 64;
                tableLayoutPanel3.ColumnStyles[4].Width = 64;
                tableLayoutPanel3.ColumnStyles[5].Width = 64;
                tableLayoutPanel3.ColumnStyles[6].Width = 64;
            }

            //cbFPS.Text = "1";
            lblResult.Text = "";
        }
        private void Gifit_FormClosing(object sender, FormClosingEventArgs e)
        {
            Giffit.Properties.Settings.Default["Repeats"] = (int) nudRepeat.Value;
            Giffit.Properties.Settings.Default["FPS"] = (int) cbFPS.SelectedIndex;
            Giffit.Properties.Settings.Default["Recents"] = recentFolders;
            Giffit.Properties.Settings.Default["Scaling"] = settings.Scaling;
            Giffit.Properties.Settings.Default["StyleIndex"] = settings.StyleIndex;
            Giffit.Properties.Settings.Default["BackgroundC"] = settings.Background;
            Giffit.Properties.Settings.Default["KeepStyle"] = preserveStyle;
            Giffit.Properties.Settings.Default["DontPreview"] = preview;
            Giffit.Properties.Settings.Default.Save(); // Saves settings in application configuration file
        }
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var info = "";
            if (previewImages.Count > 0)
                info += $"Frame {tbFrames.Value} of {tbFrames.Maximum} is sized {previewImages[0].Size.Width}x{previewImages[0].Size.Height}px\n" +
                    $"Encoding: {pbImage.Image.GetBitsPerPixel()}bpp ({((Bitmap)pbImage.Image).GetColorCount()} colours)\n\n";
            else
                info += $"Image info will be availabe after you load frames.\n\n";

                info += $"{Application.ProductName} version: {Application.ProductVersion}\n" +
                $"C: { System.Reflection.Assembly.GetAssembly(typeof(KGySoft.CoreLibraries.ArrayExtensions)).GetName().Version.ToString()} " +
                $"D: { System.Reflection.Assembly.GetAssembly(typeof(KGySoft.Drawing.Imaging.AnimatedGifConfiguration)).GetName().Version.ToString()} (KGySoft)\n" +
                $"License: Advanced";

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
            sets.Background = settings.Background;
            sets.cbUseDefault.Checked = !preserveStyle;
            sets.h = originalImages[0].Size.Height;
            sets.w = originalImages[0].Size.Width;
            sets.defI = settings.DefaultStyle;
            sets.cbDontPreview.Checked = preview;

            if (sets.ShowDialog() == DialogResult.OK)
            {
                if (settings.StyleIndex != sets.cbStyle.SelectedIndex || 
                    settings.Scaling != (decimal)sets.tbSize.Value / 100 || 
                    sets.cbUseDefault.Checked == preserveStyle ||
                    settings.Background != sets.Background ||
                    sets.cbDontPreview.Checked != preview)
                {
                    settings.Scaling = (decimal)sets.tbSize.Value / 100;
                    settings.Background = sets.Background; // set background before index otherwise we cannot use it in this run
                    settings.StyleIndex = sets.cbStyle.SelectedIndex;
                    preserveStyle = !sets.cbUseDefault.Checked;
                    preview = sets.cbDontPreview.Checked;

                    PreviewEffects(preview);
                    UpdateInfo();
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
                if (Path.GetExtension(folder) == ".gif" && File.Exists(folder))
                {
                    ToolStripMenuItem rf = new ToolStripMenuItem(Path.GetFileName(folder.TrimEnd(Path.DirectorySeparatorChar)));
                    rf.ToolTipText = folder;
                    rf.Click += Rf_Click;
                    cmsRecents.Items.Add(rf);
                }
                else if (Directory.Exists(folder) && Directory.GetFiles(folder).Length > 0)
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
                MessageBox.Show("No supported files were found in this recent directory");
            }
        }
        private void Gifit_SizeChanged(object sender, EventArgs e)
        {
            if (oneToOne)
            {
                oneToOne = false;
                btnPreview.PerformClick();
            }
            if (previewImages.Count > 0)
                UpdateInfo();
        }
        private void copyStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject(pbImage.Image);
        }
        private void cbFPS_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateInfo();
        }

        //static Color GetPixel(Point position)
        //{
        //    using (var bitmap = new Bitmap(1, 1))
        //    {
        //        using (var graphics = Graphics.FromImage(bitmap))
        //        {
        //            graphics.CopyFromScreen(position, new Point(0, 0), new Size(1, 1));
        //        }
        //        return bitmap.GetPixel(0, 0);
        //    }
        //}
        private void pbImage_MouseLeave(object sender, EventArgs e)
        {
            //lblColourInfo.Text = "";
        }
        private void pbImage_MouseUp(object sender, MouseEventArgs e)
        {
            //if (!processing && pbImage.Image != null)
            //{
            //    //Color c = GetPixel(new Point( Location.X + e.Location.X, Location.Y + e.Location.Y));
            //    ////if (c.A < 255)
            //    ////    lblColourInfo.Text = "Transparent pixel";
            //    ////else
            //    /////  lblColourInfo.Text = $"({c.R},{c.G},{c.B})";
            //    /////  
            //    //lblColourInfo.BackColor = c;
            //}
        }
    }
}