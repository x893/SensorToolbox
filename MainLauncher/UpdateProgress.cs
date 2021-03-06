﻿namespace MainLauncher
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class UpdateProgress : Form
    {
        private Button bCancelDownload;
        private IContainer components = null;
        private Initial initialInstance;
        private Label lblUpdateBytes;
        private Label lblUpdateProgress;
        private ProgressBar progressDownload;

        public UpdateProgress()
        {
            InitializeComponent();
        }

        private void CancelDownload(object sender, EventArgs e)
        {
            initialInstance.CancelDownload();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            progressDownload = new ProgressBar();
            bCancelDownload = new Button();
            lblUpdateProgress = new Label();
            lblUpdateBytes = new Label();
            base.SuspendLayout();
            progressDownload.Location = new Point(12, 0x1c);
            progressDownload.Name = "progressDownload";
            progressDownload.Size = new Size(340, 0x17);
            progressDownload.TabIndex = 0;
            bCancelDownload.Location = new Point(0x91, 0x43);
            bCancelDownload.Name = "bCancelDownload";
            bCancelDownload.Size = new Size(0x4b, 0x17);
            bCancelDownload.TabIndex = 1;
            bCancelDownload.Text = "Cancel";
            bCancelDownload.UseVisualStyleBackColor = true;
            bCancelDownload.Click += new EventHandler(CancelDownload);
            lblUpdateProgress.AutoSize = true;
            lblUpdateProgress.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblUpdateProgress.Location = new Point(12, 9);
            lblUpdateProgress.Name = "lblUpdateProgress";
            lblUpdateProgress.Size = new Size(0xd8, 0x10);
            lblUpdateProgress.TabIndex = 2;
            lblUpdateProgress.Text = "Downloading SensorToolbox: ";
            lblUpdateBytes.AutoSize = true;
            lblUpdateBytes.Location = new Point(12, 0x36);
            lblUpdateBytes.Name = "lblUpdateBytes";
            lblUpdateBytes.Size = new Size(0x22, 13);
            lblUpdateBytes.TabIndex = 3;
            lblUpdateBytes.Text = "0 of 0";
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x16c, 0x5f);
            base.ControlBox = false;
            base.Controls.Add(lblUpdateBytes);
            base.Controls.Add(lblUpdateProgress);
            base.Controls.Add(bCancelDownload);
            base.Controls.Add(progressDownload);
            base.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            base.Name = "UpdateProgress";
            base.StartPosition = FormStartPosition.CenterScreen;
            Text = "SensorToolbox Update...";
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        public void SetBytesLabel(string bytes)
        {
            lblUpdateBytes.Text = bytes;
        }

        public void SetInitialInstance(Initial i)
        {
            initialInstance = i;
        }

        public void SetProgress(int progress)
        {
            progressDownload.Value = progress;
        }

        public void SetVersionLabel(string version)
        {
            lblUpdateProgress.Text = lblUpdateProgress.Text + version;
        }
    }
}

