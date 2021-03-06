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

    public class FIFOLowPowerDemo : Form
    {
        private GroupBox AccelControllerGroup;
        private bool B_btnAdvanced = false;
        private bool B_btnDisplay = true;
        private bool B_btnRegisters = false;
        private int Bit8_2Complement;
        private int Bit8_MaxPositive;
        private bool blDirShakeChange = false;
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
        private bool dataStream = false;
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
        private GroupBox groupBox1;
        private int I_panelAdvanced_height;
        private int I_panelDisplay_height;
        private int I_panelRegisters_height;
        private Image ImageGreen;
        private Image ImageRed;
        private Image ImageYellow;
        private int iTransStat = 0;
        private Label label1;
        private Label label173;
        private Label label2;
        private Label label3;
        private Label label35;
        private Label label4;
        private Label label5;
        private Label label62;
        private Label label65;
        private Label label67;
        private Label label68;
        private Label label69;
        private Label label70;
        private Label label71;
        private Label lbl2gSensitivity;
        private Label lbl4gSensitivity;
        private Label lbl8gSensitivity;
        private Label lblCurrentMCU;
        private Label lblTransDebounce;
        private Label lblTransDebouncems;
        private Label lblTransDebounceVal;
        private Label lblTransPolX;
        private Label lblTransPolY;
        private Label lblTransPolZ;
        private Label lblTransThreshold;
        private Label lblTransThresholdg;
        private Label lblTransThresholdVal;
        private Label lbsleep;
        private Led ledMCU;
        private Led ledSleep;
        private Led ledStandby;
        private Led ledTransEA;
        private Led ledTransXDetect;
        private Led ledTransYDetect;
        private Led ledTransZDetect;
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
        private Panel p18;
        private Panel panelAdvanced;
        private Panel panelDisplay;
        private FlowLayoutPanel panelGeneral;
        private PictureBox pictureBox1;
        private WaveformGraph pltFIFOPlot;
        private Panel pOverSampling;
        private Panel pTrans2;
        private Queue qFIFOArray = new Queue();
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
        private bool ReadValueFlag_task = true;
        private string[] RegisterNames = new string[] { 
            "STATUS", "OUT_XMSB", "OUT_XLSB", "OUT_YMSB", "OUT_YLSP", "OUT_ZMSB", "OUT_ZLSB", "RESERVED ", "RESERVED ", "FSETUP", "TRIGCFG", "SYSMOD", "INTSOURCE", "WHOAMI", "XYZDATACFG", "HPFILTER", 
            "PLSTATUS", "PLCFG", "PLCOUNT", "PLBFZCOMP", "PLPLTHSREG", "FFMTCFG", "FFMTSRC", "FFMTTHS", "FFMTCOUNT", "RESERVED", "RESERVED", "RESERVED", "RESERVED", "TRANSCFG", "TRANSSRC", "TRANSTHS", 
            "TRANSCOUNT", "PULSECFG", "PULSESRC", "PULSETHSX", "PULSETHSY", "PULSETHSZ", "PULSETMLT", "PULSELTCY", "PULSEWIND", "ASLPCOUNT", "CTRLREG1", "CTRLREG2", "CTRLREG3", "CTRLREG4", "CTRLREG5", "OFFSET_X", 
            "OFFSET_Y", "OFFSET_Z"
         };
        private SaveFileDialog saveFileDialog1;
        private string sDirShake;
        private TrackBar tbTransDebounce;
        private TrackBar tbTransThreshold;
        private TextBox textBox2;
        private int timercount;
        private System.Windows.Forms.Timer tmrShakeTimer;
        private ToolStripStatusLabel toolStripStatusLabel;
        private TextBox txtCurrent;
        private int WakeOSMode;
        private int windowHeight = 0;
        private bool windowResized = false;
        private XAxis xAxis3;
        private YAxis yAxis3;

        public FIFOLowPowerDemo(object controllerObj)
        {
            InitializeComponent();
            Styles.FormatForm(this);
            Styles.FormatInterruptPanel(panelAdvanced);
            tbTransDebounce.BackColor = Color.Black;
            tbTransThreshold.BackColor = Color.Black;
            ControllerObj = (AccelController) controllerObj;
            ControllerObj.ControllerEvents += new AccelController.ControllerEventHandler(ControllerObj_ControllerEvents);
            menuStrip1.Enabled = false;
            Bit8_2Complement = 0x100;
            Bit8_MaxPositive = 0x7f;
            timercount = 0;
            B_btnDisplay = false;
            B_btnAdvanced = true;
            B_btnRegisters = true;
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
            tmrShakeTimer.Enabled = true;
            tmrShakeTimer.Start();
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

        private void btnRegisters_Click(object sender, EventArgs e)
        {
            B_btnRegisters = !B_btnRegisters;
            windowResized = false;
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
                chkTransEnableXFlag.Checked = true;
                int[] datapassed = new int[] { 0xff };
                ControllerObj.SetTransEnableXFlagFlag = true;
                ControllerReqPacket reqName = new ControllerReqPacket();
                ControllerObj.CreateNewTaskFromGUI(reqName, 5, 0x27, datapassed);
                chkTransEnableYFlag.Checked = true;
                int[] numArray2 = new int[] { 0xff };
                ControllerObj.SetTransEnableYFlagFlag = true;
                ControllerReqPacket packet2 = new ControllerReqPacket();
                ControllerObj.CreateNewTaskFromGUI(packet2, 5, 40, numArray2);
                chkTransEnableZFlag.Checked = true;
                int[] numArray3 = new int[] { 0xff };
                ControllerObj.SetTransEnableZFlagFlag = true;
                ControllerReqPacket packet3 = new ControllerReqPacket();
                ControllerObj.CreateNewTaskFromGUI(packet3, 5, 0x29, numArray3);
                chkTransEnableLatch.Checked = true;
                tbTransThreshold.Value = 11;
                double num2 = 0.063;
                double num = tbTransThreshold.Value * num2;
                lblTransThresholdVal.Text = string.Format("{0:F2}", num);
                int[] numArray4 = new int[] { tbTransThreshold.Value };
                ControllerObj.SetTransThresholdFlag = true;
                ControllerReqPacket packet4 = new ControllerReqPacket();
                ControllerObj.CreateNewTaskFromGUI(packet4, 5, 0x24, numArray4);
                tbTransDebounce.Value = 10;
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
            int[] datapassed = new int[] { 3 };
            ControllerObj.SetFIFOModeFlag = true;
            ControllerReqPacket reqName = new ControllerReqPacket();
            ControllerObj.CreateNewTaskFromGUI(reqName, 7, 0x13, datapassed);
            datapassed = new int[] { 20 };
            ControllerObj.SetFIFOWatermarkFlag = true;
            ControllerReqPacket packet2 = new ControllerReqPacket();
            ControllerObj.CreateNewTaskFromGUI(packet2, 7, 20, datapassed);
            datapassed = new int[] { 0xff };
            ControllerObj.SetTrigTransFlag = true;
            ControllerReqPacket packet3 = new ControllerReqPacket();
            ControllerObj.CreateNewTaskFromGUI(packet3, 7, 0x15, datapassed);
        }

        private void ConfigureInterrupts(bool enabled)
        {
            int[] datapassed = new int[] { enabled ? 0xff : 0, 0x60 };
            ControllerObj.SetIntsEnableFlag = true;
            ControllerReqPacket reqName = new ControllerReqPacket();
            ControllerObj.CreateNewTaskFromGUI(reqName, 0, 13, datapassed);
            datapassed = new int[] { enabled ? 0xff : 0, 0x40 };
            ControllerObj.SetIntsConfigFlag = true;
            reqName = new ControllerReqPacket();
            ControllerObj.CreateNewTaskFromGUI(reqName, 0, 14, datapassed);
            ControllerObj.AppRegisterWrite(new int[] { 0x15, 1 });
        }

        private void ControllerObj_ControllerEvents(ControllerEventType evt, object o)
        {
            XYZGees gees = new XYZGees();
            switch (((int) evt))
            {
                case 3:
                {
                    int num8;
                    sbyte xAxis;
                    byte num11;
                    XYZCounts[] countsArray = (XYZCounts[]) o;
                    StreamWriter writer = File.CreateText("TransLog.csv");
                    lock (qFIFOArray)
                    {
                        qFIFOArray.Enqueue(countsArray);
                    }
                    sbyte[] numArray2 = new sbyte[3];
                    sbyte[] numArray3 = new sbyte[3];
                    byte[] buffer = new byte[3];
                    byte[] buffer2 = new byte[3];
                    byte[] buffer3 = new byte[3];
                    byte index = 0;
                    int num7 = 0;
                    numArray2[2] = (sbyte) (xAxis = -128);
                    numArray2[0] = numArray2[1] = xAxis;
                    numArray3[2] = (sbyte) (xAxis = 0x7f);
                    numArray3[0] = numArray3[1] = xAxis;
                    buffer[2] = (byte) (num11 = 0);
                    buffer[0] = buffer[1] = num11;
                    buffer2[2] = (byte) (num11 = 0);
                    buffer2[0] = buffer2[1] = num11;
                    string str = "Trans Status Register," + string.Format("{0:X2}", iTransStat);
                    writer.WriteLine(str);
                    str = "X,Y,Z";
                    writer.WriteLine(str);
                    for (num8 = 0; num8 < countsArray.Length; num8++)
                    {
                        if (countsArray[num8] != null)
                        {
                            string[] strArray = new string[5];
                            xAxis = (sbyte) countsArray[num8].XAxis;
                            strArray[0] = xAxis.ToString();
                            strArray[1] = ",";
                            xAxis = (sbyte) countsArray[num8].YAxis;
                            strArray[2] = xAxis.ToString();
                            strArray[3] = ",";
                            strArray[4] = ((sbyte) countsArray[num8].ZAxis).ToString();
                            str = string.Concat(strArray);
                            writer.WriteLine(str);
                            if ((iTransStat & 2) != 0)
                            {
                                if (((sbyte) countsArray[num8].XAxis) > numArray2[0])
                                {
                                    numArray2[0] = (sbyte) countsArray[num8].XAxis;
                                    buffer[0] = (byte) num8;
                                }
                                if (((sbyte) countsArray[num8].XAxis) < numArray3[0])
                                {
                                    numArray3[0] = (sbyte) countsArray[num8].XAxis;
                                    buffer2[0] = (byte) num8;
                                }
                            }
                            if ((iTransStat & 8) != 0)
                            {
                                if (((sbyte) countsArray[num8].YAxis) > numArray2[1])
                                {
                                    numArray2[1] = (sbyte) countsArray[num8].YAxis;
                                    buffer[1] = (byte) num8;
                                }
                                if (((sbyte) countsArray[num8].YAxis) < numArray3[1])
                                {
                                    numArray3[1] = (sbyte) countsArray[num8].YAxis;
                                    buffer2[1] = (byte) num8;
                                }
                            }
                            if ((iTransStat & 0x20) != 0)
                            {
                                if (((sbyte) countsArray[num8].ZAxis) > numArray2[2])
                                {
                                    numArray2[2] = (sbyte) countsArray[num8].ZAxis;
                                    buffer[2] = (byte) num8;
                                }
                                if (((sbyte) countsArray[num8].ZAxis) < numArray3[2])
                                {
                                    numArray3[2] = (sbyte) countsArray[num8].ZAxis;
                                    buffer2[2] = (byte) num8;
                                }
                            }
                        }
                    }
                    for (num8 = 0; num8 < 3; num8++)
                    {
                        if (buffer[num8] < buffer2[num8])
                        {
                            buffer3[num8] = 1;
                        }
                        else if (buffer[num8] > buffer2[num8])
                        {
                            buffer3[num8] = 2;
                        }
                        if ((buffer3[num8] != 0) && ((numArray2[num8] - numArray3[num8]) > num7))
                        {
                            index = (byte) num8;
                            num7 = numArray2[num8] - numArray3[num8];
                        }
                    }
                    writer.Close();
                    blDirShakeChange = true;
                    ledMCU.Value = true;
                    if (buffer3[index] == 1)
                    {
                        sDirShake = Convert.ToChar((int) (Convert.ToByte('X') + index)) + "\r\n Positive";
                    }
                    else
                    {
                        sDirShake = Convert.ToChar((int) (Convert.ToByte('X') + index)) + "\r\n Negative";
                    }
                    break;
                }
                case 9:
                {
                    GUIUpdatePacket packet = (GUIUpdatePacket) o;
                    if (packet.PayLoad.Count > 0)
                    {
                        int[] numArray = (int[]) packet.PayLoad.Dequeue();
                        int num4 = numArray[0];
                        num4 &= 0x7f;
                        iTransStat = num4;
                    }
                    break;
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
                tmrShakeTimer.Stop();
            }
        }

        public void EndDemo()
        {
            lock (ControllerEventsLock)
            {
                tmrShakeTimer.Stop();
                tmrShakeTimer.Enabled = false;
                ControllerObj.DisableFIFO8bData();
                ControllerObj.DataFIFO8StreamEnable = false;
                ControllerObj.ResetDevice();
                ControllerObj.ControllerEvents -= new AccelController.ControllerEventHandler(ControllerObj_ControllerEvents);
                base.Close();
            }
        }

        private void FIFOLowPowerDemo_Load(object sender, EventArgs e)
        {
        }

        private void InitDevice()
        {
            ControllerObj.StartDevice();
            Thread.Sleep(10);
            ControllerObj.Boot();
            Thread.Sleep(20);
            DR_timestep = 5.0;
            ddlDataRate.SelectedIndex = 3;
            ControllerObj.SetHPDataOut(true);
            rdoOSLNLPMode.Checked = true;
            rdoOSLNLPMode_CheckedChanged(this, null);
            chkDefaultTransSettings.Checked = true;
            chkDefaultTransSettings_CheckedChanged(this, null);
            rdo8g.Checked = true;
            rdo8g_CheckedChanged(this, null);
            UpdateFormState();
            ConfigureInterrupts(true);
            ControllerObj.EnableFIFO8StreamData(0);
            ConfigureFIFO();
            rdoActive.Checked = true;
            rdoActive_CheckedChanged(this, null);
            ControllerObj.ReturnTransStatus = true;
        }

        private void InitializeComponent()
        {
            components = new Container();
            tmrShakeTimer = new System.Windows.Forms.Timer(components);
            panelGeneral = new FlowLayoutPanel();
            panelDisplay = new Panel();
            label1 = new Label();
            pTrans2 = new Panel();
            lblTransPolZ = new Label();
            lblTransPolY = new Label();
            lblTransPolX = new Label();
            ledTransEA = new Led();
            label62 = new Label();
            label65 = new Label();
            ledTransZDetect = new Led();
            ledTransYDetect = new Led();
            ledTransXDetect = new Led();
            label67 = new Label();
            label68 = new Label();
            label69 = new Label();
            label5 = new Label();
            label3 = new Label();
            label2 = new Label();
            txtCurrent = new TextBox();
            lblCurrentMCU = new Label();
            ledMCU = new Led();
            textBox2 = new TextBox();
            label4 = new Label();
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
            groupBox1 = new GroupBox();
            rdoActive = new RadioButton();
            rdoStandby = new RadioButton();
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
            ledWake = new Led();
            ledSleep = new Led();
            ledStandby = new Led();
            lbsleep = new Label();
            label173 = new Label();
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
            pTrans2.SuspendLayout();
            ((ISupportInitialize) ledTransEA).BeginInit();
            ((ISupportInitialize) ledTransZDetect).BeginInit();
            ((ISupportInitialize) ledTransYDetect).BeginInit();
            ((ISupportInitialize) ledTransXDetect).BeginInit();
            ((ISupportInitialize) ledMCU).BeginInit();
            ((ISupportInitialize) legend1).BeginInit();
            ((ISupportInitialize) pltFIFOPlot).BeginInit();
            panelAdvanced.SuspendLayout();
            gbTS.SuspendLayout();
            p18.SuspendLayout();
            tbTransDebounce.BeginInit();
            tbTransThreshold.BeginInit();
            AccelControllerGroup.SuspendLayout();
            groupBox1.SuspendLayout();
            gbDR.SuspendLayout();
            gbOM.SuspendLayout();
            pOverSampling.SuspendLayout();
            p1.SuspendLayout();
            ((ISupportInitialize) ledWake).BeginInit();
            ((ISupportInitialize) ledSleep).BeginInit();
            ((ISupportInitialize) ledStandby).BeginInit();
            menuStrip1.SuspendLayout();
            CommStrip.SuspendLayout();
            ((ISupportInitialize) pictureBox1).BeginInit();
            base.SuspendLayout();
            tmrShakeTimer.Interval = 40;
            tmrShakeTimer.Tick += new EventHandler(tmrTiltTimer_Tick);
            panelGeneral.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            panelGeneral.AutoScroll = true;
            panelGeneral.Controls.Add(panelDisplay);
            panelGeneral.Controls.Add(panelAdvanced);
            panelGeneral.Location = new Point(1, 0x38);
            panelGeneral.Name = "panelGeneral";
            panelGeneral.Size = new Size(0x3bc, 0x27f);
            panelGeneral.TabIndex = 0x47;
            panelDisplay.BorderStyle = BorderStyle.FixedSingle;
            panelDisplay.Controls.Add(label1);
            panelDisplay.Controls.Add(pTrans2);
            panelDisplay.Controls.Add(label5);
            panelDisplay.Controls.Add(label3);
            panelDisplay.Controls.Add(label2);
            panelDisplay.Controls.Add(txtCurrent);
            panelDisplay.Controls.Add(lblCurrentMCU);
            panelDisplay.Controls.Add(ledMCU);
            panelDisplay.Controls.Add(textBox2);
            panelDisplay.Controls.Add(label4);
            panelDisplay.Controls.Add(legend1);
            panelDisplay.Controls.Add(pltFIFOPlot);
            panelDisplay.Location = new Point(3, 3);
            panelDisplay.Name = "panelDisplay";
            panelDisplay.Size = new Size(0x3b2, 300);
            panelDisplay.TabIndex = 0x49;
            label1.AutoSize = true;
            label1.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.White;
            label1.Location = new Point(0x22, 0xd1);
            label1.Name = "label1";
            label1.Size = new Size(0xb3, 0x18);
            label1.TabIndex = 0xf9;
            label1.Text = "Software Direction";
            pTrans2.BackColor = Color.LightSlateGray;
            pTrans2.Controls.Add(lblTransPolZ);
            pTrans2.Controls.Add(lblTransPolY);
            pTrans2.Controls.Add(lblTransPolX);
            pTrans2.Controls.Add(ledTransEA);
            pTrans2.Controls.Add(label62);
            pTrans2.Controls.Add(label65);
            pTrans2.Controls.Add(ledTransZDetect);
            pTrans2.Controls.Add(ledTransYDetect);
            pTrans2.Controls.Add(ledTransXDetect);
            pTrans2.Controls.Add(label67);
            pTrans2.Controls.Add(label68);
            pTrans2.Controls.Add(label69);
            pTrans2.Enabled = false;
            pTrans2.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            pTrans2.ForeColor = Color.White;
            pTrans2.Location = new Point(0x11, 0x12);
            pTrans2.Name = "pTrans2";
            pTrans2.Size = new Size(0xd9, 0xbd);
            pTrans2.TabIndex = 0xf8;
            lblTransPolZ.AutoSize = true;
            lblTransPolZ.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTransPolZ.ForeColor = Color.White;
            lblTransPolZ.Location = new Point(0x21, 0x65);
            lblTransPolZ.Name = "lblTransPolZ";
            lblTransPolZ.Size = new Size(0x61, 20);
            lblTransPolZ.TabIndex = 0xa9;
            lblTransPolZ.Text = "Direction Z";
            lblTransPolY.AutoSize = true;
            lblTransPolY.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTransPolY.ForeColor = Color.White;
            lblTransPolY.Location = new Point(0x21, 0x47);
            lblTransPolY.Name = "lblTransPolY";
            lblTransPolY.Size = new Size(0x62, 20);
            lblTransPolY.TabIndex = 0xa8;
            lblTransPolY.Text = "Direction Y";
            lblTransPolX.AutoSize = true;
            lblTransPolX.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTransPolX.ForeColor = Color.White;
            lblTransPolX.Location = new Point(0x21, 0x2b);
            lblTransPolX.Name = "lblTransPolX";
            lblTransPolX.Size = new Size(0x62, 20);
            lblTransPolX.TabIndex = 0xa7;
            lblTransPolX.Text = "Direction X";
            ledTransEA.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            ledTransEA.ForeColor = Color.White;
            ledTransEA.LedStyle = LedStyle.Round3D;
            ledTransEA.Location = new Point(0xa6, 0x81);
            ledTransEA.Name = "ledTransEA";
            ledTransEA.OffColor = Color.Red;
            ledTransEA.Size = new Size(0x30, 0x34);
            ledTransEA.TabIndex = 0xa5;
            label62.AutoSize = true;
            label62.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label62.ForeColor = Color.White;
            label62.Location = new Point(8, 0x90);
            label62.Name = "label62";
            label62.Size = new Size(0x98, 0x18);
            label62.TabIndex = 0xa4;
            label62.Text = "Event Detected";
            label65.AutoSize = true;
            label65.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label65.ForeColor = Color.White;
            label65.Location = new Point(3, 3);
            label65.Name = "label65";
            label65.Size = new Size(0xa5, 0x18);
            label65.TabIndex = 0xa3;
            label65.Text = "Transient Status ";
            ledTransZDetect.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            ledTransZDetect.ForeColor = Color.White;
            ledTransZDetect.LedStyle = LedStyle.Round3D;
            ledTransZDetect.Location = new Point(0x85, 0x60);
            ledTransZDetect.Name = "ledTransZDetect";
            ledTransZDetect.OffColor = Color.Red;
            ledTransZDetect.Size = new Size(0x1d, 0x1f);
            ledTransZDetect.TabIndex = 0xa2;
            ledTransYDetect.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            ledTransYDetect.ForeColor = Color.White;
            ledTransYDetect.LedStyle = LedStyle.Round3D;
            ledTransYDetect.Location = new Point(0x85, 0x44);
            ledTransYDetect.Name = "ledTransYDetect";
            ledTransYDetect.OffColor = Color.Red;
            ledTransYDetect.Size = new Size(0x1d, 0x1f);
            ledTransYDetect.TabIndex = 0xa1;
            ledTransXDetect.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            ledTransXDetect.ForeColor = Color.White;
            ledTransXDetect.LedStyle = LedStyle.Round3D;
            ledTransXDetect.Location = new Point(0x85, 0x27);
            ledTransXDetect.Name = "ledTransXDetect";
            ledTransXDetect.OffColor = Color.Red;
            ledTransXDetect.Size = new Size(0x1d, 0x1d);
            ledTransXDetect.TabIndex = 160;
            label67.AutoSize = true;
            label67.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label67.ForeColor = Color.White;
            label67.Location = new Point(10, 0x65);
            label67.Name = "label67";
            label67.Size = new Size(20, 20);
            label67.TabIndex = 0x77;
            label67.Text = "Z";
            label68.AutoSize = true;
            label68.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label68.ForeColor = Color.White;
            label68.Location = new Point(8, 0x47);
            label68.Name = "label68";
            label68.Size = new Size(0x15, 20);
            label68.TabIndex = 0x76;
            label68.Text = "Y";
            label69.AutoSize = true;
            label69.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label69.ForeColor = Color.White;
            label69.Location = new Point(8, 0x2b);
            label69.Name = "label69";
            label69.Size = new Size(0x15, 20);
            label69.TabIndex = 0x75;
            label69.Text = "X";
            label5.AutoSize = true;
            label5.Location = new Point(0x2ff, 0xf2);
            label5.Name = "label5";
            label5.Size = new Size(140, 13);
            label5.TabIndex = 0xf7;
            label5.Text = "MCU Sleep Current = 0.5mA";
            label3.AutoSize = true;
            label3.Location = new Point(0x2ff, 0xdf);
            label3.Name = "label3";
            label3.Size = new Size(0x8b, 13);
            label3.TabIndex = 0xf6;
            label3.Text = "MCU Wake Current = 12mA";
            label2.AutoSize = true;
            label2.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(0x30a, 0x13);
            label2.Name = "label2";
            label2.Size = new Size(0x6a, 20);
            label2.TabIndex = 0xf5;
            label2.Text = "MCU Status";
            txtCurrent.BackColor = Color.LightSteelBlue;
            txtCurrent.Font = new Font("Verdana", 20f, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtCurrent.ForeColor = Color.ForestGreen;
            txtCurrent.Location = new Point(0x2df, 0xa7);
            txtCurrent.Multiline = true;
            txtCurrent.Name = "txtCurrent";
            txtCurrent.ReadOnly = true;
            txtCurrent.RightToLeft = System.Windows.Forms.RightToLeft.No;
            txtCurrent.Size = new Size(0xcf, 40);
            txtCurrent.TabIndex = 0xf4;
            txtCurrent.Text = "0.524 mA";
            txtCurrent.TextAlign = HorizontalAlignment.Center;
            lblCurrentMCU.AutoSize = true;
            lblCurrentMCU.Font = new Font("Microsoft Sans Serif", 11.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblCurrentMCU.Location = new Point(0x2e1, 0x94);
            lblCurrentMCU.Name = "lblCurrentMCU";
            lblCurrentMCU.Size = new Size(0xcd, 0x12);
            lblCurrentMCU.TabIndex = 0xf3;
            lblCurrentMCU.Text = "Estimated System Current";
            ledMCU.ForeColor = Color.White;
            ledMCU.LedStyle = LedStyle.Round3D;
            ledMCU.Location = new Point(0x31f, 0x2a);
            ledMCU.Name = "ledMCU";
            ledMCU.OffColor = Color.Red;
            ledMCU.Size = new Size(80, 0x51);
            ledMCU.TabIndex = 0xf2;
            textBox2.BackColor = Color.LightSteelBlue;
            textBox2.Font = new Font("Verdana", 24f, FontStyle.Regular, GraphicsUnit.Point, 0);
            textBox2.ForeColor = Color.ForestGreen;
            textBox2.Location = new Point(0x18, 0xea);
            textBox2.Multiline = true;
            textBox2.Name = "textBox2";
            textBox2.ReadOnly = true;
            textBox2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            textBox2.Size = new Size(190, 0x38);
            textBox2.TabIndex = 0xa7;
            textBox2.TextAlign = HorizontalAlignment.Center;
            label4.AutoSize = true;
            label4.Location = new Point(80, 2);
            label4.Name = "label4";
            label4.Size = new Size(0x31, 13);
            label4.TabIndex = 0xa6;
            label4.Text = "Direction";
            legend1.ItemLayoutMode = LegendItemLayoutMode.LeftToRight;
            legend1.Items.AddRange(new LegendItem[] { legendItem1, legendItem2, legendItem3 });
            legend1.Location = new Point(0x180, 0x105);
            legend1.Name = "legend1";
            legend1.Size = new Size(0x10f, 0x25);
            legend1.TabIndex = 0xf1;
            legendItem1.Source = MainScreenXAxis;
            legendItem1.Text = "X-Axis";
            MainScreenXAxis.LineColor = Color.Red;
            MainScreenXAxis.XAxis = xAxis3;
            MainScreenXAxis.YAxis = yAxis3;
            xAxis3.Caption = "Samples";
            xAxis3.MajorDivisions.GridVisible = true;
            xAxis3.Range = new NationalInstruments.UI.Range(0.0, 32.0);
            yAxis3.Caption = "Acceleration (g)";
            yAxis3.MajorDivisions.GridColor = Color.Silver;
            yAxis3.MajorDivisions.GridVisible = true;
            legendItem2.Source = MainScreenYAxis;
            legendItem2.Text = "Y-Axis";
            MainScreenYAxis.LineColor = Color.Blue;
            MainScreenYAxis.PointColor = Color.LimeGreen;
            MainScreenYAxis.XAxis = xAxis3;
            MainScreenYAxis.YAxis = yAxis3;
            legendItem3.Source = MainScreenZAxis;
            legendItem3.Text = "Z-Axis";
            MainScreenZAxis.LineColor = Color.GhostWhite;
            MainScreenZAxis.XAxis = xAxis3;
            MainScreenZAxis.YAxis = yAxis3;
            pltFIFOPlot.BackColor = Color.Gray;
            pltFIFOPlot.Caption = "FIFO Data";
            pltFIFOPlot.CaptionBackColor = Color.Silver;
            pltFIFOPlot.ForeColor = Color.White;
            pltFIFOPlot.ImmediateUpdates = true;
            pltFIFOPlot.InteractionModeDefault = GraphDefaultInteractionMode.ZoomX;
            pltFIFOPlot.Location = new Point(0x109, 0);
            pltFIFOPlot.Name = "pltFIFOPlot";
            pltFIFOPlot.PlotAreaBorder = Border.Raised;
            pltFIFOPlot.Plots.AddRange(new WaveformPlot[] { MainScreenXAxis, MainScreenYAxis, MainScreenZAxis });
            pltFIFOPlot.Size = new Size(0x1d1, 0xff);
            pltFIFOPlot.TabIndex = 240;
            pltFIFOPlot.XAxes.AddRange(new XAxis[] { xAxis3 });
            pltFIFOPlot.YAxes.AddRange(new YAxis[] { yAxis3 });
            panelAdvanced.BorderStyle = BorderStyle.FixedSingle;
            panelAdvanced.Controls.Add(gbTS);
            panelAdvanced.Controls.Add(AccelControllerGroup);
            panelAdvanced.Location = new Point(3, 0x135);
            panelAdvanced.Name = "panelAdvanced";
            panelAdvanced.Size = new Size(0x3b2, 0x142);
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
            gbTS.Location = new Point(3, 0xa2);
            gbTS.Name = "gbTS";
            gbTS.Size = new Size(0x3a4, 0x9b);
            gbTS.TabIndex = 0xd5;
            gbTS.TabStop = false;
            gbTS.Text = "Transient Settings";
            btnResetTransient.BackColor = Color.LightSlateGray;
            btnResetTransient.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnResetTransient.ForeColor = Color.White;
            btnResetTransient.Location = new Point(0x218, 0x71);
            btnResetTransient.Name = "btnResetTransient";
            btnResetTransient.Size = new Size(0x49, 0x1f);
            btnResetTransient.TabIndex = 0xca;
            btnResetTransient.Text = "Reset";
            btnResetTransient.UseVisualStyleBackColor = false;
            btnResetTransient.Click += new EventHandler(btnResetTransient_Click);
            btnSetTransient.BackColor = Color.LightSlateGray;
            btnSetTransient.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSetTransient.ForeColor = Color.White;
            btnSetTransient.Location = new Point(0x1cb, 0x71);
            btnSetTransient.Name = "btnSetTransient";
            btnSetTransient.Size = new Size(0x49, 0x1f);
            btnSetTransient.TabIndex = 0xc9;
            btnSetTransient.Text = "Set";
            btnSetTransient.UseVisualStyleBackColor = false;
            btnSetTransient.Click += new EventHandler(btnSetTransient_Click);
            rdoTransClearDebounce.AutoSize = true;
            rdoTransClearDebounce.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            rdoTransClearDebounce.ForeColor = Color.White;
            rdoTransClearDebounce.Location = new Point(0x315, 0x75);
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
            p18.Location = new Point(4, 0x1c);
            p18.Name = "p18";
            p18.Size = new Size(0x1a2, 0x4c);
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
            chkTransEnableLatch.Location = new Point(0x13b, 0x33);
            chkTransEnableLatch.Name = "chkTransEnableLatch";
            chkTransEnableLatch.Size = new Size(0x62, 0x13);
            chkTransEnableLatch.TabIndex = 0x81;
            chkTransEnableLatch.Text = "Enable Latch";
            chkTransEnableLatch.UseVisualStyleBackColor = true;
            chkTransEnableLatch.CheckedChanged += new EventHandler(chkTransEnableLatch_CheckedChanged);
            chkTransEnableXFlag.AutoSize = true;
            chkTransEnableXFlag.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
            chkTransEnableXFlag.Location = new Point(4, 0x33);
            chkTransEnableXFlag.Name = "chkTransEnableXFlag";
            chkTransEnableXFlag.Size = new Size(0x67, 0x13);
            chkTransEnableXFlag.TabIndex = 120;
            chkTransEnableXFlag.Text = "Enable X Flag";
            chkTransEnableXFlag.UseVisualStyleBackColor = true;
            chkTransEnableXFlag.CheckedChanged += new EventHandler(chkTransEnableXFlag_CheckedChanged);
            chkTransEnableZFlag.AutoSize = true;
            chkTransEnableZFlag.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
            chkTransEnableZFlag.Location = new Point(0xd4, 0x33);
            chkTransEnableZFlag.Name = "chkTransEnableZFlag";
            chkTransEnableZFlag.Size = new Size(0x66, 0x13);
            chkTransEnableZFlag.TabIndex = 2;
            chkTransEnableZFlag.Text = "Enable Z Flag";
            chkTransEnableZFlag.UseVisualStyleBackColor = true;
            chkTransEnableZFlag.CheckedChanged += new EventHandler(chkTransEnableZFlag_CheckedChanged);
            chkTransEnableYFlag.AutoSize = true;
            chkTransEnableYFlag.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
            chkTransEnableYFlag.Location = new Point(110, 0x33);
            chkTransEnableYFlag.Name = "chkTransEnableYFlag";
            chkTransEnableYFlag.Size = new Size(0x66, 0x13);
            chkTransEnableYFlag.TabIndex = 1;
            chkTransEnableYFlag.Text = "Enable Y Flag";
            chkTransEnableYFlag.UseVisualStyleBackColor = true;
            chkTransEnableYFlag.CheckedChanged += new EventHandler(chkTransEnableYFlag_CheckedChanged);
            tbTransDebounce.Location = new Point(0x274, 0x42);
            tbTransDebounce.Maximum = 0xff;
            tbTransDebounce.Name = "tbTransDebounce";
            tbTransDebounce.Size = new Size(0x12e, 0x2d);
            tbTransDebounce.TabIndex = 0x79;
            tbTransDebounce.Scroll += new EventHandler(tbTransDebounce_Scroll);
            lblTransDebounceVal.AutoSize = true;
            lblTransDebounceVal.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTransDebounceVal.ForeColor = Color.White;
            lblTransDebounceVal.Location = new Point(0x22b, 0x49);
            lblTransDebounceVal.Name = "lblTransDebounceVal";
            lblTransDebounceVal.Size = new Size(0x10, 0x10);
            lblTransDebounceVal.TabIndex = 0x7b;
            lblTransDebounceVal.Text = "0";
            rdoTransDecDebounce.AutoSize = true;
            rdoTransDecDebounce.Checked = true;
            rdoTransDecDebounce.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            rdoTransDecDebounce.ForeColor = Color.White;
            rdoTransDecDebounce.Location = new Point(0x266, 0x75);
            rdoTransDecDebounce.Name = "rdoTransDecDebounce";
            rdoTransDecDebounce.Size = new Size(0xb0, 20);
            rdoTransDecDebounce.TabIndex = 0xb6;
            rdoTransDecDebounce.TabStop = true;
            rdoTransDecDebounce.Text = "Decrement Debounce";
            rdoTransDecDebounce.UseVisualStyleBackColor = true;
            rdoTransDecDebounce.CheckedChanged += new EventHandler(rdoTransDecDebounce_CheckedChanged);
            tbTransThreshold.Location = new Point(0x274, 0x11);
            tbTransThreshold.Maximum = 0x7f;
            tbTransThreshold.Name = "tbTransThreshold";
            tbTransThreshold.Size = new Size(0x12e, 0x2d);
            tbTransThreshold.TabIndex = 0x7f;
            tbTransThreshold.Scroll += new EventHandler(tbTransThreshold_Scroll);
            lblTransDebounce.AutoSize = true;
            lblTransDebounce.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTransDebounce.ForeColor = Color.White;
            lblTransDebounce.Location = new Point(0x1cb, 0x49);
            lblTransDebounce.Name = "lblTransDebounce";
            lblTransDebounce.Size = new Size(0x4f, 0x10);
            lblTransDebounce.TabIndex = 0x7a;
            lblTransDebounce.Text = "Debounce";
            lblTransThreshold.AutoSize = true;
            lblTransThreshold.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTransThreshold.ForeColor = Color.White;
            lblTransThreshold.Location = new Point(0x1cb, 0x17);
            lblTransThreshold.Name = "lblTransThreshold";
            lblTransThreshold.Size = new Size(0x4e, 0x10);
            lblTransThreshold.TabIndex = 0x70;
            lblTransThreshold.Text = "Threshold";
            lblTransThresholdVal.AutoSize = true;
            lblTransThresholdVal.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTransThresholdVal.ForeColor = Color.White;
            lblTransThresholdVal.Location = new Point(570, 0x17);
            lblTransThresholdVal.Name = "lblTransThresholdVal";
            lblTransThresholdVal.Size = new Size(0x10, 0x10);
            lblTransThresholdVal.TabIndex = 0x71;
            lblTransThresholdVal.Text = "0";
            lblTransThresholdg.AutoSize = true;
            lblTransThresholdg.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTransThresholdg.ForeColor = Color.White;
            lblTransThresholdg.Location = new Point(0x267, 20);
            lblTransThresholdg.Name = "lblTransThresholdg";
            lblTransThresholdg.Size = new Size(0x11, 0x10);
            lblTransThresholdg.TabIndex = 120;
            lblTransThresholdg.Text = "g";
            lblTransDebouncems.AutoSize = true;
            lblTransDebouncems.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTransDebouncems.ForeColor = Color.White;
            lblTransDebouncems.Location = new Point(0x25d, 0x47);
            lblTransDebouncems.Name = "lblTransDebouncems";
            lblTransDebouncems.Size = new Size(0x1c, 0x10);
            lblTransDebouncems.TabIndex = 0x7c;
            lblTransDebouncems.Text = "ms";
            AccelControllerGroup.Controls.Add(groupBox1);
            AccelControllerGroup.Controls.Add(gbDR);
            AccelControllerGroup.Controls.Add(gbOM);
            AccelControllerGroup.Controls.Add(label71);
            AccelControllerGroup.Controls.Add(ledWake);
            AccelControllerGroup.Controls.Add(ledSleep);
            AccelControllerGroup.Controls.Add(ledStandby);
            AccelControllerGroup.Controls.Add(lbsleep);
            AccelControllerGroup.Controls.Add(label173);
            AccelControllerGroup.Location = new Point(3, 8);
            AccelControllerGroup.Name = "AccelControllerGroup";
            AccelControllerGroup.Size = new Size(0x3a4, 0x99);
            AccelControllerGroup.TabIndex = 0xd4;
            AccelControllerGroup.TabStop = false;
            groupBox1.Controls.Add(rdoActive);
            groupBox1.Controls.Add(rdoStandby);
            groupBox1.Location = new Point(0x2a4, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(0xbc, 0x2e);
            groupBox1.TabIndex = 0xc5;
            groupBox1.TabStop = false;
            rdoActive.AutoSize = true;
            rdoActive.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            rdoActive.Location = new Point(0x68, 0x12);
            rdoActive.Name = "rdoActive";
            rdoActive.Size = new Size(0x45, 20);
            rdoActive.TabIndex = 0x71;
            rdoActive.Text = "Active";
            rdoActive.UseVisualStyleBackColor = true;
            rdoActive.CheckedChanged += new EventHandler(rdoActive_CheckedChanged);
            rdoStandby.AutoSize = true;
            rdoStandby.Checked = true;
            rdoStandby.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            rdoStandby.Location = new Point(11, 0x12);
            rdoStandby.Name = "rdoStandby";
            rdoStandby.Size = new Size(0x57, 20);
            rdoStandby.TabIndex = 0x70;
            rdoStandby.TabStop = true;
            rdoStandby.Text = "Standby ";
            rdoStandby.UseVisualStyleBackColor = true;
            rdoStandby.CheckedChanged += new EventHandler(rdoStandby_CheckedChanged);
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
            gbOM.Location = new Point(7, 8);
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
            pOverSampling.Location = new Point(6, 0x37);
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
            rdoOSHiResMode.Checked = true;
            rdoOSHiResMode.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
            rdoOSHiResMode.Location = new Point(5, 0x30);
            rdoOSHiResMode.Name = "rdoOSHiResMode";
            rdoOSHiResMode.Size = new Size(0x6c, 0x13);
            rdoOSHiResMode.TabIndex = 0xe0;
            rdoOSHiResMode.TabStop = true;
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
            label71.AutoSize = true;
            label71.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label71.Location = new Point(0x2c1, 0x5f);
            label71.Name = "label71";
            label71.Size = new Size(0x5b, 0x10);
            label71.TabIndex = 0xc0;
            label71.Text = "Wake Mode";
            ledWake.LedStyle = LedStyle.Round3D;
            ledWake.Location = new Point(0x31e, 90);
            ledWake.Name = "ledWake";
            ledWake.OffColor = Color.Red;
            ledWake.Size = new Size(30, 0x1f);
            ledWake.TabIndex = 0xbf;
            ledSleep.LedStyle = LedStyle.Round3D;
            ledSleep.Location = new Point(0x31e, 0x74);
            ledSleep.Name = "ledSleep";
            ledSleep.OffColor = Color.Red;
            ledSleep.Size = new Size(30, 0x1f);
            ledSleep.TabIndex = 0x91;
            ledStandby.LedStyle = LedStyle.Round3D;
            ledStandby.Location = new Point(0x31e, 0x3d);
            ledStandby.Name = "ledStandby";
            ledStandby.OffColor = Color.Red;
            ledStandby.Size = new Size(30, 0x1f);
            ledStandby.TabIndex = 0xc1;
            ledStandby.Value = true;
            lbsleep.AutoSize = true;
            lbsleep.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbsleep.Location = new Point(0x2c0, 0x7b);
            lbsleep.Name = "lbsleep";
            lbsleep.Size = new Size(0x5c, 0x10);
            lbsleep.TabIndex = 0xb9;
            lbsleep.Text = "Sleep Mode";
            label173.AutoSize = true;
            label173.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label173.Location = new Point(0x2b0, 0x44);
            label173.Name = "label173";
            label173.Size = new Size(0x6c, 0x10);
            label173.TabIndex = 0xc2;
            label173.Text = "Standby Mode";
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
            CommStrip.Items.AddRange(new ToolStripItem[] { CommStripButton, toolStripStatusLabel });
            CommStrip.Location = new Point(0, 0x2ba);
            CommStrip.Name = "CommStrip";
            CommStrip.Size = new Size(0x3bd, 0x16);
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
            pictureBox1.Location = new Point(0, -1);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(0x3bd, 0x39);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 0x48;
            pictureBox1.TabStop = false;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            AutoScroll = true;
            BackColor = Color.LightSlateGray;
            base.ClientSize = new Size(0x3bd, 720);
            base.Controls.Add(CommStrip);
            base.Controls.Add(panelGeneral);
            base.Controls.Add(pictureBox1);
            base.Controls.Add(menuStrip1);
            base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            base.MaximizeBox = false;
            base.Name = "FIFOLowPowerDemo";
            base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            Text = "Directional Shake Using Embedded  FIFO- Low Power Demo: Comparison to Embedded Directional Function";
            base.Load += new EventHandler(FIFOLowPowerDemo_Load);
            base.Resize += new EventHandler(TransDemo_Resize);
            panelGeneral.ResumeLayout(false);
            panelDisplay.ResumeLayout(false);
            panelDisplay.PerformLayout();
            pTrans2.ResumeLayout(false);
            pTrans2.PerformLayout();
            ((ISupportInitialize) ledTransEA).EndInit();
            ((ISupportInitialize) ledTransZDetect).EndInit();
            ((ISupportInitialize) ledTransYDetect).EndInit();
            ((ISupportInitialize) ledTransXDetect).EndInit();
            ((ISupportInitialize) ledMCU).EndInit();
            ((ISupportInitialize) legend1).EndInit();
            ((ISupportInitialize) pltFIFOPlot).EndInit();
            panelAdvanced.ResumeLayout(false);
            gbTS.ResumeLayout(false);
            gbTS.PerformLayout();
            p18.ResumeLayout(false);
            p18.PerformLayout();
            tbTransDebounce.EndInit();
            tbTransThreshold.EndInit();
            AccelControllerGroup.ResumeLayout(false);
            AccelControllerGroup.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
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
                p18.Enabled = false;
                btnAutoCal.Enabled = false;
                int[] datapassed = new int[] { 1 };
                ControllerObj.ActiveFlag = true;
                ControllerReqPacket reqName = new ControllerReqPacket();
                ControllerObj.CreateNewTaskFromGUI(reqName, 0, 0x53, datapassed);
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
            if ((iTransStat & 1) != 0)
            {
                lblTransPolX.Text = "Positive";
            }
            else
            {
                lblTransPolX.Text = "Negative";
            }
            if ((iTransStat & 2) != 0)
            {
                ledTransXDetect.Value = true;
            }
            else
            {
                ledTransXDetect.Value = false;
            }
            if ((iTransStat & 4) != 0)
            {
                lblTransPolY.Text = "Positive";
            }
            else
            {
                lblTransPolY.Text = "Negative";
            }
            if ((iTransStat & 8) != 0)
            {
                ledTransYDetect.Value = true;
            }
            else
            {
                ledTransYDetect.Value = false;
            }
            if ((iTransStat & 0x10) != 0)
            {
                lblTransPolZ.Text = "Positive";
            }
            else
            {
                lblTransPolZ.Text = "Negative";
            }
            if ((iTransStat & 0x20) != 0)
            {
                ledTransZDetect.Value = true;
            }
            else
            {
                ledTransZDetect.Value = false;
            }
            if ((iTransStat & 0x40) != 0)
            {
                ledTransEA.Value = true;
                ledMCU.Value = true;
            }
            else
            {
                ledTransEA.Value = false;
                ledMCU.Value = false;
            }
            if (ledTransEA.Value)
            {
                if (timercount != 10)
                {
                    timercount++;
                }
                else
                {
                    iTransStat = 0;
                    ledTransEA.Value = false;
                    ledTransXDetect.Value = false;
                    ledTransYDetect.Value = false;
                    ledTransZDetect.Value = false;
                    ledMCU.Value = false;
                    switch (ddlDataRate.SelectedIndex)
                    {
                        case 0:
                            txtCurrent.Text = "0.665 mA";
                            break;

                        case 1:
                            txtCurrent.Text = "0.585 mA";
                            break;

                        case 2:
                            txtCurrent.Text = "0.544 mA";
                            break;

                        case 3:
                            txtCurrent.Text = "0.524 mA";
                            break;

                        default:
                            txtCurrent.Text = "0.524 mA";
                            break;
                    }
                    timercount = 0;
                }
            }
            UpdateFormState();
            lock (qFIFOArray)
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
                        if (countsArray[i] != null)
                        {
                            pltFIFOPlot.Plots[0].PlotYAppend((double) ((((sbyte) countsArray[i].XAxis) * 0.015625) * num));
                            pltFIFOPlot.Plots[1].PlotYAppend((double) ((((sbyte) countsArray[i].YAxis) * 0.015625) * num));
                            pltFIFOPlot.Plots[2].PlotYAppend((double) ((((sbyte) countsArray[i].ZAxis) * 0.015625) * num));
                        }
                    }
                }
            }
        }

        private void TransDemo_Resize(object sender, EventArgs e)
        {
            windowResized = true;
            windowHeight = base.Height;
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
            if (blDirShakeChange)
            {
                blDirShakeChange = false;
                textBox2.Text = sDirShake;
                switch (ddlDataRate.SelectedIndex)
                {
                    case 0:
                        txtCurrent.Text = "12.165 mA";
                        goto Label_00AF;

                    case 1:
                        txtCurrent.Text = "12.085 mA";
                        goto Label_00AF;

                    case 2:
                        txtCurrent.Text = "12.044 mA";
                        goto Label_00AF;

                    case 3:
                        txtCurrent.Text = "12.024 mA";
                        goto Label_00AF;
                }
                txtCurrent.Text = "12.024 mA";
            }
        Label_00AF:
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

