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

    public class OrientationDemo : Form
    {
        private GroupBox AccelControllerGroup;
        private bool B_btnAdvanced = false;
        private bool B_btnDisplay = true;
        private bool B_btnRegisters = false;
        private Button btnAutoCal;
        private Button btnResetPLDebounce;
        private Button btnSetPLDebounce;
        private CheckBox chkAnalogLowNoise;
        private CheckBox chkEnablePL;
        private CheckBox chkPLDefaultSettings;
        private eCommMode CommMode = eCommMode.Closed;
        private StatusStrip CommStrip;
        private ToolStripDropDownButton CommStripButton;
        private IContainer components = null;
        private object ControllerEventsLock = new object();
        private AccelController ControllerObj;
        private string CurrentFW = "4003";
        private ComboBox ddlBFTripA;
        private ComboBox ddlDataRate;
        private ComboBox ddlHysteresisA;
        private ComboBox ddlPLTripA;
        private ComboBox ddlZLockA;
        public int DeviceID;
        private double DR_timestep;
        private BoardComm dv;
        private ToolStripMenuItem enableLogDataApplicationToolStripMenuItem;
        private bool FirstTimeLoad = true;
        private const int Form_Max_Height = 720;
        private const int Form_Min_Height = 200;
        private bool FullFeaturedDevice = true;
        private int FullScaleValue = 0;
        private Gauge gaugeXY;
        private Gauge gaugeYZ;
        private GroupBox gbDR;
        private GroupBox gbOD;
        private GroupBox gbOM;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private int HysAngle;
        private int I_panelAdvanced_height;
        private int I_panelDisplay_height;
        private int I_panelRegisters_height;
        private Image ImageGreen;
        private Image ImagePL_bd;
        private Image ImagePL_bl;
        private Image ImagePL_br;
        private Image ImagePL_bu;
        private Image ImagePL_fd;
        private Image ImagePL_fl;
        private Image ImagePL_fr;
        private Image ImagePL_fu;
        private Image ImageRed;
        private Image ImageYellow;
        private int L2PResultA;
        private Label label173;
        private Label label21;
        private Label label24;
        private Label label263;
        private Label label264;
        private Label label265;
        private Label label266;
        private Label label35;
        private Label label5;
        private Label label70;
        private Label label71;
        private Label lbl2gSensitivity;
        private Label lbl4gSensitivity;
        private Label lbl8gSensitivity;
        private Label lblBouncems;
        private Label lblDebouncePL;
        private Label lblFB;
        private Label lblLockOut;
        private Label lblp2LTripAngle;
        private Label lblPLbounceVal;
        private Label lblValL2PResult;
        private Label lblValP2LResult;
        private Label lbsleep;
        private bool ledCurPLBack;
        private bool ledCurPLDown;
        private bool ledCurPLFront;
        private bool ledCurPLLeft;
        private bool ledCurPLLO;
        private bool ledCurPLNew;
        private bool ledCurPLRight;
        private bool ledCurPLUp;
        private Led ledSleep;
        private Led ledStandby;
        private Led ledWake;
        private bool LoadingFW = false;
        private ToolStripMenuItem logDataToolStripMenuItem1;
        private MenuStrip menuStrip1;
        private object objectLock = new object();
        private Panel p1;
        private int P2LResultA;
        private Panel p6;
        private Panel panelAdvanced;
        private Panel panelDisplay;
        private FlowLayoutPanel panelGeneral;
        private PictureBox pictureBox1;
        private int PL_index = 0;
        private int PLAngle;
        private PictureBox PLImage;
        private Panel pOverSampling;
        private Panel pPLActive;
        private RadioButton rdo2g;
        private RadioButton rdo4g;
        private RadioButton rdo8g;
        private RadioButton rdoActive;
        private RadioButton rdoClrDebouncePL;
        private RadioButton rdoDecDebouncePL;
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
        private TrackBar tbPL;
        private bool TimerEnabled = true;
        private System.Windows.Forms.Timer tmrTiltTimer;
        private ToolStripStatusLabel toolStripStatusLabel;
        private Panel uxPanelSettings;
        private int WakeOSMode;
        private int windowHeight = 0;
        private bool windowResized = false;

        public OrientationDemo(object controllerObj)
        {
            InitializeComponent();
            ControllerObj = (AccelController) controllerObj;
            ControllerObj.ControllerEvents += new AccelController.ControllerEventHandler(ControllerObj_ControllerEvents);
            menuStrip1.Enabled = false;
            I_panelDisplay_height = panelDisplay.Height;
            I_panelAdvanced_height = panelAdvanced.Height;
            B_btnDisplay = false;
            B_btnAdvanced = true;
            B_btnRegisters = true;
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

        private void btnResetPLDebounce_Click(object sender, EventArgs e)
        {
            btnSetPLDebounce.Enabled = true;
            btnResetPLDebounce.Enabled = false;
            tbPL.Enabled = true;
            lblPLbounceVal.Enabled = true;
            lblBouncems.Enabled = true;
            lblDebouncePL.Enabled = true;
        }

        private void btnSetPLDebounce_Click(object sender, EventArgs e)
        {
            int[] datapassed = new int[] { tbPL.Value };
            ControllerObj.SetPLDebounceFlag = true;
            ControllerReqPacket reqName = new ControllerReqPacket();
            ControllerObj.CreateNewTaskFromGUI(reqName, 4, 0x1f, datapassed);
            btnSetPLDebounce.Enabled = false;
            btnResetPLDebounce.Enabled = true;
            tbPL.Enabled = false;
            lblPLbounceVal.Enabled = false;
            lblBouncems.Enabled = false;
            lblDebouncePL.Enabled = false;
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

        private void chkEnablePL_CheckedChanged(object sender, EventArgs e)
        {
            int num;
            if (chkEnablePL.Checked)
            {
                chkPLDefaultSettings.Enabled = FullFeaturedDevice;
            }
            else
            {
                chkPLDefaultSettings.Enabled = false;
            }
            if (chkEnablePL.Checked)
            {
                num = 0xff;
                pPLActive.Enabled = true;
                p6.Enabled = true;
            }
            else
            {
                num = 0;
                pPLActive.Enabled = false;
                p6.Enabled = false;
            }
            int[] datapassed = new int[] { num };
            ControllerObj.SetEnablePLFlag = true;
            ControllerReqPacket reqName = new ControllerReqPacket();
            ControllerObj.CreateNewTaskFromGUI(reqName, 4, 0x19, datapassed);
        }

        private void chkPLDefaultSettings_CheckedChanged(object sender, EventArgs e)
        {
            if (!chkPLDefaultSettings.Checked)
            {
                ddlZLockA.Enabled = true;
                ddlBFTripA.Enabled = true;
                ddlPLTripA.Enabled = true;
                ddlHysteresisA.Enabled = true;
            }
            else
            {
                ddlZLockA.SelectedIndex = 0;
                ddlBFTripA.SelectedIndex = 1;
                ddlPLTripA.SelectedIndex = 5;
                ddlHysteresisA.SelectedIndex = 6;
                int[] datapassed = new int[] { ddlZLockA.SelectedIndex };
                ControllerObj.SetZLockAFlag = true;
                ControllerReqPacket reqName = new ControllerReqPacket();
                ControllerObj.CreateNewTaskFromGUI(reqName, 4, 0x1a, datapassed);
                ddlZLockA.Enabled = false;
                int[] numArray2 = new int[] { ddlBFTripA.SelectedIndex };
                ControllerObj.SetBFTripAFlag = true;
                ControllerReqPacket packet2 = new ControllerReqPacket();
                ControllerObj.CreateNewTaskFromGUI(packet2, 4, 0x1b, numArray2);
                ddlBFTripA.Enabled = false;
                int[] numArray3 = new int[1];
                PL_index = ddlPLTripA.SelectedIndex;
                numArray3[0] = PL_index;
                ControllerObj.SetPLTripAFlag = true;
                ControllerReqPacket packet3 = new ControllerReqPacket();
                ControllerObj.CreateNewTaskFromGUI(packet3, 4, 0x1c, numArray3);
                ddlPLTripA.Enabled = false;
                int[] numArray4 = new int[] { ddlHysteresisA.SelectedIndex };
                ControllerObj.SetHysteresisFlag = true;
                ControllerReqPacket packet4 = new ControllerReqPacket();
                ControllerObj.CreateNewTaskFromGUI(packet4, 4, 0x1d, numArray4);
                ddlHysteresisA.Enabled = false;
                switch (ddlDataRate.SelectedIndex)
                {
                    case 0:
                        tbPL.Value = 0x34;
                        break;

                    case 1:
                        tbPL.Value = 0x1a;
                        break;

                    case 2:
                        tbPL.Value = 13;
                        break;

                    case 3:
                        tbPL.Value = 6;
                        break;

                    case 4:
                        tbPL.Value = 3;
                        break;

                    case 5:
                        tbPL.Value = 1;
                        break;

                    default:
                        tbPL.Value = 0;
                        break;
                }
                tbPL.Value = 8;
                double num = tbPL.Value * DR_timestep;
                lblBouncems.Text = "ms";
                lblPLbounceVal.Text = string.Format("{0:F1}", num);
                int[] numArray5 = new int[] { tbPL.Value };
                ControllerObj.SetPLDebounceFlag = true;
                ControllerReqPacket packet5 = new ControllerReqPacket();
                ControllerObj.CreateNewTaskFromGUI(packet5, 4, 0x1f, numArray5);
                btnSetPLDebounce.Enabled = false;
                btnResetPLDebounce.Enabled = true;
                tbPL.Enabled = false;
                lblPLbounceVal.Enabled = false;
                lblBouncems.Enabled = false;
                lblDebouncePL.Enabled = false;
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
            XYZGees gees = new XYZGees();
            int num4 = (int) evt;
            if (num4 == 4)
            {
                GUIUpdatePacket packet = (GUIUpdatePacket) o;
                if (packet.PayLoad.Count > 0)
                {
                    int[] numArray = (int[]) packet.PayLoad.Dequeue();
                    try
                    {
                        if (numArray[0] == 0)
                        {
                            ledCurPLFront = true;
                        }
                        else
                        {
                            ledCurPLFront = false;
                        }
                        if (numArray[0] == 1)
                        {
                            ledCurPLBack = true;
                        }
                        else
                        {
                            ledCurPLBack = false;
                        }
                        if (numArray[1] == 0)
                        {
                            ledCurPLUp = true;
                        }
                        else
                        {
                            ledCurPLUp = false;
                        }
                        if (numArray[1] == 1)
                        {
                            ledCurPLDown = true;
                        }
                        else
                        {
                            ledCurPLDown = false;
                        }
                        if (numArray[1] == 2)
                        {
                            ledCurPLRight = true;
                        }
                        else
                        {
                            ledCurPLRight = false;
                        }
                        if (numArray[1] == 3)
                        {
                            ledCurPLLeft = true;
                        }
                        else
                        {
                            ledCurPLLeft = false;
                        }
                        if (numArray[2] == 1)
                        {
                            ledCurPLLO = true;
                        }
                        else
                        {
                            ledCurPLLO = false;
                        }
                        if (numArray[3] == 1)
                        {
                            ledCurPLNew = true;
                        }
                        else
                        {
                            ledCurPLNew = false;
                        }
                        if (ledCurPLBack && ledCurPLUp)
                        {
                            PLImage.Image = ImagePL_bu;
                        }
                        if (ledCurPLBack && ledCurPLDown)
                        {
                            PLImage.Image = ImagePL_bd;
                        }
                        if (ledCurPLBack && ledCurPLLeft)
                        {
                            PLImage.Image = ImagePL_bl;
                        }
                        if (ledCurPLBack && ledCurPLRight)
                        {
                            PLImage.Image = ImagePL_br;
                        }
                        if (ledCurPLFront && ledCurPLUp)
                        {
                            PLImage.Image = ImagePL_fu;
                        }
                        if (ledCurPLFront && ledCurPLDown)
                        {
                            PLImage.Image = ImagePL_fd;
                        }
                        if (ledCurPLFront && ledCurPLLeft)
                        {
                            PLImage.Image = ImagePL_fl;
                        }
                        if (ledCurPLFront && ledCurPLRight)
                        {
                            PLImage.Image = ImagePL_fr;
                        }
                    }
                    catch (InvalidOperationException)
                    {
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }

        private void ddlBFTripA_SelectedIndexChanged(object sender, EventArgs e)
        {
            int[] datapassed = new int[] { ddlBFTripA.SelectedIndex };
            ControllerObj.SetBFTripAFlag = true;
            ControllerReqPacket reqName = new ControllerReqPacket();
            ControllerObj.CreateNewTaskFromGUI(reqName, 4, 0x1b, datapassed);
        }

        private void ddlDataRate_SelectedIndexChanged(object sender, EventArgs e)
        {
            double num;
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
                            goto Label_02CB;

                        case 3:
                            DR_timestep = 5.0;
                            goto Label_02CB;
                    }
                    DR_timestep = 5.0;
                    break;

                case 3:
                    switch (WakeOSMode)
                    {
                        case 2:
                            DR_timestep = 2.5;
                            goto Label_02CB;

                        case 3:
                            DR_timestep = 10.0;
                            goto Label_02CB;
                    }
                    DR_timestep = 10.0;
                    break;

                case 4:
                    switch (WakeOSMode)
                    {
                        case 2:
                            DR_timestep = 2.5;
                            goto Label_02CB;

                        case 3:
                            DR_timestep = 20.0;
                            goto Label_02CB;
                    }
                    DR_timestep = 20.0;
                    break;

                case 5:
                    DR_timestep = 80.0;
                    switch (WakeOSMode)
                    {
                        case 0:
                            DR_timestep = 20.0;
                            goto Label_02CB;

                        case 1:
                            DR_timestep = 80.0;
                            goto Label_02CB;

                        case 2:
                            DR_timestep = 2.5;
                            goto Label_02CB;

                        case 3:
                            DR_timestep = 80.0;
                            goto Label_02CB;
                    }
                    break;

                case 6:
                    switch (WakeOSMode)
                    {
                        case 0:
                            DR_timestep = 20.0;
                            goto Label_02CB;

                        case 1:
                            DR_timestep = 80.0;
                            goto Label_02CB;

                        case 2:
                            DR_timestep = 2.5;
                            goto Label_02CB;

                        case 3:
                            DR_timestep = 160.0;
                            goto Label_02CB;
                    }
                    break;

                case 7:
                    switch (WakeOSMode)
                    {
                        case 0:
                            DR_timestep = 20.0;
                            goto Label_02CB;

                        case 1:
                            DR_timestep = 80.0;
                            goto Label_02CB;

                        case 2:
                            DR_timestep = 2.5;
                            goto Label_02CB;

                        case 3:
                            DR_timestep = 160.0;
                            goto Label_02CB;
                    }
                    break;
            }
        Label_02CB:
            num = tbPL.Value * DR_timestep;
            if (num > 1000.0)
            {
                lblBouncems.Text = "s";
                num /= 1000.0;
                lblPLbounceVal.Text = string.Format("{0:F4}", num);
            }
            else
            {
                lblBouncems.Text = "ms";
                lblPLbounceVal.Text = string.Format("{0:F2}", num);
            }
        }

        private void ddlHysteresisA_SelectedIndexChanged(object sender, EventArgs e)
        {
            int[] datapassed = new int[] { ddlHysteresisA.SelectedIndex };
            switch (ddlHysteresisA.SelectedIndex)
            {
                case 0:
                    HysAngle = 0;
                    break;

                case 1:
                    HysAngle = 4;
                    break;

                case 2:
                    HysAngle = 7;
                    break;

                case 3:
                    HysAngle = 11;
                    break;

                case 4:
                    HysAngle = 14;
                    break;

                case 5:
                    HysAngle = 0x11;
                    break;

                case 6:
                    HysAngle = 0x15;
                    break;

                case 7:
                    HysAngle = 0x18;
                    break;
            }
            ControllerObj.SetHysteresisFlag = true;
            ControllerReqPacket reqName = new ControllerReqPacket();
            ControllerObj.CreateNewTaskFromGUI(reqName, 4, 0x1d, datapassed);
            P2LResultA = PLAngle - HysAngle;
            L2PResultA = PLAngle + HysAngle;
            lblValP2LResult.Text = string.Format("{0:D2}", P2LResultA);
            lblValL2PResult.Text = string.Format("{0:D2}", L2PResultA);
        }

        private void ddlPLTripA_SelectedIndexChanged(object sender, EventArgs e)
        {
            int[] datapassed = new int[1];
            PL_index = ddlPLTripA.SelectedIndex;
            datapassed[0] = PL_index;
            switch (PL_index)
            {
                case 0:
                    PLAngle = 15;
                    break;

                case 1:
                    PLAngle = 20;
                    break;

                case 2:
                    PLAngle = 30;
                    break;

                case 3:
                    PLAngle = 0x23;
                    break;

                case 4:
                    PLAngle = 40;
                    break;

                case 5:
                    PLAngle = 0x2d;
                    break;

                case 6:
                    PLAngle = 0x37;
                    break;

                case 7:
                    PLAngle = 60;
                    break;

                case 8:
                    PLAngle = 70;
                    break;

                case 9:
                    PLAngle = 0x4b;
                    break;
            }
            ControllerObj.SetPLTripAFlag = true;
            ControllerReqPacket reqName = new ControllerReqPacket();
            ControllerObj.CreateNewTaskFromGUI(reqName, 4, 0x1c, datapassed);
            P2LResultA = PLAngle - HysAngle;
            L2PResultA = PLAngle + HysAngle;
            lblValP2LResult.Text = string.Format("{0:D2}", P2LResultA);
            lblValL2PResult.Text = string.Format("{0:D2}", L2PResultA);
        }

        private void ddlZLockA_SelectedIndexChanged(object sender, EventArgs e)
        {
            int[] datapassed = new int[] { ddlZLockA.SelectedIndex };
            ControllerObj.SetZLockAFlag = true;
            ControllerReqPacket reqName = new ControllerReqPacket();
            ControllerObj.CreateNewTaskFromGUI(reqName, 4, 0x1a, datapassed);
        }

        private void DecodeGUIPackets()
        {
            Exception exception;
            GUIUpdatePacket packet = new GUIUpdatePacket();
            ControllerObj.Dequeue_GuiPacket(ref packet);
            try
            {
                if (packet != null)
                {
                    int[] numArray;
                    int num2;
                    if (packet.TaskID == 0x11)
                    {
                        byte num = (byte) packet.PayLoad.Dequeue();
                    }
                    if (packet.TaskID == 0x10)
                    {
                        byte[] buffer;
                        try
                        {
                            buffer = (byte[]) packet.PayLoad.Dequeue();
                        }
                        catch (Exception exception1)
                        {
                            exception = exception1;
                            numArray = (int[]) packet.PayLoad.Dequeue();
                            buffer = new byte[numArray.Length];
                            num2 = 0;
                            while (num2 < numArray.Length)
                            {
                                buffer[num2] = (byte) numArray[num2];
                                num2++;
                            }
                        }
                        StringBuilder builder = new StringBuilder();
                        for (num2 = 0; num2 < buffer.Length; num2 += 3)
                        {
                            builder.Append(string.Format("{0:X2}", num2) + " " + RegisterNames[num2] + "\t" + string.Format("{0:X2}", buffer[num2]) + "\t\t");
                            if ((num2 + 1) < buffer.Length)
                            {
                                builder.Append(string.Format("{0:X2}", num2 + 1) + " " + RegisterNames[num2 + 1] + "\t" + string.Format("{0:X2}", buffer[num2 + 1]) + "\t\t");
                            }
                            if ((num2 + 2) < buffer.Length)
                            {
                                builder.Append(string.Format("{0:X2}", num2 + 2) + " " + RegisterNames[num2 + 2] + "\t" + string.Format("{0:X2}", buffer[num2 + 2]) + "\r\n");
                            }
                        }
                    }
                    if (packet.TaskID == 0x4f)
                    {
                        byte[] buffer2;
                        try
                        {
                            buffer2 = (byte[]) packet.PayLoad.Dequeue();
                        }
                        catch (Exception exception3)
                        {
                            exception = exception3;
                            numArray = (int[]) packet.PayLoad.Dequeue();
                            buffer2 = new byte[numArray.Length];
                            for (num2 = 0; num2 < numArray.Length; num2++)
                            {
                                buffer2[num2] = (byte) numArray[num2];
                            }
                        }
                        if (buffer2[0] == 0)
                        {
                            ledCurPLFront = true;
                        }
                        else
                        {
                            ledCurPLFront = false;
                        }
                        if (buffer2[0] == 1)
                        {
                            ledCurPLBack = true;
                        }
                        else
                        {
                            ledCurPLBack = false;
                        }
                        if (buffer2[1] == 0)
                        {
                            ledCurPLUp = true;
                        }
                        else
                        {
                            ledCurPLUp = false;
                        }
                        if (buffer2[1] == 1)
                        {
                            ledCurPLDown = true;
                        }
                        else
                        {
                            ledCurPLDown = false;
                        }
                        if (buffer2[1] == 2)
                        {
                            ledCurPLRight = true;
                        }
                        else
                        {
                            ledCurPLRight = false;
                        }
                        if (buffer2[1] == 3)
                        {
                            ledCurPLLeft = true;
                        }
                        else
                        {
                            ledCurPLLeft = false;
                        }
                        if (buffer2[2] == 1)
                        {
                            ledCurPLLO = true;
                        }
                        else
                        {
                            ledCurPLLO = false;
                        }
                        if (buffer2[3] == 1)
                        {
                            ledCurPLNew = true;
                        }
                        else
                        {
                            ledCurPLNew = false;
                        }
                        try
                        {
                            if (ledCurPLBack && ledCurPLUp)
                            {
                                PLImage.Image = ImagePL_bu;
                            }
                            if (ledCurPLBack && ledCurPLDown)
                            {
                                PLImage.Image = ImagePL_bd;
                            }
                            if (ledCurPLBack && ledCurPLLeft)
                            {
                                PLImage.Image = ImagePL_bl;
                            }
                            if (ledCurPLBack && ledCurPLRight)
                            {
                                PLImage.Image = ImagePL_br;
                            }
                            if (ledCurPLFront && ledCurPLUp)
                            {
                                PLImage.Image = ImagePL_fu;
                            }
                            if (ledCurPLFront && ledCurPLDown)
                            {
                                PLImage.Image = ImagePL_fd;
                            }
                            if (ledCurPLFront && ledCurPLLeft)
                            {
                                PLImage.Image = ImagePL_fl;
                            }
                            if (ledCurPLFront && ledCurPLRight)
                            {
                                PLImage.Image = ImagePL_fr;
                            }
                        }
                        catch (InvalidOperationException)
                        {
                        }
                    }
                }
            }
            catch (Exception exception5)
            {
                exception = exception5;
            }
        }

        private void DeleteResource()
        {
            ImageGreen.Dispose();
            ImageYellow.Dispose();
            ImageRed.Dispose();
            ImagePL_bd.Dispose();
            ImagePL_bl.Dispose();
            ImagePL_br.Dispose();
            ImagePL_bu.Dispose();
            ImagePL_fd.Dispose();
            ImagePL_fl.Dispose();
            ImagePL_fr.Dispose();
            ImagePL_fu.Dispose();
        }

        private void DisableUnavailableFeatures()
        {
            chkPLDefaultSettings.Checked = true;
            chkPLDefaultSettings.Enabled = false;
            FullFeaturedDevice = false;
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
                tmrTiltTimer.Stop();
                tmrTiltTimer.Enabled = false;
                ControllerObj.ResetDevice();
                ControllerObj.ControllerEvents -= new AccelController.ControllerEventHandler(ControllerObj_ControllerEvents);
                DeleteResource();
                base.Close();
            }
        }

        ~OrientationDemo()
        {
            ControllerObj.ControllerEvents -= new AccelController.ControllerEventHandler(ControllerObj_ControllerEvents);
        }

        private void InitDevice()
        {
            ControllerObj.StartDevice();
            Styles.FormatForm(this);
            Styles.FormatInterruptPanel(uxPanelSettings);
            Styles.FormatInterruptPanel(panelAdvanced);
            tbPL.BackColor = Color.Black;
            Thread.Sleep(10);
            ControllerObj.Boot();
            Thread.Sleep(20);
            DR_timestep = 2.5;
            ddlDataRate.SelectedIndex = 7;
            WakeOSMode = 0;
            DR_timestep = 20.0;
            chkEnablePL.Checked = true;
            chkEnablePL_CheckedChanged(this, null);
            chkPLDefaultSettings.Checked = true;
            chkPLDefaultSettings_CheckedChanged(this, null);
            SetPLInterrupts(true);
            rdoActive.Checked = true;
            rdoActive_CheckedChanged(this, null);
            ControllerObj.ReturnPLStatus = true;
        }

        private void InitializeComponent()
        {
            components = new Container();
            ScaleRangeFill fill = new ScaleRangeFill();
            ScaleRangeFill fill2 = new ScaleRangeFill();
            ScaleRangeFill fill3 = new ScaleRangeFill();
            ScaleRangeFill fill4 = new ScaleRangeFill();
            ScaleRangeFill fill5 = new ScaleRangeFill();
            ComponentResourceManager resources = new ComponentResourceManager(typeof(OrientationDemo));
            ScaleRangeFill fill6 = new ScaleRangeFill();
            ScaleRangeFill fill7 = new ScaleRangeFill();
            ScaleRangeFill fill8 = new ScaleRangeFill();
            ScaleRangeFill fill9 = new ScaleRangeFill();
            ScaleRangeFill fill10 = new ScaleRangeFill();
            ScaleRangeFill fill11 = new ScaleRangeFill();
            ScaleRangeFill fill12 = new ScaleRangeFill();
            ScaleRangeFill fill13 = new ScaleRangeFill();
            ScaleRangeFill fill14 = new ScaleRangeFill();
            gaugeYZ = new Gauge();
            tmrTiltTimer = new System.Windows.Forms.Timer(components);
            panelGeneral = new FlowLayoutPanel();
            panelDisplay = new Panel();
            lblFB = new Label();
            lblLockOut = new Label();
            PLImage = new PictureBox();
            gaugeXY = new Gauge();
            uxPanelSettings = new Panel();
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
            ledWake = new Led();
            label71 = new Label();
            label173 = new Label();
            lbsleep = new Label();
            ledStandby = new Led();
            ledSleep = new Led();
            panelAdvanced = new Panel();
            gbOD = new GroupBox();
            groupBox1 = new GroupBox();
            label21 = new Label();
            label24 = new Label();
            label5 = new Label();
            lblValL2PResult = new Label();
            lblValP2LResult = new Label();
            lblp2LTripAngle = new Label();
            chkPLDefaultSettings = new CheckBox();
            pPLActive = new Panel();
            rdoDecDebouncePL = new RadioButton();
            rdoClrDebouncePL = new RadioButton();
            btnResetPLDebounce = new Button();
            btnSetPLDebounce = new Button();
            lblBouncems = new Label();
            lblPLbounceVal = new Label();
            lblDebouncePL = new Label();
            tbPL = new TrackBar();
            p6 = new Panel();
            label263 = new Label();
            ddlHysteresisA = new ComboBox();
            ddlPLTripA = new ComboBox();
            label264 = new Label();
            label265 = new Label();
            ddlBFTripA = new ComboBox();
            ddlZLockA = new ComboBox();
            label266 = new Label();
            chkEnablePL = new CheckBox();
            rdoStandby = new RadioButton();
            rdoActive = new RadioButton();
            menuStrip1 = new MenuStrip();
            logDataToolStripMenuItem1 = new ToolStripMenuItem();
            enableLogDataApplicationToolStripMenuItem = new ToolStripMenuItem();
            pictureBox1 = new PictureBox();
            CommStrip = new StatusStrip();
            CommStripButton = new ToolStripDropDownButton();
            toolStripStatusLabel = new ToolStripStatusLabel();
            groupBox2 = new GroupBox();
            ((ISupportInitialize) gaugeYZ).BeginInit();
            panelGeneral.SuspendLayout();
            panelDisplay.SuspendLayout();
            ((ISupportInitialize) PLImage).BeginInit();
            ((ISupportInitialize) gaugeXY).BeginInit();
            uxPanelSettings.SuspendLayout();
            AccelControllerGroup.SuspendLayout();
            gbDR.SuspendLayout();
            gbOM.SuspendLayout();
            pOverSampling.SuspendLayout();
            p1.SuspendLayout();
            ((ISupportInitialize) ledWake).BeginInit();
            ((ISupportInitialize) ledStandby).BeginInit();
            ((ISupportInitialize) ledSleep).BeginInit();
            panelAdvanced.SuspendLayout();
            gbOD.SuspendLayout();
            groupBox1.SuspendLayout();
            pPLActive.SuspendLayout();
            tbPL.BeginInit();
            p6.SuspendLayout();
            menuStrip1.SuspendLayout();
            ((ISupportInitialize) pictureBox1).BeginInit();
            CommStrip.SuspendLayout();
            groupBox2.SuspendLayout();
            base.SuspendLayout();
            gaugeYZ.AutoDivisionSpacing = false;
            gaugeYZ.Caption = "Back / Front";
            gaugeYZ.CaptionPosition = CaptionPosition.Right;
            gaugeYZ.DialColor = Color.Transparent;
            gaugeYZ.Location = new Point(0xcf, 12);
            gaugeYZ.MajorDivisions.Interval = 45.0;
            gaugeYZ.MajorDivisions.LineWidth = 2f;
            gaugeYZ.MajorDivisions.TickLength = 7f;
            gaugeYZ.MinorDivisions.Interval = 5.0;
            gaugeYZ.Name = "gaugeYZ";
            gaugeYZ.PointerColor = Color.Transparent;
            gaugeYZ.Range = new Range(0.0, 360.0);
            fill.Range = new Range(70.0, 110.0);
            fill.Style = ScaleRangeFillStyle.CreateSolidStyle(Color.FromArgb(0x80, 0x80, 0xff));
            fill.Width = 50f;
            fill2.Range = new Range(250.0, 290.0);
            fill2.Style = ScaleRangeFillStyle.CreateSolidStyle(Color.FromArgb(0x80, 0x80, 0xff));
            fill2.Width = 50f;
            fill3.Range = new Range(0.0, 90.0);
            fill3.Style = ScaleRangeFillStyle.CreateSolidStyle(Color.Lime);
            fill3.Visible = false;
            fill3.Width = 2f;
            fill4.Range = new Range(270.0, 360.0);
            fill4.Style = ScaleRangeFillStyle.CreateSolidStyle(Color.Lime);
            fill4.Visible = false;
            fill4.Width = 2f;
            fill5.Range = new Range(90.0, 270.0);
            fill5.Style = ScaleRangeFillStyle.CreateSolidStyle(Color.Lime);
            fill5.Visible = false;
            fill5.Width = 2f;
            gaugeYZ.RangeFills.AddRange(new ScaleRangeFill[] { fill, fill2, fill3, fill4, fill5 });
            gaugeYZ.ScaleArc = new Arc(0f, 360f);
            gaugeYZ.ScaleBaseLineColor = SystemColors.GradientActiveCaption;
            gaugeYZ.Size = new Size(0xc5, 0xa1);
            gaugeYZ.TabIndex = 0x37;
            tmrTiltTimer.Interval = 2;
            tmrTiltTimer.Tick += new EventHandler(tmrTiltTimer_Tick);
            panelGeneral.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            panelGeneral.AutoScroll = true;
            panelGeneral.Controls.Add(panelDisplay);
            panelGeneral.Controls.Add(uxPanelSettings);
            panelGeneral.Controls.Add(panelAdvanced);
            panelGeneral.Location = new Point(1, 0x3a);
            panelGeneral.Name = "panelGeneral";
            panelGeneral.Size = new Size(0x3b7, 640);
            panelGeneral.TabIndex = 0x47;
            panelDisplay.BorderStyle = BorderStyle.FixedSingle;
            panelDisplay.Controls.Add(lblFB);
            panelDisplay.Controls.Add(lblLockOut);
            panelDisplay.Controls.Add(gaugeYZ);
            panelDisplay.Controls.Add(PLImage);
            panelDisplay.Controls.Add(gaugeXY);
            panelDisplay.Location = new Point(3, 3);
            panelDisplay.Name = "panelDisplay";
            panelDisplay.Size = new Size(0x19c, 0x17a);
            panelDisplay.TabIndex = 0x49;
            lblFB.BackColor = Color.Transparent;
            lblFB.Font = new Font("Verdana", 15.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblFB.ForeColor = Color.Red;
            lblFB.Location = new Point(0xff, 0x4e);
            lblFB.Name = "lblFB";
            lblFB.Size = new Size(0x51, 0x19);
            lblFB.TabIndex = 0xcf;
            lblFB.Text = "Front";
            lblFB.TextAlign = ContentAlignment.MiddleCenter;
            lblFB.Visible = false;
            lblLockOut.BackColor = Color.Transparent;
            lblLockOut.Font = new Font("Verdana", 15.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblLockOut.ForeColor = Color.Red;
            lblLockOut.Location = new Point(0x3e, 0x4f);
            lblLockOut.Name = "lblLockOut";
            lblLockOut.Size = new Size(0x73, 0x19);
            lblLockOut.TabIndex = 0xce;
            lblLockOut.Text = "Lock Out";
            lblLockOut.TextAlign = ContentAlignment.MiddleCenter;
            lblLockOut.Visible = false;
            PLImage.Image = (Image) resources.GetObject("PLImage.Image");
            PLImage.Location = new Point(0x1d, 0xae);
            PLImage.Name = "PLImage";
            PLImage.Size = new Size(0x163, 0xc9);
            PLImage.TabIndex = 0xc2;
            PLImage.TabStop = false;
            gaugeXY.AutoDivisionSpacing = false;
            gaugeXY.Caption = "Portrait / Landscape";
            gaugeXY.CaptionForeColor = Color.White;
            gaugeXY.CaptionPosition = CaptionPosition.Left;
            gaugeXY.DialColor = Color.Transparent;
            gaugeXY.Location = new Point(8, 12);
            gaugeXY.MajorDivisions.Interval = 45.0;
            gaugeXY.MajorDivisions.LineWidth = 2f;
            gaugeXY.MajorDivisions.TickLength = 7f;
            gaugeXY.MinorDivisions.Interval = 5.0;
            gaugeXY.MinorDivisions.TickColor = Color.LightSkyBlue;
            gaugeXY.Name = "gaugeXY";
            gaugeXY.PointerColor = Color.Transparent;
            gaugeXY.Range = new Range(0.0, 360.0);
            fill6.Range = new Range(35.0, 55.0);
            fill6.Style = ScaleRangeFillStyle.CreateSolidStyle(Color.FromArgb(0x80, 0x80, 0xff));
            fill6.Width = 50f;
            fill7.Range = new Range(125.0, 145.0);
            fill7.Style = ScaleRangeFillStyle.CreateSolidStyle(Color.FromArgb(0x80, 0x80, 0xff));
            fill7.Width = 50f;
            fill8.Range = new Range(215.0, 235.0);
            fill8.Style = ScaleRangeFillStyle.CreateSolidStyle(Color.FromArgb(0x80, 0x80, 0xff));
            fill8.Width = 50f;
            fill9.Range = new Range(305.0, 325.0);
            fill9.Style = ScaleRangeFillStyle.CreateSolidStyle(Color.FromArgb(0x80, 0x80, 0xff));
            fill9.Width = 50f;
            fill10.Range = new Range(35.0, 145.0);
            fill10.Style = ScaleRangeFillStyle.CreateSolidStyle(Color.Lime);
            fill10.Visible = false;
            fill10.Width = 2f;
            fill11.Range = new Range(125.0, 235.0);
            fill11.Style = ScaleRangeFillStyle.CreateSolidStyle(Color.Lime);
            fill11.Visible = false;
            fill11.Width = 2f;
            fill12.Range = new Range(215.0, 325.0);
            fill12.Style = ScaleRangeFillStyle.CreateSolidStyle(Color.Lime);
            fill12.Visible = false;
            fill12.Width = 2f;
            fill13.Range = new Range(305.0, 360.0);
            fill13.Style = ScaleRangeFillStyle.CreateSolidStyle(Color.Lime);
            fill13.Visible = false;
            fill13.Width = 2f;
            fill14.Range = new Range(0.0, 55.0);
            fill14.Style = ScaleRangeFillStyle.CreateSolidStyle(Color.Lime);
            fill14.Visible = false;
            fill14.Width = 2f;
            gaugeXY.RangeFills.AddRange(new ScaleRangeFill[] { fill6, fill7, fill8, fill9, fill10, fill11, fill12, fill13, fill14 });
            gaugeXY.ScaleArc = new Arc(0f, 360f);
            gaugeXY.ScaleBaseLineColor = SystemColors.GradientActiveCaption;
            gaugeXY.Size = new Size(0xc5, 0xa1);
            gaugeXY.TabIndex = 0x38;
            uxPanelSettings.Controls.Add(AccelControllerGroup);
            uxPanelSettings.Location = new Point(0x1a5, 3);
            uxPanelSettings.Name = "uxPanelSettings";
            uxPanelSettings.Size = new Size(0x209, 0x17a);
            uxPanelSettings.TabIndex = 0x4d;
            AccelControllerGroup.Controls.Add(groupBox2);
            AccelControllerGroup.Controls.Add(gbDR);
            AccelControllerGroup.Controls.Add(gbOM);
            AccelControllerGroup.Controls.Add(ledWake);
            AccelControllerGroup.Controls.Add(label71);
            AccelControllerGroup.Controls.Add(label173);
            AccelControllerGroup.Controls.Add(lbsleep);
            AccelControllerGroup.Controls.Add(ledStandby);
            AccelControllerGroup.Controls.Add(ledSleep);
            AccelControllerGroup.Location = new Point(3, -6);
            AccelControllerGroup.Name = "AccelControllerGroup";
            AccelControllerGroup.Size = new Size(0x203, 0x17a);
            AccelControllerGroup.TabIndex = 0xc5;
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
            gbDR.Location = new Point(0x120, 15);
            gbDR.Name = "gbDR";
            gbDR.Size = new Size(0xd8, 0x6d);
            gbDR.TabIndex = 0xc4;
            gbDR.TabStop = false;
            gbDR.Text = "Dynamic Range";
            lbl4gSensitivity.AutoSize = true;
            lbl4gSensitivity.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbl4gSensitivity.ForeColor = Color.White;
            lbl4gSensitivity.Location = new Point(0x3e, 0x30);
            lbl4gSensitivity.Name = "lbl4gSensitivity";
            lbl4gSensitivity.Size = new Size(0x84, 0x10);
            lbl4gSensitivity.TabIndex = 0x77;
            lbl4gSensitivity.Text = "2048 counts/g 14b";
            rdo2g.AutoSize = true;
            rdo2g.Checked = true;
            rdo2g.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            rdo2g.ForeColor = Color.White;
            rdo2g.Location = new Point(0x13, 0x13);
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
            rdo8g.Location = new Point(0x13, 0x47);
            rdo8g.Name = "rdo8g";
            rdo8g.Size = new Size(0x2b, 20);
            rdo8g.TabIndex = 0x74;
            rdo8g.Text = "8g";
            rdo8g.UseVisualStyleBackColor = true;
            rdo8g.CheckedChanged += new EventHandler(rdo8g_CheckedChanged);
            rdo4g.AutoSize = true;
            rdo4g.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            rdo4g.ForeColor = Color.White;
            rdo4g.Location = new Point(0x13, 0x2d);
            rdo4g.Name = "rdo4g";
            rdo4g.Size = new Size(0x2b, 20);
            rdo4g.TabIndex = 0x75;
            rdo4g.Text = "4g";
            rdo4g.UseVisualStyleBackColor = true;
            rdo4g.CheckedChanged += new EventHandler(rdo4g_CheckedChanged);
            lbl2gSensitivity.AutoSize = true;
            lbl2gSensitivity.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbl2gSensitivity.ForeColor = Color.White;
            lbl2gSensitivity.Location = new Point(0x3f, 0x15);
            lbl2gSensitivity.Name = "lbl2gSensitivity";
            lbl2gSensitivity.Size = new Size(0x84, 0x10);
            lbl2gSensitivity.TabIndex = 0x76;
            lbl2gSensitivity.Text = "4096 counts/g 14b";
            lbl8gSensitivity.AutoSize = true;
            lbl8gSensitivity.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbl8gSensitivity.ForeColor = Color.White;
            lbl8gSensitivity.Location = new Point(0x3e, 0x49);
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
            gbOM.Location = new Point(6, 0x99);
            gbOM.Name = "gbOM";
            gbOM.Size = new Size(0x1f7, 0xd8);
            gbOM.TabIndex = 0xc3;
            gbOM.TabStop = false;
            gbOM.Text = "Operation Mode";
            btnAutoCal.BackColor = Color.LightSlateGray;
            btnAutoCal.FlatAppearance.BorderColor = Color.Fuchsia;
            btnAutoCal.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnAutoCal.ForeColor = Color.White;
            btnAutoCal.Location = new Point(0x14b, 0x4b);
            btnAutoCal.Name = "btnAutoCal";
            btnAutoCal.Size = new Size(0x60, 0x2c);
            btnAutoCal.TabIndex = 0xc5;
            btnAutoCal.Text = "Auto Calibrate";
            btnAutoCal.UseVisualStyleBackColor = false;
            btnAutoCal.Click += new EventHandler(btnAutoCal_Click);
            chkAnalogLowNoise.AutoSize = true;
            chkAnalogLowNoise.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            chkAnalogLowNoise.ForeColor = Color.White;
            chkAnalogLowNoise.Location = new Point(0xe3, 0x21);
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
            pOverSampling.Location = new Point(1, 0x4b);
            pOverSampling.Name = "pOverSampling";
            pOverSampling.Size = new Size(0x12e, 130);
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
            rdoOSHiResMode.Location = new Point(7, 0x33);
            rdoOSHiResMode.Name = "rdoOSHiResMode";
            rdoOSHiResMode.Size = new Size(0x6c, 0x13);
            rdoOSHiResMode.TabIndex = 0xe0;
            rdoOSHiResMode.Text = "Hi Res Mode";
            rdoOSHiResMode.UseVisualStyleBackColor = true;
            rdoOSHiResMode.CheckedChanged += new EventHandler(rdoOSHiResMode_CheckedChanged);
            rdoOSLPMode.AutoSize = true;
            rdoOSLPMode.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
            rdoOSLPMode.Location = new Point(7, 0x4a);
            rdoOSLPMode.Name = "rdoOSLPMode";
            rdoOSLPMode.Size = new Size(0x87, 0x13);
            rdoOSLPMode.TabIndex = 0xdf;
            rdoOSLPMode.Text = "Low Power Mode";
            rdoOSLPMode.UseVisualStyleBackColor = true;
            rdoOSLPMode.CheckedChanged += new EventHandler(rdoOSLPMode_CheckedChanged);
            rdoOSLNLPMode.AutoSize = true;
            rdoOSLNLPMode.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
            rdoOSLNLPMode.Location = new Point(7, 0x61);
            rdoOSLNLPMode.Name = "rdoOSLNLPMode";
            rdoOSLNLPMode.Size = new Size(0xa6, 0x13);
            rdoOSLNLPMode.TabIndex = 0xdd;
            rdoOSLNLPMode.Text = "Low Noise Low Power";
            rdoOSLNLPMode.UseVisualStyleBackColor = true;
            rdoOSLNLPMode.CheckedChanged += new EventHandler(rdoOSLNLPMode_CheckedChanged);
            rdoOSNormalMode.AutoSize = true;
            rdoOSNormalMode.Checked = true;
            rdoOSNormalMode.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
            rdoOSNormalMode.Location = new Point(7, 0x1c);
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
            p1.Location = new Point(3, 0x1d);
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
            ledWake.LedStyle = LedStyle.Round3D;
            ledWake.Location = new Point(0x80, 0x59);
            ledWake.Name = "ledWake";
            ledWake.OffColor = Color.Red;
            ledWake.Size = new Size(30, 0x1f);
            ledWake.TabIndex = 0xbf;
            label71.AutoSize = true;
            label71.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label71.Location = new Point(0x27, 0x5e);
            label71.Name = "label71";
            label71.Size = new Size(0x5b, 0x10);
            label71.TabIndex = 0xc0;
            label71.Text = "Wake Mode";
            label173.AutoSize = true;
            label173.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label173.Location = new Point(0x16, 0x43);
            label173.Name = "label173";
            label173.Size = new Size(0x6c, 0x10);
            label173.TabIndex = 0xc2;
            label173.Text = "Standby Mode";
            lbsleep.AutoSize = true;
            lbsleep.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbsleep.Location = new Point(0x26, 0x7a);
            lbsleep.Name = "lbsleep";
            lbsleep.Size = new Size(0x5c, 0x10);
            lbsleep.TabIndex = 0xb9;
            lbsleep.Text = "Sleep Mode";
            ledStandby.LedStyle = LedStyle.Round3D;
            ledStandby.Location = new Point(0x80, 60);
            ledStandby.Name = "ledStandby";
            ledStandby.OffColor = Color.Red;
            ledStandby.Size = new Size(30, 0x1f);
            ledStandby.TabIndex = 0xc1;
            ledStandby.Value = true;
            ledSleep.LedStyle = LedStyle.Round3D;
            ledSleep.Location = new Point(0x80, 0x73);
            ledSleep.Name = "ledSleep";
            ledSleep.OffColor = Color.Red;
            ledSleep.Size = new Size(30, 0x1f);
            ledSleep.TabIndex = 0x91;
            panelAdvanced.Controls.Add(gbOD);
            panelAdvanced.Location = new Point(3, 0x183);
            panelAdvanced.Name = "panelAdvanced";
            panelAdvanced.Size = new Size(0x3ae, 0xd8);
            panelAdvanced.TabIndex = 0x4c;
            gbOD.BackColor = SystemColors.ActiveBorder;
            gbOD.Controls.Add(groupBox1);
            gbOD.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            gbOD.Location = new Point(8, -2);
            gbOD.Name = "gbOD";
            gbOD.Size = new Size(0x3a3, 0xd5);
            gbOD.TabIndex = 0xdb;
            gbOD.TabStop = false;
            groupBox1.BackColor = Color.LightSlateGray;
            groupBox1.Controls.Add(label21);
            groupBox1.Controls.Add(label24);
            groupBox1.Controls.Add(label5);
            groupBox1.Controls.Add(lblValL2PResult);
            groupBox1.Controls.Add(lblValP2LResult);
            groupBox1.Controls.Add(lblp2LTripAngle);
            groupBox1.Controls.Add(chkPLDefaultSettings);
            groupBox1.Controls.Add(pPLActive);
            groupBox1.Controls.Add(p6);
            groupBox1.Controls.Add(chkEnablePL);
            groupBox1.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            groupBox1.ForeColor = Color.White;
            groupBox1.Location = new Point(4, 13);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(0x399, 0xc0);
            groupBox1.TabIndex = 0xc1;
            groupBox1.TabStop = false;
            groupBox1.Text = "Orientation Detection";
            label21.AutoSize = true;
            label21.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label21.Location = new Point(0x147, 0x35);
            label21.Name = "label21";
            label21.Size = new Size(0x41, 13);
            label21.TabIndex = 0xe5;
            label21.Text = "Trip Angle";
            label24.AutoSize = true;
            label24.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label24.Location = new Point(0x145, 0x26);
            label24.Name = "label24";
            label24.Size = new Size(0x85, 13);
            label24.TabIndex = 0xe4;
            label24.Text = "Landscape-To-Portrait";
            label5.AutoSize = true;
            label5.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.Location = new Point(0xb3, 0x35);
            label5.Name = "label5";
            label5.Size = new Size(0x41, 13);
            label5.TabIndex = 0xe3;
            label5.Text = "Trip Angle";
            lblValL2PResult.AutoSize = true;
            lblValL2PResult.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblValL2PResult.Location = new Point(0x18e, 0x35);
            lblValL2PResult.Name = "lblValL2PResult";
            lblValL2PResult.Size = new Size(0x15, 13);
            lblValL2PResult.TabIndex = 0xe2;
            lblValL2PResult.Text = "66";
            lblValP2LResult.AutoSize = true;
            lblValP2LResult.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblValP2LResult.Location = new Point(250, 0x35);
            lblValP2LResult.Name = "lblValP2LResult";
            lblValP2LResult.Size = new Size(0x15, 13);
            lblValP2LResult.TabIndex = 0xe1;
            lblValP2LResult.Text = "24";
            lblp2LTripAngle.AutoSize = true;
            lblp2LTripAngle.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblp2LTripAngle.Location = new Point(0xb1, 0x26);
            lblp2LTripAngle.Name = "lblp2LTripAngle";
            lblp2LTripAngle.Size = new Size(0x85, 13);
            lblp2LTripAngle.TabIndex = 0xdf;
            lblp2LTripAngle.Text = "Portrait-To-Landscape";
            chkPLDefaultSettings.AutoSize = true;
            chkPLDefaultSettings.Enabled = false;
            chkPLDefaultSettings.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            chkPLDefaultSettings.ForeColor = Color.Gold;
            chkPLDefaultSettings.Location = new Point(11, 0x42);
            chkPLDefaultSettings.Name = "chkPLDefaultSettings";
            chkPLDefaultSettings.Size = new Size(0x94, 0x11);
            chkPLDefaultSettings.TabIndex = 0xde;
            chkPLDefaultSettings.Text = "Set Default Settings  ";
            chkPLDefaultSettings.UseVisualStyleBackColor = true;
            chkPLDefaultSettings.CheckedChanged += new EventHandler(chkPLDefaultSettings_CheckedChanged);
            pPLActive.Controls.Add(rdoDecDebouncePL);
            pPLActive.Controls.Add(rdoClrDebouncePL);
            pPLActive.Controls.Add(btnResetPLDebounce);
            pPLActive.Controls.Add(btnSetPLDebounce);
            pPLActive.Controls.Add(lblBouncems);
            pPLActive.Controls.Add(lblPLbounceVal);
            pPLActive.Controls.Add(lblDebouncePL);
            pPLActive.Controls.Add(tbPL);
            pPLActive.Enabled = false;
            pPLActive.Location = new Point(11, 0x5c);
            pPLActive.Name = "pPLActive";
            pPLActive.Size = new Size(0x1e8, 0x5b);
            pPLActive.TabIndex = 0xdd;
            rdoDecDebouncePL.AutoSize = true;
            rdoDecDebouncePL.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            rdoDecDebouncePL.Location = new Point(14, 0x3f);
            rdoDecDebouncePL.Name = "rdoDecDebouncePL";
            rdoDecDebouncePL.Size = new Size(130, 0x11);
            rdoDecDebouncePL.TabIndex = 0xc7;
            rdoDecDebouncePL.Text = "Decrement Debounce";
            rdoDecDebouncePL.UseVisualStyleBackColor = true;
            rdoDecDebouncePL.CheckedChanged += new EventHandler(rdoDecDebouncePL_CheckedChanged);
            rdoClrDebouncePL.AutoSize = true;
            rdoClrDebouncePL.Checked = true;
            rdoClrDebouncePL.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            rdoClrDebouncePL.Location = new Point(0xaf, 0x3f);
            rdoClrDebouncePL.Name = "rdoClrDebouncePL";
            rdoClrDebouncePL.Size = new Size(0x66, 0x11);
            rdoClrDebouncePL.TabIndex = 200;
            rdoClrDebouncePL.TabStop = true;
            rdoClrDebouncePL.Text = "Clear Debounce";
            rdoClrDebouncePL.UseVisualStyleBackColor = true;
            rdoClrDebouncePL.CheckedChanged += new EventHandler(rdoClrDebouncePL_CheckedChanged);
            btnResetPLDebounce.BackColor = Color.LightSlateGray;
            btnResetPLDebounce.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnResetPLDebounce.Location = new Point(0x197, 0x38);
            btnResetPLDebounce.Name = "btnResetPLDebounce";
            btnResetPLDebounce.Size = new Size(0x4e, 30);
            btnResetPLDebounce.TabIndex = 220;
            btnResetPLDebounce.Text = "Reset";
            btnResetPLDebounce.UseVisualStyleBackColor = false;
            btnResetPLDebounce.Click += new EventHandler(btnResetPLDebounce_Click);
            btnSetPLDebounce.BackColor = Color.LightSlateGray;
            btnSetPLDebounce.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSetPLDebounce.Location = new Point(0x158, 0x39);
            btnSetPLDebounce.Name = "btnSetPLDebounce";
            btnSetPLDebounce.Size = new Size(0x39, 0x1c);
            btnSetPLDebounce.TabIndex = 0xdb;
            btnSetPLDebounce.Text = "Set";
            btnSetPLDebounce.UseVisualStyleBackColor = false;
            btnSetPLDebounce.Click += new EventHandler(btnSetPLDebounce_Click);
            lblBouncems.AutoSize = true;
            lblBouncems.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblBouncems.Location = new Point(0x73, 0x12);
            lblBouncems.Name = "lblBouncems";
            lblBouncems.Size = new Size(0x16, 13);
            lblBouncems.TabIndex = 0xd1;
            lblBouncems.Text = "ms";
            lblPLbounceVal.AutoSize = true;
            lblPLbounceVal.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPLbounceVal.Location = new Point(0x42, 0x12);
            lblPLbounceVal.Name = "lblPLbounceVal";
            lblPLbounceVal.Size = new Size(14, 13);
            lblPLbounceVal.TabIndex = 0xd0;
            lblPLbounceVal.Text = "0";
            lblDebouncePL.AutoSize = true;
            lblDebouncePL.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblDebouncePL.Location = new Point(1, 0x12);
            lblDebouncePL.Name = "lblDebouncePL";
            lblDebouncePL.Size = new Size(0x45, 13);
            lblDebouncePL.TabIndex = 0xcf;
            lblDebouncePL.Text = "Debounce ";
            tbPL.Location = new Point(0x97, 12);
            tbPL.Maximum = 0xff;
            tbPL.Name = "tbPL";
            tbPL.Size = new Size(0x14e, 0x2d);
            tbPL.TabIndex = 0xce;
            tbPL.TickFrequency = 15;
            tbPL.Scroll += new EventHandler(tbPL_Scroll);
            p6.Controls.Add(label263);
            p6.Controls.Add(ddlHysteresisA);
            p6.Controls.Add(ddlPLTripA);
            p6.Controls.Add(label264);
            p6.Controls.Add(label265);
            p6.Controls.Add(ddlBFTripA);
            p6.Controls.Add(ddlZLockA);
            p6.Controls.Add(label266);
            p6.Enabled = false;
            p6.Location = new Point(510, 0x15);
            p6.Name = "p6";
            p6.Size = new Size(0x195, 0x97);
            p6.TabIndex = 0xda;
            label263.AutoSize = true;
            label263.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            label263.Location = new Point(3, 0x72);
            label263.Name = "label263";
            label263.Size = new Size(0x55, 13);
            label263.TabIndex = 0xd9;
            label263.Text = "Hysteresis Angle";
            ddlHysteresisA.DropDownStyle = ComboBoxStyle.DropDownList;
            ddlHysteresisA.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            ddlHysteresisA.FormattingEnabled = true;
            ddlHysteresisA.Items.AddRange(new object[] { "+/- 0\x00b0", "+/- 4\x00b0", "+/- 7\x00b0", "+/- 11\x00b0", "+/- 14\x00b0", "+/- 17\x00b0", "+/- 21\x00b0", "+/- 24\x00b0", "" });
            ddlHysteresisA.Location = new Point(0x77, 0x6c);
            ddlHysteresisA.Name = "ddlHysteresisA";
            ddlHysteresisA.Size = new Size(0x10f, 0x15);
            ddlHysteresisA.TabIndex = 0xd8;
            ddlHysteresisA.SelectedIndexChanged += new EventHandler(ddlHysteresisA_SelectedIndexChanged);
            ddlPLTripA.DropDownStyle = ComboBoxStyle.DropDownList;
            ddlPLTripA.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            ddlPLTripA.FormattingEnabled = true;
            ddlPLTripA.Items.AddRange(new object[] { "15\x00b0", "20\x00b0", "30\x00b0", "35\x00b0", "40\x00b0", "45\x00b0", "55\x00b0", "60\x00b0", "70\x00b0", "75\x00b0" });
            ddlPLTripA.Location = new Point(0x77, 0x4e);
            ddlPLTripA.Name = "ddlPLTripA";
            ddlPLTripA.Size = new Size(0x10f, 0x15);
            ddlPLTripA.TabIndex = 0xd7;
            ddlPLTripA.SelectedIndexChanged += new EventHandler(ddlPLTripA_SelectedIndexChanged);
            label264.AutoSize = true;
            label264.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            label264.Location = new Point(0x11, 0x52);
            label264.Name = "label264";
            label264.Size = new Size(0x47, 13);
            label264.TabIndex = 0xd6;
            label264.Text = "P-LTrip Angle";
            label265.AutoSize = true;
            label265.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            label265.Location = new Point(0x10, 0x34);
            label265.Name = "label265";
            label265.Size = new Size(0x4c, 13);
            label265.TabIndex = 0xd5;
            label265.Text = "B/F Trip Angle";
            ddlBFTripA.DropDownStyle = ComboBoxStyle.DropDownList;
            ddlBFTripA.Font = new Font("Microsoft Sans Serif", 6.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            ddlBFTripA.FormattingEnabled = true;
            ddlBFTripA.Items.AddRange(new object[] { "BF: Z<80\x00b0Z>280\x00b0 FB: Z>100\x00b0Z<260\x00b0", "BF: Z<75\x00b0Z>285\x00b0 FB: Z>105\x00b0Z<255\x00b0", "BF: Z<70\x00b0Z>290\x00b0 FB: Z>110\x00b0Z<250\x00b0", "BF: Z<65\x00b0Z>295\x00b0 FB: Z>115\x00b0Z<245\x00b0" });
            ddlBFTripA.Location = new Point(0x77, 0x31);
            ddlBFTripA.Name = "ddlBFTripA";
            ddlBFTripA.Size = new Size(0x10f, 20);
            ddlBFTripA.TabIndex = 0xd4;
            ddlBFTripA.SelectedIndexChanged += new EventHandler(ddlBFTripA_SelectedIndexChanged);
            ddlZLockA.DropDownStyle = ComboBoxStyle.DropDownList;
            ddlZLockA.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            ddlZLockA.FormattingEnabled = true;
            ddlZLockA.Items.AddRange(new object[] { "14\x00b0", "18\x00b0", "22\x00b0", "26\x00b0", "30\x00b0", "34\x00b0", "38\x00b0", "43\x00b0" });
            ddlZLockA.Location = new Point(0x77, 0x10);
            ddlZLockA.Name = "ddlZLockA";
            ddlZLockA.Size = new Size(0x10f, 0x15);
            ddlZLockA.TabIndex = 0xd3;
            ddlZLockA.SelectedIndexChanged += new EventHandler(ddlZLockA_SelectedIndexChanged);
            label266.AutoSize = true;
            label266.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            label266.Location = new Point(0x11, 20);
            label266.Name = "label266";
            label266.Size = new Size(0x4a, 13);
            label266.TabIndex = 210;
            label266.Text = "Z- Lock Angle";
            chkEnablePL.AutoSize = true;
            chkEnablePL.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            chkEnablePL.Location = new Point(11, 0x26);
            chkEnablePL.Name = "chkEnablePL";
            chkEnablePL.Size = new Size(0x5e, 0x11);
            chkEnablePL.TabIndex = 0xca;
            chkEnablePL.Text = "Enable P/L ";
            chkEnablePL.UseVisualStyleBackColor = true;
            chkEnablePL.CheckedChanged += new EventHandler(chkEnablePL_CheckedChanged);
            rdoStandby.AutoSize = true;
            rdoStandby.Checked = true;
            rdoStandby.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            rdoStandby.Location = new Point(0x12, 0x13);
            rdoStandby.Name = "rdoStandby";
            rdoStandby.Size = new Size(0x57, 20);
            rdoStandby.TabIndex = 0x70;
            rdoStandby.TabStop = true;
            rdoStandby.Text = "Standby ";
            rdoStandby.UseVisualStyleBackColor = true;
            rdoStandby.CheckedChanged += new EventHandler(rdoStandby_CheckedChanged);
            rdoActive.AutoSize = true;
            rdoActive.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            rdoActive.Location = new Point(0x6f, 0x13);
            rdoActive.Name = "rdoActive";
            rdoActive.Size = new Size(0x45, 20);
            rdoActive.TabIndex = 0x71;
            rdoActive.Text = "Active";
            rdoActive.UseVisualStyleBackColor = true;
            rdoActive.CheckedChanged += new EventHandler(rdoActive_CheckedChanged);
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
            pictureBox1.Location = new Point(0, -1);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(0x3b8, 0x39);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 0x48;
            pictureBox1.TabStop = false;
            CommStrip.Items.AddRange(new ToolStripItem[] { CommStripButton, toolStripStatusLabel });
            CommStrip.Location = new Point(0, 0x2bd);
            CommStrip.Name = "CommStrip";
            CommStrip.Size = new Size(0x3b8, 0x16);
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
            groupBox2.Controls.Add(rdoActive);
            groupBox2.Controls.Add(rdoStandby);
            groupBox2.Location = new Point(0x12, 9);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(200, 0x31);
            groupBox2.TabIndex = 0xc5;
            groupBox2.TabStop = false;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            AutoScroll = true;
            BackColor = Color.LightSlateGray;
            base.ClientSize = new Size(0x3b8, 0x2d3);
            base.Controls.Add(CommStrip);
            base.Controls.Add(panelGeneral);
            base.Controls.Add(pictureBox1);
            base.Controls.Add(menuStrip1);
            base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            base.Icon = (Icon) resources.GetObject("$Icon");
            base.MaximizeBox = false;
            base.Name = "OrientationDemo";
            base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            Text = "Orientation Application";
            base.Resize += new EventHandler(OrientationDemo_Resize);
            ((ISupportInitialize) gaugeYZ).EndInit();
            panelGeneral.ResumeLayout(false);
            panelDisplay.ResumeLayout(false);
            ((ISupportInitialize) PLImage).EndInit();
            ((ISupportInitialize) gaugeXY).EndInit();
            uxPanelSettings.ResumeLayout(false);
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
            ((ISupportInitialize) ledStandby).EndInit();
            ((ISupportInitialize) ledSleep).EndInit();
            panelAdvanced.ResumeLayout(false);
            gbOD.ResumeLayout(false);
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            pPLActive.ResumeLayout(false);
            pPLActive.PerformLayout();
            tbPL.EndInit();
            p6.ResumeLayout(false);
            p6.PerformLayout();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ((ISupportInitialize) pictureBox1).EndInit();
            CommStrip.ResumeLayout(false);
            CommStrip.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
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
                ImagePL_fu = (Image) manager.GetObject("STB_PL_front_up");
                ImagePL_fl = (Image) manager.GetObject("STB_PL_front_left");
                ImagePL_fd = (Image) manager.GetObject("STB_PL_front_down");
                ImagePL_fr = (Image) manager.GetObject("STB_PL_front_right");
                ImagePL_bu = (Image) manager.GetObject("STB_PL_back_up");
                ImagePL_bl = (Image) manager.GetObject("STB_PL_back_left");
                ImagePL_bd = (Image) manager.GetObject("STB_PL_back_down");
                ImagePL_br = (Image) manager.GetObject("STB_PL_back_right");
            }
            catch (Exception exception)
            {
                STBLogger.AddEvent(this, STBLogger.EventLevel.Information, "Exception", exception.Message + exception.Source + exception.StackTrace);
                ImageGreen = CommStripButton.Image;
                ImageYellow = CommStripButton.Image;
                ImageRed = CommStripButton.Image;
            }
        }

        private void OrientationDemo_Resize(object sender, EventArgs e)
        {
            windowResized = true;
            windowHeight = base.Height;
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
                gbOD.Enabled = false;
                gbDR.Enabled = false;
                gbOM.Enabled = false;
                p6.Enabled = false;
                btnAutoCal.Enabled = false;
                int[] datapassed = new int[] { 1 };
                ControllerObj.ActiveFlag = true;
                ControllerReqPacket reqName = new ControllerReqPacket();
                ControllerObj.CreateNewTaskFromGUI(reqName, 0, 0x53, datapassed);
                if (chkEnablePL.Checked)
                {
                    ControllerObj.ReturnPLStatus = true;
                }
            }
        }

        private void rdoClrDebouncePL_CheckedChanged(object sender, EventArgs e)
        {
            int num = rdoClrDebouncePL.Checked ? 0xff : 0;
            int[] datapassed = new int[] { num };
            ControllerObj.SetDBCNTM_PLFlag = true;
            ControllerReqPacket reqName = new ControllerReqPacket();
            ControllerObj.CreateNewTaskFromGUI(reqName, 4, 30, datapassed);
        }

        private void rdoDecDebouncePL_CheckedChanged(object sender, EventArgs e)
        {
            int num = rdoDecDebouncePL.Checked ? 0 : 0xff;
            int[] datapassed = new int[] { num };
            ControllerObj.SetDBCNTM_PLFlag = true;
            ControllerReqPacket reqName = new ControllerReqPacket();
            ControllerObj.CreateNewTaskFromGUI(reqName, 4, 30, datapassed);
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
                double num = tbPL.Value * DR_timestep;
                if (num > 1000.0)
                {
                    lblBouncems.Text = "s";
                    num /= 1000.0;
                    lblPLbounceVal.Text = string.Format("{0:F4}", num);
                }
                else
                {
                    lblBouncems.Text = "ms";
                    lblPLbounceVal.Text = string.Format("{0:F2}", num);
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
                double num = tbPL.Value * DR_timestep;
                if (num > 1000.0)
                {
                    lblBouncems.Text = "s";
                    num /= 1000.0;
                    lblPLbounceVal.Text = string.Format("{0:F4}", num);
                }
                else
                {
                    lblBouncems.Text = "ms";
                    lblPLbounceVal.Text = string.Format("{0:F2}", num);
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
                double num = tbPL.Value * DR_timestep;
                if (num > 1000.0)
                {
                    lblBouncems.Text = "s";
                    num /= 1000.0;
                    lblPLbounceVal.Text = string.Format("{0:F4}", num);
                }
                else
                {
                    lblBouncems.Text = "ms";
                    lblPLbounceVal.Text = string.Format("{0:F2}", num);
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
                double num = tbPL.Value * DR_timestep;
                if (num > 1000.0)
                {
                    lblBouncems.Text = "s";
                    num /= 1000.0;
                    lblPLbounceVal.Text = string.Format("{0:F4}", num);
                }
                else
                {
                    lblBouncems.Text = "ms";
                    lblPLbounceVal.Text = string.Format("{0:F2}", num);
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
                chkEnablePL.Enabled = true;
                chkPLDefaultSettings.Enabled = FullFeaturedDevice;
                gbOD.Enabled = true;
                gbDR.Enabled = true;
                gbOM.Enabled = true;
                p6.Enabled = true;
                btnAutoCal.Enabled = true;
                ControllerObj.ReturnPLStatus = false;
                int[] datapassed = new int[] { 0 };
                ControllerObj.ActiveFlag = true;
                ControllerReqPacket reqName = new ControllerReqPacket();
                ControllerObj.CreateNewTaskFromGUI(reqName, 0, 0x53, datapassed);
            }
        }

        private void SetPLInterrupts(bool enabled)
        {
            int[] datapassed = new int[] { enabled ? 0xff : 0, 0x10 };
            ControllerObj.SetIntsEnableFlag = true;
            ControllerReqPacket reqName = new ControllerReqPacket();
            ControllerObj.PollingOrInt[0] = 1;
            ControllerObj.CreateNewTaskFromGUI(reqName, 0, 13, datapassed);
            ControllerObj.AppRegisterWrite(new int[] { 0x15, 2 });
        }

        private void tbPL_Scroll(object sender, EventArgs e)
        {
            double num = 2.5;
            num = DR_timestep;
            double num2 = tbPL.Value * num;
            if (num2 > 1000.0)
            {
                lblBouncems.Text = "s";
                num2 /= 1000.0;
                lblPLbounceVal.Text = string.Format("{0:F4}", num2);
            }
            else
            {
                lblBouncems.Text = "ms";
                lblPLbounceVal.Text = string.Format("{0:F2}", num2);
            }
        }

        private void tmrTiltTimer_Tick(object sender, EventArgs e)
        {
            if (TimerEnabled && (ControllerObj.DeviceID != deviceID.Unsupported))
            {
                TimerEnabled = false;
                deviceID deviceId = ControllerObj.DeviceID;
                switch (deviceId)
                {
                    case deviceID.MMA8452Q:
                    case deviceID.MMA8453Q:
                    case deviceID.MMA8653FC:
                        DisableUnavailableFeatures();
                        break;

                    case deviceID.MMA8451Q:
                        lbl2gSensitivity.Text = "4096 counts/g 14b";
                        lbl4gSensitivity.Text = "2048 counts/g 14b";
                        lbl8gSensitivity.Text = "1024 counts/g 14b";
                        goto Label_01CF;
					/*
                    case deviceID.MMA8452Q:
                        lbl2gSensitivity.Text = "1024 counts/g 12b";
                        lbl4gSensitivity.Text = " 512 counts/g 12b";
                        lbl8gSensitivity.Text = " 256 counts/g 12b";
                        goto Label_01CF;
					 */
                }

                if (deviceId == deviceID.MMA8453Q)
                {
                    lbl2gSensitivity.Text = "256 counts/g 10b";
                    lbl4gSensitivity.Text = "128 counts/g 10b";
                    lbl8gSensitivity.Text = " 64 counts/g 10b";
                }
                else if (deviceId == deviceID.MMA8652FC)
                {
                    lbl2gSensitivity.Text = "1024 counts/g 12b";
                    lbl4gSensitivity.Text = " 512 counts/g 12b";
                    lbl8gSensitivity.Text = " 256 counts/g 12b";
                    chkAnalogLowNoise.Visible = false;
                }
                else if (deviceId == deviceID.MMA8653FC)
                {
                    lbl2gSensitivity.Text = "256 counts/g 10b";
                    lbl4gSensitivity.Text = "128 counts/g 10b";
                    lbl8gSensitivity.Text = " 64 counts/g 10b";
                    chkAnalogLowNoise.Visible = false;
                }
            }
        Label_01CF:
            UpdateFormState();
            UpdateMarkersInGauges();
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

        private void UpdateMarkersInGauges()
        {
            double num = 0.0;
            double num2 = 0.0;
            try
            {
                double[] numArray = new double[] { 10.0, 15.0, 20.0, 25.0 };
                double[] numArray2 = new double[] { 25.0, 28.6, 32.1, 35.7, 39.3, 42.9, 46.4, 50.0 };
                num = numArray[ddlBFTripA.SelectedIndex];
                num2 = numArray2[ddlZLockA.SelectedIndex];
                double minimum = P2LResultA;
                double maximum = L2PResultA;
                if (P2LResultA == L2PResultA)
                {
                    maximum += 0.1;
                }
                Range range = new Range(minimum, maximum);
                gaugeXY.RangeFills[0].Range = range;
                range = new Range(180.0 - maximum, 180.0 - minimum);
                gaugeXY.RangeFills[1].Range = range;
                range = new Range(180.0 + minimum, 180.0 + maximum);
                gaugeXY.RangeFills[2].Range = range;
                range = new Range(360.0 - maximum, 360.0 - minimum);
                gaugeXY.RangeFills[3].Range = range;
                range = new Range(90.0 - num, 90.0 + num);
                gaugeYZ.RangeFills[0].Range = range;
                range = new Range(270.0 - num, 270.0 + num);
                gaugeYZ.RangeFills[1].Range = range;
                range = new Range(minimum, 180.0 - minimum);
                gaugeXY.RangeFills[4].Range = range;
                range = new Range(180.0 - maximum, 180.0 + maximum);
                gaugeXY.RangeFills[5].Range = range;
                range = new Range(180.0 + minimum, 360.0 - minimum);
                gaugeXY.RangeFills[6].Range = range;
                range = new Range(360.0 - maximum, 360.0);
                gaugeXY.RangeFills[7].Range = range;
                range = new Range(0.0, maximum);
                gaugeXY.RangeFills[8].Range = range;
                if (ledCurPLLO)
                {
                    gaugeXY.RangeFills[4].Visible = false;
                    gaugeXY.RangeFills[5].Visible = false;
                    gaugeXY.RangeFills[6].Visible = false;
                    gaugeXY.RangeFills[7].Visible = false;
                    gaugeXY.RangeFills[8].Visible = false;
                    lblLockOut.Text = "Lock Out";
                    lblLockOut.ForeColor = Color.FromArgb(0xff, 0xff, 0, 0);
                    lblLockOut.Visible = true;
                }
                else
                {
                    lblLockOut.Visible = false;
                    if (ledCurPLUp)
                    {
                        gaugeXY.RangeFills[4].Visible = true;
                        gaugeXY.RangeFills[5].Visible = false;
                        gaugeXY.RangeFills[6].Visible = false;
                        gaugeXY.RangeFills[7].Visible = false;
                        gaugeXY.RangeFills[8].Visible = false;
                        lblLockOut.Text = "Up";
                        lblLockOut.ForeColor = Color.FromArgb(0xff, 0, 0, 0xff);
                        lblLockOut.Visible = true;
                    }
                    else if (ledCurPLLeft)
                    {
                        gaugeXY.RangeFills[4].Visible = false;
                        gaugeXY.RangeFills[5].Visible = true;
                        gaugeXY.RangeFills[6].Visible = false;
                        gaugeXY.RangeFills[7].Visible = false;
                        gaugeXY.RangeFills[8].Visible = false;
                        lblLockOut.Text = "Left";
                        lblLockOut.ForeColor = Color.FromArgb(0xff, 0, 0, 0xff);
                        lblLockOut.Visible = true;
                    }
                    else if (ledCurPLDown)
                    {
                        gaugeXY.RangeFills[4].Visible = false;
                        gaugeXY.RangeFills[5].Visible = false;
                        gaugeXY.RangeFills[6].Visible = true;
                        gaugeXY.RangeFills[7].Visible = false;
                        gaugeXY.RangeFills[8].Visible = false;
                        lblLockOut.Text = "Down";
                        lblLockOut.ForeColor = Color.FromArgb(0xff, 0, 0, 0xff);
                        lblLockOut.Visible = true;
                    }
                    else if (ledCurPLRight)
                    {
                        gaugeXY.RangeFills[4].Visible = false;
                        gaugeXY.RangeFills[5].Visible = false;
                        gaugeXY.RangeFills[6].Visible = false;
                        gaugeXY.RangeFills[7].Visible = true;
                        gaugeXY.RangeFills[8].Visible = true;
                        lblLockOut.Text = "Right";
                        lblLockOut.ForeColor = Color.FromArgb(0xff, 0, 0, 0xff);
                        lblLockOut.Visible = true;
                    }
                }
                if (ledCurPLFront)
                {
                    gaugeYZ.RangeFills[4].Visible = false;
                    gaugeYZ.RangeFills[3].Visible = true;
                    gaugeYZ.RangeFills[2].Visible = true;
                    lblFB.Text = "Front";
                    lblFB.ForeColor = Color.FromArgb(0xff, 0, 0, 0xff);
                    lblFB.Visible = true;
                }
                else if (ledCurPLBack)
                {
                    gaugeYZ.RangeFills[4].Visible = true;
                    gaugeYZ.RangeFills[3].Visible = false;
                    gaugeYZ.RangeFills[2].Visible = false;
                    lblFB.Text = "Back";
                    lblFB.ForeColor = Color.FromArgb(0xff, 0, 0, 0xff);
                    lblFB.Visible = true;
                }
                else
                {
                    gaugeYZ.RangeFills[4].Visible = false;
                    gaugeYZ.RangeFills[3].Visible = false;
                    gaugeYZ.RangeFills[2].Visible = false;
                    lblFB.Text = "";
                    lblFB.ForeColor = Color.FromArgb(0xff, 0, 0, 0xff);
                    lblFB.Visible = true;
                }
            }
            catch (Exception)
            {
            }
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

