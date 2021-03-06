﻿namespace MMA845x_DEMO
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class AboutPCSoftware : Form
    {
        private IContainer components = null;
        private Label label1;
        private Label label2;

        public AboutPCSoftware()
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
            ComponentResourceManager resources = new ComponentResourceManager(typeof(AboutPCSoftware));
            label2 = new Label();
            label1 = new Label();
            base.SuspendLayout();
            label2.AutoSize = true;
            label2.Font = new Font("Arial Rounded MT Bold", 12f, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(9, 0x42);
            label2.Name = "label2";
            label2.Size = new Size(0xe5, 0x12);
            label2.TabIndex = 0x31;
            label2.Text = "Release Date: July 30, 2010";
            label1.AutoSize = true;
            label1.Font = new Font("Arial Rounded MT Bold", 12f, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(0x4e, 0x17);
            label1.Name = "label1";
            label1.Size = new Size(0x7d, 0x12);
            label1.TabIndex = 0x30;
            label1.Text = "PC Version 0.1";
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = Color.LightSlateGray;
            base.ClientSize = new Size(0x124, 0xa1);
            base.Controls.Add(label2);
            base.Controls.Add(label1);
            base.Icon = (Icon) resources.GetObject("$Icon");
            base.MaximizeBox = false;
            base.Name = "AboutPCSoftware";
            base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            Text = "MMA8450Q Sensor Toolbox";
            base.ResumeLayout(false);
            base.PerformLayout();
        }
    }
}

