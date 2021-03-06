﻿namespace MMA845xQEvaluation
{
    using Freescale.SASD.Communication;
    using GlobalSTB;
    using MMA845x_DEMO.Properties;
    using NationalInstruments.UI;
    using NationalInstruments.UI.WindowsForms;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Reflection;
    using System.Resources;
    using System.Text;
    using System.Threading;
    using System.Windows.Forms;

    public class TapDemo : Form
    {
        private GroupBox AccelControllerGroup;
        private ActionButton ActionButtonState;
        private int ActiveDynamicRange;
        private bool B_ActionButtonClicked;
        private int Bit12_2Complement;
        private int Bit12_MaxPositive;
        private int Bit8_2Complement;
        private int Bit8_MaxPositive;
        private bool blDirTapChange = false;
        private Button btnAutoCal;
        private Button btnPulseResetTime2ndPulse;
        private Button btnPulseSetTime2ndPulse;
        private Button btnResetFirstPulseTimeLimit;
        private Button btnResetPulseThresholds;
        private Button btnSetFirstPulseTimeLimit;
        private Button btnSetPulseThresholds;
        private CheckBox chkAnalogLowNoise;
        private CheckBox chkPulseEnableLatch;
        private CheckBox chkPulseEnableXDP;
        private CheckBox chkPulseEnableXSP;
        private CheckBox chkPulseEnableYDP;
        private CheckBox chkPulseEnableYSP;
        private CheckBox chkPulseEnableZDP;
        private CheckBox chkPulseEnableZSP;
        private CheckBox chkPulseHPFBypass;
        private CheckBox chkPulseIgnorLatentPulses;
        private CheckBox chkPulseLPFEnable;
        private eCommMode CommMode = eCommMode.Closed;
        private StatusStrip CommStrip;
        private ToolStripDropDownButton CommStripButton;
        private IContainer components = null;
        private object ControllerEventsLock = new object();
        private AccelController ControllerObj;
        private string CurrentFW = "4003";
        private bool dataStream = false;
        private ComboBox ddlDataRate;
        private int DeviceID;
        private double DR_PulseTimeStepNoLPF;
        private double DR_timestep;
        private BoardComm dv;
        private ToolStripMenuItem enableLogDataApplicationToolStripMenuItem;
        private bool FirstTimeLoad = true;
        private const int Form_Max_Height = 720;
        private const int Form_Min_Height = 200;
        private int FullScaleValue;
        private GroupBox gbDR;
        private GroupBox gbOM;
        private GroupBox groupBox1;
        private Image ImageGreen;
        private Image ImageRed;
        private Image ImageYellow;
        private int iPulseStat = 0;
        private int iXThreshold = 0;
        private int iYThreshold = 0;
        private int iZThreshold = 0;
        private int keypad_location;
        private Label label10;
        private Label label15;
        private Label label173;
        private Label label2;
        private Label label255;
        private Label label3;
        private Label label35;
        private Label label5;
        private Label label6;
        private Label label70;
        private Label label71;
        private Label label8;
        private Label label9;
        private Label lbl2gSensitivity;
        private Label lbl4gSensitivity;
        private Label lbl8gSensitivity;
        private Label lblCurrentMCU;
        private Label lblFirstPulseTimeLimit;
        private Label lblFirstPulseTimeLimitms;
        private Label lblFirstTimeLimitVal;
        private Label lblPPolX;
        private Label lblPPolY;
        private Label lblPPolZ;
        private Label lblPulse2ndPulseWin;
        private Label lblPulse2ndPulseWinms;
        private Label lblPulse2ndPulseWinVal;
        private Label lblPulseLatency;
        private Label lblPulseLatencyms;
        private Label lblPulseLatencyVal;
        private Label lblPulseXThreshold;
        private Label lblPulseXThresholdg;
        private Label lblPulseXThresholdVal;
        private Label lblPulseYThreshold;
        private Label lblPulseYThresholdg;
        private Label lblPulseYThresholdVal;
        private Label lblPulseZThreshold;
        private Label lblPulseZThresholdg;
        private Label lblPulseZThresholdVal;
        private Label lbsleep;
        private int ledCount = 0;
        private Led ledMCU1;
        private Led ledPulseDouble;
        private Led ledPX;
        private Led ledPY;
        private Led ledPZ;
        private Led ledSleep;
        private Led ledStandby;
        private Led ledTapEA;
        private Led ledWake;
        private Legend legend1;
        private LegendItem legendItem1;
        private LegendItem legendItem2;
        private LegendItem legendItem3;
        private bool LoadingFW = false;
        private ToolStripMenuItem logDataToolStripMenuItem1;
        private WaveformPlot MainScreenXAxis;
        private WaveformPlot MainScreenYAxis;
        private WaveformPlot MainScreenZAxis;
        private MenuStrip menuStrip1;
        private object objectLock = new object();
        private Panel p1;
        private Panel p10;
        private Panel p11;
        private Panel p12;
        private Panel panel1;
        private Panel panel15;
        private Panel panelAdvanced;
        private Panel panelDisplay;
        private FlowLayoutPanel panelGeneral;
        private PictureBox pictureBox1;
        private WaveformGraph pltFIFOPlot;
        private Panel pOverSampling;
        private double pulse_step;
        private Queue qFIFOArray = new Queue();
        private RadioButton rdo2g;
        private RadioButton rdo4g;
        private RadioButton rdo8g;
        private RadioButton rdoActive;
        private RadioButton rdoDefaultSP;
        private RadioButton rdoDefaultSPDP;
        private RadioButton rdoOSHiResMode;
        private RadioButton rdoOSLNLPMode;
        private RadioButton rdoOSLPMode;
        private RadioButton rdoOSNormalMode;
        private RadioButton rdoStandby;
        private string[] RegisterNames = new string[] { 
            "STATUS", "OUT_XMSB", "OUT_XLSB", "OUT_YMSB", "OUT_YLSB", "OUT_ZMSB", "OUT_ZLSB", "RESERVED ", "RESERVED ", "FSETUP", "TRIGCFG", "SYSMOD", "INTSOURCE", "WHOAMI", "XYZDATACFG", "HPFILTER", 
            "PLSTATUS", "PLCFG", "PLCOUNT", "PLBFZCOMP", "PLPLTHSREG", "FFMTCFG", "FFMTSRC", "FFMTTHS", "FFMTCOUNT", "RESERVED", "RESERVED", "RESERVED", "RESERVED", "TRANSCFG", "TRANSSRC", "TRANSTHS", 
            "TRANSCOUNT", "PULSECFG", "PULSESRC", "PULSETHSX", "PULSETHSY", "PULSETHSZ", "PULSETMLT", "PULSELTCY", "PULSEWIND", "ASLPCOUNT", "CTRLREG1", "CTRLREG2", "CTRLREG3", "CTRLREG4", "CTRLREG5", "OFFSET_X", 
            "OFFSET_Y", "OFFSET_Z"
         };
        private bool sampleRateCheckFlag = false;
        private SaveFileDialog saveFileDialog1;
        private string sDirTap;
        private string stringNumber;
        private TrackBar tbFirstPulseTimeLimit;
        private TrackBar tbPulse2ndPulseWin;
        private TrackBar tbPulseLatency;
        private TrackBar tbPulseXThreshold;
        private TrackBar tbPulseYThreshold;
        private TrackBar tbPulseZThreshold;
        private TextBox textBox2;
        private System.Windows.Forms.Timer tmrTiltTimer;
        private ToolStripStatusLabel toolStripStatusLabel;
        private TextBox txtCurrent;
        private int WakeOSMode;
        private int windowHeight = 0;
        private int x_position;
        private XAxis xAxis3;
        private string XDirection = "";
        private int y_position;
        private YAxis yAxis3;
        private string YDirection = "";
        private string ZDirection = "";

        public TapDemo(object controllerObj)
        {
            InitializeComponent();
            Styles.FormatForm(this, new string[] { "panel15" });
            Styles.FormatInterruptPanel(panel1);
            panel15.BackColor = Color.LightSlateGray;
            ControllerObj = (AccelController) controllerObj;
            ControllerObj.ControllerEvents += new AccelController.ControllerEventHandler(ControllerObj_ControllerEvents);
            menuStrip1.Enabled = false;
            Bit8_2Complement = 0x100;
            Bit8_MaxPositive = 0x7f;
            Bit12_2Complement = 0x1000;
            Bit12_MaxPositive = 0x7ff;
            DR_timestep = 1.25;
            lblPPolX.Text = XDirection;
            lblPPolY.Text = YDirection;
            lblPPolZ.Text = ZDirection;
            ActionButtonState = ActionButton.StopDatalog;
            B_ActionButtonClicked = false;
            DeviceID = (int) ControllerObj.DeviceID;
            if (DeviceID == 5)
            {
                chkAnalogLowNoise.Visible = false;
                lbl2gSensitivity.Text = "1024 counts/g 12-bit";
                lbl4gSensitivity.Text = " 512 counts/g 12-bit";
                lbl8gSensitivity.Text = " 256 counts/g 12-bit";
            }
            else
            {
                chkAnalogLowNoise.Visible = true;
                lbl2gSensitivity.Text = "4096 counts/g 14-bit";
                lbl4gSensitivity.Text = "2048 counts/g 14-bit";
                lbl8gSensitivity.Text = "1024 counts/g 14-bit";
            }
            InitializeStatusBar();
            tmrTiltTimer.Enabled = true;
            tmrTiltTimer.Start();
        }

        private void btnAutoCal_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            int[] datapassed = new int[] { DeviceID };
            ControllerObj.AutoCalFlag = true;
            ControllerReqPacket reqName = new ControllerReqPacket();
            ControllerObj.CreateNewTaskFromGUI(reqName, 0, 0x52, datapassed);
            Thread.Sleep(0x1b58);
            Cursor = Cursors.Default;
            rdo8g.Checked = true;
            ddlDataRate.SelectedIndex = 7;
        }

        private void btnPulseResetTime2ndPulse_Click(object sender, EventArgs e)
        {
            btnPulseSetTime2ndPulse.Enabled = true;
            btnPulseResetTime2ndPulse.Enabled = false;
            tbPulseLatency.Enabled = true;
            tbPulse2ndPulseWin.Enabled = true;
            lblPulseLatency.Enabled = true;
            lblPulse2ndPulseWin.Enabled = true;
            lblPulseLatencyms.Enabled = true;
            lblPulse2ndPulseWinms.Enabled = true;
            lblPulseLatencyVal.Enabled = true;
            lblPulse2ndPulseWinVal.Enabled = true;
        }

        private void btnPulseSetTime2ndPulse_Click(object sender, EventArgs e)
        {
            int[] datapassed = new int[] { tbPulseLatency.Value };
            ControllerObj.SetPulseLatencyFlag = true;
            ControllerReqPacket reqName = new ControllerReqPacket();
            ControllerObj.CreateNewTaskFromGUI(reqName, 6, 0x47, datapassed);
            int[] numArray2 = new int[] { tbPulse2ndPulseWin.Value };
            ControllerObj.SetPulse2ndPulseWinFlag = true;
            ControllerReqPacket packet2 = new ControllerReqPacket();
            ControllerObj.CreateNewTaskFromGUI(packet2, 6, 0x48, numArray2);
            btnPulseSetTime2ndPulse.Enabled = false;
            btnPulseResetTime2ndPulse.Enabled = true;
            tbPulseLatency.Enabled = false;
            tbPulse2ndPulseWin.Enabled = false;
            lblPulseLatency.Enabled = false;
            lblPulse2ndPulseWin.Enabled = false;
            lblPulseLatencyms.Enabled = false;
            lblPulse2ndPulseWinms.Enabled = false;
            lblPulseLatencyVal.Enabled = false;
            lblPulse2ndPulseWinVal.Enabled = false;
        }

        private void btnResetFirstPulseTimeLimit_Click(object sender, EventArgs e)
        {
            btnSetFirstPulseTimeLimit.Enabled = true;
            btnResetFirstPulseTimeLimit.Enabled = false;
            tbFirstPulseTimeLimit.Enabled = true;
            lblFirstPulseTimeLimit.Enabled = true;
            lblFirstPulseTimeLimitms.Enabled = true;
            lblFirstTimeLimitVal.Enabled = true;
        }

        private void btnResetPulseThresholds_Click(object sender, EventArgs e)
        {
            btnSetPulseThresholds.Enabled = true;
            btnResetPulseThresholds.Enabled = false;
            tbPulseXThreshold.Enabled = true;
            tbPulseYThreshold.Enabled = true;
            tbPulseZThreshold.Enabled = true;
            lblPulseXThreshold.Enabled = true;
            lblPulseYThreshold.Enabled = true;
            lblPulseZThreshold.Enabled = true;
            lblPulseXThresholdg.Enabled = true;
            lblPulseYThresholdg.Enabled = true;
            lblPulseZThresholdg.Enabled = true;
            lblPulseXThresholdVal.Enabled = true;
            lblPulseYThresholdVal.Enabled = true;
            lblPulseZThresholdVal.Enabled = true;
        }

        private void btnSetFirstPulseTimeLimit_Click(object sender, EventArgs e)
        {
            int[] datapassed = new int[] { tbFirstPulseTimeLimit.Value };
            ControllerObj.SetPulseFirstTimeLimitFlag = true;
            ControllerReqPacket reqName = new ControllerReqPacket();
            ControllerObj.CreateNewTaskFromGUI(reqName, 6, 0x43, datapassed);
            btnSetFirstPulseTimeLimit.Enabled = false;
            btnResetFirstPulseTimeLimit.Enabled = true;
            tbFirstPulseTimeLimit.Enabled = false;
            lblFirstPulseTimeLimit.Enabled = false;
            lblFirstPulseTimeLimitms.Enabled = false;
            lblFirstTimeLimitVal.Enabled = false;
        }

        private void btnSetPulseThresholds_Click(object sender, EventArgs e)
        {
            int[] datapassed = new int[] { tbPulseXThreshold.Value };
            ControllerObj.SetPulseXThresholdFlag = true;
            ControllerReqPacket reqName = new ControllerReqPacket();
            ControllerObj.CreateNewTaskFromGUI(reqName, 6, 0x44, datapassed);
            int[] numArray2 = new int[] { tbPulseYThreshold.Value };
            ControllerObj.SetPulseYThresholdFlag = true;
            ControllerReqPacket packet2 = new ControllerReqPacket();
            ControllerObj.CreateNewTaskFromGUI(packet2, 6, 0x45, numArray2);
            int[] numArray3 = new int[] { tbPulseZThreshold.Value };
            ControllerObj.SetPulseZThresholdFlag = true;
            ControllerReqPacket packet3 = new ControllerReqPacket();
            ControllerObj.CreateNewTaskFromGUI(packet3, 6, 70, numArray3);
            btnSetPulseThresholds.Enabled = false;
            btnResetPulseThresholds.Enabled = true;
            tbPulseXThreshold.Enabled = false;
            tbPulseYThreshold.Enabled = false;
            tbPulseZThreshold.Enabled = false;
            lblPulseXThreshold.Enabled = false;
            lblPulseYThreshold.Enabled = false;
            lblPulseZThreshold.Enabled = false;
            lblPulseXThresholdg.Enabled = false;
            lblPulseYThresholdg.Enabled = false;
            lblPulseZThresholdg.Enabled = false;
            lblPulseXThresholdVal.Enabled = false;
            lblPulseYThresholdVal.Enabled = false;
            lblPulseZThresholdVal.Enabled = false;
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

        private void chkPulseEnableLatch_CheckedChanged(object sender, EventArgs e)
        {
            int num = chkPulseEnableLatch.Checked ? 0xff : 0;
            int[] datapassed = new int[] { num };
            ControllerObj.SetPulseLatchFlag = true;
            ControllerReqPacket reqName = new ControllerReqPacket();
            ControllerObj.CreateNewTaskFromGUI(reqName, 6, 60, datapassed);
        }

        private void chkPulseEnableXDP_CheckedChanged(object sender, EventArgs e)
        {
            int num = chkPulseEnableXDP.Checked ? 0xff : 0;
            int[] datapassed = new int[] { num };
            ControllerObj.SetPulseXDPFlag = true;
            ControllerReqPacket reqName = new ControllerReqPacket();
            ControllerObj.CreateNewTaskFromGUI(reqName, 6, 0x41, datapassed);
        }

        private void chkPulseEnableXSP_CheckedChanged(object sender, EventArgs e)
        {
            int num = chkPulseEnableXSP.Checked ? 0xff : 0;
            int[] datapassed = new int[] { num };
            ControllerObj.SetPulseXSPFlag = true;
            ControllerReqPacket reqName = new ControllerReqPacket();
            ControllerObj.CreateNewTaskFromGUI(reqName, 6, 0x42, datapassed);
        }

        private void chkPulseEnableYDP_CheckedChanged(object sender, EventArgs e)
        {
            int num = chkPulseEnableYDP.Checked ? 0xff : 0;
            int[] datapassed = new int[] { num };
            ControllerObj.SetPulseYDPFlag = true;
            ControllerReqPacket reqName = new ControllerReqPacket();
            ControllerObj.CreateNewTaskFromGUI(reqName, 6, 0x3f, datapassed);
        }

        private void chkPulseEnableYSP_CheckedChanged(object sender, EventArgs e)
        {
            int num = chkPulseEnableYSP.Checked ? 0xff : 0;
            int[] datapassed = new int[] { num };
            ControllerObj.SetPulseYSPFlag = true;
            ControllerReqPacket reqName = new ControllerReqPacket();
            ControllerObj.CreateNewTaskFromGUI(reqName, 6, 0x40, datapassed);
        }

        private void chkPulseEnableZDP_CheckedChanged(object sender, EventArgs e)
        {
            int num = chkPulseEnableZDP.Checked ? 0xff : 0;
            int[] datapassed = new int[] { num };
            ControllerObj.SetPulseZDPFlag = true;
            ControllerReqPacket reqName = new ControllerReqPacket();
            ControllerObj.CreateNewTaskFromGUI(reqName, 6, 0x3d, datapassed);
        }

        private void chkPulseEnableZSP_CheckedChanged(object sender, EventArgs e)
        {
            int num = chkPulseEnableZSP.Checked ? 0xff : 0;
            int[] datapassed = new int[] { num };
            ControllerObj.SetPulseZSPFlag = true;
            ControllerReqPacket reqName = new ControllerReqPacket();
            ControllerObj.CreateNewTaskFromGUI(reqName, 6, 0x3e, datapassed);
        }

        private void chkPulseHPFBypass_CheckedChanged(object sender, EventArgs e)
        {
            int[] datapassed = new int[1];
            int num = chkPulseHPFBypass.Checked ? 0xff : 0;
            datapassed[0] = num;
            ControllerObj.SetPulseHPFBypassFlag = true;
            ControllerReqPacket reqName = new ControllerReqPacket();
            ControllerObj.CreateNewTaskFromGUI(reqName, 6, 90, datapassed);
        }

        private void chkPulseIgnorLatentPulses_CheckedChanged(object sender, EventArgs e)
        {
            int num = chkPulseIgnorLatentPulses.Checked ? 0xff : 0;
            int[] datapassed = new int[] { num };
            ControllerObj.SetPulseDPAFlag = true;
            ControllerReqPacket reqName = new ControllerReqPacket();
            ControllerObj.CreateNewTaskFromGUI(reqName, 6, 0x3b, datapassed);
        }

        private void chkPulseLPFEnable_CheckedChanged(object sender, EventArgs e)
        {
            if (!chkPulseLPFEnable.Checked)
            {
                rdoDefaultSP.Checked = false;
                rdoDefaultSPDP.Checked = false;
            }
            int[] datapassed = new int[1];
            int num = chkPulseLPFEnable.Checked ? 0xff : 0;
            datapassed[0] = num;
            ControllerObj.SetPulseLPFEnableFlag = true;
            ControllerReqPacket reqName = new ControllerReqPacket();
            ControllerObj.CreateNewTaskFromGUI(reqName, 6, 0x59, datapassed);
            if (chkPulseLPFEnable.Checked)
            {
                pulse_step = DR_timestep;
            }
            else
            {
                pulse_step = DR_PulseTimeStepNoLPF;
            }
            double num2 = tbFirstPulseTimeLimit.Value * pulse_step;
            if (num2 > 1000.0)
            {
                lblFirstPulseTimeLimitms.Text = "s";
                num2 /= 1000.0;
                lblFirstTimeLimitVal.Text = string.Format("{0:F3}", num2);
            }
            else
            {
                lblFirstPulseTimeLimitms.Text = "ms";
                lblFirstTimeLimitVal.Text = string.Format("{0:F3}", num2);
            }
            double num3 = pulse_step * 2.0;
            double num4 = tbPulseLatency.Value * num3;
            if (num4 > 1000.0)
            {
                lblPulseLatencyms.Text = "s";
                num4 /= 1000.0;
                lblPulseLatencyVal.Text = string.Format("{0:F2}", num4);
            }
            else
            {
                lblPulseLatencyms.Text = "ms";
                lblPulseLatencyVal.Text = string.Format("{0:F2}", num4);
            }
            double num5 = pulse_step * 2.0;
            double num6 = tbPulse2ndPulseWin.Value * num5;
            if (num6 > 1000.0)
            {
                lblPulse2ndPulseWinms.Text = "s";
                num6 /= 1000.0;
                lblPulse2ndPulseWinVal.Text = string.Format("{0:F2}", num6);
            }
            else
            {
                lblPulse2ndPulseWinms.Text = "ms";
                lblPulse2ndPulseWinVal.Text = string.Format("{0:F2}", num6);
            }
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

        private void ConfigureFIFO()
        {
            int[] datapassed = new int[] { 0x10 };
            ControllerObj.SetFIFOWatermarkFlag = true;
            ControllerReqPacket reqName = new ControllerReqPacket();
            ControllerObj.CreateNewTaskFromGUI(reqName, 7, 20, datapassed);
            datapassed = new int[] { 3 };
            ControllerObj.SetFIFOModeFlag = true;
            ControllerReqPacket packet2 = new ControllerReqPacket();
            ControllerObj.CreateNewTaskFromGUI(packet2, 7, 0x13, datapassed);
            datapassed = new int[] { 0xff };
            ControllerObj.SetTrigPulseFlag = true;
            ControllerReqPacket packet3 = new ControllerReqPacket();
            ControllerObj.CreateNewTaskFromGUI(packet3, 7, 0x17, datapassed);
        }

        private void ControllerObj_ControllerEvents(ControllerEventType evt, object o)
        {
            int num = 0;
            int num2 = 0;
            int num3 = 0;
            XYZGees gees = new XYZGees();
            switch (((int) evt))
            {
                case 1:
                {
                    XYZCounts counts = (XYZCounts) o;
                    num = (counts.XAxis > Bit8_MaxPositive) ? (counts.XAxis - Bit8_2Complement) : counts.XAxis;
                    num2 = (counts.YAxis > Bit8_MaxPositive) ? (counts.YAxis - Bit8_2Complement) : counts.YAxis;
                    num3 = (counts.ZAxis > Bit8_MaxPositive) ? (counts.ZAxis - Bit8_2Complement) : counts.ZAxis;
                    gees.Mod = Math.Sqrt((double) (((num * num) + (num2 * num2)) + (num3 * num3)));
                    gees.XAxis = ((double) num) / gees.Mod;
                    gees.YAxis = ((double) num2) / gees.Mod;
                    gees.ZAxis = ((double) num3) / gees.Mod;
                    return;
                }
                case 2:
                    return;

                case 3:
                {
                    int num15;
                    sbyte xAxis;
                    byte num18;
                    int num19;
                    StreamWriter writer = File.CreateText("TapLog.csv");
                    XYZCounts[] countsArray = (XYZCounts[]) o;
                    lock (qFIFOArray)
                    {
                        qFIFOArray.Enqueue(countsArray);
                    }
                    sbyte[] numArray2 = new sbyte[3];
                    sbyte[] numArray3 = new sbyte[3];
                    byte[] buffer = new byte[3];
                    byte[] buffer2 = new byte[3];
                    byte[] buffer3 = new byte[3];
                    byte[] buffer4 = new byte[3];
                    byte[] buffer5 = new byte[3];
                    sbyte[,] numArray4 = new sbyte[3, 2];
                    byte[,] buffer6 = new byte[3, 2];
                    byte[] buffer7 = new byte[3];
                    byte num4 = 0xff;
                    byte num5 = 0xff;
                    byte index = 5;
                    byte num7 = 5;
                    byte num8 = 5;
                    byte num9 = 0;
                    byte num10 = 0;
                    byte num11 = 0;
                    byte num12 = (byte) ((0x80 / ActiveDynamicRange) * 0.4);
                    int num13 = 0;
                    int num14 = 0;
                    int[] numArray5 = new int[3];
                    int[] numArray6 = new int[3];
                    numArray2[2] = (sbyte) (xAxis = -128);
                    numArray2[0] = numArray2[1] = xAxis;
                    numArray3[2] = (sbyte) (xAxis = 0x7f);
                    numArray3[0] = numArray3[1] = xAxis;
                    buffer5[2] = (byte) (num18 = 0);
                    buffer5[0] = buffer5[1] = num18;
                    buffer7[2] = (byte) (num18 = 0);
                    buffer7[0] = buffer7[1] = num18;
                    numArray5[2] = num19 = 0;
                    numArray5[0] = numArray5[1] = num19;
                    numArray6[2] = num19 = 0;
                    numArray6[0] = numArray6[1] = num19;
                    string[] strArray = new string[] { "System config: ODR ", (1000.0 / DR_timestep).ToString(), "Hz - FS ", ActiveDynamicRange.ToString(), "g" };
                    string str = string.Concat(strArray);
                    writer.WriteLine(str);
                    str = "X,Y,Z";
                    writer.WriteLine(str);
                    for (num15 = 0; num15 < 0x20; num15++)
                    {
                        strArray = new string[5];
                        xAxis = (sbyte) countsArray[num15].XAxis;
                        strArray[0] = xAxis.ToString();
                        strArray[1] = ",";
                        xAxis = (sbyte) countsArray[num15].YAxis;
                        strArray[2] = xAxis.ToString();
                        strArray[3] = ",";
                        strArray[4] = ((sbyte) countsArray[num15].ZAxis).ToString();
                        str = string.Concat(strArray);
                        writer.WriteLine(str);
                        if (num15 < 12)
                        {
                            numArray5[0] += (sbyte) countsArray[num15].XAxis;
                            numArray5[1] += (sbyte) countsArray[num15].YAxis;
                            numArray5[2] += (sbyte) countsArray[num15].ZAxis;
                        }
                        if (num15 == 12)
                        {
                            numArray5[0] /= 12;
                            numArray5[1] /= 12;
                            numArray5[2] /= 12;
                        }
                        if (((sbyte) countsArray[num15].XAxis) > numArray2[0])
                        {
                            numArray2[0] = (sbyte) countsArray[num15].XAxis;
                            buffer[0] = (byte) num15;
                        }
                        if (((sbyte) countsArray[num15].XAxis) < numArray3[0])
                        {
                            numArray3[0] = (sbyte) countsArray[num15].XAxis;
                            buffer2[0] = (byte) num15;
                        }
                        if (num13 < (numArray2[0] - numArray3[0]))
                        {
                            num13 = numArray2[0] - numArray3[0];
                        }
                        if (((sbyte) countsArray[num15].YAxis) > numArray2[1])
                        {
                            numArray2[1] = (sbyte) countsArray[num15].YAxis;
                            buffer[1] = (byte) num15;
                        }
                        if (((sbyte) countsArray[num15].YAxis) < numArray3[1])
                        {
                            numArray3[1] = (sbyte) countsArray[num15].YAxis;
                            buffer2[1] = (byte) num15;
                        }
                        if (num13 < (numArray2[1] - numArray3[1]))
                        {
                            num13 = numArray2[1] - numArray3[1];
                        }
                        if (((sbyte) countsArray[num15].ZAxis) > numArray2[2])
                        {
                            numArray2[2] = (sbyte) countsArray[num15].ZAxis;
                            buffer[2] = (byte) num15;
                        }
                        if (((sbyte) countsArray[num15].ZAxis) < numArray3[2])
                        {
                            numArray3[2] = (sbyte) countsArray[num15].ZAxis;
                            buffer2[2] = (byte) num15;
                        }
                        if (num13 < (numArray2[2] - numArray3[2]))
                        {
                            num13 = numArray2[2] - numArray3[2];
                        }
                        if (num15 < 0x1f)
                        {
                            switch (buffer5[0])
                            {
                                case 0:
                                    if (Math.Abs((int) (((sbyte) countsArray[num15].XAxis) - ((sbyte) countsArray[num15 + 1].XAxis))) >= num12)
                                    {
                                        buffer5[0] = 1;
                                        buffer3[0] = (byte) num15;
                                        if (num4 > buffer3[0])
                                        {
                                            num4 = buffer3[0];
                                        }
                                    }
                                    break;

                                case 1:
                                    if ((Math.Sign((int) (((sbyte) countsArray[num15 - 1].XAxis) - ((sbyte) countsArray[num15].XAxis))) != Math.Sign((int) (((sbyte) countsArray[num15].XAxis) - ((sbyte) countsArray[num15 + 1].XAxis)))) && (countsArray[num15 - 1].XAxis != countsArray[num15].XAxis))
                                    {
                                        numArray4[0, buffer7[0]] = (sbyte) countsArray[num15].XAxis;
                                        buffer6[0, buffer7[0]] = (byte) num15;
                                        if (buffer7[0] == 0)
                                        {
                                            numArray6[0] = Math.Abs((int) (numArray5[0] - numArray4[0, buffer7[0]]));
                                            if (num14 < numArray6[0])
                                            {
                                                num14 = numArray6[0];
                                            }
                                            if (buffer6[0, buffer7[0]] < num5)
                                            {
                                                num5 = buffer6[0, buffer7[0]];
                                            }
                                        }
                                        buffer7[0] = (byte) (buffer7[0] + 1);
                                        if (buffer7[0] == 2)
                                        {
                                            buffer5[0] = 2;
                                        }
                                    }
                                    break;
                            }
                            switch (buffer5[1])
                            {
                                case 0:
                                    if (Math.Abs((int) (((sbyte) countsArray[num15].YAxis) - ((sbyte) countsArray[num15 + 1].YAxis))) >= num12)
                                    {
                                        buffer5[1] = 1;
                                        buffer3[1] = (byte) num15;
                                        if (num4 > buffer3[1])
                                        {
                                            num4 = buffer3[1];
                                        }
                                    }
                                    break;

                                case 1:
                                    if ((Math.Sign((int) (((sbyte) countsArray[num15 - 1].YAxis) - ((sbyte) countsArray[num15].YAxis))) != Math.Sign((int) (((sbyte) countsArray[num15].YAxis) - ((sbyte) countsArray[num15 + 1].YAxis)))) && (countsArray[num15 - 1].YAxis != countsArray[num15].YAxis))
                                    {
                                        numArray4[1, buffer7[1]] = (sbyte) countsArray[num15].YAxis;
                                        buffer6[1, buffer7[1]] = (byte) num15;
                                        if (buffer7[1] == 0)
                                        {
                                            numArray6[1] = Math.Abs((int) (numArray5[1] - numArray4[1, buffer7[1]]));
                                            if (num14 < numArray6[1])
                                            {
                                                num14 = numArray6[1];
                                            }
                                            if (buffer6[1, buffer7[1]] < num5)
                                            {
                                                num5 = buffer6[1, buffer7[1]];
                                            }
                                        }
                                        buffer7[1] = (byte) (buffer7[1] + 1);
                                        if (buffer7[1] == 2)
                                        {
                                            buffer5[1] = 2;
                                        }
                                    }
                                    break;
                            }
                            switch (buffer5[2])
                            {
                                case 0:
                                    if (Math.Abs((int) (((sbyte) countsArray[num15].ZAxis) - ((sbyte) countsArray[num15 + 1].ZAxis))) >= num12)
                                    {
                                        buffer5[2] = 1;
                                        buffer3[2] = (byte) num15;
                                        if (num4 > buffer3[2])
                                        {
                                            num4 = buffer3[2];
                                        }
                                    }
                                    break;

                                case 1:
                                    if ((Math.Sign((int) (((sbyte) countsArray[num15 - 1].ZAxis) - ((sbyte) countsArray[num15].ZAxis))) != Math.Sign((int) (((sbyte) countsArray[num15].ZAxis) - ((sbyte) countsArray[num15 + 1].ZAxis)))) && (countsArray[num15 - 1].ZAxis != countsArray[num15].ZAxis))
                                    {
                                        numArray4[2, buffer7[2]] = (sbyte) countsArray[num15].ZAxis;
                                        buffer6[2, buffer7[2]] = (byte) num15;
                                        if (buffer7[2] == 0)
                                        {
                                            numArray6[2] = Math.Abs((int) (numArray5[2] - numArray4[2, buffer7[2]]));
                                            if (num14 < numArray6[2])
                                            {
                                                num14 = numArray6[2];
                                            }
                                            if (buffer6[2, buffer7[2]] < num5)
                                            {
                                                num5 = buffer6[2, buffer7[2]];
                                            }
                                        }
                                        buffer7[2] = (byte) (buffer7[2] + 1);
                                        if (buffer7[2] == 2)
                                        {
                                            buffer5[2] = 2;
                                        }
                                    }
                                    break;
                            }
                        }
                    }
                    for (num15 = 0; num15 < 3; num15++)
                    {
                        if (buffer5[num15] == 2)
                        {
                            if (numArray4[num15, 0] > numArray4[num15, 1])
                            {
                                buffer4[num15] = 1;
                            }
                            else
                            {
                                buffer4[num15] = 2;
                            }
                            if (num14 <= (numArray6[num15] + 3))
                            {
                                num9 = (byte) (num9 + 1);
                                if (num9 == 1)
                                {
                                    index = (byte) num15;
                                }
                                else
                                {
                                    index = 5;
                                }
                                if (num13 <= ((numArray2[num15] - numArray3[num15]) + 3))
                                {
                                    num10 = (byte) (num10 + 1);
                                    if (num10 == 1)
                                    {
                                        num7 = (byte) num15;
                                    }
                                    else
                                    {
                                        num7 = 5;
                                    }
                                    if (num4 == buffer3[num15])
                                    {
                                        num11 = (byte) (num11 + 1);
                                        if (num11 == 1)
                                        {
                                            num8 = (byte) num15;
                                        }
                                        else
                                        {
                                            num8 = 5;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (num9 > 1)
                    {
                        if (num7 == 1)
                        {
                            index = num7;
                        }
                        else if (num11 == 1)
                        {
                            index = num8;
                        }
                        else
                        {
                            index = 5;
                        }
                    }
                    str = "Pulse Status Register," + string.Format("{0:X2}", iPulseStat);
                    writer.WriteLine(str);
                    str = "X thr reg, Y thr reg, Z thr reg, X thr counts, Y thr counts, Z thr counts";
                    writer.WriteLine(str);
                    strArray = new string[11];
                    strArray[0] = iXThreshold.ToString();
                    strArray[1] = ",";
                    strArray[2] = iYThreshold.ToString();
                    strArray[3] = ",";
                    strArray[4] = iZThreshold.ToString();
                    strArray[5] = ",";
                    strArray[6] = (iXThreshold * 4).ToString();
                    strArray[7] = ",";
                    strArray[8] = (iYThreshold * 4).ToString();
                    strArray[9] = ",";
                    strArray[10] = (iZThreshold * 4).ToString();
                    str = string.Concat(strArray);
                    writer.WriteLine(str);
                    str = "X DC base, Y DC base, Z DC base";
                    writer.WriteLine(str);
                    str = numArray5[0].ToString() + "," + numArray5[1].ToString() + "," + numArray5[2].ToString();
                    writer.WriteLine(str);
                    str = "X Delta, Y Delat, Z Delta";
                    writer.WriteLine(str);
                    str = numArray6[0].ToString() + "," + numArray6[1].ToString() + "," + numArray6[2].ToString();
                    writer.WriteLine(str);
                    str = "Max X, Max Y, Max Z, Min X, Min Y, Min Z";
                    writer.WriteLine(str);
                    str = numArray2[0].ToString() + "," + numArray2[1].ToString() + "," + numArray2[2].ToString() + "," + numArray3[0].ToString() + "," + numArray3[1].ToString() + "," + numArray3[2].ToString();
                    writer.WriteLine(str);
                    str = "Max X Idx, Max Y Idx, Max Z Idx, Min X Idx, Min Y Idx, Min Z Idx";
                    writer.WriteLine(str);
                    str = buffer[0].ToString() + "," + buffer[1].ToString() + "," + buffer[2].ToString() + "," + buffer2[0].ToString() + "," + buffer2[1].ToString() + "," + buffer2[2].ToString();
                    writer.WriteLine(str);
                    str = "X Dir, Y Dir, Z Dir";
                    writer.WriteLine(str);
                    str = buffer4[0].ToString() + "," + buffer4[1].ToString() + "," + buffer4[2].ToString();
                    writer.WriteLine(str);
                    str = "First X, Second X, First Y, Second Y, First Z, Second Z";
                    writer.WriteLine(str);
                    str = numArray4[0, 0].ToString() + "," + numArray4[0, 1].ToString() + "," + numArray4[1, 0].ToString() + "," + numArray4[1, 1].ToString() + "," + numArray4[2, 0].ToString() + "," + numArray4[2, 1].ToString();
                    writer.WriteLine(str);
                    str = "First X Idx, Second X Idx, First Y Idx, Second Y Idx, First Z Idx, Second Z Idx";
                    writer.WriteLine(str);
                    str = buffer6[0, 0].ToString() + "," + buffer6[0, 1].ToString() + "," + buffer6[1, 0].ToString() + "," + buffer6[1, 1].ToString() + "," + buffer6[2, 0].ToString() + "," + buffer6[2, 1].ToString();
                    writer.WriteLine(str);
                    str = "X Rise Idx, Y Rise Idx, Z Rise Idx";
                    writer.WriteLine(str);
                    str = buffer3[0].ToString() + "," + buffer3[1].ToString() + "," + buffer3[2].ToString();
                    writer.WriteLine(str);
                    str = "Lowest Peak Idx," + num5.ToString();
                    writer.WriteLine(str);
                    str = "Max Diff," + num13.ToString();
                    writer.WriteLine(str);
                    str = "First Axis," + index.ToString();
                    writer.WriteLine(str);
                    writer.Close();
                    blDirTapChange = true;
                    if (!(((index != 0) || chkPulseEnableXSP.Checked) || chkPulseEnableXDP.Checked))
                    {
                        index = 5;
                    }
                    if (!(((index != 1) || chkPulseEnableYSP.Checked) || chkPulseEnableYDP.Checked))
                    {
                        index = 5;
                    }
                    if (!(((index != 2) || chkPulseEnableZSP.Checked) || chkPulseEnableZDP.Checked))
                    {
                        index = 5;
                    }
                    blDirTapChange = true;
                    if (ledPZ.Value && ledPulseDouble.Value)
                    {
                        stringNumber = string.Format("{0:X1}", keypad_location);
                    }
                    if (index >= 3)
                    {
                        sDirTap = "N/D";
                        return;
                    }
                    ledMCU1.Value = true;
                    if (buffer4[index] == 1)
                    {
                        sDirTap = Convert.ToChar((int) (Convert.ToByte('X') + index)) + "\r\n Positive";
                    }
                    else
                    {
                        sDirTap = Convert.ToChar((int) (Convert.ToByte('X') + index)) + "\r\n Negative";
                    }
                    switch (index)
                    {
                        case 0:
                            XDirection = "";
                            YDirection = "";
                            ZDirection = "";
                            if (buffer4[index] != 1)
                            {
                                XDirection = "Negative";
                                x_position--;
                                if (x_position <= 1)
                                {
                                    x_position = 1;
                                }
                            }
                            else
                            {
                                XDirection = "Positive";
                                x_position++;
                                if (x_position >= 3)
                                {
                                    x_position = 3;
                                }
                            }
                            goto Label_14ED;

                        case 1:
                            XDirection = "";
                            YDirection = "";
                            ZDirection = "";
                            if (buffer4[index] != 1)
                            {
                                YDirection = "Negative";
                                y_position--;
                                if (y_position < 1)
                                {
                                    y_position = 1;
                                }
                            }
                            else
                            {
                                YDirection = "Positive";
                                y_position++;
                                if (y_position > 4)
                                {
                                    y_position = 4;
                                }
                            }
                            goto Label_162B;
                    }
                    return;
                }
                case 7:
                {
                    GUIUpdatePacket packet = (GUIUpdatePacket) o;
                    if (packet.PayLoad.Count > 0)
                    {
                        int[] numArray = (int[]) packet.PayLoad.Dequeue();
                        iPulseStat = numArray[0];
                        if ((numArray[0] & 0x80) != 0)
                        {
                            ledTapEA.Value = true;
                        }
                        if ((numArray[0] & 0x40) != 0)
                        {
                            ledPZ.Value = true;
                        }
                        else
                        {
                            ledPZ.Value = false;
                        }
                        if ((numArray[0] & 0x20) != 0)
                        {
                            ledPY.Value = true;
                        }
                        else
                        {
                            ledPY.Value = false;
                        }
                        if ((numArray[0] & 0x10) != 0)
                        {
                            ledPZ.Value = true;
                        }
                        else
                        {
                            ledPZ.Value = false;
                        }
                        if ((numArray[0] & 8) != 0)
                        {
                            ledPulseDouble.Value = true;
                        }
                        else
                        {
                            ledPulseDouble.Value = false;
                        }
                    }
                    return;
                }
                default:
                    return;
            }
        Label_14ED:
            switch (x_position)
            {
                case 1:
                    return;

                case 2:
                    switch (y_position)
                    {
                        case 2:
                            keypad_location = 2;
                            break;

                        case 3:
                            keypad_location = 5;
                            break;

                        case 4:
                            keypad_location = 8;
                            break;
                    }
                    return;

                case 3:
                    switch (y_position)
                    {
                        case 2:
                            keypad_location = 3;
                            break;

                        case 3:
                            keypad_location = 6;
                            break;

                        case 4:
                            keypad_location = 9;
                            break;
                    }
                    return;

                default:
                    return;
            }
        Label_162B:
            switch (x_position)
            {
                case 1:
                    switch (y_position)
                    {
                        case 2:
                            keypad_location = 1;
                            break;

                        case 3:
                            keypad_location = 4;
                            break;

                        case 4:
                            keypad_location = 7;
                            break;
                    }
                    break;

                case 2:
                    switch (y_position)
                    {
                        case 2:
                            keypad_location = 2;
                            break;

                        case 3:
                            keypad_location = 5;
                            break;

                        case 4:
                            keypad_location = 8;
                            break;
                    }
                    break;

                case 3:
                    switch (y_position)
                    {
                        case 2:
                            keypad_location = 3;
                            break;

                        case 3:
                            keypad_location = 6;
                            break;

                        case 4:
                            keypad_location = 9;
                            break;
                    }
                    break;
            }
        }

        private void ddlDataRate_SelectedIndexChanged(object sender, EventArgs e)
        {
            rdoDefaultSP.Checked = false;
            rdoDefaultSPDP.Checked = false;
            int[] datapassed = new int[] { ddlDataRate.SelectedIndex };
            ControllerObj.SetDataRateFlag = true;
            ControllerReqPacket reqName = new ControllerReqPacket();
            ControllerObj.CreateNewTaskFromGUI(reqName, 0, 5, datapassed);
            switch (ddlDataRate.SelectedIndex)
            {
                case 0:
                    DR_timestep = 1.25;
                    DR_PulseTimeStepNoLPF = 0.625;
                    break;

                case 1:
                    DR_timestep = 2.5;
                    if (WakeOSMode != 3)
                    {
                        DR_PulseTimeStepNoLPF = 0.625;
                        break;
                    }
                    DR_PulseTimeStepNoLPF = 1.25;
                    break;

                case 2:
                    switch (WakeOSMode)
                    {
                        case 2:
                            DR_timestep = 2.5;
                            DR_PulseTimeStepNoLPF = 0.625;
                            goto Label_047A;

                        case 3:
                            DR_timestep = 5.0;
                            DR_PulseTimeStepNoLPF = 2.5;
                            goto Label_047A;
                    }
                    DR_timestep = 5.0;
                    DR_PulseTimeStepNoLPF = 1.25;
                    break;

                case 3:
                    switch (WakeOSMode)
                    {
                        case 2:
                            DR_timestep = 2.5;
                            DR_PulseTimeStepNoLPF = 0.625;
                            goto Label_047A;

                        case 3:
                            DR_timestep = 10.0;
                            DR_PulseTimeStepNoLPF = 5.0;
                            goto Label_047A;
                    }
                    DR_timestep = 10.0;
                    DR_PulseTimeStepNoLPF = 2.5;
                    break;

                case 4:
                    switch (WakeOSMode)
                    {
                        case 2:
                            DR_timestep = 2.5;
                            DR_PulseTimeStepNoLPF = 0.625;
                            goto Label_047A;

                        case 3:
                            DR_timestep = 20.0;
                            DR_PulseTimeStepNoLPF = 10.0;
                            goto Label_047A;
                    }
                    DR_timestep = 20.0;
                    DR_PulseTimeStepNoLPF = 5.0;
                    break;

                case 5:
                    DR_timestep = 80.0;
                    switch (WakeOSMode)
                    {
                        case 0:
                            DR_timestep = 20.0;
                            DR_PulseTimeStepNoLPF = 5.0;
                            goto Label_047A;

                        case 1:
                            DR_timestep = 80.0;
                            DR_PulseTimeStepNoLPF = 20.0;
                            goto Label_047A;

                        case 2:
                            DR_timestep = 2.5;
                            DR_PulseTimeStepNoLPF = 0.625;
                            goto Label_047A;

                        case 3:
                            DR_timestep = 80.0;
                            DR_PulseTimeStepNoLPF = 40.0;
                            goto Label_047A;
                    }
                    break;

                case 6:
                    switch (WakeOSMode)
                    {
                        case 0:
                            DR_timestep = 20.0;
                            DR_PulseTimeStepNoLPF = 5.0;
                            goto Label_047A;

                        case 1:
                            DR_timestep = 80.0;
                            DR_PulseTimeStepNoLPF = 20.0;
                            goto Label_047A;

                        case 2:
                            DR_timestep = 2.5;
                            DR_PulseTimeStepNoLPF = 0.625;
                            goto Label_047A;

                        case 3:
                            DR_timestep = 160.0;
                            DR_PulseTimeStepNoLPF = 40.0;
                            goto Label_047A;
                    }
                    break;

                case 7:
                    switch (WakeOSMode)
                    {
                        case 0:
                            DR_timestep = 20.0;
                            DR_PulseTimeStepNoLPF = 5.0;
                            goto Label_047A;

                        case 1:
                            DR_timestep = 80.0;
                            DR_PulseTimeStepNoLPF = 20.0;
                            goto Label_047A;

                        case 2:
                            DR_timestep = 2.5;
                            DR_PulseTimeStepNoLPF = 0.625;
                            goto Label_047A;

                        case 3:
                            DR_timestep = 160.0;
                            DR_PulseTimeStepNoLPF = 40.0;
                            goto Label_047A;
                    }
                    break;
            }
        Label_047A:
            if (chkPulseLPFEnable.Checked)
            {
                pulse_step = DR_timestep;
            }
            else
            {
                pulse_step = DR_PulseTimeStepNoLPF;
            }
            double num = tbFirstPulseTimeLimit.Value * pulse_step;
            if (num > 1000.0)
            {
                lblFirstPulseTimeLimitms.Text = "s";
                num /= 1000.0;
                lblFirstTimeLimitVal.Text = string.Format("{0:F3}", num);
            }
            else
            {
                lblFirstPulseTimeLimitms.Text = "ms";
                lblFirstTimeLimitVal.Text = string.Format("{0:F3}", num);
            }
            double num2 = pulse_step * 2.0;
            double num3 = tbPulseLatency.Value * num2;
            if (num3 > 1000.0)
            {
                lblPulseLatencyms.Text = "s";
                num3 /= 1000.0;
                lblPulseLatencyVal.Text = string.Format("{0:F2}", num3);
            }
            else
            {
                lblPulseLatencyms.Text = "ms";
                lblPulseLatencyVal.Text = string.Format("{0:F2}", num3);
            }
            double num4 = pulse_step * 2.0;
            double num5 = tbPulse2ndPulseWin.Value * num4;
            if (num5 > 1000.0)
            {
                lblPulse2ndPulseWinms.Text = "s";
                num5 /= 1000.0;
                lblPulse2ndPulseWinVal.Text = string.Format("{0:F2}", num5);
            }
            else
            {
                lblPulse2ndPulseWinms.Text = "ms";
                lblPulse2ndPulseWinVal.Text = string.Format("{0:F2}", num5);
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
            }
        }

        public void EndDemo()
        {
            lock (ControllerEventsLock)
            {
                tmrTiltTimer.Stop();
                tmrTiltTimer.Enabled = false;
                ControllerObj.DisableFIFO8bData();
                ControllerObj.ControllerEvents -= new AccelController.ControllerEventHandler(ControllerObj_ControllerEvents);
                ControllerObj.ResetDevice();
                base.Close();
            }
        }

        private void InitDevice()
        {
            ControllerObj.StartDevice();
            Thread.Sleep(10);
            ControllerObj.Boot();
            Thread.Sleep(20);
            ddlDataRate.SelectedIndex = 1;
            ControllerObj.SetHPDataOut(true);
            rdo8g.Checked = true;
            rdo8g_CheckedChanged(this, null);
            rdoDefaultSPDP.Checked = true;
            rdoDefaultSPDP_CheckedChanged(this, null);
            ControllerObj.EnableFIFO8StreamData(0);
            ConfigureFIFO();
            Thread.Sleep(10);
            SetPulseInterrupts(true);
            Thread.Sleep(20);
            rdoActive.Checked = true;
            rdoActive_CheckedChanged(this, null);
            UpdateFormState();
            ControllerObj.ReturnPulseStatus = true;
        }

        private void InitializeComponent()
        {
            components = new Container();
            ComponentResourceManager resources = new ComponentResourceManager(typeof(TapDemo));
            tmrTiltTimer = new System.Windows.Forms.Timer(components);
            panelGeneral = new FlowLayoutPanel();
            panelDisplay = new Panel();
            p11 = new Panel();
            label15 = new Label();
            ledPulseDouble = new Led();
            lblPPolZ = new Label();
            lblPPolY = new Label();
            lblPPolX = new Label();
            label255 = new Label();
            label6 = new Label();
            ledTapEA = new Led();
            label8 = new Label();
            ledPZ = new Led();
            ledPX = new Led();
            label9 = new Label();
            label10 = new Label();
            ledPY = new Led();
            label5 = new Label();
            label3 = new Label();
            label2 = new Label();
            txtCurrent = new TextBox();
            lblCurrentMCU = new Label();
            ledMCU1 = new Led();
            legend1 = new Legend();
            legendItem1 = new LegendItem();
            MainScreenXAxis = new WaveformPlot();
            xAxis3 = new XAxis();
            yAxis3 = new YAxis();
            legendItem2 = new LegendItem();
            MainScreenYAxis = new WaveformPlot();
            legendItem3 = new LegendItem();
            MainScreenZAxis = new WaveformPlot();
            pltFIFOPlot = new WaveformGraph();
            groupBox1 = new GroupBox();
            textBox2 = new TextBox();
            panel1 = new Panel();
            AccelControllerGroup = new GroupBox();
            gbDR = new GroupBox();
            lbl4gSensitivity = new Label();
            rdo2g = new RadioButton();
            rdo8g = new RadioButton();
            rdo4g = new RadioButton();
            lbl2gSensitivity = new Label();
            lbl8gSensitivity = new Label();
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
            rdoStandby = new RadioButton();
            label173 = new Label();
            label71 = new Label();
            lbsleep = new Label();
            ledStandby = new Led();
            ledSleep = new Led();
            ledWake = new Led();
            rdoActive = new RadioButton();
            rdoDefaultSP = new RadioButton();
            panel15 = new Panel();
            chkPulseLPFEnable = new CheckBox();
            chkPulseHPFBypass = new CheckBox();
            rdoDefaultSPDP = new RadioButton();
            panelAdvanced = new Panel();
            btnResetPulseThresholds = new Button();
            p12 = new Panel();
            btnPulseResetTime2ndPulse = new Button();
            btnPulseSetTime2ndPulse = new Button();
            tbPulseLatency = new TrackBar();
            chkPulseEnableXDP = new CheckBox();
            chkPulseEnableYDP = new CheckBox();
            chkPulseEnableZDP = new CheckBox();
            lblPulse2ndPulseWinms = new Label();
            lblPulseLatency = new Label();
            lblPulse2ndPulseWinVal = new Label();
            lblPulseLatencyVal = new Label();
            lblPulse2ndPulseWin = new Label();
            lblPulseLatencyms = new Label();
            tbPulse2ndPulseWin = new TrackBar();
            chkPulseIgnorLatentPulses = new CheckBox();
            btnSetPulseThresholds = new Button();
            tbPulseZThreshold = new TrackBar();
            tbPulseXThreshold = new TrackBar();
            lblPulseYThreshold = new Label();
            tbPulseYThreshold = new TrackBar();
            lblPulseYThresholdVal = new Label();
            lblPulseXThresholdg = new Label();
            lblPulseXThresholdVal = new Label();
            lblPulseZThresholdg = new Label();
            lblPulseZThreshold = new Label();
            lblPulseZThresholdVal = new Label();
            lblPulseYThresholdg = new Label();
            lblPulseXThreshold = new Label();
            p10 = new Panel();
            btnResetFirstPulseTimeLimit = new Button();
            btnSetFirstPulseTimeLimit = new Button();
            chkPulseEnableLatch = new CheckBox();
            chkPulseEnableXSP = new CheckBox();
            chkPulseEnableYSP = new CheckBox();
            chkPulseEnableZSP = new CheckBox();
            tbFirstPulseTimeLimit = new TrackBar();
            lblFirstPulseTimeLimitms = new Label();
            lblFirstTimeLimitVal = new Label();
            lblFirstPulseTimeLimit = new Label();
            menuStrip1 = new MenuStrip();
            logDataToolStripMenuItem1 = new ToolStripMenuItem();
            enableLogDataApplicationToolStripMenuItem = new ToolStripMenuItem();
            CommStrip = new StatusStrip();
            CommStripButton = new ToolStripDropDownButton();
            toolStripStatusLabel = new ToolStripStatusLabel();
            saveFileDialog1 = new SaveFileDialog();
            pictureBox1 = new PictureBox();
            panelGeneral.SuspendLayout();
            panelDisplay.SuspendLayout();
            p11.SuspendLayout();
            ((ISupportInitialize) ledPulseDouble).BeginInit();
            ((ISupportInitialize) ledTapEA).BeginInit();
            ((ISupportInitialize) ledPZ).BeginInit();
            ((ISupportInitialize) ledPX).BeginInit();
            ((ISupportInitialize) ledPY).BeginInit();
            ((ISupportInitialize) ledMCU1).BeginInit();
            ((ISupportInitialize) legend1).BeginInit();
            ((ISupportInitialize) pltFIFOPlot).BeginInit();
            groupBox1.SuspendLayout();
            panel1.SuspendLayout();
            AccelControllerGroup.SuspendLayout();
            gbDR.SuspendLayout();
            gbOM.SuspendLayout();
            pOverSampling.SuspendLayout();
            p1.SuspendLayout();
            ((ISupportInitialize) ledStandby).BeginInit();
            ((ISupportInitialize) ledSleep).BeginInit();
            ((ISupportInitialize) ledWake).BeginInit();
            panel15.SuspendLayout();
            panelAdvanced.SuspendLayout();
            p12.SuspendLayout();
            tbPulseLatency.BeginInit();
            tbPulse2ndPulseWin.BeginInit();
            tbPulseZThreshold.BeginInit();
            tbPulseXThreshold.BeginInit();
            tbPulseYThreshold.BeginInit();
            p10.SuspendLayout();
            tbFirstPulseTimeLimit.BeginInit();
            menuStrip1.SuspendLayout();
            CommStrip.SuspendLayout();
            ((ISupportInitialize) pictureBox1).BeginInit();
            base.SuspendLayout();
            tmrTiltTimer.Interval = 40;
            tmrTiltTimer.Tick += new EventHandler(tmrTiltTimer_Tick);
            panelGeneral.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            panelGeneral.AutoScroll = true;
            panelGeneral.Controls.Add(panelDisplay);
            panelGeneral.Controls.Add(panel1);
            panelGeneral.Controls.Add(panelAdvanced);
            panelGeneral.Location = new Point(1, 0x35);
            panelGeneral.Name = "panelGeneral";
            panelGeneral.Size = new Size(0x3c4, 0x292);
            panelGeneral.TabIndex = 0x47;
            panelDisplay.BorderStyle = BorderStyle.FixedSingle;
            panelDisplay.Controls.Add(p11);
            panelDisplay.Controls.Add(label5);
            panelDisplay.Controls.Add(label3);
            panelDisplay.Controls.Add(label2);
            panelDisplay.Controls.Add(txtCurrent);
            panelDisplay.Controls.Add(lblCurrentMCU);
            panelDisplay.Controls.Add(ledMCU1);
            panelDisplay.Controls.Add(legend1);
            panelDisplay.Controls.Add(pltFIFOPlot);
            panelDisplay.Controls.Add(groupBox1);
            panelDisplay.Font = new Font("Microsoft Sans Serif", 9.5f, FontStyle.Bold, GraphicsUnit.Point, 0);
            panelDisplay.Location = new Point(3, 3);
            panelDisplay.Name = "panelDisplay";
            panelDisplay.Size = new Size(0x21a, 0x16e);
            panelDisplay.TabIndex = 0x49;
            p11.BackColor = Color.LightSlateGray;
            p11.BorderStyle = BorderStyle.Fixed3D;
            p11.Controls.Add(label15);
            p11.Controls.Add(ledPulseDouble);
            p11.Controls.Add(lblPPolZ);
            p11.Controls.Add(lblPPolY);
            p11.Controls.Add(lblPPolX);
            p11.Controls.Add(label255);
            p11.Controls.Add(label6);
            p11.Controls.Add(ledTapEA);
            p11.Controls.Add(label8);
            p11.Controls.Add(ledPZ);
            p11.Controls.Add(ledPX);
            p11.Controls.Add(label9);
            p11.Controls.Add(label10);
            p11.Controls.Add(ledPY);
            p11.Location = new Point(3, 3);
            p11.Name = "p11";
            p11.Size = new Size(0xc1, 0xd0);
            p11.TabIndex = 0xfe;
            label15.AutoSize = true;
            label15.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label15.ForeColor = Color.White;
            label15.Location = new Point(11, 40);
            label15.Name = "label15";
            label15.Size = new Size(90, 0x10);
            label15.TabIndex = 0xaf;
            label15.Text = "Double Tap";
            ledPulseDouble.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            ledPulseDouble.ForeColor = Color.White;
            ledPulseDouble.LedStyle = LedStyle.Round3D;
            ledPulseDouble.Location = new Point(0x66, 0x1f);
            ledPulseDouble.Name = "ledPulseDouble";
            ledPulseDouble.OffColor = Color.Red;
            ledPulseDouble.Size = new Size(40, 40);
            ledPulseDouble.TabIndex = 0xae;
            lblPPolZ.AutoSize = true;
            lblPPolZ.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPPolZ.ForeColor = Color.White;
            lblPPolZ.Location = new Point(0x60, 0x91);
            lblPPolZ.Name = "lblPPolZ";
            lblPPolZ.Size = new Size(0x53, 0x10);
            lblPPolZ.TabIndex = 0xab;
            lblPPolZ.Text = "Direction Z";
            lblPPolY.AutoSize = true;
            lblPPolY.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPPolY.ForeColor = Color.White;
            lblPPolY.Location = new Point(0x60, 0x69);
            lblPPolY.Name = "lblPPolY";
            lblPPolY.Size = new Size(0x54, 0x10);
            lblPPolY.TabIndex = 170;
            lblPPolY.Text = "Direction Y";
            lblPPolX.AutoSize = true;
            lblPPolX.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPPolX.ForeColor = Color.White;
            lblPPolX.Location = new Point(0x60, 0x45);
            lblPPolX.Name = "lblPPolX";
            lblPPolX.Size = new Size(0x53, 0x10);
            lblPPolX.TabIndex = 0xa9;
            lblPPolX.Text = "Direction X";
            label255.AutoSize = true;
            label255.Font = new Font("Microsoft Sans Serif", 15.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label255.ForeColor = Color.White;
            label255.Location = new Point(3, 5);
            label255.Name = "label255";
            label255.Size = new Size(0x7e, 0x19);
            label255.TabIndex = 0xa8;
            label255.Text = "Tap Status";
            label6.AutoSize = true;
            label6.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label6.ForeColor = Color.White;
            label6.Location = new Point(6, 0xb1);
            label6.Name = "label6";
            label6.Size = new Size(0x72, 0x10);
            label6.TabIndex = 0xa5;
            label6.Text = "Event Detected";
            ledTapEA.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            ledTapEA.ForeColor = Color.White;
            ledTapEA.LedStyle = LedStyle.Round3D;
            ledTapEA.Location = new Point(0x7e, 0xa5);
            ledTapEA.Name = "ledTapEA";
            ledTapEA.OffColor = Color.Red;
            ledTapEA.Size = new Size(40, 40);
            ledTapEA.TabIndex = 0xa4;
            label8.AutoSize = true;
            label8.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label8.ForeColor = Color.White;
            label8.Location = new Point(6, 0x8f);
            label8.Name = "label8";
            label8.Size = new Size(0x33, 0x10);
            label8.TabIndex = 0xa7;
            label8.Text = "Z-Axis";
            ledPZ.BlinkMode = LedBlinkMode.BlinkWhenOn;
            ledPZ.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            ledPZ.ForeColor = Color.White;
            ledPZ.LedStyle = LedStyle.Round3D;
            ledPZ.Location = new Point(0x37, 0x85);
            ledPZ.Name = "ledPZ";
            ledPZ.OffColor = Color.Red;
            ledPZ.Size = new Size(40, 40);
            ledPZ.TabIndex = 0xa6;
            ledPX.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            ledPX.ForeColor = Color.White;
            ledPX.LedStyle = LedStyle.Round3D;
            ledPX.Location = new Point(0x37, 0x3b);
            ledPX.Name = "ledPX";
            ledPX.OffColor = Color.Red;
            ledPX.Size = new Size(40, 40);
            ledPX.TabIndex = 0x77;
            label9.AutoSize = true;
            label9.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label9.ForeColor = Color.White;
            label9.Location = new Point(6, 110);
            label9.Name = "label9";
            label9.Size = new Size(0x34, 0x10);
            label9.TabIndex = 0xa5;
            label9.Text = "Y-Axis";
            label10.AutoSize = true;
            label10.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label10.ForeColor = Color.White;
            label10.Location = new Point(6, 0x48);
            label10.Name = "label10";
            label10.Size = new Size(0x33, 0x10);
            label10.TabIndex = 0xa3;
            label10.Text = "X-Axis";
            ledPY.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            ledPY.ForeColor = Color.White;
            ledPY.LedStyle = LedStyle.Round3D;
            ledPY.Location = new Point(0x37, 0x61);
            ledPY.Name = "ledPY";
            ledPY.OffColor = Color.Red;
            ledPY.Size = new Size(40, 40);
            ledPY.TabIndex = 0xa4;
            label5.AutoSize = true;
            label5.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.Location = new Point(0x16a, 0x12a);
            label5.Name = "label5";
            label5.Size = new Size(0xa5, 13);
            label5.TabIndex = 0xfd;
            label5.Text = "MCU Sleep Current = 0.5mA";
            label3.AutoSize = true;
            label3.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(0x16a, 0x117);
            label3.Name = "label3";
            label3.Size = new Size(0xa2, 13);
            label3.TabIndex = 0xfc;
            label3.Text = "MCU Wake Current = 12mA";
            label2.AutoSize = true;
            label2.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(0xcc, 0x11b);
            label2.Name = "label2";
            label2.Size = new Size(0x6a, 20);
            label2.TabIndex = 0xfb;
            label2.Text = "MCU Status";
            txtCurrent.BackColor = Color.LightSteelBlue;
            txtCurrent.Font = new Font("Verdana", 20f, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtCurrent.ForeColor = Color.ForestGreen;
            txtCurrent.Location = new Point(8, 0x13d);
            txtCurrent.Multiline = true;
            txtCurrent.Name = "txtCurrent";
            txtCurrent.ReadOnly = true;
            txtCurrent.RightToLeft = System.Windows.Forms.RightToLeft.No;
            txtCurrent.Size = new Size(0xa3, 40);
            txtCurrent.TabIndex = 250;
            txtCurrent.Text = "0.725 mA";
            txtCurrent.TextAlign = HorizontalAlignment.Center;
            lblCurrentMCU.AutoSize = true;
            lblCurrentMCU.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblCurrentMCU.Location = new Point(5, 0x12a);
            lblCurrentMCU.Name = "lblCurrentMCU";
            lblCurrentMCU.Size = new Size(0xb9, 0x10);
            lblCurrentMCU.TabIndex = 0xf9;
            lblCurrentMCU.Text = "Estimated System Current";
            ledMCU1.ForeColor = Color.White;
            ledMCU1.LedStyle = LedStyle.Round3D;
            ledMCU1.Location = new Point(0x131, 0x112);
            ledMCU1.Name = "ledMCU1";
            ledMCU1.OffColor = Color.Red;
            ledMCU1.Size = new Size(40, 40);
            ledMCU1.TabIndex = 0xf8;
            legend1.ItemLayoutMode = LegendItemLayoutMode.LeftToRight;
            legend1.Items.AddRange(new LegendItem[] { legendItem1, legendItem2, legendItem3 });
            legend1.Location = new Point(0xe2, 0xf3);
            legend1.Name = "legend1";
            legend1.Size = new Size(0xfe, 0x25);
            legend1.TabIndex = 0xef;
            legendItem1.Source = MainScreenXAxis;
            legendItem1.Text = "X-Axis";
            MainScreenXAxis.LineColor = Color.Red;
            MainScreenXAxis.XAxis = xAxis3;
            MainScreenXAxis.YAxis = yAxis3;
            xAxis3.Caption = "Samples";
            xAxis3.MajorDivisions.GridVisible = true;
            xAxis3.Mode = AxisMode.Fixed;
            xAxis3.Range = new NationalInstruments.UI.Range(0.0, 31.0);
            yAxis3.Caption = "Acceleration (g)";
            yAxis3.MajorDivisions.GridColor = Color.Silver;
            yAxis3.MajorDivisions.GridVisible = true;
            yAxis3.Range = new NationalInstruments.UI.Range(-8.0, 8.0);
            legendItem2.Source = MainScreenYAxis;
            legendItem2.Text = "Y-Axis";
            MainScreenYAxis.LineColor = Color.Blue;
            MainScreenYAxis.PointColor = Color.LimeGreen;
            MainScreenYAxis.XAxis = xAxis3;
            MainScreenYAxis.YAxis = yAxis3;
            legendItem3.Source = MainScreenZAxis;
            legendItem3.Text = "Z-Axis";
            MainScreenZAxis.LineColor = Color.White;
            MainScreenZAxis.XAxis = xAxis3;
            MainScreenZAxis.YAxis = yAxis3;
            pltFIFOPlot.BackColor = Color.Gray;
            pltFIFOPlot.Caption = "FIFO Data";
            pltFIFOPlot.CaptionBackColor = Color.Silver;
            pltFIFOPlot.ForeColor = Color.White;
            pltFIFOPlot.ImmediateUpdates = true;
            pltFIFOPlot.InteractionModeDefault = GraphDefaultInteractionMode.ZoomX;
            pltFIFOPlot.Location = new Point(0xcb, 5);
            pltFIFOPlot.Name = "pltFIFOPlot";
            pltFIFOPlot.PlotAreaBorder = Border.Raised;
            pltFIFOPlot.Plots.AddRange(new WaveformPlot[] { MainScreenXAxis, MainScreenYAxis, MainScreenZAxis });
            pltFIFOPlot.Size = new Size(0x148, 0xe8);
            pltFIFOPlot.TabIndex = 0xeb;
            pltFIFOPlot.XAxes.AddRange(new XAxis[] { xAxis3 });
            pltFIFOPlot.YAxes.AddRange(new YAxis[] { yAxis3 });
            groupBox1.Controls.Add(textBox2);
            groupBox1.Font = new Font("Microsoft Sans Serif", 9.5f, FontStyle.Bold, GraphicsUnit.Point, 0);
            groupBox1.Location = new Point(3, 0xd4);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(0xc1, 0x51);
            groupBox1.TabIndex = 0xa8;
            groupBox1.TabStop = false;
            groupBox1.Text = "FIFO Algorithm Direction";
            textBox2.BackColor = Color.LightSteelBlue;
            textBox2.Font = new Font("Verdana", 24f, FontStyle.Regular, GraphicsUnit.Point, 0);
            textBox2.ForeColor = Color.ForestGreen;
            textBox2.Location = new Point(0x11, 0x17);
            textBox2.Multiline = true;
            textBox2.Name = "textBox2";
            textBox2.ReadOnly = true;
            textBox2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            textBox2.Size = new Size(0x97, 0x2d);
            textBox2.TabIndex = 0xa7;
            textBox2.TextAlign = HorizontalAlignment.Center;
            panel1.BorderStyle = BorderStyle.Fixed3D;
            panel1.Controls.Add(AccelControllerGroup);
            panel1.Controls.Add(rdoDefaultSP);
            panel1.Controls.Add(panel15);
            panel1.Controls.Add(rdoDefaultSPDP);
            panel1.Location = new Point(0x223, 3);
            panel1.Name = "panel1";
            panel1.Size = new Size(0x195, 0x16e);
            panel1.TabIndex = 0x4d;
            AccelControllerGroup.Controls.Add(gbDR);
            AccelControllerGroup.Controls.Add(gbOM);
            AccelControllerGroup.Controls.Add(rdoStandby);
            AccelControllerGroup.Controls.Add(label173);
            AccelControllerGroup.Controls.Add(label71);
            AccelControllerGroup.Controls.Add(lbsleep);
            AccelControllerGroup.Controls.Add(ledStandby);
            AccelControllerGroup.Controls.Add(ledSleep);
            AccelControllerGroup.Controls.Add(ledWake);
            AccelControllerGroup.Controls.Add(rdoActive);
            AccelControllerGroup.Location = new Point(6, 3);
            AccelControllerGroup.Name = "AccelControllerGroup";
            AccelControllerGroup.Size = new Size(0x182, 300);
            AccelControllerGroup.TabIndex = 0xd5;
            AccelControllerGroup.TabStop = false;
            gbDR.BackColor = Color.LightSlateGray;
            gbDR.Controls.Add(lbl4gSensitivity);
            gbDR.Controls.Add(rdo2g);
            gbDR.Controls.Add(rdo8g);
            gbDR.Controls.Add(rdo4g);
            gbDR.Controls.Add(lbl2gSensitivity);
            gbDR.Controls.Add(lbl8gSensitivity);
            gbDR.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            gbDR.ForeColor = Color.White;
            gbDR.Location = new Point(0xb3, 0x13);
            gbDR.Name = "gbDR";
            gbDR.Size = new Size(0xc6, 0x62);
            gbDR.TabIndex = 0xc4;
            gbDR.TabStop = false;
            gbDR.Text = "Dynamic Range";
            lbl4gSensitivity.AutoSize = true;
            lbl4gSensitivity.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbl4gSensitivity.ForeColor = Color.White;
            lbl4gSensitivity.Location = new Point(0x3a, 0x34);
            lbl4gSensitivity.Name = "lbl4gSensitivity";
            lbl4gSensitivity.Size = new Size(0x84, 0x10);
            lbl4gSensitivity.TabIndex = 0x77;
            lbl4gSensitivity.Text = "2048 counts/g 14b";
            rdo2g.AutoSize = true;
            rdo2g.Checked = true;
            rdo2g.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            rdo2g.ForeColor = Color.White;
            rdo2g.Location = new Point(15, 0x17);
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
            rdo8g.Location = new Point(15, 0x4b);
            rdo8g.Name = "rdo8g";
            rdo8g.Size = new Size(0x2b, 20);
            rdo8g.TabIndex = 0x74;
            rdo8g.Text = "8g";
            rdo8g.UseVisualStyleBackColor = true;
            rdo8g.CheckedChanged += new EventHandler(rdo8g_CheckedChanged);
            rdo4g.AutoSize = true;
            rdo4g.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            rdo4g.ForeColor = Color.White;
            rdo4g.Location = new Point(15, 0x31);
            rdo4g.Name = "rdo4g";
            rdo4g.Size = new Size(0x2b, 20);
            rdo4g.TabIndex = 0x75;
            rdo4g.Text = "4g";
            rdo4g.UseVisualStyleBackColor = true;
            rdo4g.CheckedChanged += new EventHandler(rdo4g_CheckedChanged);
            lbl2gSensitivity.AutoSize = true;
            lbl2gSensitivity.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbl2gSensitivity.ForeColor = Color.White;
            lbl2gSensitivity.Location = new Point(0x3b, 0x19);
            lbl2gSensitivity.Name = "lbl2gSensitivity";
            lbl2gSensitivity.Size = new Size(0x84, 0x10);
            lbl2gSensitivity.TabIndex = 0x76;
            lbl2gSensitivity.Text = "4096 counts/g 14b";
            lbl8gSensitivity.AutoSize = true;
            lbl8gSensitivity.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbl8gSensitivity.ForeColor = Color.White;
            lbl8gSensitivity.Location = new Point(0x3a, 0x4d);
            lbl8gSensitivity.Name = "lbl8gSensitivity";
            lbl8gSensitivity.Size = new Size(0x84, 0x10);
            lbl8gSensitivity.TabIndex = 120;
            lbl8gSensitivity.Text = "1024 counts/g 14b";
            gbOM.BackColor = Color.LightSlateGray;
            gbOM.Controls.Add(btnAutoCal);
            gbOM.Controls.Add(chkAnalogLowNoise);
            gbOM.Controls.Add(pOverSampling);
            gbOM.Controls.Add(p1);
            gbOM.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            gbOM.ForeColor = Color.White;
            gbOM.Location = new Point(6, 0x7d);
            gbOM.Name = "gbOM";
            gbOM.Size = new Size(0x176, 0xa4);
            gbOM.TabIndex = 0xc3;
            gbOM.TabStop = false;
            gbOM.Text = "Operation Mode";
            btnAutoCal.BackColor = Color.LightSlateGray;
            btnAutoCal.FlatAppearance.BorderColor = Color.Fuchsia;
            btnAutoCal.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnAutoCal.ForeColor = Color.White;
            btnAutoCal.Location = new Point(0xd4, 0x15);
            btnAutoCal.Name = "btnAutoCal";
            btnAutoCal.Size = new Size(0x60, 0x2c);
            btnAutoCal.TabIndex = 0xc5;
            btnAutoCal.Text = "Auto Calibrate";
            btnAutoCal.UseVisualStyleBackColor = false;
            btnAutoCal.Click += new EventHandler(btnAutoCal_Click);
            chkAnalogLowNoise.AutoSize = true;
            chkAnalogLowNoise.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            chkAnalogLowNoise.ForeColor = Color.White;
            chkAnalogLowNoise.Location = new Point(6, 60);
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
            pOverSampling.Location = new Point(6, 80);
            pOverSampling.Name = "pOverSampling";
            pOverSampling.Size = new Size(0x12e, 0x48);
            pOverSampling.TabIndex = 0xdd;
            label70.AutoSize = true;
            label70.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label70.Location = new Point(4, 5);
            label70.Name = "label70";
            label70.Size = new Size(220, 0x10);
            label70.TabIndex = 0xde;
            label70.Text = "Oversampling Options for Data";
            rdoOSHiResMode.AutoSize = true;
            rdoOSHiResMode.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
            rdoOSHiResMode.Location = new Point(5, 0x30);
            rdoOSHiResMode.Name = "rdoOSHiResMode";
            rdoOSHiResMode.Size = new Size(0x6c, 0x13);
            rdoOSHiResMode.TabIndex = 0xe0;
            rdoOSHiResMode.Text = "Hi Res Mode";
            rdoOSHiResMode.UseVisualStyleBackColor = true;
            rdoOSHiResMode.CheckedChanged += new EventHandler(rdoOSHiResMode_CheckedChanged);
            rdoOSLPMode.AutoSize = true;
            rdoOSLPMode.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
            rdoOSLPMode.Location = new Point(0x81, 0x1c);
            rdoOSLPMode.Name = "rdoOSLPMode";
            rdoOSLPMode.Size = new Size(0x87, 0x13);
            rdoOSLPMode.TabIndex = 0xdf;
            rdoOSLPMode.Text = "Low Power Mode";
            rdoOSLPMode.UseVisualStyleBackColor = true;
            rdoOSLPMode.CheckedChanged += new EventHandler(rdoOSLPMode_CheckedChanged);
            rdoOSLNLPMode.AutoSize = true;
            rdoOSLNLPMode.Checked = true;
            rdoOSLNLPMode.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
            rdoOSLNLPMode.Location = new Point(0x81, 0x2f);
            rdoOSLNLPMode.Name = "rdoOSLNLPMode";
            rdoOSLNLPMode.Size = new Size(0xa6, 0x13);
            rdoOSLNLPMode.TabIndex = 0xdd;
            rdoOSLNLPMode.TabStop = true;
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
            p1.Location = new Point(3, 0x15);
            p1.Name = "p1";
            p1.Size = new Size(0xba, 0x21);
            p1.TabIndex = 0x91;
            ddlDataRate.DisplayMember = "(none)";
            ddlDataRate.DropDownStyle = ComboBoxStyle.DropDownList;
            ddlDataRate.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            ddlDataRate.FormattingEnabled = true;
            ddlDataRate.Items.AddRange(new object[] { "800Hz", "400 Hz", "200 Hz", "100 Hz", "50 Hz", "12.5 Hz", "6.25 Hz", "1.563 Hz" });
            ddlDataRate.Location = new Point(0x6b, 6);
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
            rdoStandby.AutoSize = true;
            rdoStandby.Checked = true;
            rdoStandby.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            rdoStandby.Location = new Point(6, 0x13);
            rdoStandby.Name = "rdoStandby";
            rdoStandby.Size = new Size(0x57, 20);
            rdoStandby.TabIndex = 0x70;
            rdoStandby.TabStop = true;
            rdoStandby.Text = "Standby ";
            rdoStandby.UseVisualStyleBackColor = true;
            rdoStandby.CheckedChanged += new EventHandler(rdoStandby_CheckedChanged);
            label173.AutoSize = true;
            label173.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label173.Location = new Point(10, 0x2e);
            label173.Name = "label173";
            label173.Size = new Size(0x6c, 0x10);
            label173.TabIndex = 0xc2;
            label173.Text = "Standby Mode";
            label71.AutoSize = true;
            label71.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label71.Location = new Point(0x1b, 0x49);
            label71.Name = "label71";
            label71.Size = new Size(0x5b, 0x10);
            label71.TabIndex = 0xc0;
            label71.Text = "Wake Mode";
            lbsleep.AutoSize = true;
            lbsleep.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbsleep.Location = new Point(0x1a, 0x65);
            lbsleep.Name = "lbsleep";
            lbsleep.Size = new Size(0x5c, 0x10);
            lbsleep.TabIndex = 0xb9;
            lbsleep.Text = "Sleep Mode";
            ledStandby.LedStyle = LedStyle.Round3D;
            ledStandby.Location = new Point(120, 0x27);
            ledStandby.Name = "ledStandby";
            ledStandby.OffColor = Color.Red;
            ledStandby.Size = new Size(30, 0x1f);
            ledStandby.TabIndex = 0xc1;
            ledStandby.Value = true;
            ledSleep.LedStyle = LedStyle.Round3D;
            ledSleep.Location = new Point(120, 0x5e);
            ledSleep.Name = "ledSleep";
            ledSleep.OffColor = Color.Red;
            ledSleep.Size = new Size(30, 0x1f);
            ledSleep.TabIndex = 0x91;
            ledWake.LedStyle = LedStyle.Round3D;
            ledWake.Location = new Point(120, 0x44);
            ledWake.Name = "ledWake";
            ledWake.OffColor = Color.Red;
            ledWake.Size = new Size(30, 0x1f);
            ledWake.TabIndex = 0xbf;
            rdoActive.AutoSize = true;
            rdoActive.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            rdoActive.Location = new Point(0x63, 0x13);
            rdoActive.Name = "rdoActive";
            rdoActive.Size = new Size(0x45, 20);
            rdoActive.TabIndex = 0x71;
            rdoActive.Text = "Active";
            rdoActive.UseVisualStyleBackColor = true;
            rdoActive.CheckedChanged += new EventHandler(rdoActive_CheckedChanged);
            rdoDefaultSP.AutoSize = true;
            rdoDefaultSP.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            rdoDefaultSP.ForeColor = Color.Gold;
            rdoDefaultSP.Location = new Point(6, 0x134);
            rdoDefaultSP.Name = "rdoDefaultSP";
            rdoDefaultSP.Size = new Size(0x9b, 20);
            rdoDefaultSP.TabIndex = 0xec;
            rdoDefaultSP.TabStop = true;
            rdoDefaultSP.Text = "Default Single Tap";
            rdoDefaultSP.UseVisualStyleBackColor = true;
            rdoDefaultSP.CheckedChanged += new EventHandler(rdoDefaultSP_CheckedChanged);
            panel15.Controls.Add(chkPulseLPFEnable);
            panel15.Controls.Add(chkPulseHPFBypass);
            panel15.Location = new Point(0x13, 0x148);
            panel15.Name = "panel15";
            panel15.Size = new Size(0x15c, 0x1f);
            panel15.TabIndex = 0xee;
            chkPulseLPFEnable.AutoSize = true;
            chkPulseLPFEnable.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            chkPulseLPFEnable.ForeColor = Color.White;
            chkPulseLPFEnable.Location = new Point(0x15, 10);
            chkPulseLPFEnable.Name = "chkPulseLPFEnable";
            chkPulseLPFEnable.Size = new Size(0x5b, 0x11);
            chkPulseLPFEnable.TabIndex = 0xea;
            chkPulseLPFEnable.Text = "LPF Enable";
            chkPulseLPFEnable.UseVisualStyleBackColor = true;
            chkPulseLPFEnable.CheckedChanged += new EventHandler(chkPulseLPFEnable_CheckedChanged);
            chkPulseHPFBypass.AutoSize = true;
            chkPulseHPFBypass.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            chkPulseHPFBypass.ForeColor = Color.White;
            chkPulseHPFBypass.Location = new Point(0xcd, 10);
            chkPulseHPFBypass.Name = "chkPulseHPFBypass";
            chkPulseHPFBypass.Size = new Size(0x5e, 0x11);
            chkPulseHPFBypass.TabIndex = 0xe9;
            chkPulseHPFBypass.Text = "HPF Bypass";
            chkPulseHPFBypass.UseVisualStyleBackColor = true;
            chkPulseHPFBypass.CheckedChanged += new EventHandler(chkPulseHPFBypass_CheckedChanged);
            rdoDefaultSPDP.AutoSize = true;
            rdoDefaultSPDP.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            rdoDefaultSPDP.ForeColor = Color.Gold;
            rdoDefaultSPDP.Location = new Point(0xa3, 0x134);
            rdoDefaultSPDP.Name = "rdoDefaultSPDP";
            rdoDefaultSPDP.Size = new Size(0xdd, 20);
            rdoDefaultSPDP.TabIndex = 0xed;
            rdoDefaultSPDP.TabStop = true;
            rdoDefaultSPDP.Text = "Default Single + Double Tap";
            rdoDefaultSPDP.UseVisualStyleBackColor = true;
            rdoDefaultSPDP.CheckedChanged += new EventHandler(rdoDefaultSPDP_CheckedChanged);
            panelAdvanced.BorderStyle = BorderStyle.Fixed3D;
            panelAdvanced.Controls.Add(btnResetPulseThresholds);
            panelAdvanced.Controls.Add(p12);
            panelAdvanced.Controls.Add(btnSetPulseThresholds);
            panelAdvanced.Controls.Add(tbPulseZThreshold);
            panelAdvanced.Controls.Add(tbPulseXThreshold);
            panelAdvanced.Controls.Add(lblPulseYThreshold);
            panelAdvanced.Controls.Add(tbPulseYThreshold);
            panelAdvanced.Controls.Add(lblPulseYThresholdVal);
            panelAdvanced.Controls.Add(lblPulseXThresholdg);
            panelAdvanced.Controls.Add(lblPulseXThresholdVal);
            panelAdvanced.Controls.Add(lblPulseZThresholdg);
            panelAdvanced.Controls.Add(lblPulseZThreshold);
            panelAdvanced.Controls.Add(lblPulseZThresholdVal);
            panelAdvanced.Controls.Add(lblPulseYThresholdg);
            panelAdvanced.Controls.Add(lblPulseXThreshold);
            panelAdvanced.Controls.Add(p10);
            panelAdvanced.Location = new Point(3, 0x177);
            panelAdvanced.Name = "panelAdvanced";
            panelAdvanced.Size = new Size(0x3b5, 0x116);
            panelAdvanced.TabIndex = 0x4c;
            btnResetPulseThresholds.BackColor = Color.LightSlateGray;
            btnResetPulseThresholds.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnResetPulseThresholds.ForeColor = Color.White;
            btnResetPulseThresholds.Location = new Point(0x2ae, 0xb8);
            btnResetPulseThresholds.Name = "btnResetPulseThresholds";
            btnResetPulseThresholds.Size = new Size(0xb0, 0x1f);
            btnResetPulseThresholds.TabIndex = 240;
            btnResetPulseThresholds.Text = "Reset XYZ Thresholds";
            btnResetPulseThresholds.UseVisualStyleBackColor = false;
            btnResetPulseThresholds.Click += new EventHandler(btnResetPulseThresholds_Click);
            p12.BackColor = Color.LightSlateGray;
            p12.Controls.Add(btnPulseResetTime2ndPulse);
            p12.Controls.Add(btnPulseSetTime2ndPulse);
            p12.Controls.Add(tbPulseLatency);
            p12.Controls.Add(chkPulseEnableXDP);
            p12.Controls.Add(chkPulseEnableYDP);
            p12.Controls.Add(chkPulseEnableZDP);
            p12.Controls.Add(lblPulse2ndPulseWinms);
            p12.Controls.Add(lblPulseLatency);
            p12.Controls.Add(lblPulse2ndPulseWinVal);
            p12.Controls.Add(lblPulseLatencyVal);
            p12.Controls.Add(lblPulse2ndPulseWin);
            p12.Controls.Add(lblPulseLatencyms);
            p12.Controls.Add(tbPulse2ndPulseWin);
            p12.Controls.Add(chkPulseIgnorLatentPulses);
            p12.Location = new Point(3, 0x7a);
            p12.Name = "p12";
            p12.Size = new Size(0x1bd, 0x97);
            p12.TabIndex = 0xe3;
            btnPulseResetTime2ndPulse.BackColor = Color.LightSlateGray;
            btnPulseResetTime2ndPulse.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnPulseResetTime2ndPulse.ForeColor = Color.White;
            btnPulseResetTime2ndPulse.Location = new Point(0xc9, 0x6f);
            btnPulseResetTime2ndPulse.Name = "btnPulseResetTime2ndPulse";
            btnPulseResetTime2ndPulse.Size = new Size(150, 0x1f);
            btnPulseResetTime2ndPulse.TabIndex = 0x9f;
            btnPulseResetTime2ndPulse.Text = "Reset Time Limits";
            btnPulseResetTime2ndPulse.UseVisualStyleBackColor = false;
            btnPulseResetTime2ndPulse.Click += new EventHandler(btnPulseResetTime2ndPulse_Click);
            btnPulseSetTime2ndPulse.BackColor = Color.LightSlateGray;
            btnPulseSetTime2ndPulse.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnPulseSetTime2ndPulse.ForeColor = Color.White;
            btnPulseSetTime2ndPulse.Location = new Point(0x2a, 0x6f);
            btnPulseSetTime2ndPulse.Name = "btnPulseSetTime2ndPulse";
            btnPulseSetTime2ndPulse.Size = new Size(0x7b, 0x1f);
            btnPulseSetTime2ndPulse.TabIndex = 0x9e;
            btnPulseSetTime2ndPulse.Text = "Set Time Limits";
            btnPulseSetTime2ndPulse.UseVisualStyleBackColor = false;
            btnPulseSetTime2ndPulse.Click += new EventHandler(btnPulseSetTime2ndPulse_Click);
            tbPulseLatency.Location = new Point(0xbc, 0x1c);
            tbPulseLatency.Maximum = 0xff;
            tbPulseLatency.Name = "tbPulseLatency";
            tbPulseLatency.Size = new Size(0xdf, 0x2d);
            tbPulseLatency.TabIndex = 0x95;
            tbPulseLatency.Scroll += new EventHandler(tbPulseLatency_Scroll);
            chkPulseEnableXDP.AutoSize = true;
            chkPulseEnableXDP.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
            chkPulseEnableXDP.ForeColor = Color.White;
            chkPulseEnableXDP.Location = new Point(6, 3);
            chkPulseEnableXDP.Name = "chkPulseEnableXDP";
            chkPulseEnableXDP.Size = new Size(0x60, 0x13);
            chkPulseEnableXDP.TabIndex = 0x7c;
            chkPulseEnableXDP.Text = "Enable X DP";
            chkPulseEnableXDP.UseVisualStyleBackColor = true;
            chkPulseEnableXDP.CheckedChanged += new EventHandler(chkPulseEnableXDP_CheckedChanged);
            chkPulseEnableYDP.AutoSize = true;
            chkPulseEnableYDP.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
            chkPulseEnableYDP.ForeColor = Color.White;
            chkPulseEnableYDP.Location = new Point(0x66, 3);
            chkPulseEnableYDP.Name = "chkPulseEnableYDP";
            chkPulseEnableYDP.Size = new Size(0x5f, 0x13);
            chkPulseEnableYDP.TabIndex = 0x7d;
            chkPulseEnableYDP.Text = "Enable Y DP";
            chkPulseEnableYDP.UseVisualStyleBackColor = true;
            chkPulseEnableYDP.CheckedChanged += new EventHandler(chkPulseEnableYDP_CheckedChanged);
            chkPulseEnableZDP.AutoSize = true;
            chkPulseEnableZDP.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
            chkPulseEnableZDP.ForeColor = Color.White;
            chkPulseEnableZDP.Location = new Point(0xc9, 4);
            chkPulseEnableZDP.Name = "chkPulseEnableZDP";
            chkPulseEnableZDP.Size = new Size(0x5f, 0x13);
            chkPulseEnableZDP.TabIndex = 0x7e;
            chkPulseEnableZDP.Text = "Enable Z DP";
            chkPulseEnableZDP.UseVisualStyleBackColor = true;
            chkPulseEnableZDP.CheckedChanged += new EventHandler(chkPulseEnableZDP_CheckedChanged);
            lblPulse2ndPulseWinms.AutoSize = true;
            lblPulse2ndPulseWinms.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblPulse2ndPulseWinms.ForeColor = Color.White;
            lblPulse2ndPulseWinms.Location = new Point(0xa3, 0x4e);
            lblPulse2ndPulseWinms.Name = "lblPulse2ndPulseWinms";
            lblPulse2ndPulseWinms.Size = new Size(0x18, 15);
            lblPulse2ndPulseWinms.TabIndex = 0x9d;
            lblPulse2ndPulseWinms.Text = "ms";
            lblPulseLatency.AutoSize = true;
            lblPulseLatency.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblPulseLatency.ForeColor = Color.White;
            lblPulseLatency.Location = new Point(6, 0x20);
            lblPulseLatency.Name = "lblPulseLatency";
            lblPulseLatency.Size = new Size(0x53, 15);
            lblPulseLatency.TabIndex = 150;
            lblPulseLatency.Text = "Pulse Latency";
            lblPulse2ndPulseWinVal.AutoSize = true;
            lblPulse2ndPulseWinVal.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblPulse2ndPulseWinVal.ForeColor = Color.White;
            lblPulse2ndPulseWinVal.Location = new Point(0x76, 0x4f);
            lblPulse2ndPulseWinVal.Name = "lblPulse2ndPulseWinVal";
            lblPulse2ndPulseWinVal.Size = new Size(14, 15);
            lblPulse2ndPulseWinVal.TabIndex = 0x9c;
            lblPulse2ndPulseWinVal.Text = "0";
            lblPulseLatencyVal.AutoSize = true;
            lblPulseLatencyVal.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblPulseLatencyVal.ForeColor = Color.White;
            lblPulseLatencyVal.Location = new Point(0x76, 0x20);
            lblPulseLatencyVal.Name = "lblPulseLatencyVal";
            lblPulseLatencyVal.Size = new Size(14, 15);
            lblPulseLatencyVal.TabIndex = 0x97;
            lblPulseLatencyVal.Text = "0";
            lblPulse2ndPulseWin.AutoSize = true;
            lblPulse2ndPulseWin.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblPulse2ndPulseWin.ForeColor = Color.White;
            lblPulse2ndPulseWin.Location = new Point(6, 0x4f);
            lblPulse2ndPulseWin.Name = "lblPulse2ndPulseWin";
            lblPulse2ndPulseWin.Size = new Size(0x6d, 15);
            lblPulse2ndPulseWin.TabIndex = 0x9b;
            lblPulse2ndPulseWin.Text = "2nd Pulse Window";
            lblPulseLatencyms.AutoSize = true;
            lblPulseLatencyms.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblPulseLatencyms.ForeColor = Color.White;
            lblPulseLatencyms.Location = new Point(0xa8, 0x1d);
            lblPulseLatencyms.Name = "lblPulseLatencyms";
            lblPulseLatencyms.Size = new Size(0x18, 15);
            lblPulseLatencyms.TabIndex = 0x98;
            lblPulseLatencyms.Text = "ms";
            tbPulse2ndPulseWin.Location = new Point(0xbc, 0x48);
            tbPulse2ndPulseWin.Maximum = 0xff;
            tbPulse2ndPulseWin.Name = "tbPulse2ndPulseWin";
            tbPulse2ndPulseWin.Size = new Size(0xdf, 0x2d);
            tbPulse2ndPulseWin.TabIndex = 0x9a;
            tbPulse2ndPulseWin.Scroll += new EventHandler(tbPulse2ndPulseWin_Scroll);
            chkPulseIgnorLatentPulses.AutoSize = true;
            chkPulseIgnorLatentPulses.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
            chkPulseIgnorLatentPulses.ForeColor = Color.White;
            chkPulseIgnorLatentPulses.Location = new Point(0x12e, 4);
            chkPulseIgnorLatentPulses.Name = "chkPulseIgnorLatentPulses";
            chkPulseIgnorLatentPulses.Size = new Size(0x8a, 0x13);
            chkPulseIgnorLatentPulses.TabIndex = 0x99;
            chkPulseIgnorLatentPulses.Text = "Ignore Latent Pulses";
            chkPulseIgnorLatentPulses.UseVisualStyleBackColor = true;
            chkPulseIgnorLatentPulses.CheckedChanged += new EventHandler(chkPulseIgnorLatentPulses_CheckedChanged);
            btnSetPulseThresholds.BackColor = Color.LightSlateGray;
            btnSetPulseThresholds.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSetPulseThresholds.ForeColor = Color.White;
            btnSetPulseThresholds.Location = new Point(0x1f8, 0xb8);
            btnSetPulseThresholds.Name = "btnSetPulseThresholds";
            btnSetPulseThresholds.Size = new Size(0x9b, 0x1f);
            btnSetPulseThresholds.TabIndex = 0xef;
            btnSetPulseThresholds.Text = "Set XYZ Thresholds";
            btnSetPulseThresholds.UseVisualStyleBackColor = false;
            btnSetPulseThresholds.Click += new EventHandler(btnSetPulseThresholds_Click);
            tbPulseZThreshold.BackColor = Color.LightSlateGray;
            tbPulseZThreshold.Location = new Point(620, 0x94);
            tbPulseZThreshold.Maximum = 0x7f;
            tbPulseZThreshold.Name = "tbPulseZThreshold";
            tbPulseZThreshold.Size = new Size(0x112, 0x2d);
            tbPulseZThreshold.TabIndex = 0xdf;
            tbPulseZThreshold.Scroll += new EventHandler(tbPulseZThreshold_Scroll);
            tbPulseXThreshold.BackColor = Color.LightSlateGray;
            tbPulseXThreshold.Location = new Point(620, 70);
            tbPulseXThreshold.Maximum = 0x7f;
            tbPulseXThreshold.Name = "tbPulseXThreshold";
            tbPulseXThreshold.Size = new Size(0x112, 0x2d);
            tbPulseXThreshold.TabIndex = 0xd7;
            tbPulseXThreshold.Scroll += new EventHandler(tbPulseXThreshold_Scroll);
            lblPulseYThreshold.AutoSize = true;
            lblPulseYThreshold.BackColor = Color.LightSlateGray;
            lblPulseYThreshold.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPulseYThreshold.ForeColor = Color.White;
            lblPulseYThreshold.Location = new Point(0x1da, 0x75);
            lblPulseYThreshold.Name = "lblPulseYThreshold";
            lblPulseYThreshold.Size = new Size(0x57, 15);
            lblPulseYThreshold.TabIndex = 220;
            lblPulseYThreshold.Text = " Y Threshold";
            tbPulseYThreshold.BackColor = Color.LightSlateGray;
            tbPulseYThreshold.Location = new Point(620, 110);
            tbPulseYThreshold.Maximum = 0x7f;
            tbPulseYThreshold.Name = "tbPulseYThreshold";
            tbPulseYThreshold.Size = new Size(0x112, 0x2d);
            tbPulseYThreshold.TabIndex = 0xdb;
            tbPulseYThreshold.Scroll += new EventHandler(tbPulseYThreshold_Scroll);
            lblPulseYThresholdVal.AutoSize = true;
            lblPulseYThresholdVal.BackColor = Color.LightSlateGray;
            lblPulseYThresholdVal.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPulseYThresholdVal.ForeColor = Color.White;
            lblPulseYThresholdVal.Location = new Point(0x237, 0x75);
            lblPulseYThresholdVal.Name = "lblPulseYThresholdVal";
            lblPulseYThresholdVal.Size = new Size(15, 15);
            lblPulseYThresholdVal.TabIndex = 0xdd;
            lblPulseYThresholdVal.Text = "0";
            lblPulseXThresholdg.AutoSize = true;
            lblPulseXThresholdg.BackColor = Color.LightSlateGray;
            lblPulseXThresholdg.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPulseXThresholdg.ForeColor = Color.White;
            lblPulseXThresholdg.Location = new Point(610, 0x4d);
            lblPulseXThresholdg.Name = "lblPulseXThresholdg";
            lblPulseXThresholdg.Size = new Size(15, 15);
            lblPulseXThresholdg.TabIndex = 0xda;
            lblPulseXThresholdg.Text = "g";
            lblPulseXThresholdVal.AutoSize = true;
            lblPulseXThresholdVal.BackColor = Color.LightSlateGray;
            lblPulseXThresholdVal.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPulseXThresholdVal.ForeColor = Color.White;
            lblPulseXThresholdVal.Location = new Point(0x237, 0x4d);
            lblPulseXThresholdVal.Name = "lblPulseXThresholdVal";
            lblPulseXThresholdVal.Size = new Size(15, 15);
            lblPulseXThresholdVal.TabIndex = 0xd9;
            lblPulseXThresholdVal.Text = "0";
            lblPulseZThresholdg.AutoSize = true;
            lblPulseZThresholdg.BackColor = Color.LightSlateGray;
            lblPulseZThresholdg.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPulseZThresholdg.ForeColor = Color.White;
            lblPulseZThresholdg.Location = new Point(610, 0x9a);
            lblPulseZThresholdg.Name = "lblPulseZThresholdg";
            lblPulseZThresholdg.Size = new Size(15, 15);
            lblPulseZThresholdg.TabIndex = 0xe2;
            lblPulseZThresholdg.Text = "g";
            lblPulseZThreshold.AutoSize = true;
            lblPulseZThreshold.BackColor = Color.LightSlateGray;
            lblPulseZThreshold.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPulseZThreshold.ForeColor = Color.White;
            lblPulseZThreshold.Location = new Point(0x1da, 0x9a);
            lblPulseZThreshold.Name = "lblPulseZThreshold";
            lblPulseZThreshold.Size = new Size(0x57, 15);
            lblPulseZThreshold.TabIndex = 0xe0;
            lblPulseZThreshold.Text = " Z Threshold";
            lblPulseZThresholdVal.AutoSize = true;
            lblPulseZThresholdVal.BackColor = Color.LightSlateGray;
            lblPulseZThresholdVal.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPulseZThresholdVal.ForeColor = Color.White;
            lblPulseZThresholdVal.Location = new Point(0x237, 0x9a);
            lblPulseZThresholdVal.Name = "lblPulseZThresholdVal";
            lblPulseZThresholdVal.Size = new Size(15, 15);
            lblPulseZThresholdVal.TabIndex = 0xe1;
            lblPulseZThresholdVal.Text = "0";
            lblPulseYThresholdg.AutoSize = true;
            lblPulseYThresholdg.BackColor = Color.LightSlateGray;
            lblPulseYThresholdg.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPulseYThresholdg.ForeColor = Color.White;
            lblPulseYThresholdg.Location = new Point(610, 0x75);
            lblPulseYThresholdg.Name = "lblPulseYThresholdg";
            lblPulseYThresholdg.Size = new Size(15, 15);
            lblPulseYThresholdg.TabIndex = 0xde;
            lblPulseYThresholdg.Text = "g";
            lblPulseXThreshold.AutoSize = true;
            lblPulseXThreshold.BackColor = Color.LightSlateGray;
            lblPulseXThreshold.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPulseXThreshold.ForeColor = Color.White;
            lblPulseXThreshold.Location = new Point(0x1da, 0x4d);
            lblPulseXThreshold.Name = "lblPulseXThreshold";
            lblPulseXThreshold.Size = new Size(0x58, 15);
            lblPulseXThreshold.TabIndex = 0xd8;
            lblPulseXThreshold.Text = " X Threshold";
            p10.BackColor = Color.LightSlateGray;
            p10.Controls.Add(btnResetFirstPulseTimeLimit);
            p10.Controls.Add(btnSetFirstPulseTimeLimit);
            p10.Controls.Add(chkPulseEnableLatch);
            p10.Controls.Add(chkPulseEnableXSP);
            p10.Controls.Add(chkPulseEnableYSP);
            p10.Controls.Add(chkPulseEnableZSP);
            p10.Controls.Add(tbFirstPulseTimeLimit);
            p10.Controls.Add(lblFirstPulseTimeLimitms);
            p10.Controls.Add(lblFirstTimeLimitVal);
            p10.Controls.Add(lblFirstPulseTimeLimit);
            p10.ForeColor = Color.White;
            p10.Location = new Point(4, 3);
            p10.Name = "p10";
            p10.Size = new Size(0x1bc, 0x71);
            p10.TabIndex = 0xd6;
            btnResetFirstPulseTimeLimit.BackColor = Color.LightSlateGray;
            btnResetFirstPulseTimeLimit.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnResetFirstPulseTimeLimit.ForeColor = Color.White;
            btnResetFirstPulseTimeLimit.Location = new Point(0xe2, 0x4e);
            btnResetFirstPulseTimeLimit.Name = "btnResetFirstPulseTimeLimit";
            btnResetFirstPulseTimeLimit.Size = new Size(0x87, 0x1f);
            btnResetFirstPulseTimeLimit.TabIndex = 150;
            btnResetFirstPulseTimeLimit.Text = "Reset Time Limit";
            btnResetFirstPulseTimeLimit.UseVisualStyleBackColor = false;
            btnResetFirstPulseTimeLimit.Click += new EventHandler(btnResetFirstPulseTimeLimit_Click);
            btnSetFirstPulseTimeLimit.BackColor = Color.LightSlateGray;
            btnSetFirstPulseTimeLimit.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSetFirstPulseTimeLimit.ForeColor = Color.White;
            btnSetFirstPulseTimeLimit.Location = new Point(0x34, 0x4e);
            btnSetFirstPulseTimeLimit.Name = "btnSetFirstPulseTimeLimit";
            btnSetFirstPulseTimeLimit.Size = new Size(0x7b, 0x1f);
            btnSetFirstPulseTimeLimit.TabIndex = 0x95;
            btnSetFirstPulseTimeLimit.Text = "Set Time Limit";
            btnSetFirstPulseTimeLimit.UseVisualStyleBackColor = false;
            btnSetFirstPulseTimeLimit.Click += new EventHandler(btnSetFirstPulseTimeLimit_Click);
            chkPulseEnableLatch.AutoSize = true;
            chkPulseEnableLatch.BackColor = Color.LightSlateGray;
            chkPulseEnableLatch.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
            chkPulseEnableLatch.ForeColor = Color.White;
            chkPulseEnableLatch.Location = new Point(0x143, 5);
            chkPulseEnableLatch.Name = "chkPulseEnableLatch";
            chkPulseEnableLatch.Size = new Size(0x62, 0x13);
            chkPulseEnableLatch.TabIndex = 0x94;
            chkPulseEnableLatch.Text = "Enable Latch";
            chkPulseEnableLatch.UseVisualStyleBackColor = false;
            chkPulseEnableLatch.CheckedChanged += new EventHandler(chkPulseEnableLatch_CheckedChanged);
            chkPulseEnableXSP.AutoSize = true;
            chkPulseEnableXSP.BackColor = Color.LightSlateGray;
            chkPulseEnableXSP.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
            chkPulseEnableXSP.ForeColor = Color.White;
            chkPulseEnableXSP.Location = new Point(6, 5);
            chkPulseEnableXSP.Name = "chkPulseEnableXSP";
            chkPulseEnableXSP.Size = new Size(0x5f, 0x13);
            chkPulseEnableXSP.TabIndex = 0x79;
            chkPulseEnableXSP.Text = "Enable X SP";
            chkPulseEnableXSP.UseVisualStyleBackColor = false;
            chkPulseEnableXSP.CheckedChanged += new EventHandler(chkPulseEnableXSP_CheckedChanged);
            chkPulseEnableYSP.AutoSize = true;
            chkPulseEnableYSP.BackColor = Color.LightSlateGray;
            chkPulseEnableYSP.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
            chkPulseEnableYSP.ForeColor = Color.White;
            chkPulseEnableYSP.Location = new Point(0x6f, 5);
            chkPulseEnableYSP.Name = "chkPulseEnableYSP";
            chkPulseEnableYSP.Size = new Size(0x5e, 0x13);
            chkPulseEnableYSP.TabIndex = 0x7a;
            chkPulseEnableYSP.Text = "Enable Y SP";
            chkPulseEnableYSP.UseVisualStyleBackColor = false;
            chkPulseEnableYSP.CheckedChanged += new EventHandler(chkPulseEnableYSP_CheckedChanged);
            chkPulseEnableZSP.AutoSize = true;
            chkPulseEnableZSP.BackColor = Color.LightSlateGray;
            chkPulseEnableZSP.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
            chkPulseEnableZSP.ForeColor = Color.White;
            chkPulseEnableZSP.Location = new Point(0xda, 5);
            chkPulseEnableZSP.Name = "chkPulseEnableZSP";
            chkPulseEnableZSP.Size = new Size(0x5e, 0x13);
            chkPulseEnableZSP.TabIndex = 0x7b;
            chkPulseEnableZSP.Text = "Enable Z SP";
            chkPulseEnableZSP.UseVisualStyleBackColor = false;
            chkPulseEnableZSP.CheckedChanged += new EventHandler(chkPulseEnableZSP_CheckedChanged);
            tbFirstPulseTimeLimit.BackColor = Color.LightSlateGray;
            tbFirstPulseTimeLimit.Location = new Point(0xbb, 0x27);
            tbFirstPulseTimeLimit.Maximum = 0xff;
            tbFirstPulseTimeLimit.Name = "tbFirstPulseTimeLimit";
            tbFirstPulseTimeLimit.Size = new Size(210, 0x2d);
            tbFirstPulseTimeLimit.TabIndex = 0x90;
            tbFirstPulseTimeLimit.Scroll += new EventHandler(tbFirstPulseTimeLimit_Scroll);
            lblFirstPulseTimeLimitms.AutoSize = true;
            lblFirstPulseTimeLimitms.BackColor = Color.LightSlateGray;
            lblFirstPulseTimeLimitms.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblFirstPulseTimeLimitms.ForeColor = Color.White;
            lblFirstPulseTimeLimitms.Location = new Point(0x8d, 0x2c);
            lblFirstPulseTimeLimitms.Name = "lblFirstPulseTimeLimitms";
            lblFirstPulseTimeLimitms.Size = new Size(0x18, 15);
            lblFirstPulseTimeLimitms.TabIndex = 0x93;
            lblFirstPulseTimeLimitms.Text = "ms";
            lblFirstTimeLimitVal.AutoSize = true;
            lblFirstTimeLimitVal.BackColor = Color.LightSlateGray;
            lblFirstTimeLimitVal.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblFirstTimeLimitVal.ForeColor = Color.White;
            lblFirstTimeLimitVal.Location = new Point(0x76, 0x2d);
            lblFirstTimeLimitVal.Name = "lblFirstTimeLimitVal";
            lblFirstTimeLimitVal.Size = new Size(14, 15);
            lblFirstTimeLimitVal.TabIndex = 0x92;
            lblFirstTimeLimitVal.Text = "0";
            lblFirstPulseTimeLimit.AutoSize = true;
            lblFirstPulseTimeLimit.BackColor = Color.LightSlateGray;
            lblFirstPulseTimeLimit.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblFirstPulseTimeLimit.ForeColor = Color.White;
            lblFirstPulseTimeLimit.Location = new Point(6, 0x2d);
            lblFirstPulseTimeLimit.Name = "lblFirstPulseTimeLimit";
            lblFirstPulseTimeLimit.Size = new Size(0x63, 15);
            lblFirstPulseTimeLimit.TabIndex = 0x91;
            lblFirstPulseTimeLimit.Text = "Pulse Time Limit";
            menuStrip1.BackColor = SystemColors.ButtonFace;
            menuStrip1.Enabled = false;
            menuStrip1.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            menuStrip1.Items.AddRange(new ToolStripItem[] { logDataToolStripMenuItem1 });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(0x3c6, 0x18);
            menuStrip1.TabIndex = 0x40;
            menuStrip1.Text = "menuStrip1";
            menuStrip1.Visible = false;
            logDataToolStripMenuItem1.DropDownItems.AddRange(new ToolStripItem[] { enableLogDataApplicationToolStripMenuItem });
            logDataToolStripMenuItem1.Name = "logDataToolStripMenuItem1";
            logDataToolStripMenuItem1.Size = new Size(0x4b, 20);
            logDataToolStripMenuItem1.Text = "Log Data";
            enableLogDataApplicationToolStripMenuItem.Name = "enableLogDataApplicationToolStripMenuItem";
            enableLogDataApplicationToolStripMenuItem.Size = new Size(0x10d, 0x16);
            enableLogDataApplicationToolStripMenuItem.Text = "Log Data XYZ with Tilt Detection";
            enableLogDataApplicationToolStripMenuItem.Click += new EventHandler(enableLogDataApplicationToolStripMenuItem_Click);
            CommStrip.Items.AddRange(new ToolStripItem[] { CommStripButton, toolStripStatusLabel });
            CommStrip.Location = new Point(0, 0x2ca);
            CommStrip.Name = "CommStrip";
            CommStrip.Size = new Size(0x3c5, 0x16);
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
            pictureBox1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            pictureBox1.BackgroundImageLayout = ImageLayout.None;
            pictureBox1.BorderStyle = BorderStyle.Fixed3D;
            pictureBox1.Image = Resources.STB_TOPBAR_LARGE;
            pictureBox1.Location = new Point(0, -3);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(0x3c5, 0x39);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 0x48;
            pictureBox1.TabStop = false;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            AutoScroll = true;
            BackColor = Color.LightSlateGray;
            base.ClientSize = new Size(0x3c5, 0x2e0);
            base.Controls.Add(CommStrip);
            base.Controls.Add(panelGeneral);
            base.Controls.Add(pictureBox1);
            base.Controls.Add(menuStrip1);
            base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            base.Icon = (Icon) resources.GetObject("$Icon");
            base.MaximizeBox = false;
            base.Name = "TapDemo";
            base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            Text = "Directional Tap Low Power: Comparing Embedded Function with Software Algorithm";
            base.Resize += new EventHandler(TapDemo_Resize);
            panelGeneral.ResumeLayout(false);
            panelDisplay.ResumeLayout(false);
            panelDisplay.PerformLayout();
            p11.ResumeLayout(false);
            p11.PerformLayout();
            ((ISupportInitialize) ledPulseDouble).EndInit();
            ((ISupportInitialize) ledTapEA).EndInit();
            ((ISupportInitialize) ledPZ).EndInit();
            ((ISupportInitialize) ledPX).EndInit();
            ((ISupportInitialize) ledPY).EndInit();
            ((ISupportInitialize) ledMCU1).EndInit();
            ((ISupportInitialize) legend1).EndInit();
            ((ISupportInitialize) pltFIFOPlot).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            AccelControllerGroup.ResumeLayout(false);
            AccelControllerGroup.PerformLayout();
            gbDR.ResumeLayout(false);
            gbDR.PerformLayout();
            gbOM.ResumeLayout(false);
            gbOM.PerformLayout();
            pOverSampling.ResumeLayout(false);
            pOverSampling.PerformLayout();
            p1.ResumeLayout(false);
            p1.PerformLayout();
            ((ISupportInitialize) ledStandby).EndInit();
            ((ISupportInitialize) ledSleep).EndInit();
            ((ISupportInitialize) ledWake).EndInit();
            panel15.ResumeLayout(false);
            panel15.PerformLayout();
            panelAdvanced.ResumeLayout(false);
            panelAdvanced.PerformLayout();
            p12.ResumeLayout(false);
            p12.PerformLayout();
            tbPulseLatency.EndInit();
            tbPulse2ndPulseWin.EndInit();
            tbPulseZThreshold.EndInit();
            tbPulseXThreshold.EndInit();
            tbPulseYThreshold.EndInit();
            p10.ResumeLayout(false);
            p10.PerformLayout();
            tbFirstPulseTimeLimit.EndInit();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            CommStrip.ResumeLayout(false);
            CommStrip.PerformLayout();
            ((ISupportInitialize) pictureBox1).EndInit();
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
                ActiveDynamicRange = 2;
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
                ActiveDynamicRange = 4;
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
                ActiveDynamicRange = 8;
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
                p10.Enabled = false;
                panel15.Enabled = false;
                p12.Enabled = false;
                rdoDefaultSPDP.Enabled = false;
                rdoDefaultSP.Enabled = false;
                int[] datapassed = new int[] { 1 };
                ControllerObj.ActiveFlag = true;
                ControllerReqPacket reqName = new ControllerReqPacket();
                ControllerObj.CreateNewTaskFromGUI(reqName, 0, 0x53, datapassed);
            }
        }

        private void rdoDefaultSP_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoDefaultSP.Checked)
            {
                rdoOSNormalMode.Checked = true;
                int[] datapassed = new int[1];
                WakeOSMode = 0;
                datapassed[0] = WakeOSMode;
                ControllerObj.SetWakeOSModeFlag = true;
                ControllerReqPacket reqName = new ControllerReqPacket();
                ControllerObj.CreateNewTaskFromGUI(reqName, 0, 0x54, datapassed);
                chkPulseEnableXSP.Checked = true;
                int[] numArray2 = new int[] { 0xff };
                ControllerObj.SetPulseXSPFlag = true;
                ControllerReqPacket packet2 = new ControllerReqPacket();
                ControllerObj.CreateNewTaskFromGUI(packet2, 0, 0x42, numArray2);
                chkPulseEnableXDP.Checked = false;
                int[] numArray3 = new int[] { 0 };
                ControllerObj.SetPulseXDPFlag = true;
                ControllerReqPacket packet3 = new ControllerReqPacket();
                ControllerObj.CreateNewTaskFromGUI(packet3, 0, 0x41, numArray3);
                chkPulseEnableYSP.Checked = true;
                int[] numArray4 = new int[] { 0xff };
                ControllerObj.SetPulseYSPFlag = true;
                ControllerReqPacket packet4 = new ControllerReqPacket();
                ControllerObj.CreateNewTaskFromGUI(packet4, 0, 0x40, numArray4);
                chkPulseEnableYDP.Checked = false;
                int[] numArray5 = new int[] { 0 };
                ControllerObj.SetPulseYDPFlag = true;
                ControllerReqPacket packet5 = new ControllerReqPacket();
                ControllerObj.CreateNewTaskFromGUI(packet5, 0, 0x3f, numArray5);
                chkPulseEnableZSP.Checked = true;
                int[] numArray6 = new int[] { 0xff };
                ControllerObj.SetPulseZSPFlag = true;
                ControllerReqPacket packet6 = new ControllerReqPacket();
                ControllerObj.CreateNewTaskFromGUI(packet6, 0, 0x3e, numArray6);
                chkPulseEnableZDP.Checked = false;
                int[] numArray7 = new int[] { 0 };
                ControllerObj.SetPulseZDPFlag = true;
                ControllerReqPacket packet7 = new ControllerReqPacket();
                ControllerObj.CreateNewTaskFromGUI(packet7, 0, 0x3d, numArray7);
                chkPulseLPFEnable.Checked = true;
                int[] numArray8 = new int[] { 0xff };
                ControllerObj.SetPulseLPFEnableFlag = true;
                ControllerReqPacket packet8 = new ControllerReqPacket();
                ControllerObj.CreateNewTaskFromGUI(packet8, 6, 0x59, numArray8);
                if (DR_timestep >= 80.0)
                {
                    tbFirstPulseTimeLimit.Value = 1;
                    tbPulseLatency.Value = 2;
                }
                else
                {
                    tbFirstPulseTimeLimit.Value = (int) (100.0 / DR_timestep);
                    tbPulseLatency.Value = tbFirstPulseTimeLimit.Value;
                }
                double num = tbFirstPulseTimeLimit.Value * DR_timestep;
                if (num > 1000.0)
                {
                    lblFirstPulseTimeLimitms.Text = "s";
                    num /= 1000.0;
                    lblFirstTimeLimitVal.Text = string.Format("{0:F2}", num);
                }
                else
                {
                    lblFirstPulseTimeLimitms.Text = "ms";
                    lblFirstTimeLimitVal.Text = string.Format("{0:F2}", num);
                }
                int[] numArray9 = new int[] { tbFirstPulseTimeLimit.Value };
                ControllerObj.SetPulseFirstTimeLimitFlag = true;
                ControllerReqPacket packet9 = new ControllerReqPacket();
                ControllerObj.CreateNewTaskFromGUI(packet9, 0, 0x43, numArray9);
                btnSetFirstPulseTimeLimit.Enabled = false;
                btnResetFirstPulseTimeLimit.Enabled = true;
                tbFirstPulseTimeLimit.Enabled = false;
                lblFirstPulseTimeLimit.Enabled = false;
                lblFirstPulseTimeLimitms.Enabled = false;
                lblFirstTimeLimitVal.Enabled = false;
                double num2 = (tbPulseLatency.Value * DR_timestep) * 2.0;
                if (num2 > 1000.0)
                {
                    lblPulseLatencyms.Text = "s";
                    num2 /= 1000.0;
                    lblPulseLatencyVal.Text = string.Format("{0:F2}", num2);
                }
                else
                {
                    lblPulseLatencyms.Text = "ms";
                    lblPulseLatencyVal.Text = string.Format("{0:F2}", num2);
                }
                int[] numArray10 = new int[] { tbPulseLatency.Value };
                ControllerObj.SetPulseLatencyFlag = true;
                ControllerReqPacket packet10 = new ControllerReqPacket();
                ControllerObj.CreateNewTaskFromGUI(packet10, 0, 0x47, numArray10);
                btnPulseSetTime2ndPulse.Enabled = false;
                btnPulseResetTime2ndPulse.Enabled = true;
                tbPulseLatency.Enabled = false;
                tbPulse2ndPulseWin.Enabled = false;
                lblPulseLatency.Enabled = false;
                lblPulse2ndPulseWin.Enabled = false;
                lblPulseLatencyms.Enabled = false;
                lblPulse2ndPulseWinms.Enabled = false;
                lblPulseLatencyVal.Enabled = false;
                lblPulse2ndPulseWinVal.Enabled = false;
                double num4 = 0.063;
                tbPulseXThreshold.Value = 0x20;
                double num3 = tbPulseXThreshold.Value * num4;
                lblPulseXThresholdVal.Text = string.Format("{0:F2}", num3);
                tbPulseYThreshold.Value = 0x20;
                double num5 = tbPulseYThreshold.Value * num4;
                lblPulseYThresholdVal.Text = string.Format("{0:F2}", num5);
                tbPulseZThreshold.Value = 0x31;
                double num6 = tbPulseZThreshold.Value * num4;
                lblPulseZThresholdVal.Text = string.Format("{0:F2}", num6);
                int[] numArray11 = new int[] { tbPulseXThreshold.Value };
                ControllerObj.SetPulseXThresholdFlag = true;
                ControllerReqPacket packet11 = new ControllerReqPacket();
                ControllerObj.CreateNewTaskFromGUI(packet11, 0, 0x44, numArray11);
                int[] numArray12 = new int[] { tbPulseYThreshold.Value };
                ControllerObj.SetPulseYThresholdFlag = true;
                ControllerReqPacket packet12 = new ControllerReqPacket();
                ControllerObj.CreateNewTaskFromGUI(packet12, 0, 0x45, numArray12);
                int[] numArray13 = new int[] { tbPulseZThreshold.Value };
                ControllerObj.SetPulseZThresholdFlag = true;
                ControllerReqPacket packet13 = new ControllerReqPacket();
                ControllerObj.CreateNewTaskFromGUI(packet13, 0, 70, numArray13);
                btnSetPulseThresholds.Enabled = false;
                btnResetPulseThresholds.Enabled = true;
                tbPulseXThreshold.Enabled = false;
                tbPulseYThreshold.Enabled = false;
                tbPulseZThreshold.Enabled = false;
                lblPulseXThreshold.Enabled = false;
                lblPulseYThreshold.Enabled = false;
                lblPulseZThreshold.Enabled = false;
                lblPulseXThresholdg.Enabled = false;
                lblPulseYThresholdg.Enabled = false;
                lblPulseZThresholdg.Enabled = false;
                lblPulseXThresholdVal.Enabled = false;
                lblPulseYThresholdVal.Enabled = false;
                lblPulseZThresholdVal.Enabled = false;
                tbPulse2ndPulseWin.Value = 0;
                lblPulse2ndPulseWinms.Text = "ms";
                lblPulse2ndPulseWinVal.Text = string.Format("{0:F1}", 0.0);
                int[] numArray14 = new int[] { tbPulse2ndPulseWin.Value };
                ControllerObj.SetPulse2ndPulseWinFlag = true;
                ControllerReqPacket packet14 = new ControllerReqPacket();
                ControllerObj.CreateNewTaskFromGUI(packet14, 0, 0x48, numArray14);
                rdoDefaultSP.Checked = true;
            }
        }

        private void rdoDefaultSPDP_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoDefaultSPDP.Checked)
            {
                rdoOSLNLPMode.Checked = true;
                int[] datapassed = new int[1];
                WakeOSMode = 1;
                datapassed[0] = WakeOSMode;
                ControllerObj.SetWakeOSModeFlag = true;
                ControllerReqPacket reqName = new ControllerReqPacket();
                ControllerObj.CreateNewTaskFromGUI(reqName, 0, 0x54, datapassed);
                chkPulseEnableXSP.Checked = true;
                int[] numArray2 = new int[] { 0xff };
                ControllerObj.SetPulseXSPFlag = true;
                ControllerReqPacket packet2 = new ControllerReqPacket();
                ControllerObj.CreateNewTaskFromGUI(packet2, 0, 0x42, numArray2);
                chkPulseEnableXDP.Checked = true;
                int[] numArray3 = new int[] { 0xff };
                ControllerObj.SetPulseXDPFlag = true;
                ControllerReqPacket packet3 = new ControllerReqPacket();
                ControllerObj.CreateNewTaskFromGUI(packet3, 0, 0x41, numArray3);
                chkPulseEnableYSP.Checked = true;
                int[] numArray4 = new int[] { 0xff };
                ControllerObj.SetPulseYSPFlag = true;
                ControllerReqPacket packet4 = new ControllerReqPacket();
                ControllerObj.CreateNewTaskFromGUI(packet4, 0, 0x40, numArray4);
                chkPulseEnableYDP.Checked = true;
                int[] numArray5 = new int[] { 0xff };
                ControllerObj.SetPulseYDPFlag = true;
                ControllerReqPacket packet5 = new ControllerReqPacket();
                ControllerObj.CreateNewTaskFromGUI(packet5, 0, 0x3f, numArray5);
                chkPulseEnableZSP.Checked = true;
                int[] numArray6 = new int[] { 0xff };
                ControllerObj.SetPulseZSPFlag = true;
                ControllerReqPacket packet6 = new ControllerReqPacket();
                ControllerObj.CreateNewTaskFromGUI(packet6, 0, 0x3e, numArray6);
                chkPulseEnableZDP.Checked = true;
                int[] numArray7 = new int[] { 0xff };
                ControllerObj.SetPulseZDPFlag = true;
                ControllerReqPacket packet7 = new ControllerReqPacket();
                ControllerObj.CreateNewTaskFromGUI(packet7, 0, 0x3d, numArray7);
                chkPulseLPFEnable.Checked = true;
                int[] numArray8 = new int[] { 0xff };
                ControllerObj.SetPulseLPFEnableFlag = true;
                ControllerReqPacket packet8 = new ControllerReqPacket();
                ControllerObj.CreateNewTaskFromGUI(packet8, 6, 0x59, numArray8);
                if (DR_timestep >= 80.0)
                {
                    tbFirstPulseTimeLimit.Value = 1;
                    tbPulseLatency.Value = 2;
                    tbPulse2ndPulseWin.Value = 2;
                }
                else
                {
                    tbFirstPulseTimeLimit.Value = (int) (100.0 / DR_timestep);
                    tbPulseLatency.Value = tbFirstPulseTimeLimit.Value;
                    tbPulse2ndPulseWin.Value = tbPulseLatency.Value + ((int) (100.0 / DR_timestep));
                }
                double num = tbFirstPulseTimeLimit.Value * DR_timestep;
                if (num > 1000.0)
                {
                    lblFirstPulseTimeLimitms.Text = "s";
                    num /= 1000.0;
                    lblFirstTimeLimitVal.Text = string.Format("{0:F2}", num);
                }
                else
                {
                    lblFirstPulseTimeLimitms.Text = "ms";
                    lblFirstTimeLimitVal.Text = string.Format("{0:F2}", num);
                }
                int[] numArray9 = new int[] { tbFirstPulseTimeLimit.Value };
                ControllerObj.SetPulseFirstTimeLimitFlag = true;
                ControllerReqPacket packet9 = new ControllerReqPacket();
                ControllerObj.CreateNewTaskFromGUI(packet9, 0, 0x43, numArray9);
                btnSetFirstPulseTimeLimit.Enabled = false;
                btnResetFirstPulseTimeLimit.Enabled = true;
                tbFirstPulseTimeLimit.Enabled = false;
                lblFirstPulseTimeLimit.Enabled = false;
                lblFirstPulseTimeLimitms.Enabled = false;
                lblFirstTimeLimitVal.Enabled = false;
                double num2 = (tbPulseLatency.Value * DR_timestep) * 2.0;
                if (num2 > 1000.0)
                {
                    lblPulseLatencyms.Text = "s";
                    num2 /= 1000.0;
                    lblPulseLatencyVal.Text = string.Format("{0:F2}", num2);
                }
                else
                {
                    lblPulseLatencyms.Text = "ms";
                    lblPulseLatencyVal.Text = string.Format("{0:F2}", num2);
                }
                int[] numArray10 = new int[] { tbPulseLatency.Value };
                ControllerObj.SetPulseLatencyFlag = true;
                ControllerReqPacket packet10 = new ControllerReqPacket();
                ControllerObj.CreateNewTaskFromGUI(packet10, 0, 0x47, numArray10);
                double num3 = (tbPulse2ndPulseWin.Value * DR_timestep) * 2.0;
                if (num3 > 1000.0)
                {
                    lblPulse2ndPulseWinms.Text = "s";
                    num3 /= 1000.0;
                    lblPulse2ndPulseWinVal.Text = string.Format("{0:F2}", num3);
                }
                else
                {
                    lblPulse2ndPulseWinms.Text = "ms";
                    lblPulse2ndPulseWinVal.Text = string.Format("{0:F2}", num3);
                }
                int[] numArray11 = new int[] { tbPulse2ndPulseWin.Value };
                ControllerObj.SetPulse2ndPulseWinFlag = true;
                ControllerReqPacket packet11 = new ControllerReqPacket();
                ControllerObj.CreateNewTaskFromGUI(packet11, 0, 0x48, numArray11);
                btnPulseSetTime2ndPulse.Enabled = false;
                btnPulseResetTime2ndPulse.Enabled = true;
                tbPulseLatency.Enabled = false;
                tbPulse2ndPulseWin.Enabled = false;
                lblPulseLatency.Enabled = false;
                lblPulse2ndPulseWin.Enabled = false;
                lblPulseLatencyms.Enabled = false;
                lblPulse2ndPulseWinms.Enabled = false;
                lblPulseLatencyVal.Enabled = false;
                lblPulse2ndPulseWinVal.Enabled = false;
                double num5 = 0.063;
                tbPulseXThreshold.Value = 0x20;
                double num4 = tbPulseXThreshold.Value * num5;
                lblPulseXThresholdVal.Text = string.Format("{0:F2}", num4);
                tbPulseYThreshold.Value = 0x20;
                double num6 = tbPulseYThreshold.Value * num5;
                lblPulseYThresholdVal.Text = string.Format("{0:F2}", num6);
                tbPulseZThreshold.Value = 0x31;
                double num7 = tbPulseZThreshold.Value * num5;
                lblPulseZThresholdVal.Text = string.Format("{0:F2}", num7);
                int[] numArray12 = new int[] { tbPulseXThreshold.Value };
                ControllerObj.SetPulseXThresholdFlag = true;
                ControllerReqPacket packet12 = new ControllerReqPacket();
                ControllerObj.CreateNewTaskFromGUI(packet12, 0, 0x44, numArray12);
                int[] numArray13 = new int[] { tbPulseYThreshold.Value };
                ControllerObj.SetPulseYThresholdFlag = true;
                ControllerReqPacket packet13 = new ControllerReqPacket();
                ControllerObj.CreateNewTaskFromGUI(packet13, 0, 0x45, numArray13);
                int[] numArray14 = new int[] { tbPulseZThreshold.Value };
                ControllerObj.SetPulseZThresholdFlag = true;
                ControllerReqPacket packet14 = new ControllerReqPacket();
                ControllerObj.CreateNewTaskFromGUI(packet14, 0, 70, numArray14);
                btnSetPulseThresholds.Enabled = false;
                btnResetPulseThresholds.Enabled = true;
                tbPulseXThreshold.Enabled = false;
                tbPulseYThreshold.Enabled = false;
                tbPulseZThreshold.Enabled = false;
                lblPulseXThreshold.Enabled = false;
                lblPulseYThreshold.Enabled = false;
                lblPulseZThreshold.Enabled = false;
                lblPulseXThresholdg.Enabled = false;
                lblPulseYThresholdg.Enabled = false;
                lblPulseZThresholdg.Enabled = false;
                lblPulseXThresholdVal.Enabled = false;
                lblPulseYThresholdVal.Enabled = false;
                lblPulseZThresholdVal.Enabled = false;
                rdoDefaultSPDP.Checked = true;
            }
        }

        private void rdoOSHiResMode_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoOSHiResMode.Checked)
            {
                rdoDefaultSP.Checked = false;
                rdoDefaultSPDP.Checked = false;
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
                        DR_PulseTimeStepNoLPF = 0.625;
                        break;

                    case 1:
                        DR_timestep = 2.5;
                        DR_PulseTimeStepNoLPF = 0.625;
                        break;

                    case 2:
                        DR_timestep = 2.5;
                        DR_PulseTimeStepNoLPF = 0.625;
                        break;

                    case 3:
                        DR_timestep = 2.5;
                        DR_PulseTimeStepNoLPF = 0.625;
                        break;

                    case 4:
                        DR_timestep = 2.5;
                        DR_PulseTimeStepNoLPF = 0.625;
                        break;

                    case 5:
                        DR_timestep = 2.5;
                        DR_PulseTimeStepNoLPF = 0.625;
                        break;

                    case 6:
                        DR_timestep = 2.5;
                        DR_PulseTimeStepNoLPF = 0.625;
                        break;

                    case 7:
                        DR_timestep = 2.5;
                        DR_PulseTimeStepNoLPF = 0.625;
                        break;
                }
                if (chkPulseLPFEnable.Checked)
                {
                    pulse_step = DR_timestep;
                }
                else
                {
                    pulse_step = DR_PulseTimeStepNoLPF;
                }
                double num = tbFirstPulseTimeLimit.Value * pulse_step;
                if (num > 1000.0)
                {
                    lblFirstPulseTimeLimitms.Text = "s";
                    num /= 1000.0;
                    lblFirstTimeLimitVal.Text = string.Format("{0:F3}", num);
                }
                else
                {
                    lblFirstPulseTimeLimitms.Text = "ms";
                    lblFirstTimeLimitVal.Text = string.Format("{0:F3}", num);
                }
                double num2 = pulse_step * 2.0;
                double num3 = tbPulseLatency.Value * num2;
                if (num3 > 1000.0)
                {
                    lblPulseLatencyms.Text = "s";
                    num3 /= 1000.0;
                    lblPulseLatencyVal.Text = string.Format("{0:F2}", num3);
                }
                else
                {
                    lblPulseLatencyms.Text = "ms";
                    lblPulseLatencyVal.Text = string.Format("{0:F2}", num3);
                }
                double num4 = pulse_step * 2.0;
                double num5 = tbPulse2ndPulseWin.Value * num4;
                if (num5 > 1000.0)
                {
                    lblPulse2ndPulseWinms.Text = "s";
                    num5 /= 1000.0;
                    lblPulse2ndPulseWinVal.Text = string.Format("{0:F2}", num5);
                }
                else
                {
                    lblPulse2ndPulseWinms.Text = "ms";
                    lblPulse2ndPulseWinVal.Text = string.Format("{0:F2}", num5);
                }
            }
        }

        private void rdoOSLNLPMode_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoOSLNLPMode.Checked)
            {
                rdoDefaultSP.Checked = false;
                rdoDefaultSPDP.Checked = false;
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
                        DR_PulseTimeStepNoLPF = 0.625;
                        break;

                    case 1:
                        DR_timestep = 2.5;
                        DR_PulseTimeStepNoLPF = 0.625;
                        break;

                    case 2:
                        DR_timestep = 5.0;
                        DR_PulseTimeStepNoLPF = 1.25;
                        break;

                    case 3:
                        DR_timestep = 10.0;
                        DR_PulseTimeStepNoLPF = 2.5;
                        break;

                    case 4:
                        DR_timestep = 20.0;
                        DR_PulseTimeStepNoLPF = 5.0;
                        break;

                    case 5:
                        DR_timestep = 80.0;
                        DR_PulseTimeStepNoLPF = 20.0;
                        break;

                    case 6:
                        DR_timestep = 80.0;
                        DR_PulseTimeStepNoLPF = 20.0;
                        break;

                    case 7:
                        DR_timestep = 80.0;
                        DR_PulseTimeStepNoLPF = 20.0;
                        break;
                }
                if (chkPulseLPFEnable.Checked)
                {
                    pulse_step = DR_timestep;
                }
                else
                {
                    pulse_step = DR_PulseTimeStepNoLPF;
                }
                double num = tbFirstPulseTimeLimit.Value * pulse_step;
                if (num > 1000.0)
                {
                    lblFirstPulseTimeLimitms.Text = "s";
                    num /= 1000.0;
                    lblFirstTimeLimitVal.Text = string.Format("{0:F3}", num);
                }
                else
                {
                    lblFirstPulseTimeLimitms.Text = "ms";
                    lblFirstTimeLimitVal.Text = string.Format("{0:F3}", num);
                }
                double num2 = pulse_step * 2.0;
                double num3 = tbPulseLatency.Value * num2;
                if (num3 > 1000.0)
                {
                    lblPulseLatencyms.Text = "s";
                    num3 /= 1000.0;
                    lblPulseLatencyVal.Text = string.Format("{0:F2}", num3);
                }
                else
                {
                    lblPulseLatencyms.Text = "ms";
                    lblPulseLatencyVal.Text = string.Format("{0:F2}", num3);
                }
                double num4 = pulse_step * 2.0;
                double num5 = tbPulse2ndPulseWin.Value * num4;
                if (num5 > 1000.0)
                {
                    lblPulse2ndPulseWinms.Text = "s";
                    num5 /= 1000.0;
                    lblPulse2ndPulseWinVal.Text = string.Format("{0:F2}", num5);
                }
                else
                {
                    lblPulse2ndPulseWinms.Text = "ms";
                    lblPulse2ndPulseWinVal.Text = string.Format("{0:F2}", num5);
                }
            }
        }

        private void rdoOSLPMode_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoOSLPMode.Checked)
            {
                rdoDefaultSP.Checked = false;
                rdoDefaultSPDP.Checked = false;
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
                        DR_PulseTimeStepNoLPF = 0.625;
                        break;

                    case 1:
                        DR_timestep = 2.5;
                        DR_PulseTimeStepNoLPF = 1.25;
                        break;

                    case 2:
                        DR_timestep = 5.0;
                        DR_PulseTimeStepNoLPF = 2.5;
                        break;

                    case 3:
                        DR_timestep = 10.0;
                        DR_PulseTimeStepNoLPF = 5.0;
                        break;

                    case 4:
                        DR_timestep = 20.0;
                        DR_PulseTimeStepNoLPF = 10.0;
                        break;

                    case 5:
                        DR_timestep = 80.0;
                        DR_PulseTimeStepNoLPF = 40.0;
                        break;

                    case 6:
                        DR_timestep = 160.0;
                        DR_PulseTimeStepNoLPF = 40.0;
                        break;

                    case 7:
                        DR_timestep = 160.0;
                        DR_PulseTimeStepNoLPF = 40.0;
                        break;
                }
                if (chkPulseLPFEnable.Checked)
                {
                    pulse_step = DR_timestep;
                }
                else
                {
                    pulse_step = DR_PulseTimeStepNoLPF;
                }
                double num = tbFirstPulseTimeLimit.Value * pulse_step;
                if (num > 1000.0)
                {
                    lblFirstPulseTimeLimitms.Text = "s";
                    num /= 1000.0;
                    lblFirstTimeLimitVal.Text = string.Format("{0:F3}", num);
                }
                else
                {
                    lblFirstPulseTimeLimitms.Text = "ms";
                    lblFirstTimeLimitVal.Text = string.Format("{0:F3}", num);
                }
                double num2 = pulse_step * 2.0;
                double num3 = tbPulseLatency.Value * num2;
                if (num3 > 1000.0)
                {
                    lblPulseLatencyms.Text = "s";
                    num3 /= 1000.0;
                    lblPulseLatencyVal.Text = string.Format("{0:F2}", num3);
                }
                else
                {
                    lblPulseLatencyms.Text = "ms";
                    lblPulseLatencyVal.Text = string.Format("{0:F2}", num3);
                }
                double num4 = pulse_step * 2.0;
                double num5 = tbPulse2ndPulseWin.Value * num4;
                if (num5 > 1000.0)
                {
                    lblPulse2ndPulseWinms.Text = "s";
                    num5 /= 1000.0;
                    lblPulse2ndPulseWinVal.Text = string.Format("{0:F2}", num5);
                }
                else
                {
                    lblPulse2ndPulseWinms.Text = "ms";
                    lblPulse2ndPulseWinVal.Text = string.Format("{0:F2}", num5);
                }
            }
        }

        private void rdoOSNormalMode_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoOSNormalMode.Checked)
            {
                rdoDefaultSP.Checked = false;
                rdoDefaultSPDP.Checked = false;
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
                        DR_PulseTimeStepNoLPF = 0.625;
                        break;

                    case 1:
                        DR_timestep = 2.5;
                        DR_PulseTimeStepNoLPF = 0.625;
                        break;

                    case 2:
                        DR_timestep = 5.0;
                        DR_PulseTimeStepNoLPF = 1.25;
                        break;

                    case 3:
                        DR_timestep = 10.0;
                        DR_PulseTimeStepNoLPF = 2.5;
                        break;

                    case 4:
                        DR_timestep = 20.0;
                        DR_PulseTimeStepNoLPF = 5.0;
                        break;

                    case 5:
                        DR_timestep = 20.0;
                        DR_PulseTimeStepNoLPF = 5.0;
                        break;

                    case 6:
                        DR_timestep = 20.0;
                        DR_PulseTimeStepNoLPF = 5.0;
                        break;

                    case 7:
                        DR_timestep = 20.0;
                        DR_PulseTimeStepNoLPF = 5.0;
                        break;
                }
                if (chkPulseLPFEnable.Checked)
                {
                    pulse_step = DR_timestep;
                }
                else
                {
                    pulse_step = DR_PulseTimeStepNoLPF;
                }
                double num = tbFirstPulseTimeLimit.Value * pulse_step;
                if (num > 1000.0)
                {
                    lblFirstPulseTimeLimitms.Text = "s";
                    num /= 1000.0;
                    lblFirstTimeLimitVal.Text = string.Format("{0:F3}", num);
                }
                else
                {
                    lblFirstPulseTimeLimitms.Text = "ms";
                    lblFirstTimeLimitVal.Text = string.Format("{0:F3}", num);
                }
                double num2 = pulse_step * 2.0;
                double num3 = tbPulseLatency.Value * num2;
                if (num3 > 1000.0)
                {
                    lblPulseLatencyms.Text = "s";
                    num3 /= 1000.0;
                    lblPulseLatencyVal.Text = string.Format("{0:F2}", num3);
                }
                else
                {
                    lblPulseLatencyms.Text = "ms";
                    lblPulseLatencyVal.Text = string.Format("{0:F2}", num3);
                }
                double num4 = pulse_step * 2.0;
                double num5 = tbPulse2ndPulseWin.Value * num4;
                if (num5 > 1000.0)
                {
                    lblPulse2ndPulseWinms.Text = "s";
                    num5 /= 1000.0;
                    lblPulse2ndPulseWinVal.Text = string.Format("{0:F2}", num5);
                }
                else
                {
                    lblPulse2ndPulseWinms.Text = "ms";
                    lblPulse2ndPulseWinVal.Text = string.Format("{0:F2}", num5);
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
                p10.Enabled = true;
                panel15.Enabled = true;
                p12.Enabled = true;
                rdoDefaultSPDP.Enabled = true;
                rdoDefaultSP.Enabled = true;
                int[] datapassed = new int[] { 0 };
                ControllerObj.ActiveFlag = true;
                ControllerReqPacket reqName = new ControllerReqPacket();
                ControllerObj.CreateNewTaskFromGUI(reqName, 0, 0x53, datapassed);
            }
        }

        private void SetPulseInterrupts(bool enabled)
        {
            int[] datapassed = new int[] { enabled ? 0xff : 0, 0x48 };
            ControllerObj.SetIntsEnableFlag = true;
            ControllerReqPacket reqName = new ControllerReqPacket();
            ControllerObj.CreateNewTaskFromGUI(reqName, 0, 13, datapassed);
            datapassed = new int[] { enabled ? 0xff : 0, 0x40 };
            ControllerObj.SetIntsConfigFlag = true;
            reqName = new ControllerReqPacket();
            ControllerObj.CreateNewTaskFromGUI(reqName, 0, 14, datapassed);
            ControllerObj.AppRegisterWrite(new int[] { 0x15, 1 });
        }

        private void TapDemo_Resize(object sender, EventArgs e)
        {
            windowHeight = base.Height;
        }

        private void tbFirstPulseTimeLimit_Scroll(object sender, EventArgs e)
        {
            if (chkPulseLPFEnable.Checked)
            {
                pulse_step = DR_timestep;
            }
            else
            {
                pulse_step = DR_PulseTimeStepNoLPF;
            }
            double num = tbFirstPulseTimeLimit.Value * pulse_step;
            if (num > 1000.0)
            {
                lblFirstPulseTimeLimitms.Text = "s";
                num /= 1000.0;
                lblFirstTimeLimitVal.Text = string.Format("{0:F2}", num);
            }
            else
            {
                lblFirstPulseTimeLimitms.Text = "ms";
                lblFirstTimeLimitVal.Text = string.Format("{0:F3}", num);
            }
        }

        private void tbPulse2ndPulseWin_Scroll(object sender, EventArgs e)
        {
            double num = pulse_step * 2.0;
            double num2 = tbPulse2ndPulseWin.Value * num;
            if (num2 > 1000.0)
            {
                lblPulse2ndPulseWinms.Text = "s";
                num2 /= 1000.0;
                lblPulse2ndPulseWinVal.Text = string.Format("{0:F2}", num2);
            }
            else
            {
                lblPulse2ndPulseWinms.Text = "ms";
                lblPulse2ndPulseWinVal.Text = string.Format("{0:F2}", num2);
            }
        }

        private void tbPulseLatency_Scroll(object sender, EventArgs e)
        {
            double num = pulse_step * 2.0;
            double num2 = tbPulseLatency.Value * num;
            if (num2 > 1000.0)
            {
                lblPulseLatencyms.Text = "s";
                num2 /= 1000.0;
                lblPulseLatencyVal.Text = string.Format("{0:F2}", num2);
            }
            else
            {
                lblPulseLatencyms.Text = "ms";
                lblPulseLatencyVal.Text = string.Format("{0:F2}", num2);
            }
        }

        private void tbPulseXThreshold_Scroll(object sender, EventArgs e)
        {
            double num2 = 0.063;
            double num = tbPulseXThreshold.Value * num2;
            lblPulseXThresholdVal.Text = string.Format("{0:F3}", num);
        }

        private void tbPulseYThreshold_Scroll(object sender, EventArgs e)
        {
            double num2 = 0.063;
            double num = tbPulseYThreshold.Value * num2;
            lblPulseYThresholdVal.Text = string.Format("{0:F3}", num);
        }

        private void tbPulseZThreshold_Scroll(object sender, EventArgs e)
        {
            double num2 = 0.063;
            double num = tbPulseZThreshold.Value * num2;
            lblPulseZThresholdVal.Text = string.Format("{0:F3}", num);
        }

        private void tmrTiltTimer_Tick(object sender, EventArgs e)
        {
            Queue queue;
            UpdateFormState();
            if (ledPZ.Value && ledPulseDouble.Value)
            {
            }
            if (ledMCU1.Value)
            {
                switch (ddlDataRate.SelectedIndex)
                {
                    case 0:
                        txtCurrent.Text = "12.165 mA";
                        goto Label_00C8;

                    case 1:
                        txtCurrent.Text = "12.085 mA";
                        goto Label_00C8;

                    case 2:
                        txtCurrent.Text = "12.044 mA";
                        goto Label_00C8;

                    case 3:
                        txtCurrent.Text = "12.024 mA";
                        goto Label_00C8;
                }
                txtCurrent.Text = "12.085 mA";
            }
        Label_00C8:
            if (ledTapEA.Value)
            {
                ledCount++;
                if (ledCount == 10)
                {
                    ledCount = 0;
                    ledTapEA.Value = false;
                    ledPX.Value = false;
                    ledPY.Value = false;
                    ledPZ.Value = false;
                    ledPulseDouble.Value = false;
                    ledMCU1.Value = false;
                    switch (ddlDataRate.SelectedIndex)
                    {
                        case 0:
                            txtCurrent.Text = "0.665 mA";
                            goto Label_01DD;

                        case 1:
                            txtCurrent.Text = "0.585 mA";
                            goto Label_01DD;

                        case 2:
                            txtCurrent.Text = "0.544 mA";
                            goto Label_01DD;

                        case 3:
                            txtCurrent.Text = "0.524 mA";
                            goto Label_01DD;
                    }
                    txtCurrent.Text = "0.585 mA";
                }
            }
        Label_01DD:
            Monitor.Enter(queue = qFIFOArray);
            try
            {
                while (qFIFOArray.Count > 0)
                {
                    int num = 0;
                    if (rdoStandby.Checked)
                    {
                        num = 0;
                    }
                    else if (rdo2g.Checked)
                    {
                        num = 1;
                    }
                    else if (rdo4g.Checked)
                    {
                        num = 2;
                    }
                    else if (rdo8g.Checked)
                    {
                        num = 4;
                    }
                    pltFIFOPlot.ClearData();
                    XYZCounts[] countsArray = (XYZCounts[]) qFIFOArray.Dequeue();
                    for (int i = 0; i < countsArray.Length; i++)
                    {
                        pltFIFOPlot.Plots[0].PlotYAppend((double) ((((sbyte) countsArray[i].XAxis) * 0.015625) * num));
                        pltFIFOPlot.Plots[1].PlotYAppend((double) ((((sbyte) countsArray[i].YAxis) * 0.015625) * num));
                        pltFIFOPlot.Plots[2].PlotYAppend((double) ((((sbyte) countsArray[i].ZAxis) * 0.015625) * num));
                    }
                }
            }
            finally
            {
                Monitor.Exit(queue);
            }
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
            if (B_ActionButtonClicked)
            {
                switch (ActionButtonState)
                {
                    case ActionButton.StartDatalog:
                        ControllerObj.EnableXYZ8StreamData();
                        AccelControllerGroup.Enabled = false;
                        ActionButtonState = ActionButton.StopDatalog;
                        menuStrip1.Enabled = false;
                        break;

                    case ActionButton.StopDatalog:
                        ActionButtonState = ActionButton.StartDatalog;
                        AccelControllerGroup.Enabled = true;
                        menuStrip1.Enabled = true;
                        break;
                }
            }
            B_ActionButtonClicked = false;
            if (blDirTapChange)
            {
                blDirTapChange = false;
                textBox2.Text = sDirTap;
            }
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

