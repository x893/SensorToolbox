﻿namespace Freescale.SASD.Communication
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Threading;
    using System.Windows.Forms;

    public class Loader : Form
    {
        private SerialPort_Corrected m_serialPort;
        private Button btnFile;
        private Button btnUpload;
        private IContainer components;
        private BoardComm dv;
        private TextBox Filename;
        private Label label1;
        public bool LoadComplete;
        private bool LoaderError;
        private OpenFileDialog openS19File;
        private ProgressBar Progress;
        private string S19Content;
        private bool silent;

        public Loader(object cc)
        {
            components = null;
            InitializeComponent();
            dv = ((BlockingComm) cc).GetBoardComm();
            S19Content = "";
        }

        public Loader(object cc, string file)
        {
            components = null;
            InitializeComponent();
            base.Show();
            dv = (BoardComm) cc;
            S19Content = "";
            Filename.Text = Path.Combine(Application.StartupPath, file);
            btnUpload_Click(this, new EventArgs());
        }

        public Loader(BoardComm cc, string file, string mode)
        {
            InitializeComponent();

            silent = false;
            LoaderError = false;
            LoadComplete = false;
            if (!mode.Equals("hidden"))
                base.Show();
            dv = cc;
            S19Content = "";
            Filename.Text = Path.Combine(Application.StartupPath, file);
            if (mode.Equals("silent") || mode.Equals("hidden"))
                silent = true;
            btnUpload_Click(this, new EventArgs());
        }

        public Loader(object cc, string file, string mode)
        {
            components = null;

            InitializeComponent();

            silent = false;
            LoaderError = false;
            LoadComplete = false;
            if (!mode.Equals("hidden"))
                base.Show();

            dv = ((BlockingComm) cc).GetBoardComm();
            S19Content = "";
            Filename.Text = Path.Combine(Application.StartupPath, file);
            if (mode.Equals("silent") || mode.Equals("hidden"))
                silent = true;

            btnUpload_Click(this, new EventArgs());
        }

        private void btnFile_Click(object sender, EventArgs e)
        {
            openS19File.InitialDirectory = Application.StartupPath;
            openS19File.ShowDialog();
            Filename.Text = openS19File.FileName;
        }

		private void LoadFile()
		{
			StreamReader reader = File.OpenText(Filename.Text);
			S19Content = reader.ReadToEnd();
			reader.Close();
		}

		private void ValidateFile()
		{
		}

		private void TransformS19()
		{
			S19Content = S19Content.Replace("\r\nS", "\r s\rS");
			S19Content = S19Content.Replace("\n", "");
			S19Content = " s\r" + S19Content + "\r";
		}

        private void btnUpload_Click(object sender, EventArgs e)
        {
            LoadComplete = false;
            try
            {
                dv.SetBootloaderMode();
                m_serialPort = dv.GetSerialPort();
                m_serialPort.ReadTimeout = 0x2710;
                m_serialPort.NewLine = "\r";
                LoadFile();
                ValidateFile();
                TransformS19();
                SendS19();
                dv.ResetBootloaderMode();
            }
            catch (Exception exception)
            {
                LoaderError = true;
                STBLogger.AddEvent(this, STBLogger.EventLevel.Error, exception.Message, exception.Message);
                if (!silent)
                {
                    MessageBox.Show("An error has occurred while loading the file to the board:" + exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
                dv.ResetBootloaderMode();
            }
            LoadComplete = true;
            base.Close();
        }

        public bool getError()
        {
            return LoaderError;
        }

		#region InitializeComponent
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
			openS19File = new System.Windows.Forms.OpenFileDialog();
			Filename = new System.Windows.Forms.TextBox();
			label1 = new System.Windows.Forms.Label();
			btnFile = new System.Windows.Forms.Button();
			btnUpload = new System.Windows.Forms.Button();
			Progress = new System.Windows.Forms.ProgressBar();
			SuspendLayout();
			// 
			// openS19File
			// 
			openS19File.Filter = "\"S19 Files (*.s19)\" | *.S19";
			openS19File.InitialDirectory = "Application.StartupPath";
			// 
			// Filename
			// 
			Filename.Location = new System.Drawing.Point(10, 23);
			Filename.Name = "Filename";
			Filename.Size = new System.Drawing.Size(365, 20);
			Filename.TabIndex = 0;
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.Location = new System.Drawing.Point(7, 9);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size(95, 13);
			label1.TabIndex = 1;
			label1.Text = "S19 File to upload:";
			// 
			// btnFile
			// 
			btnFile.Location = new System.Drawing.Point(381, 22);
			btnFile.Name = "btnFile";
			btnFile.Size = new System.Drawing.Size(28, 20);
			btnFile.TabIndex = 2;
			btnFile.Text = "...";
			btnFile.UseVisualStyleBackColor = true;
			btnFile.Click += new System.EventHandler(btnFile_Click);
			// 
			// btnUpload
			// 
			btnUpload.Location = new System.Drawing.Point(175, 62);
			btnUpload.Name = "btnUpload";
			btnUpload.Size = new System.Drawing.Size(75, 23);
			btnUpload.TabIndex = 3;
			btnUpload.Text = "Upload";
			btnUpload.UseVisualStyleBackColor = true;
			btnUpload.Click += new System.EventHandler(btnUpload_Click);
			// 
			// Progress
			// 
			Progress.Location = new System.Drawing.Point(10, 91);
			Progress.Name = "Progress";
			Progress.Size = new System.Drawing.Size(399, 23);
			Progress.Step = 1;
			Progress.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
			Progress.TabIndex = 4;
			Progress.Value = 100;
			Progress.Visible = false;
			// 
			// Loader
			// 
			AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			ClientSize = new System.Drawing.Size(419, 120);
			Controls.Add(Progress);
			Controls.Add(btnUpload);
			Controls.Add(btnFile);
			Controls.Add(label1);
			Controls.Add(Filename);
			FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			Name = "Loader";
			StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			Text = "Loader";
			ResumeLayout(false);
			PerformLayout();

        }
		#endregion

		private bool SendCommCMD(string send, string receive)
        {
            string str = "";
            m_serialPort.Write(send);

            if (receive.Length > 0)
                str = m_serialPort.ReadLine() + m_serialPort.NewLine;
            else
            {
                Thread.Sleep(30);
                m_serialPort.ReadExisting();
            }
            return (str.IndexOf(receive) != -1);
        }

        private void SendS19()
        {
            string oldValue = "";
            int length = 0;
            while (length < 5)
            {
                SendCommCMD(" v\x0001\x0016\x00a5\r", "");
                length++;
            }

            if (!(SendCommCMD(" h\r", " H\r") && SendCommCMD(" i\r", " I0\x0001\x00ff\x00ff@\x0002\x00ff\x00ff\r")))
                throw new Exception("The board's bootloader was not recognized");

            if (!SendCommCMD(" z\r", " Z\r"))
                throw new Exception("The flash was not erased");

            length = S19Content.Length;
            Progress.Value = 0;
            Progress.Visible = true;
            oldValue = S19Content.Substring(0, S19Content.IndexOf(" s\rS", 1));
            while ((S19Content.Length > 0) && (oldValue.Length > 0))
            {
                S19Content = S19Content.Replace(oldValue, "");
                Progress.Value = ((length * 100) - (S19Content.Length * 100)) / length;
                Refresh();
                if (!SendCommCMD(oldValue, " S\r"))
                    throw new Exception("S19 file was not transferred completely");
                
				try
                {
                    oldValue = S19Content.Substring(S19Content.IndexOf(" "), S19Content.IndexOf("\r", 3) + 1);
                }
                catch (Exception)
                {
                    oldValue = "";
                    S19Content = "";
                }
            }
        }
    }
}

