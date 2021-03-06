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

    public class DirectionalTapDemo : Form
    {
        private GroupBox AccelControllerGroup;
        private int ActiveDynamicRange;
        private bool B_btnAdvanced = false;
        private bool B_btnDisplay = true;
        private bool B_btnRegisters = false;
        private bool blDirShakeChange = false;
        private Button btn12;
        private Button btn13;
        private Button btn14;
        private Button btn22;
        private Button btn23;
        private Button btn24;
        private Button btn32;
        private Button btn33;
        private Button btn34;
        private Button btnAdvanced;
        private Button btnAutoCal;
        private Button btnDisplay;
        private Button btnPulseResetTime2ndPulse;
        private Button btnPulseSetTime2ndPulse;
        private Button btnRead;
        private Button btnRegisters;
        private Button btnResetFirstPulseTimeLimit;
        private Button btnResetPulseThresholds;
        private Button btnSetFirstPulseTimeLimit;
        private Button btnSetPulseThresholds;
        private Button btnUpdateRegs;
        private Button btnWrite;
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
        private ComboBox ddlDataRate;
        private int DeviceID;
        private double DR_PulseTimeStepNoLPF;
        private double DR_timestep;
        private BoardComm dv;
        private ToolStripMenuItem enableLogDataApplicationToolStripMenuItem;
        private const int Form_Max_Height = 720;
        private const int Form_Min_Height = 200;
        private int FullScaleValue;
        private GroupBox gbDR;
        private GroupBox gbOM;
        private GroupBox groupBox4;
        private int I_panelAdvanced_height;
        private int I_panelDisplay_height;
        private int I_panelRegisters_height;
        private Image ImageGreen;
        private Image ImageRed;
        private Image ImageYellow;
        private int iShakeStat = 0;
        private int keypad_location;
        private Label label10;
        private Label label15;
        private Label label173;
        private Label label255;
        private Label label35;
        private Label label42;
        private Label label49;
        private Label label51;
        private Label label6;
        private Label label70;
        private Label label71;
        private Label label8;
        private Label label9;
        private Label lbl2gSensitivity;
        private Label lbl4gSensitivity;
        private Label lbl8gSensitivity;
        private Label lblFirstPulseTimeLimit;
        private Label lblFirstPulseTimeLimitms;
        private Label lblFirstTimeLimitVal;
        private Label lblNumber;
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
        private Label lblRegValue;
        private Label lbsleep;
        private Led ledPulseDouble;
        private Led ledPX;
        private Led ledPY;
        private Led ledPZ;
        private Led ledSleep;
        private Led ledStandby;
        private Led ledTapEA;
        private Led ledWake;
        private bool LoadingFW = false;
        private ToolStripMenuItem logDataToolStripMenuItem1;
        private MenuStrip menuStrip1;
        private object objectLock = new object();
        private Panel p1;
        private Panel p10;
        private Panel p12;
        private Panel panel15;
        private Panel panelAdvanced;
        private Panel panelDisplay;
        private FlowLayoutPanel panelGeneral;
        private Panel panelRegisters;
        private PictureBox pictureBox1;
        private Panel pOverSampling;
        private Panel pTapStatus;
        private double pulse_step;
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
        private SaveFileDialog saveFileDialog1;
        private string sPPolX;
        private string sPPolY;
        private string sPPolZ;
        private string stringNumber;
        private TrackBar tbFirstPulseTimeLimit;
        private TrackBar tbPulse2ndPulseWin;
        private TrackBar tbPulseLatency;
        private TrackBar tbPulseXThreshold;
        private TrackBar tbPulseYThreshold;
        private TrackBar tbPulseZThreshold;
        private int timercount;
        private System.Windows.Forms.Timer tmrTapTimer;
        private ToolStripStatusLabel toolStripStatusLabel;
        private TextBox txtboxRegisters;
        private TextBox txtRegRead;
        private TextBox txtRegValue;
        private TextBox txtRegWrite;
        private int WakeOSMode;
        private bool windowResized = false;
        private int x_position;
        private int y_position;

        public DirectionalTapDemo(object controllerObj)
        {
            InitializeComponent();
            Styles.FormatForm(this);
            ControllerObj = (AccelController) controllerObj;
            ControllerObj.ControllerEvents += new AccelController.ControllerEventHandler(ControllerObj_ControllerEvents);
            menuStrip1.Enabled = false;
            I_panelDisplay_height = panelDisplay.Height;
            I_panelAdvanced_height = panelAdvanced.Height;
            I_panelRegisters_height = panelRegisters.Height;
            timercount = 0;
            B_btnDisplay = false;
            B_btnAdvanced = true;
            B_btnRegisters = true;
            ControllerObj.StartDevice();
            Thread.Sleep(10);
            ControllerObj.Boot();
            Thread.Sleep(20);
            DeviceID = (int) ControllerObj.DeviceID;
            WakeOSMode = 0;
            rdoOSNormalMode.Checked = true;
            rdoOSNormalMode_CheckedChanged(this, null);
            ddlDataRate.SelectedIndex = 0;
            chkPulseEnableLatch.Checked = true;
            chkPulseEnableLatch_CheckedChanged(this, null);
            DR_timestep = 1.25;
            rdo8g.Checked = true;
            rdo8g_CheckedChanged(this, null);
            FullScaleValue = 2;
            ActiveDynamicRange = 8;
            rdoDefaultSPDP.Checked = true;
            rdoDefaultSPDP_CheckedChanged(this, null);
            SetPulseInterrupts(true);
            rdoActive.Checked = true;
            rdoActive_CheckedChanged(this, null);
            UpdateFormState();
            InitializeStatusBar();
            tmrTapTimer.Enabled = true;
            tmrTapTimer.Start();
            ControllerObj.ReturnPulseStatus = true;
        }

        private void btnAdvanced_Click(object sender, EventArgs e)
        {
            B_btnAdvanced = !B_btnAdvanced;
            windowResized = false;
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

        private void btnDisplay_Click(object sender, EventArgs e)
        {
            B_btnDisplay = !B_btnDisplay;
            windowResized = false;
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

        private void btnRead_Click(object sender, EventArgs e)
        {
            int num = Convert.ToInt32(txtRegRead.Text, 0x10);
            int[] datapassed = new int[] { num };
            ControllerObj.ReadValueFlag = true;
            ControllerReqPacket reqName = new ControllerReqPacket();
            ControllerObj.CreateNewTaskFromGUI(reqName, 0, 0x11, datapassed);
        }

        private void btnRegisters_Click(object sender, EventArgs e)
        {
            B_btnRegisters = !B_btnRegisters;
            windowResized = false;
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

        private void btnUpdateRegs_Click(object sender, EventArgs e)
        {
            int[] datapassed = new int[] { 0 };
            ControllerReqPacket reqName = new ControllerReqPacket();
            ControllerObj.CreateNewTaskFromGUI(reqName, 0, 0x10, datapassed);
            txtboxRegisters.Text = "Reading data...";
        }

        private void btnWrite_Click(object sender, EventArgs e)
        {
            int num = Convert.ToInt32(txtRegWrite.Text, 0x10);
            int num2 = Convert.ToInt32(txtRegValue.Text, 0x10);
            int[] datapassed = new int[] { num, num2 };
            ControllerObj.WriteValueFlag = true;
            ControllerReqPacket reqName = new ControllerReqPacket();
            ControllerObj.CreateNewTaskFromGUI(reqName, 0, 0x12, datapassed);
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

        private void ControllerObj_ControllerEvents(ControllerEventType evt, object o)
        {
            int num = (int) evt;
            if (num == 7)
            {
                GUIUpdatePacket packet = (GUIUpdatePacket) o;
                if (packet.PayLoad.Count > 0)
                {
                    int[] numArray = (int[]) packet.PayLoad.Dequeue();
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
                        ledPX.Value = true;
                    }
                    else
                    {
                        ledPX.Value = false;
                    }
                    if ((numArray[0] & 8) != 0)
                    {
                        ledPulseDouble.Value = true;
                    }
                    else
                    {
                        ledPulseDouble.Value = false;
                    }
                    if ((numArray[0] & 4) == 0)
                    {
                        sPPolZ = Convert.ToString("Positive");
                    }
                    else
                    {
                        sPPolZ = Convert.ToString("Negative");
                    }
                    if ((numArray[0] & 2) == 0)
                    {
                        sPPolY = Convert.ToString("Positive");
                    }
                    else
                    {
                        sPPolY = Convert.ToString("Negative");
                    }
                    if ((numArray[0] & 1) == 0)
                    {
                        sPPolX = Convert.ToString("Positive");
                    }
                    else
                    {
                        sPPolX = Convert.ToString("Negative");
                    }
                }
                if (ledPZ.Value && ledPulseDouble.Value)
                {
                    stringNumber = string.Format("{0:X1}", keypad_location);
                }
                if (!ledPX.Value || ledPY.Value)
                {
                    if (ledPY.Value && !ledPX.Value)
                    {
                        if (lblPPolY.Text == "Positive")
                        {
                            y_position++;
                            if (y_position > 4)
                            {
                                y_position = 4;
                            }
                        }
                        else
                        {
                            y_position--;
                            if (y_position < 1)
                            {
                                y_position = 1;
                            }
                        }
                        switch (x_position)
                        {
                            case 1:
                                switch (y_position)
                                {
                                    case 2:
                                        keypad_location = 1;
                                        btn12.BackColor = Color.Yellow;
                                        btn13.BackColor = Color.White;
                                        btn22.BackColor = Color.White;
                                        btn23.BackColor = Color.White;
                                        btn32.BackColor = Color.White;
                                        btn33.BackColor = Color.White;
                                        btn14.BackColor = Color.White;
                                        btn24.BackColor = Color.White;
                                        btn34.BackColor = Color.White;
                                        break;

                                    case 3:
                                        keypad_location = 4;
                                        btn12.BackColor = Color.White;
                                        btn13.BackColor = Color.Yellow;
                                        btn22.BackColor = Color.White;
                                        btn23.BackColor = Color.White;
                                        btn32.BackColor = Color.White;
                                        btn33.BackColor = Color.White;
                                        btn14.BackColor = Color.White;
                                        btn24.BackColor = Color.White;
                                        btn34.BackColor = Color.White;
                                        break;

                                    case 4:
                                        keypad_location = 7;
                                        btn12.BackColor = Color.White;
                                        btn13.BackColor = Color.White;
                                        btn22.BackColor = Color.White;
                                        btn23.BackColor = Color.White;
                                        btn32.BackColor = Color.White;
                                        btn33.BackColor = Color.White;
                                        btn14.BackColor = Color.Yellow;
                                        btn24.BackColor = Color.White;
                                        btn34.BackColor = Color.White;
                                        break;
                                }
                                break;

                            case 2:
                                switch (y_position)
                                {
                                    case 2:
                                        keypad_location = 2;
                                        btn12.BackColor = Color.White;
                                        btn13.BackColor = Color.White;
                                        btn22.BackColor = Color.Yellow;
                                        btn23.BackColor = Color.White;
                                        btn32.BackColor = Color.White;
                                        btn33.BackColor = Color.White;
                                        btn14.BackColor = Color.White;
                                        btn24.BackColor = Color.White;
                                        btn34.BackColor = Color.White;
                                        break;

                                    case 3:
                                        keypad_location = 5;
                                        btn12.BackColor = Color.White;
                                        btn13.BackColor = Color.White;
                                        btn22.BackColor = Color.White;
                                        btn23.BackColor = Color.Yellow;
                                        btn32.BackColor = Color.White;
                                        btn33.BackColor = Color.White;
                                        btn14.BackColor = Color.White;
                                        btn24.BackColor = Color.White;
                                        btn34.BackColor = Color.White;
                                        break;

                                    case 4:
                                        keypad_location = 8;
                                        btn12.BackColor = Color.White;
                                        btn13.BackColor = Color.White;
                                        btn22.BackColor = Color.White;
                                        btn23.BackColor = Color.White;
                                        btn32.BackColor = Color.White;
                                        btn33.BackColor = Color.White;
                                        btn14.BackColor = Color.White;
                                        btn24.BackColor = Color.Yellow;
                                        btn34.BackColor = Color.White;
                                        break;
                                }
                                break;

                            case 3:
                                switch (y_position)
                                {
                                    case 2:
                                        keypad_location = 3;
                                        btn12.BackColor = Color.White;
                                        btn13.BackColor = Color.White;
                                        btn22.BackColor = Color.White;
                                        btn23.BackColor = Color.White;
                                        btn32.BackColor = Color.Yellow;
                                        btn33.BackColor = Color.White;
                                        btn14.BackColor = Color.White;
                                        btn24.BackColor = Color.White;
                                        btn34.BackColor = Color.White;
                                        break;

                                    case 3:
                                        keypad_location = 6;
                                        btn12.BackColor = Color.White;
                                        btn13.BackColor = Color.White;
                                        btn22.BackColor = Color.White;
                                        btn23.BackColor = Color.White;
                                        btn32.BackColor = Color.White;
                                        btn33.BackColor = Color.Yellow;
                                        btn14.BackColor = Color.White;
                                        btn24.BackColor = Color.White;
                                        btn34.BackColor = Color.White;
                                        break;

                                    case 4:
                                        keypad_location = 9;
                                        btn12.BackColor = Color.White;
                                        btn13.BackColor = Color.White;
                                        btn22.BackColor = Color.White;
                                        btn23.BackColor = Color.White;
                                        btn32.BackColor = Color.White;
                                        btn33.BackColor = Color.White;
                                        btn14.BackColor = Color.White;
                                        btn24.BackColor = Color.White;
                                        btn34.BackColor = Color.Yellow;
                                        break;
                                }
                                break;
                        }
                    }
                }
                else
                {
                    if (lblPPolX.Text == "Positive")
                    {
                        x_position++;
                        if (x_position >= 3)
                        {
                            x_position = 3;
                        }
                    }
                    else
                    {
                        x_position--;
                        if (x_position <= 1)
                        {
                            x_position = 1;
                        }
                    }
                    switch (x_position)
                    {
                        case 1:
                            switch (y_position)
                            {
                                case 2:
                                    keypad_location = 1;
                                    btn12.BackColor = Color.Yellow;
                                    btn13.BackColor = Color.White;
                                    btn22.BackColor = Color.White;
                                    btn23.BackColor = Color.White;
                                    btn32.BackColor = Color.White;
                                    btn33.BackColor = Color.White;
                                    btn14.BackColor = Color.White;
                                    btn24.BackColor = Color.White;
                                    btn34.BackColor = Color.White;
                                    break;

                                case 3:
                                    keypad_location = 4;
                                    btn12.BackColor = Color.White;
                                    btn13.BackColor = Color.Yellow;
                                    btn22.BackColor = Color.White;
                                    btn23.BackColor = Color.White;
                                    btn32.BackColor = Color.White;
                                    btn33.BackColor = Color.White;
                                    btn14.BackColor = Color.White;
                                    btn24.BackColor = Color.White;
                                    btn34.BackColor = Color.White;
                                    break;

                                case 4:
                                    keypad_location = 7;
                                    btn12.BackColor = Color.White;
                                    btn13.BackColor = Color.White;
                                    btn22.BackColor = Color.White;
                                    btn23.BackColor = Color.White;
                                    btn32.BackColor = Color.White;
                                    btn33.BackColor = Color.White;
                                    btn14.BackColor = Color.Yellow;
                                    btn24.BackColor = Color.White;
                                    btn34.BackColor = Color.White;
                                    break;
                            }
                            break;

                        case 2:
                            switch (y_position)
                            {
                                case 2:
                                    keypad_location = 2;
                                    btn12.BackColor = Color.White;
                                    btn13.BackColor = Color.White;
                                    btn22.BackColor = Color.Yellow;
                                    btn23.BackColor = Color.White;
                                    btn32.BackColor = Color.White;
                                    btn33.BackColor = Color.White;
                                    btn14.BackColor = Color.White;
                                    btn24.BackColor = Color.White;
                                    btn34.BackColor = Color.White;
                                    break;

                                case 3:
                                    keypad_location = 5;
                                    btn12.BackColor = Color.White;
                                    btn13.BackColor = Color.White;
                                    btn22.BackColor = Color.White;
                                    btn23.BackColor = Color.Yellow;
                                    btn32.BackColor = Color.White;
                                    btn33.BackColor = Color.White;
                                    btn14.BackColor = Color.White;
                                    btn24.BackColor = Color.White;
                                    btn34.BackColor = Color.White;
                                    break;

                                case 4:
                                    keypad_location = 8;
                                    btn12.BackColor = Color.White;
                                    btn13.BackColor = Color.White;
                                    btn22.BackColor = Color.White;
                                    btn23.BackColor = Color.White;
                                    btn32.BackColor = Color.White;
                                    btn33.BackColor = Color.White;
                                    btn14.BackColor = Color.White;
                                    btn24.BackColor = Color.Yellow;
                                    btn34.BackColor = Color.White;
                                    break;
                            }
                            break;

                        case 3:
                            switch (y_position)
                            {
                                case 2:
                                    keypad_location = 3;
                                    btn12.BackColor = Color.White;
                                    btn13.BackColor = Color.White;
                                    btn22.BackColor = Color.White;
                                    btn23.BackColor = Color.White;
                                    btn32.BackColor = Color.Yellow;
                                    btn33.BackColor = Color.White;
                                    btn14.BackColor = Color.White;
                                    btn24.BackColor = Color.White;
                                    btn34.BackColor = Color.White;
                                    break;

                                case 3:
                                    keypad_location = 6;
                                    btn12.BackColor = Color.White;
                                    btn13.BackColor = Color.White;
                                    btn22.BackColor = Color.White;
                                    btn23.BackColor = Color.White;
                                    btn32.BackColor = Color.White;
                                    btn33.BackColor = Color.Yellow;
                                    btn14.BackColor = Color.White;
                                    btn24.BackColor = Color.White;
                                    btn34.BackColor = Color.White;
                                    break;

                                case 4:
                                    keypad_location = 9;
                                    btn12.BackColor = Color.White;
                                    btn13.BackColor = Color.White;
                                    btn22.BackColor = Color.White;
                                    btn23.BackColor = Color.White;
                                    btn32.BackColor = Color.White;
                                    btn33.BackColor = Color.White;
                                    btn14.BackColor = Color.White;
                                    btn24.BackColor = Color.White;
                                    btn34.BackColor = Color.Yellow;
                                    break;
                            }
                            break;
                    }
                }
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
							break;

                        case 3:
                            DR_timestep = 5.0;
                            DR_PulseTimeStepNoLPF = 2.5;
							break;
						default:
							DR_timestep = 5.0;
							DR_PulseTimeStepNoLPF = 1.25;
							break;
					}
                    break;

                case 3:
					switch (WakeOSMode)
					{
						case 2:
							DR_timestep = 2.5;
							DR_PulseTimeStepNoLPF = 0.625;
							break;

						case 3:
							DR_timestep = 10.0;
							DR_PulseTimeStepNoLPF = 5.0;
							break;
						default:
							DR_timestep = 10.0;
							DR_PulseTimeStepNoLPF = 2.5;
							break;
					}
                    break;

                case 4:
					switch (WakeOSMode)
					{
						case 2:
							DR_timestep = 2.5;
							DR_PulseTimeStepNoLPF = 0.625;
							break;
						case 3:
							DR_timestep = 20.0;
							DR_PulseTimeStepNoLPF = 10.0;
							break;
						default:
							DR_timestep = 20.0;
							DR_PulseTimeStepNoLPF = 5.0;
							break;
					}
                    break;

                case 5:
                    DR_timestep = 80.0;
                    switch (WakeOSMode)
                    {
                        case 0:
                            DR_timestep = 20.0;
                            DR_PulseTimeStepNoLPF = 5.0;
							break;

                        case 1:
                            DR_timestep = 80.0;
                            DR_PulseTimeStepNoLPF = 20.0;
							break;

                        case 2:
                            DR_timestep = 2.5;
                            DR_PulseTimeStepNoLPF = 0.625;
							break;

                        case 3:
                            DR_timestep = 80.0;
                            DR_PulseTimeStepNoLPF = 40.0;
                            break;
                    }
                    break;

                case 6:
                    switch (WakeOSMode)
                    {
                        case 0:
                            DR_timestep = 20.0;
                            DR_PulseTimeStepNoLPF = 5.0;
							break;

                        case 1:
                            DR_timestep = 160.0;
                            DR_PulseTimeStepNoLPF = 40.0;
							break;

                        case 2:
                            DR_timestep = 2.5;
                            DR_PulseTimeStepNoLPF = 0.625;
							break;

                        case 3:
                            DR_timestep = 160.0;
                            DR_PulseTimeStepNoLPF = 80.0;
							break;
                    }
                    break;

                case 7:
                    switch (WakeOSMode)
                    {
                        case 0:
                            DR_timestep = 20.0;
                            DR_PulseTimeStepNoLPF = 5.0;
							break;

                        case 1:
                            DR_timestep = 640.0;
                            DR_PulseTimeStepNoLPF = 160.0;
							break;

                        case 2:
                            DR_timestep = 2.5;
                            DR_PulseTimeStepNoLPF = 0.625;
							break;

                        case 3:
                            DR_timestep = 640.0;
                            DR_PulseTimeStepNoLPF = 320.0;
							break;
                    }
                    break;
            }

            if (chkPulseLPFEnable.Checked)
                pulse_step = DR_timestep;
            else
                pulse_step = DR_PulseTimeStepNoLPF;

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
                    lblRegValue.Text = string.Format("{0:X2}", num);
                }
                if (packet.TaskID == 0x10)
                {
                    byte[] buffer = (byte[]) packet.PayLoad.Dequeue();
                    StringBuilder builder = new StringBuilder();
                    for (int i = 0; i < buffer.Length; i += 3)
                    {
                        builder.Append(string.Format("{0:X2}", i) + " " + RegisterNames[i] + "\t" + string.Format("{0:X2}", buffer[i]) + "\t\t");
                        if ((i + 1) < buffer.Length)
                            builder.Append(string.Format("{0:X2}", i + 1) + " " + RegisterNames[i + 1] + "\t" + string.Format("{0:X2}", buffer[i + 1]) + "\t\t");
                        if ((i + 2) < buffer.Length)
                            builder.Append(string.Format("{0:X2}", i + 2) + " " + RegisterNames[i + 2] + "\t" + string.Format("{0:X2}", buffer[i + 2]) + "\r\n");
                    }
                    txtboxRegisters.Text = builder.ToString();
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

        public void EndDemo()
        {
            lock (ControllerEventsLock)
            {
                tmrTapTimer.Stop();
                ControllerObj.ResetDevice();
                ControllerObj.ControllerEvents -= new AccelController.ControllerEventHandler(ControllerObj_ControllerEvents);
                base.Close();
            }
        }

        private void InitializeComponent()
        {
            components = new Container();
            ComponentResourceManager resources = new ComponentResourceManager(typeof(DirectionalTapDemo));
            tmrTapTimer = new System.Windows.Forms.Timer(components);
            panelGeneral = new FlowLayoutPanel();
            panelDisplay = new Panel();
            pTapStatus = new Panel();
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
            lblNumber = new Label();
            btn32 = new Button();
            btn22 = new Button();
            btn12 = new Button();
            btn34 = new Button();
            btn24 = new Button();
            btn14 = new Button();
            btn33 = new Button();
            btn23 = new Button();
            btn13 = new Button();
            panelAdvanced = new Panel();
            btnResetPulseThresholds = new Button();
            btnSetPulseThresholds = new Button();
            panel15 = new Panel();
            chkPulseLPFEnable = new CheckBox();
            chkPulseHPFBypass = new CheckBox();
            rdoDefaultSPDP = new RadioButton();
            rdoDefaultSP = new RadioButton();
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
            label71 = new Label();
            rdoStandby = new RadioButton();
            rdoActive = new RadioButton();
            ledWake = new Led();
            ledSleep = new Led();
            ledStandby = new Led();
            lbsleep = new Label();
            label173 = new Label();
            panelRegisters = new Panel();
            txtboxRegisters = new TextBox();
            groupBox4 = new GroupBox();
            btnWrite = new Button();
            lblRegValue = new Label();
            btnRead = new Button();
            label51 = new Label();
            txtRegRead = new TextBox();
            label49 = new Label();
            txtRegWrite = new TextBox();
            txtRegValue = new TextBox();
            label42 = new Label();
            btnUpdateRegs = new Button();
            menuStrip1 = new MenuStrip();
            logDataToolStripMenuItem1 = new ToolStripMenuItem();
            enableLogDataApplicationToolStripMenuItem = new ToolStripMenuItem();
            pictureBox1 = new PictureBox();
            btnDisplay = new Button();
            btnAdvanced = new Button();
            btnRegisters = new Button();
            CommStrip = new StatusStrip();
            CommStripButton = new ToolStripDropDownButton();
            toolStripStatusLabel = new ToolStripStatusLabel();
            saveFileDialog1 = new SaveFileDialog();
            panelGeneral.SuspendLayout();
            panelDisplay.SuspendLayout();
            pTapStatus.SuspendLayout();
            ((ISupportInitialize) ledPulseDouble).BeginInit();
            ((ISupportInitialize) ledTapEA).BeginInit();
            ((ISupportInitialize) ledPZ).BeginInit();
            ((ISupportInitialize) ledPX).BeginInit();
            ((ISupportInitialize) ledPY).BeginInit();
            panelAdvanced.SuspendLayout();
            panel15.SuspendLayout();
            p12.SuspendLayout();
            tbPulseLatency.BeginInit();
            tbPulse2ndPulseWin.BeginInit();
            tbPulseZThreshold.BeginInit();
            tbPulseXThreshold.BeginInit();
            tbPulseYThreshold.BeginInit();
            p10.SuspendLayout();
            tbFirstPulseTimeLimit.BeginInit();
            AccelControllerGroup.SuspendLayout();
            gbDR.SuspendLayout();
            gbOM.SuspendLayout();
            pOverSampling.SuspendLayout();
            p1.SuspendLayout();
            ((ISupportInitialize) ledWake).BeginInit();
            ((ISupportInitialize) ledSleep).BeginInit();
            ((ISupportInitialize) ledStandby).BeginInit();
            panelRegisters.SuspendLayout();
            groupBox4.SuspendLayout();
            menuStrip1.SuspendLayout();
            ((ISupportInitialize) pictureBox1).BeginInit();
            CommStrip.SuspendLayout();
            base.SuspendLayout();
            tmrTapTimer.Interval = 1;
            tmrTapTimer.Tick += new EventHandler(tmrTiltTimer_Tick);
            panelGeneral.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            panelGeneral.AutoScroll = true;
            panelGeneral.Controls.Add(panelDisplay);
            panelGeneral.Controls.Add(panelAdvanced);
            panelGeneral.Controls.Add(panelRegisters);
            panelGeneral.Location = new Point(1, 0x71);
            panelGeneral.Name = "panelGeneral";
            panelGeneral.Size = new Size(0x3c5, 0x1dd);
            panelGeneral.TabIndex = 0x47;
            panelDisplay.Controls.Add(pTapStatus);
            panelDisplay.Controls.Add(lblNumber);
            panelDisplay.Controls.Add(btn32);
            panelDisplay.Controls.Add(btn22);
            panelDisplay.Controls.Add(btn12);
            panelDisplay.Controls.Add(btn34);
            panelDisplay.Controls.Add(btn24);
            panelDisplay.Controls.Add(btn14);
            panelDisplay.Controls.Add(btn33);
            panelDisplay.Controls.Add(btn23);
            panelDisplay.Controls.Add(btn13);
            panelDisplay.ForeColor = Color.Maroon;
            panelDisplay.Location = new Point(3, 3);
            panelDisplay.Name = "panelDisplay";
            panelDisplay.Size = new Size(0x3b2, 280);
            panelDisplay.TabIndex = 0x49;
            pTapStatus.BackColor = Color.LightSlateGray;
            pTapStatus.Controls.Add(label15);
            pTapStatus.Controls.Add(ledPulseDouble);
            pTapStatus.Controls.Add(lblPPolZ);
            pTapStatus.Controls.Add(lblPPolY);
            pTapStatus.Controls.Add(lblPPolX);
            pTapStatus.Controls.Add(label255);
            pTapStatus.Controls.Add(label6);
            pTapStatus.Controls.Add(ledTapEA);
            pTapStatus.Controls.Add(label8);
            pTapStatus.Controls.Add(ledPZ);
            pTapStatus.Controls.Add(ledPX);
            pTapStatus.Controls.Add(label9);
            pTapStatus.Controls.Add(label10);
            pTapStatus.Controls.Add(ledPY);
            pTapStatus.Location = new Point(12, 3);
            pTapStatus.Name = "pTapStatus";
            pTapStatus.Size = new Size(0xdd, 0x108);
            pTapStatus.TabIndex = 0x10f;
            label15.AutoSize = true;
            label15.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label15.ForeColor = Color.White;
            label15.Location = new Point(11, 40);
            label15.Name = "label15";
            label15.Size = new Size(0x65, 20);
            label15.TabIndex = 0xaf;
            label15.Text = "Double Tap";
            ledPulseDouble.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            ledPulseDouble.ForeColor = Color.White;
            ledPulseDouble.LedStyle = LedStyle.Round3D;
            ledPulseDouble.Location = new Point(0x79, 0x1f);
            ledPulseDouble.Name = "ledPulseDouble";
            ledPulseDouble.OffColor = Color.Red;
            ledPulseDouble.Size = new Size(40, 40);
            ledPulseDouble.TabIndex = 0xae;
            lblPPolZ.AutoSize = true;
            lblPPolZ.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPPolZ.ForeColor = Color.White;
            lblPPolZ.Location = new Point(0x74, 170);
            lblPPolZ.Name = "lblPPolZ";
            lblPPolZ.Size = new Size(0x61, 20);
            lblPPolZ.TabIndex = 0xab;
            lblPPolZ.Text = "Direction Z";
            lblPPolY.AutoSize = true;
            lblPPolY.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPPolY.ForeColor = Color.White;
            lblPPolY.Location = new Point(0x74, 130);
            lblPPolY.Name = "lblPPolY";
            lblPPolY.Size = new Size(0x62, 20);
            lblPPolY.TabIndex = 170;
            lblPPolY.Text = "Direction Y";
            lblPPolX.AutoSize = true;
            lblPPolX.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPPolX.ForeColor = Color.White;
            lblPPolX.Location = new Point(0x74, 0x5e);
            lblPPolX.Name = "lblPPolX";
            lblPPolX.Size = new Size(0x62, 20);
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
            label6.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label6.ForeColor = Color.White;
            label6.Location = new Point(6, 0xdd);
            label6.Name = "label6";
            label6.Size = new Size(0x86, 20);
            label6.TabIndex = 0xa5;
            label6.Text = "Event Detected";
            ledTapEA.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            ledTapEA.ForeColor = Color.White;
            ledTapEA.LedStyle = LedStyle.Round3D;
            ledTapEA.Location = new Point(0x94, 0xc5);
            ledTapEA.Name = "ledTapEA";
            ledTapEA.OffColor = Color.Red;
            ledTapEA.Size = new Size(60, 60);
            ledTapEA.TabIndex = 0xa4;
            label8.AutoSize = true;
            label8.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label8.ForeColor = Color.White;
            label8.Location = new Point(12, 170);
            label8.Name = "label8";
            label8.Size = new Size(0x3b, 20);
            label8.TabIndex = 0xa7;
            label8.Text = "Z-Axis";
            ledPZ.BlinkMode = LedBlinkMode.BlinkWhenOn;
            ledPZ.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            ledPZ.ForeColor = Color.White;
            ledPZ.LedStyle = LedStyle.Round3D;
            ledPZ.Location = new Point(0x4a, 160);
            ledPZ.Name = "ledPZ";
            ledPZ.OffColor = Color.Red;
            ledPZ.Size = new Size(40, 40);
            ledPZ.TabIndex = 0xa6;
            ledPX.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            ledPX.ForeColor = Color.White;
            ledPX.LedStyle = LedStyle.Round3D;
            ledPX.Location = new Point(0x4a, 0x56);
            ledPX.Name = "ledPX";
            ledPX.OffColor = Color.Red;
            ledPX.Size = new Size(40, 40);
            ledPX.TabIndex = 0x77;
            label9.AutoSize = true;
            label9.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label9.ForeColor = Color.White;
            label9.Location = new Point(12, 0x89);
            label9.Name = "label9";
            label9.Size = new Size(60, 20);
            label9.TabIndex = 0xa5;
            label9.Text = "Y-Axis";
            label10.AutoSize = true;
            label10.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label10.ForeColor = Color.White;
            label10.Location = new Point(12, 0x63);
            label10.Name = "label10";
            label10.Size = new Size(60, 20);
            label10.TabIndex = 0xa3;
            label10.Text = "X-Axis";
            ledPY.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            ledPY.ForeColor = Color.White;
            ledPY.LedStyle = LedStyle.Round3D;
            ledPY.Location = new Point(0x4a, 0x7c);
            ledPY.Name = "ledPY";
            ledPY.OffColor = Color.Red;
            ledPY.Size = new Size(40, 40);
            ledPY.TabIndex = 0xa4;
            lblNumber.AutoSize = true;
            lblNumber.Font = new Font("Microsoft Sans Serif", 21.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblNumber.Location = new Point(0x1d0, 0x18);
            lblNumber.Name = "lblNumber";
            lblNumber.Size = new Size(0x1f, 0x21);
            lblNumber.TabIndex = 0x10b;
            lblNumber.Text = "0";
            btn32.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn32.Location = new Point(0x1f5, 0xa3);
            btn32.Name = "btn32";
            btn32.Size = new Size(0x2d, 0x2a);
            btn32.TabIndex = 0x107;
            btn32.Text = "3";
            btn32.UseVisualStyleBackColor = true;
            btn22.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn22.Location = new Point(0x1c7, 0xa3);
            btn22.Name = "btn22";
            btn22.Size = new Size(0x2d, 0x2a);
            btn22.TabIndex = 0x106;
            btn22.Text = "2";
            btn22.UseVisualStyleBackColor = true;
            btn12.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn12.Location = new Point(410, 0xa3);
            btn12.Name = "btn12";
            btn12.Size = new Size(0x2d, 0x2a);
            btn12.TabIndex = 0x105;
            btn12.Text = "1";
            btn12.UseVisualStyleBackColor = true;
            btn34.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn34.Location = new Point(0x1f5, 0x4e);
            btn34.Name = "btn34";
            btn34.Size = new Size(0x2d, 0x2a);
            btn34.TabIndex = 260;
            btn34.Text = "9";
            btn34.UseVisualStyleBackColor = true;
            btn24.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn24.Location = new Point(0x1c7, 0x4e);
            btn24.Name = "btn24";
            btn24.Size = new Size(0x2d, 0x2a);
            btn24.TabIndex = 0x103;
            btn24.Text = "8";
            btn24.UseVisualStyleBackColor = true;
            btn14.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn14.Location = new Point(410, 0x4e);
            btn14.Name = "btn14";
            btn14.Size = new Size(0x2d, 0x2a);
            btn14.TabIndex = 0x102;
            btn14.Text = "7";
            btn14.UseVisualStyleBackColor = true;
            btn33.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn33.Location = new Point(0x1f5, 0x79);
            btn33.Name = "btn33";
            btn33.Size = new Size(0x2d, 0x2a);
            btn33.TabIndex = 0x101;
            btn33.Text = "6";
            btn33.UseVisualStyleBackColor = true;
            btn23.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn23.Location = new Point(0x1c7, 0x79);
            btn23.Name = "btn23";
            btn23.Size = new Size(0x2d, 0x2a);
            btn23.TabIndex = 0x100;
            btn23.Text = "5";
            btn23.UseVisualStyleBackColor = true;
            btn13.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn13.Location = new Point(410, 0x79);
            btn13.Name = "btn13";
            btn13.Size = new Size(0x2d, 0x2a);
            btn13.TabIndex = 0xff;
            btn13.Text = "4";
            btn13.UseVisualStyleBackColor = true;
            panelAdvanced.Controls.Add(btnResetPulseThresholds);
            panelAdvanced.Controls.Add(btnSetPulseThresholds);
            panelAdvanced.Controls.Add(panel15);
            panelAdvanced.Controls.Add(rdoDefaultSPDP);
            panelAdvanced.Controls.Add(rdoDefaultSP);
            panelAdvanced.Controls.Add(p12);
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
            panelAdvanced.Controls.Add(AccelControllerGroup);
            panelAdvanced.Location = new Point(3, 0x121);
            panelAdvanced.Name = "panelAdvanced";
            panelAdvanced.Size = new Size(0x3ae, 0x27a);
            panelAdvanced.TabIndex = 0x4c;
            btnResetPulseThresholds.BackColor = Color.LightSlateGray;
            btnResetPulseThresholds.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnResetPulseThresholds.ForeColor = Color.White;
            btnResetPulseThresholds.Location = new Point(710, 0x15d);
            btnResetPulseThresholds.Name = "btnResetPulseThresholds";
            btnResetPulseThresholds.Size = new Size(0xb0, 0x1f);
            btnResetPulseThresholds.TabIndex = 0x101;
            btnResetPulseThresholds.Text = "Reset XYZ Thresholds";
            btnResetPulseThresholds.UseVisualStyleBackColor = false;
            btnResetPulseThresholds.Click += new EventHandler(btnResetPulseThresholds_Click);
            btnSetPulseThresholds.BackColor = Color.LightSlateGray;
            btnSetPulseThresholds.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSetPulseThresholds.ForeColor = Color.White;
            btnSetPulseThresholds.Location = new Point(0x221, 0x15d);
            btnSetPulseThresholds.Name = "btnSetPulseThresholds";
            btnSetPulseThresholds.Size = new Size(0x9b, 0x1f);
            btnSetPulseThresholds.TabIndex = 0x100;
            btnSetPulseThresholds.Text = "Set XYZ Thresholds";
            btnSetPulseThresholds.UseVisualStyleBackColor = false;
            btnSetPulseThresholds.Click += new EventHandler(btnSetPulseThresholds_Click);
            panel15.Controls.Add(chkPulseLPFEnable);
            panel15.Controls.Add(chkPulseHPFBypass);
            panel15.Location = new Point(0x232, 0xdf);
            panel15.Name = "panel15";
            panel15.Size = new Size(0x15c, 0x27);
            panel15.TabIndex = 0xff;
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
            rdoDefaultSPDP.Location = new Point(710, 0xaf);
            rdoDefaultSPDP.Name = "rdoDefaultSPDP";
            rdoDefaultSPDP.Size = new Size(0xdd, 20);
            rdoDefaultSPDP.TabIndex = 0xfe;
            rdoDefaultSPDP.TabStop = true;
            rdoDefaultSPDP.Text = "Default Single + Double Tap";
            rdoDefaultSPDP.UseVisualStyleBackColor = true;
            rdoDefaultSPDP.CheckedChanged += new EventHandler(rdoDefaultSPDP_CheckedChanged);
            rdoDefaultSP.AutoSize = true;
            rdoDefaultSP.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            rdoDefaultSP.ForeColor = Color.Gold;
            rdoDefaultSP.Location = new Point(0x225, 0xae);
            rdoDefaultSP.Name = "rdoDefaultSP";
            rdoDefaultSP.Size = new Size(0x9b, 20);
            rdoDefaultSP.TabIndex = 0xfd;
            rdoDefaultSP.TabStop = true;
            rdoDefaultSP.Text = "Default Single Tap";
            rdoDefaultSP.UseVisualStyleBackColor = true;
            rdoDefaultSP.CheckedChanged += new EventHandler(rdoDefaultSP_CheckedChanged);
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
            p12.Location = new Point(7, 0x1ba);
            p12.Name = "p12";
            p12.Size = new Size(0x214, 180);
            p12.TabIndex = 0xfc;
            btnPulseResetTime2ndPulse.BackColor = Color.LightSlateGray;
            btnPulseResetTime2ndPulse.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnPulseResetTime2ndPulse.ForeColor = Color.White;
            btnPulseResetTime2ndPulse.Location = new Point(370, 0x87);
            btnPulseResetTime2ndPulse.Name = "btnPulseResetTime2ndPulse";
            btnPulseResetTime2ndPulse.Size = new Size(150, 0x1f);
            btnPulseResetTime2ndPulse.TabIndex = 0x9f;
            btnPulseResetTime2ndPulse.Text = "Reset Time Limits";
            btnPulseResetTime2ndPulse.UseVisualStyleBackColor = false;
            btnPulseResetTime2ndPulse.Click += new EventHandler(btnPulseResetTime2ndPulse_Click);
            btnPulseSetTime2ndPulse.BackColor = Color.LightSlateGray;
            btnPulseSetTime2ndPulse.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnPulseSetTime2ndPulse.ForeColor = Color.White;
            btnPulseSetTime2ndPulse.Location = new Point(0xd3, 0x87);
            btnPulseSetTime2ndPulse.Name = "btnPulseSetTime2ndPulse";
            btnPulseSetTime2ndPulse.Size = new Size(0x7b, 0x1f);
            btnPulseSetTime2ndPulse.TabIndex = 0x9e;
            btnPulseSetTime2ndPulse.Text = "Set Time Limits";
            btnPulseSetTime2ndPulse.UseVisualStyleBackColor = false;
            btnPulseSetTime2ndPulse.Click += new EventHandler(btnPulseSetTime2ndPulse_Click);
            tbPulseLatency.Location = new Point(0xbc, 0x25);
            tbPulseLatency.Maximum = 0xff;
            tbPulseLatency.Name = "tbPulseLatency";
            tbPulseLatency.Size = new Size(0x14c, 0x2d);
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
            lblPulse2ndPulseWinms.Location = new Point(0xa3, 0x5d);
            lblPulse2ndPulseWinms.Name = "lblPulse2ndPulseWinms";
            lblPulse2ndPulseWinms.Size = new Size(0x18, 15);
            lblPulse2ndPulseWinms.TabIndex = 0x9d;
            lblPulse2ndPulseWinms.Text = "ms";
            lblPulseLatency.AutoSize = true;
            lblPulseLatency.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblPulseLatency.ForeColor = Color.White;
            lblPulseLatency.Location = new Point(6, 40);
            lblPulseLatency.Name = "lblPulseLatency";
            lblPulseLatency.Size = new Size(0x53, 15);
            lblPulseLatency.TabIndex = 150;
            lblPulseLatency.Text = "Pulse Latency";
            lblPulse2ndPulseWinVal.AutoSize = true;
            lblPulse2ndPulseWinVal.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblPulse2ndPulseWinVal.ForeColor = Color.White;
            lblPulse2ndPulseWinVal.Location = new Point(0x76, 0x5e);
            lblPulse2ndPulseWinVal.Name = "lblPulse2ndPulseWinVal";
            lblPulse2ndPulseWinVal.Size = new Size(14, 15);
            lblPulse2ndPulseWinVal.TabIndex = 0x9c;
            lblPulse2ndPulseWinVal.Text = "0";
            lblPulseLatencyVal.AutoSize = true;
            lblPulseLatencyVal.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblPulseLatencyVal.ForeColor = Color.White;
            lblPulseLatencyVal.Location = new Point(0x76, 40);
            lblPulseLatencyVal.Name = "lblPulseLatencyVal";
            lblPulseLatencyVal.Size = new Size(14, 15);
            lblPulseLatencyVal.TabIndex = 0x97;
            lblPulseLatencyVal.Text = "0";
            lblPulse2ndPulseWin.AutoSize = true;
            lblPulse2ndPulseWin.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblPulse2ndPulseWin.ForeColor = Color.White;
            lblPulse2ndPulseWin.Location = new Point(6, 0x5e);
            lblPulse2ndPulseWin.Name = "lblPulse2ndPulseWin";
            lblPulse2ndPulseWin.Size = new Size(0x6d, 15);
            lblPulse2ndPulseWin.TabIndex = 0x9b;
            lblPulse2ndPulseWin.Text = "2nd Pulse Window";
            lblPulseLatencyms.AutoSize = true;
            lblPulseLatencyms.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblPulseLatencyms.ForeColor = Color.White;
            lblPulseLatencyms.Location = new Point(0xa8, 0x25);
            lblPulseLatencyms.Name = "lblPulseLatencyms";
            lblPulseLatencyms.Size = new Size(0x18, 15);
            lblPulseLatencyms.TabIndex = 0x98;
            lblPulseLatencyms.Text = "ms";
            tbPulse2ndPulseWin.Location = new Point(0xbc, 0x58);
            tbPulse2ndPulseWin.Maximum = 0xff;
            tbPulse2ndPulseWin.Name = "tbPulse2ndPulseWin";
            tbPulse2ndPulseWin.Size = new Size(0x14c, 0x2d);
            tbPulse2ndPulseWin.TabIndex = 0x9a;
            tbPulse2ndPulseWin.Scroll += new EventHandler(tbPulse2ndPulseWin_Scroll);
            chkPulseIgnorLatentPulses.AutoSize = true;
            chkPulseIgnorLatentPulses.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
            chkPulseIgnorLatentPulses.ForeColor = Color.White;
            chkPulseIgnorLatentPulses.Location = new Point(0x14d, 4);
            chkPulseIgnorLatentPulses.Name = "chkPulseIgnorLatentPulses";
            chkPulseIgnorLatentPulses.Size = new Size(0x83, 0x13);
            chkPulseIgnorLatentPulses.TabIndex = 0x99;
            chkPulseIgnorLatentPulses.Text = "Ignor Latent Pulses";
            chkPulseIgnorLatentPulses.UseVisualStyleBackColor = true;
            chkPulseIgnorLatentPulses.CheckedChanged += new EventHandler(chkPulseIgnorLatentPulses_CheckedChanged);
            tbPulseZThreshold.BackColor = Color.LightSlateGray;
            tbPulseZThreshold.Location = new Point(0x9b, 0x182);
            tbPulseZThreshold.Maximum = 0x7f;
            tbPulseZThreshold.Name = "tbPulseZThreshold";
            tbPulseZThreshold.Size = new Size(0x174, 0x2d);
            tbPulseZThreshold.TabIndex = 0xf8;
            tbPulseZThreshold.Scroll += new EventHandler(tbPulseZThreshold_Scroll);
            tbPulseXThreshold.BackColor = Color.LightSlateGray;
            tbPulseXThreshold.Location = new Point(0x9b, 0x124);
            tbPulseXThreshold.Maximum = 0x7f;
            tbPulseXThreshold.Name = "tbPulseXThreshold";
            tbPulseXThreshold.Size = new Size(0x174, 0x2d);
            tbPulseXThreshold.TabIndex = 240;
            tbPulseXThreshold.Scroll += new EventHandler(tbPulseXThreshold_Scroll);
            lblPulseYThreshold.AutoSize = true;
            lblPulseYThreshold.BackColor = Color.LightSlateGray;
            lblPulseYThreshold.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPulseYThreshold.ForeColor = Color.White;
            lblPulseYThreshold.Location = new Point(9, 0x158);
            lblPulseYThreshold.Name = "lblPulseYThreshold";
            lblPulseYThreshold.Size = new Size(0x57, 15);
            lblPulseYThreshold.TabIndex = 0xf5;
            lblPulseYThreshold.Text = " Y Threshold";
            tbPulseYThreshold.BackColor = Color.LightSlateGray;
            tbPulseYThreshold.Location = new Point(0x9b, 340);
            tbPulseYThreshold.Maximum = 0x7f;
            tbPulseYThreshold.Name = "tbPulseYThreshold";
            tbPulseYThreshold.Size = new Size(0x174, 0x2d);
            tbPulseYThreshold.TabIndex = 0xf4;
            tbPulseYThreshold.Scroll += new EventHandler(tbPulseYThreshold_Scroll);
            lblPulseYThresholdVal.AutoSize = true;
            lblPulseYThresholdVal.BackColor = Color.LightSlateGray;
            lblPulseYThresholdVal.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPulseYThresholdVal.ForeColor = Color.White;
            lblPulseYThresholdVal.Location = new Point(0x66, 0x158);
            lblPulseYThresholdVal.Name = "lblPulseYThresholdVal";
            lblPulseYThresholdVal.Size = new Size(15, 15);
            lblPulseYThresholdVal.TabIndex = 0xf6;
            lblPulseYThresholdVal.Text = "0";
            lblPulseXThresholdg.AutoSize = true;
            lblPulseXThresholdg.BackColor = Color.LightSlateGray;
            lblPulseXThresholdg.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPulseXThresholdg.ForeColor = Color.White;
            lblPulseXThresholdg.Location = new Point(0x91, 0x128);
            lblPulseXThresholdg.Name = "lblPulseXThresholdg";
            lblPulseXThresholdg.Size = new Size(15, 15);
            lblPulseXThresholdg.TabIndex = 0xf3;
            lblPulseXThresholdg.Text = "g";
            lblPulseXThresholdVal.AutoSize = true;
            lblPulseXThresholdVal.BackColor = Color.LightSlateGray;
            lblPulseXThresholdVal.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPulseXThresholdVal.ForeColor = Color.White;
            lblPulseXThresholdVal.Location = new Point(0x66, 0x12a);
            lblPulseXThresholdVal.Name = "lblPulseXThresholdVal";
            lblPulseXThresholdVal.Size = new Size(15, 15);
            lblPulseXThresholdVal.TabIndex = 0xf2;
            lblPulseXThresholdVal.Text = "0";
            lblPulseZThresholdg.AutoSize = true;
            lblPulseZThresholdg.BackColor = Color.LightSlateGray;
            lblPulseZThresholdg.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPulseZThresholdg.ForeColor = Color.White;
            lblPulseZThresholdg.Location = new Point(0x91, 0x183);
            lblPulseZThresholdg.Name = "lblPulseZThresholdg";
            lblPulseZThresholdg.Size = new Size(15, 15);
            lblPulseZThresholdg.TabIndex = 0xfb;
            lblPulseZThresholdg.Text = "g";
            lblPulseZThreshold.AutoSize = true;
            lblPulseZThreshold.BackColor = Color.LightSlateGray;
            lblPulseZThreshold.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPulseZThreshold.ForeColor = Color.White;
            lblPulseZThreshold.Location = new Point(9, 0x185);
            lblPulseZThreshold.Name = "lblPulseZThreshold";
            lblPulseZThreshold.Size = new Size(0x57, 15);
            lblPulseZThreshold.TabIndex = 0xf9;
            lblPulseZThreshold.Text = " Z Threshold";
            lblPulseZThresholdVal.AutoSize = true;
            lblPulseZThresholdVal.BackColor = Color.LightSlateGray;
            lblPulseZThresholdVal.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPulseZThresholdVal.ForeColor = Color.White;
            lblPulseZThresholdVal.Location = new Point(0x66, 0x185);
            lblPulseZThresholdVal.Name = "lblPulseZThresholdVal";
            lblPulseZThresholdVal.Size = new Size(15, 15);
            lblPulseZThresholdVal.TabIndex = 250;
            lblPulseZThresholdVal.Text = "0";
            lblPulseYThresholdg.AutoSize = true;
            lblPulseYThresholdg.BackColor = Color.LightSlateGray;
            lblPulseYThresholdg.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPulseYThresholdg.ForeColor = Color.White;
            lblPulseYThresholdg.Location = new Point(0x91, 0x156);
            lblPulseYThresholdg.Name = "lblPulseYThresholdg";
            lblPulseYThresholdg.Size = new Size(15, 15);
            lblPulseYThresholdg.TabIndex = 0xf7;
            lblPulseYThresholdg.Text = "g";
            lblPulseXThreshold.AutoSize = true;
            lblPulseXThreshold.BackColor = Color.LightSlateGray;
            lblPulseXThreshold.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPulseXThreshold.ForeColor = Color.White;
            lblPulseXThreshold.Location = new Point(9, 0x12a);
            lblPulseXThreshold.Name = "lblPulseXThreshold";
            lblPulseXThreshold.Size = new Size(0x58, 15);
            lblPulseXThreshold.TabIndex = 0xf1;
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
            p10.Location = new Point(3, 0xa7);
            p10.Name = "p10";
            p10.Size = new Size(0x213, 0x7c);
            p10.TabIndex = 0xef;
            btnResetFirstPulseTimeLimit.BackColor = Color.LightSlateGray;
            btnResetFirstPulseTimeLimit.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnResetFirstPulseTimeLimit.ForeColor = Color.White;
            btnResetFirstPulseTimeLimit.Location = new Point(0x180, 0x52);
            btnResetFirstPulseTimeLimit.Name = "btnResetFirstPulseTimeLimit";
            btnResetFirstPulseTimeLimit.Size = new Size(0x87, 0x1f);
            btnResetFirstPulseTimeLimit.TabIndex = 150;
            btnResetFirstPulseTimeLimit.Text = "Reset Time Limit";
            btnResetFirstPulseTimeLimit.UseVisualStyleBackColor = false;
            btnResetFirstPulseTimeLimit.Click += new EventHandler(btnResetFirstPulseTimeLimit_Click);
            btnSetFirstPulseTimeLimit.BackColor = Color.LightSlateGray;
            btnSetFirstPulseTimeLimit.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSetFirstPulseTimeLimit.ForeColor = Color.White;
            btnSetFirstPulseTimeLimit.Location = new Point(210, 0x52);
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
            chkPulseEnableLatch.Location = new Point(0x1aa, 5);
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
            chkPulseEnableYSP.Location = new Point(0x67, 6);
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
            chkPulseEnableZSP.Location = new Point(0xca, 6);
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
            tbFirstPulseTimeLimit.Size = new Size(0x151, 0x2d);
            tbFirstPulseTimeLimit.TabIndex = 0x90;
            tbFirstPulseTimeLimit.Scroll += new EventHandler(tbFirstPulseTimeLimit_Scroll);
            lblFirstPulseTimeLimitms.AutoSize = true;
            lblFirstPulseTimeLimitms.BackColor = Color.LightSlateGray;
            lblFirstPulseTimeLimitms.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblFirstPulseTimeLimitms.ForeColor = Color.White;
            lblFirstPulseTimeLimitms.Location = new Point(0xa7, 0x2c);
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
            AccelControllerGroup.Controls.Add(gbDR);
            AccelControllerGroup.Controls.Add(gbOM);
            AccelControllerGroup.Controls.Add(label71);
            AccelControllerGroup.Controls.Add(rdoStandby);
            AccelControllerGroup.Controls.Add(rdoActive);
            AccelControllerGroup.Controls.Add(ledWake);
            AccelControllerGroup.Controls.Add(ledSleep);
            AccelControllerGroup.Controls.Add(ledStandby);
            AccelControllerGroup.Controls.Add(lbsleep);
            AccelControllerGroup.Controls.Add(label173);
            AccelControllerGroup.Location = new Point(0, 3);
            AccelControllerGroup.Name = "AccelControllerGroup";
            AccelControllerGroup.Size = new Size(0x3ab, 0x99);
            AccelControllerGroup.TabIndex = 0xd3;
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
            gbDR.Location = new Point(0x1b4, 9);
            gbDR.Name = "gbDR";
            gbDR.Size = new Size(0xd8, 0x8d);
            gbDR.TabIndex = 0xc4;
            gbDR.TabStop = false;
            gbDR.Text = "Dynamic Range";
            lbl4gSensitivity.AutoSize = true;
            lbl4gSensitivity.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbl4gSensitivity.ForeColor = Color.White;
            lbl4gSensitivity.Location = new Point(0x3e, 0x3d);
            lbl4gSensitivity.Name = "lbl4gSensitivity";
            lbl4gSensitivity.Size = new Size(0x84, 0x10);
            lbl4gSensitivity.TabIndex = 0x77;
            lbl4gSensitivity.Text = "2048 counts/g 14b";
            rdo2g.AutoSize = true;
            rdo2g.Checked = true;
            rdo2g.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            rdo2g.ForeColor = Color.White;
            rdo2g.Location = new Point(0x13, 0x20);
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
            rdo8g.Location = new Point(0x13, 0x54);
            rdo8g.Name = "rdo8g";
            rdo8g.Size = new Size(0x2b, 20);
            rdo8g.TabIndex = 0x74;
            rdo8g.Text = "8g";
            rdo8g.UseVisualStyleBackColor = true;
            rdo8g.CheckedChanged += new EventHandler(rdo8g_CheckedChanged);
            rdo4g.AutoSize = true;
            rdo4g.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            rdo4g.ForeColor = Color.White;
            rdo4g.Location = new Point(0x13, 0x3a);
            rdo4g.Name = "rdo4g";
            rdo4g.Size = new Size(0x2b, 20);
            rdo4g.TabIndex = 0x75;
            rdo4g.Text = "4g";
            rdo4g.UseVisualStyleBackColor = true;
            rdo4g.CheckedChanged += new EventHandler(rdo4g_CheckedChanged);
            lbl2gSensitivity.AutoSize = true;
            lbl2gSensitivity.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbl2gSensitivity.ForeColor = Color.White;
            lbl2gSensitivity.Location = new Point(0x3f, 0x22);
            lbl2gSensitivity.Name = "lbl2gSensitivity";
            lbl2gSensitivity.Size = new Size(0x84, 0x10);
            lbl2gSensitivity.TabIndex = 0x76;
            lbl2gSensitivity.Text = "4096 counts/g 14b";
            lbl8gSensitivity.AutoSize = true;
            lbl8gSensitivity.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbl8gSensitivity.ForeColor = Color.White;
            lbl8gSensitivity.Location = new Point(0x3e, 0x56);
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
            gbOM.Location = new Point(6, 9);
            gbOM.Name = "gbOM";
            gbOM.Size = new Size(0x1a8, 0x8d);
            gbOM.TabIndex = 0xc3;
            gbOM.TabStop = false;
            gbOM.Text = "Operation Mode";
            btnAutoCal.BackColor = Color.LightSlateGray;
            btnAutoCal.FlatAppearance.BorderColor = Color.Fuchsia;
            btnAutoCal.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnAutoCal.ForeColor = Color.White;
            btnAutoCal.Location = new Point(0x13d, 0x48);
            btnAutoCal.Name = "btnAutoCal";
            btnAutoCal.Size = new Size(0x60, 0x2c);
            btnAutoCal.TabIndex = 0xc5;
            btnAutoCal.Text = "Auto Calibrate";
            btnAutoCal.UseVisualStyleBackColor = false;
            btnAutoCal.Click += new EventHandler(btnAutoCal_Click);
            chkAnalogLowNoise.AutoSize = true;
            chkAnalogLowNoise.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            chkAnalogLowNoise.ForeColor = Color.White;
            chkAnalogLowNoise.Location = new Point(0xce, 0x1c);
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
            pOverSampling.Location = new Point(1, 0x37);
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
            rdoOSLNLPMode.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
            rdoOSLNLPMode.Location = new Point(0x81, 0x2f);
            rdoOSLNLPMode.Name = "rdoOSLNLPMode";
            rdoOSLNLPMode.Size = new Size(0xa6, 0x13);
            rdoOSLNLPMode.TabIndex = 0xdd;
            rdoOSLNLPMode.Text = "Low Noise Low Power";
            rdoOSLNLPMode.UseVisualStyleBackColor = true;
            rdoOSLNLPMode.CheckedChanged += new EventHandler(rdoOSLNLPMode_CheckedChanged);
            rdoOSNormalMode.AutoSize = true;
            rdoOSNormalMode.Checked = true;
            rdoOSNormalMode.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
            rdoOSNormalMode.Location = new Point(5, 0x1c);
            rdoOSNormalMode.Name = "rdoOSNormalMode";
            rdoOSNormalMode.Size = new Size(0x70, 0x13);
            rdoOSNormalMode.TabIndex = 220;
            rdoOSNormalMode.TabStop = true;
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
            label71.AutoSize = true;
            label71.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label71.Location = new Point(0x2c1, 0x59);
            label71.Name = "label71";
            label71.Size = new Size(0x5b, 0x10);
            label71.TabIndex = 0xc0;
            label71.Text = "Wake Mode";
            rdoStandby.AutoSize = true;
            rdoStandby.Checked = true;
            rdoStandby.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            rdoStandby.Location = new Point(0x292, 0x13);
            rdoStandby.Name = "rdoStandby";
            rdoStandby.Size = new Size(0x57, 20);
            rdoStandby.TabIndex = 0x70;
            rdoStandby.TabStop = true;
            rdoStandby.Text = "Standby ";
            rdoStandby.UseVisualStyleBackColor = true;
            rdoStandby.CheckedChanged += new EventHandler(rdoStandby_CheckedChanged);
            rdoActive.AutoSize = true;
            rdoActive.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            rdoActive.Location = new Point(0x2ef, 0x13);
            rdoActive.Name = "rdoActive";
            rdoActive.Size = new Size(0x45, 20);
            rdoActive.TabIndex = 0x71;
            rdoActive.Text = "Active";
            rdoActive.UseVisualStyleBackColor = true;
            rdoActive.CheckedChanged += new EventHandler(rdoActive_CheckedChanged);
            ledWake.LedStyle = LedStyle.Round3D;
            ledWake.Location = new Point(0x31e, 0x54);
            ledWake.Name = "ledWake";
            ledWake.OffColor = Color.Red;
            ledWake.Size = new Size(30, 0x1f);
            ledWake.TabIndex = 0xbf;
            ledSleep.LedStyle = LedStyle.Round3D;
            ledSleep.Location = new Point(0x31e, 110);
            ledSleep.Name = "ledSleep";
            ledSleep.OffColor = Color.Red;
            ledSleep.Size = new Size(30, 0x1f);
            ledSleep.TabIndex = 0x91;
            ledStandby.LedStyle = LedStyle.Round3D;
            ledStandby.Location = new Point(0x31e, 0x37);
            ledStandby.Name = "ledStandby";
            ledStandby.OffColor = Color.Red;
            ledStandby.Size = new Size(30, 0x1f);
            ledStandby.TabIndex = 0xc1;
            ledStandby.Value = true;
            lbsleep.AutoSize = true;
            lbsleep.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbsleep.Location = new Point(0x2c0, 0x75);
            lbsleep.Name = "lbsleep";
            lbsleep.Size = new Size(0x5c, 0x10);
            lbsleep.TabIndex = 0xb9;
            lbsleep.Text = "Sleep Mode";
            label173.AutoSize = true;
            label173.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label173.Location = new Point(0x2b0, 0x3e);
            label173.Name = "label173";
            label173.Size = new Size(0x6c, 0x10);
            label173.TabIndex = 0xc2;
            label173.Text = "Standby Mode";
            panelRegisters.Controls.Add(txtboxRegisters);
            panelRegisters.Controls.Add(groupBox4);
            panelRegisters.Controls.Add(btnUpdateRegs);
            panelRegisters.Location = new Point(3, 0x3a1);
            panelRegisters.Name = "panelRegisters";
            panelRegisters.Size = new Size(0x3ae, 0x100);
            panelRegisters.TabIndex = 0x4d;
            txtboxRegisters.Location = new Point(8, 0x1b);
            txtboxRegisters.Multiline = true;
            txtboxRegisters.Name = "txtboxRegisters";
            txtboxRegisters.ScrollBars = ScrollBars.Both;
            txtboxRegisters.Size = new Size(0x203, 0xd3);
            txtboxRegisters.TabIndex = 0xda;
            groupBox4.BackColor = SystemColors.ActiveBorder;
            groupBox4.Controls.Add(btnWrite);
            groupBox4.Controls.Add(lblRegValue);
            groupBox4.Controls.Add(btnRead);
            groupBox4.Controls.Add(label51);
            groupBox4.Controls.Add(txtRegRead);
            groupBox4.Controls.Add(label49);
            groupBox4.Controls.Add(txtRegWrite);
            groupBox4.Controls.Add(txtRegValue);
            groupBox4.Controls.Add(label42);
            groupBox4.Location = new Point(0x214, 3);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(0x17a, 0xec);
            groupBox4.TabIndex = 0xd9;
            groupBox4.TabStop = false;
            groupBox4.Text = "Manual Read/Write";
            btnWrite.Location = new Point(8, 0x61);
            btnWrite.Name = "btnWrite";
            btnWrite.Size = new Size(0x57, 0x1c);
            btnWrite.TabIndex = 0xce;
            btnWrite.Text = "WRITE";
            btnWrite.UseVisualStyleBackColor = true;
            btnWrite.Click += new EventHandler(btnWrite_Click);
            lblRegValue.AutoSize = true;
            lblRegValue.Location = new Point(0x101, 0x2a);
            lblRegValue.Name = "lblRegValue";
            lblRegValue.Size = new Size(0x49, 13);
            lblRegValue.TabIndex = 0xd5;
            lblRegValue.Text = "RegisterValue";
            btnRead.Location = new Point(8, 0x22);
            btnRead.Name = "btnRead";
            btnRead.Size = new Size(0x57, 0x1c);
            btnRead.TabIndex = 0xcd;
            btnRead.Text = "READ";
            btnRead.UseVisualStyleBackColor = true;
            btnRead.Click += new EventHandler(btnRead_Click);
            label51.AutoSize = true;
            label51.Location = new Point(0x6c, 40);
            label51.Name = "label51";
            label51.Size = new Size(0x40, 13);
            label51.TabIndex = 0xd4;
            label51.Text = "from register";
            txtRegRead.Location = new Point(0xae, 0x26);
            txtRegRead.Name = "txtRegRead";
            txtRegRead.Size = new Size(0x33, 20);
            txtRegRead.TabIndex = 0xcf;
            label49.AutoSize = true;
            label49.Location = new Point(0xe9, 0x69);
            label49.Name = "label49";
            label49.Size = new Size(0x33, 13);
            label49.TabIndex = 0xd3;
            label49.Text = "the value";
            txtRegWrite.Location = new Point(0xb0, 0x66);
            txtRegWrite.Name = "txtRegWrite";
            txtRegWrite.Size = new Size(0x31, 20);
            txtRegWrite.TabIndex = 0xd0;
            txtRegValue.Location = new Point(290, 0x66);
            txtRegValue.Name = "txtRegValue";
            txtRegValue.Size = new Size(0x31, 20);
            txtRegValue.TabIndex = 210;
            label42.AutoSize = true;
            label42.Location = new Point(0x6c, 0x69);
            label42.Name = "label42";
            label42.Size = new Size(0x35, 13);
            label42.TabIndex = 0xd1;
            label42.Text = "to register";
            btnUpdateRegs.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnUpdateRegs.ForeColor = SystemColors.ActiveCaption;
            btnUpdateRegs.Location = new Point(8, 2);
            btnUpdateRegs.Name = "btnUpdateRegs";
            btnUpdateRegs.Size = new Size(0x203, 0x19);
            btnUpdateRegs.TabIndex = 0xd8;
            btnUpdateRegs.Text = "Read All Registers";
            btnUpdateRegs.UseVisualStyleBackColor = true;
            btnUpdateRegs.Click += new EventHandler(btnUpdateRegs_Click);
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
            pictureBox1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            pictureBox1.BackgroundImageLayout = ImageLayout.None;
            pictureBox1.BorderStyle = BorderStyle.Fixed3D;
            pictureBox1.Image = Resources.STB_TOPBAR_LARGE;
            pictureBox1.Location = new Point(0, 0x1b);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(0x3c6, 0x39);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 0x48;
            pictureBox1.TabStop = false;
            btnDisplay.FlatStyle = FlatStyle.Flat;
            btnDisplay.Location = new Point(3, 0x57);
            btnDisplay.Margin = new Padding(0);
            btnDisplay.Name = "btnDisplay";
            btnDisplay.Size = new Size(0x4b, 0x17);
            btnDisplay.TabIndex = 60;
            btnDisplay.Text = "Demo";
            btnDisplay.UseVisualStyleBackColor = true;
            btnDisplay.Click += new EventHandler(btnDisplay_Click);
            btnAdvanced.Location = new Point(0x4e, 0x57);
            btnAdvanced.Margin = new Padding(0);
            btnAdvanced.Name = "btnAdvanced";
            btnAdvanced.Size = new Size(0x4b, 0x17);
            btnAdvanced.TabIndex = 0x4a;
            btnAdvanced.Text = "Configure";
            btnAdvanced.UseVisualStyleBackColor = true;
            btnAdvanced.Click += new EventHandler(btnAdvanced_Click);
            btnRegisters.Location = new Point(0x99, 0x57);
            btnRegisters.Margin = new Padding(0);
            btnRegisters.Name = "btnRegisters";
            btnRegisters.Size = new Size(0x4b, 0x17);
            btnRegisters.TabIndex = 0x4b;
            btnRegisters.Text = "Registers";
            btnRegisters.UseVisualStyleBackColor = true;
            btnRegisters.Click += new EventHandler(btnRegisters_Click);
            CommStrip.Items.AddRange(new ToolStripItem[] { CommStripButton, toolStripStatusLabel });
            CommStrip.Location = new Point(0, 0x239);
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
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            AutoScroll = true;
            BackColor = Color.LightSlateGray;
            base.ClientSize = new Size(0x3c6, 0x24f);
            base.Controls.Add(CommStrip);
            base.Controls.Add(panelGeneral);
            base.Controls.Add(btnRegisters);
            base.Controls.Add(btnAdvanced);
            base.Controls.Add(btnDisplay);
            base.Controls.Add(pictureBox1);
            base.Controls.Add(menuStrip1);
            base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            base.Icon = (Icon) resources.GetObject("$Icon");
            base.MaximizeBox = false;
            base.Name = "DirectionalTapDemo";
            base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            Text = "Directional Tap Embedded Algorithm";
            base.Resize += new EventHandler(ShakeDemo_Resize);
            panelGeneral.ResumeLayout(false);
            panelDisplay.ResumeLayout(false);
            panelDisplay.PerformLayout();
            pTapStatus.ResumeLayout(false);
            pTapStatus.PerformLayout();
            ((ISupportInitialize) ledPulseDouble).EndInit();
            ((ISupportInitialize) ledTapEA).EndInit();
            ((ISupportInitialize) ledPZ).EndInit();
            ((ISupportInitialize) ledPX).EndInit();
            ((ISupportInitialize) ledPY).EndInit();
            panelAdvanced.ResumeLayout(false);
            panelAdvanced.PerformLayout();
            panel15.ResumeLayout(false);
            panel15.PerformLayout();
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
            ((ISupportInitialize) ledWake).EndInit();
            ((ISupportInitialize) ledSleep).EndInit();
            ((ISupportInitialize) ledStandby).EndInit();
            panelRegisters.ResumeLayout(false);
            panelRegisters.PerformLayout();
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ((ISupportInitialize) pictureBox1).EndInit();
            CommStrip.ResumeLayout(false);
            CommStrip.PerformLayout();
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
                pTapStatus.Enabled = true;
                rdoDefaultSPDP.Enabled = false;
                rdoDefaultSP.Enabled = false;
                int[] datapassed = new int[] { 1 };
                ControllerObj.ActiveFlag = true;
                ControllerReqPacket reqName = new ControllerReqPacket();
                ControllerObj.CreateNewTaskFromGUI(reqName, 0, 0x53, datapassed);
                ControllerObj.ReturnPulseStatus = true;
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
                        DR_timestep = 160.0;
                        DR_PulseTimeStepNoLPF = 40.0;
                        break;

                    case 7:
                        DR_timestep = 640.0;
                        DR_PulseTimeStepNoLPF = 160.0;
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
                        DR_PulseTimeStepNoLPF = 80.0;
                        break;

                    case 7:
                        DR_timestep = 640.0;
                        DR_PulseTimeStepNoLPF = 320.0;
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
                pTapStatus.Enabled = false;
                rdoDefaultSPDP.Enabled = true;
                rdoDefaultSP.Enabled = true;
                int[] datapassed = new int[] { 0 };
                ControllerObj.ActiveFlag = true;
                ControllerReqPacket reqName = new ControllerReqPacket();
                ControllerObj.CreateNewTaskFromGUI(reqName, 0, 0x53, datapassed);
                ControllerObj.ReturnPulseStatus = false;
            }
        }

        private void SetPulseInterrupts(bool enabled)
        {
            int[] datapassed = new int[] { enabled ? 0xff : 0, 8 };
            ControllerObj.SetIntsEnableFlag = true;
            ControllerReqPacket reqName = new ControllerReqPacket();
            ControllerObj.CreateNewTaskFromGUI(reqName, 0, 13, datapassed);
            ControllerObj.PollingOrInt[0] = 1;
            ControllerObj.AppRegisterWrite(new int[] { 0x15, 2 });
        }

        private void ShakeDemo_Resize(object sender, EventArgs e)
        {
            windowResized = true;
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
            if (ledTapEA.Value)
            {
                if (timercount == 5)
                {
                    lblPPolZ.Text = sPPolZ;
                    lblPPolY.Text = sPPolY;
                    lblPPolX.Text = sPPolX;
                    ledTapEA.Value = false;
                    ledPX.Value = false;
                    ledPY.Value = false;
                    ledPZ.Value = false;
                    ledPulseDouble.Value = false;
                    timercount = 0;
                }
                else
                {
                    timercount++;
                }
            }
            UpdateFormState();
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

        private void UpdateFormHeight()
        {
            if (!windowResized)
            {
                int windowHeight = panelDisplay.Height + panelAdvanced.Height + panelRegisters.Height + 200;
                if (720 > base.Height)
                {
                    if (windowHeight >= 720)
                        windowHeight = 720;
                }
                else
                    windowHeight = base.Height;
                base.Height = windowHeight;
            }
        }

        private void UpdateFormState()
        {
            if (B_btnDisplay)
            {
                btnDisplay.FlatStyle = FlatStyle.Standard;
                panelDisplay.Height = 0;
            }
            else
            {
                btnDisplay.FlatStyle = FlatStyle.Flat;
                panelDisplay.Height = I_panelDisplay_height;
            }
            if (B_btnAdvanced)
            {
                btnAdvanced.FlatStyle = FlatStyle.Standard;
                panelAdvanced.Height = 0;
            }
            else
            {
                btnAdvanced.FlatStyle = FlatStyle.Flat;
                panelAdvanced.Height = I_panelAdvanced_height;
            }
            if (B_btnRegisters)
            {
                btnRegisters.FlatStyle = FlatStyle.Standard;
                panelRegisters.Height = 0;
            }
            else
            {
                btnRegisters.FlatStyle = FlatStyle.Flat;
                panelRegisters.Height = I_panelRegisters_height;
            }
            if (ledPZ.Value && ledPulseDouble.Value)
            {
                lblNumber.Text = stringNumber;
            }
            DecodeGUIPackets();
            UpdateCommStrip();
            UpdateFormHeight();
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

