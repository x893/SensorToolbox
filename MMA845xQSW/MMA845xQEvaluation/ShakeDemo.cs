﻿namespace MMA845xQEvaluation
{
    using Freescale.SASD.Communication;
    using GlobalSTB;
    using LCDLabel;
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

    public class ShakeDemo : Form
    {
        private GroupBox AccelControllerGroup;
        private Button btnAutoCal;
        private Button btnResetTransient;
        private Button btnSetTransient;
        private CheckBox chkAnalogLowNoise;
        private CheckBox chkDefaultTransSettings;
        private CheckBox chkTransBypassHPF;
        private CheckBox chkTransEnableLatch;
        private CheckBox chkTransEnableXFlag;
        private CheckBox chkTransEnableYFlag;
        private CheckBox chkTransEnableZFlag;
        private eCommMode CommMode = eCommMode.Closed;
        private StatusStrip CommStrip;
        private ToolStripDropDownButton CommStripButton;
        private IContainer components = null;
        private object ControllerEventsLock = new object();
        private AccelController ControllerObj;
        private string CurrentFW = "4003";
        private ComboBox ddlDataRate;
        private int DeviceID;
        private double DR_timestep;
        private BoardComm dv;
        private ToolStripMenuItem enableLogDataApplicationToolStripMenuItem;
        private bool FirstTimeLoad = true;
        private const int Form_Max_Height = 720;
        private const int Form_Min_Height = 200;
        private int FullScaleValue;
        private GroupBox gbDR;
        private GroupBox gbOM;
        private GroupBox gbTS;
        private int I_panelAdvanced_height;
        private int I_panelDisplay_height;
        private int I_panelRegisters_height;
        private int ImageCount = 8;
        private Image ImageGreen;
        private Image ImageRed;
        private Image[] Images = new Image[9];
        private Image ImageYellow;
        private Label label173;
        private Label label35;
        private Label label67;
        private Label label68;
        private Label label69;
        private Label label70;
        private Label label71;
        private Label lbl2gSensitivity;
        private Label lbl4gSensitivity;
        private Label lbl8gSensitivity;
        private Label lblTransDebounce;
        private Label lblTransDebouncems;
        private Label lblTransDebounceVal;
        private Label lblTransPolX;
        private Label lblTransPolZ;
        private Label lblTransThreshold;
        private Label lblTransThresholdg;
        private Label lblTransThresholdVal;
        private Label lbsleep;
        private Led ledSleep;
        private Led ledStandby;
        private Led ledTransEA;
        private Led ledTransXDetect;
        private Led ledTransYDetect;
        private Led ledTransZDetect;
        private Led ledWake;
        private bool LoadingFW = false;
        private ToolStripMenuItem logDataToolStripMenuItem1;
        private MenuStrip menuStrip1;
        private Panel p1;
        private Panel p18;
        private Panel panel1;
        private Panel panelAdvanced;
        private Panel panelDisplay;
        private FlowLayoutPanel panelGeneral;
        private PictureBox picBox1;
        private PictureBox picBox2;
        private PictureBox pictureBox1;
        private PictureBox pictureBox2;
        private Panel pOverSampling;
        private Panel pTrans2;
        private RadioButton rdo2g;
        private RadioButton rdo4g;
        private RadioButton rdo8g;
        private RadioButton rdoActive;
        private RadioButton rdoOSHiResMode;
        private RadioButton rdoOSLNLPMode;
        private RadioButton rdoOSLPMode;
        private RadioButton rdoOSNormalMode;
        private RadioButton rdoStandby;
        private RadioButton rdoTransClearDebounce;
        private RadioButton rdoTransDecDebounce;
        private string[] RegisterNames = new string[] { 
            "STATUS", "OUT_XMSB", "OUT_XLSB", "OUT_YMSB", "OUT_YLSB", "OUT_ZMSB", "OUT_ZLSB", "RESERVED ", "RESERVED ", "FSETUP", "TRIGCFG", "SYSMOD", "INTSOURCE", "WHOAMI", "XYZDATACFG", "HPFILTER", 
            "PLSTATUS", "PLCFG", "PLCOUNT", "PLBFZCOMP", "PLPLTHSREG", "FFMTCFG", "FFMTSRC", "FFMTTHS", "FFMTCOUNT", "RESERVED", "RESERVED", "RESERVED", "RESERVED", "TRANSCFG", "TRANSSRC", "TRANSTHS", 
            "TRANSCOUNT", "PULSECFG", "PULSESRC", "PULSETHSX", "PULSETHSY", "PULSETHSZ", "PULSETMLT", "PULSELTCY", "PULSEWIND", "ASLPCOUNT", "CTRLREG1", "CTRLREG2", "CTRLREG3", "CTRLREG4", "CTRLREG5", "OFFSET_X", 
            "OFFSET_Y", "OFFSET_Z"
         };
        private SaveFileDialog saveFileDialog1;
        private TrackBar tbTransDebounce;
        private TrackBar tbTransThreshold;
        private int timercount;
        private System.Windows.Forms.Timer tmrShakeTimer;
        private ToolStripStatusLabel toolStripStatusLabel;
        private GroupBox uxControls;
        private Label uxLabelEvent;
        private LcdLabel uxLeft;
        private ProgressBar uxLeftIndicator;
        private LcdLabel uxRight;
        private ProgressBar uxRightIndicator;
        private int WakeOSMode;
        private bool windowResized = false;
        private int x_counter;

        public ShakeDemo(object controllerObj)
        {
            InitializeComponent();
            Styles.FormatForm(this, new string[] { "uxLabelEvent", "uxControls" });
            Styles.FormatInterruptPanel(panelAdvanced);
            panel1.BackColor = Color.Black;
            ControllerObj = (AccelController) controllerObj;
            ControllerObj.ControllerEvents += new AccelController.ControllerEventHandler(ControllerObj_ControllerEvents);
            DeviceID = (int) ControllerObj.DeviceID;
            menuStrip1.Enabled = false;
            I_panelDisplay_height = panelDisplay.Height;
            I_panelAdvanced_height = panelAdvanced.Height;
            timercount = 0;
            InitializeStatusBar();
            x_counter = 0;
            picBox1.Image = Images[x_counter];
            picBox2.Image = Images[x_counter + 1];
            panelDisplay.Height = I_panelDisplay_height;
            panelAdvanced.Height = I_panelAdvanced_height;
            tmrShakeTimer.Enabled = true;
            tmrShakeTimer.Start();
        }

        private void btnAutoCal_Click(object sender, EventArgs e)
        {
            chkDefaultTransSettings.Checked = false;
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

        private void btnResetTransient_Click(object sender, EventArgs e)
        {
            btnSetTransient.Enabled = true;
            btnResetTransient.Enabled = false;
            tbTransThreshold.Enabled = true;
            tbTransDebounce.Enabled = true;
            lblTransThreshold.Enabled = true;
            lblTransThresholdVal.Enabled = true;
            lblTransThresholdg.Enabled = true;
            lblTransDebounce.Enabled = true;
            lblTransDebounceVal.Enabled = true;
            lblTransDebouncems.Enabled = true;
        }

        private void btnSetTransient_Click(object sender, EventArgs e)
        {
            int[] datapassed = new int[] { tbTransThreshold.Value };
            ControllerObj.SetTransThresholdFlag = true;
            ControllerReqPacket reqName = new ControllerReqPacket();
            ControllerObj.CreateNewTaskFromGUI(reqName, 5, 0x24, datapassed);
            int[] numArray2 = new int[] { tbTransDebounce.Value };
            ControllerObj.SetTransDebounceFlag = true;
            ControllerReqPacket packet2 = new ControllerReqPacket();
            ControllerObj.CreateNewTaskFromGUI(packet2, 5, 0x25, numArray2);
            btnSetTransient.Enabled = false;
            btnResetTransient.Enabled = true;
            tbTransThreshold.Enabled = false;
            tbTransDebounce.Enabled = false;
            lblTransThreshold.Enabled = false;
            lblTransThresholdVal.Enabled = false;
            lblTransThresholdg.Enabled = false;
            lblTransDebounce.Enabled = false;
            lblTransDebounceVal.Enabled = false;
            lblTransDebouncems.Enabled = false;
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

        private void chkDefaultTransSettings_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDefaultTransSettings.Checked)
            {
                ddlDataRate.SelectedIndex = 6;
                rdoOSNormalMode.Checked = true;
                rdoOSNormalMode_CheckedChanged(this, null);
                rdo8g.Checked = true;
                rdo8g_CheckedChanged(this, null);
                chkAnalogLowNoise.Checked = false;
                chkAnalogLowNoise_CheckedChanged(this, null);
                chkTransEnableXFlag.Checked = false;
                int[] datapassed = new int[] { 0 };
                ControllerObj.SetTransEnableXFlagFlag = true;
                ControllerReqPacket reqName = new ControllerReqPacket();
                ControllerObj.CreateNewTaskFromGUI(reqName, 5, 0x27, datapassed);
                chkTransEnableYFlag.Checked = true;
                int[] numArray2 = new int[] { 0xff };
                ControllerObj.SetTransEnableYFlagFlag = true;
                ControllerReqPacket packet2 = new ControllerReqPacket();
                ControllerObj.CreateNewTaskFromGUI(packet2, 5, 40, numArray2);
                chkTransEnableZFlag.Checked = false;
                int[] numArray3 = new int[] { 0 };
                ControllerObj.SetTransEnableZFlagFlag = true;
                ControllerReqPacket packet3 = new ControllerReqPacket();
                ControllerObj.CreateNewTaskFromGUI(packet3, 5, 0x29, numArray3);
                tbTransThreshold.Value = 10;
                double num2 = 0.063;
                double num = tbTransThreshold.Value * num2;
                lblTransThresholdVal.Text = string.Format("{0:F2}", num);
                int[] numArray4 = new int[] { tbTransThreshold.Value };
                ControllerObj.SetTransThresholdFlag = true;
                ControllerReqPacket packet4 = new ControllerReqPacket();
                ControllerObj.CreateNewTaskFromGUI(packet4, 5, 0x24, numArray4);
                tbTransDebounce.Value = 5;
                double num3 = tbTransDebounce.Value * DR_timestep;
                lblTransDebouncems.Text = "ms";
                lblTransDebounceVal.Text = string.Format("{0:F1}", num3);
                int[] numArray5 = new int[] { tbTransDebounce.Value };
                ControllerObj.SetTransDebounceFlag = true;
                ControllerReqPacket packet5 = new ControllerReqPacket();
                ControllerObj.CreateNewTaskFromGUI(packet5, 5, 0x25, numArray5);
                btnSetTransient.Enabled = false;
                btnResetTransient.Enabled = true;
                tbTransThreshold.Enabled = false;
                tbTransDebounce.Enabled = false;
                lblTransThreshold.Enabled = false;
                lblTransThresholdVal.Enabled = false;
                lblTransThresholdg.Enabled = false;
                lblTransDebounce.Enabled = false;
                lblTransDebounceVal.Enabled = false;
                lblTransDebouncems.Enabled = false;
            }
        }

        private void chkTransBypassHPF_CheckedChanged(object sender, EventArgs e)
        {
            int num = chkTransBypassHPF.Checked ? 0xff : 0;
            int[] datapassed = new int[] { num };
            ControllerObj.SetTransBypassHPFFlag = true;
            ControllerReqPacket reqName = new ControllerReqPacket();
            ControllerObj.CreateNewTaskFromGUI(reqName, 5, 0x23, datapassed);
        }

        private void chkTransEnableLatch_CheckedChanged(object sender, EventArgs e)
        {
            int num = chkTransEnableLatch.Checked ? 0xff : 0;
            int[] datapassed = new int[] { num };
            ControllerObj.SetTransEnableLatchFlag = true;
            ControllerReqPacket reqName = new ControllerReqPacket();
            ControllerObj.CreateNewTaskFromGUI(reqName, 5, 0x2a, datapassed);
        }

        private void chkTransEnableXFlag_CheckedChanged(object sender, EventArgs e)
        {
            int num = chkTransEnableXFlag.Checked ? 0xff : 0;
            int[] datapassed = new int[] { num };
            ControllerObj.SetTransEnableXFlagFlag = true;
            ControllerReqPacket reqName = new ControllerReqPacket();
            ControllerObj.CreateNewTaskFromGUI(reqName, 5, 0x27, datapassed);
        }

        private void chkTransEnableYFlag_CheckedChanged(object sender, EventArgs e)
        {
            int num = chkTransEnableYFlag.Checked ? 0xff : 0;
            int[] datapassed = new int[] { num };
            ControllerObj.SetTransEnableYFlagFlag = true;
            ControllerReqPacket reqName = new ControllerReqPacket();
            ControllerObj.CreateNewTaskFromGUI(reqName, 5, 40, datapassed);
        }

        private void chkTransEnableZFlag_CheckedChanged(object sender, EventArgs e)
        {
            int num = chkTransEnableZFlag.Checked ? 0xff : 0;
            int[] datapassed = new int[] { num };
            ControllerObj.SetTransEnableZFlagFlag = true;
            ControllerReqPacket reqName = new ControllerReqPacket();
            ControllerObj.CreateNewTaskFromGUI(reqName, 5, 0x29, datapassed);
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
            if (num == 9)
            {
                GUIUpdatePacket packet = (GUIUpdatePacket) o;
                if (packet.PayLoad.Count > 0)
                {
                    int[] numArray = (int[]) packet.PayLoad.Dequeue();
                    if ((numArray[0] & 0x40) != 0)
                    {
                        ledTransEA.Value = true;
                    }
                    if ((numArray[0] & 0x20) != 0)
                    {
                        ledTransZDetect.Value = true;
                    }
                    else
                    {
                        ledTransZDetect.Value = false;
                    }
                    if ((numArray[0] & 8) != 0)
                    {
                        ledTransYDetect.Value = true;
                    }
                    else
                    {
                        ledTransYDetect.Value = false;
                    }
                    if ((numArray[0] & 2) != 0)
                    {
                        ledTransXDetect.Value = true;
                    }
                    else
                    {
                        ledTransXDetect.Value = false;
                    }
                    if ((numArray[0] & 4) == 0)
                    {
						uxRightIndicator.Invoke(
							(MethodInvoker)delegate()
							{
								uxRightIndicator.Value = 100;
							});
                    }
                    else
                    {
						uxLeftIndicator.Invoke(
							(MethodInvoker)
							delegate()
							{
								uxLeftIndicator.Value = 100;
							});
                    }
                    if (ledTransYDetect.Value)
                    {
                        if ((numArray[0] & 4) == 0)
                        {
                            x_counter++;
                            if (x_counter >= ImageCount)
                            {
                                x_counter = ImageCount - 1;
                            }
                        }
                        else
                        {
                            x_counter--;
                            if (x_counter < 0)
                            {
                                x_counter = 0;
                            }
                        }
                        try
                        {
                            picBox1.Image = Images[x_counter];
                            picBox2.Image = Images[x_counter + 1];
                        }
                        catch (InvalidOperationException)
                        {
                        }
                    }
                }
            }
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
                    chkDefaultTransSettings.Checked = false;
                    break;

                case 1:
                    DR_timestep = 2.5;
                    chkDefaultTransSettings.Checked = false;
                    break;

                case 2:
                    switch (WakeOSMode)
                    {
                        case 2:
                            DR_timestep = 2.5;
                            goto Label_0317;

                        case 3:
                            DR_timestep = 5.0;
                            goto Label_0317;
                    }
                    DR_timestep = 5.0;
                    break;

                case 3:
                    chkDefaultTransSettings.Checked = false;
                    switch (WakeOSMode)
                    {
                        case 2:
                            DR_timestep = 2.5;
                            goto Label_0317;

                        case 3:
                            DR_timestep = 10.0;
                            goto Label_0317;
                    }
                    DR_timestep = 10.0;
                    break;

                case 4:
                    chkDefaultTransSettings.Checked = false;
                    switch (WakeOSMode)
                    {
                        case 2:
                            DR_timestep = 2.5;
                            goto Label_0317;

                        case 3:
                            DR_timestep = 20.0;
                            goto Label_0317;
                    }
                    DR_timestep = 20.0;
                    break;

                case 5:
                    chkDefaultTransSettings.Checked = false;
                    switch (WakeOSMode)
                    {
                        case 0:
                            DR_timestep = 20.0;
                            goto Label_0317;

                        case 1:
                            DR_timestep = 80.0;
                            goto Label_0317;

                        case 2:
                            DR_timestep = 2.5;
                            goto Label_0317;

                        case 3:
                            DR_timestep = 80.0;
                            goto Label_0317;
                    }
                    break;

                case 6:
                    chkDefaultTransSettings.Checked = false;
                    switch (WakeOSMode)
                    {
                        case 0:
                            DR_timestep = 20.0;
                            goto Label_0317;

                        case 1:
                            DR_timestep = 80.0;
                            goto Label_0317;

                        case 2:
                            DR_timestep = 2.5;
                            goto Label_0317;

                        case 3:
                            DR_timestep = 160.0;
                            goto Label_0317;
                    }
                    break;

                case 7:
                    chkDefaultTransSettings.Checked = false;
                    switch (WakeOSMode)
                    {
                        case 0:
                            DR_timestep = 20.0;
                            goto Label_0317;

                        case 1:
                            DR_timestep = 80.0;
                            goto Label_0317;

                        case 2:
                            DR_timestep = 2.5;
                            goto Label_0317;

                        case 3:
                            DR_timestep = 160.0;
                            goto Label_0317;
                    }
                    break;
            }
        Label_0317:
            num = tbTransDebounce.Value * DR_timestep;
            if (num > 1000.0)
            {
                lblTransDebouncems.Text = "s";
                num /= 1000.0;
                lblTransDebounceVal.Text = string.Format("{0:F2}", num);
            }
            else
            {
                lblTransDebouncems.Text = "ms";
                lblTransDebounceVal.Text = string.Format("{0:F2}", num);
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

        private void DeleteResource()
        {
            ImageGreen.Dispose();
            ImageYellow.Dispose();
            ImageRed.Dispose();
            for (int i = 0; i < Images.Length; i++)
            {
                Images[i].Dispose();
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
        }

        public void EndDemo()
        {
            lock (ControllerEventsLock)
            {
                tmrShakeTimer.Stop();
                tmrShakeTimer.Enabled = false;
                ControllerObj.ResetDevice();
                ControllerObj.ControllerEvents -= new AccelController.ControllerEventHandler(ControllerObj_ControllerEvents);
                DeleteResource();
                base.Close();
            }
        }

        private void InitDevice()
        {
            ControllerObj.ResetDevice();
            ControllerObj.StartDevice();
            Thread.Sleep(10);
            ControllerObj.Boot();
            Thread.Sleep(20);
            ddlDataRate.SelectedIndex = 6;
            DR_timestep = 5.0;
            WakeOSMode = 0;
            FullScaleValue = 2;
            chkDefaultTransSettings.Checked = true;
            chkDefaultTransSettings_CheckedChanged(this, null);
            chkTransEnableLatch.Checked = true;
            chkTransEnableLatch_CheckedChanged(this, null);
            SetTransInterrupts(true);
            rdo8g.Checked = true;
            rdo8g_CheckedChanged(this, null);
            rdoActive.Checked = true;
            rdoActive_CheckedChanged(this, null);
            UpdateFormState();
            ControllerObj.ReturnTransStatus = true;
            if (DeviceID == 2)
            {
                lbl2gSensitivity.Text = "4096 counts/g 14b";
                lbl4gSensitivity.Text = "2048 counts/g 14b";
                lbl8gSensitivity.Text = "1024 counts/g 14b";
            }
            else if (DeviceID == 3)
            {
                lbl2gSensitivity.Text = "1024 counts/g 12b";
                lbl4gSensitivity.Text = " 512 counts/g 12b";
                lbl8gSensitivity.Text = " 256 counts/g 12b";
            }
            else if (DeviceID == 4)
            {
                lbl2gSensitivity.Text = "256 counts/g 10b";
                lbl4gSensitivity.Text = "128 counts/g 10b";
                lbl8gSensitivity.Text = " 64 counts/g 10b";
            }
            else if (DeviceID == 5)
            {
                lbl2gSensitivity.Text = "1024 counts/g 12b";
                lbl4gSensitivity.Text = " 512 counts/g 12b";
                lbl8gSensitivity.Text = " 256 counts/g 12b";
                chkAnalogLowNoise.Visible = false;
            }
            else if (DeviceID == 6)
            {
                lbl2gSensitivity.Text = "256 counts/g 10b";
                lbl4gSensitivity.Text = "128 counts/g 10b";
                lbl8gSensitivity.Text = " 64 counts/g 10b";
                chkAnalogLowNoise.Visible = false;
            }
        }

        private void InitializeComponent()
        {
            components = new Container();
            ComponentResourceManager resources = new ComponentResourceManager(typeof(ShakeDemo));
            tmrShakeTimer = new System.Windows.Forms.Timer(components);
            panelGeneral = new FlowLayoutPanel();
            panelDisplay = new Panel();
            uxRightIndicator = new ProgressBar();
            uxLeftIndicator = new ProgressBar();
            panel1 = new Panel();
            uxLeft = new LcdLabel();
            uxRight = new LcdLabel();
            pictureBox1 = new PictureBox();
            lblTransPolZ = new Label();
            picBox2 = new PictureBox();
            ledTransYDetect = new Led();
            lblTransPolX = new Label();
            label68 = new Label();
            pTrans2 = new Panel();
            ledTransEA = new Led();
            uxLabelEvent = new Label();
            ledTransZDetect = new Led();
            label67 = new Label();
            ledTransXDetect = new Led();
            label69 = new Label();
            picBox1 = new PictureBox();
            panelAdvanced = new Panel();
            gbTS = new GroupBox();
            btnResetTransient = new Button();
            btnSetTransient = new Button();
            rdoTransClearDebounce = new RadioButton();
            p18 = new Panel();
            chkDefaultTransSettings = new CheckBox();
            chkTransBypassHPF = new CheckBox();
            chkTransEnableLatch = new CheckBox();
            chkTransEnableXFlag = new CheckBox();
            chkTransEnableZFlag = new CheckBox();
            chkTransEnableYFlag = new CheckBox();
            tbTransDebounce = new TrackBar();
            lblTransDebounceVal = new Label();
            rdoTransDecDebounce = new RadioButton();
            tbTransThreshold = new TrackBar();
            lblTransDebounce = new Label();
            lblTransThreshold = new Label();
            lblTransThresholdVal = new Label();
            lblTransThresholdg = new Label();
            lblTransDebouncems = new Label();
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
            pOverSampling = new Panel();
            label70 = new Label();
            chkAnalogLowNoise = new CheckBox();
            rdoOSHiResMode = new RadioButton();
            rdoOSLPMode = new RadioButton();
            rdoOSLNLPMode = new RadioButton();
            rdoOSNormalMode = new RadioButton();
            p1 = new Panel();
            ddlDataRate = new ComboBox();
            label35 = new Label();
            label173 = new Label();
            label71 = new Label();
            ledStandby = new Led();
            ledWake = new Led();
            ledSleep = new Led();
            lbsleep = new Label();
            rdoStandby = new RadioButton();
            rdoActive = new RadioButton();
            menuStrip1 = new MenuStrip();
            logDataToolStripMenuItem1 = new ToolStripMenuItem();
            enableLogDataApplicationToolStripMenuItem = new ToolStripMenuItem();
            saveFileDialog1 = new SaveFileDialog();
            pictureBox2 = new PictureBox();
            CommStrip = new StatusStrip();
            CommStripButton = new ToolStripDropDownButton();
            toolStripStatusLabel = new ToolStripStatusLabel();
            uxControls = new GroupBox();
            panelGeneral.SuspendLayout();
            panelDisplay.SuspendLayout();
            panel1.SuspendLayout();
            ((ISupportInitialize) pictureBox1).BeginInit();
            ((ISupportInitialize) picBox2).BeginInit();
            ((ISupportInitialize) ledTransYDetect).BeginInit();
            pTrans2.SuspendLayout();
            ((ISupportInitialize) ledTransEA).BeginInit();
            ((ISupportInitialize) ledTransZDetect).BeginInit();
            ((ISupportInitialize) ledTransXDetect).BeginInit();
            ((ISupportInitialize) picBox1).BeginInit();
            panelAdvanced.SuspendLayout();
            gbTS.SuspendLayout();
            p18.SuspendLayout();
            tbTransDebounce.BeginInit();
            tbTransThreshold.BeginInit();
            AccelControllerGroup.SuspendLayout();
            gbDR.SuspendLayout();
            gbOM.SuspendLayout();
            pOverSampling.SuspendLayout();
            p1.SuspendLayout();
            ((ISupportInitialize) ledStandby).BeginInit();
            ((ISupportInitialize) ledWake).BeginInit();
            ((ISupportInitialize) ledSleep).BeginInit();
            ((ISupportInitialize) pictureBox2).BeginInit();
            CommStrip.SuspendLayout();
            uxControls.SuspendLayout();
            base.SuspendLayout();
            tmrShakeTimer.Interval = 40;
            tmrShakeTimer.Tick += new EventHandler(tmrTiltTimer_Tick);
            panelGeneral.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            panelGeneral.AutoScroll = true;
            panelGeneral.Controls.Add(panelDisplay);
            panelGeneral.Controls.Add(panelAdvanced);
            panelGeneral.Location = new Point(1, 0x3b);
            panelGeneral.Name = "panelGeneral";
            panelGeneral.Size = new Size(0x44c, 0x1d4);
            panelGeneral.TabIndex = 0x47;
            panelDisplay.BorderStyle = BorderStyle.FixedSingle;
            panelDisplay.Controls.Add(uxControls);
            panelDisplay.Controls.Add(panel1);
            panelDisplay.Controls.Add(pictureBox1);
            panelDisplay.Controls.Add(lblTransPolZ);
            panelDisplay.Controls.Add(picBox2);
            panelDisplay.Controls.Add(ledTransYDetect);
            panelDisplay.Controls.Add(lblTransPolX);
            panelDisplay.Controls.Add(label68);
            panelDisplay.Controls.Add(pTrans2);
            panelDisplay.Controls.Add(ledTransZDetect);
            panelDisplay.Controls.Add(label67);
            panelDisplay.Controls.Add(ledTransXDetect);
            panelDisplay.Controls.Add(label69);
            panelDisplay.Controls.Add(picBox1);
            panelDisplay.Location = new Point(3, 3);
            panelDisplay.Name = "panelDisplay";
            panelDisplay.Size = new Size(0x20b, 0x1ce);
            panelDisplay.TabIndex = 0x49;
            uxRightIndicator.Location = new Point(0x1b0, 5);
            uxRightIndicator.MarqueeAnimationSpeed = 500;
            uxRightIndicator.Name = "uxRightIndicator";
            uxRightIndicator.Size = new Size(0x47, 0x17);
            uxRightIndicator.TabIndex = 0x100;
            uxLeftIndicator.Location = new Point(3, 5);
            uxLeftIndicator.MarqueeAnimationSpeed = 500;
            uxLeftIndicator.Name = "uxLeftIndicator";
            uxLeftIndicator.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            uxLeftIndicator.Size = new Size(0x47, 0x17);
            uxLeftIndicator.Style = ProgressBarStyle.Continuous;
            uxLeftIndicator.TabIndex = 0x100;
            panel1.BackColor = Color.Black;
            panel1.Controls.Add(uxLeft);
            panel1.Controls.Add(uxLeftIndicator);
            panel1.Controls.Add(uxRightIndicator);
            panel1.Controls.Add(uxRight);
            panel1.Location = new Point(0, 0x1a6);
            panel1.Name = "panel1";
            panel1.Size = new Size(0x206, 0x22);
            panel1.TabIndex = 0xff;
            uxLeft.BackColor = Color.Black;
            uxLeft.BackGround = Color.Transparent;
            uxLeft.BorderColor = Color.Black;
            uxLeft.BorderSpace = 3;
            uxLeft.CharSpacing = 2;
            uxLeft.DotMatrix = DotMatrix.mat7x9;
            uxLeft.LineSpacing = 2;
            uxLeft.Location = new Point(0x55, 3);
            uxLeft.Name = "uxLeft";
            uxLeft.NumberOfCharacters = 11;
            uxLeft.PixelHeight = 1;
            uxLeft.PixelOff = Color.Transparent;
            uxLeft.PixelOn = Color.Gold;
            uxLeft.PixelShape = PixelShape.Square;
            uxLeft.PixelSize = PixelSize.pix1x1;
            uxLeft.PixelSpacing = 1;
            uxLeft.PixelWidth = 1;
            uxLeft.Size = new Size(0xab, 0x19);
            uxLeft.TabIndex = 0xfe;
            uxLeft.Text = "Picture 0";
            uxLeft.TextLines = 1;
            uxRight.BackColor = Color.Black;
            uxRight.BackGround = Color.Transparent;
            uxRight.BorderColor = Color.Black;
            uxRight.BorderSpace = 3;
            uxRight.CharSpacing = 2;
            uxRight.DotMatrix = DotMatrix.mat7x9;
            uxRight.LineSpacing = 2;
            uxRight.Location = new Point(0x112, 3);
            uxRight.Name = "uxRight";
            uxRight.NumberOfCharacters = 10;
            uxRight.PixelHeight = 1;
            uxRight.PixelOff = Color.Transparent;
            uxRight.PixelOn = Color.Gold;
            uxRight.PixelShape = PixelShape.Square;
            uxRight.PixelSize = PixelSize.pix1x1;
            uxRight.PixelSpacing = 1;
            uxRight.PixelWidth = 1;
            uxRight.Size = new Size(0x9c, 0x19);
            uxRight.TabIndex = 0xfe;
            uxRight.Text = "Picture 1";
            uxRight.TextLines = 1;
            pictureBox1.Image = (Image) resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(0xe1, 7);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(0x9b, 0x9c);
            pictureBox1.TabIndex = 0xfd;
            pictureBox1.TabStop = false;
            lblTransPolZ.AutoSize = true;
            lblTransPolZ.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTransPolZ.ForeColor = Color.White;
            lblTransPolZ.Location = new Point(0x1b9, 0x13);
            lblTransPolZ.Name = "lblTransPolZ";
            lblTransPolZ.Size = new Size(0x61, 20);
            lblTransPolZ.TabIndex = 0xa9;
            lblTransPolZ.Text = "Direction Z";
            lblTransPolZ.Visible = false;
            picBox2.BackgroundImageLayout = ImageLayout.Stretch;
            picBox2.BorderStyle = BorderStyle.FixedSingle;
            picBox2.InitialImage = null;
            picBox2.Location = new Point(0x105, 0xc2);
            picBox2.Name = "picBox2";
            picBox2.Size = new Size(0xfd, 0xe3);
            picBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            picBox2.TabIndex = 0xfb;
            picBox2.TabStop = false;
            ledTransYDetect.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            ledTransYDetect.ForeColor = Color.White;
            ledTransYDetect.LedStyle = LedStyle.Round3D;
            ledTransYDetect.Location = new Point(0x254, 0x10);
            ledTransYDetect.Name = "ledTransYDetect";
            ledTransYDetect.OffColor = Color.Red;
            ledTransYDetect.Size = new Size(0x1d, 0x1f);
            ledTransYDetect.TabIndex = 0xa1;
            ledTransYDetect.Visible = false;
            lblTransPolX.AutoSize = true;
            lblTransPolX.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTransPolX.ForeColor = Color.White;
            lblTransPolX.Location = new Point(360, 0x13);
            lblTransPolX.Name = "lblTransPolX";
            lblTransPolX.Size = new Size(0x51, 20);
            lblTransPolX.TabIndex = 0xa7;
            lblTransPolX.Text = "Direction";
            lblTransPolX.Visible = false;
            label68.AutoSize = true;
            label68.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label68.ForeColor = Color.White;
            label68.Location = new Point(0x153, 0x10);
            label68.Name = "label68";
            label68.Size = new Size(0x15, 20);
            label68.TabIndex = 0x76;
            label68.Text = "Y";
            label68.Visible = false;
            pTrans2.BackColor = Color.LightSlateGray;
            pTrans2.Controls.Add(ledTransEA);
            pTrans2.Controls.Add(uxLabelEvent);
            pTrans2.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            pTrans2.ForeColor = Color.White;
            pTrans2.Location = new Point(0, 0x66);
            pTrans2.Name = "pTrans2";
            pTrans2.Size = new Size(0xd8, 0x38);
            pTrans2.TabIndex = 0xf9;
            ledTransEA.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            ledTransEA.ForeColor = Color.White;
            ledTransEA.LedStyle = LedStyle.Round3D;
            ledTransEA.Location = new Point(160, 3);
            ledTransEA.Name = "ledTransEA";
            ledTransEA.OffColor = Color.Red;
            ledTransEA.Size = new Size(0x30, 0x34);
            ledTransEA.TabIndex = 0xa5;
            ledTransEA.StateChanged += new ActionEventHandler(ledTransEA_StateChanged);
            uxLabelEvent.AutoSize = true;
            uxLabelEvent.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            uxLabelEvent.ForeColor = Color.White;
            uxLabelEvent.Location = new Point(2, 0x10);
            uxLabelEvent.Name = "uxLabelEvent";
            uxLabelEvent.Size = new Size(0x98, 0x18);
            uxLabelEvent.TabIndex = 0xa4;
            uxLabelEvent.Text = "Event Detected";
            ledTransZDetect.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            ledTransZDetect.ForeColor = Color.White;
            ledTransZDetect.LedStyle = LedStyle.Round3D;
            ledTransZDetect.Location = new Point(0x21a, 13);
            ledTransZDetect.Name = "ledTransZDetect";
            ledTransZDetect.OffColor = Color.Red;
            ledTransZDetect.Size = new Size(0x1d, 0x1f);
            ledTransZDetect.TabIndex = 0xa2;
            ledTransZDetect.Visible = false;
            label67.AutoSize = true;
            label67.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label67.ForeColor = Color.White;
            label67.Location = new Point(260, 9);
            label67.Name = "label67";
            label67.Size = new Size(20, 20);
            label67.TabIndex = 0x77;
            label67.Text = "Z";
            label67.Visible = false;
            ledTransXDetect.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            ledTransXDetect.ForeColor = Color.White;
            ledTransXDetect.LedStyle = LedStyle.Round3D;
            ledTransXDetect.Location = new Point(0x237, 7);
            ledTransXDetect.Name = "ledTransXDetect";
            ledTransXDetect.OffColor = Color.Red;
            ledTransXDetect.Size = new Size(0x1d, 0x1d);
            ledTransXDetect.TabIndex = 160;
            ledTransXDetect.Visible = false;
            label69.AutoSize = true;
            label69.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label69.ForeColor = Color.White;
            label69.Location = new Point(0x101, 0x27);
            label69.Name = "label69";
            label69.Size = new Size(0x15, 20);
            label69.TabIndex = 0x75;
            label69.Text = "X";
            label69.Visible = false;
            picBox1.BackgroundImageLayout = ImageLayout.Stretch;
            picBox1.BorderStyle = BorderStyle.FixedSingle;
            picBox1.Location = new Point(-1, 0xc2);
            picBox1.Name = "picBox1";
            picBox1.Size = new Size(0x100, 0xe3);
            picBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            picBox1.TabIndex = 250;
            picBox1.TabStop = false;
            panelAdvanced.Controls.Add(gbTS);
            panelAdvanced.Controls.Add(AccelControllerGroup);
            panelAdvanced.Location = new Point(0x214, 3);
            panelAdvanced.Name = "panelAdvanced";
            panelAdvanced.Size = new Size(0x233, 0x1ce);
            panelAdvanced.TabIndex = 0x4c;
            gbTS.BackColor = Color.LightSlateGray;
            gbTS.Controls.Add(btnResetTransient);
            gbTS.Controls.Add(btnSetTransient);
            gbTS.Controls.Add(rdoTransClearDebounce);
            gbTS.Controls.Add(p18);
            gbTS.Controls.Add(tbTransDebounce);
            gbTS.Controls.Add(lblTransDebounceVal);
            gbTS.Controls.Add(rdoTransDecDebounce);
            gbTS.Controls.Add(tbTransThreshold);
            gbTS.Controls.Add(lblTransDebounce);
            gbTS.Controls.Add(lblTransThreshold);
            gbTS.Controls.Add(lblTransThresholdVal);
            gbTS.Controls.Add(lblTransThresholdg);
            gbTS.Controls.Add(lblTransDebouncems);
            gbTS.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            gbTS.ForeColor = Color.White;
            gbTS.Location = new Point(8, 0xf1);
            gbTS.Name = "gbTS";
            gbTS.Size = new Size(550, 0xd8);
            gbTS.TabIndex = 0xd6;
            gbTS.TabStop = false;
            gbTS.Text = "Transient Settings";
            btnResetTransient.BackColor = Color.LightSlateGray;
            btnResetTransient.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnResetTransient.ForeColor = Color.White;
            btnResetTransient.Location = new Point(0x4f, 0xa7);
            btnResetTransient.Name = "btnResetTransient";
            btnResetTransient.Size = new Size(0x49, 0x1f);
            btnResetTransient.TabIndex = 0xca;
            btnResetTransient.Text = "Reset";
            btnResetTransient.UseVisualStyleBackColor = false;
            btnResetTransient.Click += new EventHandler(btnResetTransient_Click);
            btnSetTransient.BackColor = Color.LightSlateGray;
            btnSetTransient.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSetTransient.ForeColor = Color.White;
            btnSetTransient.Location = new Point(2, 0xa7);
            btnSetTransient.Name = "btnSetTransient";
            btnSetTransient.Size = new Size(0x49, 0x1f);
            btnSetTransient.TabIndex = 0xc9;
            btnSetTransient.Text = "Set";
            btnSetTransient.UseVisualStyleBackColor = false;
            btnSetTransient.Click += new EventHandler(btnSetTransient_Click);
            rdoTransClearDebounce.AutoSize = true;
            rdoTransClearDebounce.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            rdoTransClearDebounce.ForeColor = Color.White;
            rdoTransClearDebounce.Location = new Point(0x14c, 0xb1);
            rdoTransClearDebounce.Name = "rdoTransClearDebounce";
            rdoTransClearDebounce.Size = new Size(0x8a, 20);
            rdoTransClearDebounce.TabIndex = 0xb7;
            rdoTransClearDebounce.TabStop = true;
            rdoTransClearDebounce.Text = "Clear Debounce";
            rdoTransClearDebounce.UseVisualStyleBackColor = true;
            rdoTransClearDebounce.CheckedChanged += new EventHandler(rdoTransClearDebounce_CheckedChanged);
            p18.Controls.Add(chkDefaultTransSettings);
            p18.Controls.Add(chkTransBypassHPF);
            p18.Controls.Add(chkTransEnableLatch);
            p18.Controls.Add(chkTransEnableXFlag);
            p18.Controls.Add(chkTransEnableZFlag);
            p18.Controls.Add(chkTransEnableYFlag);
            p18.ForeColor = Color.White;
            p18.Location = new Point(10, 0x18);
            p18.Name = "p18";
            p18.Size = new Size(0x1f2, 0x3d);
            p18.TabIndex = 0xb8;
            chkDefaultTransSettings.AutoSize = true;
            chkDefaultTransSettings.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            chkDefaultTransSettings.ForeColor = Color.Gold;
            chkDefaultTransSettings.Location = new Point(3, 9);
            chkDefaultTransSettings.Name = "chkDefaultTransSettings";
            chkDefaultTransSettings.Size = new Size(0xd1, 20);
            chkDefaultTransSettings.TabIndex = 0xcb;
            chkDefaultTransSettings.Text = "Default Transient Settings ";
            chkDefaultTransSettings.UseVisualStyleBackColor = true;
            chkDefaultTransSettings.CheckedChanged += new EventHandler(chkDefaultTransSettings_CheckedChanged);
            chkTransBypassHPF.AutoSize = true;
            chkTransBypassHPF.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            chkTransBypassHPF.Location = new Point(0x114, 5);
            chkTransBypassHPF.Name = "chkTransBypassHPF";
            chkTransBypassHPF.Size = new Size(0x7e, 0x18);
            chkTransBypassHPF.TabIndex = 0xcc;
            chkTransBypassHPF.Text = "Bypass HPF";
            chkTransBypassHPF.UseVisualStyleBackColor = true;
            chkTransBypassHPF.CheckedChanged += new EventHandler(chkTransBypassHPF_CheckedChanged);
            chkTransEnableLatch.AutoSize = true;
            chkTransEnableLatch.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
            chkTransEnableLatch.Location = new Point(0x153, 0x21);
            chkTransEnableLatch.Name = "chkTransEnableLatch";
            chkTransEnableLatch.Size = new Size(0x62, 0x13);
            chkTransEnableLatch.TabIndex = 0x81;
            chkTransEnableLatch.Text = "Enable Latch";
            chkTransEnableLatch.UseVisualStyleBackColor = true;
            chkTransEnableLatch.CheckedChanged += new EventHandler(chkTransEnableLatch_CheckedChanged);
            chkTransEnableXFlag.AutoSize = true;
            chkTransEnableXFlag.Checked = true;
            chkTransEnableXFlag.CheckState = CheckState.Checked;
            chkTransEnableXFlag.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
            chkTransEnableXFlag.Location = new Point(4, 0x21);
            chkTransEnableXFlag.Name = "chkTransEnableXFlag";
            chkTransEnableXFlag.Size = new Size(0x67, 0x13);
            chkTransEnableXFlag.TabIndex = 120;
            chkTransEnableXFlag.Text = "Enable X Flag";
            chkTransEnableXFlag.UseVisualStyleBackColor = true;
            chkTransEnableXFlag.CheckedChanged += new EventHandler(chkTransEnableXFlag_CheckedChanged);
            chkTransEnableZFlag.AutoSize = true;
            chkTransEnableZFlag.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
            chkTransEnableZFlag.Location = new Point(0xe4, 0x21);
            chkTransEnableZFlag.Name = "chkTransEnableZFlag";
            chkTransEnableZFlag.Size = new Size(0x66, 0x13);
            chkTransEnableZFlag.TabIndex = 2;
            chkTransEnableZFlag.Text = "Enable Z Flag";
            chkTransEnableZFlag.UseVisualStyleBackColor = true;
            chkTransEnableZFlag.CheckedChanged += new EventHandler(chkTransEnableZFlag_CheckedChanged);
            chkTransEnableYFlag.AutoSize = true;
            chkTransEnableYFlag.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
            chkTransEnableYFlag.Location = new Point(0x76, 0x21);
            chkTransEnableYFlag.Name = "chkTransEnableYFlag";
            chkTransEnableYFlag.Size = new Size(0x66, 0x13);
            chkTransEnableYFlag.TabIndex = 1;
            chkTransEnableYFlag.Text = "Enable Y Flag";
            chkTransEnableYFlag.UseVisualStyleBackColor = true;
            chkTransEnableYFlag.CheckedChanged += new EventHandler(chkTransEnableYFlag_CheckedChanged);
            tbTransDebounce.BackColor = Color.Black;
            tbTransDebounce.Location = new Point(0xab, 0x7f);
            tbTransDebounce.Maximum = 0xff;
            tbTransDebounce.Name = "tbTransDebounce";
            tbTransDebounce.Size = new Size(0x12e, 0x2d);
            tbTransDebounce.TabIndex = 0x79;
            tbTransDebounce.Scroll += new EventHandler(tbTransDebounce_Scroll);
            lblTransDebounceVal.AutoSize = true;
            lblTransDebounceVal.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTransDebounceVal.ForeColor = Color.White;
            lblTransDebounceVal.Location = new Point(0x62, 0x8b);
            lblTransDebounceVal.Name = "lblTransDebounceVal";
            lblTransDebounceVal.Size = new Size(0x10, 0x10);
            lblTransDebounceVal.TabIndex = 0x7b;
            lblTransDebounceVal.Text = "0";
            rdoTransDecDebounce.AutoSize = true;
            rdoTransDecDebounce.Checked = true;
            rdoTransDecDebounce.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            rdoTransDecDebounce.ForeColor = Color.White;
            rdoTransDecDebounce.Location = new Point(0x9d, 0xb1);
            rdoTransDecDebounce.Name = "rdoTransDecDebounce";
            rdoTransDecDebounce.Size = new Size(0xb0, 20);
            rdoTransDecDebounce.TabIndex = 0xb6;
            rdoTransDecDebounce.TabStop = true;
            rdoTransDecDebounce.Text = "Decrement Debounce";
            rdoTransDecDebounce.UseVisualStyleBackColor = true;
            rdoTransDecDebounce.CheckedChanged += new EventHandler(rdoTransDecDebounce_CheckedChanged);
            tbTransThreshold.BackColor = Color.Black;
            tbTransThreshold.Location = new Point(0xab, 0x55);
            tbTransThreshold.Maximum = 0x7f;
            tbTransThreshold.Name = "tbTransThreshold";
            tbTransThreshold.Size = new Size(0x12e, 0x2d);
            tbTransThreshold.TabIndex = 0x7f;
            tbTransThreshold.Scroll += new EventHandler(tbTransThreshold_Scroll);
            lblTransDebounce.AutoSize = true;
            lblTransDebounce.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTransDebounce.ForeColor = Color.White;
            lblTransDebounce.Location = new Point(2, 0x8b);
            lblTransDebounce.Name = "lblTransDebounce";
            lblTransDebounce.Size = new Size(0x4f, 0x10);
            lblTransDebounce.TabIndex = 0x7a;
            lblTransDebounce.Text = "Debounce";
            lblTransThreshold.AutoSize = true;
            lblTransThreshold.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTransThreshold.ForeColor = Color.White;
            lblTransThreshold.Location = new Point(2, 0x5c);
            lblTransThreshold.Name = "lblTransThreshold";
            lblTransThreshold.Size = new Size(0x4e, 0x10);
            lblTransThreshold.TabIndex = 0x70;
            lblTransThreshold.Text = "Threshold";
            lblTransThresholdVal.AutoSize = true;
            lblTransThresholdVal.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTransThresholdVal.ForeColor = Color.White;
            lblTransThresholdVal.Location = new Point(0x71, 0x5c);
            lblTransThresholdVal.Name = "lblTransThresholdVal";
            lblTransThresholdVal.Size = new Size(0x10, 0x10);
            lblTransThresholdVal.TabIndex = 0x71;
            lblTransThresholdVal.Text = "0";
            lblTransThresholdg.AutoSize = true;
            lblTransThresholdg.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTransThresholdg.ForeColor = Color.White;
            lblTransThresholdg.Location = new Point(0x90, 0x5c);
            lblTransThresholdg.Name = "lblTransThresholdg";
            lblTransThresholdg.Size = new Size(0x11, 0x10);
            lblTransThresholdg.TabIndex = 120;
            lblTransThresholdg.Text = "g";
            lblTransDebouncems.AutoSize = true;
            lblTransDebouncems.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTransDebouncems.ForeColor = Color.White;
            lblTransDebouncems.Location = new Point(0x8a, 0x8b);
            lblTransDebouncems.Name = "lblTransDebouncems";
            lblTransDebouncems.Size = new Size(0x1c, 0x10);
            lblTransDebouncems.TabIndex = 0x7c;
            lblTransDebouncems.Text = "ms";
            AccelControllerGroup.Controls.Add(gbDR);
            AccelControllerGroup.Controls.Add(gbOM);
            AccelControllerGroup.Controls.Add(label173);
            AccelControllerGroup.Controls.Add(label71);
            AccelControllerGroup.Controls.Add(ledStandby);
            AccelControllerGroup.Controls.Add(ledWake);
            AccelControllerGroup.Controls.Add(ledSleep);
            AccelControllerGroup.Controls.Add(lbsleep);
            AccelControllerGroup.Location = new Point(5, 1);
            AccelControllerGroup.Name = "AccelControllerGroup";
            AccelControllerGroup.Size = new Size(550, 0xe9);
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
            gbDR.Location = new Point(8, 0x69);
            gbDR.Name = "gbDR";
            gbDR.Size = new Size(210, 0x76);
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
            gbOM.Controls.Add(pOverSampling);
            gbOM.Controls.Add(p1);
            gbOM.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            gbOM.ForeColor = Color.White;
            gbOM.Location = new Point(0xe8, 12);
            gbOM.Name = "gbOM";
            gbOM.Size = new Size(0x138, 0xda);
            gbOM.TabIndex = 0xc3;
            gbOM.TabStop = false;
            gbOM.Text = "Operation Mode";
            btnAutoCal.BackColor = Color.LightSlateGray;
            btnAutoCal.FlatAppearance.BorderColor = Color.Fuchsia;
            btnAutoCal.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnAutoCal.ForeColor = Color.White;
            btnAutoCal.Location = new Point(0xc3, 10);
            btnAutoCal.Name = "btnAutoCal";
            btnAutoCal.Size = new Size(0x60, 0x2c);
            btnAutoCal.TabIndex = 0xc5;
            btnAutoCal.Text = "Auto Calibrate";
            btnAutoCal.UseVisualStyleBackColor = false;
            btnAutoCal.Click += new EventHandler(btnAutoCal_Click);
            pOverSampling.Controls.Add(label70);
            pOverSampling.Controls.Add(chkAnalogLowNoise);
            pOverSampling.Controls.Add(rdoOSHiResMode);
            pOverSampling.Controls.Add(rdoOSLPMode);
            pOverSampling.Controls.Add(rdoOSLNLPMode);
            pOverSampling.Controls.Add(rdoOSNormalMode);
            pOverSampling.Location = new Point(1, 0x41);
            pOverSampling.Name = "pOverSampling";
            pOverSampling.Size = new Size(0x12e, 0x93);
            pOverSampling.TabIndex = 0xdd;
            label70.AutoSize = true;
            label70.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label70.Location = new Point(4, 0x19);
            label70.Name = "label70";
            label70.Size = new Size(220, 0x10);
            label70.TabIndex = 0xde;
            label70.Text = "Oversampling Options for Data";
            chkAnalogLowNoise.AutoSize = true;
            chkAnalogLowNoise.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            chkAnalogLowNoise.ForeColor = Color.White;
            chkAnalogLowNoise.Location = new Point(10, 5);
            chkAnalogLowNoise.Name = "chkAnalogLowNoise";
            chkAnalogLowNoise.Size = new Size(200, 0x11);
            chkAnalogLowNoise.TabIndex = 0xde;
            chkAnalogLowNoise.Text = "Enable Low Noise (Up to 5.5g)";
            chkAnalogLowNoise.UseVisualStyleBackColor = true;
            chkAnalogLowNoise.CheckedChanged += new EventHandler(chkAnalogLowNoise_CheckedChanged);
            rdoOSHiResMode.AutoSize = true;
            rdoOSHiResMode.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
            rdoOSHiResMode.Location = new Point(5, 0x48);
            rdoOSHiResMode.Name = "rdoOSHiResMode";
            rdoOSHiResMode.Size = new Size(0x6c, 0x13);
            rdoOSHiResMode.TabIndex = 0xe0;
            rdoOSHiResMode.Text = "Hi Res Mode";
            rdoOSHiResMode.UseVisualStyleBackColor = true;
            rdoOSHiResMode.CheckedChanged += new EventHandler(rdoOSHiResMode_CheckedChanged);
            rdoOSLPMode.AutoSize = true;
            rdoOSLPMode.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
            rdoOSLPMode.Location = new Point(5, 0x60);
            rdoOSLPMode.Name = "rdoOSLPMode";
            rdoOSLPMode.Size = new Size(0x87, 0x13);
            rdoOSLPMode.TabIndex = 0xdf;
            rdoOSLPMode.Text = "Low Power Mode";
            rdoOSLPMode.UseVisualStyleBackColor = true;
            rdoOSLPMode.CheckedChanged += new EventHandler(rdoOSLPMode_CheckedChanged);
            rdoOSLNLPMode.AutoSize = true;
            rdoOSLNLPMode.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
            rdoOSLNLPMode.Location = new Point(5, 120);
            rdoOSLNLPMode.Name = "rdoOSLNLPMode";
            rdoOSLNLPMode.Size = new Size(0xa6, 0x13);
            rdoOSLNLPMode.TabIndex = 0xdd;
            rdoOSLNLPMode.Text = "Low Noise Low Power";
            rdoOSLNLPMode.UseVisualStyleBackColor = true;
            rdoOSLNLPMode.CheckedChanged += new EventHandler(rdoOSLNLPMode_CheckedChanged);
            rdoOSNormalMode.AutoSize = true;
            rdoOSNormalMode.Checked = true;
            rdoOSNormalMode.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
            rdoOSNormalMode.Location = new Point(5, 0x30);
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
            label35.BackColor = Color.Transparent;
            label35.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label35.Location = new Point(4, 10);
            label35.Name = "label35";
            label35.Size = new Size(0x62, 0x10);
            label35.TabIndex = 0x7e;
            label35.Text = "Sample Rate";
            label173.AutoSize = true;
            label173.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label173.Location = new Point(9, 0x16);
            label173.Name = "label173";
            label173.Size = new Size(0x6c, 0x10);
            label173.TabIndex = 0xc2;
            label173.Text = "Standby Mode";
            label71.AutoSize = true;
            label71.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label71.Location = new Point(0x1a, 0x31);
            label71.Name = "label71";
            label71.Size = new Size(0x5b, 0x10);
            label71.TabIndex = 0xc0;
            label71.Text = "Wake Mode";
            ledStandby.LedStyle = LedStyle.Round3D;
            ledStandby.Location = new Point(0x77, 12);
            ledStandby.Name = "ledStandby";
            ledStandby.OffColor = Color.Red;
            ledStandby.Size = new Size(30, 0x1f);
            ledStandby.TabIndex = 0xc1;
            ledStandby.Value = true;
            ledWake.LedStyle = LedStyle.Round3D;
            ledWake.Location = new Point(0x77, 0x27);
            ledWake.Name = "ledWake";
            ledWake.OffColor = Color.Red;
            ledWake.Size = new Size(30, 0x1f);
            ledWake.TabIndex = 0xbf;
            ledSleep.LedStyle = LedStyle.Round3D;
            ledSleep.Location = new Point(0x77, 0x44);
            ledSleep.Name = "ledSleep";
            ledSleep.OffColor = Color.Red;
            ledSleep.Size = new Size(30, 0x1f);
            ledSleep.TabIndex = 0x91;
            lbsleep.AutoSize = true;
            lbsleep.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbsleep.Location = new Point(0x19, 0x4e);
            lbsleep.Name = "lbsleep";
            lbsleep.Size = new Size(0x5c, 0x10);
            lbsleep.TabIndex = 0xb9;
            lbsleep.Text = "Sleep Mode";
            rdoStandby.AutoSize = true;
            rdoStandby.Checked = true;
            rdoStandby.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            rdoStandby.ForeColor = Color.White;
            rdoStandby.Location = new Point(10, 0x13);
            rdoStandby.Name = "rdoStandby";
            rdoStandby.Size = new Size(0x57, 20);
            rdoStandby.TabIndex = 0x70;
            rdoStandby.TabStop = true;
            rdoStandby.Text = "Standby ";
            rdoStandby.UseVisualStyleBackColor = true;
            rdoStandby.CheckedChanged += new EventHandler(rdoStandby_CheckedChanged);
            rdoActive.AutoSize = true;
            rdoActive.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            rdoActive.ForeColor = Color.White;
            rdoActive.Location = new Point(0x67, 0x13);
            rdoActive.Name = "rdoActive";
            rdoActive.Size = new Size(0x45, 20);
            rdoActive.TabIndex = 0x71;
            rdoActive.Text = "Active";
            rdoActive.UseVisualStyleBackColor = true;
            rdoActive.CheckedChanged += new EventHandler(rdoActive_CheckedChanged);
            menuStrip1.BackColor = SystemColors.ButtonFace;
            menuStrip1.Enabled = false;
            menuStrip1.Font = new Font("Arial Rounded MT Bold", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
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
            enableLogDataApplicationToolStripMenuItem.Size = new Size(0xee, 0x16);
            enableLogDataApplicationToolStripMenuItem.Text = "Log Data XYZ with Tilt Detection";
            enableLogDataApplicationToolStripMenuItem.Click += new EventHandler(enableLogDataApplicationToolStripMenuItem_Click);
            pictureBox2.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            pictureBox2.BackgroundImageLayout = ImageLayout.None;
            pictureBox2.BorderStyle = BorderStyle.Fixed3D;
            pictureBox2.Image = Resources.STB_TOPBAR_LARGE;
            pictureBox2.Location = new Point(0, -1);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(0x44d, 0x39);
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.TabIndex = 0x4d;
            pictureBox2.TabStop = false;
            CommStrip.Items.AddRange(new ToolStripItem[] { CommStripButton, toolStripStatusLabel });
            CommStrip.Location = new Point(0, 530);
            CommStrip.Name = "CommStrip";
            CommStrip.Size = new Size(0x44d, 0x16);
            CommStrip.TabIndex = 0x4e;
            CommStrip.Text = "statusStrip1";
            CommStripButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
            CommStripButton.Image = Resources.imgYellowState;
            CommStripButton.ImageTransparentColor = Color.Magenta;
            CommStripButton.Name = "CommStripButton";
            CommStripButton.ShowDropDownArrow = false;
            CommStripButton.Size = new Size(20, 20);
            CommStripButton.Text = "toolStripDropDownButton1";
            toolStripStatusLabel.BackColor = SystemColors.ButtonFace;
            toolStripStatusLabel.Name = "toolStripStatusLabel";
            toolStripStatusLabel.Size = new Size(0xd1, 0x11);
            toolStripStatusLabel.Text = "COM Port Not Connected, Please Connect";
            uxControls.BackColor = Color.LightSlateGray;
            uxControls.Controls.Add(rdoActive);
            uxControls.Controls.Add(rdoStandby);
            uxControls.Location = new Point(6, 9);
            uxControls.Name = "uxControls";
            uxControls.Size = new Size(200, 50);
            uxControls.TabIndex = 0x101;
            uxControls.TabStop = false;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            AutoScroll = true;
            BackColor = Color.LightSlateGray;
            base.ClientSize = new Size(0x44d, 0x228);
            base.Controls.Add(CommStrip);
            base.Controls.Add(pictureBox2);
            base.Controls.Add(panelGeneral);
            base.Controls.Add(menuStrip1);
            base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            base.Icon = (Icon) resources.GetObject("$Icon");
            base.MaximizeBox = false;
            base.Name = "ShakeDemo";
            base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            Text = "Directional Shake/Flick Demo: Embedded Direction Algorithm";
            base.Resize += new EventHandler(ShakeDemo_Resize);
            panelGeneral.ResumeLayout(false);
            panelDisplay.ResumeLayout(false);
            panelDisplay.PerformLayout();
            panel1.ResumeLayout(false);
            ((ISupportInitialize) pictureBox1).EndInit();
            ((ISupportInitialize) picBox2).EndInit();
            ((ISupportInitialize) ledTransYDetect).EndInit();
            pTrans2.ResumeLayout(false);
            pTrans2.PerformLayout();
            ((ISupportInitialize) ledTransEA).EndInit();
            ((ISupportInitialize) ledTransZDetect).EndInit();
            ((ISupportInitialize) ledTransXDetect).EndInit();
            ((ISupportInitialize) picBox1).EndInit();
            panelAdvanced.ResumeLayout(false);
            gbTS.ResumeLayout(false);
            gbTS.PerformLayout();
            p18.ResumeLayout(false);
            p18.PerformLayout();
            tbTransDebounce.EndInit();
            tbTransThreshold.EndInit();
            AccelControllerGroup.ResumeLayout(false);
            AccelControllerGroup.PerformLayout();
            gbDR.ResumeLayout(false);
            gbDR.PerformLayout();
            gbOM.ResumeLayout(false);
            pOverSampling.ResumeLayout(false);
            pOverSampling.PerformLayout();
            p1.ResumeLayout(false);
            p1.PerformLayout();
            ((ISupportInitialize) ledStandby).EndInit();
            ((ISupportInitialize) ledWake).EndInit();
            ((ISupportInitialize) ledSleep).EndInit();
            ((ISupportInitialize) pictureBox2).EndInit();
            CommStrip.ResumeLayout(false);
            CommStrip.PerformLayout();
            uxControls.ResumeLayout(false);
            uxControls.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void InitializeStatusBar()
        {
            dv = new BoardComm();
            ControllerObj.GetCommObject(ref dv);
            LoadResource();
        }

        private void ledTransEA_StateChanged(object sender, ActionEventArgs e)
        {
            if (!ledTransEA.Value)
            {
                uxLeftIndicator.Invoke(
					(MethodInvoker)delegate()
					{
						uxLeftIndicator.Value = 0;
					});
                uxRightIndicator.Invoke(
					(MethodInvoker)delegate()
					{
						uxRightIndicator.Value = 0;
					});
            }
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
                Images[0] = (Image) manager.GetObject("Bluehills");
                Images[1] = (Image) manager.GetObject("DSC03168");
                Images[2] = (Image) manager.GetObject("Sunset");
                Images[3] = (Image) manager.GetObject("Waterlilies");
                Images[4] = (Image) manager.GetObject("flowers");
                Images[5] = (Image) manager.GetObject("giraffe");
                Images[6] = (Image) manager.GetObject("kowala");
                Images[7] = (Image) manager.GetObject("owl");
                Images[8] = (Image) manager.GetObject("Winter");
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
                chkDefaultTransSettings.Checked = false;
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
                chkDefaultTransSettings.Checked = false;
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
            int[] numArray;
            ControllerReqPacket packet;
            if (rdoActive.Checked)
            {
                ledWake.Value = true;
                ledStandby.Value = false;
                ledSleep.Value = false;
                gbDR.Enabled = false;
                gbOM.Enabled = false;
                p18.Enabled = false;
                btnAutoCal.Enabled = false;
                numArray = new int[] { 1 };
                ControllerObj.ActiveFlag = true;
                packet = new ControllerReqPacket();
                ControllerObj.CreateNewTaskFromGUI(packet, 0, 0x53, numArray);
            }
            else
            {
                ledWake.Value = false;
                ledStandby.Value = true;
                ledSleep.Value = false;
                gbDR.Enabled = true;
                gbOM.Enabled = true;
                p18.Enabled = true;
                btnAutoCal.Enabled = true;
                numArray = new int[] { 0 };
                ControllerObj.ActiveFlag = true;
                packet = new ControllerReqPacket();
                ControllerObj.CreateNewTaskFromGUI(packet, 0, 0x53, numArray);
            }
        }

        private void rdoOSHiResMode_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoOSHiResMode.Checked)
            {
                chkDefaultTransSettings.Checked = false;
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
                double num = tbTransDebounce.Value * DR_timestep;
                if (num > 1000.0)
                {
                    lblTransDebouncems.Text = "s";
                    num /= 1000.0;
                    lblTransDebounceVal.Text = string.Format("{0:F2}", num);
                }
                else
                {
                    lblTransDebouncems.Text = "ms";
                    lblTransDebounceVal.Text = string.Format("{0:F2}", num);
                }
            }
        }

        private void rdoOSLNLPMode_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoOSLNLPMode.Checked)
            {
                chkDefaultTransSettings.Checked = false;
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
                double num = tbTransDebounce.Value * DR_timestep;
                if (num > 1000.0)
                {
                    lblTransDebouncems.Text = "s";
                    num /= 1000.0;
                    lblTransDebounceVal.Text = string.Format("{0:F2}", num);
                }
                else
                {
                    lblTransDebouncems.Text = "ms";
                    lblTransDebounceVal.Text = string.Format("{0:F2}", num);
                }
            }
        }

        private void rdoOSLPMode_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoOSLPMode.Checked)
            {
                chkDefaultTransSettings.Checked = false;
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
                double num = tbTransDebounce.Value * DR_timestep;
                if (num > 1000.0)
                {
                    lblTransDebouncems.Text = "s";
                    num /= 1000.0;
                    lblTransDebounceVal.Text = string.Format("{0:F2}", num);
                }
                else
                {
                    lblTransDebouncems.Text = "ms";
                    lblTransDebounceVal.Text = string.Format("{0:F2}", num);
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
                double num = tbTransDebounce.Value * DR_timestep;
                if (num > 1000.0)
                {
                    lblTransDebouncems.Text = "s";
                    num /= 1000.0;
                    lblTransDebounceVal.Text = string.Format("{0:F2}", num);
                }
                else
                {
                    lblTransDebouncems.Text = "ms";
                    lblTransDebounceVal.Text = string.Format("{0:F2}", num);
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
                p18.Enabled = true;
                int[] datapassed = new int[] { 0 };
                ControllerObj.ActiveFlag = true;
                ControllerReqPacket reqName = new ControllerReqPacket();
                ControllerObj.CreateNewTaskFromGUI(reqName, 0, 0x53, datapassed);
            }
        }

        private void rdoTransClearDebounce_CheckedChanged(object sender, EventArgs e)
        {
            int num = rdoTransClearDebounce.Checked ? 0xff : 0;
            int[] datapassed = new int[] { num };
            ControllerObj.SetTransDBCNTMFlag = true;
            ControllerReqPacket reqName = new ControllerReqPacket();
            ControllerObj.CreateNewTaskFromGUI(reqName, 5, 0x26, datapassed);
        }

        private void rdoTransDecDebounce_CheckedChanged(object sender, EventArgs e)
        {
            int num = rdoTransDecDebounce.Checked ? 0 : 0xff;
            int[] datapassed = new int[] { num };
            ControllerObj.SetTransDBCNTMFlag = true;
            ControllerReqPacket reqName = new ControllerReqPacket();
            ControllerObj.CreateNewTaskFromGUI(reqName, 5, 0x26, datapassed);
        }

        private void SetTransInterrupts(bool enabled)
        {
            int[] datapassed = new int[] { enabled ? 0xff : 0, 0x20 };
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

        private void tbTransDebounce_Scroll(object sender, EventArgs e)
        {
            double num = tbTransDebounce.Value * DR_timestep;
            if (num > 1000.0)
            {
                lblTransDebouncems.Text = "s";
                num /= 1000.0;
                lblTransDebounceVal.Text = string.Format("{0:F2}", num);
            }
            else
            {
                lblTransDebouncems.Text = "ms";
                lblTransDebounceVal.Text = string.Format("{0:F2}", num);
            }
        }

        private void tbTransThreshold_Scroll(object sender, EventArgs e)
        {
            double num2 = 0.063;
            double num = tbTransThreshold.Value * num2;
            lblTransThresholdVal.Text = string.Format("{0:F3}", num);
        }

        private void tmrTiltTimer_Tick(object sender, EventArgs e)
        {
            if (ledTransEA.Value)
            {
                if (timercount == 3)
                {
                    ledTransEA.Value = false;
                    ledTransXDetect.Value = false;
                    ledTransYDetect.Value = false;
                    ledTransZDetect.Value = false;
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

        private void UpdateFormHeight()
        {
            if (!windowResized)
            {
                int windowHeight = (panelDisplay.Height + panelAdvanced.Height) + 200;
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
            uxLeft.Text = string.Format("Picture {0}", x_counter);
            uxRight.Text = string.Format("Picture {0}", x_counter + 1);
            DecodeGUIPackets();
            UpdateCommStrip();
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

