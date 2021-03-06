﻿namespace MMA845x_DEMO
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class AboutFirmware : Form
    {
        private IContainer components = null;
        private Label label1;
        private Label label2;

        public AboutFirmware()
        {
            InitializeComponent();
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
            ComponentResourceManager resources = new ComponentResourceManager(typeof(AboutFirmware));
            label1 = new Label();
            label2 = new Label();
            base.SuspendLayout();
            label1.AutoSize = true;
            label1.Font = new Font("Arial Rounded MT Bold", 12f, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(0x3b, 0x15);
            label1.Name = "label1";
            label1.Size = new Size(0xb1, 0x12);
            label1.TabIndex = 0x2d;
            label1.Text = "Firmware Version 0.1";
            label2.AutoSize = true;
            label2.Font = new Font("Arial Rounded MT Bold", 12f, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(4, 0x3e);
            label2.Name = "label2";
            label2.Size = new Size(0xe9, 0x12);
            label2.TabIndex = 0x2e;
            label2.Text = "Release Date:  July 30, 2010";
            base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = Color.LightSlateGray;
            base.ClientSize = new Size(0x124, 0x9b);
            base.Controls.Add(label2);
            base.Controls.Add(label1);
            base.Icon = (Icon) resources.GetObject("$Icon");
            base.MaximizeBox = false;
            base.Name = "AboutFirmware";
            base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            Text = "About Firmware";
            base.ResumeLayout(false);
            base.PerformLayout();
        }
    }
}

