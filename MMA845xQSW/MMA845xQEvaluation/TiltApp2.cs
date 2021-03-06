﻿namespace MMA845xQEvaluation
{
    using Freescale.SASD.Communication;
    using GlobalSTB;
    using MMA845x_DEMO.Properties;
    using NationalInstruments.UI;
    using NationalInstruments.UI.WindowsForms;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Reflection;
    using System.Resources;
    using System.Text;
    using System.Threading;
    using System.Windows.Forms;

    public class TiltApp2 : Form
    {
        private string ArcminAngle;
        private int B;
        private Color Back;
        private int Bit8_2Complement;
        private int Bit8_MaxPositive;
        private int BitFull_2Complement;
        private int BitFull_MaxPositive;
        private Button btnAutoCal;
        private CheckBox chkAnalogLowNoise;
        private eCommMode CommMode = eCommMode.Closed;
        private StatusStrip CommStrip;
        private ToolStripDropDownButton CommStripButton;
        private IContainer components = null;
        private object ControllerEventsLock = new object();
        private AccelController ControllerObj;
        private string CurrentFW = "4003";
        private bool dataStream = false;
        private ComboBox ddlDataRate;
        private deviceID DeviceID;
        private Color Dial;
        private double DR_timestep;
        private BoardComm dv;
        private ToolStripMenuItem enableLogDataApplicationToolStripMenuItem;
        private const int fade_max = 0xff;
        private const int fade_min = 10;
        private bool FirstTimeLoad = true;
        private Color Fore;
        private const int Form_Max_Height = 720;
        private const int Form_Min_Height = 200;
        private int FullOrEight;
        private int FullScaleValue;
        private string FullValueAngle;
        private int G;
        private Gauge gaugeArcmin;
        private Gauge gaugeXY;
        private GroupBox gbDR;
        private GroupBox gbOM;
        private GroupBox groupBox1;
        private GroupBox groupBoxXY;
        private int I_panelAdvanced_height;
        private int I_panelDisplay_height;
        private Image ImageGreen;
        private Image ImageRed;
        private Image ImageYellow;
        private Label label1;
        private Label label173;
        private Label label2;
        private Label label3;
        private Label label35;
        private Label label4;
        private Label label5;
        private Label label70;
        private Label label71;
        private Label labelXY;
        private Label lbl2gSensitivity;
        private Label lbl4gSensitivity;
        private Label lbl8gSensitivity;
        private Label lblArcmin;
        private Label lblFullValue;
        private Label lbsleep;
        private Led ledSleep;
        private Led ledStandby;
        private Led ledWake;
        private bool LoadingFW = false;
        private ToolStripMenuItem logDataToolStripMenuItem1;
        private MenuStrip menuStrip1;
        private object objectLock = new object();
        private Panel p1;
        private Panel panelAdvanced;
        private Panel panelDisplay;
        private FlowLayoutPanel panelGeneral;
        private PictureBox pictureBox1;
        private PictureBox pictureBox2;
        private Color Pointer;
        private Panel pOverSampling;
        private int R;
        private const double range_max = 0.25;
        private const double range_min = 0.1;
        private RadioButton rdo2g;
        private RadioButton rdo4g;
        private RadioButton rdo8g;
        private RadioButton rdoActive;
        private RadioButton rdoOSHiResMode;
        private RadioButton rdoOSLNLPMode;
        private RadioButton rdoOSLPMode;
        private RadioButton rdoOSNormalMode;
        private RadioButton rdoStandby;
        private string[] RegisterNames = new string[] { 
            "STATUS", "OUT_XMSB", "OUT_XLSB", "OUT_YMSB", "OUT_YLSP", "OUT_ZMSB", "OUT_ZLSB", "RESERVED ", "RESERVED ", "FSETUP", "TRIGCFG", "SYSMOD", "INTSOURCE", "WHOAMI", "XYZDATACFG", "HPFILTER", 
            "PLSTATUS", "PLCFG", "PLCOUNT", "PLBFZCOMP", "PLPLTHSREG", "FFMTCFG", "FFMTSRC", "FFMTTHS", "FFMTCOUNT", "RESERVED", "RESERVED", "RESERVED", "RESERVED", "TRANSCFG", "TRANSSRC", "TRANSTHS", 
            "TRANSCOUNT", "PULSECFG", "PULSESRC", "PULSETHSX", "PULSETHSY", "PULSETHSZ", "PULSETMLT", "PULSELTCY", "PULSEWIND", "ASLPCOUNT", "CTRLREG1", "CTRLREG2", "CTRLREG3", "CTRLREG4", "CTRLREG5", "OFFSET_X", 
            "OFFSET_Y", "OFFSET_Z"
         };
        private Color Spindle;
        private bool TimerEnabled = false;
        private System.Windows.Forms.Timer tmrTiltTimer;
        private ToolStripStatusLabel toolStripStatusLabel;
        private int WakeOSMode;
        private int windowHeight = 0;
        private string XYAngle;

        public TiltApp2(object controllerObj)
        {
            InitializeComponent();
            Styles.FormatForm(this);
            Styles.FormatInterruptPanel(panelAdvanced);
            ControllerObj = (AccelController) controllerObj;
            ControllerObj.ControllerEvents += new AccelController.ControllerEventHandler(ControllerObj_ControllerEvents);
            DeviceID = ControllerObj.DeviceID;
            if (DeviceID == deviceID.MMA8661Q)
            {
                FullOrEight = 0;
            }
            else
            {
                FullOrEight = 1;
            }
            menuStrip1.Enabled = false;
            Bit8_2Complement = 0x100;
            Bit8_MaxPositive = 0x7f;
            if (DeviceID == deviceID.MMA8451Q)
            {
                BitFull_2Complement = 0x4000;
                BitFull_MaxPositive = 0x2000;
                lbl2gSensitivity.Text = "4096 counts/g 14b";
                lbl4gSensitivity.Text = "2048 counts/g 14b";
                lbl8gSensitivity.Text = "1024 counts/g 14b";
            }
            else if (DeviceID == deviceID.MMA8452Q)
            {
                BitFull_2Complement = 0x1000;
                BitFull_MaxPositive = 0x800;
                lbl2gSensitivity.Text = "1024 counts/g 12b";
                lbl4gSensitivity.Text = " 512 counts/g 12b";
                lbl8gSensitivity.Text = " 256 counts/g 12b";
            }
            else if (DeviceID == deviceID.MMA8453Q)
            {
                BitFull_2Complement = 0x400;
                BitFull_MaxPositive = 0x200;
                lbl2gSensitivity.Text = "256 counts/g 10b";
                lbl4gSensitivity.Text = "128 counts/g 10b";
                lbl8gSensitivity.Text = " 64 counts/g 10b";
            }
            else if (DeviceID == deviceID.MMA8652FC)
            {
                BitFull_2Complement = 0x1000;
                BitFull_MaxPositive = 0x800;
                chkAnalogLowNoise.Visible = false;
                lbl2gSensitivity.Text = "1024 counts/g 12b";
                lbl4gSensitivity.Text = " 512 counts/g 12b";
                lbl8gSensitivity.Text = " 256 counts/g 12b";
            }
            else if (DeviceID == deviceID.MMA8653FC)
            {
                BitFull_2Complement = 0x400;
                BitFull_MaxPositive = 0x200;
                chkAnalogLowNoise.Visible = false;
                lbl2gSensitivity.Text = "256 counts/g 10b";
                lbl4gSensitivity.Text = "128 counts/g 10b";
                lbl8gSensitivity.Text = " 64 counts/g 10b";
            }
            I_panelDisplay_height = panelDisplay.Height;
            I_panelAdvanced_height = panelAdvanced.Height;
            R = BackColor.R;
            G = BackColor.G;
            B = BackColor.B;
            Fore = gaugeXY.ForeColor;
            Pointer = gaugeXY.PointerColor;
            Back = gaugeXY.BackColor;
            Dial = gaugeXY.DialColor;
            Spindle = gaugeXY.SpindleColor;
            InitializeStatusBar();
            tmrTiltTimer.Enabled = true;
            tmrTiltTimer.Start();
        }

        private void btnAutoCal_Click_1(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            int[] datapassed = new int[] { (int)DeviceID };
            ControllerObj.AutoCalFlag = true;
            ControllerReqPacket reqName = new ControllerReqPacket();
            ControllerObj.CreateNewTaskFromGUI(reqName, 0, 0x52, datapassed);
            Thread.Sleep(0x1b58);
            Cursor = Cursors.Default;
            rdo8g.Checked = true;
            ddlDataRate.SelectedIndex = 7;
        }

        private void chkAnalogLowNoise_CheckedChanged(object sender, EventArgs e)
        {
            int[] datapassed = new int[1];
            int num = chkAnalogLowNoise.Checked ? 0xff : 0;
            datapassed[0] = num;
            ControllerObj.SetEnableAnalogLowNoiseFlag = true;
            ControllerReqPacket reqName = new ControllerReqPacket();
            ControllerObj.CreateNewTaskFromGUI(reqName, 0, 0x57, datapassed);
        }

        private void CommStripButton_Click(object sender, EventArgs e)
        {
            switch (CommMode)
            {
                case eCommMode.Closed:
                    dv.FindHw(" I0");
                    break;

                case eCommMode.Running:
                    CommStripButton.Text = "Find HW";
                    dv.Close();
                    break;

                case eCommMode.Bootloader:
                    CommStripButton.Text = "Find HW";
                    dv.Close();
                    break;
            }
        }

        private void ControllerObj_ControllerEvents(ControllerEventType evt, object o)
        {
            lock (ControllerEventsLock)
            {
                XYZCounts counts;
                int num = 0;
                int num2 = 0;
                int num3 = 0;
                XYZGees gees = new XYZGees();
                switch (((int) evt))
                {
                    case 1:
                        counts = (XYZCounts) o;
                        num = (counts.XAxis > Bit8_MaxPositive) ? (counts.XAxis - Bit8_2Complement) : counts.XAxis;
                        num2 = (counts.YAxis > Bit8_MaxPositive) ? (counts.YAxis - Bit8_2Complement) : counts.YAxis;
                        num3 = (counts.ZAxis > Bit8_MaxPositive) ? (counts.ZAxis - Bit8_2Complement) : counts.ZAxis;
                        gees.Mod = Math.Sqrt((double) (((num * num) + (num2 * num2)) + (num3 * num3)));
                        gees.XAxis = ((double) num) / gees.Mod;
                        gees.YAxis = ((double) num2) / gees.Mod;
                        gees.ZAxis = ((double) num3) / gees.Mod;
                        UpdateAngles(gees);
                        break;

                    case 2:
                        counts = (XYZCounts) o;
                        num = (counts.XAxis > BitFull_MaxPositive) ? (counts.XAxis - BitFull_2Complement) : counts.XAxis;
                        num2 = (counts.YAxis > BitFull_MaxPositive) ? (counts.YAxis - BitFull_2Complement) : counts.YAxis;
                        num3 = (counts.ZAxis > BitFull_MaxPositive) ? (counts.ZAxis - BitFull_2Complement) : counts.ZAxis;
                        gees.Mod = Math.Sqrt((double) (((num * num) + (num2 * num2)) + (num3 * num3)));
                        gees.XAxis = ((double) num) / gees.Mod;
                        gees.YAxis = ((double) num2) / gees.Mod;
                        gees.ZAxis = ((double) num3) / gees.Mod;
                        UpdateAngles(gees);
                        break;
                }
            }
        }

        private void ddlDataRate_SelectedIndexChanged(object sender, EventArgs e)
        {
            int[] datapassed = new int[] { ddlDataRate.SelectedIndex };
            ControllerObj.SetDataRateFlag = true;
            ControllerReqPacket reqName = new ControllerReqPacket();
            ControllerObj.CreateNewTaskFromGUI(reqName, 0, 5, datapassed);
            switch (ddlDataRate.SelectedIndex)
            {
                case 0:
                    DR_timestep = 1.25;
                    break;

                case 1:
                    DR_timestep = 2.5;
                    break;

                case 2:
                    switch (WakeOSMode)
                    {
                        case 2:
                            DR_timestep = 2.5;
                            return;

                        case 3:
                            DR_timestep = 5.0;
                            return;
                    }
                    DR_timestep = 5.0;
                    break;

                case 3:
                    switch (WakeOSMode)
                    {
                        case 2:
                            DR_timestep = 2.5;
                            return;

                        case 3:
                            DR_timestep = 10.0;
                            return;
                    }
                    DR_timestep = 10.0;
                    break;

                case 4:
                    switch (WakeOSMode)
                    {
                        case 2:
                            DR_timestep = 2.5;
                            return;

                        case 3:
                            DR_timestep = 20.0;
                            return;
                    }
                    DR_timestep = 20.0;
                    break;

                case 5:
                    DR_timestep = 80.0;
                    switch (WakeOSMode)
                    {
                        case 0:
                            DR_timestep = 20.0;
                            return;

                        case 1:
                            DR_timestep = 80.0;
                            return;

                        case 2:
                            DR_timestep = 2.5;
                            return;

                        case 3:
                            DR_timestep = 80.0;
                            return;
                    }
                    break;

                case 6:
                    switch (WakeOSMode)
                    {
                        case 0:
                            DR_timestep = 20.0;
                            return;

                        case 1:
                            DR_timestep = 80.0;
                            return;

                        case 2:
                            DR_timestep = 2.5;
                            return;

                        case 3:
                            DR_timestep = 160.0;
                            return;
                    }
                    break;

                case 7:
                    switch (WakeOSMode)
                    {
                        case 0:
                            DR_timestep = 20.0;
                            return;

                        case 1:
                            DR_timestep = 80.0;
                            return;

                        case 2:
                            DR_timestep = 2.5;
                            return;

                        case 3:
                            DR_timestep = 160.0;
                            return;
                    }
                    break;
            }
        }

        private void DecodeGUIPackets()
        {
            GUIUpdatePacket packet = new GUIUpdatePacket();
            ControllerObj.Dequeue_GuiPacket(ref packet);
            if (packet != null)
            {
                if (packet.TaskID == 0x11)
                {
                    byte num = (byte) packet.PayLoad.Dequeue();
                }
                if (packet.TaskID == 0x10)
                {
                    byte[] buffer = (byte[]) packet.PayLoad.Dequeue();
                    StringBuilder builder = new StringBuilder();
                    for (int i = 0; i < buffer.Length; i += 3)
                    {
                        builder.Append(string.Format("{0:X2}", i) + " " + RegisterNames[i] + "\t" + string.Format("{0:X2}", buffer[i]) + "\t\t");
                        if ((i + 1) < buffer.Length)
                        {
                            builder.Append(string.Format("{0:X2}", i + 1) + " " + RegisterNames[i + 1] + "\t" + string.Format("{0:X2}", buffer[i + 1]) + "\t\t");
                        }
                        if ((i + 2) < buffer.Length)
                        {
                            builder.Append(string.Format("{0:X2}", i + 2) + " " + RegisterNames[i + 2] + "\t" + string.Format("{0:X2}", buffer[i + 2]) + "\r\n");
                        }
                    }
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void enableLogDataApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataStream)
            {
                dataStream = false;
                tmrTiltTimer.Stop();
                groupBoxXY.Enabled = false;
            }
        }

        public void EndDemo()
        {
            lock (ControllerEventsLock)
            {
                tmrTiltTimer.Stop();
                tmrTiltTimer.Enabled = false;
                ControllerObj.DisableData();
                ControllerObj.ResetDevice();
                ControllerObj.ControllerEvents -= new AccelController.ControllerEventHandler(ControllerObj_ControllerEvents);
                base.Close();
            }
        }

        private int FadeCalculus(double A, double B)
        {
            double x = Math.Abs(A);
            double num3 = Math.Abs(B);
            double num4 = 0.0;
            x = (x < 0.1) ? 0.1 : ((x > 0.25) ? 0.25 : x);
            num3 = (num3 < 0.1) ? 0.1 : ((num3 > 0.25) ? 0.25 : num3);
            x = (x - 0.1) / 0.15;
            num3 = (num3 - 0.1) / 0.15;
            num4 = Math.Sqrt(Math.Pow(x, 2.0) + Math.Pow(num3, 2.0)) / Math.Sqrt(2.0);
            num4 = (num4 * 245.0) + 10.0;
            return (int) num4;
        }

        private void FadeGauges(double X, double Y, double Z)
        {
            lock (gaugeXY)
            {
            }
        }

        private void InitDevice()
        {
            ControllerObj.StartDevice();
            int[] datapassed = new int[] { FullScaleValue };
            ControllerObj.BootFlag = true;
            ControllerReqPacket reqName = new ControllerReqPacket();
            ControllerObj.CreateNewTaskFromGUI(reqName, 0, 0x51, datapassed);
            Thread.Sleep(10);
            rdoStandby.Checked = true;
            if (DeviceID == deviceID.MMA8661Q)
            {
                int[] numArray2 = new int[] { 0xff };
                ControllerObj.SetFREADFlag = true;
                ControllerReqPacket packet2 = new ControllerReqPacket();
                ControllerObj.CreateNewTaskFromGUI(packet2, 0, 0x58, numArray2);
            }
            rdo2g.Checked = true;
            rdo2g_CheckedChanged(this, null);
            ddlDataRate.SelectedIndex = 6;
            rdoOSHiResMode.Checked = true;
            rdoOSHiResMode_CheckedChanged(this, null);
            chkAnalogLowNoise.Checked = true;
            chkAnalogLowNoise_CheckedChanged(this, null);
            FullScaleValue = 0;
            WakeOSMode = 2;
            DR_timestep = 160.0;
            rdoActive.Checked = true;
            rdoActive_CheckedChanged(this, null);
        }

        private void InitializeComponent()
        {
            components = new Container();
            ComponentResourceManager resources = new ComponentResourceManager(typeof(TiltApp2));
            ScaleCustomDivision division = new ScaleCustomDivision();
            ScaleRangeFill fill = new ScaleRangeFill();
            labelXY = new Label();
            tmrTiltTimer = new System.Windows.Forms.Timer(components);
            panelGeneral = new FlowLayoutPanel();
            panelDisplay = new Panel();
            groupBoxXY = new GroupBox();
            pictureBox2 = new PictureBox();
            label5 = new Label();
            label3 = new Label();
            label4 = new Label();
            lblFullValue = new Label();
            label2 = new Label();
            lblArcmin = new Label();
            label1 = new Label();
            gaugeArcmin = new Gauge();
            gaugeXY = new Gauge();
            panelAdvanced = new Panel();
            gbOM = new GroupBox();
            btnAutoCal = new Button();
            chkAnalogLowNoise = new CheckBox();
            pOverSampling = new Panel();
            label70 = new Label();
            rdoOSHiResMode = new RadioButton();
            rdoOSLPMode = new RadioButton();
            rdoOSLNLPMode = new RadioButton();
            rdoOSNormalMode = new RadioButton();
            p1 = new Panel();
            ddlDataRate = new ComboBox();
            label35 = new Label();
            gbDR = new GroupBox();
            lbl4gSensitivity = new Label();
            rdo2g = new RadioButton();
            rdo8g = new RadioButton();
            rdo4g = new RadioButton();
            lbl2gSensitivity = new Label();
            lbl8gSensitivity = new Label();
            rdoStandby = new RadioButton();
            rdoActive = new RadioButton();
            label71 = new Label();
            label173 = new Label();
            lbsleep = new Label();
            ledStandby = new Led();
            ledSleep = new Led();
            ledWake = new Led();
            menuStrip1 = new MenuStrip();
            logDataToolStripMenuItem1 = new ToolStripMenuItem();
            enableLogDataApplicationToolStripMenuItem = new ToolStripMenuItem();
            pictureBox1 = new PictureBox();
            CommStrip = new StatusStrip();
            CommStripButton = new ToolStripDropDownButton();
            toolStripStatusLabel = new ToolStripStatusLabel();
            groupBox1 = new GroupBox();
            panelGeneral.SuspendLayout();
            panelDisplay.SuspendLayout();
            groupBoxXY.SuspendLayout();
            ((ISupportInitialize) pictureBox2).BeginInit();
            ((ISupportInitialize) gaugeArcmin).BeginInit();
            ((ISupportInitialize) gaugeXY).BeginInit();
            panelAdvanced.SuspendLayout();
            gbOM.SuspendLayout();
            pOverSampling.SuspendLayout();
            p1.SuspendLayout();
            gbDR.SuspendLayout();
            ((ISupportInitialize) ledStandby).BeginInit();
            ((ISupportInitialize) ledSleep).BeginInit();
            ((ISupportInitialize) ledWake).BeginInit();
            menuStrip1.SuspendLayout();
            ((ISupportInitialize) pictureBox1).BeginInit();
            CommStrip.SuspendLayout();
            groupBox1.SuspendLayout();
            base.SuspendLayout();
            labelXY.BackColor = Color.Silver;
            labelXY.BorderStyle = BorderStyle.Fixed3D;
            labelXY.Font = new Font("Arial Rounded MT Bold", 15.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            labelXY.ForeColor = Color.Black;
            labelXY.Location = new Point(270, 0x117);
            labelXY.Name = "labelXY";
            labelXY.Size = new Size(0x31, 0x21);
            labelXY.TabIndex = 50;
            tmrTiltTimer.Interval = 40;
            tmrTiltTimer.Tick += new EventHandler(tmrTiltTimer_Tick);
            panelGeneral.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            panelGeneral.AutoScroll = true;
            panelGeneral.Controls.Add(panelDisplay);
            panelGeneral.Controls.Add(panelAdvanced);
            panelGeneral.Location = new Point(1, 0x3a);
            panelGeneral.Name = "panelGeneral";
            panelGeneral.Size = new Size(0x3c5, 0x224);
            panelGeneral.TabIndex = 0x47;
            panelDisplay.BorderStyle = BorderStyle.FixedSingle;
            panelDisplay.Controls.Add(groupBoxXY);
            panelDisplay.Location = new Point(3, 3);
            panelDisplay.Name = "panelDisplay";
            panelDisplay.Size = new Size(0x21a, 0x21e);
            panelDisplay.TabIndex = 0x49;
            groupBoxXY.Controls.Add(pictureBox2);
            groupBoxXY.Controls.Add(label5);
            groupBoxXY.Controls.Add(label3);
            groupBoxXY.Controls.Add(label4);
            groupBoxXY.Controls.Add(lblFullValue);
            groupBoxXY.Controls.Add(label2);
            groupBoxXY.Controls.Add(lblArcmin);
            groupBoxXY.Controls.Add(label1);
            groupBoxXY.Controls.Add(gaugeArcmin);
            groupBoxXY.Controls.Add(gaugeXY);
            groupBoxXY.Controls.Add(labelXY);
            groupBoxXY.Font = new Font("Arial Rounded MT Bold", 11.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            groupBoxXY.ForeColor = Color.Yellow;
            groupBoxXY.Location = new Point(11, -1);
            groupBoxXY.Name = "groupBoxXY";
            groupBoxXY.Size = new Size(0x209, 0x21a);
            groupBoxXY.TabIndex = 60;
            groupBoxXY.TabStop = false;
            pictureBox2.Image = (Image) resources.GetObject("pictureBox2.Image");
            pictureBox2.Location = new Point(0x67, 0x13d);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(0x12e, 0xd9);
            pictureBox2.SizeMode = PictureBoxSizeMode.AutoSize;
            pictureBox2.TabIndex = 0x40;
            pictureBox2.TabStop = false;
            label5.AutoSize = true;
            label5.Font = new Font("Arial Rounded MT Bold", 20.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            label5.ForeColor = Color.Yellow;
            label5.Location = new Point(0xe4, 12);
            label5.Name = "label5";
            label5.Size = new Size(70, 0x20);
            label5.TabIndex = 0x3f;
            label5.Text = "Fine";
            label3.AutoSize = true;
            label3.Font = new Font("Arial Rounded MT Bold", 20.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.ForeColor = Color.Yellow;
            label3.Location = new Point(6, 12);
            label3.Name = "label3";
            label3.Size = new Size(110, 0x20);
            label3.TabIndex = 0x3e;
            label3.Text = "Coarse";
            label4.AutoSize = true;
            label4.Font = new Font("Arial Rounded MT Bold", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            label4.ForeColor = Color.Black;
            label4.Location = new Point(0x155, 0x11f);
            label4.Name = "label4";
            label4.Size = new Size(80, 15);
            label4.TabIndex = 0x3d;
            label4.Text = "Total Angle";
            lblFullValue.BackColor = Color.Silver;
            lblFullValue.BorderStyle = BorderStyle.Fixed3D;
            lblFullValue.Font = new Font("Arial Rounded MT Bold", 15.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblFullValue.ForeColor = Color.Black;
            lblFullValue.Location = new Point(0x1ab, 0x117);
            lblFullValue.Name = "lblFullValue";
            lblFullValue.Size = new Size(0x35, 0x21);
            lblFullValue.TabIndex = 60;
            label2.AutoSize = true;
            label2.Font = new Font("Arial Rounded MT Bold", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.ForeColor = Color.Black;
            label2.Location = new Point(11, 0x11f);
            label2.Name = "label2";
            label2.Size = new Size(0x52, 15);
            label2.TabIndex = 0x3b;
            label2.Text = "Arcminutes";
            lblArcmin.BackColor = Color.Silver;
            lblArcmin.BorderStyle = BorderStyle.Fixed3D;
            lblArcmin.Font = new Font("Arial Rounded MT Bold", 15.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblArcmin.ForeColor = Color.Black;
            lblArcmin.Location = new Point(0x63, 0x117);
            lblArcmin.Name = "lblArcmin";
            lblArcmin.Size = new Size(0x2f, 0x21);
            lblArcmin.TabIndex = 0x3a;
            label1.AutoSize = true;
            label1.Font = new Font("Arial Rounded MT Bold", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.Black;
            label1.Location = new Point(0xa9, 0x11f);
            label1.Name = "label1";
            label1.Size = new Size(0x61, 15);
            label1.TabIndex = 0x39;
            label1.Text = "XY- Axis Angle";
            gaugeArcmin.CustomDivisions.AddRange(new ScaleCustomDivision[] { division });
            gaugeArcmin.DialColor = Color.Black;
            gaugeArcmin.ForeColor = Color.CadetBlue;
            gaugeArcmin.Location = new Point(0x11f, 0x2e);
            gaugeArcmin.MajorDivisions.Base = 1.0;
            gaugeArcmin.MajorDivisions.Interval = 90.0;
            gaugeArcmin.MajorDivisions.LabelForeColor = Color.White;
            gaugeArcmin.MajorDivisions.TickColor = Color.White;
            gaugeArcmin.MinorDivisions.Interval = 36.0;
            gaugeArcmin.MinorDivisions.LineWidth = 3f;
            gaugeArcmin.MinorDivisions.TickColor = Color.White;
            gaugeArcmin.Name = "gaugeArcmin";
            gaugeArcmin.PointerColor = Color.Red;
            gaugeArcmin.Range = new Range(0.0, 60.0);
            fill.Range = new Range(0.0, 360.0);
            fill.Style = ScaleRangeFillStyle.CreateGradientStyle(Color.Yellow, Color.Red, 0.69387755102040816);
            fill.Width = 2f;
            gaugeArcmin.RangeFills.AddRange(new ScaleRangeFill[] { fill });
            gaugeArcmin.ScaleArc = new Arc(90f, -360f);
            gaugeArcmin.ScaleBaseLineVisible = true;
            gaugeArcmin.Size = new Size(0xe0, 0xea);
            gaugeArcmin.SpindleColor = Color.Yellow;
            gaugeArcmin.TabIndex = 0x38;
            gaugeXY.AutoDivisionSpacing = false;
            gaugeXY.Font = new Font("Calibri", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
            gaugeXY.ForeColor = Color.Black;
            gaugeXY.Location = new Point(12, 0x2b);
            gaugeXY.MajorDivisions.Interval = 20.0;
            gaugeXY.MajorDivisions.LineWidth = 2f;
            gaugeXY.MajorDivisions.TickLength = 7f;
            gaugeXY.MinorDivisions.TickColor = Color.CornflowerBlue;
            gaugeXY.Name = "gaugeXY";
            gaugeXY.Range = new Range(0.0, 360.0);
            gaugeXY.ScaleArc = new Arc(0f, 360f);
            gaugeXY.Size = new Size(0xe0, 0xea);
            gaugeXY.SpindleColor = Color.CornflowerBlue;
            gaugeXY.TabIndex = 0x38;
            panelAdvanced.BorderStyle = BorderStyle.Fixed3D;
            panelAdvanced.Controls.Add(groupBox1);
            panelAdvanced.Controls.Add(gbOM);
            panelAdvanced.Controls.Add(gbDR);
            panelAdvanced.Controls.Add(label71);
            panelAdvanced.Controls.Add(label173);
            panelAdvanced.Controls.Add(lbsleep);
            panelAdvanced.Controls.Add(ledStandby);
            panelAdvanced.Controls.Add(ledSleep);
            panelAdvanced.Controls.Add(ledWake);
            panelAdvanced.Location = new Point(0x223, 3);
            panelAdvanced.Name = "panelAdvanced";
            panelAdvanced.Size = new Size(380, 0x21e);
            panelAdvanced.TabIndex = 0x4c;
            gbOM.BackColor = Color.LightSlateGray;
            gbOM.Controls.Add(btnAutoCal);
            gbOM.Controls.Add(chkAnalogLowNoise);
            gbOM.Controls.Add(pOverSampling);
            gbOM.Controls.Add(p1);
            gbOM.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            gbOM.ForeColor = Color.White;
            gbOM.Location = new Point(0x17, 0xe9);
            gbOM.Name = "gbOM";
            gbOM.Size = new Size(0x156, 0x11d);
            gbOM.TabIndex = 0xc3;
            gbOM.TabStop = false;
            gbOM.Text = "Operation Mode";
            btnAutoCal.BackColor = Color.LightSlateGray;
            btnAutoCal.FlatAppearance.BorderColor = Color.Fuchsia;
            btnAutoCal.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnAutoCal.ForeColor = Color.White;
            btnAutoCal.Location = new Point(7, 0x53);
            btnAutoCal.Name = "btnAutoCal";
            btnAutoCal.Size = new Size(0x60, 0x2c);
            btnAutoCal.TabIndex = 0xc5;
            btnAutoCal.Text = "Auto Calibrate";
            btnAutoCal.UseVisualStyleBackColor = false;
            btnAutoCal.Click += new EventHandler(btnAutoCal_Click_1);
            chkAnalogLowNoise.AutoSize = true;
            chkAnalogLowNoise.Checked = true;
            chkAnalogLowNoise.CheckState = CheckState.Checked;
            chkAnalogLowNoise.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            chkAnalogLowNoise.ForeColor = Color.White;
            chkAnalogLowNoise.Location = new Point(5, 0x18);
            chkAnalogLowNoise.Name = "chkAnalogLowNoise";
            chkAnalogLowNoise.Size = new Size(200, 0x11);
            chkAnalogLowNoise.TabIndex = 0xde;
            chkAnalogLowNoise.Text = "Enable Low Noise (Up to 5.5g)";
            chkAnalogLowNoise.UseVisualStyleBackColor = true;
            chkAnalogLowNoise.CheckedChanged += new EventHandler(chkAnalogLowNoise_CheckedChanged);
            pOverSampling.Controls.Add(label70);
            pOverSampling.Controls.Add(rdoOSHiResMode);
            pOverSampling.Controls.Add(rdoOSLPMode);
            pOverSampling.Controls.Add(rdoOSLNLPMode);
            pOverSampling.Controls.Add(rdoOSNormalMode);
            pOverSampling.Location = new Point(7, 0x88);
            pOverSampling.Name = "pOverSampling";
            pOverSampling.Size = new Size(0x149, 0x8d);
            pOverSampling.TabIndex = 0xdd;
            label70.AutoSize = true;
            label70.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label70.Location = new Point(4, 5);
            label70.Name = "label70";
            label70.Size = new Size(220, 0x10);
            label70.TabIndex = 0xde;
            label70.Text = "Oversampling Options for Data";
            rdoOSHiResMode.AutoSize = true;
            rdoOSHiResMode.Checked = true;
            rdoOSHiResMode.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
            rdoOSHiResMode.Location = new Point(5, 0x38);
            rdoOSHiResMode.Name = "rdoOSHiResMode";
            rdoOSHiResMode.Size = new Size(0x6c, 0x13);
            rdoOSHiResMode.TabIndex = 0xe0;
            rdoOSHiResMode.TabStop = true;
            rdoOSHiResMode.Text = "Hi Res Mode";
            rdoOSHiResMode.UseVisualStyleBackColor = true;
            rdoOSHiResMode.CheckedChanged += new EventHandler(rdoOSHiResMode_CheckedChanged);
            rdoOSLPMode.AutoSize = true;
            rdoOSLPMode.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
            rdoOSLPMode.Location = new Point(5, 0x54);
            rdoOSLPMode.Name = "rdoOSLPMode";
            rdoOSLPMode.Size = new Size(0x87, 0x13);
            rdoOSLPMode.TabIndex = 0xdf;
            rdoOSLPMode.Text = "Low Power Mode";
            rdoOSLPMode.UseVisualStyleBackColor = true;
            rdoOSLPMode.CheckedChanged += new EventHandler(rdoOSLPMode_CheckedChanged);
            rdoOSLNLPMode.AutoSize = true;
            rdoOSLNLPMode.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
            rdoOSLNLPMode.Location = new Point(5, 0x70);
            rdoOSLNLPMode.Name = "rdoOSLNLPMode";
            rdoOSLNLPMode.Size = new Size(0xa6, 0x13);
            rdoOSLNLPMode.TabIndex = 0xdd;
            rdoOSLNLPMode.Text = "Low Noise Low Power";
            rdoOSLNLPMode.UseVisualStyleBackColor = true;
            rdoOSLNLPMode.CheckedChanged += new EventHandler(rdoOSLNLPMode_CheckedChanged);
            rdoOSNormalMode.AutoSize = true;
            rdoOSNormalMode.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
            rdoOSNormalMode.Location = new Point(5, 0x1c);
            rdoOSNormalMode.Name = "rdoOSNormalMode";
            rdoOSNormalMode.Size = new Size(0x70, 0x13);
            rdoOSNormalMode.TabIndex = 220;
            rdoOSNormalMode.Text = "Normal Mode";
            rdoOSNormalMode.UseVisualStyleBackColor = true;
            rdoOSNormalMode.CheckedChanged += new EventHandler(rdoOSNormalMode_CheckedChanged);
            p1.BackColor = Color.LightSlateGray;
            p1.Controls.Add(ddlDataRate);
            p1.Controls.Add(label35);
            p1.Location = new Point(7, 0x2f);
            p1.Name = "p1";
            p1.Size = new Size(0xe0, 0x21);
            p1.TabIndex = 0x91;
            ddlDataRate.DisplayMember = "(none)";
            ddlDataRate.DropDownStyle = ComboBoxStyle.DropDownList;
            ddlDataRate.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            ddlDataRate.FormattingEnabled = true;
            ddlDataRate.Items.AddRange(new object[] { "800Hz", "400 Hz", "200 Hz", "100 Hz", "50 Hz", "12.5 Hz", "6.25 Hz", "1.563 Hz" });
            ddlDataRate.Location = new Point(0x77, 6);
            ddlDataRate.Name = "ddlDataRate";
            ddlDataRate.Size = new Size(0x4c, 0x15);
            ddlDataRate.TabIndex = 0x7d;
            ddlDataRate.SelectedIndexChanged += new EventHandler(ddlDataRate_SelectedIndexChanged);
            label35.AutoSize = true;
            label35.BackColor = Color.LightSlateGray;
            label35.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label35.Location = new Point(4, 10);
            label35.Name = "label35";
            label35.Size = new Size(0x62, 0x10);
            label35.TabIndex = 0x7e;
            label35.Text = "Sample Rate";
            gbDR.BackColor = Color.LightSlateGray;
            gbDR.Controls.Add(lbl4gSensitivity);
            gbDR.Controls.Add(rdo2g);
            gbDR.Controls.Add(rdo8g);
            gbDR.Controls.Add(rdo4g);
            gbDR.Controls.Add(lbl2gSensitivity);
            gbDR.Controls.Add(lbl8gSensitivity);
            gbDR.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            gbDR.ForeColor = Color.White;
            gbDR.Location = new Point(0x17, 0x6d);
            gbDR.Name = "gbDR";
            gbDR.Size = new Size(0x155, 0x71);
            gbDR.TabIndex = 0xc4;
            gbDR.TabStop = false;
            gbDR.Text = "Dynamic Range";
            lbl4gSensitivity.AutoSize = true;
            lbl4gSensitivity.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbl4gSensitivity.ForeColor = Color.White;
            lbl4gSensitivity.Location = new Point(0x3e, 0x36);
            lbl4gSensitivity.Name = "lbl4gSensitivity";
            lbl4gSensitivity.Size = new Size(0x84, 0x10);
            lbl4gSensitivity.TabIndex = 0x77;
            lbl4gSensitivity.Text = "2048 counts/g 14b";
            rdo2g.AutoSize = true;
            rdo2g.Checked = true;
            rdo2g.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            rdo2g.ForeColor = Color.White;
            rdo2g.Location = new Point(0x13, 0x19);
            rdo2g.Name = "rdo2g";
            rdo2g.Size = new Size(0x2b, 20);
            rdo2g.TabIndex = 0x73;
            rdo2g.TabStop = true;
            rdo2g.Text = "2g";
            rdo2g.UseVisualStyleBackColor = true;
            rdo2g.CheckedChanged += new EventHandler(rdo2g_CheckedChanged);
            rdo8g.AutoSize = true;
            rdo8g.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            rdo8g.ForeColor = Color.White;
            rdo8g.Location = new Point(0x13, 0x4d);
            rdo8g.Name = "rdo8g";
            rdo8g.Size = new Size(0x2b, 20);
            rdo8g.TabIndex = 0x74;
            rdo8g.Text = "8g";
            rdo8g.UseVisualStyleBackColor = true;
            rdo8g.CheckedChanged += new EventHandler(rdo8g_CheckedChanged);
            rdo4g.AutoSize = true;
            rdo4g.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            rdo4g.ForeColor = Color.White;
            rdo4g.Location = new Point(0x13, 0x33);
            rdo4g.Name = "rdo4g";
            rdo4g.Size = new Size(0x2b, 20);
            rdo4g.TabIndex = 0x75;
            rdo4g.Text = "4g";
            rdo4g.UseVisualStyleBackColor = true;
            rdo4g.CheckedChanged += new EventHandler(rdo4g_CheckedChanged);
            lbl2gSensitivity.AutoSize = true;
            lbl2gSensitivity.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbl2gSensitivity.ForeColor = Color.White;
            lbl2gSensitivity.Location = new Point(0x3f, 0x1b);
            lbl2gSensitivity.Name = "lbl2gSensitivity";
            lbl2gSensitivity.Size = new Size(0x84, 0x10);
            lbl2gSensitivity.TabIndex = 0x76;
            lbl2gSensitivity.Text = "4096 counts/g 14b";
            lbl8gSensitivity.AutoSize = true;
            lbl8gSensitivity.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbl8gSensitivity.ForeColor = Color.White;
            lbl8gSensitivity.Location = new Point(0x3e, 0x4f);
            lbl8gSensitivity.Name = "lbl8gSensitivity";
            lbl8gSensitivity.Size = new Size(0x84, 0x10);
            lbl8gSensitivity.TabIndex = 120;
            lbl8gSensitivity.Text = "1024 counts/g 14b";
            rdoStandby.AutoSize = true;
            rdoStandby.Checked = true;
            rdoStandby.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            rdoStandby.Location = new Point(11, 0x15);
            rdoStandby.Name = "rdoStandby";
            rdoStandby.Size = new Size(0x57, 20);
            rdoStandby.TabIndex = 0x70;
            rdoStandby.TabStop = true;
            rdoStandby.Text = "Standby ";
            rdoStandby.UseVisualStyleBackColor = true;
            rdoStandby.CheckedChanged += new EventHandler(rdoStandby_CheckedChanged);
            rdoActive.AutoSize = true;
            rdoActive.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            rdoActive.Location = new Point(0x68, 0x15);
            rdoActive.Name = "rdoActive";
            rdoActive.Size = new Size(0x45, 20);
            rdoActive.TabIndex = 0x71;
            rdoActive.Text = "Active";
            rdoActive.UseVisualStyleBackColor = true;
            rdoActive.CheckedChanged += new EventHandler(rdoActive_CheckedChanged);
            label71.AutoSize = true;
            label71.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label71.Location = new Point(0xeb, 0x33);
            label71.Name = "label71";
            label71.Size = new Size(0x5b, 0x10);
            label71.TabIndex = 0xc0;
            label71.Text = "Wake Mode";
            label173.AutoSize = true;
            label173.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label173.Location = new Point(0xda, 0x18);
            label173.Name = "label173";
            label173.Size = new Size(0x6c, 0x10);
            label173.TabIndex = 0xc2;
            label173.Text = "Standby Mode";
            lbsleep.AutoSize = true;
            lbsleep.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbsleep.Location = new Point(0xea, 0x4f);
            lbsleep.Name = "lbsleep";
            lbsleep.Size = new Size(0x5c, 0x10);
            lbsleep.TabIndex = 0xb9;
            lbsleep.Text = "Sleep Mode";
            ledStandby.LedStyle = LedStyle.Round3D;
            ledStandby.Location = new Point(0x148, 0x11);
            ledStandby.Name = "ledStandby";
            ledStandby.OffColor = Color.Red;
            ledStandby.Size = new Size(30, 0x1f);
            ledStandby.TabIndex = 0xc1;
            ledStandby.Value = true;
            ledSleep.LedStyle = LedStyle.Round3D;
            ledSleep.Location = new Point(0x148, 0x48);
            ledSleep.Name = "ledSleep";
            ledSleep.OffColor = Color.Red;
            ledSleep.Size = new Size(30, 0x1f);
            ledSleep.TabIndex = 0x91;
            ledWake.LedStyle = LedStyle.Round3D;
            ledWake.Location = new Point(0x148, 0x2e);
            ledWake.Name = "ledWake";
            ledWake.OffColor = Color.Red;
            ledWake.Size = new Size(30, 0x1f);
            ledWake.TabIndex = 0xbf;
            menuStrip1.BackColor = SystemColors.ButtonFace;
            menuStrip1.Enabled = false;
            menuStrip1.Font = new Font("Arial Rounded MT Bold", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            menuStrip1.Items.AddRange(new ToolStripItem[] { logDataToolStripMenuItem1 });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(0x3c6, 0x18);
            menuStrip1.TabIndex = 0x40;
            menuStrip1.Text = "menuStrip1";
            menuStrip1.Visible = false;
            logDataToolStripMenuItem1.DropDownItems.AddRange(new ToolStripItem[] { enableLogDataApplicationToolStripMenuItem });
            logDataToolStripMenuItem1.Name = "logDataToolStripMenuItem1";
            logDataToolStripMenuItem1.Size = new Size(0x4d, 20);
            logDataToolStripMenuItem1.Text = "Log Data";
            enableLogDataApplicationToolStripMenuItem.Name = "enableLogDataApplicationToolStripMenuItem";
            enableLogDataApplicationToolStripMenuItem.Size = new Size(0x120, 0x16);
            enableLogDataApplicationToolStripMenuItem.Text = "Log Data XYZ with Tilt Detection";
            enableLogDataApplicationToolStripMenuItem.Click += new EventHandler(enableLogDataApplicationToolStripMenuItem_Click);
            pictureBox1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            pictureBox1.BackgroundImageLayout = ImageLayout.None;
            pictureBox1.BorderStyle = BorderStyle.Fixed3D;
            pictureBox1.Image = Resources.STB_TOPBAR_LARGE;
            pictureBox1.Location = new Point(0, -1);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(0x3c6, 0x39);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 0x48;
            pictureBox1.TabStop = false;
            CommStrip.Items.AddRange(new ToolStripItem[] { CommStripButton, toolStripStatusLabel });
            CommStrip.Location = new Point(0, 0x261);
            CommStrip.Name = "CommStrip";
            CommStrip.Size = new Size(0x3c6, 0x16);
            CommStrip.TabIndex = 0x4c;
            CommStrip.Text = "statusStrip1";
            CommStripButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
            CommStripButton.Image = Resources.imgYellowState;
            CommStripButton.ImageTransparentColor = Color.Magenta;
            CommStripButton.Name = "CommStripButton";
            CommStripButton.ShowDropDownArrow = false;
            CommStripButton.Size = new Size(20, 20);
            CommStripButton.Text = "toolStripDropDownButton1";
            CommStripButton.Click += new EventHandler(CommStripButton_Click);
            toolStripStatusLabel.BackColor = SystemColors.ButtonFace;
            toolStripStatusLabel.Name = "toolStripStatusLabel";
            toolStripStatusLabel.Size = new Size(0xd1, 0x11);
            toolStripStatusLabel.Text = "COM Port Not Connected, Please Connect";
            groupBox1.Controls.Add(rdoActive);
            groupBox1.Controls.Add(rdoStandby);
            groupBox1.Location = new Point(0x17, 0x10);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(0xb9, 0x34);
            groupBox1.TabIndex = 0xc5;
            groupBox1.TabStop = false;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            AutoScroll = true;
            AutoSize = true;
            BackColor = Color.LightSlateGray;
            base.ClientSize = new Size(0x3c6, 0x277);
            base.Controls.Add(CommStrip);
            base.Controls.Add(panelGeneral);
            base.Controls.Add(pictureBox1);
            base.Controls.Add(menuStrip1);
            base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            base.Icon = (Icon) resources.GetObject("$Icon");
            base.MaximizeBox = false;
            base.Name = "TiltApp2";
            base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            Text = "Tilt Application";
            base.Resize += new EventHandler(TiltApp2_Resize);
            panelGeneral.ResumeLayout(false);
            panelDisplay.ResumeLayout(false);
            groupBoxXY.ResumeLayout(false);
            groupBoxXY.PerformLayout();
            ((ISupportInitialize) pictureBox2).EndInit();
            ((ISupportInitialize) gaugeArcmin).EndInit();
            ((ISupportInitialize) gaugeXY).EndInit();
            panelAdvanced.ResumeLayout(false);
            panelAdvanced.PerformLayout();
            gbOM.ResumeLayout(false);
            gbOM.PerformLayout();
            pOverSampling.ResumeLayout(false);
            pOverSampling.PerformLayout();
            p1.ResumeLayout(false);
            p1.PerformLayout();
            gbDR.ResumeLayout(false);
            gbDR.PerformLayout();
            ((ISupportInitialize) ledStandby).EndInit();
            ((ISupportInitialize) ledSleep).EndInit();
            ((ISupportInitialize) ledWake).EndInit();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ((ISupportInitialize) pictureBox1).EndInit();
            CommStrip.ResumeLayout(false);
            CommStrip.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void InitializeStatusBar()
        {
            dv = new BoardComm();
            ControllerObj.GetCommObject(ref dv);
            LoadResource();
        }

        private void LoadResource()
        {
            try
            {
                Assembly executingAssembly = Assembly.GetExecutingAssembly();
                ResourceManager manager = new ResourceManager("MMA845x_DEMO.Properties.Resources", executingAssembly);
                ImageGreen = (Image) manager.GetObject("imgGreenState");
                ImageYellow = (Image) manager.GetObject("imgYellowState");
                ImageRed = (Image) manager.GetObject("imgRedState");
            }
            catch (Exception exception)
            {
                STBLogger.AddEvent(this, STBLogger.EventLevel.Information, "Exception", exception.Message + exception.Source + exception.StackTrace);
                ImageGreen = CommStripButton.Image;
                ImageYellow = CommStripButton.Image;
                ImageRed = CommStripButton.Image;
            }
        }

        private void rdo2g_CheckedChanged(object sender, EventArgs e)
        {
            if (rdo2g.Checked)
            {
                int[] datapassed = new int[1];
                FullScaleValue = 0;
                datapassed[0] = FullScaleValue;
                ControllerObj.SetFullScaleValueFlag = true;
                ControllerReqPacket reqName = new ControllerReqPacket();
                ControllerObj.CreateNewTaskFromGUI(reqName, 0, 4, datapassed);
            }
        }

        private void rdo4g_CheckedChanged(object sender, EventArgs e)
        {
            if (rdo4g.Checked)
            {
                int[] datapassed = new int[1];
                FullScaleValue = 1;
                datapassed[0] = FullScaleValue;
                ControllerObj.SetFullScaleValueFlag = true;
                ControllerReqPacket reqName = new ControllerReqPacket();
                ControllerObj.CreateNewTaskFromGUI(reqName, 0, 4, datapassed);
            }
        }

        private void rdo8g_CheckedChanged(object sender, EventArgs e)
        {
            if (rdo8g.Checked)
            {
                int[] datapassed = new int[1];
                FullScaleValue = 2;
                datapassed[0] = FullScaleValue;
                ControllerObj.SetFullScaleValueFlag = true;
                ControllerReqPacket reqName = new ControllerReqPacket();
                ControllerObj.CreateNewTaskFromGUI(reqName, 0, 4, datapassed);
            }
        }

        private void rdoActive_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoActive.Checked)
            {
                ledWake.Value = true;
                ledStandby.Value = false;
                ledSleep.Value = false;
                gbDR.Enabled = false;
                gbOM.Enabled = false;
                btnAutoCal.Enabled = false;
                int[] datapassed = new int[] { 1 };
                ControllerObj.ActiveFlag = true;
                ControllerReqPacket reqName = new ControllerReqPacket();
                ControllerObj.CreateNewTaskFromGUI(reqName, 0, 0x53, datapassed);
                if (DeviceID == deviceID.MMA8661Q)
                {
                    ControllerObj.EnableXYZ8StreamData();
                }
                else
                {
                    ControllerObj.EnableXYZ14StreamData();
                }
            }
        }

        private void rdoOSHiResMode_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoOSHiResMode.Checked)
            {
                int[] datapassed = new int[1];
                WakeOSMode = 2;
                datapassed[0] = WakeOSMode;
                ControllerObj.SetWakeOSModeFlag = true;
                ControllerReqPacket reqName = new ControllerReqPacket();
                ControllerObj.CreateNewTaskFromGUI(reqName, 0, 0x54, datapassed);
                switch (ddlDataRate.SelectedIndex)
                {
                    case 0:
                        DR_timestep = 1.25;
                        break;

                    case 1:
                        DR_timestep = 2.5;
                        break;

                    case 2:
                        DR_timestep = 2.5;
                        break;

                    case 3:
                        DR_timestep = 2.5;
                        break;

                    case 4:
                        DR_timestep = 2.5;
                        break;

                    case 5:
                        DR_timestep = 2.5;
                        break;

                    case 6:
                        DR_timestep = 2.5;
                        break;

                    case 7:
                        DR_timestep = 2.5;
                        break;
                }
            }
        }

        private void rdoOSLNLPMode_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoOSLNLPMode.Checked)
            {
                int[] datapassed = new int[1];
                WakeOSMode = 1;
                datapassed[0] = WakeOSMode;
                ControllerObj.SetWakeOSModeFlag = true;
                ControllerReqPacket reqName = new ControllerReqPacket();
                ControllerObj.CreateNewTaskFromGUI(reqName, 0, 0x54, datapassed);
                switch (ddlDataRate.SelectedIndex)
                {
                    case 0:
                        DR_timestep = 1.25;
                        break;

                    case 1:
                        DR_timestep = 2.5;
                        break;

                    case 2:
                        DR_timestep = 5.0;
                        break;

                    case 3:
                        DR_timestep = 10.0;
                        break;

                    case 4:
                        DR_timestep = 20.0;
                        break;

                    case 5:
                        DR_timestep = 80.0;
                        break;

                    case 6:
                        DR_timestep = 80.0;
                        break;

                    case 7:
                        DR_timestep = 80.0;
                        break;
                }
            }
        }

        private void rdoOSLPMode_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoOSLPMode.Checked)
            {
                int[] datapassed = new int[1];
                WakeOSMode = 3;
                datapassed[0] = WakeOSMode;
                ControllerObj.SetWakeOSModeFlag = true;
                ControllerReqPacket reqName = new ControllerReqPacket();
                ControllerObj.CreateNewTaskFromGUI(reqName, 0, 0x54, datapassed);
                switch (ddlDataRate.SelectedIndex)
                {
                    case 0:
                        DR_timestep = 1.25;
                        break;

                    case 1:
                        DR_timestep = 2.5;
                        break;

                    case 2:
                        DR_timestep = 5.0;
                        break;

                    case 3:
                        DR_timestep = 10.0;
                        break;

                    case 4:
                        DR_timestep = 20.0;
                        break;

                    case 5:
                        DR_timestep = 80.0;
                        break;

                    case 6:
                        DR_timestep = 160.0;
                        break;

                    case 7:
                        DR_timestep = 160.0;
                        break;
                }
            }
        }

        private void rdoOSNormalMode_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoOSNormalMode.Checked)
            {
                int[] datapassed = new int[1];
                WakeOSMode = 0;
                datapassed[0] = WakeOSMode;
                ControllerObj.SetWakeOSModeFlag = true;
                ControllerReqPacket reqName = new ControllerReqPacket();
                ControllerObj.CreateNewTaskFromGUI(reqName, 0, 0x54, datapassed);
                switch (ddlDataRate.SelectedIndex)
                {
                    case 0:
                        DR_timestep = 1.25;
                        break;

                    case 1:
                        DR_timestep = 2.5;
                        break;

                    case 2:
                        DR_timestep = 5.0;
                        break;

                    case 3:
                        DR_timestep = 10.0;
                        break;

                    case 4:
                        DR_timestep = 20.0;
                        break;

                    case 5:
                        DR_timestep = 20.0;
                        break;

                    case 6:
                        DR_timestep = 20.0;
                        break;

                    case 7:
                        DR_timestep = 20.0;
                        break;
                }
            }
        }

        private void rdoStandby_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoStandby.Checked)
            {
                ledWake.Value = false;
                ledStandby.Value = true;
                ledSleep.Value = false;
                gbDR.Enabled = true;
                gbOM.Enabled = true;
                btnAutoCal.Enabled = true;
                int[] datapassed = new int[] { 0 };
                ControllerObj.ActiveFlag = true;
                ControllerReqPacket reqName = new ControllerReqPacket();
                ControllerObj.CreateNewTaskFromGUI(reqName, 0, 0x53, datapassed);
                ControllerObj.DisableData();
            }
        }

        private double TiltAngle(double A, double B)
        {
            double num = A;
            double num2 = B;
            double num3 = 0.0;
            if (num == 0.0)
            {
                num = 0.0001;
            }
            if (num2 == 0.0)
            {
                num2 = 0.0001;
            }
            if ((num > 0.0) && (num2 < 0.0))
            {
                return (180.0 + ((Math.Atan(num / num2) * 180.0) / 3.1415926535897931));
            }
            if ((num < 0.0) && (num2 < 0.0))
            {
                return (180.0 + ((Math.Atan(num / num2) * 180.0) / 3.1415926535897931));
            }
            if ((num < 0.0) && (num2 > 0.0))
            {
                return (360.0 + ((Math.Atan(num / num2) * 180.0) / 3.1415926535897931));
            }
            if ((num > 0.0) && (num2 > 0.0))
            {
                num3 = (Math.Atan(num / num2) * 180.0) / 3.1415926535897931;
            }
            return num3;
        }

        private void TiltApp2_Resize(object sender, EventArgs e)
        {
            windowHeight = base.Height;
        }

        private void TiltXY(double dataX, double dataY)
        {
            object obj2;
            double d = 0.0;
            double num2 = 0.0;
            d = TiltAngle(dataX, dataY);
            num2 = d - ((int) d);
            num2 *= 60.0;
            Monitor.Enter(obj2 = objectLock);
            try
            {
                gaugeXY.Value = (int) d;
                gaugeArcmin.Value = num2;
                string str4 = double.IsNaN(d) ? "Locked" : d.ToString("0");
                string str5 = double.IsNaN(num2) ? "Locked" : num2.ToString("0");
                string str6 = double.IsNaN(d) ? "Locked" : d.ToString("0.00");
                string str = Convert.ToString(str4);
                string str2 = Convert.ToString(str5);
                string str3 = Convert.ToString(str6);
                XYAngle = str;
                ArcminAngle = str2;
                FullValueAngle = str3;
            }
            catch (Exception)
            {
                XYAngle = "Locked";
                ArcminAngle = "Locked";
                FullValueAngle = "Locked";
            }
            finally
            {
                Monitor.Exit(obj2);
            }
        }

        private void tmrTiltTimer_Tick(object sender, EventArgs e)
        {
            lock (objectLock)
            {
                labelXY.Text = XYAngle;
                lblArcmin.Text = ArcminAngle;
                lblFullValue.Text = FullValueAngle;
            }
            UpdateFormState();
            lock (gaugeXY)
            {
                gaugeXY.Update();
            }
        }

        public void UpdateAngles(XYZGees xyz_gees)
        {
            TiltXY(xyz_gees.XAxis, xyz_gees.YAxis);
        }

        private void UpdateCommStrip()
        {
            CommunicationState invalid = CommunicationState.Invalid;
            string commStatus = "";
            try
            {
                invalid = dv.GetCommState();
                commStatus = dv.GetCommStatus();
            }
            catch (Exception)
            {
            }
            switch (invalid)
            {
                case CommunicationState.HWFind:
                    CommMode = eCommMode.FindingHW;
                    break;

                case CommunicationState.Idle:
                    CommMode = eCommMode.Closed;
                    break;

                case CommunicationState.Ready:
                    CommMode = eCommMode.Running;
                    if (commStatus.Substring(commStatus.IndexOf("SW:") + 3, 4) != CurrentFW)
                    {
                        CommMode = eCommMode.Bootloader;
                    }
                    else if (FirstTimeLoad)
                    {
                        FirstTimeLoad = false;
                        InitDevice();
                    }
                    break;

                default:
                    CommMode = eCommMode.Closed;
                    break;
            }
            toolStripStatusLabel.Text = commStatus;
            if (CommMode == eCommMode.FindingHW)
            {
                panelGeneral.Enabled = false;
                CommStripButton.Enabled = true;
                CommStripButton.Image = ImageYellow;
            }
            else if (CommMode == eCommMode.Running)
            {
                panelGeneral.Enabled = true;
                CommStripButton.Enabled = true;
                CommStripButton.Image = ImageGreen;
            }
            else if (CommMode == eCommMode.Closed)
            {
                panelGeneral.Enabled = false;
                CommStripButton.Enabled = true;
                CommStripButton.Image = ImageRed;
            }
            else if (CommMode == eCommMode.Bootloader)
            {
                panelGeneral.Enabled = false;
                CommStripButton.Image = ImageYellow;
                if (!LoadingFW)
                {
                    LoadingFW = true;
                    Loader loader = new Loader(dv, "MMA845x-FW-Bootloader.s19", "silent");
                    dv.Close();
                    CommMode = eCommMode.Closed;
                    CommStripButton_Click(this, new EventArgs());
                    return;
                }
            }
            CommStrip.Refresh();
        }

        private void UpdateFormState()
        {
            DecodeGUIPackets();
            UpdateCommStrip();
        }

        private enum ActionButton
        {
            Invalid = 3,
            StartDatalog = 1,
            StopDatalog = 2
        }

        private enum eCommMode
        {
            Bootloader = 4,
            Closed = 1,
            FindingHW = 2,
            Running = 3
        }
    }
}

