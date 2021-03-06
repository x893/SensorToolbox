﻿namespace MainLauncher
{
	using Freescale.SASD.Communication;
	using MainLauncher.Properties;
	using System;
	using System.ComponentModel;
	using System.Diagnostics;
	using System.Drawing;
	using System.IO;
	using System.Net;
	using System.Reflection;
	using System.Runtime.InteropServices;
	using System.Text;
	using System.Windows.Forms;

	public enum HWID_Names
	{
		MMA7260_HWID = 0,
		MMA7360_HWID = 1,
		MMA7361_HWID = 2,
		MMA7455_HWID = 3,
		MMA7660_HWID = 4,
		MMA7660EVK_HWID = 5,
		MC33941_HWID = 6,
		MPR083_HWID = 7,
		MPR084_HWID = 8,
		MPR08X_HWID = 9,
		MPR08X3_HWID = 10,
		MPR08X4_HWID = 11,
		MPR031_HWID = 12,
		MPR121_HWID = 13,
		MPXAZ6115_HWID = 14,
		MPXV5010_HWID = 15,
		MPXV5004DP_HWID = 16,
		MMA8450_HWID = 17,
		MMA8451Q_HWID = 18,
		MMA8452Q_HWID = 19,
		MMA8453Q_HWID = 20,
		MPL3115A_HWID = 21,
		MMA8652FC_HWID = 22,
		MMA8653FC_HWID = 23,
		MMA3110_HWID = 24,
		MMA9550L = 25,
		MMA8491Q = 36,
		Unsupported = 0x3e8
	}

	public class Initial : Form
	{
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern bool ReleaseCapture();

		[DllImport("wininet.dll")]
		private static extern bool InternetGetConnectedState(ref int lpdwFlags, int dwReserved);

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);

		private bool BoardFound;
		private PictureBox CloseBox;
		private BlockingComm Comm;
		private IContainer components;
		private string GUICurrentlyRunning;
		private string HWID = string.Empty;
		private bool isJustOpen;
		private Label label1;
		private FwLoadedNames LoadedFW;
		private bool LoadingIDFW;
		private PictureBox pictureBox1;

		private const string STB_UPDATE_URL = "http://www.freescale.com/lgfiles/updates/Sensors/SensorToolboxInstaller.exe";
		private const string STB_VERSION_URL = "http://www.freescale.com/lgfiles/updates/Sensors";

		private string[] SupportedHWID_Id = new string[] { 
            "1001", "1002", "2001", "2004",
			"5431",
			"8001",
			"9001", "9002", "9003", "9004", "9005", "9101", "9102",
			"A001", "A002", "A003", 
            "3001", "3002", "3003", "3004", "5001", "3005", "3006", "4001", "0003", "3009"
         };

		private SW[] SupportedHWID_Sw;
		private string[] SupportedSW;
		private System.Windows.Forms.Timer TimerScreen;
		private UpdateProgress updateProgress;
		private Label uxLabelVersion;
		private WebClient webClient;

		public Initial(string par)
		{
			SupportedHWID_Sw = new SW[26];
			SupportedHWID_Sw[16] = SW.ProtonSW;
			SupportedHWID_Sw[17] = SW.MMA845XQ;
			SupportedHWID_Sw[18] = SW.MMA845XQ;
			SupportedHWID_Sw[19] = SW.MMA845XQ;
			SupportedHWID_Sw[20] = SW.Horizon;
			SupportedHWID_Sw[21] = SW.MMA845XQ;
			SupportedHWID_Sw[22] = SW.MMA845XQ;
			SupportedHWID_Sw[23] = SW.Maxwell;
			SupportedHWID_Sw[24] = SW.Eve;
			SupportedHWID_Sw[25] = SW.Plutino;

			SupportedSW = new string[] {
				@"SensorToolbox_MFC\SensorToolbox.exe",
				@"MMA8450\MMA845x-DEMO.exe",
				@"MMA845xQ\MMA845xQSW.exe",
				@"MPL3115A\MPL3115A-EVAL.exe",
				@"MMA845xQ\MMA845xQSW.exe",
				@"MAG3110\MAG3110-EVAL.exe",
				@"MMA955x\MMA955x_Evaluation Application.exe",
				@"MMA845xQ\MMA845xQSW.exe",
				""
			};
			isJustOpen = true;
			GUICurrentlyRunning = string.Empty;
			BoardFound = false;
			LoadingIDFW = false;

			InitializeComponent();
			isJustOpen = string.IsNullOrEmpty(par);

			GUICurrentlyRunning = par;
			InitializeCommunication();
			TimerScreen.Enabled = true;
			uxLabelVersion.Text = "Rev.: " + FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion;
		}

		#region CheckForUpdates
		private void CheckForUpdates()
		{
			int lpdwFlags = 0;

			if (InternetGetConnectedState(ref lpdwFlags, 0))
			{
				Version new_version;
				try
				{
					HttpWebRequest request = WebRequest.Create(STB_VERSION_URL) as HttpWebRequest;
					using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
					{
						StreamReader reader = new StreamReader(response.GetResponseStream());
						new_version = new Version(reader.ReadToEnd());
					}
				}
				catch (Exception)
				{
					return;
				}

				if (FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion.CompareTo(new_version.ToString()) < 0)
				{
					if ((MessageBox.Show("There is an update available. Would you like to download and install it?", "Sensor Toolbox Update...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes))
					{
						try
						{
							Uri address = new Uri(STB_UPDATE_URL);
							string fileName = Path.GetTempPath() + "SensorToolboxInstaller.exe";
							webClient = new WebClient();
							webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChangedCallback);
							webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(FileCompletedCallback);
							using (updateProgress = new UpdateProgress())
							{
								updateProgress.SetInitialInstance(this);
								updateProgress.SetVersionLabel(new_version.ToString());
								updateProgress.ShowDialog();
							}
							webClient.DownloadFileAsync(address, fileName);
						}
						catch (Exception)
						{
							MessageBox.Show("An error occurred while downloading Sensor Toolbox update! Please try again.", "Error...", MessageBoxButtons.OK, MessageBoxIcon.Hand);
						}
					}
				}
			}
		}

		public void CancelDownload()
		{
			webClient.CancelAsync();
		}

		private void ProgressChangedCallback(object sender, DownloadProgressChangedEventArgs e)
		{
			updateProgress.SetBytesLabel((((((double)e.BytesReceived) / 1024.0) / 1024.0)).ToString("F01") + " MB of " + (((((double)e.TotalBytesToReceive) / 1024.0) / 1024.0)).ToString("F01") + " MB received");
			updateProgress.SetProgress(e.ProgressPercentage);
		}

		private void FileCompletedCallback(object sender, AsyncCompletedEventArgs e)
		{
			updateProgress.Close();
			if (e.Cancelled)
				MessageBox.Show("Sensor Toolbox update was cancelled!", "Error...", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			else if (e.Error != null)
				MessageBox.Show("An error occurred while downloading Sensor Toolbox update! Please try again.", "Error...", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			else
			{
				try
				{
					Process.Start(Path.GetTempPath() + "SensorToolboxInstaller.exe");
				}
				catch (Exception)
				{
					MessageBox.Show("An error occurred while installing the application. Please try again.", "Error...", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
				finally
				{
					Application.Exit();
				}
			}
		}
		#endregion

		private void CloseBox_Click(object sender, EventArgs e)
		{
			End();
		}

		private void End()
		{
			Comm.End();
			base.Close();
		}

		public void CommEventCallback(object sender, CommEvent cmd)
		{
			if (cmd == CommEvent.CommOpen)
				HWID = GetHWID(Comm.GetCommStatus());
		}

		private HWID_Names FindHW(string id)
		{
			for (int hwid = 0; hwid < SupportedHWID_Id.Length; hwid++)
				if (SupportedHWID_Id[hwid].Equals(id.ToUpper()))
					return (HWID_Names)hwid;
			return HWID_Names.Unsupported;
		}

		private string GetHWID(string complete_id)
		{
			string hwid;
			Encoding encoding = Encoding.GetEncoding("Windows-1252");
			int baudRate = Comm.GetSerialPort().BaudRate;
			if (complete_id.IndexOf("HW:") != -1)
			{
				LoadingIDFW = false;
				hwid = complete_id.Substring(complete_id.IndexOf("HW:") + 3, 4);
				string swid = complete_id.Substring(complete_id.IndexOf("SW:") + 3, 4);
				if (((swid == "ffff" && baudRate == 115200) || hwid == "30ff") || hwid == "3000")
				{
					Loader loader;
					LoadingIDFW = true;
					if ((hwid == "30ff" || swid == "ffff") && LoadedFW == FwLoadedNames.BOOTLOADER)
					{
						LoadedFW = FwLoadedNames.ID_FW;
						loader = new Loader(Comm, "ID-FW.s19", "hidden");
					}
					else if (hwid == "30ff" && LoadedFW == FwLoadedNames.ID_FW)
					{
						LoadedFW = FwLoadedNames.Plutino;
						loader = new Loader(Comm, "ID-FW-Plutino.s19", "hidden");
					}
					hwid = "";
					Comm.Close();
					Comm.FindAnyHw();
					LoadingIDFW = false;
				}
				return hwid;
			}
			hwid = complete_id.Substring(complete_id.IndexOf(" with ID ") + " with ID ".Length, 2);
			return string.Format("{0:x4}", (encoding.GetBytes(hwid.Substring(0, 1))[0] << 8) | encoding.GetBytes(hwid.Substring(1, 1))[0]);
		}

		private void Initial_Load(object sender, EventArgs e)
		{
			LoadedFW = FwLoadedNames.BOOTLOADER;
			//! CheckForUpdates();
			Comm.FindAnyHw();
		}

		private void Initial_FormClosed(object sender, FormClosedEventArgs e)
		{
			if (!BoardFound)
				Console.Write("Exit request");
			Comm.End();
		}


		private void InitializeCommunication()
		{
			Comm = new BlockingComm();
			Comm.CommEvents += new CommEventHandler(CommEventCallback);
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
			components = new Container();
			TimerScreen = new System.Windows.Forms.Timer(components);
			pictureBox1 = new PictureBox();
			label1 = new Label();
			CloseBox = new PictureBox();
			uxLabelVersion = new Label();
			((ISupportInitialize)pictureBox1).BeginInit();
			((ISupportInitialize)CloseBox).BeginInit();
			base.SuspendLayout();
			TimerScreen.Interval = 200;
			TimerScreen.Tick += new EventHandler(TimerScreen_Tick);
			pictureBox1.Cursor = Cursors.NoMove2D;
			pictureBox1.Image = Resources.stb;
			pictureBox1.Location = new Point(0, 0);
			pictureBox1.Name = "pictureBox1";
			pictureBox1.Size = new Size(0x252, 0x84);
			pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
			pictureBox1.TabIndex = 1;
			pictureBox1.TabStop = false;
			pictureBox1.MouseDown += new MouseEventHandler(pictureBox1_MouseDown);
			label1.BackColor = Color.FromArgb(0x2c, 0x89, 0xa8);
			label1.Font = new Font("Tahoma", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			label1.ForeColor = Color.FromArgb(0xff, 0xc2, 12);
			label1.ImageAlign = ContentAlignment.TopCenter;
			label1.Location = new Point(2, 0x77);
			label1.Name = "label1";
			label1.Size = new Size(0x16e, 12);
			label1.TabIndex = 2;
			label1.Text = "label1";
			CloseBox.BackColor = Color.FromArgb(0x9a, 0xc3, 0xd7);
			CloseBox.Cursor = Cursors.Hand;
			CloseBox.Image = Resources.close;
			CloseBox.Location = new Point(0x23d, 1);
			CloseBox.Name = "CloseBox";
			CloseBox.Size = new Size(0x13, 0x13);
			CloseBox.SizeMode = PictureBoxSizeMode.StretchImage;
			CloseBox.TabIndex = 3;
			CloseBox.TabStop = false;
			CloseBox.Click += new EventHandler(CloseBox_Click);
			uxLabelVersion.AutoSize = true;
			uxLabelVersion.BackColor = Color.FromArgb(0x2c, 0x89, 0xa8);
			uxLabelVersion.Font = new Font("Tahoma", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			uxLabelVersion.ForeColor = Color.FromArgb(0xff, 0xc2, 12);
			uxLabelVersion.ImageAlign = ContentAlignment.TopCenter;
			uxLabelVersion.Location = new Point(0x193, 0x74);
			uxLabelVersion.Name = "uxLabelVersion";
			uxLabelVersion.Size = new Size(0x17, 13);
			uxLabelVersion.TabIndex = 2;
			uxLabelVersion.Text = "----";
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new Size(0x254, 0x91);
			base.Controls.Add(CloseBox);
			base.Controls.Add(uxLabelVersion);
			base.Controls.Add(label1);
			base.Controls.Add(pictureBox1);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			base.Name = "Initial";
			base.Opacity = 0.95;
			base.StartPosition = FormStartPosition.CenterScreen;
			Text = "SensorToolbox - Detecting HW";
			base.TransparencyKey = SystemColors.Control;
			base.Load += new EventHandler(Initial_Load);
			base.FormClosed += new FormClosedEventHandler(Initial_FormClosed);
			((ISupportInitialize)pictureBox1).EndInit();
			((ISupportInitialize)CloseBox).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
		#endregion

		private void LaunchApp(string app, string args)
		{
			app = Path.Combine(Application.StartupPath, app);
			ProcessStartInfo startInfo = new ProcessStartInfo(app);
			startInfo.WindowStyle = ProcessWindowStyle.Normal;
			startInfo.WorkingDirectory = app.Substring(0, app.LastIndexOf("\\"));
			startInfo.Arguments = args;
			Process.Start(startInfo);
		}

		private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				ReleaseCapture();
				SendMessage(base.Handle, 0xA1, 2, 0);
			}
		}

		private void RunApplication(string id)
		{
			string args = "";
			HWID_Names hwid = HWID_Names.Unsupported;
			SW swid = SW.Unsupported;
			BoardFound = true;
			hwid = FindHW(id);

			if (id == "0003")
				args = Comm.GetSerialPort().PortName;

			if (hwid == HWID_Names.Unsupported)
				swid = SW.Unsupported;
			else
				swid = SupportedHWID_Sw[(int)hwid];
			
			TimerScreen.Enabled = false;
			Comm.End();

			if (isJustOpen)
			{	// SW.MMA845XQ
				if (SW.Unsupported == swid)
					MessageBox.Show("Board not recognized or is unsupported.  The application will now shut down.");
				else
				{
					BoardFound = true;
					LaunchApp(SupportedSW[(int)swid], args);
				}
			}
			else if (GUICurrentlyRunning == swid.ToString())
			{
				Console.Write("SW:" + swid.ToString() + ", HW:" + id);
				LaunchApp(SupportedSW[(int)swid], args);
			}
			else
			{
				Console.Write("Exit request");
				LaunchApp(SupportedSW[(int)swid], args);
			}
			End();
		}

		private void TimerScreen_Tick(object sender, EventArgs e)
		{
			if (LoadingIDFW)
				label1.Text = "Loading Identification FW...";
			else
				label1.Text = Comm.GetCommStatus();

			if (HWID != string.Empty)
			{
				TimerScreen.Enabled = false;
				RunApplication(HWID);
			}
		}
	}
}