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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutBox1));
			okButton = new System.Windows.Forms.Button();
			tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
			logoPictureBox = new System.Windows.Forms.PictureBox();
			labelProductName = new System.Windows.Forms.Label();
			labelVersion = new System.Windows.Forms.Label();
			labelCopyright = new System.Windows.Forms.Label();
			textBoxDescription = new System.Windows.Forms.RichTextBox();
			tableLayoutPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(logoPictureBox)).BeginInit();
			SuspendLayout();
			// 
			// okButton
			// 
			okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			okButton.BackColor = System.Drawing.SystemColors.Control;
			okButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			okButton.Location = new System.Drawing.Point(288, 292);
			okButton.Name = "okButton";
			okButton.Size = new System.Drawing.Size(75, 20);
			okButton.TabIndex = 24;
			okButton.Text = "&OK";
			okButton.UseVisualStyleBackColor = false;
			okButton.Click += new System.EventHandler(okButton_Click);
			// 
			// tableLayoutPanel
			// 
			tableLayoutPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(111)))), ((int)(((byte)(123)))));
			tableLayoutPanel.ColumnCount = 2;
			tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
			tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
			tableLayoutPanel.Controls.Add(okButton, 1, 5);
			tableLayoutPanel.Controls.Add(logoPictureBox, 0, 0);
			tableLayoutPanel.Controls.Add(labelProductName, 1, 1);
			tableLayoutPanel.Controls.Add(labelVersion, 1, 2);
			tableLayoutPanel.Controls.Add(labelCopyright, 1, 3);
			tableLayoutPanel.Controls.Add(textBoxDescription, 1, 4);
			tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			tableLayoutPanel.Location = new System.Drawing.Point(9, 9);
			tableLayoutPanel.Name = "tableLayoutPanel";
			tableLayoutPanel.RowCount = 6;
			tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15.44118F));
			tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15.44118F));
			tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15.44118F));
			tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 53.67647F));
			tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			tableLayoutPanel.Size = new System.Drawing.Size(366, 315);
			tableLayoutPanel.TabIndex = 0;
			// 
			// logoPictureBox
			// 
			tableLayoutPanel.SetColumnSpan(logoPictureBox, 2);
			logoPictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
			logoPictureBox.Image = ((System.Drawing.Image)(resources.GetObject("logoPictureBox.Image")));
			logoPictureBox.Location = new System.Drawing.Point(3, 3);
			logoPictureBox.Name = "logoPictureBox";
			logoPictureBox.Size = new System.Drawing.Size(360, 120);
			logoPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			logoPictureBox.TabIndex = 25;
			logoPictureBox.TabStop = false;
			// 
			// labelProductName
			// 
			labelProductName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(111)))), ((int)(((byte)(123)))));
			labelProductName.Dock = System.Windows.Forms.DockStyle.Fill;
			labelProductName.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			labelProductName.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			labelProductName.Location = new System.Drawing.Point(115, 126);
			labelProductName.Margin = new System.Windows.Forms.Padding(6, 0, 3, 0);
			labelProductName.MaximumSize = new System.Drawing.Size(0, 17);
			labelProductName.Name = "labelProductName";
			labelProductName.Size = new System.Drawing.Size(248, 17);
			labelProductName.TabIndex = 19;
			labelProductName.Text = "Product Name";
			labelProductName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// labelVersion
			// 
			labelVersion.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(111)))), ((int)(((byte)(123)))));
			labelVersion.Dock = System.Windows.Forms.DockStyle.Fill;
			labelVersion.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			labelVersion.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			labelVersion.Location = new System.Drawing.Point(115, 151);
			labelVersion.Margin = new System.Windows.Forms.Padding(6, 0, 3, 0);
			labelVersion.MaximumSize = new System.Drawing.Size(0, 17);
			labelVersion.Name = "labelVersion";
			labelVersion.Size = new System.Drawing.Size(248, 17);
			labelVersion.TabIndex = 0;
			labelVersion.Text = "Product Version";
			labelVersion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// labelCopyright
			// 
			labelCopyright.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(111)))), ((int)(((byte)(123)))));
			labelCopyright.Dock = System.Windows.Forms.DockStyle.Fill;
			labelCopyright.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			labelCopyright.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			labelCopyright.Location = new System.Drawing.Point(115, 176);
			labelCopyright.Margin = new System.Windows.Forms.Padding(6, 0, 3, 0);
			labelCopyright.MaximumSize = new System.Drawing.Size(0, 17);
			labelCopyright.Name = "labelCopyright";
			labelCopyright.Size = new System.Drawing.Size(248, 17);
			labelCopyright.TabIndex = 21;
			labelCopyright.Text = "Copyright";
			labelCopyright.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// textBoxDescription
			// 
			textBoxDescription.Location = new System.Drawing.Point(112, 204);
			textBoxDescription.Name = "textBoxDescription";
			textBoxDescription.ReadOnly = true;
			textBoxDescription.Size = new System.Drawing.Size(251, 81);
			textBoxDescription.TabIndex = 26;
			textBoxDescription.Text = "";
			// 
			// AboutBox1
			// 
			AcceptButton = okButton;
			AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(111)))), ((int)(((byte)(123)))));
			ClientSize = new System.Drawing.Size(384, 333);
			Controls.Add(tableLayoutPanel);
			FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			MaximizeBox = false;
			MinimizeBox = false;
			Name = "AboutBox1";
			Padding = new System.Windows.Forms.Padding(9);
			ShowIcon = false;
			ShowInTaskbar = false;
			StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			Text = "About";
			tableLayoutPanel.ResumeLayout(false);
			tableLayoutPanel.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(logoPictureBox)).EndInit();
			ResumeLayout(false);

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

