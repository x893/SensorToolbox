﻿using Freescale.SASD.Communication;
using MMA845x_DEMO.Properties;
using NationalInstruments.UI;
using NationalInstruments.UI.WindowsForms;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Resources;
using System.Threading;
using System.Windows.Forms;

namespace MMA845xQEvaluation
{
    public class NeutronEvaluationSoftware : Form
	{
		public int DeviceID = 7;

		private enum eCommMode
		{
			Closed = 1,
			FindingHW = 2,
			Running = 3
		}
        
		#region Privates
		private Button btnAutoCal;
        private Button btnDisableData;
        private Button btnMFF1Reset;
        private Button btnMFF1Set;
        private Button btnPulseResetTime2ndPulse;
        private Button btnPulseSetTime2ndPulse;
        private Button btnRead;
        private Button btnResetFirstPulseTimeLimit;
        private Button btnResetPLDebounce;
        private Button btnResetPulseThresholds;
        private Button btnResetTransient;
        private Button btnResetTransientNEW;
        private Button btnResetWatermark;
        private Button btnSetFirstPulseTimeLimit;
        private Button btnSetMode;
        private Button btnSetPLDebounce;
        private Button btnSetPulseThresholds;
        private Button btnSetSleepTimer;
        private Button btnSetTransient;
        private Button btnSetTransientNEW;
        private Button btnSleepTimerReset;
        private Button btnUpdateRegs;
        private Button btnViewData;
        private Button btnWatermark;
        private Button btnWrite;
        private Button btnWriteCal;
        private Button button1;
        private CheckBox chkAnalogLowNoise;
        private CheckBox chkDefaultFFSettings1;
        private CheckBox chkDefaultMotion1;
        private CheckBox chkDefaultTransSettings;
        private CheckBox chkDefaultTransSettings1;
        private CheckBox chkDisableFIFO;
        private CheckBox chkEnableASleep;
        private CheckBox chkEnablePL;
        private CheckBox chkEnableX;
        private CheckBox chkEnableXMFF;
        private CheckBox chkEnableY;
        private CheckBox chkEnableYMFF;
        private CheckBox chkEnableZ;
        private CheckBox chkEnableZMFF;
        private CheckBox chkEnIntASleep;
        private CheckBox chkEnIntDR;
        private CheckBox chkEnIntFIFO;
        private CheckBox chkEnIntMFF1;
        private CheckBox chkEnIntPL;
        private CheckBox chkEnIntPulse;
        private CheckBox chkEnIntTrans;
        private CheckBox chkEnIntTrans1;
        private CheckBox chkHPFDataOut;
        private CheckBox chkMFF1EnableLatch;
        private CheckBox chkPLDefaultSettings;
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
        private CheckBox chkSelfTest;
        private CheckBox chkTransBypassHPF;
        private CheckBox chkTransBypassHPFNEW;
        private CheckBox chkTransEnableLatch;
        private CheckBox chkTransEnableLatchNEW;
        private CheckBox chkTransEnableXFlag;
        private CheckBox chkTransEnableXFlagNEW;
        private CheckBox chkTransEnableYFlag;
        private CheckBox chkTransEnableYFlagNEW;
        private CheckBox chkTransEnableZFlag;
        private CheckBox chkTransEnableZFlagNEW;
        private CheckBox chkTransLog;
        private CheckBox chkTriggerLP;
        private CheckBox chkTriggerMFF;
        private CheckBox chkTriggerPulse;
        private CheckBox chkTrigTrans;
        private CheckBox chkWakeFIFOGate;
        private CheckBox chkWakeLP;
        private CheckBox chkWakeMFF1;
        private CheckBox chkWakePulse;
        private CheckBox chkWakeTrans;
        private CheckBox chkWakeTrans1;
        private CheckBox chkXEFE;
        private CheckBox chkXEnableTrans;
        private CheckBox chkXYZ12Log;
        private CheckBox chkXYZ8Log;
        private CheckBox chkYEFE;
        private CheckBox chkYEnableTrans;
        private CheckBox chkZEFE;
        private CheckBox chkZEnableTrans;
        private Queue CntrlQueue;
        private eCommMode CommMode = eCommMode.Closed;
        private StatusStrip CommStrip;
        private ToolStripDropDownButton CommStripButton;
        private IContainer components = null;
        private object ControllerEventsLock = new object();
        private XYZCounts Count10bData = new XYZCounts();
        private XYZCounts Count12bData = new XYZCounts();
        private XYZCounts Count14bData = new XYZCounts();
        private XYZCounts Count8bData = new XYZCounts();
        private TabPage DataConfigPage;
        private ComboBox ddlBFTripA;
        private ComboBox ddlDataRate;
        private ComboBox ddlHPFilter;
        private ComboBox ddlHysteresisA;
        private ComboBox ddlPLTripA;
        private ComboBox ddlSleepSR;
        private ComboBox ddlZLockA;
        private XYZIntCounts DisplayXYZ10bit = new XYZIntCounts();
        private XYZIntCounts DisplayXYZ12bit = new XYZIntCounts();
        private XYZIntCounts DisplayXYZ14bit = new XYZIntCounts();
        private XYZIntCounts DisplayXYZ8bit = new XYZIntCounts();
        private double DR_PulseTimeStepNoLPF;
        private double DR_timestep;
        private double DRSleep_PulseTimeStepNoLPF;
        private double DRSleep_timestep;
        private BoardComm dv;
        private int FIFODump8orFull;
        private int FIFOModeValue;
        private TabPage FIFOPage;
        private int FullScaleValue;
        private GroupBox gb3bF;
        private GroupBox gbASS;
        private GroupBox gbDR;
        private GroupBox gbIF;
        private GroupBox gbMF1;
        private GroupBox gbOC;
        private GroupBox gbOD;
        private GroupBox gbOM;
        private GroupBox gbPLDisappear;
        private GroupBox gbRegisterName;
        private GroupBox gbSDPS;
        private GroupBox gbST;
        private GroupBox gbStatus;
        private GroupBox gbTS;
        private GroupBox gbTSNEW;
        private GroupBox gbWatermark;
        private GroupBox gbwfs;
        private GroupBox gbXD;
        private XYZGees Gees10bData = new XYZGees();
        private XYZGees Gees12bData = new XYZGees();
        private XYZGees Gees14bData = new XYZGees();
        private XYZGees Gees8bData = new XYZGees();
        private WaveformGraph gphXYZ;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private GroupBox groupBox3;
        private GroupBox groupBox4;
        private GroupBox groupBox6;
        private Queue GuiQueue;
        private int HysAngle;
        private int Hysteresis_index;
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
        private Label label1;
        private Label label10;
        private Label label103;
        private Label label11;
        private Label label110;
        private Label label111;
        private Label label112;
        private Label label113;
        private Label label114;
        private Label label115;
        private Label label116;
        private Label label117;
        private Label label118;
        private Label label119;
        private Label label12;
        private Label label120;
        private Label label13;
        private Label label14;
        private Label label15;
        private Label label16;
        private Label label17;
        private Label label172;
        private Label label173;
        private Label label18;
        private Label label19;
        private Label label191;
        private Label label199;
        private Label label2;
        private Label label20;
        private Label label203;
        private Label label204;
        private Label label205;
        private Label label206;
        private Label label207;
        private Label label21;
        private Label label230;
        private Label label231;
        private Label label232;
        private Label label233;
        private Label label236;
        private Label label238;
        private Label label239;
        private Label label24;
        private Label label25;
        private Label label255;
        private Label label256;
        private Label label258;
        private Label label263;
        private Label label264;
        private Label label265;
        private Label label266;
        private Label label268;
        private Label label27;
        private Label label273;
        private Label label275;
        private Label label277;
        private Label label279;
        private Label label281;
        private Label label283;
        private Label label285;
        private Label label287;
        private Label label29;
        private Label label3;
        private Label label32;
        private Label label34;
        private Label label35;
        private Label label36;
        private Label label37;
        private Label label38;
        private Label label39;
        private Label label4;
        private Label label40;
        private Label label41;
        private Label label42;
        private Label label43;
        private Label label44;
        private Label label45;
        private Label label46;
        private Label label47;
        private Label label48;
        private Label label49;
        private Label label5;
        private Label label51;
        private Label label53;
        private Label label54;
        private Label label56;
        private Label label57;
        private Label label58;
        private Label label6;
        private Label label60;
        private Label label61;
        private Label label62;
        private Label label63;
        private Label label64;
        private Label label65;
        private Label label66;
        private Label label67;
        private Label label68;
        private Label label69;
        private Label label7;
        private Label label70;
        private Label label71;
        private Label label72;
        private Label label8;
        private Label label9;
        private Label label96;
        private Label label97;
        private Label label98;
        private Label lb_BitN0;
        private Label lb_BitN07;
        private Label lb_BitN1;
        private Label lb_BitN2;
        private Label lb_BitN3;
        private Label lb_BitN4;
        private Label lb_BitN5;
        private Label lb_BitN6;
        private Label lb_BitN7;
        private Label lb_BitV0;
        private Label lb_BitV07;
        private Label lb_BitV1;
        private Label lb_BitV2;
        private Label lb_BitV3;
        private Label lb_BitV4;
        private Label lb_BitV5;
        private Label lb_BitV6;
        private Label lb_BitV7;
        private Label lbl00;
        private Label lbl01;
        private Label lbl02;
        private Label lbl03;
        private Label lbl04;
        private Label lbl05;
        private Label lbl06;
        private Label lbl09;
        private Label lbl0A;
        private Label lbl0B;
        private Label lbl0C;
        private Label lbl0D;
        private Label lbl0E;
        private Label lbl0F;
        private Label lbl10;
        private Label lbl11;
        private Label lbl12;
        private Label lbl13;
        private Label lbl14;
        private Label lbl15;
        private Label lbl16;
        private Label lbl17;
        private Label lbl18;
        private Label lbl19;
        private Label lbl1A;
        private Label lbl1B;
        private Label lbl1C;
        private Label lbl1D;
        private Label lbl1E;
        private Label lbl1F;
        private Label lbl20;
        private Label lbl21;
        private Label lbl22;
        private Label lbl23;
        private Label lbl24;
        private Label lbl25;
        private Label lbl26;
        private Label lbl27;
        private Label lbl28;
        private Label lbl29;
        private Label lbl2A;
        private Label lbl2B;
        private Label lbl2C;
        private Label lbl2D;
        private Label lbl2E;
        private Label lbl2F;
        private Label lbl2gSensitivity;
        private Label lbl30;
        private Label lbl31;
        private Label lbl4gSensitivity;
        private Label lbl8gSensitivity;
        private Label lblBouncems;
        private Label lblComment1;
        private Label lblComment2;
        private Label lblCurrent_FIFO_Count;
        private Label lblDebouncePL;
        private Label lblF_Count;
        private Label lblFCnt0;
        private Label lblFCnt1;
        private Label lblFCnt2;
        private Label lblFCnt3;
        private Label lblFCnt4;
        private Label lblFCnt5;
        private Label lblFIFOStatus;
        private Label lblFileName;
        private Label lblFirstPulseTimeLimit;
        private Label lblFirstPulseTimeLimitms;
        private Label lblFirstTimeLimitVal;
        private Label lblFOvf;
        private Label lblMFF1Debounce;
        private Label lblMFF1Debouncems;
        private Label lblMFF1DebounceVal;
        private Label lblMFF1Threshold;
        private Label lblMFF1Thresholdg;
        private Label lblMFF1ThresholdVal;
        private Label lblMotionDataType;
        private Label lblntFIFO;
        private Label lblp2LTripAngle;
        private Label lblPLbounceVal;
        private Label lblPLWarning;
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
        private Label lblSleepms;
        private Label lblSleepTimer;
        private Label lblSleepTimerValue;
        private Label lblTransDataType;
        private Label lblTransDebounce;
        private Label lblTransDebouncems;
        private Label lblTransDebouncemsNEW;
        private Label lblTransDebounceNEW;
        private Label lblTransDebounceVal;
        private Label lblTransDebounceValNEW;
        private Label lblTransNewPolX;
        private Label lblTransNewPolY;
        private Label lblTransNewPolZ;
        private Label lblTransPolX;
        private Label lblTransPolY;
        private Label lblTransPolZ;
        private Label lblTransThreshold;
        private Label lblTransThresholdg;
        private Label lblTransThresholdgNEW;
        private Label lblTransThresholdNEW;
        private Label lblTransThresholdVal;
        private Label lblTransThresholdValNEW;
        private Label lblValL2PResult;
        private Label lblValP2LResult;
        private Label lblWatermark;
        private Label lblWatermarkValue;
        private Label lblWmrk;
        private Label lblX12bC;
        private Label lblX12bG;
        private Label lblX8bC;
        private Label lblX8bG;
        private Label lblXCal;
        private Label lblXDrdy;
        private Label lblXFull;
        private Label lblXHP;
        private Label lblXOW;
        private Label lblXYZDrdy;
        private Label lblXYZOW;
        private Label lblY12bC;
        private Label lblY12bG;
        private Label lblY8bC;
        private Label lblY8bG;
        private Label lblYCal;
        private Label lblYDrdy;
        private Label lblYFull;
        private Label lblYHP;
        private Label lblYOW;
        private Label lblZ12bC;
        private Label lblZ12bG;
        private Label lblZ8bC;
        private Label lblZ8bG;
        private Label lblZCal;
        private Label lblZDrdy;
        private Label lblZFull;
        private Label lblZHP;
        private Label lblZOW;
        private Label lbsleep;
        private Led ledASleep;
        private Led ledCurPLBack;
        private Led ledCurPLDown;
        private Led ledCurPLFront;
        private Led ledCurPLLeft;
        private Led ledCurPLLO;
        private Led ledCurPLNew;
        private Led ledCurPLRight;
        private Led ledCurPLUp;
        private Led ledDataReady;
        private Led ledFIFO;
        private Led ledFIFOStatus;
        private Led ledMFF1;
        private Led ledMFF1EA;
        private Led ledMFF1XHE;
        private Led ledMFF1YHE;
        private Led ledMFF1ZHE;
        private Led ledOrient;
        private Led ledOverFlow;
        private Led ledPulse;
        private Led ledPulseDouble;
        private Led ledPulseEA;
        private Led ledPX;
        private Led ledPY;
        private Led ledPZ;
        private Led ledRealTimeStatus;
        private Led ledSleep;
        private Led ledStandby;
        private Led ledTrans;
        private Led ledTrans1;
        private Led ledTransEA;
        private Led ledTransEANEW;
        private Led ledTransXDetect;
        private Led ledTransXDetectNEW;
        private Led ledTransYDetect;
        private Led ledTransYDetectNEW;
        private Led ledTransZDetect;
        private Led ledTransZDetectNEW;
        private Led ledTrigLP;
        private Led ledTrigMFF;
        private Led ledTrigTap;
        private Led ledTrigTrans;
        private Led ledWake;
        private Led ledWatermark;
        private Led ledXDR;
        private Led ledXOW;
        private Led ledXYZDR;
        private Led ledXYZOW;
        private Led ledYDR;
        private Led ledYOW;
        private Led ledZDR;
        private Led ledZOW;
        private Legend legend1;
        private Legend legend2;
        private Legend legend3;
        private LegendItem legendItem1;
        private LegendItem legendItem2;
        private LegendItem legendItem3;
        private LegendItem legendItem4;
        private LegendItem legendItem5;
        private LegendItem legendItem6;
        private LegendItem legendItem7;
        private LegendItem legendItem8;
        private LegendItem legendItem9;
        private TabPage MainScreenEval;
        private WaveformGraph MainScreenGraph;
        private WaveformPlot MainScreenXAxis;
        private WaveformPlot MainScreenYAxis;
        private WaveformPlot MainScreenZAxis;
        private TabPage MFF1_2Page;
        private WaveformGraph MFFGraph;
        private Panel p1;
        private Panel p10;
        private Panel p11;
        private Panel p12;
        private Panel p14;
        private Panel p14b8bSelect;
        private Panel p15;
        private Panel p18;
        private Panel p19;
        private Panel p2;
        private Panel p20;
        private Panel p21;
        private int P2LResultA;
        private Panel p4;
        private Panel p5;
        private Panel p6;
        private Panel p7;
        private Panel p8;
        private Panel p9;
        private Panel panel1;
        private Panel panel10;
        private Panel panel11;
        private Panel panel12;
        private Panel panel15;
        private Panel panel16;
        private Panel panel17;
        private Panel panel18;
        private Panel panel3;
        private Panel panel4;
        private Panel panel5;
        private Panel panel6;
        private Panel panel8;
        private Panel panel9;
        private Panel pFullResData;
        private PictureBox pictureBox2;
        private int PL_index;
        private TabPage PL_Page;
        private int PLAngle;
        private PictureBox PLImage;
        private Panel pnlAutoSleep;
        private Panel pOverSampling;
        private Panel pPLActive;
        private Panel pSOS;
        private Panel pTrans2;
        private Panel pTransNEW;
        private Panel pTriggerMode;
        private double pulse_step;
        private TabPage PulseDetection;
        private RadioButton rb01;
        private RadioButton rb02;
        private RadioButton rb03;
        private RadioButton rb04;
        private RadioButton rb05;
        private RadioButton rb06;
        private RadioButton rb09;
        private RadioButton rb0A;
        private RadioButton rb0B;
        private RadioButton rb0C;
        private RadioButton rb0D;
        private RadioButton rb0E;
        private RadioButton rb0F;
        private RadioButton rb10;
        private RadioButton rb11;
        private RadioButton rb12;
        private RadioButton rb13;
        private RadioButton rb14;
        private RadioButton rb15;
        private RadioButton rb16;
        private RadioButton rb17;
        private RadioButton rb18;
        private RadioButton rb19;
        private RadioButton rb1A;
        private RadioButton rb1B;
        private RadioButton rb1C;
        private RadioButton rb1D;
        private RadioButton rb1E;
        private RadioButton rb1F;
        private RadioButton rb20;
        private RadioButton rb21;
        private RadioButton rb22;
        private RadioButton rb23;
        private RadioButton rb24;
        private RadioButton rb25;
        private RadioButton rb26;
        private RadioButton rb27;
        private RadioButton rb28;
        private RadioButton rb29;
        private RadioButton rb2A;
        private RadioButton rb2B;
        private RadioButton rb2C;
        private RadioButton rb2D;
        private RadioButton rb2E;
        private RadioButton rb2F;
        private RadioButton rb30;
        private RadioButton rb31;
        private RadioButton rbR00;
        private RadioButton rdo2g;
        private RadioButton rdo4g;
        private RadioButton rdo8bitDataMain;
        private RadioButton rdo8g;
        private RadioButton rdoActive;
        private RadioButton rdoASleepINT_I1;
        private RadioButton rdoASleepINT_I2;
        private RadioButton rdoCircular;
        private RadioButton rdoClrDebouncePL;
        private RadioButton rdoDecDebouncePL;
        private RadioButton rdoDefaultSP;
        private RadioButton rdoDefaultSPDP;
        private RadioButton rdoDRINT_I1;
        private RadioButton rdoDRINT_I2;
        private RadioButton rdoFIFO14bitDataDisplay;
        private RadioButton rdoFIFO8bitDataDisplay;
        private RadioButton rdoFIFOINT_I1;
        private RadioButton rdoFIFOINT_I2;
        private RadioButton rdoFill;
        private RadioButton rdoINTActiveHigh;
        private RadioButton rdoINTActiveLow;
        private RadioButton rdoINTOpenDrain;
        private RadioButton rdoINTPushPull;
        private RadioButton rdoMFF1And;
        private RadioButton rdoMFF1ClearDebounce;
        private RadioButton rdoMFF1DecDebounce;
        private RadioButton rdoMFF1INT_I1;
        private RadioButton rdoMFF1INT_I2;
        private RadioButton rdoMFF1Or;
        private RadioButton rdoOSHiResMode;
        private RadioButton rdoOSLNLPMode;
        private RadioButton rdoOSLPMode;
        private RadioButton rdoOSNormalMode;
        private RadioButton rdoPLINT_I1;
        private RadioButton rdoPLINT_I2;
        private RadioButton rdoPulseINT_I1;
        private RadioButton rdoPulseINT_I2;
        private RadioButton rdoSOSHiResMode;
        private RadioButton rdoSOSLNLPMode;
        private RadioButton rdoSOSLPMode;
        private RadioButton rdoSOSNormalMode;
        private RadioButton rdoStandby;
        private RadioButton rdoTrans1INT_I1;
        private RadioButton rdoTrans1INT_I2;
        private RadioButton rdoTransClearDebounce;
        private RadioButton rdoTransClearDebounceNEW;
        private RadioButton rdoTransDecDebounce;
        private RadioButton rdoTransDecDebounceNEW;
        private RadioButton rdoTransINT_I1;
        private RadioButton rdoTransINT_I2;
        private RadioButton rdoTriggerMode;
        private RadioButton rdoXYZFullResMain;
        private TabPage Register_Page;
        private RichTextBox rtbFIFOdump;
        private static ReaderWriterLock RWLock = new ReaderWriterLock();
        private static ReaderWriterLock RWLockDispValues = new ReaderWriterLock();
        private static ReaderWriterLock RWLockGetCalValues = new ReaderWriterLock();
        private int SleepOSMode;
        private int SystemPolling;
        private TableLayoutPanel tableLayoutPanel1;
        private TabControl TabTool;
        private TrackBar tbFirstPulseTimeLimit;
        private TrackBar tbMFF1Debounce;
        private TrackBar tbMFF1Threshold;
        private TrackBar tbPL;
        private TrackBar tbPulse2ndPulseWin;
        private TrackBar tbPulseLatency;
        private TrackBar tbPulseXThreshold;
        private TrackBar tbPulseYThreshold;
        private TrackBar tbPulseZThreshold;
        private TrackBar tbSleepCounter;
        private TrackBar tbTransDebounce;
        private TrackBar tbTransDebounceNEW;
        private TrackBar tbTransThreshold;
        private TrackBar tbTransThresholdNEW;
        private TrackBar tBWatermark;
        private System.Windows.Forms.Timer TmrActive;
        private System.Windows.Forms.Timer tmrDataDisplay;
        private ToolStripStatusLabel toolStripStatusLabel;
        private ToolTip toolTip1;
        private TabPage TransientDetection;
        private MaskedTextBox txtCalX;
        private MaskedTextBox txtCalY;
        private MaskedTextBox txtCalZ;
        private TextBox txtRegRead;
        private TextBox txtRegValue;
        private TextBox txtRegWrite;
        private TextBox txtSaveFileName;
        private AccelController VeyronControllerObj;
        private int WakeOSMode;
        private WaveformPlot waveformPlot1;
        private WaveformPlot waveformPlot2;
        private WaveformPlot waveformPlot3;
        private WaveformPlot XAxis;
        private NationalInstruments.UI.XAxis xAxis1;
        private NationalInstruments.UI.XAxis xAxis2;
        private NationalInstruments.UI.XAxis xAxis3;
        private WaveformPlot YAxis;
        private NationalInstruments.UI.YAxis yAxis1;
        private NationalInstruments.UI.YAxis yAxis2;
        private NationalInstruments.UI.YAxis yAxis3;
        private WaveformPlot ZAxis;
		#endregion

		public NeutronEvaluationSoftware(object accelControllder)
        {
            InitializeComponent();

            string[] strArray = new string[2];
            VeyronControllerObj = (AccelController) accelControllder;
            this.Visible = true;
            int selectedIndex = TabTool.SelectedIndex;

            if (DeviceID != 2)
            {
                TabTool.TabPages.Remove(FIFOPage);
                ledFIFO.Visible = false;
                panel12.Visible = false;
                chkWakeFIFOGate.Visible = false;
                gbPLDisappear.Enabled = false;
                rb09.Visible = false;
                rb0A.Visible = false;
                lbl09.Visible = false;
                lbl0A.Visible = false;
                ledFIFOStatus.Visible = false;
                ledRealTimeStatus.Visible = false;
                lblFIFOStatus.Visible = false;
                lblFCnt0.Visible = false;
                lblFCnt1.Visible = false;
                lblFCnt2.Visible = false;
                lblFCnt3.Visible = false;
                lblFCnt4.Visible = false;
                lblFCnt5.Visible = false;
                lblWmrk.Visible = false;
                lblFOvf.Visible = false;
                gbStatus.Text = "Data Status Reg 0x00: Real Time Data Status";
            }

            if (DeviceID == 4)
            {
                Text = "MMA8453Q Full System Evaluation Software";
                chkHPFDataOut.Visible = false;
                rdoXYZFullResMain.Text = "XYZ10-bit";
                lbl2gSensitivity.Text = "256 counts/g 10-bit";
                lbl4gSensitivity.Text = "128 counts/g 10-bit";
                lbl8gSensitivity.Text = "64  counts/g 10-bit";
                lblXFull.Text = "X10-bit =";
                lblYFull.Text = "Y10-bit =";
                lblZFull.Text = "Z10-bit =";
            }
            else if (DeviceID == 3)
            {
                Text = "MMA8452Q Full System Evaluation Software";
                rdoXYZFullResMain.Text = "XYZ12-bit";
                lbl2gSensitivity.Text = "1024 counts/g 12-bit";
                lbl4gSensitivity.Text = " 512 counts/g 12-bit";
                lbl8gSensitivity.Text = " 256 counts/g 12-bit";
                lblXFull.Text = "X12-bit =";
                lblYFull.Text = "Y12-bit =";
                lblZFull.Text = "Z12-bit =";
            }
            else if (DeviceID == 7)
            {
                gbPLDisappear.Enabled = true;
                pFullResData.Visible = false;
                chkHPFDataOut.Visible = false;
                rdoXYZFullResMain.Visible = false;
                lbl2gSensitivity.Text = "64 counts/g 8-bit";
                rdo4g.Visible = false;
                rdo8g.Visible = false;
                lbl4gSensitivity.Visible = false;
                lbl8gSensitivity.Visible = false;
                lblXFull.Visible = false;
                lblYFull.Visible = false;
                lblZFull.Visible = false;
            }

            FullScaleValue = 0;
            GuiQueue = VeyronControllerObj.GetGUIQueue;
            CntrlQueue = VeyronControllerObj.GetControllerQueue;
            VeyronControllerObj.SetGuiHandle2(this);

			LoadResource();

            VeyronControllerObj.ControllerEvents += new AccelController.ControllerEventHandler(ControllerObj_ControllerEvents);

            TabTool.SelectedIndex = 0;
            ddlSleepSR.SelectedIndex = 0;
            ddlHPFilter.SelectedIndex = 0;
            WakeOSMode = 0;
            SleepOSMode = 0;
            DR_timestep = 2.5;
            DRSleep_timestep = 20.0;
            DR_PulseTimeStepNoLPF = 0.625;
            DRSleep_PulseTimeStepNoLPF = 5.0;
            rdo2g.Checked = true;
            rb2g_CheckedChanged(this, null);
            ddlZLockA.SelectedIndex = 4;
            ddlBFTripA.SelectedIndex = 1;
            ddlPLTripA.SelectedIndex = 5;
            ddlHysteresisA.SelectedIndex = 4;
            chkEnablePL.Checked = false;
            FIFOModeValue = 2;
            rdoStandby.Checked = true;
            rdoActive.Checked = false;
            p8.Enabled = true;
            SystemPolling = 0;
            FIFODump8orFull = 0;
            TmrActive.Enabled = true;
            TmrActive.Start();
            tmrDataDisplay.Enabled = true;
            tmrDataDisplay.Start();
            btnViewData.Enabled = false;
            btnDisableData.Enabled = false;
            gbXD.Enabled = false;

            VeyronControllerObj.BootFlag = true;
            CreateNewTaskFromGUI(new ControllerReqPacket(), 0, TaskID.Boot, new int[] { FullScaleValue });
            rdoINTOpenDrain.Checked = true;
            VeyronControllerObj.SetINTPPODFlag = true;
            CreateNewTaskFromGUI(new ControllerReqPacket(), 0, TaskID.SetINTPPOD, new int[] { 0xff });

            rdoINTActiveHigh.Checked = true;
            VeyronControllerObj.SetINTActiveFlag = true;
            CreateNewTaskFromGUI(new ControllerReqPacket(), 0, TaskID.SetINTPolarity, new int[] { 0xff });
            rdoFIFO8bitDataDisplay.Checked = true;
            rdoFIFO14bitDataDisplay.Checked = false;

            VeyronControllerObj.SetFREADFlag = true;
			CreateNewTaskFromGUI(new ControllerReqPacket(), 0, TaskID.SetFREAD, new int[] { 0xff });
            ddlDataRate.SelectedIndex = 0;
            ddlDataRate_SelectedIndexChanged(this, null);
        }

        private void btnAutoCal_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            VeyronControllerObj.AutoCalFlag = true;
            ControllerReqPacket reqName = new ControllerReqPacket();
            CreateNewTaskFromGUI(reqName, 0, TaskID.AutoCal, new int[] { DeviceID });
            Thread.Sleep(7000);
            Cursor = Cursors.Default;
            rdo2g.Checked = true;
            ddlDataRate.SelectedIndex = 5;
            chkAnalogLowNoise.Checked = true;
            rdoOSHiResMode.Checked = true;
        }

        private void btnDisableData_Click(object sender, EventArgs e)
        {
            VeyronControllerObj.DisableData();
            btnViewData.Enabled = true;
        }

        private void btnExitDataCfg_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void btnExitFIFO_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void btnExitMFF1_2_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void btnExitPL_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void btnExitPulse_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void btnExitRegisters_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void btnExitTrans_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void btnMFF1Reset_Click(object sender, EventArgs e)
        {
            btnMFF1Set.Enabled = true;
            btnMFF1Reset.Enabled = false;
            tbMFF1Threshold.Enabled = true;
            tbMFF1Debounce.Enabled = true;
            lblMFF1Threshold.Enabled = true;
            lblMFF1ThresholdVal.Enabled = true;
            lblMFF1Thresholdg.Enabled = true;
            lblMFF1Debounce.Enabled = true;
            lblMFF1DebounceVal.Enabled = true;
            lblMFF1Debouncems.Enabled = true;
        }

        private void btnMFF1Set_Click(object sender, EventArgs e)
        {
            VeyronControllerObj.SetMFF1ThresholdFlag = true;
			CreateNewTaskFromGUI(new ControllerReqPacket(), 3, TaskID.SetMFF1Threshold, new int[] { tbMFF1Threshold.Value });
            VeyronControllerObj.SetMFF1DebounceFlag = true;
			CreateNewTaskFromGUI(new ControllerReqPacket(), 3, TaskID.SetMFF1Debounce, new int[] { tbMFF1Debounce.Value });
            btnMFF1Set.Enabled = false;
            btnMFF1Reset.Enabled = true;
            tbMFF1Threshold.Enabled = false;
            tbMFF1Debounce.Enabled = false;
            lblMFF1Threshold.Enabled = false;
            lblMFF1ThresholdVal.Enabled = false;
            lblMFF1Thresholdg.Enabled = false;
            lblMFF1Debounce.Enabled = false;
            lblMFF1DebounceVal.Enabled = false;
            lblMFF1Debouncems.Enabled = false;
        }

        private void btnPulseResetTime2ndPulse_Click_1(object sender, EventArgs e)
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

        private void btnPulseSetTime2ndPulse_Click_1(object sender, EventArgs e)
        {
            VeyronControllerObj.SetPulseLatencyFlag = true;
			CreateNewTaskFromGUI(new ControllerReqPacket(), 6, TaskID.SetPulseLatency, new int[] { tbPulseLatency.Value });
            VeyronControllerObj.SetPulse2ndPulseWinFlag = true;
            CreateNewTaskFromGUI(new ControllerReqPacket(), 6, TaskID.SetPulse2ndPulseWin, new int[] { tbPulse2ndPulseWin.Value });
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
            VeyronControllerObj.ReadValueFlag = true;
            CreateNewTaskFromGUI(new ControllerReqPacket(), 1, TaskID.ReadValue, new int[] { Convert.ToInt32(txtRegRead.Text, 0x10) });
        }

        private void btnResetFirstPulseTimeLimit_Click_1(object sender, EventArgs e)
        {
            btnSetFirstPulseTimeLimit.Enabled = true;
            btnResetFirstPulseTimeLimit.Enabled = false;
            tbFirstPulseTimeLimit.Enabled = true;
            lblFirstPulseTimeLimit.Enabled = true;
            lblFirstPulseTimeLimitms.Enabled = true;
            lblFirstTimeLimitVal.Enabled = true;
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

        private void btnResetPulseThresholds_Click_1(object sender, EventArgs e)
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

        private void btnResetTransientNEW_Click_1(object sender, EventArgs e)
        {
            btnSetTransientNEW.Enabled = true;
            btnResetTransientNEW.Enabled = false;
            tbTransThresholdNEW.Enabled = true;
            tbTransDebounceNEW.Enabled = true;
            lblTransThresholdNEW.Enabled = true;
            lblTransThresholdValNEW.Enabled = true;
            lblTransThresholdgNEW.Enabled = true;
            lblTransDebounceNEW.Enabled = true;
            lblTransDebounceValNEW.Enabled = true;
            lblTransDebouncemsNEW.Enabled = true;
        }

        private void btnResetWatermark_Click(object sender, EventArgs e)
        {
            btnWatermark.Enabled = true;
            btnResetWatermark.Enabled = false;
            lblWatermarkValue.Enabled = true;
            tBWatermark.Enabled = true;
            lblWatermark.Enabled = true;
        }

        private void btnSetFirstPulseTimeLimit_Click_1(object sender, EventArgs e)
        {
            VeyronControllerObj.SetPulseFirstTimeLimitFlag = true;
            CreateNewTaskFromGUI(new ControllerReqPacket(), 6, TaskID.SetPulseFirstTimeLimit, new int[] { tbFirstPulseTimeLimit.Value });

            btnSetFirstPulseTimeLimit.Enabled = false;
            btnResetFirstPulseTimeLimit.Enabled = true;
            tbFirstPulseTimeLimit.Enabled = false;
            lblFirstPulseTimeLimit.Enabled = false;
            lblFirstPulseTimeLimitms.Enabled = false;
            lblFirstTimeLimitVal.Enabled = false;
        }

        private void btnSetMode_Click(object sender, EventArgs e)
        {
            if (rdoFill.Checked && rdoFill.Checked)
                FIFOModeValue = 2;

            VeyronControllerObj.SetFIFOModeFlag = true;
			CreateNewTaskFromGUI(new ControllerReqPacket(), 7, TaskID.SetFIFOMode, new int[1] { FIFOModeValue });

            btnSetMode.Enabled = false;
            rdoCircular.Enabled = false;
            rdoFill.Enabled = false;
            rdoTriggerMode.Enabled = false;
            p14b8bSelect.Enabled = false;
            rdoFIFO8bitDataDisplay.Enabled = false;
            rdoFIFO14bitDataDisplay.Enabled = false;
        }

        private void btnSetPLDebounce_Click(object sender, EventArgs e)
        {
            VeyronControllerObj.SetPLDebounceFlag = true;
            CreateNewTaskFromGUI(new ControllerReqPacket(), 4, TaskID.SetPLDebounce, new int[] { tbPL.Value });

            btnSetPLDebounce.Enabled = false;
            btnResetPLDebounce.Enabled = true;
            tbPL.Enabled = false;
            lblPLbounceVal.Enabled = false;
            lblBouncems.Enabled = false;
            lblDebouncePL.Enabled = false;
        }

        private void btnSetPulseThresholds_Click_1(object sender, EventArgs e)
        {
            VeyronControllerObj.SetPulseXThresholdFlag = true;
            CreateNewTaskFromGUI(new ControllerReqPacket(), 6, TaskID.SetPulseXThreshold, new int[] { tbPulseXThreshold.Value });

            VeyronControllerObj.SetPulseYThresholdFlag = true;
			CreateNewTaskFromGUI(new ControllerReqPacket(), 6, TaskID.SetPulseYThreshold, new int[] { tbPulseYThreshold.Value });

            VeyronControllerObj.SetPulseZThresholdFlag = true;
			CreateNewTaskFromGUI(new ControllerReqPacket(), 6, TaskID.SetPulseZThreshold, new int[] { tbPulseZThreshold.Value });

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

        private void btnSetSleepTimer_Click(object sender, EventArgs e)
        {
            VeyronControllerObj.SetSleepTimerFlag = true;
            CreateNewTaskFromGUI(new ControllerReqPacket(), 0, TaskID.SetSleepTimer, new int[] { tbSleepCounter.Value });

            btnSetSleepTimer.Enabled = false;
            lblSleepTimer.Enabled = false;
            lblSleepTimerValue.Enabled = false;
            lblSleepms.Enabled = false;
            btnSleepTimerReset.Enabled = true;
        }

        private void btnSetTransient_Click(object sender, EventArgs e)
        {
            VeyronControllerObj.SetTransThresholdFlag = true;
            CreateNewTaskFromGUI(new ControllerReqPacket(), 5, TaskID.SetTransThreshold, new int[] { tbTransThreshold.Value });

            VeyronControllerObj.SetTransDebounceFlag = true;
            CreateNewTaskFromGUI(new ControllerReqPacket(), 5, TaskID.SetTransDebounce, new int[] { tbTransDebounce.Value });

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

        private void btnSetTransientNEW_Click_1(object sender, EventArgs e)
        {
            VeyronControllerObj.SetTransThresholdNEWFlag = true;
            CreateNewTaskFromGUI(new ControllerReqPacket(), 5, TaskID.SetTransThresholdNEW, new int[] { tbTransThresholdNEW.Value });

            VeyronControllerObj.SetTransDebounceNEWFlag = true;
            CreateNewTaskFromGUI(new ControllerReqPacket(), 5, TaskID.SetTransDebounceNEW, new int[] { tbTransDebounceNEW.Value });

            btnSetTransientNEW.Enabled = false;
            btnResetTransientNEW.Enabled = true;
            tbTransThresholdNEW.Enabled = false;
            tbTransDebounceNEW.Enabled = false;
            lblTransThresholdNEW.Enabled = false;
            lblTransThresholdValNEW.Enabled = false;
            lblTransThresholdgNEW.Enabled = false;
            lblTransDebounceNEW.Enabled = false;
            lblTransDebounceValNEW.Enabled = false;
            lblTransDebouncemsNEW.Enabled = false;
        }

        private void btnSleepTimerReset_Click(object sender, EventArgs e)
        {
            btnSetSleepTimer.Enabled = true;
            lblSleepTimer.Enabled = true;
            lblSleepTimerValue.Enabled = true;
            lblSleepms.Enabled = true;
            btnSleepTimerReset.Enabled = false;
        }

        private void btnUpdateRegs_Click(object sender, EventArgs e)
        {
			CreateNewTaskFromGUI(new ControllerReqPacket(), 1, TaskID.RegisterDump, new int[] { 0 });
        }

        private void btnViewData_Click(object sender, EventArgs e)
        {
            btnViewData.Enabled = false;
            btnDisableData.Enabled = true;
            MainScreenGraph.Enabled = true;

            if (rdo8bitDataMain.Checked)
            {
                VeyronControllerObj.Data8bFlag = true;
                CreateNewTaskFromGUI(new ControllerReqPacket(), 0, 1, new int[] { SystemPolling });
                UpdateMainScreenWaveform(Gees8bData);
            }

            else if (rdoXYZFullResMain.Checked)
            {
                switch (DeviceID)
                {
                    case 2:
                    {
                        VeyronControllerObj.Data14bFlag = true;
						CreateNewTaskFromGUI(new ControllerReqPacket(), 0, TaskID.XYZ14StreamData, new int[] { SystemPolling });
                        UpdateMainScreenWaveform(Gees14bData);
                        break;
                    }
                    case 3:
                    {
                        VeyronControllerObj.Data12bFlag = true;
						CreateNewTaskFromGUI(new ControllerReqPacket(), 0, TaskID.XYZ12StreamData, new int[] { SystemPolling });
                        UpdateMainScreenWaveform(Gees12bData);
                        break;
                    }
                    case 4:
                    {
                        VeyronControllerObj.Data10bFlag = true;
						CreateNewTaskFromGUI(new ControllerReqPacket(), 0, TaskID.XYZ10StreamData, new int[] { SystemPolling });
                        UpdateMainScreenWaveform(Gees10bData);
                        break;
                    }
                }
            }
        }

        private void btnWatermark_Click(object sender, EventArgs e)
        {
            VeyronControllerObj.SetFIFOWatermarkFlag = true;
			CreateNewTaskFromGUI(new ControllerReqPacket(), 7, TaskID.SetFIFOWatermark, new int[] { tBWatermark.Value });

            btnWatermark.Enabled = false;
            btnResetWatermark.Enabled = true;
            lblWatermarkValue.Enabled = false;
            tBWatermark.Enabled = false;
            lblWatermark.Enabled = false;
        }

        private void btnWrite_Click(object sender, EventArgs e)
        {
            VeyronControllerObj.WriteValueFlag = true;
            CreateNewTaskFromGUI(new ControllerReqPacket(), 1, TaskID.WriteValue, new int[] { Convert.ToInt32(txtRegWrite.Text, 0x10), Convert.ToInt32(txtRegValue.Text, 0x10) });
        }

        private void btnWriteCal_Click(object sender, EventArgs e)
        {
            VeyronControllerObj.SetCalFlag = true;
            CreateNewTaskFromGUI(new ControllerReqPacket(), 0, TaskID.SetCal, new int[] { Convert.ToInt32(txtCalX.Text), Convert.ToInt32(txtCalY.Text), Convert.ToInt32(txtCalZ.Text) });
        }

        private void chkAnalogLowNoise_CheckedChanged(object sender, EventArgs e)
        {
            VeyronControllerObj.SetEnableAnalogLowNoiseFlag = true;
			CreateNewTaskFromGUI(new ControllerReqPacket(), 0, TaskID.SetEnableAnalogLowNoise, new int[] { chkAnalogLowNoise.Checked ? 0xff : 0 });
        }

        private void chkDefaultFFSettings1_CheckedChanged_1(object sender, EventArgs e)
        {
            if (chkDefaultFFSettings1.Checked)
            {
                chkDefaultMotion1.Checked = false;
                chkXEFE.Enabled = true;
                chkXEFE.Checked = true;
                chkYEFE.Enabled = true;
                chkYEFE.Checked = true;
                chkZEFE.Enabled = true;
                chkZEFE.Checked = true;

                VeyronControllerObj.SetMFF1XEFEFlag = true;
				CreateNewTaskFromGUI(new ControllerReqPacket(), 3, TaskID.SetMFF1XEFE, new int[] { 0xff });

                VeyronControllerObj.SetMFF1YEFEFlag = true;
				CreateNewTaskFromGUI(new ControllerReqPacket(), 3, TaskID.SetMFF1YEFE, new int[] { 0xff });

                VeyronControllerObj.SetMFF1ZEFEFlag = true;
				CreateNewTaskFromGUI(new ControllerReqPacket(), 3, TaskID.SetMFF1ZEFE, new int[] { 0xff });

                rdoMFF1Or.Checked = false;
                rdoMFF1And.Checked = true;

                VeyronControllerObj.SetMFF1AndOrFlag = true;
				CreateNewTaskFromGUI(new ControllerReqPacket(), 3, TaskID.SetMFF1AndOr, new int[] { 0 });

                double num2 = 0.063;
                tbMFF1Threshold.Value = 4;
                double num = tbMFF1Threshold.Value * num2;
                lblMFF1ThresholdVal.Text = string.Format("{0:F2}", num);

                VeyronControllerObj.SetMFF1ThresholdFlag = true;
                CreateNewTaskFromGUI(new ControllerReqPacket(), 0, 0x38, new int[] { tbMFF1Threshold.Value });

                switch (ddlDataRate.SelectedIndex)
                {
                    case 0:
                        tbMFF1Debounce.Value = 0x40;
                        lblMFF1DebounceVal.Text = string.Format("{0:F2}", 40);
                        break;

                    case 1:
                        tbMFF1Debounce.Value = 0x20;
                        lblMFF1DebounceVal.Text = string.Format("{0:F2}", 40);
                        break;

                    case 2:
                        tbMFF1Debounce.Value = 0x10;
                        lblMFF1DebounceVal.Text = string.Format("{0:F2}", 40);
                        break;

                    case 3:
                        tbMFF1Debounce.Value = 8;
                        lblMFF1DebounceVal.Text = string.Format("{0:F2}", 40);
                        break;

                    case 4:
                        tbMFF1Debounce.Value = 4;
                        lblMFF1DebounceVal.Text = string.Format("{0:F2}", 0);
                        break;

                    case 5:
                        tbMFF1Debounce.Value = 2;
                        lblMFF1DebounceVal.Text = string.Format("{0:F2}", 0);
                        break;

                    default:
                        tbMFF1Debounce.Value = 0;
                        lblMFF1DebounceVal.Text = string.Format("{0:F2}", 0);
                        break;
                }

                VeyronControllerObj.SetMFF1DebounceFlag = true;
				CreateNewTaskFromGUI(new ControllerReqPacket(), 3, TaskID.SetMFF1Debounce, new int[] { tbMFF1Debounce.Value });

                btnMFF1Set.Enabled = false;
                btnMFF1Reset.Enabled = true;
                tbMFF1Threshold.Enabled = false;
                tbMFF1Debounce.Enabled = false;
                lblMFF1Threshold.Enabled = false;
                lblMFF1ThresholdVal.Enabled = false;
                lblMFF1Thresholdg.Enabled = false;
                lblMFF1Debounce.Enabled = false;
                lblMFF1DebounceVal.Enabled = false;
                lblMFF1Debouncems.Enabled = false;
            }
        }

        private void chkDefaultMotion1_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDefaultMotion1.Checked)
            {
                chkDefaultFFSettings1.Checked = false;
                chkXEFE.Checked = true;

                VeyronControllerObj.SetMFF1XEFEFlag = true;
                CreateNewTaskFromGUI(new ControllerReqPacket(), 3, TaskID.SetMFF1XEFE, new int[] { 0xff });
                chkYEFE.Checked = true;

                VeyronControllerObj.SetMFF1YEFEFlag = true;
                CreateNewTaskFromGUI(new ControllerReqPacket(), 3, TaskID.SetMFF1YEFE, new int[] { 0xff });

                chkZEFE.Checked = true;
                VeyronControllerObj.SetMFF1ZEFEFlag = true;
                CreateNewTaskFromGUI(new ControllerReqPacket(), 3, TaskID.SetMFF1ZEFE, new int[] { 0xff });

                rdoMFF1Or.Checked = true;
                rdoMFF1And.Checked = false;
                VeyronControllerObj.SetMFF1AndOrFlag = true;
                CreateNewTaskFromGUI(new ControllerReqPacket(), 0, TaskID.SetMFF1AndOr, new int[] { 0xff });

                double num2 = 0.063;
                tbMFF1Threshold.Value = 0x18;
                double num = tbMFF1Threshold.Value * num2;
                lblMFF1ThresholdVal.Text = string.Format("{0:F3}", num);

                VeyronControllerObj.SetMFF1ThresholdFlag = true;
                CreateNewTaskFromGUI(new ControllerReqPacket(), 3, TaskID.SetMFF1Threshold, new int[] { tbMFF1Threshold.Value });

                tbMFF1Debounce.Value = 0;
                lblMFF1DebounceVal.Text = string.Format("{0:F2}", 0);

                VeyronControllerObj.SetMFF1DebounceFlag = true;
				CreateNewTaskFromGUI(new ControllerReqPacket(), 0, TaskID.SetMFF1Debounce, new int[] { 0 });

                btnMFF1Set.Enabled = false;
                btnMFF1Reset.Enabled = true;
                tbMFF1Threshold.Enabled = false;
                tbMFF1Debounce.Enabled = false;
                lblMFF1Threshold.Enabled = false;
                lblMFF1ThresholdVal.Enabled = false;
                lblMFF1Thresholdg.Enabled = false;
                lblMFF1Debounce.Enabled = false;
                lblMFF1DebounceVal.Enabled = false;
                lblMFF1Debouncems.Enabled = false;
                chkXEFE.Enabled = true;
                chkYEFE.Enabled = true;
                chkZEFE.Enabled = true;
            }
        }

        private void chkDefaultTransSettings_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDefaultTransSettings.Checked)
            {
                chkTransEnableXFlag.Checked = true;

                VeyronControllerObj.SetTransEnableXFlagFlag = true;
                CreateNewTaskFromGUI(new ControllerReqPacket(), 5, 0x27, new int[] { 0xff });

                chkTransEnableYFlag.Checked = true;

                VeyronControllerObj.SetTransEnableYFlagFlag = true;
                CreateNewTaskFromGUI(new ControllerReqPacket(), 5, 40, new int[] { 0xff });

                chkTransEnableZFlag.Checked = true;

                VeyronControllerObj.SetTransEnableZFlagFlag = true;
				CreateNewTaskFromGUI(new ControllerReqPacket(), 5, 0x29, new int[] { 0xff });

                tbTransThreshold.Value = 8;
                double num2 = 0.063;
                double num = tbTransThreshold.Value * num2;
                lblTransThresholdVal.Text = string.Format("{0:F2}", num);

                VeyronControllerObj.SetTransThresholdFlag = true;
                CreateNewTaskFromGUI(new ControllerReqPacket(), 5, 0x24, new int[] { tbTransThreshold.Value });

                tbTransDebounce.Value = 0;
                double num3 = tbTransDebounce.Value;
                lblTransDebouncems.Text = "ms";
                lblTransDebounceVal.Text = string.Format("{0:F1}", num3);

                VeyronControllerObj.SetTransDebounceFlag = true;
                CreateNewTaskFromGUI(new ControllerReqPacket(), 5, 0x25, new int[] { tbTransDebounce.Value });

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

        private void chkDefaultTransSettings1_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDefaultTransSettings1.Checked)
            {
                chkTransEnableXFlagNEW.Checked = true;
                int[] datapassed = new int[] { 0xff };
                VeyronControllerObj.SetTransEnableXFlagNEWFlag = true;
                CreateNewTaskFromGUI(new ControllerReqPacket(), 5, 0x2c, datapassed);
                chkTransEnableYFlagNEW.Checked = true;
                int[] numArray2 = new int[] { 0xff };
                VeyronControllerObj.SetTransEnableYFlagNEWFlag = true;
                CreateNewTaskFromGUI(new ControllerReqPacket(), 5, 0x2d, numArray2);
                chkTransEnableZFlagNEW.Checked = true;
                int[] numArray3 = new int[] { 0xff };
                VeyronControllerObj.SetTransEnableZFlagNEWFlag = true;
                CreateNewTaskFromGUI(new ControllerReqPacket(), 5, 0x2e, numArray3);
                tbTransThresholdNEW.Value = 8;
                double num2 = 0.063;
                double num = tbTransThresholdNEW.Value * num2;
                lblTransThresholdValNEW.Text = string.Format("{0:F2}", num);
                int[] numArray4 = new int[] { tbTransThresholdNEW.Value };
                VeyronControllerObj.SetTransThresholdNEWFlag = true;
                CreateNewTaskFromGUI(new ControllerReqPacket(), 5, 0x30, numArray4);
                tbTransDebounceNEW.Value = 0;
                double num3 = tbTransDebounceNEW.Value;
                lblTransDebouncemsNEW.Text = "ms";
                lblTransDebounceValNEW.Text = string.Format("{0:F1}", num3);
                int[] numArray5 = new int[] { tbTransDebounceNEW.Value };
                VeyronControllerObj.SetTransDebounceNEWFlag = true;
                CreateNewTaskFromGUI(new ControllerReqPacket(), 5, 0x31, numArray5);
                btnSetTransientNEW.Enabled = false;
                btnResetTransientNEW.Enabled = true;
                tbTransThresholdNEW.Enabled = false;
                tbTransDebounceNEW.Enabled = false;
                lblTransThresholdNEW.Enabled = false;
                lblTransThresholdValNEW.Enabled = false;
                lblTransThresholdgNEW.Enabled = false;
                lblTransDebounceNEW.Enabled = false;
                lblTransDebounceValNEW.Enabled = false;
                lblTransDebouncemsNEW.Enabled = false;
            }
        }

        private void chkDisableFIFO_CheckedChanged(object sender, EventArgs e)
        {
            int[] datapassed = new int[1];
            if (chkDisableFIFO.Checked)
            {
                if (rdoStandby.Checked)
                {
                    p14b8bSelect.Enabled = true;
                    rdoFIFO14bitDataDisplay.Enabled = true;
                    rdoFIFO8bitDataDisplay.Enabled = true;
                }
                btnSetMode.Enabled = true;
                rdoFill.Enabled = true;
                rdoTriggerMode.Enabled = true;
                rdoCircular.Enabled = true;
                VeyronControllerObj.ReturnFIFOStatus = false;
                VeyronControllerObj.ReturnPLStatus = false;
                VeyronControllerObj.ReturnTransStatus = false;
                VeyronControllerObj.ReturnMFF1Status = false;
                VeyronControllerObj.ReturnPulseStatus = false;
                int num = 0;
                p4.Enabled = false;
                p5.Enabled = false;
                pTriggerMode.Enabled = false;
                datapassed[0] = num;
                VeyronControllerObj.SetFIFOModeFlag = true;
                CreateNewTaskFromGUI(new ControllerReqPacket(), 7, 0x13, datapassed);
            }
            if (!(chkDisableFIFO.Checked || !rdoStandby.Checked))
            {
                p4.Enabled = true;
                pTriggerMode.Enabled = true;
                p5.Enabled = false;
                p14b8bSelect.Enabled = false;
                rdoFIFO14bitDataDisplay.Enabled = false;
                rdoFIFO8bitDataDisplay.Enabled = false;
            }
            if (!chkDisableFIFO.Checked && rdoActive.Checked)
            {
                btnSetMode.Enabled = false;
                p14b8bSelect.Enabled = false;
                rdoFIFO14bitDataDisplay.Enabled = false;
                rdoFIFO8bitDataDisplay.Enabled = false;
                int[] numArray2 = new int[5];
                numArray2[0] = SystemPolling;
                numArray2[1] = tBWatermark.Value;
                numArray2[2] = FIFODump8orFull;
                numArray2[4] = FullScaleValue;

                if (rdoFill.Checked)
                    numArray2[3] = 0;
                else if (rdoCircular.Checked)
                    numArray2[3] = 1;
				else if (rdoTriggerMode.Checked)
				{
					numArray2[3] = 2;
					if (chkTrigTrans.Checked)
						VeyronControllerObj.ReturnTransStatus = true;
					if (chkTriggerLP.Checked)
						VeyronControllerObj.ReturnPLStatus = true;
					if (chkTriggerMFF.Checked)
						VeyronControllerObj.ReturnMFF1Status = true;
					if (chkTriggerPulse.Checked)
						VeyronControllerObj.ReturnPulseStatus = true;
				}
                VeyronControllerObj.PollingFIFOFlag = true;
                CreateNewTaskFromGUI(new ControllerReqPacket(), 7, 80, numArray2);
                p4.Enabled = true;
                p5.Enabled = true;
                rdoFIFO8bitDataDisplay.Enabled = false;
                rdoFIFO14bitDataDisplay.Enabled = false;
            }
        }

        private void chkEnableASleep_CheckedChanged(object sender, EventArgs e)
        {
            int num;
            pnlAutoSleep.Enabled = true;
            int[] datapassed = new int[1];
            if (chkEnableASleep.Checked)
            {
                pnlAutoSleep.Enabled = true;
                num = 0xff;
                btnSetSleepTimer.Enabled = true;
                lblSleepTimer.Enabled = true;
                lblSleepTimerValue.Enabled = true;
                lblSleepms.Enabled = true;
            }
            else
            {
                pnlAutoSleep.Enabled = false;
                num = 0;
            }
            datapassed[0] = num;
            VeyronControllerObj.SetEnableASleepFlag = true;
            CreateNewTaskFromGUI(new ControllerReqPacket(), 0, 7, datapassed);
        }

        private void chkEnablePL_CheckedChanged(object sender, EventArgs e)
        {
            int num;
            if (chkEnablePL.Checked)
            {
                chkPLDefaultSettings.Enabled = true;
                gbPLDisappear.Enabled = true;
            }
            else
            {
                chkPLDefaultSettings.Enabled = false;
                gbPLDisappear.Enabled = false;
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
            VeyronControllerObj.SetEnablePLFlag = true;
            CreateNewTaskFromGUI(new ControllerReqPacket(), 4, 0x19, datapassed);
        }

		private void chkEnableX_CheckedChanged(object sender, EventArgs e)
		{
			WaveformPlotCollection plots = MainScreenGraph.Plots;
			plots[0].Visible = (chkEnableX.Checked);
		}

		private void chkEnableXMFF_CheckedChanged(object sender, EventArgs e)
		{
			WaveformPlotCollection plots = MFFGraph.Plots;
			plots[0].Visible = (chkEnableXMFF.Checked);
		}

		private void chkEnableY_CheckedChanged(object sender, EventArgs e)
		{
			WaveformPlotCollection plots = MainScreenGraph.Plots;
			plots[1].Visible = (chkEnableY.Checked);
		}

		private void chkEnableYMFF_CheckedChanged(object sender, EventArgs e)
		{
			WaveformPlotCollection plots = MFFGraph.Plots;
			plots[1].Visible = (chkEnableYMFF.Checked);
		}

        private void chkEnableZ_CheckedChanged(object sender, EventArgs e)
        {
            WaveformPlotCollection plots = MainScreenGraph.Plots;
            if (chkEnableZ.Checked)
                plots[2].Visible = true;
            else
                plots[2].Visible = false;
        }

		private void chkEnableZMFF_CheckedChanged(object sender, EventArgs e)
		{
			WaveformPlotCollection plots = MFFGraph.Plots;
			if (chkEnableZMFF.Checked)
				plots[2].Visible = true;
			else
				plots[2].Visible = false;
		}

        private void chkEnIntASleep_CheckedChanged(object sender, EventArgs e)
        {
            VeyronControllerObj.SetIntsEnableFlag = true;
			CreateNewTaskFromGUI(new ControllerReqPacket(), 0, TaskID.SetIntsEnable, new int[2] { chkEnIntASleep.Checked ? 0xff : 0, 0x80 });
        }

        private void chkEnIntDR_CheckedChanged(object sender, EventArgs e)
        {
            VeyronControllerObj.SetIntsEnableFlag = true;
			CreateNewTaskFromGUI(new ControllerReqPacket(), 0, TaskID.SetIntsEnable, new int[2] { chkEnIntDR.Checked ? 0xff : 0, 1 });
        }

        private void chkEnIntFIFO_CheckedChanged(object sender, EventArgs e)
        {
            VeyronControllerObj.SetIntsEnableFlag = true;
			CreateNewTaskFromGUI(new ControllerReqPacket(), 0, TaskID.SetIntsEnable, new int[2] { chkEnIntFIFO.Checked ? 0xff : 0, 0x40 });
        }

        private void chkEnIntMFF1_CheckedChanged(object sender, EventArgs e)
        {
            VeyronControllerObj.SetIntsEnableFlag = true;
            CreateNewTaskFromGUI(new ControllerReqPacket(), 0, TaskID.SetIntsEnable, new int[2]{chkEnIntMFF1.Checked ? 0xff : 0, 4});
        }

        private void chkEnIntPL_CheckedChanged(object sender, EventArgs e)
        {
            VeyronControllerObj.SetIntsEnableFlag = true;
			CreateNewTaskFromGUI(new ControllerReqPacket(), 0, TaskID.SetIntsEnable, new int[2] { chkEnIntPL.Checked ? 0xff : 0, 0x10 });
        }

        private void chkEnIntPulse_CheckedChanged(object sender, EventArgs e)
        {
            VeyronControllerObj.SetIntsEnableFlag = true;
			CreateNewTaskFromGUI(new ControllerReqPacket(), 0, TaskID.SetIntsEnable, new int[2] { chkEnIntPulse.Checked ? 0xff : 0, 8 });
        }

        private void chkEnIntTrans_CheckedChanged(object sender, EventArgs e)
        {
            VeyronControllerObj.SetIntsEnableFlag = true;
			CreateNewTaskFromGUI(new ControllerReqPacket(), 0, TaskID.SetIntsEnable, new int[2] { chkEnIntTrans.Checked ? 0xff : 0, 0x20 });
        }

        private void chkEnIntTrans1_CheckedChanged(object sender, EventArgs e)
        {
            VeyronControllerObj.SetIntsEnableFlag = true;
			CreateNewTaskFromGUI(new ControllerReqPacket(), 0, TaskID.SetIntsEnable, new int[2] { chkEnIntTrans1.Checked ? 0xff : 0, 2 });
        }

        private void chkHPFDataOut_CheckedChanged(object sender, EventArgs e)
        {
            VeyronControllerObj.SetHPFDataOutFlag = true;
            ControllerReqPacket reqName = new ControllerReqPacket();
			CreateNewTaskFromGUI(reqName, 0, TaskID.SetHPFDataOut, new int[1] { chkHPFDataOut.Checked ? 0xff : 0 });
            if (chkHPFDataOut.Checked)
            {
                lblMotionDataType.Text = "HPF Data Out";
                lblTransDataType.Text = "HPF Data Out";
            }
            else
            {
                lblMotionDataType.Text = "LPF Data Out";
                lblTransDataType.Text = "LPF Data Out";
            }
        }

        private void chkMFF1EnableLatch_CheckedChanged(object sender, EventArgs e)
        {
            int num = chkMFF1EnableLatch.Checked ? 0xff : 0;
            int[] datapassed = new int[] { num };
            VeyronControllerObj.SetMFF1LatchFlag = true;
            ControllerReqPacket reqName = new ControllerReqPacket();
            CreateNewTaskFromGUI(reqName, 3, TaskID.SetMFF1Latch, datapassed);
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
                ddlZLockA.SelectedIndex = 4;
                ddlBFTripA.SelectedIndex = 1;
                ddlPLTripA.SelectedIndex = 5;
                ddlHysteresisA.SelectedIndex = 4;

                VeyronControllerObj.SetZLockAFlag = true;
                CreateNewTaskFromGUI(new ControllerReqPacket(), 4, TaskID.SetZLockA, new int[] { ddlZLockA.SelectedIndex });
                ddlZLockA.Enabled = false;

                VeyronControllerObj.SetBFTripAFlag = true;
                CreateNewTaskFromGUI(new ControllerReqPacket(), 4, TaskID.SetBFTripA, new int[] { ddlBFTripA.SelectedIndex });
                ddlBFTripA.Enabled = false;

                PL_index = ddlPLTripA.SelectedIndex;
                VeyronControllerObj.SetPLTripAFlag = true;
                CreateNewTaskFromGUI(new ControllerReqPacket(), 4, TaskID.SetPLTripA, new int[1]{PL_index});
                ddlPLTripA.Enabled = false;

                Hysteresis_index = ddlHysteresisA.SelectedIndex;
                VeyronControllerObj.SetHysteresisFlag = true;
                CreateNewTaskFromGUI(new ControllerReqPacket(), 4, TaskID.SetHysteresis, new int[1]{Hysteresis_index});
                ddlHysteresisA.Enabled = false;
                switch (ddlDataRate.SelectedIndex)
                {
                    case 0:
                        tbPL.Value = 13;
                        break;

                    case 1:
                        tbPL.Value = 6;
                        break;

                    case 2:
                        tbPL.Value = 3;
                        break;

                    case 3:
                        tbPL.Value = 1;
                        break;

                    default:
                        tbPL.Value = 0;
                        break;
                }
                double num = tbPL.Value * DR_timestep;
                lblBouncems.Text = "ms";
                lblPLbounceVal.Text = string.Format("{0:F1}", num);

				VeyronControllerObj.SetPLDebounceFlag = true;
                CreateNewTaskFromGUI(new ControllerReqPacket(), 4, TaskID.SetPLDebounce, new int[] { tbPL.Value });

                btnSetPLDebounce.Enabled = false;
                btnResetPLDebounce.Enabled = true;
                tbPL.Enabled = false;
                lblPLbounceVal.Enabled = false;
                lblBouncems.Enabled = false;
                lblDebouncePL.Enabled = false;
            }
        }

        private void chkPulseEnableLatch_CheckedChanged_1(object sender, EventArgs e)
        {
            VeyronControllerObj.SetPulseLatchFlag = true;
            CreateNewTaskFromGUI(new ControllerReqPacket(), 6, TaskID.SetPulseLatch, new int[] { chkPulseEnableLatch.Checked ? 0xff : 0 });
        }

        private void chkPulseEnableXDP_CheckedChanged_1(object sender, EventArgs e)
        {
            VeyronControllerObj.SetPulseXDPFlag = true;
            CreateNewTaskFromGUI(new ControllerReqPacket(), 6, TaskID.SetPulseXDP, new int[] { chkPulseEnableXDP.Checked ? 0xff : 0 });
        }

        private void chkPulseEnableXSP_CheckedChanged_1(object sender, EventArgs e)
        {
            VeyronControllerObj.SetPulseXSPFlag = true;
			CreateNewTaskFromGUI(new ControllerReqPacket(), 6, TaskID.SetPulseXSP, new int[] { chkPulseEnableXSP.Checked ? 0xff : 0 });
        }

        private void chkPulseEnableYDP_CheckedChanged_1(object sender, EventArgs e)
        {
            VeyronControllerObj.SetPulseYDPFlag = true;
			CreateNewTaskFromGUI(new ControllerReqPacket(), 6, TaskID.SetPulseYDP, new int[] { chkPulseEnableYDP.Checked ? 0xff : 0 });
		}

        private void chkPulseEnableYSP_CheckedChanged_1(object sender, EventArgs e)
        {
            VeyronControllerObj.SetPulseYSPFlag = true;
			CreateNewTaskFromGUI(new ControllerReqPacket(), 6, TaskID.SetPulseYSP, new int[] { chkPulseEnableYSP.Checked ? 0xff : 0 });
		}

        private void chkPulseEnableZDP_CheckedChanged(object sender, EventArgs e)
        {
            VeyronControllerObj.SetPulseZDPFlag = true;
			CreateNewTaskFromGUI(new ControllerReqPacket(), 6, TaskID.SetPulseZDP, new int[] { chkPulseEnableZDP.Checked ? 0xff : 0 });
		}

        private void chkPulseEnableZSP_CheckedChanged_1(object sender, EventArgs e)
        {
            VeyronControllerObj.SetPulseZSPFlag = true;
			CreateNewTaskFromGUI(new ControllerReqPacket(), 6, TaskID.SetPulseZSP, new int[] { chkPulseEnableZSP.Checked ? 0xff : 0 });
		}

        private void chkPulseHPFBypass_CheckedChanged(object sender, EventArgs e)
        {
            VeyronControllerObj.SetPulseHPFBypassFlag = true;
			CreateNewTaskFromGUI(new ControllerReqPacket(), 6, TaskID.SetPulseHPFBypass, new int[] { chkPulseHPFBypass.Checked ? 0xff : 0 });
		}

        private void chkPulseIgnorLatentPulses_CheckedChanged_1(object sender, EventArgs e)
        {
            VeyronControllerObj.SetPulseDPAFlag = true;
			CreateNewTaskFromGUI(new ControllerReqPacket(), 6, TaskID.SetPulseDPA, new int[] { chkPulseIgnorLatentPulses.Checked ? 0xff : 0 });
		}

        private void chkPulseLPFEnable_CheckedChanged(object sender, EventArgs e)
        {
            if (!chkPulseLPFEnable.Checked)
            {
                rdoDefaultSP.Checked = false;
                rdoDefaultSPDP.Checked = false;
            }
            VeyronControllerObj.SetPulseLPFEnableFlag = true;
			CreateNewTaskFromGUI(new ControllerReqPacket(), 6, TaskID.SetPulseLPFEnable, new int[1] { chkPulseLPFEnable.Checked ? 0xff : 0 });

			pulse_step = (chkPulseLPFEnable.Checked) ? DR_timestep : DR_PulseTimeStepNoLPF;

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

        private void chkSelfTest_CheckedChanged(object sender, EventArgs e)
        {
            VeyronControllerObj.SetSelfTestFlag = true;
			CreateNewTaskFromGUI(new ControllerReqPacket(), 10, TaskID.SetSelfTest, new int[1] { chkSelfTest.Checked ? 0xff : 0 });
        }

        private void chkTransBypassHPF_CheckedChanged(object sender, EventArgs e)
        {
            VeyronControllerObj.SetTransBypassHPFFlag = true;
			CreateNewTaskFromGUI(new ControllerReqPacket(), 5, TaskID.SetTransBypassHPF, new int[1] { chkTransBypassHPF.Checked ? 0xff : 0 });
        }

        private void chkTransBypassHPFNEW_CheckedChanged(object sender, EventArgs e)
        {
            VeyronControllerObj.SetTransBypassHPFNEWFlag = true;
			CreateNewTaskFromGUI(new ControllerReqPacket(), 5, TaskID.SetTransBypassHPFNEW, new int[1] { chkTransBypassHPFNEW.Checked ? 0xff : 0 });
		}

        private void chkTransEnableLatch_CheckedChanged(object sender, EventArgs e)
        {
            VeyronControllerObj.SetTransEnableLatchFlag = true;
			CreateNewTaskFromGUI(new ControllerReqPacket(), 5, TaskID.SetTransEnableLatch, new int[1] { chkTransEnableLatch.Checked ? 0xff : 0 });
        }

        private void chkTransEnableLatchNEW_CheckedChanged_1(object sender, EventArgs e)
        {
            VeyronControllerObj.SetTransEnableLatchNEWFlag = true;
			CreateNewTaskFromGUI(new ControllerReqPacket(), 5, TaskID.SetTransEnableLatchNEW, new int[1] { chkTransEnableLatchNEW.Checked ? 0xff : 0 });
        }

        private void chkTransEnableXFlag_CheckedChanged_1(object sender, EventArgs e)
        {
            VeyronControllerObj.SetTransEnableXFlagFlag = true;
			CreateNewTaskFromGUI(new ControllerReqPacket(), 5, TaskID.SetTransEnableXFlag, new int[1] { chkTransEnableXFlag.Checked ? 0xff : 0 });
		}

        private void chkTransEnableXFlagNEW_CheckedChanged_1(object sender, EventArgs e)
        {
            VeyronControllerObj.SetTransEnableXFlagNEWFlag = true;
			CreateNewTaskFromGUI(new ControllerReqPacket(), 5, TaskID.SetTransEnableXFlagNEW, new int[1] { chkTransEnableXFlagNEW.Checked ? 0xff : 0 });
		}

        private void chkTransEnableYFlag_CheckedChanged(object sender, EventArgs e)
        {
            VeyronControllerObj.SetTransEnableYFlagFlag = true;
			CreateNewTaskFromGUI(new ControllerReqPacket(), 5, TaskID.SetTransEnableYFlag, new int[1] { chkTransEnableYFlag.Checked ? 0xff : 0 });
		}

        private void chkTransEnableYFlagNEW_CheckedChanged_1(object sender, EventArgs e)
        {
            VeyronControllerObj.SetTransEnableYFlagNEWFlag = true;
			CreateNewTaskFromGUI(new ControllerReqPacket(), 5, TaskID.SetTransEnableYFlagNEW, new int[1] { chkTransEnableYFlagNEW.Checked ? 0xff : 0 });
		}

        private void chkTransEnableZFlag_CheckedChanged(object sender, EventArgs e)
        {
            VeyronControllerObj.SetTransEnableZFlagFlag = true;
			CreateNewTaskFromGUI(new ControllerReqPacket(), 5, TaskID.SetTransEnableZFlag, new int[1] { chkTransEnableZFlag.Checked ? 0xff : 0 });
		}

        private void chkTransEnableZFlagNEW_CheckedChanged_1(object sender, EventArgs e)
        {
            VeyronControllerObj.SetTransEnableZFlagNEWFlag = true;
			CreateNewTaskFromGUI(new ControllerReqPacket(), 5, TaskID.SetTransEnableZFlagNEW, new int[1] { chkTransEnableZFlagNEW.Checked ? 0xff : 0 });
		}

        private void chkTriggerLP_CheckedChanged(object sender, EventArgs e)
        {
            VeyronControllerObj.SetTrigLPFlag = true;
			CreateNewTaskFromGUI(new ControllerReqPacket(), 7, TaskID.SetTrigLP, new int[1] { chkTriggerLP.Checked ? 0xff : 0 });
		}

        private void chkTriggerMFF_CheckedChanged(object sender, EventArgs e)
        {
            VeyronControllerObj.SetTrigMFFFlag = true;
			CreateNewTaskFromGUI(new ControllerReqPacket(), 7, TaskID.SetTrigMFF, new int[1] { chkTriggerMFF.Checked ? 0xff : 0 });
        }

        private void chkTriggerPulse_CheckedChanged(object sender, EventArgs e)
        {
            VeyronControllerObj.SetTrigPulseFlag = true;
			CreateNewTaskFromGUI(new ControllerReqPacket(), 7, TaskID.SetTrigPulse, new int[1] { chkTriggerPulse.Checked ? 0xff : 0 });
        }

        private void chkTrigTrans_CheckedChanged(object sender, EventArgs e)
        {
            VeyronControllerObj.SetTrigTransFlag = true;
			CreateNewTaskFromGUI(new ControllerReqPacket(), 7, TaskID.SetTrigTrans, new int[1] { chkTrigTrans.Checked ? 0xff : 0 });
        }

        private void chkWakeFIFOGate_CheckedChanged(object sender, EventArgs e)
        {
            VeyronControllerObj.SetWakeFromSleepFlag = true;
			CreateNewTaskFromGUI(new ControllerReqPacket(), 0, TaskID.SetWakeFromSleep, new int[2] { chkWakeFIFOGate.Checked ? 0xff : 0, 0x80 });
        }

        private void chkWakeLP_CheckedChanged(object sender, EventArgs e)
        {
            VeyronControllerObj.SetWakeFromSleepFlag = true;
			CreateNewTaskFromGUI(new ControllerReqPacket(), 0, TaskID.SetWakeFromSleep, new int[2] { chkWakeLP.Checked ? 0xff : 0, 0x20 });
        }

        private void chkWakeMFF1_CheckedChanged(object sender, EventArgs e)
        {
            VeyronControllerObj.SetWakeFromSleepFlag = true;
			CreateNewTaskFromGUI(new ControllerReqPacket(), 0, TaskID.SetWakeFromSleep, new int[2] { chkWakeMFF1.Checked ? 0xff : 0, 0x08 });
		}

        private void chkWakePulse_CheckedChanged(object sender, EventArgs e)
        {
            VeyronControllerObj.SetWakeFromSleepFlag = true;
			CreateNewTaskFromGUI(new ControllerReqPacket(), 0, TaskID.SetWakeFromSleep, new int[2] { chkWakePulse.Checked ? 0xff : 0, 0x10 });
        }

        private void chkWakeTrans_CheckedChanged(object sender, EventArgs e)
        {
            VeyronControllerObj.SetWakeFromSleepFlag = true;
			CreateNewTaskFromGUI(new ControllerReqPacket(), 0, TaskID.SetWakeFromSleep, new int[2] { chkWakeTrans.Checked ? 0xff : 0, 0x40 });
        }

        private void chkWakeTrans1_CheckedChanged(object sender, EventArgs e)
        {
            VeyronControllerObj.SetWakeFromSleepFlag = true;
			CreateNewTaskFromGUI(new ControllerReqPacket(), 0, TaskID.SetWakeFromSleep, new int[2] { chkWakeTrans1.Checked ? 0xff : 0, 0x04 });
        }

        private void chkXEFE_CheckedChanged(object sender, EventArgs e)
        {
            VeyronControllerObj.SetMFF1XEFEFlag = true;
			CreateNewTaskFromGUI(new ControllerReqPacket(), 3, TaskID.SetMFF1XEFE, new int[] { chkXEFE.Checked ? 0xff : 0 });
        }

		private void chkXEnableTrans_CheckedChanged(object sender, EventArgs e)
		{
			WaveformPlotCollection plots = gphXYZ.Plots;
			plots[0].Visible = (chkXEnableTrans.Checked);
		}

        private void chkYEFE_CheckedChanged(object sender, EventArgs e)
        {
            VeyronControllerObj.SetMFF1YEFEFlag = true;
			CreateNewTaskFromGUI(new ControllerReqPacket(), 3, TaskID.SetMFF1YEFE, new int[] { chkYEFE.Checked ? 0xff : 0 });
        }

		private void chkYEnableTrans_CheckedChanged(object sender, EventArgs e)
		{
			WaveformPlotCollection plots = gphXYZ.Plots;
			plots[1].Visible = (chkYEnableTrans.Checked);
		}

        private void chkZEFE_CheckedChanged(object sender, EventArgs e)
        {
            VeyronControllerObj.SetMFF1ZEFEFlag = true;
			CreateNewTaskFromGUI(new ControllerReqPacket(), 3, TaskID.SetMFF1ZEFE, new int[] { chkZEFE.Checked ? 0xff : 0 });
        }

		private void chkZEnableTrans_CheckedChanged(object sender, EventArgs e)
		{
			WaveformPlotCollection plots = gphXYZ.Plots;
			plots[2].Visible = (chkZEnableTrans.Checked);
		}

        private void CommCallback(object o, CommEvent cmd)
        {
        }

        private void CommStripButton_Click(object sender, EventArgs e)
        {
        }

        private void ControllerObj_ControllerEvents(ControllerEventType evt, object o)
        {
            XYZGees gees = new XYZGees();
            switch ((int) evt)
            {
                case 1:
                    UpdateRaw8bVariables(o);
                    break;

                case 2:
                    switch (DeviceID)
                    {
                        case 2:
                            UpdateRaw14bVariables(o);
                            return;

                        case 3:
                            UpdateRaw12bVariables(o);
                            return;

                        case 4:
                            UpdateRaw10bVariables(o);
                            return;
                    }
                    break;

                case 4:
                    GuiQueue.Enqueue((GUIUpdatePacket) o);
                    break;

                case 7:
                    GuiQueue.Enqueue((GUIUpdatePacket) o);
                    break;

                case 9:
                    GuiQueue.Enqueue((GUIUpdatePacket) o);
                    break;
            }
        }

		private void CreateNewTaskFromGUI(ControllerReqPacket request, int FormNum, int taskId, int[] data)
		{
			request.FormID = FormNum;
			request.TaskID = taskId;
			request.PayLoad.Enqueue(data);
			CntrlQueue.Enqueue(request);
		}
        private void CreateNewTaskFromGUI(ControllerReqPacket request, int formNum, TaskID taskId, int[] data)
        {
            request.FormID = formNum;
            request.TaskID = (int)taskId;
            request.PayLoad.Enqueue(data);
            CntrlQueue.Enqueue(request);
        }

        private void ddlBFTripA_SelectedIndexChanged(object sender, EventArgs e)
        {
            VeyronControllerObj.SetBFTripAFlag = true;
			CreateNewTaskFromGUI(new ControllerReqPacket(), 4, TaskID.SetBFTripA, new int[] { ddlBFTripA.SelectedIndex });
        }

        private void ddlDataRate_SelectedIndexChanged(object sender, EventArgs e)
        {
            double num2;
            rdoDefaultSP.Checked = false;
            rdoDefaultSPDP.Checked = false;
            chkPLDefaultSettings.Checked = false;

            VeyronControllerObj.SetDataRateFlag = true;
			CreateNewTaskFromGUI(new ControllerReqPacket(), 0, TaskID.SetDataRate, new int[] { ddlDataRate.SelectedIndex + 2 });

            int selectedIndex = ddlHPFilter.SelectedIndex;
            tbSleepCounter_Scroll(this, null);
            switch (ddlDataRate.SelectedIndex)
            {
                case 0:
					switch (WakeOSMode)
					{
						case 2:
							DR_timestep = 2.5;
							DR_PulseTimeStepNoLPF = 0.625;
							ddlHPFilter.Items.Clear();
							ddlHPFilter.Items.Add("16Hz");
							ddlHPFilter.Items.Add("8Hz");
							ddlHPFilter.Items.Add("4Hz");
							ddlHPFilter.Items.Add("2Hz");
							ddlHPFilter.SelectedIndex = selectedIndex;
							break;

						case 3:
							DR_timestep = 5.0;
							DR_PulseTimeStepNoLPF = 2.5;
							ddlHPFilter.Items.Clear();
							ddlHPFilter.Items.Add("4Hz");
							ddlHPFilter.Items.Add("2Hz");
							ddlHPFilter.Items.Add("1Hz");
							ddlHPFilter.Items.Add("0.5Hz");
							ddlHPFilter.SelectedIndex = selectedIndex;
							break;
						default:
							DR_timestep = 5.0;
							DR_PulseTimeStepNoLPF = 1.25;
							ddlHPFilter.Items.Clear();
							ddlHPFilter.Items.Add("8Hz");
							ddlHPFilter.Items.Add("4Hz");
							ddlHPFilter.Items.Add("2Hz");
							ddlHPFilter.Items.Add("1Hz");
							ddlHPFilter.SelectedIndex = selectedIndex;
							break;
					}
					break;

                case 1:
					switch (WakeOSMode)
					{
						case 2:
							DR_timestep = 2.5;
							DR_PulseTimeStepNoLPF = 0.625;
							ddlHPFilter.Items.Clear();
							ddlHPFilter.Items.Add("16Hz");
							ddlHPFilter.Items.Add("8Hz");
							ddlHPFilter.Items.Add("4Hz");
							ddlHPFilter.Items.Add("2Hz");
							ddlHPFilter.SelectedIndex = selectedIndex;
							break;

						case 3:
							DR_timestep = 10.0;
							DR_PulseTimeStepNoLPF = 5.0;
							ddlHPFilter.Items.Clear();
							ddlHPFilter.Items.Add("2Hz");
							ddlHPFilter.Items.Add("1Hz");
							ddlHPFilter.Items.Add("0.5Hz");
							ddlHPFilter.Items.Add("0.25Hz");
							ddlHPFilter.SelectedIndex = selectedIndex;
							break;
						default:
							DR_timestep = 10.0;
							DR_PulseTimeStepNoLPF = 2.5;
							ddlHPFilter.Items.Clear();
							ddlHPFilter.Items.Add("4Hz");
							ddlHPFilter.Items.Add("2Hz");
							ddlHPFilter.Items.Add("1Hz");
							ddlHPFilter.Items.Add("0.5Hz");
							ddlHPFilter.SelectedIndex = selectedIndex;
							break;
					}
					break;

                case 2:
                    switch (WakeOSMode)
                    {
                        case 2:
                            DR_timestep = 2.5;
                            DR_PulseTimeStepNoLPF = 0.625;
                            ddlHPFilter.Items.Clear();
                            ddlHPFilter.Items.Add("16Hz");
                            ddlHPFilter.Items.Add("8Hz");
                            ddlHPFilter.Items.Add("4Hz");
                            ddlHPFilter.Items.Add("2Hz");
                            ddlHPFilter.SelectedIndex = selectedIndex;
							break;

                        case 3:
                            DR_timestep = 20.0;
                            DR_PulseTimeStepNoLPF = 10.0;
                            ddlHPFilter.Items.Clear();
                            ddlHPFilter.Items.Add("1Hz");
                            ddlHPFilter.Items.Add("0.5Hz");
                            ddlHPFilter.Items.Add("0.25Hz");
                            ddlHPFilter.Items.Add("0.125Hz");
                            ddlHPFilter.SelectedIndex = selectedIndex;
							break;
						default:
							DR_timestep = 20.0;
							DR_PulseTimeStepNoLPF = 5.0;
							ddlHPFilter.Items.Clear();
							ddlHPFilter.Items.Add("2Hz");
							ddlHPFilter.Items.Add("1Hz");
							ddlHPFilter.Items.Add("0.5Hz");
							ddlHPFilter.Items.Add("0.25Hz");
							ddlHPFilter.SelectedIndex = selectedIndex;
							break;
					}
                    break;

                case 3:
                    DR_timestep = 80.0;
                    switch (WakeOSMode)
                    {
                        case 0:
                            DR_timestep = 20.0;
                            DR_PulseTimeStepNoLPF = 5.0;
                            ddlHPFilter.Items.Clear();
                            ddlHPFilter.Items.Add("2Hz");
                            ddlHPFilter.Items.Add("1Hz");
                            ddlHPFilter.Items.Add("0.5Hz");
                            ddlHPFilter.Items.Add("0.25Hz");
                            ddlHPFilter.SelectedIndex = selectedIndex;
							break;

                        case 1:
                            DR_timestep = 80.0;
                            DR_PulseTimeStepNoLPF = 20.0;
                            ddlHPFilter.Items.Clear();
                            ddlHPFilter.Items.Add("0.5Hz");
                            ddlHPFilter.Items.Add("0.25Hz");
                            ddlHPFilter.Items.Add("0.125Hz");
                            ddlHPFilter.Items.Add("0.063Hz");
                            ddlHPFilter.SelectedIndex = selectedIndex;
							break;

                        case 2:
                            DR_timestep = 2.5;
                            DR_PulseTimeStepNoLPF = 0.625;
                            ddlHPFilter.Items.Clear();
                            ddlHPFilter.Items.Add("16Hz");
                            ddlHPFilter.Items.Add("8Hz");
                            ddlHPFilter.Items.Add("4Hz");
                            ddlHPFilter.Items.Add("2Hz");
                            ddlHPFilter.SelectedIndex = selectedIndex;
							break;

                        case 3:
                            DR_timestep = 80.0;
                            DR_PulseTimeStepNoLPF = 40.0;
                            ddlHPFilter.Items.Clear();
                            ddlHPFilter.Items.Add("0.25Hz");
                            ddlHPFilter.Items.Add("0.125Hz");
                            ddlHPFilter.Items.Add("0.063Hz");
                            ddlHPFilter.Items.Add("0.031Hz");
                            ddlHPFilter.SelectedIndex = selectedIndex;
							break;
                    }
                    break;

                case 4:
                    switch (WakeOSMode)
                    {
                        case 0:
                            DR_timestep = 20.0;
                            DR_PulseTimeStepNoLPF = 5.0;
                            ddlHPFilter.Items.Clear();
                            ddlHPFilter.Items.Add("2Hz");
                            ddlHPFilter.Items.Add("1Hz");
                            ddlHPFilter.Items.Add("0.5Hz");
                            ddlHPFilter.Items.Add("0.25Hz");
                            ddlHPFilter.SelectedIndex = selectedIndex;
							break;

                        case 1:
                            DR_timestep = 160.0;
                            DR_PulseTimeStepNoLPF = 40.0;
                            ddlHPFilter.Items.Clear();
                            ddlHPFilter.Items.Add("0.25Hz");
                            ddlHPFilter.Items.Add("0.125Hz");
                            ddlHPFilter.Items.Add("0.063Hz");
                            ddlHPFilter.Items.Add("0.031Hz");
                            ddlHPFilter.SelectedIndex = selectedIndex;
							break;

                        case 2:
                            DR_timestep = 2.5;
                            DR_PulseTimeStepNoLPF = 0.625;
                            ddlHPFilter.Items.Clear();
                            ddlHPFilter.Items.Add("16Hz");
                            ddlHPFilter.Items.Add("8Hz");
                            ddlHPFilter.Items.Add("4Hz");
                            ddlHPFilter.Items.Add("2Hz");
                            ddlHPFilter.SelectedIndex = selectedIndex;
							break;

                        case 3:
                            DR_timestep = 160.0;
                            DR_PulseTimeStepNoLPF = 80.0;
                            ddlHPFilter.Items.Clear();
                            ddlHPFilter.Items.Add("0.125Hz");
                            ddlHPFilter.Items.Add("0.063Hz");
                            ddlHPFilter.Items.Add("0.031Hz");
                            ddlHPFilter.Items.Add("0.016Hz");
                            ddlHPFilter.SelectedIndex = selectedIndex;
							break;
                    }
                    break;

                case 5:
                    switch (WakeOSMode)
                    {
                        case 0:
                            DR_timestep = 20.0;
                            DR_PulseTimeStepNoLPF = 5.0;
                            ddlHPFilter.Items.Clear();
                            ddlHPFilter.Items.Add("2Hz");
                            ddlHPFilter.Items.Add("1Hz");
                            ddlHPFilter.Items.Add("0.5Hz");
                            ddlHPFilter.Items.Add("0.25Hz");
                            ddlHPFilter.SelectedIndex = selectedIndex;
							break;

                        case 1:
                            DR_timestep = 640.0;
                            DR_PulseTimeStepNoLPF = 160.0;
                            ddlHPFilter.Items.Clear();
                            ddlHPFilter.Items.Add("0.063Hz");
                            ddlHPFilter.Items.Add("0.031Hz");
                            ddlHPFilter.Items.Add("0.016Hz");
                            ddlHPFilter.Items.Add("0.008Hz");
                            ddlHPFilter.SelectedIndex = selectedIndex;
							break;

                        case 2:
                            DR_timestep = 2.5;
                            DR_PulseTimeStepNoLPF = 0.625;
                            ddlHPFilter.Items.Clear();
                            ddlHPFilter.Items.Add("16Hz");
                            ddlHPFilter.Items.Add("8Hz");
                            ddlHPFilter.Items.Add("4Hz");
                            ddlHPFilter.Items.Add("2Hz");
                            ddlHPFilter.SelectedIndex = selectedIndex;
							break;

                        case 3:
                            DR_timestep = 640.0;
                            DR_PulseTimeStepNoLPF = 320.0;
                            ddlHPFilter.Items.Clear();
                            ddlHPFilter.Items.Add("0.031Hz");
                            ddlHPFilter.Items.Add("0.016Hz");
                            ddlHPFilter.Items.Add("0.008Hz");
                            ddlHPFilter.Items.Add("0.004Hz");
                            ddlHPFilter.SelectedIndex = selectedIndex;
							break;
                    }
                    break;
            }

            num2 = tbMFF1Debounce.Value * DR_timestep;
            if (num2 > 1000.0)
            {
                lblMFF1Debouncems.Text = "s";
                num2 /= 1000.0;
                lblMFF1DebounceVal.Text = string.Format("{0:F2}", num2);
            }
            else
            {
                lblMFF1Debouncems.Text = "ms";
                lblMFF1DebounceVal.Text = string.Format("{0:F2}", num2);
            }
            double num3 = tbPL.Value * DR_timestep;
            if (num3 > 1000.0)
            {
                lblBouncems.Text = "s";
                num3 /= 1000.0;
                lblPLbounceVal.Text = string.Format("{0:F4}", num3);
            }
            else
            {
                lblBouncems.Text = "ms";
                lblPLbounceVal.Text = string.Format("{0:F2}", num3);
            }
            double num4 = tbTransDebounce.Value * DR_timestep;
            if (num4 > 1000.0)
            {
                lblTransDebouncems.Text = "s";
                num4 /= 1000.0;
                lblTransDebounceVal.Text = string.Format("{0:F2}", num4);
            }
            else
            {
                lblTransDebouncems.Text = "ms";
                lblTransDebounceVal.Text = string.Format("{0:F2}", num4);
            }
            if (chkPulseLPFEnable.Checked)
            {
                pulse_step = DR_timestep;
            }
            else
            {
                pulse_step = DR_PulseTimeStepNoLPF;
            }
            double num5 = tbFirstPulseTimeLimit.Value * pulse_step;
            if (num5 > 1000.0)
            {
                lblFirstPulseTimeLimitms.Text = "s";
                num5 /= 1000.0;
                lblFirstTimeLimitVal.Text = string.Format("{0:F3}", num5);
            }
            else
            {
                lblFirstPulseTimeLimitms.Text = "ms";
                lblFirstTimeLimitVal.Text = string.Format("{0:F3}", num5);
            }
            double num6 = pulse_step * 2.0;
            double num7 = tbPulseLatency.Value * num6;
            if (num7 > 1000.0)
            {
                lblPulseLatencyms.Text = "s";
                num7 /= 1000.0;
                lblPulseLatencyVal.Text = string.Format("{0:F2}", num7);
            }
            else
            {
                lblPulseLatencyms.Text = "ms";
                lblPulseLatencyVal.Text = string.Format("{0:F2}", num7);
            }
            double num8 = pulse_step * 2.0;
            double num9 = tbPulse2ndPulseWin.Value * num8;
            if (num9 > 1000.0)
            {
                lblPulse2ndPulseWinms.Text = "s";
                num9 /= 1000.0;
                lblPulse2ndPulseWinVal.Text = string.Format("{0:F2}", num9);
            }
            else
            {
                lblPulse2ndPulseWinms.Text = "ms";
                lblPulse2ndPulseWinVal.Text = string.Format("{0:F2}", num9);
            }
        }

        private void ddlHPFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            VeyronControllerObj.SetHPFilterFlag = true;
			CreateNewTaskFromGUI(new ControllerReqPacket(), 0, TaskID.SetHPFilter, new int[] { ddlHPFilter.SelectedIndex });
        }

        private void ddlHysteresisA_SelectedIndexChanged(object sender, EventArgs e)
        {
            Hysteresis_index = ddlHysteresisA.SelectedIndex;

            switch (Hysteresis_index)
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
            VeyronControllerObj.SetHysteresisFlag = true;
			CreateNewTaskFromGUI(new ControllerReqPacket(), 4, TaskID.SetHysteresis, new int[] { Hysteresis_index });

            P2LResultA = PLAngle - HysAngle;
            L2PResultA = PLAngle + HysAngle;
            lblValP2LResult.Text = string.Format("{0:D2}", P2LResultA);
            lblValL2PResult.Text = string.Format("{0:D2}", L2PResultA);
            if (P2LResultA < 0)
            {
                lblPLWarning.Visible = true;
                lblPLWarning.Text = " Invalid P2L Angle!!";
            }
            else if (L2PResultA > 90)
            {
                lblPLWarning.Visible = true;
                lblPLWarning.Text = " Invalid L2P Angle!!";
            }
            else
            {
                lblPLWarning.Visible = false;
            }
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
            VeyronControllerObj.SetPLTripAFlag = true;
			CreateNewTaskFromGUI(new ControllerReqPacket(), 4, TaskID.SetPLTripA, new int[] { Hysteresis_index });

            P2LResultA = PLAngle - HysAngle;
            L2PResultA = PLAngle + HysAngle;
            lblValP2LResult.Text = string.Format("{0:D2}", P2LResultA);
            if (P2LResultA < 0)
            {
                lblPLWarning.Visible = true;
                lblPLWarning.Text = " Invalid P2L Angle!!";
            }
            else if (L2PResultA > 90)
            {
                lblPLWarning.Visible = true;
                lblPLWarning.Text = " Invalid L2P Angle!!";
            }
            else
            {
                lblPLWarning.Visible = false;
            }
            lblValL2PResult.Text = string.Format("{0:D2}", L2PResultA);
        }

        private void ddlSleepSR_SelectedIndexChanged(object sender, EventArgs e)
        {
            VeyronControllerObj.SetSleepSampleRateFlag = true;
			CreateNewTaskFromGUI(new ControllerReqPacket(), 0, TaskID.SetSleepSampleRate, new int[] { ddlSleepSR.SelectedIndex });
            int selectedIndex = ddlHPFilter.SelectedIndex;
            switch (ddlSleepSR.SelectedIndex)
            {
                case 0:
                    switch (SleepOSMode)
                    {
                        case 0:
                            DRSleep_timestep = 20.0;
                            DRSleep_PulseTimeStepNoLPF = 5.0;
                            return;

                        case 1:
                            DRSleep_timestep = 20.0;
                            DRSleep_PulseTimeStepNoLPF = 5.0;
                            return;

                        case 2:
                            DRSleep_timestep = 2.5;
                            DRSleep_PulseTimeStepNoLPF = 0.625;
                            return;

                        case 3:
                            DRSleep_timestep = 20.0;
                            DRSleep_PulseTimeStepNoLPF = 10.0;
                            return;
                    }
                    break;

                case 1:
                    switch (SleepOSMode)
                    {
                        case 0:
                            DRSleep_timestep = 20.0;
                            DRSleep_PulseTimeStepNoLPF = 5.0;
                            return;

                        case 1:
                            DRSleep_timestep = 80.0;
                            DRSleep_PulseTimeStepNoLPF = 20.0;
                            return;

                        case 2:
                            DRSleep_timestep = 2.5;
                            DRSleep_PulseTimeStepNoLPF = 0.625;
                            return;

                        case 3:
                            DRSleep_timestep = 80.0;
                            DRSleep_PulseTimeStepNoLPF = 40.0;
                            return;
                    }
                    break;

                case 2:
                    switch (SleepOSMode)
                    {
                        case 0:
                            DRSleep_timestep = 20.0;
                            DRSleep_PulseTimeStepNoLPF = 5.0;
                            return;

                        case 1:
                            DRSleep_timestep = 160.0;
                            DRSleep_PulseTimeStepNoLPF = 40.0;
                            return;

                        case 2:
                            DRSleep_timestep = 2.5;
                            DRSleep_PulseTimeStepNoLPF = 0.625;
                            return;

                        case 3:
                            DRSleep_timestep = 160.0;
                            DRSleep_PulseTimeStepNoLPF = 80.0;
                            return;
                    }
                    break;

                case 3:
                    switch (SleepOSMode)
                    {
                        case 0:
                            DRSleep_timestep = 20.0;
                            DRSleep_PulseTimeStepNoLPF = 5.0;
                            return;

                        case 1:
                            DRSleep_timestep = 640.0;
                            DRSleep_PulseTimeStepNoLPF = 160.0;
                            return;

                        case 2:
                            DRSleep_timestep = 2.5;
                            DRSleep_PulseTimeStepNoLPF = 0.625;
                            return;

                        case 3:
                            DRSleep_timestep = 640.0;
                            DRSleep_PulseTimeStepNoLPF = 320.0;
                            return;
                    }
                    break;
            }
        }

        private void ddlZLockA_SelectedIndexChanged(object sender, EventArgs e)
        {
            VeyronControllerObj.SetZLockAFlag = true;
			CreateNewTaskFromGUI(new ControllerReqPacket(), 4, TaskID.SetZLockA, new int[] { ddlZLockA.SelectedIndex });
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
                TmrActive.Stop();
                tmrDataDisplay.Stop();
                VeyronControllerObj.ResetDevice();
                VeyronControllerObj.ControllerEvents -= new AccelController.ControllerEventHandler(ControllerObj_ControllerEvents);
                base.Close();
            }
        }

        public object GetGuiHandle()
        {
            return this;
        }

        private void GUI_DataConfigScreen(GUIUpdatePacket guiPacket)
        {
            if (!chkDisableFIFO.Checked)
            {
                ledFIFOStatus.Value = true;
                ledRealTimeStatus.Value = false;
            }
            else
            {
                ledFIFOStatus.Value = false;
                ledRealTimeStatus.Value = true;
            }
			if (guiPacket.TaskID == 0x4e)
			{
				byte[] buffer = (byte[])guiPacket.PayLoad.Dequeue();
				ledXDR.Value = (buffer[0] == 1);
				ledYDR.Value = (buffer[1] == 1);
				ledZDR.Value = (buffer[2] == 1);
				ledXYZDR.Value = (buffer[3] == 1);
				ledXOW.Value = (buffer[4] == 1);
				ledYOW.Value = (buffer[5] == 1);
				ledZOW.Value = (buffer[6] == 1);
				ledXYZOW.Value = (buffer[7] == 1);
			}
        }

        private void GUI_FIFOScreen(GUIUpdatePacket guiPacket)
        {
			if (guiPacket.TaskID == 0x4c)
			{
				byte[] buffer = (byte[])guiPacket.PayLoad.Dequeue();
				ledTrigMFF.Value = (buffer[6] == 1);
			}
			if (guiPacket.TaskID == 0x49)
			{
				byte[] buffer2 = (byte[])guiPacket.PayLoad.Dequeue();
				ledTrigTrans.Value = (buffer2[6] == 1);
			}
			if (guiPacket.TaskID == 0x4b)
			{
				byte[] buffer3 = (byte[])guiPacket.PayLoad.Dequeue();
				ledTrigTap.Value = (buffer3[6] == 1);
			}
			if (guiPacket.TaskID == 0x4f)
			{
				byte[] buffer4 = (byte[])guiPacket.PayLoad.Dequeue();
				ledTrigLP.Value = (buffer4[3] == 1);
			}
			if (guiPacket.TaskID == 80)
			{
				byte[] buffer5 = (byte[])guiPacket.PayLoad.Dequeue();
				int num = 3;
				int[] numArray = new int[0x60];
				if (rdoCircular.Checked)
					num = 1;
				else
					num = 0x20;

				numArray = (int[])guiPacket.PayLoad.Dequeue();
				if (numArray[0] != 0)
					for (int i = 0; i < num; i++)
					{
						string text = numArray[i * 3].ToString() + "   " + numArray[(i * 3) + 1].ToString() + "   " + numArray[(i * 3) + 2].ToString() + "\n";
						rtbFIFOdump.AppendText(text);
						rtbFIFOdump.ScrollToCaret();
					}

				ledOverFlow.Value = (buffer5[0] == 1);
				ledWatermark.Value = (buffer5[1] == 1);
				lblF_Count.Text = buffer5[2].ToString();
			}
        }

		private void GUI_MFF1_2Screen(GUIUpdatePacket guiPacket)
		{
			if (guiPacket.TaskID == 0x4c)
			{
				if (rdoActive.Checked && ((chkXEFE.Checked || chkYEFE.Checked) || chkZEFE.Checked))
				{
					MFFGraph.Enabled = true;
					if (rdo8bitDataMain.Checked)
						UpdateWaveformMFF(Gees8bData);
					else
					{
						switch (DeviceID)
						{
							case 2:
								UpdateWaveformMFF(Gees14bData);
								break;

							case 3:
								UpdateWaveformMFF(Gees12bData);
								break;

							case 4:
								UpdateWaveformMFF(Gees10bData);
								break;
						}
					}
				}
				byte[] buffer = (byte[])guiPacket.PayLoad.Dequeue();
				ledMFF1XHE.Value = (buffer[0] == 1);
				lblXHP.Text = ((buffer[1] == 1) ? "Negative" : "Positive");
				ledMFF1YHE.Value = (buffer[2] == 1);
				lblYHP.Text = (buffer[3] == 1) ? "Negative" : "Positive";
				ledMFF1ZHE.Value = (buffer[4] == 1);
				lblZHP.Text = (buffer[5] == 1) ? "Negative" : "Positive";
				ledMFF1EA.Value = (buffer[6] == 1);
			}
		}

		private void GUI_PulseDetectScreen(GUIUpdatePacket guiPacket)
		{
			if (guiPacket.TaskID == 0x4b)
			{
				byte[] buffer = (byte[])guiPacket.PayLoad.Dequeue();
				ledPX.Value = (buffer[0] == 1);
				ledPY.Value = (buffer[1] == 1);
				ledPZ.Value = (buffer[2] == 1);
				lblPPolX.Text = (buffer[3] == 1) ? "Negative" : "Positive";
				lblPPolY.Text = (buffer[4] == 1) ? "Negative" : "Positive";
				lblPPolZ.Text = (buffer[5] == 1) ? "Negative" : "Positive";
				ledPulseEA.Value = (buffer[6] == 1);
				ledPulseDouble.Value = (buffer[7] == 1);
			}
		}

        private void GUI_RegistersScreen(GUIUpdatePacket guiPacket)
        {
            if (guiPacket.TaskID == 0x11)
            {
                byte num = (byte) guiPacket.PayLoad.Dequeue();
                lblRegValue.Text = string.Format("{0:X2}", num);
            }
            if (guiPacket.TaskID == 0x5d)
            {
                int[] numArray = (int[]) guiPacket.PayLoad.Dequeue();
                lb_BitV07.Text = string.Format("0x{0:X2}", numArray[0]);
                lb_BitV0.Text = string.Format("{0:D1}", numArray[1]);
                lb_BitV1.Text = string.Format("{0:D1}", numArray[2]);
                lb_BitV2.Text = string.Format("{0:D1}", numArray[3]);
                lb_BitV3.Text = string.Format("{0:D1}", numArray[4]);
                lb_BitV4.Text = string.Format("{0:D1}", numArray[5]);
                lb_BitV5.Text = string.Format("{0:D1}", numArray[6]);
                lb_BitV6.Text = string.Format("{0:D1}", numArray[7]);
                lb_BitV7.Text = string.Format("{0:D1}", numArray[8]);
                switch (numArray[9])
                {
                    case 0:
                        lbl00.Text = string.Format("0x{0:X2}", numArray[0]);
                        break;

                    case 1:
                        lbl01.Text = string.Format("0x{0:X2}", numArray[0]);
                        break;

                    case 2:
                        lbl02.Text = string.Format("0x{0:X2}", numArray[0]);
                        break;

                    case 3:
                        lbl03.Text = string.Format("0x{0:X2}", numArray[0]);
                        break;

                    case 4:
                        lbl04.Text = string.Format("0x{0:X2}", numArray[0]);
                        break;

                    case 5:
                        lbl05.Text = string.Format("0x{0:X2}", numArray[0]);
                        break;

                    case 6:
                        lbl06.Text = string.Format("0x{0:X2}", numArray[0]);
                        break;

                    case 9:
                        lbl09.Text = string.Format("0x{0:X2}", numArray[0]);
                        break;

                    case 10:
                        lbl0A.Text = string.Format("0x{0:X2}", numArray[0]);
                        break;

                    case 11:
                        lbl0B.Text = string.Format("0x{0:X2}", numArray[0]);
                        break;

                    case 12:
                        lbl0C.Text = string.Format("0x{0:X2}", numArray[0]);
                        break;

                    case 13:
                        lbl0D.Text = string.Format("0x{0:X2}", numArray[0]);
                        break;

                    case 14:
                        lbl0E.Text = string.Format("0x{0:X2}", numArray[0]);
                        break;

                    case 15:
                        lbl0F.Text = string.Format("0x{0:X2}", numArray[0]);
                        break;

                    case 0x10:
                        lbl10.Text = string.Format("0x{0:X2}", numArray[0]);
                        break;

                    case 0x11:
                        lbl11.Text = string.Format("0x{0:X2}", numArray[0]);
                        break;

                    case 0x12:
                        lbl12.Text = string.Format("0x{0:X2}", numArray[0]);
                        break;

                    case 0x13:
                        lbl13.Text = string.Format("0x{0:X2}", numArray[0]);
                        break;

                    case 20:
                        lbl14.Text = string.Format("0x{0:X2}", numArray[0]);
                        break;

                    case 0x15:
                        lbl15.Text = string.Format("0x{0:X2}", numArray[0]);
                        break;

                    case 0x16:
                        lbl16.Text = string.Format("0x{0:X2}", numArray[0]);
                        break;

                    case 0x17:
                        lbl17.Text = string.Format("0x{0:X2}", numArray[0]);
                        break;

                    case 0x18:
                        lbl18.Text = string.Format("0x{0:X2}", numArray[0]);
                        break;

                    case 0x19:
                        lbl19.Text = string.Format("0x{0:X2}", numArray[0]);
                        break;

                    case 0x1a:
                        lbl1A.Text = string.Format("0x{0:X2}", numArray[0]);
                        break;

                    case 0x1b:
                        lbl1B.Text = string.Format("0x{0:X2}", numArray[0]);
                        break;

                    case 0x1c:
                        lbl1C.Text = string.Format("0x{0:X2}", numArray[0]);
                        break;

                    case 0x1d:
                        lbl1D.Text = string.Format("0x{0:X2}", numArray[0]);
                        break;

                    case 30:
                        lbl1E.Text = string.Format("0x{0:X2}", numArray[0]);
                        break;

                    case 0x1f:
                        lbl1F.Text = string.Format("0x{0:X2}", numArray[0]);
                        break;

                    case 0x20:
                        lbl20.Text = string.Format("0x{0:X2}", numArray[0]);
                        break;

                    case 0x21:
                        lbl21.Text = string.Format("0x{0:X2}", numArray[0]);
                        break;

                    case 0x22:
                        lbl22.Text = string.Format("0x{0:X2}", numArray[0]);
                        break;

                    case 0x23:
                        lbl23.Text = string.Format("0x{0:X2}", numArray[0]);
                        break;

                    case 0x24:
                        lbl24.Text = string.Format("0x{0:X2}", numArray[0]);
                        break;

                    case 0x25:
                        lbl25.Text = string.Format("0x{0:X2}", numArray[0]);
                        break;

                    case 0x26:
                        lbl26.Text = string.Format("0x{0:X2}", numArray[0]);
                        break;

                    case 0x27:
                        lbl27.Text = string.Format("0x{0:X2}", numArray[0]);
                        break;

                    case 40:
                        lbl28.Text = string.Format("0x{0:X2}", numArray[0]);
                        break;

                    case 0x29:
                        lbl29.Text = string.Format("0x{0:X2}", numArray[0]);
                        break;

                    case 0x2a:
                        lbl2A.Text = string.Format("0x{0:X2}", numArray[0]);
                        break;

                    case 0x2b:
                        lbl2B.Text = string.Format("0x{0:X2}", numArray[0]);
                        break;

                    case 0x2c:
                        lbl2C.Text = string.Format("0x{0:X2}", numArray[0]);
                        break;

                    case 0x2d:
                        lbl2D.Text = string.Format("0x{0:X2}", numArray[0]);
                        break;

                    case 0x2e:
                        lbl2E.Text = string.Format("0x{0:X2}", numArray[0]);
                        break;

                    case 0x2f:
                        lbl2F.Text = string.Format("0x{0:X2}", numArray[0]);
                        break;

                    case 0x30:
                        lbl30.Text = string.Format("0x{0:X2}", numArray[0]);
                        break;

                    case 0x31:
                        lbl31.Text = string.Format("0x{0:X2}", numArray[0]);
                        break;
                }
            }
            if (guiPacket.TaskID == 0x10)
            {
                byte[] buffer = (byte[]) guiPacket.PayLoad.Dequeue();
                lbl00.Text = string.Format("0x{0:X2}", buffer[0]);
                lbl01.Text = string.Format("0x{0:X2}", buffer[1]);
                lbl02.Text = string.Format("0x{0:X2}", buffer[2]);
                lbl03.Text = string.Format("0x{0:X2}", buffer[3]);
                lbl04.Text = string.Format("0x{0:X2}", buffer[4]);
                lbl05.Text = string.Format("0x{0:X2}", buffer[5]);
                lbl06.Text = string.Format("0x{0:X2}", buffer[6]);
                lbl09.Text = string.Format("0x{0:X2}", buffer[9]);
                lbl0A.Text = string.Format("0x{0:X2}", buffer[10]);
                lbl0B.Text = string.Format("0x{0:X2}", buffer[11]);
                lbl0C.Text = string.Format("0x{0:X2}", buffer[12]);
                lbl0D.Text = string.Format("0x{0:X2}", buffer[13]);
                lbl0E.Text = string.Format("0x{0:X2}", buffer[14]);
                lbl0F.Text = string.Format("0x{0:X2}", buffer[15]);
                lbl10.Text = string.Format("0x{0:X2}", buffer[0x10]);
                lbl11.Text = string.Format("0x{0:X2}", buffer[0x11]);
                lbl12.Text = string.Format("0x{0:X2}", buffer[0x12]);
                lbl13.Text = string.Format("0x{0:X2}", buffer[0x13]);
                lbl14.Text = string.Format("0x{0:X2}", buffer[20]);
                lbl15.Text = string.Format("0x{0:X2}", buffer[0x15]);
                lbl16.Text = string.Format("0x{0:X2}", buffer[0x16]);
                lbl17.Text = string.Format("0x{0:X2}", buffer[0x17]);
                lbl18.Text = string.Format("0x{0:X2}", buffer[0x18]);
                lbl19.Text = string.Format("0x{0:X2}", buffer[0x19]);
                lbl1A.Text = string.Format("0x{0:X2}", buffer[0x1a]);
                lbl1B.Text = string.Format("0x{0:X2}", buffer[0x1b]);
                lbl1C.Text = string.Format("0x{0:X2}", buffer[0x1c]);
                lbl1D.Text = string.Format("0x{0:X2}", buffer[0x1d]);
                lbl1E.Text = string.Format("0x{0:X2}", buffer[30]);
                lbl1F.Text = string.Format("0x{0:X2}", buffer[0x1f]);
                lbl20.Text = string.Format("0x{0:X2}", buffer[0x20]);
                lbl21.Text = string.Format("0x{0:X2}", buffer[0x21]);
                lbl22.Text = string.Format("0x{0:X2}", buffer[0x22]);
                lbl23.Text = string.Format("0x{0:X2}", buffer[0x23]);
                lbl24.Text = string.Format("0x{0:X2}", buffer[0x24]);
                lbl25.Text = string.Format("0x{0:X2}", buffer[0x25]);
                lbl26.Text = string.Format("0x{0:X2}", buffer[0x26]);
                lbl27.Text = string.Format("0x{0:X2}", buffer[0x27]);
                lbl28.Text = string.Format("0x{0:X2}", buffer[40]);
                lbl29.Text = string.Format("0x{0:X2}", buffer[0x29]);
                lbl2A.Text = string.Format("0x{0:X2}", buffer[0x2a]);
                lbl2B.Text = string.Format("0x{0:X2}", buffer[0x2b]);
                lbl2C.Text = string.Format("0x{0:X2}", buffer[0x2c]);
                lbl2D.Text = string.Format("0x{0:X2}", buffer[0x2d]);
                lbl2E.Text = string.Format("0x{0:X2}", buffer[0x2e]);
                lbl2F.Text = string.Format("0x{0:X2}", buffer[0x2f]);
                lbl30.Text = string.Format("0x{0:X2}", buffer[0x30]);
                lbl31.Text = string.Format("0x{0:X2}", buffer[0x31]);
            }
            VeyronControllerObj.DisableGetRegs();
        }

		private void GUI_ReturningPLStatus(GUIUpdatePacket guiPacket)
		{
			if (guiPacket.TaskID == 0x4f)
			{
				byte[] buffer = (byte[])guiPacket.PayLoad.Dequeue();
				ledCurPLFront.Value = (buffer[0] == 0);
				ledCurPLBack.Value = (buffer[0] == 1);
				ledCurPLUp.Value = (buffer[1] == 0);
				ledCurPLDown.Value = (buffer[1] == 1);
				ledCurPLRight.Value = (buffer[1] == 2);
				ledCurPLLeft.Value = (buffer[1] == 3);
				ledCurPLLO.Value = (buffer[2] == 1);
				ledCurPLNew.Value = (buffer[3] == 1);
				try
				{
					if (ledCurPLBack.Value && ledCurPLUp.Value)
						PLImage.Image = ImagePL_bu;
					if (ledCurPLBack.Value && ledCurPLDown.Value)
						PLImage.Image = ImagePL_bd;
					if (ledCurPLBack.Value && ledCurPLLeft.Value)
						PLImage.Image = ImagePL_bl;
					if (ledCurPLBack.Value && ledCurPLRight.Value)
						PLImage.Image = ImagePL_br;
					if (ledCurPLFront.Value && ledCurPLUp.Value)
						PLImage.Image = ImagePL_fu;
					if (ledCurPLFront.Value && ledCurPLDown.Value)
						PLImage.Image = ImagePL_fd;
					if (ledCurPLFront.Value && ledCurPLLeft.Value)
						PLImage.Image = ImagePL_fl;
					if (ledCurPLFront.Value && ledCurPLRight.Value)
						PLImage.Image = ImagePL_fr;
				}
				catch (InvalidOperationException) { }
			}
		}

        private void GUI_SetCal(GUIUpdatePacket guiPacket)
        {
            RWLockGetCalValues.AcquireWriterLock(-1);
            try
            {
                int[] numArray = (int[]) guiPacket.PayLoad.Dequeue();
                lblXCal.Text = string.Format("{0:F0}", numArray[0]);
                lblYCal.Text = string.Format("{0:F0}", numArray[1]);
                lblZCal.Text = string.Format("{0:F0}", numArray[2]);
            }
            finally
            {
                RWLockGetCalValues.ReleaseWriterLock();
            }
        }

		private void GUI_SetUpInterrupts(GUIUpdatePacket guiPacket)
		{
			byte[] buffer = (byte[])guiPacket.PayLoad.Dequeue();
			ledDataReady.Value = (buffer[0] == 1);
			ledMFF1.Value = (buffer[2] == 1);
			ledPulse.Value = (buffer[3] == 1);
			ledOrient.Value = (buffer[4] == 1);
			ledTrans.Value = (buffer[5] == 1);
			ledTrans1.Value = (buffer[1] == 1);
			ledFIFO.Value = (buffer[6] == 1);
			ledASleep.Value = (buffer[7] == 1);
		}

		private void GUI_TransientDetectScreen(GUIUpdatePacket guiPacket)
		{
			if (rdoActive.Checked && ((((chkTransEnableXFlag.Checked || chkTransEnableXFlagNEW.Checked) || (chkTransEnableYFlag.Checked || chkTransEnableYFlagNEW.Checked)) || chkTransEnableZFlag.Checked) || chkTransEnableZFlagNEW.Checked))
			{
				gphXYZ.Enabled = true;
				if (rdo8bitDataMain.Checked)
					UpdateWaveformTrans(Gees8bData);
				else
				{
					switch (DeviceID)
					{
						case 2:
							UpdateWaveformTrans(Gees14bData);
							break;

						case 3:
							UpdateWaveformTrans(Gees12bData);
							break;

						case 4:
							UpdateWaveformTrans(Gees10bData);
							break;
					}
				}
			}
			if (guiPacket.TaskID == 0x49)
			{
				byte[] buffer = (byte[])guiPacket.PayLoad.Dequeue();
				ledTransXDetect.Value = (buffer[0] == 1);
				lblTransPolX.Text = (buffer[1] == 1) ? "Negative" : "Positive";
				ledTransYDetect.Value = (buffer[2] == 1);
				lblTransPolY.Text = (buffer[3] == 1) ? "Negative" : "Positive";
				ledTransZDetect.Value = (buffer[4] == 1);
				lblTransPolZ.Text = (buffer[5] == 1) ? "Negative" : "Positive";
				ledTransEA.Value = (buffer[6] == 1);
			}
			if (guiPacket.TaskID == 0x4d)
			{
				byte[] buffer2 = (byte[])guiPacket.PayLoad.Dequeue();
				ledTransXDetectNEW.Value = (buffer2[0] == 1);
				lblTransNewPolX.Text = (buffer2[1] == 1) ? "Negative" : "Positive";
				ledTransYDetectNEW.Value = (buffer2[2] == 1);
				lblTransNewPolY.Text = (buffer2[3] == 1) ? "Negative" : "Positive";
				ledTransZDetectNEW.Value = (buffer2[4] == 1);
				lblTransNewPolZ.Text = (buffer2[5] == 1) ? "Negative" : "Positive";
				ledTransEANEW.Value = (buffer2[6] == 1);
			}
		}

		#region InitializeComponent
		private void InitializeComponent()
        {
            components = new Container();
            PL_Page = new TabPage();
            gbOD = new GroupBox();
            lblPLWarning = new Label();
            label21 = new Label();
            label24 = new Label();
            label5 = new Label();
            lblValL2PResult = new Label();
            lblValP2LResult = new Label();
            lblp2LTripAngle = new Label();
            chkPLDefaultSettings = new CheckBox();
            pPLActive = new Panel();
            btnResetPLDebounce = new Button();
            btnSetPLDebounce = new Button();
            lblBouncems = new Label();
            lblPLbounceVal = new Label();
            lblDebouncePL = new Label();
            tbPL = new TrackBar();
            p6 = new Panel();
            gbPLDisappear = new GroupBox();
            label263 = new Label();
            ddlHysteresisA = new ComboBox();
            ddlPLTripA = new ComboBox();
            label264 = new Label();
            label265 = new Label();
            ddlBFTripA = new ComboBox();
            ddlZLockA = new ComboBox();
            label266 = new Label();
            rdoClrDebouncePL = new RadioButton();
            rdoDecDebouncePL = new RadioButton();
            chkEnablePL = new CheckBox();
            p7 = new Panel();
            label191 = new Label();
            ledCurPLNew = new Led();
            label273 = new Label();
            ledCurPLRight = new Led();
            label275 = new Label();
            ledCurPLLeft = new Led();
            label277 = new Label();
            ledCurPLDown = new Led();
            label279 = new Label();
            ledCurPLUp = new Led();
            label281 = new Label();
            ledCurPLBack = new Led();
            label283 = new Label();
            ledCurPLFront = new Led();
            ledCurPLLO = new Led();
            label285 = new Label();
            MainScreenEval = new TabPage();
            chkEnableZ = new CheckBox();
            chkEnableY = new CheckBox();
            chkEnableX = new CheckBox();
            pSOS = new Panel();
            label18 = new Label();
            rdoSOSHiResMode = new RadioButton();
            label16 = new Label();
            rdoSOSLPMode = new RadioButton();
            rdoSOSLNLPMode = new RadioButton();
            rdoSOSNormalMode = new RadioButton();
            label60 = new Label();
            btnViewData = new Button();
            btnDisableData = new Button();
            p14b8bSelect = new Panel();
            rdoXYZFullResMain = new RadioButton();
            rdo8bitDataMain = new RadioButton();
            panel3 = new Panel();
            btnAutoCal = new Button();
            gbOC = new GroupBox();
            label34 = new Label();
            label46 = new Label();
            label47 = new Label();
            btnWriteCal = new Button();
            label43 = new Label();
            txtCalX = new MaskedTextBox();
            label44 = new Label();
            label45 = new Label();
            txtCalY = new MaskedTextBox();
            txtCalZ = new MaskedTextBox();
            lblXCal = new Label();
            lblYCal = new Label();
            lblZCal = new Label();
            gbXD = new GroupBox();
            pFullResData = new Panel();
            label118 = new Label();
            label119 = new Label();
            label120 = new Label();
            lblY12bC = new Label();
            lblX12bC = new Label();
            lblZ12bC = new Label();
            label112 = new Label();
            label113 = new Label();
            label114 = new Label();
            lblXFull = new Label();
            lblYFull = new Label();
            lblZFull = new Label();
            lblY12bG = new Label();
            lblX12bG = new Label();
            lblZ12bG = new Label();
            label115 = new Label();
            label116 = new Label();
            label117 = new Label();
            lblZ8bC = new Label();
            lblY8bC = new Label();
            lblX8bC = new Label();
            label48 = new Label();
            label110 = new Label();
            label111 = new Label();
            lblZ8bG = new Label();
            lblY8bG = new Label();
            label53 = new Label();
            lblX8bG = new Label();
            label56 = new Label();
            label58 = new Label();
            legend1 = new Legend();
            legendItem1 = new LegendItem();
            MainScreenXAxis = new WaveformPlot();
            xAxis3 = new NationalInstruments.UI.XAxis();
            yAxis3 = new NationalInstruments.UI.YAxis();
            legendItem2 = new LegendItem();
            MainScreenYAxis = new WaveformPlot();
            legendItem3 = new LegendItem();
            MainScreenZAxis = new WaveformPlot();
            gbST = new GroupBox();
            chkSelfTest = new CheckBox();
            MainScreenGraph = new WaveformGraph();
            gbwfs = new GroupBox();
            chkWakeTrans1 = new CheckBox();
            chkWakeTrans = new CheckBox();
            chkWakeFIFOGate = new CheckBox();
            chkWakeLP = new CheckBox();
            chkWakePulse = new CheckBox();
            chkWakeMFF1 = new CheckBox();
            gbIF = new GroupBox();
            p9 = new Panel();
            ledTrans1 = new Led();
            ledOrient = new Led();
            ledASleep = new Led();
            ledMFF1 = new Led();
            ledPulse = new Led();
            ledTrans = new Led();
            ledDataReady = new Led();
            ledFIFO = new Led();
            p8 = new Panel();
            panel18 = new Panel();
            rdoTrans1INT_I1 = new RadioButton();
            rdoTrans1INT_I2 = new RadioButton();
            chkEnIntTrans1 = new CheckBox();
            label36 = new Label();
            panel12 = new Panel();
            rdoFIFOINT_I1 = new RadioButton();
            rdoFIFOINT_I2 = new RadioButton();
            chkEnIntFIFO = new CheckBox();
            lblntFIFO = new Label();
            panel11 = new Panel();
            rdoDRINT_I1 = new RadioButton();
            rdoDRINT_I2 = new RadioButton();
            chkEnIntDR = new CheckBox();
            label6 = new Label();
            panel10 = new Panel();
            rdoTransINT_I1 = new RadioButton();
            rdoTransINT_I2 = new RadioButton();
            chkEnIntTrans = new CheckBox();
            label3 = new Label();
            panel9 = new Panel();
            rdoPulseINT_I1 = new RadioButton();
            rdoPulseINT_I2 = new RadioButton();
            chkEnIntPulse = new CheckBox();
            label2 = new Label();
            panel4 = new Panel();
            rdoASleepINT_I1 = new RadioButton();
            rdoASleepINT_I2 = new RadioButton();
            chkEnIntASleep = new CheckBox();
            label1 = new Label();
            panel8 = new Panel();
            rdoMFF1INT_I1 = new RadioButton();
            rdoMFF1INT_I2 = new RadioButton();
            chkEnIntMFF1 = new CheckBox();
            label4 = new Label();
            panel6 = new Panel();
            rdoPLINT_I1 = new RadioButton();
            rdoPLINT_I2 = new RadioButton();
            chkEnIntPL = new CheckBox();
            label172 = new Label();
            label66 = new Label();
            gbASS = new GroupBox();
            pnlAutoSleep = new Panel();
            btnSleepTimerReset = new Button();
            btnSetSleepTimer = new Button();
            label199 = new Label();
            lblSleepms = new Label();
            lblSleepTimerValue = new Label();
            lblSleepTimer = new Label();
            tbSleepCounter = new TrackBar();
            ddlSleepSR = new ComboBox();
            chkEnableASleep = new CheckBox();
            p2 = new Panel();
            label37 = new Label();
            ddlHPFilter = new ComboBox();
            groupBox6 = new GroupBox();
            label38 = new Label();
            chkXYZ12Log = new CheckBox();
            chkTransLog = new CheckBox();
            chkXYZ8Log = new CheckBox();
            lblFileName = new Label();
            txtSaveFileName = new TextBox();
            gbOM = new GroupBox();
            chkAnalogLowNoise = new CheckBox();
            chkHPFDataOut = new CheckBox();
            pOverSampling = new Panel();
            label70 = new Label();
            rdoOSHiResMode = new RadioButton();
            rdoOSLPMode = new RadioButton();
            rdoOSLNLPMode = new RadioButton();
            rdoOSNormalMode = new RadioButton();
            rdoStandby = new RadioButton();
            rdoActive = new RadioButton();
            ledSleep = new Led();
            lbsleep = new Label();
            label173 = new Label();
            p1 = new Panel();
            ddlDataRate = new ComboBox();
            label35 = new Label();
            ledStandby = new Led();
            label71 = new Label();
            ledWake = new Led();
            gbDR = new GroupBox();
            lbl4gSensitivity = new Label();
            rdo2g = new RadioButton();
            rdo8g = new RadioButton();
            rdo4g = new RadioButton();
            lbl2gSensitivity = new Label();
            lbl8gSensitivity = new Label();
            TabTool = new TabControl();
            Register_Page = new TabPage();
            gbRegisterName = new GroupBox();
            lblComment2 = new Label();
            lblComment1 = new Label();
            tableLayoutPanel1 = new TableLayoutPanel();
            lb_BitV5 = new Label();
            lb_BitV4 = new Label();
            lb_BitV6 = new Label();
            lb_BitV7 = new Label();
            lb_BitV0 = new Label();
            lb_BitV07 = new Label();
            lb_BitV1 = new Label();
            lb_BitV3 = new Label();
            lb_BitV2 = new Label();
            lb_BitN7 = new Label();
            lb_BitN6 = new Label();
            lb_BitN5 = new Label();
            lb_BitN4 = new Label();
            lb_BitN3 = new Label();
            lb_BitN2 = new Label();
            lb_BitN1 = new Label();
            lb_BitN0 = new Label();
            label25 = new Label();
            label27 = new Label();
            label29 = new Label();
            label32 = new Label();
            label39 = new Label();
            label40 = new Label();
            label41 = new Label();
            label72 = new Label();
            label96 = new Label();
            label97 = new Label();
            label98 = new Label();
            label103 = new Label();
            lb_BitN07 = new Label();
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
            groupBox1 = new GroupBox();
            panel1 = new Panel();
            label10 = new Label();
            label12 = new Label();
            btnUpdateRegs = new Button();
            label11 = new Label();
            label9 = new Label();
            label8 = new Label();
            rb31 = new RadioButton();
            rb30 = new RadioButton();
            rb2F = new RadioButton();
            rb2E = new RadioButton();
            rb2D = new RadioButton();
            rb2C = new RadioButton();
            rb2B = new RadioButton();
            rb2A = new RadioButton();
            rb29 = new RadioButton();
            rb28 = new RadioButton();
            rb27 = new RadioButton();
            rb26 = new RadioButton();
            rb25 = new RadioButton();
            rb24 = new RadioButton();
            rb23 = new RadioButton();
            rb22 = new RadioButton();
            rb21 = new RadioButton();
            rb20 = new RadioButton();
            rb1F = new RadioButton();
            rb1E = new RadioButton();
            rb1D = new RadioButton();
            rb1C = new RadioButton();
            rb1B = new RadioButton();
            rb1A = new RadioButton();
            rb19 = new RadioButton();
            rb18 = new RadioButton();
            rb17 = new RadioButton();
            rb16 = new RadioButton();
            rb15 = new RadioButton();
            rb14 = new RadioButton();
            rb13 = new RadioButton();
            rb12 = new RadioButton();
            rb11 = new RadioButton();
            rb10 = new RadioButton();
            rb0F = new RadioButton();
            rb0E = new RadioButton();
            rb0D = new RadioButton();
            rb0C = new RadioButton();
            rb0B = new RadioButton();
            rb0A = new RadioButton();
            rb09 = new RadioButton();
            rb06 = new RadioButton();
            rb05 = new RadioButton();
            rb04 = new RadioButton();
            rb03 = new RadioButton();
            rb02 = new RadioButton();
            rb01 = new RadioButton();
            rbR00 = new RadioButton();
            lbl1A = new Label();
            lbl1B = new Label();
            lbl1C = new Label();
            lbl19 = new Label();
            label57 = new Label();
            label54 = new Label();
            lbl28 = new Label();
            lbl26 = new Label();
            lbl1E = new Label();
            lbl1F = new Label();
            lbl20 = new Label();
            lbl21 = new Label();
            lbl22 = new Label();
            lbl23 = new Label();
            lbl27 = new Label();
            lbl25 = new Label();
            lbl1D = new Label();
            lbl24 = new Label();
            lbl29 = new Label();
            lbl31 = new Label();
            lbl2F = new Label();
            lbl30 = new Label();
            lbl2E = new Label();
            lbl2C = new Label();
            lbl2D = new Label();
            lbl2B = new Label();
            lbl17 = new Label();
            lbl18 = new Label();
            lbl16 = new Label();
            lbl15 = new Label();
            lbl13 = new Label();
            lbl0B = new Label();
            lbl0C = new Label();
            lbl0D = new Label();
            lbl0E = new Label();
            lbl0F = new Label();
            lbl10 = new Label();
            lbl14 = new Label();
            lbl12 = new Label();
            lbl0A = new Label();
            lbl09 = new Label();
            lbl01 = new Label();
            lbl02 = new Label();
            lbl03 = new Label();
            lbl04 = new Label();
            lbl05 = new Label();
            lbl06 = new Label();
            lbl00 = new Label();
            lbl2A = new Label();
            lbl11 = new Label();
            DataConfigPage = new TabPage();
            groupBox2 = new GroupBox();
            gbStatus = new GroupBox();
            p21 = new Panel();
            lblFIFOStatus = new Label();
            ledFIFOStatus = new Led();
            lblFOvf = new Label();
            ledRealTimeStatus = new Led();
            label61 = new Label();
            lblWmrk = new Label();
            lblFCnt5 = new Label();
            lblFCnt4 = new Label();
            lblFCnt3 = new Label();
            lblFCnt2 = new Label();
            lblFCnt1 = new Label();
            lblFCnt0 = new Label();
            ledXYZOW = new Led();
            ledZOW = new Led();
            ledYOW = new Led();
            ledXOW = new Led();
            ledZDR = new Led();
            ledYDR = new Led();
            ledXDR = new Led();
            ledXYZDR = new Led();
            lblXYZOW = new Label();
            lblXYZDrdy = new Label();
            lblXDrdy = new Label();
            lblZOW = new Label();
            lblYOW = new Label();
            lblYDrdy = new Label();
            lblXOW = new Label();
            lblZDrdy = new Label();
            groupBox3 = new GroupBox();
            p20 = new Panel();
            panel16 = new Panel();
            rdoINTPushPull = new RadioButton();
            rdoINTOpenDrain = new RadioButton();
            panel17 = new Panel();
            rdoINTActiveLow = new RadioButton();
            rdoINTActiveHigh = new RadioButton();
            MFF1_2Page = new TabPage();
            chkEnableZMFF = new CheckBox();
            chkEnableYMFF = new CheckBox();
            chkEnableXMFF = new CheckBox();
            lblMotionDataType = new Label();
            legend3 = new Legend();
            legendItem7 = new LegendItem();
            XAxis = new WaveformPlot();
            xAxis1 = new NationalInstruments.UI.XAxis();
            yAxis1 = new NationalInstruments.UI.YAxis();
            legendItem8 = new LegendItem();
            YAxis = new WaveformPlot();
            legendItem9 = new LegendItem();
            waveformPlot3 = new WaveformPlot();
            xAxis2 = new NationalInstruments.UI.XAxis();
            yAxis2 = new NationalInstruments.UI.YAxis();
            MFFGraph = new WaveformGraph();
            waveformPlot1 = new WaveformPlot();
            waveformPlot2 = new WaveformPlot();
            gbMF1 = new GroupBox();
            chkDefaultFFSettings1 = new CheckBox();
            chkDefaultMotion1 = new CheckBox();
            btnMFF1Reset = new Button();
            btnMFF1Set = new Button();
            rdoMFF1ClearDebounce = new RadioButton();
            p14 = new Panel();
            chkMFF1EnableLatch = new CheckBox();
            chkYEFE = new CheckBox();
            chkZEFE = new CheckBox();
            chkXEFE = new CheckBox();
            rdoMFF1And = new RadioButton();
            rdoMFF1Or = new RadioButton();
            p15 = new Panel();
            label20 = new Label();
            label230 = new Label();
            label231 = new Label();
            ledMFF1EA = new Led();
            label232 = new Label();
            label233 = new Label();
            lblXHP = new Label();
            ledMFF1XHE = new Led();
            lblZHP = new Label();
            label236 = new Label();
            ledMFF1YHE = new Led();
            lblYHP = new Label();
            label238 = new Label();
            ledMFF1ZHE = new Led();
            label239 = new Label();
            rdoMFF1DecDebounce = new RadioButton();
            lblMFF1Threshold = new Label();
            lblMFF1ThresholdVal = new Label();
            lblMFF1Debouncems = new Label();
            lblMFF1Thresholdg = new Label();
            tbMFF1Threshold = new TrackBar();
            lblMFF1DebounceVal = new Label();
            tbMFF1Debounce = new TrackBar();
            lblMFF1Debounce = new Label();
            TransientDetection = new TabPage();
            chkZEnableTrans = new CheckBox();
            chkYEnableTrans = new CheckBox();
            chkXEnableTrans = new CheckBox();
            lblTransDataType = new Label();
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
            gbTSNEW = new GroupBox();
            btnResetTransientNEW = new Button();
            btnSetTransientNEW = new Button();
            rdoTransClearDebounceNEW = new RadioButton();
            pTransNEW = new Panel();
            chkDefaultTransSettings1 = new CheckBox();
            chkTransBypassHPFNEW = new CheckBox();
            chkTransEnableLatchNEW = new CheckBox();
            chkTransEnableXFlagNEW = new CheckBox();
            chkTransEnableZFlagNEW = new CheckBox();
            chkTransEnableYFlagNEW = new CheckBox();
            tbTransDebounceNEW = new TrackBar();
            p19 = new Panel();
            lblTransNewPolZ = new Label();
            lblTransNewPolY = new Label();
            lblTransNewPolX = new Label();
            ledTransEANEW = new Led();
            label203 = new Label();
            label204 = new Label();
            ledTransZDetectNEW = new Led();
            ledTransYDetectNEW = new Led();
            ledTransXDetectNEW = new Led();
            label205 = new Label();
            label206 = new Label();
            label207 = new Label();
            lblTransDebounceValNEW = new Label();
            rdoTransDecDebounceNEW = new RadioButton();
            tbTransThresholdNEW = new TrackBar();
            lblTransDebounceNEW = new Label();
            lblTransThresholdNEW = new Label();
            lblTransThresholdValNEW = new Label();
            lblTransThresholdgNEW = new Label();
            lblTransDebouncemsNEW = new Label();
            legend2 = new Legend();
            legendItem4 = new LegendItem();
            legendItem5 = new LegendItem();
            legendItem6 = new LegendItem();
            ZAxis = new WaveformPlot();
            gphXYZ = new WaveformGraph();
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
            PulseDetection = new TabPage();
            gbSDPS = new GroupBox();
            panel15 = new Panel();
            chkPulseLPFEnable = new CheckBox();
            chkPulseHPFBypass = new CheckBox();
            rdoDefaultSPDP = new RadioButton();
            rdoDefaultSP = new RadioButton();
            btnResetPulseThresholds = new Button();
            btnSetPulseThresholds = new Button();
            tbPulseZThreshold = new TrackBar();
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
            p11 = new Panel();
            label15 = new Label();
            ledPulseDouble = new Led();
            lblPPolZ = new Label();
            lblPPolY = new Label();
            lblPPolX = new Label();
            label255 = new Label();
            label256 = new Label();
            ledPulseEA = new Led();
            label258 = new Label();
            ledPZ = new Led();
            ledPX = new Led();
            label268 = new Label();
            label287 = new Label();
            ledPY = new Led();
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
            FIFOPage = new TabPage();
            rdoFIFO14bitDataDisplay = new RadioButton();
            rtbFIFOdump = new RichTextBox();
            rdoFIFO8bitDataDisplay = new RadioButton();
            gb3bF = new GroupBox();
            p5 = new Panel();
            label19 = new Label();
            label17 = new Label();
            label14 = new Label();
            label13 = new Label();
            ledTrigMFF = new Led();
            ledTrigTap = new Led();
            ledTrigLP = new Led();
            ledTrigTrans = new Led();
            button1 = new Button();
            lblCurrent_FIFO_Count = new Label();
            lblF_Count = new Label();
            ledOverFlow = new Led();
            ledWatermark = new Led();
            label64 = new Label();
            label63 = new Label();
            p4 = new Panel();
            gbWatermark = new GroupBox();
            btnResetWatermark = new Button();
            lblWatermarkValue = new Label();
            tBWatermark = new TrackBar();
            btnWatermark = new Button();
            lblWatermark = new Label();
            pTriggerMode = new Panel();
            chkTriggerMFF = new CheckBox();
            chkTriggerPulse = new CheckBox();
            chkTriggerLP = new CheckBox();
            chkTrigTrans = new CheckBox();
            rdoTriggerMode = new RadioButton();
            chkDisableFIFO = new CheckBox();
            rdoFill = new RadioButton();
            rdoCircular = new RadioButton();
            btnSetMode = new Button();
            TmrActive = new System.Windows.Forms.Timer(components);
            toolTip1 = new ToolTip(components);
            tmrDataDisplay = new System.Windows.Forms.Timer(components);
            panel5 = new Panel();
            CommStrip = new StatusStrip();
            toolStripStatusLabel = new ToolStripStatusLabel();
            label7 = new Label();
            CommStripButton = new ToolStripDropDownButton();
            pictureBox2 = new PictureBox();
            PLImage = new PictureBox();
            PL_Page.SuspendLayout();
            gbOD.SuspendLayout();
            pPLActive.SuspendLayout();
            tbPL.BeginInit();
            p6.SuspendLayout();
            gbPLDisappear.SuspendLayout();
            p7.SuspendLayout();
            ((ISupportInitialize) ledCurPLNew).BeginInit();
            ((ISupportInitialize) ledCurPLRight).BeginInit();
            ((ISupportInitialize) ledCurPLLeft).BeginInit();
            ((ISupportInitialize) ledCurPLDown).BeginInit();
            ((ISupportInitialize) ledCurPLUp).BeginInit();
            ((ISupportInitialize) ledCurPLBack).BeginInit();
            ((ISupportInitialize) ledCurPLFront).BeginInit();
            ((ISupportInitialize) ledCurPLLO).BeginInit();
            MainScreenEval.SuspendLayout();
            pSOS.SuspendLayout();
            p14b8bSelect.SuspendLayout();
            panel3.SuspendLayout();
            gbOC.SuspendLayout();
            gbXD.SuspendLayout();
            pFullResData.SuspendLayout();
            ((ISupportInitialize) legend1).BeginInit();
            gbST.SuspendLayout();
            ((ISupportInitialize) MainScreenGraph).BeginInit();
            gbwfs.SuspendLayout();
            gbIF.SuspendLayout();
            p9.SuspendLayout();
            ((ISupportInitialize) ledTrans1).BeginInit();
            ((ISupportInitialize) ledOrient).BeginInit();
            ((ISupportInitialize) ledASleep).BeginInit();
            ((ISupportInitialize) ledMFF1).BeginInit();
            ((ISupportInitialize) ledPulse).BeginInit();
            ((ISupportInitialize) ledTrans).BeginInit();
            ((ISupportInitialize) ledDataReady).BeginInit();
            ((ISupportInitialize) ledFIFO).BeginInit();
            p8.SuspendLayout();
            panel18.SuspendLayout();
            panel12.SuspendLayout();
            panel11.SuspendLayout();
            panel10.SuspendLayout();
            panel9.SuspendLayout();
            panel4.SuspendLayout();
            panel8.SuspendLayout();
            panel6.SuspendLayout();
            gbASS.SuspendLayout();
            pnlAutoSleep.SuspendLayout();
            tbSleepCounter.BeginInit();
            p2.SuspendLayout();
            groupBox6.SuspendLayout();
            gbOM.SuspendLayout();
            pOverSampling.SuspendLayout();
            ((ISupportInitialize) ledSleep).BeginInit();
            p1.SuspendLayout();
            ((ISupportInitialize) ledStandby).BeginInit();
            ((ISupportInitialize) ledWake).BeginInit();
            gbDR.SuspendLayout();
            TabTool.SuspendLayout();
            Register_Page.SuspendLayout();
            gbRegisterName.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            groupBox4.SuspendLayout();
            groupBox1.SuspendLayout();
            panel1.SuspendLayout();
            DataConfigPage.SuspendLayout();
            groupBox2.SuspendLayout();
            gbStatus.SuspendLayout();
            p21.SuspendLayout();
            ((ISupportInitialize) ledFIFOStatus).BeginInit();
            ((ISupportInitialize) ledRealTimeStatus).BeginInit();
            ((ISupportInitialize) ledXYZOW).BeginInit();
            ((ISupportInitialize) ledZOW).BeginInit();
            ((ISupportInitialize) ledYOW).BeginInit();
            ((ISupportInitialize) ledXOW).BeginInit();
            ((ISupportInitialize) ledZDR).BeginInit();
            ((ISupportInitialize) ledYDR).BeginInit();
            ((ISupportInitialize) ledXDR).BeginInit();
            ((ISupportInitialize) ledXYZDR).BeginInit();
            groupBox3.SuspendLayout();
            p20.SuspendLayout();
            panel16.SuspendLayout();
            panel17.SuspendLayout();
            MFF1_2Page.SuspendLayout();
            ((ISupportInitialize) legend3).BeginInit();
            ((ISupportInitialize) MFFGraph).BeginInit();
            gbMF1.SuspendLayout();
            p14.SuspendLayout();
            p15.SuspendLayout();
            ((ISupportInitialize) ledMFF1EA).BeginInit();
            ((ISupportInitialize) ledMFF1XHE).BeginInit();
            ((ISupportInitialize) ledMFF1YHE).BeginInit();
            ((ISupportInitialize) ledMFF1ZHE).BeginInit();
            tbMFF1Threshold.BeginInit();
            tbMFF1Debounce.BeginInit();
            TransientDetection.SuspendLayout();
            pTrans2.SuspendLayout();
            ((ISupportInitialize) ledTransEA).BeginInit();
            ((ISupportInitialize) ledTransZDetect).BeginInit();
            ((ISupportInitialize) ledTransYDetect).BeginInit();
            ((ISupportInitialize) ledTransXDetect).BeginInit();
            gbTSNEW.SuspendLayout();
            pTransNEW.SuspendLayout();
            tbTransDebounceNEW.BeginInit();
            p19.SuspendLayout();
            ((ISupportInitialize) ledTransEANEW).BeginInit();
            ((ISupportInitialize) ledTransZDetectNEW).BeginInit();
            ((ISupportInitialize) ledTransYDetectNEW).BeginInit();
            ((ISupportInitialize) ledTransXDetectNEW).BeginInit();
            tbTransThresholdNEW.BeginInit();
            ((ISupportInitialize) legend2).BeginInit();
            ((ISupportInitialize) gphXYZ).BeginInit();
            gbTS.SuspendLayout();
            p18.SuspendLayout();
            tbTransDebounce.BeginInit();
            tbTransThreshold.BeginInit();
            PulseDetection.SuspendLayout();
            gbSDPS.SuspendLayout();
            panel15.SuspendLayout();
            tbPulseZThreshold.BeginInit();
            p12.SuspendLayout();
            tbPulseLatency.BeginInit();
            tbPulse2ndPulseWin.BeginInit();
            p10.SuspendLayout();
            tbFirstPulseTimeLimit.BeginInit();
            p11.SuspendLayout();
            ((ISupportInitialize) ledPulseDouble).BeginInit();
            ((ISupportInitialize) ledPulseEA).BeginInit();
            ((ISupportInitialize) ledPZ).BeginInit();
            ((ISupportInitialize) ledPX).BeginInit();
            ((ISupportInitialize) ledPY).BeginInit();
            tbPulseXThreshold.BeginInit();
            tbPulseYThreshold.BeginInit();
            FIFOPage.SuspendLayout();
            gb3bF.SuspendLayout();
            p5.SuspendLayout();
            ((ISupportInitialize) ledTrigMFF).BeginInit();
            ((ISupportInitialize) ledTrigTap).BeginInit();
            ((ISupportInitialize) ledTrigLP).BeginInit();
            ((ISupportInitialize) ledTrigTrans).BeginInit();
            ((ISupportInitialize) ledOverFlow).BeginInit();
            ((ISupportInitialize) ledWatermark).BeginInit();
            p4.SuspendLayout();
            gbWatermark.SuspendLayout();
            tBWatermark.BeginInit();
            pTriggerMode.SuspendLayout();
            panel5.SuspendLayout();
            CommStrip.SuspendLayout();
            ((ISupportInitialize) pictureBox2).BeginInit();
            ((ISupportInitialize) PLImage).BeginInit();
            base.SuspendLayout();
            PL_Page.BackColor = Color.SlateGray;
            PL_Page.BorderStyle = BorderStyle.Fixed3D;
            PL_Page.Controls.Add(PLImage);
            PL_Page.Controls.Add(gbOD);
            PL_Page.Controls.Add(p7);
            PL_Page.ForeColor = Color.White;
            PL_Page.Location = new Point(4, 0x19);
            PL_Page.Name = "PL_Page";
            PL_Page.Padding = new Padding(3);
            PL_Page.Size = new Size(0x464, 0x20d);
            PL_Page.TabIndex = 1;
            PL_Page.Text = "Orientation";
            PL_Page.UseVisualStyleBackColor = true;
            gbOD.BackColor = Color.LightSlateGray;
            gbOD.Controls.Add(lblPLWarning);
            gbOD.Controls.Add(label21);
            gbOD.Controls.Add(label24);
            gbOD.Controls.Add(label5);
            gbOD.Controls.Add(lblValL2PResult);
            gbOD.Controls.Add(lblValP2LResult);
            gbOD.Controls.Add(lblp2LTripAngle);
            gbOD.Controls.Add(chkPLDefaultSettings);
            gbOD.Controls.Add(pPLActive);
            gbOD.Controls.Add(p6);
            gbOD.Controls.Add(chkEnablePL);
            gbOD.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            gbOD.ForeColor = Color.White;
            gbOD.Location = new Point(0x3b, 0x27);
            gbOD.Name = "gbOD";
            gbOD.Size = new Size(0x22d, 0x19b);
            gbOD.TabIndex = 0xc0;
            gbOD.TabStop = false;
            gbOD.Text = "Orientation Detection";
            lblPLWarning.AutoSize = true;
            lblPLWarning.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPLWarning.ForeColor = Color.Red;
            lblPLWarning.Location = new Point(0xc6, 0x52);
            lblPLWarning.Name = "lblPLWarning";
            lblPLWarning.Size = new Size(0x70, 20);
            lblPLWarning.TabIndex = 230;
            lblPLWarning.Text = "Invalid Angle";
            lblPLWarning.Visible = false;
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
            pPLActive.Controls.Add(btnResetPLDebounce);
            pPLActive.Controls.Add(btnSetPLDebounce);
            pPLActive.Controls.Add(lblBouncems);
            pPLActive.Controls.Add(lblPLbounceVal);
            pPLActive.Controls.Add(lblDebouncePL);
            pPLActive.Controls.Add(tbPL);
            pPLActive.Enabled = false;
            pPLActive.Location = new Point(0x22, 0x131);
            pPLActive.Name = "pPLActive";
            pPLActive.Size = new Size(0x1e8, 0x5b);
            pPLActive.TabIndex = 0xdd;
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
            tbPL.Size = new Size(0x14e, 40);
            tbPL.TabIndex = 0xce;
            tbPL.TickFrequency = 15;
            tbPL.Scroll += new EventHandler(trackBarPL_Scroll_1);
            p6.Controls.Add(gbPLDisappear);
            p6.Controls.Add(rdoClrDebouncePL);
            p6.Controls.Add(rdoDecDebouncePL);
            p6.Enabled = false;
            p6.Location = new Point(0x52, 0x69);
            p6.Name = "p6";
            p6.Size = new Size(0x178, 0xc2);
            p6.TabIndex = 0xda;
            gbPLDisappear.Controls.Add(label263);
            gbPLDisappear.Controls.Add(ddlHysteresisA);
            gbPLDisappear.Controls.Add(ddlPLTripA);
            gbPLDisappear.Controls.Add(label264);
            gbPLDisappear.Controls.Add(label265);
            gbPLDisappear.Controls.Add(ddlBFTripA);
            gbPLDisappear.Controls.Add(ddlZLockA);
            gbPLDisappear.Controls.Add(label266);
            gbPLDisappear.Enabled = false;
            gbPLDisappear.Location = new Point(0x15, 7);
            gbPLDisappear.Name = "gbPLDisappear";
            gbPLDisappear.Size = new Size(0x157, 0x9d);
            gbPLDisappear.TabIndex = 0xda;
            gbPLDisappear.TabStop = false;
            label263.AutoSize = true;
            label263.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            label263.Location = new Point(8, 0x7c);
            label263.Name = "label263";
            label263.Size = new Size(0x55, 13);
            label263.TabIndex = 0xd9;
            label263.Text = "Hysteresis Angle";
            ddlHysteresisA.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            ddlHysteresisA.FormattingEnabled = true;
            ddlHysteresisA.Items.AddRange(new object[] { "+/- 0\x00b0", "+/- 4\x00b0", "+/- 7\x00b0", "+/- 11\x00b0", "+/- 14\x00b0", "+/- 17\x00b0", "+/- 21\x00b0", "+/- 24\x00b0", "" });
            ddlHysteresisA.Location = new Point(0x62, 0x76);
            ddlHysteresisA.Name = "ddlHysteresisA";
            ddlHysteresisA.Size = new Size(0xdd, 0x15);
            ddlHysteresisA.TabIndex = 0xd8;
            ddlHysteresisA.SelectedIndexChanged += new EventHandler(ddlHysteresisA_SelectedIndexChanged);
            ddlPLTripA.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            ddlPLTripA.FormattingEnabled = true;
            ddlPLTripA.Items.AddRange(new object[] { "15\x00b0", "20\x00b0", "30\x00b0", "35\x00b0", "40\x00b0", "45\x00b0", "55\x00b0", "60\x00b0", "70\x00b0", "75\x00b0" });
            ddlPLTripA.Location = new Point(0x62, 0x58);
            ddlPLTripA.Name = "ddlPLTripA";
            ddlPLTripA.Size = new Size(0xdd, 0x15);
            ddlPLTripA.TabIndex = 0xd7;
            ddlPLTripA.SelectedIndexChanged += new EventHandler(ddlPLTripA_SelectedIndexChanged);
            label264.AutoSize = true;
            label264.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            label264.Location = new Point(0x16, 0x5c);
            label264.Name = "label264";
            label264.Size = new Size(0x47, 13);
            label264.TabIndex = 0xd6;
            label264.Text = "P-LTrip Angle";
            label265.AutoSize = true;
            label265.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            label265.Location = new Point(0x15, 0x3e);
            label265.Name = "label265";
            label265.Size = new Size(0x4c, 13);
            label265.TabIndex = 0xd5;
            label265.Text = "B/F Trip Angle";
            ddlBFTripA.Font = new Font("Microsoft Sans Serif", 6.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            ddlBFTripA.FormattingEnabled = true;
            ddlBFTripA.Items.AddRange(new object[] { "BF: Z<80\x00b0Z>280\x00b0 FB: Z>100\x00b0Z<260\x00b0", "BF: Z<75\x00b0Z>285\x00b0 FB: Z>105\x00b0Z<255\x00b0", "BF: Z<70\x00b0Z>290\x00b0 FB: Z>110\x00b0Z<250\x00b0", "BF: Z<65\x00b0Z>295\x00b0 FB: Z>115\x00b0Z<245\x00b0" });
            ddlBFTripA.Location = new Point(0x62, 0x3b);
            ddlBFTripA.Name = "ddlBFTripA";
            ddlBFTripA.Size = new Size(0xdd, 20);
            ddlBFTripA.TabIndex = 0xd4;
            ddlBFTripA.SelectedIndexChanged += new EventHandler(ddlBFTripA_SelectedIndexChanged);
            ddlZLockA.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            ddlZLockA.FormattingEnabled = true;
            ddlZLockA.Items.AddRange(new object[] { "14\x00b0", "18\x00b0", "22\x00b0", "26\x00b0", "30\x00b0", "34\x00b0", "38\x00b0", "43\x00b0" });
            ddlZLockA.Location = new Point(0x62, 0x1a);
            ddlZLockA.Name = "ddlZLockA";
            ddlZLockA.Size = new Size(0xdd, 0x15);
            ddlZLockA.TabIndex = 0xd3;
            ddlZLockA.SelectedIndexChanged += new EventHandler(ddlZLockA_SelectedIndexChanged);
            label266.AutoSize = true;
            label266.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            label266.Location = new Point(0x16, 0x20);
            label266.Name = "label266";
            label266.Size = new Size(0x4a, 13);
            label266.TabIndex = 210;
            label266.Text = "Z- Lock Angle";
            rdoClrDebouncePL.AutoSize = true;
            rdoClrDebouncePL.Checked = true;
            rdoClrDebouncePL.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            rdoClrDebouncePL.Location = new Point(0xc6, 0xa7);
            rdoClrDebouncePL.Name = "rdoClrDebouncePL";
            rdoClrDebouncePL.Size = new Size(0x66, 0x11);
            rdoClrDebouncePL.TabIndex = 200;
            rdoClrDebouncePL.TabStop = true;
            rdoClrDebouncePL.Text = "Clear Debounce";
            rdoClrDebouncePL.UseVisualStyleBackColor = true;
            rdoClrDebouncePL.CheckedChanged += new EventHandler(rdoClrDebouncePL_CheckedChanged);
            rdoDecDebouncePL.AutoSize = true;
            rdoDecDebouncePL.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            rdoDecDebouncePL.Location = new Point(0x2f, 0xa7);
            rdoDecDebouncePL.Name = "rdoDecDebouncePL";
            rdoDecDebouncePL.Size = new Size(130, 0x11);
            rdoDecDebouncePL.TabIndex = 0xc7;
            rdoDecDebouncePL.Text = "Decrement Debounce";
            rdoDecDebouncePL.UseVisualStyleBackColor = true;
            rdoDecDebouncePL.CheckedChanged += new EventHandler(rdoDecDebouncePL_CheckedChanged);
            chkEnablePL.AutoSize = true;
            chkEnablePL.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            chkEnablePL.Location = new Point(11, 0x26);
            chkEnablePL.Name = "chkEnablePL";
            chkEnablePL.Size = new Size(0x5e, 0x11);
            chkEnablePL.TabIndex = 0xca;
            chkEnablePL.Text = "Enable P/L ";
            chkEnablePL.UseVisualStyleBackColor = true;
            chkEnablePL.CheckedChanged += new EventHandler(chkEnablePL_CheckedChanged);
            p7.BackColor = Color.LightSlateGray;
            p7.Controls.Add(label191);
            p7.Controls.Add(ledCurPLNew);
            p7.Controls.Add(label273);
            p7.Controls.Add(ledCurPLRight);
            p7.Controls.Add(label275);
            p7.Controls.Add(ledCurPLLeft);
            p7.Controls.Add(label277);
            p7.Controls.Add(ledCurPLDown);
            p7.Controls.Add(label279);
            p7.Controls.Add(ledCurPLUp);
            p7.Controls.Add(label281);
            p7.Controls.Add(ledCurPLBack);
            p7.Controls.Add(label283);
            p7.Controls.Add(ledCurPLFront);
            p7.Controls.Add(ledCurPLLO);
            p7.Controls.Add(label285);
            p7.ForeColor = Color.White;
            p7.Location = new Point(0x2dd, 0x100);
            p7.Name = "p7";
            p7.Size = new Size(0x164, 0xc2);
            p7.TabIndex = 0xcc;
            label191.AutoSize = true;
            label191.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label191.ForeColor = Color.White;
            label191.Location = new Point(0x1d, 0x12);
            label191.Name = "label191";
            label191.Size = new Size(0x10d, 20);
            label191.TabIndex = 0xcb;
            label191.Text = "New Portrait/Landscape Position";
            ledCurPLNew.ForeColor = Color.White;
            ledCurPLNew.LedStyle = LedStyle.Round3D;
            ledCurPLNew.Location = new Point(0x130, 10);
            ledCurPLNew.Name = "ledCurPLNew";
            ledCurPLNew.OffColor = Color.Red;
            ledCurPLNew.Size = new Size(0x2a, 0x2a);
            ledCurPLNew.TabIndex = 0xca;
            label273.AutoSize = true;
            label273.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label273.ForeColor = Color.White;
            label273.Location = new Point(0xea, 0x9d);
            label273.Name = "label273";
            label273.Size = new Size(0x2c, 0x10);
            label273.TabIndex = 200;
            label273.Text = "Right";
            ledCurPLRight.ForeColor = Color.White;
            ledCurPLRight.LedStyle = LedStyle.Round3D;
            ledCurPLRight.Location = new Point(0x116, 0x9a);
            ledCurPLRight.Name = "ledCurPLRight";
            ledCurPLRight.OffColor = Color.Red;
            ledCurPLRight.Size = new Size(0x16, 0x16);
            ledCurPLRight.TabIndex = 0xc7;
            label275.AutoSize = true;
            label275.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label275.ForeColor = Color.White;
            label275.Location = new Point(0xf2, 0x85);
            label275.Name = "label275";
            label275.Size = new Size(0x21, 0x10);
            label275.TabIndex = 0xc6;
            label275.Text = "Left";
            ledCurPLLeft.ForeColor = Color.White;
            ledCurPLLeft.LedStyle = LedStyle.Round3D;
            ledCurPLLeft.Location = new Point(0x116, 0x83);
            ledCurPLLeft.Name = "ledCurPLLeft";
            ledCurPLLeft.OffColor = Color.Red;
            ledCurPLLeft.Size = new Size(0x16, 0x16);
            ledCurPLLeft.TabIndex = 0xc5;
            label277.AutoSize = true;
            label277.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label277.ForeColor = Color.White;
            label277.Location = new Point(0xe8, 0x6f);
            label277.Name = "label277";
            label277.Size = new Size(0x2e, 0x10);
            label277.TabIndex = 0xc4;
            label277.Text = "Down";
            ledCurPLDown.ForeColor = Color.White;
            ledCurPLDown.LedStyle = LedStyle.Round3D;
            ledCurPLDown.Location = new Point(0x116, 0x6d);
            ledCurPLDown.Name = "ledCurPLDown";
            ledCurPLDown.OffColor = Color.Red;
            ledCurPLDown.Size = new Size(0x16, 0x16);
            ledCurPLDown.TabIndex = 0xc3;
            label279.AutoSize = true;
            label279.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label279.ForeColor = Color.White;
            label279.Location = new Point(0xf5, 0x59);
            label279.Name = "label279";
            label279.Size = new Size(0x1c, 0x10);
            label279.TabIndex = 0xc2;
            label279.Text = "Up";
            ledCurPLUp.ForeColor = Color.White;
            ledCurPLUp.LedStyle = LedStyle.Round3D;
            ledCurPLUp.Location = new Point(0x116, 0x56);
            ledCurPLUp.Name = "ledCurPLUp";
            ledCurPLUp.OffColor = Color.Red;
            ledCurPLUp.Size = new Size(0x16, 0x16);
            ledCurPLUp.TabIndex = 0xc1;
            label281.AutoSize = true;
            label281.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label281.ForeColor = Color.White;
            label281.Location = new Point(0x87, 0x71);
            label281.Name = "label281";
            label281.Size = new Size(0x2b, 0x10);
            label281.TabIndex = 0xc0;
            label281.Text = "Back";
            ledCurPLBack.ForeColor = Color.White;
            ledCurPLBack.LedStyle = LedStyle.Round3D;
            ledCurPLBack.Location = new Point(0xb2, 110);
            ledCurPLBack.Name = "ledCurPLBack";
            ledCurPLBack.OffColor = Color.Red;
            ledCurPLBack.Size = new Size(0x16, 0x16);
            ledCurPLBack.TabIndex = 0xbf;
            label283.AutoSize = true;
            label283.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label283.ForeColor = Color.White;
            label283.Location = new Point(0x84, 0x59);
            label283.Name = "label283";
            label283.Size = new Size(0x2b, 0x10);
            label283.TabIndex = 190;
            label283.Text = "Front";
            ledCurPLFront.ForeColor = Color.White;
            ledCurPLFront.LedStyle = LedStyle.Round3D;
            ledCurPLFront.Location = new Point(0xb2, 0x56);
            ledCurPLFront.Name = "ledCurPLFront";
            ledCurPLFront.OffColor = Color.Red;
            ledCurPLFront.Size = new Size(0x16, 0x16);
            ledCurPLFront.TabIndex = 0xbd;
            ledCurPLLO.ForeColor = Color.White;
            ledCurPLLO.LedStyle = LedStyle.Round3D;
            ledCurPLLO.Location = new Point(0x4b, 0x58);
            ledCurPLLO.Name = "ledCurPLLO";
            ledCurPLLO.OffColor = Color.Red;
            ledCurPLLO.Size = new Size(0x16, 0x16);
            ledCurPLLO.TabIndex = 0xbb;
            label285.AutoSize = true;
            label285.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label285.ForeColor = Color.White;
            label285.Location = new Point(14, 0x59);
            label285.Name = "label285";
            label285.Size = new Size(0x3e, 0x10);
            label285.TabIndex = 0xbc;
            label285.Text = "Lockout";
            MainScreenEval.BackColor = Color.SlateGray;
            MainScreenEval.BorderStyle = BorderStyle.Fixed3D;
            MainScreenEval.Controls.Add(chkEnableZ);
            MainScreenEval.Controls.Add(chkEnableY);
            MainScreenEval.Controls.Add(chkEnableX);
            MainScreenEval.Controls.Add(pSOS);
            MainScreenEval.Controls.Add(label60);
            MainScreenEval.Controls.Add(btnViewData);
            MainScreenEval.Controls.Add(btnDisableData);
            MainScreenEval.Controls.Add(p14b8bSelect);
            MainScreenEval.Controls.Add(panel3);
            MainScreenEval.Controls.Add(MainScreenGraph);
            MainScreenEval.Controls.Add(gbwfs);
            MainScreenEval.Controls.Add(gbIF);
            MainScreenEval.Controls.Add(gbASS);
            MainScreenEval.ForeColor = Color.White;
            MainScreenEval.Location = new Point(4, 0x19);
            MainScreenEval.Name = "MainScreenEval";
            MainScreenEval.Padding = new Padding(3);
            MainScreenEval.Size = new Size(0x464, 0x20d);
            MainScreenEval.TabIndex = 0;
            MainScreenEval.Text = "Main Screen";
            MainScreenEval.UseVisualStyleBackColor = true;
            chkEnableZ.AutoSize = true;
            chkEnableZ.Checked = true;
            chkEnableZ.CheckState = CheckState.Checked;
            chkEnableZ.Location = new Point(0xba, 0x14d);
            chkEnableZ.Name = "chkEnableZ";
            chkEnableZ.Size = new Size(70, 20);
            chkEnableZ.TabIndex = 0xf6;
            chkEnableZ.Text = "Z-Axis";
            chkEnableZ.UseVisualStyleBackColor = true;
            chkEnableZ.CheckedChanged += new EventHandler(chkEnableZ_CheckedChanged);
            chkEnableY.AutoSize = true;
            chkEnableY.Checked = true;
            chkEnableY.CheckState = CheckState.Checked;
            chkEnableY.Location = new Point(0x74, 0x14d);
            chkEnableY.Name = "chkEnableY";
            chkEnableY.Size = new Size(0x47, 20);
            chkEnableY.TabIndex = 0xf5;
            chkEnableY.Text = "Y-Axis";
            chkEnableY.UseVisualStyleBackColor = true;
            chkEnableY.CheckedChanged += new EventHandler(chkEnableY_CheckedChanged);
            chkEnableX.AutoSize = true;
            chkEnableX.Checked = true;
            chkEnableX.CheckState = CheckState.Checked;
            chkEnableX.Location = new Point(0x2f, 0x14d);
            chkEnableX.Name = "chkEnableX";
            chkEnableX.Size = new Size(70, 20);
            chkEnableX.TabIndex = 0xf4;
            chkEnableX.Text = "X-Axis";
            chkEnableX.UseVisualStyleBackColor = true;
            chkEnableX.CheckedChanged += new EventHandler(chkEnableX_CheckedChanged);
            pSOS.Controls.Add(label18);
            pSOS.Controls.Add(rdoSOSHiResMode);
            pSOS.Controls.Add(label16);
            pSOS.Controls.Add(rdoSOSLPMode);
            pSOS.Controls.Add(rdoSOSLNLPMode);
            pSOS.Controls.Add(rdoSOSNormalMode);
            pSOS.Location = new Point(0x26b, -2);
            pSOS.Name = "pSOS";
            pSOS.Size = new Size(0x1f7, 0x34);
            pSOS.TabIndex = 0xea;
            label18.AutoSize = true;
            label18.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label18.Location = new Point(5, 15);
            label18.Name = "label18";
            label18.Size = new Size(0x5c, 0x10);
            label18.TabIndex = 0xec;
            label18.Text = "Sleep Mode";
            rdoSOSHiResMode.AutoSize = true;
            rdoSOSHiResMode.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
            rdoSOSHiResMode.Location = new Point(0x11d, 0x1c);
            rdoSOSHiResMode.Name = "rdoSOSHiResMode";
            rdoSOSHiResMode.Size = new Size(0x44, 0x13);
            rdoSOSHiResMode.TabIndex = 0xe0;
            rdoSOSHiResMode.Text = "Hi Res";
            rdoSOSHiResMode.UseVisualStyleBackColor = true;
            rdoSOSHiResMode.CheckedChanged += new EventHandler(rdoSOSHiResMode_CheckedChanged);
            label16.AutoSize = true;
            label16.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label16.Location = new Point(0x61, 0x10);
            label16.Name = "label16";
            label16.Size = new Size(0xa5, 0x10);
            label16.TabIndex = 0xeb;
            label16.Text = "Oversampling Options ";
            rdoSOSLPMode.AutoSize = true;
            rdoSOSLPMode.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
            rdoSOSLPMode.Location = new Point(0x165, 3);
            rdoSOSLPMode.Name = "rdoSOSLPMode";
            rdoSOSLPMode.Size = new Size(0x5f, 0x13);
            rdoSOSLPMode.TabIndex = 0xdf;
            rdoSOSLPMode.Text = "Low Power";
            rdoSOSLPMode.UseVisualStyleBackColor = true;
            rdoSOSLPMode.CheckedChanged += new EventHandler(rdoSOSLPMode_CheckedChanged);
            rdoSOSLNLPMode.AutoSize = true;
            rdoSOSLNLPMode.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
            rdoSOSLNLPMode.Location = new Point(0x165, 0x1c);
            rdoSOSLNLPMode.Name = "rdoSOSLNLPMode";
            rdoSOSLNLPMode.Size = new Size(140, 0x13);
            rdoSOSLNLPMode.TabIndex = 0xdd;
            rdoSOSLNLPMode.Text = "Low Noise+Power";
            rdoSOSLNLPMode.UseVisualStyleBackColor = true;
            rdoSOSLNLPMode.CheckedChanged += new EventHandler(rdoSOSLNLPMode_CheckedChanged);
            rdoSOSNormalMode.AutoSize = true;
            rdoSOSNormalMode.Checked = true;
            rdoSOSNormalMode.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
            rdoSOSNormalMode.Location = new Point(0x11c, 3);
            rdoSOSNormalMode.Name = "rdoSOSNormalMode";
            rdoSOSNormalMode.Size = new Size(0x48, 0x13);
            rdoSOSNormalMode.TabIndex = 220;
            rdoSOSNormalMode.TabStop = true;
            rdoSOSNormalMode.Text = "Normal";
            rdoSOSNormalMode.UseVisualStyleBackColor = true;
            rdoSOSNormalMode.CheckedChanged += new EventHandler(rdoSOSNormalMode_CheckedChanged);
            label60.AutoSize = true;
            label60.Font = new Font("Microsoft Sans Serif", 18f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label60.ForeColor = Color.White;
            label60.Location = new Point(0x2d5, 200);
            label60.Name = "label60";
            label60.Size = new Size(0x130, 0x1d);
            label60.TabIndex = 0xe9;
            label60.Text = "System Interrupt Settings";
            btnViewData.BackColor = Color.LightSlateGray;
            btnViewData.FlatAppearance.BorderColor = Color.FromArgb(0, 0xc0, 0);
            btnViewData.FlatAppearance.BorderSize = 5;
            btnViewData.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnViewData.ForeColor = Color.White;
            btnViewData.Location = new Point(470, 0x147);
            btnViewData.Name = "btnViewData";
            btnViewData.Size = new Size(0x39, 0x21);
            btnViewData.TabIndex = 0xe2;
            btnViewData.Text = "View";
            btnViewData.UseVisualStyleBackColor = false;
            btnViewData.Click += new EventHandler(btnViewData_Click);
            btnDisableData.BackColor = Color.LightSlateGray;
            btnDisableData.FlatAppearance.BorderColor = Color.FromArgb(0xc0, 0, 0);
            btnDisableData.FlatAppearance.BorderSize = 5;
            btnDisableData.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnDisableData.ForeColor = Color.Transparent;
            btnDisableData.Location = new Point(0x213, 0x147);
            btnDisableData.Name = "btnDisableData";
            btnDisableData.Size = new Size(0x52, 0x21);
            btnDisableData.TabIndex = 0xe3;
            btnDisableData.Text = "Disable";
            btnDisableData.UseVisualStyleBackColor = false;
            btnDisableData.Click += new EventHandler(btnDisableData_Click);
            p14b8bSelect.BackColor = Color.LightSlateGray;
            p14b8bSelect.Controls.Add(rdoXYZFullResMain);
            p14b8bSelect.Controls.Add(rdo8bitDataMain);
            p14b8bSelect.Location = new Point(0x11a, 0x149);
            p14b8bSelect.Name = "p14b8bSelect";
            p14b8bSelect.Size = new Size(0xb9, 0x20);
            p14b8bSelect.TabIndex = 0xe1;
            rdoXYZFullResMain.AutoSize = true;
            rdoXYZFullResMain.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            rdoXYZFullResMain.ForeColor = Color.White;
            rdoXYZFullResMain.Location = new Point(4, 5);
            rdoXYZFullResMain.Name = "rdoXYZFullResMain";
            rdoXYZFullResMain.Size = new Size(0x5c, 20);
            rdoXYZFullResMain.TabIndex = 0xdf;
            rdoXYZFullResMain.Text = "XYZ14-bit";
            rdoXYZFullResMain.UseVisualStyleBackColor = true;
            rdoXYZFullResMain.CheckedChanged += new EventHandler(rdoXYZFullResMain_CheckedChanged);
            rdo8bitDataMain.AutoSize = true;
            rdo8bitDataMain.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            rdo8bitDataMain.ForeColor = Color.White;
            rdo8bitDataMain.Location = new Point(0x62, 5);
            rdo8bitDataMain.Name = "rdo8bitDataMain";
            rdo8bitDataMain.Size = new Size(0x54, 20);
            rdo8bitDataMain.TabIndex = 0xde;
            rdo8bitDataMain.Text = "XYZ8-bit";
            rdo8bitDataMain.UseVisualStyleBackColor = true;
            rdo8bitDataMain.CheckedChanged += new EventHandler(rdo8bitDataMain_CheckedChanged);
            panel3.BackColor = Color.LightSlateGray;
            panel3.Controls.Add(btnAutoCal);
            panel3.Controls.Add(gbOC);
            panel3.Controls.Add(gbXD);
            panel3.Controls.Add(legend1);
            panel3.Controls.Add(gbST);
            panel3.Location = new Point(3, 0x16e);
            panel3.Name = "panel3";
            panel3.Size = new Size(0x265, 0x99);
            panel3.TabIndex = 0xe4;
            btnAutoCal.BackColor = Color.LightSlateGray;
            btnAutoCal.FlatAppearance.BorderColor = Color.Fuchsia;
            btnAutoCal.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnAutoCal.ForeColor = Color.White;
            btnAutoCal.Location = new Point(0xf3, 0x66);
            btnAutoCal.Name = "btnAutoCal";
            btnAutoCal.Size = new Size(0x60, 0x2c);
            btnAutoCal.TabIndex = 0x19;
            btnAutoCal.Text = "Auto Calibrate";
            toolTip1.SetToolTip(btnAutoCal, "Calibrates the XYZ offset.  Keep board in the x=0g,y=0g and z=+/-1g position");
            btnAutoCal.UseVisualStyleBackColor = false;
            btnAutoCal.Click += new EventHandler(btnAutoCal_Click);
            gbOC.BackColor = Color.LightSlateGray;
            gbOC.Controls.Add(label34);
            gbOC.Controls.Add(label46);
            gbOC.Controls.Add(label47);
            gbOC.Controls.Add(btnWriteCal);
            gbOC.Controls.Add(label43);
            gbOC.Controls.Add(txtCalX);
            gbOC.Controls.Add(label44);
            gbOC.Controls.Add(label45);
            gbOC.Controls.Add(txtCalY);
            gbOC.Controls.Add(txtCalZ);
            gbOC.Controls.Add(lblXCal);
            gbOC.Controls.Add(lblYCal);
            gbOC.Controls.Add(lblZCal);
            gbOC.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            gbOC.ForeColor = Color.White;
            gbOC.Location = new Point(3, 0x41);
            gbOC.Name = "gbOC";
            gbOC.Size = new Size(0xe1, 0x53);
            gbOC.TabIndex = 0x7b;
            gbOC.TabStop = false;
            gbOC.Text = "Offset Calibration";
            label34.AutoSize = true;
            label34.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label34.ForeColor = Color.White;
            label34.Location = new Point(0x4f, 0x16);
            label34.Name = "label34";
            label34.Size = new Size(0x2d, 13);
            label34.TabIndex = 0x16;
            label34.Text = "counts";
            label46.AutoSize = true;
            label46.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label46.ForeColor = Color.White;
            label46.Location = new Point(0x4f, 0x29);
            label46.Name = "label46";
            label46.Size = new Size(0x2d, 13);
            label46.TabIndex = 0x17;
            label46.Text = "counts";
            label47.AutoSize = true;
            label47.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label47.ForeColor = Color.White;
            label47.Location = new Point(0x4f, 60);
            label47.Name = "label47";
            label47.Size = new Size(0x2d, 13);
            label47.TabIndex = 0x18;
            label47.Text = "counts";
            btnWriteCal.BackColor = Color.LightSlateGray;
            btnWriteCal.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnWriteCal.ForeColor = Color.White;
            btnWriteCal.Location = new Point(0xa5, 0x1f);
            btnWriteCal.Name = "btnWriteCal";
            btnWriteCal.Size = new Size(0x38, 0x24);
            btnWriteCal.TabIndex = 0x15;
            btnWriteCal.Text = "write";
            toolTip1.SetToolTip(btnWriteCal, "Based on 8g count values. This will write the offset values to the XYZ calibration registers");
            btnWriteCal.UseVisualStyleBackColor = false;
            btnWriteCal.Click += new EventHandler(btnWriteCal_Click);
            label43.AutoSize = true;
            label43.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label43.ForeColor = Color.White;
            label43.Location = new Point(4, 0x17);
            label43.Name = "label43";
            label43.Size = new Size(0x30, 13);
            label43.TabIndex = 12;
            label43.Text = "XCal = ";
            txtCalX.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtCalX.Location = new Point(0x7f, 0x11);
            txtCalX.Name = "txtCalX";
            txtCalX.Size = new Size(0x23, 20);
            txtCalX.TabIndex = 13;
            txtCalX.Text = "0";
            label44.AutoSize = true;
            label44.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label44.ForeColor = Color.White;
            label44.Location = new Point(5, 0x29);
            label44.Name = "label44";
            label44.Size = new Size(0x30, 13);
            label44.TabIndex = 14;
            label44.Text = "YCal = ";
            label45.AutoSize = true;
            label45.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label45.ForeColor = Color.White;
            label45.Location = new Point(5, 60);
            label45.Name = "label45";
            label45.Size = new Size(0x30, 13);
            label45.TabIndex = 15;
            label45.Text = "ZCal = ";
            txtCalY.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtCalY.Location = new Point(0x7f, 0x25);
            txtCalY.Name = "txtCalY";
            txtCalY.Size = new Size(0x23, 20);
            txtCalY.TabIndex = 0x10;
            txtCalY.Text = "0";
            txtCalZ.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtCalZ.Location = new Point(0x7f, 0x39);
            txtCalZ.Name = "txtCalZ";
            txtCalZ.Size = new Size(0x23, 20);
            txtCalZ.TabIndex = 0x11;
            txtCalZ.Text = "0";
            lblXCal.AutoSize = true;
            lblXCal.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblXCal.ForeColor = Color.White;
            lblXCal.Location = new Point(50, 0x17);
            lblXCal.Name = "lblXCal";
            lblXCal.Size = new Size(0x20, 13);
            lblXCal.TabIndex = 0x12;
            lblXCal.Text = "Xcal";
            lblYCal.AutoSize = true;
            lblYCal.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblYCal.ForeColor = Color.White;
            lblYCal.Location = new Point(50, 0x2a);
            lblYCal.Name = "lblYCal";
            lblYCal.Size = new Size(0x20, 13);
            lblYCal.TabIndex = 0x13;
            lblYCal.Text = "Ycal";
            lblZCal.AutoSize = true;
            lblZCal.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblZCal.ForeColor = Color.White;
            lblZCal.Location = new Point(50, 0x3d);
            lblZCal.Name = "lblZCal";
            lblZCal.Size = new Size(0x20, 13);
            lblZCal.TabIndex = 20;
            lblZCal.Text = "Zcal";
            gbXD.BackColor = Color.LightSlateGray;
            gbXD.Controls.Add(pFullResData);
            gbXD.Controls.Add(label115);
            gbXD.Controls.Add(label116);
            gbXD.Controls.Add(label117);
            gbXD.Controls.Add(lblZ8bC);
            gbXD.Controls.Add(lblY8bC);
            gbXD.Controls.Add(lblX8bC);
            gbXD.Controls.Add(label48);
            gbXD.Controls.Add(label110);
            gbXD.Controls.Add(label111);
            gbXD.Controls.Add(lblZ8bG);
            gbXD.Controls.Add(lblY8bG);
            gbXD.Controls.Add(label53);
            gbXD.Controls.Add(lblX8bG);
            gbXD.Controls.Add(label56);
            gbXD.Controls.Add(label58);
            gbXD.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            gbXD.ForeColor = Color.White;
            gbXD.Location = new Point(0x166, 4);
            gbXD.Name = "gbXD";
            gbXD.Size = new Size(0xf9, 0x60);
            gbXD.TabIndex = 0x7a;
            gbXD.TabStop = false;
            gbXD.Text = "XYZ Data";
            pFullResData.BackColor = Color.LightSlateGray;
            pFullResData.Controls.Add(label118);
            pFullResData.Controls.Add(label119);
            pFullResData.Controls.Add(label120);
            pFullResData.Controls.Add(lblY12bC);
            pFullResData.Controls.Add(lblX12bC);
            pFullResData.Controls.Add(lblZ12bC);
            pFullResData.Controls.Add(label112);
            pFullResData.Controls.Add(label113);
            pFullResData.Controls.Add(label114);
            pFullResData.Controls.Add(lblXFull);
            pFullResData.Controls.Add(lblYFull);
            pFullResData.Controls.Add(lblZFull);
            pFullResData.Controls.Add(lblY12bG);
            pFullResData.Controls.Add(lblX12bG);
            pFullResData.Controls.Add(lblZ12bG);
            pFullResData.Location = new Point(6, 0x61);
            pFullResData.Name = "pFullResData";
            pFullResData.Size = new Size(240, 0x3f);
            pFullResData.TabIndex = 0x35;
            label118.AutoSize = true;
            label118.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label118.ForeColor = Color.Green;
            label118.Location = new Point(0xb7, 0x1b);
            label118.Name = "label118";
            label118.Size = new Size(0x2d, 13);
            label118.TabIndex = 0x31;
            label118.Text = "counts";
            label119.AutoSize = true;
            label119.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label119.ForeColor = Color.DarkRed;
            label119.Location = new Point(0xb7, 9);
            label119.Name = "label119";
            label119.Size = new Size(0x2d, 13);
            label119.TabIndex = 0x2f;
            label119.Text = "counts";
            label120.AutoSize = true;
            label120.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label120.ForeColor = Color.Gold;
            label120.Location = new Point(0xb7, 0x2b);
            label120.Name = "label120";
            label120.Size = new Size(0x2d, 13);
            label120.TabIndex = 0x30;
            label120.Text = "counts";
            lblY12bC.AutoSize = true;
            lblY12bC.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblY12bC.ForeColor = Color.Green;
            lblY12bC.Location = new Point(0x92, 0x1b);
            lblY12bC.Name = "lblY12bC";
            lblY12bC.Size = new Size(14, 13);
            lblY12bC.TabIndex = 0x2b;
            lblY12bC.Text = "0";
            lblX12bC.AutoSize = true;
            lblX12bC.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblX12bC.ForeColor = Color.DarkRed;
            lblX12bC.Location = new Point(0x92, 9);
            lblX12bC.Name = "lblX12bC";
            lblX12bC.Size = new Size(14, 13);
            lblX12bC.TabIndex = 0x29;
            lblX12bC.Text = "0";
            lblZ12bC.AutoSize = true;
            lblZ12bC.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblZ12bC.ForeColor = Color.Gold;
            lblZ12bC.Location = new Point(0x92, 0x2b);
            lblZ12bC.Name = "lblZ12bC";
            lblZ12bC.Size = new Size(14, 13);
            lblZ12bC.TabIndex = 0x2a;
            lblZ12bC.Text = "0";
            label112.AutoSize = true;
            label112.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label112.ForeColor = Color.Green;
            label112.Location = new Point(0x79, 0x1a);
            label112.Name = "label112";
            label112.Size = new Size(14, 13);
            label112.TabIndex = 0x25;
            label112.Text = "g";
            label113.AutoSize = true;
            label113.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label113.ForeColor = Color.DarkRed;
            label113.Location = new Point(0x79, 8);
            label113.Name = "label113";
            label113.Size = new Size(14, 13);
            label113.TabIndex = 0x23;
            label113.Text = "g";
            label114.AutoSize = true;
            label114.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label114.ForeColor = Color.Gold;
            label114.Location = new Point(0x79, 0x2a);
            label114.Name = "label114";
            label114.Size = new Size(14, 13);
            label114.TabIndex = 0x24;
            label114.Text = "g";
            lblXFull.AutoSize = true;
            lblXFull.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblXFull.ForeColor = Color.DarkRed;
            lblXFull.Location = new Point(20, 8);
            lblXFull.Name = "lblXFull";
            lblXFull.Size = new Size(0x3e, 13);
            lblXFull.TabIndex = 0x15;
            lblXFull.Text = "X14-bit = ";
            lblYFull.AutoSize = true;
            lblYFull.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblYFull.ForeColor = Color.Green;
            lblYFull.Location = new Point(0x13, 0x18);
            lblYFull.Name = "lblYFull";
            lblYFull.Size = new Size(0x3a, 13);
            lblYFull.TabIndex = 0x16;
            lblYFull.Text = "Y14-bit =";
            lblZFull.AutoSize = true;
            lblZFull.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblZFull.ForeColor = Color.Gold;
            lblZFull.Location = new Point(0x13, 0x29);
            lblZFull.Name = "lblZFull";
            lblZFull.Size = new Size(0x3a, 13);
            lblZFull.TabIndex = 0x17;
            lblZFull.Text = "Z14-bit =";
            lblY12bG.AutoSize = true;
            lblY12bG.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblY12bG.ForeColor = Color.Green;
            lblY12bG.Location = new Point(80, 0x1a);
            lblY12bG.Name = "lblY12bG";
            lblY12bG.Size = new Size(14, 13);
            lblY12bG.TabIndex = 0x1f;
            lblY12bG.Text = "0";
            lblX12bG.AutoSize = true;
            lblX12bG.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblX12bG.ForeColor = Color.DarkRed;
            lblX12bG.Location = new Point(80, 9);
            lblX12bG.Name = "lblX12bG";
            lblX12bG.Size = new Size(14, 13);
            lblX12bG.TabIndex = 0x1c;
            lblX12bG.Text = "0";
            lblZ12bG.AutoSize = true;
            lblZ12bG.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblZ12bG.ForeColor = Color.Gold;
            lblZ12bG.Location = new Point(80, 0x2a);
            lblZ12bG.Name = "lblZ12bG";
            lblZ12bG.Size = new Size(14, 13);
            lblZ12bG.TabIndex = 30;
            lblZ12bG.Text = "0";
            label115.AutoSize = true;
            label115.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label115.ForeColor = Color.Gold;
            label115.Location = new Point(0xb6, 0x45);
            label115.Name = "label115";
            label115.Size = new Size(0x2d, 13);
            label115.TabIndex = 0x34;
            label115.Text = "counts";
            label116.AutoSize = true;
            label116.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label116.ForeColor = Color.Green;
            label116.Location = new Point(0xb6, 0x30);
            label116.Name = "label116";
            label116.Size = new Size(0x2d, 13);
            label116.TabIndex = 0x33;
            label116.Text = "counts";
            label117.AutoSize = true;
            label117.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label117.ForeColor = Color.DarkRed;
            label117.Location = new Point(0xb6, 0x1c);
            label117.Name = "label117";
            label117.Size = new Size(0x2d, 13);
            label117.TabIndex = 50;
            label117.Text = "counts";
            lblZ8bC.AutoSize = true;
            lblZ8bC.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblZ8bC.ForeColor = Color.Gold;
            lblZ8bC.Location = new Point(0x97, 0x45);
            lblZ8bC.Name = "lblZ8bC";
            lblZ8bC.Size = new Size(14, 13);
            lblZ8bC.TabIndex = 0x2e;
            lblZ8bC.Text = "0";
            lblY8bC.AutoSize = true;
            lblY8bC.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblY8bC.ForeColor = Color.Green;
            lblY8bC.Location = new Point(0x97, 0x30);
            lblY8bC.Name = "lblY8bC";
            lblY8bC.Size = new Size(14, 13);
            lblY8bC.TabIndex = 0x2d;
            lblY8bC.Text = "0";
            lblX8bC.AutoSize = true;
            lblX8bC.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblX8bC.ForeColor = Color.DarkRed;
            lblX8bC.Location = new Point(0x97, 0x1c);
            lblX8bC.Name = "lblX8bC";
            lblX8bC.Size = new Size(14, 13);
            lblX8bC.TabIndex = 0x2c;
            lblX8bC.Text = "0";
            label48.AutoSize = true;
            label48.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label48.ForeColor = Color.Gold;
            label48.Location = new Point(0x76, 0x44);
            label48.Name = "label48";
            label48.Size = new Size(14, 13);
            label48.TabIndex = 40;
            label48.Text = "g";
            label110.AutoSize = true;
            label110.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label110.ForeColor = Color.Green;
            label110.Location = new Point(0x76, 0x2f);
            label110.Name = "label110";
            label110.Size = new Size(14, 13);
            label110.TabIndex = 0x27;
            label110.Text = "g";
            label111.AutoSize = true;
            label111.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label111.ForeColor = Color.DarkRed;
            label111.Location = new Point(0x76, 0x1b);
            label111.Name = "label111";
            label111.Size = new Size(14, 13);
            label111.TabIndex = 0x26;
            label111.Text = "g";
            lblZ8bG.AutoSize = true;
            lblZ8bG.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblZ8bG.ForeColor = Color.Gold;
            lblZ8bG.Location = new Point(0x4d, 0x44);
            lblZ8bG.Name = "lblZ8bG";
            lblZ8bG.Size = new Size(14, 13);
            lblZ8bG.TabIndex = 0x22;
            lblZ8bG.Text = "0";
            lblY8bG.AutoSize = true;
            lblY8bG.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblY8bG.ForeColor = Color.Green;
            lblY8bG.Location = new Point(0x4d, 0x30);
            lblY8bG.Name = "lblY8bG";
            lblY8bG.Size = new Size(14, 13);
            lblY8bG.TabIndex = 0x21;
            lblY8bG.Text = "0";
            label53.AutoSize = true;
            label53.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label53.ForeColor = Color.DarkRed;
            label53.Location = new Point(0x18, 0x1b);
            label53.Name = "label53";
            label53.Size = new Size(0x37, 13);
            label53.TabIndex = 0x18;
            label53.Text = "X8-bit = ";
            lblX8bG.AutoSize = true;
            lblX8bG.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblX8bG.ForeColor = Color.DarkRed;
            lblX8bG.Location = new Point(0x4d, 0x1b);
            lblX8bG.Name = "lblX8bG";
            lblX8bG.Size = new Size(14, 13);
            lblX8bG.TabIndex = 0x20;
            lblX8bG.Text = "0";
            label56.AutoSize = true;
            label56.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label56.ForeColor = Color.Gold;
            label56.Location = new Point(0x19, 0x44);
            label56.Name = "label56";
            label56.Size = new Size(0x37, 13);
            label56.TabIndex = 0x19;
            label56.Text = "Z8-bit = ";
            label58.AutoSize = true;
            label58.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label58.ForeColor = Color.Green;
            label58.Location = new Point(0x19, 0x30);
            label58.Name = "label58";
            label58.Size = new Size(0x37, 13);
            label58.TabIndex = 0x1a;
            label58.Text = "Y8-bit = ";
            legend1.Items.AddRange(new LegendItem[] { legendItem1, legendItem2, legendItem3 });
            legend1.Location = new Point(0x60, 3);
            legend1.Name = "legend1";
            legend1.Size = new Size(0x103, 0x1d);
            legend1.TabIndex = 0xe8;
            legendItem1.Source = MainScreenXAxis;
            legendItem1.Text = "X-Axis";
            MainScreenXAxis.LineColor = Color.Red;
            MainScreenXAxis.XAxis = xAxis3;
            MainScreenXAxis.YAxis = yAxis3;
            xAxis3.Caption = "Samples";
            yAxis3.Caption = "Acceleration in g's";
            yAxis3.Range = new NationalInstruments.UI.Range(-8.0, 8.0);
            legendItem2.Source = MainScreenYAxis;
            legendItem2.Text = "Y-Axis";
            MainScreenYAxis.LineColor = Color.LawnGreen;
            MainScreenYAxis.PointColor = Color.DarkGreen;
            MainScreenYAxis.XAxis = xAxis3;
            MainScreenYAxis.YAxis = yAxis3;
            legendItem3.Source = MainScreenZAxis;
            legendItem3.Text = "Z-Axis";
            MainScreenZAxis.LineColor = Color.Gold;
            MainScreenZAxis.XAxis = xAxis3;
            MainScreenZAxis.YAxis = yAxis3;
            gbST.BackColor = Color.LightSlateGray;
            gbST.Controls.Add(chkSelfTest);
            gbST.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            gbST.ForeColor = Color.White;
            gbST.Location = new Point(4, 4);
            gbST.Name = "gbST";
            gbST.Size = new Size(0x58, 0x2a);
            gbST.TabIndex = 0xc9;
            gbST.TabStop = false;
            gbST.Text = "Self Test";
            gbST.Visible = false;
            chkSelfTest.AutoSize = true;
            chkSelfTest.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            chkSelfTest.Location = new Point(6, 0x13);
            chkSelfTest.Name = "chkSelfTest";
            chkSelfTest.Size = new Size(0x41, 0x11);
            chkSelfTest.TabIndex = 0;
            chkSelfTest.Text = "Enable";
            toolTip1.SetToolTip(chkSelfTest, "Verify sensor is working. Z axis data changes by 1g");
            chkSelfTest.UseVisualStyleBackColor = true;
            chkSelfTest.CheckedChanged += new EventHandler(chkSelfTest_CheckedChanged);
            MainScreenGraph.BackColor = Color.LightSlateGray;
            MainScreenGraph.Caption = "Real Time Output";
            MainScreenGraph.CaptionBackColor = Color.LightSlateGray;
            MainScreenGraph.ForeColor = Color.White;
            MainScreenGraph.ImmediateUpdates = true;
            MainScreenGraph.Location = new Point(-1, 1);
            MainScreenGraph.Name = "MainScreenGraph";
            MainScreenGraph.PlotAreaBorder = Border.Raised;
            MainScreenGraph.Plots.AddRange(new WaveformPlot[] { MainScreenXAxis, MainScreenYAxis, MainScreenZAxis });
            MainScreenGraph.Size = new Size(0x266, 0x141);
            MainScreenGraph.TabIndex = 0xdd;
            MainScreenGraph.XAxes.AddRange(new NationalInstruments.UI.XAxis[] { xAxis3 });
            MainScreenGraph.YAxes.AddRange(new NationalInstruments.UI.YAxis[] { yAxis3 });
            gbwfs.BackColor = Color.LightSlateGray;
            gbwfs.Controls.Add(chkWakeTrans1);
            gbwfs.Controls.Add(chkWakeTrans);
            gbwfs.Controls.Add(chkWakeFIFOGate);
            gbwfs.Controls.Add(chkWakeLP);
            gbwfs.Controls.Add(chkWakePulse);
            gbwfs.Controls.Add(chkWakeMFF1);
            gbwfs.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
            gbwfs.ForeColor = Color.White;
            gbwfs.Location = new Point(0x3cd, 0x38);
            gbwfs.Name = "gbwfs";
            gbwfs.Size = new Size(0x88, 0x87);
            gbwfs.TabIndex = 200;
            gbwfs.TabStop = false;
            gbwfs.Text = "Wake from Sleep";
            toolTip1.SetToolTip(gbwfs, "Interrupt Based Wake from Sleep Functions- must be mapped to Interrupt function in Map below");
            chkWakeTrans1.AutoSize = true;
            chkWakeTrans1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            chkWakeTrans1.Location = new Point(0x18, 0x68);
            chkWakeTrans1.Name = "chkWakeTrans1";
            chkWakeTrans1.Size = new Size(90, 0x11);
            chkWakeTrans1.TabIndex = 7;
            chkWakeTrans1.Text = "Transient 1";
            chkWakeTrans1.UseVisualStyleBackColor = true;
            chkWakeTrans1.Visible = false;
            chkWakeTrans1.CheckedChanged += new EventHandler(chkWakeTrans1_CheckedChanged);
            chkWakeTrans.AutoSize = true;
            chkWakeTrans.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            chkWakeTrans.Location = new Point(0x18, 0x26);
            chkWakeTrans.Name = "chkWakeTrans";
            chkWakeTrans.Size = new Size(0x53, 0x11);
            chkWakeTrans.TabIndex = 6;
            chkWakeTrans.Text = "Transient ";
            chkWakeTrans.UseVisualStyleBackColor = true;
            chkWakeTrans.CheckedChanged += new EventHandler(chkWakeTrans_CheckedChanged);
            chkWakeFIFOGate.AutoSize = true;
            chkWakeFIFOGate.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            chkWakeFIFOGate.Location = new Point(0x18, 0x17);
            chkWakeFIFOGate.Name = "chkWakeFIFOGate";
            chkWakeFIFOGate.Size = new Size(0x54, 0x11);
            chkWakeFIFOGate.TabIndex = 1;
            chkWakeFIFOGate.Text = "FIFO Gate";
            chkWakeFIFOGate.UseVisualStyleBackColor = true;
            chkWakeFIFOGate.CheckedChanged += new EventHandler(chkWakeFIFOGate_CheckedChanged);
            chkWakeLP.AutoSize = true;
            chkWakeLP.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            chkWakeLP.Location = new Point(0x18, 0x36);
            chkWakeLP.Name = "chkWakeLP";
            chkWakeLP.Size = new Size(0x58, 0x11);
            chkWakeLP.TabIndex = 5;
            chkWakeLP.Text = "Orientation";
            chkWakeLP.UseVisualStyleBackColor = true;
            chkWakeLP.CheckedChanged += new EventHandler(chkWakeLP_CheckedChanged);
            chkWakePulse.AutoSize = true;
            chkWakePulse.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            chkWakePulse.Location = new Point(0x18, 70);
            chkWakePulse.Name = "chkWakePulse";
            chkWakePulse.Size = new Size(0x39, 0x11);
            chkWakePulse.TabIndex = 2;
            chkWakePulse.Text = "Pulse";
            chkWakePulse.UseVisualStyleBackColor = true;
            chkWakePulse.CheckedChanged += new EventHandler(chkWakePulse_CheckedChanged);
            chkWakeMFF1.AutoSize = true;
            chkWakeMFF1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            chkWakeMFF1.Location = new Point(0x18, 0x57);
            chkWakeMFF1.Name = "chkWakeMFF1";
            chkWakeMFF1.Size = new Size(0x54, 0x11);
            chkWakeMFF1.TabIndex = 3;
            chkWakeMFF1.Text = "Motion/FF";
            chkWakeMFF1.UseVisualStyleBackColor = true;
            chkWakeMFF1.CheckedChanged += new EventHandler(chkWakeMFF1_CheckedChanged);
            gbIF.BackColor = Color.LightSlateGray;
            gbIF.Controls.Add(p9);
            gbIF.Controls.Add(p8);
            gbIF.Controls.Add(label66);
            gbIF.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            gbIF.ForeColor = Color.White;
            gbIF.Location = new Point(0x26d, 0xe8);
            gbIF.Name = "gbIF";
            gbIF.Size = new Size(0x1e8, 0x11e);
            gbIF.TabIndex = 0xc6;
            gbIF.TabStop = false;
            p9.BackColor = Color.LightSlateGray;
            p9.Controls.Add(ledTrans1);
            p9.Controls.Add(ledOrient);
            p9.Controls.Add(ledASleep);
            p9.Controls.Add(ledMFF1);
            p9.Controls.Add(ledPulse);
            p9.Controls.Add(ledTrans);
            p9.Controls.Add(ledDataReady);
            p9.Controls.Add(ledFIFO);
            p9.Enabled = false;
            p9.Location = new Point(40, 0x24);
            p9.Name = "p9";
            p9.Size = new Size(0x2d, 0xf5);
            p9.TabIndex = 0xc6;
            ledTrans1.LedStyle = LedStyle.Round3D;
            ledTrans1.Location = new Point(12, 0xd1);
            ledTrans1.Name = "ledTrans1";
            ledTrans1.OffColor = Color.Red;
            ledTrans1.Size = new Size(30, 30);
            ledTrans1.TabIndex = 0x9d;
            ledTrans1.Visible = false;
            ledOrient.LedStyle = LedStyle.Round3D;
            ledOrient.Location = new Point(12, 0x98);
            ledOrient.Name = "ledOrient";
            ledOrient.OffColor = Color.Red;
            ledOrient.Size = new Size(30, 30);
            ledOrient.TabIndex = 0x9c;
            ledASleep.LedStyle = LedStyle.Round3D;
            ledASleep.Location = new Point(12, 0xb6);
            ledASleep.Name = "ledASleep";
            ledASleep.OffColor = Color.Red;
            ledASleep.Size = new Size(30, 30);
            ledASleep.TabIndex = 0x9c;
            ledMFF1.LedStyle = LedStyle.Round3D;
            ledMFF1.Location = new Point(12, 0x7a);
            ledMFF1.Name = "ledMFF1";
            ledMFF1.OffColor = Color.Red;
            ledMFF1.Size = new Size(30, 30);
            ledMFF1.TabIndex = 0x9c;
            ledPulse.LedStyle = LedStyle.Round3D;
            ledPulse.Location = new Point(12, 0x5c);
            ledPulse.Name = "ledPulse";
            ledPulse.OffColor = Color.Red;
            ledPulse.Size = new Size(30, 30);
            ledPulse.TabIndex = 0x9c;
            ledTrans.LedStyle = LedStyle.Round3D;
            ledTrans.Location = new Point(12, 0x3e);
            ledTrans.Name = "ledTrans";
            ledTrans.OffColor = Color.Red;
            ledTrans.Size = new Size(30, 30);
            ledTrans.TabIndex = 0x9c;
            ledDataReady.LedStyle = LedStyle.Round3D;
            ledDataReady.Location = new Point(12, 0x20);
            ledDataReady.Name = "ledDataReady";
            ledDataReady.OffColor = Color.Red;
            ledDataReady.Size = new Size(30, 30);
            ledDataReady.TabIndex = 0x9c;
            ledFIFO.LedStyle = LedStyle.Round3D;
            ledFIFO.Location = new Point(12, 3);
            ledFIFO.Name = "ledFIFO";
            ledFIFO.OffColor = Color.Red;
            ledFIFO.Size = new Size(30, 30);
            ledFIFO.TabIndex = 0x9c;
            p8.Controls.Add(panel18);
            p8.Controls.Add(panel12);
            p8.Controls.Add(panel11);
            p8.Controls.Add(panel10);
            p8.Controls.Add(panel9);
            p8.Controls.Add(panel4);
            p8.Controls.Add(panel8);
            p8.Controls.Add(panel6);
            p8.Location = new Point(0x53, 0x25);
            p8.Name = "p8";
            p8.Size = new Size(0x17e, 0xf4);
            p8.TabIndex = 0xc7;
            panel18.Controls.Add(rdoTrans1INT_I1);
            panel18.Controls.Add(rdoTrans1INT_I2);
            panel18.Controls.Add(chkEnIntTrans1);
            panel18.Controls.Add(label36);
            panel18.Location = new Point(1, 0xce);
            panel18.Name = "panel18";
            panel18.Size = new Size(0x173, 0x20);
            panel18.TabIndex = 0xc6;
            panel18.Visible = false;
            rdoTrans1INT_I1.AutoSize = true;
            rdoTrans1INT_I1.BackColor = Color.LightSlateGray;
            rdoTrans1INT_I1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            rdoTrans1INT_I1.Location = new Point(0xd1, 7);
            rdoTrans1INT_I1.Name = "rdoTrans1INT_I1";
            rdoTrans1INT_I1.Size = new Size(0x35, 0x11);
            rdoTrans1INT_I1.TabIndex = 0x9a;
            rdoTrans1INT_I1.Text = "INT1";
            rdoTrans1INT_I1.UseVisualStyleBackColor = false;
            rdoTrans1INT_I1.CheckedChanged += new EventHandler(rdoTrans1INT_I1_CheckedChanged);
            rdoTrans1INT_I2.AutoSize = true;
            rdoTrans1INT_I2.BackColor = Color.LightSlateGray;
            rdoTrans1INT_I2.Checked = true;
            rdoTrans1INT_I2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            rdoTrans1INT_I2.Location = new Point(270, 7);
            rdoTrans1INT_I2.Name = "rdoTrans1INT_I2";
            rdoTrans1INT_I2.Size = new Size(0x35, 0x11);
            rdoTrans1INT_I2.TabIndex = 0x9b;
            rdoTrans1INT_I2.TabStop = true;
            rdoTrans1INT_I2.Text = "INT2";
            rdoTrans1INT_I2.UseVisualStyleBackColor = false;
            rdoTrans1INT_I2.CheckedChanged += new EventHandler(rdoTrans1INT_I2_CheckedChanged);
            chkEnIntTrans1.AutoSize = true;
            chkEnIntTrans1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            chkEnIntTrans1.Location = new Point(0x72, 6);
            chkEnIntTrans1.Name = "chkEnIntTrans1";
            chkEnIntTrans1.Size = new Size(0x59, 0x11);
            chkEnIntTrans1.TabIndex = 0x88;
            chkEnIntTrans1.Text = "INT enable";
            chkEnIntTrans1.UseVisualStyleBackColor = true;
            chkEnIntTrans1.CheckedChanged += new EventHandler(chkEnIntTrans1_CheckedChanged);
            label36.AutoSize = true;
            label36.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label36.Location = new Point(4, 5);
            label36.Name = "label36";
            label36.Size = new Size(0x63, 20);
            label36.TabIndex = 0xb0;
            label36.Text = "Transient 1";
            panel12.Controls.Add(rdoFIFOINT_I1);
            panel12.Controls.Add(rdoFIFOINT_I2);
            panel12.Controls.Add(chkEnIntFIFO);
            panel12.Controls.Add(lblntFIFO);
            panel12.Location = new Point(1, 2);
            panel12.Name = "panel12";
            panel12.Size = new Size(0x173, 0x1c);
            panel12.TabIndex = 0xc5;
            rdoFIFOINT_I1.AutoSize = true;
            rdoFIFOINT_I1.BackColor = Color.LightSlateGray;
            rdoFIFOINT_I1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            rdoFIFOINT_I1.Location = new Point(0xd1, 4);
            rdoFIFOINT_I1.Name = "rdoFIFOINT_I1";
            rdoFIFOINT_I1.Size = new Size(0x35, 0x11);
            rdoFIFOINT_I1.TabIndex = 0x9a;
            rdoFIFOINT_I1.Text = "INT1";
            rdoFIFOINT_I1.UseVisualStyleBackColor = false;
            rdoFIFOINT_I1.CheckedChanged += new EventHandler(rdoFIFOINT_I1_CheckedChanged);
            rdoFIFOINT_I2.AutoSize = true;
            rdoFIFOINT_I2.BackColor = Color.LightSlateGray;
            rdoFIFOINT_I2.Checked = true;
            rdoFIFOINT_I2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            rdoFIFOINT_I2.Location = new Point(270, 4);
            rdoFIFOINT_I2.Name = "rdoFIFOINT_I2";
            rdoFIFOINT_I2.Size = new Size(0x35, 0x11);
            rdoFIFOINT_I2.TabIndex = 0x9b;
            rdoFIFOINT_I2.TabStop = true;
            rdoFIFOINT_I2.Text = "INT2";
            rdoFIFOINT_I2.UseVisualStyleBackColor = false;
            rdoFIFOINT_I2.CheckedChanged += new EventHandler(rdoFIFOINT_I2_CheckedChanged);
            chkEnIntFIFO.AutoSize = true;
            chkEnIntFIFO.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            chkEnIntFIFO.Location = new Point(0x73, 6);
            chkEnIntFIFO.Name = "chkEnIntFIFO";
            chkEnIntFIFO.Size = new Size(0x59, 0x11);
            chkEnIntFIFO.TabIndex = 0x88;
            chkEnIntFIFO.Text = "INT enable";
            chkEnIntFIFO.UseVisualStyleBackColor = true;
            chkEnIntFIFO.CheckedChanged += new EventHandler(chkEnIntFIFO_CheckedChanged);
            lblntFIFO.AutoSize = true;
            lblntFIFO.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblntFIFO.Location = new Point(2, 4);
            lblntFIFO.Name = "lblntFIFO";
            lblntFIFO.Size = new Size(50, 20);
            lblntFIFO.TabIndex = 0xb0;
            lblntFIFO.Text = "FIFO";
            panel11.Controls.Add(rdoDRINT_I1);
            panel11.Controls.Add(rdoDRINT_I2);
            panel11.Controls.Add(chkEnIntDR);
            panel11.Controls.Add(label6);
            panel11.Location = new Point(1, 30);
            panel11.Name = "panel11";
            panel11.Size = new Size(0x173, 30);
            panel11.TabIndex = 0xc4;
            rdoDRINT_I1.AutoSize = true;
            rdoDRINT_I1.BackColor = Color.LightSlateGray;
            rdoDRINT_I1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            rdoDRINT_I1.Location = new Point(0xd1, 7);
            rdoDRINT_I1.Name = "rdoDRINT_I1";
            rdoDRINT_I1.Size = new Size(0x35, 0x11);
            rdoDRINT_I1.TabIndex = 0x9a;
            rdoDRINT_I1.Text = "INT1";
            rdoDRINT_I1.UseVisualStyleBackColor = false;
            rdoDRINT_I1.CheckedChanged += new EventHandler(rdoDRINT_I1_CheckedChanged);
            rdoDRINT_I2.AutoSize = true;
            rdoDRINT_I2.BackColor = Color.LightSlateGray;
            rdoDRINT_I2.Checked = true;
            rdoDRINT_I2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            rdoDRINT_I2.Location = new Point(270, 7);
            rdoDRINT_I2.Name = "rdoDRINT_I2";
            rdoDRINT_I2.Size = new Size(0x35, 0x11);
            rdoDRINT_I2.TabIndex = 0x9b;
            rdoDRINT_I2.TabStop = true;
            rdoDRINT_I2.Text = "INT2";
            rdoDRINT_I2.UseVisualStyleBackColor = false;
            rdoDRINT_I2.CheckedChanged += new EventHandler(rdoDRINT_I2_CheckedChanged);
            chkEnIntDR.AutoSize = true;
            chkEnIntDR.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            chkEnIntDR.Location = new Point(0x73, 7);
            chkEnIntDR.Name = "chkEnIntDR";
            chkEnIntDR.Size = new Size(0x59, 0x11);
            chkEnIntDR.TabIndex = 0x88;
            chkEnIntDR.Text = "INT enable";
            chkEnIntDR.UseVisualStyleBackColor = true;
            chkEnIntDR.CheckedChanged += new EventHandler(chkEnIntDR_CheckedChanged);
            label6.AutoSize = true;
            label6.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label6.Location = new Point(2, 4);
            label6.Name = "label6";
            label6.Size = new Size(0x68, 20);
            label6.TabIndex = 0xb0;
            label6.Text = "Data Ready";
            panel10.Controls.Add(rdoTransINT_I1);
            panel10.Controls.Add(rdoTransINT_I2);
            panel10.Controls.Add(chkEnIntTrans);
            panel10.Controls.Add(label3);
            panel10.Location = new Point(1, 60);
            panel10.Name = "panel10";
            panel10.Size = new Size(0x173, 0x20);
            panel10.TabIndex = 0xc4;
            rdoTransINT_I1.AutoSize = true;
            rdoTransINT_I1.BackColor = Color.LightSlateGray;
            rdoTransINT_I1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            rdoTransINT_I1.Location = new Point(0xd1, 7);
            rdoTransINT_I1.Name = "rdoTransINT_I1";
            rdoTransINT_I1.Size = new Size(0x35, 0x11);
            rdoTransINT_I1.TabIndex = 0x9a;
            rdoTransINT_I1.Text = "INT1";
            rdoTransINT_I1.UseVisualStyleBackColor = false;
            rdoTransINT_I1.CheckedChanged += new EventHandler(rdoTransINT_I1_CheckedChanged);
            rdoTransINT_I2.AutoSize = true;
            rdoTransINT_I2.BackColor = Color.LightSlateGray;
            rdoTransINT_I2.Checked = true;
            rdoTransINT_I2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            rdoTransINT_I2.Location = new Point(270, 7);
            rdoTransINT_I2.Name = "rdoTransINT_I2";
            rdoTransINT_I2.Size = new Size(0x35, 0x11);
            rdoTransINT_I2.TabIndex = 0x9b;
            rdoTransINT_I2.TabStop = true;
            rdoTransINT_I2.Text = "INT2";
            rdoTransINT_I2.UseVisualStyleBackColor = false;
            rdoTransINT_I2.CheckedChanged += new EventHandler(rdoTransINT_I2_CheckedChanged);
            chkEnIntTrans.AutoSize = true;
            chkEnIntTrans.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            chkEnIntTrans.Location = new Point(0x72, 6);
            chkEnIntTrans.Name = "chkEnIntTrans";
            chkEnIntTrans.Size = new Size(0x59, 0x11);
            chkEnIntTrans.TabIndex = 0x88;
            chkEnIntTrans.Text = "INT enable";
            chkEnIntTrans.UseVisualStyleBackColor = true;
            chkEnIntTrans.CheckedChanged += new EventHandler(chkEnIntTrans_CheckedChanged);
            label3.AutoSize = true;
            label3.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(4, 5);
            label3.Name = "label3";
            label3.Size = new Size(0x59, 20);
            label3.TabIndex = 0xb0;
            label3.Text = "Transient ";
            panel9.Controls.Add(rdoPulseINT_I1);
            panel9.Controls.Add(rdoPulseINT_I2);
            panel9.Controls.Add(chkEnIntPulse);
            panel9.Controls.Add(label2);
            panel9.Location = new Point(1, 0x5d);
            panel9.Name = "panel9";
            panel9.Size = new Size(0x173, 0x1d);
            panel9.TabIndex = 0xc4;
            rdoPulseINT_I1.AutoSize = true;
            rdoPulseINT_I1.BackColor = Color.LightSlateGray;
            rdoPulseINT_I1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            rdoPulseINT_I1.Location = new Point(0xd1, 5);
            rdoPulseINT_I1.Name = "rdoPulseINT_I1";
            rdoPulseINT_I1.Size = new Size(0x35, 0x11);
            rdoPulseINT_I1.TabIndex = 0x9a;
            rdoPulseINT_I1.Text = "INT1";
            rdoPulseINT_I1.UseVisualStyleBackColor = false;
            rdoPulseINT_I1.CheckedChanged += new EventHandler(rdoPulseINT_I1_CheckedChanged);
            rdoPulseINT_I2.AutoSize = true;
            rdoPulseINT_I2.BackColor = Color.LightSlateGray;
            rdoPulseINT_I2.Checked = true;
            rdoPulseINT_I2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            rdoPulseINT_I2.Location = new Point(270, 5);
            rdoPulseINT_I2.Name = "rdoPulseINT_I2";
            rdoPulseINT_I2.Size = new Size(0x35, 0x11);
            rdoPulseINT_I2.TabIndex = 0x9b;
            rdoPulseINT_I2.TabStop = true;
            rdoPulseINT_I2.Text = "INT2";
            rdoPulseINT_I2.UseVisualStyleBackColor = false;
            rdoPulseINT_I2.CheckedChanged += new EventHandler(rdoPulseINT_I2_CheckedChanged);
            chkEnIntPulse.AutoSize = true;
            chkEnIntPulse.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            chkEnIntPulse.Location = new Point(0x72, 5);
            chkEnIntPulse.Name = "chkEnIntPulse";
            chkEnIntPulse.Size = new Size(0x59, 0x11);
            chkEnIntPulse.TabIndex = 0x88;
            chkEnIntPulse.Text = "INT enable";
            chkEnIntPulse.UseVisualStyleBackColor = true;
            chkEnIntPulse.CheckedChanged += new EventHandler(chkEnIntPulse_CheckedChanged);
            label2.AutoSize = true;
            label2.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(3, 4);
            label2.Name = "label2";
            label2.Size = new Size(0x35, 20);
            label2.TabIndex = 0xb0;
            label2.Text = "Pulse";
            panel4.Controls.Add(rdoASleepINT_I1);
            panel4.Controls.Add(rdoASleepINT_I2);
            panel4.Controls.Add(chkEnIntASleep);
            panel4.Controls.Add(label1);
            panel4.Location = new Point(1, 0xb3);
            panel4.Name = "panel4";
            panel4.Size = new Size(0x173, 0x1f);
            panel4.TabIndex = 0xc2;
            rdoASleepINT_I1.AutoSize = true;
            rdoASleepINT_I1.BackColor = Color.LightSlateGray;
            rdoASleepINT_I1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            rdoASleepINT_I1.Location = new Point(0xd1, 7);
            rdoASleepINT_I1.Name = "rdoASleepINT_I1";
            rdoASleepINT_I1.Size = new Size(0x35, 0x11);
            rdoASleepINT_I1.TabIndex = 0x9a;
            rdoASleepINT_I1.Text = "INT1";
            rdoASleepINT_I1.UseVisualStyleBackColor = false;
            rdoASleepINT_I1.CheckedChanged += new EventHandler(rdoASleepINT_I1_CheckedChanged);
            rdoASleepINT_I2.AutoSize = true;
            rdoASleepINT_I2.BackColor = Color.LightSlateGray;
            rdoASleepINT_I2.Checked = true;
            rdoASleepINT_I2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            rdoASleepINT_I2.Location = new Point(270, 7);
            rdoASleepINT_I2.Name = "rdoASleepINT_I2";
            rdoASleepINT_I2.Size = new Size(0x35, 0x11);
            rdoASleepINT_I2.TabIndex = 0x9b;
            rdoASleepINT_I2.TabStop = true;
            rdoASleepINT_I2.Text = "INT2";
            rdoASleepINT_I2.UseVisualStyleBackColor = false;
            rdoASleepINT_I2.CheckedChanged += new EventHandler(rdoASleepINT_I2_CheckedChanged);
            chkEnIntASleep.AutoSize = true;
            chkEnIntASleep.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            chkEnIntASleep.Location = new Point(0x72, 8);
            chkEnIntASleep.Name = "chkEnIntASleep";
            chkEnIntASleep.Size = new Size(0x59, 0x11);
            chkEnIntASleep.TabIndex = 0x88;
            chkEnIntASleep.Text = "INT enable";
            chkEnIntASleep.UseVisualStyleBackColor = true;
            chkEnIntASleep.CheckedChanged += new EventHandler(chkEnIntASleep_CheckedChanged);
            label1.AutoSize = true;
            label1.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(6, 5);
            label1.Name = "label1";
            label1.Size = new Size(0x62, 20);
            label1.TabIndex = 0xb0;
            label1.Text = "Auto Sleep";
            panel8.Controls.Add(rdoMFF1INT_I1);
            panel8.Controls.Add(rdoMFF1INT_I2);
            panel8.Controls.Add(chkEnIntMFF1);
            panel8.Controls.Add(label4);
            panel8.Location = new Point(1, 0x79);
            panel8.Name = "panel8";
            panel8.Size = new Size(0x173, 30);
            panel8.TabIndex = 0xc4;
            rdoMFF1INT_I1.AutoSize = true;
            rdoMFF1INT_I1.BackColor = Color.LightSlateGray;
            rdoMFF1INT_I1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            rdoMFF1INT_I1.Location = new Point(0xd1, 6);
            rdoMFF1INT_I1.Name = "rdoMFF1INT_I1";
            rdoMFF1INT_I1.Size = new Size(0x35, 0x11);
            rdoMFF1INT_I1.TabIndex = 0x9a;
            rdoMFF1INT_I1.Text = "INT1";
            rdoMFF1INT_I1.UseVisualStyleBackColor = false;
            rdoMFF1INT_I1.CheckedChanged += new EventHandler(rdoMFF1INT_I1_CheckedChanged);
            rdoMFF1INT_I2.AutoSize = true;
            rdoMFF1INT_I2.BackColor = Color.LightSlateGray;
            rdoMFF1INT_I2.Checked = true;
            rdoMFF1INT_I2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            rdoMFF1INT_I2.Location = new Point(270, 6);
            rdoMFF1INT_I2.Name = "rdoMFF1INT_I2";
            rdoMFF1INT_I2.Size = new Size(0x35, 0x11);
            rdoMFF1INT_I2.TabIndex = 0x9b;
            rdoMFF1INT_I2.TabStop = true;
            rdoMFF1INT_I2.Text = "INT2";
            rdoMFF1INT_I2.UseVisualStyleBackColor = false;
            rdoMFF1INT_I2.CheckedChanged += new EventHandler(rdoMFF1INT_I2_CheckedChanged);
            chkEnIntMFF1.AutoSize = true;
            chkEnIntMFF1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            chkEnIntMFF1.Location = new Point(0x72, 6);
            chkEnIntMFF1.Name = "chkEnIntMFF1";
            chkEnIntMFF1.Size = new Size(0x59, 0x11);
            chkEnIntMFF1.TabIndex = 0x88;
            chkEnIntMFF1.Text = "INT enable";
            chkEnIntMFF1.UseVisualStyleBackColor = true;
            chkEnIntMFF1.CheckedChanged += new EventHandler(chkEnIntMFF1_CheckedChanged);
            label4.AutoSize = true;
            label4.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.Location = new Point(3, 5);
            label4.Name = "label4";
            label4.Size = new Size(90, 20);
            label4.TabIndex = 0xb0;
            label4.Text = "Motion FF";
            panel6.Controls.Add(rdoPLINT_I1);
            panel6.Controls.Add(rdoPLINT_I2);
            panel6.Controls.Add(chkEnIntPL);
            panel6.Controls.Add(label172);
            panel6.Location = new Point(1, 150);
            panel6.Name = "panel6";
            panel6.Size = new Size(0x173, 0x1f);
            panel6.TabIndex = 0xc3;
            rdoPLINT_I1.AutoSize = true;
            rdoPLINT_I1.BackColor = Color.LightSlateGray;
            rdoPLINT_I1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            rdoPLINT_I1.Location = new Point(0xd1, 6);
            rdoPLINT_I1.Name = "rdoPLINT_I1";
            rdoPLINT_I1.Size = new Size(0x35, 0x11);
            rdoPLINT_I1.TabIndex = 0x9a;
            rdoPLINT_I1.Text = "INT1";
            rdoPLINT_I1.UseVisualStyleBackColor = false;
            rdoPLINT_I1.CheckedChanged += new EventHandler(rdoPLINT_I1_CheckedChanged);
            rdoPLINT_I2.AutoSize = true;
            rdoPLINT_I2.BackColor = Color.LightSlateGray;
            rdoPLINT_I2.Checked = true;
            rdoPLINT_I2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            rdoPLINT_I2.Location = new Point(270, 6);
            rdoPLINT_I2.Name = "rdoPLINT_I2";
            rdoPLINT_I2.Size = new Size(0x35, 0x11);
            rdoPLINT_I2.TabIndex = 0x9b;
            rdoPLINT_I2.TabStop = true;
            rdoPLINT_I2.Text = "INT2";
            rdoPLINT_I2.UseVisualStyleBackColor = false;
            rdoPLINT_I2.CheckedChanged += new EventHandler(rdoPLINT_I2_CheckedChanged);
            chkEnIntPL.AutoSize = true;
            chkEnIntPL.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            chkEnIntPL.Location = new Point(0x72, 7);
            chkEnIntPL.Name = "chkEnIntPL";
            chkEnIntPL.Size = new Size(0x59, 0x11);
            chkEnIntPL.TabIndex = 0x88;
            chkEnIntPL.Text = "INT enable";
            chkEnIntPL.UseVisualStyleBackColor = true;
            chkEnIntPL.CheckedChanged += new EventHandler(chkEnIntPL_CheckedChanged);
            label172.AutoSize = true;
            label172.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label172.Location = new Point(4, 6);
            label172.Name = "label172";
            label172.Size = new Size(0x62, 20);
            label172.TabIndex = 0xb0;
            label172.Text = "Orientation";
            label66.AutoSize = true;
            label66.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Underline | FontStyle.Bold, GraphicsUnit.Point, 0);
            label66.ForeColor = Color.White;
            label66.Location = new Point(0x11d, 0x13);
            label66.Name = "label66";
            label66.Size = new Size(0x7d, 0x10);
            label66.TabIndex = 0xae;
            label66.Text = "Interrupt  Routing";
            gbASS.BackColor = Color.LightSlateGray;
            gbASS.Controls.Add(pnlAutoSleep);
            gbASS.Controls.Add(chkEnableASleep);
            gbASS.FlatStyle = FlatStyle.Popup;
            gbASS.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            gbASS.ForeColor = Color.White;
            gbASS.Location = new Point(0x26e, 0x39);
            gbASS.Name = "gbASS";
            gbASS.Size = new Size(0x159, 0x87);
            gbASS.TabIndex = 0xbd;
            gbASS.TabStop = false;
            gbASS.Text = "Auto Sleep Settings";
            pnlAutoSleep.Controls.Add(btnSleepTimerReset);
            pnlAutoSleep.Controls.Add(btnSetSleepTimer);
            pnlAutoSleep.Controls.Add(label199);
            pnlAutoSleep.Controls.Add(lblSleepms);
            pnlAutoSleep.Controls.Add(lblSleepTimerValue);
            pnlAutoSleep.Controls.Add(lblSleepTimer);
            pnlAutoSleep.Controls.Add(tbSleepCounter);
            pnlAutoSleep.Controls.Add(ddlSleepSR);
            pnlAutoSleep.Enabled = false;
            pnlAutoSleep.Location = new Point(5, 0x26);
            pnlAutoSleep.Name = "pnlAutoSleep";
            pnlAutoSleep.Size = new Size(0x14f, 0x5c);
            pnlAutoSleep.TabIndex = 0x92;
            btnSleepTimerReset.BackColor = Color.LightSlateGray;
            btnSleepTimerReset.Enabled = false;
            btnSleepTimerReset.Location = new Point(220, 0x17);
            btnSleepTimerReset.Name = "btnSleepTimerReset";
            btnSleepTimerReset.Size = new Size(0x3d, 0x17);
            btnSleepTimerReset.TabIndex = 0x92;
            btnSleepTimerReset.Text = "Reset";
            toolTip1.SetToolTip(btnSleepTimerReset, "This buton writes in the desired sleep time set for the device to wait to time out before switching to sleep mode.");
            btnSleepTimerReset.UseVisualStyleBackColor = false;
            btnSleepTimerReset.Click += new EventHandler(btnSleepTimerReset_Click);
            btnSetSleepTimer.BackColor = Color.LightSlateGray;
            btnSetSleepTimer.ForeColor = Color.White;
            btnSetSleepTimer.Location = new Point(0x11f, 0x17);
            btnSetSleepTimer.Name = "btnSetSleepTimer";
            btnSetSleepTimer.Size = new Size(0x27, 0x17);
            btnSetSleepTimer.TabIndex = 0x91;
            btnSetSleepTimer.Text = "Set";
            toolTip1.SetToolTip(btnSetSleepTimer, "Writes in the sleep time");
            btnSetSleepTimer.UseVisualStyleBackColor = false;
            btnSetSleepTimer.Click += new EventHandler(btnSetSleepTimer_Click);
            label199.AutoSize = true;
            label199.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label199.Location = new Point(3, 7);
            label199.Name = "label199";
            label199.Size = new Size(0x73, 13);
            label199.TabIndex = 0x8f;
            label199.Text = "Sleep Sample Rate";
            lblSleepms.AutoSize = true;
            lblSleepms.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblSleepms.Location = new Point(0x89, 30);
            lblSleepms.Name = "lblSleepms";
            lblSleepms.Size = new Size(0x16, 13);
            lblSleepms.TabIndex = 0x8e;
            lblSleepms.Text = "ms";
            lblSleepTimerValue.AutoSize = true;
            lblSleepTimerValue.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblSleepTimerValue.Location = new Point(0x52, 30);
            lblSleepTimerValue.Name = "lblSleepTimerValue";
            lblSleepTimerValue.Size = new Size(14, 13);
            lblSleepTimerValue.TabIndex = 0x8d;
            lblSleepTimerValue.Text = "0";
            lblSleepTimer.AutoSize = true;
            lblSleepTimer.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblSleepTimer.Location = new Point(2, 30);
            lblSleepTimer.Name = "lblSleepTimer";
            lblSleepTimer.Size = new Size(0x4a, 13);
            lblSleepTimer.TabIndex = 140;
            lblSleepTimer.Text = "Sleep Timer";
            tbSleepCounter.BackColor = Color.LightSlateGray;
            tbSleepCounter.Location = new Point(2, 0x31);
            tbSleepCounter.Maximum = 0xff;
            tbSleepCounter.Name = "tbSleepCounter";
            tbSleepCounter.Size = new Size(0x14c, 40);
            tbSleepCounter.TabIndex = 0x89;
            tbSleepCounter.TickFrequency = 15;
            toolTip1.SetToolTip(tbSleepCounter, "The sleep counter is the time out period to change the sample rate");
            tbSleepCounter.Scroll += new EventHandler(tbSleepCounter_Scroll);
            ddlSleepSR.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            ddlSleepSR.FormattingEnabled = true;
            ddlSleepSR.Items.AddRange(new object[] { "50 Hz", "12.5 Hz", "6.25 Hz", "1.56 Hz" });
            ddlSleepSR.Location = new Point(0x7b, 2);
            ddlSleepSR.Name = "ddlSleepSR";
            ddlSleepSR.Size = new Size(0x48, 0x15);
            ddlSleepSR.TabIndex = 0x65;
            toolTip1.SetToolTip(ddlSleepSR, "This sets the sample rate when the devie sees no interrupt or activity for the set period of time (sleep timer).");
            ddlSleepSR.SelectedIndexChanged += new EventHandler(ddlSleepSR_SelectedIndexChanged);
            chkEnableASleep.AutoSize = true;
            chkEnableASleep.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            chkEnableASleep.Location = new Point(8, 0x12);
            chkEnableASleep.Name = "chkEnableASleep";
            chkEnableASleep.Size = new Size(0x83, 0x11);
            chkEnableASleep.TabIndex = 0x90;
            chkEnableASleep.Text = "Enable Auto Sleep";
            toolTip1.SetToolTip(chkEnableASleep, "Enable Interrupt Based Sleep Functions");
            chkEnableASleep.UseVisualStyleBackColor = true;
            chkEnableASleep.CheckedChanged += new EventHandler(chkEnableASleep_CheckedChanged);
            p2.Controls.Add(label37);
            p2.Controls.Add(ddlHPFilter);
            p2.ForeColor = Color.Black;
            p2.Location = new Point(0xc0, 0x26);
            p2.Name = "p2";
            p2.Size = new Size(0xa3, 0x1a);
            p2.TabIndex = 0xda;
            label37.AutoSize = true;
            label37.BackColor = Color.LightSlateGray;
            label37.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label37.ForeColor = Color.White;
            label37.Location = new Point(14, 4);
            label37.Name = "label37";
            label37.Size = new Size(0x44, 0x10);
            label37.TabIndex = 0x65;
            label37.Text = "HP Filter";
            ddlHPFilter.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            ddlHPFilter.FormattingEnabled = true;
            ddlHPFilter.Items.AddRange(new object[] { "4 Hz", "2 Hz", "1 Hz", "0.5 Hz" });
            ddlHPFilter.Location = new Point(0x5b, 2);
            ddlHPFilter.Name = "ddlHPFilter";
            ddlHPFilter.Size = new Size(0x45, 0x15);
            ddlHPFilter.TabIndex = 100;
            toolTip1.SetToolTip(ddlHPFilter, "HP Filter 3dB Cut-off Frequency Value Selection");
            ddlHPFilter.SelectedIndexChanged += new EventHandler(ddlHPFilter_SelectedIndexChanged);
            groupBox6.BackColor = SystemColors.ActiveBorder;
            groupBox6.Controls.Add(label38);
            groupBox6.Controls.Add(chkXYZ12Log);
            groupBox6.Controls.Add(chkTransLog);
            groupBox6.Controls.Add(chkXYZ8Log);
            groupBox6.Controls.Add(lblFileName);
            groupBox6.Controls.Add(txtSaveFileName);
            groupBox6.Location = new Point(0, 0);
            groupBox6.Name = "groupBox6";
            groupBox6.Size = new Size(200, 100);
            groupBox6.TabIndex = 0xe8;
            groupBox6.TabStop = false;
            label38.AutoSize = true;
            label38.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label38.Location = new Point(0x62, 5);
            label38.Name = "label38";
            label38.Size = new Size(100, 0x10);
            label38.TabIndex = 0xda;
            label38.Text = "Datalog Data";
            chkXYZ12Log.Location = new Point(0, 0);
            chkXYZ12Log.Name = "chkXYZ12Log";
            chkXYZ12Log.Size = new Size(0x68, 0x18);
            chkXYZ12Log.TabIndex = 0xdb;
            chkTransLog.Location = new Point(0, 0);
            chkTransLog.Name = "chkTransLog";
            chkTransLog.Size = new Size(0x68, 0x18);
            chkTransLog.TabIndex = 220;
            chkXYZ8Log.Location = new Point(0, 0);
            chkXYZ8Log.Name = "chkXYZ8Log";
            chkXYZ8Log.Size = new Size(0x68, 0x18);
            chkXYZ8Log.TabIndex = 0xdd;
            lblFileName.Location = new Point(0, 0);
            lblFileName.Name = "lblFileName";
            lblFileName.Size = new Size(100, 0x17);
            lblFileName.TabIndex = 0xde;
            txtSaveFileName.Location = new Point(0, 0);
            txtSaveFileName.Name = "txtSaveFileName";
            txtSaveFileName.Size = new Size(100, 20);
            txtSaveFileName.TabIndex = 0xdf;
            gbOM.BackColor = Color.LightSlateGray;
            gbOM.Controls.Add(chkAnalogLowNoise);
            gbOM.Controls.Add(chkHPFDataOut);
            gbOM.Controls.Add(pOverSampling);
            gbOM.Controls.Add(rdoStandby);
            gbOM.Controls.Add(p2);
            gbOM.Controls.Add(rdoActive);
            gbOM.Controls.Add(ledSleep);
            gbOM.Controls.Add(lbsleep);
            gbOM.Controls.Add(label173);
            gbOM.Controls.Add(p1);
            gbOM.Controls.Add(ledStandby);
            gbOM.Controls.Add(label71);
            gbOM.Controls.Add(ledWake);
            gbOM.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            gbOM.ForeColor = Color.White;
            gbOM.Location = new Point(4, -3);
            gbOM.Name = "gbOM";
            gbOM.Size = new Size(0x26a, 0x89);
            gbOM.TabIndex = 0xc2;
            gbOM.TabStop = false;
            chkAnalogLowNoise.AutoSize = true;
            chkAnalogLowNoise.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            chkAnalogLowNoise.ForeColor = Color.White;
            chkAnalogLowNoise.Location = new Point(0x16c, 0x71);
            chkAnalogLowNoise.Name = "chkAnalogLowNoise";
            chkAnalogLowNoise.Size = new Size(0xbd, 0x11);
            chkAnalogLowNoise.TabIndex = 0xde;
            chkAnalogLowNoise.Text = "Enable Low Noise (Up to 4g)";
            toolTip1.SetToolTip(chkAnalogLowNoise, "Enable Interrupt Based Sleep Functions");
            chkAnalogLowNoise.UseVisualStyleBackColor = true;
            chkAnalogLowNoise.CheckedChanged += new EventHandler(chkAnalogLowNoise_CheckedChanged);
            chkHPFDataOut.AutoSize = true;
            chkHPFDataOut.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            chkHPFDataOut.ForeColor = Color.White;
            chkHPFDataOut.Location = new Point(0x16c, 0x5e);
            chkHPFDataOut.Name = "chkHPFDataOut";
            chkHPFDataOut.Size = new Size(0x69, 0x11);
            chkHPFDataOut.TabIndex = 0xe7;
            chkHPFDataOut.Text = "HPF Data Out";
            toolTip1.SetToolTip(chkHPFDataOut, "HPF Data");
            chkHPFDataOut.UseVisualStyleBackColor = true;
            chkHPFDataOut.CheckedChanged += new EventHandler(chkHPFDataOut_CheckedChanged);
            pOverSampling.Controls.Add(label70);
            pOverSampling.Controls.Add(rdoOSHiResMode);
            pOverSampling.Controls.Add(rdoOSLPMode);
            pOverSampling.Controls.Add(rdoOSLNLPMode);
            pOverSampling.Controls.Add(rdoOSNormalMode);
            pOverSampling.Location = new Point(4, 0x43);
            pOverSampling.Name = "pOverSampling";
            pOverSampling.Size = new Size(0x138, 0x42);
            pOverSampling.TabIndex = 0xdd;
            label70.AutoSize = true;
            label70.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label70.Location = new Point(4, 4);
            label70.Name = "label70";
            label70.Size = new Size(220, 0x10);
            label70.TabIndex = 0xde;
            label70.Text = "Oversampling Options for Data";
            rdoOSHiResMode.AutoSize = true;
            rdoOSHiResMode.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
            rdoOSHiResMode.Location = new Point(5, 0x2a);
            rdoOSHiResMode.Name = "rdoOSHiResMode";
            rdoOSHiResMode.Size = new Size(0x6c, 0x13);
            rdoOSHiResMode.TabIndex = 0xe0;
            rdoOSHiResMode.Text = "Hi Res Mode";
            rdoOSHiResMode.UseVisualStyleBackColor = true;
            rdoOSHiResMode.CheckedChanged += new EventHandler(rdoOSHiResMode_CheckedChanged);
            rdoOSLPMode.AutoSize = true;
            rdoOSLPMode.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
            rdoOSLPMode.Location = new Point(0x81, 0x16);
            rdoOSLPMode.Name = "rdoOSLPMode";
            rdoOSLPMode.Size = new Size(0x87, 0x13);
            rdoOSLPMode.TabIndex = 0xdf;
            rdoOSLPMode.Text = "Low Power Mode";
            rdoOSLPMode.UseVisualStyleBackColor = true;
            rdoOSLPMode.CheckedChanged += new EventHandler(rdoOSLPMode_CheckedChanged);
            rdoOSLNLPMode.AutoSize = true;
            rdoOSLNLPMode.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
            rdoOSLNLPMode.Location = new Point(0x81, 0x29);
            rdoOSLNLPMode.Name = "rdoOSLNLPMode";
            rdoOSLNLPMode.Size = new Size(0xa6, 0x13);
            rdoOSLNLPMode.TabIndex = 0xdd;
            rdoOSLNLPMode.Text = "Low Noise Low Power";
            rdoOSLNLPMode.UseVisualStyleBackColor = true;
            rdoOSLNLPMode.CheckedChanged += new EventHandler(rdoOSLNLPMode_CheckedChanged);
            rdoOSNormalMode.AutoSize = true;
            rdoOSNormalMode.Checked = true;
            rdoOSNormalMode.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
            rdoOSNormalMode.Location = new Point(5, 0x16);
            rdoOSNormalMode.Name = "rdoOSNormalMode";
            rdoOSNormalMode.Size = new Size(0x70, 0x13);
            rdoOSNormalMode.TabIndex = 220;
            rdoOSNormalMode.TabStop = true;
            rdoOSNormalMode.Text = "Normal Mode";
            rdoOSNormalMode.UseVisualStyleBackColor = true;
            rdoOSNormalMode.CheckedChanged += new EventHandler(rdoOSNormalMode_CheckedChanged);
            rdoStandby.AutoSize = true;
            rdoStandby.Checked = true;
            rdoStandby.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            rdoStandby.Location = new Point(6, 11);
            rdoStandby.Name = "rdoStandby";
            rdoStandby.Size = new Size(0x57, 20);
            rdoStandby.TabIndex = 0x70;
            rdoStandby.TabStop = true;
            rdoStandby.Text = "Standby ";
            toolTip1.SetToolTip(rdoStandby, "Standby Mode used to set up all embedded functions");
            rdoStandby.UseVisualStyleBackColor = true;
            rdoStandby.CheckedChanged += new EventHandler(rBStandby_CheckedChanged);
            rdoActive.AutoSize = true;
            rdoActive.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            rdoActive.Location = new Point(0x63, 11);
            rdoActive.Name = "rdoActive";
            rdoActive.Size = new Size(0x45, 20);
            rdoActive.TabIndex = 0x71;
            rdoActive.Text = "Active";
            toolTip1.SetToolTip(rdoActive, "Active Mode Set");
            rdoActive.UseVisualStyleBackColor = true;
            rdoActive.CheckedChanged += new EventHandler(rBActive_CheckedChanged);
            ledSleep.LedStyle = LedStyle.Round3D;
            ledSleep.Location = new Point(0x246, 0x41);
            ledSleep.Name = "ledSleep";
            ledSleep.OffColor = Color.Red;
            ledSleep.Size = new Size(30, 0x1f);
            ledSleep.TabIndex = 0x91;
            lbsleep.AutoSize = true;
            lbsleep.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbsleep.Location = new Point(0x1ec, 0x48);
            lbsleep.Name = "lbsleep";
            lbsleep.Size = new Size(0x5c, 0x10);
            lbsleep.TabIndex = 0xb9;
            lbsleep.Text = "Sleep Mode";
            label173.AutoSize = true;
            label173.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label173.Location = new Point(0x1dc, 0x11);
            label173.Name = "label173";
            label173.Size = new Size(0x6c, 0x10);
            label173.TabIndex = 0xc2;
            label173.Text = "Standby Mode";
            p1.BackColor = Color.LightSlateGray;
            p1.Controls.Add(ddlDataRate);
            p1.Controls.Add(label35);
            p1.Location = new Point(3, 0x22);
            p1.Name = "p1";
            p1.Size = new Size(0xba, 0x21);
            p1.TabIndex = 0x91;
            ddlDataRate.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            ddlDataRate.FormattingEnabled = true;
            ddlDataRate.Items.AddRange(new object[] { "200 Hz", "100 Hz", "50 Hz", "12.5 Hz", "6.25 Hz", "1.563 Hz" });
            ddlDataRate.Location = new Point(0x6b, 6);
            ddlDataRate.Name = "ddlDataRate";
            ddlDataRate.Size = new Size(0x4c, 0x15);
            ddlDataRate.TabIndex = 0x7d;
            toolTip1.SetToolTip(ddlDataRate, "Active Mode Sample Rate Selection");
            ddlDataRate.SelectedIndexChanged += new EventHandler(ddlDataRate_SelectedIndexChanged);
            label35.AutoSize = true;
            label35.BackColor = Color.LightSlateGray;
            label35.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label35.Location = new Point(4, 10);
            label35.Name = "label35";
            label35.Size = new Size(0x62, 0x10);
            label35.TabIndex = 0x7e;
            label35.Text = "Sample Rate";
            ledStandby.LedStyle = LedStyle.Round3D;
            ledStandby.Location = new Point(0x246, 10);
            ledStandby.Name = "ledStandby";
            ledStandby.OffColor = Color.Red;
            ledStandby.Size = new Size(30, 0x1f);
            ledStandby.TabIndex = 0xc1;
            ledStandby.Value = true;
            label71.AutoSize = true;
            label71.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label71.Location = new Point(0x1ed, 0x2c);
            label71.Name = "label71";
            label71.Size = new Size(0x5b, 0x10);
            label71.TabIndex = 0xc0;
            label71.Text = "Wake Mode";
            ledWake.LedStyle = LedStyle.Round3D;
            ledWake.Location = new Point(0x246, 0x27);
            ledWake.Name = "ledWake";
            ledWake.OffColor = Color.Red;
            ledWake.Size = new Size(30, 0x1f);
            ledWake.TabIndex = 0xbf;
            gbDR.BackColor = Color.LightSlateGray;
            gbDR.Controls.Add(lbl4gSensitivity);
            gbDR.Controls.Add(rdo2g);
            gbDR.Controls.Add(rdo8g);
            gbDR.Controls.Add(rdo4g);
            gbDR.Controls.Add(lbl2gSensitivity);
            gbDR.Controls.Add(lbl8gSensitivity);
            gbDR.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            gbDR.ForeColor = Color.White;
            gbDR.Location = new Point(0x271, 5);
            gbDR.Name = "gbDR";
            gbDR.Size = new Size(200, 0x3f);
            gbDR.TabIndex = 0xb9;
            gbDR.TabStop = false;
            gbDR.Text = "Dynamic Range";
            lbl4gSensitivity.AutoSize = true;
            lbl4gSensitivity.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbl4gSensitivity.ForeColor = Color.White;
            lbl4gSensitivity.Location = new Point(0x30, 0x2e);
            lbl4gSensitivity.Name = "lbl4gSensitivity";
            lbl4gSensitivity.Size = new Size(0x84, 0x10);
            lbl4gSensitivity.TabIndex = 0x77;
            lbl4gSensitivity.Text = "2048 counts/g 14b";
            rdo2g.AutoSize = true;
            rdo2g.Checked = true;
            rdo2g.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            rdo2g.ForeColor = Color.White;
            rdo2g.Location = new Point(5, 0x18);
            rdo2g.Name = "rdo2g";
            rdo2g.Size = new Size(0x2b, 20);
            rdo2g.TabIndex = 0x73;
            rdo2g.TabStop = true;
            rdo2g.Text = "2g";
            rdo2g.UseVisualStyleBackColor = true;
            rdo2g.CheckedChanged += new EventHandler(rb2g_CheckedChanged);
            rdo8g.AutoSize = true;
            rdo8g.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            rdo8g.ForeColor = Color.White;
            rdo8g.Location = new Point(5, 0x45);
            rdo8g.Name = "rdo8g";
            rdo8g.Size = new Size(0x2b, 20);
            rdo8g.TabIndex = 0x74;
            rdo8g.Text = "8g";
            rdo8g.UseVisualStyleBackColor = true;
            rdo8g.CheckedChanged += new EventHandler(rb8g_CheckedChanged);
            rdo4g.AutoSize = true;
            rdo4g.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            rdo4g.ForeColor = Color.White;
            rdo4g.Location = new Point(5, 0x2b);
            rdo4g.Name = "rdo4g";
            rdo4g.Size = new Size(0x2b, 20);
            rdo4g.TabIndex = 0x75;
            rdo4g.Text = "4g";
            rdo4g.UseVisualStyleBackColor = true;
            rdo4g.CheckedChanged += new EventHandler(rb4g_CheckedChanged);
            lbl2gSensitivity.AutoSize = true;
            lbl2gSensitivity.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbl2gSensitivity.ForeColor = Color.White;
            lbl2gSensitivity.Location = new Point(0x31, 0x1a);
            lbl2gSensitivity.Name = "lbl2gSensitivity";
            lbl2gSensitivity.Size = new Size(0x84, 0x10);
            lbl2gSensitivity.TabIndex = 0x76;
            lbl2gSensitivity.Text = "4096 counts/g 14b";
            lbl8gSensitivity.AutoSize = true;
            lbl8gSensitivity.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbl8gSensitivity.ForeColor = Color.White;
            lbl8gSensitivity.Location = new Point(0x30, 0x49);
            lbl8gSensitivity.Name = "lbl8gSensitivity";
            lbl8gSensitivity.Size = new Size(0x84, 0x10);
            lbl8gSensitivity.TabIndex = 120;
            lbl8gSensitivity.Text = "1024 counts/g 14b";
            TabTool.Controls.Add(MainScreenEval);
            TabTool.Controls.Add(Register_Page);
            TabTool.Controls.Add(DataConfigPage);
            TabTool.Controls.Add(MFF1_2Page);
            TabTool.Controls.Add(PL_Page);
            TabTool.Controls.Add(TransientDetection);
            TabTool.Controls.Add(PulseDetection);
            TabTool.Controls.Add(FIFOPage);
            TabTool.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            TabTool.Location = new Point(0, 0x90);
            TabTool.Name = "TabTool";
            TabTool.SelectedIndex = 0;
            TabTool.Size = new Size(0x46c, 0x22a);
            TabTool.TabIndex = 0x71;
            TabTool.SelectedIndexChanged += new EventHandler(TabTool_SelectedIndexChanged_1);
            Register_Page.BackColor = Color.SlateGray;
            Register_Page.BorderStyle = BorderStyle.Fixed3D;
            Register_Page.Controls.Add(gbRegisterName);
            Register_Page.Controls.Add(groupBox4);
            Register_Page.Controls.Add(groupBox1);
            Register_Page.Location = new Point(4, 0x19);
            Register_Page.Name = "Register_Page";
            Register_Page.Padding = new Padding(3);
            Register_Page.Size = new Size(0x464, 0x20d);
            Register_Page.TabIndex = 3;
            Register_Page.Text = "Registers";
            Register_Page.UseVisualStyleBackColor = true;
            gbRegisterName.BackColor = Color.LightSteelBlue;
            gbRegisterName.Controls.Add(lblComment2);
            gbRegisterName.Controls.Add(lblComment1);
            gbRegisterName.Controls.Add(tableLayoutPanel1);
            gbRegisterName.Location = new Point(1, 0x131);
            gbRegisterName.Name = "gbRegisterName";
            gbRegisterName.Size = new Size(0x34a, 0xd6);
            gbRegisterName.TabIndex = 0xd8;
            gbRegisterName.TabStop = false;
            gbRegisterName.Text = "Register";
            lblComment2.AutoSize = true;
            lblComment2.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblComment2.Location = new Point(15, 0x84);
            lblComment2.Name = "lblComment2";
            lblComment2.Size = new Size(0x4c, 0x10);
            lblComment2.TabIndex = 0x143;
            lblComment2.Text = "Comment:";
            lblComment1.AutoSize = true;
            lblComment1.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblComment1.Location = new Point(14, 0x65);
            lblComment1.Name = "lblComment1";
            lblComment1.Size = new Size(0x4c, 0x10);
            lblComment1.TabIndex = 0x142;
            lblComment1.Text = "Comment:";
            tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            tableLayoutPanel1.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            tableLayoutPanel1.ColumnCount = 10;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 77f));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 86f));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80f));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 86f));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 90f));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 81f));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 75f));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 81f));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 94f));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 66f));
            tableLayoutPanel1.Controls.Add(lb_BitV5, 0, 2);
            tableLayoutPanel1.Controls.Add(lb_BitV4, 0, 2);
            tableLayoutPanel1.Controls.Add(lb_BitV6, 0, 2);
            tableLayoutPanel1.Controls.Add(lb_BitV7, 0, 2);
            tableLayoutPanel1.Controls.Add(lb_BitV0, 0, 2);
            tableLayoutPanel1.Controls.Add(lb_BitV07, 0, 2);
            tableLayoutPanel1.Controls.Add(lb_BitV1, 0, 2);
            tableLayoutPanel1.Controls.Add(lb_BitV3, 0, 2);
            tableLayoutPanel1.Controls.Add(lb_BitV2, 0, 2);
            tableLayoutPanel1.Controls.Add(lb_BitN7, 0, 1);
            tableLayoutPanel1.Controls.Add(lb_BitN6, 1, 1);
            tableLayoutPanel1.Controls.Add(lb_BitN5, 2, 1);
            tableLayoutPanel1.Controls.Add(lb_BitN4, 3, 1);
            tableLayoutPanel1.Controls.Add(lb_BitN3, 4, 1);
            tableLayoutPanel1.Controls.Add(lb_BitN2, 5, 1);
            tableLayoutPanel1.Controls.Add(lb_BitN1, 6, 1);
            tableLayoutPanel1.Controls.Add(lb_BitN0, 7, 1);
            tableLayoutPanel1.Controls.Add(label25, 7, 0);
            tableLayoutPanel1.Controls.Add(label27, 6, 0);
            tableLayoutPanel1.Controls.Add(label29, 5, 0);
            tableLayoutPanel1.Controls.Add(label32, 4, 0);
            tableLayoutPanel1.Controls.Add(label39, 3, 0);
            tableLayoutPanel1.Controls.Add(label40, 2, 0);
            tableLayoutPanel1.Controls.Add(label41, 1, 0);
            tableLayoutPanel1.Controls.Add(label72, 0, 0);
            tableLayoutPanel1.Controls.Add(label96, 9, 0);
            tableLayoutPanel1.Controls.Add(label97, 9, 1);
            tableLayoutPanel1.Controls.Add(label98, 9, 2);
            tableLayoutPanel1.Controls.Add(label103, 8, 0);
            tableLayoutPanel1.Controls.Add(lb_BitN07, 8, 1);
            tableLayoutPanel1.Location = new Point(11, 0x21);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 4;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 15f));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 15f));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 15f));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20f));
            tableLayoutPanel1.Size = new Size(0x339, 0x31);
            tableLayoutPanel1.TabIndex = 1;
            lb_BitV5.AutoSize = true;
            lb_BitV5.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0xee);
            lb_BitV5.ForeColor = Color.FromArgb(0xc9, 2, 50);
            lb_BitV5.Location = new Point(0xa9, 0x21);
            lb_BitV5.Name = "lb_BitV5";
            lb_BitV5.Size = new Size(14, 13);
            lb_BitV5.TabIndex = 0x25;
            lb_BitV5.Text = "0";
            lb_BitV4.AutoSize = true;
            lb_BitV4.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0xee);
            lb_BitV4.ForeColor = Color.FromArgb(0xc9, 2, 50);
            lb_BitV4.Location = new Point(250, 0x21);
            lb_BitV4.Name = "lb_BitV4";
            lb_BitV4.Size = new Size(14, 13);
            lb_BitV4.TabIndex = 0x24;
            lb_BitV4.Text = "0";
            lb_BitV6.AutoSize = true;
            lb_BitV6.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0xee);
            lb_BitV6.ForeColor = Color.FromArgb(0xc9, 2, 50);
            lb_BitV6.Location = new Point(0x52, 0x21);
            lb_BitV6.Name = "lb_BitV6";
            lb_BitV6.Size = new Size(14, 13);
            lb_BitV6.TabIndex = 0x23;
            lb_BitV6.Text = "0";
            lb_BitV7.AutoSize = true;
            lb_BitV7.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0xee);
            lb_BitV7.ForeColor = Color.FromArgb(0xc9, 2, 50);
            lb_BitV7.Location = new Point(4, 0x21);
            lb_BitV7.Name = "lb_BitV7";
            lb_BitV7.Size = new Size(14, 13);
            lb_BitV7.TabIndex = 0x22;
            lb_BitV7.Text = "0";
            lb_BitV0.AutoSize = true;
            lb_BitV0.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0xee);
            lb_BitV0.ForeColor = Color.FromArgb(0xc9, 2, 50);
            lb_BitV0.Location = new Point(0x24a, 0x21);
            lb_BitV0.Name = "lb_BitV0";
            lb_BitV0.Size = new Size(14, 13);
            lb_BitV0.TabIndex = 0x21;
            lb_BitV0.Text = "0";
            lb_BitV07.AutoSize = true;
            lb_BitV07.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0xee);
            lb_BitV07.ForeColor = Color.FromArgb(0xc9, 2, 50);
            lb_BitV07.Location = new Point(0x29c, 0x21);
            lb_BitV07.Name = "lb_BitV07";
            lb_BitV07.Size = new Size(14, 13);
            lb_BitV07.TabIndex = 0x20;
            lb_BitV07.Text = "0";
            lb_BitV1.AutoSize = true;
            lb_BitV1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0xee);
            lb_BitV1.ForeColor = Color.FromArgb(0xc9, 2, 50);
            lb_BitV1.Location = new Point(510, 0x21);
            lb_BitV1.Name = "lb_BitV1";
            lb_BitV1.Size = new Size(14, 13);
            lb_BitV1.TabIndex = 0x1f;
            lb_BitV1.Text = "0";
            lb_BitV3.AutoSize = true;
            lb_BitV3.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0xee);
            lb_BitV3.ForeColor = Color.FromArgb(0xc9, 2, 50);
            lb_BitV3.Location = new Point(0x151, 0x21);
            lb_BitV3.Name = "lb_BitV3";
            lb_BitV3.Size = new Size(14, 13);
            lb_BitV3.TabIndex = 30;
            lb_BitV3.Text = "0";
            lb_BitV2.AutoSize = true;
            lb_BitV2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0xee);
            lb_BitV2.ForeColor = Color.FromArgb(0xc9, 2, 50);
            lb_BitV2.Location = new Point(0x1ac, 0x21);
            lb_BitV2.Name = "lb_BitV2";
            lb_BitV2.Size = new Size(14, 13);
            lb_BitV2.TabIndex = 0x1b;
            lb_BitV2.Text = "0";
            lb_BitN7.AutoSize = true;
            lb_BitN7.Font = new Font("Microsoft Sans Serif", 6.75f, FontStyle.Regular, GraphicsUnit.Point, 0xee);
            lb_BitN7.Location = new Point(4, 0x11);
            lb_BitN7.Name = "lb_BitN7";
            lb_BitN7.Size = new Size(0x2d, 12);
            lb_BitN7.TabIndex = 0x13;
            lb_BitN7.Text = "Unknown";
            lb_BitN6.AutoSize = true;
            lb_BitN6.Font = new Font("Microsoft Sans Serif", 6.75f, FontStyle.Regular, GraphicsUnit.Point, 0xee);
            lb_BitN6.Location = new Point(0x52, 0x11);
            lb_BitN6.Name = "lb_BitN6";
            lb_BitN6.Size = new Size(0x2d, 12);
            lb_BitN6.TabIndex = 0x12;
            lb_BitN6.Text = "Unknown";
            lb_BitN5.AutoSize = true;
            lb_BitN5.Font = new Font("Microsoft Sans Serif", 6.75f, FontStyle.Regular, GraphicsUnit.Point, 0xee);
            lb_BitN5.Location = new Point(0xa9, 0x11);
            lb_BitN5.Name = "lb_BitN5";
            lb_BitN5.Size = new Size(0x2d, 12);
            lb_BitN5.TabIndex = 0x11;
            lb_BitN5.Text = "Unknown";
            lb_BitN4.AutoSize = true;
            lb_BitN4.Font = new Font("Microsoft Sans Serif", 6.75f, FontStyle.Regular, GraphicsUnit.Point, 0xee);
            lb_BitN4.Location = new Point(250, 0x11);
            lb_BitN4.Name = "lb_BitN4";
            lb_BitN4.Size = new Size(0x2d, 12);
            lb_BitN4.TabIndex = 0x10;
            lb_BitN4.Text = "Unknown";
            lb_BitN3.AutoSize = true;
            lb_BitN3.Font = new Font("Microsoft Sans Serif", 6.75f, FontStyle.Regular, GraphicsUnit.Point, 0xee);
            lb_BitN3.Location = new Point(0x151, 0x11);
            lb_BitN3.Name = "lb_BitN3";
            lb_BitN3.Size = new Size(0x2d, 12);
            lb_BitN3.TabIndex = 15;
            lb_BitN3.Text = "Unknown";
            lb_BitN2.AutoSize = true;
            lb_BitN2.Font = new Font("Microsoft Sans Serif", 6.75f, FontStyle.Regular, GraphicsUnit.Point, 0xee);
            lb_BitN2.Location = new Point(0x1ac, 0x11);
            lb_BitN2.Name = "lb_BitN2";
            lb_BitN2.Size = new Size(0x2d, 12);
            lb_BitN2.TabIndex = 14;
            lb_BitN2.Text = "Unknown";
            lb_BitN1.AutoSize = true;
            lb_BitN1.Font = new Font("Microsoft Sans Serif", 6.75f, FontStyle.Regular, GraphicsUnit.Point, 0xee);
            lb_BitN1.Location = new Point(510, 0x11);
            lb_BitN1.Name = "lb_BitN1";
            lb_BitN1.Size = new Size(0x2d, 12);
            lb_BitN1.TabIndex = 13;
            lb_BitN1.Text = "Unknown";
            lb_BitN0.AutoSize = true;
            lb_BitN0.Font = new Font("Microsoft Sans Serif", 6.75f, FontStyle.Regular, GraphicsUnit.Point, 0xee);
            lb_BitN0.Location = new Point(0x24a, 0x11);
            lb_BitN0.Name = "lb_BitN0";
            lb_BitN0.Size = new Size(0x2d, 12);
            lb_BitN0.TabIndex = 12;
            lb_BitN0.Text = "Unknown";
            label25.AutoSize = true;
            label25.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0xee);
            label25.Location = new Point(0x24a, 1);
            label25.Name = "label25";
            label25.Size = new Size(0x17, 13);
            label25.TabIndex = 11;
            label25.Text = "D0";
            label27.AutoSize = true;
            label27.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0xee);
            label27.Location = new Point(510, 1);
            label27.Name = "label27";
            label27.Size = new Size(0x17, 13);
            label27.TabIndex = 10;
            label27.Text = "D1";
            label29.AutoSize = true;
            label29.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0xee);
            label29.Location = new Point(0x1ac, 1);
            label29.Name = "label29";
            label29.Size = new Size(0x17, 13);
            label29.TabIndex = 9;
            label29.Text = "D2";
            label32.AutoSize = true;
            label32.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0xee);
            label32.Location = new Point(0x151, 1);
            label32.Name = "label32";
            label32.Size = new Size(0x17, 13);
            label32.TabIndex = 8;
            label32.Text = "D3";
            label39.AutoSize = true;
            label39.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0xee);
            label39.Location = new Point(250, 1);
            label39.Name = "label39";
            label39.Size = new Size(0x17, 13);
            label39.TabIndex = 7;
            label39.Text = "D4";
            label40.AutoSize = true;
            label40.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0xee);
            label40.Location = new Point(0xa9, 1);
            label40.Name = "label40";
            label40.Size = new Size(0x17, 13);
            label40.TabIndex = 6;
            label40.Text = "D5";
            label41.AutoSize = true;
            label41.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0xee);
            label41.Location = new Point(0x52, 1);
            label41.Name = "label41";
            label41.Size = new Size(0x17, 13);
            label41.TabIndex = 5;
            label41.Text = "D6";
            label72.AutoSize = true;
            label72.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0xee);
            label72.Location = new Point(4, 1);
            label72.Name = "label72";
            label72.Size = new Size(0x17, 13);
            label72.TabIndex = 4;
            label72.Text = "D7";
            label96.AutoSize = true;
            label96.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0xee);
            label96.ForeColor = Color.Green;
            label96.Location = new Point(0x2fb, 1);
            label96.Name = "label96";
            label96.Size = new Size(0x16, 13);
            label96.TabIndex = 1;
            label96.Text = "Bit";
            label97.AutoSize = true;
            label97.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0xee);
            label97.ForeColor = Color.Green;
            label97.Location = new Point(0x2fb, 0x11);
            label97.Name = "label97";
            label97.Size = new Size(0x38, 13);
            label97.TabIndex = 2;
            label97.Text = "Function";
            label98.AutoSize = true;
            label98.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0xee);
            label98.ForeColor = Color.Green;
            label98.Location = new Point(0x2fb, 0x21);
            label98.Name = "label98";
            label98.Size = new Size(0x27, 13);
            label98.TabIndex = 3;
            label98.Text = "Value";
            label103.AutoSize = true;
            label103.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0xee);
            label103.Location = new Point(0x29c, 1);
            label103.Name = "label103";
            label103.Size = new Size(0x33, 13);
            label103.TabIndex = 0x1c;
            label103.Text = "D0 - D8";
            lb_BitN07.AutoSize = true;
            lb_BitN07.Font = new Font("Microsoft Sans Serif", 6.75f, FontStyle.Regular, GraphicsUnit.Point, 0xee);
            lb_BitN07.Location = new Point(0x29c, 0x11);
            lb_BitN07.Name = "lb_BitN07";
            lb_BitN07.Size = new Size(0x18, 12);
            lb_BitN07.TabIndex = 0x1d;
            lb_BitN07.Text = "HEX";
            groupBox4.BackColor = Color.LightSteelBlue;
            groupBox4.Controls.Add(btnWrite);
            groupBox4.Controls.Add(lblRegValue);
            groupBox4.Controls.Add(btnRead);
            groupBox4.Controls.Add(label51);
            groupBox4.Controls.Add(txtRegRead);
            groupBox4.Controls.Add(label49);
            groupBox4.Controls.Add(txtRegWrite);
            groupBox4.Controls.Add(txtRegValue);
            groupBox4.Controls.Add(label42);
            groupBox4.ForeColor = Color.Black;
            groupBox4.Location = new Point(0x34e, 0x132);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(0x110, 0x67);
            groupBox4.TabIndex = 0xd7;
            groupBox4.TabStop = false;
            groupBox4.Text = "Manual Read/Write Hex Only";
            btnWrite.Location = new Point(0xcb, 0x41);
            btnWrite.Name = "btnWrite";
            btnWrite.Size = new Size(0x42, 0x1c);
            btnWrite.TabIndex = 0xce;
            btnWrite.Text = "WRITE";
            btnWrite.UseVisualStyleBackColor = true;
            btnWrite.Click += new EventHandler(btnWrite_Click);
            lblRegValue.AutoSize = true;
            lblRegValue.Location = new Point(0xca, 0x1c);
            lblRegValue.Name = "lblRegValue";
            lblRegValue.Size = new Size(0x30, 0x10);
            lblRegValue.TabIndex = 0xd5;
            lblRegValue.Text = "Value";
            btnRead.Location = new Point(0x81, 0x15);
            btnRead.Name = "btnRead";
            btnRead.Size = new Size(0x45, 0x1c);
            btnRead.TabIndex = 0xcd;
            btnRead.Text = "READ";
            btnRead.UseVisualStyleBackColor = true;
            btnRead.Click += new EventHandler(btnRead_Click);
            label51.AutoSize = true;
            label51.Location = new Point(3, 0x1b);
            label51.Name = "label51";
            label51.Size = new Size(0x43, 0x10);
            label51.TabIndex = 0xd4;
            label51.Text = "Register";
            txtRegRead.Location = new Point(0x48, 0x18);
            txtRegRead.Name = "txtRegRead";
            txtRegRead.Size = new Size(0x33, 0x16);
            txtRegRead.TabIndex = 0xcf;
            label49.AutoSize = true;
            label49.Location = new Point(0x71, 0x47);
            label49.Name = "label49";
            label49.Size = new Size(0x30, 0x10);
            label49.TabIndex = 0xd3;
            label49.Text = "Value";
            txtRegWrite.Location = new Point(0x4a, 0x44);
            txtRegWrite.Name = "txtRegWrite";
            txtRegWrite.Size = new Size(0x23, 0x16);
            txtRegWrite.TabIndex = 0xd0;
            txtRegValue.Location = new Point(0xa1, 0x44);
            txtRegValue.Name = "txtRegValue";
            txtRegValue.Size = new Size(40, 0x16);
            txtRegValue.TabIndex = 210;
            label42.AutoSize = true;
            label42.Location = new Point(7, 0x47);
            label42.Name = "label42";
            label42.Size = new Size(0x43, 0x10);
            label42.TabIndex = 0xd1;
            label42.Text = "Register";
            groupBox1.BackColor = Color.LightSteelBlue;
            groupBox1.Controls.Add(panel1);
            groupBox1.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            groupBox1.ForeColor = Color.Black;
            groupBox1.Location = new Point(-1, 2);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(0x460, 0x12b);
            groupBox1.TabIndex = 0x53;
            groupBox1.TabStop = false;
            groupBox1.Text = "All Registers";
            panel1.Controls.Add(label7);
            panel1.Controls.Add(label10);
            panel1.Controls.Add(label12);
            panel1.Controls.Add(btnUpdateRegs);
            panel1.Controls.Add(label11);
            panel1.Controls.Add(label9);
            panel1.Controls.Add(label8);
            panel1.Controls.Add(rb31);
            panel1.Controls.Add(rb30);
            panel1.Controls.Add(rb2F);
            panel1.Controls.Add(rb2E);
            panel1.Controls.Add(rb2D);
            panel1.Controls.Add(rb2C);
            panel1.Controls.Add(rb2B);
            panel1.Controls.Add(rb2A);
            panel1.Controls.Add(rb29);
            panel1.Controls.Add(rb28);
            panel1.Controls.Add(rb27);
            panel1.Controls.Add(rb26);
            panel1.Controls.Add(rb25);
            panel1.Controls.Add(rb24);
            panel1.Controls.Add(rb23);
            panel1.Controls.Add(rb22);
            panel1.Controls.Add(rb21);
            panel1.Controls.Add(rb20);
            panel1.Controls.Add(rb1F);
            panel1.Controls.Add(rb1E);
            panel1.Controls.Add(rb1D);
            panel1.Controls.Add(rb1C);
            panel1.Controls.Add(rb1B);
            panel1.Controls.Add(rb1A);
            panel1.Controls.Add(rb19);
            panel1.Controls.Add(rb18);
            panel1.Controls.Add(rb17);
            panel1.Controls.Add(rb16);
            panel1.Controls.Add(rb15);
            panel1.Controls.Add(rb14);
            panel1.Controls.Add(rb13);
            panel1.Controls.Add(rb12);
            panel1.Controls.Add(rb11);
            panel1.Controls.Add(rb10);
            panel1.Controls.Add(rb0F);
            panel1.Controls.Add(rb0E);
            panel1.Controls.Add(rb0D);
            panel1.Controls.Add(rb0C);
            panel1.Controls.Add(rb0B);
            panel1.Controls.Add(rb0A);
            panel1.Controls.Add(rb09);
            panel1.Controls.Add(rb06);
            panel1.Controls.Add(rb05);
            panel1.Controls.Add(rb04);
            panel1.Controls.Add(rb03);
            panel1.Controls.Add(rb02);
            panel1.Controls.Add(rb01);
            panel1.Controls.Add(rbR00);
            panel1.Controls.Add(lbl1A);
            panel1.Controls.Add(lbl1B);
            panel1.Controls.Add(lbl1C);
            panel1.Controls.Add(lbl19);
            panel1.Controls.Add(label57);
            panel1.Controls.Add(label54);
            panel1.Controls.Add(lbl28);
            panel1.Controls.Add(lbl26);
            panel1.Controls.Add(lbl1E);
            panel1.Controls.Add(lbl1F);
            panel1.Controls.Add(lbl20);
            panel1.Controls.Add(lbl21);
            panel1.Controls.Add(lbl22);
            panel1.Controls.Add(lbl23);
            panel1.Controls.Add(lbl27);
            panel1.Controls.Add(lbl25);
            panel1.Controls.Add(lbl1D);
            panel1.Controls.Add(lbl24);
            panel1.Controls.Add(lbl29);
            panel1.Controls.Add(lbl31);
            panel1.Controls.Add(lbl2F);
            panel1.Controls.Add(lbl30);
            panel1.Controls.Add(lbl2E);
            panel1.Controls.Add(lbl2C);
            panel1.Controls.Add(lbl2D);
            panel1.Controls.Add(lbl2B);
            panel1.Controls.Add(lbl17);
            panel1.Controls.Add(lbl18);
            panel1.Controls.Add(lbl16);
            panel1.Controls.Add(lbl15);
            panel1.Controls.Add(lbl13);
            panel1.Controls.Add(lbl0B);
            panel1.Controls.Add(lbl0C);
            panel1.Controls.Add(lbl0D);
            panel1.Controls.Add(lbl0E);
            panel1.Controls.Add(lbl0F);
            panel1.Controls.Add(lbl10);
            panel1.Controls.Add(lbl14);
            panel1.Controls.Add(lbl12);
            panel1.Controls.Add(lbl0A);
            panel1.Controls.Add(lbl09);
            panel1.Controls.Add(lbl01);
            panel1.Controls.Add(lbl02);
            panel1.Controls.Add(lbl03);
            panel1.Controls.Add(lbl04);
            panel1.Controls.Add(lbl05);
            panel1.Controls.Add(lbl06);
            panel1.Controls.Add(lbl00);
            panel1.Controls.Add(lbl2A);
            panel1.Controls.Add(lbl11);
            panel1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            panel1.Location = new Point(6, 14);
            panel1.Name = "panel1";
            panel1.Size = new Size(0x455, 0x117);
            panel1.TabIndex = 0x4f;
            label10.AutoSize = true;
            label10.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label10.Location = new Point(0xab, 0x6c);
            label10.Name = "label10";
            label10.Size = new Size(140, 0x10);
            label10.TabIndex = 0x145;
            label10.Text = "Portrait/Landscape";
            label12.AutoSize = true;
            label12.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label12.Location = new Point(0x3b1, 0x10);
            label12.Name = "label12";
            label12.Size = new Size(0x30, 0x10);
            label12.TabIndex = 0x144;
            label12.Text = "Offset";
            btnUpdateRegs.BackColor = SystemColors.ButtonFace;
            btnUpdateRegs.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnUpdateRegs.ForeColor = Color.Black;
            btnUpdateRegs.Location = new Point(0x390, 0xbd);
            btnUpdateRegs.Name = "btnUpdateRegs";
            btnUpdateRegs.Size = new Size(0xc2, 60);
            btnUpdateRegs.TabIndex = 0xcc;
            btnUpdateRegs.Text = "Update All Registers";
            btnUpdateRegs.UseVisualStyleBackColor = false;
            btnUpdateRegs.Click += new EventHandler(btnUpdateRegs_Click);
            label11.AutoSize = true;
            label11.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label11.Location = new Point(0x2fe, 0x10);
            label11.Name = "label11";
            label11.Size = new Size(0x75, 0x10);
            label11.TabIndex = 0x143;
            label11.Text = "Control Settings";
            label9.AutoSize = true;
            label9.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label9.Location = new Point(0x174, 0x10);
            label9.Name = "label9";
            label9.Size = new Size(0x76, 0x10);
            label9.TabIndex = 0x142;
            label9.Text = "Motion/FF + Jolt";
            label8.AutoSize = true;
            label8.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label8.Location = new Point(0xab, 0x10);
            label8.Name = "label8";
            label8.Size = new Size(0x59, 0x10);
            label8.TabIndex = 0x141;
            label8.Text = "Data Config";
            rb31.AutoSize = true;
            rb31.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            rb31.Location = new Point(0x3d1, 0x47);
            rb31.Name = "rb31";
            rb31.Size = new Size(0x57, 0x11);
            rb31.TabIndex = 320;
            rb31.TabStop = true;
            rb31.Text = "R31   OFF_Z";
            rb31.UseVisualStyleBackColor = true;
            rb31.MouseHover += new EventHandler(rb31_MouseHover);
            rb30.AutoSize = true;
            rb30.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            rb30.Location = new Point(0x3d1, 0x36);
            rb30.Name = "rb30";
            rb30.Size = new Size(0x57, 0x11);
            rb30.TabIndex = 0x13f;
            rb30.TabStop = true;
            rb30.Text = "R30   OFF_Y";
            rb30.UseVisualStyleBackColor = true;
            rb30.MouseHover += new EventHandler(rb30_MouseHover);
            rb2F.AutoSize = true;
            rb2F.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            rb2F.Location = new Point(0x3d1, 0x24);
            rb2F.Name = "rb2F";
            rb2F.Size = new Size(0x57, 0x11);
            rb2F.TabIndex = 0x13e;
            rb2F.TabStop = true;
            rb2F.Text = "R2F   OFF_X";
            rb2F.UseVisualStyleBackColor = true;
            rb2F.MouseHover += new EventHandler(rb2F_MouseHover);
            rb2E.AutoSize = true;
            rb2E.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            rb2E.Location = new Point(800, 0x84);
            rb2E.Name = "rb2E";
            rb2E.Size = new Size(0x76, 0x11);
            rb2E.TabIndex = 0x13d;
            rb2E.TabStop = true;
            rb2E.Text = "R2E   CTRL_REG5";
            rb2E.UseVisualStyleBackColor = true;
            rb2E.MouseHover += new EventHandler(rb2E_MouseHover);
            rb2D.AutoSize = true;
            rb2D.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            rb2D.Location = new Point(800, 0x71);
            rb2D.Name = "rb2D";
            rb2D.Size = new Size(0x77, 0x11);
            rb2D.TabIndex = 0x13c;
            rb2D.TabStop = true;
            rb2D.Text = "R2D   CTRL_REG4";
            rb2D.UseVisualStyleBackColor = true;
            rb2D.MouseHover += new EventHandler(rb2D_MouseHover);
            rb2C.AutoSize = true;
            rb2C.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            rb2C.Location = new Point(800, 0x5e);
            rb2C.Name = "rb2C";
            rb2C.Size = new Size(0x76, 0x11);
            rb2C.TabIndex = 0x13b;
            rb2C.TabStop = true;
            rb2C.Text = "R2C   CTRL_REG3";
            rb2C.UseVisualStyleBackColor = true;
            rb2C.MouseHover += new EventHandler(rb2C_MouseHover);
            rb2B.AutoSize = true;
            rb2B.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            rb2B.Location = new Point(800, 0x4c);
            rb2B.Name = "rb2B";
            rb2B.Size = new Size(0x76, 0x11);
            rb2B.TabIndex = 0x13a;
            rb2B.TabStop = true;
            rb2B.Text = "R2B   CTRL_REG2";
            rb2B.UseVisualStyleBackColor = true;
            rb2B.MouseHover += new EventHandler(rb2B_MouseHover);
            rb2A.AutoSize = true;
            rb2A.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            rb2A.Location = new Point(800, 0x38);
            rb2A.Name = "rb2A";
            rb2A.Size = new Size(0x76, 0x11);
            rb2A.TabIndex = 0x139;
            rb2A.TabStop = true;
            rb2A.Text = "R2A   CTRL_REG1";
            rb2A.UseVisualStyleBackColor = true;
            rb2A.MouseHover += new EventHandler(rb2A_MouseHover);
            rb29.AutoSize = true;
            rb29.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            rb29.Location = new Point(800, 0x24);
            rb29.Name = "rb29";
            rb29.Size = new Size(0x7d, 0x11);
            rb29.TabIndex = 0x138;
            rb29.TabStop = true;
            rb29.Text = "R29   ASLP_COUNT";
            rb29.UseVisualStyleBackColor = true;
            rb29.MouseHover += new EventHandler(rb29_MouseHover);
            rb28.AutoSize = true;
            rb28.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            rb28.Location = new Point(0x270, 0xb1);
            rb28.Name = "rb28";
            rb28.Size = new Size(0x7d, 0x11);
            rb28.TabIndex = 0x137;
            rb28.TabStop = true;
            rb28.Text = "R28   PULSE_WIND";
            rb28.UseVisualStyleBackColor = true;
            rb28.MouseHover += new EventHandler(rb28_MouseHover);
            rb27.AutoSize = true;
            rb27.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            rb27.Location = new Point(0x270, 0x9a);
            rb27.Name = "rb27";
            rb27.Size = new Size(0x7a, 0x11);
            rb27.TabIndex = 310;
            rb27.TabStop = true;
            rb27.Text = "R27   PULSE_LTCY";
            rb27.UseVisualStyleBackColor = true;
            rb27.MouseHover += new EventHandler(rb27_MouseHover);
            rb26.AutoSize = true;
            rb26.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            rb26.Location = new Point(0x270, 0x87);
            rb26.Name = "rb26";
            rb26.Size = new Size(0x7c, 0x11);
            rb26.TabIndex = 0x135;
            rb26.TabStop = true;
            rb26.Text = "R26   PULSE_TMLT";
            rb26.UseVisualStyleBackColor = true;
            rb26.MouseHover += new EventHandler(rb26_MouseHover);
            rb25.AutoSize = true;
            rb25.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            rb25.Location = new Point(0x270, 0x73);
            rb25.Name = "rb25";
            rb25.Size = new Size(0x7c, 0x11);
            rb25.TabIndex = 0x134;
            rb25.TabStop = true;
            rb25.Text = "R25   PULSE_THSZ";
            rb25.UseVisualStyleBackColor = true;
            rb25.MouseHover += new EventHandler(rb25_MouseHover);
            rb24.AutoSize = true;
            rb24.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            rb24.Location = new Point(0x270, 0x5f);
            rb24.Name = "rb24";
            rb24.Size = new Size(0x7c, 0x11);
            rb24.TabIndex = 0x132;
            rb24.TabStop = true;
            rb24.Text = "R24   PULSE_THSY";
            rb24.UseVisualStyleBackColor = true;
            rb24.MouseHover += new EventHandler(rb24_MouseHover);
            rb23.AutoSize = true;
            rb23.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            rb23.Location = new Point(0x270, 0x4c);
            rb23.Name = "rb23";
            rb23.Size = new Size(0x7c, 0x11);
            rb23.TabIndex = 0x131;
            rb23.TabStop = true;
            rb23.Text = "R23   PULSE_THSX";
            rb23.UseVisualStyleBackColor = true;
            rb23.MouseHover += new EventHandler(rb23_MouseHover);
            rb22.AutoSize = true;
            rb22.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            rb22.Location = new Point(0x270, 0x38);
            rb22.Name = "rb22";
            rb22.Size = new Size(0x75, 0x11);
            rb22.TabIndex = 0x130;
            rb22.TabStop = true;
            rb22.Text = "R22   PULSE_SRC";
            rb22.UseVisualStyleBackColor = true;
            rb22.MouseHover += new EventHandler(rb22_MouseHover);
            rb21.AutoSize = true;
            rb21.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            rb21.Location = new Point(0x270, 0x25);
            rb21.Name = "rb21";
            rb21.Size = new Size(0x74, 0x11);
            rb21.TabIndex = 0x12f;
            rb21.TabStop = true;
            rb21.Text = "R21   PULSE_CFG";
            rb21.UseVisualStyleBackColor = true;
            rb21.MouseHover += new EventHandler(rb21_MouseHover);
            rb20.AutoSize = true;
            rb20.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            rb20.Location = new Point(0x194, 0xaf);
            rb20.Name = "rb20";
            rb20.Size = new Size(0x9d, 0x11);
            rb20.TabIndex = 0x12e;
            rb20.TabStop = true;
            rb20.Text = "R20  TRANSIENT_COUNT";
            rb20.UseVisualStyleBackColor = true;
            rb20.MouseHover += new EventHandler(rb20_MouseHover);
            rb1F.AutoSize = true;
            rb1F.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            rb1F.Location = new Point(0x194, 0x9c);
            rb1F.Name = "rb1F";
            rb1F.Size = new Size(0x8d, 0x11);
            rb1F.TabIndex = 0x12d;
            rb1F.TabStop = true;
            rb1F.Text = "R1F  TRANSIENT_THS";
            rb1F.UseVisualStyleBackColor = true;
            rb1F.MouseHover += new EventHandler(rb1F_MouseHover);
            rb1E.AutoSize = true;
            rb1E.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            rb1E.Location = new Point(0x194, 0x88);
            rb1E.Name = "rb1E";
            rb1E.Size = new Size(0x8e, 0x11);
            rb1E.TabIndex = 300;
            rb1E.TabStop = true;
            rb1E.Text = "R1E  TRANSIENT_SRC";
            rb1E.UseVisualStyleBackColor = true;
            rb1E.MouseHover += new EventHandler(rb1E_MouseHover);
            rb1D.AutoSize = true;
            rb1D.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            rb1D.Location = new Point(0x194, 0x75);
            rb1D.Name = "rb1D";
            rb1D.Size = new Size(0x8e, 0x11);
            rb1D.TabIndex = 0x12b;
            rb1D.TabStop = true;
            rb1D.Text = "R1D  TRANSIENT_CFG";
            rb1D.UseVisualStyleBackColor = true;
            rb1D.MouseHover += new EventHandler(rb1D_MouseHover);
            rb1C.AutoSize = true;
            rb1C.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            rb1C.Location = new Point(0x3ad, 0x103);
            rb1C.Name = "rb1C";
            rb1C.Size = new Size(0xa7, 0x11);
            rb1C.TabIndex = 0x12a;
            rb1C.TabStop = true;
            rb1C.Text = "R1C TRANSIENT_COUNT_1";
            rb1C.UseVisualStyleBackColor = true;
            rb1C.Visible = false;
            rb1C.MouseHover += new EventHandler(rb1C_MouseHover);
            rb1B.AutoSize = true;
            rb1B.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            rb1B.Location = new Point(0x2f1, 0x102);
            rb1B.Name = "rb1B";
            rb1B.Size = new Size(0x97, 0x11);
            rb1B.TabIndex = 0x129;
            rb1B.TabStop = true;
            rb1B.Text = "R1B TRANSIENT_THS_1";
            rb1B.UseVisualStyleBackColor = true;
            rb1B.Visible = false;
            rb1B.MouseHover += new EventHandler(rb1B_MouseHover);
            rb1A.AutoSize = true;
            rb1A.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            rb1A.Location = new Point(0x238, 0x102);
            rb1A.Name = "rb1A";
            rb1A.Size = new Size(0x97, 0x11);
            rb1A.TabIndex = 0x128;
            rb1A.TabStop = true;
            rb1A.Text = "R1A TRANSIENT_SRC_1";
            rb1A.UseVisualStyleBackColor = true;
            rb1A.Visible = false;
            rb1A.MouseHover += new EventHandler(rb1A_MouseHover);
            rb19.AutoSize = true;
            rb19.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            rb19.Location = new Point(0x17d, 0x102);
            rb19.Name = "rb19";
            rb19.Size = new Size(0x95, 0x11);
            rb19.TabIndex = 0x127;
            rb19.TabStop = true;
            rb19.Text = "R19 TRANSIENT_CFG_1";
            rb19.UseVisualStyleBackColor = true;
            rb19.Visible = false;
            rb19.MouseHover += new EventHandler(rb19_MouseHover);
            rb18.AutoSize = true;
            rb18.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            rb18.Location = new Point(0x194, 0x62);
            rb18.Name = "rb18";
            rb18.Size = new Size(0x84, 0x11);
            rb18.TabIndex = 0x126;
            rb18.TabStop = true;
            rb18.Text = "R18   FF_MT_COUNT";
            rb18.UseVisualStyleBackColor = true;
            rb18.MouseHover += new EventHandler(rb18_MouseHover);
            rb17.AutoSize = true;
            rb17.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            rb17.Location = new Point(0x194, 0x4e);
            rb17.Name = "rb17";
            rb17.Size = new Size(0x74, 0x11);
            rb17.TabIndex = 0x125;
            rb17.TabStop = true;
            rb17.Text = "R17   FF_MT_THS";
            rb17.UseVisualStyleBackColor = true;
            rb17.MouseHover += new EventHandler(rb17_MouseHover);
            rb16.AutoSize = true;
            rb16.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            rb16.Location = new Point(0x194, 0x3b);
            rb16.Name = "rb16";
            rb16.Size = new Size(0x74, 0x11);
            rb16.TabIndex = 0x124;
            rb16.TabStop = true;
            rb16.Text = "R16   FF_MT_SRC";
            rb16.UseVisualStyleBackColor = true;
            rb16.MouseHover += new EventHandler(rb16_MouseHover);
            rb15.AutoSize = true;
            rb15.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            rb15.Location = new Point(0x194, 0x27);
            rb15.Name = "rb15";
            rb15.Size = new Size(0x73, 0x11);
            rb15.TabIndex = 0x123;
            rb15.TabStop = true;
            rb15.Text = "R15   FF_MT_CFG";
            rb15.UseVisualStyleBackColor = true;
            rb15.MouseHover += new EventHandler(rb15_MouseHover);
            rb14.AutoSize = true;
            rb14.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            rb14.Location = new Point(0xc9, 0xcc);
            rb14.Name = "rb14";
            rb14.Size = new Size(0x95, 0x11);
            rb14.TabIndex = 290;
            rb14.TabStop = true;
            rb14.Text = "R14   PL_P_L_THS_REG";
            rb14.UseVisualStyleBackColor = true;
            rb14.MouseHover += new EventHandler(rb14_MouseHover);
            rb13.AutoSize = true;
            rb13.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            rb13.Location = new Point(0xc9, 0xb9);
            rb13.Name = "rb13";
            rb13.Size = new Size(130, 0x11);
            rb13.TabIndex = 0x121;
            rb13.TabStop = true;
            rb13.Text = "R13   PL_BF_ZCOMP";
            rb13.UseVisualStyleBackColor = true;
            rb13.MouseHover += new EventHandler(rb13_MouseHover);
            rb12.AutoSize = true;
            rb12.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            rb12.Location = new Point(0xc9, 0xa6);
            rb12.Name = "rb12";
            rb12.Size = new Size(0x6f, 0x11);
            rb12.TabIndex = 0x120;
            rb12.TabStop = true;
            rb12.Text = "R12   PL_COUNT";
            rb12.UseVisualStyleBackColor = true;
            rb12.MouseHover += new EventHandler(rb12_MouseHover);
            rb11.AutoSize = true;
            rb11.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            rb11.Location = new Point(0xc9, 0x92);
            rb11.Name = "rb11";
            rb11.Size = new Size(0x5e, 0x11);
            rb11.TabIndex = 0x11f;
            rb11.TabStop = true;
            rb11.Text = "R11   PL_CFG";
            rb11.UseVisualStyleBackColor = true;
            rb11.MouseHover += new EventHandler(rb11_MouseHover);
            rb10.AutoSize = true;
            rb10.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            rb10.Location = new Point(0xc9, 0x7f);
            rb10.Name = "rb10";
            rb10.Size = new Size(0x74, 0x11);
            rb10.TabIndex = 0x11e;
            rb10.TabStop = true;
            rb10.Text = "R10   PL_STATUS";
            rb10.UseVisualStyleBackColor = true;
            rb10.MouseHover += new EventHandler(rb10_MouseHover);
            rb0F.AutoSize = true;
            rb0F.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            rb0F.Location = new Point(0xc9, 0x51);
            rb0F.Name = "rb0F";
            rb0F.Size = new Size(160, 0x11);
            rb0F.TabIndex = 0x11d;
            rb0F.TabStop = true;
            rb0F.Text = "R0F   HP_FILTER_CUTOFF";
            rb0F.UseVisualStyleBackColor = true;
            rb0F.MouseHover += new EventHandler(rb0F_MouseHover);
            rb0E.AutoSize = true;
            rb0E.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            rb0E.Location = new Point(0xc9, 60);
            rb0E.Name = "rb0E";
            rb0E.Size = new Size(0x8a, 0x11);
            rb0E.TabIndex = 0x11c;
            rb0E.TabStop = true;
            rb0E.Text = "R0E   XYZ_DATA_CFG";
            rb0E.UseVisualStyleBackColor = true;
            rb0E.MouseHover += new EventHandler(rb0E_MouseHover);
            rb0D.AutoSize = true;
            rb0D.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            rb0D.Location = new Point(0xc9, 0x27);
            rb0D.Name = "rb0D";
            rb0D.Size = new Size(0x72, 0x11);
            rb0D.TabIndex = 0x11b;
            rb0D.TabStop = true;
            rb0D.Text = "R0D   WHO_AM_I";
            rb0D.UseVisualStyleBackColor = true;
            rb0D.MouseHover += new EventHandler(rb0D_MouseHover);
            rb0C.AutoSize = true;
            rb0C.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            rb0C.Location = new Point(0x24, 0x93);
            rb0C.Name = "rb0C";
            rb0C.Size = new Size(0x7c, 0x11);
            rb0C.TabIndex = 0x119;
            rb0C.TabStop = true;
            rb0C.Text = "R0C   INT_SOURCE";
            rb0C.UseVisualStyleBackColor = true;
            rb0C.MouseHover += new EventHandler(rb0C_MouseHover);
            rb0B.AutoSize = true;
            rb0B.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            rb0B.Location = new Point(0x24, 130);
            rb0B.Name = "rb0B";
            rb0B.Size = new Size(0x62, 0x11);
            rb0B.TabIndex = 280;
            rb0B.TabStop = true;
            rb0B.Text = "R0B  SYSMOD";
            rb0B.UseVisualStyleBackColor = true;
            rb0B.MouseHover += new EventHandler(rb0B_MouseHover);
            rb0A.AutoSize = true;
            rb0A.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            rb0A.Location = new Point(0x27, 0xfc);
            rb0A.Name = "rb0A";
            rb0A.Size = new Size(0x79, 0x11);
            rb0A.TabIndex = 0x117;
            rb0A.TabStop = true;
            rb0A.Text = "R0A  Trig_SOURCE";
            rb0A.UseVisualStyleBackColor = true;
            rb0A.MouseHover += new EventHandler(rb0A_MouseHover);
            rb09.AutoSize = true;
            rb09.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            rb09.Location = new Point(0x27, 0xeb);
            rb09.Name = "rb09";
            rb09.Size = new Size(0x66, 0x11);
            rb09.TabIndex = 0x116;
            rb09.TabStop = true;
            rb09.Text = "R09   F_SETUP";
            rb09.UseVisualStyleBackColor = true;
            rb09.MouseHover += new EventHandler(rb09_MouseHover);
            rb06.AutoSize = true;
            rb06.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            rb06.Location = new Point(0x27, 0xd8);
            rb06.Name = "rb06";
            rb06.Size = new Size(0x74, 0x11);
            rb06.TabIndex = 0x115;
            rb06.TabStop = true;
            rb06.Text = "R06   OUT_Z_LSB";
            rb06.UseVisualStyleBackColor = true;
            rb06.Visible = false;
            rb06.MouseHover += new EventHandler(rb06_MouseHover);
            rb05.AutoSize = true;
            rb05.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            rb05.Location = new Point(0x24, 0x55);
            rb05.Name = "rb05";
            rb05.Size = new Size(0x77, 0x11);
            rb05.TabIndex = 0x112;
            rb05.TabStop = true;
            rb05.Text = "R05   OUT_Z_MSB";
            rb05.UseVisualStyleBackColor = true;
            rb05.MouseHover += new EventHandler(rb05_MouseHover);
            rb04.AutoSize = true;
            rb04.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            rb04.Location = new Point(0x27, 200);
            rb04.Name = "rb04";
            rb04.Size = new Size(0x74, 0x11);
            rb04.TabIndex = 0x111;
            rb04.TabStop = true;
            rb04.Text = "R04   OUT_Y_LSB";
            rb04.UseVisualStyleBackColor = true;
            rb04.Visible = false;
            rb04.MouseHover += new EventHandler(rb04_MouseHover);
            rb03.AutoSize = true;
            rb03.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            rb03.Location = new Point(0x24, 0x44);
            rb03.Name = "rb03";
            rb03.Size = new Size(0x77, 0x11);
            rb03.TabIndex = 0x110;
            rb03.TabStop = true;
            rb03.Text = "R03   OUT_Y_MSB";
            rb03.UseVisualStyleBackColor = true;
            rb03.MouseHover += new EventHandler(rb03_MouseHover);
            rb02.AutoSize = true;
            rb02.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            rb02.Location = new Point(0x27, 0xb5);
            rb02.Name = "rb02";
            rb02.Size = new Size(0x74, 0x11);
            rb02.TabIndex = 0x10f;
            rb02.TabStop = true;
            rb02.Text = "R02   OUT_X_LSB";
            rb02.UseVisualStyleBackColor = true;
            rb02.Visible = false;
            rb02.MouseHover += new EventHandler(rb02_MouseHover);
            rb01.AutoSize = true;
            rb01.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            rb01.Location = new Point(0x24, 0x34);
            rb01.Name = "rb01";
            rb01.Size = new Size(0x77, 0x11);
            rb01.TabIndex = 270;
            rb01.TabStop = true;
            rb01.Text = "R01   OUT_X_MSB";
            rb01.UseVisualStyleBackColor = true;
            rb01.MouseHover += new EventHandler(rb01_MouseHover);
            rbR00.AutoSize = true;
            rbR00.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            rbR00.Location = new Point(0x24, 0x23);
            rbR00.Name = "rbR00";
            rbR00.Size = new Size(0x61, 0x11);
            rbR00.TabIndex = 0x10d;
            rbR00.TabStop = true;
            rbR00.Text = "R00   STATUS";
            rbR00.UseVisualStyleBackColor = true;
            rbR00.MouseHover += new EventHandler(rbR00_MouseHover);
            lbl1A.AutoSize = true;
            lbl1A.BackColor = Color.LightSteelBlue;
            lbl1A.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbl1A.Location = new Point(0x218, 260);
            lbl1A.Name = "lbl1A";
            lbl1A.Size = new Size(0x13, 13);
            lbl1A.TabIndex = 0x10c;
            lbl1A.Text = "00";
            lbl1A.Visible = false;
            lbl1B.AutoSize = true;
            lbl1B.BackColor = Color.LightSteelBlue;
            lbl1B.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbl1B.Location = new Point(0x2d1, 0x105);
            lbl1B.Name = "lbl1B";
            lbl1B.Size = new Size(0x13, 13);
            lbl1B.TabIndex = 0x10b;
            lbl1B.Text = "00";
            lbl1B.Visible = false;
            lbl1C.AutoSize = true;
            lbl1C.BackColor = Color.LightSteelBlue;
            lbl1C.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbl1C.Location = new Point(0x38d, 0x105);
            lbl1C.Name = "lbl1C";
            lbl1C.Size = new Size(0x13, 13);
            lbl1C.TabIndex = 0x10a;
            lbl1C.Text = "00";
            lbl1C.Visible = false;
            lbl19.AutoSize = true;
            lbl19.BackColor = Color.LightSteelBlue;
            lbl19.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbl19.Location = new Point(0x15d, 0x102);
            lbl19.Name = "lbl19";
            lbl19.Size = new Size(0x13, 13);
            lbl19.TabIndex = 0x109;
            lbl19.Text = "00";
            lbl19.Visible = false;
            label57.AutoSize = true;
            label57.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label57.Location = new Point(590, 0x10);
            label57.Name = "label57";
            label57.Size = new Size(80, 0x10);
            label57.TabIndex = 260;
            label57.Text = "Pulse/Tap";
            label54.AutoSize = true;
            label54.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label54.Location = new Point(5, 0x10);
            label54.Name = "label54";
            label54.Size = new Size(0x49, 0x10);
            label54.TabIndex = 0x103;
            label54.Text = "XYZ Data";
            lbl28.AutoSize = true;
            lbl28.BackColor = Color.LightSteelBlue;
            lbl28.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbl28.Location = new Point(0x250, 0xb2);
            lbl28.Name = "lbl28";
            lbl28.Size = new Size(0x13, 13);
            lbl28.TabIndex = 0x102;
            lbl28.Text = "00";
            lbl26.AutoSize = true;
            lbl26.BackColor = Color.LightSteelBlue;
            lbl26.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbl26.Location = new Point(0x250, 0x89);
            lbl26.Name = "lbl26";
            lbl26.Size = new Size(0x13, 13);
            lbl26.TabIndex = 0x101;
            lbl26.Text = "00";
            lbl1E.AutoSize = true;
            lbl1E.BackColor = Color.LightSteelBlue;
            lbl1E.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbl1E.Location = new Point(0x174, 0x8b);
            lbl1E.Name = "lbl1E";
            lbl1E.Size = new Size(0x13, 13);
            lbl1E.TabIndex = 0x100;
            lbl1E.Text = "00";
            lbl1F.AutoSize = true;
            lbl1F.BackColor = Color.LightSteelBlue;
            lbl1F.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbl1F.Location = new Point(0x174, 0x9e);
            lbl1F.Name = "lbl1F";
            lbl1F.Size = new Size(0x13, 13);
            lbl1F.TabIndex = 0xff;
            lbl1F.Text = "00";
            lbl20.AutoSize = true;
            lbl20.BackColor = Color.LightSteelBlue;
            lbl20.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbl20.Location = new Point(0x174, 0xb1);
            lbl20.Name = "lbl20";
            lbl20.Size = new Size(0x13, 13);
            lbl20.TabIndex = 0xfe;
            lbl20.Text = "00";
            lbl21.AutoSize = true;
            lbl21.BackColor = Color.LightSteelBlue;
            lbl21.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbl21.Location = new Point(0x250, 0x27);
            lbl21.Name = "lbl21";
            lbl21.Size = new Size(0x13, 13);
            lbl21.TabIndex = 0xfd;
            lbl21.Text = "00";
            lbl22.AutoSize = true;
            lbl22.BackColor = Color.LightSteelBlue;
            lbl22.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbl22.Location = new Point(0x250, 0x3a);
            lbl22.Name = "lbl22";
            lbl22.Size = new Size(0x13, 13);
            lbl22.TabIndex = 0xfc;
            lbl22.Text = "00";
            lbl23.AutoSize = true;
            lbl23.BackColor = Color.LightSteelBlue;
            lbl23.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbl23.Location = new Point(0x250, 0x4c);
            lbl23.Name = "lbl23";
            lbl23.Size = new Size(0x13, 13);
            lbl23.TabIndex = 0xfb;
            lbl23.Text = "00";
            lbl27.AutoSize = true;
            lbl27.BackColor = Color.LightSteelBlue;
            lbl27.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbl27.Location = new Point(0x250, 0x9d);
            lbl27.Name = "lbl27";
            lbl27.Size = new Size(0x13, 13);
            lbl27.TabIndex = 250;
            lbl27.Text = "00";
            lbl25.AutoSize = true;
            lbl25.BackColor = Color.LightSteelBlue;
            lbl25.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbl25.Location = new Point(0x250, 0x75);
            lbl25.Name = "lbl25";
            lbl25.Size = new Size(0x13, 13);
            lbl25.TabIndex = 0xf9;
            lbl25.Text = "00";
            lbl1D.AutoSize = true;
            lbl1D.BackColor = Color.LightSteelBlue;
            lbl1D.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbl1D.Location = new Point(0x174, 0x77);
            lbl1D.Name = "lbl1D";
            lbl1D.Size = new Size(0x13, 13);
            lbl1D.TabIndex = 0xf8;
            lbl1D.Text = "00";
            lbl24.AutoSize = true;
            lbl24.BackColor = Color.LightSteelBlue;
            lbl24.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbl24.Location = new Point(0x250, 0x5f);
            lbl24.Name = "lbl24";
            lbl24.Size = new Size(0x13, 13);
            lbl24.TabIndex = 0xed;
            lbl24.Text = "00";
            lbl29.AutoSize = true;
            lbl29.BackColor = Color.LightSteelBlue;
            lbl29.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbl29.Location = new Point(0x301, 0x26);
            lbl29.Name = "lbl29";
            lbl29.Size = new Size(0x13, 13);
            lbl29.TabIndex = 0xec;
            lbl29.Text = "00";
            lbl31.AutoSize = true;
            lbl31.BackColor = Color.LightSteelBlue;
            lbl31.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbl31.Location = new Point(0x3b2, 0x49);
            lbl31.Name = "lbl31";
            lbl31.Size = new Size(0x13, 13);
            lbl31.TabIndex = 0xeb;
            lbl31.Text = "00";
            lbl2F.AutoSize = true;
            lbl2F.BackColor = Color.LightSteelBlue;
            lbl2F.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbl2F.Location = new Point(0x3b2, 0x26);
            lbl2F.Name = "lbl2F";
            lbl2F.Size = new Size(0x13, 13);
            lbl2F.TabIndex = 0xea;
            lbl2F.Text = "00";
            lbl30.AutoSize = true;
            lbl30.BackColor = Color.LightSteelBlue;
            lbl30.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbl30.Location = new Point(0x3b2, 0x38);
            lbl30.Name = "lbl30";
            lbl30.Size = new Size(0x13, 13);
            lbl30.TabIndex = 0xe9;
            lbl30.Text = "00";
            lbl2E.AutoSize = true;
            lbl2E.BackColor = Color.LightSteelBlue;
            lbl2E.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbl2E.Location = new Point(0x301, 0x86);
            lbl2E.Name = "lbl2E";
            lbl2E.Size = new Size(0x13, 13);
            lbl2E.TabIndex = 0xe8;
            lbl2E.Text = "00";
            lbl2C.AutoSize = true;
            lbl2C.BackColor = Color.LightSteelBlue;
            lbl2C.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbl2C.Location = new Point(0x301, 0x5f);
            lbl2C.Name = "lbl2C";
            lbl2C.Size = new Size(0x13, 13);
            lbl2C.TabIndex = 0xe7;
            lbl2C.Text = "00";
            lbl2D.AutoSize = true;
            lbl2D.BackColor = Color.LightSteelBlue;
            lbl2D.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbl2D.Location = new Point(0x301, 0x72);
            lbl2D.Name = "lbl2D";
            lbl2D.Size = new Size(0x13, 13);
            lbl2D.TabIndex = 230;
            lbl2D.Text = "00";
            lbl2B.AutoSize = true;
            lbl2B.BackColor = Color.LightSteelBlue;
            lbl2B.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbl2B.Location = new Point(0x301, 0x4c);
            lbl2B.Name = "lbl2B";
            lbl2B.Size = new Size(0x13, 13);
            lbl2B.TabIndex = 0xe5;
            lbl2B.Text = "00";
            lbl17.AutoSize = true;
            lbl17.BackColor = Color.LightSteelBlue;
            lbl17.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbl17.Location = new Point(0x174, 0x4e);
            lbl17.Name = "lbl17";
            lbl17.Size = new Size(0x13, 13);
            lbl17.TabIndex = 0xe3;
            lbl17.Text = "00";
            lbl18.AutoSize = true;
            lbl18.BackColor = Color.LightSteelBlue;
            lbl18.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbl18.Location = new Point(0x174, 0x62);
            lbl18.Name = "lbl18";
            lbl18.Size = new Size(0x13, 13);
            lbl18.TabIndex = 0xe2;
            lbl18.Text = "00";
            lbl16.AutoSize = true;
            lbl16.BackColor = Color.LightSteelBlue;
            lbl16.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbl16.Location = new Point(0x174, 0x3b);
            lbl16.Name = "lbl16";
            lbl16.Size = new Size(0x13, 13);
            lbl16.TabIndex = 0xdb;
            lbl16.Text = "00";
            lbl15.AutoSize = true;
            lbl15.BackColor = Color.LightSteelBlue;
            lbl15.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbl15.Location = new Point(0x174, 0x27);
            lbl15.Name = "lbl15";
            lbl15.Size = new Size(0x13, 13);
            lbl15.TabIndex = 0xda;
            lbl15.Text = "00";
            lbl13.AutoSize = true;
            lbl13.BackColor = Color.LightSteelBlue;
            lbl13.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbl13.Location = new Point(170, 0xb9);
            lbl13.Name = "lbl13";
            lbl13.Size = new Size(0x13, 13);
            lbl13.TabIndex = 0xd8;
            lbl13.Text = "00";
            lbl0B.AutoSize = true;
            lbl0B.BackColor = Color.LightSteelBlue;
            lbl0B.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbl0B.Location = new Point(5, 0x83);
            lbl0B.Name = "lbl0B";
            lbl0B.Size = new Size(0x13, 13);
            lbl0B.TabIndex = 0xd7;
            lbl0B.Text = "00";
            lbl0C.AutoSize = true;
            lbl0C.BackColor = Color.LightSteelBlue;
            lbl0C.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbl0C.Location = new Point(5, 150);
            lbl0C.Name = "lbl0C";
            lbl0C.Size = new Size(0x13, 13);
            lbl0C.TabIndex = 0xd6;
            lbl0C.Text = "00";
            lbl0D.AutoSize = true;
            lbl0D.BackColor = Color.LightSteelBlue;
            lbl0D.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbl0D.Location = new Point(170, 40);
            lbl0D.Name = "lbl0D";
            lbl0D.Size = new Size(0x13, 13);
            lbl0D.TabIndex = 0xd5;
            lbl0D.Text = "00";
            lbl0E.AutoSize = true;
            lbl0E.BackColor = Color.LightSteelBlue;
            lbl0E.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbl0E.Location = new Point(170, 0x3d);
            lbl0E.Name = "lbl0E";
            lbl0E.Size = new Size(0x13, 13);
            lbl0E.TabIndex = 0xd4;
            lbl0E.Text = "00";
            lbl0F.AutoSize = true;
            lbl0F.BackColor = Color.LightSteelBlue;
            lbl0F.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbl0F.Location = new Point(170, 0x52);
            lbl0F.Name = "lbl0F";
            lbl0F.Size = new Size(0x13, 13);
            lbl0F.TabIndex = 0xd3;
            lbl0F.Text = "00";
            lbl10.AutoSize = true;
            lbl10.BackColor = Color.LightSteelBlue;
            lbl10.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbl10.Location = new Point(170, 0x7f);
            lbl10.Name = "lbl10";
            lbl10.Size = new Size(0x13, 13);
            lbl10.TabIndex = 210;
            lbl10.Text = "00";
            lbl14.AutoSize = true;
            lbl14.BackColor = Color.LightSteelBlue;
            lbl14.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbl14.Location = new Point(170, 0xcc);
            lbl14.Name = "lbl14";
            lbl14.Size = new Size(0x13, 13);
            lbl14.TabIndex = 0xd1;
            lbl14.Text = "00";
            lbl12.AutoSize = true;
            lbl12.BackColor = Color.LightSteelBlue;
            lbl12.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbl12.Location = new Point(170, 0xa6);
            lbl12.Name = "lbl12";
            lbl12.Size = new Size(0x13, 13);
            lbl12.TabIndex = 0xd0;
            lbl12.Text = "00";
            lbl0A.AutoSize = true;
            lbl0A.BackColor = Color.LightSteelBlue;
            lbl0A.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbl0A.Location = new Point(7, 0xfe);
            lbl0A.Name = "lbl0A";
            lbl0A.Size = new Size(0x13, 13);
            lbl0A.TabIndex = 0xcf;
            lbl0A.Text = "00";
            lbl09.AutoSize = true;
            lbl09.BackColor = Color.LightSteelBlue;
            lbl09.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbl09.Location = new Point(8, 0xeb);
            lbl09.Name = "lbl09";
            lbl09.Size = new Size(0x13, 13);
            lbl09.TabIndex = 0xce;
            lbl09.Text = "00";
            lbl01.AutoSize = true;
            lbl01.BackColor = Color.LightSteelBlue;
            lbl01.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbl01.Location = new Point(5, 0x36);
            lbl01.Name = "lbl01";
            lbl01.Size = new Size(0x13, 13);
            lbl01.TabIndex = 0x9c;
            lbl01.Text = "00";
            lbl02.AutoSize = true;
            lbl02.BackColor = Color.LightSteelBlue;
            lbl02.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbl02.Location = new Point(8, 0xb7);
            lbl02.Name = "lbl02";
            lbl02.Size = new Size(0x13, 13);
            lbl02.TabIndex = 0x9b;
            lbl02.Text = "00";
            lbl02.Visible = false;
            lbl03.AutoSize = true;
            lbl03.BackColor = Color.LightSteelBlue;
            lbl03.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbl03.Location = new Point(5, 70);
            lbl03.Name = "lbl03";
            lbl03.Size = new Size(0x13, 13);
            lbl03.TabIndex = 0x9a;
            lbl03.Text = "00";
            lbl04.AutoSize = true;
            lbl04.BackColor = Color.LightSteelBlue;
            lbl04.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbl04.Location = new Point(8, 0xca);
            lbl04.Name = "lbl04";
            lbl04.Size = new Size(0x13, 13);
            lbl04.TabIndex = 0x99;
            lbl04.Text = "00";
            lbl04.Visible = false;
            lbl05.AutoSize = true;
            lbl05.BackColor = Color.LightSteelBlue;
            lbl05.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbl05.Location = new Point(5, 0x56);
            lbl05.Name = "lbl05";
            lbl05.Size = new Size(0x13, 13);
            lbl05.TabIndex = 0x98;
            lbl05.Text = "00";
            lbl06.AutoSize = true;
            lbl06.BackColor = Color.LightSteelBlue;
            lbl06.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbl06.Location = new Point(8, 0xda);
            lbl06.Name = "lbl06";
            lbl06.Size = new Size(0x13, 13);
            lbl06.TabIndex = 0x97;
            lbl06.Text = "00";
            lbl06.Visible = false;
            lbl00.AutoSize = true;
            lbl00.BackColor = Color.LightSteelBlue;
            lbl00.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbl00.Location = new Point(5, 0x25);
            lbl00.Name = "lbl00";
            lbl00.Size = new Size(0x13, 13);
            lbl00.TabIndex = 0x94;
            lbl00.Text = "00";
            lbl2A.AutoSize = true;
            lbl2A.BackColor = Color.LightSteelBlue;
            lbl2A.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbl2A.Location = new Point(0x301, 0x39);
            lbl2A.Name = "lbl2A";
            lbl2A.Size = new Size(0x13, 13);
            lbl2A.TabIndex = 0x93;
            lbl2A.Text = "00";
            lbl11.AutoSize = true;
            lbl11.BackColor = Color.LightSteelBlue;
            lbl11.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbl11.Location = new Point(170, 0x92);
            lbl11.Name = "lbl11";
            lbl11.Size = new Size(0x13, 13);
            lbl11.TabIndex = 0x92;
            lbl11.Text = "00";
            DataConfigPage.BackColor = Color.SlateGray;
            DataConfigPage.Controls.Add(groupBox2);
            DataConfigPage.Location = new Point(4, 0x19);
            DataConfigPage.Name = "DataConfigPage";
            DataConfigPage.Padding = new Padding(3);
            DataConfigPage.Size = new Size(0x464, 0x20d);
            DataConfigPage.TabIndex = 6;
            DataConfigPage.Text = "DataConfig";
            DataConfigPage.UseVisualStyleBackColor = true;
            groupBox2.BackColor = Color.LightSlateGray;
            groupBox2.Controls.Add(gbStatus);
            groupBox2.Controls.Add(groupBox3);
            groupBox2.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            groupBox2.ForeColor = Color.White;
            groupBox2.Location = new Point(10, 0x20);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(0x41f, 0x1dc);
            groupBox2.TabIndex = 12;
            groupBox2.TabStop = false;
            groupBox2.Text = "Data Configuration  and Interrupt Configuration Settings";
            gbStatus.BackColor = Color.LightSlateGray;
            gbStatus.Controls.Add(p21);
            gbStatus.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            gbStatus.ForeColor = Color.White;
            gbStatus.Location = new Point(0x11a, 0x33);
            gbStatus.Name = "gbStatus";
            gbStatus.Size = new Size(0x2ad, 370);
            gbStatus.TabIndex = 0xbb;
            gbStatus.TabStop = false;
            gbStatus.Text = "Data Status Reg 0x00: Real Time/ FIFO Data Status";
            p21.BackColor = Color.LightSlateGray;
            p21.Controls.Add(lblFIFOStatus);
            p21.Controls.Add(ledFIFOStatus);
            p21.Controls.Add(lblFOvf);
            p21.Controls.Add(ledRealTimeStatus);
            p21.Controls.Add(label61);
            p21.Controls.Add(lblWmrk);
            p21.Controls.Add(lblFCnt5);
            p21.Controls.Add(lblFCnt4);
            p21.Controls.Add(lblFCnt3);
            p21.Controls.Add(lblFCnt2);
            p21.Controls.Add(lblFCnt1);
            p21.Controls.Add(lblFCnt0);
            p21.Controls.Add(ledXYZOW);
            p21.Controls.Add(ledZOW);
            p21.Controls.Add(ledYOW);
            p21.Controls.Add(ledXOW);
            p21.Controls.Add(ledZDR);
            p21.Controls.Add(ledYDR);
            p21.Controls.Add(ledXDR);
            p21.Controls.Add(ledXYZDR);
            p21.Controls.Add(lblXYZOW);
            p21.Controls.Add(lblXYZDrdy);
            p21.Controls.Add(lblXDrdy);
            p21.Controls.Add(lblZOW);
            p21.Controls.Add(lblYOW);
            p21.Controls.Add(lblYDrdy);
            p21.Controls.Add(lblXOW);
            p21.Controls.Add(lblZDrdy);
            p21.Enabled = false;
            p21.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            p21.Location = new Point(6, 0x19);
            p21.Name = "p21";
            p21.Size = new Size(0x2a1, 0x153);
            p21.TabIndex = 0xb9;
            lblFIFOStatus.AutoSize = true;
            lblFIFOStatus.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblFIFOStatus.ForeColor = Color.White;
            lblFIFOStatus.Location = new Point(0xe2, 0x1c);
            lblFIFOStatus.Name = "lblFIFOStatus";
            lblFIFOStatus.Size = new Size(0x58, 0x10);
            lblFIFOStatus.TabIndex = 0xd7;
            lblFIFOStatus.Text = "FIFO Status";
            ledFIFOStatus.LedStyle = LedStyle.Round3D;
            ledFIFOStatus.Location = new Point(180, 0x13);
            ledFIFOStatus.Name = "ledFIFOStatus";
            ledFIFOStatus.OffColor = Color.Red;
            ledFIFOStatus.Size = new Size(0x27, 0x29);
            ledFIFOStatus.TabIndex = 0xd8;
            lblFOvf.AutoSize = true;
            lblFOvf.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblFOvf.ForeColor = Color.White;
            lblFOvf.Location = new Point(0x22e, 0x11d);
            lblFOvf.Name = "lblFOvf";
            lblFOvf.Size = new Size(0x37, 0x10);
            lblFOvf.TabIndex = 0xe3;
            lblFOvf.Text = "F_OVF";
            ledRealTimeStatus.LedStyle = LedStyle.Round3D;
            ledRealTimeStatus.Location = new Point(0x87, 0x13);
            ledRealTimeStatus.Name = "ledRealTimeStatus";
            ledRealTimeStatus.OffColor = Color.Red;
            ledRealTimeStatus.Size = new Size(40, 40);
            ledRealTimeStatus.TabIndex = 0xd6;
            label61.AutoSize = true;
            label61.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label61.ForeColor = Color.White;
            label61.Location = new Point(9, 0x1c);
            label61.Name = "label61";
            label61.Size = new Size(0x7f, 0x10);
            label61.TabIndex = 0xc6;
            label61.Text = "Real Time Status";
            lblWmrk.AutoSize = true;
            lblWmrk.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblWmrk.ForeColor = Color.White;
            lblWmrk.Location = new Point(0x22e, 0xe0);
            lblWmrk.Name = "lblWmrk";
            lblWmrk.Size = new Size(0x69, 0x10);
            lblWmrk.TabIndex = 0xe2;
            lblWmrk.Text = "WATERMARK";
            lblFCnt5.AutoSize = true;
            lblFCnt5.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblFCnt5.ForeColor = Color.White;
            lblFCnt5.Location = new Point(0x22e, 0xa6);
            lblFCnt5.Name = "lblFCnt5";
            lblFCnt5.Size = new Size(0x38, 0x10);
            lblFCnt5.TabIndex = 0xe1;
            lblFCnt5.Text = "FCNT5";
            lblFCnt4.AutoSize = true;
            lblFCnt4.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblFCnt4.ForeColor = Color.White;
            lblFCnt4.Location = new Point(0x22e, 0x66);
            lblFCnt4.Name = "lblFCnt4";
            lblFCnt4.Size = new Size(0x38, 0x10);
            lblFCnt4.TabIndex = 0xe0;
            lblFCnt4.Text = "FCNT4";
            lblFCnt3.AutoSize = true;
            lblFCnt3.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblFCnt3.ForeColor = Color.White;
            lblFCnt3.Location = new Point(0xcb, 0x11a);
            lblFCnt3.Name = "lblFCnt3";
            lblFCnt3.Size = new Size(0x38, 0x10);
            lblFCnt3.TabIndex = 0xdf;
            lblFCnt3.Text = "FCNT3";
            lblFCnt2.AutoSize = true;
            lblFCnt2.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblFCnt2.ForeColor = Color.White;
            lblFCnt2.Location = new Point(0xcb, 0xe0);
            lblFCnt2.Name = "lblFCnt2";
            lblFCnt2.Size = new Size(0x38, 0x10);
            lblFCnt2.TabIndex = 0xde;
            lblFCnt2.Text = "FCNT2";
            lblFCnt1.AutoSize = true;
            lblFCnt1.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblFCnt1.ForeColor = Color.White;
            lblFCnt1.Location = new Point(0xcb, 0xa3);
            lblFCnt1.Name = "lblFCnt1";
            lblFCnt1.Size = new Size(0x38, 0x10);
            lblFCnt1.TabIndex = 0xdd;
            lblFCnt1.Text = "FCNT1";
            lblFCnt0.AutoSize = true;
            lblFCnt0.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblFCnt0.ForeColor = Color.White;
            lblFCnt0.Location = new Point(0xcb, 0x68);
            lblFCnt0.Name = "lblFCnt0";
            lblFCnt0.Size = new Size(0x38, 0x10);
            lblFCnt0.TabIndex = 220;
            lblFCnt0.Text = "FCNT0";
            ledXYZOW.LedStyle = LedStyle.Round3D;
            ledXYZOW.Location = new Point(0x200, 0x113);
            ledXYZOW.Name = "ledXYZOW";
            ledXYZOW.OffColor = Color.Red;
            ledXYZOW.Size = new Size(40, 40);
            ledXYZOW.TabIndex = 0xdb;
            ledZOW.LedStyle = LedStyle.Round3D;
            ledZOW.Location = new Point(0x200, 0xd6);
            ledZOW.Name = "ledZOW";
            ledZOW.OffColor = Color.Red;
            ledZOW.Size = new Size(40, 40);
            ledZOW.TabIndex = 0xda;
            ledYOW.LedStyle = LedStyle.Round3D;
            ledYOW.Location = new Point(0x200, 0x9c);
            ledYOW.Name = "ledYOW";
            ledYOW.OffColor = Color.Red;
            ledYOW.Size = new Size(40, 40);
            ledYOW.TabIndex = 0xd9;
            ledXOW.LedStyle = LedStyle.Round3D;
            ledXOW.Location = new Point(0x200, 0x60);
            ledXOW.Name = "ledXOW";
            ledXOW.OffColor = Color.Red;
            ledXOW.Size = new Size(40, 40);
            ledXOW.TabIndex = 0xd8;
            ledZDR.LedStyle = LedStyle.Round3D;
            ledZDR.Location = new Point(0x9d, 0xd5);
            ledZDR.Name = "ledZDR";
            ledZDR.OffColor = Color.Red;
            ledZDR.Size = new Size(40, 40);
            ledZDR.TabIndex = 0xd7;
            ledYDR.LedStyle = LedStyle.Round3D;
            ledYDR.Location = new Point(0x9d, 0x9b);
            ledYDR.Name = "ledYDR";
            ledYDR.OffColor = Color.Red;
            ledYDR.Size = new Size(40, 40);
            ledYDR.TabIndex = 0xd6;
            ledXDR.LedStyle = LedStyle.Round3D;
            ledXDR.Location = new Point(0x9d, 0x5f);
            ledXDR.Name = "ledXDR";
            ledXDR.OffColor = Color.Red;
            ledXDR.Size = new Size(40, 40);
            ledXDR.TabIndex = 0xd5;
            ledXYZDR.LedStyle = LedStyle.Round3D;
            ledXYZDR.Location = new Point(0x9d, 0x112);
            ledXYZDR.Name = "ledXYZDR";
            ledXYZDR.OffColor = Color.Red;
            ledXYZDR.Size = new Size(40, 40);
            ledXYZDR.TabIndex = 0xd3;
            lblXYZOW.AutoSize = true;
            lblXYZOW.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblXYZOW.ForeColor = Color.White;
            lblXYZOW.Location = new Point(0x17f, 0x11d);
            lblXYZOW.Name = "lblXYZOW";
            lblXYZOW.Size = new Size(0x83, 0x10);
            lblXYZOW.TabIndex = 210;
            lblXYZOW.Text = "X Y or Z Overwrite";
            lblXYZDrdy.AutoSize = true;
            lblXYZDrdy.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblXYZDrdy.ForeColor = Color.White;
            lblXYZDrdy.Location = new Point(8, 0x11a);
            lblXYZDrdy.Name = "lblXYZDrdy";
            lblXYZDrdy.Size = new Size(0x95, 0x10);
            lblXYZDrdy.TabIndex = 0xd1;
            lblXYZDrdy.Text = "X Y or Z Data Ready";
            lblXDrdy.AutoSize = true;
            lblXDrdy.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblXDrdy.ForeColor = Color.White;
            lblXDrdy.Location = new Point(0x36, 0x68);
            lblXDrdy.Name = "lblXDrdy";
            lblXDrdy.Size = new Size(0x68, 0x10);
            lblXDrdy.TabIndex = 0xc5;
            lblXDrdy.Text = "X Data Ready";
            lblZOW.AutoSize = true;
            lblZOW.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblZOW.ForeColor = Color.White;
            lblZOW.Location = new Point(430, 0xe0);
            lblZOW.Name = "lblZOW";
            lblZOW.Size = new Size(0x56, 0x10);
            lblZOW.TabIndex = 0xcf;
            lblZOW.Text = "Z Overwrite";
            lblYOW.AutoSize = true;
            lblYOW.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblYOW.ForeColor = Color.White;
            lblYOW.Location = new Point(0x1ad, 0xa6);
            lblYOW.Name = "lblYOW";
            lblYOW.Size = new Size(0x57, 0x10);
            lblYOW.TabIndex = 0xcd;
            lblYOW.Text = "Y Overwrite";
            lblYDrdy.AutoSize = true;
            lblYDrdy.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblYDrdy.ForeColor = Color.White;
            lblYDrdy.Location = new Point(0x36, 0xa3);
            lblYDrdy.Name = "lblYDrdy";
            lblYDrdy.Size = new Size(0x69, 0x10);
            lblYDrdy.TabIndex = 0xc7;
            lblYDrdy.Text = "Y Data Ready";
            lblXOW.AutoSize = true;
            lblXOW.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblXOW.ForeColor = Color.White;
            lblXOW.Location = new Point(0x1ad, 0x66);
            lblXOW.Name = "lblXOW";
            lblXOW.Size = new Size(0x56, 0x10);
            lblXOW.TabIndex = 0xcb;
            lblXOW.Text = "X Overwrite";
            lblZDrdy.AutoSize = true;
            lblZDrdy.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblZDrdy.ForeColor = Color.White;
            lblZDrdy.Location = new Point(0x37, 0xdd);
            lblZDrdy.Name = "lblZDrdy";
            lblZDrdy.Size = new Size(0x68, 0x10);
            lblZDrdy.TabIndex = 0xc9;
            lblZDrdy.Text = "Z Data Ready";
            groupBox3.BackColor = Color.LightSlateGray;
            groupBox3.Controls.Add(p20);
            groupBox3.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            groupBox3.ForeColor = Color.White;
            groupBox3.Location = new Point(6, 0x33);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(0x102, 370);
            groupBox3.TabIndex = 0xba;
            groupBox3.TabStop = false;
            groupBox3.Text = "Interrupt Settings Reg 0x2C";
            p20.BackColor = Color.LightSlateGray;
            p20.Controls.Add(panel16);
            p20.Controls.Add(panel17);
            p20.ForeColor = Color.White;
            p20.Location = new Point(6, 0x18);
            p20.Name = "p20";
            p20.Size = new Size(230, 0xec);
            p20.TabIndex = 0xb8;
            panel16.BackColor = Color.LightSlateGray;
            panel16.Controls.Add(rdoINTPushPull);
            panel16.Controls.Add(rdoINTOpenDrain);
            panel16.Enabled = false;
            panel16.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Regular, GraphicsUnit.Point, 0);
            panel16.ForeColor = Color.White;
            panel16.Location = new Point(0x13, 0x67);
            panel16.Name = "panel16";
            panel16.Size = new Size(0x9a, 0x41);
            panel16.TabIndex = 0xce;
            rdoINTPushPull.AutoSize = true;
            rdoINTPushPull.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            rdoINTPushPull.Location = new Point(9, 6);
            rdoINTPushPull.Name = "rdoINTPushPull";
            rdoINTPushPull.Size = new Size(120, 20);
            rdoINTPushPull.TabIndex = 0xc9;
            rdoINTPushPull.Text = "INT Push/Pull";
            rdoINTPushPull.UseVisualStyleBackColor = true;
            rdoINTPushPull.CheckedChanged += new EventHandler(rdoINTPushPull_CheckedChanged);
            rdoINTOpenDrain.AutoSize = true;
            rdoINTOpenDrain.Checked = true;
            rdoINTOpenDrain.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            rdoINTOpenDrain.Location = new Point(9, 0x1d);
            rdoINTOpenDrain.Name = "rdoINTOpenDrain";
            rdoINTOpenDrain.Size = new Size(0x85, 20);
            rdoINTOpenDrain.TabIndex = 200;
            rdoINTOpenDrain.TabStop = true;
            rdoINTOpenDrain.Text = "INT Open Drain";
            rdoINTOpenDrain.UseVisualStyleBackColor = true;
            rdoINTOpenDrain.CheckedChanged += new EventHandler(rdoINTOpenDrain_CheckedChanged);
            panel17.BackColor = Color.LightSlateGray;
            panel17.Controls.Add(rdoINTActiveLow);
            panel17.Controls.Add(rdoINTActiveHigh);
            panel17.Enabled = false;
            panel17.ForeColor = Color.White;
            panel17.Location = new Point(0x13, 0x12);
            panel17.Name = "panel17";
            panel17.Size = new Size(0xcb, 0x41);
            panel17.TabIndex = 0xcd;
            rdoINTActiveLow.AutoSize = true;
            rdoINTActiveLow.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            rdoINTActiveLow.Location = new Point(6, 6);
            rdoINTActiveLow.Name = "rdoINTActiveLow";
            rdoINTActiveLow.Size = new Size(0xba, 20);
            rdoINTActiveLow.TabIndex = 0xcb;
            rdoINTActiveLow.Text = "INT Polarity Active Low";
            rdoINTActiveLow.UseVisualStyleBackColor = true;
            rdoINTActiveLow.CheckedChanged += new EventHandler(rdoINTActiveLow_CheckedChanged);
            rdoINTActiveHigh.AutoSize = true;
            rdoINTActiveHigh.Checked = true;
            rdoINTActiveHigh.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            rdoINTActiveHigh.Location = new Point(6, 0x1f);
            rdoINTActiveHigh.Name = "rdoINTActiveHigh";
            rdoINTActiveHigh.Size = new Size(0xbf, 20);
            rdoINTActiveHigh.TabIndex = 0xca;
            rdoINTActiveHigh.TabStop = true;
            rdoINTActiveHigh.Text = "INT Polarity Active High";
            rdoINTActiveHigh.UseVisualStyleBackColor = true;
            rdoINTActiveHigh.CheckedChanged += new EventHandler(rdoINTActiveHigh_CheckedChanged);
            MFF1_2Page.BackColor = Color.SlateGray;
            MFF1_2Page.BorderStyle = BorderStyle.Fixed3D;
            MFF1_2Page.Controls.Add(chkEnableZMFF);
            MFF1_2Page.Controls.Add(chkEnableYMFF);
            MFF1_2Page.Controls.Add(chkEnableXMFF);
            MFF1_2Page.Controls.Add(lblMotionDataType);
            MFF1_2Page.Controls.Add(legend3);
            MFF1_2Page.Controls.Add(MFFGraph);
            MFF1_2Page.Controls.Add(gbMF1);
            MFF1_2Page.Location = new Point(4, 0x19);
            MFF1_2Page.Name = "MFF1_2Page";
            MFF1_2Page.Padding = new Padding(3);
            MFF1_2Page.Size = new Size(0x464, 0x20d);
            MFF1_2Page.TabIndex = 2;
            MFF1_2Page.Text = "Motion/FF";
            MFF1_2Page.UseVisualStyleBackColor = true;
            chkEnableZMFF.AutoSize = true;
            chkEnableZMFF.Checked = true;
            chkEnableZMFF.CheckState = CheckState.Checked;
            chkEnableZMFF.Location = new Point(0x33f, 0x15);
            chkEnableZMFF.Name = "chkEnableZMFF";
            chkEnableZMFF.Size = new Size(70, 20);
            chkEnableZMFF.TabIndex = 0xf3;
            chkEnableZMFF.Text = "Z-Axis";
            chkEnableZMFF.UseVisualStyleBackColor = true;
            chkEnableZMFF.CheckedChanged += new EventHandler(chkEnableZMFF_CheckedChanged);
            chkEnableYMFF.AutoSize = true;
            chkEnableYMFF.Checked = true;
            chkEnableYMFF.CheckState = CheckState.Checked;
            chkEnableYMFF.Location = new Point(0x2f9, 0x15);
            chkEnableYMFF.Name = "chkEnableYMFF";
            chkEnableYMFF.Size = new Size(0x47, 20);
            chkEnableYMFF.TabIndex = 0xf2;
            chkEnableYMFF.Text = "Y-Axis";
            chkEnableYMFF.UseVisualStyleBackColor = true;
            chkEnableYMFF.CheckedChanged += new EventHandler(chkEnableYMFF_CheckedChanged);
            chkEnableXMFF.AutoSize = true;
            chkEnableXMFF.Checked = true;
            chkEnableXMFF.CheckState = CheckState.Checked;
            chkEnableXMFF.Location = new Point(0x2b4, 0x15);
            chkEnableXMFF.Name = "chkEnableXMFF";
            chkEnableXMFF.Size = new Size(70, 20);
            chkEnableXMFF.TabIndex = 0xf1;
            chkEnableXMFF.Text = "X-Axis";
            chkEnableXMFF.UseVisualStyleBackColor = true;
            chkEnableXMFF.CheckedChanged += new EventHandler(chkEnableXMFF_CheckedChanged);
            lblMotionDataType.AutoSize = true;
            lblMotionDataType.ForeColor = SystemColors.ActiveCaptionText;
            lblMotionDataType.Location = new Point(0x22f, 0x16);
            lblMotionDataType.Name = "lblMotionDataType";
            lblMotionDataType.Size = new Size(0x63, 0x10);
            lblMotionDataType.TabIndex = 0x10;
            lblMotionDataType.Text = "LPF Data Out";
            legend3.ForeColor = Color.White;
            legend3.Items.AddRange(new LegendItem[] { legendItem7, legendItem8, legendItem9 });
            legend3.Location = new Point(0x253, 0x179);
            legend3.Name = "legend3";
            legend3.Size = new Size(0x1ca, 0x23);
            legend3.TabIndex = 15;
            legendItem7.Source = XAxis;
            legendItem7.Text = "Motion/FF X Axis ";
            XAxis.LineColor = Color.Red;
            XAxis.XAxis = xAxis1;
            XAxis.YAxis = yAxis1;
            xAxis1.Caption = "Samples";
            yAxis1.Caption = "Acceleration in g's";
            yAxis1.Range = new NationalInstruments.UI.Range(-8.0, 8.0);
            legendItem8.Source = YAxis;
            legendItem8.Text = "Motion/FF Y Axis ";
            YAxis.LineColor = Color.LawnGreen;
            YAxis.XAxis = xAxis1;
            YAxis.YAxis = yAxis1;
            legendItem9.Source = waveformPlot3;
            legendItem9.Text = "Motion/FF Z Axis ";
            waveformPlot3.LineColor = Color.Gold;
            waveformPlot3.XAxis = xAxis2;
            waveformPlot3.YAxis = yAxis2;
            xAxis2.Caption = "Samples";
            yAxis2.Caption = "Acceleration in g's";
            yAxis2.Range = new NationalInstruments.UI.Range(-8.0, 8.0);
            MFFGraph.BackColor = Color.LightSlateGray;
            MFFGraph.Caption = "Real Time Output";
            MFFGraph.CaptionBackColor = Color.LightSlateGray;
            MFFGraph.CaptionForeColor = SystemColors.ButtonHighlight;
            MFFGraph.ForeColor = SystemColors.Window;
            MFFGraph.ImmediateUpdates = true;
            MFFGraph.Location = new Point(0x221, 0x29);
            MFFGraph.Name = "MFFGraph";
            MFFGraph.PlotAreaBorder = Border.Raised;
            MFFGraph.Plots.AddRange(new WaveformPlot[] { waveformPlot1, waveformPlot2, waveformPlot3 });
            MFFGraph.Size = new Size(0x20b, 0x13b);
            MFFGraph.TabIndex = 14;
            MFFGraph.XAxes.AddRange(new NationalInstruments.UI.XAxis[] { xAxis2 });
            MFFGraph.YAxes.AddRange(new NationalInstruments.UI.YAxis[] { yAxis2 });
            waveformPlot1.LineColor = Color.Red;
            waveformPlot1.XAxis = xAxis2;
            waveformPlot1.YAxis = yAxis2;
            waveformPlot2.LineColor = Color.LawnGreen;
            waveformPlot2.XAxis = xAxis2;
            waveformPlot2.YAxis = yAxis2;
            gbMF1.BackColor = Color.LightSlateGray;
            gbMF1.Controls.Add(chkDefaultFFSettings1);
            gbMF1.Controls.Add(chkDefaultMotion1);
            gbMF1.Controls.Add(btnMFF1Reset);
            gbMF1.Controls.Add(btnMFF1Set);
            gbMF1.Controls.Add(rdoMFF1ClearDebounce);
            gbMF1.Controls.Add(p14);
            gbMF1.Controls.Add(p15);
            gbMF1.Controls.Add(rdoMFF1DecDebounce);
            gbMF1.Controls.Add(lblMFF1Threshold);
            gbMF1.Controls.Add(lblMFF1ThresholdVal);
            gbMF1.Controls.Add(lblMFF1Debouncems);
            gbMF1.Controls.Add(lblMFF1Thresholdg);
            gbMF1.Controls.Add(tbMFF1Threshold);
            gbMF1.Controls.Add(lblMFF1DebounceVal);
            gbMF1.Controls.Add(tbMFF1Debounce);
            gbMF1.Controls.Add(lblMFF1Debounce);
            gbMF1.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            gbMF1.ForeColor = Color.White;
            gbMF1.Location = new Point(0x12, 3);
            gbMF1.Name = "gbMF1";
            gbMF1.Size = new Size(480, 0x1fc);
            gbMF1.TabIndex = 8;
            gbMF1.TabStop = false;
            gbMF1.Text = "Motion Freefall ";
            chkDefaultFFSettings1.AutoSize = true;
            chkDefaultFFSettings1.ForeColor = Color.Gold;
            chkDefaultFFSettings1.Location = new Point(260, 0x1d);
            chkDefaultFFSettings1.Name = "chkDefaultFFSettings1";
            chkDefaultFFSettings1.Size = new Size(0xc5, 20);
            chkDefaultFFSettings1.TabIndex = 0xba;
            chkDefaultFFSettings1.Text = "Default Freefall Settings ";
            chkDefaultFFSettings1.UseVisualStyleBackColor = true;
            chkDefaultFFSettings1.CheckedChanged += new EventHandler(chkDefaultFFSettings1_CheckedChanged_1);
            chkDefaultMotion1.AutoSize = true;
            chkDefaultMotion1.ForeColor = Color.Gold;
            chkDefaultMotion1.Location = new Point(11, 0x1d);
            chkDefaultMotion1.Name = "chkDefaultMotion1";
            chkDefaultMotion1.Size = new Size(190, 20);
            chkDefaultMotion1.TabIndex = 0xb9;
            chkDefaultMotion1.Text = "Default Motion Settings ";
            chkDefaultMotion1.UseVisualStyleBackColor = true;
            chkDefaultMotion1.CheckedChanged += new EventHandler(chkDefaultMotion1_CheckedChanged);
            btnMFF1Reset.BackColor = Color.LightSlateGray;
            btnMFF1Reset.ForeColor = Color.White;
            btnMFF1Reset.Location = new Point(0x61, 0xea);
            btnMFF1Reset.Name = "btnMFF1Reset";
            btnMFF1Reset.Size = new Size(0x3e, 0x1f);
            btnMFF1Reset.TabIndex = 0xb8;
            btnMFF1Reset.Text = "Reset";
            btnMFF1Reset.UseVisualStyleBackColor = false;
            btnMFF1Reset.Click += new EventHandler(btnMFF1Reset_Click);
            btnMFF1Set.BackColor = Color.LightSlateGray;
            btnMFF1Set.ForeColor = Color.White;
            btnMFF1Set.Location = new Point(0x1d, 0xea);
            btnMFF1Set.Name = "btnMFF1Set";
            btnMFF1Set.Size = new Size(0x3e, 0x1f);
            btnMFF1Set.TabIndex = 0xb7;
            btnMFF1Set.Text = "Set";
            toolTip1.SetToolTip(btnMFF1Set, "Sets Threshold and debounce, writes in values");
            btnMFF1Set.UseVisualStyleBackColor = false;
            btnMFF1Set.Click += new EventHandler(btnMFF1Set_Click);
            rdoMFF1ClearDebounce.AutoSize = true;
            rdoMFF1ClearDebounce.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            rdoMFF1ClearDebounce.ForeColor = Color.White;
            rdoMFF1ClearDebounce.Location = new Point(0x162, 0xf2);
            rdoMFF1ClearDebounce.Name = "rdoMFF1ClearDebounce";
            rdoMFF1ClearDebounce.Size = new Size(0x66, 0x11);
            rdoMFF1ClearDebounce.TabIndex = 0xb5;
            rdoMFF1ClearDebounce.TabStop = true;
            rdoMFF1ClearDebounce.Text = "Clear Debounce";
            rdoMFF1ClearDebounce.UseVisualStyleBackColor = true;
            rdoMFF1ClearDebounce.CheckedChanged += new EventHandler(rdoMFF1ClearDebounce_CheckedChanged);
            p14.BackColor = Color.LightSlateGray;
            p14.Controls.Add(chkMFF1EnableLatch);
            p14.Controls.Add(chkYEFE);
            p14.Controls.Add(chkZEFE);
            p14.Controls.Add(chkXEFE);
            p14.Controls.Add(rdoMFF1And);
            p14.Controls.Add(rdoMFF1Or);
            p14.ForeColor = Color.White;
            p14.Location = new Point(7, 0x4c);
            p14.Name = "p14";
            p14.Size = new Size(0x1ca, 0x3e);
            p14.TabIndex = 0xb6;
            chkMFF1EnableLatch.AutoSize = true;
            chkMFF1EnableLatch.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            chkMFF1EnableLatch.Location = new Point(3, 0x1a);
            chkMFF1EnableLatch.Name = "chkMFF1EnableLatch";
            chkMFF1EnableLatch.Size = new Size(0x59, 0x11);
            chkMFF1EnableLatch.TabIndex = 0x80;
            chkMFF1EnableLatch.Text = "Enable Latch";
            chkMFF1EnableLatch.UseVisualStyleBackColor = true;
            chkMFF1EnableLatch.CheckedChanged += new EventHandler(chkMFF1EnableLatch_CheckedChanged);
            chkYEFE.AutoSize = true;
            chkYEFE.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            chkYEFE.Location = new Point(0xd8, 5);
            chkYEFE.Name = "chkYEFE";
            chkYEFE.Size = new Size(0x5b, 0x11);
            chkYEFE.TabIndex = 0x7f;
            chkYEFE.Text = "Enable Y-Axis";
            chkYEFE.UseVisualStyleBackColor = true;
            chkYEFE.CheckedChanged += new EventHandler(chkYEFE_CheckedChanged);
            chkZEFE.AutoSize = true;
            chkZEFE.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            chkZEFE.Location = new Point(0x13b, 4);
            chkZEFE.Name = "chkZEFE";
            chkZEFE.Size = new Size(0x5b, 0x11);
            chkZEFE.TabIndex = 0x7e;
            chkZEFE.Text = "Enable Z-Axis";
            chkZEFE.UseVisualStyleBackColor = true;
            chkZEFE.CheckedChanged += new EventHandler(chkZEFE_CheckedChanged);
            chkXEFE.AutoSize = true;
            chkXEFE.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            chkXEFE.Location = new Point(0x77, 5);
            chkXEFE.Name = "chkXEFE";
            chkXEFE.Size = new Size(0x5b, 0x11);
            chkXEFE.TabIndex = 0x7a;
            chkXEFE.Text = "Enable X-Axis";
            chkXEFE.UseVisualStyleBackColor = true;
            chkXEFE.CheckedChanged += new EventHandler(chkXEFE_CheckedChanged);
            rdoMFF1And.AutoSize = true;
            rdoMFF1And.Checked = true;
            rdoMFF1And.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            rdoMFF1And.Location = new Point(0x2c, 2);
            rdoMFF1And.Name = "rdoMFF1And";
            rdoMFF1And.Size = new Size(0x30, 0x11);
            rdoMFF1And.TabIndex = 1;
            rdoMFF1And.TabStop = true;
            rdoMFF1And.Text = "AND";
            rdoMFF1And.UseVisualStyleBackColor = true;
            rdoMFF1And.CheckedChanged += new EventHandler(rdoMFF1And_CheckedChanged);
            rdoMFF1Or.AutoSize = true;
            rdoMFF1Or.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            rdoMFF1Or.Location = new Point(2, 2);
            rdoMFF1Or.Name = "rdoMFF1Or";
            rdoMFF1Or.Size = new Size(0x29, 0x11);
            rdoMFF1Or.TabIndex = 0;
            rdoMFF1Or.TabStop = true;
            rdoMFF1Or.Text = "OR";
            rdoMFF1Or.UseVisualStyleBackColor = true;
            rdoMFF1Or.CheckedChanged += new EventHandler(rdoMFF1Or_CheckedChanged);
            p15.BackColor = Color.LightSlateGray;
            p15.Controls.Add(label20);
            p15.Controls.Add(label230);
            p15.Controls.Add(label231);
            p15.Controls.Add(ledMFF1EA);
            p15.Controls.Add(label232);
            p15.Controls.Add(label233);
            p15.Controls.Add(lblXHP);
            p15.Controls.Add(ledMFF1XHE);
            p15.Controls.Add(lblZHP);
            p15.Controls.Add(label236);
            p15.Controls.Add(ledMFF1YHE);
            p15.Controls.Add(lblYHP);
            p15.Controls.Add(label238);
            p15.Controls.Add(ledMFF1ZHE);
            p15.Controls.Add(label239);
            p15.Enabled = false;
            p15.ForeColor = Color.White;
            p15.Location = new Point(0x17, 0x11b);
            p15.Name = "p15";
            p15.Size = new Size(0x1b2, 0xdb);
            p15.TabIndex = 5;
            label20.AutoSize = true;
            label20.BackColor = Color.LightSlateGray;
            label20.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label20.ForeColor = Color.White;
            label20.Location = new Point(0xb0, 0x11);
            label20.Name = "label20";
            label20.Size = new Size(0x98, 0x18);
            label20.TabIndex = 0xb9;
            label20.Text = "Event Detected";
            label230.AutoSize = true;
            label230.BackColor = Color.LightSlateGray;
            label230.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label230.ForeColor = Color.White;
            label230.Location = new Point(0xd1, 0x65);
            label230.Name = "label230";
            label230.Size = new Size(0x51, 20);
            label230.TabIndex = 0xb8;
            label230.Text = "Direction";
            label231.AutoSize = true;
            label231.BackColor = Color.LightSlateGray;
            label231.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label231.ForeColor = Color.White;
            label231.Location = new Point(0x57, 0x65);
            label231.Name = "label231";
            label231.Size = new Size(0x72, 20);
            label231.TabIndex = 0xb7;
            label231.Text = "Axis of Event";
            ledMFF1EA.ForeColor = Color.White;
            ledMFF1EA.LedStyle = LedStyle.Round3D;
            ledMFF1EA.Location = new Point(0x14e, 4);
            ledMFF1EA.Name = "ledMFF1EA";
            ledMFF1EA.OffColor = Color.Red;
            ledMFF1EA.Size = new Size(0x38, 0x36);
            ledMFF1EA.TabIndex = 0xb6;
            label232.AutoSize = true;
            label232.BackColor = Color.LightSlateGray;
            label232.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label232.ForeColor = Color.White;
            label232.Location = new Point(0x17, 0x11);
            label232.Name = "label232";
            label232.Size = new Size(0x93, 0x18);
            label232.TabIndex = 0xb5;
            label232.Text = "Motion OR FF ";
            label233.AutoSize = true;
            label233.BackColor = Color.LightSlateGray;
            label233.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label233.ForeColor = Color.White;
            label233.Location = new Point(0x7a, 0x44);
            label233.Name = "label233";
            label233.Size = new Size(0x87, 0x18);
            label233.TabIndex = 180;
            label233.Text = "Motion Status";
            lblXHP.AutoSize = true;
            lblXHP.BackColor = Color.LightSlateGray;
            lblXHP.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblXHP.ForeColor = Color.White;
            lblXHP.Location = new Point(0xd5, 0x84);
            lblXHP.Name = "lblXHP";
            lblXHP.Size = new Size(70, 13);
            lblXHP.TabIndex = 0xaf;
            lblXHP.Text = "X-Direction";
            ledMFF1XHE.ForeColor = Color.White;
            ledMFF1XHE.LedStyle = LedStyle.Round3D;
            ledMFF1XHE.Location = new Point(0x9d, 0x7f);
            ledMFF1XHE.Name = "ledMFF1XHE";
            ledMFF1XHE.OffColor = Color.Red;
            ledMFF1XHE.Size = new Size(30, 30);
            ledMFF1XHE.TabIndex = 0xa7;
            lblZHP.AutoSize = true;
            lblZHP.BackColor = Color.LightSlateGray;
            lblZHP.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblZHP.ForeColor = Color.White;
            lblZHP.Location = new Point(0xd5, 0xc0);
            lblZHP.Name = "lblZHP";
            lblZHP.Size = new Size(70, 13);
            lblZHP.TabIndex = 0xb3;
            lblZHP.Text = "Z Direction";
            label236.AutoSize = true;
            label236.BackColor = Color.LightSlateGray;
            label236.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label236.ForeColor = Color.White;
            label236.Location = new Point(0x6c, 0x86);
            label236.Name = "label236";
            label236.Size = new Size(0x2e, 13);
            label236.TabIndex = 0xa8;
            label236.Text = "X- Axis";
            ledMFF1YHE.ForeColor = Color.White;
            ledMFF1YHE.LedStyle = LedStyle.Round3D;
            ledMFF1YHE.Location = new Point(0x9d, 0x9b);
            ledMFF1YHE.Name = "ledMFF1YHE";
            ledMFF1YHE.OffColor = Color.Red;
            ledMFF1YHE.Size = new Size(30, 30);
            ledMFF1YHE.TabIndex = 0xa9;
            lblYHP.AutoSize = true;
            lblYHP.BackColor = Color.LightSlateGray;
            lblYHP.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblYHP.ForeColor = Color.White;
            lblYHP.Location = new Point(0xd5, 0xa2);
            lblYHP.Name = "lblYHP";
            lblYHP.Size = new Size(70, 13);
            lblYHP.TabIndex = 0xb1;
            lblYHP.Text = "Y Direction";
            label238.AutoSize = true;
            label238.BackColor = Color.LightSlateGray;
            label238.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label238.ForeColor = Color.White;
            label238.Location = new Point(0x6c, 0xa2);
            label238.Name = "label238";
            label238.Size = new Size(0x2a, 13);
            label238.TabIndex = 170;
            label238.Text = "Y-Axis";
            ledMFF1ZHE.ForeColor = Color.White;
            ledMFF1ZHE.LedStyle = LedStyle.Round3D;
            ledMFF1ZHE.Location = new Point(0x9d, 0xb9);
            ledMFF1ZHE.Name = "ledMFF1ZHE";
            ledMFF1ZHE.OffColor = Color.Red;
            ledMFF1ZHE.Size = new Size(30, 30);
            ledMFF1ZHE.TabIndex = 0xab;
            label239.AutoSize = true;
            label239.BackColor = Color.LightSlateGray;
            label239.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label239.ForeColor = Color.White;
            label239.Location = new Point(0x6c, 0xc0);
            label239.Name = "label239";
            label239.Size = new Size(0x2a, 13);
            label239.TabIndex = 0xac;
            label239.Text = "Z-Axis";
            rdoMFF1DecDebounce.AutoSize = true;
            rdoMFF1DecDebounce.Checked = true;
            rdoMFF1DecDebounce.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            rdoMFF1DecDebounce.ForeColor = Color.White;
            rdoMFF1DecDebounce.Location = new Point(0xd1, 0xf2);
            rdoMFF1DecDebounce.Name = "rdoMFF1DecDebounce";
            rdoMFF1DecDebounce.Size = new Size(130, 0x11);
            rdoMFF1DecDebounce.TabIndex = 180;
            rdoMFF1DecDebounce.TabStop = true;
            rdoMFF1DecDebounce.Text = "Decrement Debounce";
            rdoMFF1DecDebounce.UseVisualStyleBackColor = true;
            rdoMFF1DecDebounce.CheckedChanged += new EventHandler(rdoMFF1DecDebounce_CheckedChanged);
            lblMFF1Threshold.AutoSize = true;
            lblMFF1Threshold.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblMFF1Threshold.ForeColor = Color.White;
            lblMFF1Threshold.Location = new Point(0x10, 0x9c);
            lblMFF1Threshold.Name = "lblMFF1Threshold";
            lblMFF1Threshold.Size = new Size(0x3f, 13);
            lblMFF1Threshold.TabIndex = 0x89;
            lblMFF1Threshold.Text = "Threshold";
            lblMFF1ThresholdVal.AutoSize = true;
            lblMFF1ThresholdVal.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblMFF1ThresholdVal.ForeColor = Color.White;
            lblMFF1ThresholdVal.Location = new Point(0x53, 0x9c);
            lblMFF1ThresholdVal.Name = "lblMFF1ThresholdVal";
            lblMFF1ThresholdVal.Size = new Size(14, 13);
            lblMFF1ThresholdVal.TabIndex = 0x8a;
            lblMFF1ThresholdVal.Text = "0";
            lblMFF1Debouncems.AutoSize = true;
            lblMFF1Debouncems.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblMFF1Debouncems.ForeColor = Color.White;
            lblMFF1Debouncems.Location = new Point(0x83, 0xc7);
            lblMFF1Debouncems.Name = "lblMFF1Debouncems";
            lblMFF1Debouncems.Size = new Size(0x16, 13);
            lblMFF1Debouncems.TabIndex = 0x8f;
            lblMFF1Debouncems.Text = "ms";
            lblMFF1Thresholdg.AutoSize = true;
            lblMFF1Thresholdg.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblMFF1Thresholdg.ForeColor = Color.White;
            lblMFF1Thresholdg.Location = new Point(0x86, 0x99);
            lblMFF1Thresholdg.Name = "lblMFF1Thresholdg";
            lblMFF1Thresholdg.Size = new Size(14, 13);
            lblMFF1Thresholdg.TabIndex = 0x8b;
            lblMFF1Thresholdg.Text = "g";
            tbMFF1Threshold.Location = new Point(0x95, 0x90);
            tbMFF1Threshold.Maximum = 0x7f;
            tbMFF1Threshold.Name = "tbMFF1Threshold";
            tbMFF1Threshold.Size = new Size(0x13c, 40);
            tbMFF1Threshold.TabIndex = 0x88;
            tbMFF1Threshold.Scroll += new EventHandler(tbMFF1Threshold_Scroll);
            lblMFF1DebounceVal.AutoSize = true;
            lblMFF1DebounceVal.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblMFF1DebounceVal.ForeColor = Color.White;
            lblMFF1DebounceVal.Location = new Point(0x52, 0xc7);
            lblMFF1DebounceVal.Name = "lblMFF1DebounceVal";
            lblMFF1DebounceVal.Size = new Size(14, 13);
            lblMFF1DebounceVal.TabIndex = 0x8e;
            lblMFF1DebounceVal.Text = "0";
            tbMFF1Debounce.Location = new Point(0x95, 190);
            tbMFF1Debounce.Maximum = 0xff;
            tbMFF1Debounce.Name = "tbMFF1Debounce";
            tbMFF1Debounce.Size = new Size(0x13c, 40);
            tbMFF1Debounce.TabIndex = 140;
            tbMFF1Debounce.Scroll += new EventHandler(tbMFF1Debounce_Scroll);
            lblMFF1Debounce.AutoSize = true;
            lblMFF1Debounce.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblMFF1Debounce.ForeColor = Color.White;
            lblMFF1Debounce.Location = new Point(0x10, 0xc7);
            lblMFF1Debounce.Name = "lblMFF1Debounce";
            lblMFF1Debounce.Size = new Size(0x45, 13);
            lblMFF1Debounce.TabIndex = 0x8d;
            lblMFF1Debounce.Text = "Debounce ";
            TransientDetection.BackColor = Color.SlateGray;
            TransientDetection.Controls.Add(chkZEnableTrans);
            TransientDetection.Controls.Add(chkYEnableTrans);
            TransientDetection.Controls.Add(chkXEnableTrans);
            TransientDetection.Controls.Add(lblTransDataType);
            TransientDetection.Controls.Add(pTrans2);
            TransientDetection.Controls.Add(gbTSNEW);
            TransientDetection.Controls.Add(legend2);
            TransientDetection.Controls.Add(gphXYZ);
            TransientDetection.Controls.Add(gbTS);
            TransientDetection.Location = new Point(4, 0x19);
            TransientDetection.Name = "TransientDetection";
            TransientDetection.Padding = new Padding(3);
            TransientDetection.Size = new Size(0x464, 0x20d);
            TransientDetection.TabIndex = 4;
            TransientDetection.Text = "Transient Detection";
            TransientDetection.UseVisualStyleBackColor = true;
            chkZEnableTrans.AutoSize = true;
            chkZEnableTrans.Checked = true;
            chkZEnableTrans.CheckState = CheckState.Checked;
            chkZEnableTrans.Location = new Point(0x323, 7);
            chkZEnableTrans.Name = "chkZEnableTrans";
            chkZEnableTrans.Size = new Size(70, 20);
            chkZEnableTrans.TabIndex = 0xf6;
            chkZEnableTrans.Text = "Z-Axis";
            chkZEnableTrans.UseVisualStyleBackColor = true;
            chkZEnableTrans.CheckedChanged += new EventHandler(chkZEnableTrans_CheckedChanged);
            chkYEnableTrans.AutoSize = true;
            chkYEnableTrans.Checked = true;
            chkYEnableTrans.CheckState = CheckState.Checked;
            chkYEnableTrans.Location = new Point(0x2dd, 7);
            chkYEnableTrans.Name = "chkYEnableTrans";
            chkYEnableTrans.Size = new Size(0x47, 20);
            chkYEnableTrans.TabIndex = 0xf5;
            chkYEnableTrans.Text = "Y-Axis";
            chkYEnableTrans.UseVisualStyleBackColor = true;
            chkYEnableTrans.CheckedChanged += new EventHandler(chkYEnableTrans_CheckedChanged);
            chkXEnableTrans.AutoSize = true;
            chkXEnableTrans.Checked = true;
            chkXEnableTrans.CheckState = CheckState.Checked;
            chkXEnableTrans.Location = new Point(0x298, 7);
            chkXEnableTrans.Name = "chkXEnableTrans";
            chkXEnableTrans.Size = new Size(70, 20);
            chkXEnableTrans.TabIndex = 0xf4;
            chkXEnableTrans.Text = "X-Axis";
            chkXEnableTrans.UseVisualStyleBackColor = true;
            chkXEnableTrans.CheckedChanged += new EventHandler(chkXEnableTrans_CheckedChanged);
            lblTransDataType.AutoSize = true;
            lblTransDataType.ForeColor = SystemColors.ActiveCaptionText;
            lblTransDataType.Location = new Point(0x207, 11);
            lblTransDataType.Name = "lblTransDataType";
            lblTransDataType.Size = new Size(0x63, 0x10);
            lblTransDataType.TabIndex = 0xbb;
            lblTransDataType.Text = "LPF Data Out";
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
            pTrans2.Location = new Point(0x8a, 0x137);
            pTrans2.Name = "pTrans2";
            pTrans2.Size = new Size(0xd9, 0xbd);
            pTrans2.TabIndex = 0xba;
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
            gbTSNEW.BackColor = Color.LightSlateGray;
            gbTSNEW.Controls.Add(btnResetTransientNEW);
            gbTSNEW.Controls.Add(btnSetTransientNEW);
            gbTSNEW.Controls.Add(rdoTransClearDebounceNEW);
            gbTSNEW.Controls.Add(pTransNEW);
            gbTSNEW.Controls.Add(tbTransDebounceNEW);
            gbTSNEW.Controls.Add(p19);
            gbTSNEW.Controls.Add(lblTransDebounceValNEW);
            gbTSNEW.Controls.Add(rdoTransDecDebounceNEW);
            gbTSNEW.Controls.Add(tbTransThresholdNEW);
            gbTSNEW.Controls.Add(lblTransDebounceNEW);
            gbTSNEW.Controls.Add(lblTransThresholdNEW);
            gbTSNEW.Controls.Add(lblTransThresholdValNEW);
            gbTSNEW.Controls.Add(lblTransThresholdgNEW);
            gbTSNEW.Controls.Add(lblTransDebouncemsNEW);
            gbTSNEW.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            gbTSNEW.ForeColor = Color.White;
            gbTSNEW.Location = new Point(0x234, 0x170);
            gbTSNEW.Name = "gbTSNEW";
            gbTSNEW.Size = new Size(0x1f5, 0xf9);
            gbTSNEW.TabIndex = 0xb9;
            gbTSNEW.TabStop = false;
            gbTSNEW.Text = "Transient Settings 1";
            gbTSNEW.Visible = false;
            btnResetTransientNEW.BackColor = Color.LightSlateGray;
            btnResetTransientNEW.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnResetTransientNEW.ForeColor = Color.White;
            btnResetTransientNEW.Location = new Point(0x52, 0xd4);
            btnResetTransientNEW.Name = "btnResetTransientNEW";
            btnResetTransientNEW.Size = new Size(0x49, 0x1f);
            btnResetTransientNEW.TabIndex = 0xca;
            btnResetTransientNEW.Text = "Reset";
            btnResetTransientNEW.UseVisualStyleBackColor = false;
            btnResetTransientNEW.Click += new EventHandler(btnResetTransientNEW_Click_1);
            btnSetTransientNEW.BackColor = Color.LightSlateGray;
            btnSetTransientNEW.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSetTransientNEW.ForeColor = Color.White;
            btnSetTransientNEW.Location = new Point(5, 0xd4);
            btnSetTransientNEW.Name = "btnSetTransientNEW";
            btnSetTransientNEW.Size = new Size(0x49, 0x1f);
            btnSetTransientNEW.TabIndex = 0xc9;
            btnSetTransientNEW.Text = "Set";
            toolTip1.SetToolTip(btnSetTransientNEW, "Sets the Threshold and the Debounce Counter ");
            btnSetTransientNEW.UseVisualStyleBackColor = false;
            btnSetTransientNEW.Click += new EventHandler(btnSetTransientNEW_Click_1);
            rdoTransClearDebounceNEW.AutoSize = true;
            rdoTransClearDebounceNEW.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            rdoTransClearDebounceNEW.ForeColor = Color.White;
            rdoTransClearDebounceNEW.Location = new Point(340, 0xd8);
            rdoTransClearDebounceNEW.Name = "rdoTransClearDebounceNEW";
            rdoTransClearDebounceNEW.Size = new Size(0x8a, 20);
            rdoTransClearDebounceNEW.TabIndex = 0xb7;
            rdoTransClearDebounceNEW.TabStop = true;
            rdoTransClearDebounceNEW.Text = "Clear Debounce";
            rdoTransClearDebounceNEW.UseVisualStyleBackColor = true;
            rdoTransClearDebounceNEW.CheckedChanged += new EventHandler(rdoTransClearDebounceNEW_CheckedChanged_1);
            pTransNEW.Controls.Add(chkDefaultTransSettings1);
            pTransNEW.Controls.Add(chkTransBypassHPFNEW);
            pTransNEW.Controls.Add(chkTransEnableLatchNEW);
            pTransNEW.Controls.Add(chkTransEnableXFlagNEW);
            pTransNEW.Controls.Add(chkTransEnableZFlagNEW);
            pTransNEW.Controls.Add(chkTransEnableYFlagNEW);
            pTransNEW.ForeColor = Color.White;
            pTransNEW.Location = new Point(4, 0x1c);
            pTransNEW.Name = "pTransNEW";
            pTransNEW.Size = new Size(0x1d8, 0x4c);
            pTransNEW.TabIndex = 0xb8;
            chkDefaultTransSettings1.AutoSize = true;
            chkDefaultTransSettings1.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            chkDefaultTransSettings1.ForeColor = Color.Gold;
            chkDefaultTransSettings1.Location = new Point(6, 9);
            chkDefaultTransSettings1.Name = "chkDefaultTransSettings1";
            chkDefaultTransSettings1.Size = new Size(0xd9, 20);
            chkDefaultTransSettings1.TabIndex = 0xcb;
            chkDefaultTransSettings1.Text = "Default Transient Settings 1";
            chkDefaultTransSettings1.UseVisualStyleBackColor = true;
            chkDefaultTransSettings1.CheckedChanged += new EventHandler(chkDefaultTransSettings1_CheckedChanged);
            chkTransBypassHPFNEW.AutoSize = true;
            chkTransBypassHPFNEW.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            chkTransBypassHPFNEW.Location = new Point(0x156, 5);
            chkTransBypassHPFNEW.Name = "chkTransBypassHPFNEW";
            chkTransBypassHPFNEW.Size = new Size(0x7e, 0x18);
            chkTransBypassHPFNEW.TabIndex = 0xcc;
            chkTransBypassHPFNEW.Text = "Bypass HPF";
            chkTransBypassHPFNEW.UseVisualStyleBackColor = true;
            chkTransBypassHPFNEW.CheckedChanged += new EventHandler(chkTransBypassHPFNEW_CheckedChanged);
            chkTransEnableLatchNEW.AutoSize = true;
            chkTransEnableLatchNEW.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
            chkTransEnableLatchNEW.Location = new Point(0x16f, 0x2b);
            chkTransEnableLatchNEW.Name = "chkTransEnableLatchNEW";
            chkTransEnableLatchNEW.Size = new Size(0x62, 0x13);
            chkTransEnableLatchNEW.TabIndex = 0x81;
            chkTransEnableLatchNEW.Text = "Enable Latch";
            chkTransEnableLatchNEW.UseVisualStyleBackColor = true;
            chkTransEnableLatchNEW.CheckedChanged += new EventHandler(chkTransEnableLatchNEW_CheckedChanged_1);
            chkTransEnableXFlagNEW.AutoSize = true;
            chkTransEnableXFlagNEW.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
            chkTransEnableXFlagNEW.Location = new Point(6, 0x2b);
            chkTransEnableXFlagNEW.Name = "chkTransEnableXFlagNEW";
            chkTransEnableXFlagNEW.Size = new Size(0x67, 0x13);
            chkTransEnableXFlagNEW.TabIndex = 120;
            chkTransEnableXFlagNEW.Text = "Enable X Flag";
            chkTransEnableXFlagNEW.UseVisualStyleBackColor = true;
            chkTransEnableXFlagNEW.CheckedChanged += new EventHandler(chkTransEnableXFlagNEW_CheckedChanged_1);
            chkTransEnableZFlagNEW.AutoSize = true;
            chkTransEnableZFlagNEW.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
            chkTransEnableZFlagNEW.Location = new Point(0xf7, 0x2b);
            chkTransEnableZFlagNEW.Name = "chkTransEnableZFlagNEW";
            chkTransEnableZFlagNEW.Size = new Size(0x66, 0x13);
            chkTransEnableZFlagNEW.TabIndex = 2;
            chkTransEnableZFlagNEW.Text = "Enable Z Flag";
            chkTransEnableZFlagNEW.UseVisualStyleBackColor = true;
            chkTransEnableZFlagNEW.CheckedChanged += new EventHandler(chkTransEnableZFlagNEW_CheckedChanged_1);
            chkTransEnableYFlagNEW.AutoSize = true;
            chkTransEnableYFlagNEW.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
            chkTransEnableYFlagNEW.Location = new Point(0x84, 0x2b);
            chkTransEnableYFlagNEW.Name = "chkTransEnableYFlagNEW";
            chkTransEnableYFlagNEW.Size = new Size(0x66, 0x13);
            chkTransEnableYFlagNEW.TabIndex = 1;
            chkTransEnableYFlagNEW.Text = "Enable Y Flag";
            chkTransEnableYFlagNEW.UseVisualStyleBackColor = true;
            chkTransEnableYFlagNEW.CheckedChanged += new EventHandler(chkTransEnableYFlagNEW_CheckedChanged_1);
            tbTransDebounceNEW.Location = new Point(0xae, 0xa5);
            tbTransDebounceNEW.Maximum = 0xff;
            tbTransDebounceNEW.Name = "tbTransDebounceNEW";
            tbTransDebounceNEW.Size = new Size(0x12e, 40);
            tbTransDebounceNEW.TabIndex = 0x79;
            tbTransDebounceNEW.Scroll += new EventHandler(tbTransDebounceNEW_Scroll_1);
            p19.BackColor = Color.LightSlateGray;
            p19.Controls.Add(lblTransNewPolZ);
            p19.Controls.Add(lblTransNewPolY);
            p19.Controls.Add(lblTransNewPolX);
            p19.Controls.Add(ledTransEANEW);
            p19.Controls.Add(label203);
            p19.Controls.Add(label204);
            p19.Controls.Add(ledTransZDetectNEW);
            p19.Controls.Add(ledTransYDetectNEW);
            p19.Controls.Add(ledTransXDetectNEW);
            p19.Controls.Add(label205);
            p19.Controls.Add(label206);
            p19.Controls.Add(label207);
            p19.Enabled = false;
            p19.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            p19.ForeColor = Color.White;
            p19.Location = new Point(0xf2, 0x10);
            p19.Name = "p19";
            p19.Size = new Size(0xe4, 0xbd);
            p19.TabIndex = 0xb8;
            p19.Visible = false;
            lblTransNewPolZ.AutoSize = true;
            lblTransNewPolZ.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTransNewPolZ.ForeColor = Color.White;
            lblTransNewPolZ.Location = new Point(0x29, 0x66);
            lblTransNewPolZ.Name = "lblTransNewPolZ";
            lblTransNewPolZ.Size = new Size(0x61, 20);
            lblTransNewPolZ.TabIndex = 0xa9;
            lblTransNewPolZ.Text = "Direction Z";
            lblTransNewPolY.AutoSize = true;
            lblTransNewPolY.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTransNewPolY.ForeColor = Color.White;
            lblTransNewPolY.Location = new Point(0x29, 0x48);
            lblTransNewPolY.Name = "lblTransNewPolY";
            lblTransNewPolY.Size = new Size(0x62, 20);
            lblTransNewPolY.TabIndex = 0xa8;
            lblTransNewPolY.Text = "Direction Y";
            lblTransNewPolX.AutoSize = true;
            lblTransNewPolX.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTransNewPolX.ForeColor = Color.White;
            lblTransNewPolX.Location = new Point(0x29, 0x2b);
            lblTransNewPolX.Name = "lblTransNewPolX";
            lblTransNewPolX.Size = new Size(0x62, 20);
            lblTransNewPolX.TabIndex = 0xa7;
            lblTransNewPolX.Text = "Direction X";
            ledTransEANEW.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            ledTransEANEW.ForeColor = Color.White;
            ledTransEANEW.LedStyle = LedStyle.Round3D;
            ledTransEANEW.Location = new Point(0xaf, 0x83);
            ledTransEANEW.Name = "ledTransEANEW";
            ledTransEANEW.OffColor = Color.Red;
            ledTransEANEW.Size = new Size(0x31, 0x34);
            ledTransEANEW.TabIndex = 0xa5;
            label203.AutoSize = true;
            label203.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label203.ForeColor = Color.White;
            label203.Location = new Point(10, 0x93);
            label203.Name = "label203";
            label203.Size = new Size(0x98, 0x18);
            label203.TabIndex = 0xa4;
            label203.Text = "Event Detected";
            label204.AutoSize = true;
            label204.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label204.ForeColor = Color.White;
            label204.Location = new Point(3, 3);
            label204.Name = "label204";
            label204.Size = new Size(0xb0, 0x18);
            label204.TabIndex = 0xa3;
            label204.Text = "Transient Status 1";
            ledTransZDetectNEW.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            ledTransZDetectNEW.ForeColor = Color.White;
            ledTransZDetectNEW.LedStyle = LedStyle.Round3D;
            ledTransZDetectNEW.Location = new Point(140, 0x63);
            ledTransZDetectNEW.Name = "ledTransZDetectNEW";
            ledTransZDetectNEW.OffColor = Color.Red;
            ledTransZDetectNEW.Size = new Size(30, 0x1d);
            ledTransZDetectNEW.TabIndex = 0xa2;
            ledTransYDetectNEW.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            ledTransYDetectNEW.ForeColor = Color.White;
            ledTransYDetectNEW.LedStyle = LedStyle.Round3D;
            ledTransYDetectNEW.Location = new Point(140, 70);
            ledTransYDetectNEW.Name = "ledTransYDetectNEW";
            ledTransYDetectNEW.OffColor = Color.Red;
            ledTransYDetectNEW.Size = new Size(30, 0x1b);
            ledTransYDetectNEW.TabIndex = 0xa1;
            ledTransXDetectNEW.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            ledTransXDetectNEW.ForeColor = Color.White;
            ledTransXDetectNEW.LedStyle = LedStyle.Round3D;
            ledTransXDetectNEW.Location = new Point(140, 0x27);
            ledTransXDetectNEW.Name = "ledTransXDetectNEW";
            ledTransXDetectNEW.OffColor = Color.Red;
            ledTransXDetectNEW.Size = new Size(30, 0x1d);
            ledTransXDetectNEW.TabIndex = 160;
            label205.AutoSize = true;
            label205.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label205.ForeColor = Color.White;
            label205.Location = new Point(15, 0x66);
            label205.Name = "label205";
            label205.Size = new Size(20, 20);
            label205.TabIndex = 0x77;
            label205.Text = "Z";
            label206.AutoSize = true;
            label206.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label206.ForeColor = Color.White;
            label206.Location = new Point(13, 0x48);
            label206.Name = "label206";
            label206.Size = new Size(0x15, 20);
            label206.TabIndex = 0x76;
            label206.Text = "Y";
            label207.AutoSize = true;
            label207.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label207.ForeColor = Color.White;
            label207.Location = new Point(14, 0x2b);
            label207.Name = "label207";
            label207.Size = new Size(0x15, 20);
            label207.TabIndex = 0x75;
            label207.Text = "X";
            lblTransDebounceValNEW.AutoSize = true;
            lblTransDebounceValNEW.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTransDebounceValNEW.ForeColor = Color.White;
            lblTransDebounceValNEW.Location = new Point(0x65, 0xac);
            lblTransDebounceValNEW.Name = "lblTransDebounceValNEW";
            lblTransDebounceValNEW.Size = new Size(0x10, 0x10);
            lblTransDebounceValNEW.TabIndex = 0x7b;
            lblTransDebounceValNEW.Text = "0";
            rdoTransDecDebounceNEW.AutoSize = true;
            rdoTransDecDebounceNEW.Checked = true;
            rdoTransDecDebounceNEW.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            rdoTransDecDebounceNEW.ForeColor = Color.White;
            rdoTransDecDebounceNEW.Location = new Point(160, 0xd8);
            rdoTransDecDebounceNEW.Name = "rdoTransDecDebounceNEW";
            rdoTransDecDebounceNEW.Size = new Size(0xb0, 20);
            rdoTransDecDebounceNEW.TabIndex = 0xb6;
            rdoTransDecDebounceNEW.TabStop = true;
            rdoTransDecDebounceNEW.Text = "Decrement Debounce";
            rdoTransDecDebounceNEW.UseVisualStyleBackColor = true;
            rdoTransDecDebounceNEW.CheckedChanged += new EventHandler(rdoTransDecDebounceNEW_CheckedChanged_1);
            tbTransThresholdNEW.Location = new Point(0xae, 0x74);
            tbTransThresholdNEW.Maximum = 0x7f;
            tbTransThresholdNEW.Name = "tbTransThresholdNEW";
            tbTransThresholdNEW.Size = new Size(0x12e, 40);
            tbTransThresholdNEW.TabIndex = 0x7f;
            tbTransThresholdNEW.Scroll += new EventHandler(tbTransThresholdNEW_Scroll_1);
            lblTransDebounceNEW.AutoSize = true;
            lblTransDebounceNEW.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTransDebounceNEW.ForeColor = Color.White;
            lblTransDebounceNEW.Location = new Point(5, 0xac);
            lblTransDebounceNEW.Name = "lblTransDebounceNEW";
            lblTransDebounceNEW.Size = new Size(0x4f, 0x10);
            lblTransDebounceNEW.TabIndex = 0x7a;
            lblTransDebounceNEW.Text = "Debounce";
            lblTransThresholdNEW.AutoSize = true;
            lblTransThresholdNEW.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTransThresholdNEW.ForeColor = Color.White;
            lblTransThresholdNEW.Location = new Point(5, 0x7a);
            lblTransThresholdNEW.Name = "lblTransThresholdNEW";
            lblTransThresholdNEW.Size = new Size(0x4e, 0x10);
            lblTransThresholdNEW.TabIndex = 0x70;
            lblTransThresholdNEW.Text = "Threshold";
            lblTransThresholdValNEW.AutoSize = true;
            lblTransThresholdValNEW.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTransThresholdValNEW.ForeColor = Color.White;
            lblTransThresholdValNEW.Location = new Point(0x74, 0x7a);
            lblTransThresholdValNEW.Name = "lblTransThresholdValNEW";
            lblTransThresholdValNEW.Size = new Size(0x10, 0x10);
            lblTransThresholdValNEW.TabIndex = 0x71;
            lblTransThresholdValNEW.Text = "0";
            lblTransThresholdgNEW.AutoSize = true;
            lblTransThresholdgNEW.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTransThresholdgNEW.ForeColor = Color.White;
            lblTransThresholdgNEW.Location = new Point(0xa1, 0x77);
            lblTransThresholdgNEW.Name = "lblTransThresholdgNEW";
            lblTransThresholdgNEW.Size = new Size(0x11, 0x10);
            lblTransThresholdgNEW.TabIndex = 120;
            lblTransThresholdgNEW.Text = "g";
            lblTransDebouncemsNEW.AutoSize = true;
            lblTransDebouncemsNEW.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTransDebouncemsNEW.ForeColor = Color.White;
            lblTransDebouncemsNEW.Location = new Point(0x97, 170);
            lblTransDebouncemsNEW.Name = "lblTransDebouncemsNEW";
            lblTransDebouncemsNEW.Size = new Size(0x1c, 0x10);
            lblTransDebouncemsNEW.TabIndex = 0x7c;
            lblTransDebouncemsNEW.Text = "ms";
            legend2.ForeColor = Color.White;
            legend2.Items.AddRange(new LegendItem[] { legendItem4, legendItem5, legendItem6 });
            legend2.Location = new Point(0x26b, 0x13e);
            legend2.Name = "legend2";
            legend2.Size = new Size(0x1b7, 0x23);
            legend2.TabIndex = 14;
            legendItem4.Source = XAxis;
            legendItem4.Text = "Transient X Axis ";
            legendItem5.Source = YAxis;
            legendItem5.Text = "Transient Y Axis ";
            legendItem6.Source = ZAxis;
            legendItem6.Text = "Transient Z Axis ";
            ZAxis.LineColor = Color.Gold;
            ZAxis.XAxis = xAxis1;
            ZAxis.YAxis = yAxis1;
            gphXYZ.BackColor = Color.LightSlateGray;
            gphXYZ.Caption = "Real Time Output";
            gphXYZ.CaptionBackColor = Color.LightSlateGray;
            gphXYZ.CaptionForeColor = SystemColors.ButtonHighlight;
            gphXYZ.ForeColor = SystemColors.Window;
            gphXYZ.ImmediateUpdates = true;
            gphXYZ.Location = new Point(0x201, 30);
            gphXYZ.Name = "gphXYZ";
            gphXYZ.PlotAreaBorder = Border.Raised;
            gphXYZ.Plots.AddRange(new WaveformPlot[] { XAxis, YAxis, ZAxis });
            gphXYZ.Size = new Size(0x25e, 0x11a);
            gphXYZ.TabIndex = 13;
            gphXYZ.XAxes.AddRange(new NationalInstruments.UI.XAxis[] { xAxis1 });
            gphXYZ.YAxes.AddRange(new NationalInstruments.UI.YAxis[] { yAxis1 });
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
            gbTS.Location = new Point(0, 0);
            gbTS.Name = "gbTS";
            gbTS.Size = new Size(0x1f5, 0x11b);
            gbTS.TabIndex = 12;
            gbTS.TabStop = false;
            gbTS.Text = "Transient Settings";
            btnResetTransient.BackColor = Color.LightSlateGray;
            btnResetTransient.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnResetTransient.ForeColor = Color.White;
            btnResetTransient.Location = new Point(0x52, 0xd4);
            btnResetTransient.Name = "btnResetTransient";
            btnResetTransient.Size = new Size(0x49, 0x1f);
            btnResetTransient.TabIndex = 0xca;
            btnResetTransient.Text = "Reset";
            btnResetTransient.UseVisualStyleBackColor = false;
            btnResetTransient.Click += new EventHandler(btnResetTransient_Click);
            btnSetTransient.BackColor = Color.LightSlateGray;
            btnSetTransient.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSetTransient.ForeColor = Color.White;
            btnSetTransient.Location = new Point(5, 0xd4);
            btnSetTransient.Name = "btnSetTransient";
            btnSetTransient.Size = new Size(0x49, 0x1f);
            btnSetTransient.TabIndex = 0xc9;
            btnSetTransient.Text = "Set";
            toolTip1.SetToolTip(btnSetTransient, "Sets the Threshold and the Debounce Counter ");
            btnSetTransient.UseVisualStyleBackColor = false;
            btnSetTransient.Click += new EventHandler(btnSetTransient_Click);
            rdoTransClearDebounce.AutoSize = true;
            rdoTransClearDebounce.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            rdoTransClearDebounce.ForeColor = Color.White;
            rdoTransClearDebounce.Location = new Point(340, 0xd8);
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
            p18.Size = new Size(0x1d8, 0x4c);
            p18.TabIndex = 0xb8;
            chkDefaultTransSettings.AutoSize = true;
            chkDefaultTransSettings.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            chkDefaultTransSettings.ForeColor = Color.Gold;
            chkDefaultTransSettings.Location = new Point(3, 9);
            chkDefaultTransSettings.Name = "chkDefaultTransSettings";
            chkDefaultTransSettings.Size = new Size(0xcd, 20);
            chkDefaultTransSettings.TabIndex = 0xcb;
            chkDefaultTransSettings.Text = "Default Transient Settings";
            chkDefaultTransSettings.UseVisualStyleBackColor = true;
            chkDefaultTransSettings.CheckedChanged += new EventHandler(chkDefaultTransSettings_CheckedChanged);
            chkTransBypassHPF.AutoSize = true;
            chkTransBypassHPF.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            chkTransBypassHPF.Location = new Point(0x156, 6);
            chkTransBypassHPF.Name = "chkTransBypassHPF";
            chkTransBypassHPF.Size = new Size(0x7e, 0x18);
            chkTransBypassHPF.TabIndex = 0xcc;
            chkTransBypassHPF.Text = "Bypass HPF";
            chkTransBypassHPF.UseVisualStyleBackColor = true;
            chkTransBypassHPF.CheckedChanged += new EventHandler(chkTransBypassHPF_CheckedChanged);
            chkTransEnableLatch.AutoSize = true;
            chkTransEnableLatch.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
            chkTransEnableLatch.Location = new Point(0x16d, 0x33);
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
            chkTransEnableXFlag.CheckedChanged += new EventHandler(chkTransEnableXFlag_CheckedChanged_1);
            chkTransEnableZFlag.AutoSize = true;
            chkTransEnableZFlag.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
            chkTransEnableZFlag.Location = new Point(0xf5, 0x33);
            chkTransEnableZFlag.Name = "chkTransEnableZFlag";
            chkTransEnableZFlag.Size = new Size(0x66, 0x13);
            chkTransEnableZFlag.TabIndex = 2;
            chkTransEnableZFlag.Text = "Enable Z Flag";
            chkTransEnableZFlag.UseVisualStyleBackColor = true;
            chkTransEnableZFlag.CheckedChanged += new EventHandler(chkTransEnableZFlag_CheckedChanged);
            chkTransEnableYFlag.AutoSize = true;
            chkTransEnableYFlag.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
            chkTransEnableYFlag.Location = new Point(130, 0x33);
            chkTransEnableYFlag.Name = "chkTransEnableYFlag";
            chkTransEnableYFlag.Size = new Size(0x66, 0x13);
            chkTransEnableYFlag.TabIndex = 1;
            chkTransEnableYFlag.Text = "Enable Y Flag";
            chkTransEnableYFlag.UseVisualStyleBackColor = true;
            chkTransEnableYFlag.CheckedChanged += new EventHandler(chkTransEnableYFlag_CheckedChanged);
            tbTransDebounce.Location = new Point(0xae, 0xa5);
            tbTransDebounce.Maximum = 0xff;
            tbTransDebounce.Name = "tbTransDebounce";
            tbTransDebounce.Size = new Size(0x12e, 40);
            tbTransDebounce.TabIndex = 0x79;
            tbTransDebounce.Scroll += new EventHandler(tbTransDebounce_Scroll_1);
            lblTransDebounceVal.AutoSize = true;
            lblTransDebounceVal.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTransDebounceVal.ForeColor = Color.White;
            lblTransDebounceVal.Location = new Point(0x65, 0xac);
            lblTransDebounceVal.Name = "lblTransDebounceVal";
            lblTransDebounceVal.Size = new Size(0x10, 0x10);
            lblTransDebounceVal.TabIndex = 0x7b;
            lblTransDebounceVal.Text = "0";
            rdoTransDecDebounce.AutoSize = true;
            rdoTransDecDebounce.Checked = true;
            rdoTransDecDebounce.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            rdoTransDecDebounce.ForeColor = Color.White;
            rdoTransDecDebounce.Location = new Point(160, 0xd8);
            rdoTransDecDebounce.Name = "rdoTransDecDebounce";
            rdoTransDecDebounce.Size = new Size(0xb0, 20);
            rdoTransDecDebounce.TabIndex = 0xb6;
            rdoTransDecDebounce.TabStop = true;
            rdoTransDecDebounce.Text = "Decrement Debounce";
            rdoTransDecDebounce.UseVisualStyleBackColor = true;
            rdoTransDecDebounce.CheckedChanged += new EventHandler(rdoTransDecDebounce_CheckedChanged);
            tbTransThreshold.Location = new Point(0xae, 0x74);
            tbTransThreshold.Maximum = 0x7f;
            tbTransThreshold.Name = "tbTransThreshold";
            tbTransThreshold.Size = new Size(0x12e, 40);
            tbTransThreshold.TabIndex = 0x7f;
            tbTransThreshold.Scroll += new EventHandler(tbTransThreshold_Scroll_1);
            lblTransDebounce.AutoSize = true;
            lblTransDebounce.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTransDebounce.ForeColor = Color.White;
            lblTransDebounce.Location = new Point(5, 0xac);
            lblTransDebounce.Name = "lblTransDebounce";
            lblTransDebounce.Size = new Size(0x4f, 0x10);
            lblTransDebounce.TabIndex = 0x7a;
            lblTransDebounce.Text = "Debounce";
            lblTransThreshold.AutoSize = true;
            lblTransThreshold.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTransThreshold.ForeColor = Color.White;
            lblTransThreshold.Location = new Point(5, 0x7a);
            lblTransThreshold.Name = "lblTransThreshold";
            lblTransThreshold.Size = new Size(0x4e, 0x10);
            lblTransThreshold.TabIndex = 0x70;
            lblTransThreshold.Text = "Threshold";
            lblTransThresholdVal.AutoSize = true;
            lblTransThresholdVal.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTransThresholdVal.ForeColor = Color.White;
            lblTransThresholdVal.Location = new Point(0x74, 0x7a);
            lblTransThresholdVal.Name = "lblTransThresholdVal";
            lblTransThresholdVal.Size = new Size(0x10, 0x10);
            lblTransThresholdVal.TabIndex = 0x71;
            lblTransThresholdVal.Text = "0";
            lblTransThresholdg.AutoSize = true;
            lblTransThresholdg.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTransThresholdg.ForeColor = Color.White;
            lblTransThresholdg.Location = new Point(0xa1, 0x77);
            lblTransThresholdg.Name = "lblTransThresholdg";
            lblTransThresholdg.Size = new Size(0x11, 0x10);
            lblTransThresholdg.TabIndex = 120;
            lblTransThresholdg.Text = "g";
            lblTransDebouncems.AutoSize = true;
            lblTransDebouncems.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTransDebouncems.ForeColor = Color.White;
            lblTransDebouncems.Location = new Point(0x97, 170);
            lblTransDebouncems.Name = "lblTransDebouncems";
            lblTransDebouncems.Size = new Size(0x1c, 0x10);
            lblTransDebouncems.TabIndex = 0x7c;
            lblTransDebouncems.Text = "ms";
            PulseDetection.BackColor = Color.LightSlateGray;
            PulseDetection.Controls.Add(gbSDPS);
            PulseDetection.Location = new Point(4, 0x19);
            PulseDetection.Name = "PulseDetection";
            PulseDetection.Padding = new Padding(3);
            PulseDetection.Size = new Size(0x464, 0x20d);
            PulseDetection.TabIndex = 5;
            PulseDetection.Text = "Pulse Detection";
            PulseDetection.UseVisualStyleBackColor = true;
            gbSDPS.BackColor = Color.LightSlateGray;
            gbSDPS.Controls.Add(panel15);
            gbSDPS.Controls.Add(rdoDefaultSPDP);
            gbSDPS.Controls.Add(rdoDefaultSP);
            gbSDPS.Controls.Add(btnResetPulseThresholds);
            gbSDPS.Controls.Add(btnSetPulseThresholds);
            gbSDPS.Controls.Add(tbPulseZThreshold);
            gbSDPS.Controls.Add(p12);
            gbSDPS.Controls.Add(p10);
            gbSDPS.Controls.Add(p11);
            gbSDPS.Controls.Add(tbPulseXThreshold);
            gbSDPS.Controls.Add(lblPulseYThreshold);
            gbSDPS.Controls.Add(tbPulseYThreshold);
            gbSDPS.Controls.Add(lblPulseYThresholdVal);
            gbSDPS.Controls.Add(lblPulseXThresholdg);
            gbSDPS.Controls.Add(lblPulseXThresholdVal);
            gbSDPS.Controls.Add(lblPulseZThresholdg);
            gbSDPS.Controls.Add(lblPulseZThreshold);
            gbSDPS.Controls.Add(lblPulseZThresholdVal);
            gbSDPS.Controls.Add(lblPulseYThresholdg);
            gbSDPS.Controls.Add(lblPulseXThreshold);
            gbSDPS.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            gbSDPS.ForeColor = Color.White;
            gbSDPS.Location = new Point(0x19, 3);
            gbSDPS.Name = "gbSDPS";
            gbSDPS.Size = new Size(0x430, 0x21b);
            gbSDPS.TabIndex = 8;
            gbSDPS.TabStop = false;
            gbSDPS.Text = "Single/ Double Tap Settings";
            panel15.Controls.Add(chkPulseLPFEnable);
            panel15.Controls.Add(chkPulseHPFBypass);
            panel15.Location = new Point(0x279, 0x2e);
            panel15.Name = "panel15";
            panel15.Size = new Size(0x15c, 0x27);
            panel15.TabIndex = 0xeb;
            chkPulseLPFEnable.AutoSize = true;
            chkPulseLPFEnable.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            chkPulseLPFEnable.ForeColor = Color.White;
            chkPulseLPFEnable.Location = new Point(0x2c, 10);
            chkPulseLPFEnable.Name = "chkPulseLPFEnable";
            chkPulseLPFEnable.Size = new Size(0x5b, 0x11);
            chkPulseLPFEnable.TabIndex = 0xea;
            chkPulseLPFEnable.Text = "LPF Enable";
            toolTip1.SetToolTip(chkPulseLPFEnable, "FDE FIFO Data Enable");
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
            toolTip1.SetToolTip(chkPulseHPFBypass, "HPF Data");
            chkPulseHPFBypass.UseVisualStyleBackColor = true;
            chkPulseHPFBypass.CheckedChanged += new EventHandler(chkPulseHPFBypass_CheckedChanged);
            rdoDefaultSPDP.AutoSize = true;
            rdoDefaultSPDP.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            rdoDefaultSPDP.ForeColor = Color.Gold;
            rdoDefaultSPDP.Location = new Point(0x344, 15);
            rdoDefaultSPDP.Name = "rdoDefaultSPDP";
            rdoDefaultSPDP.Size = new Size(0xdd, 20);
            rdoDefaultSPDP.TabIndex = 0xa4;
            rdoDefaultSPDP.TabStop = true;
            rdoDefaultSPDP.Text = "Default Single + Double Tap";
            rdoDefaultSPDP.UseVisualStyleBackColor = true;
            rdoDefaultSPDP.CheckedChanged += new EventHandler(rdoDefaultSPDP_CheckedChanged);
            rdoDefaultSP.AutoSize = true;
            rdoDefaultSP.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            rdoDefaultSP.ForeColor = Color.Gold;
            rdoDefaultSP.Location = new Point(0x279, 15);
            rdoDefaultSP.Name = "rdoDefaultSP";
            rdoDefaultSP.Size = new Size(0x9b, 20);
            rdoDefaultSP.TabIndex = 0xa3;
            rdoDefaultSP.TabStop = true;
            rdoDefaultSP.Text = "Default Single Tap";
            rdoDefaultSP.UseVisualStyleBackColor = true;
            rdoDefaultSP.CheckedChanged += new EventHandler(rdoDefaultSP_CheckedChanged);
            btnResetPulseThresholds.BackColor = Color.LightSlateGray;
            btnResetPulseThresholds.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnResetPulseThresholds.ForeColor = Color.White;
            btnResetPulseThresholds.Location = new Point(0x15c, 0x12d);
            btnResetPulseThresholds.Name = "btnResetPulseThresholds";
            btnResetPulseThresholds.Size = new Size(0xb0, 0x1f);
            btnResetPulseThresholds.TabIndex = 160;
            btnResetPulseThresholds.Text = "Reset XYZ Thresholds";
            toolTip1.SetToolTip(btnResetPulseThresholds, "Sets the XYZ Threshold Values which can be modified in Active Mode");
            btnResetPulseThresholds.UseVisualStyleBackColor = false;
            btnResetPulseThresholds.Click += new EventHandler(btnResetPulseThresholds_Click_1);
            btnSetPulseThresholds.BackColor = Color.LightSlateGray;
            btnSetPulseThresholds.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSetPulseThresholds.ForeColor = Color.White;
            btnSetPulseThresholds.Location = new Point(0xb7, 0x12d);
            btnSetPulseThresholds.Name = "btnSetPulseThresholds";
            btnSetPulseThresholds.Size = new Size(0x9b, 0x1f);
            btnSetPulseThresholds.TabIndex = 0x9f;
            btnSetPulseThresholds.Text = "Set XYZ Thresholds";
            toolTip1.SetToolTip(btnSetPulseThresholds, "Sets the XYZ Threshold Values which can be modified in Active Mode");
            btnSetPulseThresholds.UseVisualStyleBackColor = false;
            btnSetPulseThresholds.Click += new EventHandler(btnSetPulseThresholds_Click_1);
            tbPulseZThreshold.BackColor = Color.LightSlateGray;
            tbPulseZThreshold.Location = new Point(0x98, 250);
            tbPulseZThreshold.Maximum = 0x7f;
            tbPulseZThreshold.Name = "tbPulseZThreshold";
            tbPulseZThreshold.Size = new Size(0x174, 40);
            tbPulseZThreshold.TabIndex = 140;
            tbPulseZThreshold.Scroll += new EventHandler(tbPulseZThreshold_Scroll_1);
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
            p12.Location = new Point(4, 0x155);
            p12.Name = "p12";
            p12.Size = new Size(0x214, 180);
            p12.TabIndex = 0x9e;
            btnPulseResetTime2ndPulse.BackColor = Color.LightSlateGray;
            btnPulseResetTime2ndPulse.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnPulseResetTime2ndPulse.ForeColor = Color.White;
            btnPulseResetTime2ndPulse.Location = new Point(370, 0x87);
            btnPulseResetTime2ndPulse.Name = "btnPulseResetTime2ndPulse";
            btnPulseResetTime2ndPulse.Size = new Size(150, 0x1f);
            btnPulseResetTime2ndPulse.TabIndex = 0x9f;
            btnPulseResetTime2ndPulse.Text = "Reset Time Limits";
            toolTip1.SetToolTip(btnPulseResetTime2ndPulse, "Sets the time between pulses (the latency) and the 2nd pulse window time period");
            btnPulseResetTime2ndPulse.UseVisualStyleBackColor = false;
            btnPulseResetTime2ndPulse.Click += new EventHandler(btnPulseResetTime2ndPulse_Click_1);
            btnPulseSetTime2ndPulse.BackColor = Color.LightSlateGray;
            btnPulseSetTime2ndPulse.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnPulseSetTime2ndPulse.ForeColor = Color.White;
            btnPulseSetTime2ndPulse.Location = new Point(0xd3, 0x87);
            btnPulseSetTime2ndPulse.Name = "btnPulseSetTime2ndPulse";
            btnPulseSetTime2ndPulse.Size = new Size(0x7b, 0x1f);
            btnPulseSetTime2ndPulse.TabIndex = 0x9e;
            btnPulseSetTime2ndPulse.Text = "Set Time Limits";
            toolTip1.SetToolTip(btnPulseSetTime2ndPulse, "Sets the time between pulses (the latency) and the 2nd pulse window time period");
            btnPulseSetTime2ndPulse.UseVisualStyleBackColor = false;
            btnPulseSetTime2ndPulse.Click += new EventHandler(btnPulseSetTime2ndPulse_Click_1);
            tbPulseLatency.Location = new Point(0xbc, 0x25);
            tbPulseLatency.Maximum = 0xff;
            tbPulseLatency.Name = "tbPulseLatency";
            tbPulseLatency.Size = new Size(0x14c, 40);
            tbPulseLatency.TabIndex = 0x95;
            tbPulseLatency.Scroll += new EventHandler(tbPulseLatency_Scroll_1);
            chkPulseEnableXDP.AutoSize = true;
            chkPulseEnableXDP.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
            chkPulseEnableXDP.ForeColor = Color.White;
            chkPulseEnableXDP.Location = new Point(6, 3);
            chkPulseEnableXDP.Name = "chkPulseEnableXDP";
            chkPulseEnableXDP.Size = new Size(0x60, 0x13);
            chkPulseEnableXDP.TabIndex = 0x7c;
            chkPulseEnableXDP.Text = "Enable X DP";
            chkPulseEnableXDP.UseVisualStyleBackColor = true;
            chkPulseEnableXDP.CheckedChanged += new EventHandler(chkPulseEnableXDP_CheckedChanged_1);
            chkPulseEnableYDP.AutoSize = true;
            chkPulseEnableYDP.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
            chkPulseEnableYDP.ForeColor = Color.White;
            chkPulseEnableYDP.Location = new Point(0x66, 3);
            chkPulseEnableYDP.Name = "chkPulseEnableYDP";
            chkPulseEnableYDP.Size = new Size(0x5f, 0x13);
            chkPulseEnableYDP.TabIndex = 0x7d;
            chkPulseEnableYDP.Text = "Enable Y DP";
            chkPulseEnableYDP.UseVisualStyleBackColor = true;
            chkPulseEnableYDP.CheckedChanged += new EventHandler(chkPulseEnableYDP_CheckedChanged_1);
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
            tbPulse2ndPulseWin.Size = new Size(0x14c, 40);
            tbPulse2ndPulseWin.TabIndex = 0x9a;
            tbPulse2ndPulseWin.Scroll += new EventHandler(tbPulse2ndPulseWin_Scroll_1);
            chkPulseIgnorLatentPulses.AutoSize = true;
            chkPulseIgnorLatentPulses.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
            chkPulseIgnorLatentPulses.ForeColor = Color.White;
            chkPulseIgnorLatentPulses.Location = new Point(0x14d, 4);
            chkPulseIgnorLatentPulses.Name = "chkPulseIgnorLatentPulses";
            chkPulseIgnorLatentPulses.Size = new Size(0x83, 0x13);
            chkPulseIgnorLatentPulses.TabIndex = 0x99;
            chkPulseIgnorLatentPulses.Text = "Ignor Latent Pulses";
            chkPulseIgnorLatentPulses.UseVisualStyleBackColor = true;
            chkPulseIgnorLatentPulses.CheckedChanged += new EventHandler(chkPulseIgnorLatentPulses_CheckedChanged_1);
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
            p10.Location = new Point(5, 0x1b);
            p10.Name = "p10";
            p10.Size = new Size(0x213, 0x79);
            p10.TabIndex = 5;
            btnResetFirstPulseTimeLimit.BackColor = Color.LightSlateGray;
            btnResetFirstPulseTimeLimit.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnResetFirstPulseTimeLimit.ForeColor = Color.White;
            btnResetFirstPulseTimeLimit.Location = new Point(0x180, 0x52);
            btnResetFirstPulseTimeLimit.Name = "btnResetFirstPulseTimeLimit";
            btnResetFirstPulseTimeLimit.Size = new Size(0x87, 0x1f);
            btnResetFirstPulseTimeLimit.TabIndex = 150;
            btnResetFirstPulseTimeLimit.Text = "Reset Time Limit";
            toolTip1.SetToolTip(btnResetFirstPulseTimeLimit, " Sets the Pulse Time Limit");
            btnResetFirstPulseTimeLimit.UseVisualStyleBackColor = false;
            btnResetFirstPulseTimeLimit.Click += new EventHandler(btnResetFirstPulseTimeLimit_Click_1);
            btnSetFirstPulseTimeLimit.BackColor = Color.LightSlateGray;
            btnSetFirstPulseTimeLimit.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSetFirstPulseTimeLimit.ForeColor = Color.White;
            btnSetFirstPulseTimeLimit.Location = new Point(210, 0x52);
            btnSetFirstPulseTimeLimit.Name = "btnSetFirstPulseTimeLimit";
            btnSetFirstPulseTimeLimit.Size = new Size(0x7b, 0x1f);
            btnSetFirstPulseTimeLimit.TabIndex = 0x95;
            btnSetFirstPulseTimeLimit.Text = "Set Time Limit";
            toolTip1.SetToolTip(btnSetFirstPulseTimeLimit, "Sets the Pulse Time Limit");
            btnSetFirstPulseTimeLimit.UseVisualStyleBackColor = false;
            btnSetFirstPulseTimeLimit.Click += new EventHandler(btnSetFirstPulseTimeLimit_Click_1);
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
            chkPulseEnableLatch.CheckedChanged += new EventHandler(chkPulseEnableLatch_CheckedChanged_1);
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
            chkPulseEnableXSP.CheckedChanged += new EventHandler(chkPulseEnableXSP_CheckedChanged_1);
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
            chkPulseEnableYSP.CheckedChanged += new EventHandler(chkPulseEnableYSP_CheckedChanged_1);
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
            chkPulseEnableZSP.CheckedChanged += new EventHandler(chkPulseEnableZSP_CheckedChanged_1);
            tbFirstPulseTimeLimit.BackColor = Color.LightSlateGray;
            tbFirstPulseTimeLimit.Location = new Point(0xbb, 0x27);
            tbFirstPulseTimeLimit.Maximum = 0xff;
            tbFirstPulseTimeLimit.Name = "tbFirstPulseTimeLimit";
            tbFirstPulseTimeLimit.Size = new Size(0x151, 40);
            tbFirstPulseTimeLimit.TabIndex = 0x90;
            tbFirstPulseTimeLimit.Scroll += new EventHandler(tbFirstPulseTimeLimit_Scroll_1);
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
            toolTip1.SetToolTip(lblFirstPulseTimeLimit, "This value should be greater than 2ms to make sense");
            p11.BackColor = Color.LightSlateGray;
            p11.Controls.Add(label15);
            p11.Controls.Add(ledPulseDouble);
            p11.Controls.Add(lblPPolZ);
            p11.Controls.Add(lblPPolY);
            p11.Controls.Add(lblPPolX);
            p11.Controls.Add(label255);
            p11.Controls.Add(label256);
            p11.Controls.Add(ledPulseEA);
            p11.Controls.Add(label258);
            p11.Controls.Add(ledPZ);
            p11.Controls.Add(ledPX);
            p11.Controls.Add(label268);
            p11.Controls.Add(label287);
            p11.Controls.Add(ledPY);
            p11.Enabled = false;
            p11.Location = new Point(0x249, 0x84);
            p11.Name = "p11";
            p11.Size = new Size(0x1b7, 0x177);
            p11.TabIndex = 0x72;
            label15.AutoSize = true;
            label15.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label15.ForeColor = Color.White;
            label15.Location = new Point(0x6f, 0x3f);
            label15.Name = "label15";
            label15.Size = new Size(0x65, 20);
            label15.TabIndex = 0xad;
            label15.Text = "Double Tap";
            ledPulseDouble.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            ledPulseDouble.ForeColor = Color.White;
            ledPulseDouble.LedStyle = LedStyle.Round3D;
            ledPulseDouble.Location = new Point(0xdf, 0x39);
            ledPulseDouble.Name = "ledPulseDouble";
            ledPulseDouble.OffColor = Color.Red;
            ledPulseDouble.Size = new Size(40, 40);
            ledPulseDouble.TabIndex = 0xac;
            lblPPolZ.AutoSize = true;
            lblPPolZ.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPPolZ.ForeColor = Color.White;
            lblPPolZ.Location = new Point(0x115, 0xed);
            lblPPolZ.Name = "lblPPolZ";
            lblPPolZ.Size = new Size(0x61, 20);
            lblPPolZ.TabIndex = 0xab;
            lblPPolZ.Text = "Direction Z";
            lblPPolY.AutoSize = true;
            lblPPolY.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPPolY.ForeColor = Color.White;
            lblPPolY.Location = new Point(0x115, 0xb0);
            lblPPolY.Name = "lblPPolY";
            lblPPolY.Size = new Size(0x62, 20);
            lblPPolY.TabIndex = 170;
            lblPPolY.Text = "Direction Y";
            lblPPolX.AutoSize = true;
            lblPPolX.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPPolX.ForeColor = Color.White;
            lblPPolX.Location = new Point(0x115, 0x72);
            lblPPolX.Name = "lblPPolX";
            lblPPolX.Size = new Size(0x62, 20);
            lblPPolX.TabIndex = 0xa9;
            lblPPolX.Text = "Direction X";
            label255.AutoSize = true;
            label255.Font = new Font("Microsoft Sans Serif", 15.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label255.ForeColor = Color.White;
            label255.Location = new Point(60, 0x11);
            label255.Name = "label255";
            label255.Size = new Size(0x175, 0x19);
            label255.TabIndex = 0xa8;
            label255.Text = "Single Tap and Double Tap Status";
            label256.AutoSize = true;
            label256.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label256.ForeColor = Color.White;
            label256.Location = new Point(0x60, 0x139);
            label256.Name = "label256";
            label256.Size = new Size(0x86, 20);
            label256.TabIndex = 0xa5;
            label256.Text = "Event Detected";
            ledPulseEA.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            ledPulseEA.ForeColor = Color.White;
            ledPulseEA.LedStyle = LedStyle.Round3D;
            ledPulseEA.Location = new Point(0xf7, 0x121);
            ledPulseEA.Name = "ledPulseEA";
            ledPulseEA.OffColor = Color.Red;
            ledPulseEA.Size = new Size(60, 60);
            ledPulseEA.TabIndex = 0xa4;
            label258.AutoSize = true;
            label258.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label258.ForeColor = Color.White;
            label258.Location = new Point(0x6f, 0xef);
            label258.Name = "label258";
            label258.Size = new Size(0x63, 20);
            label258.TabIndex = 0xa7;
            label258.Text = "Z Detected";
            ledPZ.BlinkMode = LedBlinkMode.BlinkWhenOn;
            ledPZ.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            ledPZ.ForeColor = Color.White;
            ledPZ.LedStyle = LedStyle.Round3D;
            ledPZ.Location = new Point(0xdf, 0xe3);
            ledPZ.Name = "ledPZ";
            ledPZ.OffColor = Color.Red;
            ledPZ.Size = new Size(40, 40);
            ledPZ.TabIndex = 0xa6;
            ledPX.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            ledPX.ForeColor = Color.White;
            ledPX.LedStyle = LedStyle.Round3D;
            ledPX.Location = new Point(0xdf, 0x6a);
            ledPX.Name = "ledPX";
            ledPX.OffColor = Color.Red;
            ledPX.Size = new Size(40, 40);
            ledPX.TabIndex = 0x77;
            label268.AutoSize = true;
            label268.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label268.ForeColor = Color.White;
            label268.Location = new Point(0x6f, 0xb2);
            label268.Name = "label268";
            label268.Size = new Size(100, 20);
            label268.TabIndex = 0xa5;
            label268.Text = "Y Detected";
            label287.AutoSize = true;
            label287.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label287.ForeColor = Color.White;
            label287.Location = new Point(0x6f, 0x74);
            label287.Name = "label287";
            label287.Size = new Size(100, 20);
            label287.TabIndex = 0xa3;
            label287.Text = "X Detected";
            ledPY.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            ledPY.ForeColor = Color.White;
            ledPY.LedStyle = LedStyle.Round3D;
            ledPY.Location = new Point(0xdf, 0xa6);
            ledPY.Name = "ledPY";
            ledPY.OffColor = Color.Red;
            ledPY.Size = new Size(40, 40);
            ledPY.TabIndex = 0xa4;
            tbPulseXThreshold.BackColor = Color.LightSlateGray;
            tbPulseXThreshold.Location = new Point(0x98, 0x9c);
            tbPulseXThreshold.Maximum = 0x7f;
            tbPulseXThreshold.Name = "tbPulseXThreshold";
            tbPulseXThreshold.Size = new Size(0x174, 40);
            tbPulseXThreshold.TabIndex = 0x84;
            tbPulseXThreshold.Scroll += new EventHandler(tbPulseXThreshold_Scroll_1);
            lblPulseYThreshold.AutoSize = true;
            lblPulseYThreshold.BackColor = Color.LightSlateGray;
            lblPulseYThreshold.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPulseYThreshold.ForeColor = Color.White;
            lblPulseYThreshold.Location = new Point(6, 0xd0);
            lblPulseYThreshold.Name = "lblPulseYThreshold";
            lblPulseYThreshold.Size = new Size(0x57, 15);
            lblPulseYThreshold.TabIndex = 0x89;
            lblPulseYThreshold.Text = " Y Threshold";
            tbPulseYThreshold.BackColor = Color.LightSlateGray;
            tbPulseYThreshold.Location = new Point(0x98, 0xcc);
            tbPulseYThreshold.Maximum = 0x7f;
            tbPulseYThreshold.Name = "tbPulseYThreshold";
            tbPulseYThreshold.Size = new Size(0x174, 40);
            tbPulseYThreshold.TabIndex = 0x88;
            tbPulseYThreshold.Scroll += new EventHandler(tbPulseYThreshold_Scroll_1);
            lblPulseYThresholdVal.AutoSize = true;
            lblPulseYThresholdVal.BackColor = Color.LightSlateGray;
            lblPulseYThresholdVal.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPulseYThresholdVal.ForeColor = Color.White;
            lblPulseYThresholdVal.Location = new Point(0x63, 0xd0);
            lblPulseYThresholdVal.Name = "lblPulseYThresholdVal";
            lblPulseYThresholdVal.Size = new Size(15, 15);
            lblPulseYThresholdVal.TabIndex = 0x8a;
            lblPulseYThresholdVal.Text = "0";
            lblPulseXThresholdg.AutoSize = true;
            lblPulseXThresholdg.BackColor = Color.LightSlateGray;
            lblPulseXThresholdg.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPulseXThresholdg.ForeColor = Color.White;
            lblPulseXThresholdg.Location = new Point(0x8e, 160);
            lblPulseXThresholdg.Name = "lblPulseXThresholdg";
            lblPulseXThresholdg.Size = new Size(15, 15);
            lblPulseXThresholdg.TabIndex = 0x87;
            lblPulseXThresholdg.Text = "g";
            lblPulseXThresholdVal.AutoSize = true;
            lblPulseXThresholdVal.BackColor = Color.LightSlateGray;
            lblPulseXThresholdVal.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPulseXThresholdVal.ForeColor = Color.White;
            lblPulseXThresholdVal.Location = new Point(0x63, 0xa2);
            lblPulseXThresholdVal.Name = "lblPulseXThresholdVal";
            lblPulseXThresholdVal.Size = new Size(15, 15);
            lblPulseXThresholdVal.TabIndex = 0x86;
            lblPulseXThresholdVal.Text = "0";
            lblPulseZThresholdg.AutoSize = true;
            lblPulseZThresholdg.BackColor = Color.LightSlateGray;
            lblPulseZThresholdg.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPulseZThresholdg.ForeColor = Color.White;
            lblPulseZThresholdg.Location = new Point(0x8e, 0xfb);
            lblPulseZThresholdg.Name = "lblPulseZThresholdg";
            lblPulseZThresholdg.Size = new Size(15, 15);
            lblPulseZThresholdg.TabIndex = 0x8f;
            lblPulseZThresholdg.Text = "g";
            lblPulseZThreshold.AutoSize = true;
            lblPulseZThreshold.BackColor = Color.LightSlateGray;
            lblPulseZThreshold.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPulseZThreshold.ForeColor = Color.White;
            lblPulseZThreshold.Location = new Point(6, 0xfd);
            lblPulseZThreshold.Name = "lblPulseZThreshold";
            lblPulseZThreshold.Size = new Size(0x57, 15);
            lblPulseZThreshold.TabIndex = 0x8d;
            lblPulseZThreshold.Text = " Z Threshold";
            lblPulseZThresholdVal.AutoSize = true;
            lblPulseZThresholdVal.BackColor = Color.LightSlateGray;
            lblPulseZThresholdVal.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPulseZThresholdVal.ForeColor = Color.White;
            lblPulseZThresholdVal.Location = new Point(0x63, 0xfd);
            lblPulseZThresholdVal.Name = "lblPulseZThresholdVal";
            lblPulseZThresholdVal.Size = new Size(15, 15);
            lblPulseZThresholdVal.TabIndex = 0x8e;
            lblPulseZThresholdVal.Text = "0";
            lblPulseYThresholdg.AutoSize = true;
            lblPulseYThresholdg.BackColor = Color.LightSlateGray;
            lblPulseYThresholdg.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPulseYThresholdg.ForeColor = Color.White;
            lblPulseYThresholdg.Location = new Point(0x8e, 0xce);
            lblPulseYThresholdg.Name = "lblPulseYThresholdg";
            lblPulseYThresholdg.Size = new Size(15, 15);
            lblPulseYThresholdg.TabIndex = 0x8b;
            lblPulseYThresholdg.Text = "g";
            lblPulseXThreshold.AutoSize = true;
            lblPulseXThreshold.BackColor = Color.LightSlateGray;
            lblPulseXThreshold.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPulseXThreshold.ForeColor = Color.White;
            lblPulseXThreshold.Location = new Point(6, 0xa2);
            lblPulseXThreshold.Name = "lblPulseXThreshold";
            lblPulseXThreshold.Size = new Size(0x58, 15);
            lblPulseXThreshold.TabIndex = 0x85;
            lblPulseXThreshold.Text = " X Threshold";
            FIFOPage.BackColor = Color.SlateGray;
            FIFOPage.Controls.Add(rdoFIFO14bitDataDisplay);
            FIFOPage.Controls.Add(rtbFIFOdump);
            FIFOPage.Controls.Add(rdoFIFO8bitDataDisplay);
            FIFOPage.Controls.Add(gb3bF);
            FIFOPage.Location = new Point(4, 0x19);
            FIFOPage.Name = "FIFOPage";
            FIFOPage.Padding = new Padding(3);
            FIFOPage.Size = new Size(0x464, 0x20d);
            FIFOPage.TabIndex = 7;
            FIFOPage.Text = "FIFO";
            FIFOPage.UseVisualStyleBackColor = true;
            rdoFIFO14bitDataDisplay.AutoSize = true;
            rdoFIFO14bitDataDisplay.BackColor = Color.LightSlateGray;
            rdoFIFO14bitDataDisplay.Checked = true;
            rdoFIFO14bitDataDisplay.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            rdoFIFO14bitDataDisplay.ForeColor = Color.Black;
            rdoFIFO14bitDataDisplay.Location = new Point(0x35a, 11);
            rdoFIFO14bitDataDisplay.Name = "rdoFIFO14bitDataDisplay";
            rdoFIFO14bitDataDisplay.Size = new Size(160, 0x18);
            rdoFIFO14bitDataDisplay.TabIndex = 220;
            rdoFIFO14bitDataDisplay.TabStop = true;
            rdoFIFO14bitDataDisplay.Text = "View 14-bit Data";
            rdoFIFO14bitDataDisplay.UseVisualStyleBackColor = false;
            rdoFIFO14bitDataDisplay.CheckedChanged += new EventHandler(rdoFIFO14bitDataDisplay_CheckedChanged);
            rtbFIFOdump.Location = new Point(0x2ad, 0x2f);
            rtbFIFOdump.Name = "rtbFIFOdump";
            rtbFIFOdump.Size = new Size(0x19c, 0x1d8);
            rtbFIFOdump.TabIndex = 0xcf;
            rtbFIFOdump.Text = "";
            rdoFIFO8bitDataDisplay.AutoSize = true;
            rdoFIFO8bitDataDisplay.BackColor = Color.LightSlateGray;
            rdoFIFO8bitDataDisplay.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            rdoFIFO8bitDataDisplay.ForeColor = Color.Black;
            rdoFIFO8bitDataDisplay.Location = new Point(0x2ba, 10);
            rdoFIFO8bitDataDisplay.Name = "rdoFIFO8bitDataDisplay";
            rdoFIFO8bitDataDisplay.Size = new Size(0x90, 0x18);
            rdoFIFO8bitDataDisplay.TabIndex = 0xdb;
            rdoFIFO8bitDataDisplay.Text = "View 8bit Data";
            rdoFIFO8bitDataDisplay.UseVisualStyleBackColor = false;
            rdoFIFO8bitDataDisplay.CheckedChanged += new EventHandler(rdoFIFO8bitDataDisplay_CheckedChanged);
            gb3bF.BackColor = Color.LightSlateGray;
            gb3bF.Controls.Add(p5);
            gb3bF.Controls.Add(p4);
            gb3bF.Controls.Add(rdoTriggerMode);
            gb3bF.Controls.Add(chkDisableFIFO);
            gb3bF.Controls.Add(rdoFill);
            gb3bF.Controls.Add(rdoCircular);
            gb3bF.Controls.Add(btnSetMode);
            gb3bF.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            gb3bF.ForeColor = Color.White;
            gb3bF.Location = new Point(3, 6);
            gb3bF.Name = "gb3bF";
            gb3bF.Size = new Size(0x2a4, 560);
            gb3bF.TabIndex = 0x81;
            gb3bF.TabStop = false;
            gb3bF.Text = "32 Sample FIFO";
            p5.BackColor = Color.LightSteelBlue;
            p5.Controls.Add(label19);
            p5.Controls.Add(label17);
            p5.Controls.Add(label14);
            p5.Controls.Add(label13);
            p5.Controls.Add(ledTrigMFF);
            p5.Controls.Add(ledTrigTap);
            p5.Controls.Add(ledTrigLP);
            p5.Controls.Add(ledTrigTrans);
            p5.Controls.Add(button1);
            p5.Controls.Add(lblCurrent_FIFO_Count);
            p5.Controls.Add(lblF_Count);
            p5.Controls.Add(ledOverFlow);
            p5.Controls.Add(ledWatermark);
            p5.Controls.Add(label64);
            p5.Controls.Add(label63);
            p5.Enabled = false;
            p5.ForeColor = Color.Black;
            p5.Location = new Point(3, 0x16a);
            p5.Name = "p5";
            p5.Size = new Size(0x299, 0x98);
            p5.TabIndex = 0xda;
            label19.AutoSize = true;
            label19.Location = new Point(0x1f6, 8);
            label19.Name = "label19";
            label19.Size = new Size(0x35, 0x18);
            label19.TabIndex = 0xa4;
            label19.Text = "MFF";
            label17.AutoSize = true;
            label17.Location = new Point(0x173, 8);
            label17.Name = "label17";
            label17.Size = new Size(0x2e, 0x18);
            label17.TabIndex = 0xa3;
            label17.Text = "Tap";
            label14.AutoSize = true;
            label14.Location = new Point(0xf8, 8);
            label14.Name = "label14";
            label14.Size = new Size(0x22, 0x18);
            label14.TabIndex = 0xa2;
            label14.Text = "LP";
            label13.AutoSize = true;
            label13.Location = new Point(0x4e, 8);
            label13.Name = "label13";
            label13.Size = new Size(0x61, 0x18);
            label13.TabIndex = 0xa1;
            label13.Text = "Transient";
            ledTrigMFF.LedStyle = LedStyle.Round3D;
            ledTrigMFF.Location = new Point(0x1da, 3);
            ledTrigMFF.Name = "ledTrigMFF";
            ledTrigMFF.OffColor = Color.Red;
            ledTrigMFF.Size = new Size(30, 30);
            ledTrigMFF.TabIndex = 0x9f;
            ledTrigTap.LedStyle = LedStyle.Round3D;
            ledTrigTap.Location = new Point(340, 3);
            ledTrigTap.Name = "ledTrigTap";
            ledTrigTap.OffColor = Color.Red;
            ledTrigTap.Size = new Size(30, 30);
            ledTrigTap.TabIndex = 160;
            ledTrigLP.LedStyle = LedStyle.Round3D;
            ledTrigLP.Location = new Point(0xd9, 3);
            ledTrigLP.Name = "ledTrigLP";
            ledTrigLP.OffColor = Color.Red;
            ledTrigLP.Size = new Size(30, 30);
            ledTrigLP.TabIndex = 0x9d;
            ledTrigTrans.LedStyle = LedStyle.Round3D;
            ledTrigTrans.Location = new Point(0x29, 3);
            ledTrigTrans.Name = "ledTrigTrans";
            ledTrigTrans.OffColor = Color.Red;
            ledTrigTrans.Size = new Size(30, 30);
            ledTrigTrans.TabIndex = 0x9e;
            button1.BackColor = Color.LimeGreen;
            button1.Location = new Point(0x204, 0x40);
            button1.Name = "button1";
            button1.Size = new Size(0x6c, 0x22);
            button1.TabIndex = 0x33;
            button1.Text = "Dump";
            button1.UseVisualStyleBackColor = false;
            button1.Visible = false;
            lblCurrent_FIFO_Count.AutoSize = true;
            lblCurrent_FIFO_Count.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblCurrent_FIFO_Count.Location = new Point(260, 0x45);
            lblCurrent_FIFO_Count.Name = "lblCurrent_FIFO_Count";
            lblCurrent_FIFO_Count.Size = new Size(0xc1, 0x18);
            lblCurrent_FIFO_Count.TabIndex = 0x30;
            lblCurrent_FIFO_Count.Text = "Current FIFO Count";
            lblF_Count.AutoSize = true;
            lblF_Count.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblF_Count.Location = new Point(0x1d3, 0x45);
            lblF_Count.Name = "lblF_Count";
            lblF_Count.Size = new Size(0x15, 0x18);
            lblF_Count.TabIndex = 0x31;
            lblF_Count.Text = "0";
            ledOverFlow.LedStyle = LedStyle.Round3D;
            ledOverFlow.Location = new Point(0x9b, 0x6d);
            ledOverFlow.Name = "ledOverFlow";
            ledOverFlow.OffColor = Color.Red;
            ledOverFlow.Size = new Size(40, 40);
            ledOverFlow.TabIndex = 0x2c;
            ledWatermark.LedStyle = LedStyle.Round3D;
            ledWatermark.Location = new Point(0x257, 0x6b);
            ledWatermark.Name = "ledWatermark";
            ledWatermark.OffColor = Color.Red;
            ledWatermark.Size = new Size(40, 40);
            ledWatermark.TabIndex = 0x2f;
            label64.AutoSize = true;
            label64.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label64.Location = new Point(14, 0x71);
            label64.Name = "label64";
            label64.Size = new Size(0x8b, 0x18);
            label64.TabIndex = 0x2d;
            label64.Text = "Overflow Flag";
            label63.AutoSize = true;
            label63.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label63.Location = new Point(0x1bb, 0x71);
            label63.Name = "label63";
            label63.Size = new Size(0x9c, 0x18);
            label63.TabIndex = 0x2e;
            label63.Text = "Watermark Flag";
            p4.BackColor = Color.LightSteelBlue;
            p4.Controls.Add(gbWatermark);
            p4.Controls.Add(pTriggerMode);
            p4.ForeColor = Color.Black;
            p4.Location = new Point(3, 0x70);
            p4.Name = "p4";
            p4.Size = new Size(0x299, 0xf4);
            p4.TabIndex = 0xda;
            gbWatermark.Controls.Add(btnResetWatermark);
            gbWatermark.Controls.Add(lblWatermarkValue);
            gbWatermark.Controls.Add(tBWatermark);
            gbWatermark.Controls.Add(btnWatermark);
            gbWatermark.Controls.Add(lblWatermark);
            gbWatermark.Location = new Point(0x1f, 0x37);
            gbWatermark.Name = "gbWatermark";
            gbWatermark.Size = new Size(0x277, 0x86);
            gbWatermark.TabIndex = 0xd3;
            gbWatermark.TabStop = false;
            btnResetWatermark.BackColor = Color.FromArgb(0, 0xc0, 0);
            btnResetWatermark.Enabled = false;
            btnResetWatermark.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnResetWatermark.ForeColor = Color.Black;
            btnResetWatermark.Location = new Point(0x1ff, 12);
            btnResetWatermark.Name = "btnResetWatermark";
            btnResetWatermark.Size = new Size(110, 0x34);
            btnResetWatermark.TabIndex = 0xcd;
            btnResetWatermark.Text = "Reset Watermark\r\n";
            btnResetWatermark.UseVisualStyleBackColor = false;
            btnResetWatermark.Click += new EventHandler(btnResetWatermark_Click);
            lblWatermarkValue.AutoSize = true;
            lblWatermarkValue.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblWatermarkValue.Location = new Point(0x8a, 0x58);
            lblWatermarkValue.Name = "lblWatermarkValue";
            lblWatermarkValue.Size = new Size(0x15, 0x18);
            lblWatermarkValue.TabIndex = 50;
            lblWatermarkValue.Text = "1";
            toolTip1.SetToolTip(lblWatermarkValue, "This is the set value for # of Samples in the FIFO to trigger the Flag");
            tBWatermark.Location = new Point(0xa2, 0x58);
            tBWatermark.Maximum = 0x20;
            tBWatermark.Minimum = 1;
            tBWatermark.Name = "tBWatermark";
            tBWatermark.Size = new Size(0x1cb, 40);
            tBWatermark.TabIndex = 0x20;
            tBWatermark.TickFrequency = 2;
            toolTip1.SetToolTip(tBWatermark, "Watermark Value");
            tBWatermark.Value = 1;
            tBWatermark.Scroll += new EventHandler(tBWatermark_Scroll);
            btnWatermark.BackColor = Color.FromArgb(0, 0xc0, 0);
            btnWatermark.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnWatermark.ForeColor = Color.Black;
            btnWatermark.Location = new Point(20, 12);
            btnWatermark.Name = "btnWatermark";
            btnWatermark.Size = new Size(110, 0x34);
            btnWatermark.TabIndex = 0xcc;
            btnWatermark.Text = "Set Watermark";
            btnWatermark.UseVisualStyleBackColor = false;
            btnWatermark.Click += new EventHandler(btnWatermark_Click);
            lblWatermark.AutoSize = true;
            lblWatermark.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblWatermark.Location = new Point(0x10, 0x58);
            lblWatermark.Name = "lblWatermark";
            lblWatermark.Size = new Size(0x79, 0x18);
            lblWatermark.TabIndex = 0x2a;
            lblWatermark.Text = "Watermark: ";
            toolTip1.SetToolTip(lblWatermark, "This is the # of Sample to trigger the flag in the FIFO");
            pTriggerMode.Controls.Add(chkTriggerMFF);
            pTriggerMode.Controls.Add(chkTriggerPulse);
            pTriggerMode.Controls.Add(chkTriggerLP);
            pTriggerMode.Controls.Add(chkTrigTrans);
            pTriggerMode.Location = new Point(0x1f, 0xc3);
            pTriggerMode.Name = "pTriggerMode";
            pTriggerMode.Size = new Size(0x24a, 0x2e);
            pTriggerMode.TabIndex = 210;
            chkTriggerMFF.AutoSize = true;
            chkTriggerMFF.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            chkTriggerMFF.Location = new Point(0x1bc, 12);
            chkTriggerMFF.Name = "chkTriggerMFF";
            chkTriggerMFF.Size = new Size(0x70, 20);
            chkTriggerMFF.TabIndex = 0xd1;
            chkTriggerMFF.Text = "Trigger MFF";
            chkTriggerMFF.UseVisualStyleBackColor = true;
            chkTriggerMFF.CheckedChanged += new EventHandler(chkTriggerMFF_CheckedChanged);
            chkTriggerPulse.AutoSize = true;
            chkTriggerPulse.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            chkTriggerPulse.Location = new Point(0x132, 12);
            chkTriggerPulse.Name = "chkTriggerPulse";
            chkTriggerPulse.Size = new Size(110, 20);
            chkTriggerPulse.TabIndex = 0xd0;
            chkTriggerPulse.Text = "Trigger Tap";
            chkTriggerPulse.UseVisualStyleBackColor = true;
            chkTriggerPulse.CheckedChanged += new EventHandler(chkTriggerPulse_CheckedChanged);
            chkTriggerLP.AutoSize = true;
            chkTriggerLP.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            chkTriggerLP.Location = new Point(0xb6, 12);
            chkTriggerLP.Name = "chkTriggerLP";
            chkTriggerLP.Size = new Size(100, 20);
            chkTriggerLP.TabIndex = 0xcf;
            chkTriggerLP.Text = "Trigger LP";
            chkTriggerLP.UseVisualStyleBackColor = true;
            chkTriggerLP.CheckedChanged += new EventHandler(chkTriggerLP_CheckedChanged);
            chkTrigTrans.AutoSize = true;
            chkTrigTrans.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            chkTrigTrans.Location = new Point(15, 12);
            chkTrigTrans.Name = "chkTrigTrans";
            chkTrigTrans.Size = new Size(0x93, 20);
            chkTrigTrans.TabIndex = 0xce;
            chkTrigTrans.Text = "Trigger Transient";
            chkTrigTrans.UseVisualStyleBackColor = true;
            chkTrigTrans.CheckedChanged += new EventHandler(chkTrigTrans_CheckedChanged);
            rdoTriggerMode.AutoSize = true;
            rdoTriggerMode.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            rdoTriggerMode.Location = new Point(0x1db, 0x49);
            rdoTriggerMode.Name = "rdoTriggerMode";
            rdoTriggerMode.Size = new Size(0x84, 0x18);
            rdoTriggerMode.TabIndex = 0xcd;
            rdoTriggerMode.Text = "Trigger Mode";
            toolTip1.SetToolTip(rdoTriggerMode, "Allows values to continuously overwrite holding the last 32 values");
            rdoTriggerMode.UseVisualStyleBackColor = true;
            rdoTriggerMode.CheckedChanged += new EventHandler(rdoTriggerMode_CheckedChanged);
            chkDisableFIFO.AutoSize = true;
            chkDisableFIFO.Checked = true;
            chkDisableFIFO.CheckState = CheckState.Checked;
            chkDisableFIFO.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            chkDisableFIFO.ForeColor = Color.Black;
            chkDisableFIFO.Location = new Point(0x11, 0x1f);
            chkDisableFIFO.Name = "chkDisableFIFO";
            chkDisableFIFO.Size = new Size(0x97, 0x1c);
            chkDisableFIFO.TabIndex = 0x27;
            chkDisableFIFO.Text = "Disable FIFO";
            toolTip1.SetToolTip(chkDisableFIFO, "Turns on/off the FIFO function");
            chkDisableFIFO.UseVisualStyleBackColor = true;
            chkDisableFIFO.CheckedChanged += new EventHandler(chkDisableFIFO_CheckedChanged);
            rdoFill.AutoSize = true;
            rdoFill.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            rdoFill.Location = new Point(0xbf, 0x49);
            rdoFill.Name = "rdoFill";
            rdoFill.Size = new Size(0x69, 0x18);
            rdoFill.TabIndex = 0x26;
            rdoFill.Text = "Fill Buffer";
            toolTip1.SetToolTip(rdoFill, "Allows samples to hold up to 32 values for X,Y and Z");
            rdoFill.UseVisualStyleBackColor = true;
            rdoFill.CheckedChanged += new EventHandler(rdoFill_CheckedChanged);
            rdoCircular.AutoSize = true;
            rdoCircular.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            rdoCircular.Location = new Point(0x138, 0x49);
            rdoCircular.Name = "rdoCircular";
            rdoCircular.Size = new Size(0x8f, 0x18);
            rdoCircular.TabIndex = 0x25;
            rdoCircular.Text = "Circular Buffer";
            toolTip1.SetToolTip(rdoCircular, "Allows values to continuously overwrite holding the last 32 values");
            rdoCircular.UseVisualStyleBackColor = true;
            rdoCircular.CheckedChanged += new EventHandler(rdoCircular_CheckedChanged);
            btnSetMode.BackColor = Color.FromArgb(0, 0xc0, 0);
            btnSetMode.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSetMode.ForeColor = Color.Black;
            btnSetMode.Location = new Point(20, 0x41);
            btnSetMode.Name = "btnSetMode";
            btnSetMode.Size = new Size(0x99, 0x29);
            btnSetMode.TabIndex = 0xcb;
            btnSetMode.Text = "Change Mode ";
            toolTip1.SetToolTip(btnSetMode, "Sets FIFO MODE and Watermark");
            btnSetMode.UseVisualStyleBackColor = false;
            btnSetMode.Click += new EventHandler(btnSetMode_Click);
            TmrActive.Interval = 1;
            TmrActive.Tick += new EventHandler(TmrActive_Tick);
            tmrDataDisplay.Interval = 1;
            tmrDataDisplay.Tick += new EventHandler(tmrDataDisplay_Tick);
            panel5.BackColor = Color.LightSlateGray;
            panel5.Controls.Add(pictureBox2);
            panel5.Controls.Add(gbOM);
            panel5.Controls.Add(gbDR);
            panel5.Location = new Point(0, 1);
            panel5.Name = "panel5";
            panel5.Size = new Size(0x46a, 0x8d);
            panel5.TabIndex = 230;
            CommStrip.Items.AddRange(new ToolStripItem[] { CommStripButton, toolStripStatusLabel });
            CommStrip.Location = new Point(0, 0x2bb);
            CommStrip.Name = "CommStrip";
            CommStrip.Size = new Size(0x46c, 0x16);
            CommStrip.TabIndex = 0xe7;
            CommStrip.Text = "statusStrip1";
            toolStripStatusLabel.BackColor = SystemColors.ButtonFace;
            toolStripStatusLabel.Name = "toolStripStatusLabel";
            toolStripStatusLabel.Size = new Size(0xd1, 0x11);
            toolStripStatusLabel.Text = "COM Port Not Connected, Please Connect";
            label7.AutoSize = true;
            label7.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            label7.Location = new Point(0x19, 0x6f);
            label7.Name = "label7";
            label7.Size = new Size(0x6b, 0x10);
            label7.TabIndex = 0x146;
            label7.Text = "System Config";
            CommStripButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
            CommStripButton.Image = Resources.imgYellowState;
            CommStripButton.ImageTransparentColor = Color.Magenta;
            CommStripButton.Name = "CommStripButton";
            CommStripButton.ShowDropDownArrow = false;
            CommStripButton.Size = new Size(20, 20);
            CommStripButton.Text = "toolStripDropDownButton1";
            pictureBox2.Image = Resources.FSL;
            pictureBox2.Location = new Point(0x3ba, 5);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(0xaf, 0x44);
            pictureBox2.TabIndex = 230;
            pictureBox2.TabStop = false;
            PLImage.Image = Resources.STB_PL_back_up;
            PLImage.InitialImage = Resources.STB_PL_back_up;
            PLImage.Location = new Point(0x2de, 0x31);
            PLImage.Name = "PLImage";
            PLImage.Size = new Size(0x163, 0xc9);
            PLImage.TabIndex = 0xc1;
            PLImage.TabStop = false;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            AutoSize = true;
            BackColor = Color.Silver;
            base.ClientSize = new Size(0x46c, 0x2d1);
            base.Controls.Add(CommStrip);
            base.Controls.Add(panel5);
            base.Controls.Add(TabTool);
            base.Controls.Add(groupBox6);
            base.Name = "NeutronEvaluationSoftware";
            base.ShowInTaskbar = false;
            base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            Text = "Lab126 Full System Evaluation Software";
            base.FormClosing += new FormClosingEventHandler(VeyronEvaluationSoftware_FormClosing);
            PL_Page.ResumeLayout(false);
            gbOD.ResumeLayout(false);
            gbOD.PerformLayout();
            pPLActive.ResumeLayout(false);
            pPLActive.PerformLayout();
            tbPL.EndInit();
            p6.ResumeLayout(false);
            p6.PerformLayout();
            gbPLDisappear.ResumeLayout(false);
            gbPLDisappear.PerformLayout();
            p7.ResumeLayout(false);
            p7.PerformLayout();
            ((ISupportInitialize) ledCurPLNew).EndInit();
            ((ISupportInitialize) ledCurPLRight).EndInit();
            ((ISupportInitialize) ledCurPLLeft).EndInit();
            ((ISupportInitialize) ledCurPLDown).EndInit();
            ((ISupportInitialize) ledCurPLUp).EndInit();
            ((ISupportInitialize) ledCurPLBack).EndInit();
            ((ISupportInitialize) ledCurPLFront).EndInit();
            ((ISupportInitialize) ledCurPLLO).EndInit();
            MainScreenEval.ResumeLayout(false);
            MainScreenEval.PerformLayout();
            pSOS.ResumeLayout(false);
            pSOS.PerformLayout();
            p14b8bSelect.ResumeLayout(false);
            p14b8bSelect.PerformLayout();
            panel3.ResumeLayout(false);
            gbOC.ResumeLayout(false);
            gbOC.PerformLayout();
            gbXD.ResumeLayout(false);
            gbXD.PerformLayout();
            pFullResData.ResumeLayout(false);
            pFullResData.PerformLayout();
            ((ISupportInitialize) legend1).EndInit();
            gbST.ResumeLayout(false);
            gbST.PerformLayout();
            ((ISupportInitialize) MainScreenGraph).EndInit();
            gbwfs.ResumeLayout(false);
            gbwfs.PerformLayout();
            gbIF.ResumeLayout(false);
            gbIF.PerformLayout();
            p9.ResumeLayout(false);
            ((ISupportInitialize) ledTrans1).EndInit();
            ((ISupportInitialize) ledOrient).EndInit();
            ((ISupportInitialize) ledASleep).EndInit();
            ((ISupportInitialize) ledMFF1).EndInit();
            ((ISupportInitialize) ledPulse).EndInit();
            ((ISupportInitialize) ledTrans).EndInit();
            ((ISupportInitialize) ledDataReady).EndInit();
            ((ISupportInitialize) ledFIFO).EndInit();
            p8.ResumeLayout(false);
            panel18.ResumeLayout(false);
            panel18.PerformLayout();
            panel12.ResumeLayout(false);
            panel12.PerformLayout();
            panel11.ResumeLayout(false);
            panel11.PerformLayout();
            panel10.ResumeLayout(false);
            panel10.PerformLayout();
            panel9.ResumeLayout(false);
            panel9.PerformLayout();
            panel4.ResumeLayout(false);
            panel4.PerformLayout();
            panel8.ResumeLayout(false);
            panel8.PerformLayout();
            panel6.ResumeLayout(false);
            panel6.PerformLayout();
            gbASS.ResumeLayout(false);
            gbASS.PerformLayout();
            pnlAutoSleep.ResumeLayout(false);
            pnlAutoSleep.PerformLayout();
            tbSleepCounter.EndInit();
            p2.ResumeLayout(false);
            p2.PerformLayout();
            groupBox6.ResumeLayout(false);
            groupBox6.PerformLayout();
            gbOM.ResumeLayout(false);
            gbOM.PerformLayout();
            pOverSampling.ResumeLayout(false);
            pOverSampling.PerformLayout();
            ((ISupportInitialize) ledSleep).EndInit();
            p1.ResumeLayout(false);
            p1.PerformLayout();
            ((ISupportInitialize) ledStandby).EndInit();
            ((ISupportInitialize) ledWake).EndInit();
            gbDR.ResumeLayout(false);
            gbDR.PerformLayout();
            TabTool.ResumeLayout(false);
            Register_Page.ResumeLayout(false);
            gbRegisterName.ResumeLayout(false);
            gbRegisterName.PerformLayout();
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
            groupBox1.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            DataConfigPage.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            gbStatus.ResumeLayout(false);
            p21.ResumeLayout(false);
            p21.PerformLayout();
            ((ISupportInitialize) ledFIFOStatus).EndInit();
            ((ISupportInitialize) ledRealTimeStatus).EndInit();
            ((ISupportInitialize) ledXYZOW).EndInit();
            ((ISupportInitialize) ledZOW).EndInit();
            ((ISupportInitialize) ledYOW).EndInit();
            ((ISupportInitialize) ledXOW).EndInit();
            ((ISupportInitialize) ledZDR).EndInit();
            ((ISupportInitialize) ledYDR).EndInit();
            ((ISupportInitialize) ledXDR).EndInit();
            ((ISupportInitialize) ledXYZDR).EndInit();
            groupBox3.ResumeLayout(false);
            p20.ResumeLayout(false);
            panel16.ResumeLayout(false);
            panel16.PerformLayout();
            panel17.ResumeLayout(false);
            panel17.PerformLayout();
            MFF1_2Page.ResumeLayout(false);
            MFF1_2Page.PerformLayout();
            ((ISupportInitialize) legend3).EndInit();
            ((ISupportInitialize) MFFGraph).EndInit();
            gbMF1.ResumeLayout(false);
            gbMF1.PerformLayout();
            p14.ResumeLayout(false);
            p14.PerformLayout();
            p15.ResumeLayout(false);
            p15.PerformLayout();
            ((ISupportInitialize) ledMFF1EA).EndInit();
            ((ISupportInitialize) ledMFF1XHE).EndInit();
            ((ISupportInitialize) ledMFF1YHE).EndInit();
            ((ISupportInitialize) ledMFF1ZHE).EndInit();
            tbMFF1Threshold.EndInit();
            tbMFF1Debounce.EndInit();
            TransientDetection.ResumeLayout(false);
            TransientDetection.PerformLayout();
            pTrans2.ResumeLayout(false);
            pTrans2.PerformLayout();
            ((ISupportInitialize) ledTransEA).EndInit();
            ((ISupportInitialize) ledTransZDetect).EndInit();
            ((ISupportInitialize) ledTransYDetect).EndInit();
            ((ISupportInitialize) ledTransXDetect).EndInit();
            gbTSNEW.ResumeLayout(false);
            gbTSNEW.PerformLayout();
            pTransNEW.ResumeLayout(false);
            pTransNEW.PerformLayout();
            tbTransDebounceNEW.EndInit();
            p19.ResumeLayout(false);
            p19.PerformLayout();
            ((ISupportInitialize) ledTransEANEW).EndInit();
            ((ISupportInitialize) ledTransZDetectNEW).EndInit();
            ((ISupportInitialize) ledTransYDetectNEW).EndInit();
            ((ISupportInitialize) ledTransXDetectNEW).EndInit();
            tbTransThresholdNEW.EndInit();
            ((ISupportInitialize) legend2).EndInit();
            ((ISupportInitialize) gphXYZ).EndInit();
            gbTS.ResumeLayout(false);
            gbTS.PerformLayout();
            p18.ResumeLayout(false);
            p18.PerformLayout();
            tbTransDebounce.EndInit();
            tbTransThreshold.EndInit();
            PulseDetection.ResumeLayout(false);
            gbSDPS.ResumeLayout(false);
            gbSDPS.PerformLayout();
            panel15.ResumeLayout(false);
            panel15.PerformLayout();
            tbPulseZThreshold.EndInit();
            p12.ResumeLayout(false);
            p12.PerformLayout();
            tbPulseLatency.EndInit();
            tbPulse2ndPulseWin.EndInit();
            p10.ResumeLayout(false);
            p10.PerformLayout();
            tbFirstPulseTimeLimit.EndInit();
            p11.ResumeLayout(false);
            p11.PerformLayout();
            ((ISupportInitialize) ledPulseDouble).EndInit();
            ((ISupportInitialize) ledPulseEA).EndInit();
            ((ISupportInitialize) ledPZ).EndInit();
            ((ISupportInitialize) ledPX).EndInit();
            ((ISupportInitialize) ledPY).EndInit();
            tbPulseXThreshold.EndInit();
            tbPulseYThreshold.EndInit();
            FIFOPage.ResumeLayout(false);
            FIFOPage.PerformLayout();
            gb3bF.ResumeLayout(false);
            gb3bF.PerformLayout();
            p5.ResumeLayout(false);
            p5.PerformLayout();
            ((ISupportInitialize) ledTrigMFF).EndInit();
            ((ISupportInitialize) ledTrigTap).EndInit();
            ((ISupportInitialize) ledTrigLP).EndInit();
            ((ISupportInitialize) ledTrigTrans).EndInit();
            ((ISupportInitialize) ledOverFlow).EndInit();
            ((ISupportInitialize) ledWatermark).EndInit();
            p4.ResumeLayout(false);
            gbWatermark.ResumeLayout(false);
            gbWatermark.PerformLayout();
            tBWatermark.EndInit();
            pTriggerMode.ResumeLayout(false);
            pTriggerMode.PerformLayout();
            panel5.ResumeLayout(false);
            CommStrip.ResumeLayout(false);
            CommStrip.PerformLayout();
            ((ISupportInitialize) pictureBox2).EndInit();
            ((ISupportInitialize) PLImage).EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }
		#endregion

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

        private void OnClose(object sender, EventArgs e)
        {
            VeyronControllerObj.DisableData();
            VeyronControllerObj.CloseConnection();
            base.Close();
        }

        private void rb01_MouseHover(object sender, EventArgs e)
        {
            rb01.Checked = true;
            gbRegisterName.Text = "Register: 0x01 OUT_X_MSB Read Only";
            lb_BitN07.Text = "OUT_X_MSB";
            lb_BitN0.Text = "XD6";
            lb_BitN1.Text = "XD7";
            lb_BitN2.Text = "XD8";
            lb_BitN3.Text = "XD9";
            lb_BitN4.Text = "XD10";
            lb_BitN5.Text = "XD11";
            lb_BitN6.Text = "XD12";
            lb_BitN7.Text = "XD13";
            lblComment1.Text = "";
            lblComment2.Text = "";
            int[] datapassed = new int[] { 1 };
            VeyronControllerObj.ReadValueParsedFlag = true;
            ControllerReqPacket reqName = new ControllerReqPacket();
            CreateNewTaskFromGUI(reqName, 1, 0x5d, datapassed);
        }

        private void rb02_MouseHover(object sender, EventArgs e)
        {
            rb02.Checked = true;
            gbRegisterName.Text = "Register: 0x02 OUT_X_LSB Read Only";
            lb_BitN07.Text = "OUT_X_LSB";
            lb_BitN0.Text = "XD0";
            lb_BitN1.Text = "XD1";
            lb_BitN2.Text = "XD2";
            lb_BitN3.Text = "XD3";
            lb_BitN4.Text = "XD4";
            lb_BitN5.Text = "XD5";
            lb_BitN6.Text = "XD6";
            lb_BitN7.Text = "XD7";
            lblComment1.Text = "";
            lblComment2.Text = "";
            int[] datapassed = new int[] { 2 };
            VeyronControllerObj.ReadValueParsedFlag = true;
            ControllerReqPacket reqName = new ControllerReqPacket();
            CreateNewTaskFromGUI(reqName, 1, 0x5d, datapassed);
        }

        private void rb03_MouseHover(object sender, EventArgs e)
        {
            rb03.Checked = true;
            gbRegisterName.Text = "Register: 0x03 OUT_Y_MSB Read Only";
            lb_BitN07.Text = "OUT_Y_MSB";
            lb_BitN0.Text = "YD6";
            lb_BitN1.Text = "YD7";
            lb_BitN2.Text = "YD8";
            lb_BitN3.Text = "YD9";
            lb_BitN4.Text = "YD10";
            lb_BitN5.Text = "YD11";
            lb_BitN6.Text = "YD12";
            lb_BitN7.Text = "YD13";
            lblComment1.Text = "";
            lblComment2.Text = "";
            int[] datapassed = new int[] { 3 };
            VeyronControllerObj.ReadValueParsedFlag = true;
			CreateNewTaskFromGUI(new ControllerReqPacket(), 1, 0x5d, datapassed);
        }

        private void rb04_MouseHover(object sender, EventArgs e)
        {
            rb04.Checked = true;
            gbRegisterName.Text = "Register: 0x04 OUT_Y_LSB Read Only";
            lb_BitN07.Text = "OUT_Y_LSB";
            lb_BitN0.Text = "YD0";
            lb_BitN1.Text = "YD1";
            lb_BitN2.Text = "YD2";
            lb_BitN3.Text = "YD3";
            lb_BitN4.Text = "YD4";
            lb_BitN5.Text = "YD5";
            lb_BitN6.Text = "YD6";
            lb_BitN7.Text = "YD7";
            lblComment1.Text = "";
            lblComment2.Text = "";
            int[] datapassed = new int[] { 4 };
            VeyronControllerObj.ReadValueParsedFlag = true;
			CreateNewTaskFromGUI(new ControllerReqPacket(), 1, 0x5d, datapassed);
        }

        private void rb05_MouseHover(object sender, EventArgs e)
        {
            rb05.Checked = true;
            gbRegisterName.Text = "Register: 0x05 OUT_Z_MSB Read Only";
            lb_BitN07.Text = "OUT_Z_MSB";
            lb_BitN0.Text = "ZD6";
            lb_BitN1.Text = "ZD7";
            lb_BitN2.Text = "ZD8";
            lb_BitN3.Text = "ZD9";
            lb_BitN4.Text = "ZD10";
            lb_BitN5.Text = "ZD11";
            lb_BitN6.Text = "ZD12";
            lb_BitN7.Text = "ZD13";
            lblComment1.Text = "";
            lblComment2.Text = "";
            int[] datapassed = new int[] { 5 };
            VeyronControllerObj.ReadValueParsedFlag = true;
			CreateNewTaskFromGUI(new ControllerReqPacket(), 1, 0x5d, datapassed);
        }

        private void rb06_MouseHover(object sender, EventArgs e)
        {
            rb06.Checked = true;
            gbRegisterName.Text = "Register: 0x06 OUT_Z_LSB Read Only";
            lb_BitN07.Text = "OUT_Z_LSB";
            lb_BitN0.Text = "ZD0";
            lb_BitN1.Text = "ZD1";
            lb_BitN2.Text = "ZD2";
            lb_BitN3.Text = "ZD3";
            lb_BitN4.Text = "ZD4";
            lb_BitN5.Text = "ZD5";
            lb_BitN6.Text = "ZD6";
            lb_BitN7.Text = "ZD7";
            lblComment1.Text = "";
            lblComment2.Text = "";
            int[] datapassed = new int[] { 6 };
            VeyronControllerObj.ReadValueParsedFlag = true;
			CreateNewTaskFromGUI(new ControllerReqPacket(), 1, 0x5d, datapassed);
        }

        private void rb09_MouseHover(object sender, EventArgs e)
        {
            rb09.Checked = true;
            gbRegisterName.Text = "Register: 0x09 FIFO Setup Read/Write";
            lb_BitN07.Text = "F_SETUP";
            lb_BitN0.Text = "F_WMRK0";
            lb_BitN1.Text = "F_WMRK1";
            lb_BitN2.Text = "F_WMRK2";
            lb_BitN3.Text = "F_WMRK3";
            lb_BitN4.Text = "F_WMRK4";
            lb_BitN5.Text = "F_WMRK5";
            lb_BitN6.Text = "F_MODE0";
            lb_BitN7.Text = "F_MODE1";
            lblComment1.Text = "F_WMRK[0:5]: 0 -31 Samples";
            lblComment2.Text = "F_MODE[0:1]: 00=FIFO OFF, 01=Circular Buffer, 10=Fill Buffer, 11=Trigger";
            int[] datapassed = new int[] { 9 };
            VeyronControllerObj.ReadValueParsedFlag = true;
			CreateNewTaskFromGUI(new ControllerReqPacket(), 1, 0x5d, datapassed);
        }

        private void rb0A_MouseHover(object sender, EventArgs e)
        {
            rb0A.Checked = true;
            gbRegisterName.Text = "Register: 0x0A FIFO Triggers Read/Write";
            lb_BitN07.Text = "TRIG_CFG";
            lb_BitN0.Text = "--";
            lb_BitN1.Text = "--";
            lb_BitN2.Text = "Trig_FF_MT";
            lb_BitN3.Text = "Trig_PULSE";
            lb_BitN4.Text = "Trig_LNDPRT";
            lb_BitN5.Text = "Trig_TRANS";
            lb_BitN6.Text = "--";
            lb_BitN7.Text = "--";
            lblComment1.Text = "";
            lblComment2.Text = "";
            int[] datapassed = new int[] { 10 };
            VeyronControllerObj.ReadValueParsedFlag = true;
			CreateNewTaskFromGUI(new ControllerReqPacket(), 1, 0x5d, datapassed);
        }

        private void rb0B_MouseHover(object sender, EventArgs e)
        {
            rb0B.Checked = true;
            gbRegisterName.Text = "Register: 0x0B System Mode Read Only";
            lb_BitN07.Text = "SYSMOD";
            lb_BitN0.Text = "SYSMOD0";
            lb_BitN1.Text = "SYSMOD1";
            lb_BitN2.Text = "FGT_0";
            lb_BitN3.Text = "FGT_1";
            lb_BitN4.Text = "FGT_2";
            lb_BitN5.Text = "FGT_3";
            lb_BitN6.Text = "FGT_4";
            lb_BitN7.Text = "FGERR";
            lblComment1.Text = "SYSMOD[0:1]: 00=Standby, 01=Wake, 10=Sleep";
            lblComment2.Text = "";
            int[] datapassed = new int[] { 11 };
            VeyronControllerObj.ReadValueParsedFlag = true;
			CreateNewTaskFromGUI(new ControllerReqPacket(), 1, 0x5d, datapassed);
        }

        private void rb0C_MouseHover(object sender, EventArgs e)
        {
            rb0C.Checked = true;
            gbRegisterName.Text = "Register: 0x0C Interrupt Source Read Only";
            lb_BitN07.Text = "INT_SOURCE";
            lb_BitN0.Text = "SRC_DRDY";
            lb_BitN1.Text = "--";
            lb_BitN2.Text = "SRC_FF_MT";
            lb_BitN3.Text = "SRC_PULSE";
            lb_BitN4.Text = "SRC_LNDPRT";
            lb_BitN5.Text = "SRC_TRANS";
            if (DeviceID == 2)
            {
                lb_BitN6.Text = "SRC_FIFO";
            }
            else
            {
                lb_BitN6.Text = "--";
            }
            lb_BitN7.Text = "SRC_ASLP";
            lblComment1.Text = "";
            lblComment2.Text = "";
            int[] datapassed = new int[] { 12 };
            VeyronControllerObj.ReadValueParsedFlag = true;
			CreateNewTaskFromGUI(new ControllerReqPacket(), 1, 0x5d, datapassed);
        }

        private void rb0D_MouseHover(object sender, EventArgs e)
        {
            rb0D.Checked = true;
            gbRegisterName.Text = "Register: 0x0D Who AM I Device ID Read Only";
            lb_BitN07.Text = "WHO_AM_I";
            lb_BitN0.Text = "ID0";
            lb_BitN1.Text = "ID1";
            lb_BitN2.Text = "ID2";
            lb_BitN3.Text = "ID3";
            lb_BitN4.Text = "ID4";
            lb_BitN5.Text = "ID5";
            lb_BitN6.Text = "ID6";
            lb_BitN7.Text = "ID7";
            lblComment1.Text = "";
            lblComment2.Text = "";
            int[] datapassed = new int[] { 13 };
            VeyronControllerObj.ReadValueParsedFlag = true;
			CreateNewTaskFromGUI(new ControllerReqPacket(), 1, 0x5d, datapassed);
        }

        private void rb0E_MouseHover(object sender, EventArgs e)
        {
            rb0E.Checked = true;
            gbRegisterName.Text = "Register: 0x0E XXY Data Configuration Read/Write";
            lb_BitN07.Text = "XYZ_DATA_CFG";
            lb_BitN0.Text = "FS0";
            lb_BitN1.Text = "FS1";
            lb_BitN2.Text = "--";
            lb_BitN3.Text = "--";
            lb_BitN4.Text = "--";
            lb_BitN5.Text = "--";
            lb_BitN6.Text = "--";
            lb_BitN7.Text = "--";
            lblComment1.Text = "";
            lblComment2.Text = "";
            int[] datapassed = new int[] { 14 };
            VeyronControllerObj.ReadValueParsedFlag = true;
			CreateNewTaskFromGUI(new ControllerReqPacket(), 1, 0x5d, datapassed);
        }

        private void rb0F_MouseHover(object sender, EventArgs e)
        {
            rb0F.Checked = true;
            gbRegisterName.Text = "Register: 0x0F High Pass Filter Settings Read/Write";
            lb_BitN07.Text = "HP_FILTER_CUTOFF";
            lb_BitN0.Text = "SEL0";
            lb_BitN1.Text = "SEL1";
            lb_BitN2.Text = "--";
            lb_BitN3.Text = "--";
            lb_BitN4.Text = "LPF_EN";
            lb_BitN5.Text = "HPF_BYP";
            lb_BitN6.Text = "--";
            lb_BitN7.Text = "--";
            lblComment1.Text = "SEL[0:1]: 00,01,10,11 Provide 4 HPF Cut-Off Filter Options";
            lblComment2.Text = "Values depend on ODR(CTRL_REG1) + Oversampling Mode (CTRL_REG2)";
            int[] datapassed = new int[] { 15 };
            VeyronControllerObj.ReadValueParsedFlag = true;
			CreateNewTaskFromGUI(new ControllerReqPacket(), 1, 0x5d, datapassed);
        }

        private void rb10_MouseHover(object sender, EventArgs e)
        {
            rb10.Checked = true;
            gbRegisterName.Text = "Register: 0x10 Portrait/Landscape Status Read Only";
            lb_BitN07.Text = "PL_STATUS";
            lb_BitN0.Text = "BAFR0";
            lb_BitN1.Text = "LAPO0";
            lb_BitN2.Text = "LAPO1";
            lb_BitN3.Text = "--";
            lb_BitN4.Text = "--";
            lb_BitN5.Text = "--";
            lb_BitN6.Text = "LO";
            lb_BitN7.Text = "NEWLP";
            lblComment1.Text = "BAFRO: 0=Front, 1=Back";
            lblComment2.Text = "LAPO[0:1]: 00=Portrait Up, 01=Portrait Down, 10=Landscape Right, 11=Landscape Left";
            int[] datapassed = new int[] { 0x10 };
            VeyronControllerObj.ReadValueParsedFlag = true;
			CreateNewTaskFromGUI(new ControllerReqPacket(), 1, 0x5d, datapassed);
        }

        private void rb11_MouseHover(object sender, EventArgs e)
        {
            rb11.Checked = true;
            gbRegisterName.Text = "Register: 0x11 Portrait/Landscape Configuration Read/Write ";
            lb_BitN07.Text = "PL_CFG";
            lb_BitN0.Text = "--";
            lb_BitN1.Text = "--";
            lb_BitN2.Text = "--";
            lb_BitN3.Text = "--";
            lb_BitN4.Text = "--";
            lb_BitN5.Text = "--";
            lb_BitN6.Text = "PLEN";
            lb_BitN7.Text = "DBCNTM";
            lblComment1.Text = "DBCNTM: 0=Decrement Counter, 1=Clear Counter";
            lblComment2.Text = "";
            int[] datapassed = new int[] { 0x11 };
            VeyronControllerObj.ReadValueParsedFlag = true;
			CreateNewTaskFromGUI(new ControllerReqPacket(), 1, 0x5d, datapassed);
        }

        private void rb12_MouseHover(object sender, EventArgs e)
        {
            rb12.Checked = true;
            gbRegisterName.Text = "Register: 0x12 Portrait/Landscape Debounce Counter Read/Write ";
            lb_BitN07.Text = "PL_COUNT";
            lb_BitN0.Text = "DBNCE0";
            lb_BitN1.Text = "DBNCE1";
            lb_BitN2.Text = "DBNCE2";
            lb_BitN3.Text = "DBNCE3";
            lb_BitN4.Text = "DBNCE4";
            lb_BitN5.Text = "DBNCE5";
            lb_BitN6.Text = "DBNCE6";
            lb_BitN7.Text = "DBNCE7";
            lblComment1.Text = "Incremental Steps depend on ODR(CTRL_REG1) + Oversampling Mode (CTRL Reg2)";
            lblComment2.Text = "";
            int[] datapassed = new int[] { 0x12 };
            VeyronControllerObj.ReadValueParsedFlag = true;
			CreateNewTaskFromGUI(new ControllerReqPacket(), 1, 0x5d, datapassed);
        }

        private void rb13_MouseHover(object sender, EventArgs e)
        {
            rb13.Checked = true;
            gbRegisterName.Text = "Register: 0x13 Portrait/Landscape Back/Front, Z-Compensation Read/Write ";
            lb_BitN07.Text = "PL_BF_ZCOMP";
            lb_BitN0.Text = "ZLOCK0";
            lb_BitN1.Text = "ZLOCK1";
            lb_BitN2.Text = "ZLOCK2";
            lb_BitN3.Text = "--";
            lb_BitN4.Text = "--";
            lb_BitN5.Text = "--";
            lb_BitN6.Text = "BKFR0";
            lb_BitN7.Text = "BKFR1";
            lblComment1.Text = "ZLOCK: 000=18deg, 111=43deg  Increments of 5 degrees";
            lblComment2.Text = "BKFR: 00=+/-10deg, 11= +/-25deg   Increments of 5 degrees";
            int[] datapassed = new int[] { 0x13 };
            VeyronControllerObj.ReadValueParsedFlag = true;
			CreateNewTaskFromGUI(new ControllerReqPacket(), 1, 0x5d, datapassed);
        }

        private void rb14_MouseHover(object sender, EventArgs e)
        {
            rb14.Checked = true;
            gbRegisterName.Text = "Register: 0x14 Portrait/Landscape Threshold Read/Write ";
            lb_BitN07.Text = "PL_P_L_THS_REG";
            lb_BitN0.Text = "HYS0";
            lb_BitN1.Text = "HYS1";
            lb_BitN2.Text = "HYS2";
            lb_BitN3.Text = "P_L_THS0";
            lb_BitN4.Text = "P_L_THS1";
            lb_BitN5.Text = "P_L_THS2";
            lb_BitN6.Text = "P_L_THS3";
            lb_BitN7.Text = "P_L_THS4";
            lblComment1.Text = "HYS[0:2]: 000= +/-0deg, 111= +/-24deg  Increments of 4 degrees";
            lblComment2.Text = "P_L_THS[0:4]:15 deg to 75 deg: Review look-up table for values";
            int[] datapassed = new int[] { 20 };
            VeyronControllerObj.ReadValueParsedFlag = true;
			CreateNewTaskFromGUI(new ControllerReqPacket(), 1, 0x5d, datapassed);
        }

        private void rb15_MouseHover(object sender, EventArgs e)
        {
            rb15.Checked = true;
            gbRegisterName.Text = "Register: 0x15 Motion/Freefall Configuration Read/Write ";
            lb_BitN07.Text = "FF_MT_CFG";
            lb_BitN0.Text = "--";
            lb_BitN1.Text = "--";
            lb_BitN2.Text = "--";
            lb_BitN3.Text = "XEFE";
            lb_BitN4.Text = "YEFE";
            lb_BitN5.Text = "ZEFE";
            lb_BitN6.Text = "OAE";
            lb_BitN7.Text = "ELE";
            lblComment1.Text = "0AE=0 Freefall, 0AE=1 Motion";
            lblComment2.Text = "";
            int[] datapassed = new int[] { 0x15 };
            VeyronControllerObj.ReadValueParsedFlag = true;
			CreateNewTaskFromGUI(new ControllerReqPacket(), 1, 0x5d, datapassed);
        }

        private void rb16_MouseHover(object sender, EventArgs e)
        {
            rb16.Checked = true;
            gbRegisterName.Text = "Register: 0x16 Motion/Freefall Source Read Only ";
            lb_BitN07.Text = "FF_MT_SRC";
            lb_BitN0.Text = "XHP";
            lb_BitN1.Text = "XHE";
            lb_BitN2.Text = "YHP";
            lb_BitN3.Text = "YHE";
            lb_BitN4.Text = "ZHP";
            lb_BitN5.Text = "ZHE";
            lb_BitN6.Text = "--";
            lb_BitN7.Text = "EA";
            lblComment1.Text = "XHP,YHP,ZHP: 0 = Positive  1 = Negative";
            lblComment2.Text = "";
            int[] datapassed = new int[] { 0x16 };
            VeyronControllerObj.ReadValueParsedFlag = true;
			CreateNewTaskFromGUI(new ControllerReqPacket(), 1, 0x5d, datapassed);
        }

        private void rb17_MouseHover(object sender, EventArgs e)
        {
            rb17.Checked = true;
            gbRegisterName.Text = "Register: 0x17 Motion/Freefall Threshold Read/Write";
            lb_BitN07.Text = "FF_MT_THS";
            lb_BitN0.Text = "THS0";
            lb_BitN1.Text = "THS1";
            lb_BitN2.Text = "THS2";
            lb_BitN3.Text = "THS3";
            lb_BitN4.Text = "THS4";
            lb_BitN5.Text = "THS5";
            lb_BitN6.Text = "THS6";
            lb_BitN7.Text = "DBCNTM";
            lblComment1.Text = "THS[0:6]: 0g to 8g, increments of 63mg per count";
            lblComment2.Text = "DBCNTM: 0 = decrement counter, 1 = clear counter";
            int[] datapassed = new int[] { 0x17 };
            VeyronControllerObj.ReadValueParsedFlag = true;
			CreateNewTaskFromGUI(new ControllerReqPacket(), 1, 0x5d, datapassed);
        }

        private void rb18_MouseHover(object sender, EventArgs e)
        {
            rb18.Checked = true;
            gbRegisterName.Text = "Register: 0x18 Motion/Freefall Debounce Counter Read/Write";
            lb_BitN07.Text = "FF_MT_COUNT";
            lb_BitN0.Text = "DBNCE0";
            lb_BitN1.Text = "DBNCE1";
            lb_BitN2.Text = "DBNCE2";
            lb_BitN3.Text = "DBNCE3";
            lb_BitN4.Text = "DBNCE4";
            lb_BitN5.Text = "DBNCE5";
            lb_BitN6.Text = "DBNCE6";
            lb_BitN7.Text = "DBNCE7";
            lblComment1.Text = "DBNCE[0:7]: Step Count depends on ODR (CTRL_REG1) and Oversampling Mode (CTRL_REG2)";
            lblComment2.Text = "";
            int[] datapassed = new int[] { 0x18 };
            VeyronControllerObj.ReadValueParsedFlag = true;
			CreateNewTaskFromGUI(new ControllerReqPacket(), 1, 0x5d, datapassed);
        }

        private void rb19_MouseHover(object sender, EventArgs e)
        {
            rb19.Checked = true;
            gbRegisterName.Text = "Register: 0x19 Transient Configuration Read/Write";
            lb_BitN07.Text = "TRANSIENT_CFG1";
            lb_BitN0.Text = "HPF_BYP";
            lb_BitN1.Text = "XEFE";
            lb_BitN2.Text = "YEFE";
            lb_BitN3.Text = "ZEFE";
            lb_BitN4.Text = "ELE";
            lb_BitN5.Text = "--";
            lb_BitN6.Text = "--";
            lb_BitN7.Text = "--";
            lblComment1.Text = "";
            lblComment2.Text = "";
            int[] datapassed = new int[] { 0x19 };
            VeyronControllerObj.ReadValueParsedFlag = true;
			CreateNewTaskFromGUI(new ControllerReqPacket(), 1, 0x5d, datapassed);
        }

        private void rb1A_MouseHover(object sender, EventArgs e)
        {
            rb1A.Checked = true;
            gbRegisterName.Text = "Register: 0x1A Transient Source Read Only";
            lb_BitN07.Text = "TRANSIENT_SRC1";
            lb_BitN0.Text = "X_TRANS_POL";
            lb_BitN1.Text = "XTRANSE";
            lb_BitN2.Text = "Y_TRANS_POL";
            lb_BitN3.Text = "YTRANSE";
            lb_BitN4.Text = "Z_TRANS_POL";
            lb_BitN5.Text = "ZTRANSE";
            lb_BitN6.Text = "EA";
            lb_BitN7.Text = "--";
            lblComment1.Text = "X_TRANS_POL, Y_TRANS_POL, Z_TRANS_POL: 0 = Positive, 1 = Negative";
            lblComment2.Text = "";
            int[] datapassed = new int[] { 0x1a };
            VeyronControllerObj.ReadValueParsedFlag = true;
			CreateNewTaskFromGUI(new ControllerReqPacket(), 1, 0x5d, datapassed);
        }

        private void rb1B_MouseHover(object sender, EventArgs e)
        {
            rb1B.Checked = true;
            gbRegisterName.Text = "Register: 0x1B Transient Threshold Read/Write";
            lb_BitN07.Text = "TRANSIENT_THS1";
            lb_BitN0.Text = "THS0";
            lb_BitN1.Text = "THS1";
            lb_BitN2.Text = "THS2";
            lb_BitN3.Text = "THS3";
            lb_BitN4.Text = "THS4";
            lb_BitN5.Text = "THS5";
            lb_BitN6.Text = "THS6";
            lb_BitN7.Text = "DBCNTM";
            lblComment1.Text = "THS[0:6]: 0g to 8g, increments of 63mg per count";
            lblComment2.Text = "DBCNTM: 0 = decrement counter, 1 = clear counter";
            int[] datapassed = new int[] { 0x1b };
            VeyronControllerObj.ReadValueParsedFlag = true;
			CreateNewTaskFromGUI(new ControllerReqPacket(), 1, 0x5d, datapassed);
        }

        private void rb1C_MouseHover(object sender, EventArgs e)
        {
            rb1C.Checked = true;
            gbRegisterName.Text = "Register: 0x1C Transient Debounce Counter Read/Write";
            lb_BitN07.Text = "TRANSIENT_COUNT1";
            lb_BitN0.Text = "DBNCE0";
            lb_BitN1.Text = "DBNCE1";
            lb_BitN2.Text = "DBNCE2";
            lb_BitN3.Text = "DBNCE3";
            lb_BitN4.Text = "DBNCE4";
            lb_BitN5.Text = "DBNCE5";
            lb_BitN6.Text = "DBNCE6";
            lb_BitN7.Text = "DBNCE7";
            lblComment1.Text = "DBNCE[0:7]: Step Count depends on ODR (CTRL_REG1) and Oversampling Mode (CTRL_REG2)";
            lblComment2.Text = "";
            int[] datapassed = new int[] { 0x1c };
            VeyronControllerObj.ReadValueParsedFlag = true;
			CreateNewTaskFromGUI(new ControllerReqPacket(), 1, 0x5d, datapassed);
        }

        private void rb1D_MouseHover(object sender, EventArgs e)
        {
            rb1D.Checked = true;
            gbRegisterName.Text = "Register: 0x1D Transient Configuration Read/Write";
            lb_BitN07.Text = "TRANSIENT_CFG";
            lb_BitN0.Text = "HPF_BYP";
            lb_BitN1.Text = "XEFE";
            lb_BitN2.Text = "YEFE";
            lb_BitN3.Text = "ZEFE";
            lb_BitN4.Text = "ELE";
            lb_BitN5.Text = "--";
            lb_BitN6.Text = "--";
            lb_BitN7.Text = "--";
            lblComment1.Text = "";
            lblComment2.Text = "";
            int[] datapassed = new int[] { 0x1d };
            VeyronControllerObj.ReadValueParsedFlag = true;
			CreateNewTaskFromGUI(new ControllerReqPacket(), 1, 0x5d, datapassed);
        }

        private void rb1E_MouseHover(object sender, EventArgs e)
        {
            rb1E.Checked = true;
            gbRegisterName.Text = "Register: 0x1E Transient Source Read Only";
            lb_BitN07.Text = "TRANSIENT_SRC";
            lb_BitN0.Text = "X_TRANS_POL";
            lb_BitN1.Text = "XTRANSE";
            lb_BitN2.Text = "Y_TRANS_POL";
            lb_BitN3.Text = "YTRANSE";
            lb_BitN4.Text = "Z_TRANS_POL";
            lb_BitN5.Text = "ZTRANSE";
            lb_BitN6.Text = "EA";
            lb_BitN7.Text = "--";
            lblComment1.Text = "X_TRANS_POL, Y_TRANS_POL, Z_TRANS_POL: 0 = Positive, 1 = Negative";
            lblComment2.Text = "";
            int[] datapassed = new int[] { 30 };
            VeyronControllerObj.ReadValueParsedFlag = true;
            CreateNewTaskFromGUI(new ControllerReqPacket(), 1, 0x5d, datapassed);
        }

        private void rb1F_MouseHover(object sender, EventArgs e)
        {
            rb1F.Checked = true;
            gbRegisterName.Text = "Register: 0x1F Transient Threshold Read/Write";
            lb_BitN07.Text = "TRANSIENT_THS";
            lb_BitN0.Text = "THS0";
            lb_BitN1.Text = "THS1";
            lb_BitN2.Text = "THS2";
            lb_BitN3.Text = "THS3";
            lb_BitN4.Text = "THS4";
            lb_BitN5.Text = "THS5";
            lb_BitN6.Text = "THS6";
            lb_BitN7.Text = "DBCNTM";
            lblComment1.Text = "THS[0:6]: 0g to 8g, increments of 63mg per count";
            lblComment2.Text = "DBCNTM: 0 = decrement counter, 1 = clear counter";
            int[] datapassed = new int[] { 0x1f };
            VeyronControllerObj.ReadValueParsedFlag = true;
			CreateNewTaskFromGUI(new ControllerReqPacket(), 1, 0x5d, datapassed);
        }

        private void rb20_MouseHover(object sender, EventArgs e)
        {
            rb20.Checked = true;
            gbRegisterName.Text = "Register: 0x20 Transient Debounce Counter Read/Write";
            lb_BitN07.Text = "TRANSIENT_COUNT";
            lb_BitN0.Text = "DBNCE0";
            lb_BitN1.Text = "DBNCE1";
            lb_BitN2.Text = "DBNCE2";
            lb_BitN3.Text = "DBNCE3";
            lb_BitN4.Text = "DBNCE4";
            lb_BitN5.Text = "DBNCE5";
            lb_BitN6.Text = "DBNCE6";
            lb_BitN7.Text = "DBNCE7";
            lblComment1.Text = "DBNCE[0:7]: Step Count depends on ODR (CTRL_REG1) and Oversampling Mode (CTRL_REG2)";
            lblComment2.Text = "";
            int[] datapassed = new int[] { 0x20 };
            VeyronControllerObj.ReadValueParsedFlag = true;
            CreateNewTaskFromGUI(new ControllerReqPacket(), 1, 0x5d, datapassed);
        }

        private void rb21_MouseHover(object sender, EventArgs e)
        {
            rb21.Checked = true;
            gbRegisterName.Text = "Register: 0x21 Pulse Configuration Read/Write";
            lb_BitN07.Text = "PULSE_CFG";
            lb_BitN0.Text = "XSPEFE";
            lb_BitN1.Text = "XDPEFE";
            lb_BitN2.Text = "YSPEFE";
            lb_BitN3.Text = "YDPEFE";
            lb_BitN4.Text = "ZSPEFE";
            lb_BitN5.Text = "ZDPEFE";
            lb_BitN6.Text = "ELE";
            lb_BitN7.Text = "DPA";
            lblComment1.Text = "DPA: 1 = Double Pulse Abort, 0 =  No Double Pulse Abort";
            lblComment2.Text = "";
            int[] datapassed = new int[] { 0x21 };
            VeyronControllerObj.ReadValueParsedFlag = true;
            CreateNewTaskFromGUI(new ControllerReqPacket(), 1, 0x5d, datapassed);
        }

        private void rb22_MouseHover(object sender, EventArgs e)
        {
            rb22.Checked = true;
            gbRegisterName.Text = "Register: 0x22 Pulse Source Read Only";
            lb_BitN07.Text = "PULSE_SRC";
            lb_BitN0.Text = "POL_X";
            lb_BitN1.Text = "POL_Y";
            lb_BitN2.Text = "POL_Z";
            lb_BitN3.Text = "DPE";
            lb_BitN4.Text = "AxX";
            lb_BitN5.Text = "AxY";
            lb_BitN6.Text = "AxZ";
            lb_BitN7.Text = "EA";
            lblComment1.Text = "DPE = 1 Double Pulse on First Event";
            lblComment2.Text = "POL_X,POL_Y,POL_Z: 0 = Positive, 1 = Negative";
            int[] datapassed = new int[] { 0x22 };
            VeyronControllerObj.ReadValueParsedFlag = true;
            CreateNewTaskFromGUI(new ControllerReqPacket(), 1, 0x5d, datapassed);
        }

        private void rb23_MouseHover(object sender, EventArgs e)
        {
            rb23.Checked = true;
            gbRegisterName.Text = "Register: 0x23 Pulse Threshold X Axis Read/Write";
            lb_BitN07.Text = "PULSE_THSX";
            lb_BitN0.Text = "THSX0";
            lb_BitN1.Text = "THSX1";
            lb_BitN2.Text = "THSX2";
            lb_BitN3.Text = "THSX3";
            lb_BitN4.Text = "THSX4";
            lb_BitN5.Text = "THSX5";
            lb_BitN6.Text = "THSX6";
            lb_BitN7.Text = "--";
            lblComment1.Text = "THSX[0:6]: 0g to 8g, increments of 63mg per count";
            lblComment2.Text = "";
            int[] datapassed = new int[] { 0x23 };
            VeyronControllerObj.ReadValueParsedFlag = true;
            CreateNewTaskFromGUI(new ControllerReqPacket(), 1, 0x5d, datapassed);
        }

        private void rb24_MouseHover(object sender, EventArgs e)
        {
            rb24.Checked = true;
            gbRegisterName.Text = "Register: 0x24 Pulse Threshold Y Axis Read/Write";
            lb_BitN07.Text = "PULSE_THSY";
            lb_BitN0.Text = "THSY0";
            lb_BitN1.Text = "THSY1";
            lb_BitN2.Text = "THSY2";
            lb_BitN3.Text = "THSY3";
            lb_BitN4.Text = "THSY4";
            lb_BitN5.Text = "THSY5";
            lb_BitN6.Text = "THSY6";
            lb_BitN7.Text = "--";
            lblComment1.Text = "THSY[0:6]: 0g to 8g, increments of 63mg per count";
            lblComment2.Text = "";
            int[] datapassed = new int[] { 0x24 };
            VeyronControllerObj.ReadValueParsedFlag = true;
            CreateNewTaskFromGUI(new ControllerReqPacket(), 1, 0x5d, datapassed);
        }

        private void rb25_MouseHover(object sender, EventArgs e)
        {
            rb25.Checked = true;
            gbRegisterName.Text = "Register: 0x25 Pulse Threshold Z Axis Read/Write";
            lb_BitN07.Text = "PULSE_THSZ";
            lb_BitN0.Text = "THSZ0";
            lb_BitN1.Text = "THSZ1";
            lb_BitN2.Text = "THSZ2";
            lb_BitN3.Text = "THSZ3";
            lb_BitN4.Text = "THSZ4";
            lb_BitN5.Text = "THSZ5";
            lb_BitN6.Text = "THSZ6";
            lb_BitN7.Text = "--";
            lblComment1.Text = "THSZ[0:6]: 0g to 8g, increments of 63mg per count";
            lblComment2.Text = "";
            int[] datapassed = new int[] { 0x25 };
            VeyronControllerObj.ReadValueParsedFlag = true;
            CreateNewTaskFromGUI(new ControllerReqPacket(), 1, 0x5d, datapassed);
        }

        private void rb26_MouseHover(object sender, EventArgs e)
        {
            rb26.Checked = true;
            gbRegisterName.Text = "Register: 0x26 Pulse Time Limit Read/Write";
            lb_BitN07.Text = "PULSE_TMLT";
            lb_BitN0.Text = "TMLT0";
            lb_BitN1.Text = "TMLT1";
            lb_BitN2.Text = "TMLT2";
            lb_BitN3.Text = "TMLT3";
            lb_BitN4.Text = "TMLT4";
            lb_BitN5.Text = "TMLT5";
            lb_BitN6.Text = "TMLT6";
            lb_BitN7.Text = "TMLT7";
            lblComment1.Text = "TMLT[0:7]: Time Step depends on ODR (CTRL_REG1) and Oversampling Mode(CTRL_REG2)";
            lblComment2.Text = "Also depends on LPF_EN (HP_FILTER_CUTOFF).  Duration of the Pulse to cross up and down the threshold.";
            int[] datapassed = new int[] { 0x26 };
            VeyronControllerObj.ReadValueParsedFlag = true;
            CreateNewTaskFromGUI(new ControllerReqPacket(), 1, 0x5d, datapassed);
        }

        private void rb27_MouseHover(object sender, EventArgs e)
        {
            rb27.Checked = true;
            gbRegisterName.Text = "Register: 0x27 Pulse Time Latency Read/Write";
            lb_BitN07.Text = "PULSE_LTCY";
            lb_BitN0.Text = "LTCY0";
            lb_BitN1.Text = "LTCY1";
            lb_BitN2.Text = "LTCY2";
            lb_BitN3.Text = "LTCY3";
            lb_BitN4.Text = "LTCY4";
            lb_BitN5.Text = "LTCY5";
            lb_BitN6.Text = "LTCY6";
            lb_BitN7.Text = "LTCY7";
            lblComment1.Text = "LTCY[0:7]: Time Step depends on ODR (CTRL_REG1) and Oversampling Mode(CTRL_REG2)";
            lblComment2.Text = "Also depends on LPF_EN (HP_FILTER_CUTOFF).  Wait time after the pulse occurs.";
            int[] datapassed = new int[] { 0x27 };
            VeyronControllerObj.ReadValueParsedFlag = true;
            CreateNewTaskFromGUI(new ControllerReqPacket(), 1, 0x5d, datapassed);
        }

        private void rb28_MouseHover(object sender, EventArgs e)
        {
            rb28.Checked = true;
            gbRegisterName.Text = "Register: 0x28 Pulse Second Time Window Read/Write";
            lb_BitN07.Text = "PULSE_WIND";
            lb_BitN0.Text = "WIND0";
            lb_BitN1.Text = "WIND1";
            lb_BitN2.Text = "WIND2";
            lb_BitN3.Text = "WIND3";
            lb_BitN4.Text = "WIND4";
            lb_BitN5.Text = "WIND5";
            lb_BitN6.Text = "WIND6";
            lb_BitN7.Text = "WIND7";
            lblComment1.Text = "WIND[0:7]: Time Step depends on ODR (CTRL_REG1) and Oversampling Mode(CTRL_REG2)";
            lblComment2.Text = "Also depends on LPF_EN (HP_FILTER_CUTOFF).  Time period for the Second Pulse to Occur.";
            int[] datapassed = new int[] { 40 };
            VeyronControllerObj.ReadValueParsedFlag = true;
            ControllerReqPacket reqName = new ControllerReqPacket();
            CreateNewTaskFromGUI(reqName, 1, 0x5d, datapassed);
        }

        private void rb29_MouseHover(object sender, EventArgs e)
        {
            rb29.Checked = true;
            gbRegisterName.Text = "Register: 0x29 AutoSleep Time-out Counter Read/Write";
            lb_BitN07.Text = "ASLP_COUNT";
            lb_BitN0.Text = "D0";
            lb_BitN1.Text = "D1";
            lb_BitN2.Text = "D2";
            lb_BitN3.Text = "D3";
            lb_BitN4.Text = "D4";
            lb_BitN5.Text = "D5";
            lb_BitN6.Text = "D6";
            lb_BitN7.Text = "D7";
            lblComment1.Text = "D[0:7]: Time Step is 320ms for all ODRs, except 1.56Hz which is 640ms per count";
            lblComment2.Text = "Total Sleep Time-out is 0-81s or for 1.56Hz the Time-out period is 0-162s";
            int[] datapassed = new int[] { 0x29 };
            VeyronControllerObj.ReadValueParsedFlag = true;
            ControllerReqPacket reqName = new ControllerReqPacket();
            CreateNewTaskFromGUI(reqName, 1, 0x5d, datapassed);
        }

        private void rb2A_MouseHover(object sender, EventArgs e)
        {
            rb2A.Checked = true;
            gbRegisterName.Text = "Register: 0x2A Control 1 Read/Write";
            lb_BitN07.Text = "CTRL_REG1";
            lb_BitN0.Text = "ACTIVE";
            lb_BitN1.Text = "F_READ";
            lb_BitN2.Text = "LNOISE";
            lb_BitN3.Text = "DR0";
            lb_BitN4.Text = "DR1";
            lb_BitN5.Text = "DR2";
            lb_BitN6.Text = "ASLP_RATE0";
            lb_BitN7.Text = "ASLP_RATE1";
            lblComment1.Text = "F_READ = 1 Fast Read Mode 8-bit only, = 0 Full Resolution";
            lblComment2.Text = "LNOISE = 1 Changes the total g-range to 4g limit, with lower noise";
            int[] datapassed = new int[] { 0x2a };
            VeyronControllerObj.ReadValueParsedFlag = true;
            ControllerReqPacket reqName = new ControllerReqPacket();
            CreateNewTaskFromGUI(reqName, 1, 0x5d, datapassed);
        }

        private void rb2B_MouseHover(object sender, EventArgs e)
        {
            rb2B.Checked = true;
            gbRegisterName.Text = "Register: 0x2B Control 2 Read/Write";
            lb_BitN07.Text = "CTRL_REG2";
            lb_BitN0.Text = "MODS0";
            lb_BitN1.Text = "MODS1";
            lb_BitN2.Text = "SLPE";
            lb_BitN3.Text = "SMODS0";
            lb_BitN4.Text = "SMODS1";
            lb_BitN5.Text = "--";
            lb_BitN6.Text = "RST";
            lb_BitN7.Text = "ST";
            lblComment1.Text = "(S)MODS: 00 = Normal Mode, 01 = Low Noise Low Power, 10 = Hi Resolution, 11 = Low Power";
            lblComment2.Text = "ST= Self Test, RST= Reboot";
            int[] datapassed = new int[] { 0x2b };
            VeyronControllerObj.ReadValueParsedFlag = true;
            ControllerReqPacket reqName = new ControllerReqPacket();
            CreateNewTaskFromGUI(reqName, 1, 0x5d, datapassed);
        }

        private void rb2C_MouseHover(object sender, EventArgs e)
        {
            rb2C.Checked = true;
            gbRegisterName.Text = "Register: 0x2C Control 3 Read/Write";
            lb_BitN07.Text = "CTRL_REG3";
            lb_BitN0.Text = "PP_OD";
            lb_BitN1.Text = "IPOL";
            lb_BitN2.Text = "--";
            lb_BitN3.Text = "WAKE_FF_MT";
            lb_BitN4.Text = "WAKE_PULSE";
            lb_BitN5.Text = "WAKE_LNDPRT";
            lb_BitN6.Text = "WAKE_TRANS";
            if (DeviceID == 2)
            {
                lb_BitN7.Text = "FIFO_GATE";
            }
            else
            {
                lb_BitN7.Text = "--";
            }
            lblComment1.Text = "PPOD: 0 = Push-Pull, 1 = Open Drain";
            lblComment2.Text = "IPOL: 0 = Active Low, 1 = Active High";
            int[] datapassed = new int[] { 0x2c };
            VeyronControllerObj.ReadValueParsedFlag = true;
            ControllerReqPacket reqName = new ControllerReqPacket();
            CreateNewTaskFromGUI(reqName, 1, 0x5d, datapassed);
        }

        private void rb2D_MouseHover(object sender, EventArgs e)
        {
            rb2D.Checked = true;
            gbRegisterName.Text = "Register: 0x2D Control 4 Read/Write";
            lb_BitN07.Text = "CTRL_REG4";
            lb_BitN0.Text = "INT_EN_DRDY";
            lb_BitN1.Text = "--";
            lb_BitN2.Text = "INT_EN_FF_MT";
            lb_BitN3.Text = "INT_EN_PULSE";
            lb_BitN4.Text = "INT_EN_LNDPRT";
            lb_BitN5.Text = "INT_EN_TRANS";
            if (DeviceID == 2)
            {
                lb_BitN6.Text = "INT_EN_FIFO";
            }
            else
            {
                lb_BitN6.Text = "--";
            }
            lb_BitN7.Text = "INT_EN_ASLP";
            lblComment1.Text = "Enables the Interrupt for each function";
            lblComment2.Text = "";
            int[] datapassed = new int[] { 0x2d };
            VeyronControllerObj.ReadValueParsedFlag = true;
            ControllerReqPacket reqName = new ControllerReqPacket();
            CreateNewTaskFromGUI(reqName, 1, 0x5d, datapassed);
        }

        private void rb2E_MouseHover(object sender, EventArgs e)
        {
            rb2E.Checked = true;
            gbRegisterName.Text = "Register: 0x2E Control 5 Read/Write";
            lb_BitN07.Text = "CTRL_REG5";
            lb_BitN0.Text = "INT_CFG_DRDY";
            lb_BitN1.Text = "--";
            lb_BitN2.Text = "INT_CFG_FF_MT";
            lb_BitN3.Text = "INT_CFG_PULSE";
            lb_BitN4.Text = "INT_CFG_LNDPRT";
            lb_BitN5.Text = "INT_CFG_TRANS";
            if (DeviceID == 2)
            {
                lb_BitN6.Text = "INT_CFG_FIFO";
            }
            else
            {
                lb_BitN6.Text = "--";
            }
            lb_BitN7.Text = "INT_CFG_ASLP";
            lblComment1.Text = "0: Interrupt routed to INT2, 1: Interrupt routed to INT1";
            lblComment2.Text = "";
            int[] datapassed = new int[] { 0x2e };
            VeyronControllerObj.ReadValueParsedFlag = true;
            ControllerReqPacket reqName = new ControllerReqPacket();
            CreateNewTaskFromGUI(reqName, 1, 0x5d, datapassed);
        }

        private void rb2F_MouseHover(object sender, EventArgs e)
        {
            rb2F.Checked = true;
            gbRegisterName.Text = "Register: 0x2F X-Offset Read/Write";
            lb_BitN07.Text = "OFF_X";
            lb_BitN0.Text = "D0";
            lb_BitN1.Text = "D1";
            lb_BitN2.Text = "D2";
            lb_BitN3.Text = "D3";
            lb_BitN4.Text = "D4";
            lb_BitN5.Text = "D5";
            lb_BitN6.Text = "D6";
            lb_BitN7.Text = "D7";
            lblComment1.Text = "1mg per LSB.  Maximum is +/-128mg.";
            lblComment2.Text = "";
            int[] datapassed = new int[] { 0x2f };
            VeyronControllerObj.ReadValueParsedFlag = true;
            ControllerReqPacket reqName = new ControllerReqPacket();
            CreateNewTaskFromGUI(reqName, 1, 0x5d, datapassed);
        }

        private void rb2g_CheckedChanged(object sender, EventArgs e)
        {
            if (rdo2g.Checked)
            {
                int[] datapassed = new int[1];
                FullScaleValue = 0;
                datapassed[0] = FullScaleValue;
                VeyronControllerObj.SetFullScaleValueFlag = true;
                ControllerReqPacket reqName = new ControllerReqPacket();
                CreateNewTaskFromGUI(reqName, 0, 4, datapassed);
            }
        }

        private void rb30_MouseHover(object sender, EventArgs e)
        {
            rb30.Checked = true;
            gbRegisterName.Text = "Register: 0x30 Y-Offset Read/Write";
            lb_BitN07.Text = "OFF_Y";
            lb_BitN0.Text = "D0";
            lb_BitN1.Text = "D1";
            lb_BitN2.Text = "D2";
            lb_BitN3.Text = "D3";
            lb_BitN4.Text = "D4";
            lb_BitN5.Text = "D5";
            lb_BitN6.Text = "D6";
            lb_BitN7.Text = "D7";
            lblComment1.Text = "1mg per LSB.  Maximum is +/-128mg.";
            lblComment2.Text = "";
            int[] datapassed = new int[] { 0x30 };
            VeyronControllerObj.ReadValueParsedFlag = true;
            ControllerReqPacket reqName = new ControllerReqPacket();
            CreateNewTaskFromGUI(reqName, 1, 0x5d, datapassed);
        }

        private void rb31_MouseHover(object sender, EventArgs e)
        {
            rb31.Checked = true;
            gbRegisterName.Text = "Register: 0x31 Z-Offset Read/Write";
            lb_BitN07.Text = "OFF_Z";
            lb_BitN0.Text = "D0";
            lb_BitN1.Text = "D1";
            lb_BitN2.Text = "D2";
            lb_BitN3.Text = "D3";
            lb_BitN4.Text = "D4";
            lb_BitN5.Text = "D5";
            lb_BitN6.Text = "D6";
            lb_BitN7.Text = "D7";
            lblComment1.Text = "1mg per LSB.  Maximum is +/-128mg.";
            lblComment2.Text = "";
            int[] datapassed = new int[] { 0x31 };
            VeyronControllerObj.ReadValueParsedFlag = true;
            ControllerReqPacket reqName = new ControllerReqPacket();
            CreateNewTaskFromGUI(reqName, 1, 0x5d, datapassed);
        }

        private void rb4g_CheckedChanged(object sender, EventArgs e)
        {
            if (rdo4g.Checked)
            {
                int[] datapassed = new int[1];
                FullScaleValue = 1;
                datapassed[0] = FullScaleValue;
                VeyronControllerObj.SetFullScaleValueFlag = true;
                ControllerReqPacket reqName = new ControllerReqPacket();
                CreateNewTaskFromGUI(reqName, 0, 4, datapassed);
            }
        }

        private void rb8g_CheckedChanged(object sender, EventArgs e)
        {
            if (rdo8g.Checked)
            {
                int[] datapassed = new int[1];
                FullScaleValue = 2;
                datapassed[0] = FullScaleValue;
                VeyronControllerObj.SetFullScaleValueFlag = true;
                ControllerReqPacket reqName = new ControllerReqPacket();
                CreateNewTaskFromGUI(reqName, 0, 4, datapassed);
            }
        }

        private void rBActive_CheckedChanged(object sender, EventArgs e)
        {
            if (!rdoActive.Checked)
            {
                btnDisableData.Enabled = false;
                btnViewData.Enabled = false;
                chkDefaultMotion1.Enabled = true;
                chkDefaultFFSettings1.Enabled = true;
                chkDefaultTransSettings.Enabled = true;
                chkDefaultTransSettings1.Enabled = true;
                rdoDefaultSP.Enabled = true;
                rdoDefaultSPDP.Enabled = true;
            }
            if (rdoActive.Checked)
            {
                int[] numArray2;
                ControllerReqPacket packet2;
                ControllerReqPacket packet3;
                ControllerReqPacket packet4;
                ControllerReqPacket packet5;
                gbWatermark.Enabled = false;
                btnSetMode.Text = "Set MODE";
                chkDefaultMotion1.Enabled = false;
                chkDefaultFFSettings1.Enabled = false;
                chkDefaultTransSettings.Enabled = false;
                chkDefaultTransSettings1.Enabled = false;
                rdoDefaultSP.Enabled = false;
                rdoDefaultSPDP.Enabled = false;
                chkPLDefaultSettings.Enabled = false;
                chkEnablePL.Enabled = false;
                pTriggerMode.Enabled = false;
                btnViewData.Enabled = true;
                p14b8bSelect.Enabled = false;
                ledWake.Value = true;
                p1.Enabled = false;
                gbASS.Enabled = false;
                gbOC.Enabled = false;
                gbXD.Enabled = true;
                p2.Enabled = false;
                p4.Enabled = false;
                gbwfs.Enabled = false;
                p6.Enabled = false;
                p7.Enabled = true;
                p8.Enabled = false;
                p10.Enabled = false;
                p11.Enabled = true;
                p12.Enabled = false;
                p14.Enabled = false;
                p15.Enabled = true;
                p18.Enabled = false;
                pTransNEW.Enabled = false;
                p19.Enabled = true;
                pTrans2.Enabled = true;
                p20.Enabled = false;
                p21.Enabled = true;
                gbST.Enabled = false;
                btnAutoCal.Enabled = false;
                gbDR.Enabled = false;
                rdoFIFO8bitDataDisplay.Enabled = false;
                rdoFIFO14bitDataDisplay.Enabled = false;
                pOverSampling.Enabled = false;
                pSOS.Enabled = false;
                p14b8bSelect.Enabled = false;
                chkHPFDataOut.Enabled = false;
                chkAnalogLowNoise.Enabled = false;
                pTriggerMode.Enabled = false;
                chkPulseLPFEnable.Enabled = false;
                chkPulseHPFBypass.Enabled = false;
                int[] datapassed = new int[] { 1 };
                VeyronControllerObj.ActiveFlag = true;
                ControllerReqPacket reqName = new ControllerReqPacket();
                CreateNewTaskFromGUI(reqName, 0, 0x53, datapassed);
                if (chkEnableASleep.Checked)
                {
                    VeyronControllerObj.ReturnSysmodStatus = true;
                }
                if (!chkDisableFIFO.Checked && rdoActive.Checked)
                {
                    p4.Enabled = false;
                    p5.Enabled = true;
                    pTriggerMode.Enabled = false;
                    if (rdoTriggerMode.Checked)
                    {
                        if (chkTrigTrans.Checked)
                        {
                            VeyronControllerObj.ReturnTransStatus = true;
                        }
                        if (chkTriggerLP.Checked)
                        {
                            VeyronControllerObj.ReturnPLStatus = true;
                        }
                        if (chkTriggerMFF.Checked)
                        {
                            VeyronControllerObj.ReturnMFF1Status = true;
                        }
                        if (chkTriggerPulse.Checked)
                        {
                            VeyronControllerObj.ReturnPulseStatus = true;
                        }
                    }
                }
                if (chkEnablePL.Checked)
                {
                    p7.Enabled = true;
                }
                switch (TabTool.SelectedIndex)
                {
                    case 0:
                        if ((((!chkEnIntFIFO.Checked && !chkEnIntDR.Checked) && (!chkEnIntTrans.Checked && !chkEnIntPulse.Checked)) && ((!chkEnIntMFF1.Checked && !chkEnIntTrans1.Checked) && !chkEnIntPL.Checked)) && !chkEnIntASleep.Checked)
                        {
                            p9.Enabled = false;
                            break;
                        }
                        VeyronControllerObj.PollInterruptMapStatus = true;
                        p9.Enabled = true;
                        break;

                    case 2:
                        VeyronControllerObj.ReturnDataStatus = true;
                        break;

                    case 3:
                        if ((!chkXEFE.Checked && !chkYEFE.Checked) && !chkZEFE.Checked)
                        {
                            break;
                        }
                        VeyronControllerObj.ReturnMFF1Status = true;
                        numArray2 = new int[] { SystemPolling };
                        if (!rdo8bitDataMain.Checked)
                        {
                            switch (DeviceID)
                            {
                                case 2:
                                    VeyronControllerObj.Data14bFlag = true;
                                    packet3 = new ControllerReqPacket();
                                    CreateNewTaskFromGUI(packet3, 3, 0x5c, numArray2);
                                    break;

                                case 3:
                                    VeyronControllerObj.Data12bFlag = true;
                                    packet4 = new ControllerReqPacket();
                                    CreateNewTaskFromGUI(packet4, 3, 2, numArray2);
                                    break;

                                case 4:
                                    VeyronControllerObj.Data10bFlag = true;
                                    packet5 = new ControllerReqPacket();
                                    CreateNewTaskFromGUI(packet5, 3, 0x5b, numArray2);
                                    break;
                            }
                        }
                        else
                        {
                            VeyronControllerObj.Data8bFlag = true;
                            packet2 = new ControllerReqPacket();
                            CreateNewTaskFromGUI(packet2, 3, 1, numArray2);
                        }
                        MFFGraph.Enabled = true;
                        return;

                    case 4:
                        if (chkEnablePL.Checked)
                        {
                            VeyronControllerObj.ReturnPLStatus = true;
                        }
                        break;

                    case 5:
                        if ((chkTransEnableXFlag.Checked || chkTransEnableYFlag.Checked) || chkTransEnableZFlag.Checked)
                        {
                            VeyronControllerObj.ReturnTransStatus = true;
                        }
                        if ((chkTransEnableXFlagNEW.Checked || chkTransEnableYFlagNEW.Checked) || chkTransEnableZFlagNEW.Checked)
                        {
                            VeyronControllerObj.ReturnTrans1Status = true;
                        }
                        if ((((chkTransEnableXFlagNEW.Checked || chkTransEnableXFlag.Checked) || (chkTransEnableYFlag.Checked || chkTransEnableYFlagNEW.Checked)) || chkTransEnableZFlag.Checked) || chkTransEnableZFlagNEW.Checked)
                        {
                            numArray2 = new int[] { SystemPolling };
                            if (rdo8bitDataMain.Checked)
                            {
                                VeyronControllerObj.Data8bFlag = true;
                                packet2 = new ControllerReqPacket();
                                CreateNewTaskFromGUI(packet2, 5, 1, numArray2);
                            }
                            else
                            {
                                switch (DeviceID)
                                {
                                    case 2:
                                        VeyronControllerObj.Data14bFlag = true;
                                        packet3 = new ControllerReqPacket();
                                        CreateNewTaskFromGUI(packet3, 5, 0x5c, numArray2);
                                        break;

                                    case 3:
                                        VeyronControllerObj.Data12bFlag = true;
                                        packet4 = new ControllerReqPacket();
                                        CreateNewTaskFromGUI(packet4, 5, 2, numArray2);
                                        break;

                                    case 4:
                                        VeyronControllerObj.Data10bFlag = true;
                                        packet5 = new ControllerReqPacket();
                                        CreateNewTaskFromGUI(packet5, 5, 0x5b, numArray2);
                                        break;
                                }
                            }
                            gphXYZ.Enabled = true;
                        }
                        if ((chkTransEnableXFlagNEW.Checked || chkTransEnableYFlagNEW.Checked) || chkTransEnableZFlagNEW.Checked)
                        {
                        }
                        return;

                    case 6:
                        if ((((chkPulseEnableXSP.Checked || chkPulseEnableYSP.Checked) || (chkPulseEnableZSP.Checked || chkPulseEnableXDP.Checked)) || chkPulseEnableYDP.Checked) || chkPulseEnableZDP.Checked)
                        {
                            VeyronControllerObj.ReturnPulseStatus = true;
                        }
                        break;

                    case 7:
                    {
                        if (chkDisableFIFO.Checked)
                        {
                            break;
                        }
                        int[] numArray3 = new int[5];
                        numArray3[0] = SystemPolling;
                        numArray3[1] = tBWatermark.Value;
                        numArray3[2] = FIFODump8orFull;
                        numArray3[4] = FullScaleValue;
                        if (!rdoFill.Checked)
                        {
                            if (rdoCircular.Checked)
                            {
                                numArray3[3] = 1;
                            }
                            else if (rdoTriggerMode.Checked)
                            {
                                numArray3[3] = 2;
                            }
                        }
                        else
                        {
                            numArray3[3] = 0;
                        }
                        VeyronControllerObj.PollingFIFOFlag = true;
                        ControllerReqPacket packet6 = new ControllerReqPacket();
                        CreateNewTaskFromGUI(packet6, 7, 80, numArray3);
                        return;
                    }
                }
            }
        }

        private void rbR00_MouseHover(object sender, EventArgs e)
        {
            rbR00.Checked = true;
            if (chkDisableFIFO.Checked)
            {
                gbRegisterName.Text = "Register: 0x00 Status Register Read Only";
                lb_BitN07.Text = "Status";
                lb_BitN0.Text = "XDR";
                lb_BitN1.Text = "YDR";
                lb_BitN2.Text = "ZDR";
                lb_BitN3.Text = "ZYXDR";
                lb_BitN4.Text = "XOW";
                lb_BitN5.Text = "YOW";
                lb_BitN6.Text = "ZOW";
                lb_BitN7.Text = "ZYXOW";
                lblComment1.Text = "F_MODE=0";
                lblComment2.Text = "";
            }
            else
            {
                gbRegisterName.Text = "Register: 0x00 FIFO Status Register Read Only";
                lb_BitN07.Text = "F_STATUS";
                lb_BitN0.Text = "F_CNT0";
                lb_BitN1.Text = "F_CNT1";
                lb_BitN2.Text = "F_CNT2";
                lb_BitN3.Text = "F_CNT3";
                lb_BitN4.Text = "F_CNT4";
                lb_BitN5.Text = "F_CNT5";
                lb_BitN6.Text = "F_WMRK_FLAG";
                lb_BitN7.Text = "F_OVF";
                lblComment1.Text = "F_CNT[0:5]= 0- 31 Samples";
                lblComment2.Text = "F_MODE>0";
            }
            int[] datapassed = new int[] { 0 };
            VeyronControllerObj.ReadValueParsedFlag = true;
            ControllerReqPacket reqName = new ControllerReqPacket();
            CreateNewTaskFromGUI(reqName, 1, 0x5d, datapassed);
        }

        private void rBStandby_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoStandby.Checked)
            {
                gbWatermark.Enabled = true;
                btnWatermark.Enabled = true;
                btnSetMode.Text = "Set MODE";
                chkAnalogLowNoise.Enabled = true;
                chkEnablePL.Enabled = true;
                if (chkEnablePL.Checked)
                {
                    chkPLDefaultSettings.Enabled = true;
                }
                gbWatermark.Enabled = true;
                rdoDefaultSP.Enabled = true;
                rdoDefaultSPDP.Enabled = true;
                chkDefaultTransSettings.Enabled = true;
                chkDefaultTransSettings1.Enabled = true;
                chkDefaultMotion1.Enabled = true;
                VeyronControllerObj.ReturnSysmodStatus = false;
                VeyronControllerObj.PollInterruptMapStatus = false;
                VeyronControllerObj.ReturnFIFOStatus = false;
                VeyronControllerObj.ReturnMFF1Status = false;
                VeyronControllerObj.ReturnPLStatus = false;
                VeyronControllerObj.ReturnPulseStatus = false;
                VeyronControllerObj.ReturnDataStatus = false;
                VeyronControllerObj.ReturnTransStatus = false;
                VeyronControllerObj.ReturnTrans1Status = false;
                VeyronControllerObj.DisableData();
                gphXYZ.ClearData();
                gphXYZ.Enabled = false;
                MFFGraph.ClearData();
                MFFGraph.Enabled = false;
                btnViewData.Enabled = false;
                btnDisableData.Enabled = false;
                if (!chkDisableFIFO.Checked)
                {
                    p4.Enabled = true;
                    p14b8bSelect.Enabled = false;
                    rdoFIFO14bitDataDisplay.Enabled = false;
                    rdoFIFO8bitDataDisplay.Enabled = false;
                }
                else
                {
                    p4.Enabled = false;
                    p14b8bSelect.Enabled = true;
                    rdoFIFO14bitDataDisplay.Enabled = true;
                    rdoFIFO8bitDataDisplay.Enabled = true;
                }
                int[] datapassed = new int[] { 0 };
                VeyronControllerObj.ActiveFlag = true;
                ControllerReqPacket reqName = new ControllerReqPacket();
                CreateNewTaskFromGUI(reqName, 0, 0x53, datapassed);
                ledStandby.Value = true;
                ledWake.Value = false;
                ledSleep.Value = false;
                p1.Enabled = true;
                gbASS.Enabled = true;
                gbOC.Enabled = true;
                gbXD.Enabled = false;
                gbwfs.Enabled = true;
                p2.Enabled = true;
                p5.Enabled = false;
                p6.Enabled = true;
                p7.Enabled = false;
                p9.Enabled = false;
                p8.Enabled = true;
                p10.Enabled = true;
                p11.Enabled = false;
                p12.Enabled = true;
                p14.Enabled = true;
                p15.Enabled = false;
                p18.Enabled = true;
                pTransNEW.Enabled = true;
                p19.Enabled = false;
                pTrans2.Enabled = false;
                p20.Enabled = true;
                p21.Enabled = false;
                gbST.Enabled = true;
                btnAutoCal.Enabled = true;
                gbDR.Enabled = true;
                pOverSampling.Enabled = true;
                pSOS.Enabled = true;
                if (chkDisableFIFO.Checked)
                {
                    p14b8bSelect.Enabled = true;
                    rdoFIFO14bitDataDisplay.Enabled = true;
                    rdoFIFO8bitDataDisplay.Enabled = true;
                }
                chkHPFDataOut.Enabled = true;
                chkAnalogLowNoise.Enabled = true;
                chkPulseLPFEnable.Enabled = true;
                chkPulseHPFBypass.Enabled = true;
            }
            else
            {
                ledStandby.Value = false;
            }
        }

        private void rdo8bitDataMain_CheckedChanged(object sender, EventArgs e)
        {
            int num;
            int[] numArray;
            ControllerReqPacket packet;
            if (rdoXYZFullResMain.Checked)
            {
                rdoFIFO14bitDataDisplay.Checked = true;
                rdoFIFO8bitDataDisplay.Checked = false;
                num = 0;
                numArray = new int[] { num };
                VeyronControllerObj.SetFREADFlag = true;
                packet = new ControllerReqPacket();
                CreateNewTaskFromGUI(packet, 0, 0x58, numArray);
                rdoFIFO14bitDataDisplay.Checked = true;
                rdoFIFO8bitDataDisplay.Checked = false;
            }
            else
            {
                rdoFIFO8bitDataDisplay.Checked = true;
                rdoFIFO14bitDataDisplay.Checked = false;
                num = 0xff;
                numArray = new int[] { num };
                VeyronControllerObj.SetFREADFlag = true;
                packet = new ControllerReqPacket();
                CreateNewTaskFromGUI(packet, 0, 0x58, numArray);
            }
        }

        private void rdoASleepINT_I1_CheckedChanged(object sender, EventArgs e)
        {
            int[] datapassed = new int[2];
            int num = rdoASleepINT_I1.Checked ? 0xff : 0;
            datapassed[0] = num;
            datapassed[1] = 0x80;
            VeyronControllerObj.SetIntsConfigFlag = true;
            ControllerReqPacket reqName = new ControllerReqPacket();
            CreateNewTaskFromGUI(reqName, 0, 14, datapassed);
        }

        private void rdoASleepINT_I2_CheckedChanged(object sender, EventArgs e)
        {
            int[] datapassed = new int[2];
            int num = rdoASleepINT_I2.Checked ? 0 : 0xff;
            datapassed[0] = num;
            datapassed[1] = 0x80;
            VeyronControllerObj.SetIntsConfigFlag = true;
            ControllerReqPacket reqName = new ControllerReqPacket();
            CreateNewTaskFromGUI(reqName, 0, 14, datapassed);
        }

        private void rdoCircular_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoCircular.Checked)
            {
                FIFOModeValue = 1;
            }
        }

        private void rdoClrDebouncePL_CheckedChanged(object sender, EventArgs e)
        {
            int num = rdoClrDebouncePL.Checked ? 0xff : 0;
            int[] datapassed = new int[] { num };
            VeyronControllerObj.SetDBCNTM_PLFlag = true;
            ControllerReqPacket reqName = new ControllerReqPacket();
            CreateNewTaskFromGUI(reqName, 4, 30, datapassed);
        }

        private void rdoDecDebouncePL_CheckedChanged(object sender, EventArgs e)
        {
            int num = rdoDecDebouncePL.Checked ? 0 : 0xff;
            int[] datapassed = new int[] { num };
            VeyronControllerObj.SetDBCNTM_PLFlag = true;
            ControllerReqPacket reqName = new ControllerReqPacket();
            CreateNewTaskFromGUI(reqName, 4, 30, datapassed);
        }

        private void rdoDefaultSP_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoDefaultSP.Checked)
            {
                rdoOSNormalMode.Checked = true;
                int[] datapassed = new int[1];
                WakeOSMode = 0;
                datapassed[0] = WakeOSMode;
                VeyronControllerObj.SetWakeOSModeFlag = true;
                ControllerReqPacket reqName = new ControllerReqPacket();
                CreateNewTaskFromGUI(reqName, 0, 0x54, datapassed);
                chkPulseEnableXSP.Checked = true;
                int[] numArray2 = new int[] { 0xff };
                VeyronControllerObj.SetPulseXSPFlag = true;
                ControllerReqPacket packet2 = new ControllerReqPacket();
                CreateNewTaskFromGUI(packet2, 0, 0x42, numArray2);
                chkPulseEnableXDP.Checked = false;
                int[] numArray3 = new int[] { 0 };
                VeyronControllerObj.SetPulseXDPFlag = true;
                ControllerReqPacket packet3 = new ControllerReqPacket();
                CreateNewTaskFromGUI(packet3, 0, 0x41, numArray3);
                chkPulseEnableYSP.Checked = true;
                int[] numArray4 = new int[] { 0xff };
                VeyronControllerObj.SetPulseYSPFlag = true;
                ControllerReqPacket packet4 = new ControllerReqPacket();
                CreateNewTaskFromGUI(packet4, 0, 0x40, numArray4);
                chkPulseEnableYDP.Checked = false;
                int[] numArray5 = new int[] { 0 };
                VeyronControllerObj.SetPulseYDPFlag = true;
                ControllerReqPacket packet5 = new ControllerReqPacket();
                CreateNewTaskFromGUI(packet5, 0, 0x3f, numArray5);
                chkPulseEnableZSP.Checked = true;
                int[] numArray6 = new int[] { 0xff };
                VeyronControllerObj.SetPulseZSPFlag = true;
                ControllerReqPacket packet6 = new ControllerReqPacket();
                CreateNewTaskFromGUI(packet6, 0, 0x3e, numArray6);
                chkPulseEnableZDP.Checked = false;
                int[] numArray7 = new int[] { 0 };
                VeyronControllerObj.SetPulseZDPFlag = true;
                ControllerReqPacket packet7 = new ControllerReqPacket();
                CreateNewTaskFromGUI(packet7, 0, 0x3d, numArray7);
                chkPulseLPFEnable.Checked = true;
                int[] numArray8 = new int[] { 0xff };
                VeyronControllerObj.SetPulseLPFEnableFlag = true;
                ControllerReqPacket packet8 = new ControllerReqPacket();
                CreateNewTaskFromGUI(packet8, 6, 0x59, numArray8);
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
                VeyronControllerObj.SetPulseFirstTimeLimitFlag = true;
                ControllerReqPacket packet9 = new ControllerReqPacket();
                CreateNewTaskFromGUI(packet9, 0, 0x43, numArray9);
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
                VeyronControllerObj.SetPulseLatencyFlag = true;
                ControllerReqPacket packet10 = new ControllerReqPacket();
                CreateNewTaskFromGUI(packet10, 0, 0x47, numArray10);
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
                tbPulseZThreshold.Value = 0x2a;
                double num6 = tbPulseZThreshold.Value * num4;
                lblPulseZThresholdVal.Text = string.Format("{0:F2}", num6);
                int[] numArray11 = new int[] { tbPulseXThreshold.Value };
                VeyronControllerObj.SetPulseXThresholdFlag = true;
                ControllerReqPacket packet11 = new ControllerReqPacket();
                CreateNewTaskFromGUI(packet11, 0, 0x44, numArray11);
                int[] numArray12 = new int[] { tbPulseYThreshold.Value };
                VeyronControllerObj.SetPulseYThresholdFlag = true;
                ControllerReqPacket packet12 = new ControllerReqPacket();
                CreateNewTaskFromGUI(packet12, 0, 0x45, numArray12);
                int[] numArray13 = new int[] { tbPulseZThreshold.Value };
                VeyronControllerObj.SetPulseZThresholdFlag = true;
                ControllerReqPacket packet13 = new ControllerReqPacket();
                CreateNewTaskFromGUI(packet13, 0, 70, numArray13);
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
                VeyronControllerObj.SetPulse2ndPulseWinFlag = true;
                ControllerReqPacket packet14 = new ControllerReqPacket();
                CreateNewTaskFromGUI(packet14, 0, 0x48, numArray14);
            }
        }

        private void rdoDefaultSPDP_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoDefaultSPDP.Checked)
            {
                rdoOSNormalMode.Checked = true;
                int[] datapassed = new int[1];
                WakeOSMode = 0;
                datapassed[0] = WakeOSMode;
                VeyronControllerObj.SetWakeOSModeFlag = true;
                ControllerReqPacket reqName = new ControllerReqPacket();
                CreateNewTaskFromGUI(reqName, 0, 0x54, datapassed);
                chkPulseEnableXSP.Checked = true;
                int[] numArray2 = new int[] { 0xff };
                VeyronControllerObj.SetPulseXSPFlag = true;
                ControllerReqPacket packet2 = new ControllerReqPacket();
                CreateNewTaskFromGUI(packet2, 0, 0x42, numArray2);
                chkPulseEnableXDP.Checked = true;
                int[] numArray3 = new int[] { 0xff };
                VeyronControllerObj.SetPulseXDPFlag = true;
                ControllerReqPacket packet3 = new ControllerReqPacket();
                CreateNewTaskFromGUI(packet3, 0, 0x41, numArray3);
                chkPulseEnableYSP.Checked = true;
                int[] numArray4 = new int[] { 0xff };
                VeyronControllerObj.SetPulseYSPFlag = true;
                ControllerReqPacket packet4 = new ControllerReqPacket();
                CreateNewTaskFromGUI(packet4, 0, 0x40, numArray4);
                chkPulseEnableYDP.Checked = true;
                int[] numArray5 = new int[] { 0xff };
                VeyronControllerObj.SetPulseYDPFlag = true;
                ControllerReqPacket packet5 = new ControllerReqPacket();
                CreateNewTaskFromGUI(packet5, 0, 0x3f, numArray5);
                chkPulseEnableZSP.Checked = true;
                int[] numArray6 = new int[] { 0xff };
                VeyronControllerObj.SetPulseZSPFlag = true;
                ControllerReqPacket packet6 = new ControllerReqPacket();
                CreateNewTaskFromGUI(packet6, 0, 0x3e, numArray6);
                chkPulseEnableZDP.Checked = true;
                int[] numArray7 = new int[] { 0xff };
                VeyronControllerObj.SetPulseZDPFlag = true;
                ControllerReqPacket packet7 = new ControllerReqPacket();
                CreateNewTaskFromGUI(packet7, 0, 0x3d, numArray7);
                chkPulseLPFEnable.Checked = true;
                int[] numArray8 = new int[] { 0xff };
                VeyronControllerObj.SetPulseLPFEnableFlag = true;
                ControllerReqPacket packet8 = new ControllerReqPacket();
                CreateNewTaskFromGUI(packet8, 6, 0x59, numArray8);
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
                VeyronControllerObj.SetPulseFirstTimeLimitFlag = true;
                ControllerReqPacket packet9 = new ControllerReqPacket();
                CreateNewTaskFromGUI(packet9, 0, 0x43, numArray9);
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
                VeyronControllerObj.SetPulseLatencyFlag = true;
                ControllerReqPacket packet10 = new ControllerReqPacket();
                CreateNewTaskFromGUI(packet10, 0, 0x47, numArray10);
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
                VeyronControllerObj.SetPulse2ndPulseWinFlag = true;
                ControllerReqPacket packet11 = new ControllerReqPacket();
                CreateNewTaskFromGUI(packet11, 0, 0x48, numArray11);
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
                tbPulseZThreshold.Value = 0x2a;
                double num7 = tbPulseZThreshold.Value * num5;
                lblPulseZThresholdVal.Text = string.Format("{0:F2}", num7);
                int[] numArray12 = new int[] { tbPulseXThreshold.Value };
                VeyronControllerObj.SetPulseXThresholdFlag = true;
                ControllerReqPacket packet12 = new ControllerReqPacket();
                CreateNewTaskFromGUI(packet12, 0, 0x44, numArray12);
                int[] numArray13 = new int[] { tbPulseYThreshold.Value };
                VeyronControllerObj.SetPulseYThresholdFlag = true;
                ControllerReqPacket packet13 = new ControllerReqPacket();
                CreateNewTaskFromGUI(packet13, 0, 0x45, numArray13);
                int[] numArray14 = new int[] { tbPulseZThreshold.Value };
                VeyronControllerObj.SetPulseZThresholdFlag = true;
                ControllerReqPacket packet14 = new ControllerReqPacket();
                CreateNewTaskFromGUI(packet14, 0, 70, numArray14);
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
        }

        private void rdoDRINT_I1_CheckedChanged(object sender, EventArgs e)
        {
            int[] datapassed = new int[2];
            int num = rdoDRINT_I1.Checked ? 0xff : 0;
            datapassed[0] = num;
            datapassed[1] = 1;
            VeyronControllerObj.SetIntsConfigFlag = true;
            ControllerReqPacket reqName = new ControllerReqPacket();
            CreateNewTaskFromGUI(reqName, 0, 14, datapassed);
        }

        private void rdoDRINT_I2_CheckedChanged(object sender, EventArgs e)
        {
            int[] datapassed = new int[2];
            int num = rdoDRINT_I2.Checked ? 0 : 0xff;
            datapassed[0] = num;
            datapassed[1] = 1;
            VeyronControllerObj.SetIntsConfigFlag = true;
            ControllerReqPacket reqName = new ControllerReqPacket();
            CreateNewTaskFromGUI(reqName, 0, 14, datapassed);
        }

        private void rdoFIFO14bitDataDisplay_CheckedChanged(object sender, EventArgs e)
        {
            int num;
            int[] numArray;
            ControllerReqPacket packet;
            if (rdoFIFO14bitDataDisplay.Checked)
            {
                num = 0;
                numArray = new int[] { num };
                VeyronControllerObj.SetFREADFlag = true;
                packet = new ControllerReqPacket();
                CreateNewTaskFromGUI(packet, 7, 0x58, numArray);
                FIFODump8orFull = 0;
                rdoXYZFullResMain.Checked = true;
                rdo8bitDataMain.Checked = false;
            }
            else
            {
                num = 0xff;
                numArray = new int[] { num };
                VeyronControllerObj.SetFREADFlag = true;
                packet = new ControllerReqPacket();
                CreateNewTaskFromGUI(packet, 7, 0x58, numArray);
                FIFODump8orFull = 1;
                rdoXYZFullResMain.Checked = false;
                rdo8bitDataMain.Checked = true;
            }
        }

        private void rdoFIFO8bitDataDisplay_CheckedChanged(object sender, EventArgs e)
        {
            int num;
            int[] numArray;
            ControllerReqPacket packet;
            if (rdoFIFO8bitDataDisplay.Checked)
            {
                num = 0xff;
                numArray = new int[] { num };
                VeyronControllerObj.SetFREADFlag = true;
                packet = new ControllerReqPacket();
                CreateNewTaskFromGUI(packet, 7, 0x58, numArray);
                FIFODump8orFull = 1;
                rdoXYZFullResMain.Checked = false;
                rdo8bitDataMain.Checked = true;
            }
            else
            {
                num = 0;
                numArray = new int[] { num };
                VeyronControllerObj.SetFREADFlag = true;
                packet = new ControllerReqPacket();
                CreateNewTaskFromGUI(packet, 7, 0x58, numArray);
                FIFODump8orFull = 0;
                rdoXYZFullResMain.Checked = true;
                rdo8bitDataMain.Checked = false;
            }
        }

        private void rdoFIFOINT_I1_CheckedChanged(object sender, EventArgs e)
        {
            int[] datapassed = new int[2];
            int num = rdoFIFOINT_I1.Checked ? 0xff : 0;
            datapassed[0] = num;
            datapassed[1] = 0x40;
            VeyronControllerObj.SetIntsConfigFlag = true;
            ControllerReqPacket reqName = new ControllerReqPacket();
            CreateNewTaskFromGUI(reqName, 0, 14, datapassed);
        }

        private void rdoFIFOINT_I2_CheckedChanged(object sender, EventArgs e)
        {
            int[] datapassed = new int[2];
            int num = rdoFIFOINT_I2.Checked ? 0 : 0xff;
            datapassed[0] = num;
            datapassed[1] = 0x40;
            VeyronControllerObj.SetIntsConfigFlag = true;
            ControllerReqPacket reqName = new ControllerReqPacket();
            CreateNewTaskFromGUI(reqName, 0, 14, datapassed);
        }

        private void rdoFill_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoFill.Checked)
            {
                FIFOModeValue = 2;
            }
        }

        private void rdoINTActiveHigh_CheckedChanged(object sender, EventArgs e)
        {
            int num = rdoINTActiveHigh.Checked ? 0xff : 0;
            int[] datapassed = new int[] { num };
            VeyronControllerObj.SetINTActiveFlag = true;
            ControllerReqPacket reqName = new ControllerReqPacket();
            CreateNewTaskFromGUI(reqName, 2, 0x21, datapassed);
        }

        private void rdoINTActiveLow_CheckedChanged(object sender, EventArgs e)
        {
            int num = rdoINTActiveLow.Checked ? 0 : 0xff;
            int[] datapassed = new int[] { num };
            VeyronControllerObj.SetINTActiveFlag = true;
            ControllerReqPacket reqName = new ControllerReqPacket();
            CreateNewTaskFromGUI(reqName, 2, 0x21, datapassed);
        }

        private void rdoINTOpenDrain_CheckedChanged(object sender, EventArgs e)
        {
            int num = rdoINTOpenDrain.Checked ? 0xff : 0;
            int[] datapassed = new int[] { num };
            VeyronControllerObj.SetINTPPODFlag = true;
            ControllerReqPacket reqName = new ControllerReqPacket();
            CreateNewTaskFromGUI(reqName, 2, 0x20, datapassed);
        }

        private void rdoINTPushPull_CheckedChanged(object sender, EventArgs e)
        {
            int num = rdoINTPushPull.Checked ? 0 : 0xff;
            int[] datapassed = new int[] { num };
            VeyronControllerObj.SetINTPPODFlag = true;
            ControllerReqPacket reqName = new ControllerReqPacket();
            CreateNewTaskFromGUI(reqName, 2, 0x20, datapassed);
        }

        private void rdoMFF1And_CheckedChanged(object sender, EventArgs e)
        {
            int num = rdoMFF1And.Checked ? 0 : 0xff;
            int[] datapassed = new int[] { num };
            VeyronControllerObj.SetMFF1AndOrFlag = true;
            ControllerReqPacket reqName = new ControllerReqPacket();
            CreateNewTaskFromGUI(reqName, 3, 0x33, datapassed);
        }

        private void rdoMFF1ClearDebounce_CheckedChanged(object sender, EventArgs e)
        {
            int num = rdoMFF1ClearDebounce.Checked ? 0xff : 0;
            int[] datapassed = new int[] { num };
            VeyronControllerObj.SetMFF1DBCNTMFlag = true;
            ControllerReqPacket reqName = new ControllerReqPacket();
            CreateNewTaskFromGUI(reqName, 3, 0x3a, datapassed);
        }

        private void rdoMFF1DecDebounce_CheckedChanged(object sender, EventArgs e)
        {
            int num = rdoMFF1DecDebounce.Checked ? 0 : 0xff;
            int[] datapassed = new int[] { num };
            VeyronControllerObj.SetMFF1DBCNTMFlag = true;
            ControllerReqPacket reqName = new ControllerReqPacket();
            CreateNewTaskFromGUI(reqName, 3, 0x3a, datapassed);
        }

        private void rdoMFF1INT_I1_CheckedChanged(object sender, EventArgs e)
        {
            int[] datapassed = new int[2];
            int num = rdoMFF1INT_I1.Checked ? 0xff : 0;
            datapassed[0] = num;
            datapassed[1] = 4;
            VeyronControllerObj.SetIntsConfigFlag = true;
            ControllerReqPacket reqName = new ControllerReqPacket();
            CreateNewTaskFromGUI(reqName, 0, 14, datapassed);
        }

        private void rdoMFF1INT_I2_CheckedChanged(object sender, EventArgs e)
        {
            int[] datapassed = new int[2];
            int num = rdoMFF1INT_I2.Checked ? 0 : 0xff;
            datapassed[0] = num;
            datapassed[1] = 4;
            VeyronControllerObj.SetIntsConfigFlag = true;
            ControllerReqPacket reqName = new ControllerReqPacket();
            CreateNewTaskFromGUI(reqName, 0, 14, datapassed);
        }

        private void rdoMFF1Or_CheckedChanged(object sender, EventArgs e)
        {
            int num = rdoMFF1Or.Checked ? 0xff : 0;
            int[] datapassed = new int[] { num };
            VeyronControllerObj.SetMFF1AndOrFlag = true;
            ControllerReqPacket reqName = new ControllerReqPacket();
            CreateNewTaskFromGUI(reqName, 3, 0x33, datapassed);
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
                VeyronControllerObj.SetWakeOSModeFlag = true;
                ControllerReqPacket reqName = new ControllerReqPacket();
                CreateNewTaskFromGUI(reqName, 0, 0x54, datapassed);
                int selectedIndex = ddlHPFilter.SelectedIndex;
                switch (ddlDataRate.SelectedIndex)
                {
                    case 0:
                        DR_timestep = 2.5;
                        DR_PulseTimeStepNoLPF = 0.625;
                        ddlHPFilter.Items.Clear();
                        ddlHPFilter.Items.Add("16Hz");
                        ddlHPFilter.Items.Add("8Hz");
                        ddlHPFilter.Items.Add("4Hz");
                        ddlHPFilter.Items.Add("2Hz");
                        ddlHPFilter.SelectedIndex = selectedIndex;
                        break;

                    case 1:
                        DR_timestep = 2.5;
                        DR_PulseTimeStepNoLPF = 0.625;
                        ddlHPFilter.Items.Clear();
                        ddlHPFilter.Items.Add("16Hz");
                        ddlHPFilter.Items.Add("8Hz");
                        ddlHPFilter.Items.Add("4Hz");
                        ddlHPFilter.Items.Add("2Hz");
                        ddlHPFilter.SelectedIndex = selectedIndex;
                        break;

                    case 2:
                        DR_timestep = 2.5;
                        DR_PulseTimeStepNoLPF = 0.625;
                        ddlHPFilter.Items.Clear();
                        ddlHPFilter.Items.Add("16Hz");
                        ddlHPFilter.Items.Add("8Hz");
                        ddlHPFilter.Items.Add("4Hz");
                        ddlHPFilter.Items.Add("2Hz");
                        ddlHPFilter.SelectedIndex = selectedIndex;
                        break;

                    case 3:
                        DR_timestep = 2.5;
                        DR_PulseTimeStepNoLPF = 0.625;
                        ddlHPFilter.Items.Clear();
                        ddlHPFilter.Items.Add("16Hz");
                        ddlHPFilter.Items.Add("8Hz");
                        ddlHPFilter.Items.Add("4Hz");
                        ddlHPFilter.Items.Add("2Hz");
                        ddlHPFilter.SelectedIndex = selectedIndex;
                        break;

                    case 4:
                        DR_timestep = 2.5;
                        DR_PulseTimeStepNoLPF = 0.625;
                        ddlHPFilter.Items.Clear();
                        ddlHPFilter.Items.Add("16Hz");
                        ddlHPFilter.Items.Add("8Hz");
                        ddlHPFilter.Items.Add("4Hz");
                        ddlHPFilter.Items.Add("2Hz");
                        ddlHPFilter.SelectedIndex = selectedIndex;
                        break;

                    case 5:
                        DR_timestep = 2.5;
                        DR_PulseTimeStepNoLPF = 0.625;
                        ddlHPFilter.Items.Clear();
                        ddlHPFilter.Items.Add("16Hz");
                        ddlHPFilter.Items.Add("8Hz");
                        ddlHPFilter.Items.Add("4Hz");
                        ddlHPFilter.Items.Add("2Hz");
                        ddlHPFilter.SelectedIndex = selectedIndex;
                        break;
                }
                double num2 = tbMFF1Debounce.Value * DR_timestep;
                if (num2 > 1000.0)
                {
                    lblMFF1Debouncems.Text = "s";
                    num2 /= 1000.0;
                    lblMFF1DebounceVal.Text = string.Format("{0:F2}", num2);
                }
                else
                {
                    lblMFF1Debouncems.Text = "ms";
                    lblMFF1DebounceVal.Text = string.Format("{0:F2}", num2);
                }
                double num3 = tbPL.Value * DR_timestep;
                if (num3 > 1000.0)
                {
                    lblBouncems.Text = "s";
                    num3 /= 1000.0;
                    lblPLbounceVal.Text = string.Format("{0:F4}", num3);
                }
                else
                {
                    lblBouncems.Text = "ms";
                    lblPLbounceVal.Text = string.Format("{0:F2}", num3);
                }
                double num4 = tbTransDebounce.Value * DR_timestep;
                if (num4 > 1000.0)
                {
                    lblTransDebouncems.Text = "s";
                    num4 /= 1000.0;
                    lblTransDebounceVal.Text = string.Format("{0:F2}", num4);
                }
                else
                {
                    lblTransDebouncems.Text = "ms";
                    lblTransDebounceVal.Text = string.Format("{0:F2}", num4);
                }
                if (chkPulseLPFEnable.Checked)
                {
                    pulse_step = DR_timestep;
                }
                else
                {
                    pulse_step = DR_PulseTimeStepNoLPF;
                }
                double num5 = tbFirstPulseTimeLimit.Value * pulse_step;
                if (num5 > 1000.0)
                {
                    lblFirstPulseTimeLimitms.Text = "s";
                    num5 /= 1000.0;
                    lblFirstTimeLimitVal.Text = string.Format("{0:F3}", num5);
                }
                else
                {
                    lblFirstPulseTimeLimitms.Text = "ms";
                    lblFirstTimeLimitVal.Text = string.Format("{0:F3}", num5);
                }
                double num6 = pulse_step * 2.0;
                double num7 = tbPulseLatency.Value * num6;
                if (num7 > 1000.0)
                {
                    lblPulseLatencyms.Text = "s";
                    num7 /= 1000.0;
                    lblPulseLatencyVal.Text = string.Format("{0:F2}", num7);
                }
                else
                {
                    lblPulseLatencyms.Text = "ms";
                    lblPulseLatencyVal.Text = string.Format("{0:F2}", num7);
                }
                double num8 = pulse_step * 2.0;
                double num9 = tbPulse2ndPulseWin.Value * num8;
                if (num9 > 1000.0)
                {
                    lblPulse2ndPulseWinms.Text = "s";
                    num9 /= 1000.0;
                    lblPulse2ndPulseWinVal.Text = string.Format("{0:F2}", num9);
                }
                else
                {
                    lblPulse2ndPulseWinms.Text = "ms";
                    lblPulse2ndPulseWinVal.Text = string.Format("{0:F2}", num9);
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
                VeyronControllerObj.SetWakeOSModeFlag = true;
                ControllerReqPacket reqName = new ControllerReqPacket();
                CreateNewTaskFromGUI(reqName, 0, 0x54, datapassed);
                int selectedIndex = ddlHPFilter.SelectedIndex;
                switch (ddlDataRate.SelectedIndex)
                {
                    case 0:
                        DR_timestep = 5.0;
                        DR_PulseTimeStepNoLPF = 1.25;
                        ddlHPFilter.Items.Clear();
                        ddlHPFilter.Items.Add("8Hz");
                        ddlHPFilter.Items.Add("4Hz");
                        ddlHPFilter.Items.Add("2Hz");
                        ddlHPFilter.Items.Add("1Hz");
                        ddlHPFilter.SelectedIndex = selectedIndex;
                        break;

                    case 1:
                        DR_timestep = 10.0;
                        DR_PulseTimeStepNoLPF = 2.5;
                        ddlHPFilter.Items.Clear();
                        ddlHPFilter.Items.Add("4Hz");
                        ddlHPFilter.Items.Add("2Hz");
                        ddlHPFilter.Items.Add("1Hz");
                        ddlHPFilter.Items.Add("0.5Hz");
                        ddlHPFilter.SelectedIndex = selectedIndex;
                        break;

                    case 2:
                        DR_timestep = 20.0;
                        DR_PulseTimeStepNoLPF = 5.0;
                        ddlHPFilter.Items.Clear();
                        ddlHPFilter.Items.Add("2Hz");
                        ddlHPFilter.Items.Add("1Hz");
                        ddlHPFilter.Items.Add("0.5Hz");
                        ddlHPFilter.Items.Add("0.25Hz");
                        ddlHPFilter.SelectedIndex = selectedIndex;
                        break;

                    case 3:
                        DR_timestep = 80.0;
                        DR_PulseTimeStepNoLPF = 20.0;
                        ddlHPFilter.Items.Clear();
                        ddlHPFilter.Items.Add("0.5Hz");
                        ddlHPFilter.Items.Add("0.25Hz");
                        ddlHPFilter.Items.Add("0.125Hz");
                        ddlHPFilter.Items.Add("0.063Hz");
                        ddlHPFilter.SelectedIndex = selectedIndex;
                        break;

                    case 4:
                        DR_timestep = 160.0;
                        DR_PulseTimeStepNoLPF = 40.0;
                        ddlHPFilter.Items.Clear();
                        ddlHPFilter.Items.Add("0.25Hz");
                        ddlHPFilter.Items.Add("0.125Hz");
                        ddlHPFilter.Items.Add("0.063Hz");
                        ddlHPFilter.Items.Add("0.031Hz");
                        ddlHPFilter.SelectedIndex = selectedIndex;
                        break;

                    case 5:
                        DR_timestep = 640.0;
                        DR_PulseTimeStepNoLPF = 160.0;
                        ddlHPFilter.Items.Clear();
                        ddlHPFilter.Items.Add("0.063Hz");
                        ddlHPFilter.Items.Add("0.031Hz");
                        ddlHPFilter.Items.Add("0.016Hz");
                        ddlHPFilter.Items.Add("0.008Hz");
                        ddlHPFilter.SelectedIndex = selectedIndex;
                        break;
                }
                double num2 = tbMFF1Debounce.Value * DR_timestep;
                if (num2 > 1000.0)
                {
                    lblMFF1Debouncems.Text = "s";
                    num2 /= 1000.0;
                    lblMFF1DebounceVal.Text = string.Format("{0:F2}", num2);
                }
                else
                {
                    lblMFF1Debouncems.Text = "ms";
                    lblMFF1DebounceVal.Text = string.Format("{0:F2}", num2);
                }
                double num3 = tbPL.Value * DR_timestep;
                if (num3 > 1000.0)
                {
                    lblBouncems.Text = "s";
                    num3 /= 1000.0;
                    lblPLbounceVal.Text = string.Format("{0:F4}", num3);
                }
                else
                {
                    lblBouncems.Text = "ms";
                    lblPLbounceVal.Text = string.Format("{0:F2}", num3);
                }
                double num4 = tbTransDebounce.Value * DR_timestep;
                if (num4 > 1000.0)
                {
                    lblTransDebouncems.Text = "s";
                    num4 /= 1000.0;
                    lblTransDebounceVal.Text = string.Format("{0:F2}", num4);
                }
                else
                {
                    lblTransDebouncems.Text = "ms";
                    lblTransDebounceVal.Text = string.Format("{0:F2}", num4);
                }
                if (chkPulseLPFEnable.Checked)
                {
                    pulse_step = DR_timestep;
                }
                else
                {
                    pulse_step = DR_PulseTimeStepNoLPF;
                }
                double num5 = tbFirstPulseTimeLimit.Value * pulse_step;
                if (num5 > 1000.0)
                {
                    lblFirstPulseTimeLimitms.Text = "s";
                    num5 /= 1000.0;
                    lblFirstTimeLimitVal.Text = string.Format("{0:F3}", num5);
                }
                else
                {
                    lblFirstPulseTimeLimitms.Text = "ms";
                    lblFirstTimeLimitVal.Text = string.Format("{0:F3}", num5);
                }
                double num6 = pulse_step * 2.0;
                double num7 = tbPulseLatency.Value * num6;
                if (num7 > 1000.0)
                {
                    lblPulseLatencyms.Text = "s";
                    num7 /= 1000.0;
                    lblPulseLatencyVal.Text = string.Format("{0:F2}", num7);
                }
                else
                {
                    lblPulseLatencyms.Text = "ms";
                    lblPulseLatencyVal.Text = string.Format("{0:F2}", num7);
                }
                double num8 = pulse_step * 2.0;
                double num9 = tbPulse2ndPulseWin.Value * num8;
                if (num9 > 1000.0)
                {
                    lblPulse2ndPulseWinms.Text = "s";
                    num9 /= 1000.0;
                    lblPulse2ndPulseWinVal.Text = string.Format("{0:F2}", num9);
                }
                else
                {
                    lblPulse2ndPulseWinms.Text = "ms";
                    lblPulse2ndPulseWinVal.Text = string.Format("{0:F2}", num9);
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
                VeyronControllerObj.SetWakeOSModeFlag = true;
                ControllerReqPacket reqName = new ControllerReqPacket();
                CreateNewTaskFromGUI(reqName, 0, 0x54, datapassed);
                int selectedIndex = ddlHPFilter.SelectedIndex;
                switch (ddlDataRate.SelectedIndex)
                {
                    case 0:
                        DR_timestep = 5.0;
                        DR_PulseTimeStepNoLPF = 2.5;
                        ddlHPFilter.Items.Clear();
                        ddlHPFilter.Items.Add("4Hz");
                        ddlHPFilter.Items.Add("2Hz");
                        ddlHPFilter.Items.Add("1Hz");
                        ddlHPFilter.Items.Add("0.5Hz");
                        ddlHPFilter.SelectedIndex = selectedIndex;
                        break;

                    case 1:
                        DR_timestep = 10.0;
                        DR_PulseTimeStepNoLPF = 5.0;
                        ddlHPFilter.Items.Clear();
                        ddlHPFilter.Items.Add("2Hz");
                        ddlHPFilter.Items.Add("1Hz");
                        ddlHPFilter.Items.Add("0.5Hz");
                        ddlHPFilter.Items.Add("0.25Hz");
                        ddlHPFilter.SelectedIndex = selectedIndex;
                        break;

                    case 2:
                        DR_timestep = 20.0;
                        DR_PulseTimeStepNoLPF = 10.0;
                        ddlHPFilter.Items.Clear();
                        ddlHPFilter.Items.Add("1Hz");
                        ddlHPFilter.Items.Add("0.5Hz");
                        ddlHPFilter.Items.Add("0.25Hz");
                        ddlHPFilter.Items.Add("0.125Hz");
                        ddlHPFilter.SelectedIndex = selectedIndex;
                        break;

                    case 3:
                        DR_timestep = 80.0;
                        DR_PulseTimeStepNoLPF = 40.0;
                        ddlHPFilter.Items.Clear();
                        ddlHPFilter.Items.Add("0.25Hz");
                        ddlHPFilter.Items.Add("0.125Hz");
                        ddlHPFilter.Items.Add("0.063Hz");
                        ddlHPFilter.Items.Add("0.031Hz");
                        ddlHPFilter.SelectedIndex = selectedIndex;
                        break;

                    case 4:
                        DR_timestep = 160.0;
                        DR_PulseTimeStepNoLPF = 80.0;
                        ddlHPFilter.Items.Clear();
                        ddlHPFilter.Items.Add("0.125Hz");
                        ddlHPFilter.Items.Add("0.063Hz");
                        ddlHPFilter.Items.Add("0.031Hz");
                        ddlHPFilter.Items.Add("0.016Hz");
                        ddlHPFilter.SelectedIndex = selectedIndex;
                        break;

                    case 5:
                        DR_timestep = 640.0;
                        DR_PulseTimeStepNoLPF = 320.0;
                        ddlHPFilter.Items.Clear();
                        ddlHPFilter.Items.Add("0.031Hz");
                        ddlHPFilter.Items.Add("0.016Hz");
                        ddlHPFilter.Items.Add("0.008Hz");
                        ddlHPFilter.Items.Add("0.004Hz");
                        ddlHPFilter.SelectedIndex = selectedIndex;
                        break;
                }
                double num2 = tbMFF1Debounce.Value * DR_timestep;
                if (num2 > 1000.0)
                {
                    lblMFF1Debouncems.Text = "s";
                    num2 /= 1000.0;
                    lblMFF1DebounceVal.Text = string.Format("{0:F2}", num2);
                }
                else
                {
                    lblMFF1Debouncems.Text = "ms";
                    lblMFF1DebounceVal.Text = string.Format("{0:F2}", num2);
                }
                double num3 = tbPL.Value * DR_timestep;
                if (num3 > 1000.0)
                {
                    lblBouncems.Text = "s";
                    num3 /= 1000.0;
                    lblPLbounceVal.Text = string.Format("{0:F4}", num3);
                }
                else
                {
                    lblBouncems.Text = "ms";
                    lblPLbounceVal.Text = string.Format("{0:F2}", num3);
                }
                double num4 = tbTransDebounce.Value * DR_timestep;
                if (num4 > 1000.0)
                {
                    lblTransDebouncems.Text = "s";
                    num4 /= 1000.0;
                    lblTransDebounceVal.Text = string.Format("{0:F2}", num4);
                }
                else
                {
                    lblTransDebouncems.Text = "ms";
                    lblTransDebounceVal.Text = string.Format("{0:F2}", num4);
                }
                if (chkPulseLPFEnable.Checked)
                {
                    pulse_step = DR_timestep;
                }
                else
                {
                    pulse_step = DR_PulseTimeStepNoLPF;
                }
                double num5 = tbFirstPulseTimeLimit.Value * pulse_step;
                if (num5 > 1000.0)
                {
                    lblFirstPulseTimeLimitms.Text = "s";
                    num5 /= 1000.0;
                    lblFirstTimeLimitVal.Text = string.Format("{0:F3}", num5);
                }
                else
                {
                    lblFirstPulseTimeLimitms.Text = "ms";
                    lblFirstTimeLimitVal.Text = string.Format("{0:F3}", num5);
                }
                double num6 = pulse_step * 2.0;
                double num7 = tbPulseLatency.Value * num6;
                if (num7 > 1000.0)
                {
                    lblPulseLatencyms.Text = "s";
                    num7 /= 1000.0;
                    lblPulseLatencyVal.Text = string.Format("{0:F2}", num7);
                }
                else
                {
                    lblPulseLatencyms.Text = "ms";
                    lblPulseLatencyVal.Text = string.Format("{0:F2}", num7);
                }
                double num8 = pulse_step * 2.0;
                double num9 = tbPulse2ndPulseWin.Value * num8;
                if (num9 > 1000.0)
                {
                    lblPulse2ndPulseWinms.Text = "s";
                    num9 /= 1000.0;
                    lblPulse2ndPulseWinVal.Text = string.Format("{0:F2}", num9);
                }
                else
                {
                    lblPulse2ndPulseWinms.Text = "ms";
                    lblPulse2ndPulseWinVal.Text = string.Format("{0:F2}", num9);
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
                VeyronControllerObj.SetWakeOSModeFlag = true;
                ControllerReqPacket reqName = new ControllerReqPacket();
                CreateNewTaskFromGUI(reqName, 0, 0x54, datapassed);
                int selectedIndex = ddlHPFilter.SelectedIndex;
                switch (ddlDataRate.SelectedIndex)
                {
                    case 0:
                        DR_timestep = 5.0;
                        DR_PulseTimeStepNoLPF = 1.25;
                        ddlHPFilter.Items.Clear();
                        ddlHPFilter.Items.Add("8Hz");
                        ddlHPFilter.Items.Add("4Hz");
                        ddlHPFilter.Items.Add("2Hz");
                        ddlHPFilter.Items.Add("1Hz");
                        ddlHPFilter.SelectedIndex = selectedIndex;
                        break;

                    case 1:
                        DR_timestep = 10.0;
                        DR_PulseTimeStepNoLPF = 2.5;
                        ddlHPFilter.Items.Clear();
                        ddlHPFilter.Items.Add("4Hz");
                        ddlHPFilter.Items.Add("2Hz");
                        ddlHPFilter.Items.Add("1Hz");
                        ddlHPFilter.Items.Add("0.5Hz");
                        ddlHPFilter.SelectedIndex = selectedIndex;
                        break;

                    case 2:
                        DR_timestep = 20.0;
                        DR_PulseTimeStepNoLPF = 5.0;
                        ddlHPFilter.Items.Clear();
                        ddlHPFilter.Items.Add("2Hz");
                        ddlHPFilter.Items.Add("1Hz");
                        ddlHPFilter.Items.Add("0.5Hz");
                        ddlHPFilter.Items.Add("0.25Hz");
                        ddlHPFilter.SelectedIndex = selectedIndex;
                        break;

                    case 3:
                        DR_timestep = 20.0;
                        DR_PulseTimeStepNoLPF = 5.0;
                        ddlHPFilter.Items.Clear();
                        ddlHPFilter.Items.Add("2Hz");
                        ddlHPFilter.Items.Add("1Hz");
                        ddlHPFilter.Items.Add("0.5Hz");
                        ddlHPFilter.Items.Add("0.25Hz");
                        ddlHPFilter.SelectedIndex = selectedIndex;
                        break;

                    case 4:
                        DR_timestep = 20.0;
                        DR_PulseTimeStepNoLPF = 5.0;
                        ddlHPFilter.Items.Clear();
                        ddlHPFilter.Items.Add("2Hz");
                        ddlHPFilter.Items.Add("1Hz");
                        ddlHPFilter.Items.Add("0.5Hz");
                        ddlHPFilter.Items.Add("0.25Hz");
                        ddlHPFilter.SelectedIndex = selectedIndex;
                        break;

                    case 5:
                        DR_timestep = 20.0;
                        DR_PulseTimeStepNoLPF = 5.0;
                        ddlHPFilter.Items.Clear();
                        ddlHPFilter.Items.Add("2Hz");
                        ddlHPFilter.Items.Add("1Hz");
                        ddlHPFilter.Items.Add("0.5Hz");
                        ddlHPFilter.Items.Add("0.25Hz");
                        ddlHPFilter.SelectedIndex = selectedIndex;
                        break;
                }
                double num2 = tbMFF1Debounce.Value * DR_timestep;
                if (num2 > 1000.0)
                {
                    lblMFF1Debouncems.Text = "s";
                    num2 /= 1000.0;
                    lblMFF1DebounceVal.Text = string.Format("{0:F2}", num2);
                }
                else
                {
                    lblMFF1Debouncems.Text = "ms";
                    lblMFF1DebounceVal.Text = string.Format("{0:F2}", num2);
                }
                double num3 = tbPL.Value * DR_timestep;
                if (num3 > 1000.0)
                {
                    lblBouncems.Text = "s";
                    num3 /= 1000.0;
                    lblPLbounceVal.Text = string.Format("{0:F4}", num3);
                }
                else
                {
                    lblBouncems.Text = "ms";
                    lblPLbounceVal.Text = string.Format("{0:F2}", num3);
                }
                double num4 = tbTransDebounce.Value * DR_timestep;
                if (num4 > 1000.0)
                {
                    lblTransDebouncems.Text = "s";
                    num4 /= 1000.0;
                    lblTransDebounceVal.Text = string.Format("{0:F2}", num4);
                }
                else
                {
                    lblTransDebouncems.Text = "ms";
                    lblTransDebounceVal.Text = string.Format("{0:F2}", num4);
                }
                if (chkPulseLPFEnable.Checked)
                {
                    pulse_step = DR_timestep;
                }
                else
                {
                    pulse_step = DR_PulseTimeStepNoLPF;
                }
                double num5 = tbFirstPulseTimeLimit.Value * pulse_step;
                if (num5 > 1000.0)
                {
                    lblFirstPulseTimeLimitms.Text = "s";
                    num5 /= 1000.0;
                    lblFirstTimeLimitVal.Text = string.Format("{0:F3}", num5);
                }
                else
                {
                    lblFirstPulseTimeLimitms.Text = "ms";
                    lblFirstTimeLimitVal.Text = string.Format("{0:F3}", num5);
                }
                double num6 = pulse_step * 2.0;
                double num7 = tbPulseLatency.Value * num6;
                if (num7 > 1000.0)
                {
                    lblPulseLatencyms.Text = "s";
                    num7 /= 1000.0;
                    lblPulseLatencyVal.Text = string.Format("{0:F2}", num7);
                }
                else
                {
                    lblPulseLatencyms.Text = "ms";
                    lblPulseLatencyVal.Text = string.Format("{0:F2}", num7);
                }
                double num8 = pulse_step * 2.0;
                double num9 = tbPulse2ndPulseWin.Value * num8;
                if (num9 > 1000.0)
                {
                    lblPulse2ndPulseWinms.Text = "s";
                    num9 /= 1000.0;
                    lblPulse2ndPulseWinVal.Text = string.Format("{0:F2}", num9);
                }
                else
                {
                    lblPulse2ndPulseWinms.Text = "ms";
                    lblPulse2ndPulseWinVal.Text = string.Format("{0:F2}", num9);
                }
            }
        }

        private void rdoPLINT_I1_CheckedChanged(object sender, EventArgs e)
        {
            int[] datapassed = new int[2];
            int num = rdoPLINT_I1.Checked ? 0xff : 0;
            datapassed[0] = num;
            datapassed[1] = 0x10;
            VeyronControllerObj.SetIntsConfigFlag = true;
            ControllerReqPacket reqName = new ControllerReqPacket();
            CreateNewTaskFromGUI(reqName, 0, 14, datapassed);
        }

        private void rdoPLINT_I2_CheckedChanged(object sender, EventArgs e)
        {
            int[] datapassed = new int[2];
            int num = rdoPLINT_I2.Checked ? 0 : 0xff;
            datapassed[0] = num;
            datapassed[1] = 0x10;
            VeyronControllerObj.SetIntsConfigFlag = true;
            ControllerReqPacket reqName = new ControllerReqPacket();
            CreateNewTaskFromGUI(reqName, 0, 14, datapassed);
        }

        private void rdoPulseINT_I1_CheckedChanged(object sender, EventArgs e)
        {
            int[] datapassed = new int[2];
            int num = rdoPulseINT_I1.Checked ? 0xff : 0;
            datapassed[0] = num;
            datapassed[1] = 8;
            VeyronControllerObj.SetIntsConfigFlag = true;
            ControllerReqPacket reqName = new ControllerReqPacket();
            CreateNewTaskFromGUI(reqName, 0, 14, datapassed);
        }

        private void rdoPulseINT_I2_CheckedChanged(object sender, EventArgs e)
        {
            int[] datapassed = new int[2];
            int num = rdoPulseINT_I2.Checked ? 0 : 0xff;
            datapassed[0] = num;
            datapassed[1] = 8;
            VeyronControllerObj.SetIntsConfigFlag = true;
            ControllerReqPacket reqName = new ControllerReqPacket();
            CreateNewTaskFromGUI(reqName, 0, 14, datapassed);
        }

        private void rdoSOSHiResMode_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoSOSHiResMode.Checked)
            {
                int[] datapassed = new int[1];
                SleepOSMode = 2;
                datapassed[0] = SleepOSMode;
                VeyronControllerObj.SetSleepOSModeFlag = true;
                ControllerReqPacket reqName = new ControllerReqPacket();
                CreateNewTaskFromGUI(reqName, 0, 0x55, datapassed);
                switch (ddlSleepSR.SelectedIndex)
                {
                    case 2:
                        DRSleep_timestep = 2.5;
                        DRSleep_PulseTimeStepNoLPF = 0.625;
                        break;

                    case 3:
                        DRSleep_timestep = 2.5;
                        DRSleep_PulseTimeStepNoLPF = 0.625;
                        break;

                    case 4:
                        DRSleep_timestep = 2.5;
                        DRSleep_PulseTimeStepNoLPF = 0.625;
                        break;

                    case 5:
                        DRSleep_timestep = 2.5;
                        DRSleep_PulseTimeStepNoLPF = 0.625;
                        break;
                }
            }
        }

        private void rdoSOSLNLPMode_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoSOSLNLPMode.Checked)
            {
                int[] datapassed = new int[1];
                SleepOSMode = 1;
                datapassed[0] = SleepOSMode;
                VeyronControllerObj.SetSleepOSModeFlag = true;
                ControllerReqPacket reqName = new ControllerReqPacket();
                CreateNewTaskFromGUI(reqName, 0, 0x55, datapassed);
                switch (ddlSleepSR.SelectedIndex)
                {
                    case 2:
                        DRSleep_timestep = 20.0;
                        DRSleep_PulseTimeStepNoLPF = 5.0;
                        break;

                    case 3:
                        DRSleep_timestep = 80.0;
                        DRSleep_PulseTimeStepNoLPF = 20.0;
                        break;

                    case 4:
                        DRSleep_timestep = 160.0;
                        DRSleep_PulseTimeStepNoLPF = 40.0;
                        break;

                    case 5:
                        DRSleep_timestep = 640.0;
                        DRSleep_PulseTimeStepNoLPF = 160.0;
                        break;
                }
            }
        }

        private void rdoSOSLPMode_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoSOSLPMode.Checked)
            {
                int[] datapassed = new int[1];
                SleepOSMode = 3;
                datapassed[0] = SleepOSMode;
                VeyronControllerObj.SetSleepOSModeFlag = true;
                ControllerReqPacket reqName = new ControllerReqPacket();
                CreateNewTaskFromGUI(reqName, 0, 0x55, datapassed);
                switch (ddlSleepSR.SelectedIndex)
                {
                    case 2:
                        DRSleep_timestep = 20.0;
                        DRSleep_PulseTimeStepNoLPF = 10.0;
                        break;

                    case 3:
                        DRSleep_timestep = 80.0;
                        DRSleep_PulseTimeStepNoLPF = 40.0;
                        break;

                    case 4:
                        DRSleep_timestep = 160.0;
                        DRSleep_PulseTimeStepNoLPF = 80.0;
                        break;

                    case 5:
                        DRSleep_timestep = 640.0;
                        DRSleep_PulseTimeStepNoLPF = 320.0;
                        break;
                }
            }
        }

        private void rdoSOSNormalMode_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoSOSNormalMode.Checked)
            {
                int[] datapassed = new int[1];
                SleepOSMode = 0;
                datapassed[0] = SleepOSMode;
                VeyronControllerObj.SetSleepOSModeFlag = true;
                ControllerReqPacket reqName = new ControllerReqPacket();
                CreateNewTaskFromGUI(reqName, 0, 0x55, datapassed);
                switch (ddlSleepSR.SelectedIndex)
                {
                    case 2:
                        DRSleep_timestep = 20.0;
                        DRSleep_PulseTimeStepNoLPF = 5.0;
                        break;

                    case 3:
                        DRSleep_timestep = 20.0;
                        DRSleep_PulseTimeStepNoLPF = 5.0;
                        break;

                    case 4:
                        DRSleep_timestep = 20.0;
                        DRSleep_PulseTimeStepNoLPF = 5.0;
                        break;

                    case 5:
                        DRSleep_timestep = 20.0;
                        DRSleep_PulseTimeStepNoLPF = 5.0;
                        break;
                }
            }
        }

        private void rdoTrans1INT_I1_CheckedChanged(object sender, EventArgs e)
        {
            int[] datapassed = new int[2];
            int num = rdoTrans1INT_I1.Checked ? 0xff : 0;
            datapassed[0] = num;
            datapassed[1] = 2;
            VeyronControllerObj.SetIntsConfigFlag = true;
            ControllerReqPacket reqName = new ControllerReqPacket();
            CreateNewTaskFromGUI(reqName, 0, 14, datapassed);
        }

        private void rdoTrans1INT_I2_CheckedChanged(object sender, EventArgs e)
        {
            int[] datapassed = new int[2];
            int num = rdoTrans1INT_I2.Checked ? 0 : 0xff;
            datapassed[0] = num;
            datapassed[1] = 2;
            VeyronControllerObj.SetIntsConfigFlag = true;
            ControllerReqPacket reqName = new ControllerReqPacket();
            CreateNewTaskFromGUI(reqName, 0, 14, datapassed);
        }

        private void rdoTransClearDebounce_CheckedChanged(object sender, EventArgs e)
        {
            int num = rdoTransClearDebounce.Checked ? 0xff : 0;
            int[] datapassed = new int[] { num };
            VeyronControllerObj.SetTransDBCNTMFlag = true;
            ControllerReqPacket reqName = new ControllerReqPacket();
            CreateNewTaskFromGUI(reqName, 5, 0x26, datapassed);
        }

        private void rdoTransClearDebounceNEW_CheckedChanged_1(object sender, EventArgs e)
        {
            int num = rdoTransClearDebounceNEW.Checked ? 0xff : 0;
            int[] datapassed = new int[] { num };
            VeyronControllerObj.SetTransDBCNTMNEWFlag = true;
            ControllerReqPacket reqName = new ControllerReqPacket();
            CreateNewTaskFromGUI(reqName, 5, 50, datapassed);
        }

        private void rdoTransDecDebounce_CheckedChanged(object sender, EventArgs e)
        {
            int num = rdoTransDecDebounce.Checked ? 0 : 0xff;
            int[] datapassed = new int[] { num };
            VeyronControllerObj.SetTransDBCNTMFlag = true;
            ControllerReqPacket reqName = new ControllerReqPacket();
            CreateNewTaskFromGUI(reqName, 5, 0x26, datapassed);
        }

        private void rdoTransDecDebounceNEW_CheckedChanged_1(object sender, EventArgs e)
        {
            int num = rdoTransDecDebounceNEW.Checked ? 0 : 0xff;
            int[] datapassed = new int[] { num };
            VeyronControllerObj.SetTransDBCNTMNEWFlag = true;
            ControllerReqPacket reqName = new ControllerReqPacket();
            CreateNewTaskFromGUI(reqName, 5, 50, datapassed);
        }

        private void rdoTransINT_I1_CheckedChanged(object sender, EventArgs e)
        {
            int[] datapassed = new int[2];
            int num = rdoTransINT_I1.Checked ? 0xff : 0;
            datapassed[0] = num;
            datapassed[1] = 0x20;
            VeyronControllerObj.SetIntsConfigFlag = true;
            ControllerReqPacket reqName = new ControllerReqPacket();
            CreateNewTaskFromGUI(reqName, 0, 14, datapassed);
        }

        private void rdoTransINT_I2_CheckedChanged(object sender, EventArgs e)
        {
            int[] datapassed = new int[2];
            int num = rdoTransINT_I2.Checked ? 0 : 0xff;
            datapassed[0] = num;
            datapassed[1] = 0x20;
            VeyronControllerObj.SetIntsConfigFlag = true;
            ControllerReqPacket reqName = new ControllerReqPacket();
            CreateNewTaskFromGUI(reqName, 0, 14, datapassed);
        }

        private void rdoTriggerMode_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoTriggerMode.Checked)
            {
                FIFOModeValue = 3;
            }
        }

        private void rdoXYZFullResMain_CheckedChanged(object sender, EventArgs e)
        {
            int num;
            int[] numArray;
            ControllerReqPacket packet;
            if (rdoXYZFullResMain.Checked)
            {
                rdoFIFO14bitDataDisplay.Checked = true;
                rdoFIFO8bitDataDisplay.Checked = false;
                num = 0;
                numArray = new int[] { num };
                VeyronControllerObj.SetFREADFlag = true;
                packet = new ControllerReqPacket();
                CreateNewTaskFromGUI(packet, 0, 0x58, numArray);
                rdoFIFO14bitDataDisplay.Checked = true;
                rdoFIFO8bitDataDisplay.Checked = false;
            }
            else
            {
                rdoFIFO8bitDataDisplay.Checked = true;
                rdoFIFO14bitDataDisplay.Checked = false;
                num = 0xff;
                numArray = new int[] { num };
                VeyronControllerObj.SetFREADFlag = true;
                packet = new ControllerReqPacket();
                CreateNewTaskFromGUI(packet, 0, 0x58, numArray);
            }
        }

        private void TabTool_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            int[] numArray;
            ControllerReqPacket packet;
            ControllerReqPacket packet2;
            ControllerReqPacket packet3;
            ControllerReqPacket packet4;
            switch (TabTool.SelectedIndex)
            {
                case 0:
                    if (rdoActive.Checked && chkEnableASleep.Checked)
                    {
                        VeyronControllerObj.ReturnSysmodStatus = true;
                    }
                    VeyronControllerObj.ReturnTransStatus = false;
                    VeyronControllerObj.ReturnPulseStatus = false;
                    VeyronControllerObj.ReturnDataStatus = false;
                    VeyronControllerObj.ReturnFIFOStatus = false;
                    VeyronControllerObj.ReturnMFF1Status = false;
                    VeyronControllerObj.ReturnPLStatus = false;
                    VeyronControllerObj.DisableData();
                    if (rdoActive.Checked)
                    {
                        btnViewData.Enabled = true;
                    }
                    if (rdoActive.Checked)
                    {
                        if ((((chkEnIntFIFO.Checked || chkEnIntDR.Checked) || (chkEnIntTrans.Checked || chkEnIntPulse.Checked)) || (chkEnIntMFF1.Checked || chkEnIntPL.Checked)) || chkEnIntASleep.Checked)
                        {
                            VeyronControllerObj.PollInterruptMapStatus = true;
                            p9.Enabled = true;
                        }
                        else
                        {
                            p9.Enabled = false;
                        }
                    }
                    btnDisableData.Enabled = false;
                    return;

                case 1:
                    if (!chkEnableASleep.Checked || !rdoActive.Checked)
                    {
                        VeyronControllerObj.ReturnSysmodStatus = false;
                        break;
                    }
                    VeyronControllerObj.ReturnSysmodStatus = true;
                    break;

                case 2:
                    if (!chkEnableASleep.Checked || !rdoActive.Checked)
                    {
                        VeyronControllerObj.ReturnSysmodStatus = false;
                    }
                    else
                    {
                        VeyronControllerObj.ReturnSysmodStatus = true;
                    }
                    VeyronControllerObj.PollInterruptMapStatus = false;
                    VeyronControllerObj.ReturnMFF1Status = false;
                    VeyronControllerObj.ReturnPLStatus = false;
                    VeyronControllerObj.ReturnTransStatus = false;
                    VeyronControllerObj.ReturnPulseStatus = false;
                    VeyronControllerObj.ReturnFIFOStatus = false;
                    VeyronControllerObj.DisableData();
                    if (rdoActive.Checked)
                    {
                        VeyronControllerObj.ReturnDataStatus = true;
                    }
                    return;

                case 3:
                    if (!chkEnableASleep.Checked || !rdoActive.Checked)
                    {
                        VeyronControllerObj.ReturnSysmodStatus = false;
                    }
                    else
                    {
                        VeyronControllerObj.ReturnSysmodStatus = true;
                    }
                    VeyronControllerObj.PollInterruptMapStatus = false;
                    VeyronControllerObj.ReturnDataStatus = false;
                    VeyronControllerObj.ReturnPLStatus = false;
                    VeyronControllerObj.ReturnTransStatus = false;
                    VeyronControllerObj.ReturnPulseStatus = false;
                    VeyronControllerObj.ReturnFIFOStatus = false;
                    VeyronControllerObj.DisableData();
                    if (rdoActive.Checked && ((chkXEFE.Checked || chkYEFE.Checked) || chkZEFE.Checked))
                    {
                        VeyronControllerObj.ReturnMFF1Status = true;
                        numArray = new int[] { SystemPolling };
                        if (rdo8bitDataMain.Checked)
                        {
                            VeyronControllerObj.Data8bFlag = true;
                            packet = new ControllerReqPacket();
                            CreateNewTaskFromGUI(packet, 3, 1, numArray);
                            UpdateWaveformMFF(Gees8bData);
                        }
                        else if (rdoXYZFullResMain.Checked)
                        {
                            switch (DeviceID)
                            {
                                case 2:
                                    VeyronControllerObj.Data14bFlag = true;
                                    packet2 = new ControllerReqPacket();
                                    CreateNewTaskFromGUI(packet2, 3, 0x5c, numArray);
                                    UpdateWaveformMFF(Gees14bData);
                                    break;

                                case 3:
                                    VeyronControllerObj.Data12bFlag = true;
                                    packet3 = new ControllerReqPacket();
                                    CreateNewTaskFromGUI(packet3, 3, 2, numArray);
                                    UpdateWaveformMFF(Gees12bData);
                                    break;

                                case 4:
                                    VeyronControllerObj.Data10bFlag = true;
                                    packet4 = new ControllerReqPacket();
                                    CreateNewTaskFromGUI(packet4, 3, 0x5b, numArray);
                                    UpdateWaveformMFF(Gees10bData);
                                    break;
                            }
                        }
                    }
                    return;

                case 4:
                    if (!chkEnableASleep.Checked || !rdoActive.Checked)
                    {
                        VeyronControllerObj.ReturnSysmodStatus = false;
                    }
                    else
                    {
                        VeyronControllerObj.ReturnSysmodStatus = true;
                    }
                    VeyronControllerObj.PollInterruptMapStatus = false;
                    VeyronControllerObj.ReturnDataStatus = false;
                    VeyronControllerObj.ReturnMFF1Status = false;
                    VeyronControllerObj.ReturnTransStatus = false;
                    VeyronControllerObj.ReturnPulseStatus = false;
                    VeyronControllerObj.ReturnFIFOStatus = false;
                    VeyronControllerObj.DisableData();
                    if (rdoActive.Checked && chkEnablePL.Checked)
                    {
                        VeyronControllerObj.ReturnPLStatus = true;
                    }
                    return;

                case 5:
                    if (!chkEnableASleep.Checked || !rdoActive.Checked)
                    {
                        VeyronControllerObj.ReturnSysmodStatus = false;
                    }
                    else
                    {
                        VeyronControllerObj.ReturnSysmodStatus = true;
                    }
                    VeyronControllerObj.PollInterruptMapStatus = false;
                    VeyronControllerObj.ReturnDataStatus = false;
                    VeyronControllerObj.ReturnMFF1Status = false;
                    VeyronControllerObj.ReturnPLStatus = false;
                    VeyronControllerObj.ReturnPulseStatus = false;
                    VeyronControllerObj.ReturnFIFOStatus = false;
                    VeyronControllerObj.DisableData();
                    if (rdoActive.Checked)
                    {
                        if ((chkTransEnableXFlag.Checked || chkTransEnableYFlag.Checked) || chkTransEnableZFlag.Checked)
                        {
                            VeyronControllerObj.ReturnTransStatus = true;
                        }
                        if ((chkTransEnableXFlagNEW.Checked || chkTransEnableYFlagNEW.Checked) || chkTransEnableZFlagNEW.Checked)
                        {
                            VeyronControllerObj.ReturnTrans1Status = true;
                        }
                        if (VeyronControllerObj.ReturnTrans1Status || VeyronControllerObj.ReturnTransStatus)
                        {
                            numArray = new int[] { SystemPolling };
                            gphXYZ.Enabled = true;
                            if (rdo8bitDataMain.Checked)
                            {
                                VeyronControllerObj.Data8bFlag = true;
                                packet = new ControllerReqPacket();
                                CreateNewTaskFromGUI(packet, 5, 1, numArray);
                            }
                            else if (rdoXYZFullResMain.Checked)
                            {
                                switch (DeviceID)
                                {
                                    case 2:
                                        VeyronControllerObj.Data14bFlag = true;
                                        packet2 = new ControllerReqPacket();
                                        CreateNewTaskFromGUI(packet2, 5, 0x5c, numArray);
                                        break;

                                    case 3:
                                        VeyronControllerObj.Data12bFlag = true;
                                        packet3 = new ControllerReqPacket();
                                        CreateNewTaskFromGUI(packet3, 5, 2, numArray);
                                        break;

                                    case 4:
                                        VeyronControllerObj.Data10bFlag = true;
                                        packet4 = new ControllerReqPacket();
                                        CreateNewTaskFromGUI(packet4, 5, 0x5b, numArray);
                                        break;
                                }
                            }
                        }
                    }
                    return;

                case 6:
                    if (!chkEnableASleep.Checked || !rdoActive.Checked)
                    {
                        VeyronControllerObj.ReturnSysmodStatus = false;
                    }
                    else
                    {
                        VeyronControllerObj.ReturnSysmodStatus = true;
                    }
                    VeyronControllerObj.PollInterruptMapStatus = false;
                    VeyronControllerObj.ReturnDataStatus = false;
                    VeyronControllerObj.ReturnMFF1Status = false;
                    VeyronControllerObj.ReturnPLStatus = false;
                    VeyronControllerObj.ReturnTransStatus = false;
                    VeyronControllerObj.ReturnFIFOStatus = false;
                    VeyronControllerObj.DisableData();
                    if (rdoActive.Checked && ((((chkPulseEnableXSP.Checked || chkPulseEnableYSP.Checked) || (chkPulseEnableZSP.Checked || chkPulseEnableXDP.Checked)) || chkPulseEnableYDP.Checked) || chkPulseEnableZDP.Checked))
                    {
                        VeyronControllerObj.ReturnPulseStatus = true;
                    }
                    return;

                case 7:
                    if (!chkEnableASleep.Checked || !rdoActive.Checked)
                    {
                        VeyronControllerObj.ReturnSysmodStatus = false;
                    }
                    else
                    {
                        VeyronControllerObj.ReturnSysmodStatus = true;
                    }
                    VeyronControllerObj.PollInterruptMapStatus = false;
                    VeyronControllerObj.ReturnDataStatus = false;
                    VeyronControllerObj.ReturnMFF1Status = false;
                    VeyronControllerObj.ReturnPLStatus = false;
                    VeyronControllerObj.ReturnTransStatus = false;
                    VeyronControllerObj.ReturnPulseStatus = false;
                    VeyronControllerObj.DisableData();
                    if (rdoActive.Checked && !chkDisableFIFO.Checked)
                    {
                        int[] datapassed = new int[5];
                        datapassed[0] = SystemPolling;
                        datapassed[1] = tBWatermark.Value;
                        datapassed[2] = FIFODump8orFull;
                        datapassed[4] = FullScaleValue;
                        if (rdoFill.Checked)
                        {
                            datapassed[3] = 0;
                        }
                        else if (rdoCircular.Checked)
                        {
                            datapassed[3] = 1;
                        }
                        else if (rdoTriggerMode.Checked)
                        {
                            datapassed[3] = 2;
                            if (chkTrigTrans.Checked)
                            {
                                VeyronControllerObj.ReturnTransStatus = true;
                            }
                            if (chkTriggerLP.Checked)
                            {
                                VeyronControllerObj.ReturnPLStatus = true;
                            }
                            if (chkTriggerMFF.Checked)
                            {
                                VeyronControllerObj.ReturnMFF1Status = true;
                            }
                            if (chkTriggerPulse.Checked)
                            {
                                VeyronControllerObj.ReturnPulseStatus = true;
                            }
                        }
                        VeyronControllerObj.PollingFIFOFlag = true;
                        ControllerReqPacket reqName = new ControllerReqPacket();
                        CreateNewTaskFromGUI(reqName, 7, 80, datapassed);
                    }
                    return;

                default:
                    return;
            }
            VeyronControllerObj.PollInterruptMapStatus = false;
            VeyronControllerObj.ReturnDataStatus = false;
            VeyronControllerObj.ReturnMFF1Status = false;
            VeyronControllerObj.ReturnPLStatus = false;
            VeyronControllerObj.ReturnTransStatus = false;
            VeyronControllerObj.ReturnPulseStatus = false;
            VeyronControllerObj.ReturnFIFOStatus = false;
            VeyronControllerObj.DisableData();
        }

        private void tbFirstPulseTimeLimit_Scroll_1(object sender, EventArgs e)
        {
            if (chkPulseLPFEnable.Checked)
            {
                if (ledSleep.Value)
                {
                    pulse_step = DRSleep_timestep;
                }
                else
                {
                    pulse_step = DR_timestep;
                }
            }
            else if (ledSleep.Value)
            {
                pulse_step = DRSleep_PulseTimeStepNoLPF;
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

        private void tbMFF1Debounce_Scroll(object sender, EventArgs e)
        {
            double num = 0.0;
            if (ledSleep.Value)
            {
                num = DRSleep_timestep;
            }
            else
            {
                num = DR_timestep;
            }
            double num2 = tbMFF1Debounce.Value * num;
            if (num2 > 1000.0)
            {
                lblMFF1Debouncems.Text = "s";
                num2 /= 1000.0;
                lblMFF1DebounceVal.Text = string.Format("{0:F2}", num2);
            }
            else
            {
                lblMFF1Debouncems.Text = "ms";
                lblMFF1DebounceVal.Text = string.Format("{0:F2}", num2);
            }
        }

        private void tbMFF1Threshold_Scroll(object sender, EventArgs e)
        {
            double num2 = 0.063;
            double num = tbMFF1Threshold.Value * num2;
            lblMFF1ThresholdVal.Text = string.Format("{0:F2}", num);
        }

        private void tbPulse2ndPulseWin_Scroll_1(object sender, EventArgs e)
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

        private void tbPulseLatency_Scroll_1(object sender, EventArgs e)
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

        private void tbPulseXThreshold_Scroll_1(object sender, EventArgs e)
        {
            double num2 = 0.063;
            double num = tbPulseXThreshold.Value * num2;
            lblPulseXThresholdVal.Text = string.Format("{0:F3}", num);
        }

        private void tbPulseYThreshold_Scroll_1(object sender, EventArgs e)
        {
            double num2 = 0.063;
            double num = tbPulseYThreshold.Value * num2;
            lblPulseYThresholdVal.Text = string.Format("{0:F3}", num);
        }

        private void tbPulseZThreshold_Scroll_1(object sender, EventArgs e)
        {
            double num2 = 0.063;
            double num = tbPulseZThreshold.Value * num2;
            lblPulseZThresholdVal.Text = string.Format("{0:F3}", num);
        }

        private void tbSleepCounter_Scroll(object sender, EventArgs e)
        {
            double num2;
            btnSetSleepTimer.Enabled = true;
            lblSleepTimer.Enabled = true;
            lblSleepTimerValue.Enabled = true;
            lblSleepms.Enabled = true;
            if (ddlDataRate.SelectedIndex != 5)
            {
                num2 = 320.0;
            }
            else
            {
                num2 = 640.0;
            }
            double num = tbSleepCounter.Value * num2;
            if (num > 1000.0)
            {
                lblSleepms.Text = "s";
                num /= 1000.0;
                lblSleepTimerValue.Text = string.Format("{0:F3}", num);
            }
            else
            {
                lblSleepms.Text = "ms";
                lblSleepTimerValue.Text = string.Format("{0:F1}", num);
            }
        }

        private void tbTransDebounce_Scroll_1(object sender, EventArgs e)
        {
            double num = 2.5;
            if (ledSleep.Value)
            {
                num = DRSleep_timestep;
            }
            else
            {
                num = DR_timestep;
            }
            double num2 = tbTransDebounce.Value * num;
            if (num2 > 1000.0)
            {
                lblTransDebouncems.Text = "s";
                num2 /= 1000.0;
                lblTransDebounceVal.Text = string.Format("{0:F2}", num2);
            }
            else
            {
                lblTransDebouncems.Text = "ms";
                lblTransDebounceVal.Text = string.Format("{0:F2}", num2);
            }
        }

        private void tbTransDebounceNEW_Scroll_1(object sender, EventArgs e)
        {
            double num = 2.5;
            if (ledSleep.Value)
            {
                num = DRSleep_timestep;
            }
            else
            {
                num = DR_timestep;
            }
            double num2 = tbTransDebounceNEW.Value * num;
            if (num2 > 1000.0)
            {
                lblTransDebouncemsNEW.Text = "s";
                num2 /= 1000.0;
                lblTransDebounceValNEW.Text = string.Format("{0:F2}", num2);
            }
            else
            {
                lblTransDebouncemsNEW.Text = "ms";
                lblTransDebounceValNEW.Text = string.Format("{0:F1}", num2);
            }
        }

        private void tbTransThreshold_Scroll_1(object sender, EventArgs e)
        {
            double num2 = 0.063;
            double num = tbTransThreshold.Value * num2;
            lblTransThresholdVal.Text = string.Format("{0:F3}", num);
        }

        private void tbTransThresholdNEW_Scroll_1(object sender, EventArgs e)
        {
            double num2 = 0.063;
            double num = tbTransThresholdNEW.Value * num2;
            lblTransThresholdValNEW.Text = string.Format("{0:F3}", num);
        }

        private void tBWatermark_Scroll(object sender, EventArgs e)
        {
            lblWatermarkValue.Text = string.Format("{0:F0}", tBWatermark.Value);
        }

        private void TmrActive_Tick(object sender, EventArgs e)
        {
            if (rdoStandby.Checked)
            {
                ledStandby.Value = true;
                ledWake.Value = false;
                ledSleep.Value = false;
            }
            if (rdoActive.Checked && !btnViewData.Enabled)
            {
                ledStandby.Value = false;
                MainScreenGraph.Enabled = true;
                if (rdo8bitDataMain.Checked)
                {
                    UpdateMainScreenWaveform(Gees8bData);
                }
                else if (rdoXYZFullResMain.Checked)
                {
                    switch (DeviceID)
                    {
                        case 2:
                            UpdateMainScreenWaveform(Gees14bData);
                            break;

                        case 3:
                            UpdateMainScreenWaveform(Gees12bData);
                            break;

                        case 4:
                            UpdateMainScreenWaveform(Gees10bData);
                            break;
                    }
                }
            }
            GUIUpdatePacket guiPacket = null;
            RWLock.AcquireWriterLock(-1);
            try
            {
                if (GuiQueue.Count != 0)
                {
                    guiPacket = (GUIUpdatePacket) GuiQueue.Dequeue();
                }
            }
            finally
            {
                RWLock.ReleaseWriterLock();
            }
            if (guiPacket != null)
            {
                if (guiPacket.TaskID == 0x4a)
                {
                    byte[] buffer = (byte[]) guiPacket.PayLoad.Dequeue();
                    double num = 0.0;
                    double num2 = 0.0;
                    if (buffer[0] == 1)
                    {
                        ledStandby.Value = true;
                        num = DR_timestep;
                        num2 = DR_PulseTimeStepNoLPF;
                    }
                    else
                    {
                        ledStandby.Value = false;
                    }
                    if (buffer[1] == 1)
                    {
                        ledWake.Value = true;
                        num = DR_timestep;
                        num2 = DR_PulseTimeStepNoLPF;
                    }
                    else
                    {
                        ledWake.Value = false;
                    }
                    if (buffer[2] == 1)
                    {
                        ledSleep.Value = true;
                        num = DRSleep_timestep;
                        num2 = DRSleep_PulseTimeStepNoLPF;
                    }
                    else
                    {
                        ledSleep.Value = false;
                    }
                    double num3 = tbMFF1Debounce.Value * num;
                    if (num3 > 1000.0)
                    {
                        lblMFF1Debouncems.Text = "s";
                        num3 /= 1000.0;
                        lblMFF1DebounceVal.Text = string.Format("{0:F2}", num3);
                    }
                    else
                    {
                        lblMFF1Debouncems.Text = "ms";
                        lblMFF1DebounceVal.Text = string.Format("{0:F2}", num3);
                    }
                    double num4 = tbPL.Value * num;
                    if (num4 > 1000.0)
                    {
                        lblBouncems.Text = "s";
                        num4 /= 1000.0;
                        lblPLbounceVal.Text = string.Format("{0:F4}", num4);
                    }
                    else
                    {
                        lblBouncems.Text = "ms";
                        lblPLbounceVal.Text = string.Format("{0:F2}", num4);
                    }
                    double num5 = tbTransDebounce.Value * num;
                    if (num5 > 1000.0)
                    {
                        lblTransDebouncems.Text = "s";
                        num5 /= 1000.0;
                        lblTransDebounceVal.Text = string.Format("{0:F2}", num5);
                    }
                    else
                    {
                        lblTransDebouncems.Text = "ms";
                        lblTransDebounceVal.Text = string.Format("{0:F2}", num5);
                    }
                    if (chkPulseLPFEnable.Checked)
                    {
                        pulse_step = num;
                    }
                    else
                    {
                        pulse_step = num2;
                    }
                    double num6 = tbFirstPulseTimeLimit.Value * pulse_step;
                    if (num6 > 1000.0)
                    {
                        lblFirstPulseTimeLimitms.Text = "s";
                        num6 /= 1000.0;
                        lblFirstTimeLimitVal.Text = string.Format("{0:F3}", num6);
                    }
                    else
                    {
                        lblFirstPulseTimeLimitms.Text = "ms";
                        lblFirstTimeLimitVal.Text = string.Format("{0:F3}", num6);
                    }
                    double num7 = pulse_step * 2.0;
                    double num8 = tbPulseLatency.Value * num7;
                    if (num8 > 1000.0)
                    {
                        lblPulseLatencyms.Text = "s";
                        num8 /= 1000.0;
                        lblPulseLatencyVal.Text = string.Format("{0:F2}", num8);
                    }
                    else
                    {
                        lblPulseLatencyms.Text = "ms";
                        lblPulseLatencyVal.Text = string.Format("{0:F2}", num8);
                    }
                    double num9 = pulse_step * 2.0;
                    double num10 = tbPulse2ndPulseWin.Value * num9;
                    if (num10 > 1000.0)
                    {
                        lblPulse2ndPulseWinms.Text = "s";
                        num10 /= 1000.0;
                        lblPulse2ndPulseWinVal.Text = string.Format("{0:F2}", num10);
                    }
                    else
                    {
                        lblPulse2ndPulseWinms.Text = "ms";
                        lblPulse2ndPulseWinVal.Text = string.Format("{0:F2}", num10);
                    }
                }
                switch (TabTool.SelectedIndex)
                {
                    case 0:
                        if (guiPacket.TaskID == 12)
                        {
                            GUI_SetCal(guiPacket);
                        }
                        if (guiPacket.TaskID == 15)
                        {
                            GUI_SetUpInterrupts(guiPacket);
                        }
                        break;

                    case 1:
                        GUI_RegistersScreen(guiPacket);
                        break;

                    case 2:
                        GUI_DataConfigScreen(guiPacket);
                        break;

                    case 3:
                        GUI_MFF1_2Screen(guiPacket);
                        break;

                    case 4:
                        GUI_ReturningPLStatus(guiPacket);
                        break;

                    case 5:
                        GUI_TransientDetectScreen(guiPacket);
                        break;

                    case 6:
                        GUI_PulseDetectScreen(guiPacket);
                        break;

                    case 7:
                        GUI_FIFOScreen(guiPacket);
                        break;
                }
            }
            UpdateCommStrip();
        }

        private void tmrDataDisplay_Tick(object sender, EventArgs e)
        {
            RWLockDispValues.AcquireWriterLock(-1);
            try
            {
                if (rdo8bitDataMain.Checked)
                {
                    lblX8bC.Text = DisplayXYZ8bit.XAxis.ToString();
                    lblY8bC.Text = DisplayXYZ8bit.YAxis.ToString();
                    lblZ8bC.Text = DisplayXYZ8bit.ZAxis.ToString();
                    lblX8bG.Text = string.Format("{0:F3}", Gees8bData.XAxis);
                    lblY8bG.Text = string.Format("{0:F3}", Gees8bData.YAxis);
                    lblZ8bG.Text = string.Format("{0:F3}", Gees8bData.ZAxis);
                }
                else
                {
                    switch (DeviceID)
                    {
                        case 2:
                            lblX12bC.Text = DisplayXYZ14bit.XAxis.ToString();
                            lblY12bC.Text = DisplayXYZ14bit.YAxis.ToString();
                            lblZ12bC.Text = DisplayXYZ14bit.ZAxis.ToString();
                            lblX12bG.Text = string.Format("{0:F3}", Gees14bData.XAxis);
                            lblY12bG.Text = string.Format("{0:F3}", Gees14bData.YAxis);
                            lblZ12bG.Text = string.Format("{0:F3}", Gees14bData.ZAxis);
                            return;

                        case 3:
                            lblX12bC.Text = DisplayXYZ12bit.XAxis.ToString();
                            lblY12bC.Text = DisplayXYZ12bit.YAxis.ToString();
                            lblZ12bC.Text = DisplayXYZ12bit.ZAxis.ToString();
                            lblX12bG.Text = string.Format("{0:F3}", Gees12bData.XAxis);
                            lblY12bG.Text = string.Format("{0:F3}", Gees12bData.YAxis);
                            lblZ12bG.Text = string.Format("{0:F3}", Gees12bData.ZAxis);
                            return;

                        case 4:
                            lblX12bC.Text = DisplayXYZ10bit.XAxis.ToString();
                            lblY12bC.Text = DisplayXYZ10bit.YAxis.ToString();
                            lblZ12bC.Text = DisplayXYZ10bit.ZAxis.ToString();
                            lblX12bG.Text = string.Format("{0:F3}", Gees10bData.XAxis);
                            lblY12bG.Text = string.Format("{0:F3}", Gees10bData.YAxis);
                            lblZ12bG.Text = string.Format("{0:F3}", Gees10bData.ZAxis);
                            return;
                    }
                }
            }
            finally
            {
                RWLockDispValues.ReleaseWriterLock();
            }
        }

        private void trackBarPL_Scroll_1(object sender, EventArgs e)
        {
            double num = 2.5;
            if (ledSleep.Value)
            {
                num = DRSleep_timestep;
            }
            else
            {
                num = DR_timestep;
            }
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
            { }

            if (invalid == CommunicationState.HWFind)
                CommMode = eCommMode.FindingHW;
            else if (invalid == CommunicationState.Idle)
                CommMode = eCommMode.Closed;
            else if (invalid == CommunicationState.Ready)
                CommMode = eCommMode.Running;
            else
                CommMode = eCommMode.Closed;
            
			toolStripStatusLabel.Text = commStatus;

            if (CommMode == eCommMode.FindingHW)
            {
                CommStripButton.Enabled = true;
                CommStripButton.Image = ImageYellow;
            }
            else if (CommMode == eCommMode.Running)
            {
                CommStripButton.Enabled = true;
                CommStripButton.Image = ImageGreen;
            }
            else if (CommMode == eCommMode.Closed)
            {
                CommStripButton.Enabled = true;
                CommStripButton.Image = ImageRed;
            }
            CommStrip.Refresh();
        }

        private void UpdateMainScreenWaveform(XYZGees xyzData)
        {
            WaveformPlotCollection plots = MainScreenGraph.Plots;
            plots[0].PlotYAppend(xyzData.XAxis);
            plots[1].PlotYAppend(xyzData.YAxis);
            plots[2].PlotYAppend(xyzData.ZAxis);
            MainScreenGraph.Update();
        }

        public void UpdateRaw10bVariables(object counts10bData)
        {
            RWLockDispValues.AcquireWriterLock(-1);
            try
            {
                Count10bData = (XYZCounts) counts10bData;
                DisplayXYZ10bit.XAxis = Count10bData.XAxis;
                if ((DisplayXYZ10bit.XAxis & 0x200) == 0x200)
                {
                    DisplayXYZ10bit.XAxis = ~DisplayXYZ10bit.XAxis + 1;
                    DisplayXYZ10bit.XAxis &= 0x3ff;
                    DisplayXYZ10bit.XAxis *= -1;
                }
                DisplayXYZ10bit.YAxis = Count10bData.YAxis;
                if ((DisplayXYZ10bit.YAxis & 0x200) == 0x200)
                {
                    DisplayXYZ10bit.YAxis = ~DisplayXYZ10bit.YAxis + 1;
                    DisplayXYZ10bit.YAxis &= 0x3ff;
                    DisplayXYZ10bit.YAxis *= -1;
                }
                DisplayXYZ10bit.ZAxis = Count10bData.ZAxis;
                if ((DisplayXYZ10bit.ZAxis & 0x200) == 0x200)
                {
                    DisplayXYZ10bit.ZAxis = ~DisplayXYZ10bit.ZAxis + 1;
                    DisplayXYZ10bit.ZAxis &= 0x3ff;
                    DisplayXYZ10bit.ZAxis *= -1;
                }
                if (FullScaleValue == 0)
                {
                    Gees10bData.XAxis = ((double) DisplayXYZ10bit.XAxis) / 256.0;
                    Gees10bData.YAxis = ((double) DisplayXYZ10bit.YAxis) / 256.0;
                    Gees10bData.ZAxis = ((double) DisplayXYZ10bit.ZAxis) / 256.0;
                }
                if (FullScaleValue == 1)
                {
                    Gees10bData.XAxis = ((double) DisplayXYZ10bit.XAxis) / 128.0;
                    Gees10bData.YAxis = ((double) DisplayXYZ10bit.YAxis) / 128.0;
                    Gees10bData.ZAxis = ((double) DisplayXYZ10bit.ZAxis) / 128.0;
                }
                if (FullScaleValue == 2)
                {
                    Gees10bData.XAxis = ((double) DisplayXYZ10bit.XAxis) / 64.0;
                    Gees10bData.YAxis = ((double) DisplayXYZ10bit.YAxis) / 64.0;
                    Gees10bData.ZAxis = ((double) DisplayXYZ10bit.ZAxis) / 64.0;
                }
            }
            finally
            {
                RWLockDispValues.ReleaseWriterLock();
            }
        }

        public void UpdateRaw12bVariables(object counts12bData)
        {
            RWLockDispValues.AcquireWriterLock(-1);
            try
            {
                Count12bData = (XYZCounts) counts12bData;
                DisplayXYZ12bit.XAxis = Count12bData.XAxis;
                if ((DisplayXYZ12bit.XAxis & 0x800) == 0x800)
                {
                    DisplayXYZ12bit.XAxis = ~DisplayXYZ12bit.XAxis + 1;
                    DisplayXYZ12bit.XAxis &= 0xfff;
                    DisplayXYZ12bit.XAxis *= -1;
                }
                DisplayXYZ12bit.YAxis = Count12bData.YAxis;
                if ((DisplayXYZ12bit.YAxis & 0x800) == 0x800)
                {
                    DisplayXYZ12bit.YAxis = ~DisplayXYZ12bit.YAxis + 1;
                    DisplayXYZ12bit.YAxis &= 0xfff;
                    DisplayXYZ12bit.YAxis *= -1;
                }
                DisplayXYZ12bit.ZAxis = Count12bData.ZAxis;
                if ((DisplayXYZ12bit.ZAxis & 0x800) == 0x800)
                {
                    DisplayXYZ12bit.ZAxis = ~DisplayXYZ12bit.ZAxis + 1;
                    DisplayXYZ12bit.ZAxis &= 0xfff;
                    DisplayXYZ12bit.ZAxis *= -1;
                }
                if (FullScaleValue == 0)
                {
                    Gees12bData.XAxis = ((double) DisplayXYZ12bit.XAxis) / 1024.0;
                    Gees12bData.YAxis = ((double) DisplayXYZ12bit.YAxis) / 1024.0;
                    Gees12bData.ZAxis = ((double) DisplayXYZ12bit.ZAxis) / 1024.0;
                }
                if (FullScaleValue == 1)
                {
                    Gees12bData.XAxis = ((double) DisplayXYZ12bit.XAxis) / 512.0;
                    Gees12bData.YAxis = ((double) DisplayXYZ12bit.YAxis) / 512.0;
                    Gees12bData.ZAxis = ((double) DisplayXYZ12bit.ZAxis) / 512.0;
                }
                if (FullScaleValue == 2)
                {
                    Gees12bData.XAxis = ((double) DisplayXYZ12bit.XAxis) / 256.0;
                    Gees12bData.YAxis = ((double) DisplayXYZ12bit.YAxis) / 256.0;
                    Gees12bData.ZAxis = ((double) DisplayXYZ12bit.ZAxis) / 256.0;
                }
            }
            finally
            {
                RWLockDispValues.ReleaseWriterLock();
            }
        }

        public void UpdateRaw14bVariables(object counts14bData)
        {
            RWLockDispValues.AcquireWriterLock(-1);
            try
            {
                Count14bData = (XYZCounts) counts14bData;
                DisplayXYZ14bit.XAxis = Count14bData.XAxis;
                if ((DisplayXYZ14bit.XAxis & 0x2000) == 0x2000)
                {
                    DisplayXYZ14bit.XAxis = ~DisplayXYZ14bit.XAxis + 1;
                    DisplayXYZ14bit.XAxis &= 0x3fff;
                    DisplayXYZ14bit.XAxis *= -1;
                }
                DisplayXYZ14bit.YAxis = Count14bData.YAxis;
                if ((DisplayXYZ14bit.YAxis & 0x2000) == 0x2000)
                {
                    DisplayXYZ14bit.YAxis = ~DisplayXYZ14bit.YAxis + 1;
                    DisplayXYZ14bit.YAxis &= 0x3fff;
                    DisplayXYZ14bit.YAxis *= -1;
                }
                DisplayXYZ14bit.ZAxis = Count14bData.ZAxis;
                if ((DisplayXYZ14bit.ZAxis & 0x2000) == 0x2000)
                {
                    DisplayXYZ14bit.ZAxis = ~DisplayXYZ14bit.ZAxis + 1;
                    DisplayXYZ14bit.ZAxis &= 0x3fff;
                    DisplayXYZ14bit.ZAxis *= -1;
                }
                if (FullScaleValue == 0)
                {
                    Gees14bData.XAxis = ((double) DisplayXYZ14bit.XAxis) / 4096.0;
                    Gees14bData.YAxis = ((double) DisplayXYZ14bit.YAxis) / 4096.0;
                    Gees14bData.ZAxis = ((double) DisplayXYZ14bit.ZAxis) / 4096.0;
                }
                if (FullScaleValue == 1)
                {
                    Gees14bData.XAxis = ((double) DisplayXYZ14bit.XAxis) / 2048.0;
                    Gees14bData.YAxis = ((double) DisplayXYZ14bit.YAxis) / 2048.0;
                    Gees14bData.ZAxis = ((double) DisplayXYZ14bit.ZAxis) / 2048.0;
                }
                if (FullScaleValue == 2)
                {
                    Gees14bData.XAxis = ((double) DisplayXYZ14bit.XAxis) / 1024.0;
                    Gees14bData.YAxis = ((double) DisplayXYZ14bit.YAxis) / 1024.0;
                    Gees14bData.ZAxis = ((double) DisplayXYZ14bit.ZAxis) / 1024.0;
                }
            }
            finally
            {
                RWLockDispValues.ReleaseWriterLock();
            }
        }

        public void UpdateRaw8bVariables(object counts8bData)
        {
            RWLockDispValues.AcquireWriterLock(-1);
            try
            {
                Count8bData = (XYZCounts) counts8bData;
                DisplayXYZ8bit.XAxis = Count8bData.XAxis;
                if ((DisplayXYZ8bit.XAxis & 0x80) == 0x80)
                {
                    DisplayXYZ8bit.XAxis = ~DisplayXYZ8bit.XAxis + 1;
                    DisplayXYZ8bit.XAxis &= 0xff;
                    DisplayXYZ8bit.XAxis *= -1;
                }
                DisplayXYZ8bit.YAxis = Count8bData.YAxis;
                if ((DisplayXYZ8bit.YAxis & 0x80) == 0x80)
                {
                    DisplayXYZ8bit.YAxis = ~DisplayXYZ8bit.YAxis + 1;
                    DisplayXYZ8bit.YAxis &= 0xff;
                    DisplayXYZ8bit.YAxis *= -1;
                }
                DisplayXYZ8bit.ZAxis = Count8bData.ZAxis;
                if ((DisplayXYZ8bit.ZAxis & 0x80) == 0x80)
                {
                    DisplayXYZ8bit.ZAxis = ~DisplayXYZ8bit.ZAxis + 1;
                    DisplayXYZ8bit.ZAxis &= 0xff;
                    DisplayXYZ8bit.ZAxis *= -1;
                }
                if (FullScaleValue == 0)
                {
                    Gees8bData.XAxis = ((double) DisplayXYZ8bit.XAxis) / 64.0;
                    Gees8bData.YAxis = ((double) DisplayXYZ8bit.YAxis) / 64.0;
                    Gees8bData.ZAxis = ((double) DisplayXYZ8bit.ZAxis) / 64.0;
                }
                if (FullScaleValue == 1)
                {
                    Gees8bData.XAxis = ((double) DisplayXYZ8bit.XAxis) / 32.0;
                    Gees8bData.YAxis = ((double) DisplayXYZ8bit.YAxis) / 32.0;
                    Gees8bData.ZAxis = ((double) DisplayXYZ8bit.ZAxis) / 32.0;
                }
                if (FullScaleValue == 2)
                {
                    Gees8bData.XAxis = ((double) DisplayXYZ8bit.XAxis) / 16.0;
                    Gees8bData.YAxis = ((double) DisplayXYZ8bit.YAxis) / 16.0;
                    Gees8bData.ZAxis = ((double) DisplayXYZ8bit.ZAxis) / 16.0;
                }
            }
            finally
            {
                RWLockDispValues.ReleaseWriterLock();
            }
        }

        private void UpdateWaveformMFF(XYZGees xyzData)
        {
            WaveformPlotCollection plots = MFFGraph.Plots;
            plots[0].PlotYAppend(xyzData.XAxis);
            plots[1].PlotYAppend(xyzData.YAxis);
            plots[2].PlotYAppend(xyzData.ZAxis);
            MFFGraph.Update();
        }

        private void UpdateWaveformTrans(XYZGees xyzData)
        {
            WaveformPlotCollection plots = gphXYZ.Plots;
            plots[0].PlotYAppend(xyzData.XAxis);
            plots[1].PlotYAppend(xyzData.YAxis);
            plots[2].PlotYAppend(xyzData.ZAxis);
            gphXYZ.Update();
        }

        private void VeyronEvaluationSoftware_FormClosing(object sender, FormClosingEventArgs e)
        {
            VeyronControllerObj.DisableData();
        }
    }
}

