
namespace BasicGiffer
{
    partial class Gifit
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Gifit));
            this.pbImage = new System.Windows.Forms.PictureBox();
            this.cmsActions = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveGIFToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.recentfoldersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsRecents = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.clearrecentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.btnSave = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.nudRepeat = new System.Windows.Forms.NumericUpDown();
            this.lblResult = new System.Windows.Forms.Label();
            this.cbFPS = new System.Windows.Forms.ComboBox();
            this.btnSettings = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.btnPlay = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.lblColourInfo = new System.Windows.Forms.Label();
            this.lblCurFrame = new System.Windows.Forms.Label();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.btnLoop = new System.Windows.Forms.Button();
            this.btnPreview = new System.Windows.Forms.Button();
            this.tbFrames = new System.Windows.Forms.TrackBar();
            this.saveGIF = new System.Windows.Forms.SaveFileDialog();
            this.tAnimation = new System.Windows.Forms.Timer(this.components);
            this.ofd = new System.Windows.Forms.OpenFileDialog();
            this.ttip = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pbImage)).BeginInit();
            this.cmsActions.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudRepeat)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbFrames)).BeginInit();
            this.SuspendLayout();
            // 
            // pbImage
            // 
            this.pbImage.BackColor = System.Drawing.Color.White;
            this.pbImage.BackgroundImage = global::Giffit.Properties.Resources.drop;
            this.pbImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pbImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbImage.ContextMenuStrip = this.cmsActions;
            this.pbImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbImage.Location = new System.Drawing.Point(3, 3);
            this.pbImage.Name = "pbImage";
            this.pbImage.Size = new System.Drawing.Size(1062, 661);
            this.pbImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbImage.TabIndex = 4;
            this.pbImage.TabStop = false;
            this.pbImage.Tag = "dropper";
            this.pbImage.DoubleClick += new System.EventHandler(this.pbImage_DoubleClick);
            this.pbImage.MouseLeave += new System.EventHandler(this.pbImage_MouseLeave);
            this.pbImage.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pbImage_MouseUp);
            // 
            // cmsActions
            // 
            this.cmsActions.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.cmsActions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.addToolStripMenuItem,
            this.saveGIFToolStripMenuItem,
            this.copyStripMenuItem,
            this.toolStripSeparator1,
            this.recentfoldersToolStripMenuItem,
            this.clearrecentToolStripMenuItem});
            this.cmsActions.Name = "cmsActions";
            this.cmsActions.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.cmsActions.ShowImageMargin = false;
            this.cmsActions.ShowItemToolTips = false;
            this.cmsActions.Size = new System.Drawing.Size(276, 202);
            this.cmsActions.Opening += new System.ComponentModel.CancelEventHandler(this.cmsActions_Opening_1);
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 8.883117F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+N";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(275, 32);
            this.newToolStripMenuItem.Text = "&New Animation ...";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // addToolStripMenuItem
            // 
            this.addToolStripMenuItem.Enabled = false;
            this.addToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 8.883117F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.addToolStripMenuItem.Name = "addToolStripMenuItem";
            this.addToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+I";
            this.addToolStripMenuItem.Size = new System.Drawing.Size(275, 32);
            this.addToolStripMenuItem.Text = "&Add Images ...";
            this.addToolStripMenuItem.ToolTipText = "Ctrl+I";
            this.addToolStripMenuItem.Click += new System.EventHandler(this.addToolStripMenuItem_Click);
            // 
            // saveGIFToolStripMenuItem
            // 
            this.saveGIFToolStripMenuItem.Enabled = false;
            this.saveGIFToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 8.883117F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.saveGIFToolStripMenuItem.Name = "saveGIFToolStripMenuItem";
            this.saveGIFToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+S";
            this.saveGIFToolStripMenuItem.Size = new System.Drawing.Size(275, 32);
            this.saveGIFToolStripMenuItem.Text = "&Export ...";
            this.saveGIFToolStripMenuItem.Click += new System.EventHandler(this.saveGIFToolStripMenuItem_Click);
            // 
            // copyStripMenuItem
            // 
            this.copyStripMenuItem.Enabled = false;
            this.copyStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 8.883117F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.copyStripMenuItem.Name = "copyStripMenuItem";
            this.copyStripMenuItem.ShortcutKeyDisplayString = "Ctrl+C";
            this.copyStripMenuItem.Size = new System.Drawing.Size(275, 32);
            this.copyStripMenuItem.Text = "&Copy Frame";
            this.copyStripMenuItem.ToolTipText = "Ctrl+C";
            this.copyStripMenuItem.Click += new System.EventHandler(this.copyStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(272, 6);
            // 
            // recentfoldersToolStripMenuItem
            // 
            this.recentfoldersToolStripMenuItem.DropDown = this.cmsRecents;
            this.recentfoldersToolStripMenuItem.Enabled = false;
            this.recentfoldersToolStripMenuItem.Name = "recentfoldersToolStripMenuItem";
            this.recentfoldersToolStripMenuItem.Size = new System.Drawing.Size(275, 32);
            this.recentfoldersToolStripMenuItem.Text = "Recents";
            // 
            // cmsRecents
            // 
            this.cmsRecents.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.cmsRecents.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.cmsRecents.Name = "cmsRecents";
            this.cmsRecents.OwnerItem = this.recentfoldersToolStripMenuItem;
            this.cmsRecents.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.cmsRecents.ShowImageMargin = false;
            this.cmsRecents.Size = new System.Drawing.Size(36, 4);
            // 
            // clearrecentToolStripMenuItem
            // 
            this.clearrecentToolStripMenuItem.Name = "clearrecentToolStripMenuItem";
            this.clearrecentToolStripMenuItem.Size = new System.Drawing.Size(275, 32);
            this.clearrecentToolStripMenuItem.Text = "Clear Recents";
            this.clearrecentToolStripMenuItem.Click += new System.EventHandler(this.clearrecentToolStripMenuItem_Click);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 6;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 95F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 85F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 89F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 172F));
            this.tableLayoutPanel2.Controls.Add(this.btnSave, 5, 0);
            this.tableLayoutPanel2.Controls.Add(this.label1, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.nudRepeat, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.lblResult, 4, 0);
            this.tableLayoutPanel2.Controls.Add(this.cbFPS, 3, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 785);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1062, 64);
            this.tableLayoutPanel2.TabIndex = 2;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.BackColor = System.Drawing.SystemColors.Control;
            this.btnSave.Enabled = false;
            this.btnSave.Location = new System.Drawing.Point(893, 12);
            this.btnSave.Margin = new System.Windows.Forms.Padding(3, 12, 12, 12);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(157, 40);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "&Export";
            this.ttip.SetToolTip(this.btnSave, "Save to file (Ctrl+S)");
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(189, 19);
            this.label1.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 25);
            this.label1.TabIndex = 2;
            this.label1.Text = "FPS:";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 19);
            this.label2.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(81, 25);
            this.label2.TabIndex = 4;
            this.label2.Text = "Repeats:";
            // 
            // nudRepeat
            // 
            this.nudRepeat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.nudRepeat.Location = new System.Drawing.Point(98, 15);
            this.nudRepeat.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.nudRepeat.Name = "nudRepeat";
            this.nudRepeat.Size = new System.Drawing.Size(79, 33);
            this.nudRepeat.TabIndex = 5;
            this.ttip.SetToolTip(this.nudRepeat, "How many times to play the animation. Zero is for endless loops.");
            this.nudRepeat.ValueChanged += new System.EventHandler(this.nudRepeat_ValueChanged);
            this.nudRepeat.KeyUp += new System.Windows.Forms.KeyEventHandler(this.nudRepeat_KeyUp);
            // 
            // lblResult
            // 
            this.lblResult.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblResult.Location = new System.Drawing.Point(327, 9);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new System.Drawing.Size(560, 45);
            this.lblResult.TabIndex = 3;
            this.lblResult.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbFPS
            // 
            this.cbFPS.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cbFPS.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFPS.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbFPS.FormattingEnabled = true;
            this.cbFPS.Items.AddRange(new object[] {
            "100",
            "50",
            "33.33",
            "25",
            "20",
            "16.67",
            "14.29",
            "12.5",
            "10",
            "5",
            "2.5",
            "1",
            "0.5",
            "0.2"});
            this.cbFPS.Location = new System.Drawing.Point(238, 15);
            this.cbFPS.Name = "cbFPS";
            this.cbFPS.Size = new System.Drawing.Size(83, 33);
            this.cbFPS.TabIndex = 6;
            this.ttip.SetToolTip(this.cbFPS, "Frames per second to use based on 1/100 second");
            this.cbFPS.SelectedIndexChanged += new System.EventHandler(this.cbFPS_SelectedIndexChanged);
            // 
            // btnSettings
            // 
            this.btnSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSettings.Enabled = false;
            this.btnSettings.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.btnSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSettings.Image = global::Giffit.Properties.Resources.magic_wand;
            this.btnSettings.Location = new System.Drawing.Point(615, 2);
            this.btnSettings.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(55, 45);
            this.btnSettings.TabIndex = 6;
            this.ttip.SetToolTip(this.btnSettings, "Image settings (S)");
            this.btnSettings.UseVisualStyleBackColor = true;
            this.btnSettings.Click += new System.EventHandler(this.btnSettings_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.pbImage, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.tbFrames, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 0F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1068, 852);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 9;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 57F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 57F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 57F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 57F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 57F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.tableLayoutPanel3.Controls.Add(this.btnPlay, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnStop, 3, 0);
            this.tableLayoutPanel3.Controls.Add(this.lblColourInfo, 7, 0);
            this.tableLayoutPanel3.Controls.Add(this.lblCurFrame, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.linkLabel1, 8, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnLoop, 4, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnSettings, 6, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnPreview, 5, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 730);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(1062, 49);
            this.tableLayoutPanel3.TabIndex = 5;
            // 
            // btnPlay
            // 
            this.btnPlay.BackColor = System.Drawing.SystemColors.Control;
            this.btnPlay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnPlay.Enabled = false;
            this.btnPlay.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.btnPlay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPlay.Image = global::Giffit.Properties.Resources.play;
            this.btnPlay.Location = new System.Drawing.Point(387, 2);
            this.btnPlay.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(55, 45);
            this.btnPlay.TabIndex = 0;
            this.ttip.SetToolTip(this.btnPlay, "Play (Space)");
            this.btnPlay.UseVisualStyleBackColor = false;
            this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
            // 
            // btnStop
            // 
            this.btnStop.AutoSize = true;
            this.btnStop.BackColor = System.Drawing.SystemColors.ControlDark;
            this.btnStop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnStop.Enabled = false;
            this.btnStop.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.btnStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStop.Image = global::Giffit.Properties.Resources.stop;
            this.btnStop.Location = new System.Drawing.Point(444, 2);
            this.btnStop.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(55, 45);
            this.btnStop.TabIndex = 1;
            this.ttip.SetToolTip(this.btnStop, "Stop (Space)");
            this.btnStop.UseVisualStyleBackColor = false;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // lblColourInfo
            // 
            this.lblColourInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblColourInfo.Font = new System.Drawing.Font("Segoe UI", 7.948052F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblColourInfo.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblColourInfo.Location = new System.Drawing.Point(671, 8);
            this.lblColourInfo.Margin = new System.Windows.Forms.Padding(0);
            this.lblColourInfo.Name = "lblColourInfo";
            this.lblColourInfo.Size = new System.Drawing.Size(316, 33);
            this.lblColourInfo.TabIndex = 2;
            this.lblColourInfo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblCurFrame
            // 
            this.lblCurFrame.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCurFrame.AutoSize = true;
            this.lblCurFrame.Font = new System.Drawing.Font("Segoe UI Semibold", 8.883117F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblCurFrame.Location = new System.Drawing.Point(3, 0);
            this.lblCurFrame.Name = "lblCurFrame";
            this.lblCurFrame.Size = new System.Drawing.Size(64, 49);
            this.lblCurFrame.TabIndex = 3;
            this.lblCurFrame.Text = "1";
            this.lblCurFrame.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // linkLabel1
            // 
            this.linkLabel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Font = new System.Drawing.Font("Segoe UI", 8.883117F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.linkLabel1.Location = new System.Drawing.Point(990, 0);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(69, 49);
            this.linkLabel1.TabIndex = 4;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "info";
            this.linkLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // btnLoop
            // 
            this.btnLoop.BackColor = System.Drawing.SystemColors.Control;
            this.btnLoop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnLoop.Enabled = false;
            this.btnLoop.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.btnLoop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLoop.Image = global::Giffit.Properties.Resources.loopbv;
            this.btnLoop.Location = new System.Drawing.Point(501, 2);
            this.btnLoop.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.btnLoop.Name = "btnLoop";
            this.btnLoop.Size = new System.Drawing.Size(55, 45);
            this.btnLoop.TabIndex = 2;
            this.ttip.SetToolTip(this.btnLoop, "Loopback: add duplicate all frames in reverse order");
            this.btnLoop.UseVisualStyleBackColor = false;
            this.btnLoop.Click += new System.EventHandler(this.btnLoop_Click);
            // 
            // btnPreview
            // 
            this.btnPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnPreview.Enabled = false;
            this.btnPreview.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.btnPreview.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPreview.Image = global::Giffit.Properties.Resources.preview;
            this.btnPreview.Location = new System.Drawing.Point(558, 2);
            this.btnPreview.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.btnPreview.Name = "btnPreview";
            this.btnPreview.Size = new System.Drawing.Size(55, 45);
            this.btnPreview.TabIndex = 7;
            this.ttip.SetToolTip(this.btnPreview, "Zoom to 100% (Z)");
            this.btnPreview.UseVisualStyleBackColor = true;
            this.btnPreview.Click += new System.EventHandler(this.btnPreview_Click);
            // 
            // tbFrames
            // 
            this.tbFrames.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbFrames.Enabled = false;
            this.tbFrames.Location = new System.Drawing.Point(3, 670);
            this.tbFrames.Maximum = 1;
            this.tbFrames.Minimum = 1;
            this.tbFrames.Name = "tbFrames";
            this.tbFrames.Size = new System.Drawing.Size(1062, 54);
            this.tbFrames.TabIndex = 3;
            this.tbFrames.Value = 1;
            this.tbFrames.ValueChanged += new System.EventHandler(this.tbFrames_ValueChanged);
            // 
            // saveGIF
            // 
            this.saveGIF.DefaultExt = "gif";
            this.saveGIF.FileName = "animation";
            this.saveGIF.Filter = "GIF Animation |*.gif|Current frame as GIF|*.gif|Current frame as JPG|*.jpg|Curren" +
    "t frame as PNG|*.png|Current frame as TIFF|*.tiff";
            this.saveGIF.SupportMultiDottedExtensions = true;
            this.saveGIF.Title = "Export GIF";
            // 
            // tAnimation
            // 
            this.tAnimation.Tick += new System.EventHandler(this.tAnimation_Tick);
            // 
            // ofd
            // 
            this.ofd.Filter = " GIF |*.gif| JPG |*.jpg| PNG |*.png| BMP |*.bmp| TIFF |*.tiff| All supported file" +
    "s |*.gif;*.jpg;*.png;*.bmp;*.tiff";
            this.ofd.FilterIndex = 6;
            this.ofd.Multiselect = true;
            this.ofd.RestoreDirectory = true;
            this.ofd.Title = "Import Images";
            // 
            // ttip
            // 
            this.ttip.AutomaticDelay = 900;
            this.ttip.BackColor = System.Drawing.SystemColors.HighlightText;
            // 
            // Gifit
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1068, 852);
            this.Controls.Add(this.tableLayoutPanel1);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(2333, 1478);
            this.MinimumSize = new System.Drawing.Size(639, 524);
            this.Name = "Gifit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Giffit";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Gifit_FormClosing);
            this.Load += new System.EventHandler(this.Gifit_Load);
            this.SizeChanged += new System.EventHandler(this.Gifit_SizeChanged);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.BasicGiffer_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.BasicGiffer_DragEnter);
            this.DragLeave += new System.EventHandler(this.Gifit_DragLeave);
            ((System.ComponentModel.ISupportInitialize)(this.pbImage)).EndInit();
            this.cmsActions.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudRepeat)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbFrames)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pbImage;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TrackBar tbFrames;
        private System.Windows.Forms.Label lblResult;
        private System.Windows.Forms.SaveFileDialog saveGIF;
        private System.Windows.Forms.Timer tAnimation;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Button btnPlay;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnLoop;
        private System.Windows.Forms.Label lblCurFrame;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown nudRepeat;
        private System.Windows.Forms.OpenFileDialog ofd;
        private System.Windows.Forms.ToolTip ttip;
        private System.Windows.Forms.ContextMenuStrip cmsActions;
        private System.Windows.Forms.Button btnSettings;
        private System.Windows.Forms.Button btnPreview;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveGIFToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem recentfoldersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearrecentToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip cmsRecents;
        private System.Windows.Forms.ToolStripMenuItem copyStripMenuItem;
        private System.Windows.Forms.ComboBox cbFPS;
        private System.Windows.Forms.Label lblColourInfo;
    }
}

