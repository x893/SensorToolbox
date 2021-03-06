﻿namespace GlobalSTB
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Reflection;
    using System.Windows.Forms;

    internal class AboutBox1 : Form
    {
        private IContainer components = null;
        private Label labelCopyright;
        private Label labelProductName;
        private Label labelVersion;
        private PictureBox logoPictureBox;
        private Button okButton;
        private TableLayoutPanel tableLayoutPanel;
        private RichTextBox textBoxDescription;

        public AboutBox1()
        {
            InitializeComponent();
            Text = string.Format("About {0}", AssemblyTitle);
            labelProductName.Text = AssemblyProduct;
            labelVersion.Text = string.Format("Version {0}", AssemblyProductVersion);
            labelCopyright.Text = AssemblyCopyright;
            textBoxDescription.Text = string.Format("{0}\r\n{1}", AssemblyDescription, AssemblyVersion);
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
            ComponentResourceManager manager = new ComponentResourceManager(typeof(GlobalSTB.AboutBox1));
            okButton = new Button();
            tableLayoutPanel = new TableLayoutPanel();
            logoPictureBox = new PictureBox();
            labelProductName = new Label();
            labelVersion = new Label();
            labelCopyright = new Label();
            textBoxDescription = new RichTextBox();
            tableLayoutPanel.SuspendLayout();
            ((ISupportInitialize) logoPictureBox).BeginInit();
            base.SuspendLayout();
            okButton.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            okButton.BackColor = SystemColors.Control;
            okButton.DialogResult = DialogResult.Cancel;
            okButton.Location = new Point(0x120, 0x124);
            okButton.Name = "okButton";
            okButton.Size = new Size(0x4b, 20);
            okButton.TabIndex = 0x18;
            okButton.Text = "&OK";
            okButton.UseVisualStyleBackColor = false;
            okButton.Click += new EventHandler(okButton_Click);
            tableLayoutPanel.BackColor = Color.FromArgb(0x5b, 0x6f, 0x7b);
            tableLayoutPanel.ColumnCount = 2;
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30f));
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70f));
            tableLayoutPanel.Controls.Add(okButton, 1, 5);
            tableLayoutPanel.Controls.Add(logoPictureBox, 0, 0);
            tableLayoutPanel.Controls.Add(labelProductName, 1, 1);
            tableLayoutPanel.Controls.Add(labelVersion, 1, 2);
            tableLayoutPanel.Controls.Add(labelCopyright, 1, 3);
            tableLayoutPanel.Controls.Add(textBoxDescription, 1, 4);
            tableLayoutPanel.Dock = DockStyle.Fill;
            tableLayoutPanel.Location = new Point(9, 9);
            tableLayoutPanel.Name = "tableLayoutPanel";
            tableLayoutPanel.RowCount = 6;
            tableLayoutPanel.RowStyles.Add(new RowStyle());
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 15.44118f));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 15.44118f));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 15.44118f));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 53.67647f));
            tableLayoutPanel.RowStyles.Add(new RowStyle());
            tableLayoutPanel.Size = new Size(0x16e, 0x13b);
            tableLayoutPanel.TabIndex = 0;
            tableLayoutPanel.SetColumnSpan(logoPictureBox, 2);
            logoPictureBox.Dock = DockStyle.Fill;
            logoPictureBox.Image = (Image) manager.GetObject("logoPictureBox.Image");
            logoPictureBox.Location = new Point(3, 3);
            logoPictureBox.Name = "logoPictureBox";
            logoPictureBox.Size = new Size(360, 120);
            logoPictureBox.SizeMode = PictureBoxSizeMode.AutoSize;
            logoPictureBox.TabIndex = 0x19;
            logoPictureBox.TabStop = false;
            labelProductName.BackColor = Color.FromArgb(0x5b, 0x6f, 0x7b);
            labelProductName.Dock = DockStyle.Fill;
            labelProductName.Font = new Font("Segoe UI", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            labelProductName.ForeColor = SystemColors.ActiveCaptionText;
            labelProductName.Location = new Point(0x73, 0x7e);
            labelProductName.Margin = new Padding(6, 0, 3, 0);
            labelProductName.MaximumSize = new Size(0, 0x11);
            labelProductName.Name = "labelProductName";
            labelProductName.Size = new Size(0xf8, 0x11);
            labelProductName.TabIndex = 0x13;
            labelProductName.Text = "Product Name";
            labelProductName.TextAlign = ContentAlignment.MiddleLeft;
            labelVersion.BackColor = Color.FromArgb(0x5b, 0x6f, 0x7b);
            labelVersion.Dock = DockStyle.Fill;
            labelVersion.Font = new Font("Segoe UI", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            labelVersion.ForeColor = SystemColors.ActiveCaptionText;
            labelVersion.Location = new Point(0x73, 0x97);
            labelVersion.Margin = new Padding(6, 0, 3, 0);
            labelVersion.MaximumSize = new Size(0, 0x11);
            labelVersion.Name = "labelVersion";
            labelVersion.Size = new Size(0xf8, 0x11);
            labelVersion.TabIndex = 0;
            labelVersion.Text = "Product Version";
            labelVersion.TextAlign = ContentAlignment.MiddleLeft;
            labelCopyright.BackColor = Color.FromArgb(0x5b, 0x6f, 0x7b);
            labelCopyright.Dock = DockStyle.Fill;
            labelCopyright.Font = new Font("Segoe UI", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            labelCopyright.ForeColor = SystemColors.ActiveCaptionText;
            labelCopyright.Location = new Point(0x73, 0xb0);
            labelCopyright.Margin = new Padding(6, 0, 3, 0);
            labelCopyright.MaximumSize = new Size(0, 0x11);
            labelCopyright.Name = "labelCopyright";
            labelCopyright.Size = new Size(0xf8, 0x11);
            labelCopyright.TabIndex = 0x15;
            labelCopyright.Text = "Copyright";
            labelCopyright.TextAlign = ContentAlignment.MiddleLeft;
            textBoxDescription.Location = new Point(0x70, 0xcc);
            textBoxDescription.Name = "textBoxDescription";
            textBoxDescription.ReadOnly = true;
            textBoxDescription.Size = new Size(0xfb, 0x51);
            textBoxDescription.TabIndex = 0x1a;
            textBoxDescription.Text = "";
            base.AcceptButton = okButton;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = Color.FromArgb(0x5b, 0x6f, 0x7b);
            base.ClientSize = new Size(0x180, 0x14d);
            base.Controls.Add(tableLayoutPanel);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "AboutBox1";
            base.Padding = new Padding(9);
            base.ShowIcon = false;
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterScreen;
            Text = "About";
            tableLayoutPanel.ResumeLayout(false);
            tableLayoutPanel.PerformLayout();
            ((ISupportInitialize) logoPictureBox).EndInit();
            base.ResumeLayout(false);
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        public string AssemblyCompany
        {
            get
            {
                object[] customAttributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                if (customAttributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCompanyAttribute) customAttributes[0]).Company;
            }
        }

        public string AssemblyCopyright
        {
            get
            {
                object[] customAttributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                if (customAttributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCopyrightAttribute) customAttributes[0]).Copyright;
            }
        }

        public string AssemblyDescription
        {
            get
            {
                object[] customAttributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                if (customAttributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyDescriptionAttribute) customAttributes[0]).Description;
            }
        }

        public string AssemblyProduct
        {
            get
            {
                object[] customAttributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                if (customAttributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyProductAttribute) customAttributes[0]).Product;
            }
        }

        public string AssemblyProductVersion
        {
            get
            {
                return FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion;
            }
        }

        public string AssemblyTitle
        {
            get
            {
                object[] customAttributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (customAttributes.Length > 0)
                {
                    AssemblyTitleAttribute attribute = (AssemblyTitleAttribute) customAttributes[0];
                    if (attribute.Title != "")
                    {
                        return attribute.Title;
                    }
                }
                return Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        public string AssemblyVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        public string DescriptionBox
        {
            get
            {
                return textBoxDescription.Text;
            }
            set
            {
                textBoxDescription.Text = value;
            }
        }
    }
}

