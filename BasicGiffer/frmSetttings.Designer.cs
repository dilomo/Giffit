﻿
namespace Giffit
{
    partial class frmSettings
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnStore = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblsize = new System.Windows.Forms.Label();
            this.tbSize = new System.Windows.Forms.TrackBar();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cbStyle = new System.Windows.Forms.ComboBox();
            this.cbPersistent = new System.Windows.Forms.CheckBox();
            this.cd = new System.Windows.Forms.ColorDialog();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbSize)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnStore
            // 
            this.btnStore.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStore.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnStore.Location = new System.Drawing.Point(146, 328);
            this.btnStore.Name = "btnStore";
            this.btnStore.Size = new System.Drawing.Size(194, 36);
            this.btnStore.TabIndex = 0;
            this.btnStore.Text = "&Apply";
            this.btnStore.UseVisualStyleBackColor = true;
            this.btnStore.Click += new System.EventHandler(this.btnStore_Click);
            // 
            // btnReset
            // 
            this.btnReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnReset.Location = new System.Drawing.Point(13, 328);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(95, 36);
            this.btnReset.TabIndex = 1;
            this.btnReset.Text = "&Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.lblsize);
            this.groupBox1.Controls.Add(this.tbSize);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(327, 109);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Size";
            // 
            // lblsize
            // 
            this.lblsize.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblsize.Location = new System.Drawing.Point(3, 76);
            this.lblsize.Name = "lblsize";
            this.lblsize.Size = new System.Drawing.Size(321, 30);
            this.lblsize.TabIndex = 5;
            this.lblsize.Text = "2000x1600 px (100%)";
            this.lblsize.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tbSize
            // 
            this.tbSize.Dock = System.Windows.Forms.DockStyle.Top;
            this.tbSize.Location = new System.Drawing.Point(3, 29);
            this.tbSize.Maximum = 100;
            this.tbSize.Minimum = 1;
            this.tbSize.Name = "tbSize";
            this.tbSize.Size = new System.Drawing.Size(321, 72);
            this.tbSize.TabIndex = 6;
            this.tbSize.TickFrequency = 5;
            this.tbSize.Value = 50;
            this.tbSize.Scroll += new System.EventHandler(this.tbSize_Scroll);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.cbStyle);
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBox2.Location = new System.Drawing.Point(9, 127);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(327, 133);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Style";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(84, 84);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(230, 33);
            this.label2.TabIndex = 2;
            this.label2.Text = "Background colour";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.White;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.label1.Location = new System.Drawing.Point(12, 84);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 33);
            this.label1.TabIndex = 1;
            this.label1.Click += new System.EventHandler(this.label1_Click);
            this.label1.MouseEnter += new System.EventHandler(this.label1_MouseEnter);
            this.label1.MouseLeave += new System.EventHandler(this.label1_MouseLeave);
            // 
            // cbStyle
            // 
            this.cbStyle.DisplayMember = "1";
            this.cbStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbStyle.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbStyle.FormattingEnabled = true;
            this.cbStyle.Location = new System.Drawing.Point(12, 36);
            this.cbStyle.Margin = new System.Windows.Forms.Padding(9, 3, 9, 3);
            this.cbStyle.MaxDropDownItems = 15;
            this.cbStyle.Name = "cbStyle";
            this.cbStyle.Size = new System.Drawing.Size(302, 33);
            this.cbStyle.TabIndex = 0;
            this.cbStyle.ValueMember = "0";
            // 
            // cbPersistent
            // 
            this.cbPersistent.AutoSize = true;
            this.cbPersistent.Location = new System.Drawing.Point(21, 266);
            this.cbPersistent.Name = "cbPersistent";
            this.cbPersistent.Size = new System.Drawing.Size(236, 29);
            this.cbPersistent.TabIndex = 1;
            this.cbPersistent.Text = "Keep settings on restart";
            this.cbPersistent.UseVisualStyleBackColor = true;
            // 
            // cd
            // 
            this.cd.Color = System.Drawing.Color.White;
            // 
            // frmSettings
            // 
            this.AcceptButton = this.btnStore;
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(353, 374);
            this.Controls.Add(this.cbPersistent);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnStore);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Image Settings";
            this.Load += new System.EventHandler(this.frmSettings_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbSize)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStore;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblsize;
        private System.Windows.Forms.GroupBox groupBox2;
        public System.Windows.Forms.TrackBar tbSize;
        public System.Windows.Forms.ComboBox cbStyle;
        public System.Windows.Forms.CheckBox cbPersistent;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ColorDialog cd;
    }
}