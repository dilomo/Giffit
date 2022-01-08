
namespace BasicGiffer
{
    partial class InfoForm
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
            this.btnClose = new System.Windows.Forms.Button();
            this.lblInfo = new System.Windows.Forms.Label();
            this.linkSupport = new System.Windows.Forms.LinkLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(327, 336);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(119, 38);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "&Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.button1_Click);
            // 
            // lblInfo
            // 
            this.lblInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblInfo.BackColor = System.Drawing.SystemColors.Window;
            this.lblInfo.Location = new System.Drawing.Point(0, 0);
            this.lblInfo.Margin = new System.Windows.Forms.Padding(30);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Padding = new System.Windows.Forms.Padding(30);
            this.lblInfo.Size = new System.Drawing.Size(459, 252);
            this.lblInfo.TabIndex = 2;
            this.lblInfo.Text = "label1   ";
            // 
            // linkSupport
            // 
            this.linkSupport.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.linkSupport.AutoSize = true;
            this.linkSupport.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.linkSupport.Location = new System.Drawing.Point(0, 244);
            this.linkSupport.Name = "linkSupport";
            this.linkSupport.Padding = new System.Windows.Forms.Padding(30, 0, 0, 0);
            this.linkSupport.Size = new System.Drawing.Size(287, 25);
            this.linkSupport.TabIndex = 3;
            this.linkSupport.TabStop = true;
            this.linkSupport.Text = "Support and feature requests";
            this.linkSupport.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkSupport_LinkClicked);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.BackColor = System.Drawing.SystemColors.Window;
            this.label1.Location = new System.Drawing.Point(0, 244);
            this.label1.Margin = new System.Windows.Forms.Padding(30, 15, 30, 15);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(30, 15, 30, 15);
            this.label1.Size = new System.Drawing.Size(459, 77);
            this.label1.TabIndex = 2;
            this.label1.Text = "©2022 Anton Kerezov, All Rights Reserved.";
            this.label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // InfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(459, 387);
            this.Controls.Add(this.linkSupport);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.btnClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InfoForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = " Info ";
            this.Load += new System.EventHandler(this.InfoForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.LinkLabel linkSupport;
        public System.Windows.Forms.Label lblInfo;
        public System.Windows.Forms.Label label1;
    }
}