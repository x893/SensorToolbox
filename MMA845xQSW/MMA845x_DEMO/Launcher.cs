﻿namespace MMA845x_DEMO
{
	using Freescale.SASD.Communication;
	using GlobalSTB;
	using MMA845x_DEMO.Properties;
	using MMA845xQEvaluation;
	using System;
	using System.ComponentModel;
	using System.Diagnostics;
	using System.Drawing;
	using System.IO;
	using System.Reflection;
	using System.Resources;
	using System.Threading;
	using System.Windows.Forms;

	public class Launcher : Form
	{
		private enum eCommMode
		{
			Bootloader = 3,
			Closed = 1,
			FindingHW = 2,
			Running = 4
		}
		#region Privates 
		private ToolStripMenuItem aboutToolStripMenuItem;
		private Button bGraphicalDatalogger;
		private Button bNVMDatalogger;
		private Loader Bootloader;
		private Button btnFlick;
		private Button btnTapDemo;
		private Button buttonFIFOLP;
		private Button buttonOrientation;
		private Button buttonTilt;
		private CommClass cc;
		private eCommMode CommMode = eCommMode.Closed;
		private StatusStrip CommStrip;
		private ToolStripDropDownButton CommStripButton;
		private IContainer components = null;
		private AccelController ControllerObj = new AccelController();
		private deviceID m_DeviceID = deviceID.Unsupported;
		private DirectionalTapDemo DirectionalTapDemonstration;
		private TapDemo dmTapDemo;
		private bool DoReconnect;
		private BoardComm dv;
		private VeyronEvaluationSoftware8451 Eval;
		private Button EvalModeButton;
		private bool FIFODemosEnabled = true;
		private FIFOLowPowerDemo FIFOLPDemostration;
		private ToolStripMenuItem firmwareVersionToolStripMenuItem;
		private GroupBox groupBoxApplicationSelection;
		private Image ImageGreen;
		private Image ImageRed;
		private Image ImageYellow;
		private ToolStripMenuItem itemAbout;
		private ToolStripMenuItem itemHelpAbout;
		private Label label1;
		private Label label2;
		private Label label3;
		private Label label4;
		private Label label5;
		private Label label6;
		private Label label7;
		private Label label8;
		private MenuStrip mnuMenu;
		private OrientationDemo OrientationDemonstration;
		private ToolStripMenuItem pCSoftwareToolStripMenuItem;
		private PictureBox pictureBox1;
		private PictureBox pictureBox2;
		private Panel pnlBootloader;
		private ShakeDemo ShakeDemostration;
		private TiltApp2 tilt2;
		private System.Windows.Forms.Timer timer1;
		private ToolStripDropDownButton toolStripDropDownButton1;
		private ToolStripStatusLabel toolStripStatusLabel;
		private ToolStripStatusLabel toolStripStatusLabel1;
		#endregion

		public Launcher(string[] args)
		{
			InitializeComponent();

			groupBoxApplicationSelection.Enabled = false;
			ControllerObj.InitializeController();
			LoadResource();
			InitializeStatusBar();
			CommStripButton_Click(this, new EventArgs());
			timer1.Enabled = true;
		}

		private void Apps_FormClosing(object sender, FormClosingEventArgs e)
		{
			try
			{
				ControllerObj.EndController();
			}
			catch (Exception)
			{
				MessageBox.Show("No communication with the device, please check USB connection.", "USB Communication Failure", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				base.Close();
			}
		}

		private void bNVMDatalogger_Click(object sender, EventArgs e)
		{
			ProcessStartInfo startInfo = new ProcessStartInfo(Path.Combine(Application.StartupPath, "MMA845xQNVMDL.exe"));
			Process.Start(startInfo).WaitForExit(100);
			base.Close();
		}

		private void btnDTap_Click(object sender, EventArgs e)
		{
			using (DirectionalTapDemonstration = new DirectionalTapDemo(ControllerObj))
			{
				DirectionalTapDemonstration.ShowDialog();
				DirectionalTapDemonstration.EndDemo();
			}
			DirectionalTapDemonstration = null;
		}

		private void btnFlick_Click(object sender, EventArgs e)
		{
			using (ShakeDemostration = new ShakeDemo(ControllerObj))
			{
				ShakeDemostration.ShowDialog();
				ShakeDemostration.EndDemo();
			}
			ShakeDemostration = null;
		}

		private void btnTapDemo_Click(object sender, EventArgs e)
		{
			using (dmTapDemo = new TapDemo(ControllerObj))
			{
				dmTapDemo.ShowDialog();
				dmTapDemo.EndDemo();
			}
			dmTapDemo = null;
		}

		private void buttonFIFOLP_Click(object sender, EventArgs e)
		{
			using (FIFOLPDemostration = new FIFOLowPowerDemo(ControllerObj))
			{
				FIFOLPDemostration.ShowDialog();
				FIFOLPDemostration.EndDemo();
			}
			FIFOLPDemostration = null;
		}

		private void buttonOrientation_Click(object sender, EventArgs e)
		{
			using (OrientationDemonstration = new OrientationDemo(ControllerObj))
			{
				OrientationDemonstration.ShowDialog();
				OrientationDemonstration.EndDemo();
			}
			OrientationDemonstration = null;
		}

		private void buttonScope_Click(object sender, EventArgs e)
		{
			ProcessStartInfo startInfo = new ProcessStartInfo(Path.Combine(Application.StartupPath, "MMA845xQDL.exe"));
			Process.Start(startInfo).WaitForExit(100);
			base.Close();
		}

		private void buttonTilt_Click(object sender, EventArgs e)
		{
			using (tilt2 = new TiltApp2(ControllerObj))
			{
				tilt2.ShowDialog();
				tilt2.EndDemo();
			}
			tilt2 = null;
		}

		private void CommCallback(object o, CommEvent cmd)
		{
		}

		private void CommStripButton_Click(object sender, EventArgs e)
		{
			switch (CommMode)
			{
				case eCommMode.Closed:
					dv.FindHw(" I0");
					break;

				case eCommMode.Bootloader:
					CommStripButton.Text = "Find HW";
					dv.Close();
					break;

				case eCommMode.Running:
					CommStripButton.Text = "Find HW";
					dv.Close();
					break;
			}
		}

		private void evaluationModeButton_Click(object sender, EventArgs e)
		{
			using (Eval = new VeyronEvaluationSoftware8451(ControllerObj))
			{
				Eval.ShowDialog();
				Eval.EndDemo();
			}
			Eval = null;
		}

		private void InitializeStatusBar()
		{
			dv = new BoardComm();
			cc = dv;
			ControllerObj.GetCommObject(ref cc);
			dv = (BoardComm)cc;
			dv.CommEvents += new CommEventHandler(CommCallback);
		}

		private void itemHelpAbout_Click(object sender, EventArgs e)
		{
			new GlobalSTB.AboutBox1().ShowDialog();
		}

		private void label5_Click(object sender, EventArgs e)
		{
			Bootloader = new Loader(dv, "MMA845x-FW-Bootloader.s19", "normal"); //!!! "silent");
		}

		private void LoadResource()
		{
			try
			{
				Assembly executingAssembly = Assembly.GetExecutingAssembly();
				ResourceManager manager = new ResourceManager("MMA845x_DEMO.Properties.Resources", executingAssembly);
				ImageGreen = (Image)manager.GetObject("imgGreenState");
				ImageYellow = (Image)manager.GetObject("imgYellowState");
				ImageRed = (Image)manager.GetObject("imgRedState");
			}
			catch (Exception exception)
			{
				STBLogger.AddEvent(this, STBLogger.EventLevel.Information, "Exception", exception.Message + exception.Source + exception.StackTrace);
				ImageGreen = CommStripButton.Image;
				ImageYellow = CommStripButton.Image;
				ImageRed = CommStripButton.Image;
			}
		}

		private void pCSoftwareToolStripMenuItem_Click(object sender, EventArgs e)
		{
			new AboutPCSoftware().ShowDialog();
		}

		private void Reconnect()
		{
			bool flag = false;
			ProcessStartInfo startInfo = new ProcessStartInfo(Path.Combine(Application.StartupPath, @"..\MainLauncher.exe"))
			{
				Arguments = "ProtonSW",
				RedirectStandardOutput = true,
				UseShellExecute = false
			};
			Process process = Process.Start(startInfo);
			StreamReader standardOutput = process.StandardOutput;
			process.WaitForExit(0x4650);
			if (process.HasExited)
			{
				Console.WriteLine(standardOutput.ReadToEnd());
				flag = true;
			}
			else
			{
				flag = true;
			}
			if (flag)
			{
				base.Close();
			}
			else
			{
				CommStripButton_Click(this, new EventArgs());
				timer1.Enabled = true;
			}
		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			UpdateCommStrip();
			if ((m_DeviceID == deviceID.MMA8451Q) || (m_DeviceID == deviceID.MMA8652FC))
			{
				buttonFIFOLP.Enabled = FIFODemosEnabled;
				btnTapDemo.Enabled = FIFODemosEnabled;
				label4.Enabled = FIFODemosEnabled;
			}
			else if (m_DeviceID == deviceID.MMA8491Q)
			{
				buttonFIFOLP.Enabled = false;
				btnTapDemo.Enabled = false;
				label4.Enabled = false;
				buttonOrientation.Enabled = false;
				bGraphicalDatalogger.Enabled = false;
				btnFlick.Enabled = false;
			}
			else
			{
				buttonFIFOLP.Enabled = false;
				btnTapDemo.Enabled = false;
				label4.Enabled = false;
			}
		}

		private void UpdateCommStrip()
		{
			if (DoReconnect)
			{
				DoReconnect = false;
				timer1.Enabled = false;
				Thread.Sleep(100);
				Reconnect();
			}

			string commStatus = dv.GetCommStatus();
			switch (dv.GetCommState())
			{
				case CommunicationState.HWFind:
					CommMode = eCommMode.FindingHW;
					break;

				case CommunicationState.Idle:
					CommMode = eCommMode.Closed;
					break;

				case CommunicationState.Ready:
					CommMode = eCommMode.Running;
					string hwid = commStatus.Substring(commStatus.IndexOf("HW:") + 3, 4);
					if (hwid != "3002" &&
						hwid != "3003" &&
						hwid != "3004" &&
						hwid != "3005" &&
						hwid != "3006" &&
						hwid != "3009"
						)
					{
						dv.Close();
						DoReconnect = true;
					}
					break;

				default:
					CommMode = eCommMode.Closed;
					break;
			}

			toolStripStatusLabel.Text = commStatus;

			if (ControllerObj.DeviceID != deviceID.Unsupported && ControllerObj.DeviceID != m_DeviceID)
			{
				m_DeviceID = ControllerObj.DeviceID;
				groupBoxApplicationSelection.Text = ControllerObj.DeviceID.ToString();
			}

			pnlBootloader.Visible = false;
			switch (CommMode)
			{
				case eCommMode.Bootloader:
					groupBoxApplicationSelection.Enabled = false;
					CommStripButton.Image = ImageGreen;
					pnlBootloader.Visible = true;
					break;

				case eCommMode.FindingHW:
					groupBoxApplicationSelection.Enabled = false;
					CommStripButton.Image = ImageYellow;
					break;
				case eCommMode.Running:
					groupBoxApplicationSelection.Enabled = true;
					CommStripButton.Image = ImageGreen;
					break;
				case eCommMode.Closed:
					groupBoxApplicationSelection.Enabled = false;
					CommStripButton.Image = ImageRed;
					DoReconnect = true;
					break;
			}
			CommStrip.Refresh();
		}

		#region InitializeComponent
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
				components.Dispose();
			base.Dispose(disposing);
		}

		private void firmwareVersionToolStripMenuItem_Click(object sender, EventArgs e)
		{
			new AboutFirmware().ShowDialog();
		}

		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Launcher));
			this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.firmwareVersionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.pCSoftwareToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.groupBoxApplicationSelection = new System.Windows.Forms.GroupBox();
			this.label8 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.bNVMDatalogger = new System.Windows.Forms.Button();
			this.btnFlick = new System.Windows.Forms.Button();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.EvalModeButton = new System.Windows.Forms.Button();
			this.bGraphicalDatalogger = new System.Windows.Forms.Button();
			this.btnTapDemo = new System.Windows.Forms.Button();
			this.buttonOrientation = new System.Windows.Forms.Button();
			this.buttonTilt = new System.Windows.Forms.Button();
			this.buttonFIFOLP = new System.Windows.Forms.Button();
			this.pictureBox2 = new System.Windows.Forms.PictureBox();
			this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
			this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
			this.CommStrip = new System.Windows.Forms.StatusStrip();
			this.CommStripButton = new System.Windows.Forms.ToolStripDropDownButton();
			this.pnlBootloader = new System.Windows.Forms.Panel();
			this.label5 = new System.Windows.Forms.Label();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.mnuMenu = new System.Windows.Forms.MenuStrip();
			this.itemAbout = new System.Windows.Forms.ToolStripMenuItem();
			this.itemHelpAbout = new System.Windows.Forms.ToolStripMenuItem();
			this.groupBoxApplicationSelection.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
			this.CommStrip.SuspendLayout();
			this.pnlBootloader.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.mnuMenu.SuspendLayout();
			this.SuspendLayout();
			// 
			// toolStripStatusLabel
			// 
			this.toolStripStatusLabel.BackColor = System.Drawing.SystemColors.ButtonFace;
			this.toolStripStatusLabel.Name = "toolStripStatusLabel";
			this.toolStripStatusLabel.Size = new System.Drawing.Size(482, 20);
			this.toolStripStatusLabel.Spring = true;
			this.toolStripStatusLabel.Text = "COM Port Not Connected, Please Connect";
			this.toolStripStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// timer1
			// 
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// aboutToolStripMenuItem
			// 
			this.aboutToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.firmwareVersionToolStripMenuItem,
            this.pCSoftwareToolStripMenuItem});
			this.aboutToolStripMenuItem.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
			this.aboutToolStripMenuItem.Size = new System.Drawing.Size(57, 20);
			this.aboutToolStripMenuItem.Text = "About";
			// 
			// firmwareVersionToolStripMenuItem
			// 
			this.firmwareVersionToolStripMenuItem.Name = "firmwareVersionToolStripMenuItem";
			this.firmwareVersionToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
			this.firmwareVersionToolStripMenuItem.Text = "Firmware Version";
			this.firmwareVersionToolStripMenuItem.Click += new System.EventHandler(this.firmwareVersionToolStripMenuItem_Click);
			// 
			// pCSoftwareToolStripMenuItem
			// 
			this.pCSoftwareToolStripMenuItem.Name = "pCSoftwareToolStripMenuItem";
			this.pCSoftwareToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
			this.pCSoftwareToolStripMenuItem.Text = "PC Software";
			this.pCSoftwareToolStripMenuItem.Click += new System.EventHandler(this.pCSoftwareToolStripMenuItem_Click);
			// 
			// groupBoxApplicationSelection
			// 
			this.groupBoxApplicationSelection.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBoxApplicationSelection.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(111)))), ((int)(((byte)(123)))));
			this.groupBoxApplicationSelection.Controls.Add(this.label8);
			this.groupBoxApplicationSelection.Controls.Add(this.label7);
			this.groupBoxApplicationSelection.Controls.Add(this.label6);
			this.groupBoxApplicationSelection.Controls.Add(this.bNVMDatalogger);
			this.groupBoxApplicationSelection.Controls.Add(this.btnFlick);
			this.groupBoxApplicationSelection.Controls.Add(this.label4);
			this.groupBoxApplicationSelection.Controls.Add(this.label3);
			this.groupBoxApplicationSelection.Controls.Add(this.label2);
			this.groupBoxApplicationSelection.Controls.Add(this.label1);
			this.groupBoxApplicationSelection.Controls.Add(this.EvalModeButton);
			this.groupBoxApplicationSelection.Controls.Add(this.bGraphicalDatalogger);
			this.groupBoxApplicationSelection.Controls.Add(this.btnTapDemo);
			this.groupBoxApplicationSelection.Controls.Add(this.buttonOrientation);
			this.groupBoxApplicationSelection.Controls.Add(this.buttonTilt);
			this.groupBoxApplicationSelection.Controls.Add(this.buttonFIFOLP);
			this.groupBoxApplicationSelection.Controls.Add(this.pictureBox2);
			this.groupBoxApplicationSelection.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.groupBoxApplicationSelection.ForeColor = System.Drawing.Color.AliceBlue;
			this.groupBoxApplicationSelection.Location = new System.Drawing.Point(11, 86);
			this.groupBoxApplicationSelection.Name = "groupBoxApplicationSelection";
			this.groupBoxApplicationSelection.Size = new System.Drawing.Size(493, 487);
			this.groupBoxApplicationSelection.TabIndex = 42;
			this.groupBoxApplicationSelection.TabStop = false;
			this.groupBoxApplicationSelection.Text = "MMA8451Q";
			// 
			// label8
			// 
			this.label8.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label8.ForeColor = System.Drawing.Color.Black;
			this.label8.Location = new System.Drawing.Point(262, 330);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(201, 46);
			this.label8.TabIndex = 25;
			this.label8.Text = "Supports delay\r\nup to 800Hz Full resolution\r\ntethered and untethered";
			this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label7
			// 
			this.label7.Enabled = false;
			this.label7.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label7.ForeColor = System.Drawing.Color.Black;
			this.label7.Location = new System.Drawing.Point(55, 339);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(201, 28);
			this.label7.TabIndex = 24;
			this.label7.Text = "Graphical output\r\nup to 200Hz Full resolution";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.BackColor = System.Drawing.Color.Transparent;
			this.label6.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label6.ForeColor = System.Drawing.Color.White;
			this.label6.Location = new System.Drawing.Point(184, 268);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(136, 16);
			this.label6.TabIndex = 23;
			this.label6.Text = "[[ XYZ Dataloggers ]]";
			// 
			// bNVMDatalogger
			// 
			this.bNVMDatalogger.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(138)))), ((int)(((byte)(167)))));
			this.bNVMDatalogger.Cursor = System.Windows.Forms.Cursors.Hand;
			this.bNVMDatalogger.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.bNVMDatalogger.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.bNVMDatalogger.ForeColor = System.Drawing.Color.Black;
			this.bNVMDatalogger.Location = new System.Drawing.Point(262, 287);
			this.bNVMDatalogger.MinimumSize = new System.Drawing.Size(100, 30);
			this.bNVMDatalogger.Name = "bNVMDatalogger";
			this.bNVMDatalogger.Size = new System.Drawing.Size(201, 40);
			this.bNVMDatalogger.TabIndex = 22;
			this.bNVMDatalogger.Text = "NVM Datalogger";
			this.bNVMDatalogger.UseVisualStyleBackColor = false;
			this.bNVMDatalogger.Click += new System.EventHandler(this.bNVMDatalogger_Click);
			// 
			// btnFlick
			// 
			this.btnFlick.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(138)))), ((int)(((byte)(167)))));
			this.btnFlick.Cursor = System.Windows.Forms.Cursors.Hand;
			this.btnFlick.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnFlick.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnFlick.ForeColor = System.Drawing.Color.Black;
			this.btnFlick.Location = new System.Drawing.Point(158, 123);
			this.btnFlick.Name = "btnFlick";
			this.btnFlick.Size = new System.Drawing.Size(201, 40);
			this.btnFlick.TabIndex = 15;
			this.btnFlick.Text = "Directional Flick";
			this.btnFlick.UseVisualStyleBackColor = false;
			this.btnFlick.Click += new System.EventHandler(this.btnFlick_Click);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label4.ForeColor = System.Drawing.Color.White;
			this.label4.Location = new System.Drawing.Point(161, 396);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(185, 16);
			this.label4.TabIndex = 21;
			this.label4.Text = "[[ Low Power FIFO Demos ]]";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label3.ForeColor = System.Drawing.Color.White;
			this.label3.Location = new System.Drawing.Point(161, 104);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(198, 16);
			this.label3.TabIndex = 20;
			this.label3.Text = "[[ Basic Directional Gestures ]]";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.BackColor = System.Drawing.Color.Transparent;
			this.label2.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.ForeColor = System.Drawing.Color.White;
			this.label2.Location = new System.Drawing.Point(184, 22);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(149, 16);
			this.label2.TabIndex = 19;
			this.label2.Text = "[[ System Evaluation ]]";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.BackColor = System.Drawing.Color.Transparent;
			this.label1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.ForeColor = System.Drawing.Color.White;
			this.label1.Location = new System.Drawing.Point(184, 186);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(154, 16);
			this.label1.TabIndex = 17;
			this.label1.Text = "[[ Tilt Demonstrations ]]";
			// 
			// EvalModeButton
			// 
			this.EvalModeButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(138)))), ((int)(((byte)(167)))));
			this.EvalModeButton.Cursor = System.Windows.Forms.Cursors.Hand;
			this.EvalModeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.EvalModeButton.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.EvalModeButton.ForeColor = System.Drawing.Color.Black;
			this.EvalModeButton.Location = new System.Drawing.Point(158, 41);
			this.EvalModeButton.MinimumSize = new System.Drawing.Size(100, 30);
			this.EvalModeButton.Name = "EvalModeButton";
			this.EvalModeButton.Size = new System.Drawing.Size(201, 40);
			this.EvalModeButton.TabIndex = 13;
			this.EvalModeButton.Text = "Full System Evaluation";
			this.EvalModeButton.UseVisualStyleBackColor = false;
			this.EvalModeButton.Click += new System.EventHandler(this.evaluationModeButton_Click);
			// 
			// bGraphicalDatalogger
			// 
			this.bGraphicalDatalogger.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(138)))), ((int)(((byte)(167)))));
			this.bGraphicalDatalogger.Cursor = System.Windows.Forms.Cursors.Hand;
			this.bGraphicalDatalogger.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.bGraphicalDatalogger.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.bGraphicalDatalogger.ForeColor = System.Drawing.Color.Black;
			this.bGraphicalDatalogger.Location = new System.Drawing.Point(55, 287);
			this.bGraphicalDatalogger.MinimumSize = new System.Drawing.Size(100, 30);
			this.bGraphicalDatalogger.Name = "bGraphicalDatalogger";
			this.bGraphicalDatalogger.Size = new System.Drawing.Size(201, 40);
			this.bGraphicalDatalogger.TabIndex = 13;
			this.bGraphicalDatalogger.Text = "Graphical Datalogger";
			this.bGraphicalDatalogger.UseVisualStyleBackColor = false;
			this.bGraphicalDatalogger.Click += new System.EventHandler(this.buttonScope_Click);
			// 
			// btnTapDemo
			// 
			this.btnTapDemo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(138)))), ((int)(((byte)(167)))));
			this.btnTapDemo.Cursor = System.Windows.Forms.Cursors.Hand;
			this.btnTapDemo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnTapDemo.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnTapDemo.ForeColor = System.Drawing.Color.Black;
			this.btnTapDemo.Location = new System.Drawing.Point(262, 415);
			this.btnTapDemo.MinimumSize = new System.Drawing.Size(100, 30);
			this.btnTapDemo.Name = "btnTapDemo";
			this.btnTapDemo.Size = new System.Drawing.Size(201, 40);
			this.btnTapDemo.TabIndex = 4;
			this.btnTapDemo.Text = "Directional Tap Low-Power with FIFO";
			this.btnTapDemo.UseVisualStyleBackColor = false;
			this.btnTapDemo.Click += new System.EventHandler(this.btnTapDemo_Click);
			// 
			// buttonOrientation
			// 
			this.buttonOrientation.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(138)))), ((int)(((byte)(167)))));
			this.buttonOrientation.Cursor = System.Windows.Forms.Cursors.Hand;
			this.buttonOrientation.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.buttonOrientation.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.buttonOrientation.ForeColor = System.Drawing.Color.Black;
			this.buttonOrientation.Location = new System.Drawing.Point(262, 205);
			this.buttonOrientation.MinimumSize = new System.Drawing.Size(100, 30);
			this.buttonOrientation.Name = "buttonOrientation";
			this.buttonOrientation.Size = new System.Drawing.Size(201, 40);
			this.buttonOrientation.TabIndex = 5;
			this.buttonOrientation.Text = "Low Resolution Portrait/Landscape";
			this.buttonOrientation.UseVisualStyleBackColor = false;
			this.buttonOrientation.Click += new System.EventHandler(this.buttonOrientation_Click);
			// 
			// buttonTilt
			// 
			this.buttonTilt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(138)))), ((int)(((byte)(167)))));
			this.buttonTilt.Cursor = System.Windows.Forms.Cursors.Hand;
			this.buttonTilt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.buttonTilt.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.buttonTilt.ForeColor = System.Drawing.Color.Black;
			this.buttonTilt.Location = new System.Drawing.Point(55, 205);
			this.buttonTilt.MinimumSize = new System.Drawing.Size(100, 30);
			this.buttonTilt.Name = "buttonTilt";
			this.buttonTilt.Size = new System.Drawing.Size(201, 40);
			this.buttonTilt.TabIndex = 6;
			this.buttonTilt.Text = "Hi Resolution Tilt Detection";
			this.buttonTilt.UseVisualStyleBackColor = false;
			this.buttonTilt.Click += new System.EventHandler(this.buttonTilt_Click);
			// 
			// buttonFIFOLP
			// 
			this.buttonFIFOLP.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(138)))), ((int)(((byte)(167)))));
			this.buttonFIFOLP.Cursor = System.Windows.Forms.Cursors.Hand;
			this.buttonFIFOLP.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.buttonFIFOLP.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.buttonFIFOLP.ForeColor = System.Drawing.Color.Black;
			this.buttonFIFOLP.Location = new System.Drawing.Point(55, 415);
			this.buttonFIFOLP.Name = "buttonFIFOLP";
			this.buttonFIFOLP.Size = new System.Drawing.Size(201, 40);
			this.buttonFIFOLP.TabIndex = 0;
			this.buttonFIFOLP.Text = "Directional Shake Low-Power with FIFO";
			this.buttonFIFOLP.UseVisualStyleBackColor = false;
			this.buttonFIFOLP.Click += new System.EventHandler(this.buttonFIFOLP_Click);
			// 
			// pictureBox2
			// 
			this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
			this.pictureBox2.Location = new System.Drawing.Point(6, 25);
			this.pictureBox2.Name = "pictureBox2";
			this.pictureBox2.Size = new System.Drawing.Size(481, 456);
			this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this.pictureBox2.TabIndex = 26;
			this.pictureBox2.TabStop = false;
			// 
			// toolStripDropDownButton1
			// 
			this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton1.Image")));
			this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
			this.toolStripDropDownButton1.ShowDropDownArrow = false;
			this.toolStripDropDownButton1.Size = new System.Drawing.Size(20, 20);
			this.toolStripDropDownButton1.Text = "toolStripDropDownButton1";
			// 
			// toolStripStatusLabel1
			// 
			this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
			this.toolStripStatusLabel1.Size = new System.Drawing.Size(23, 23);
			// 
			// CommStrip
			// 
			this.CommStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CommStripButton,
            this.toolStripStatusLabel});
			this.CommStrip.Location = new System.Drawing.Point(0, 576);
			this.CommStrip.Name = "CommStrip";
			this.CommStrip.Size = new System.Drawing.Size(517, 25);
			this.CommStrip.TabIndex = 47;
			this.CommStrip.Text = "statusStrip1";
			// 
			// CommStripButton
			// 
			this.CommStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.CommStripButton.Image = ((System.Drawing.Image)(resources.GetObject("CommStripButton.Image")));
			this.CommStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.CommStripButton.Name = "CommStripButton";
			this.CommStripButton.ShowDropDownArrow = false;
			this.CommStripButton.Size = new System.Drawing.Size(20, 23);
			this.CommStripButton.Text = "toolStripDropDownButton1";
			this.CommStripButton.Click += new System.EventHandler(this.CommStripButton_Click);
			// 
			// pnlBootloader
			// 
			this.pnlBootloader.BackColor = System.Drawing.Color.White;
			this.pnlBootloader.Controls.Add(this.label5);
			this.pnlBootloader.Location = new System.Drawing.Point(0, 31);
			this.pnlBootloader.Name = "pnlBootloader";
			this.pnlBootloader.Size = new System.Drawing.Size(517, 31);
			this.pnlBootloader.TabIndex = 22;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Cursor = System.Windows.Forms.Cursors.Hand;
			this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label5.ForeColor = System.Drawing.Color.Blue;
			this.label5.Location = new System.Drawing.Point(46, 6);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(429, 20);
			this.label5.TabIndex = 1;
			this.label5.Text = "The board is in Bootloader mode. Click here to upgrade FW.";
			this.label5.Click += new System.EventHandler(this.label5_Click);
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
			this.pictureBox1.Location = new System.Drawing.Point(-323, 24);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(840, 60);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.pictureBox1.TabIndex = 48;
			this.pictureBox1.TabStop = false;
			// 
			// mnuMenu
			// 
			this.mnuMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.itemAbout});
			this.mnuMenu.Location = new System.Drawing.Point(0, 0);
			this.mnuMenu.Name = "mnuMenu";
			this.mnuMenu.Size = new System.Drawing.Size(517, 28);
			this.mnuMenu.TabIndex = 49;
			this.mnuMenu.Text = "menuStrip1";
			// 
			// itemAbout
			// 
			this.itemAbout.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.itemHelpAbout});
			this.itemAbout.Name = "itemAbout";
			this.itemAbout.Size = new System.Drawing.Size(53, 24);
			this.itemAbout.Text = "Help";
			// 
			// itemHelpAbout
			// 
			this.itemHelpAbout.Name = "itemHelpAbout";
			this.itemHelpAbout.Size = new System.Drawing.Size(119, 24);
			this.itemHelpAbout.Text = "About";
			this.itemHelpAbout.Click += new System.EventHandler(this.itemHelpAbout_Click);
			// 
			// Launcher
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(111)))), ((int)(((byte)(123)))));
			this.ClientSize = new System.Drawing.Size(517, 601);
			this.Controls.Add(this.pnlBootloader);
			this.Controls.Add(this.pictureBox1);
			this.Controls.Add(this.CommStrip);
			this.Controls.Add(this.mnuMenu);
			this.Controls.Add(this.groupBoxApplicationSelection);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MainMenuStrip = this.mnuMenu;
			this.MaximizeBox = false;
			this.Name = "Launcher";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "MMA845x Demos Launcher";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Apps_FormClosing);
			this.groupBoxApplicationSelection.ResumeLayout(false);
			this.groupBoxApplicationSelection.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
			this.CommStrip.ResumeLayout(false);
			this.CommStrip.PerformLayout();
			this.pnlBootloader.ResumeLayout(false);
			this.pnlBootloader.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.mnuMenu.ResumeLayout(false);
			this.mnuMenu.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion
	}
}