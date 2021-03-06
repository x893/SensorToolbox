﻿using Freescale.SASD.Communication;
using GlobalSTB;
using LCDLabel;
using MMA845x_DEMO.Properties;
using NationalInstruments.UI;
using NationalInstruments.UI.WindowsForms;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace MMA845xQEvaluation
{
	public class VeyronEvaluationSoftware8451 : Form
	{
		public deviceID DeviceID = deviceID.Unsupported;

		#region Privates 

		private int _averageCounter;
		private Dictionary<deviceID, string> _registerMapFile;
		private RegisterView _registerView;
		private object ControllerEventsLock = new object();
		private string CurrentFW = "4003";
		private double DR_PulseTimeStepNoLPF;
		private double DR_timestep;
		private double DRSleep_PulseTimeStepNoLPF;
		private double DRSleep_timestep;

		private Button btnAutoCal;
		private Button btnMFF1Reset;
		private Button btnMFF1Set;
		private Button btnPulseResetTime2ndPulse;
		private Button btnPulseSetTime2ndPulse;
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
		private Button btnWatermark;
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
		private CheckBox chkEnableYAxis;
		private CheckBox chkEnableYMFF;
		private CheckBox chkEnableZAxis;
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
		private IContainer components;
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
		private BoardComm dv;
		private int FIFODump8orFull;
		private int FIFOModeValue;
		private TabPage FIFOPage;
		private bool FirstTimeLoad = true;
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
		private GroupBox groupBox2;
		private GroupBox groupBox3;
		private GroupBox groupBox5;
		private GroupBox groupBox6;
		private GroupBox groupBox7;
		private GroupBox groupBox8;
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
		private Label label13;
		private Label label14;
		private Label label15;
		private Label label17;
		private Label label172;
		private Label lbsStandby;
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
		private Label label22;
		private Label label23;
		private Label label230;
		private Label label231;
		private Label label232;
		private Label label233;
		private Label label236;
		private Label label238;
		private Label label239;
		private Label label24;
		private Label label255;
		private Label label256;
		private Label label258;
		private Label label263;
		private Label label264;
		private Label label265;
		private Label label266;
		private Label label268;
		private Label label273;
		private Label label275;
		private Label label277;
		private Label label279;
		private Label label281;
		private Label label283;
		private Label label285;
		private Label label287;
		private Label label3;
		private Label label34;
		private Label label35;
		private Label label36;
		private Label label37;
		private Label label38;
		private Label label4;
		private Label label43;
		private Label label44;
		private Label label45;
		private Label label46;
		private Label label47;
		private Label label5;
		private Label label6;
		private Label label61;
		private Label label62;
		private Label label63;
		private Label label64;
		private Label label65;
		private Label label67;
		private Label label68;
		private Label label69;
		private Label label7;
		private Label label70;
		private Label lbsWake;
		private Label lbl2gSensitivity;
		private Label lbl4gSensitivity;
		private Label lbl8gSensitivity;
		private Label lblBouncems;
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
		private Label lblMFFDataBit;
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
		private Label lblSleepms;
		private Label lblSleepTimer;
		private Label lblSleepTimerValue;
		private Label lblTransDataBit;
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
		private Label lblXCal;
		private Label lblXDrdy;
		private Label lblXHP;
		private Label lblXOW;
		private Label lblXYZDrdy;
		private Label lblXYZOW;
		private Label lblYCal;
		private Label lblYDrdy;
		private Label lblYHP;
		private Label lblYOW;
		private Label lblZCal;
		private Label lblZDrdy;
		private Label lblZHP;
		private Label lblZOW;
		private Label lbsSleep;
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
		private bool LoadingFW = false;
		private TabPage MainScreenEval;
		private WaveformGraph MainScreenGraph;
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
		private PictureBox pictureBox2;
		private int PL_index;
		private TabPage PL_Page;
		private int PLAngle;
		private PictureBox PLImage;
		private Panel pnlAutoSleep;
		private Panel pOverSampling;
		private Panel pPLActive;
		private GroupBox pSOS;
		private Panel pTrans2;
		private Panel pTransNEW;
		private Panel pTriggerMode;
		private double pulse_step;
		private TabPage PulseDetection;
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
		private RadioButton rdoDisabled;
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
		private static ReaderWriterLock RWLock = new ReaderWriterLock();
		private static ReaderWriterLock RWLockDispValues = new ReaderWriterLock();
		private static ReaderWriterLock RWLockGetCalValues = new ReaderWriterLock();
		private int SleepOSMode;
		private int SystemPolling;
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
		private TextBox txtSaveFileName;
		private WaveformPlot uxAccelX;
		private LcdLabel uxAccelXValue;
		private WaveformPlot uxAccelY;
		private LcdLabel uxAccelYValue;
		private WaveformPlot uxAccelZ;
		private LcdLabel uxAccelZValue;
		private ComboBox uxAverageData;
		private NationalInstruments.UI.YAxis uxAxisAccel;
		private NationalInstruments.UI.YAxis uxAxisCounts;
		private NationalInstruments.UI.XAxis uxAxisSamples;
		private Button uxClearFifoDataList;
		private Button uxCopyFifoData;
		private ListView uxFifoList;
		private Panel uxPanelSettings;
		private CheckBox uxStart;
		private Switch uxSwitchCounts;
		private ColumnHeader uxXFifo;
		private ColumnHeader uxYFifo;
		private ColumnHeader uxZFifo;
		private AccelController VeyronControllerObj;
		private int WakeOSMode;
		private WaveformPlot waveformPlot1;
		private WaveformPlot waveformPlot2;
		private WaveformPlot waveformPlot3;
		private WaveformPlot XAxis;
		private NationalInstruments.UI.XAxis xAxis1;
		private NationalInstruments.UI.XAxis xAxis2;
		private WaveformPlot YAxis;
		private NationalInstruments.UI.YAxis yAxis1;
		private NationalInstruments.UI.YAxis yAxis2;
		private WaveformPlot ZAxis;
		#endregion

		public VeyronEvaluationSoftware8451(object accelController)
		{
			Dictionary<deviceID, string> dictionary = new Dictionary<deviceID, string>();

			dictionary.Add(deviceID.MMA8451Q,
			#region
@"0x00	STATUS/F_STATUS(1)(2)	F_MODE = 00: 0x00 STATUS: Data Status Register (Read Only)	R	6	FMODE = 0, real time status FMODE > 0, FIFO status | 	ZYXOW,ZOW,YOW,XOW,ZYXDR,ZDR,YDR,XDR
0x01	OUT_X_MSB(1)(2)	0x01 OUT_X_MSB: X_MSB Register (Read Only)	R	6	[7:0] are 8 MSBs of 14-bit sample. | Root pointer to XYZ FIFO data.	XD13,XD12,XD11,XD10,XD9,XD8,XD7,XD6
0x02	OUT_X_LSB(1)(2)	0x02 OUT_X_LSB: X_LSB Register (Read Only)	R	6	[7:2] are 6 LSBs of 14-bit real-time sample | 	XD5,XD4,XD3,XD2,XD1,XD0,0,0
0x03	OUT_Y_MSB(1)(2)	0x03 OUT_Y_MSB: Y_MSB Register (Read Only)	R	6	[7:0] are 8 MSBs of 14-bit real-time sample | 	YD13,YD12,YD11,YD10,YD9,YD8,YD7,YD6
0x04	OUT_Y_LSB(1)(2)	0x04 OUT_Y_LSB: Y_LSB Register (Read Only)	R	6	[7:2] are 6 LSBs of 14-bit real-time sample | 	YD5,YD4,YD3,YD2,YD1,YD0,0,0
0x05	OUT_Z_MSB(1)(2)	0x05 OUT_Z_MSB: Z_MSB Register (Read Only)	R	6	[7:0] are 8 MSBs of 14-bit real-time sample | 	ZD13,ZD12,ZD11,ZD10,ZD9,ZD8,ZD7,ZD6
0x06	OUT_Z_LSB(1)(2)	0x06 OUT_Z_LSB: Z_LSB Register (Read Only)	R	6	[7:2] are 6 LSBs of 14-bit real-time sample | 	ZD5,ZD4,ZD3,ZD2,ZD1,ZD0,0,0
0x07	Reserved	0x06 OUT_Z_LSB: Z_LSB Register (Read Only)	R	0	Reserved. Read return 0x00. | 	
0x08	Reserved	0x06 OUT_Z_LSB: Z_LSB Register (Read Only)	R	0	Reserved. Read return 0x00. | 	
0x09	F_SETUP(1)(3)	0x09 F_SETUP: FIFO Set-up Register (Read/Write)	R/W	10	FIFO set-up | 	F_MODE1,F_MODE0,F_WMRK5,F_WMRK4,F_WMRK3,F_WMRK2,F_WMRK1,F_WMRK0
0x0A	TRIG_CFG(1)(4)	0x0A: TRIG_CFG Trigger Configuration Register (Read/Write)	R/W	18	Map of FIFO data capture events | 	—,—,Trig_TRANS,Trig_LNDPRT,Trig_PULSE,Trig_FF_MT,—,—
0x0B	SYSMOD(1)(2)	0x0B SYSMOD: System Mode Register (Read Only)	R	6	Current System Mode | 	FGERR,FGT_4,FGT_3,FGT_2,FGT_1,FGT_0,SYSMOD1,SYSMOD0
0x0C	INT_SOURCE(1)(2)	0x0C INT_SOURCE: System Interrupt Status Register (Read Only)	R	6	Interrupt status | 	SRC_ASLP,SRC_FIFO,SRC_TRANS,SRC_LNDPRT,SRC_PULSE,SRC_FF_MT,—,SRC_DRDY
0x0D	WHO_AM_I(1)	0x0D: WHO_AM_I Device ID Register (Read Only)	R	2	Device ID (0x1A) | 	0,0,0,1,1,0,1,0
0x0E	XYZ_DATA_CFG(1)(4)	0x0E: XYZ_DATA_CFG (Read/Write)	R/W	18	Dynamic Range Settings | 	0,0,0,HPF_OUT,0,0,FS1,FS0
0x0F	HP_FILTER_CUTOFF(1)(4)	0x0F HP_FILTER_CUTOFF: High Pass Filter Register (Read/Write)	R/W	18	Cut-off frequency is set to 16 Hz @ 800 Hz | 	0,0,Pulse_HPF_BYP,Pulse_LPF_EN,0,0,SEL1,SEL0
0x10	PL_STATUS(1)(2)	0x10 PL_STATUS Register (Read Only)	R	6	Landscape/Portrait orientation status | 	NEWLP,LO,—,—,—,LAPO[1],LAPO[0],BAFRO
0x11	PL_CFG(1)(4)	0x11 PL_CFG Register (Read/Write)	R/W	18	Landscape/Portrait configuration. | 	DBCNTM,PL_EN,—,—,—,—,—,—
0x12	PL_COUNT(1)(3)	0x12 PL_COUNT Register (Read/Write)	R/W	10	Landscape/Portrait debounce counter | 	DBNCE[7],DBNCE[6],DBNCE[5],DBNCE[4],DBNCE[3],DBNCE[2],DBNCE[1],DBNCE[0]
0x13	PL_BF_ZCOMP(1)(4)	0x13: PL_BF_ZCOMP Register (Read/Write)	R/W	18	Back/Front, Z-Lock Trip threshold | 	BKFR[1],BKFR[0],—,—,—,ZLOCK[2],ZLOCK[1],ZLOCK[0]
0x14	P_L_THS_REG(1)(4)	0x14: P_L_THS_REG Register (Read/Write)	R/W	18	Portrait to Landscape Trip Angle is 29\x00b0 | 	P_L_THS[4],P_L_THS[3],P_L_THS[2],P_L_THS[1],P_L_THS[0],HYS[2],HYS[1],HYS[0]
0x15	FF_MT_CFG(1)(4)	0x15 FF_MT_CFG Register (Read/Write)	R/W	18	Freefall/Motion functional block configuration | 	ELE,OAE,ZEFE,YEFE,XEFE,—,—,—
0x16	FF_MT_SRC(1)(2)	0x16: FF_MT_SRC Freefall and Motion Source Register (Read Only)	R	6	Freefall/Motion event source register | 	EA,—,ZHE,ZHP,YHE,YHP,XHE,XHP
0x17	FF_MT_THS(1)(3)	0x17 FF_MT_THS Register (Read/Write)	R/W	10	Freefall/Motion threshold register | 	DBCNTM,THS6,THS5,THS4,THS3,THS2,THS1,THS0
0x18	FF_MT_COUNT(1)(3)	0x18 FF_MT_COUNT_Register (Read/Write)	R/W	10	Freefall/Motion debounce counter | 	D7,D6,D5,D4,D3,D2,D1,D0
0x19	Reserved	0x18 FF_MT_COUNT_Register (Read/Write)	R	0	Reserved. Read return 0x00. | 	
0x1A	Reserved	0x18 FF_MT_COUNT_Register (Read/Write)	R	0	Reserved. Read return 0x00. | 	
0x1B	Reserved	0x18 FF_MT_COUNT_Register (Read/Write)	R	0	Reserved. Read return 0x00. | 	
0x1C	Reserved	0x18 FF_MT_COUNT_Register (Read/Write)	R	0	Reserved. Read return 0x00. | 	
0x1D	TRANSIENT_CFG(1)(4)	0x1D TRANSIENT_CFG Register (Read/Write)	R/W	18	Transient functional block configuration | 	—,—,—,ELE,ZTEFE,YTEFE,XTEFE,HPF_BYP
0x1E	TRANSIENT_SRC(1)(2)	0x1E TRANSIENT_SRC Register (Read Only)	R	6	Transient event status register | 	—,EA,ZTRANSE,Z_Trans_Pol,YTRANSE,Y_Trans_Pol,XTRANSE,X_Trans_Pol
0x1F	TRANSIENT_THS(1)(3)	0x1F TRANSIENT_THS Register (Read/Write)	R/W	10	Transient event threshold | 	DBCNTM,THS6,THS5,THS4,THS3,THS2,THS1,THS0
0x20	TRANSIENT_COUNT(1)(3)	0x20 TRANSIENT_COUNT Register (Read/Write)	R/W	10	Transient debounce counter | 	D7,D6,D5,D4,D3,D2,D1,D0
0x21	PULSE_CFG(1)(4)	0x21 PULSE_CFG Register (Read/Write)	R/W	18	ELE, Double_XYZ or Single_XYZ | 	DPA,ELE,ZDPEFE,ZSPEFE,YDPEFE,YSPEFE,XDPEFE,XSPEFE
0x22	PULSE_SRC(1)(2)	0x22 PULSE_SRC Register (Read Only)	R	6	EA, Double_XYZ or Single_XYZ | 	EA,AxZ,AxY,AxX,DPE,PolZ,PolY,PolX
0x23	PULSE_THSX(1)(3)	0x23 PULSE_THSX Register (Read/Write)	R/W	10	X pulse threshold | 	0,THSX6,THSX5,THSX4,THSX3,THSX2,THSX1,THSX0
0x24	PULSE_THSY(1)(3)	0x24 PULSE_THSY Register (Read/Write)	R/W	10	Y pulse threshold | 	0,THSY6,THSY5,THSY4,THSY3,THSY2,THSY1,THSY0
0x25	PULSE_THSZ(1)(3)	0x25 PULSE_THSZ Register (Read/Write)	R/W	10	Z pulse threshold | 	0,THSZ6,THSZ5,THSZ4,THSZ3,THSZ2,THSZ1,THSZ0
0x26	PULSE_TMLT(1)(4)	0x26 PULSE_TMLT Register (Read/Write)	R/W	18	Time limit for pulse | 	TMLT7,TMLT6,TMLT5,TMLT4,TMLT3,TMLT2,TMLT1,TMLT0
0x27	PULSE_LTCY(1)(4)	0x27 PULSE_LTCY Register (Read/Write)	R/W	18	Latency time for 2nd pulse | 	LTCY7,LTCY6,LTCY5,LTCY4,LTCY3,LTCY2,LTCY1,LTCY0
0x28	PULSE_WIND(1)(4)	0x28: PULSE_WIND Second Pulse Time Window Register	R/W	18	Window time for 2nd pulse | 	WIND7,WIND6,WIND5,WIND4,WIND3,WIND2,WIND1,WIND0
0x29	ASLP_COUNT(1)(4)	0x29 ASLP_COUNT Register (Read/Write)	R/W	18	Counter setting for Auto-SLEEP | 	D7,D6,D5,D4,D3,D2,D1,D0
0x2A	CTRL_REG1(1)(4)	0x2A CTRL_REG1 Register (Read/Write)	R/W	18	ODR = 800 Hz, STANDBY Mode. | 	ASLP_RATE1,ASLP_RATE0,DR2,DR1,DR0,LNOISE,F_READ,ACTIVE
0x2B	CTRL_REG2(1)(4)	0x2B CTRL_REG2 Register (Read/Write)	R/W	18	Sleep Enable, OS Modes, RST, ST | 	ST,RST,0,SMODS1,SMODS0,SLPE,MODS1,MODS0
0x2C	CTRL_REG3(1)(4)	0x2C CTRL_REG3 Register (Read/Write)	R/W	18	Wake from Sleep, IPOL, PP_OD | 	FIFO_GATE,WAKE_TRANS,WAKE_LNDPRT,WAKE_PULSE,WAKE_FF_MT,—,IPOL,PP_OD
0x2D	CTRL_REG4(1)(4)	0x2D CTRL_REG4 Register (Read/Write)	R/W	18	Interrupt enable register | 	INT_EN_ASLP,INT_EN_FIFO,INT_EN_TRANS,INT_EN_LNDPR,INT_EN_PULSE,INT_EN_FF_MT,—,INT_EN_DRDY
0x2E	CTRL_REG5(1)(4)	0x2E: CTRL_REG5 Interrupt Configuration Register	R/W	18	Interrupt pin (INT1/INT2) map | 	INT_CFG_ASLP,INT_CFG_FIFO,INT_CFG_TRANS,INT_CFG_LNDPRT,INT_CFG_PULSE,INT_CFG_FF_MT,—,INT_CFG_DRDY
0x2F	OFF_X(1)(4)	0x2F OFF_X Register (Read/Write)	R/W	18	X-axis offset adjust | 	D7,D6,D5,D4,D3,D2,D1,D0
0x30	OFF_Y(1)(4)	0x30 OFF_Y Register (Read/Write)	R/W	18	Y-axis offset adjust | 	D7,D6,D5,D4,D3,D2,D1,D0
0x31	OFF_Z(1)(4)	0x31 OFF_Z Register (Read/Write)	R/W	18	Z-axis offset adjust | 	D7,D6,D5,D4,D3,D2,D1,D0
^^
1. Register contents are preserved when transition from ACTIVE to STANDBY mode occurs.
2. Register contents are reset when transition from STANDBY to ACTIVE mode occurs.
3. Register contents can be modified anytime in STANDBY or ACTIVE mode. A write to this register will cause a reset of the corresponding internal system debounce counter.
4. Modification of this register’s contents can only occur when device is STANDBY mode except CTRL_REG1 ACTIVE bit and CTRL_REG2 RST bit"
				#endregion
				);
			dictionary.Add(deviceID.MMA8452Q,
			#region
@"0x00	STATUS(1)(2)	0x00 STATUS: Data Status Register (Read Only)	R	6	Real time status | 	ZYXOW,ZOW,YOW,XOW,ZYXDR,ZDR,YDR,XDR
0x01	OUT_X_MSB(1)(2)	0x01 OUT_X_MSB: X_MSB Register (Read Only)	R	6	[7:0] are 8 MSBs of 12-bit sample. | 	XD11,XD10,XD9,XD8,XD7,XD6,XD5,XD4
0x02	OUT_X_LSB(1)(2)	0x02 OUT_X_LSB: X_LSB Register (Read Only)	R	6	[7:4] are 4 LSBs of 12-bit sample. | 	XD3,XD2,XD1,XD0,0,0,0,0
0x03	OUT_Y_MSB(1)(2)	0x03 OUT_Y_MSB: Y_MSB Register (Read Only)	R	6	[7:0] are 8 MSBs of 12-bit sample. | 	YD11,YD10,YD9,YD8,YD7,YD6,YD5,YD4
0x04	OUT_Y_LSB(1)(2)	0x04 OUT_Y_LSB: Y_LSB Register (Read Only)	R	6	[7:4] are 4 LSBs of 12-bit sample. | 	YD3,YD2,XD1,XD0,0,0,0,0
0x05	OUT_Z_MSB(1)(2)	0x05 OUT_Z_MSB: Z_MSB Register (Read Only)	R	6	[7:0] are 8 MSBs of 12-bit sample. | 	ZD11,ZD10,ZD9,ZD8,ZD7,ZD6,ZD5,ZD4
0x06	OUT_Z_LSB(1)(2)	0x06 OUT_Z_LSB: Z_LSB Register (Read Only)	R	6	[7:4] are 4 LSBs of 12-bit sample. | 	ZD3,ZD2,ZD1,ZD0,0,0,0,0
0x07	Reserved	0x06 OUT_Z_LSB: Z_LSB Register (Read Only)	R	0	Reserved. Read return 0x00. | 	
0x08	Reserved	0x06 OUT_Z_LSB: Z_LSB Register (Read Only)	R	0	Reserved. Read return 0x00. | 	
0x0B	SYSMOD(1)(2)	0x0B SYSMOD: System Mode Register (Read Only)	R	6	Current System Mode | 	0,0,0,0,0,0,SYSMOD1,SYSMOD0
0x0C	INT_SOURCE(1)(2)	0x0C INT_SOURCE: System Interrupt Status Register (Read Only)	R	6	Interrupt status | 	SRC_ASLP,0,SRC_TRANS,SRC_LNDPRT,SRC_PULSE,SRC_FF_MT,0,SRC_DRDY
0x0D	WHO_AM_I(1)	0x0D: WHO_AM_I Device ID Register (Read Only)	R	2	Device ID (0x2A) | 	0,0,1,0,1,0,1,0
0x0E	XYZ_DATA_CFG(1)(3)	0x0E: XYZ_DATA_CFG (Read/Write)	R/W	10	HPF Data Out and Dynamic Range Settings | 	0,0,0,HPF_OUT,0,0,FS1,FS0
0x0F	HP_FILTER_CUTOFF(1)(3)	0x0F HP_FILTER_CUTOFF: High Pass Filter Register (Read/Write)	R/W	10	Cut-off frequency is set to 16 Hz @ 800 Hz | 	0,0,Pulse_HPF_BYP,Pulse_LPF_EN,0,0,SEL1,SEL0
0x10	PL_STATUS(1)(2)	0x10 PL_STATUS Register (Read Only)	R	6	Landscape/Portrait orientation status | 	NEWLP,LO,0,0,0,LAPO[1],LAPO[0],BAFRO
0x11	PL_CFG(1)(3)	0x11 PL_CFG Register (Read/Write)	R/W	10	Landscape/Portrait configuration. | 	DBCNTM,PL_EN,0,0,0,0,0,0
0x12	PL_COUNT(1)(3)	0x12 PL_COUNT Register (Read/Write)	R/W	10	Landscape/Portrait debounce counter | 	DBNCE[7],DBNCE[6],DBNCE[5],DBNCE[4],DBNCE[3],DBNCE[2],DBNCE[1],DBNCE[0]
0x13	PL_BF_ZCOMP(1)(3)	0x13: PL_BF_ZCOMP Register (Read/Write)	R/W	10	Back-Front, Z-Lock Trip threshold | 	BKFR[1],BKFR[0],0,0,0,ZLOCK[2],ZLOCK[1],ZLOCK[0]
0x14	P_L_THS_REG(1)(3)	0x14: P_L_THS_REG Register (Read/Write)	R/W	10	Portrait to Landscape Trip Angle is 29\x00b0 | 	P_L_THS[4],P_L_THS[3],P_L_THS[2],P_L_THS[1],P_L_THS[0],HYS[2],HYS[1],HYS[0]
0x15	FF_MT_CFG(1)(3)	0x15 FF_MT_CFG Register (Read/Write)	R/W	10	Freefall/Motion functional block configuration | 	ELE,OAE,ZEFE,YEFE,XEFE,0,0,0
0x16	FF_MT_SRC(1)(2)	0x16: FF_MT_SRC Freefall and Motion Source Register (Read Only)	R	6	Freefall/Motion event source register | 	EA,0,ZHE,ZHP,YHE,YHP,XHE,XHP
0x17	FF_MT_THS(1)(3)	0x17 FF_MT_THS Register (Read/Write)	R/W	10	Freefall/Motion threshold register | 	DBCNTM,THS6,THS5,THS4,THS3,THS2,THS1,THS0
0x18	FF_MT_COUNT(1)(3)	0x18 FF_MT_COUNT_Register (Read/Write)	R/W	10	Freefall/Motion debounce counter | 	D7,D6,D5,D4,D3,D2,D1,D0
0x19	Reserved	0x18 FF_MT_COUNT_Register (Read/Write)	R	0	Reserved. Read return 0x00. | 	
0x1A	Reserved	0x18 FF_MT_COUNT_Register (Read/Write)	R	0	Reserved. Read return 0x00. | 	
0x1B	Reserved	0x18 FF_MT_COUNT_Register (Read/Write)	R	0	Reserved. Read return 0x00. | 	
0x1C	Reserved	0x18 FF_MT_COUNT_Register (Read/Write)	R	0	Reserved. Read return 0x00. | 	
0x1D	TRANSIENT_CFG(1)(3)	0x1D TRANSIENT_CFG Register (Read/Write)	R/W	10	Transient functional block configuration | 	0,0,0,ELE,ZTEFE,YTEFE,XTEFE,HPF_BYP
0x1E	TRANSIENT_SRC(1)(2)	0x1E TRANSIENT_SRC Register (Read Only)	R	6	Transient event status register | 	0,EA,ZTRANSE,Z_Trans_Pol,YTRANSE,Y_Trans_Pol,XTRANSE,X_Trans_Pol
0x1F	TRANSIENT_THS(1)(3)	0x1F TRANSIENT_THS Register (Read/Write)	R/W	10	Transient event threshold | 	DBCNTM,THS6,THS5,THS4,THS3,THS2,THS1,THS0
0x20	TRANSIENT_COUNT(1)(3)	0x20 TRANSIENT_COUNT Register (Read/Write)	R/W	10	Transient debounce counter | 	D7,D6,D5,D4,D3,D2,D1,D0
0x21	PULSE_CFG(1)(3)	0x21 PULSE_CFG Register (Read/Write)	R/W	10	ELE, Double_XYZ or Single_XYZ | 	DPA,ELE,ZDPEFE,ZSPEFE,YDPEFE,YSPEFE,XDPEFE,XSPEFE
0x22	PULSE_SRC(1)(2)	0x22 PULSE_SRC Register (Read Only)	R	6	EA, Double_XYZ or Single_XYZ | 	EA,AxZ,AxY,AxX,DPE,PolZ,PolY,PolX
0x23	PULSE_THSX(1)(3)	0x23 PULSE_THSX Register (Read/Write)	R/W	10	X pulse threshold | 	0,THSX6,THSX5,THSX4,THSX3,THSX2,THSX1,THSX0
0x24	PULSE_THSY(1)(3)	0x24 PULSE_THSY Register (Read/Write)	R/W	10	Y pulse threshold | 	0,THSY6,THSY5,THSY4,THSY3,THSY2,THSY1,THSY0
0x25	PULSE_THSZ(1)(3)	0x25 PULSE_THSZ Register (Read/Write)	R/W	10	Z pulse threshold | 	0,THSZ6,THSZ5,THSZ4,THSZ3,THSZ2,THSZ1,THSZ0
0x26	PULSE_TMLT(1)(3)	0x26 PULSE_TMLT Register (Read/Write)	R/W	10	Time limit for pulse | 	TMLT7,TMLT6,TMLT5,TMLT4,TMLT3,TMLT2,TMLT1,TMLT0
0x27	PULSE_LTCY(1)(3)	0x27 PULSE_LTCY Register (Read/Write)	R/W	10	Latency time for 2nd pulse | 	LTCY7,LTCY6,LTCY5,LTCY4,LTCY3,LTCY2,LTCY1,LTCY0
0x28	PULSE_WIND(1)(3)	0x28: PULSE_WIND Second Pulse Time Window Register	R/W	10	Window time for 2nd pulse | 	WIND7,WIND6,WIND5,WIND4,WIND3,WIND2,WIND1,WIND0
0x29	ASLP_COUNT(1)(3)	0x29 ASLP_COUNT Register (Read/Write)	R/W	10	Counter setting for Auto-SLEEP | 	D7,D6,D5,D4,D3,D2,D1,D0
0x2A	CTRL_REG1(1)(3)	0x2A CTRL_REG1 Register (Read/Write)	R/W	10	ODR = 800 Hz, STANDBY Mode. | 	ASLP_RATE1,ASLP_RATE0,DR2,DR1,DR0,LNOISE,F_READ,ACTIVE
0x2B	CTRL_REG2(1)(3)	0x2B CTRL_REG2 Register (Read/Write)	R/W	10	Sleep Enable, OS Modes, RST, ST | 	ST,RST,0,SMODS1,SMODS0,SLPE,MODS1,MODS0
0x2C	CTRL_REG3(1)(3)	0x2C CTRL_REG3 Register (Read/Write)	R/W	10	Wake from Sleep, IPOL, PP_OD | 	0,WAKE_TRANS,WAKE_LNDPRT,WAKE_PULSE,WAKE_FF_MT,0,IPOL,PP_OD
0x2D	CTRL_REG4(1)(3)	0x2D CTRL_REG4 Register (Read/Write)	R/W	10	Interrupt enable register | 	INT_EN_ASLP,0,INT_EN_TRANS,INT_EN_LNDPRT,INT_EN_PULSE,INT_EN_FF_MT,0,INT_EN_DRDY
0x2E	CTRL_REG5(1)(3)	0x2E: CTRL_REG5 Interrupt Configuration Register	R/W	10	Interrupt pin (INT1/INT2) map | 	INT_CFG_ASLP,0,INT_CFG_TRANS,INT_CFG_LNDPRT,INT_CFG_PULSE,INT_CFG_FF_MT,0,INT_CFG_DRDY
0x2F	OFF_X(1)(3)	0x2F OFF_X Register (Read/Write)	R/W	10	X-axis offset adjust | 	D7,D6,D5,D4,D3,D2,D1,D0
0x30	OFF_Y(1)(3)	0x30 OFF_Y Register (Read/Write)	R/W	10	Y-axis offset adjust | 	D7,D6,D5,D4,D3,D2,D1,D0
0x31	OFF_Z(1)(3)	0x31 OFF_Z Register (Read/Write)	R/W	10	Z-axis offset adjust | 	D7,D6,D5,D4,D3,D2,D1,D0
^^
1. Register contents are preserved when transition from ACTIVE to STANDBY mode occurs.
2. Register contents are reset when transition from STANDBY to ACTIVE mode occurs.
3. Modification of this register’s contents can only occur when device is STANDBY mode except CTRL_REG1 ACTIVE bit and CTRL_REG2 RST bit."
				#endregion
				);
			dictionary.Add(deviceID.MMA8453Q,
			#region
 @"0x00	STATUS(1)(2)	0x00 STATUS: Data Status Register (Read Only)	R	6	Real time status | 	ZYXOW,ZOW,YOW,XOW,ZYXDR,ZDR,YDR,XDR
0x01	OUT_X_MSB(1)(2)	0x01 OUT_X_MSB: X_MSB Register (Read Only)	R	6	[7:0] are 8 MSBs of 10-bit sample. | 	XD9,XD8,XD7,XD6,XD5,XD4,XD3,XD2
0x02	OUT_X_LSB(1)(2)	0x02 OUT_X_LSB: X_LSB Register (Read Only)	R	6	[7:6] are 2 LSBs of 10-bit sample | 	XD1,XD0,0,0,0,0,0,0
0x03	OUT_Y_MSB(1)(2)	0x03 OUT_Y_MSB: Y_MSB Register (Read Only)	R	6	[7:0] are 8 MSBs of 10-bit sample | 	YD9,YD8,YD7,YD6,YD5,YD4,YD3,YD2
0x04	OUT_Y_LSB(1)(2)	0x04 OUT_Y_LSB: Y_LSB Register (Read Only)	R	6	[7:6] are 2 LSBs of 10-bit sample | 	YD1,YD0,0,0,0,0,0,0
0x05	OUT_Z_MSB(1)(2)	0x05 OUT_Z_MSB: Z_MSB Register (Read Only)	R	6	[7:0] are 8 MSBs of 10-bit sample | 	ZD9,ZD8,ZD7,ZD6,ZD5,ZD4,ZD3,ZD2
0x06	OUT_Z_LSB(1)(2)	0x06 OUT_Z_LSB: Z_LSB Register (Read Only)	R	6	[7:6] are 2 LSBs of 10-bit sample | 	ZD1,ZD0,0,0,0,0,0,0
0x07	Reserved	0x06 OUT_Z_LSB: Z_LSB Register (Read Only)	R	0	Reserved. Read return 0x00. | 	
0x08	Reserved	0x06 OUT_Z_LSB: Z_LSB Register (Read Only)	R	0	Reserved. Read return 0x00. | 	
0x0B	SYSMOD(1)(2)	0x0B SYSMOD: System Mode Register (Read Only)	R	6	Current System Mode | 	0,0,0,0,0,0,SYSMOD1,SYSMOD0
0x0C	INT_SOURCE(1)(2)	0x0C INT_SOURCE: System Interrupt Status Register (Read Only)	R	6	Interrupt status | 	SRC_ASLP,0,SRC_TRANS,SRC_LNDPRT,SRC_PULSE,SRC_FF_MT,0,SRC_DRDY
0x0D	WHO_AM_I(1)	0x0D: WHO_AM_I Device ID Register (Read Only)	R	2	Device ID (0x3A) | 	0,0,1,1,1,0,1,0
0x0E	XYZ_DATA_CFG(1)(3)	0x0E: XYZ_DATA_CFG (Read/Write)	R/W	10	Dynamic Range Settings | 	0,0,0,0,0,0,FS1,FS0
0x0F	HP_FILTER_CUTOFF(1)(3)	0x0F HP_FILTER_CUTOFF: High Pass Filter Register (Read/Write)	R/W	10	Cut-off frequency is set to 16 Hz @ 800 Hz | 	0,0,Pulse_HPF_BYP,Pulse_LPF_EN,0,0,SEL1,SEL0
0x10	PL_STATUS(1)(2)	0x10 PL_STATUS Register (Read Only)	R	6	Landscape/Portrait orientation status | 	NEWLP,LO,0,0,0,LAPO[1],LAPO[0],BAFRO
0x11	PL_CFG(1)(3)	0x11 PL_CFG Register (Read/Write)	R/W	10	Landscape/Portrait configuration. | 	DBCNTM,PL_EN,0,0,0,0,0,0
0x12	PL_COUNT(1)(3)	0x12 PL_COUNT Register (Read/Write)	R/W	10	Landscape/Portrait debounce counter | 	DBNCE[7],DBNCE[6],DBNCE[5],DBNCE[4],DBNCE[3],DBNCE[2],DBNCE[1],DBNCE[0]
0x13	PL_BF_ZCOMP(1)(3)	0x13: PL_BF_ZCOMP Register (Read/Write)	R/W	10	Back-Front, Z-Lock Trip threshold | 	BKFR[1],BKFR[0],0,0,0,ZLOCK[2],ZLOCK[1],ZLOCK[0]
0x14	P_L_THS_REG(1)(3)	0x14: P_L_THS_REG Register (Read/Write)	R/W	10	Portrait to Landscape Trip Angle is 29\x00b0 | 	P_L_THS[4],P_L_THS[3],P_L_THS[2],P_L_THS[1],P_L_THS[0],HYS[2],HYS[1],HYS[0]
0x15	FF_MT_CFG(1)(3)	0x15 FF_MT_CFG Register (Read/Write)	R/W	10	Freefall/Motion functional block configuration | 	ELE,OAE,ZEFE,YEFE,XEFE,0,0,0
0x16	FF_MT_SRC(1)(2)	0x16: FF_MT_SRC Freefall and Motion Source Register (Read Only)	R	6	Freefall/Motion event source register | 	EA,0,ZHE,ZHP,YHE,YHP,XHE,XHP
0x17	FF_MT_THS(1)(3)	0x17 FF_MT_THS Register (Read/Write)	R/W	10	Freefall/Motion threshold register | 	DBCNTM,THS6,THS5,THS4,THS3,THS2,THS1,THS0
0x18	FF_MT_COUNT(1)(3)	0x18 FF_MT_COUNT_Register (Read/Write)	R/W	10	Freefall/Motion debounce counter | 	D7,D6,D5,D4,D3,D2,D1,D0
0x19	Reserved	0x18 FF_MT_COUNT_Register (Read/Write)	R	0	Reserved. Read return 0x00. | 	
0x1A	Reserved	0x18 FF_MT_COUNT_Register (Read/Write)	R	0	Reserved. Read return 0x00. | 	
0x1B	Reserved	0x18 FF_MT_COUNT_Register (Read/Write)	R	0	Reserved. Read return 0x00. | 	
0x1C	Reserved	0x18 FF_MT_COUNT_Register (Read/Write)	R	0	Reserved. Read return 0x00. | 	
0x1D	TRANSIENT_CFG(1)(3)	0x1D TRANSIENT_CFG Register (Read/Write)	R/W	10	Transient functional block configuration | 	0,0,0,ELE,ZTEFE,YTEFE,XTEFE,HPF_BYP
0x1E	TRANSIENT_SRC(1)(2)	0x1E TRANSIENT_SRC Register (Read Only)	R	6	Transient event status register | 	0,EA,ZTRANSE,Z_Trans_Pol,YTRANSE,Y_Trans_Pol,XTRANSE,X_Trans_Pol
0x1F	TRANSIENT_THS(1)(3)	0x1F TRANSIENT_THS Register (Read/Write)	R/W	10	Transient event threshold | 	DBCNTM,THS6,THS5,THS4,THS3,THS2,THS1,THS0
0x20	TRANSIENT_COUNT(1)(3)	0x20 TRANSIENT_COUNT Register (Read/Write)	R/W	10	Transient debounce counter | 	D7,D6,D5,D4,D3,D2,D1,D0
0x21	PULSE_CFG(1)(3)	0x21 PULSE_CFG Register (Read/Write)	R/W	10	ELE, Double_XYZ or Single_XYZ | 	DPA,ELE,ZDPEFE,ZSPEFE,YDPEFE,YSPEFE,XDPEFE,XSPEFE
0x22	PULSE_SRC(1)(2)	0x22 PULSE_SRC Register (Read Only)	R	6	EA, Double_XYZ or Single_XYZ | 	EA,AxZ,AxY,AxX,DPE,PolZ,PolY,PolX
0x23	PULSE_THSX(1)(3)	0x23 PULSE_THSX Register (Read/Write)	R/W	10	X pulse threshold | 	0,THSX6,THSX5,THSX4,THSX3,THSX2,THSX1,THSX0
0x24	PULSE_THSY(1)(3)	0x24 PULSE_THSY Register (Read/Write)	R/W	10	Y pulse threshold | 	0,THSY6,THSY5,THSY4,THSY3,THSY2,THSY1,THSY0
0x25	PULSE_THSZ(1)(3)	0x25 PULSE_THSZ Register (Read/Write)	R/W	10	Z pulse threshold | 	0,THSZ6,THSZ5,THSZ4,THSZ3,THSZ2,THSZ1,THSZ0
0x26	PULSE_TMLT(1)(3)	0x26 PULSE_TMLT Register (Read/Write)	R/W	10	Time limit for pulse | 	TMLT7,TMLT6,TMLT5,TMLT4,TMLT3,TMLT2,TMLT1,TMLT0
0x27	PULSE_LTCY(1)(3)	0x27 PULSE_LTCY Register (Read/Write)	R/W	10	Latency time for 2nd pulse | 	LTCY7,LTCY6,LTCY5,LTCY4,LTCY3,LTCY2,LTCY1,LTCY0
0x28	PULSE_WIND(1)(3)	0x28: PULSE_WIND Second Pulse Time Window Register	R/W	10	Window time for 2nd pulse | 	WIND7,WIND6,WIND5,WIND4,WIND3,WIND2,WIND1,WIND0
0x29	ASLP_COUNT(1)(3)	0x29 ASLP_COUNT Register (Read/Write)	R/W	10	Counter setting for Auto-SLEEP | 	D7,D6,D5,D4,D3,D2,D1,D0
0x2A	CTRL_REG1(1)(3)	0x2A CTRL_REG1 Register (Read/Write)	R/W	10	ODR = 800 Hz, STANDBY Mode. | 	ASLP_RATE1,ASLP_RATE0,DR2,DR1,DR0,LNOISE,F_READ,ACTIVE
0x2B	CTRL_REG2(1)(3)	0x2B CTRL_REG2 Register (Read/Write)	R/W	10	Sleep Enable, OS Modes, RST, ST | 	ST,RST,0,SMODS1,SMODS0,SLPE,MODS1,MODS0
0x2C	CTRL_REG3(1)(3)	0x2C CTRL_REG3 Register (Read/Write)	R/W	10	Wake from Sleep, IPOL, PP_OD | 	0,WAKE_TRANS,WAKE_LNDPRT,WAKE_PULSE,WAKE_FF_MT,0,IPOL,PP_OD
0x2D	CTRL_REG4(1)(3)	0x2D CTRL_REG4 Register (Read/Write)	R/W	10	Interrupt enable register | 	INT_EN_ASLP,0,INT_EN_TRANS,INT_EN_LNDPR,INT_EN_PULSE,INT_EN_FF_MT,0,INT_EN_DRDY
0x2E	CTRL_REG5(1)(3)	0x2E: CTRL_REG5 Interrupt Configuration Register	R/W	10	Interrupt pin (INT1/INT2) map | 	INT_CFG_ASLP,0,INT_CFG_TRANS,INT_CFG_LNDPRT,INT_CFG_PULSE,INT_CFG_FF_MT,0,INT_CFG_DRDY
0x2F	OFF_X(1)(3)	0x2F OFF_X Register (Read/Write)	R/W	10	X-axis offset adjust | 	D7,D6,D5,D4,D3,D2,D1,D0
0x30	OFF_Y(1)(3)	0x30 OFF_Y Register (Read/Write)	R/W	10	Y-axis offset adjust | 	D7,D6,D5,D4,D3,D2,D1,D0
0x31	OFF_Z(1)(3)	0x31 OFF_Z Register (Read/Write)	R/W	10	Z-axis offset adjust | 	D7,D6,D5,D4,D3,D2,D1,D0
^^
1. Register contents are preserved when transition from ACTIVE to STANDBY mode occurs.
2. Register contents are reset when transition from STANDBY to ACTIVE mode occurs.
3. Modification of this register’s contents can only occur when device is STANDBY mode except CTRL_REG1 ACTIVE bit and CTRL_REG2 RST bit."
			#endregion
			);
			dictionary.Add(deviceID.MMA8652FC,
			#region
@"0x00	STATUS(1)(2)	0x00 STATUS: Data Status Register (Read Only)	R	6	Real time status | 	ZYXOW,ZOW,YOW,XOW,ZYXDR,ZDR,YDR,XDR
0x01	OUT_X_MSB(1)(2)	0x01 OUT_X_MSB: X_MSB Register (Read Only)	R	6	[7:0] are 8 MSBs of 12-bit sample. | 	XD11,XD10,XD9,XD8,XD7,XD6,XD5,XD4
0x02	OUT_X_LSB(1)(2)	0x02 OUT_X_LSB: X_LSB Register (Read Only)	R	6	[7:4] are 4 LSBs of 12-bit sample. | 	XD3,XD2,XD1,XD0,0,0,0,0
0x03	OUT_Y_MSB(1)(2)	0x03 OUT_Y_MSB: Y_MSB Register (Read Only)	R	6	[7:0] are 8 MSBs of 12-bit sample. | 	YD11,YD10,YD9,YD8,YD7,YD6,YD5,YD4
0x04	OUT_Y_LSB(1)(2)	0x04 OUT_Y_LSB: Y_LSB Register (Read Only)	R	6	[7:4] are 4 LSBs of 12-bit sample. | 	YD3,YD2,XD1,XD0,0,0,0,0
0x05	OUT_Z_MSB(1)(2)	0x05 OUT_Z_MSB: Z_MSB Register (Read Only)	R	6	[7:0] are 8 MSBs of 12-bit sample. | 	ZD11,ZD10,ZD9,ZD8,ZD7,ZD6,ZD5,ZD4
0x06	OUT_Z_LSB(1)(2)	0x06 OUT_Z_LSB: Z_LSB Register (Read Only)	R	6	[7:4] are 4 LSBs of 12-bit sample. | 	ZD3,ZD2,ZD1,ZD0,0,0,0,0
0x07	Reserved	0x06 OUT_Z_LSB: Z_LSB Register (Read Only)	R	0	Reserved. Read return 0x00. | 	
0x08	Reserved	0x06 OUT_Z_LSB: Z_LSB Register (Read Only)	R	0	Reserved. Read return 0x00. | 	
0x0B	SYSMOD(1)(2)	0x0B SYSMOD: System Mode Register (Read Only)	R	6	Current System Mode | 	0,0,0,0,0,0,SYSMOD1,SYSMOD0
0x0C	INT_SOURCE(1)(2)	0x0C INT_SOURCE: System Interrupt Status Register (Read Only)	R	6	Interrupt status | 	SRC_ASLP,0,SRC_TRANS,SRC_LNDPRT,SRC_PULSE,SRC_FF_MT,0,SRC_DRDY
0x0D	WHO_AM_I(1)	0x0D: WHO_AM_I Device ID Register (Read Only)	R	2	Device ID (0x4A) | 	0,1,0,0,1,0,1,0
0x0E	XYZ_DATA_CFG(1)(3)	0x0E: XYZ_DATA_CFG (Read/Write)	R/W	10	HPF Data Out and Dynamic Range Settings | 	0,0,0,HPF_OUT,0,0,FS1,FS0
0x0F	HP_FILTER_CUTOFF(1)(3)	0x0F HP_FILTER_CUTOFF: High Pass Filter Register (Read/Write)	R/W	10	Cut-off frequency is set to 16 Hz @ 800 Hz | 	0,0,Pulse_HPF_BYP,Pulse_LPF_EN,0,0,SEL1,SEL0
0x10	PL_STATUS(1)(2)	0x10 PL_STATUS Register (Read Only)	R	6	Landscape/Portrait orientation status | 	NEWLP,LO,0,0,0,LAPO[1],LAPO[0],BAFRO
0x11	PL_CFG(1)(3)	0x11 PL_CFG Register (Read/Write)	R/W	10	Landscape/Portrait configuration. | 	DBCNTM,PL_EN,0,0,0,0,0,0
0x12	PL_COUNT(1)(3)	0x12 PL_COUNT Register (Read/Write)	R/W	10	Landscape/Portrait debounce counter | 	DBNCE[7],DBNCE[6],DBNCE[5],DBNCE[4],DBNCE[3],DBNCE[2],DBNCE[1],DBNCE[0]
0x13	PL_BF_ZCOMP(1)(3)	0x13: PL_BF_ZCOMP Register (Read/Write)	R/W	10	Back-Front, Z-Lock Trip threshold | 	BKFR[1],BKFR[0],0,0,0,ZLOCK[2],ZLOCK[1],ZLOCK[0]
0x14	P_L_THS_REG(1)(3)	0x14: P_L_THS_REG Register (Read/Write)	R/W	10	Portrait to Landscape Trip Angle is 29\x00b0 | 	P_L_THS[4],P_L_THS[3],P_L_THS[2],P_L_THS[1],P_L_THS[0],HYS[2],HYS[1],HYS[0]
0x15	FF_MT_CFG(1)(3)	0x15 FF_MT_CFG Register (Read/Write)	R/W	10	Freefall/Motion functional block configuration | 	ELE,OAE,ZEFE,YEFE,XEFE,0,0,0
0x16	FF_MT_SRC(1)(2)	0x16: FF_MT_SRC Freefall and Motion Source Register (Read Only)	R	6	Freefall/Motion event source register | 	EA,0,ZHE,ZHP,YHE,YHP,XHE,XHP
0x17	FF_MT_THS(1)(3)	0x17 FF_MT_THS Register (Read/Write)	R/W	10	Freefall/Motion threshold register | 	DBCNTM,THS6,THS5,THS4,THS3,THS2,THS1,THS0
0x18	FF_MT_COUNT(1)(3)	0x18 FF_MT_COUNT_Register (Read/Write)	R/W	10	Freefall/Motion debounce counter | 	D7,D6,D5,D4,D3,D2,D1,D0
0x19	Reserved	0x18 FF_MT_COUNT_Register (Read/Write)	R	0	Reserved. Read return 0x00. | 	
0x1A	Reserved	0x18 FF_MT_COUNT_Register (Read/Write)	R	0	Reserved. Read return 0x00. | 	
0x1B	Reserved	0x18 FF_MT_COUNT_Register (Read/Write)	R	0	Reserved. Read return 0x00. | 	
0x1C	Reserved	0x18 FF_MT_COUNT_Register (Read/Write)	R	0	Reserved. Read return 0x00. | 	
0x1D	TRANSIENT_CFG(1)(3)	0x1D TRANSIENT_CFG Register (Read/Write)	R/W	10	Transient functional block configuration | 	0,0,0,ELE,ZTEFE,YTEFE,XTEFE,HPF_BYP
0x1E	TRANSIENT_SRC(1)(2)	0x1E TRANSIENT_SRC Register (Read Only)	R	6	Transient event status register | 	0,EA,ZTRANSE,Z_Trans_Pol,YTRANSE,Y_Trans_Pol,XTRANSE,X_Trans_Pol
0x1F	TRANSIENT_THS(1)(3)	0x1F TRANSIENT_THS Register (Read/Write)	R/W	10	Transient event threshold | 	DBCNTM,THS6,THS5,THS4,THS3,THS2,THS1,THS0
0x20	TRANSIENT_COUNT(1)(3)	0x20 TRANSIENT_COUNT Register (Read/Write)	R/W	10	Transient debounce counter | 	D7,D6,D5,D4,D3,D2,D1,D0
0x21	PULSE_CFG(1)(3)	0x21 PULSE_CFG Register (Read/Write)	R/W	10	ELE, Double_XYZ or Single_XYZ | 	DPA,ELE,ZDPEFE,ZSPEFE,YDPEFE,YSPEFE,XDPEFE,XSPEFE
0x22	PULSE_SRC(1)(2)	0x22 PULSE_SRC Register (Read Only)	R	6	EA, Double_XYZ or Single_XYZ | 	EA,AxZ,AxY,AxX,DPE,PolZ,PolY,PolX
0x23	PULSE_THSX(1)(3)	0x23 PULSE_THSX Register (Read/Write)	R/W	10	X pulse threshold | 	0,THSX6,THSX5,THSX4,THSX3,THSX2,THSX1,THSX0
0x24	PULSE_THSY(1)(3)	0x24 PULSE_THSY Register (Read/Write)	R/W	10	Y pulse threshold | 	0,THSY6,THSY5,THSY4,THSY3,THSY2,THSY1,THSY0
0x25	PULSE_THSZ(1)(3)	0x25 PULSE_THSZ Register (Read/Write)	R/W	10	Z pulse threshold | 	0,THSZ6,THSZ5,THSZ4,THSZ3,THSZ2,THSZ1,THSZ0
0x26	PULSE_TMLT(1)(3)	0x26 PULSE_TMLT Register (Read/Write)	R/W	10	Time limit for pulse | 	TMLT7,TMLT6,TMLT5,TMLT4,TMLT3,TMLT2,TMLT1,TMLT0
0x27	PULSE_LTCY(1)(3)	0x27 PULSE_LTCY Register (Read/Write)	R/W	10	Latency time for 2nd pulse | 	LTCY7,LTCY6,LTCY5,LTCY4,LTCY3,LTCY2,LTCY1,LTCY0
0x28	PULSE_WIND(1)(3)	0x28: PULSE_WIND Second Pulse Time Window Register	R/W	10	Window time for 2nd pulse | 	WIND7,WIND6,WIND5,WIND4,WIND3,WIND2,WIND1,WIND0
0x29	ASLP_COUNT(1)(3)	0x29 ASLP_COUNT Register (Read/Write)	R/W	10	Counter setting for Auto-SLEEP | 	D7,D6,D5,D4,D3,D2,D1,D0
0x2A	CTRL_REG1(1)(3)	0x2A CTRL_REG1 Register (Read/Write)	R/W	10	ODR = 800 Hz, STANDBY Mode. | 	ASLP_RATE1,ASLP_RATE0,DR2,DR1,DR0,LNOISE,F_READ,ACTIVE
0x2B	CTRL_REG2(1)(3)	0x2B CTRL_REG2 Register (Read/Write)	R/W	10	Sleep Enable, OS Modes, RST, ST | 	ST,RST,0,SMODS1,SMODS0,SLPE,MODS1,MODS0
0x2C	CTRL_REG3(1)(3)	0x2C CTRL_REG3 Register (Read/Write)	R/W	10	Wake from Sleep, IPOL, PP_OD | 	0,WAKE_TRANS,WAKE_LNDPRT,WAKE_PULSE,WAKE_FF_MT,0,IPOL,PP_OD
0x2D	CTRL_REG4(1)(3)	0x2D CTRL_REG4 Register (Read/Write)	R/W	10	Interrupt enable register | 	INT_EN_ASLP,0,INT_EN_TRANS,INT_EN_LNDPRT,INT_EN_PULSE,INT_EN_FF_MT,0,INT_EN_DRDY
0x2E	CTRL_REG5(1)(3)	0x2E: CTRL_REG5 Interrupt Configuration Register	R/W	10	Interrupt pin (INT1/INT2) map | 	INT_CFG_ASLP,0,INT_CFG_TRANS,INT_CFG_LNDPRT,INT_CFG_PULSE,INT_CFG_FF_MT,0,INT_CFG_DRDY
0x2F	OFF_X(1)(3)	0x2F OFF_X Register (Read/Write)	R/W	10	X-axis offset adjust | 	D7,D6,D5,D4,D3,D2,D1,D0
0x30	OFF_Y(1)(3)	0x30 OFF_Y Register (Read/Write)	R/W	10	Y-axis offset adjust | 	D7,D6,D5,D4,D3,D2,D1,D0
0x31	OFF_Z(1)(3)	0x31 OFF_Z Register (Read/Write)	R/W	10	Z-axis offset adjust | 	D7,D6,D5,D4,D3,D2,D1,D0
^^
1. Register contents are preserved when transition from ACTIVE to STANDBY mode occurs.
2. Register contents are reset when transition from STANDBY to ACTIVE mode occurs.
3. Modification of this register’s contents can only occur when device is STANDBY mode except CTRL_REG1 ACTIVE bit and CTRL_REG2 RST bit."
			#endregion
			);
			dictionary.Add(deviceID.MMA8653FC,
			#region
@"0x00	STATUS(1)(2)	0x00 STATUS: Data Status Register (Read Only)	R	6	Real time status | 	ZYXOW,ZOW,YOW,XOW,ZYXDR,ZDR,YDR,XDR
0x01	OUT_X_MSB(1)(2)	0x01 OUT_X_MSB: X_MSB Register (Read Only)	R	6	[7:0] are 8 MSBs of 10-bit sample. | 	XD9,XD8,XD7,XD6,XD5,XD4,XD3,XD2
0x02	OUT_X_LSB(1)(2)	0x02 OUT_X_LSB: X_LSB Register (Read Only)	R	6	[7:6] are 2 LSBs of 10-bit sample | 	XD1,XD0,0,0,0,0,0,0
0x03	OUT_Y_MSB(1)(2)	0x03 OUT_Y_MSB: Y_MSB Register (Read Only)	R	6	[7:0] are 8 MSBs of 10-bit sample | 	YD9,YD8,YD7,YD6,YD5,YD4,YD3,YD2
0x04	OUT_Y_LSB(1)(2)	0x04 OUT_Y_LSB: Y_LSB Register (Read Only)	R	6	[7:6] are 2 LSBs of 10-bit sample | 	YD1,YD0,0,0,0,0,0,0
0x05	OUT_Z_MSB(1)(2)	0x05 OUT_Z_MSB: Z_MSB Register (Read Only)	R	6	[7:0] are 8 MSBs of 10-bit sample | 	ZD9,ZD8,ZD7,ZD6,ZD5,ZD4,ZD3,ZD2
0x06	OUT_Z_LSB(1)(2)	0x06 OUT_Z_LSB: Z_LSB Register (Read Only)	R	6	[7:6] are 2 LSBs of 10-bit sample | 	ZD1,ZD0,0,0,0,0,0,0
0x07	Reserved	0x06 OUT_Z_LSB: Z_LSB Register (Read Only)	R	0	Reserved. Read return 0x00. | 	
0x08	Reserved	0x06 OUT_Z_LSB: Z_LSB Register (Read Only)	R	0	Reserved. Read return 0x00. | 	
0x0B	SYSMOD(1)(2)	0x0B SYSMOD: System Mode Register (Read Only)	R	6	Current System Mode | 	0,0,0,0,0,0,SYSMOD1,SYSMOD0
0x0C	INT_SOURCE(1)(2)	0x0C INT_SOURCE: System Interrupt Status Register (Read Only)	R	6	Interrupt status | 	SRC_ASLP,0,SRC_TRANS,SRC_LNDPRT,SRC_PULSE,SRC_FF_MT,0,SRC_DRDY
0x0D	WHO_AM_I(1)	0x0D: WHO_AM_I Device ID Register (Read Only)	R	2	Device ID (0x5A) | 	0,1,0,1,1,0,1,0
0x0E	XYZ_DATA_CFG(1)(3)	0x0E: XYZ_DATA_CFG (Read/Write)	R/W	10	Dynamic Range Settings | 	0,0,0,0,0,0,FS1,FS0
0x0F	HP_FILTER_CUTOFF(1)(3)	0x0F HP_FILTER_CUTOFF: High Pass Filter Register (Read/Write)	R/W	10	Cut-off frequency is set to 16 Hz @ 800 Hz | 	0,0,Pulse_HPF_BYP,Pulse_LPF_EN,0,0,SEL1,SEL0
0x10	PL_STATUS(1)(2)	0x10 PL_STATUS Register (Read Only)	R	6	Landscape/Portrait orientation status | 	NEWLP,LO,0,0,0,LAPO[1],LAPO[0],BAFRO
0x11	PL_CFG(1)(3)	0x11 PL_CFG Register (Read/Write)	R/W	10	Landscape/Portrait configuration. | 	DBCNTM,PL_EN,0,0,0,0,0,0
0x12	PL_COUNT(1)(3)	0x12 PL_COUNT Register (Read/Write)	R/W	10	Landscape/Portrait debounce counter | 	DBNCE[7],DBNCE[6],DBNCE[5],DBNCE[4],DBNCE[3],DBNCE[2],DBNCE[1],DBNCE[0]
0x13	PL_BF_ZCOMP(1)(3)	0x13: PL_BF_ZCOMP Register (Read/Write)	R/W	10	Back-Front, Z-Lock Trip threshold | 	BKFR[1],BKFR[0],0,0,0,ZLOCK[2],ZLOCK[1],ZLOCK[0]
0x14	P_L_THS_REG(1)(3)	0x14: P_L_THS_REG Register (Read/Write)	R/W	10	Portrait to Landscape Trip Angle is 29\x00b0 | 	P_L_THS[4],P_L_THS[3],P_L_THS[2],P_L_THS[1],P_L_THS[0],HYS[2],HYS[1],HYS[0]
0x15	FF_MT_CFG(1)(3)	0x15 FF_MT_CFG Register (Read/Write)	R/W	10	Freefall/Motion functional block configuration | 	ELE,OAE,ZEFE,YEFE,XEFE,0,0,0
0x16	FF_MT_SRC(1)(2)	0x16: FF_MT_SRC Freefall and Motion Source Register (Read Only)	R	6	Freefall/Motion event source register | 	EA,0,ZHE,ZHP,YHE,YHP,XHE,XHP
0x17	FF_MT_THS(1)(3)	0x17 FF_MT_THS Register (Read/Write)	R/W	10	Freefall/Motion threshold register | 	DBCNTM,THS6,THS5,THS4,THS3,THS2,THS1,THS0
0x18	FF_MT_COUNT(1)(3)	0x18 FF_MT_COUNT_Register (Read/Write)	R/W	10	Freefall/Motion debounce counter | 	D7,D6,D5,D4,D3,D2,D1,D0
0x19	Reserved	0x18 FF_MT_COUNT_Register (Read/Write)	R	0	Reserved. Read return 0x00. | 	
0x1A	Reserved	0x18 FF_MT_COUNT_Register (Read/Write)	R	0	Reserved. Read return 0x00. | 	
0x1B	Reserved	0x18 FF_MT_COUNT_Register (Read/Write)	R	0	Reserved. Read return 0x00. | 	
0x1C	Reserved	0x18 FF_MT_COUNT_Register (Read/Write)	R	0	Reserved. Read return 0x00. | 	
0x1D	TRANSIENT_CFG(1)(3)	0x1D TRANSIENT_CFG Register (Read/Write)	R/W	10	Transient functional block configuration | 	0,0,0,ELE,ZTEFE,YTEFE,XTEFE,HPF_BYP
0x1E	TRANSIENT_SRC(1)(2)	0x1E TRANSIENT_SRC Register (Read Only)	R	6	Transient event status register | 	0,EA,ZTRANSE,Z_Trans_Pol,YTRANSE,Y_Trans_Pol,XTRANSE,X_Trans_Pol
0x1F	TRANSIENT_THS(1)(3)	0x1F TRANSIENT_THS Register (Read/Write)	R/W	10	Transient event threshold | 	DBCNTM,THS6,THS5,THS4,THS3,THS2,THS1,THS0
0x20	TRANSIENT_COUNT(1)(3)	0x20 TRANSIENT_COUNT Register (Read/Write)	R/W	10	Transient debounce counter | 	D7,D6,D5,D4,D3,D2,D1,D0
0x21	PULSE_CFG(1)(3)	0x21 PULSE_CFG Register (Read/Write)	R/W	10	ELE, Double_XYZ or Single_XYZ | 	DPA,ELE,ZDPEFE,ZSPEFE,YDPEFE,YSPEFE,XDPEFE,XSPEFE
0x22	PULSE_SRC(1)(2)	0x22 PULSE_SRC Register (Read Only)	R	6	EA, Double_XYZ or Single_XYZ | 	EA,AxZ,AxY,AxX,DPE,PolZ,PolY,PolX
0x23	PULSE_THSX(1)(3)	0x23 PULSE_THSX Register (Read/Write)	R/W	10	X pulse threshold | 	0,THSX6,THSX5,THSX4,THSX3,THSX2,THSX1,THSX0
0x24	PULSE_THSY(1)(3)	0x24 PULSE_THSY Register (Read/Write)	R/W	10	Y pulse threshold | 	0,THSY6,THSY5,THSY4,THSY3,THSY2,THSY1,THSY0
0x25	PULSE_THSZ(1)(3)	0x25 PULSE_THSZ Register (Read/Write)	R/W	10	Z pulse threshold | 	0,THSZ6,THSZ5,THSZ4,THSZ3,THSZ2,THSZ1,THSZ0
0x26	PULSE_TMLT(1)(3)	0x26 PULSE_TMLT Register (Read/Write)	R/W	10	Time limit for pulse | 	TMLT7,TMLT6,TMLT5,TMLT4,TMLT3,TMLT2,TMLT1,TMLT0
0x27	PULSE_LTCY(1)(3)	0x27 PULSE_LTCY Register (Read/Write)	R/W	10	Latency time for 2nd pulse | 	LTCY7,LTCY6,LTCY5,LTCY4,LTCY3,LTCY2,LTCY1,LTCY0
0x28	PULSE_WIND(1)(3)	0x28: PULSE_WIND Second Pulse Time Window Register	R/W	10	Window time for 2nd pulse | 	WIND7,WIND6,WIND5,WIND4,WIND3,WIND2,WIND1,WIND0
0x29	ASLP_COUNT(1)(3)	0x29 ASLP_COUNT Register (Read/Write)	R/W	10	Counter setting for Auto-SLEEP | 	D7,D6,D5,D4,D3,D2,D1,D0
0x2A	CTRL_REG1(1)(3)	0x2A CTRL_REG1 Register (Read/Write)	R/W	10	ODR = 800 Hz, STANDBY Mode. | 	ASLP_RATE1,ASLP_RATE0,DR2,DR1,DR0,LNOISE,F_READ,ACTIVE
0x2B	CTRL_REG2(1)(3)	0x2B CTRL_REG2 Register (Read/Write)	R/W	10	Sleep Enable, OS Modes, RST, ST | 	ST,RST,0,SMODS1,SMODS0,SLPE,MODS1,MODS0
0x2C	CTRL_REG3(1)(3)	0x2C CTRL_REG3 Register (Read/Write)	R/W	10	Wake from Sleep, IPOL, PP_OD | 	0,WAKE_TRANS,WAKE_LNDPRT,WAKE_PULSE,WAKE_FF_MT,0,IPOL,PP_OD
0x2D	CTRL_REG4(1)(3)	0x2D CTRL_REG4 Register (Read/Write)	R/W	10	Interrupt enable register | 	INT_EN_ASLP,0,INT_EN_TRANS,INT_EN_LNDPR,INT_EN_PULSE,INT_EN_FF_MT,0,INT_EN_DRDY
0x2E	CTRL_REG5(1)(3)	0x2E: CTRL_REG5 Interrupt Configuration Register	R/W	10	Interrupt pin (INT1/INT2) map | 	INT_CFG_ASLP,0,INT_CFG_TRANS,INT_CFG_LNDPRT,INT_CFG_PULSE,INT_CFG_FF_MT,0,INT_CFG_DRDY
0x2F	OFF_X(1)(3)	0x2F OFF_X Register (Read/Write)	R/W	10	X-axis offset adjust | 	D7,D6,D5,D4,D3,D2,D1,D0
0x30	OFF_Y(1)(3)	0x30 OFF_Y Register (Read/Write)	R/W	10	Y-axis offset adjust | 	D7,D6,D5,D4,D3,D2,D1,D0
0x31	OFF_Z(1)(3)	0x31 OFF_Z Register (Read/Write)	R/W	10	Z-axis offset adjust | 	D7,D6,D5,D4,D3,D2,D1,D0
^^
1. Register contents are preserved when transition from ACTIVE to STANDBY mode occurs.
2. Register contents are reset when transition from STANDBY to ACTIVE mode occurs.
3. Modification of this register’s contents can only occur when device is STANDBY mode except CTRL_REG1 ACTIVE bit and CTRL_REG2 RST bit."
			#endregion
			);
			dictionary.Add(deviceID.MMA8491Q,
			#region
@"0x00	STATUS(1)(2)	0x00 STATUS: Data Status Register (Read Only)	R	6	Real time status | 	ZYXOW,ZOW,YOW,XOW,ZYXDR,ZDR,YDR,XDR
0x01	OUT_X_MSB(1)(2)	0x01 OUT_X_MSB: X_MSB Register (Read Only)	R	6	[7:0] are 8 MSBs of 14-bit sample.| 	XD13,XD12,XD11,XD10,XD9,XD8,XD7,XD6
0x02	OUT_X_LSB(1)(2)	0x02 OUT_X_LSB: X_LSB Register (Read Only)	R	6	[7:6] are 6 LSBs of 14-bit sample | 	XD5,XD4,XD3,XD2,XD1,XD0,0,0
0x03	OUT_Y_MSB(1)(2)	0x01 OUT_Y_MSB: Y_MSB Register (Read Only)	R	6	[7:0] are 8 MSBs of 14-bit sample.| 	YD13,YD12,YD11,YD10,YD9,YD8,YD7,YD6
0x04	OUT_Y_LSB(1)(2)	0x02 OUT_Y_LSB: Y_LSB Register (Read Only)	R	6	[7:6] are 6 LSBs of 14-bit sample | 	YD5,YD4,YD3,YD2,YD1,YD0,0,0
0x05	OUT_Z_MSB(1)(2)	0x01 OUT_Z_MSB: Z_MSB Register (Read Only)	R	6	[7:0] are 8 MSBs of 14-bit sample.| 	ZD13,ZD12,ZD11,ZD10,ZD9,ZD8,ZD7,ZD6
0x06	OUT_Z_LSB(1)(2)	0x02 OUT_Z_LSB: Z_LSB Register (Read Only)	R	6	[7:6] are 6 LSBs of 14-bit sample | 	ZD5,ZD4,ZD3,ZD2,ZD1,ZD0,0,0

^^
1. Register contents are preserved when transition from ACTIVE to STANDBY mode occurs.
2. Register contents are reset when transition from STANDBY to ACTIVE mode occurs.
3. Modification of this register’s contents can only occur when device is STANDBY mode except CTRL_REG1 ACTIVE bit and CTRL_REG2 RST bit."
			#endregion
			);

			_registerMapFile = dictionary;

			this.SuspendLayout();

			InitializeComponent();

			Styles.FormatForm(this);
			Styles.FormatInterruptPanel(uxPanelSettings);
			tbSleepCounter.BackColor = Color.Black;
			pSOS.BackColor = Color.Transparent;
			_registerView = new RegisterView(Register_Page.Width, Register_Page.Height);
			_registerView.BackColor = Color.LightSlateGray;

			VeyronControllerObj = (AccelController)accelController;

			int selectedIndex = TabTool.SelectedIndex;
			FullScaleValue = 0;
			GuiQueue = VeyronControllerObj.GetGUIQueue;
			CntrlQueue = VeyronControllerObj.GetControllerQueue;
			VeyronControllerObj.SetGuiHandle1(this);

			dv = new BoardComm();
			VeyronControllerObj.GetCommObject(ref dv);
			LoadResource();

			VeyronControllerObj.ControllerEvents += new AccelController.ControllerEventHandler(ControllerObj_ControllerEvents);

			uxAverageData.SelectedIndex = 0;
			_averageCounter = 0;
			uxStart.Checked = false;

			this.ResumeLayout(true);

			TmrActive.Enabled = true;
			TmrActive.Start();
			tmrDataDisplay.Enabled = true;
			tmrDataDisplay.Start();
		}

		private void btnAutoCal_Click(object sender, EventArgs e)
		{
			Cursor = Cursors.WaitCursor;
			int[] datapassed = new int[] { (int)DeviceID };
			VeyronControllerObj.AutoCalFlag = true;
			ControllerReqPacket reqName = new ControllerReqPacket();
			CreateNewTaskFromGUI(reqName, 0, 0x52, datapassed);
			Thread.Sleep(0x1b58);
			Cursor = Cursors.Default;
			rdo2g.Checked = true;
			ddlDataRate.SelectedIndex = 7;
			chkAnalogLowNoise.Checked = true;
			rdoOSHiResMode.Checked = true;
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
			int[] datapassed = new int[] { tbMFF1Threshold.Value };
			VeyronControllerObj.SetMFF1ThresholdFlag = true;
			ControllerReqPacket reqName = new ControllerReqPacket();
			CreateNewTaskFromGUI(reqName, 3, 0x38, datapassed);
			int[] numArray2 = new int[] { tbMFF1Debounce.Value };
			VeyronControllerObj.SetMFF1DebounceFlag = true;
			ControllerReqPacket packet2 = new ControllerReqPacket();
			CreateNewTaskFromGUI(packet2, 3, 0x39, numArray2);
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
			int[] datapassed = new int[] { tbPulseLatency.Value };
			VeyronControllerObj.SetPulseLatencyFlag = true;
			ControllerReqPacket reqName = new ControllerReqPacket();
			CreateNewTaskFromGUI(reqName, 6, 0x47, datapassed);
			int[] numArray2 = new int[] { tbPulse2ndPulseWin.Value };
			VeyronControllerObj.SetPulse2ndPulseWinFlag = true;
			ControllerReqPacket packet2 = new ControllerReqPacket();
			CreateNewTaskFromGUI(packet2, 6, 0x48, numArray2);
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
			int[] datapassed = new int[] { tbFirstPulseTimeLimit.Value };
			VeyronControllerObj.SetPulseFirstTimeLimitFlag = true;
			ControllerReqPacket reqName = new ControllerReqPacket();
			CreateNewTaskFromGUI(reqName, 6, 0x43, datapassed);
			btnSetFirstPulseTimeLimit.Enabled = false;
			btnResetFirstPulseTimeLimit.Enabled = true;
			tbFirstPulseTimeLimit.Enabled = false;
			lblFirstPulseTimeLimit.Enabled = false;
			lblFirstPulseTimeLimitms.Enabled = false;
			lblFirstTimeLimitVal.Enabled = false;
		}

		private void btnSetMode_Click(object sender, EventArgs e)
		{
			int[] datapassed = new int[1];
			if (rdoFill.Checked && rdoFill.Checked)
			{
				FIFOModeValue = 2;
			}
			datapassed[0] = FIFOModeValue;
			VeyronControllerObj.SetFIFOModeFlag = true;
			ControllerReqPacket reqName = new ControllerReqPacket();
			CreateNewTaskFromGUI(reqName, 7, 0x13, datapassed);
			p14b8bSelect.Enabled = false;
			rdoFIFO8bitDataDisplay.Enabled = false;
			rdoFIFO14bitDataDisplay.Enabled = false;
		}

		private void btnSetPLDebounce_Click(object sender, EventArgs e)
		{
			int[] datapassed = new int[] { tbPL.Value };
			VeyronControllerObj.SetPLDebounceFlag = true;
			ControllerReqPacket reqName = new ControllerReqPacket();
			CreateNewTaskFromGUI(reqName, 4, 0x1f, datapassed);
			btnSetPLDebounce.Enabled = false;
			btnResetPLDebounce.Enabled = true;
			tbPL.Enabled = false;
			lblPLbounceVal.Enabled = false;
			lblBouncems.Enabled = false;
			lblDebouncePL.Enabled = false;
		}

		private void btnSetPulseThresholds_Click_1(object sender, EventArgs e)
		{
			int[] datapassed = new int[] { tbPulseXThreshold.Value };
			VeyronControllerObj.SetPulseXThresholdFlag = true;
			ControllerReqPacket reqName = new ControllerReqPacket();
			CreateNewTaskFromGUI(reqName, 6, 0x44, datapassed);
			int[] numArray2 = new int[] { tbPulseYThreshold.Value };
			VeyronControllerObj.SetPulseYThresholdFlag = true;
			ControllerReqPacket packet2 = new ControllerReqPacket();
			CreateNewTaskFromGUI(packet2, 6, 0x45, numArray2);
			int[] numArray3 = new int[] { tbPulseZThreshold.Value };
			VeyronControllerObj.SetPulseZThresholdFlag = true;
			ControllerReqPacket packet3 = new ControllerReqPacket();
			CreateNewTaskFromGUI(packet3, 6, 70, numArray3);
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
			int[] datapassed = new int[] { tbSleepCounter.Value };
			VeyronControllerObj.SetSleepTimerFlag = true;
			ControllerReqPacket reqName = new ControllerReqPacket();
			CreateNewTaskFromGUI(reqName, 0, 9, datapassed);
			btnSetSleepTimer.Enabled = false;
			lblSleepTimer.Enabled = false;
			lblSleepTimerValue.Enabled = false;
			lblSleepms.Enabled = false;
			btnSleepTimerReset.Enabled = true;
		}

		private void btnSetTransient_Click(object sender, EventArgs e)
		{
			int[] datapassed = new int[] { tbTransThreshold.Value };
			VeyronControllerObj.SetTransThresholdFlag = true;
			ControllerReqPacket reqName = new ControllerReqPacket();
			CreateNewTaskFromGUI(reqName, 5, 0x24, datapassed);
			int[] numArray2 = new int[] { tbTransDebounce.Value };
			VeyronControllerObj.SetTransDebounceFlag = true;
			ControllerReqPacket packet2 = new ControllerReqPacket();
			CreateNewTaskFromGUI(packet2, 5, 0x25, numArray2);
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
			int[] datapassed = new int[] { tbTransThresholdNEW.Value };
			VeyronControllerObj.SetTransThresholdNEWFlag = true;
			ControllerReqPacket reqName = new ControllerReqPacket();
			CreateNewTaskFromGUI(reqName, 5, 0x30, datapassed);
			int[] numArray2 = new int[] { tbTransDebounceNEW.Value };
			VeyronControllerObj.SetTransDebounceNEWFlag = true;
			ControllerReqPacket packet2 = new ControllerReqPacket();
			CreateNewTaskFromGUI(packet2, 5, 0x31, numArray2);
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

		private void btnWatermark_Click(object sender, EventArgs e)
		{
			int[] datapassed = new int[] { tBWatermark.Value };
			VeyronControllerObj.SetFIFOWatermarkFlag = true;
			ControllerReqPacket reqName = new ControllerReqPacket();
			CreateNewTaskFromGUI(reqName, 7, 20, datapassed);
			btnWatermark.Enabled = false;
			btnResetWatermark.Enabled = true;
			lblWatermarkValue.Enabled = false;
			tBWatermark.Enabled = false;
			lblWatermark.Enabled = false;
		}

		private void btnWriteCal_Click(object sender, EventArgs e)
		{
			try
			{
				int num = Convert.ToInt32(txtCalX.Text);
				int num2 = Convert.ToInt32(txtCalY.Text);
				int num3 = Convert.ToInt32(txtCalZ.Text);
				int[] datapassed = new int[] { num, num2, num3 };
				VeyronControllerObj.SetCalFlag = true;
				ControllerReqPacket reqName = new ControllerReqPacket();
				CreateNewTaskFromGUI(reqName, 0, 12, datapassed);
			}
			catch (Exception)
			{
				MessageBox.Show("Please, enter a valid number");
			}
		}

		private void ButtonExportRegisters_Click(object sender, EventArgs e)
		{
			int[] datapassed = new int[] { 0 };
			ControllerReqPacket reqName = new ControllerReqPacket();
			CreateNewTaskFromGUI(reqName, 1, 0x10, datapassed);
		}

		private void changeAxisCount()
		{
			switch (DeviceID)
			{
				case deviceID.MMA8451Q:
					setCountsAxis(14);
					break;

				case deviceID.MMA8452Q:
					setCountsAxis(12);
					break;

				case deviceID.MMA8453Q:
					setCountsAxis(10);
					break;
			}
		}

		private void chkAnalogLowNoise_CheckedChanged(object sender, EventArgs e)
		{
			int[] datapassed = new int[1];
			int num = chkAnalogLowNoise.Checked ? 0xff : 0;
			datapassed[0] = num;
			VeyronControllerObj.SetEnableAnalogLowNoiseFlag = true;
			ControllerReqPacket reqName = new ControllerReqPacket();
			CreateNewTaskFromGUI(reqName, 0, 0x57, datapassed);
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
				int[] datapassed = new int[] { 0xff };
				VeyronControllerObj.SetMFF1XEFEFlag = true;
				ControllerReqPacket reqName = new ControllerReqPacket();
				CreateNewTaskFromGUI(reqName, 3, 0x35, datapassed);
				int[] numArray2 = new int[] { 0xff };
				VeyronControllerObj.SetMFF1YEFEFlag = true;
				ControllerReqPacket packet2 = new ControllerReqPacket();
				CreateNewTaskFromGUI(packet2, 3, 0x36, numArray2);
				int[] numArray3 = new int[] { 0xff };
				VeyronControllerObj.SetMFF1ZEFEFlag = true;
				ControllerReqPacket packet3 = new ControllerReqPacket();
				CreateNewTaskFromGUI(packet3, 3, 0x37, numArray3);
				rdoMFF1Or.Checked = false;
				rdoMFF1And.Checked = true;
				int[] numArray4 = new int[] { 0 };
				VeyronControllerObj.SetMFF1AndOrFlag = true;
				ControllerReqPacket packet4 = new ControllerReqPacket();
				CreateNewTaskFromGUI(packet4, 3, 0x33, numArray4);
				double num2 = 0.063;
				tbMFF1Threshold.Value = 4;
				double num = tbMFF1Threshold.Value * num2;
				lblMFF1ThresholdVal.Text = string.Format("{0:F2}", num);
				int[] numArray5 = new int[] { tbMFF1Threshold.Value };
				VeyronControllerObj.SetMFF1ThresholdFlag = true;
				ControllerReqPacket packet5 = new ControllerReqPacket();
				CreateNewTaskFromGUI(packet5, 0, 0x38, numArray5);
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
				int[] numArray6 = new int[] { tbMFF1Debounce.Value };
				VeyronControllerObj.SetMFF1DebounceFlag = true;
				ControllerReqPacket packet6 = new ControllerReqPacket();
				CreateNewTaskFromGUI(packet6, 3, 0x39, numArray6);
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
				int[] datapassed = new int[] { 0xff };
				VeyronControllerObj.SetMFF1XEFEFlag = true;
				ControllerReqPacket reqName = new ControllerReqPacket();
				CreateNewTaskFromGUI(reqName, 3, 0x35, datapassed);
				chkYEFE.Checked = true;
				int[] numArray2 = new int[] { 0xff };
				VeyronControllerObj.SetMFF1YEFEFlag = true;
				ControllerReqPacket packet2 = new ControllerReqPacket();
				CreateNewTaskFromGUI(packet2, 3, 0x36, numArray2);
				chkZEFE.Checked = true;
				int[] numArray3 = new int[] { 0xff };
				VeyronControllerObj.SetMFF1ZEFEFlag = true;
				ControllerReqPacket packet3 = new ControllerReqPacket();
				CreateNewTaskFromGUI(packet3, 3, 0x37, numArray3);
				rdoMFF1Or.Checked = true;
				rdoMFF1And.Checked = false;
				int[] numArray4 = new int[] { 0xff };
				VeyronControllerObj.SetMFF1AndOrFlag = true;
				ControllerReqPacket packet4 = new ControllerReqPacket();
				CreateNewTaskFromGUI(packet4, 0, 0x33, numArray4);
				double num2 = 0.063;
				tbMFF1Threshold.Value = 0x18;
				double num = tbMFF1Threshold.Value * num2;
				lblMFF1ThresholdVal.Text = string.Format("{0:F3}", num);
				int[] numArray5 = new int[] { tbMFF1Threshold.Value };
				VeyronControllerObj.SetMFF1ThresholdFlag = true;
				ControllerReqPacket packet5 = new ControllerReqPacket();
				CreateNewTaskFromGUI(packet5, 3, 0x38, numArray5);
				tbMFF1Debounce.Value = 0;
				lblMFF1DebounceVal.Text = string.Format("{0:F2}", 0);
				int[] numArray6 = new int[] { 0 };
				VeyronControllerObj.SetMFF1DebounceFlag = true;
				ControllerReqPacket packet6 = new ControllerReqPacket();
				CreateNewTaskFromGUI(packet6, 0, 0x39, numArray6);
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
				int[] datapassed = new int[] { 0xff };
				VeyronControllerObj.SetTransEnableXFlagFlag = true;
				ControllerReqPacket reqName = new ControllerReqPacket();
				CreateNewTaskFromGUI(reqName, 5, 0x27, datapassed);
				chkTransEnableYFlag.Checked = true;
				int[] numArray2 = new int[] { 0xff };
				VeyronControllerObj.SetTransEnableYFlagFlag = true;
				ControllerReqPacket packet2 = new ControllerReqPacket();
				CreateNewTaskFromGUI(packet2, 5, 40, numArray2);
				chkTransEnableZFlag.Checked = true;
				int[] numArray3 = new int[] { 0xff };
				VeyronControllerObj.SetTransEnableZFlagFlag = true;
				ControllerReqPacket packet3 = new ControllerReqPacket();
				CreateNewTaskFromGUI(packet3, 5, 0x29, numArray3);
				tbTransThreshold.Value = 8;
				double num2 = 0.063;
				double num = tbTransThreshold.Value * num2;
				lblTransThresholdVal.Text = string.Format("{0:F2}", num);
				int[] numArray4 = new int[] { tbTransThreshold.Value };
				VeyronControllerObj.SetTransThresholdFlag = true;
				ControllerReqPacket packet4 = new ControllerReqPacket();
				CreateNewTaskFromGUI(packet4, 5, 0x24, numArray4);
				tbTransDebounce.Value = 0;
				double num3 = tbTransDebounce.Value;
				lblTransDebouncems.Text = "ms";
				lblTransDebounceVal.Text = string.Format("{0:F1}", num3);
				int[] numArray5 = new int[] { tbTransDebounce.Value };
				VeyronControllerObj.SetTransDebounceFlag = true;
				ControllerReqPacket packet5 = new ControllerReqPacket();
				CreateNewTaskFromGUI(packet5, 5, 0x25, numArray5);
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
				ControllerReqPacket reqName = new ControllerReqPacket();
				CreateNewTaskFromGUI(reqName, 5, 0x2c, datapassed);
				chkTransEnableYFlagNEW.Checked = true;
				int[] numArray2 = new int[] { 0xff };
				VeyronControllerObj.SetTransEnableYFlagNEWFlag = true;
				ControllerReqPacket packet2 = new ControllerReqPacket();
				CreateNewTaskFromGUI(packet2, 5, 0x2d, numArray2);
				chkTransEnableZFlagNEW.Checked = true;
				int[] numArray3 = new int[] { 0xff };
				VeyronControllerObj.SetTransEnableZFlagNEWFlag = true;
				ControllerReqPacket packet3 = new ControllerReqPacket();
				CreateNewTaskFromGUI(packet3, 5, 0x2e, numArray3);
				tbTransThresholdNEW.Value = 8;
				double num2 = 0.063;
				double num = tbTransThresholdNEW.Value * num2;
				lblTransThresholdValNEW.Text = string.Format("{0:F2}", num);
				int[] numArray4 = new int[] { tbTransThresholdNEW.Value };
				VeyronControllerObj.SetTransThresholdNEWFlag = true;
				ControllerReqPacket packet4 = new ControllerReqPacket();
				CreateNewTaskFromGUI(packet4, 5, 0x30, numArray4);
				tbTransDebounceNEW.Value = 0;
				double num3 = tbTransDebounceNEW.Value;
				lblTransDebouncemsNEW.Text = "ms";
				lblTransDebounceValNEW.Text = string.Format("{0:F1}", num3);
				int[] numArray5 = new int[] { tbTransDebounceNEW.Value };
				VeyronControllerObj.SetTransDebounceNEWFlag = true;
				ControllerReqPacket packet5 = new ControllerReqPacket();
				CreateNewTaskFromGUI(packet5, 5, 0x31, numArray5);
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
			if (rdoDisabled.Checked)
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
				p5.Enabled = false;
				datapassed[0] = num;
				VeyronControllerObj.SetFIFOModeFlag = true;
				ControllerReqPacket reqName = new ControllerReqPacket();
				CreateNewTaskFromGUI(reqName, 7, 0x13, datapassed);
			}
			if (!(rdoDisabled.Checked || !rdoStandby.Checked))
			{
				p4.Enabled = true;
				pTriggerMode.Enabled = true;
				p5.Enabled = false;
				p14b8bSelect.Enabled = false;
				rdoFIFO14bitDataDisplay.Enabled = false;
				rdoFIFO8bitDataDisplay.Enabled = false;
			}
			if (!rdoDisabled.Checked && rdoActive.Checked)
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
				{
					numArray2[3] = 0;
				}
				else if (rdoCircular.Checked)
				{
					numArray2[3] = 1;
				}
				else if (rdoTriggerMode.Checked)
				{
					numArray2[3] = 2;
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
				ControllerReqPacket packet2 = new ControllerReqPacket();
				CreateNewTaskFromGUI(packet2, 7, 80, numArray2);
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
			ControllerReqPacket reqName = new ControllerReqPacket();
			CreateNewTaskFromGUI(reqName, 0, 7, datapassed);
		}

		private void chkEnablePL_CheckedChanged(object sender, EventArgs e)
		{
			int num;
			if (chkEnablePL.Checked)
			{
				chkPLDefaultSettings.Enabled = true;
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
			VeyronControllerObj.SetEnablePLFlag = true;
			ControllerReqPacket reqName = new ControllerReqPacket();
			CreateNewTaskFromGUI(reqName, 4, 0x19, datapassed);
		}

		private void chkEnableX_CheckedChanged(object sender, EventArgs e)
		{
			WaveformPlotCollection plots = MainScreenGraph.Plots;
			if (chkEnableX.Checked)
			{
				plots[0].Visible = true;
			}
			else
			{
				plots[0].Visible = false;
			}
		}

		private void chkEnableXMFF_CheckedChanged(object sender, EventArgs e)
		{
			WaveformPlotCollection plots = MFFGraph.Plots;
			if (chkEnableXMFF.Checked)
			{
				plots[0].Visible = true;
			}
			else
			{
				plots[0].Visible = false;
			}
		}

		private void chkEnableYAxis_CheckedChanged(object sender, EventArgs e)
		{
			WaveformPlotCollection plots = MainScreenGraph.Plots;
			if (chkEnableYAxis.Checked)
			{
				plots[1].Visible = true;
			}
			else
			{
				plots[1].Visible = false;
			}
		}

		private void chkEnableYMFF_CheckedChanged(object sender, EventArgs e)
		{
			WaveformPlotCollection plots = MFFGraph.Plots;
			if (chkEnableYMFF.Checked)
			{
				plots[1].Visible = true;
			}
			else
			{
				plots[1].Visible = false;
			}
		}

		private void chkEnableZAxis_CheckedChanged(object sender, EventArgs e)
		{
			WaveformPlotCollection plots = MainScreenGraph.Plots;
			if (chkEnableZAxis.Checked)
			{
				plots[2].Visible = true;
			}
			else
			{
				plots[2].Visible = false;
			}
		}

		private void chkEnableZMFF_CheckedChanged(object sender, EventArgs e)
		{
			WaveformPlotCollection plots = MFFGraph.Plots;
			if (chkEnableZMFF.Checked)
			{
				plots[2].Visible = true;
			}
			else
			{
				plots[2].Visible = false;
			}
		}

		private void chkEnIntASleep_CheckedChanged(object sender, EventArgs e)
		{
			int[] datapassed = new int[2];
			int num = chkEnIntASleep.Checked ? 0xff : 0;
			datapassed[0] = num;
			datapassed[1] = 0x80;
			VeyronControllerObj.SetIntsEnableFlag = true;
			ControllerReqPacket reqName = new ControllerReqPacket();
			CreateNewTaskFromGUI(reqName, 0, 13, datapassed);
		}

		private void chkEnIntDR_CheckedChanged(object sender, EventArgs e)
		{
			int[] datapassed = new int[2];
			int num = chkEnIntDR.Checked ? 0xff : 0;
			datapassed[0] = num;
			datapassed[1] = 1;
			VeyronControllerObj.SetIntsEnableFlag = true;
			ControllerReqPacket reqName = new ControllerReqPacket();
			CreateNewTaskFromGUI(reqName, 0, 13, datapassed);
		}

		private void chkEnIntFIFO_CheckedChanged(object sender, EventArgs e)
		{
			int[] datapassed = new int[2];
			int num = chkEnIntFIFO.Checked ? 0xff : 0;
			datapassed[0] = num;
			datapassed[1] = 0x40;
			VeyronControllerObj.SetIntsEnableFlag = true;
			ControllerReqPacket reqName = new ControllerReqPacket();
			CreateNewTaskFromGUI(reqName, 0, 13, datapassed);
		}

		private void chkEnIntMFF1_CheckedChanged(object sender, EventArgs e)
		{
			int[] datapassed = new int[2];
			int num = chkEnIntMFF1.Checked ? 0xff : 0;
			datapassed[0] = num;
			datapassed[1] = 4;
			VeyronControllerObj.SetIntsEnableFlag = true;
			ControllerReqPacket reqName = new ControllerReqPacket();
			CreateNewTaskFromGUI(reqName, 0, 13, datapassed);
		}

		private void chkEnIntPL_CheckedChanged(object sender, EventArgs e)
		{
			int[] datapassed = new int[2];
			int num = chkEnIntPL.Checked ? 0xff : 0;
			datapassed[0] = num;
			datapassed[1] = 0x10;
			VeyronControllerObj.SetIntsEnableFlag = true;
			ControllerReqPacket reqName = new ControllerReqPacket();
			CreateNewTaskFromGUI(reqName, 0, 13, datapassed);
		}

		private void chkEnIntPulse_CheckedChanged(object sender, EventArgs e)
		{
			int[] datapassed = new int[2];
			int num = chkEnIntPulse.Checked ? 0xff : 0;
			datapassed[0] = num;
			datapassed[1] = 8;
			VeyronControllerObj.SetIntsEnableFlag = true;
			ControllerReqPacket reqName = new ControllerReqPacket();
			CreateNewTaskFromGUI(reqName, 0, 13, datapassed);
		}

		private void chkEnIntTrans_CheckedChanged(object sender, EventArgs e)
		{
			int[] datapassed = new int[2];
			int num = chkEnIntTrans.Checked ? 0xff : 0;
			datapassed[0] = num;
			datapassed[1] = 0x20;
			VeyronControllerObj.SetIntsEnableFlag = true;
			ControllerReqPacket reqName = new ControllerReqPacket();
			CreateNewTaskFromGUI(reqName, 0, 13, datapassed);
		}

		private void chkEnIntTrans1_CheckedChanged(object sender, EventArgs e)
		{
			int[] datapassed = new int[2];
			int num = chkEnIntTrans1.Checked ? 0xff : 0;
			datapassed[0] = num;
			datapassed[1] = 2;
			VeyronControllerObj.SetIntsEnableFlag = true;
			ControllerReqPacket reqName = new ControllerReqPacket();
			CreateNewTaskFromGUI(reqName, 0, 13, datapassed);
		}

		private void chkHPFDataOut_CheckedChanged(object sender, EventArgs e)
		{
			int[] datapassed = new int[1];
			int num = chkHPFDataOut.Checked ? 0xff : 0;
			datapassed[0] = num;
			VeyronControllerObj.SetHPFDataOutFlag = true;
			ControllerReqPacket reqName = new ControllerReqPacket();
			CreateNewTaskFromGUI(reqName, 0, 0x56, datapassed);
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
			CreateNewTaskFromGUI(reqName, 3, 0x34, datapassed);
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
				int[] datapassed = new int[] { ddlZLockA.SelectedIndex };
				VeyronControllerObj.SetZLockAFlag = true;
				ControllerReqPacket reqName = new ControllerReqPacket();
				CreateNewTaskFromGUI(reqName, 4, 0x1a, datapassed);
				ddlZLockA.Enabled = false;
				int[] numArray2 = new int[] { ddlBFTripA.SelectedIndex };
				VeyronControllerObj.SetBFTripAFlag = true;
				ControllerReqPacket packet2 = new ControllerReqPacket();
				CreateNewTaskFromGUI(packet2, 4, 0x1b, numArray2);
				ddlBFTripA.Enabled = false;
				int[] numArray3 = new int[1];
				PL_index = ddlPLTripA.SelectedIndex;
				numArray3[0] = PL_index;
				VeyronControllerObj.SetPLTripAFlag = true;
				ControllerReqPacket packet3 = new ControllerReqPacket();
				CreateNewTaskFromGUI(packet3, 4, 0x1c, numArray3);
				ddlPLTripA.Enabled = false;
				int[] numArray4 = new int[1];
				Hysteresis_index = ddlHysteresisA.SelectedIndex;
				numArray4[0] = Hysteresis_index;
				VeyronControllerObj.SetHysteresisFlag = true;
				ControllerReqPacket packet4 = new ControllerReqPacket();
				CreateNewTaskFromGUI(packet4, 4, 0x1d, numArray4);
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
				double num = tbPL.Value * DR_timestep;
				lblBouncems.Text = "ms";
				lblPLbounceVal.Text = string.Format("{0:F1}", num);
				int[] numArray5 = new int[] { tbPL.Value };
				VeyronControllerObj.SetPLDebounceFlag = true;
				ControllerReqPacket packet5 = new ControllerReqPacket();
				CreateNewTaskFromGUI(packet5, 4, 0x1f, numArray5);
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
			int num = chkPulseEnableLatch.Checked ? 0xff : 0;
			int[] datapassed = new int[] { num };
			VeyronControllerObj.SetPulseLatchFlag = true;
			ControllerReqPacket reqName = new ControllerReqPacket();
			CreateNewTaskFromGUI(reqName, 6, 60, datapassed);
		}

		private void chkPulseEnableXDP_CheckedChanged_1(object sender, EventArgs e)
		{
			int num = chkPulseEnableXDP.Checked ? 0xff : 0;
			int[] datapassed = new int[] { num };
			VeyronControllerObj.SetPulseXDPFlag = true;
			ControllerReqPacket reqName = new ControllerReqPacket();
			CreateNewTaskFromGUI(reqName, 6, 0x41, datapassed);
		}

		private void chkPulseEnableXSP_CheckedChanged_1(object sender, EventArgs e)
		{
			int num = chkPulseEnableXSP.Checked ? 0xff : 0;
			int[] datapassed = new int[] { num };
			VeyronControllerObj.SetPulseXSPFlag = true;
			ControllerReqPacket reqName = new ControllerReqPacket();
			CreateNewTaskFromGUI(reqName, 6, 0x42, datapassed);
		}

		private void chkPulseEnableYDP_CheckedChanged_1(object sender, EventArgs e)
		{
			int num = chkPulseEnableYDP.Checked ? 0xff : 0;
			int[] datapassed = new int[] { num };
			VeyronControllerObj.SetPulseYDPFlag = true;
			ControllerReqPacket reqName = new ControllerReqPacket();
			CreateNewTaskFromGUI(reqName, 6, 0x3f, datapassed);
		}

		private void chkPulseEnableYSP_CheckedChanged_1(object sender, EventArgs e)
		{
			int num = chkPulseEnableYSP.Checked ? 0xff : 0;
			int[] datapassed = new int[] { num };
			VeyronControllerObj.SetPulseYSPFlag = true;
			ControllerReqPacket reqName = new ControllerReqPacket();
			CreateNewTaskFromGUI(reqName, 6, 0x40, datapassed);
		}

		private void chkPulseEnableZDP_CheckedChanged(object sender, EventArgs e)
		{
			int num = chkPulseEnableZDP.Checked ? 0xff : 0;
			int[] datapassed = new int[] { num };
			VeyronControllerObj.SetPulseZDPFlag = true;
			ControllerReqPacket reqName = new ControllerReqPacket();
			CreateNewTaskFromGUI(reqName, 6, 0x3d, datapassed);
		}

		private void chkPulseEnableZSP_CheckedChanged_1(object sender, EventArgs e)
		{
			int num = chkPulseEnableZSP.Checked ? 0xff : 0;
			int[] datapassed = new int[] { num };
			VeyronControllerObj.SetPulseZSPFlag = true;
			ControllerReqPacket reqName = new ControllerReqPacket();
			CreateNewTaskFromGUI(reqName, 6, 0x3e, datapassed);
		}

		private void chkPulseHPFBypass_CheckedChanged(object sender, EventArgs e)
		{
			int[] datapassed = new int[1];
			int num = chkPulseHPFBypass.Checked ? 0xff : 0;
			datapassed[0] = num;
			VeyronControllerObj.SetPulseHPFBypassFlag = true;
			ControllerReqPacket reqName = new ControllerReqPacket();
			CreateNewTaskFromGUI(reqName, 6, 90, datapassed);
		}

		private void chkPulseIgnorLatentPulses_CheckedChanged_1(object sender, EventArgs e)
		{
			int num = chkPulseIgnorLatentPulses.Checked ? 0xff : 0;
			int[] datapassed = new int[] { num };
			VeyronControllerObj.SetPulseDPAFlag = true;
			ControllerReqPacket reqName = new ControllerReqPacket();
			CreateNewTaskFromGUI(reqName, 6, 0x3b, datapassed);
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
			VeyronControllerObj.SetPulseLPFEnableFlag = true;
			ControllerReqPacket reqName = new ControllerReqPacket();
			CreateNewTaskFromGUI(reqName, 6, 0x59, datapassed);
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

		private void chkSelfTest_CheckedChanged(object sender, EventArgs e)
		{
			int[] datapassed = new int[1];
			int num = chkSelfTest.Checked ? 0xff : 0;
			datapassed[0] = num;
			VeyronControllerObj.SetSelfTestFlag = true;
			ControllerReqPacket reqName = new ControllerReqPacket();
			CreateNewTaskFromGUI(reqName, 0, 10, datapassed);
		}

		private void chkTransBypassHPF_CheckedChanged(object sender, EventArgs e)
		{
			int num = chkTransBypassHPF.Checked ? 0xff : 0;
			int[] datapassed = new int[] { num };
			VeyronControllerObj.SetTransBypassHPFFlag = true;
			ControllerReqPacket reqName = new ControllerReqPacket();
			CreateNewTaskFromGUI(reqName, 5, 0x23, datapassed);
		}

		private void chkTransBypassHPFNEW_CheckedChanged(object sender, EventArgs e)
		{
			int num = chkTransBypassHPFNEW.Checked ? 0xff : 0;
			int[] datapassed = new int[] { num };
			VeyronControllerObj.SetTransBypassHPFNEWFlag = true;
			ControllerReqPacket reqName = new ControllerReqPacket();
			CreateNewTaskFromGUI(reqName, 5, 0x2b, datapassed);
		}

		private void chkTransEnableLatch_CheckedChanged(object sender, EventArgs e)
		{
			int num = chkTransEnableLatch.Checked ? 0xff : 0;
			int[] datapassed = new int[] { num };
			VeyronControllerObj.SetTransEnableLatchFlag = true;
			ControllerReqPacket reqName = new ControllerReqPacket();
			CreateNewTaskFromGUI(reqName, 5, 0x2a, datapassed);
		}

		private void chkTransEnableLatchNEW_CheckedChanged_1(object sender, EventArgs e)
		{
			int num = chkTransEnableLatchNEW.Checked ? 0xff : 0;
			int[] datapassed = new int[] { num };
			VeyronControllerObj.SetTransEnableLatchNEWFlag = true;
			ControllerReqPacket reqName = new ControllerReqPacket();
			CreateNewTaskFromGUI(reqName, 5, 0x2f, datapassed);
		}

		private void chkTransEnableXFlag_CheckedChanged_1(object sender, EventArgs e)
		{
			int num = chkTransEnableXFlag.Checked ? 0xff : 0;
			int[] datapassed = new int[] { num };
			VeyronControllerObj.SetTransEnableXFlagFlag = true;
			ControllerReqPacket reqName = new ControllerReqPacket();
			CreateNewTaskFromGUI(reqName, 5, 0x27, datapassed);
		}

		private void chkTransEnableXFlagNEW_CheckedChanged_1(object sender, EventArgs e)
		{
			int num = chkTransEnableXFlagNEW.Checked ? 0xff : 0;
			int[] datapassed = new int[] { num };
			VeyronControllerObj.SetTransEnableXFlagNEWFlag = true;
			ControllerReqPacket reqName = new ControllerReqPacket();
			CreateNewTaskFromGUI(reqName, 5, 0x2c, datapassed);
		}

		private void chkTransEnableYFlag_CheckedChanged(object sender, EventArgs e)
		{
			int num = chkTransEnableYFlag.Checked ? 0xff : 0;
			int[] datapassed = new int[] { num };
			VeyronControllerObj.SetTransEnableYFlagFlag = true;
			ControllerReqPacket reqName = new ControllerReqPacket();
			CreateNewTaskFromGUI(reqName, 5, 40, datapassed);
		}

		private void chkTransEnableYFlagNEW_CheckedChanged_1(object sender, EventArgs e)
		{
			int num = chkTransEnableYFlagNEW.Checked ? 0xff : 0;
			int[] datapassed = new int[] { num };
			VeyronControllerObj.SetTransEnableYFlagNEWFlag = true;
			ControllerReqPacket reqName = new ControllerReqPacket();
			CreateNewTaskFromGUI(reqName, 5, 0x2d, datapassed);
		}

		private void chkTransEnableZFlag_CheckedChanged(object sender, EventArgs e)
		{
			int num = chkTransEnableZFlag.Checked ? 0xff : 0;
			int[] datapassed = new int[] { num };
			VeyronControllerObj.SetTransEnableZFlagFlag = true;
			ControllerReqPacket reqName = new ControllerReqPacket();
			CreateNewTaskFromGUI(reqName, 5, 0x29, datapassed);
		}

		private void chkTransEnableZFlagNEW_CheckedChanged_1(object sender, EventArgs e)
		{
			int num = chkTransEnableZFlagNEW.Checked ? 0xff : 0;
			int[] datapassed = new int[] { num };
			VeyronControllerObj.SetTransEnableZFlagNEWFlag = true;
			ControllerReqPacket reqName = new ControllerReqPacket();
			CreateNewTaskFromGUI(reqName, 5, 0x2e, datapassed);
		}

		private void chkTriggerLP_CheckedChanged(object sender, EventArgs e)
		{
			int num = chkTriggerLP.Checked ? 0xff : 0;
			int[] datapassed = new int[] { num };
			VeyronControllerObj.SetTrigLPFlag = true;
			ControllerReqPacket reqName = new ControllerReqPacket();
			CreateNewTaskFromGUI(reqName, 7, 0x16, datapassed);
		}

		private void chkTriggerMFF_CheckedChanged(object sender, EventArgs e)
		{
			int num = chkTriggerMFF.Checked ? 0xff : 0;
			int[] datapassed = new int[] { num };
			VeyronControllerObj.SetTrigMFFFlag = true;
			ControllerReqPacket reqName = new ControllerReqPacket();
			CreateNewTaskFromGUI(reqName, 7, 0x18, datapassed);
		}

		private void chkTriggerPulse_CheckedChanged(object sender, EventArgs e)
		{
			int num = chkTriggerPulse.Checked ? 0xff : 0;
			int[] datapassed = new int[] { num };
			VeyronControllerObj.SetTrigPulseFlag = true;
			ControllerReqPacket reqName = new ControllerReqPacket();
			CreateNewTaskFromGUI(reqName, 7, 0x17, datapassed);
		}

		private void chkTrigTrans_CheckedChanged(object sender, EventArgs e)
		{
			int num = chkTrigTrans.Checked ? 0xff : 0;
			int[] datapassed = new int[] { num };
			VeyronControllerObj.SetTrigTransFlag = true;
			ControllerReqPacket reqName = new ControllerReqPacket();
			CreateNewTaskFromGUI(reqName, 7, 0x15, datapassed);
		}

		private void chkWakeFIFOGate_CheckedChanged(object sender, EventArgs e)
		{
			int[] datapassed = new int[2];
			int num = chkWakeFIFOGate.Checked ? 0xff : 0;
			datapassed[0] = num;
			datapassed[1] = 0x80;
			VeyronControllerObj.SetWakeFromSleepFlag = true;
			ControllerReqPacket reqName = new ControllerReqPacket();
			CreateNewTaskFromGUI(reqName, 0, 6, datapassed);
		}

		private void chkWakeLP_CheckedChanged(object sender, EventArgs e)
		{
			int[] datapassed = new int[2];
			int num = chkWakeLP.Checked ? 0xff : 0;
			datapassed[0] = num;
			datapassed[1] = 0x20;
			VeyronControllerObj.SetWakeFromSleepFlag = true;
			ControllerReqPacket reqName = new ControllerReqPacket();
			CreateNewTaskFromGUI(reqName, 0, 6, datapassed);
		}

		private void chkWakeMFF1_CheckedChanged(object sender, EventArgs e)
		{
			int[] datapassed = new int[2];
			int num = chkWakeMFF1.Checked ? 0xff : 0;
			datapassed[0] = num;
			datapassed[1] = 8;
			VeyronControllerObj.SetWakeFromSleepFlag = true;
			ControllerReqPacket reqName = new ControllerReqPacket();
			CreateNewTaskFromGUI(reqName, 0, 6, datapassed);
		}

		private void chkWakePulse_CheckedChanged(object sender, EventArgs e)
		{
			int[] datapassed = new int[2];
			int num = chkWakePulse.Checked ? 0xff : 0;
			datapassed[0] = num;
			datapassed[1] = 0x10;
			VeyronControllerObj.SetWakeFromSleepFlag = true;
			ControllerReqPacket reqName = new ControllerReqPacket();
			CreateNewTaskFromGUI(reqName, 0, 6, datapassed);
		}

		private void chkWakeTrans_CheckedChanged(object sender, EventArgs e)
		{
			int[] datapassed = new int[2];
			int num = chkWakeTrans.Checked ? 0xff : 0;
			datapassed[0] = num;
			datapassed[1] = 0x40;
			VeyronControllerObj.SetWakeFromSleepFlag = true;
			ControllerReqPacket reqName = new ControllerReqPacket();
			CreateNewTaskFromGUI(reqName, 0, 6, datapassed);
		}

		private void chkWakeTrans1_CheckedChanged(object sender, EventArgs e)
		{
			int[] datapassed = new int[2];
			int num = chkWakeTrans1.Checked ? 0xff : 0;
			datapassed[0] = num;
			datapassed[1] = 4;
			VeyronControllerObj.SetWakeFromSleepFlag = true;
			ControllerReqPacket reqName = new ControllerReqPacket();
			CreateNewTaskFromGUI(reqName, 0, 6, datapassed);
		}

		private void chkXEFE_CheckedChanged(object sender, EventArgs e)
		{
			int num = chkXEFE.Checked ? 0xff : 0;
			int[] datapassed = new int[] { num };
			VeyronControllerObj.SetMFF1XEFEFlag = true;
			ControllerReqPacket reqName = new ControllerReqPacket();
			CreateNewTaskFromGUI(reqName, 3, 0x35, datapassed);
		}

		private void chkXEnableTrans_CheckedChanged(object sender, EventArgs e)
		{
			WaveformPlotCollection plots = gphXYZ.Plots;
			if (chkXEnableTrans.Checked)
			{
				plots[0].Visible = true;
			}
			else
			{
				plots[0].Visible = false;
			}
		}

		private void chkYEFE_CheckedChanged(object sender, EventArgs e)
		{
			int num = chkYEFE.Checked ? 0xff : 0;
			int[] datapassed = new int[] { num };
			VeyronControllerObj.SetMFF1YEFEFlag = true;
			ControllerReqPacket reqName = new ControllerReqPacket();
			CreateNewTaskFromGUI(reqName, 3, 0x36, datapassed);
		}

		private void chkYEnableTrans_CheckedChanged(object sender, EventArgs e)
		{
			WaveformPlotCollection plots = gphXYZ.Plots;
			if (chkYEnableTrans.Checked)
			{
				plots[1].Visible = true;
			}
			else
			{
				plots[1].Visible = false;
			}
		}

		private void chkZEFE_CheckedChanged(object sender, EventArgs e)
		{
			int num = chkZEFE.Checked ? 0xff : 0;
			int[] datapassed = new int[] { num };
			VeyronControllerObj.SetMFF1ZEFEFlag = true;
			ControllerReqPacket reqName = new ControllerReqPacket();
			CreateNewTaskFromGUI(reqName, 3, 0x37, datapassed);
		}

		private void chkZEnableTrans_CheckedChanged(object sender, EventArgs e)
		{
			WaveformPlotCollection plots = gphXYZ.Plots;
			if (chkZEnableTrans.Checked)
			{
				plots[2].Visible = true;
			}
			else
			{
				plots[2].Visible = false;
			}
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
			XYZGees gees = new XYZGees();
			switch (((int)evt))
			{
				case 1:
					UpdateRaw8bVariables(o);
					break;

				case 2:
					switch (DeviceID)
					{
						case deviceID.MMA8491Q:
							UpdateRaw14bVariables(o);
							return;

						case deviceID.MMA8451Q:
							UpdateRaw14bVariables(o);
							return;

						case deviceID.MMA8452Q:
							UpdateRaw12bVariables(o);
							return;

						case deviceID.MMA8453Q:
							UpdateRaw10bVariables(o);
							return;

						case deviceID.MMA8652FC:
							UpdateRaw12bVariables(o);
							return;

						case deviceID.MMA8653FC:
							UpdateRaw10bVariables(o);
							return;
					}
					break;

				case 4:
					GuiQueue.Enqueue((GUIUpdatePacket)o);
					break;

				case 7:
					GuiQueue.Enqueue((GUIUpdatePacket)o);
					break;

				case 9:
					GuiQueue.Enqueue((GUIUpdatePacket)o);
					break;
			}
		}

		private void CreateNewTaskFromGUI(ControllerReqPacket ReqName, int FormNum, int TaskNum, int[] datapassed)
		{
			ReqName.FormID = FormNum;
			ReqName.TaskID = TaskNum;
			ReqName.PayLoad.Enqueue(datapassed);
			CntrlQueue.Enqueue(ReqName);
		}

		private void ddlBFTripA_SelectedIndexChanged(object sender, EventArgs e)
		{
			int[] datapassed = new int[] { ddlBFTripA.SelectedIndex };
			VeyronControllerObj.SetBFTripAFlag = true;
			ControllerReqPacket reqName = new ControllerReqPacket();
			CreateNewTaskFromGUI(reqName, 4, 0x1b, datapassed);
		}

		private void ddlDataRate_SelectedIndexChanged(object sender, EventArgs e)
		{
			double num2;
			rdoDefaultSP.Checked = false;
			rdoDefaultSPDP.Checked = false;
			chkPLDefaultSettings.Checked = false;
			int[] datapassed = new int[] { ddlDataRate.SelectedIndex };
			VeyronControllerObj.SetDataRateFlag = true;
			ControllerReqPacket reqName = new ControllerReqPacket();
			CreateNewTaskFromGUI(reqName, 0, 5, datapassed);
			int selectedIndex = ddlHPFilter.SelectedIndex;
			tbSleepCounter_Scroll(this, null);
			switch (ddlDataRate.SelectedIndex)
			{
				case 0:
					DR_timestep = 1.25;
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
					if (WakeOSMode == 3)
					{
						DR_PulseTimeStepNoLPF = 1.25;
						ddlHPFilter.Items.Clear();
						ddlHPFilter.Items.Add("8Hz");
						ddlHPFilter.Items.Add("4Hz");
						ddlHPFilter.Items.Add("2Hz");
						ddlHPFilter.Items.Add("1Hz");
						ddlHPFilter.SelectedIndex = selectedIndex;
						break;
					}
					DR_PulseTimeStepNoLPF = 0.625;
					ddlHPFilter.Items.Clear();
					ddlHPFilter.Items.Add("16Hz");
					ddlHPFilter.Items.Add("8Hz");
					ddlHPFilter.Items.Add("4Hz");
					ddlHPFilter.Items.Add("2Hz");
					ddlHPFilter.SelectedIndex = selectedIndex;
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
							goto Label_0FE3;

						case 3:
							DR_timestep = 5.0;
							DR_PulseTimeStepNoLPF = 2.5;
							ddlHPFilter.Items.Clear();
							ddlHPFilter.Items.Add("4Hz");
							ddlHPFilter.Items.Add("2Hz");
							ddlHPFilter.Items.Add("1Hz");
							ddlHPFilter.Items.Add("0.5Hz");
							ddlHPFilter.SelectedIndex = selectedIndex;
							goto Label_0FE3;
					}
					DR_timestep = 5.0;
					DR_PulseTimeStepNoLPF = 1.25;
					ddlHPFilter.Items.Clear();
					ddlHPFilter.Items.Add("8Hz");
					ddlHPFilter.Items.Add("4Hz");
					ddlHPFilter.Items.Add("2Hz");
					ddlHPFilter.Items.Add("1Hz");
					ddlHPFilter.SelectedIndex = selectedIndex;
					break;

				case 3:
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
							goto Label_0FE3;

						case 3:
							DR_timestep = 10.0;
							DR_PulseTimeStepNoLPF = 5.0;
							ddlHPFilter.Items.Clear();
							ddlHPFilter.Items.Add("2Hz");
							ddlHPFilter.Items.Add("1Hz");
							ddlHPFilter.Items.Add("0.5Hz");
							ddlHPFilter.Items.Add("0.25Hz");
							ddlHPFilter.SelectedIndex = selectedIndex;
							goto Label_0FE3;
					}
					DR_timestep = 10.0;
					DR_PulseTimeStepNoLPF = 2.5;
					ddlHPFilter.Items.Clear();
					ddlHPFilter.Items.Add("4Hz");
					ddlHPFilter.Items.Add("2Hz");
					ddlHPFilter.Items.Add("1Hz");
					ddlHPFilter.Items.Add("0.5Hz");
					ddlHPFilter.SelectedIndex = selectedIndex;
					break;

				case 4:
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
							goto Label_0FE3;

						case 3:
							DR_timestep = 20.0;
							DR_PulseTimeStepNoLPF = 10.0;
							ddlHPFilter.Items.Clear();
							ddlHPFilter.Items.Add("1Hz");
							ddlHPFilter.Items.Add("0.5Hz");
							ddlHPFilter.Items.Add("0.25Hz");
							ddlHPFilter.Items.Add("0.125Hz");
							ddlHPFilter.SelectedIndex = selectedIndex;
							goto Label_0FE3;
					}
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
							goto Label_0FE3;

						case 1:
							DR_timestep = 80.0;
							DR_PulseTimeStepNoLPF = 20.0;
							ddlHPFilter.Items.Clear();
							ddlHPFilter.Items.Add("0.5Hz");
							ddlHPFilter.Items.Add("0.25Hz");
							ddlHPFilter.Items.Add("0.125Hz");
							ddlHPFilter.Items.Add("0.063Hz");
							ddlHPFilter.SelectedIndex = selectedIndex;
							goto Label_0FE3;

						case 2:
							DR_timestep = 2.5;
							DR_PulseTimeStepNoLPF = 0.625;
							ddlHPFilter.Items.Clear();
							ddlHPFilter.Items.Add("16Hz");
							ddlHPFilter.Items.Add("8Hz");
							ddlHPFilter.Items.Add("4Hz");
							ddlHPFilter.Items.Add("2Hz");
							ddlHPFilter.SelectedIndex = selectedIndex;
							goto Label_0FE3;

						case 3:
							DR_timestep = 80.0;
							DR_PulseTimeStepNoLPF = 40.0;
							ddlHPFilter.Items.Clear();
							ddlHPFilter.Items.Add("0.25Hz");
							ddlHPFilter.Items.Add("0.125Hz");
							ddlHPFilter.Items.Add("0.063Hz");
							ddlHPFilter.Items.Add("0.031Hz");
							ddlHPFilter.SelectedIndex = selectedIndex;
							goto Label_0FE3;
					}
					break;

				case 6:
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
							goto Label_0FE3;

						case 1:
							DR_timestep = 80.0;
							DR_PulseTimeStepNoLPF = 20.0;
							ddlHPFilter.Items.Clear();
							ddlHPFilter.Items.Add("0.5Hz");
							ddlHPFilter.Items.Add("0.25Hz");
							ddlHPFilter.Items.Add("0.125Hz");
							ddlHPFilter.Items.Add("0.063Hz");
							ddlHPFilter.SelectedIndex = selectedIndex;
							goto Label_0FE3;

						case 2:
							DR_timestep = 2.5;
							DR_PulseTimeStepNoLPF = 0.625;
							ddlHPFilter.Items.Clear();
							ddlHPFilter.Items.Add("16Hz");
							ddlHPFilter.Items.Add("8Hz");
							ddlHPFilter.Items.Add("4Hz");
							ddlHPFilter.Items.Add("2Hz");
							ddlHPFilter.SelectedIndex = selectedIndex;
							goto Label_0FE3;

						case 3:
							DR_timestep = 160.0;
							DR_PulseTimeStepNoLPF = 40.0;
							ddlHPFilter.Items.Clear();
							ddlHPFilter.Items.Add("0.25Hz");
							ddlHPFilter.Items.Add("0.125Hz");
							ddlHPFilter.Items.Add("0.063Hz");
							ddlHPFilter.Items.Add("0.031Hz");
							ddlHPFilter.SelectedIndex = selectedIndex;
							goto Label_0FE3;
					}
					break;

				case 7:
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
							goto Label_0FE3;

						case 1:
							DR_timestep = 80.0;
							DR_PulseTimeStepNoLPF = 20.0;
							ddlHPFilter.Items.Clear();
							ddlHPFilter.Items.Add("0.5Hz");
							ddlHPFilter.Items.Add("0.25Hz");
							ddlHPFilter.Items.Add("0.125Hz");
							ddlHPFilter.Items.Add("0.063Hz");
							ddlHPFilter.SelectedIndex = selectedIndex;
							goto Label_0FE3;

						case 2:
							DR_timestep = 2.5;
							DR_PulseTimeStepNoLPF = 0.625;
							ddlHPFilter.Items.Clear();
							ddlHPFilter.Items.Add("16Hz");
							ddlHPFilter.Items.Add("8Hz");
							ddlHPFilter.Items.Add("4Hz");
							ddlHPFilter.Items.Add("2Hz");
							ddlHPFilter.SelectedIndex = selectedIndex;
							goto Label_0FE3;

						case 3:
							DR_timestep = 160.0;
							DR_PulseTimeStepNoLPF = 40.0;
							ddlHPFilter.Items.Clear();
							ddlHPFilter.Items.Add("0.25Hz");
							ddlHPFilter.Items.Add("0.125Hz");
							ddlHPFilter.Items.Add("0.063Hz");
							ddlHPFilter.Items.Add("0.031Hz");
							ddlHPFilter.SelectedIndex = selectedIndex;
							goto Label_0FE3;
					}
					break;
			}
		Label_0FE3:
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
				pulse_step = DR_timestep;
			else
				pulse_step = DR_PulseTimeStepNoLPF;

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
			int[] datapassed = new int[] { ddlHPFilter.SelectedIndex };
			VeyronControllerObj.SetHPFilterFlag = true;
			ControllerReqPacket reqName = new ControllerReqPacket();
			CreateNewTaskFromGUI(reqName, 0, 11, datapassed);
		}

		private void ddlHysteresisA_SelectedIndexChanged(object sender, EventArgs e)
		{
			int[] datapassed = new int[1];
			Hysteresis_index = ddlHysteresisA.SelectedIndex;
			datapassed[0] = Hysteresis_index;
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
					HysAngle = 17;
					break;

				case 6:
					HysAngle = 21;
					break;

				case 7:
					HysAngle = 24;
					break;
			}
			VeyronControllerObj.SetHysteresisFlag = true;
			ControllerReqPacket reqName = new ControllerReqPacket();
			CreateNewTaskFromGUI(reqName, 4, 0x1d, datapassed);
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
				lblPLWarning.Visible = false;
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
			ControllerReqPacket reqName = new ControllerReqPacket();
			CreateNewTaskFromGUI(reqName, 4, 0x1c, datapassed);
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
			int[] datapassed = new int[] { ddlSleepSR.SelectedIndex };
			VeyronControllerObj.SetSleepSampleRateFlag = true;
			ControllerReqPacket reqName = new ControllerReqPacket();
			CreateNewTaskFromGUI(reqName, 0, 8, datapassed);
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
			int[] datapassed = new int[] { ddlZLockA.SelectedIndex };
			VeyronControllerObj.SetZLockAFlag = true;
			ControllerReqPacket reqName = new ControllerReqPacket();
			CreateNewTaskFromGUI(reqName, 4, 0x1a, datapassed);
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
				TmrActive.Enabled = false;
				tmrDataDisplay.Stop();
				tmrDataDisplay.Enabled = false;
				VeyronControllerObj.ResetDevice();
				VeyronControllerObj.ControllerEvents -= new AccelController.ControllerEventHandler(ControllerObj_ControllerEvents);
				DeleteResource();
				base.Close();
			}
		}

		public object GetGuiHandle()
		{
			return this;
		}

		private void GUI_DataConfigScreen(GUIUpdatePacket guiPacket)
		{
			if (!rdoDisabled.Checked)
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
					num = 32;

				numArray = (int[])guiPacket.PayLoad.Dequeue();
				if (numArray[0] != 0)
				{
					for (int i = 0; i < num; i++)
					{
						try
						{
							string str = numArray[i * 3].ToString() + "   " + numArray[(i * 3) + 1].ToString() + "   " + numArray[(i * 3) + 2].ToString() + "\n";
							ListViewItem item = new ListViewItem(new string[] { numArray[i * 3].ToString(), numArray[(i * 3) + 1].ToString(), numArray[(i * 3) + 2].ToString() });
							uxFifoList.Items.Add(item);
							item.EnsureVisible();
							uxFifoList.Update();
						}
						catch (Exception)
						{
							break;
						}
					}
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
				if (rdoActive.Checked && (chkXEFE.Checked || chkYEFE.Checked || chkZEFE.Checked))
				{
					MFFGraph.Enabled = true;
					if (rdo8bitDataMain.Checked)
						UpdateWaveformMFF(Gees8bData);
					else
					{
						switch (DeviceID)
						{
							case deviceID.MMA8451Q:
								UpdateWaveformMFF(Gees14bData);
								break;

							case deviceID.MMA8452Q:
								UpdateWaveformMFF(Gees12bData);
								break;

							case deviceID.MMA8453Q:
								UpdateWaveformMFF(Gees10bData);
								break;

							case deviceID.MMA8652FC:
								UpdateWaveformMFF(Gees12bData);
								break;

							case deviceID.MMA8653FC:
								UpdateWaveformMFF(Gees10bData);
								break;
						}
					}
				}
				byte[] buffer = (byte[])guiPacket.PayLoad.Dequeue();

				ledMFF1XHE.Value = (buffer[0] == 1);
				lblXHP.Text = (buffer[1] == 1) ? "Negative" : "Positive";
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
				guiPacket.PayLoad.Dequeue();

			if (guiPacket.TaskID == 0x5d)
			{
				int[] bits = (int[])guiPacket.PayLoad.Dequeue();
				_registerView.SetLedsValue(bits, 1);
			}

			if (guiPacket.TaskID == 0x10)
			{
				byte[] regValues = (byte[])guiPacket.PayLoad.Dequeue();
				_registerView.ExportRegisters(regValues);
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
				int[] numArray = (int[])guiPacket.PayLoad.Dequeue();
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
			if (rdoActive.Checked
			&& (chkTransEnableXFlag.Checked ||
				chkTransEnableXFlagNEW.Checked ||
				chkTransEnableYFlag.Checked ||
				chkTransEnableYFlagNEW.Checked ||
				chkTransEnableZFlag.Checked ||
				chkTransEnableZFlagNEW.Checked)
				)
			{
				gphXYZ.Enabled = true;
				if (rdo8bitDataMain.Checked)
					UpdateWaveformTrans(Gees8bData);
				else
				{
					switch (DeviceID)
					{
						case deviceID.MMA8451Q:
							UpdateWaveformTrans(Gees14bData);
							break;

						case deviceID.MMA8452Q:
							UpdateWaveformTrans(Gees12bData);
							break;

						case deviceID.MMA8453Q:
							UpdateWaveformTrans(Gees10bData);
							break;

						case deviceID.MMA8652FC:
							UpdateWaveformTrans(Gees12bData);
							break;

						case deviceID.MMA8653FC:
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

		#region InitDevice 
		private void InitDevice()
		{
			VeyronControllerObj.AppRegisterWrite(new int[] { 0x15, 2 });
			TabTool.SelectedIndex = 0;
			ddlDataRate.SelectedIndex = 0;
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
			VeyronControllerObj.PollingOrInt[0] = 0;
			uxStart.Enabled = false;
			gbXD.Enabled = false;
			int[] datapassed = new int[] { FullScaleValue };
			VeyronControllerObj.BootFlag = true;
			ControllerReqPacket reqName = new ControllerReqPacket();
			CreateNewTaskFromGUI(reqName, 0, 0x51, datapassed);
			int[] numArray2 = new int[] { 0xff };
			rdoINTOpenDrain.Checked = true;
			VeyronControllerObj.SetINTPPODFlag = true;
			ControllerReqPacket packet2 = new ControllerReqPacket();
			CreateNewTaskFromGUI(packet2, 0, 0x20, numArray2);
		}
		#endregion

		#region InitializeComponent
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VeyronEvaluationSoftware8451));
			this.PL_Page = new System.Windows.Forms.TabPage();
			this.PLImage = new System.Windows.Forms.PictureBox();
			this.gbOD = new System.Windows.Forms.GroupBox();
			this.lblPLWarning = new System.Windows.Forms.Label();
			this.label21 = new System.Windows.Forms.Label();
			this.label24 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.lblValL2PResult = new System.Windows.Forms.Label();
			this.lblValP2LResult = new System.Windows.Forms.Label();
			this.lblp2LTripAngle = new System.Windows.Forms.Label();
			this.chkPLDefaultSettings = new System.Windows.Forms.CheckBox();
			this.pPLActive = new System.Windows.Forms.Panel();
			this.btnResetPLDebounce = new System.Windows.Forms.Button();
			this.btnSetPLDebounce = new System.Windows.Forms.Button();
			this.lblBouncems = new System.Windows.Forms.Label();
			this.lblPLbounceVal = new System.Windows.Forms.Label();
			this.lblDebouncePL = new System.Windows.Forms.Label();
			this.tbPL = new System.Windows.Forms.TrackBar();
			this.p6 = new System.Windows.Forms.Panel();
			this.gbPLDisappear = new System.Windows.Forms.GroupBox();
			this.label263 = new System.Windows.Forms.Label();
			this.ddlHysteresisA = new System.Windows.Forms.ComboBox();
			this.ddlPLTripA = new System.Windows.Forms.ComboBox();
			this.label264 = new System.Windows.Forms.Label();
			this.label265 = new System.Windows.Forms.Label();
			this.ddlBFTripA = new System.Windows.Forms.ComboBox();
			this.ddlZLockA = new System.Windows.Forms.ComboBox();
			this.label266 = new System.Windows.Forms.Label();
			this.rdoClrDebouncePL = new System.Windows.Forms.RadioButton();
			this.rdoDecDebouncePL = new System.Windows.Forms.RadioButton();
			this.chkEnablePL = new System.Windows.Forms.CheckBox();
			this.p7 = new System.Windows.Forms.Panel();
			this.label191 = new System.Windows.Forms.Label();
			this.ledCurPLNew = new NationalInstruments.UI.WindowsForms.Led();
			this.label273 = new System.Windows.Forms.Label();
			this.ledCurPLRight = new NationalInstruments.UI.WindowsForms.Led();
			this.label275 = new System.Windows.Forms.Label();
			this.ledCurPLLeft = new NationalInstruments.UI.WindowsForms.Led();
			this.label277 = new System.Windows.Forms.Label();
			this.ledCurPLDown = new NationalInstruments.UI.WindowsForms.Led();
			this.label279 = new System.Windows.Forms.Label();
			this.ledCurPLUp = new NationalInstruments.UI.WindowsForms.Led();
			this.label281 = new System.Windows.Forms.Label();
			this.ledCurPLBack = new NationalInstruments.UI.WindowsForms.Led();
			this.label283 = new System.Windows.Forms.Label();
			this.ledCurPLFront = new NationalInstruments.UI.WindowsForms.Led();
			this.ledCurPLLO = new NationalInstruments.UI.WindowsForms.Led();
			this.label285 = new System.Windows.Forms.Label();
			this.MainScreenEval = new System.Windows.Forms.TabPage();
			this.groupBox8 = new System.Windows.Forms.GroupBox();
			this.panel3 = new System.Windows.Forms.Panel();
			this.gbOC = new System.Windows.Forms.GroupBox();
			this.label34 = new System.Windows.Forms.Label();
			this.label46 = new System.Windows.Forms.Label();
			this.label47 = new System.Windows.Forms.Label();
			this.btnWriteCal = new System.Windows.Forms.Button();
			this.label43 = new System.Windows.Forms.Label();
			this.txtCalX = new System.Windows.Forms.MaskedTextBox();
			this.label44 = new System.Windows.Forms.Label();
			this.label45 = new System.Windows.Forms.Label();
			this.txtCalY = new System.Windows.Forms.MaskedTextBox();
			this.txtCalZ = new System.Windows.Forms.MaskedTextBox();
			this.lblXCal = new System.Windows.Forms.Label();
			this.lblYCal = new System.Windows.Forms.Label();
			this.lblZCal = new System.Windows.Forms.Label();
			this.btnAutoCal = new System.Windows.Forms.Button();
			this.uxStart = new System.Windows.Forms.CheckBox();
			this.groupBox7 = new System.Windows.Forms.GroupBox();
			this.p14b8bSelect = new System.Windows.Forms.Panel();
			this.rdoXYZFullResMain = new System.Windows.Forms.RadioButton();
			this.rdo8bitDataMain = new System.Windows.Forms.RadioButton();
			this.groupBox5 = new System.Windows.Forms.GroupBox();
			this.legend1 = new NationalInstruments.UI.WindowsForms.Legend();
			this.legendItem1 = new NationalInstruments.UI.LegendItem();
			this.uxAccelX = new NationalInstruments.UI.WaveformPlot();
			this.uxAxisSamples = new NationalInstruments.UI.XAxis();
			this.uxAxisAccel = new NationalInstruments.UI.YAxis();
			this.legendItem2 = new NationalInstruments.UI.LegendItem();
			this.uxAccelY = new NationalInstruments.UI.WaveformPlot();
			this.legendItem3 = new NationalInstruments.UI.LegendItem();
			this.uxAccelZ = new NationalInstruments.UI.WaveformPlot();
			this.chkEnableZAxis = new System.Windows.Forms.CheckBox();
			this.chkEnableX = new System.Windows.Forms.CheckBox();
			this.chkEnableYAxis = new System.Windows.Forms.CheckBox();
			this.gbST = new System.Windows.Forms.GroupBox();
			this.chkSelfTest = new System.Windows.Forms.CheckBox();
			this.gbXD = new System.Windows.Forms.GroupBox();
			this.uxAverageData = new System.Windows.Forms.ComboBox();
			this.label22 = new System.Windows.Forms.Label();
			this.label23 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.uxSwitchCounts = new NationalInstruments.UI.WindowsForms.Switch();
			this.uxAccelZValue = new LCDLabel.LcdLabel();
			this.uxAccelYValue = new LCDLabel.LcdLabel();
			this.uxAccelXValue = new LCDLabel.LcdLabel();
			this.MainScreenGraph = new NationalInstruments.UI.WindowsForms.WaveformGraph();
			this.uxAxisCounts = new NationalInstruments.UI.YAxis();
			this.uxPanelSettings = new System.Windows.Forms.Panel();
			this.pSOS = new System.Windows.Forms.GroupBox();
			this.rdoSOSNormalMode = new System.Windows.Forms.RadioButton();
			this.rdoSOSHiResMode = new System.Windows.Forms.RadioButton();
			this.rdoSOSLNLPMode = new System.Windows.Forms.RadioButton();
			this.rdoSOSLPMode = new System.Windows.Forms.RadioButton();
			this.gbASS = new System.Windows.Forms.GroupBox();
			this.pnlAutoSleep = new System.Windows.Forms.Panel();
			this.btnSleepTimerReset = new System.Windows.Forms.Button();
			this.btnSetSleepTimer = new System.Windows.Forms.Button();
			this.label199 = new System.Windows.Forms.Label();
			this.lblSleepms = new System.Windows.Forms.Label();
			this.lblSleepTimerValue = new System.Windows.Forms.Label();
			this.lblSleepTimer = new System.Windows.Forms.Label();
			this.tbSleepCounter = new System.Windows.Forms.TrackBar();
			this.ddlSleepSR = new System.Windows.Forms.ComboBox();
			this.chkEnableASleep = new System.Windows.Forms.CheckBox();
			this.gbIF = new System.Windows.Forms.GroupBox();
			this.p9 = new System.Windows.Forms.Panel();
			this.ledTrans1 = new NationalInstruments.UI.WindowsForms.Led();
			this.ledOrient = new NationalInstruments.UI.WindowsForms.Led();
			this.ledASleep = new NationalInstruments.UI.WindowsForms.Led();
			this.ledMFF1 = new NationalInstruments.UI.WindowsForms.Led();
			this.ledPulse = new NationalInstruments.UI.WindowsForms.Led();
			this.ledTrans = new NationalInstruments.UI.WindowsForms.Led();
			this.ledDataReady = new NationalInstruments.UI.WindowsForms.Led();
			this.ledFIFO = new NationalInstruments.UI.WindowsForms.Led();
			this.p8 = new System.Windows.Forms.Panel();
			this.panel18 = new System.Windows.Forms.Panel();
			this.rdoTrans1INT_I1 = new System.Windows.Forms.RadioButton();
			this.rdoTrans1INT_I2 = new System.Windows.Forms.RadioButton();
			this.chkEnIntTrans1 = new System.Windows.Forms.CheckBox();
			this.label36 = new System.Windows.Forms.Label();
			this.panel12 = new System.Windows.Forms.Panel();
			this.rdoFIFOINT_I1 = new System.Windows.Forms.RadioButton();
			this.rdoFIFOINT_I2 = new System.Windows.Forms.RadioButton();
			this.chkEnIntFIFO = new System.Windows.Forms.CheckBox();
			this.lblntFIFO = new System.Windows.Forms.Label();
			this.panel11 = new System.Windows.Forms.Panel();
			this.rdoDRINT_I1 = new System.Windows.Forms.RadioButton();
			this.rdoDRINT_I2 = new System.Windows.Forms.RadioButton();
			this.chkEnIntDR = new System.Windows.Forms.CheckBox();
			this.label6 = new System.Windows.Forms.Label();
			this.panel10 = new System.Windows.Forms.Panel();
			this.rdoTransINT_I1 = new System.Windows.Forms.RadioButton();
			this.rdoTransINT_I2 = new System.Windows.Forms.RadioButton();
			this.chkEnIntTrans = new System.Windows.Forms.CheckBox();
			this.label3 = new System.Windows.Forms.Label();
			this.panel9 = new System.Windows.Forms.Panel();
			this.rdoPulseINT_I1 = new System.Windows.Forms.RadioButton();
			this.rdoPulseINT_I2 = new System.Windows.Forms.RadioButton();
			this.chkEnIntPulse = new System.Windows.Forms.CheckBox();
			this.label2 = new System.Windows.Forms.Label();
			this.panel4 = new System.Windows.Forms.Panel();
			this.rdoASleepINT_I1 = new System.Windows.Forms.RadioButton();
			this.rdoASleepINT_I2 = new System.Windows.Forms.RadioButton();
			this.chkEnIntASleep = new System.Windows.Forms.CheckBox();
			this.label1 = new System.Windows.Forms.Label();
			this.panel8 = new System.Windows.Forms.Panel();
			this.rdoMFF1INT_I1 = new System.Windows.Forms.RadioButton();
			this.rdoMFF1INT_I2 = new System.Windows.Forms.RadioButton();
			this.chkEnIntMFF1 = new System.Windows.Forms.CheckBox();
			this.label4 = new System.Windows.Forms.Label();
			this.panel6 = new System.Windows.Forms.Panel();
			this.rdoPLINT_I1 = new System.Windows.Forms.RadioButton();
			this.rdoPLINT_I2 = new System.Windows.Forms.RadioButton();
			this.chkEnIntPL = new System.Windows.Forms.CheckBox();
			this.label172 = new System.Windows.Forms.Label();
			this.gbwfs = new System.Windows.Forms.GroupBox();
			this.chkWakeTrans1 = new System.Windows.Forms.CheckBox();
			this.chkWakeTrans = new System.Windows.Forms.CheckBox();
			this.chkWakeFIFOGate = new System.Windows.Forms.CheckBox();
			this.chkWakeLP = new System.Windows.Forms.CheckBox();
			this.chkWakePulse = new System.Windows.Forms.CheckBox();
			this.chkWakeMFF1 = new System.Windows.Forms.CheckBox();
			this.p2 = new System.Windows.Forms.Panel();
			this.label37 = new System.Windows.Forms.Label();
			this.ddlHPFilter = new System.Windows.Forms.ComboBox();
			this.groupBox6 = new System.Windows.Forms.GroupBox();
			this.label38 = new System.Windows.Forms.Label();
			this.chkXYZ12Log = new System.Windows.Forms.CheckBox();
			this.chkTransLog = new System.Windows.Forms.CheckBox();
			this.chkXYZ8Log = new System.Windows.Forms.CheckBox();
			this.lblFileName = new System.Windows.Forms.Label();
			this.txtSaveFileName = new System.Windows.Forms.TextBox();
			this.gbOM = new System.Windows.Forms.GroupBox();
			this.chkAnalogLowNoise = new System.Windows.Forms.CheckBox();
			this.chkHPFDataOut = new System.Windows.Forms.CheckBox();
			this.pOverSampling = new System.Windows.Forms.Panel();
			this.label70 = new System.Windows.Forms.Label();
			this.rdoOSHiResMode = new System.Windows.Forms.RadioButton();
			this.rdoOSLPMode = new System.Windows.Forms.RadioButton();
			this.rdoOSLNLPMode = new System.Windows.Forms.RadioButton();
			this.rdoOSNormalMode = new System.Windows.Forms.RadioButton();
			this.rdoStandby = new System.Windows.Forms.RadioButton();
			this.rdoActive = new System.Windows.Forms.RadioButton();
			this.ledSleep = new NationalInstruments.UI.WindowsForms.Led();
			this.lbsSleep = new System.Windows.Forms.Label();
			this.lbsStandby = new System.Windows.Forms.Label();
			this.p1 = new System.Windows.Forms.Panel();
			this.ddlDataRate = new System.Windows.Forms.ComboBox();
			this.label35 = new System.Windows.Forms.Label();
			this.ledStandby = new NationalInstruments.UI.WindowsForms.Led();
			this.lbsWake = new System.Windows.Forms.Label();
			this.ledWake = new NationalInstruments.UI.WindowsForms.Led();
			this.gbDR = new System.Windows.Forms.GroupBox();
			this.lbl4gSensitivity = new System.Windows.Forms.Label();
			this.rdo2g = new System.Windows.Forms.RadioButton();
			this.rdo8g = new System.Windows.Forms.RadioButton();
			this.rdo4g = new System.Windows.Forms.RadioButton();
			this.lbl2gSensitivity = new System.Windows.Forms.Label();
			this.lbl8gSensitivity = new System.Windows.Forms.Label();
			this.TabTool = new System.Windows.Forms.TabControl();
			this.Register_Page = new System.Windows.Forms.TabPage();
			this.DataConfigPage = new System.Windows.Forms.TabPage();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.gbStatus = new System.Windows.Forms.GroupBox();
			this.p21 = new System.Windows.Forms.Panel();
			this.lblFIFOStatus = new System.Windows.Forms.Label();
			this.ledFIFOStatus = new NationalInstruments.UI.WindowsForms.Led();
			this.lblFOvf = new System.Windows.Forms.Label();
			this.ledRealTimeStatus = new NationalInstruments.UI.WindowsForms.Led();
			this.label61 = new System.Windows.Forms.Label();
			this.lblWmrk = new System.Windows.Forms.Label();
			this.lblFCnt5 = new System.Windows.Forms.Label();
			this.lblFCnt4 = new System.Windows.Forms.Label();
			this.lblFCnt3 = new System.Windows.Forms.Label();
			this.lblFCnt2 = new System.Windows.Forms.Label();
			this.lblFCnt1 = new System.Windows.Forms.Label();
			this.lblFCnt0 = new System.Windows.Forms.Label();
			this.ledXYZOW = new NationalInstruments.UI.WindowsForms.Led();
			this.ledZOW = new NationalInstruments.UI.WindowsForms.Led();
			this.ledYOW = new NationalInstruments.UI.WindowsForms.Led();
			this.ledXOW = new NationalInstruments.UI.WindowsForms.Led();
			this.ledZDR = new NationalInstruments.UI.WindowsForms.Led();
			this.ledYDR = new NationalInstruments.UI.WindowsForms.Led();
			this.ledXDR = new NationalInstruments.UI.WindowsForms.Led();
			this.ledXYZDR = new NationalInstruments.UI.WindowsForms.Led();
			this.lblXYZOW = new System.Windows.Forms.Label();
			this.lblXYZDrdy = new System.Windows.Forms.Label();
			this.lblXDrdy = new System.Windows.Forms.Label();
			this.lblZOW = new System.Windows.Forms.Label();
			this.lblYOW = new System.Windows.Forms.Label();
			this.lblYDrdy = new System.Windows.Forms.Label();
			this.lblXOW = new System.Windows.Forms.Label();
			this.lblZDrdy = new System.Windows.Forms.Label();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.p20 = new System.Windows.Forms.Panel();
			this.panel16 = new System.Windows.Forms.Panel();
			this.rdoINTPushPull = new System.Windows.Forms.RadioButton();
			this.rdoINTOpenDrain = new System.Windows.Forms.RadioButton();
			this.panel17 = new System.Windows.Forms.Panel();
			this.rdoINTActiveLow = new System.Windows.Forms.RadioButton();
			this.rdoINTActiveHigh = new System.Windows.Forms.RadioButton();
			this.MFF1_2Page = new System.Windows.Forms.TabPage();
			this.lblMFFDataBit = new System.Windows.Forms.Label();
			this.chkEnableZMFF = new System.Windows.Forms.CheckBox();
			this.chkEnableYMFF = new System.Windows.Forms.CheckBox();
			this.chkEnableXMFF = new System.Windows.Forms.CheckBox();
			this.lblMotionDataType = new System.Windows.Forms.Label();
			this.legend3 = new NationalInstruments.UI.WindowsForms.Legend();
			this.legendItem7 = new NationalInstruments.UI.LegendItem();
			this.XAxis = new NationalInstruments.UI.WaveformPlot();
			this.xAxis1 = new NationalInstruments.UI.XAxis();
			this.yAxis1 = new NationalInstruments.UI.YAxis();
			this.legendItem8 = new NationalInstruments.UI.LegendItem();
			this.YAxis = new NationalInstruments.UI.WaveformPlot();
			this.legendItem9 = new NationalInstruments.UI.LegendItem();
			this.waveformPlot3 = new NationalInstruments.UI.WaveformPlot();
			this.xAxis2 = new NationalInstruments.UI.XAxis();
			this.yAxis2 = new NationalInstruments.UI.YAxis();
			this.MFFGraph = new NationalInstruments.UI.WindowsForms.WaveformGraph();
			this.waveformPlot1 = new NationalInstruments.UI.WaveformPlot();
			this.waveformPlot2 = new NationalInstruments.UI.WaveformPlot();
			this.gbMF1 = new System.Windows.Forms.GroupBox();
			this.chkDefaultFFSettings1 = new System.Windows.Forms.CheckBox();
			this.chkDefaultMotion1 = new System.Windows.Forms.CheckBox();
			this.btnMFF1Reset = new System.Windows.Forms.Button();
			this.btnMFF1Set = new System.Windows.Forms.Button();
			this.rdoMFF1ClearDebounce = new System.Windows.Forms.RadioButton();
			this.p14 = new System.Windows.Forms.Panel();
			this.chkMFF1EnableLatch = new System.Windows.Forms.CheckBox();
			this.chkYEFE = new System.Windows.Forms.CheckBox();
			this.chkZEFE = new System.Windows.Forms.CheckBox();
			this.chkXEFE = new System.Windows.Forms.CheckBox();
			this.rdoMFF1And = new System.Windows.Forms.RadioButton();
			this.rdoMFF1Or = new System.Windows.Forms.RadioButton();
			this.p15 = new System.Windows.Forms.Panel();
			this.label20 = new System.Windows.Forms.Label();
			this.label230 = new System.Windows.Forms.Label();
			this.label231 = new System.Windows.Forms.Label();
			this.ledMFF1EA = new NationalInstruments.UI.WindowsForms.Led();
			this.label232 = new System.Windows.Forms.Label();
			this.label233 = new System.Windows.Forms.Label();
			this.lblXHP = new System.Windows.Forms.Label();
			this.ledMFF1XHE = new NationalInstruments.UI.WindowsForms.Led();
			this.lblZHP = new System.Windows.Forms.Label();
			this.label236 = new System.Windows.Forms.Label();
			this.ledMFF1YHE = new NationalInstruments.UI.WindowsForms.Led();
			this.lblYHP = new System.Windows.Forms.Label();
			this.label238 = new System.Windows.Forms.Label();
			this.ledMFF1ZHE = new NationalInstruments.UI.WindowsForms.Led();
			this.label239 = new System.Windows.Forms.Label();
			this.rdoMFF1DecDebounce = new System.Windows.Forms.RadioButton();
			this.lblMFF1Threshold = new System.Windows.Forms.Label();
			this.lblMFF1ThresholdVal = new System.Windows.Forms.Label();
			this.lblMFF1Debouncems = new System.Windows.Forms.Label();
			this.lblMFF1Thresholdg = new System.Windows.Forms.Label();
			this.tbMFF1Threshold = new System.Windows.Forms.TrackBar();
			this.lblMFF1DebounceVal = new System.Windows.Forms.Label();
			this.tbMFF1Debounce = new System.Windows.Forms.TrackBar();
			this.lblMFF1Debounce = new System.Windows.Forms.Label();
			this.TransientDetection = new System.Windows.Forms.TabPage();
			this.lblTransDataBit = new System.Windows.Forms.Label();
			this.chkZEnableTrans = new System.Windows.Forms.CheckBox();
			this.chkYEnableTrans = new System.Windows.Forms.CheckBox();
			this.chkXEnableTrans = new System.Windows.Forms.CheckBox();
			this.lblTransDataType = new System.Windows.Forms.Label();
			this.pTrans2 = new System.Windows.Forms.Panel();
			this.lblTransPolZ = new System.Windows.Forms.Label();
			this.lblTransPolY = new System.Windows.Forms.Label();
			this.lblTransPolX = new System.Windows.Forms.Label();
			this.ledTransEA = new NationalInstruments.UI.WindowsForms.Led();
			this.label62 = new System.Windows.Forms.Label();
			this.label65 = new System.Windows.Forms.Label();
			this.ledTransZDetect = new NationalInstruments.UI.WindowsForms.Led();
			this.ledTransYDetect = new NationalInstruments.UI.WindowsForms.Led();
			this.ledTransXDetect = new NationalInstruments.UI.WindowsForms.Led();
			this.label67 = new System.Windows.Forms.Label();
			this.label68 = new System.Windows.Forms.Label();
			this.label69 = new System.Windows.Forms.Label();
			this.gbTSNEW = new System.Windows.Forms.GroupBox();
			this.btnResetTransientNEW = new System.Windows.Forms.Button();
			this.btnSetTransientNEW = new System.Windows.Forms.Button();
			this.rdoTransClearDebounceNEW = new System.Windows.Forms.RadioButton();
			this.pTransNEW = new System.Windows.Forms.Panel();
			this.chkDefaultTransSettings1 = new System.Windows.Forms.CheckBox();
			this.chkTransBypassHPFNEW = new System.Windows.Forms.CheckBox();
			this.chkTransEnableLatchNEW = new System.Windows.Forms.CheckBox();
			this.chkTransEnableXFlagNEW = new System.Windows.Forms.CheckBox();
			this.chkTransEnableZFlagNEW = new System.Windows.Forms.CheckBox();
			this.chkTransEnableYFlagNEW = new System.Windows.Forms.CheckBox();
			this.tbTransDebounceNEW = new System.Windows.Forms.TrackBar();
			this.p19 = new System.Windows.Forms.Panel();
			this.lblTransNewPolZ = new System.Windows.Forms.Label();
			this.lblTransNewPolY = new System.Windows.Forms.Label();
			this.lblTransNewPolX = new System.Windows.Forms.Label();
			this.ledTransEANEW = new NationalInstruments.UI.WindowsForms.Led();
			this.label203 = new System.Windows.Forms.Label();
			this.label204 = new System.Windows.Forms.Label();
			this.ledTransZDetectNEW = new NationalInstruments.UI.WindowsForms.Led();
			this.ledTransYDetectNEW = new NationalInstruments.UI.WindowsForms.Led();
			this.ledTransXDetectNEW = new NationalInstruments.UI.WindowsForms.Led();
			this.label205 = new System.Windows.Forms.Label();
			this.label206 = new System.Windows.Forms.Label();
			this.label207 = new System.Windows.Forms.Label();
			this.lblTransDebounceValNEW = new System.Windows.Forms.Label();
			this.rdoTransDecDebounceNEW = new System.Windows.Forms.RadioButton();
			this.tbTransThresholdNEW = new System.Windows.Forms.TrackBar();
			this.lblTransDebounceNEW = new System.Windows.Forms.Label();
			this.lblTransThresholdNEW = new System.Windows.Forms.Label();
			this.lblTransThresholdValNEW = new System.Windows.Forms.Label();
			this.lblTransThresholdgNEW = new System.Windows.Forms.Label();
			this.lblTransDebouncemsNEW = new System.Windows.Forms.Label();
			this.legend2 = new NationalInstruments.UI.WindowsForms.Legend();
			this.legendItem4 = new NationalInstruments.UI.LegendItem();
			this.legendItem5 = new NationalInstruments.UI.LegendItem();
			this.legendItem6 = new NationalInstruments.UI.LegendItem();
			this.ZAxis = new NationalInstruments.UI.WaveformPlot();
			this.gphXYZ = new NationalInstruments.UI.WindowsForms.WaveformGraph();
			this.gbTS = new System.Windows.Forms.GroupBox();
			this.btnResetTransient = new System.Windows.Forms.Button();
			this.btnSetTransient = new System.Windows.Forms.Button();
			this.rdoTransClearDebounce = new System.Windows.Forms.RadioButton();
			this.p18 = new System.Windows.Forms.Panel();
			this.chkDefaultTransSettings = new System.Windows.Forms.CheckBox();
			this.chkTransBypassHPF = new System.Windows.Forms.CheckBox();
			this.chkTransEnableLatch = new System.Windows.Forms.CheckBox();
			this.chkTransEnableXFlag = new System.Windows.Forms.CheckBox();
			this.chkTransEnableZFlag = new System.Windows.Forms.CheckBox();
			this.chkTransEnableYFlag = new System.Windows.Forms.CheckBox();
			this.tbTransDebounce = new System.Windows.Forms.TrackBar();
			this.lblTransDebounceVal = new System.Windows.Forms.Label();
			this.rdoTransDecDebounce = new System.Windows.Forms.RadioButton();
			this.tbTransThreshold = new System.Windows.Forms.TrackBar();
			this.lblTransDebounce = new System.Windows.Forms.Label();
			this.lblTransThreshold = new System.Windows.Forms.Label();
			this.lblTransThresholdVal = new System.Windows.Forms.Label();
			this.lblTransThresholdg = new System.Windows.Forms.Label();
			this.lblTransDebouncems = new System.Windows.Forms.Label();
			this.PulseDetection = new System.Windows.Forms.TabPage();
			this.gbSDPS = new System.Windows.Forms.GroupBox();
			this.panel15 = new System.Windows.Forms.Panel();
			this.chkPulseLPFEnable = new System.Windows.Forms.CheckBox();
			this.chkPulseHPFBypass = new System.Windows.Forms.CheckBox();
			this.rdoDefaultSPDP = new System.Windows.Forms.RadioButton();
			this.rdoDefaultSP = new System.Windows.Forms.RadioButton();
			this.btnResetPulseThresholds = new System.Windows.Forms.Button();
			this.btnSetPulseThresholds = new System.Windows.Forms.Button();
			this.tbPulseZThreshold = new System.Windows.Forms.TrackBar();
			this.p12 = new System.Windows.Forms.Panel();
			this.btnPulseResetTime2ndPulse = new System.Windows.Forms.Button();
			this.btnPulseSetTime2ndPulse = new System.Windows.Forms.Button();
			this.tbPulseLatency = new System.Windows.Forms.TrackBar();
			this.chkPulseEnableXDP = new System.Windows.Forms.CheckBox();
			this.chkPulseEnableYDP = new System.Windows.Forms.CheckBox();
			this.chkPulseEnableZDP = new System.Windows.Forms.CheckBox();
			this.lblPulse2ndPulseWinms = new System.Windows.Forms.Label();
			this.lblPulseLatency = new System.Windows.Forms.Label();
			this.lblPulse2ndPulseWinVal = new System.Windows.Forms.Label();
			this.lblPulseLatencyVal = new System.Windows.Forms.Label();
			this.lblPulse2ndPulseWin = new System.Windows.Forms.Label();
			this.lblPulseLatencyms = new System.Windows.Forms.Label();
			this.tbPulse2ndPulseWin = new System.Windows.Forms.TrackBar();
			this.chkPulseIgnorLatentPulses = new System.Windows.Forms.CheckBox();
			this.p10 = new System.Windows.Forms.Panel();
			this.btnResetFirstPulseTimeLimit = new System.Windows.Forms.Button();
			this.btnSetFirstPulseTimeLimit = new System.Windows.Forms.Button();
			this.chkPulseEnableLatch = new System.Windows.Forms.CheckBox();
			this.chkPulseEnableXSP = new System.Windows.Forms.CheckBox();
			this.chkPulseEnableYSP = new System.Windows.Forms.CheckBox();
			this.chkPulseEnableZSP = new System.Windows.Forms.CheckBox();
			this.tbFirstPulseTimeLimit = new System.Windows.Forms.TrackBar();
			this.lblFirstPulseTimeLimitms = new System.Windows.Forms.Label();
			this.lblFirstTimeLimitVal = new System.Windows.Forms.Label();
			this.lblFirstPulseTimeLimit = new System.Windows.Forms.Label();
			this.p11 = new System.Windows.Forms.Panel();
			this.label15 = new System.Windows.Forms.Label();
			this.ledPulseDouble = new NationalInstruments.UI.WindowsForms.Led();
			this.lblPPolZ = new System.Windows.Forms.Label();
			this.lblPPolY = new System.Windows.Forms.Label();
			this.lblPPolX = new System.Windows.Forms.Label();
			this.label255 = new System.Windows.Forms.Label();
			this.label256 = new System.Windows.Forms.Label();
			this.ledPulseEA = new NationalInstruments.UI.WindowsForms.Led();
			this.label258 = new System.Windows.Forms.Label();
			this.ledPZ = new NationalInstruments.UI.WindowsForms.Led();
			this.ledPX = new NationalInstruments.UI.WindowsForms.Led();
			this.label268 = new System.Windows.Forms.Label();
			this.label287 = new System.Windows.Forms.Label();
			this.ledPY = new NationalInstruments.UI.WindowsForms.Led();
			this.tbPulseXThreshold = new System.Windows.Forms.TrackBar();
			this.lblPulseYThreshold = new System.Windows.Forms.Label();
			this.tbPulseYThreshold = new System.Windows.Forms.TrackBar();
			this.lblPulseYThresholdVal = new System.Windows.Forms.Label();
			this.lblPulseXThresholdg = new System.Windows.Forms.Label();
			this.lblPulseXThresholdVal = new System.Windows.Forms.Label();
			this.lblPulseZThresholdg = new System.Windows.Forms.Label();
			this.lblPulseZThreshold = new System.Windows.Forms.Label();
			this.lblPulseZThresholdVal = new System.Windows.Forms.Label();
			this.lblPulseYThresholdg = new System.Windows.Forms.Label();
			this.lblPulseXThreshold = new System.Windows.Forms.Label();
			this.FIFOPage = new System.Windows.Forms.TabPage();
			this.uxCopyFifoData = new System.Windows.Forms.Button();
			this.uxClearFifoDataList = new System.Windows.Forms.Button();
			this.uxFifoList = new System.Windows.Forms.ListView();
			this.uxXFifo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.uxYFifo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.uxZFifo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.rdoFIFO14bitDataDisplay = new System.Windows.Forms.RadioButton();
			this.rdoFIFO8bitDataDisplay = new System.Windows.Forms.RadioButton();
			this.gb3bF = new System.Windows.Forms.GroupBox();
			this.rdoDisabled = new System.Windows.Forms.RadioButton();
			this.p5 = new System.Windows.Forms.Panel();
			this.label19 = new System.Windows.Forms.Label();
			this.label17 = new System.Windows.Forms.Label();
			this.label14 = new System.Windows.Forms.Label();
			this.label13 = new System.Windows.Forms.Label();
			this.ledTrigMFF = new NationalInstruments.UI.WindowsForms.Led();
			this.ledTrigTap = new NationalInstruments.UI.WindowsForms.Led();
			this.ledTrigLP = new NationalInstruments.UI.WindowsForms.Led();
			this.ledTrigTrans = new NationalInstruments.UI.WindowsForms.Led();
			this.button1 = new System.Windows.Forms.Button();
			this.lblCurrent_FIFO_Count = new System.Windows.Forms.Label();
			this.lblF_Count = new System.Windows.Forms.Label();
			this.ledOverFlow = new NationalInstruments.UI.WindowsForms.Led();
			this.ledWatermark = new NationalInstruments.UI.WindowsForms.Led();
			this.label64 = new System.Windows.Forms.Label();
			this.label63 = new System.Windows.Forms.Label();
			this.p4 = new System.Windows.Forms.Panel();
			this.gbWatermark = new System.Windows.Forms.GroupBox();
			this.btnResetWatermark = new System.Windows.Forms.Button();
			this.lblWatermarkValue = new System.Windows.Forms.Label();
			this.tBWatermark = new System.Windows.Forms.TrackBar();
			this.btnWatermark = new System.Windows.Forms.Button();
			this.lblWatermark = new System.Windows.Forms.Label();
			this.pTriggerMode = new System.Windows.Forms.Panel();
			this.chkTriggerMFF = new System.Windows.Forms.CheckBox();
			this.chkTriggerPulse = new System.Windows.Forms.CheckBox();
			this.chkTriggerLP = new System.Windows.Forms.CheckBox();
			this.chkTrigTrans = new System.Windows.Forms.CheckBox();
			this.rdoTriggerMode = new System.Windows.Forms.RadioButton();
			this.chkDisableFIFO = new System.Windows.Forms.CheckBox();
			this.rdoFill = new System.Windows.Forms.RadioButton();
			this.rdoCircular = new System.Windows.Forms.RadioButton();
			this.btnSetMode = new System.Windows.Forms.Button();
			this.TmrActive = new System.Windows.Forms.Timer(this.components);
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.tmrDataDisplay = new System.Windows.Forms.Timer(this.components);
			this.panel5 = new System.Windows.Forms.Panel();
			this.pictureBox2 = new System.Windows.Forms.PictureBox();
			this.CommStrip = new System.Windows.Forms.StatusStrip();
			this.CommStripButton = new System.Windows.Forms.ToolStripDropDownButton();
			this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.PL_Page.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.PLImage)).BeginInit();
			this.gbOD.SuspendLayout();
			this.pPLActive.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.tbPL)).BeginInit();
			this.p6.SuspendLayout();
			this.gbPLDisappear.SuspendLayout();
			this.p7.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.ledCurPLNew)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ledCurPLRight)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ledCurPLLeft)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ledCurPLDown)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ledCurPLUp)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ledCurPLBack)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ledCurPLFront)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ledCurPLLO)).BeginInit();
			this.MainScreenEval.SuspendLayout();
			this.groupBox8.SuspendLayout();
			this.panel3.SuspendLayout();
			this.gbOC.SuspendLayout();
			this.groupBox7.SuspendLayout();
			this.p14b8bSelect.SuspendLayout();
			this.groupBox5.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.legend1)).BeginInit();
			this.gbST.SuspendLayout();
			this.gbXD.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.uxSwitchCounts)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.MainScreenGraph)).BeginInit();
			this.uxPanelSettings.SuspendLayout();
			this.pSOS.SuspendLayout();
			this.gbASS.SuspendLayout();
			this.pnlAutoSleep.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.tbSleepCounter)).BeginInit();
			this.gbIF.SuspendLayout();
			this.p9.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.ledTrans1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ledOrient)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ledASleep)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ledMFF1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ledPulse)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ledTrans)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ledDataReady)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ledFIFO)).BeginInit();
			this.p8.SuspendLayout();
			this.panel18.SuspendLayout();
			this.panel12.SuspendLayout();
			this.panel11.SuspendLayout();
			this.panel10.SuspendLayout();
			this.panel9.SuspendLayout();
			this.panel4.SuspendLayout();
			this.panel8.SuspendLayout();
			this.panel6.SuspendLayout();
			this.gbwfs.SuspendLayout();
			this.p2.SuspendLayout();
			this.groupBox6.SuspendLayout();
			this.gbOM.SuspendLayout();
			this.pOverSampling.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.ledSleep)).BeginInit();
			this.p1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.ledStandby)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ledWake)).BeginInit();
			this.gbDR.SuspendLayout();
			this.TabTool.SuspendLayout();
			this.DataConfigPage.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.gbStatus.SuspendLayout();
			this.p21.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.ledFIFOStatus)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ledRealTimeStatus)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ledXYZOW)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ledZOW)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ledYOW)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ledXOW)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ledZDR)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ledYDR)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ledXDR)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ledXYZDR)).BeginInit();
			this.groupBox3.SuspendLayout();
			this.p20.SuspendLayout();
			this.panel16.SuspendLayout();
			this.panel17.SuspendLayout();
			this.MFF1_2Page.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.legend3)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.MFFGraph)).BeginInit();
			this.gbMF1.SuspendLayout();
			this.p14.SuspendLayout();
			this.p15.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.ledMFF1EA)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ledMFF1XHE)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ledMFF1YHE)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ledMFF1ZHE)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.tbMFF1Threshold)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.tbMFF1Debounce)).BeginInit();
			this.TransientDetection.SuspendLayout();
			this.pTrans2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.ledTransEA)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ledTransZDetect)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ledTransYDetect)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ledTransXDetect)).BeginInit();
			this.gbTSNEW.SuspendLayout();
			this.pTransNEW.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.tbTransDebounceNEW)).BeginInit();
			this.p19.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.ledTransEANEW)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ledTransZDetectNEW)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ledTransYDetectNEW)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ledTransXDetectNEW)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.tbTransThresholdNEW)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.legend2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.gphXYZ)).BeginInit();
			this.gbTS.SuspendLayout();
			this.p18.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.tbTransDebounce)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.tbTransThreshold)).BeginInit();
			this.PulseDetection.SuspendLayout();
			this.gbSDPS.SuspendLayout();
			this.panel15.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.tbPulseZThreshold)).BeginInit();
			this.p12.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.tbPulseLatency)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.tbPulse2ndPulseWin)).BeginInit();
			this.p10.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.tbFirstPulseTimeLimit)).BeginInit();
			this.p11.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.ledPulseDouble)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ledPulseEA)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ledPZ)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ledPX)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ledPY)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.tbPulseXThreshold)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.tbPulseYThreshold)).BeginInit();
			this.FIFOPage.SuspendLayout();
			this.gb3bF.SuspendLayout();
			this.p5.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.ledTrigMFF)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ledTrigTap)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ledTrigLP)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ledTrigTrans)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ledOverFlow)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ledWatermark)).BeginInit();
			this.p4.SuspendLayout();
			this.gbWatermark.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.tBWatermark)).BeginInit();
			this.pTriggerMode.SuspendLayout();
			this.panel5.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
			this.CommStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// PL_Page
			// 
			this.PL_Page.BackColor = System.Drawing.Color.LightSlateGray;
			this.PL_Page.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.PL_Page.Controls.Add(this.PLImage);
			this.PL_Page.Controls.Add(this.gbOD);
			this.PL_Page.Controls.Add(this.p7);
			this.PL_Page.ForeColor = System.Drawing.Color.White;
			this.PL_Page.Location = new System.Drawing.Point(4, 25);
			this.PL_Page.Name = "PL_Page";
			this.PL_Page.Padding = new System.Windows.Forms.Padding(3);
			this.PL_Page.Size = new System.Drawing.Size(1124, 525);
			this.PL_Page.TabIndex = 1;
			this.PL_Page.Text = "Orientation";
			// 
			// PLImage
			// 
			this.PLImage.Image = ((System.Drawing.Image)(resources.GetObject("PLImage.Image")));
			this.PLImage.InitialImage = ((System.Drawing.Image)(resources.GetObject("PLImage.InitialImage")));
			this.PLImage.Location = new System.Drawing.Point(734, 49);
			this.PLImage.Name = "PLImage";
			this.PLImage.Size = new System.Drawing.Size(355, 201);
			this.PLImage.TabIndex = 193;
			this.PLImage.TabStop = false;
			// 
			// gbOD
			// 
			this.gbOD.BackColor = System.Drawing.Color.LightSlateGray;
			this.gbOD.Controls.Add(this.lblPLWarning);
			this.gbOD.Controls.Add(this.label21);
			this.gbOD.Controls.Add(this.label24);
			this.gbOD.Controls.Add(this.label5);
			this.gbOD.Controls.Add(this.lblValL2PResult);
			this.gbOD.Controls.Add(this.lblValP2LResult);
			this.gbOD.Controls.Add(this.lblp2LTripAngle);
			this.gbOD.Controls.Add(this.chkPLDefaultSettings);
			this.gbOD.Controls.Add(this.pPLActive);
			this.gbOD.Controls.Add(this.p6);
			this.gbOD.Controls.Add(this.chkEnablePL);
			this.gbOD.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.gbOD.ForeColor = System.Drawing.Color.White;
			this.gbOD.Location = new System.Drawing.Point(59, 39);
			this.gbOD.Name = "gbOD";
			this.gbOD.Size = new System.Drawing.Size(557, 411);
			this.gbOD.TabIndex = 192;
			this.gbOD.TabStop = false;
			this.gbOD.Text = "Orientation Detection";
			// 
			// lblPLWarning
			// 
			this.lblPLWarning.AutoSize = true;
			this.lblPLWarning.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblPLWarning.ForeColor = System.Drawing.Color.Red;
			this.lblPLWarning.Location = new System.Drawing.Point(198, 82);
			this.lblPLWarning.Name = "lblPLWarning";
			this.lblPLWarning.Size = new System.Drawing.Size(112, 20);
			this.lblPLWarning.TabIndex = 230;
			this.lblPLWarning.Text = "Invalid Angle";
			this.lblPLWarning.Visible = false;
			// 
			// label21
			// 
			this.label21.AutoSize = true;
			this.label21.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label21.Location = new System.Drawing.Point(327, 53);
			this.label21.Name = "label21";
			this.label21.Size = new System.Drawing.Size(65, 13);
			this.label21.TabIndex = 229;
			this.label21.Text = "Trip Angle";
			// 
			// label24
			// 
			this.label24.AutoSize = true;
			this.label24.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label24.Location = new System.Drawing.Point(325, 38);
			this.label24.Name = "label24";
			this.label24.Size = new System.Drawing.Size(133, 13);
			this.label24.TabIndex = 228;
			this.label24.Text = "Landscape-To-Portrait";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label5.Location = new System.Drawing.Point(179, 53);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(65, 13);
			this.label5.TabIndex = 227;
			this.label5.Text = "Trip Angle";
			// 
			// lblValL2PResult
			// 
			this.lblValL2PResult.AutoSize = true;
			this.lblValL2PResult.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblValL2PResult.Location = new System.Drawing.Point(398, 53);
			this.lblValL2PResult.Name = "lblValL2PResult";
			this.lblValL2PResult.Size = new System.Drawing.Size(21, 13);
			this.lblValL2PResult.TabIndex = 226;
			this.lblValL2PResult.Text = "66";
			// 
			// lblValP2LResult
			// 
			this.lblValP2LResult.AutoSize = true;
			this.lblValP2LResult.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblValP2LResult.Location = new System.Drawing.Point(250, 53);
			this.lblValP2LResult.Name = "lblValP2LResult";
			this.lblValP2LResult.Size = new System.Drawing.Size(21, 13);
			this.lblValP2LResult.TabIndex = 225;
			this.lblValP2LResult.Text = "24";
			// 
			// lblp2LTripAngle
			// 
			this.lblp2LTripAngle.AutoSize = true;
			this.lblp2LTripAngle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblp2LTripAngle.Location = new System.Drawing.Point(177, 38);
			this.lblp2LTripAngle.Name = "lblp2LTripAngle";
			this.lblp2LTripAngle.Size = new System.Drawing.Size(133, 13);
			this.lblp2LTripAngle.TabIndex = 223;
			this.lblp2LTripAngle.Text = "Portrait-To-Landscape";
			// 
			// chkPLDefaultSettings
			// 
			this.chkPLDefaultSettings.AutoSize = true;
			this.chkPLDefaultSettings.Enabled = false;
			this.chkPLDefaultSettings.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chkPLDefaultSettings.ForeColor = System.Drawing.Color.Gold;
			this.chkPLDefaultSettings.Location = new System.Drawing.Point(11, 66);
			this.chkPLDefaultSettings.Name = "chkPLDefaultSettings";
			this.chkPLDefaultSettings.Size = new System.Drawing.Size(148, 17);
			this.chkPLDefaultSettings.TabIndex = 222;
			this.chkPLDefaultSettings.Text = "Set Default Settings  ";
			this.chkPLDefaultSettings.UseVisualStyleBackColor = true;
			this.chkPLDefaultSettings.CheckedChanged += new System.EventHandler(this.chkPLDefaultSettings_CheckedChanged);
			// 
			// pPLActive
			// 
			this.pPLActive.Controls.Add(this.btnResetPLDebounce);
			this.pPLActive.Controls.Add(this.btnSetPLDebounce);
			this.pPLActive.Controls.Add(this.lblBouncems);
			this.pPLActive.Controls.Add(this.lblPLbounceVal);
			this.pPLActive.Controls.Add(this.lblDebouncePL);
			this.pPLActive.Controls.Add(this.tbPL);
			this.pPLActive.Enabled = false;
			this.pPLActive.Location = new System.Drawing.Point(34, 305);
			this.pPLActive.Name = "pPLActive";
			this.pPLActive.Size = new System.Drawing.Size(488, 91);
			this.pPLActive.TabIndex = 221;
			// 
			// btnResetPLDebounce
			// 
			this.btnResetPLDebounce.BackColor = System.Drawing.Color.LightSlateGray;
			this.btnResetPLDebounce.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnResetPLDebounce.Location = new System.Drawing.Point(407, 56);
			this.btnResetPLDebounce.Name = "btnResetPLDebounce";
			this.btnResetPLDebounce.Size = new System.Drawing.Size(78, 30);
			this.btnResetPLDebounce.TabIndex = 220;
			this.btnResetPLDebounce.Text = "Reset";
			this.btnResetPLDebounce.UseVisualStyleBackColor = false;
			this.btnResetPLDebounce.Click += new System.EventHandler(this.btnResetPLDebounce_Click);
			// 
			// btnSetPLDebounce
			// 
			this.btnSetPLDebounce.BackColor = System.Drawing.Color.LightSlateGray;
			this.btnSetPLDebounce.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnSetPLDebounce.Location = new System.Drawing.Point(344, 57);
			this.btnSetPLDebounce.Name = "btnSetPLDebounce";
			this.btnSetPLDebounce.Size = new System.Drawing.Size(57, 28);
			this.btnSetPLDebounce.TabIndex = 219;
			this.btnSetPLDebounce.Text = "Set";
			this.btnSetPLDebounce.UseVisualStyleBackColor = false;
			this.btnSetPLDebounce.Click += new System.EventHandler(this.btnSetPLDebounce_Click);
			// 
			// lblBouncems
			// 
			this.lblBouncems.AutoSize = true;
			this.lblBouncems.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblBouncems.Location = new System.Drawing.Point(115, 18);
			this.lblBouncems.Name = "lblBouncems";
			this.lblBouncems.Size = new System.Drawing.Size(22, 13);
			this.lblBouncems.TabIndex = 209;
			this.lblBouncems.Text = "ms";
			// 
			// lblPLbounceVal
			// 
			this.lblPLbounceVal.AutoSize = true;
			this.lblPLbounceVal.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblPLbounceVal.Location = new System.Drawing.Point(66, 18);
			this.lblPLbounceVal.Name = "lblPLbounceVal";
			this.lblPLbounceVal.Size = new System.Drawing.Size(14, 13);
			this.lblPLbounceVal.TabIndex = 208;
			this.lblPLbounceVal.Text = "0";
			// 
			// lblDebouncePL
			// 
			this.lblDebouncePL.AutoSize = true;
			this.lblDebouncePL.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblDebouncePL.Location = new System.Drawing.Point(1, 18);
			this.lblDebouncePL.Name = "lblDebouncePL";
			this.lblDebouncePL.Size = new System.Drawing.Size(69, 13);
			this.lblDebouncePL.TabIndex = 207;
			this.lblDebouncePL.Text = "Debounce ";
			// 
			// tbPL
			// 
			this.tbPL.Location = new System.Drawing.Point(151, 12);
			this.tbPL.Maximum = 255;
			this.tbPL.Name = "tbPL";
			this.tbPL.Size = new System.Drawing.Size(334, 45);
			this.tbPL.TabIndex = 206;
			this.tbPL.TickFrequency = 15;
			this.tbPL.Scroll += new System.EventHandler(this.trackBarPL_Scroll_1);
			// 
			// p6
			// 
			this.p6.Controls.Add(this.gbPLDisappear);
			this.p6.Controls.Add(this.rdoClrDebouncePL);
			this.p6.Controls.Add(this.rdoDecDebouncePL);
			this.p6.Enabled = false;
			this.p6.Location = new System.Drawing.Point(49, 105);
			this.p6.Name = "p6";
			this.p6.Size = new System.Drawing.Size(454, 194);
			this.p6.TabIndex = 218;
			// 
			// gbPLDisappear
			// 
			this.gbPLDisappear.Controls.Add(this.label263);
			this.gbPLDisappear.Controls.Add(this.ddlHysteresisA);
			this.gbPLDisappear.Controls.Add(this.ddlPLTripA);
			this.gbPLDisappear.Controls.Add(this.label264);
			this.gbPLDisappear.Controls.Add(this.label265);
			this.gbPLDisappear.Controls.Add(this.ddlBFTripA);
			this.gbPLDisappear.Controls.Add(this.ddlZLockA);
			this.gbPLDisappear.Controls.Add(this.label266);
			this.gbPLDisappear.Location = new System.Drawing.Point(16, 3);
			this.gbPLDisappear.Name = "gbPLDisappear";
			this.gbPLDisappear.Size = new System.Drawing.Size(413, 157);
			this.gbPLDisappear.TabIndex = 218;
			this.gbPLDisappear.TabStop = false;
			// 
			// label263
			// 
			this.label263.AutoSize = true;
			this.label263.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label263.Location = new System.Drawing.Point(8, 124);
			this.label263.Name = "label263";
			this.label263.Size = new System.Drawing.Size(85, 13);
			this.label263.TabIndex = 217;
			this.label263.Text = "Hysteresis Angle";
			// 
			// ddlHysteresisA
			// 
			this.ddlHysteresisA.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ddlHysteresisA.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ddlHysteresisA.FormattingEnabled = true;
			this.ddlHysteresisA.Items.AddRange(new object[] {
            "+/- 0°",
            "+/- 4°",
            "+/- 7°",
            "+/- 11°",
            "+/- 14°",
            "+/- 17°",
            "+/- 21°",
            "+/- 24°",
            ""});
			this.ddlHysteresisA.Location = new System.Drawing.Point(156, 118);
			this.ddlHysteresisA.Name = "ddlHysteresisA";
			this.ddlHysteresisA.Size = new System.Drawing.Size(221, 21);
			this.ddlHysteresisA.TabIndex = 216;
			this.ddlHysteresisA.SelectedIndexChanged += new System.EventHandler(this.ddlHysteresisA_SelectedIndexChanged);
			// 
			// ddlPLTripA
			// 
			this.ddlPLTripA.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ddlPLTripA.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ddlPLTripA.FormattingEnabled = true;
			this.ddlPLTripA.Items.AddRange(new object[] {
            "15°",
            "20°",
            "30°",
            "35°",
            "40°",
            "45°",
            "55°",
            "60°",
            "70°",
            "75°"});
			this.ddlPLTripA.Location = new System.Drawing.Point(156, 88);
			this.ddlPLTripA.Name = "ddlPLTripA";
			this.ddlPLTripA.Size = new System.Drawing.Size(221, 21);
			this.ddlPLTripA.TabIndex = 215;
			this.ddlPLTripA.SelectedIndexChanged += new System.EventHandler(this.ddlPLTripA_SelectedIndexChanged);
			// 
			// label264
			// 
			this.label264.AutoSize = true;
			this.label264.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label264.Location = new System.Drawing.Point(22, 92);
			this.label264.Name = "label264";
			this.label264.Size = new System.Drawing.Size(71, 13);
			this.label264.TabIndex = 214;
			this.label264.Text = "P-LTrip Angle";
			// 
			// label265
			// 
			this.label265.AutoSize = true;
			this.label265.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label265.Location = new System.Drawing.Point(21, 62);
			this.label265.Name = "label265";
			this.label265.Size = new System.Drawing.Size(76, 13);
			this.label265.TabIndex = 213;
			this.label265.Text = "B/F Trip Angle";
			// 
			// ddlBFTripA
			// 
			this.ddlBFTripA.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ddlBFTripA.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ddlBFTripA.FormattingEnabled = true;
			this.ddlBFTripA.Items.AddRange(new object[] {
            "BF: Z<80°Z>280° FB: Z>100°Z<260°",
            "BF: Z<75°Z>285° FB: Z>105°Z<255°",
            "BF: Z<70°Z>290° FB: Z>110°Z<250°",
            "BF: Z<65°Z>295° FB: Z>115°Z<245°"});
			this.ddlBFTripA.Location = new System.Drawing.Point(156, 59);
			this.ddlBFTripA.Name = "ddlBFTripA";
			this.ddlBFTripA.Size = new System.Drawing.Size(221, 20);
			this.ddlBFTripA.TabIndex = 212;
			this.ddlBFTripA.SelectedIndexChanged += new System.EventHandler(this.ddlBFTripA_SelectedIndexChanged);
			// 
			// ddlZLockA
			// 
			this.ddlZLockA.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ddlZLockA.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ddlZLockA.FormattingEnabled = true;
			this.ddlZLockA.Items.AddRange(new object[] {
            "14°",
            "18°",
            "22°",
            "26°",
            "30°",
            "34°",
            "38°",
            "43°"});
			this.ddlZLockA.Location = new System.Drawing.Point(156, 26);
			this.ddlZLockA.Name = "ddlZLockA";
			this.ddlZLockA.Size = new System.Drawing.Size(221, 21);
			this.ddlZLockA.TabIndex = 211;
			this.ddlZLockA.SelectedIndexChanged += new System.EventHandler(this.ddlZLockA_SelectedIndexChanged);
			// 
			// label266
			// 
			this.label266.AutoSize = true;
			this.label266.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label266.Location = new System.Drawing.Point(22, 32);
			this.label266.Name = "label266";
			this.label266.Size = new System.Drawing.Size(74, 13);
			this.label266.TabIndex = 210;
			this.label266.Text = "Z- Lock Angle";
			// 
			// rdoClrDebouncePL
			// 
			this.rdoClrDebouncePL.AutoSize = true;
			this.rdoClrDebouncePL.Checked = true;
			this.rdoClrDebouncePL.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.rdoClrDebouncePL.Location = new System.Drawing.Point(193, 163);
			this.rdoClrDebouncePL.Name = "rdoClrDebouncePL";
			this.rdoClrDebouncePL.Size = new System.Drawing.Size(102, 17);
			this.rdoClrDebouncePL.TabIndex = 200;
			this.rdoClrDebouncePL.TabStop = true;
			this.rdoClrDebouncePL.Text = "Clear Debounce";
			this.rdoClrDebouncePL.UseVisualStyleBackColor = true;
			this.rdoClrDebouncePL.CheckedChanged += new System.EventHandler(this.rdoClrDebouncePL_CheckedChanged);
			// 
			// rdoDecDebouncePL
			// 
			this.rdoDecDebouncePL.AutoSize = true;
			this.rdoDecDebouncePL.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.rdoDecDebouncePL.Location = new System.Drawing.Point(42, 163);
			this.rdoDecDebouncePL.Name = "rdoDecDebouncePL";
			this.rdoDecDebouncePL.Size = new System.Drawing.Size(130, 17);
			this.rdoDecDebouncePL.TabIndex = 199;
			this.rdoDecDebouncePL.Text = "Decrement Debounce";
			this.rdoDecDebouncePL.UseVisualStyleBackColor = true;
			this.rdoDecDebouncePL.CheckedChanged += new System.EventHandler(this.rdoDecDebouncePL_CheckedChanged);
			// 
			// chkEnablePL
			// 
			this.chkEnablePL.AutoSize = true;
			this.chkEnablePL.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chkEnablePL.Location = new System.Drawing.Point(11, 38);
			this.chkEnablePL.Name = "chkEnablePL";
			this.chkEnablePL.Size = new System.Drawing.Size(94, 17);
			this.chkEnablePL.TabIndex = 202;
			this.chkEnablePL.Text = "Enable P/L ";
			this.chkEnablePL.UseVisualStyleBackColor = true;
			this.chkEnablePL.CheckedChanged += new System.EventHandler(this.chkEnablePL_CheckedChanged);
			// 
			// p7
			// 
			this.p7.BackColor = System.Drawing.Color.LightSlateGray;
			this.p7.Controls.Add(this.label191);
			this.p7.Controls.Add(this.ledCurPLNew);
			this.p7.Controls.Add(this.label273);
			this.p7.Controls.Add(this.ledCurPLRight);
			this.p7.Controls.Add(this.label275);
			this.p7.Controls.Add(this.ledCurPLLeft);
			this.p7.Controls.Add(this.label277);
			this.p7.Controls.Add(this.ledCurPLDown);
			this.p7.Controls.Add(this.label279);
			this.p7.Controls.Add(this.ledCurPLUp);
			this.p7.Controls.Add(this.label281);
			this.p7.Controls.Add(this.ledCurPLBack);
			this.p7.Controls.Add(this.label283);
			this.p7.Controls.Add(this.ledCurPLFront);
			this.p7.Controls.Add(this.ledCurPLLO);
			this.p7.Controls.Add(this.label285);
			this.p7.ForeColor = System.Drawing.Color.White;
			this.p7.Location = new System.Drawing.Point(733, 256);
			this.p7.Name = "p7";
			this.p7.Size = new System.Drawing.Size(356, 194);
			this.p7.TabIndex = 204;
			// 
			// label191
			// 
			this.label191.AutoSize = true;
			this.label191.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label191.ForeColor = System.Drawing.Color.White;
			this.label191.Location = new System.Drawing.Point(29, 18);
			this.label191.Name = "label191";
			this.label191.Size = new System.Drawing.Size(269, 20);
			this.label191.TabIndex = 203;
			this.label191.Text = "New Portrait/Landscape Position";
			// 
			// ledCurPLNew
			// 
			this.ledCurPLNew.ForeColor = System.Drawing.Color.White;
			this.ledCurPLNew.LedStyle = NationalInstruments.UI.LedStyle.Round3D;
			this.ledCurPLNew.Location = new System.Drawing.Point(304, 10);
			this.ledCurPLNew.Name = "ledCurPLNew";
			this.ledCurPLNew.OffColor = System.Drawing.Color.Red;
			this.ledCurPLNew.Size = new System.Drawing.Size(42, 42);
			this.ledCurPLNew.TabIndex = 202;
			// 
			// label273
			// 
			this.label273.AutoSize = true;
			this.label273.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label273.ForeColor = System.Drawing.Color.White;
			this.label273.Location = new System.Drawing.Point(234, 157);
			this.label273.Name = "label273";
			this.label273.Size = new System.Drawing.Size(44, 16);
			this.label273.TabIndex = 200;
			this.label273.Text = "Right";
			// 
			// ledCurPLRight
			// 
			this.ledCurPLRight.ForeColor = System.Drawing.Color.White;
			this.ledCurPLRight.LedStyle = NationalInstruments.UI.LedStyle.Round3D;
			this.ledCurPLRight.Location = new System.Drawing.Point(278, 154);
			this.ledCurPLRight.Name = "ledCurPLRight";
			this.ledCurPLRight.OffColor = System.Drawing.Color.Red;
			this.ledCurPLRight.Size = new System.Drawing.Size(22, 22);
			this.ledCurPLRight.TabIndex = 199;
			// 
			// label275
			// 
			this.label275.AutoSize = true;
			this.label275.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label275.ForeColor = System.Drawing.Color.White;
			this.label275.Location = new System.Drawing.Point(242, 133);
			this.label275.Name = "label275";
			this.label275.Size = new System.Drawing.Size(33, 16);
			this.label275.TabIndex = 198;
			this.label275.Text = "Left";
			// 
			// ledCurPLLeft
			// 
			this.ledCurPLLeft.ForeColor = System.Drawing.Color.White;
			this.ledCurPLLeft.LedStyle = NationalInstruments.UI.LedStyle.Round3D;
			this.ledCurPLLeft.Location = new System.Drawing.Point(278, 131);
			this.ledCurPLLeft.Name = "ledCurPLLeft";
			this.ledCurPLLeft.OffColor = System.Drawing.Color.Red;
			this.ledCurPLLeft.Size = new System.Drawing.Size(22, 22);
			this.ledCurPLLeft.TabIndex = 197;
			// 
			// label277
			// 
			this.label277.AutoSize = true;
			this.label277.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label277.ForeColor = System.Drawing.Color.White;
			this.label277.Location = new System.Drawing.Point(232, 111);
			this.label277.Name = "label277";
			this.label277.Size = new System.Drawing.Size(46, 16);
			this.label277.TabIndex = 196;
			this.label277.Text = "Down";
			// 
			// ledCurPLDown
			// 
			this.ledCurPLDown.ForeColor = System.Drawing.Color.White;
			this.ledCurPLDown.LedStyle = NationalInstruments.UI.LedStyle.Round3D;
			this.ledCurPLDown.Location = new System.Drawing.Point(278, 109);
			this.ledCurPLDown.Name = "ledCurPLDown";
			this.ledCurPLDown.OffColor = System.Drawing.Color.Red;
			this.ledCurPLDown.Size = new System.Drawing.Size(22, 22);
			this.ledCurPLDown.TabIndex = 195;
			// 
			// label279
			// 
			this.label279.AutoSize = true;
			this.label279.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label279.ForeColor = System.Drawing.Color.White;
			this.label279.Location = new System.Drawing.Point(245, 89);
			this.label279.Name = "label279";
			this.label279.Size = new System.Drawing.Size(28, 16);
			this.label279.TabIndex = 194;
			this.label279.Text = "Up";
			// 
			// ledCurPLUp
			// 
			this.ledCurPLUp.ForeColor = System.Drawing.Color.White;
			this.ledCurPLUp.LedStyle = NationalInstruments.UI.LedStyle.Round3D;
			this.ledCurPLUp.Location = new System.Drawing.Point(278, 86);
			this.ledCurPLUp.Name = "ledCurPLUp";
			this.ledCurPLUp.OffColor = System.Drawing.Color.Red;
			this.ledCurPLUp.Size = new System.Drawing.Size(22, 22);
			this.ledCurPLUp.TabIndex = 193;
			// 
			// label281
			// 
			this.label281.AutoSize = true;
			this.label281.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label281.ForeColor = System.Drawing.Color.White;
			this.label281.Location = new System.Drawing.Point(135, 113);
			this.label281.Name = "label281";
			this.label281.Size = new System.Drawing.Size(43, 16);
			this.label281.TabIndex = 192;
			this.label281.Text = "Back";
			// 
			// ledCurPLBack
			// 
			this.ledCurPLBack.ForeColor = System.Drawing.Color.White;
			this.ledCurPLBack.LedStyle = NationalInstruments.UI.LedStyle.Round3D;
			this.ledCurPLBack.Location = new System.Drawing.Point(178, 110);
			this.ledCurPLBack.Name = "ledCurPLBack";
			this.ledCurPLBack.OffColor = System.Drawing.Color.Red;
			this.ledCurPLBack.Size = new System.Drawing.Size(22, 22);
			this.ledCurPLBack.TabIndex = 191;
			// 
			// label283
			// 
			this.label283.AutoSize = true;
			this.label283.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label283.ForeColor = System.Drawing.Color.White;
			this.label283.Location = new System.Drawing.Point(132, 89);
			this.label283.Name = "label283";
			this.label283.Size = new System.Drawing.Size(43, 16);
			this.label283.TabIndex = 190;
			this.label283.Text = "Front";
			// 
			// ledCurPLFront
			// 
			this.ledCurPLFront.ForeColor = System.Drawing.Color.White;
			this.ledCurPLFront.LedStyle = NationalInstruments.UI.LedStyle.Round3D;
			this.ledCurPLFront.Location = new System.Drawing.Point(178, 86);
			this.ledCurPLFront.Name = "ledCurPLFront";
			this.ledCurPLFront.OffColor = System.Drawing.Color.Red;
			this.ledCurPLFront.Size = new System.Drawing.Size(22, 22);
			this.ledCurPLFront.TabIndex = 189;
			// 
			// ledCurPLLO
			// 
			this.ledCurPLLO.ForeColor = System.Drawing.Color.White;
			this.ledCurPLLO.LedStyle = NationalInstruments.UI.LedStyle.Round3D;
			this.ledCurPLLO.Location = new System.Drawing.Point(75, 88);
			this.ledCurPLLO.Name = "ledCurPLLO";
			this.ledCurPLLO.OffColor = System.Drawing.Color.Red;
			this.ledCurPLLO.Size = new System.Drawing.Size(22, 22);
			this.ledCurPLLO.TabIndex = 187;
			// 
			// label285
			// 
			this.label285.AutoSize = true;
			this.label285.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label285.ForeColor = System.Drawing.Color.White;
			this.label285.Location = new System.Drawing.Point(14, 89);
			this.label285.Name = "label285";
			this.label285.Size = new System.Drawing.Size(62, 16);
			this.label285.TabIndex = 188;
			this.label285.Text = "Lockout";
			// 
			// MainScreenEval
			// 
			this.MainScreenEval.BackColor = System.Drawing.Color.LightSlateGray;
			this.MainScreenEval.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.MainScreenEval.Controls.Add(this.groupBox8);
			this.MainScreenEval.Controls.Add(this.uxStart);
			this.MainScreenEval.Controls.Add(this.groupBox7);
			this.MainScreenEval.Controls.Add(this.groupBox5);
			this.MainScreenEval.Controls.Add(this.gbST);
			this.MainScreenEval.Controls.Add(this.gbXD);
			this.MainScreenEval.Controls.Add(this.MainScreenGraph);
			this.MainScreenEval.Controls.Add(this.uxPanelSettings);
			this.MainScreenEval.ForeColor = System.Drawing.Color.White;
			this.MainScreenEval.Location = new System.Drawing.Point(4, 25);
			this.MainScreenEval.Name = "MainScreenEval";
			this.MainScreenEval.Padding = new System.Windows.Forms.Padding(3);
			this.MainScreenEval.Size = new System.Drawing.Size(1124, 525);
			this.MainScreenEval.TabIndex = 0;
			this.MainScreenEval.Text = "Main Screen";
			// 
			// groupBox8
			// 
			this.groupBox8.Controls.Add(this.panel3);
			this.groupBox8.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.groupBox8.ForeColor = System.Drawing.Color.White;
			this.groupBox8.Location = new System.Drawing.Point(10, 387);
			this.groupBox8.Name = "groupBox8";
			this.groupBox8.Size = new System.Drawing.Size(372, 120);
			this.groupBox8.TabIndex = 241;
			this.groupBox8.TabStop = false;
			this.groupBox8.Text = "Calibration";
			// 
			// panel3
			// 
			this.panel3.BackColor = System.Drawing.Color.LightSlateGray;
			this.panel3.Controls.Add(this.gbOC);
			this.panel3.Controls.Add(this.btnAutoCal);
			this.panel3.Location = new System.Drawing.Point(6, 21);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(360, 93);
			this.panel3.TabIndex = 228;
			// 
			// gbOC
			// 
			this.gbOC.BackColor = System.Drawing.Color.LightSlateGray;
			this.gbOC.Controls.Add(this.label34);
			this.gbOC.Controls.Add(this.label46);
			this.gbOC.Controls.Add(this.label47);
			this.gbOC.Controls.Add(this.btnWriteCal);
			this.gbOC.Controls.Add(this.label43);
			this.gbOC.Controls.Add(this.txtCalX);
			this.gbOC.Controls.Add(this.label44);
			this.gbOC.Controls.Add(this.label45);
			this.gbOC.Controls.Add(this.txtCalY);
			this.gbOC.Controls.Add(this.txtCalZ);
			this.gbOC.Controls.Add(this.lblXCal);
			this.gbOC.Controls.Add(this.lblYCal);
			this.gbOC.Controls.Add(this.lblZCal);
			this.gbOC.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.gbOC.ForeColor = System.Drawing.Color.White;
			this.gbOC.Location = new System.Drawing.Point(3, 7);
			this.gbOC.Name = "gbOC";
			this.gbOC.Size = new System.Drawing.Size(253, 83);
			this.gbOC.TabIndex = 123;
			this.gbOC.TabStop = false;
			this.gbOC.Text = "Offset Calibration";
			// 
			// label34
			// 
			this.label34.AutoSize = true;
			this.label34.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label34.ForeColor = System.Drawing.Color.White;
			this.label34.Location = new System.Drawing.Point(85, 22);
			this.label34.Name = "label34";
			this.label34.Size = new System.Drawing.Size(45, 13);
			this.label34.TabIndex = 22;
			this.label34.Text = "counts";
			// 
			// label46
			// 
			this.label46.AutoSize = true;
			this.label46.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label46.ForeColor = System.Drawing.Color.White;
			this.label46.Location = new System.Drawing.Point(85, 41);
			this.label46.Name = "label46";
			this.label46.Size = new System.Drawing.Size(45, 13);
			this.label46.TabIndex = 23;
			this.label46.Text = "counts";
			// 
			// label47
			// 
			this.label47.AutoSize = true;
			this.label47.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label47.ForeColor = System.Drawing.Color.White;
			this.label47.Location = new System.Drawing.Point(85, 60);
			this.label47.Name = "label47";
			this.label47.Size = new System.Drawing.Size(45, 13);
			this.label47.TabIndex = 24;
			this.label47.Text = "counts";
			// 
			// btnWriteCal
			// 
			this.btnWriteCal.BackColor = System.Drawing.Color.LightSlateGray;
			this.btnWriteCal.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnWriteCal.ForeColor = System.Drawing.Color.White;
			this.btnWriteCal.Location = new System.Drawing.Point(186, 26);
			this.btnWriteCal.Name = "btnWriteCal";
			this.btnWriteCal.Size = new System.Drawing.Size(56, 44);
			this.btnWriteCal.TabIndex = 21;
			this.btnWriteCal.Text = "Write";
			this.toolTip1.SetToolTip(this.btnWriteCal, "Based on 8g count values. This will write the offset values to the XYZ calibratio" +
        "n registers");
			this.btnWriteCal.UseVisualStyleBackColor = false;
			this.btnWriteCal.Click += new System.EventHandler(this.btnWriteCal_Click);
			// 
			// label43
			// 
			this.label43.AutoSize = true;
			this.label43.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label43.ForeColor = System.Drawing.Color.White;
			this.label43.Location = new System.Drawing.Point(4, 22);
			this.label43.Name = "label43";
			this.label43.Size = new System.Drawing.Size(48, 13);
			this.label43.TabIndex = 12;
			this.label43.Text = "XCal = ";
			// 
			// txtCalX
			// 
			this.txtCalX.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtCalX.Location = new System.Drawing.Point(133, 17);
			this.txtCalX.Name = "txtCalX";
			this.txtCalX.Size = new System.Drawing.Size(35, 20);
			this.txtCalX.TabIndex = 13;
			this.txtCalX.Text = "0";
			// 
			// label44
			// 
			this.label44.AutoSize = true;
			this.label44.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label44.ForeColor = System.Drawing.Color.White;
			this.label44.Location = new System.Drawing.Point(5, 40);
			this.label44.Name = "label44";
			this.label44.Size = new System.Drawing.Size(48, 13);
			this.label44.TabIndex = 14;
			this.label44.Text = "YCal = ";
			// 
			// label45
			// 
			this.label45.AutoSize = true;
			this.label45.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label45.ForeColor = System.Drawing.Color.White;
			this.label45.Location = new System.Drawing.Point(5, 59);
			this.label45.Name = "label45";
			this.label45.Size = new System.Drawing.Size(48, 13);
			this.label45.TabIndex = 15;
			this.label45.Text = "ZCal = ";
			// 
			// txtCalY
			// 
			this.txtCalY.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtCalY.Location = new System.Drawing.Point(133, 37);
			this.txtCalY.Name = "txtCalY";
			this.txtCalY.Size = new System.Drawing.Size(35, 20);
			this.txtCalY.TabIndex = 16;
			this.txtCalY.Text = "0";
			// 
			// txtCalZ
			// 
			this.txtCalZ.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtCalZ.Location = new System.Drawing.Point(133, 57);
			this.txtCalZ.Name = "txtCalZ";
			this.txtCalZ.Size = new System.Drawing.Size(35, 20);
			this.txtCalZ.TabIndex = 17;
			this.txtCalZ.Text = "0";
			// 
			// lblXCal
			// 
			this.lblXCal.AutoSize = true;
			this.lblXCal.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblXCal.ForeColor = System.Drawing.Color.White;
			this.lblXCal.Location = new System.Drawing.Point(54, 22);
			this.lblXCal.Name = "lblXCal";
			this.lblXCal.Size = new System.Drawing.Size(32, 13);
			this.lblXCal.TabIndex = 18;
			this.lblXCal.Text = "Xcal";
			// 
			// lblYCal
			// 
			this.lblYCal.AutoSize = true;
			this.lblYCal.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblYCal.ForeColor = System.Drawing.Color.White;
			this.lblYCal.Location = new System.Drawing.Point(54, 41);
			this.lblYCal.Name = "lblYCal";
			this.lblYCal.Size = new System.Drawing.Size(32, 13);
			this.lblYCal.TabIndex = 19;
			this.lblYCal.Text = "Ycal";
			// 
			// lblZCal
			// 
			this.lblZCal.AutoSize = true;
			this.lblZCal.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblZCal.ForeColor = System.Drawing.Color.White;
			this.lblZCal.Location = new System.Drawing.Point(54, 60);
			this.lblZCal.Name = "lblZCal";
			this.lblZCal.Size = new System.Drawing.Size(32, 13);
			this.lblZCal.TabIndex = 20;
			this.lblZCal.Text = "Zcal";
			// 
			// btnAutoCal
			// 
			this.btnAutoCal.BackColor = System.Drawing.Color.LightSlateGray;
			this.btnAutoCal.FlatAppearance.BorderColor = System.Drawing.Color.Fuchsia;
			this.btnAutoCal.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnAutoCal.ForeColor = System.Drawing.Color.White;
			this.btnAutoCal.Location = new System.Drawing.Point(261, 12);
			this.btnAutoCal.Name = "btnAutoCal";
			this.btnAutoCal.Size = new System.Drawing.Size(96, 80);
			this.btnAutoCal.TabIndex = 25;
			this.btnAutoCal.Text = "Auto Calibrate";
			this.toolTip1.SetToolTip(this.btnAutoCal, "Calibrates the XYZ offset.  Keep board in the x=0g,y=0g and z=+/-1g position");
			this.btnAutoCal.UseVisualStyleBackColor = false;
			this.btnAutoCal.Click += new System.EventHandler(this.btnAutoCal_Click);
			// 
			// uxStart
			// 
			this.uxStart.Appearance = System.Windows.Forms.Appearance.Button;
			this.uxStart.BackColor = System.Drawing.Color.SlateGray;
			this.uxStart.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.uxStart.Location = new System.Drawing.Point(10, 336);
			this.uxStart.Name = "uxStart";
			this.uxStart.Size = new System.Drawing.Size(109, 33);
			this.uxStart.TabIndex = 240;
			this.uxStart.Text = "Start";
			this.uxStart.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.uxStart.UseVisualStyleBackColor = false;
			this.uxStart.CheckedChanged += new System.EventHandler(this.uxStart_CheckedChanged);
			// 
			// groupBox7
			// 
			this.groupBox7.Controls.Add(this.p14b8bSelect);
			this.groupBox7.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.groupBox7.ForeColor = System.Drawing.Color.White;
			this.groupBox7.Location = new System.Drawing.Point(10, 2);
			this.groupBox7.Name = "groupBox7";
			this.groupBox7.Size = new System.Drawing.Size(112, 75);
			this.groupBox7.TabIndex = 239;
			this.groupBox7.TabStop = false;
			this.groupBox7.Text = "Resolution";
			// 
			// p14b8bSelect
			// 
			this.p14b8bSelect.BackColor = System.Drawing.Color.LightSlateGray;
			this.p14b8bSelect.Controls.Add(this.rdoXYZFullResMain);
			this.p14b8bSelect.Controls.Add(this.rdo8bitDataMain);
			this.p14b8bSelect.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.p14b8bSelect.Location = new System.Drawing.Point(18, 17);
			this.p14b8bSelect.Name = "p14b8bSelect";
			this.p14b8bSelect.Size = new System.Drawing.Size(78, 52);
			this.p14b8bSelect.TabIndex = 225;
			// 
			// rdoXYZFullResMain
			// 
			this.rdoXYZFullResMain.AutoSize = true;
			this.rdoXYZFullResMain.Checked = true;
			this.rdoXYZFullResMain.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.rdoXYZFullResMain.ForeColor = System.Drawing.Color.White;
			this.rdoXYZFullResMain.Location = new System.Drawing.Point(4, 6);
			this.rdoXYZFullResMain.Name = "rdoXYZFullResMain";
			this.rdoXYZFullResMain.Size = new System.Drawing.Size(63, 19);
			this.rdoXYZFullResMain.TabIndex = 223;
			this.rdoXYZFullResMain.TabStop = true;
			this.rdoXYZFullResMain.Text = "14 bits";
			this.rdoXYZFullResMain.UseVisualStyleBackColor = true;
			this.rdoXYZFullResMain.CheckedChanged += new System.EventHandler(this.rdoXYZFullResMain_CheckedChanged);
			// 
			// rdo8bitDataMain
			// 
			this.rdo8bitDataMain.AutoSize = true;
			this.rdo8bitDataMain.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.rdo8bitDataMain.ForeColor = System.Drawing.Color.White;
			this.rdo8bitDataMain.Location = new System.Drawing.Point(4, 28);
			this.rdo8bitDataMain.Name = "rdo8bitDataMain";
			this.rdo8bitDataMain.Size = new System.Drawing.Size(56, 19);
			this.rdo8bitDataMain.TabIndex = 222;
			this.rdo8bitDataMain.Text = "8 bits";
			this.rdo8bitDataMain.UseVisualStyleBackColor = true;
			this.rdo8bitDataMain.CheckedChanged += new System.EventHandler(this.rdo8bitDataMain_CheckedChanged);
			// 
			// groupBox5
			// 
			this.groupBox5.BackColor = System.Drawing.Color.Transparent;
			this.groupBox5.Controls.Add(this.legend1);
			this.groupBox5.Controls.Add(this.chkEnableZAxis);
			this.groupBox5.Controls.Add(this.chkEnableX);
			this.groupBox5.Controls.Add(this.chkEnableYAxis);
			this.groupBox5.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.groupBox5.ForeColor = System.Drawing.Color.White;
			this.groupBox5.Location = new System.Drawing.Point(10, 77);
			this.groupBox5.Name = "groupBox5";
			this.groupBox5.Size = new System.Drawing.Size(112, 100);
			this.groupBox5.TabIndex = 238;
			this.groupBox5.TabStop = false;
			this.groupBox5.Text = "View";
			// 
			// legend1
			// 
			this.legend1.CaptionFont = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.legend1.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.legend1.Items.AddRange(new NationalInstruments.UI.LegendItem[] {
            this.legendItem1,
            this.legendItem2,
            this.legendItem3});
			this.legend1.ItemSize = new System.Drawing.Size(15, 15);
			this.legend1.Location = new System.Drawing.Point(28, 25);
			this.legend1.Name = "legend1";
			this.legend1.Size = new System.Drawing.Size(68, 65);
			this.legend1.TabIndex = 232;
			// 
			// legendItem1
			// 
			this.legendItem1.Source = this.uxAccelX;
			this.legendItem1.Text = "X-Axis";
			// 
			// uxAccelX
			// 
			this.uxAccelX.CanScaleYAxis = true;
			this.uxAccelX.LineColor = System.Drawing.Color.Red;
			this.uxAccelX.XAxis = this.uxAxisSamples;
			this.uxAccelX.YAxis = this.uxAxisAccel;
			// 
			// uxAxisSamples
			// 
			this.uxAxisSamples.Caption = "Samples";
			this.uxAxisSamples.CaptionFont = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.uxAxisSamples.MajorDivisions.GridColor = System.Drawing.Color.DarkGray;
			this.uxAxisSamples.MajorDivisions.GridVisible = true;
			this.uxAxisSamples.MajorDivisions.LabelFont = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.uxAxisSamples.Range = NationalInstruments.UI.Range.Parse("0; 1000");
			// 
			// uxAxisAccel
			// 
			this.uxAxisAccel.AutoSpacing = false;
			this.uxAxisAccel.Caption = "Acceleration/g";
			this.uxAxisAccel.CaptionFont = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.uxAxisAccel.MajorDivisions.GridColor = System.Drawing.Color.DarkGray;
			this.uxAxisAccel.MajorDivisions.GridVisible = true;
			this.uxAxisAccel.MajorDivisions.LabelFont = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.uxAxisAccel.Mode = NationalInstruments.UI.AxisMode.Fixed;
			this.uxAxisAccel.Range = NationalInstruments.UI.Range.Parse("-0,5; 0,5");
			// 
			// legendItem2
			// 
			this.legendItem2.Source = this.uxAccelY;
			this.legendItem2.Text = "Y-Axis";
			// 
			// uxAccelY
			// 
			this.uxAccelY.CanScaleYAxis = true;
			this.uxAccelY.LineColor = System.Drawing.Color.White;
			this.uxAccelY.PointColor = System.Drawing.Color.DarkGreen;
			this.uxAccelY.XAxis = this.uxAxisSamples;
			this.uxAccelY.YAxis = this.uxAxisAccel;
			// 
			// legendItem3
			// 
			this.legendItem3.Source = this.uxAccelZ;
			this.legendItem3.Text = "Z-Axis";
			// 
			// uxAccelZ
			// 
			this.uxAccelZ.CanScaleYAxis = true;
			this.uxAccelZ.LineColor = System.Drawing.Color.Blue;
			this.uxAccelZ.PointColor = System.Drawing.Color.White;
			this.uxAccelZ.XAxis = this.uxAxisSamples;
			this.uxAccelZ.YAxis = this.uxAxisAccel;
			// 
			// chkEnableZAxis
			// 
			this.chkEnableZAxis.AutoSize = true;
			this.chkEnableZAxis.Checked = true;
			this.chkEnableZAxis.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkEnableZAxis.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chkEnableZAxis.Location = new System.Drawing.Point(10, 71);
			this.chkEnableZAxis.Name = "chkEnableZAxis";
			this.chkEnableZAxis.Size = new System.Drawing.Size(15, 14);
			this.chkEnableZAxis.TabIndex = 237;
			this.chkEnableZAxis.UseVisualStyleBackColor = true;
			this.chkEnableZAxis.CheckedChanged += new System.EventHandler(this.chkEnableZAxis_CheckedChanged);
			// 
			// chkEnableX
			// 
			this.chkEnableX.AutoSize = true;
			this.chkEnableX.Checked = true;
			this.chkEnableX.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkEnableX.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chkEnableX.Location = new System.Drawing.Point(10, 31);
			this.chkEnableX.Name = "chkEnableX";
			this.chkEnableX.Size = new System.Drawing.Size(15, 14);
			this.chkEnableX.TabIndex = 235;
			this.chkEnableX.UseVisualStyleBackColor = true;
			this.chkEnableX.CheckedChanged += new System.EventHandler(this.chkEnableX_CheckedChanged);
			// 
			// chkEnableYAxis
			// 
			this.chkEnableYAxis.AutoSize = true;
			this.chkEnableYAxis.Checked = true;
			this.chkEnableYAxis.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkEnableYAxis.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chkEnableYAxis.Location = new System.Drawing.Point(10, 51);
			this.chkEnableYAxis.Name = "chkEnableYAxis";
			this.chkEnableYAxis.Size = new System.Drawing.Size(15, 14);
			this.chkEnableYAxis.TabIndex = 236;
			this.chkEnableYAxis.UseVisualStyleBackColor = true;
			this.chkEnableYAxis.CheckedChanged += new System.EventHandler(this.chkEnableYAxis_CheckedChanged);
			// 
			// gbST
			// 
			this.gbST.BackColor = System.Drawing.Color.LightSlateGray;
			this.gbST.Controls.Add(this.chkSelfTest);
			this.gbST.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.gbST.ForeColor = System.Drawing.Color.White;
			this.gbST.Location = new System.Drawing.Point(414, 415);
			this.gbST.Name = "gbST";
			this.gbST.Size = new System.Drawing.Size(103, 83);
			this.gbST.TabIndex = 201;
			this.gbST.TabStop = false;
			this.gbST.Text = "Self Test";
			// 
			// chkSelfTest
			// 
			this.chkSelfTest.AutoSize = true;
			this.chkSelfTest.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chkSelfTest.Location = new System.Drawing.Point(21, 36);
			this.chkSelfTest.Name = "chkSelfTest";
			this.chkSelfTest.Size = new System.Drawing.Size(65, 17);
			this.chkSelfTest.TabIndex = 0;
			this.chkSelfTest.Text = "Enable";
			this.toolTip1.SetToolTip(this.chkSelfTest, "Verify sensor is working. Z axis data changes by 1g");
			this.chkSelfTest.UseVisualStyleBackColor = true;
			this.chkSelfTest.CheckedChanged += new System.EventHandler(this.chkSelfTest_CheckedChanged);
			// 
			// gbXD
			// 
			this.gbXD.BackColor = System.Drawing.Color.LightSlateGray;
			this.gbXD.Controls.Add(this.uxAverageData);
			this.gbXD.Controls.Add(this.label23);
			this.gbXD.Controls.Add(this.label7);
			this.gbXD.Controls.Add(this.uxSwitchCounts);
			this.gbXD.Controls.Add(this.uxAccelZValue);
			this.gbXD.Controls.Add(this.uxAccelYValue);
			this.gbXD.Controls.Add(this.uxAccelXValue);
			this.gbXD.Controls.Add(this.label22);
			this.gbXD.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.gbXD.ForeColor = System.Drawing.Color.White;
			this.gbXD.Location = new System.Drawing.Point(9, 182);
			this.gbXD.Name = "gbXD";
			this.gbXD.Size = new System.Drawing.Size(112, 141);
			this.gbXD.TabIndex = 122;
			this.gbXD.TabStop = false;
			this.gbXD.Text = "XYZ Data";
			// 
			// uxAverageData
			// 
			this.uxAverageData.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.uxAverageData.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.uxAverageData.FormattingEnabled = true;
			this.uxAverageData.Items.AddRange(new object[] {
            "1",
            "2",
            "4",
            "8",
            "16"});
			this.uxAverageData.Location = new System.Drawing.Point(64, 43);
			this.uxAverageData.Name = "uxAverageData";
			this.uxAverageData.Size = new System.Drawing.Size(45, 23);
			this.uxAverageData.TabIndex = 241;
			this.uxAverageData.SelectedIndexChanged += new System.EventHandler(this.uxAverageData_SelectedIndexChanged);
			// 
			// label22
			// 
			this.label22.AutoSize = true;
			this.label22.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label22.Location = new System.Drawing.Point(3, 23);
			this.label22.Name = "label22";
			this.label22.Size = new System.Drawing.Size(22, 15);
			this.label22.TabIndex = 240;
			this.label22.Text = "g\'s";
			// 
			// label23
			// 
			this.label23.AutoSize = true;
			this.label23.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label23.Location = new System.Drawing.Point(4, 47);
			this.label23.Name = "label23";
			this.label23.Size = new System.Drawing.Size(53, 15);
			this.label23.TabIndex = 240;
			this.label23.Text = "Average:";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label7.Location = new System.Drawing.Point(57, 23);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(45, 15);
			this.label7.TabIndex = 240;
			this.label7.Text = "Counts";
			// 
			// uxSwitchCounts
			// 
			this.uxSwitchCounts.Location = new System.Drawing.Point(29, 18);
			this.uxSwitchCounts.Name = "uxSwitchCounts";
			this.uxSwitchCounts.Size = new System.Drawing.Size(27, 28);
			this.uxSwitchCounts.SwitchStyle = NationalInstruments.UI.SwitchStyle.HorizontalSlide3D;
			this.uxSwitchCounts.TabIndex = 239;
			this.uxSwitchCounts.StateChanged += new NationalInstruments.UI.ActionEventHandler(this.uxSwitchCounts_StateChanged);
			// 
			// uxAccelZValue
			// 
			this.uxAccelZValue.AutoPad = false;
			this.uxAccelZValue.BackColor = System.Drawing.Color.LightSlateGray;
			this.uxAccelZValue.BackGround = System.Drawing.Color.Transparent;
			this.uxAccelZValue.BorderColor = System.Drawing.Color.Black;
			this.uxAccelZValue.BorderSpace = 3;
			this.uxAccelZValue.CharSpacing = 2;
			this.uxAccelZValue.DotMatrix = LCDLabel.DotMatrix.mat5x7;
			this.uxAccelZValue.LineSpacing = 2;
			this.uxAccelZValue.Location = new System.Drawing.Point(8, 114);
			this.uxAccelZValue.Name = "uxAccelZValue";
			this.uxAccelZValue.NumberOfCharacters = 8;
			this.uxAccelZValue.PixelHeight = 1;
			this.uxAccelZValue.PixelOff = System.Drawing.Color.Transparent;
			this.uxAccelZValue.PixelOn = System.Drawing.Color.Gold;
			this.uxAccelZValue.PixelShape = LCDLabel.PixelShape.Square;
			this.uxAccelZValue.PixelSize = LCDLabel.PixelSize.pix1x1;
			this.uxAccelZValue.PixelSpacing = 1;
			this.uxAccelZValue.PixelWidth = 1;
			this.uxAccelZValue.Size = new System.Drawing.Size(94, 21);
			this.uxAccelZValue.TabIndex = 238;
			this.uxAccelZValue.Text = "X=1.45";
			this.uxAccelZValue.TextLines = 1;
			// 
			// uxAccelYValue
			// 
			this.uxAccelYValue.AutoPad = false;
			this.uxAccelYValue.BackColor = System.Drawing.Color.LightSlateGray;
			this.uxAccelYValue.BackGround = System.Drawing.Color.Transparent;
			this.uxAccelYValue.BorderColor = System.Drawing.Color.Black;
			this.uxAccelYValue.BorderSpace = 3;
			this.uxAccelYValue.CharSpacing = 2;
			this.uxAccelYValue.DotMatrix = LCDLabel.DotMatrix.mat5x7;
			this.uxAccelYValue.LineSpacing = 2;
			this.uxAccelYValue.Location = new System.Drawing.Point(8, 93);
			this.uxAccelYValue.Name = "uxAccelYValue";
			this.uxAccelYValue.NumberOfCharacters = 8;
			this.uxAccelYValue.PixelHeight = 1;
			this.uxAccelYValue.PixelOff = System.Drawing.Color.Transparent;
			this.uxAccelYValue.PixelOn = System.Drawing.Color.Gold;
			this.uxAccelYValue.PixelShape = LCDLabel.PixelShape.Square;
			this.uxAccelYValue.PixelSize = LCDLabel.PixelSize.pix1x1;
			this.uxAccelYValue.PixelSpacing = 1;
			this.uxAccelYValue.PixelWidth = 1;
			this.uxAccelYValue.Size = new System.Drawing.Size(94, 21);
			this.uxAccelYValue.TabIndex = 238;
			this.uxAccelYValue.Text = "X=1.45";
			this.uxAccelYValue.TextLines = 1;
			// 
			// uxAccelXValue
			// 
			this.uxAccelXValue.AutoPad = false;
			this.uxAccelXValue.BackColor = System.Drawing.Color.LightSlateGray;
			this.uxAccelXValue.BackGround = System.Drawing.Color.Transparent;
			this.uxAccelXValue.BorderColor = System.Drawing.Color.Black;
			this.uxAccelXValue.BorderSpace = 3;
			this.uxAccelXValue.CharSpacing = 2;
			this.uxAccelXValue.DotMatrix = LCDLabel.DotMatrix.mat5x7;
			this.uxAccelXValue.LineSpacing = 2;
			this.uxAccelXValue.Location = new System.Drawing.Point(8, 72);
			this.uxAccelXValue.Name = "uxAccelXValue";
			this.uxAccelXValue.NumberOfCharacters = 8;
			this.uxAccelXValue.PixelHeight = 1;
			this.uxAccelXValue.PixelOff = System.Drawing.Color.Transparent;
			this.uxAccelXValue.PixelOn = System.Drawing.Color.Gold;
			this.uxAccelXValue.PixelShape = LCDLabel.PixelShape.Square;
			this.uxAccelXValue.PixelSize = LCDLabel.PixelSize.pix1x1;
			this.uxAccelXValue.PixelSpacing = 1;
			this.uxAccelXValue.PixelWidth = 1;
			this.uxAccelXValue.Size = new System.Drawing.Size(94, 21);
			this.uxAccelXValue.TabIndex = 238;
			this.uxAccelXValue.Text = "X=1.45";
			this.uxAccelXValue.TextLines = 1;
			// 
			// MainScreenGraph
			// 
			this.MainScreenGraph.BackColor = System.Drawing.Color.LightSlateGray;
			this.MainScreenGraph.BackgroundImageAlignment = NationalInstruments.UI.ImageAlignment.Center;
			this.MainScreenGraph.Border = NationalInstruments.UI.Border.None;
			this.MainScreenGraph.CaptionBackColor = System.Drawing.Color.LightSlateGray;
			this.MainScreenGraph.CaptionFont = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.MainScreenGraph.ForeColor = System.Drawing.Color.Honeydew;
			this.MainScreenGraph.ImmediateUpdates = true;
			this.MainScreenGraph.Location = new System.Drawing.Point(117, 2);
			this.MainScreenGraph.Name = "MainScreenGraph";
			this.MainScreenGraph.PlotAreaBorder = NationalInstruments.UI.Border.Raised;
			this.MainScreenGraph.PlotAreaColor = System.Drawing.Color.SandyBrown;
			this.MainScreenGraph.Plots.AddRange(new NationalInstruments.UI.WaveformPlot[] {
            this.uxAccelX,
            this.uxAccelY,
            this.uxAccelZ});
			this.MainScreenGraph.Size = new System.Drawing.Size(488, 389);
			this.MainScreenGraph.TabIndex = 221;
			this.MainScreenGraph.XAxes.AddRange(new NationalInstruments.UI.XAxis[] {
            this.uxAxisSamples});
			this.MainScreenGraph.YAxes.AddRange(new NationalInstruments.UI.YAxis[] {
            this.uxAxisAccel,
            this.uxAxisCounts});
			this.MainScreenGraph.PlotDataChanged += new NationalInstruments.UI.XYPlotDataChangedEventHandler(this.MainScreenGraph_PlotDataChanged);
			// 
			// uxAxisCounts
			// 
			this.uxAxisCounts.Caption = "Counts";
			this.uxAxisCounts.CaptionBackColor = System.Drawing.Color.Transparent;
			this.uxAxisCounts.CaptionFont = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.uxAxisCounts.CaptionForeColor = System.Drawing.Color.Honeydew;
			this.uxAxisCounts.CaptionPosition = NationalInstruments.UI.YAxisPosition.Right;
			this.uxAxisCounts.MajorDivisions.Interval = 256D;
			this.uxAxisCounts.MajorDivisions.LabelFont = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.uxAxisCounts.MajorDivisions.TickVisible = false;
			this.uxAxisCounts.Mode = NationalInstruments.UI.AxisMode.Fixed;
			this.uxAxisCounts.Position = NationalInstruments.UI.YAxisPosition.Right;
			this.uxAxisCounts.Range = NationalInstruments.UI.Range.Parse("-2048; 2047");
			// 
			// uxPanelSettings
			// 
			this.uxPanelSettings.Controls.Add(this.pSOS);
			this.uxPanelSettings.Controls.Add(this.gbASS);
			this.uxPanelSettings.Controls.Add(this.gbIF);
			this.uxPanelSettings.Controls.Add(this.gbwfs);
			this.uxPanelSettings.Location = new System.Drawing.Point(613, 8);
			this.uxPanelSettings.Name = "uxPanelSettings";
			this.uxPanelSettings.Size = new System.Drawing.Size(504, 507);
			this.uxPanelSettings.TabIndex = 242;
			// 
			// pSOS
			// 
			this.pSOS.Controls.Add(this.rdoSOSNormalMode);
			this.pSOS.Controls.Add(this.rdoSOSHiResMode);
			this.pSOS.Controls.Add(this.rdoSOSLNLPMode);
			this.pSOS.Controls.Add(this.rdoSOSLPMode);
			this.pSOS.Location = new System.Drawing.Point(6, 6);
			this.pSOS.Name = "pSOS";
			this.pSOS.Size = new System.Drawing.Size(487, 65);
			this.pSOS.TabIndex = 225;
			this.pSOS.TabStop = false;
			this.pSOS.Text = "Sleep Mode Oversampling Options";
			// 
			// rdoSOSNormalMode
			// 
			this.rdoSOSNormalMode.AutoSize = true;
			this.rdoSOSNormalMode.Checked = true;
			this.rdoSOSNormalMode.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.rdoSOSNormalMode.Location = new System.Drawing.Point(36, 29);
			this.rdoSOSNormalMode.Name = "rdoSOSNormalMode";
			this.rdoSOSNormalMode.Size = new System.Drawing.Size(72, 19);
			this.rdoSOSNormalMode.TabIndex = 220;
			this.rdoSOSNormalMode.TabStop = true;
			this.rdoSOSNormalMode.Text = "Normal";
			this.rdoSOSNormalMode.UseVisualStyleBackColor = true;
			this.rdoSOSNormalMode.CheckedChanged += new System.EventHandler(this.rdoSOSNormalMode_CheckedChanged);
			// 
			// rdoSOSHiResMode
			// 
			this.rdoSOSHiResMode.AutoSize = true;
			this.rdoSOSHiResMode.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.rdoSOSHiResMode.Location = new System.Drawing.Point(242, 29);
			this.rdoSOSHiResMode.Name = "rdoSOSHiResMode";
			this.rdoSOSHiResMode.Size = new System.Drawing.Size(68, 19);
			this.rdoSOSHiResMode.TabIndex = 224;
			this.rdoSOSHiResMode.Text = "Hi Res";
			this.rdoSOSHiResMode.UseVisualStyleBackColor = true;
			this.rdoSOSHiResMode.CheckedChanged += new System.EventHandler(this.rdoSOSHiResMode_CheckedChanged);
			// 
			// rdoSOSLNLPMode
			// 
			this.rdoSOSLNLPMode.AutoSize = true;
			this.rdoSOSLNLPMode.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.rdoSOSLNLPMode.Location = new System.Drawing.Point(332, 29);
			this.rdoSOSLNLPMode.Name = "rdoSOSLNLPMode";
			this.rdoSOSLNLPMode.Size = new System.Drawing.Size(140, 19);
			this.rdoSOSLNLPMode.TabIndex = 221;
			this.rdoSOSLNLPMode.Text = "Low Noise+Power";
			this.rdoSOSLNLPMode.UseVisualStyleBackColor = true;
			this.rdoSOSLNLPMode.CheckedChanged += new System.EventHandler(this.rdoSOSLNLPMode_CheckedChanged);
			// 
			// rdoSOSLPMode
			// 
			this.rdoSOSLPMode.AutoSize = true;
			this.rdoSOSLPMode.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.rdoSOSLPMode.Location = new System.Drawing.Point(125, 29);
			this.rdoSOSLPMode.Name = "rdoSOSLPMode";
			this.rdoSOSLPMode.Size = new System.Drawing.Size(95, 19);
			this.rdoSOSLPMode.TabIndex = 223;
			this.rdoSOSLPMode.Text = "Low Power";
			this.rdoSOSLPMode.UseVisualStyleBackColor = true;
			this.rdoSOSLPMode.CheckedChanged += new System.EventHandler(this.rdoSOSLPMode_CheckedChanged);
			// 
			// gbASS
			// 
			this.gbASS.BackColor = System.Drawing.Color.LightSlateGray;
			this.gbASS.Controls.Add(this.pnlAutoSleep);
			this.gbASS.Controls.Add(this.chkEnableASleep);
			this.gbASS.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.gbASS.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.gbASS.ForeColor = System.Drawing.Color.White;
			this.gbASS.Location = new System.Drawing.Point(6, 77);
			this.gbASS.Name = "gbASS";
			this.gbASS.Size = new System.Drawing.Size(345, 135);
			this.gbASS.TabIndex = 189;
			this.gbASS.TabStop = false;
			this.gbASS.Text = "Auto Sleep Settings";
			// 
			// pnlAutoSleep
			// 
			this.pnlAutoSleep.Controls.Add(this.btnSleepTimerReset);
			this.pnlAutoSleep.Controls.Add(this.btnSetSleepTimer);
			this.pnlAutoSleep.Controls.Add(this.label199);
			this.pnlAutoSleep.Controls.Add(this.lblSleepms);
			this.pnlAutoSleep.Controls.Add(this.lblSleepTimerValue);
			this.pnlAutoSleep.Controls.Add(this.lblSleepTimer);
			this.pnlAutoSleep.Controls.Add(this.tbSleepCounter);
			this.pnlAutoSleep.Controls.Add(this.ddlSleepSR);
			this.pnlAutoSleep.Enabled = false;
			this.pnlAutoSleep.Location = new System.Drawing.Point(5, 38);
			this.pnlAutoSleep.Name = "pnlAutoSleep";
			this.pnlAutoSleep.Size = new System.Drawing.Size(335, 92);
			this.pnlAutoSleep.TabIndex = 146;
			// 
			// btnSleepTimerReset
			// 
			this.btnSleepTimerReset.BackColor = System.Drawing.Color.LightSlateGray;
			this.btnSleepTimerReset.Enabled = false;
			this.btnSleepTimerReset.Location = new System.Drawing.Point(220, 21);
			this.btnSleepTimerReset.Name = "btnSleepTimerReset";
			this.btnSleepTimerReset.Size = new System.Drawing.Size(61, 25);
			this.btnSleepTimerReset.TabIndex = 146;
			this.btnSleepTimerReset.Text = "Reset";
			this.toolTip1.SetToolTip(this.btnSleepTimerReset, "This buton writes in the desired sleep time set for the device to wait to time ou" +
        "t before switching to sleep mode.");
			this.btnSleepTimerReset.UseVisualStyleBackColor = false;
			this.btnSleepTimerReset.Click += new System.EventHandler(this.btnSleepTimerReset_Click);
			// 
			// btnSetSleepTimer
			// 
			this.btnSetSleepTimer.BackColor = System.Drawing.Color.LightSlateGray;
			this.btnSetSleepTimer.ForeColor = System.Drawing.Color.White;
			this.btnSetSleepTimer.Location = new System.Drawing.Point(287, 21);
			this.btnSetSleepTimer.Name = "btnSetSleepTimer";
			this.btnSetSleepTimer.Size = new System.Drawing.Size(39, 25);
			this.btnSetSleepTimer.TabIndex = 145;
			this.btnSetSleepTimer.Text = "Set";
			this.toolTip1.SetToolTip(this.btnSetSleepTimer, "Writes in the sleep time");
			this.btnSetSleepTimer.UseVisualStyleBackColor = false;
			this.btnSetSleepTimer.Click += new System.EventHandler(this.btnSetSleepTimer_Click);
			// 
			// label199
			// 
			this.label199.AutoSize = true;
			this.label199.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label199.Location = new System.Drawing.Point(3, 7);
			this.label199.Name = "label199";
			this.label199.Size = new System.Drawing.Size(115, 13);
			this.label199.TabIndex = 143;
			this.label199.Text = "Sleep Sample Rate";
			// 
			// lblSleepms
			// 
			this.lblSleepms.AutoSize = true;
			this.lblSleepms.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblSleepms.Location = new System.Drawing.Point(137, 30);
			this.lblSleepms.Name = "lblSleepms";
			this.lblSleepms.Size = new System.Drawing.Size(22, 13);
			this.lblSleepms.TabIndex = 142;
			this.lblSleepms.Text = "ms";
			// 
			// lblSleepTimerValue
			// 
			this.lblSleepTimerValue.AutoSize = true;
			this.lblSleepTimerValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblSleepTimerValue.Location = new System.Drawing.Point(82, 30);
			this.lblSleepTimerValue.Name = "lblSleepTimerValue";
			this.lblSleepTimerValue.Size = new System.Drawing.Size(14, 13);
			this.lblSleepTimerValue.TabIndex = 141;
			this.lblSleepTimerValue.Text = "0";
			// 
			// lblSleepTimer
			// 
			this.lblSleepTimer.AutoSize = true;
			this.lblSleepTimer.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblSleepTimer.Location = new System.Drawing.Point(2, 30);
			this.lblSleepTimer.Name = "lblSleepTimer";
			this.lblSleepTimer.Size = new System.Drawing.Size(74, 13);
			this.lblSleepTimer.TabIndex = 140;
			this.lblSleepTimer.Text = "Sleep Timer";
			// 
			// tbSleepCounter
			// 
			this.tbSleepCounter.BackColor = System.Drawing.Color.LightSlateGray;
			this.tbSleepCounter.Location = new System.Drawing.Point(2, 49);
			this.tbSleepCounter.Maximum = 255;
			this.tbSleepCounter.Name = "tbSleepCounter";
			this.tbSleepCounter.Size = new System.Drawing.Size(332, 45);
			this.tbSleepCounter.TabIndex = 137;
			this.tbSleepCounter.TickFrequency = 15;
			this.toolTip1.SetToolTip(this.tbSleepCounter, "The sleep counter is the time out period to change the sample rate");
			this.tbSleepCounter.Scroll += new System.EventHandler(this.tbSleepCounter_Scroll);
			// 
			// ddlSleepSR
			// 
			this.ddlSleepSR.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ddlSleepSR.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ddlSleepSR.FormattingEnabled = true;
			this.ddlSleepSR.Items.AddRange(new object[] {
            "50 Hz",
            "12.5 Hz",
            "6.25 Hz",
            "1.56 Hz"});
			this.ddlSleepSR.Location = new System.Drawing.Point(123, 2);
			this.ddlSleepSR.Name = "ddlSleepSR";
			this.ddlSleepSR.Size = new System.Drawing.Size(72, 21);
			this.ddlSleepSR.TabIndex = 101;
			this.toolTip1.SetToolTip(this.ddlSleepSR, "This sets the sample rate when the devie sees no interrupt or activity for the se" +
        "t period of time (sleep timer).");
			this.ddlSleepSR.SelectedIndexChanged += new System.EventHandler(this.ddlSleepSR_SelectedIndexChanged);
			// 
			// chkEnableASleep
			// 
			this.chkEnableASleep.AutoSize = true;
			this.chkEnableASleep.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chkEnableASleep.Location = new System.Drawing.Point(8, 18);
			this.chkEnableASleep.Name = "chkEnableASleep";
			this.chkEnableASleep.Size = new System.Drawing.Size(131, 17);
			this.chkEnableASleep.TabIndex = 144;
			this.chkEnableASleep.Text = "Enable Auto Sleep";
			this.toolTip1.SetToolTip(this.chkEnableASleep, "Enable Interrupt Based Sleep Functions");
			this.chkEnableASleep.UseVisualStyleBackColor = true;
			this.chkEnableASleep.CheckedChanged += new System.EventHandler(this.chkEnableASleep_CheckedChanged);
			// 
			// gbIF
			// 
			this.gbIF.BackColor = System.Drawing.Color.LightSlateGray;
			this.gbIF.Controls.Add(this.p9);
			this.gbIF.Controls.Add(this.p8);
			this.gbIF.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.gbIF.ForeColor = System.Drawing.Color.White;
			this.gbIF.Location = new System.Drawing.Point(5, 213);
			this.gbIF.Name = "gbIF";
			this.gbIF.Size = new System.Drawing.Size(488, 286);
			this.gbIF.TabIndex = 198;
			this.gbIF.TabStop = false;
			this.gbIF.Text = "Interrupts";
			// 
			// p9
			// 
			this.p9.BackColor = System.Drawing.Color.LightSlateGray;
			this.p9.Controls.Add(this.ledTrans1);
			this.p9.Controls.Add(this.ledOrient);
			this.p9.Controls.Add(this.ledASleep);
			this.p9.Controls.Add(this.ledMFF1);
			this.p9.Controls.Add(this.ledPulse);
			this.p9.Controls.Add(this.ledTrans);
			this.p9.Controls.Add(this.ledDataReady);
			this.p9.Controls.Add(this.ledFIFO);
			this.p9.Location = new System.Drawing.Point(40, 36);
			this.p9.Name = "p9";
			this.p9.Size = new System.Drawing.Size(45, 245);
			this.p9.TabIndex = 198;
			// 
			// ledTrans1
			// 
			this.ledTrans1.LedStyle = NationalInstruments.UI.LedStyle.Round3D;
			this.ledTrans1.Location = new System.Drawing.Point(12, 209);
			this.ledTrans1.Name = "ledTrans1";
			this.ledTrans1.OffColor = System.Drawing.Color.Red;
			this.ledTrans1.Size = new System.Drawing.Size(30, 30);
			this.ledTrans1.TabIndex = 157;
			this.ledTrans1.Visible = false;
			// 
			// ledOrient
			// 
			this.ledOrient.LedStyle = NationalInstruments.UI.LedStyle.Round3D;
			this.ledOrient.Location = new System.Drawing.Point(12, 152);
			this.ledOrient.Name = "ledOrient";
			this.ledOrient.OffColor = System.Drawing.Color.Red;
			this.ledOrient.Size = new System.Drawing.Size(30, 30);
			this.ledOrient.TabIndex = 156;
			// 
			// ledASleep
			// 
			this.ledASleep.LedStyle = NationalInstruments.UI.LedStyle.Round3D;
			this.ledASleep.Location = new System.Drawing.Point(12, 182);
			this.ledASleep.Name = "ledASleep";
			this.ledASleep.OffColor = System.Drawing.Color.Red;
			this.ledASleep.Size = new System.Drawing.Size(30, 30);
			this.ledASleep.TabIndex = 156;
			// 
			// ledMFF1
			// 
			this.ledMFF1.LedStyle = NationalInstruments.UI.LedStyle.Round3D;
			this.ledMFF1.Location = new System.Drawing.Point(12, 122);
			this.ledMFF1.Name = "ledMFF1";
			this.ledMFF1.OffColor = System.Drawing.Color.Red;
			this.ledMFF1.Size = new System.Drawing.Size(30, 30);
			this.ledMFF1.TabIndex = 156;
			// 
			// ledPulse
			// 
			this.ledPulse.LedStyle = NationalInstruments.UI.LedStyle.Round3D;
			this.ledPulse.Location = new System.Drawing.Point(12, 92);
			this.ledPulse.Name = "ledPulse";
			this.ledPulse.OffColor = System.Drawing.Color.Red;
			this.ledPulse.Size = new System.Drawing.Size(30, 30);
			this.ledPulse.TabIndex = 156;
			// 
			// ledTrans
			// 
			this.ledTrans.LedStyle = NationalInstruments.UI.LedStyle.Round3D;
			this.ledTrans.Location = new System.Drawing.Point(12, 62);
			this.ledTrans.Name = "ledTrans";
			this.ledTrans.OffColor = System.Drawing.Color.Red;
			this.ledTrans.Size = new System.Drawing.Size(30, 30);
			this.ledTrans.TabIndex = 156;
			// 
			// ledDataReady
			// 
			this.ledDataReady.LedStyle = NationalInstruments.UI.LedStyle.Round3D;
			this.ledDataReady.Location = new System.Drawing.Point(12, 32);
			this.ledDataReady.Name = "ledDataReady";
			this.ledDataReady.OffColor = System.Drawing.Color.Red;
			this.ledDataReady.Size = new System.Drawing.Size(30, 30);
			this.ledDataReady.TabIndex = 156;
			// 
			// ledFIFO
			// 
			this.ledFIFO.LedStyle = NationalInstruments.UI.LedStyle.Round3D;
			this.ledFIFO.Location = new System.Drawing.Point(12, 3);
			this.ledFIFO.Name = "ledFIFO";
			this.ledFIFO.OffColor = System.Drawing.Color.Red;
			this.ledFIFO.Size = new System.Drawing.Size(30, 30);
			this.ledFIFO.TabIndex = 156;
			// 
			// p8
			// 
			this.p8.Controls.Add(this.panel18);
			this.p8.Controls.Add(this.panel12);
			this.p8.Controls.Add(this.panel11);
			this.p8.Controls.Add(this.panel10);
			this.p8.Controls.Add(this.panel9);
			this.p8.Controls.Add(this.panel4);
			this.p8.Controls.Add(this.panel8);
			this.p8.Controls.Add(this.panel6);
			this.p8.Location = new System.Drawing.Point(83, 37);
			this.p8.Name = "p8";
			this.p8.Size = new System.Drawing.Size(382, 244);
			this.p8.TabIndex = 199;
			// 
			// panel18
			// 
			this.panel18.Controls.Add(this.rdoTrans1INT_I1);
			this.panel18.Controls.Add(this.rdoTrans1INT_I2);
			this.panel18.Controls.Add(this.chkEnIntTrans1);
			this.panel18.Controls.Add(this.label36);
			this.panel18.Location = new System.Drawing.Point(1, 206);
			this.panel18.Name = "panel18";
			this.panel18.Size = new System.Drawing.Size(371, 32);
			this.panel18.TabIndex = 198;
			this.panel18.Visible = false;
			// 
			// rdoTrans1INT_I1
			// 
			this.rdoTrans1INT_I1.AutoSize = true;
			this.rdoTrans1INT_I1.BackColor = System.Drawing.Color.LightSlateGray;
			this.rdoTrans1INT_I1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.rdoTrans1INT_I1.Location = new System.Drawing.Point(209, 7);
			this.rdoTrans1INT_I1.Name = "rdoTrans1INT_I1";
			this.rdoTrans1INT_I1.Size = new System.Drawing.Size(53, 17);
			this.rdoTrans1INT_I1.TabIndex = 154;
			this.rdoTrans1INT_I1.Text = "INT1";
			this.rdoTrans1INT_I1.UseVisualStyleBackColor = false;
			this.rdoTrans1INT_I1.CheckedChanged += new System.EventHandler(this.rdoTrans1INT_I1_CheckedChanged);
			// 
			// rdoTrans1INT_I2
			// 
			this.rdoTrans1INT_I2.AutoSize = true;
			this.rdoTrans1INT_I2.BackColor = System.Drawing.Color.LightSlateGray;
			this.rdoTrans1INT_I2.Checked = true;
			this.rdoTrans1INT_I2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.rdoTrans1INT_I2.Location = new System.Drawing.Point(270, 7);
			this.rdoTrans1INT_I2.Name = "rdoTrans1INT_I2";
			this.rdoTrans1INT_I2.Size = new System.Drawing.Size(53, 17);
			this.rdoTrans1INT_I2.TabIndex = 155;
			this.rdoTrans1INT_I2.TabStop = true;
			this.rdoTrans1INT_I2.Text = "INT2";
			this.rdoTrans1INT_I2.UseVisualStyleBackColor = false;
			this.rdoTrans1INT_I2.CheckedChanged += new System.EventHandler(this.rdoTrans1INT_I2_CheckedChanged);
			// 
			// chkEnIntTrans1
			// 
			this.chkEnIntTrans1.AutoSize = true;
			this.chkEnIntTrans1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chkEnIntTrans1.Location = new System.Drawing.Point(114, 6);
			this.chkEnIntTrans1.Name = "chkEnIntTrans1";
			this.chkEnIntTrans1.Size = new System.Drawing.Size(89, 17);
			this.chkEnIntTrans1.TabIndex = 136;
			this.chkEnIntTrans1.Text = "INT enable";
			this.chkEnIntTrans1.UseVisualStyleBackColor = true;
			this.chkEnIntTrans1.CheckedChanged += new System.EventHandler(this.chkEnIntTrans1_CheckedChanged);
			// 
			// label36
			// 
			this.label36.AutoSize = true;
			this.label36.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label36.Location = new System.Drawing.Point(4, 5);
			this.label36.Name = "label36";
			this.label36.Size = new System.Drawing.Size(99, 20);
			this.label36.TabIndex = 176;
			this.label36.Text = "Transient 1";
			// 
			// panel12
			// 
			this.panel12.Controls.Add(this.rdoFIFOINT_I1);
			this.panel12.Controls.Add(this.rdoFIFOINT_I2);
			this.panel12.Controls.Add(this.chkEnIntFIFO);
			this.panel12.Controls.Add(this.lblntFIFO);
			this.panel12.Location = new System.Drawing.Point(1, 2);
			this.panel12.Name = "panel12";
			this.panel12.Size = new System.Drawing.Size(371, 28);
			this.panel12.TabIndex = 197;
			// 
			// rdoFIFOINT_I1
			// 
			this.rdoFIFOINT_I1.AutoSize = true;
			this.rdoFIFOINT_I1.BackColor = System.Drawing.Color.LightSlateGray;
			this.rdoFIFOINT_I1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.rdoFIFOINT_I1.Location = new System.Drawing.Point(209, 4);
			this.rdoFIFOINT_I1.Name = "rdoFIFOINT_I1";
			this.rdoFIFOINT_I1.Size = new System.Drawing.Size(53, 17);
			this.rdoFIFOINT_I1.TabIndex = 154;
			this.rdoFIFOINT_I1.Text = "INT1";
			this.rdoFIFOINT_I1.UseVisualStyleBackColor = false;
			this.rdoFIFOINT_I1.CheckedChanged += new System.EventHandler(this.rdoFIFOINT_I1_CheckedChanged);
			// 
			// rdoFIFOINT_I2
			// 
			this.rdoFIFOINT_I2.AutoSize = true;
			this.rdoFIFOINT_I2.BackColor = System.Drawing.Color.LightSlateGray;
			this.rdoFIFOINT_I2.Checked = true;
			this.rdoFIFOINT_I2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.rdoFIFOINT_I2.Location = new System.Drawing.Point(270, 4);
			this.rdoFIFOINT_I2.Name = "rdoFIFOINT_I2";
			this.rdoFIFOINT_I2.Size = new System.Drawing.Size(53, 17);
			this.rdoFIFOINT_I2.TabIndex = 155;
			this.rdoFIFOINT_I2.TabStop = true;
			this.rdoFIFOINT_I2.Text = "INT2";
			this.rdoFIFOINT_I2.UseVisualStyleBackColor = false;
			this.rdoFIFOINT_I2.CheckedChanged += new System.EventHandler(this.rdoFIFOINT_I2_CheckedChanged);
			// 
			// chkEnIntFIFO
			// 
			this.chkEnIntFIFO.AutoSize = true;
			this.chkEnIntFIFO.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chkEnIntFIFO.Location = new System.Drawing.Point(115, 6);
			this.chkEnIntFIFO.Name = "chkEnIntFIFO";
			this.chkEnIntFIFO.Size = new System.Drawing.Size(89, 17);
			this.chkEnIntFIFO.TabIndex = 136;
			this.chkEnIntFIFO.Text = "INT enable";
			this.chkEnIntFIFO.UseVisualStyleBackColor = true;
			this.chkEnIntFIFO.CheckedChanged += new System.EventHandler(this.chkEnIntFIFO_CheckedChanged);
			// 
			// lblntFIFO
			// 
			this.lblntFIFO.AutoSize = true;
			this.lblntFIFO.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblntFIFO.Location = new System.Drawing.Point(2, 4);
			this.lblntFIFO.Name = "lblntFIFO";
			this.lblntFIFO.Size = new System.Drawing.Size(50, 20);
			this.lblntFIFO.TabIndex = 176;
			this.lblntFIFO.Text = "FIFO";
			// 
			// panel11
			// 
			this.panel11.Controls.Add(this.rdoDRINT_I1);
			this.panel11.Controls.Add(this.rdoDRINT_I2);
			this.panel11.Controls.Add(this.chkEnIntDR);
			this.panel11.Controls.Add(this.label6);
			this.panel11.Location = new System.Drawing.Point(1, 30);
			this.panel11.Name = "panel11";
			this.panel11.Size = new System.Drawing.Size(371, 30);
			this.panel11.TabIndex = 196;
			// 
			// rdoDRINT_I1
			// 
			this.rdoDRINT_I1.AutoSize = true;
			this.rdoDRINT_I1.BackColor = System.Drawing.Color.LightSlateGray;
			this.rdoDRINT_I1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.rdoDRINT_I1.Location = new System.Drawing.Point(209, 7);
			this.rdoDRINT_I1.Name = "rdoDRINT_I1";
			this.rdoDRINT_I1.Size = new System.Drawing.Size(53, 17);
			this.rdoDRINT_I1.TabIndex = 154;
			this.rdoDRINT_I1.Text = "INT1";
			this.rdoDRINT_I1.UseVisualStyleBackColor = false;
			this.rdoDRINT_I1.CheckedChanged += new System.EventHandler(this.rdoDRINT_I1_CheckedChanged);
			// 
			// rdoDRINT_I2
			// 
			this.rdoDRINT_I2.AutoSize = true;
			this.rdoDRINT_I2.BackColor = System.Drawing.Color.LightSlateGray;
			this.rdoDRINT_I2.Checked = true;
			this.rdoDRINT_I2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.rdoDRINT_I2.Location = new System.Drawing.Point(270, 7);
			this.rdoDRINT_I2.Name = "rdoDRINT_I2";
			this.rdoDRINT_I2.Size = new System.Drawing.Size(53, 17);
			this.rdoDRINT_I2.TabIndex = 155;
			this.rdoDRINT_I2.TabStop = true;
			this.rdoDRINT_I2.Text = "INT2";
			this.rdoDRINT_I2.UseVisualStyleBackColor = false;
			this.rdoDRINT_I2.CheckedChanged += new System.EventHandler(this.rdoDRINT_I2_CheckedChanged);
			// 
			// chkEnIntDR
			// 
			this.chkEnIntDR.AutoSize = true;
			this.chkEnIntDR.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chkEnIntDR.Location = new System.Drawing.Point(115, 7);
			this.chkEnIntDR.Name = "chkEnIntDR";
			this.chkEnIntDR.Size = new System.Drawing.Size(89, 17);
			this.chkEnIntDR.TabIndex = 136;
			this.chkEnIntDR.Text = "INT enable";
			this.chkEnIntDR.UseVisualStyleBackColor = true;
			this.chkEnIntDR.CheckedChanged += new System.EventHandler(this.chkEnIntDR_CheckedChanged);
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label6.Location = new System.Drawing.Point(2, 4);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(104, 20);
			this.label6.TabIndex = 176;
			this.label6.Text = "Data Ready";
			// 
			// panel10
			// 
			this.panel10.Controls.Add(this.rdoTransINT_I1);
			this.panel10.Controls.Add(this.rdoTransINT_I2);
			this.panel10.Controls.Add(this.chkEnIntTrans);
			this.panel10.Controls.Add(this.label3);
			this.panel10.Location = new System.Drawing.Point(1, 60);
			this.panel10.Name = "panel10";
			this.panel10.Size = new System.Drawing.Size(371, 32);
			this.panel10.TabIndex = 196;
			// 
			// rdoTransINT_I1
			// 
			this.rdoTransINT_I1.AutoSize = true;
			this.rdoTransINT_I1.BackColor = System.Drawing.Color.LightSlateGray;
			this.rdoTransINT_I1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.rdoTransINT_I1.Location = new System.Drawing.Point(209, 7);
			this.rdoTransINT_I1.Name = "rdoTransINT_I1";
			this.rdoTransINT_I1.Size = new System.Drawing.Size(53, 17);
			this.rdoTransINT_I1.TabIndex = 154;
			this.rdoTransINT_I1.Text = "INT1";
			this.rdoTransINT_I1.UseVisualStyleBackColor = false;
			this.rdoTransINT_I1.CheckedChanged += new System.EventHandler(this.rdoTransINT_I1_CheckedChanged);
			// 
			// rdoTransINT_I2
			// 
			this.rdoTransINT_I2.AutoSize = true;
			this.rdoTransINT_I2.BackColor = System.Drawing.Color.LightSlateGray;
			this.rdoTransINT_I2.Checked = true;
			this.rdoTransINT_I2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.rdoTransINT_I2.Location = new System.Drawing.Point(270, 7);
			this.rdoTransINT_I2.Name = "rdoTransINT_I2";
			this.rdoTransINT_I2.Size = new System.Drawing.Size(53, 17);
			this.rdoTransINT_I2.TabIndex = 155;
			this.rdoTransINT_I2.TabStop = true;
			this.rdoTransINT_I2.Text = "INT2";
			this.rdoTransINT_I2.UseVisualStyleBackColor = false;
			this.rdoTransINT_I2.CheckedChanged += new System.EventHandler(this.rdoTransINT_I2_CheckedChanged);
			// 
			// chkEnIntTrans
			// 
			this.chkEnIntTrans.AutoSize = true;
			this.chkEnIntTrans.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chkEnIntTrans.Location = new System.Drawing.Point(114, 6);
			this.chkEnIntTrans.Name = "chkEnIntTrans";
			this.chkEnIntTrans.Size = new System.Drawing.Size(89, 17);
			this.chkEnIntTrans.TabIndex = 136;
			this.chkEnIntTrans.Text = "INT enable";
			this.chkEnIntTrans.UseVisualStyleBackColor = true;
			this.chkEnIntTrans.CheckedChanged += new System.EventHandler(this.chkEnIntTrans_CheckedChanged);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label3.Location = new System.Drawing.Point(4, 5);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(89, 20);
			this.label3.TabIndex = 176;
			this.label3.Text = "Transient ";
			// 
			// panel9
			// 
			this.panel9.Controls.Add(this.rdoPulseINT_I1);
			this.panel9.Controls.Add(this.rdoPulseINT_I2);
			this.panel9.Controls.Add(this.chkEnIntPulse);
			this.panel9.Controls.Add(this.label2);
			this.panel9.Location = new System.Drawing.Point(1, 93);
			this.panel9.Name = "panel9";
			this.panel9.Size = new System.Drawing.Size(371, 29);
			this.panel9.TabIndex = 196;
			// 
			// rdoPulseINT_I1
			// 
			this.rdoPulseINT_I1.AutoSize = true;
			this.rdoPulseINT_I1.BackColor = System.Drawing.Color.LightSlateGray;
			this.rdoPulseINT_I1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.rdoPulseINT_I1.Location = new System.Drawing.Point(209, 5);
			this.rdoPulseINT_I1.Name = "rdoPulseINT_I1";
			this.rdoPulseINT_I1.Size = new System.Drawing.Size(53, 17);
			this.rdoPulseINT_I1.TabIndex = 154;
			this.rdoPulseINT_I1.Text = "INT1";
			this.rdoPulseINT_I1.UseVisualStyleBackColor = false;
			this.rdoPulseINT_I1.CheckedChanged += new System.EventHandler(this.rdoPulseINT_I1_CheckedChanged);
			// 
			// rdoPulseINT_I2
			// 
			this.rdoPulseINT_I2.AutoSize = true;
			this.rdoPulseINT_I2.BackColor = System.Drawing.Color.LightSlateGray;
			this.rdoPulseINT_I2.Checked = true;
			this.rdoPulseINT_I2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.rdoPulseINT_I2.Location = new System.Drawing.Point(270, 5);
			this.rdoPulseINT_I2.Name = "rdoPulseINT_I2";
			this.rdoPulseINT_I2.Size = new System.Drawing.Size(53, 17);
			this.rdoPulseINT_I2.TabIndex = 155;
			this.rdoPulseINT_I2.TabStop = true;
			this.rdoPulseINT_I2.Text = "INT2";
			this.rdoPulseINT_I2.UseVisualStyleBackColor = false;
			this.rdoPulseINT_I2.CheckedChanged += new System.EventHandler(this.rdoPulseINT_I2_CheckedChanged);
			// 
			// chkEnIntPulse
			// 
			this.chkEnIntPulse.AutoSize = true;
			this.chkEnIntPulse.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chkEnIntPulse.Location = new System.Drawing.Point(114, 5);
			this.chkEnIntPulse.Name = "chkEnIntPulse";
			this.chkEnIntPulse.Size = new System.Drawing.Size(89, 17);
			this.chkEnIntPulse.TabIndex = 136;
			this.chkEnIntPulse.Text = "INT enable";
			this.chkEnIntPulse.UseVisualStyleBackColor = true;
			this.chkEnIntPulse.CheckedChanged += new System.EventHandler(this.chkEnIntPulse_CheckedChanged);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(3, 4);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(53, 20);
			this.label2.TabIndex = 176;
			this.label2.Text = "Pulse";
			// 
			// panel4
			// 
			this.panel4.Controls.Add(this.rdoASleepINT_I1);
			this.panel4.Controls.Add(this.rdoASleepINT_I2);
			this.panel4.Controls.Add(this.chkEnIntASleep);
			this.panel4.Controls.Add(this.label1);
			this.panel4.Location = new System.Drawing.Point(1, 179);
			this.panel4.Name = "panel4";
			this.panel4.Size = new System.Drawing.Size(371, 31);
			this.panel4.TabIndex = 194;
			// 
			// rdoASleepINT_I1
			// 
			this.rdoASleepINT_I1.AutoSize = true;
			this.rdoASleepINT_I1.BackColor = System.Drawing.Color.LightSlateGray;
			this.rdoASleepINT_I1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.rdoASleepINT_I1.Location = new System.Drawing.Point(209, 7);
			this.rdoASleepINT_I1.Name = "rdoASleepINT_I1";
			this.rdoASleepINT_I1.Size = new System.Drawing.Size(53, 17);
			this.rdoASleepINT_I1.TabIndex = 154;
			this.rdoASleepINT_I1.Text = "INT1";
			this.rdoASleepINT_I1.UseVisualStyleBackColor = false;
			this.rdoASleepINT_I1.CheckedChanged += new System.EventHandler(this.rdoASleepINT_I1_CheckedChanged);
			// 
			// rdoASleepINT_I2
			// 
			this.rdoASleepINT_I2.AutoSize = true;
			this.rdoASleepINT_I2.BackColor = System.Drawing.Color.LightSlateGray;
			this.rdoASleepINT_I2.Checked = true;
			this.rdoASleepINT_I2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.rdoASleepINT_I2.Location = new System.Drawing.Point(270, 7);
			this.rdoASleepINT_I2.Name = "rdoASleepINT_I2";
			this.rdoASleepINT_I2.Size = new System.Drawing.Size(53, 17);
			this.rdoASleepINT_I2.TabIndex = 155;
			this.rdoASleepINT_I2.TabStop = true;
			this.rdoASleepINT_I2.Text = "INT2";
			this.rdoASleepINT_I2.UseVisualStyleBackColor = false;
			this.rdoASleepINT_I2.CheckedChanged += new System.EventHandler(this.rdoASleepINT_I2_CheckedChanged);
			// 
			// chkEnIntASleep
			// 
			this.chkEnIntASleep.AutoSize = true;
			this.chkEnIntASleep.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chkEnIntASleep.Location = new System.Drawing.Point(114, 8);
			this.chkEnIntASleep.Name = "chkEnIntASleep";
			this.chkEnIntASleep.Size = new System.Drawing.Size(89, 17);
			this.chkEnIntASleep.TabIndex = 136;
			this.chkEnIntASleep.Text = "INT enable";
			this.chkEnIntASleep.UseVisualStyleBackColor = true;
			this.chkEnIntASleep.CheckedChanged += new System.EventHandler(this.chkEnIntASleep_CheckedChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(6, 5);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(98, 20);
			this.label1.TabIndex = 176;
			this.label1.Text = "Auto Sleep";
			// 
			// panel8
			// 
			this.panel8.Controls.Add(this.rdoMFF1INT_I1);
			this.panel8.Controls.Add(this.rdoMFF1INT_I2);
			this.panel8.Controls.Add(this.chkEnIntMFF1);
			this.panel8.Controls.Add(this.label4);
			this.panel8.Location = new System.Drawing.Point(1, 121);
			this.panel8.Name = "panel8";
			this.panel8.Size = new System.Drawing.Size(371, 30);
			this.panel8.TabIndex = 196;
			// 
			// rdoMFF1INT_I1
			// 
			this.rdoMFF1INT_I1.AutoSize = true;
			this.rdoMFF1INT_I1.BackColor = System.Drawing.Color.LightSlateGray;
			this.rdoMFF1INT_I1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.rdoMFF1INT_I1.Location = new System.Drawing.Point(209, 6);
			this.rdoMFF1INT_I1.Name = "rdoMFF1INT_I1";
			this.rdoMFF1INT_I1.Size = new System.Drawing.Size(53, 17);
			this.rdoMFF1INT_I1.TabIndex = 154;
			this.rdoMFF1INT_I1.Text = "INT1";
			this.rdoMFF1INT_I1.UseVisualStyleBackColor = false;
			this.rdoMFF1INT_I1.CheckedChanged += new System.EventHandler(this.rdoMFF1INT_I1_CheckedChanged);
			// 
			// rdoMFF1INT_I2
			// 
			this.rdoMFF1INT_I2.AutoSize = true;
			this.rdoMFF1INT_I2.BackColor = System.Drawing.Color.LightSlateGray;
			this.rdoMFF1INT_I2.Checked = true;
			this.rdoMFF1INT_I2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.rdoMFF1INT_I2.Location = new System.Drawing.Point(270, 6);
			this.rdoMFF1INT_I2.Name = "rdoMFF1INT_I2";
			this.rdoMFF1INT_I2.Size = new System.Drawing.Size(53, 17);
			this.rdoMFF1INT_I2.TabIndex = 155;
			this.rdoMFF1INT_I2.TabStop = true;
			this.rdoMFF1INT_I2.Text = "INT2";
			this.rdoMFF1INT_I2.UseVisualStyleBackColor = false;
			this.rdoMFF1INT_I2.CheckedChanged += new System.EventHandler(this.rdoMFF1INT_I2_CheckedChanged);
			// 
			// chkEnIntMFF1
			// 
			this.chkEnIntMFF1.AutoSize = true;
			this.chkEnIntMFF1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chkEnIntMFF1.Location = new System.Drawing.Point(114, 6);
			this.chkEnIntMFF1.Name = "chkEnIntMFF1";
			this.chkEnIntMFF1.Size = new System.Drawing.Size(89, 17);
			this.chkEnIntMFF1.TabIndex = 136;
			this.chkEnIntMFF1.Text = "INT enable";
			this.chkEnIntMFF1.UseVisualStyleBackColor = true;
			this.chkEnIntMFF1.CheckedChanged += new System.EventHandler(this.chkEnIntMFF1_CheckedChanged);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label4.Location = new System.Drawing.Point(3, 5);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(90, 20);
			this.label4.TabIndex = 176;
			this.label4.Text = "Motion FF";
			// 
			// panel6
			// 
			this.panel6.Controls.Add(this.rdoPLINT_I1);
			this.panel6.Controls.Add(this.rdoPLINT_I2);
			this.panel6.Controls.Add(this.chkEnIntPL);
			this.panel6.Controls.Add(this.label172);
			this.panel6.Location = new System.Drawing.Point(1, 150);
			this.panel6.Name = "panel6";
			this.panel6.Size = new System.Drawing.Size(371, 31);
			this.panel6.TabIndex = 195;
			// 
			// rdoPLINT_I1
			// 
			this.rdoPLINT_I1.AutoSize = true;
			this.rdoPLINT_I1.BackColor = System.Drawing.Color.LightSlateGray;
			this.rdoPLINT_I1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.rdoPLINT_I1.Location = new System.Drawing.Point(209, 6);
			this.rdoPLINT_I1.Name = "rdoPLINT_I1";
			this.rdoPLINT_I1.Size = new System.Drawing.Size(53, 17);
			this.rdoPLINT_I1.TabIndex = 154;
			this.rdoPLINT_I1.Text = "INT1";
			this.rdoPLINT_I1.UseVisualStyleBackColor = false;
			this.rdoPLINT_I1.CheckedChanged += new System.EventHandler(this.rdoPLINT_I1_CheckedChanged);
			// 
			// rdoPLINT_I2
			// 
			this.rdoPLINT_I2.AutoSize = true;
			this.rdoPLINT_I2.BackColor = System.Drawing.Color.LightSlateGray;
			this.rdoPLINT_I2.Checked = true;
			this.rdoPLINT_I2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.rdoPLINT_I2.Location = new System.Drawing.Point(270, 6);
			this.rdoPLINT_I2.Name = "rdoPLINT_I2";
			this.rdoPLINT_I2.Size = new System.Drawing.Size(53, 17);
			this.rdoPLINT_I2.TabIndex = 155;
			this.rdoPLINT_I2.TabStop = true;
			this.rdoPLINT_I2.Text = "INT2";
			this.rdoPLINT_I2.UseVisualStyleBackColor = false;
			this.rdoPLINT_I2.CheckedChanged += new System.EventHandler(this.rdoPLINT_I2_CheckedChanged);
			// 
			// chkEnIntPL
			// 
			this.chkEnIntPL.AutoSize = true;
			this.chkEnIntPL.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chkEnIntPL.Location = new System.Drawing.Point(114, 7);
			this.chkEnIntPL.Name = "chkEnIntPL";
			this.chkEnIntPL.Size = new System.Drawing.Size(89, 17);
			this.chkEnIntPL.TabIndex = 136;
			this.chkEnIntPL.Text = "INT enable";
			this.chkEnIntPL.UseVisualStyleBackColor = true;
			this.chkEnIntPL.CheckedChanged += new System.EventHandler(this.chkEnIntPL_CheckedChanged);
			// 
			// label172
			// 
			this.label172.AutoSize = true;
			this.label172.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label172.Location = new System.Drawing.Point(4, 6);
			this.label172.Name = "label172";
			this.label172.Size = new System.Drawing.Size(98, 20);
			this.label172.TabIndex = 176;
			this.label172.Text = "Orientation";
			// 
			// gbwfs
			// 
			this.gbwfs.BackColor = System.Drawing.Color.LightSlateGray;
			this.gbwfs.Controls.Add(this.chkWakeTrans1);
			this.gbwfs.Controls.Add(this.chkWakeTrans);
			this.gbwfs.Controls.Add(this.chkWakeFIFOGate);
			this.gbwfs.Controls.Add(this.chkWakeLP);
			this.gbwfs.Controls.Add(this.chkWakePulse);
			this.gbwfs.Controls.Add(this.chkWakeMFF1);
			this.gbwfs.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.gbwfs.ForeColor = System.Drawing.Color.White;
			this.gbwfs.Location = new System.Drawing.Point(357, 76);
			this.gbwfs.Name = "gbwfs";
			this.gbwfs.Size = new System.Drawing.Size(136, 135);
			this.gbwfs.TabIndex = 200;
			this.gbwfs.TabStop = false;
			this.gbwfs.Text = "Wake from Sleep";
			this.toolTip1.SetToolTip(this.gbwfs, "Interrupt Based Wake from Sleep Functions- must be mapped to Interrupt function i" +
        "n Map below");
			// 
			// chkWakeTrans1
			// 
			this.chkWakeTrans1.AutoSize = true;
			this.chkWakeTrans1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chkWakeTrans1.Location = new System.Drawing.Point(24, 110);
			this.chkWakeTrans1.Name = "chkWakeTrans1";
			this.chkWakeTrans1.Size = new System.Drawing.Size(90, 17);
			this.chkWakeTrans1.TabIndex = 7;
			this.chkWakeTrans1.Text = "Transient 1";
			this.chkWakeTrans1.UseVisualStyleBackColor = true;
			this.chkWakeTrans1.Visible = false;
			this.chkWakeTrans1.CheckedChanged += new System.EventHandler(this.chkWakeTrans1_CheckedChanged);
			// 
			// chkWakeTrans
			// 
			this.chkWakeTrans.AutoSize = true;
			this.chkWakeTrans.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chkWakeTrans.Location = new System.Drawing.Point(24, 38);
			this.chkWakeTrans.Name = "chkWakeTrans";
			this.chkWakeTrans.Size = new System.Drawing.Size(83, 17);
			this.chkWakeTrans.TabIndex = 6;
			this.chkWakeTrans.Text = "Transient ";
			this.chkWakeTrans.UseVisualStyleBackColor = true;
			this.chkWakeTrans.CheckedChanged += new System.EventHandler(this.chkWakeTrans_CheckedChanged);
			// 
			// chkWakeFIFOGate
			// 
			this.chkWakeFIFOGate.AutoSize = true;
			this.chkWakeFIFOGate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chkWakeFIFOGate.Location = new System.Drawing.Point(24, 20);
			this.chkWakeFIFOGate.Name = "chkWakeFIFOGate";
			this.chkWakeFIFOGate.Size = new System.Drawing.Size(84, 17);
			this.chkWakeFIFOGate.TabIndex = 1;
			this.chkWakeFIFOGate.Text = "FIFO Gate";
			this.chkWakeFIFOGate.UseVisualStyleBackColor = true;
			this.chkWakeFIFOGate.CheckedChanged += new System.EventHandler(this.chkWakeFIFOGate_CheckedChanged);
			// 
			// chkWakeLP
			// 
			this.chkWakeLP.AutoSize = true;
			this.chkWakeLP.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chkWakeLP.Location = new System.Drawing.Point(24, 56);
			this.chkWakeLP.Name = "chkWakeLP";
			this.chkWakeLP.Size = new System.Drawing.Size(88, 17);
			this.chkWakeLP.TabIndex = 5;
			this.chkWakeLP.Text = "Orientation";
			this.chkWakeLP.UseVisualStyleBackColor = true;
			this.chkWakeLP.CheckedChanged += new System.EventHandler(this.chkWakeLP_CheckedChanged);
			// 
			// chkWakePulse
			// 
			this.chkWakePulse.AutoSize = true;
			this.chkWakePulse.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chkWakePulse.Location = new System.Drawing.Point(24, 74);
			this.chkWakePulse.Name = "chkWakePulse";
			this.chkWakePulse.Size = new System.Drawing.Size(57, 17);
			this.chkWakePulse.TabIndex = 2;
			this.chkWakePulse.Text = "Pulse";
			this.chkWakePulse.UseVisualStyleBackColor = true;
			this.chkWakePulse.CheckedChanged += new System.EventHandler(this.chkWakePulse_CheckedChanged);
			// 
			// chkWakeMFF1
			// 
			this.chkWakeMFF1.AutoSize = true;
			this.chkWakeMFF1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chkWakeMFF1.Location = new System.Drawing.Point(24, 92);
			this.chkWakeMFF1.Name = "chkWakeMFF1";
			this.chkWakeMFF1.Size = new System.Drawing.Size(84, 17);
			this.chkWakeMFF1.TabIndex = 3;
			this.chkWakeMFF1.Text = "Motion/FF";
			this.chkWakeMFF1.UseVisualStyleBackColor = true;
			this.chkWakeMFF1.CheckedChanged += new System.EventHandler(this.chkWakeMFF1_CheckedChanged);
			// 
			// p2
			// 
			this.p2.Controls.Add(this.label37);
			this.p2.Controls.Add(this.ddlHPFilter);
			this.p2.ForeColor = System.Drawing.Color.Black;
			this.p2.Location = new System.Drawing.Point(192, 38);
			this.p2.Name = "p2";
			this.p2.Size = new System.Drawing.Size(163, 26);
			this.p2.TabIndex = 218;
			// 
			// label37
			// 
			this.label37.AutoSize = true;
			this.label37.BackColor = System.Drawing.Color.LightSlateGray;
			this.label37.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label37.ForeColor = System.Drawing.Color.White;
			this.label37.Location = new System.Drawing.Point(14, 4);
			this.label37.Name = "label37";
			this.label37.Size = new System.Drawing.Size(68, 16);
			this.label37.TabIndex = 101;
			this.label37.Text = "HP Filter";
			// 
			// ddlHPFilter
			// 
			this.ddlHPFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ddlHPFilter.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ddlHPFilter.FormattingEnabled = true;
			this.ddlHPFilter.Items.AddRange(new object[] {
            "4 Hz",
            "2 Hz",
            "1 Hz",
            "0.5 Hz"});
			this.ddlHPFilter.Location = new System.Drawing.Point(91, 2);
			this.ddlHPFilter.Name = "ddlHPFilter";
			this.ddlHPFilter.Size = new System.Drawing.Size(69, 21);
			this.ddlHPFilter.TabIndex = 100;
			this.toolTip1.SetToolTip(this.ddlHPFilter, "HP Filter 3dB Cut-off Frequency Value Selection");
			this.ddlHPFilter.SelectedIndexChanged += new System.EventHandler(this.ddlHPFilter_SelectedIndexChanged);
			// 
			// groupBox6
			// 
			this.groupBox6.BackColor = System.Drawing.SystemColors.ActiveBorder;
			this.groupBox6.Controls.Add(this.label38);
			this.groupBox6.Controls.Add(this.chkXYZ12Log);
			this.groupBox6.Controls.Add(this.chkTransLog);
			this.groupBox6.Controls.Add(this.chkXYZ8Log);
			this.groupBox6.Controls.Add(this.lblFileName);
			this.groupBox6.Controls.Add(this.txtSaveFileName);
			this.groupBox6.Location = new System.Drawing.Point(0, 0);
			this.groupBox6.Name = "groupBox6";
			this.groupBox6.Size = new System.Drawing.Size(200, 100);
			this.groupBox6.TabIndex = 232;
			this.groupBox6.TabStop = false;
			// 
			// label38
			// 
			this.label38.AutoSize = true;
			this.label38.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label38.Location = new System.Drawing.Point(98, 5);
			this.label38.Name = "label38";
			this.label38.Size = new System.Drawing.Size(100, 16);
			this.label38.TabIndex = 218;
			this.label38.Text = "Datalog Data";
			// 
			// chkXYZ12Log
			// 
			this.chkXYZ12Log.Location = new System.Drawing.Point(0, 0);
			this.chkXYZ12Log.Name = "chkXYZ12Log";
			this.chkXYZ12Log.Size = new System.Drawing.Size(104, 24);
			this.chkXYZ12Log.TabIndex = 219;
			// 
			// chkTransLog
			// 
			this.chkTransLog.Location = new System.Drawing.Point(0, 0);
			this.chkTransLog.Name = "chkTransLog";
			this.chkTransLog.Size = new System.Drawing.Size(104, 24);
			this.chkTransLog.TabIndex = 220;
			// 
			// chkXYZ8Log
			// 
			this.chkXYZ8Log.Location = new System.Drawing.Point(0, 0);
			this.chkXYZ8Log.Name = "chkXYZ8Log";
			this.chkXYZ8Log.Size = new System.Drawing.Size(104, 24);
			this.chkXYZ8Log.TabIndex = 221;
			// 
			// lblFileName
			// 
			this.lblFileName.Location = new System.Drawing.Point(0, 0);
			this.lblFileName.Name = "lblFileName";
			this.lblFileName.Size = new System.Drawing.Size(100, 23);
			this.lblFileName.TabIndex = 222;
			// 
			// txtSaveFileName
			// 
			this.txtSaveFileName.Location = new System.Drawing.Point(0, 0);
			this.txtSaveFileName.Name = "txtSaveFileName";
			this.txtSaveFileName.Size = new System.Drawing.Size(100, 20);
			this.txtSaveFileName.TabIndex = 223;
			// 
			// gbOM
			// 
			this.gbOM.BackColor = System.Drawing.Color.LightSlateGray;
			this.gbOM.Controls.Add(this.chkAnalogLowNoise);
			this.gbOM.Controls.Add(this.chkHPFDataOut);
			this.gbOM.Controls.Add(this.pOverSampling);
			this.gbOM.Controls.Add(this.rdoStandby);
			this.gbOM.Controls.Add(this.p2);
			this.gbOM.Controls.Add(this.rdoActive);
			this.gbOM.Controls.Add(this.ledSleep);
			this.gbOM.Controls.Add(this.lbsSleep);
			this.gbOM.Controls.Add(this.lbsStandby);
			this.gbOM.Controls.Add(this.p1);
			this.gbOM.Controls.Add(this.ledStandby);
			this.gbOM.Controls.Add(this.lbsWake);
			this.gbOM.Controls.Add(this.ledWake);
			this.gbOM.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.gbOM.ForeColor = System.Drawing.Color.White;
			this.gbOM.Location = new System.Drawing.Point(4, -3);
			this.gbOM.Name = "gbOM";
			this.gbOM.Size = new System.Drawing.Size(618, 137);
			this.gbOM.TabIndex = 194;
			this.gbOM.TabStop = false;
			// 
			// chkAnalogLowNoise
			// 
			this.chkAnalogLowNoise.AutoSize = true;
			this.chkAnalogLowNoise.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chkAnalogLowNoise.ForeColor = System.Drawing.Color.White;
			this.chkAnalogLowNoise.Location = new System.Drawing.Point(364, 113);
			this.chkAnalogLowNoise.Name = "chkAnalogLowNoise";
			this.chkAnalogLowNoise.Size = new System.Drawing.Size(200, 17);
			this.chkAnalogLowNoise.TabIndex = 222;
			this.chkAnalogLowNoise.Text = "Enable Low Noise (Up to 5.5g)";
			this.toolTip1.SetToolTip(this.chkAnalogLowNoise, "Enable Interrupt Based Sleep Functions");
			this.chkAnalogLowNoise.UseVisualStyleBackColor = true;
			this.chkAnalogLowNoise.CheckedChanged += new System.EventHandler(this.chkAnalogLowNoise_CheckedChanged);
			// 
			// chkHPFDataOut
			// 
			this.chkHPFDataOut.AutoSize = true;
			this.chkHPFDataOut.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chkHPFDataOut.ForeColor = System.Drawing.Color.White;
			this.chkHPFDataOut.Location = new System.Drawing.Point(364, 94);
			this.chkHPFDataOut.Name = "chkHPFDataOut";
			this.chkHPFDataOut.Size = new System.Drawing.Size(105, 17);
			this.chkHPFDataOut.TabIndex = 231;
			this.chkHPFDataOut.Text = "HPF Data Out";
			this.toolTip1.SetToolTip(this.chkHPFDataOut, "HPF Data");
			this.chkHPFDataOut.UseVisualStyleBackColor = true;
			this.chkHPFDataOut.CheckedChanged += new System.EventHandler(this.chkHPFDataOut_CheckedChanged);
			// 
			// pOverSampling
			// 
			this.pOverSampling.Controls.Add(this.label70);
			this.pOverSampling.Controls.Add(this.rdoOSHiResMode);
			this.pOverSampling.Controls.Add(this.rdoOSLPMode);
			this.pOverSampling.Controls.Add(this.rdoOSLNLPMode);
			this.pOverSampling.Controls.Add(this.rdoOSNormalMode);
			this.pOverSampling.Location = new System.Drawing.Point(4, 67);
			this.pOverSampling.Name = "pOverSampling";
			this.pOverSampling.Size = new System.Drawing.Size(312, 66);
			this.pOverSampling.TabIndex = 221;
			// 
			// label70
			// 
			this.label70.AutoSize = true;
			this.label70.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label70.Location = new System.Drawing.Point(4, 4);
			this.label70.Name = "label70";
			this.label70.Size = new System.Drawing.Size(220, 16);
			this.label70.TabIndex = 222;
			this.label70.Text = "Oversampling Options for Data";
			// 
			// rdoOSHiResMode
			// 
			this.rdoOSHiResMode.AutoSize = true;
			this.rdoOSHiResMode.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.rdoOSHiResMode.Location = new System.Drawing.Point(5, 42);
			this.rdoOSHiResMode.Name = "rdoOSHiResMode";
			this.rdoOSHiResMode.Size = new System.Drawing.Size(108, 19);
			this.rdoOSHiResMode.TabIndex = 224;
			this.rdoOSHiResMode.Text = "Hi Res Mode";
			this.rdoOSHiResMode.UseVisualStyleBackColor = true;
			this.rdoOSHiResMode.CheckedChanged += new System.EventHandler(this.rdoOSHiResMode_CheckedChanged);
			// 
			// rdoOSLPMode
			// 
			this.rdoOSLPMode.AutoSize = true;
			this.rdoOSLPMode.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.rdoOSLPMode.Location = new System.Drawing.Point(129, 22);
			this.rdoOSLPMode.Name = "rdoOSLPMode";
			this.rdoOSLPMode.Size = new System.Drawing.Size(135, 19);
			this.rdoOSLPMode.TabIndex = 223;
			this.rdoOSLPMode.Text = "Low Power Mode";
			this.rdoOSLPMode.UseVisualStyleBackColor = true;
			this.rdoOSLPMode.CheckedChanged += new System.EventHandler(this.rdoOSLPMode_CheckedChanged);
			// 
			// rdoOSLNLPMode
			// 
			this.rdoOSLNLPMode.AutoSize = true;
			this.rdoOSLNLPMode.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.rdoOSLNLPMode.Location = new System.Drawing.Point(129, 41);
			this.rdoOSLNLPMode.Name = "rdoOSLNLPMode";
			this.rdoOSLNLPMode.Size = new System.Drawing.Size(166, 19);
			this.rdoOSLNLPMode.TabIndex = 221;
			this.rdoOSLNLPMode.Text = "Low Noise Low Power";
			this.rdoOSLNLPMode.UseVisualStyleBackColor = true;
			this.rdoOSLNLPMode.CheckedChanged += new System.EventHandler(this.rdoOSLNLPMode_CheckedChanged);
			// 
			// rdoOSNormalMode
			// 
			this.rdoOSNormalMode.AutoSize = true;
			this.rdoOSNormalMode.Checked = true;
			this.rdoOSNormalMode.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.rdoOSNormalMode.Location = new System.Drawing.Point(5, 22);
			this.rdoOSNormalMode.Name = "rdoOSNormalMode";
			this.rdoOSNormalMode.Size = new System.Drawing.Size(112, 19);
			this.rdoOSNormalMode.TabIndex = 220;
			this.rdoOSNormalMode.TabStop = true;
			this.rdoOSNormalMode.Text = "Normal Mode";
			this.rdoOSNormalMode.UseVisualStyleBackColor = true;
			this.rdoOSNormalMode.CheckedChanged += new System.EventHandler(this.rdoOSNormalMode_CheckedChanged);
			// 
			// rdoStandby
			// 
			this.rdoStandby.AutoSize = true;
			this.rdoStandby.Checked = true;
			this.rdoStandby.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.rdoStandby.Location = new System.Drawing.Point(6, 11);
			this.rdoStandby.Name = "rdoStandby";
			this.rdoStandby.Size = new System.Drawing.Size(87, 20);
			this.rdoStandby.TabIndex = 112;
			this.rdoStandby.TabStop = true;
			this.rdoStandby.Text = "Standby ";
			this.toolTip1.SetToolTip(this.rdoStandby, "Standby Mode used to set up all embedded functions");
			this.rdoStandby.UseVisualStyleBackColor = true;
			this.rdoStandby.CheckedChanged += new System.EventHandler(this.rBStandby_CheckedChanged);
			// 
			// rdoActive
			// 
			this.rdoActive.AutoSize = true;
			this.rdoActive.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.rdoActive.Location = new System.Drawing.Point(99, 11);
			this.rdoActive.Name = "rdoActive";
			this.rdoActive.Size = new System.Drawing.Size(69, 20);
			this.rdoActive.TabIndex = 113;
			this.rdoActive.Text = "Active";
			this.toolTip1.SetToolTip(this.rdoActive, "Active Mode Set");
			this.rdoActive.UseVisualStyleBackColor = true;
			this.rdoActive.CheckedChanged += new System.EventHandler(this.rBActive_CheckedChanged);
			// 
			// ledSleep
			// 
			this.ledSleep.LedStyle = NationalInstruments.UI.LedStyle.Round3D;
			this.ledSleep.Location = new System.Drawing.Point(582, 65);
			this.ledSleep.Name = "ledSleep";
			this.ledSleep.OffColor = System.Drawing.Color.Red;
			this.ledSleep.Size = new System.Drawing.Size(30, 31);
			this.ledSleep.TabIndex = 145;
			// 
			// lbsSleep
			// 
			this.lbsSleep.AutoSize = true;
			this.lbsSleep.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lbsSleep.Location = new System.Drawing.Point(492, 72);
			this.lbsSleep.Name = "lbsSleep";
			this.lbsSleep.Size = new System.Drawing.Size(92, 16);
			this.lbsSleep.TabIndex = 185;
			this.lbsSleep.Text = "Sleep Mode";
			// 
			// lbsStandby
			// 
			this.lbsStandby.AutoSize = true;
			this.lbsStandby.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lbsStandby.Location = new System.Drawing.Point(476, 17);
			this.lbsStandby.Name = "lbsStandby";
			this.lbsStandby.Size = new System.Drawing.Size(108, 16);
			this.lbsStandby.TabIndex = 194;
			this.lbsStandby.Text = "Standby Mode";
			// 
			// p1
			// 
			this.p1.BackColor = System.Drawing.Color.LightSlateGray;
			this.p1.Controls.Add(this.ddlDataRate);
			this.p1.Controls.Add(this.label35);
			this.p1.Location = new System.Drawing.Point(3, 34);
			this.p1.Name = "p1";
			this.p1.Size = new System.Drawing.Size(186, 33);
			this.p1.TabIndex = 145;
			// 
			// ddlDataRate
			// 
			this.ddlDataRate.DisplayMember = "(none)";
			this.ddlDataRate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ddlDataRate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ddlDataRate.FormattingEnabled = true;
			this.ddlDataRate.Items.AddRange(new object[] {
            "800Hz",
            "400 Hz",
            "200 Hz",
            "100 Hz",
            "50 Hz",
            "12.5 Hz",
            "6.25 Hz",
            "1.563 Hz"});
			this.ddlDataRate.Location = new System.Drawing.Point(107, 6);
			this.ddlDataRate.Name = "ddlDataRate";
			this.ddlDataRate.Size = new System.Drawing.Size(76, 21);
			this.ddlDataRate.TabIndex = 125;
			this.ddlDataRate.TabStop = false;
			this.toolTip1.SetToolTip(this.ddlDataRate, "Active Mode Sample Rate Selection");
			this.ddlDataRate.SelectedIndexChanged += new System.EventHandler(this.ddlDataRate_SelectedIndexChanged);
			// 
			// label35
			// 
			this.label35.AutoSize = true;
			this.label35.BackColor = System.Drawing.Color.LightSlateGray;
			this.label35.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label35.Location = new System.Drawing.Point(4, 10);
			this.label35.Name = "label35";
			this.label35.Size = new System.Drawing.Size(98, 16);
			this.label35.TabIndex = 126;
			this.label35.Text = "Sample Rate";
			// 
			// ledStandby
			// 
			this.ledStandby.LedStyle = NationalInstruments.UI.LedStyle.Round3D;
			this.ledStandby.Location = new System.Drawing.Point(582, 10);
			this.ledStandby.Name = "ledStandby";
			this.ledStandby.OffColor = System.Drawing.Color.Red;
			this.ledStandby.Size = new System.Drawing.Size(30, 31);
			this.ledStandby.TabIndex = 193;
			this.ledStandby.Value = true;
			// 
			// lbsWake
			// 
			this.lbsWake.AutoSize = true;
			this.lbsWake.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lbsWake.Location = new System.Drawing.Point(493, 44);
			this.lbsWake.Name = "lbsWake";
			this.lbsWake.Size = new System.Drawing.Size(91, 16);
			this.lbsWake.TabIndex = 192;
			this.lbsWake.Text = "Wake Mode";
			// 
			// ledWake
			// 
			this.ledWake.LedStyle = NationalInstruments.UI.LedStyle.Round3D;
			this.ledWake.Location = new System.Drawing.Point(582, 39);
			this.ledWake.Name = "ledWake";
			this.ledWake.OffColor = System.Drawing.Color.Red;
			this.ledWake.Size = new System.Drawing.Size(30, 31);
			this.ledWake.TabIndex = 191;
			// 
			// gbDR
			// 
			this.gbDR.BackColor = System.Drawing.Color.LightSlateGray;
			this.gbDR.Controls.Add(this.lbl4gSensitivity);
			this.gbDR.Controls.Add(this.rdo2g);
			this.gbDR.Controls.Add(this.rdo8g);
			this.gbDR.Controls.Add(this.rdo4g);
			this.gbDR.Controls.Add(this.lbl2gSensitivity);
			this.gbDR.Controls.Add(this.lbl8gSensitivity);
			this.gbDR.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.gbDR.ForeColor = System.Drawing.Color.White;
			this.gbDR.Location = new System.Drawing.Point(627, 1);
			this.gbDR.Name = "gbDR";
			this.gbDR.Size = new System.Drawing.Size(200, 115);
			this.gbDR.TabIndex = 185;
			this.gbDR.TabStop = false;
			this.gbDR.Text = "Dynamic Range";
			// 
			// lbl4gSensitivity
			// 
			this.lbl4gSensitivity.AutoSize = true;
			this.lbl4gSensitivity.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lbl4gSensitivity.ForeColor = System.Drawing.Color.White;
			this.lbl4gSensitivity.Location = new System.Drawing.Point(48, 53);
			this.lbl4gSensitivity.Name = "lbl4gSensitivity";
			this.lbl4gSensitivity.Size = new System.Drawing.Size(132, 16);
			this.lbl4gSensitivity.TabIndex = 119;
			this.lbl4gSensitivity.Text = "2048 counts/g 14b";
			// 
			// rdo2g
			// 
			this.rdo2g.AutoSize = true;
			this.rdo2g.Checked = true;
			this.rdo2g.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.rdo2g.ForeColor = System.Drawing.Color.White;
			this.rdo2g.Location = new System.Drawing.Point(5, 24);
			this.rdo2g.Name = "rdo2g";
			this.rdo2g.Size = new System.Drawing.Size(43, 20);
			this.rdo2g.TabIndex = 115;
			this.rdo2g.TabStop = true;
			this.rdo2g.Text = "2g";
			this.rdo2g.UseVisualStyleBackColor = true;
			this.rdo2g.CheckedChanged += new System.EventHandler(this.rb2g_CheckedChanged);
			// 
			// rdo8g
			// 
			this.rdo8g.AutoSize = true;
			this.rdo8g.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.rdo8g.ForeColor = System.Drawing.Color.White;
			this.rdo8g.Location = new System.Drawing.Point(5, 76);
			this.rdo8g.Name = "rdo8g";
			this.rdo8g.Size = new System.Drawing.Size(43, 20);
			this.rdo8g.TabIndex = 116;
			this.rdo8g.Text = "8g";
			this.rdo8g.UseVisualStyleBackColor = true;
			this.rdo8g.CheckedChanged += new System.EventHandler(this.rb8g_CheckedChanged);
			// 
			// rdo4g
			// 
			this.rdo4g.AutoSize = true;
			this.rdo4g.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.rdo4g.ForeColor = System.Drawing.Color.White;
			this.rdo4g.Location = new System.Drawing.Point(5, 50);
			this.rdo4g.Name = "rdo4g";
			this.rdo4g.Size = new System.Drawing.Size(43, 20);
			this.rdo4g.TabIndex = 117;
			this.rdo4g.Text = "4g";
			this.rdo4g.UseVisualStyleBackColor = true;
			this.rdo4g.CheckedChanged += new System.EventHandler(this.rb4g_CheckedChanged);
			// 
			// lbl2gSensitivity
			// 
			this.lbl2gSensitivity.AutoSize = true;
			this.lbl2gSensitivity.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lbl2gSensitivity.ForeColor = System.Drawing.Color.White;
			this.lbl2gSensitivity.Location = new System.Drawing.Point(49, 26);
			this.lbl2gSensitivity.Name = "lbl2gSensitivity";
			this.lbl2gSensitivity.Size = new System.Drawing.Size(132, 16);
			this.lbl2gSensitivity.TabIndex = 118;
			this.lbl2gSensitivity.Text = "4096 counts/g 14b";
			// 
			// lbl8gSensitivity
			// 
			this.lbl8gSensitivity.AutoSize = true;
			this.lbl8gSensitivity.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lbl8gSensitivity.ForeColor = System.Drawing.Color.White;
			this.lbl8gSensitivity.Location = new System.Drawing.Point(48, 80);
			this.lbl8gSensitivity.Name = "lbl8gSensitivity";
			this.lbl8gSensitivity.Size = new System.Drawing.Size(132, 16);
			this.lbl8gSensitivity.TabIndex = 120;
			this.lbl8gSensitivity.Text = "1024 counts/g 14b";
			// 
			// TabTool
			// 
			this.TabTool.Controls.Add(this.MainScreenEval);
			this.TabTool.Controls.Add(this.Register_Page);
			this.TabTool.Controls.Add(this.DataConfigPage);
			this.TabTool.Controls.Add(this.MFF1_2Page);
			this.TabTool.Controls.Add(this.PL_Page);
			this.TabTool.Controls.Add(this.TransientDetection);
			this.TabTool.Controls.Add(this.PulseDetection);
			this.TabTool.Controls.Add(this.FIFOPage);
			this.TabTool.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.TabTool.Location = new System.Drawing.Point(0, 141);
			this.TabTool.Name = "TabTool";
			this.TabTool.SelectedIndex = 0;
			this.TabTool.Size = new System.Drawing.Size(1132, 554);
			this.TabTool.TabIndex = 113;
			this.TabTool.SelectedIndexChanged += new System.EventHandler(this.TabTool_SelectedIndexChanged);
			// 
			// Register_Page
			// 
			this.Register_Page.BackColor = System.Drawing.Color.LightSlateGray;
			this.Register_Page.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.Register_Page.Location = new System.Drawing.Point(4, 25);
			this.Register_Page.Name = "Register_Page";
			this.Register_Page.Padding = new System.Windows.Forms.Padding(3);
			this.Register_Page.Size = new System.Drawing.Size(1124, 525);
			this.Register_Page.TabIndex = 3;
			this.Register_Page.Text = "Registers";
			// 
			// DataConfigPage
			// 
			this.DataConfigPage.BackColor = System.Drawing.Color.LightSlateGray;
			this.DataConfigPage.Controls.Add(this.groupBox2);
			this.DataConfigPage.Location = new System.Drawing.Point(4, 25);
			this.DataConfigPage.Name = "DataConfigPage";
			this.DataConfigPage.Padding = new System.Windows.Forms.Padding(3);
			this.DataConfigPage.Size = new System.Drawing.Size(1124, 525);
			this.DataConfigPage.TabIndex = 6;
			this.DataConfigPage.Text = "DataConfig";
			// 
			// groupBox2
			// 
			this.groupBox2.BackColor = System.Drawing.Color.LightSlateGray;
			this.groupBox2.Controls.Add(this.gbStatus);
			this.groupBox2.Controls.Add(this.groupBox3);
			this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.groupBox2.ForeColor = System.Drawing.Color.White;
			this.groupBox2.Location = new System.Drawing.Point(10, 32);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(1055, 476);
			this.groupBox2.TabIndex = 12;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Data Configuration  and Interrupt Configuration Settings";
			// 
			// gbStatus
			// 
			this.gbStatus.BackColor = System.Drawing.Color.LightSlateGray;
			this.gbStatus.Controls.Add(this.p21);
			this.gbStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.gbStatus.ForeColor = System.Drawing.Color.White;
			this.gbStatus.Location = new System.Drawing.Point(282, 51);
			this.gbStatus.Name = "gbStatus";
			this.gbStatus.Size = new System.Drawing.Size(685, 370);
			this.gbStatus.TabIndex = 187;
			this.gbStatus.TabStop = false;
			this.gbStatus.Text = "Data Status Reg 0x00: Real Time/ FIFO Data Status";
			// 
			// p21
			// 
			this.p21.BackColor = System.Drawing.Color.LightSlateGray;
			this.p21.Controls.Add(this.lblFIFOStatus);
			this.p21.Controls.Add(this.ledFIFOStatus);
			this.p21.Controls.Add(this.lblFOvf);
			this.p21.Controls.Add(this.ledRealTimeStatus);
			this.p21.Controls.Add(this.label61);
			this.p21.Controls.Add(this.lblWmrk);
			this.p21.Controls.Add(this.lblFCnt5);
			this.p21.Controls.Add(this.lblFCnt4);
			this.p21.Controls.Add(this.lblFCnt3);
			this.p21.Controls.Add(this.lblFCnt2);
			this.p21.Controls.Add(this.lblFCnt1);
			this.p21.Controls.Add(this.lblFCnt0);
			this.p21.Controls.Add(this.ledXYZOW);
			this.p21.Controls.Add(this.ledZOW);
			this.p21.Controls.Add(this.ledYOW);
			this.p21.Controls.Add(this.ledXOW);
			this.p21.Controls.Add(this.ledZDR);
			this.p21.Controls.Add(this.ledYDR);
			this.p21.Controls.Add(this.ledXDR);
			this.p21.Controls.Add(this.ledXYZDR);
			this.p21.Controls.Add(this.lblXYZOW);
			this.p21.Controls.Add(this.lblXYZDrdy);
			this.p21.Controls.Add(this.lblXDrdy);
			this.p21.Controls.Add(this.lblZOW);
			this.p21.Controls.Add(this.lblYOW);
			this.p21.Controls.Add(this.lblYDrdy);
			this.p21.Controls.Add(this.lblXOW);
			this.p21.Controls.Add(this.lblZDrdy);
			this.p21.Enabled = false;
			this.p21.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.p21.Location = new System.Drawing.Point(6, 25);
			this.p21.Name = "p21";
			this.p21.Size = new System.Drawing.Size(673, 339);
			this.p21.TabIndex = 185;
			// 
			// lblFIFOStatus
			// 
			this.lblFIFOStatus.AutoSize = true;
			this.lblFIFOStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblFIFOStatus.ForeColor = System.Drawing.Color.White;
			this.lblFIFOStatus.Location = new System.Drawing.Point(226, 28);
			this.lblFIFOStatus.Name = "lblFIFOStatus";
			this.lblFIFOStatus.Size = new System.Drawing.Size(88, 16);
			this.lblFIFOStatus.TabIndex = 215;
			this.lblFIFOStatus.Text = "FIFO Status";
			// 
			// ledFIFOStatus
			// 
			this.ledFIFOStatus.LedStyle = NationalInstruments.UI.LedStyle.Round3D;
			this.ledFIFOStatus.Location = new System.Drawing.Point(180, 19);
			this.ledFIFOStatus.Name = "ledFIFOStatus";
			this.ledFIFOStatus.OffColor = System.Drawing.Color.Red;
			this.ledFIFOStatus.Size = new System.Drawing.Size(39, 41);
			this.ledFIFOStatus.TabIndex = 216;
			// 
			// lblFOvf
			// 
			this.lblFOvf.AutoSize = true;
			this.lblFOvf.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblFOvf.ForeColor = System.Drawing.Color.White;
			this.lblFOvf.Location = new System.Drawing.Point(558, 285);
			this.lblFOvf.Name = "lblFOvf";
			this.lblFOvf.Size = new System.Drawing.Size(55, 16);
			this.lblFOvf.TabIndex = 227;
			this.lblFOvf.Text = "F_OVF";
			// 
			// ledRealTimeStatus
			// 
			this.ledRealTimeStatus.LedStyle = NationalInstruments.UI.LedStyle.Round3D;
			this.ledRealTimeStatus.Location = new System.Drawing.Point(135, 19);
			this.ledRealTimeStatus.Name = "ledRealTimeStatus";
			this.ledRealTimeStatus.OffColor = System.Drawing.Color.Red;
			this.ledRealTimeStatus.Size = new System.Drawing.Size(40, 40);
			this.ledRealTimeStatus.TabIndex = 214;
			// 
			// label61
			// 
			this.label61.AutoSize = true;
			this.label61.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label61.ForeColor = System.Drawing.Color.White;
			this.label61.Location = new System.Drawing.Point(9, 28);
			this.label61.Name = "label61";
			this.label61.Size = new System.Drawing.Size(127, 16);
			this.label61.TabIndex = 198;
			this.label61.Text = "Real Time Status";
			// 
			// lblWmrk
			// 
			this.lblWmrk.AutoSize = true;
			this.lblWmrk.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblWmrk.ForeColor = System.Drawing.Color.White;
			this.lblWmrk.Location = new System.Drawing.Point(558, 224);
			this.lblWmrk.Name = "lblWmrk";
			this.lblWmrk.Size = new System.Drawing.Size(105, 16);
			this.lblWmrk.TabIndex = 226;
			this.lblWmrk.Text = "WATERMARK";
			// 
			// lblFCnt5
			// 
			this.lblFCnt5.AutoSize = true;
			this.lblFCnt5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblFCnt5.ForeColor = System.Drawing.Color.White;
			this.lblFCnt5.Location = new System.Drawing.Point(558, 166);
			this.lblFCnt5.Name = "lblFCnt5";
			this.lblFCnt5.Size = new System.Drawing.Size(56, 16);
			this.lblFCnt5.TabIndex = 225;
			this.lblFCnt5.Text = "FCNT5";
			// 
			// lblFCnt4
			// 
			this.lblFCnt4.AutoSize = true;
			this.lblFCnt4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblFCnt4.ForeColor = System.Drawing.Color.White;
			this.lblFCnt4.Location = new System.Drawing.Point(558, 102);
			this.lblFCnt4.Name = "lblFCnt4";
			this.lblFCnt4.Size = new System.Drawing.Size(56, 16);
			this.lblFCnt4.TabIndex = 224;
			this.lblFCnt4.Text = "FCNT4";
			// 
			// lblFCnt3
			// 
			this.lblFCnt3.AutoSize = true;
			this.lblFCnt3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblFCnt3.ForeColor = System.Drawing.Color.White;
			this.lblFCnt3.Location = new System.Drawing.Point(203, 282);
			this.lblFCnt3.Name = "lblFCnt3";
			this.lblFCnt3.Size = new System.Drawing.Size(56, 16);
			this.lblFCnt3.TabIndex = 223;
			this.lblFCnt3.Text = "FCNT3";
			// 
			// lblFCnt2
			// 
			this.lblFCnt2.AutoSize = true;
			this.lblFCnt2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblFCnt2.ForeColor = System.Drawing.Color.White;
			this.lblFCnt2.Location = new System.Drawing.Point(203, 224);
			this.lblFCnt2.Name = "lblFCnt2";
			this.lblFCnt2.Size = new System.Drawing.Size(56, 16);
			this.lblFCnt2.TabIndex = 222;
			this.lblFCnt2.Text = "FCNT2";
			// 
			// lblFCnt1
			// 
			this.lblFCnt1.AutoSize = true;
			this.lblFCnt1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblFCnt1.ForeColor = System.Drawing.Color.White;
			this.lblFCnt1.Location = new System.Drawing.Point(203, 163);
			this.lblFCnt1.Name = "lblFCnt1";
			this.lblFCnt1.Size = new System.Drawing.Size(56, 16);
			this.lblFCnt1.TabIndex = 221;
			this.lblFCnt1.Text = "FCNT1";
			// 
			// lblFCnt0
			// 
			this.lblFCnt0.AutoSize = true;
			this.lblFCnt0.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblFCnt0.ForeColor = System.Drawing.Color.White;
			this.lblFCnt0.Location = new System.Drawing.Point(203, 104);
			this.lblFCnt0.Name = "lblFCnt0";
			this.lblFCnt0.Size = new System.Drawing.Size(56, 16);
			this.lblFCnt0.TabIndex = 220;
			this.lblFCnt0.Text = "FCNT0";
			// 
			// ledXYZOW
			// 
			this.ledXYZOW.LedStyle = NationalInstruments.UI.LedStyle.Round3D;
			this.ledXYZOW.Location = new System.Drawing.Point(512, 275);
			this.ledXYZOW.Name = "ledXYZOW";
			this.ledXYZOW.OffColor = System.Drawing.Color.Red;
			this.ledXYZOW.Size = new System.Drawing.Size(40, 40);
			this.ledXYZOW.TabIndex = 219;
			// 
			// ledZOW
			// 
			this.ledZOW.LedStyle = NationalInstruments.UI.LedStyle.Round3D;
			this.ledZOW.Location = new System.Drawing.Point(512, 214);
			this.ledZOW.Name = "ledZOW";
			this.ledZOW.OffColor = System.Drawing.Color.Red;
			this.ledZOW.Size = new System.Drawing.Size(40, 40);
			this.ledZOW.TabIndex = 218;
			// 
			// ledYOW
			// 
			this.ledYOW.LedStyle = NationalInstruments.UI.LedStyle.Round3D;
			this.ledYOW.Location = new System.Drawing.Point(512, 156);
			this.ledYOW.Name = "ledYOW";
			this.ledYOW.OffColor = System.Drawing.Color.Red;
			this.ledYOW.Size = new System.Drawing.Size(40, 40);
			this.ledYOW.TabIndex = 217;
			// 
			// ledXOW
			// 
			this.ledXOW.LedStyle = NationalInstruments.UI.LedStyle.Round3D;
			this.ledXOW.Location = new System.Drawing.Point(512, 96);
			this.ledXOW.Name = "ledXOW";
			this.ledXOW.OffColor = System.Drawing.Color.Red;
			this.ledXOW.Size = new System.Drawing.Size(40, 40);
			this.ledXOW.TabIndex = 216;
			// 
			// ledZDR
			// 
			this.ledZDR.LedStyle = NationalInstruments.UI.LedStyle.Round3D;
			this.ledZDR.Location = new System.Drawing.Point(157, 213);
			this.ledZDR.Name = "ledZDR";
			this.ledZDR.OffColor = System.Drawing.Color.Red;
			this.ledZDR.Size = new System.Drawing.Size(40, 40);
			this.ledZDR.TabIndex = 215;
			// 
			// ledYDR
			// 
			this.ledYDR.LedStyle = NationalInstruments.UI.LedStyle.Round3D;
			this.ledYDR.Location = new System.Drawing.Point(157, 155);
			this.ledYDR.Name = "ledYDR";
			this.ledYDR.OffColor = System.Drawing.Color.Red;
			this.ledYDR.Size = new System.Drawing.Size(40, 40);
			this.ledYDR.TabIndex = 214;
			// 
			// ledXDR
			// 
			this.ledXDR.LedStyle = NationalInstruments.UI.LedStyle.Round3D;
			this.ledXDR.Location = new System.Drawing.Point(157, 95);
			this.ledXDR.Name = "ledXDR";
			this.ledXDR.OffColor = System.Drawing.Color.Red;
			this.ledXDR.Size = new System.Drawing.Size(40, 40);
			this.ledXDR.TabIndex = 213;
			// 
			// ledXYZDR
			// 
			this.ledXYZDR.LedStyle = NationalInstruments.UI.LedStyle.Round3D;
			this.ledXYZDR.Location = new System.Drawing.Point(157, 274);
			this.ledXYZDR.Name = "ledXYZDR";
			this.ledXYZDR.OffColor = System.Drawing.Color.Red;
			this.ledXYZDR.Size = new System.Drawing.Size(40, 40);
			this.ledXYZDR.TabIndex = 211;
			// 
			// lblXYZOW
			// 
			this.lblXYZOW.AutoSize = true;
			this.lblXYZOW.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblXYZOW.ForeColor = System.Drawing.Color.White;
			this.lblXYZOW.Location = new System.Drawing.Point(383, 285);
			this.lblXYZOW.Name = "lblXYZOW";
			this.lblXYZOW.Size = new System.Drawing.Size(131, 16);
			this.lblXYZOW.TabIndex = 210;
			this.lblXYZOW.Text = "X Y or Z Overwrite";
			// 
			// lblXYZDrdy
			// 
			this.lblXYZDrdy.AutoSize = true;
			this.lblXYZDrdy.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblXYZDrdy.ForeColor = System.Drawing.Color.White;
			this.lblXYZDrdy.Location = new System.Drawing.Point(8, 282);
			this.lblXYZDrdy.Name = "lblXYZDrdy";
			this.lblXYZDrdy.Size = new System.Drawing.Size(149, 16);
			this.lblXYZDrdy.TabIndex = 209;
			this.lblXYZDrdy.Text = "X Y or Z Data Ready";
			// 
			// lblXDrdy
			// 
			this.lblXDrdy.AutoSize = true;
			this.lblXDrdy.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblXDrdy.ForeColor = System.Drawing.Color.White;
			this.lblXDrdy.Location = new System.Drawing.Point(54, 104);
			this.lblXDrdy.Name = "lblXDrdy";
			this.lblXDrdy.Size = new System.Drawing.Size(104, 16);
			this.lblXDrdy.TabIndex = 197;
			this.lblXDrdy.Text = "X Data Ready";
			// 
			// lblZOW
			// 
			this.lblZOW.AutoSize = true;
			this.lblZOW.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblZOW.ForeColor = System.Drawing.Color.White;
			this.lblZOW.Location = new System.Drawing.Point(430, 224);
			this.lblZOW.Name = "lblZOW";
			this.lblZOW.Size = new System.Drawing.Size(86, 16);
			this.lblZOW.TabIndex = 207;
			this.lblZOW.Text = "Z Overwrite";
			// 
			// lblYOW
			// 
			this.lblYOW.AutoSize = true;
			this.lblYOW.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblYOW.ForeColor = System.Drawing.Color.White;
			this.lblYOW.Location = new System.Drawing.Point(429, 166);
			this.lblYOW.Name = "lblYOW";
			this.lblYOW.Size = new System.Drawing.Size(87, 16);
			this.lblYOW.TabIndex = 205;
			this.lblYOW.Text = "Y Overwrite";
			// 
			// lblYDrdy
			// 
			this.lblYDrdy.AutoSize = true;
			this.lblYDrdy.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblYDrdy.ForeColor = System.Drawing.Color.White;
			this.lblYDrdy.Location = new System.Drawing.Point(54, 163);
			this.lblYDrdy.Name = "lblYDrdy";
			this.lblYDrdy.Size = new System.Drawing.Size(105, 16);
			this.lblYDrdy.TabIndex = 199;
			this.lblYDrdy.Text = "Y Data Ready";
			// 
			// lblXOW
			// 
			this.lblXOW.AutoSize = true;
			this.lblXOW.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblXOW.ForeColor = System.Drawing.Color.White;
			this.lblXOW.Location = new System.Drawing.Point(429, 102);
			this.lblXOW.Name = "lblXOW";
			this.lblXOW.Size = new System.Drawing.Size(86, 16);
			this.lblXOW.TabIndex = 203;
			this.lblXOW.Text = "X Overwrite";
			// 
			// lblZDrdy
			// 
			this.lblZDrdy.AutoSize = true;
			this.lblZDrdy.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblZDrdy.ForeColor = System.Drawing.Color.White;
			this.lblZDrdy.Location = new System.Drawing.Point(55, 221);
			this.lblZDrdy.Name = "lblZDrdy";
			this.lblZDrdy.Size = new System.Drawing.Size(104, 16);
			this.lblZDrdy.TabIndex = 201;
			this.lblZDrdy.Text = "Z Data Ready";
			// 
			// groupBox3
			// 
			this.groupBox3.BackColor = System.Drawing.Color.LightSlateGray;
			this.groupBox3.Controls.Add(this.p20);
			this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.groupBox3.ForeColor = System.Drawing.Color.White;
			this.groupBox3.Location = new System.Drawing.Point(6, 51);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(258, 370);
			this.groupBox3.TabIndex = 186;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Interrupt Settings Reg 0x2C";
			// 
			// p20
			// 
			this.p20.BackColor = System.Drawing.Color.LightSlateGray;
			this.p20.Controls.Add(this.panel16);
			this.p20.Controls.Add(this.panel17);
			this.p20.ForeColor = System.Drawing.Color.White;
			this.p20.Location = new System.Drawing.Point(6, 24);
			this.p20.Name = "p20";
			this.p20.Size = new System.Drawing.Size(230, 236);
			this.p20.TabIndex = 184;
			// 
			// panel16
			// 
			this.panel16.BackColor = System.Drawing.Color.LightSlateGray;
			this.panel16.Controls.Add(this.rdoINTPushPull);
			this.panel16.Controls.Add(this.rdoINTOpenDrain);
			this.panel16.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.panel16.ForeColor = System.Drawing.Color.White;
			this.panel16.Location = new System.Drawing.Point(19, 103);
			this.panel16.Name = "panel16";
			this.panel16.Size = new System.Drawing.Size(154, 65);
			this.panel16.TabIndex = 206;
			// 
			// rdoINTPushPull
			// 
			this.rdoINTPushPull.AutoSize = true;
			this.rdoINTPushPull.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.rdoINTPushPull.Location = new System.Drawing.Point(9, 6);
			this.rdoINTPushPull.Name = "rdoINTPushPull";
			this.rdoINTPushPull.Size = new System.Drawing.Size(120, 20);
			this.rdoINTPushPull.TabIndex = 201;
			this.rdoINTPushPull.Text = "INT Push/Pull";
			this.rdoINTPushPull.UseVisualStyleBackColor = true;
			this.rdoINTPushPull.CheckedChanged += new System.EventHandler(this.rdoINTPushPull_CheckedChanged);
			// 
			// rdoINTOpenDrain
			// 
			this.rdoINTOpenDrain.AutoSize = true;
			this.rdoINTOpenDrain.Checked = true;
			this.rdoINTOpenDrain.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.rdoINTOpenDrain.Location = new System.Drawing.Point(9, 29);
			this.rdoINTOpenDrain.Name = "rdoINTOpenDrain";
			this.rdoINTOpenDrain.Size = new System.Drawing.Size(133, 20);
			this.rdoINTOpenDrain.TabIndex = 200;
			this.rdoINTOpenDrain.TabStop = true;
			this.rdoINTOpenDrain.Text = "INT Open Drain";
			this.rdoINTOpenDrain.UseVisualStyleBackColor = true;
			this.rdoINTOpenDrain.CheckedChanged += new System.EventHandler(this.rdoINTOpenDrain_CheckedChanged);
			// 
			// panel17
			// 
			this.panel17.BackColor = System.Drawing.Color.LightSlateGray;
			this.panel17.Controls.Add(this.rdoINTActiveLow);
			this.panel17.Controls.Add(this.rdoINTActiveHigh);
			this.panel17.ForeColor = System.Drawing.Color.White;
			this.panel17.Location = new System.Drawing.Point(19, 18);
			this.panel17.Name = "panel17";
			this.panel17.Size = new System.Drawing.Size(203, 65);
			this.panel17.TabIndex = 205;
			// 
			// rdoINTActiveLow
			// 
			this.rdoINTActiveLow.AutoSize = true;
			this.rdoINTActiveLow.Checked = true;
			this.rdoINTActiveLow.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.rdoINTActiveLow.Location = new System.Drawing.Point(6, 6);
			this.rdoINTActiveLow.Name = "rdoINTActiveLow";
			this.rdoINTActiveLow.Size = new System.Drawing.Size(186, 20);
			this.rdoINTActiveLow.TabIndex = 203;
			this.rdoINTActiveLow.TabStop = true;
			this.rdoINTActiveLow.Text = "INT Polarity Active Low";
			this.rdoINTActiveLow.UseVisualStyleBackColor = true;
			this.rdoINTActiveLow.CheckedChanged += new System.EventHandler(this.rdoINTActiveLow_CheckedChanged);
			// 
			// rdoINTActiveHigh
			// 
			this.rdoINTActiveHigh.AutoSize = true;
			this.rdoINTActiveHigh.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.rdoINTActiveHigh.Location = new System.Drawing.Point(6, 31);
			this.rdoINTActiveHigh.Name = "rdoINTActiveHigh";
			this.rdoINTActiveHigh.Size = new System.Drawing.Size(191, 20);
			this.rdoINTActiveHigh.TabIndex = 202;
			this.rdoINTActiveHigh.Text = "INT Polarity Active High";
			this.rdoINTActiveHigh.UseVisualStyleBackColor = true;
			this.rdoINTActiveHigh.CheckedChanged += new System.EventHandler(this.rdoINTActiveHigh_CheckedChanged);
			// 
			// MFF1_2Page
			// 
			this.MFF1_2Page.BackColor = System.Drawing.Color.LightSlateGray;
			this.MFF1_2Page.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.MFF1_2Page.Controls.Add(this.lblMFFDataBit);
			this.MFF1_2Page.Controls.Add(this.chkEnableZMFF);
			this.MFF1_2Page.Controls.Add(this.chkEnableYMFF);
			this.MFF1_2Page.Controls.Add(this.chkEnableXMFF);
			this.MFF1_2Page.Controls.Add(this.lblMotionDataType);
			this.MFF1_2Page.Controls.Add(this.legend3);
			this.MFF1_2Page.Controls.Add(this.MFFGraph);
			this.MFF1_2Page.Controls.Add(this.gbMF1);
			this.MFF1_2Page.Location = new System.Drawing.Point(4, 25);
			this.MFF1_2Page.Name = "MFF1_2Page";
			this.MFF1_2Page.Padding = new System.Windows.Forms.Padding(3);
			this.MFF1_2Page.Size = new System.Drawing.Size(1124, 525);
			this.MFF1_2Page.TabIndex = 2;
			this.MFF1_2Page.Text = "Motion/FF";
			// 
			// lblMFFDataBit
			// 
			this.lblMFFDataBit.AutoSize = true;
			this.lblMFFDataBit.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.lblMFFDataBit.Location = new System.Drawing.Point(954, 18);
			this.lblMFFDataBit.Name = "lblMFFDataBit";
			this.lblMFFDataBit.Size = new System.Drawing.Size(83, 16);
			this.lblMFFDataBit.TabIndex = 241;
			this.lblMFFDataBit.Text = "14-bit Data";
			// 
			// chkEnableZMFF
			// 
			this.chkEnableZMFF.AutoSize = true;
			this.chkEnableZMFF.Checked = true;
			this.chkEnableZMFF.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkEnableZMFF.Location = new System.Drawing.Point(857, 18);
			this.chkEnableZMFF.Name = "chkEnableZMFF";
			this.chkEnableZMFF.Size = new System.Drawing.Size(70, 20);
			this.chkEnableZMFF.TabIndex = 240;
			this.chkEnableZMFF.Text = "Z-Axis";
			this.chkEnableZMFF.UseVisualStyleBackColor = true;
			this.chkEnableZMFF.CheckedChanged += new System.EventHandler(this.chkEnableZMFF_CheckedChanged);
			// 
			// chkEnableYMFF
			// 
			this.chkEnableYMFF.AutoSize = true;
			this.chkEnableYMFF.Checked = true;
			this.chkEnableYMFF.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkEnableYMFF.Location = new System.Drawing.Point(787, 18);
			this.chkEnableYMFF.Name = "chkEnableYMFF";
			this.chkEnableYMFF.Size = new System.Drawing.Size(71, 20);
			this.chkEnableYMFF.TabIndex = 239;
			this.chkEnableYMFF.Text = "Y-Axis";
			this.chkEnableYMFF.UseVisualStyleBackColor = true;
			this.chkEnableYMFF.CheckedChanged += new System.EventHandler(this.chkEnableYMFF_CheckedChanged);
			// 
			// chkEnableXMFF
			// 
			this.chkEnableXMFF.AutoSize = true;
			this.chkEnableXMFF.Checked = true;
			this.chkEnableXMFF.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkEnableXMFF.Location = new System.Drawing.Point(718, 18);
			this.chkEnableXMFF.Name = "chkEnableXMFF";
			this.chkEnableXMFF.Size = new System.Drawing.Size(70, 20);
			this.chkEnableXMFF.TabIndex = 238;
			this.chkEnableXMFF.Text = "X-Axis";
			this.chkEnableXMFF.UseVisualStyleBackColor = true;
			this.chkEnableXMFF.CheckedChanged += new System.EventHandler(this.chkEnableXMFF_CheckedChanged);
			// 
			// lblMotionDataType
			// 
			this.lblMotionDataType.AutoSize = true;
			this.lblMotionDataType.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.lblMotionDataType.Location = new System.Drawing.Point(559, 22);
			this.lblMotionDataType.Name = "lblMotionDataType";
			this.lblMotionDataType.Size = new System.Drawing.Size(99, 16);
			this.lblMotionDataType.TabIndex = 16;
			this.lblMotionDataType.Text = "LPF Data Out";
			// 
			// legend3
			// 
			this.legend3.ForeColor = System.Drawing.Color.White;
			this.legend3.Items.AddRange(new NationalInstruments.UI.LegendItem[] {
            this.legendItem7,
            this.legendItem8,
            this.legendItem9});
			this.legend3.Location = new System.Drawing.Point(595, 377);
			this.legend3.Name = "legend3";
			this.legend3.Size = new System.Drawing.Size(458, 35);
			this.legend3.TabIndex = 15;
			// 
			// legendItem7
			// 
			this.legendItem7.Source = this.XAxis;
			this.legendItem7.Text = "Motion/FF X Axis ";
			// 
			// XAxis
			// 
			this.XAxis.CanScaleYAxis = true;
			this.XAxis.LineColor = System.Drawing.Color.Red;
			this.XAxis.XAxis = this.xAxis1;
			this.XAxis.YAxis = this.yAxis1;
			// 
			// xAxis1
			// 
			this.xAxis1.Caption = "Samples";
			// 
			// yAxis1
			// 
			this.yAxis1.Caption = "Acceleration in g\'s";
			this.yAxis1.Range = NationalInstruments.UI.Range.Parse("-8; 8");
			// 
			// legendItem8
			// 
			this.legendItem8.Source = this.YAxis;
			this.legendItem8.Text = "Motion/FF Y Axis ";
			// 
			// YAxis
			// 
			this.YAxis.CanScaleYAxis = true;
			this.YAxis.LineColor = System.Drawing.Color.LawnGreen;
			this.YAxis.XAxis = this.xAxis1;
			this.YAxis.YAxis = this.yAxis1;
			// 
			// legendItem9
			// 
			this.legendItem9.Source = this.waveformPlot3;
			this.legendItem9.Text = "Motion/FF Z Axis ";
			// 
			// waveformPlot3
			// 
			this.waveformPlot3.CanScaleYAxis = true;
			this.waveformPlot3.LineColor = System.Drawing.Color.Gold;
			this.waveformPlot3.XAxis = this.xAxis2;
			this.waveformPlot3.YAxis = this.yAxis2;
			// 
			// xAxis2
			// 
			this.xAxis2.Caption = "Samples";
			// 
			// yAxis2
			// 
			this.yAxis2.Caption = "Acceleration in g\'s";
			this.yAxis2.Range = NationalInstruments.UI.Range.Parse("-8; 8");
			// 
			// MFFGraph
			// 
			this.MFFGraph.BackColor = System.Drawing.Color.LightSlateGray;
			this.MFFGraph.Caption = "Real Time Output";
			this.MFFGraph.CaptionBackColor = System.Drawing.Color.LightSlateGray;
			this.MFFGraph.CaptionForeColor = System.Drawing.SystemColors.ButtonHighlight;
			this.MFFGraph.ForeColor = System.Drawing.SystemColors.Window;
			this.MFFGraph.ImmediateUpdates = true;
			this.MFFGraph.Location = new System.Drawing.Point(545, 41);
			this.MFFGraph.Name = "MFFGraph";
			this.MFFGraph.PlotAreaBorder = NationalInstruments.UI.Border.Raised;
			this.MFFGraph.Plots.AddRange(new NationalInstruments.UI.WaveformPlot[] {
            this.waveformPlot1,
            this.waveformPlot2,
            this.waveformPlot3});
			this.MFFGraph.Size = new System.Drawing.Size(523, 315);
			this.MFFGraph.TabIndex = 14;
			this.MFFGraph.XAxes.AddRange(new NationalInstruments.UI.XAxis[] {
            this.xAxis2});
			this.MFFGraph.YAxes.AddRange(new NationalInstruments.UI.YAxis[] {
            this.yAxis2});
			// 
			// waveformPlot1
			// 
			this.waveformPlot1.CanScaleYAxis = true;
			this.waveformPlot1.LineColor = System.Drawing.Color.Red;
			this.waveformPlot1.XAxis = this.xAxis2;
			this.waveformPlot1.YAxis = this.yAxis2;
			// 
			// waveformPlot2
			// 
			this.waveformPlot2.CanScaleYAxis = true;
			this.waveformPlot2.LineColor = System.Drawing.Color.LawnGreen;
			this.waveformPlot2.XAxis = this.xAxis2;
			this.waveformPlot2.YAxis = this.yAxis2;
			// 
			// gbMF1
			// 
			this.gbMF1.BackColor = System.Drawing.Color.LightSlateGray;
			this.gbMF1.Controls.Add(this.chkDefaultFFSettings1);
			this.gbMF1.Controls.Add(this.chkDefaultMotion1);
			this.gbMF1.Controls.Add(this.btnMFF1Reset);
			this.gbMF1.Controls.Add(this.btnMFF1Set);
			this.gbMF1.Controls.Add(this.rdoMFF1ClearDebounce);
			this.gbMF1.Controls.Add(this.p14);
			this.gbMF1.Controls.Add(this.p15);
			this.gbMF1.Controls.Add(this.rdoMFF1DecDebounce);
			this.gbMF1.Controls.Add(this.lblMFF1Threshold);
			this.gbMF1.Controls.Add(this.lblMFF1ThresholdVal);
			this.gbMF1.Controls.Add(this.lblMFF1Debouncems);
			this.gbMF1.Controls.Add(this.lblMFF1Thresholdg);
			this.gbMF1.Controls.Add(this.tbMFF1Threshold);
			this.gbMF1.Controls.Add(this.lblMFF1DebounceVal);
			this.gbMF1.Controls.Add(this.tbMFF1Debounce);
			this.gbMF1.Controls.Add(this.lblMFF1Debounce);
			this.gbMF1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.gbMF1.ForeColor = System.Drawing.Color.White;
			this.gbMF1.Location = new System.Drawing.Point(18, 3);
			this.gbMF1.Name = "gbMF1";
			this.gbMF1.Size = new System.Drawing.Size(480, 508);
			this.gbMF1.TabIndex = 8;
			this.gbMF1.TabStop = false;
			this.gbMF1.Text = "Motion Freefall ";
			// 
			// chkDefaultFFSettings1
			// 
			this.chkDefaultFFSettings1.AutoSize = true;
			this.chkDefaultFFSettings1.ForeColor = System.Drawing.Color.Gold;
			this.chkDefaultFFSettings1.Location = new System.Drawing.Point(260, 29);
			this.chkDefaultFFSettings1.Name = "chkDefaultFFSettings1";
			this.chkDefaultFFSettings1.Size = new System.Drawing.Size(197, 20);
			this.chkDefaultFFSettings1.TabIndex = 186;
			this.chkDefaultFFSettings1.Text = "Default Freefall Settings ";
			this.chkDefaultFFSettings1.UseVisualStyleBackColor = true;
			this.chkDefaultFFSettings1.CheckedChanged += new System.EventHandler(this.chkDefaultFFSettings1_CheckedChanged_1);
			// 
			// chkDefaultMotion1
			// 
			this.chkDefaultMotion1.AutoSize = true;
			this.chkDefaultMotion1.ForeColor = System.Drawing.Color.Gold;
			this.chkDefaultMotion1.Location = new System.Drawing.Point(11, 29);
			this.chkDefaultMotion1.Name = "chkDefaultMotion1";
			this.chkDefaultMotion1.Size = new System.Drawing.Size(190, 20);
			this.chkDefaultMotion1.TabIndex = 185;
			this.chkDefaultMotion1.Text = "Default Motion Settings ";
			this.chkDefaultMotion1.UseVisualStyleBackColor = true;
			this.chkDefaultMotion1.CheckedChanged += new System.EventHandler(this.chkDefaultMotion1_CheckedChanged);
			// 
			// btnMFF1Reset
			// 
			this.btnMFF1Reset.BackColor = System.Drawing.Color.LightSlateGray;
			this.btnMFF1Reset.ForeColor = System.Drawing.Color.White;
			this.btnMFF1Reset.Location = new System.Drawing.Point(97, 234);
			this.btnMFF1Reset.Name = "btnMFF1Reset";
			this.btnMFF1Reset.Size = new System.Drawing.Size(62, 31);
			this.btnMFF1Reset.TabIndex = 184;
			this.btnMFF1Reset.Text = "Reset";
			this.btnMFF1Reset.UseVisualStyleBackColor = false;
			this.btnMFF1Reset.Click += new System.EventHandler(this.btnMFF1Reset_Click);
			// 
			// btnMFF1Set
			// 
			this.btnMFF1Set.BackColor = System.Drawing.Color.LightSlateGray;
			this.btnMFF1Set.ForeColor = System.Drawing.Color.White;
			this.btnMFF1Set.Location = new System.Drawing.Point(29, 234);
			this.btnMFF1Set.Name = "btnMFF1Set";
			this.btnMFF1Set.Size = new System.Drawing.Size(62, 31);
			this.btnMFF1Set.TabIndex = 183;
			this.btnMFF1Set.Text = "Set";
			this.toolTip1.SetToolTip(this.btnMFF1Set, "Sets Threshold and debounce, writes in values");
			this.btnMFF1Set.UseVisualStyleBackColor = false;
			this.btnMFF1Set.Click += new System.EventHandler(this.btnMFF1Set_Click);
			// 
			// rdoMFF1ClearDebounce
			// 
			this.rdoMFF1ClearDebounce.AutoSize = true;
			this.rdoMFF1ClearDebounce.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.rdoMFF1ClearDebounce.ForeColor = System.Drawing.Color.White;
			this.rdoMFF1ClearDebounce.Location = new System.Drawing.Point(354, 242);
			this.rdoMFF1ClearDebounce.Name = "rdoMFF1ClearDebounce";
			this.rdoMFF1ClearDebounce.Size = new System.Drawing.Size(102, 17);
			this.rdoMFF1ClearDebounce.TabIndex = 181;
			this.rdoMFF1ClearDebounce.TabStop = true;
			this.rdoMFF1ClearDebounce.Text = "Clear Debounce";
			this.rdoMFF1ClearDebounce.UseVisualStyleBackColor = true;
			this.rdoMFF1ClearDebounce.CheckedChanged += new System.EventHandler(this.rdoMFF1ClearDebounce_CheckedChanged);
			// 
			// p14
			// 
			this.p14.BackColor = System.Drawing.Color.LightSlateGray;
			this.p14.Controls.Add(this.chkMFF1EnableLatch);
			this.p14.Controls.Add(this.chkYEFE);
			this.p14.Controls.Add(this.chkZEFE);
			this.p14.Controls.Add(this.chkXEFE);
			this.p14.Controls.Add(this.rdoMFF1And);
			this.p14.Controls.Add(this.rdoMFF1Or);
			this.p14.ForeColor = System.Drawing.Color.White;
			this.p14.Location = new System.Drawing.Point(7, 76);
			this.p14.Name = "p14";
			this.p14.Size = new System.Drawing.Size(458, 62);
			this.p14.TabIndex = 182;
			// 
			// chkMFF1EnableLatch
			// 
			this.chkMFF1EnableLatch.AutoSize = true;
			this.chkMFF1EnableLatch.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chkMFF1EnableLatch.Location = new System.Drawing.Point(3, 26);
			this.chkMFF1EnableLatch.Name = "chkMFF1EnableLatch";
			this.chkMFF1EnableLatch.Size = new System.Drawing.Size(89, 17);
			this.chkMFF1EnableLatch.TabIndex = 128;
			this.chkMFF1EnableLatch.Text = "Enable Latch";
			this.chkMFF1EnableLatch.UseVisualStyleBackColor = true;
			this.chkMFF1EnableLatch.CheckedChanged += new System.EventHandler(this.chkMFF1EnableLatch_CheckedChanged);
			// 
			// chkYEFE
			// 
			this.chkYEFE.AutoSize = true;
			this.chkYEFE.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chkYEFE.Location = new System.Drawing.Point(232, 5);
			this.chkYEFE.Name = "chkYEFE";
			this.chkYEFE.Size = new System.Drawing.Size(91, 17);
			this.chkYEFE.TabIndex = 127;
			this.chkYEFE.Text = "Enable Y-Axis";
			this.chkYEFE.UseVisualStyleBackColor = true;
			this.chkYEFE.CheckedChanged += new System.EventHandler(this.chkYEFE_CheckedChanged);
			// 
			// chkZEFE
			// 
			this.chkZEFE.AutoSize = true;
			this.chkZEFE.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chkZEFE.Location = new System.Drawing.Point(347, 4);
			this.chkZEFE.Name = "chkZEFE";
			this.chkZEFE.Size = new System.Drawing.Size(91, 17);
			this.chkZEFE.TabIndex = 126;
			this.chkZEFE.Text = "Enable Z-Axis";
			this.chkZEFE.UseVisualStyleBackColor = true;
			this.chkZEFE.CheckedChanged += new System.EventHandler(this.chkZEFE_CheckedChanged);
			// 
			// chkXEFE
			// 
			this.chkXEFE.AutoSize = true;
			this.chkXEFE.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chkXEFE.Location = new System.Drawing.Point(119, 5);
			this.chkXEFE.Name = "chkXEFE";
			this.chkXEFE.Size = new System.Drawing.Size(91, 17);
			this.chkXEFE.TabIndex = 122;
			this.chkXEFE.Text = "Enable X-Axis";
			this.chkXEFE.UseVisualStyleBackColor = true;
			this.chkXEFE.CheckedChanged += new System.EventHandler(this.chkXEFE_CheckedChanged);
			// 
			// rdoMFF1And
			// 
			this.rdoMFF1And.AutoSize = true;
			this.rdoMFF1And.Checked = true;
			this.rdoMFF1And.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.rdoMFF1And.Location = new System.Drawing.Point(44, 2);
			this.rdoMFF1And.Name = "rdoMFF1And";
			this.rdoMFF1And.Size = new System.Drawing.Size(48, 17);
			this.rdoMFF1And.TabIndex = 1;
			this.rdoMFF1And.TabStop = true;
			this.rdoMFF1And.Text = "AND";
			this.rdoMFF1And.UseVisualStyleBackColor = true;
			this.rdoMFF1And.CheckedChanged += new System.EventHandler(this.rdoMFF1And_CheckedChanged);
			// 
			// rdoMFF1Or
			// 
			this.rdoMFF1Or.AutoSize = true;
			this.rdoMFF1Or.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.rdoMFF1Or.Location = new System.Drawing.Point(2, 2);
			this.rdoMFF1Or.Name = "rdoMFF1Or";
			this.rdoMFF1Or.Size = new System.Drawing.Size(41, 17);
			this.rdoMFF1Or.TabIndex = 0;
			this.rdoMFF1Or.TabStop = true;
			this.rdoMFF1Or.Text = "OR";
			this.rdoMFF1Or.UseVisualStyleBackColor = true;
			this.rdoMFF1Or.CheckedChanged += new System.EventHandler(this.rdoMFF1Or_CheckedChanged);
			// 
			// p15
			// 
			this.p15.BackColor = System.Drawing.Color.LightSlateGray;
			this.p15.Controls.Add(this.label20);
			this.p15.Controls.Add(this.label230);
			this.p15.Controls.Add(this.label231);
			this.p15.Controls.Add(this.ledMFF1EA);
			this.p15.Controls.Add(this.label232);
			this.p15.Controls.Add(this.label233);
			this.p15.Controls.Add(this.lblXHP);
			this.p15.Controls.Add(this.ledMFF1XHE);
			this.p15.Controls.Add(this.lblZHP);
			this.p15.Controls.Add(this.label236);
			this.p15.Controls.Add(this.ledMFF1YHE);
			this.p15.Controls.Add(this.lblYHP);
			this.p15.Controls.Add(this.label238);
			this.p15.Controls.Add(this.ledMFF1ZHE);
			this.p15.Controls.Add(this.label239);
			this.p15.Enabled = false;
			this.p15.ForeColor = System.Drawing.Color.White;
			this.p15.Location = new System.Drawing.Point(23, 283);
			this.p15.Name = "p15";
			this.p15.Size = new System.Drawing.Size(434, 219);
			this.p15.TabIndex = 5;
			// 
			// label20
			// 
			this.label20.AutoSize = true;
			this.label20.BackColor = System.Drawing.Color.LightSlateGray;
			this.label20.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label20.ForeColor = System.Drawing.Color.White;
			this.label20.Location = new System.Drawing.Point(176, 17);
			this.label20.Name = "label20";
			this.label20.Size = new System.Drawing.Size(152, 24);
			this.label20.TabIndex = 185;
			this.label20.Text = "Event Detected";
			// 
			// label230
			// 
			this.label230.AutoSize = true;
			this.label230.BackColor = System.Drawing.Color.LightSlateGray;
			this.label230.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label230.ForeColor = System.Drawing.Color.White;
			this.label230.Location = new System.Drawing.Point(209, 101);
			this.label230.Name = "label230";
			this.label230.Size = new System.Drawing.Size(81, 20);
			this.label230.TabIndex = 184;
			this.label230.Text = "Direction";
			// 
			// label231
			// 
			this.label231.AutoSize = true;
			this.label231.BackColor = System.Drawing.Color.LightSlateGray;
			this.label231.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label231.ForeColor = System.Drawing.Color.White;
			this.label231.Location = new System.Drawing.Point(87, 101);
			this.label231.Name = "label231";
			this.label231.Size = new System.Drawing.Size(114, 20);
			this.label231.TabIndex = 183;
			this.label231.Text = "Axis of Event";
			// 
			// ledMFF1EA
			// 
			this.ledMFF1EA.ForeColor = System.Drawing.Color.White;
			this.ledMFF1EA.LedStyle = NationalInstruments.UI.LedStyle.Round3D;
			this.ledMFF1EA.Location = new System.Drawing.Point(334, 4);
			this.ledMFF1EA.Name = "ledMFF1EA";
			this.ledMFF1EA.OffColor = System.Drawing.Color.Red;
			this.ledMFF1EA.Size = new System.Drawing.Size(56, 54);
			this.ledMFF1EA.TabIndex = 182;
			// 
			// label232
			// 
			this.label232.AutoSize = true;
			this.label232.BackColor = System.Drawing.Color.LightSlateGray;
			this.label232.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label232.ForeColor = System.Drawing.Color.White;
			this.label232.Location = new System.Drawing.Point(23, 17);
			this.label232.Name = "label232";
			this.label232.Size = new System.Drawing.Size(147, 24);
			this.label232.TabIndex = 181;
			this.label232.Text = "Motion OR FF ";
			// 
			// label233
			// 
			this.label233.AutoSize = true;
			this.label233.BackColor = System.Drawing.Color.LightSlateGray;
			this.label233.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label233.ForeColor = System.Drawing.Color.White;
			this.label233.Location = new System.Drawing.Point(122, 68);
			this.label233.Name = "label233";
			this.label233.Size = new System.Drawing.Size(135, 24);
			this.label233.TabIndex = 180;
			this.label233.Text = "Motion Status";
			// 
			// lblXHP
			// 
			this.lblXHP.AutoSize = true;
			this.lblXHP.BackColor = System.Drawing.Color.LightSlateGray;
			this.lblXHP.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblXHP.ForeColor = System.Drawing.Color.White;
			this.lblXHP.Location = new System.Drawing.Point(213, 132);
			this.lblXHP.Name = "lblXHP";
			this.lblXHP.Size = new System.Drawing.Size(70, 13);
			this.lblXHP.TabIndex = 175;
			this.lblXHP.Text = "X-Direction";
			// 
			// ledMFF1XHE
			// 
			this.ledMFF1XHE.ForeColor = System.Drawing.Color.White;
			this.ledMFF1XHE.LedStyle = NationalInstruments.UI.LedStyle.Round3D;
			this.ledMFF1XHE.Location = new System.Drawing.Point(157, 127);
			this.ledMFF1XHE.Name = "ledMFF1XHE";
			this.ledMFF1XHE.OffColor = System.Drawing.Color.Red;
			this.ledMFF1XHE.Size = new System.Drawing.Size(30, 30);
			this.ledMFF1XHE.TabIndex = 167;
			// 
			// lblZHP
			// 
			this.lblZHP.AutoSize = true;
			this.lblZHP.BackColor = System.Drawing.Color.LightSlateGray;
			this.lblZHP.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblZHP.ForeColor = System.Drawing.Color.White;
			this.lblZHP.Location = new System.Drawing.Point(213, 192);
			this.lblZHP.Name = "lblZHP";
			this.lblZHP.Size = new System.Drawing.Size(70, 13);
			this.lblZHP.TabIndex = 179;
			this.lblZHP.Text = "Z Direction";
			// 
			// label236
			// 
			this.label236.AutoSize = true;
			this.label236.BackColor = System.Drawing.Color.LightSlateGray;
			this.label236.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label236.ForeColor = System.Drawing.Color.White;
			this.label236.Location = new System.Drawing.Point(108, 134);
			this.label236.Name = "label236";
			this.label236.Size = new System.Drawing.Size(46, 13);
			this.label236.TabIndex = 168;
			this.label236.Text = "X- Axis";
			// 
			// ledMFF1YHE
			// 
			this.ledMFF1YHE.ForeColor = System.Drawing.Color.White;
			this.ledMFF1YHE.LedStyle = NationalInstruments.UI.LedStyle.Round3D;
			this.ledMFF1YHE.Location = new System.Drawing.Point(157, 155);
			this.ledMFF1YHE.Name = "ledMFF1YHE";
			this.ledMFF1YHE.OffColor = System.Drawing.Color.Red;
			this.ledMFF1YHE.Size = new System.Drawing.Size(30, 30);
			this.ledMFF1YHE.TabIndex = 169;
			// 
			// lblYHP
			// 
			this.lblYHP.AutoSize = true;
			this.lblYHP.BackColor = System.Drawing.Color.LightSlateGray;
			this.lblYHP.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblYHP.ForeColor = System.Drawing.Color.White;
			this.lblYHP.Location = new System.Drawing.Point(213, 162);
			this.lblYHP.Name = "lblYHP";
			this.lblYHP.Size = new System.Drawing.Size(70, 13);
			this.lblYHP.TabIndex = 177;
			this.lblYHP.Text = "Y Direction";
			// 
			// label238
			// 
			this.label238.AutoSize = true;
			this.label238.BackColor = System.Drawing.Color.LightSlateGray;
			this.label238.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label238.ForeColor = System.Drawing.Color.White;
			this.label238.Location = new System.Drawing.Point(108, 162);
			this.label238.Name = "label238";
			this.label238.Size = new System.Drawing.Size(42, 13);
			this.label238.TabIndex = 170;
			this.label238.Text = "Y-Axis";
			// 
			// ledMFF1ZHE
			// 
			this.ledMFF1ZHE.ForeColor = System.Drawing.Color.White;
			this.ledMFF1ZHE.LedStyle = NationalInstruments.UI.LedStyle.Round3D;
			this.ledMFF1ZHE.Location = new System.Drawing.Point(157, 185);
			this.ledMFF1ZHE.Name = "ledMFF1ZHE";
			this.ledMFF1ZHE.OffColor = System.Drawing.Color.Red;
			this.ledMFF1ZHE.Size = new System.Drawing.Size(30, 30);
			this.ledMFF1ZHE.TabIndex = 171;
			// 
			// label239
			// 
			this.label239.AutoSize = true;
			this.label239.BackColor = System.Drawing.Color.LightSlateGray;
			this.label239.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label239.ForeColor = System.Drawing.Color.White;
			this.label239.Location = new System.Drawing.Point(108, 192);
			this.label239.Name = "label239";
			this.label239.Size = new System.Drawing.Size(42, 13);
			this.label239.TabIndex = 172;
			this.label239.Text = "Z-Axis";
			// 
			// rdoMFF1DecDebounce
			// 
			this.rdoMFF1DecDebounce.AutoSize = true;
			this.rdoMFF1DecDebounce.Checked = true;
			this.rdoMFF1DecDebounce.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.rdoMFF1DecDebounce.ForeColor = System.Drawing.Color.White;
			this.rdoMFF1DecDebounce.Location = new System.Drawing.Point(187, 242);
			this.rdoMFF1DecDebounce.Name = "rdoMFF1DecDebounce";
			this.rdoMFF1DecDebounce.Size = new System.Drawing.Size(130, 17);
			this.rdoMFF1DecDebounce.TabIndex = 180;
			this.rdoMFF1DecDebounce.TabStop = true;
			this.rdoMFF1DecDebounce.Text = "Decrement Debounce";
			this.rdoMFF1DecDebounce.UseVisualStyleBackColor = true;
			this.rdoMFF1DecDebounce.CheckedChanged += new System.EventHandler(this.rdoMFF1DecDebounce_CheckedChanged);
			// 
			// lblMFF1Threshold
			// 
			this.lblMFF1Threshold.AutoSize = true;
			this.lblMFF1Threshold.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblMFF1Threshold.ForeColor = System.Drawing.Color.White;
			this.lblMFF1Threshold.Location = new System.Drawing.Point(16, 156);
			this.lblMFF1Threshold.Name = "lblMFF1Threshold";
			this.lblMFF1Threshold.Size = new System.Drawing.Size(63, 13);
			this.lblMFF1Threshold.TabIndex = 137;
			this.lblMFF1Threshold.Text = "Threshold";
			// 
			// lblMFF1ThresholdVal
			// 
			this.lblMFF1ThresholdVal.AutoSize = true;
			this.lblMFF1ThresholdVal.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblMFF1ThresholdVal.ForeColor = System.Drawing.Color.White;
			this.lblMFF1ThresholdVal.Location = new System.Drawing.Point(83, 156);
			this.lblMFF1ThresholdVal.Name = "lblMFF1ThresholdVal";
			this.lblMFF1ThresholdVal.Size = new System.Drawing.Size(14, 13);
			this.lblMFF1ThresholdVal.TabIndex = 138;
			this.lblMFF1ThresholdVal.Text = "0";
			// 
			// lblMFF1Debouncems
			// 
			this.lblMFF1Debouncems.AutoSize = true;
			this.lblMFF1Debouncems.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblMFF1Debouncems.ForeColor = System.Drawing.Color.White;
			this.lblMFF1Debouncems.Location = new System.Drawing.Point(131, 199);
			this.lblMFF1Debouncems.Name = "lblMFF1Debouncems";
			this.lblMFF1Debouncems.Size = new System.Drawing.Size(22, 13);
			this.lblMFF1Debouncems.TabIndex = 143;
			this.lblMFF1Debouncems.Text = "ms";
			// 
			// lblMFF1Thresholdg
			// 
			this.lblMFF1Thresholdg.AutoSize = true;
			this.lblMFF1Thresholdg.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblMFF1Thresholdg.ForeColor = System.Drawing.Color.White;
			this.lblMFF1Thresholdg.Location = new System.Drawing.Point(134, 153);
			this.lblMFF1Thresholdg.Name = "lblMFF1Thresholdg";
			this.lblMFF1Thresholdg.Size = new System.Drawing.Size(14, 13);
			this.lblMFF1Thresholdg.TabIndex = 139;
			this.lblMFF1Thresholdg.Text = "g";
			// 
			// tbMFF1Threshold
			// 
			this.tbMFF1Threshold.Location = new System.Drawing.Point(149, 144);
			this.tbMFF1Threshold.Maximum = 127;
			this.tbMFF1Threshold.Name = "tbMFF1Threshold";
			this.tbMFF1Threshold.Size = new System.Drawing.Size(316, 45);
			this.tbMFF1Threshold.TabIndex = 136;
			this.tbMFF1Threshold.Scroll += new System.EventHandler(this.tbMFF1Threshold_Scroll);
			// 
			// lblMFF1DebounceVal
			// 
			this.lblMFF1DebounceVal.AutoSize = true;
			this.lblMFF1DebounceVal.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblMFF1DebounceVal.ForeColor = System.Drawing.Color.White;
			this.lblMFF1DebounceVal.Location = new System.Drawing.Point(82, 199);
			this.lblMFF1DebounceVal.Name = "lblMFF1DebounceVal";
			this.lblMFF1DebounceVal.Size = new System.Drawing.Size(14, 13);
			this.lblMFF1DebounceVal.TabIndex = 142;
			this.lblMFF1DebounceVal.Text = "0";
			// 
			// tbMFF1Debounce
			// 
			this.tbMFF1Debounce.Location = new System.Drawing.Point(149, 190);
			this.tbMFF1Debounce.Maximum = 255;
			this.tbMFF1Debounce.Name = "tbMFF1Debounce";
			this.tbMFF1Debounce.Size = new System.Drawing.Size(316, 45);
			this.tbMFF1Debounce.TabIndex = 140;
			this.tbMFF1Debounce.Scroll += new System.EventHandler(this.tbMFF1Debounce_Scroll);
			// 
			// lblMFF1Debounce
			// 
			this.lblMFF1Debounce.AutoSize = true;
			this.lblMFF1Debounce.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblMFF1Debounce.ForeColor = System.Drawing.Color.White;
			this.lblMFF1Debounce.Location = new System.Drawing.Point(16, 199);
			this.lblMFF1Debounce.Name = "lblMFF1Debounce";
			this.lblMFF1Debounce.Size = new System.Drawing.Size(69, 13);
			this.lblMFF1Debounce.TabIndex = 141;
			this.lblMFF1Debounce.Text = "Debounce ";
			// 
			// TransientDetection
			// 
			this.TransientDetection.BackColor = System.Drawing.Color.LightSlateGray;
			this.TransientDetection.Controls.Add(this.lblTransDataBit);
			this.TransientDetection.Controls.Add(this.chkZEnableTrans);
			this.TransientDetection.Controls.Add(this.chkYEnableTrans);
			this.TransientDetection.Controls.Add(this.chkXEnableTrans);
			this.TransientDetection.Controls.Add(this.lblTransDataType);
			this.TransientDetection.Controls.Add(this.pTrans2);
			this.TransientDetection.Controls.Add(this.gbTSNEW);
			this.TransientDetection.Controls.Add(this.legend2);
			this.TransientDetection.Controls.Add(this.gphXYZ);
			this.TransientDetection.Controls.Add(this.gbTS);
			this.TransientDetection.Location = new System.Drawing.Point(4, 25);
			this.TransientDetection.Name = "TransientDetection";
			this.TransientDetection.Padding = new System.Windows.Forms.Padding(3);
			this.TransientDetection.Size = new System.Drawing.Size(1124, 525);
			this.TransientDetection.TabIndex = 4;
			this.TransientDetection.Text = "Transient Detection";
			// 
			// lblTransDataBit
			// 
			this.lblTransDataBit.AutoSize = true;
			this.lblTransDataBit.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.lblTransDataBit.Location = new System.Drawing.Point(1021, 11);
			this.lblTransDataBit.Name = "lblTransDataBit";
			this.lblTransDataBit.Size = new System.Drawing.Size(83, 16);
			this.lblTransDataBit.TabIndex = 244;
			this.lblTransDataBit.Text = "14-bit Data";
			// 
			// chkZEnableTrans
			// 
			this.chkZEnableTrans.AutoSize = true;
			this.chkZEnableTrans.Checked = true;
			this.chkZEnableTrans.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkZEnableTrans.Location = new System.Drawing.Point(782, 7);
			this.chkZEnableTrans.Name = "chkZEnableTrans";
			this.chkZEnableTrans.Size = new System.Drawing.Size(70, 20);
			this.chkZEnableTrans.TabIndex = 243;
			this.chkZEnableTrans.Text = "Z-Axis";
			this.chkZEnableTrans.UseVisualStyleBackColor = true;
			this.chkZEnableTrans.CheckedChanged += new System.EventHandler(this.chkZEnableTrans_CheckedChanged);
			// 
			// chkYEnableTrans
			// 
			this.chkYEnableTrans.AutoSize = true;
			this.chkYEnableTrans.Checked = true;
			this.chkYEnableTrans.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkYEnableTrans.Location = new System.Drawing.Point(712, 7);
			this.chkYEnableTrans.Name = "chkYEnableTrans";
			this.chkYEnableTrans.Size = new System.Drawing.Size(71, 20);
			this.chkYEnableTrans.TabIndex = 242;
			this.chkYEnableTrans.Text = "Y-Axis";
			this.chkYEnableTrans.UseVisualStyleBackColor = true;
			this.chkYEnableTrans.CheckedChanged += new System.EventHandler(this.chkYEnableTrans_CheckedChanged);
			// 
			// chkXEnableTrans
			// 
			this.chkXEnableTrans.AutoSize = true;
			this.chkXEnableTrans.Checked = true;
			this.chkXEnableTrans.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkXEnableTrans.Location = new System.Drawing.Point(643, 7);
			this.chkXEnableTrans.Name = "chkXEnableTrans";
			this.chkXEnableTrans.Size = new System.Drawing.Size(70, 20);
			this.chkXEnableTrans.TabIndex = 241;
			this.chkXEnableTrans.Text = "X-Axis";
			this.chkXEnableTrans.UseVisualStyleBackColor = true;
			this.chkXEnableTrans.CheckedChanged += new System.EventHandler(this.chkXEnableTrans_CheckedChanged);
			// 
			// lblTransDataType
			// 
			this.lblTransDataType.AutoSize = true;
			this.lblTransDataType.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.lblTransDataType.Location = new System.Drawing.Point(519, 11);
			this.lblTransDataType.Name = "lblTransDataType";
			this.lblTransDataType.Size = new System.Drawing.Size(99, 16);
			this.lblTransDataType.TabIndex = 187;
			this.lblTransDataType.Text = "LPF Data Out";
			// 
			// pTrans2
			// 
			this.pTrans2.BackColor = System.Drawing.Color.LightSlateGray;
			this.pTrans2.Controls.Add(this.lblTransPolZ);
			this.pTrans2.Controls.Add(this.lblTransPolY);
			this.pTrans2.Controls.Add(this.lblTransPolX);
			this.pTrans2.Controls.Add(this.ledTransEA);
			this.pTrans2.Controls.Add(this.label62);
			this.pTrans2.Controls.Add(this.label65);
			this.pTrans2.Controls.Add(this.ledTransZDetect);
			this.pTrans2.Controls.Add(this.ledTransYDetect);
			this.pTrans2.Controls.Add(this.ledTransXDetect);
			this.pTrans2.Controls.Add(this.label67);
			this.pTrans2.Controls.Add(this.label68);
			this.pTrans2.Controls.Add(this.label69);
			this.pTrans2.Enabled = false;
			this.pTrans2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.pTrans2.ForeColor = System.Drawing.Color.White;
			this.pTrans2.Location = new System.Drawing.Point(138, 311);
			this.pTrans2.Name = "pTrans2";
			this.pTrans2.Size = new System.Drawing.Size(217, 189);
			this.pTrans2.TabIndex = 186;
			// 
			// lblTransPolZ
			// 
			this.lblTransPolZ.AutoSize = true;
			this.lblTransPolZ.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblTransPolZ.ForeColor = System.Drawing.Color.White;
			this.lblTransPolZ.Location = new System.Drawing.Point(33, 101);
			this.lblTransPolZ.Name = "lblTransPolZ";
			this.lblTransPolZ.Size = new System.Drawing.Size(97, 20);
			this.lblTransPolZ.TabIndex = 169;
			this.lblTransPolZ.Text = "Direction Z";
			// 
			// lblTransPolY
			// 
			this.lblTransPolY.AutoSize = true;
			this.lblTransPolY.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblTransPolY.ForeColor = System.Drawing.Color.White;
			this.lblTransPolY.Location = new System.Drawing.Point(33, 71);
			this.lblTransPolY.Name = "lblTransPolY";
			this.lblTransPolY.Size = new System.Drawing.Size(98, 20);
			this.lblTransPolY.TabIndex = 168;
			this.lblTransPolY.Text = "Direction Y";
			// 
			// lblTransPolX
			// 
			this.lblTransPolX.AutoSize = true;
			this.lblTransPolX.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblTransPolX.ForeColor = System.Drawing.Color.White;
			this.lblTransPolX.Location = new System.Drawing.Point(33, 43);
			this.lblTransPolX.Name = "lblTransPolX";
			this.lblTransPolX.Size = new System.Drawing.Size(98, 20);
			this.lblTransPolX.TabIndex = 167;
			this.lblTransPolX.Text = "Direction X";
			// 
			// ledTransEA
			// 
			this.ledTransEA.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ledTransEA.ForeColor = System.Drawing.Color.White;
			this.ledTransEA.LedStyle = NationalInstruments.UI.LedStyle.Round3D;
			this.ledTransEA.Location = new System.Drawing.Point(166, 129);
			this.ledTransEA.Name = "ledTransEA";
			this.ledTransEA.OffColor = System.Drawing.Color.Red;
			this.ledTransEA.Size = new System.Drawing.Size(48, 52);
			this.ledTransEA.TabIndex = 165;
			// 
			// label62
			// 
			this.label62.AutoSize = true;
			this.label62.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label62.ForeColor = System.Drawing.Color.White;
			this.label62.Location = new System.Drawing.Point(8, 144);
			this.label62.Name = "label62";
			this.label62.Size = new System.Drawing.Size(152, 24);
			this.label62.TabIndex = 164;
			this.label62.Text = "Event Detected";
			// 
			// label65
			// 
			this.label65.AutoSize = true;
			this.label65.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label65.ForeColor = System.Drawing.Color.White;
			this.label65.Location = new System.Drawing.Point(3, 3);
			this.label65.Name = "label65";
			this.label65.Size = new System.Drawing.Size(165, 24);
			this.label65.TabIndex = 163;
			this.label65.Text = "Transient Status ";
			// 
			// ledTransZDetect
			// 
			this.ledTransZDetect.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ledTransZDetect.ForeColor = System.Drawing.Color.White;
			this.ledTransZDetect.LedStyle = NationalInstruments.UI.LedStyle.Round3D;
			this.ledTransZDetect.Location = new System.Drawing.Point(133, 96);
			this.ledTransZDetect.Name = "ledTransZDetect";
			this.ledTransZDetect.OffColor = System.Drawing.Color.Red;
			this.ledTransZDetect.Size = new System.Drawing.Size(29, 31);
			this.ledTransZDetect.TabIndex = 162;
			// 
			// ledTransYDetect
			// 
			this.ledTransYDetect.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ledTransYDetect.ForeColor = System.Drawing.Color.White;
			this.ledTransYDetect.LedStyle = NationalInstruments.UI.LedStyle.Round3D;
			this.ledTransYDetect.Location = new System.Drawing.Point(133, 68);
			this.ledTransYDetect.Name = "ledTransYDetect";
			this.ledTransYDetect.OffColor = System.Drawing.Color.Red;
			this.ledTransYDetect.Size = new System.Drawing.Size(29, 31);
			this.ledTransYDetect.TabIndex = 161;
			// 
			// ledTransXDetect
			// 
			this.ledTransXDetect.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ledTransXDetect.ForeColor = System.Drawing.Color.White;
			this.ledTransXDetect.LedStyle = NationalInstruments.UI.LedStyle.Round3D;
			this.ledTransXDetect.Location = new System.Drawing.Point(133, 39);
			this.ledTransXDetect.Name = "ledTransXDetect";
			this.ledTransXDetect.OffColor = System.Drawing.Color.Red;
			this.ledTransXDetect.Size = new System.Drawing.Size(29, 29);
			this.ledTransXDetect.TabIndex = 160;
			// 
			// label67
			// 
			this.label67.AutoSize = true;
			this.label67.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label67.ForeColor = System.Drawing.Color.White;
			this.label67.Location = new System.Drawing.Point(10, 101);
			this.label67.Name = "label67";
			this.label67.Size = new System.Drawing.Size(20, 20);
			this.label67.TabIndex = 119;
			this.label67.Text = "Z";
			// 
			// label68
			// 
			this.label68.AutoSize = true;
			this.label68.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label68.ForeColor = System.Drawing.Color.White;
			this.label68.Location = new System.Drawing.Point(8, 71);
			this.label68.Name = "label68";
			this.label68.Size = new System.Drawing.Size(21, 20);
			this.label68.TabIndex = 118;
			this.label68.Text = "Y";
			// 
			// label69
			// 
			this.label69.AutoSize = true;
			this.label69.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label69.ForeColor = System.Drawing.Color.White;
			this.label69.Location = new System.Drawing.Point(8, 43);
			this.label69.Name = "label69";
			this.label69.Size = new System.Drawing.Size(21, 20);
			this.label69.TabIndex = 117;
			this.label69.Text = "X";
			// 
			// gbTSNEW
			// 
			this.gbTSNEW.BackColor = System.Drawing.Color.LightSlateGray;
			this.gbTSNEW.Controls.Add(this.btnResetTransientNEW);
			this.gbTSNEW.Controls.Add(this.btnSetTransientNEW);
			this.gbTSNEW.Controls.Add(this.rdoTransClearDebounceNEW);
			this.gbTSNEW.Controls.Add(this.pTransNEW);
			this.gbTSNEW.Controls.Add(this.tbTransDebounceNEW);
			this.gbTSNEW.Controls.Add(this.p19);
			this.gbTSNEW.Controls.Add(this.lblTransDebounceValNEW);
			this.gbTSNEW.Controls.Add(this.rdoTransDecDebounceNEW);
			this.gbTSNEW.Controls.Add(this.tbTransThresholdNEW);
			this.gbTSNEW.Controls.Add(this.lblTransDebounceNEW);
			this.gbTSNEW.Controls.Add(this.lblTransThresholdNEW);
			this.gbTSNEW.Controls.Add(this.lblTransThresholdValNEW);
			this.gbTSNEW.Controls.Add(this.lblTransThresholdgNEW);
			this.gbTSNEW.Controls.Add(this.lblTransDebouncemsNEW);
			this.gbTSNEW.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.gbTSNEW.ForeColor = System.Drawing.Color.White;
			this.gbTSNEW.Location = new System.Drawing.Point(564, 368);
			this.gbTSNEW.Name = "gbTSNEW";
			this.gbTSNEW.Size = new System.Drawing.Size(501, 249);
			this.gbTSNEW.TabIndex = 185;
			this.gbTSNEW.TabStop = false;
			this.gbTSNEW.Text = "Transient Settings 1";
			this.gbTSNEW.Visible = false;
			// 
			// btnResetTransientNEW
			// 
			this.btnResetTransientNEW.BackColor = System.Drawing.Color.LightSlateGray;
			this.btnResetTransientNEW.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnResetTransientNEW.ForeColor = System.Drawing.Color.White;
			this.btnResetTransientNEW.Location = new System.Drawing.Point(82, 212);
			this.btnResetTransientNEW.Name = "btnResetTransientNEW";
			this.btnResetTransientNEW.Size = new System.Drawing.Size(73, 31);
			this.btnResetTransientNEW.TabIndex = 202;
			this.btnResetTransientNEW.Text = "Reset";
			this.btnResetTransientNEW.UseVisualStyleBackColor = false;
			this.btnResetTransientNEW.Click += new System.EventHandler(this.btnResetTransientNEW_Click_1);
			// 
			// btnSetTransientNEW
			// 
			this.btnSetTransientNEW.BackColor = System.Drawing.Color.LightSlateGray;
			this.btnSetTransientNEW.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnSetTransientNEW.ForeColor = System.Drawing.Color.White;
			this.btnSetTransientNEW.Location = new System.Drawing.Point(5, 212);
			this.btnSetTransientNEW.Name = "btnSetTransientNEW";
			this.btnSetTransientNEW.Size = new System.Drawing.Size(73, 31);
			this.btnSetTransientNEW.TabIndex = 201;
			this.btnSetTransientNEW.Text = "Set";
			this.toolTip1.SetToolTip(this.btnSetTransientNEW, "Sets the Threshold and the Debounce Counter ");
			this.btnSetTransientNEW.UseVisualStyleBackColor = false;
			this.btnSetTransientNEW.Click += new System.EventHandler(this.btnSetTransientNEW_Click_1);
			// 
			// rdoTransClearDebounceNEW
			// 
			this.rdoTransClearDebounceNEW.AutoSize = true;
			this.rdoTransClearDebounceNEW.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.rdoTransClearDebounceNEW.ForeColor = System.Drawing.Color.White;
			this.rdoTransClearDebounceNEW.Location = new System.Drawing.Point(340, 216);
			this.rdoTransClearDebounceNEW.Name = "rdoTransClearDebounceNEW";
			this.rdoTransClearDebounceNEW.Size = new System.Drawing.Size(138, 20);
			this.rdoTransClearDebounceNEW.TabIndex = 183;
			this.rdoTransClearDebounceNEW.TabStop = true;
			this.rdoTransClearDebounceNEW.Text = "Clear Debounce";
			this.rdoTransClearDebounceNEW.UseVisualStyleBackColor = true;
			this.rdoTransClearDebounceNEW.CheckedChanged += new System.EventHandler(this.rdoTransClearDebounceNEW_CheckedChanged_1);
			// 
			// pTransNEW
			// 
			this.pTransNEW.Controls.Add(this.chkDefaultTransSettings1);
			this.pTransNEW.Controls.Add(this.chkTransBypassHPFNEW);
			this.pTransNEW.Controls.Add(this.chkTransEnableLatchNEW);
			this.pTransNEW.Controls.Add(this.chkTransEnableXFlagNEW);
			this.pTransNEW.Controls.Add(this.chkTransEnableZFlagNEW);
			this.pTransNEW.Controls.Add(this.chkTransEnableYFlagNEW);
			this.pTransNEW.ForeColor = System.Drawing.Color.White;
			this.pTransNEW.Location = new System.Drawing.Point(4, 28);
			this.pTransNEW.Name = "pTransNEW";
			this.pTransNEW.Size = new System.Drawing.Size(472, 76);
			this.pTransNEW.TabIndex = 184;
			// 
			// chkDefaultTransSettings1
			// 
			this.chkDefaultTransSettings1.AutoSize = true;
			this.chkDefaultTransSettings1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chkDefaultTransSettings1.ForeColor = System.Drawing.Color.Gold;
			this.chkDefaultTransSettings1.Location = new System.Drawing.Point(6, 9);
			this.chkDefaultTransSettings1.Name = "chkDefaultTransSettings1";
			this.chkDefaultTransSettings1.Size = new System.Drawing.Size(217, 20);
			this.chkDefaultTransSettings1.TabIndex = 203;
			this.chkDefaultTransSettings1.Text = "Default Transient Settings 1";
			this.chkDefaultTransSettings1.UseVisualStyleBackColor = true;
			this.chkDefaultTransSettings1.CheckedChanged += new System.EventHandler(this.chkDefaultTransSettings1_CheckedChanged);
			// 
			// chkTransBypassHPFNEW
			// 
			this.chkTransBypassHPFNEW.AutoSize = true;
			this.chkTransBypassHPFNEW.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chkTransBypassHPFNEW.Location = new System.Drawing.Point(342, 5);
			this.chkTransBypassHPFNEW.Name = "chkTransBypassHPFNEW";
			this.chkTransBypassHPFNEW.Size = new System.Drawing.Size(126, 24);
			this.chkTransBypassHPFNEW.TabIndex = 204;
			this.chkTransBypassHPFNEW.Text = "Bypass HPF";
			this.chkTransBypassHPFNEW.UseVisualStyleBackColor = true;
			this.chkTransBypassHPFNEW.CheckedChanged += new System.EventHandler(this.chkTransBypassHPFNEW_CheckedChanged);
			// 
			// chkTransEnableLatchNEW
			// 
			this.chkTransEnableLatchNEW.AutoSize = true;
			this.chkTransEnableLatchNEW.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chkTransEnableLatchNEW.Location = new System.Drawing.Point(367, 43);
			this.chkTransEnableLatchNEW.Name = "chkTransEnableLatchNEW";
			this.chkTransEnableLatchNEW.Size = new System.Drawing.Size(98, 19);
			this.chkTransEnableLatchNEW.TabIndex = 129;
			this.chkTransEnableLatchNEW.Text = "Enable Latch";
			this.chkTransEnableLatchNEW.UseVisualStyleBackColor = true;
			this.chkTransEnableLatchNEW.CheckedChanged += new System.EventHandler(this.chkTransEnableLatchNEW_CheckedChanged_1);
			// 
			// chkTransEnableXFlagNEW
			// 
			this.chkTransEnableXFlagNEW.AutoSize = true;
			this.chkTransEnableXFlagNEW.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chkTransEnableXFlagNEW.Location = new System.Drawing.Point(6, 43);
			this.chkTransEnableXFlagNEW.Name = "chkTransEnableXFlagNEW";
			this.chkTransEnableXFlagNEW.Size = new System.Drawing.Size(103, 19);
			this.chkTransEnableXFlagNEW.TabIndex = 120;
			this.chkTransEnableXFlagNEW.Text = "Enable X Flag";
			this.chkTransEnableXFlagNEW.UseVisualStyleBackColor = true;
			this.chkTransEnableXFlagNEW.CheckedChanged += new System.EventHandler(this.chkTransEnableXFlagNEW_CheckedChanged_1);
			// 
			// chkTransEnableZFlagNEW
			// 
			this.chkTransEnableZFlagNEW.AutoSize = true;
			this.chkTransEnableZFlagNEW.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chkTransEnableZFlagNEW.Location = new System.Drawing.Point(247, 43);
			this.chkTransEnableZFlagNEW.Name = "chkTransEnableZFlagNEW";
			this.chkTransEnableZFlagNEW.Size = new System.Drawing.Size(102, 19);
			this.chkTransEnableZFlagNEW.TabIndex = 2;
			this.chkTransEnableZFlagNEW.Text = "Enable Z Flag";
			this.chkTransEnableZFlagNEW.UseVisualStyleBackColor = true;
			this.chkTransEnableZFlagNEW.CheckedChanged += new System.EventHandler(this.chkTransEnableZFlagNEW_CheckedChanged_1);
			// 
			// chkTransEnableYFlagNEW
			// 
			this.chkTransEnableYFlagNEW.AutoSize = true;
			this.chkTransEnableYFlagNEW.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chkTransEnableYFlagNEW.Location = new System.Drawing.Point(132, 43);
			this.chkTransEnableYFlagNEW.Name = "chkTransEnableYFlagNEW";
			this.chkTransEnableYFlagNEW.Size = new System.Drawing.Size(102, 19);
			this.chkTransEnableYFlagNEW.TabIndex = 1;
			this.chkTransEnableYFlagNEW.Text = "Enable Y Flag";
			this.chkTransEnableYFlagNEW.UseVisualStyleBackColor = true;
			this.chkTransEnableYFlagNEW.CheckedChanged += new System.EventHandler(this.chkTransEnableYFlagNEW_CheckedChanged_1);
			// 
			// tbTransDebounceNEW
			// 
			this.tbTransDebounceNEW.Location = new System.Drawing.Point(174, 165);
			this.tbTransDebounceNEW.Maximum = 255;
			this.tbTransDebounceNEW.Name = "tbTransDebounceNEW";
			this.tbTransDebounceNEW.Size = new System.Drawing.Size(302, 45);
			this.tbTransDebounceNEW.TabIndex = 121;
			this.tbTransDebounceNEW.Scroll += new System.EventHandler(this.tbTransDebounceNEW_Scroll_1);
			// 
			// p19
			// 
			this.p19.BackColor = System.Drawing.Color.LightSlateGray;
			this.p19.Controls.Add(this.lblTransNewPolZ);
			this.p19.Controls.Add(this.lblTransNewPolY);
			this.p19.Controls.Add(this.lblTransNewPolX);
			this.p19.Controls.Add(this.ledTransEANEW);
			this.p19.Controls.Add(this.label203);
			this.p19.Controls.Add(this.label204);
			this.p19.Controls.Add(this.ledTransZDetectNEW);
			this.p19.Controls.Add(this.ledTransYDetectNEW);
			this.p19.Controls.Add(this.ledTransXDetectNEW);
			this.p19.Controls.Add(this.label205);
			this.p19.Controls.Add(this.label206);
			this.p19.Controls.Add(this.label207);
			this.p19.Enabled = false;
			this.p19.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.p19.ForeColor = System.Drawing.Color.White;
			this.p19.Location = new System.Drawing.Point(242, 16);
			this.p19.Name = "p19";
			this.p19.Size = new System.Drawing.Size(228, 189);
			this.p19.TabIndex = 184;
			this.p19.Visible = false;
			// 
			// lblTransNewPolZ
			// 
			this.lblTransNewPolZ.AutoSize = true;
			this.lblTransNewPolZ.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblTransNewPolZ.ForeColor = System.Drawing.Color.White;
			this.lblTransNewPolZ.Location = new System.Drawing.Point(41, 102);
			this.lblTransNewPolZ.Name = "lblTransNewPolZ";
			this.lblTransNewPolZ.Size = new System.Drawing.Size(97, 20);
			this.lblTransNewPolZ.TabIndex = 169;
			this.lblTransNewPolZ.Text = "Direction Z";
			// 
			// lblTransNewPolY
			// 
			this.lblTransNewPolY.AutoSize = true;
			this.lblTransNewPolY.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblTransNewPolY.ForeColor = System.Drawing.Color.White;
			this.lblTransNewPolY.Location = new System.Drawing.Point(41, 72);
			this.lblTransNewPolY.Name = "lblTransNewPolY";
			this.lblTransNewPolY.Size = new System.Drawing.Size(98, 20);
			this.lblTransNewPolY.TabIndex = 168;
			this.lblTransNewPolY.Text = "Direction Y";
			// 
			// lblTransNewPolX
			// 
			this.lblTransNewPolX.AutoSize = true;
			this.lblTransNewPolX.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblTransNewPolX.ForeColor = System.Drawing.Color.White;
			this.lblTransNewPolX.Location = new System.Drawing.Point(41, 43);
			this.lblTransNewPolX.Name = "lblTransNewPolX";
			this.lblTransNewPolX.Size = new System.Drawing.Size(98, 20);
			this.lblTransNewPolX.TabIndex = 167;
			this.lblTransNewPolX.Text = "Direction X";
			// 
			// ledTransEANEW
			// 
			this.ledTransEANEW.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ledTransEANEW.ForeColor = System.Drawing.Color.White;
			this.ledTransEANEW.LedStyle = NationalInstruments.UI.LedStyle.Round3D;
			this.ledTransEANEW.Location = new System.Drawing.Point(175, 131);
			this.ledTransEANEW.Name = "ledTransEANEW";
			this.ledTransEANEW.OffColor = System.Drawing.Color.Red;
			this.ledTransEANEW.Size = new System.Drawing.Size(49, 52);
			this.ledTransEANEW.TabIndex = 165;
			// 
			// label203
			// 
			this.label203.AutoSize = true;
			this.label203.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label203.ForeColor = System.Drawing.Color.White;
			this.label203.Location = new System.Drawing.Point(10, 147);
			this.label203.Name = "label203";
			this.label203.Size = new System.Drawing.Size(152, 24);
			this.label203.TabIndex = 164;
			this.label203.Text = "Event Detected";
			// 
			// label204
			// 
			this.label204.AutoSize = true;
			this.label204.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label204.ForeColor = System.Drawing.Color.White;
			this.label204.Location = new System.Drawing.Point(3, 3);
			this.label204.Name = "label204";
			this.label204.Size = new System.Drawing.Size(176, 24);
			this.label204.TabIndex = 163;
			this.label204.Text = "Transient Status 1";
			// 
			// ledTransZDetectNEW
			// 
			this.ledTransZDetectNEW.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ledTransZDetectNEW.ForeColor = System.Drawing.Color.White;
			this.ledTransZDetectNEW.LedStyle = NationalInstruments.UI.LedStyle.Round3D;
			this.ledTransZDetectNEW.Location = new System.Drawing.Point(140, 99);
			this.ledTransZDetectNEW.Name = "ledTransZDetectNEW";
			this.ledTransZDetectNEW.OffColor = System.Drawing.Color.Red;
			this.ledTransZDetectNEW.Size = new System.Drawing.Size(30, 29);
			this.ledTransZDetectNEW.TabIndex = 162;
			// 
			// ledTransYDetectNEW
			// 
			this.ledTransYDetectNEW.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ledTransYDetectNEW.ForeColor = System.Drawing.Color.White;
			this.ledTransYDetectNEW.LedStyle = NationalInstruments.UI.LedStyle.Round3D;
			this.ledTransYDetectNEW.Location = new System.Drawing.Point(140, 70);
			this.ledTransYDetectNEW.Name = "ledTransYDetectNEW";
			this.ledTransYDetectNEW.OffColor = System.Drawing.Color.Red;
			this.ledTransYDetectNEW.Size = new System.Drawing.Size(30, 27);
			this.ledTransYDetectNEW.TabIndex = 161;
			// 
			// ledTransXDetectNEW
			// 
			this.ledTransXDetectNEW.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ledTransXDetectNEW.ForeColor = System.Drawing.Color.White;
			this.ledTransXDetectNEW.LedStyle = NationalInstruments.UI.LedStyle.Round3D;
			this.ledTransXDetectNEW.Location = new System.Drawing.Point(140, 39);
			this.ledTransXDetectNEW.Name = "ledTransXDetectNEW";
			this.ledTransXDetectNEW.OffColor = System.Drawing.Color.Red;
			this.ledTransXDetectNEW.Size = new System.Drawing.Size(30, 29);
			this.ledTransXDetectNEW.TabIndex = 160;
			// 
			// label205
			// 
			this.label205.AutoSize = true;
			this.label205.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label205.ForeColor = System.Drawing.Color.White;
			this.label205.Location = new System.Drawing.Point(15, 102);
			this.label205.Name = "label205";
			this.label205.Size = new System.Drawing.Size(20, 20);
			this.label205.TabIndex = 119;
			this.label205.Text = "Z";
			// 
			// label206
			// 
			this.label206.AutoSize = true;
			this.label206.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label206.ForeColor = System.Drawing.Color.White;
			this.label206.Location = new System.Drawing.Point(13, 72);
			this.label206.Name = "label206";
			this.label206.Size = new System.Drawing.Size(21, 20);
			this.label206.TabIndex = 118;
			this.label206.Text = "Y";
			// 
			// label207
			// 
			this.label207.AutoSize = true;
			this.label207.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label207.ForeColor = System.Drawing.Color.White;
			this.label207.Location = new System.Drawing.Point(14, 43);
			this.label207.Name = "label207";
			this.label207.Size = new System.Drawing.Size(21, 20);
			this.label207.TabIndex = 117;
			this.label207.Text = "X";
			// 
			// lblTransDebounceValNEW
			// 
			this.lblTransDebounceValNEW.AutoSize = true;
			this.lblTransDebounceValNEW.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblTransDebounceValNEW.ForeColor = System.Drawing.Color.White;
			this.lblTransDebounceValNEW.Location = new System.Drawing.Point(101, 172);
			this.lblTransDebounceValNEW.Name = "lblTransDebounceValNEW";
			this.lblTransDebounceValNEW.Size = new System.Drawing.Size(16, 16);
			this.lblTransDebounceValNEW.TabIndex = 123;
			this.lblTransDebounceValNEW.Text = "0";
			// 
			// rdoTransDecDebounceNEW
			// 
			this.rdoTransDecDebounceNEW.AutoSize = true;
			this.rdoTransDecDebounceNEW.Checked = true;
			this.rdoTransDecDebounceNEW.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.rdoTransDecDebounceNEW.ForeColor = System.Drawing.Color.White;
			this.rdoTransDecDebounceNEW.Location = new System.Drawing.Point(160, 216);
			this.rdoTransDecDebounceNEW.Name = "rdoTransDecDebounceNEW";
			this.rdoTransDecDebounceNEW.Size = new System.Drawing.Size(176, 20);
			this.rdoTransDecDebounceNEW.TabIndex = 182;
			this.rdoTransDecDebounceNEW.TabStop = true;
			this.rdoTransDecDebounceNEW.Text = "Decrement Debounce";
			this.rdoTransDecDebounceNEW.UseVisualStyleBackColor = true;
			this.rdoTransDecDebounceNEW.CheckedChanged += new System.EventHandler(this.rdoTransDecDebounceNEW_CheckedChanged_1);
			// 
			// tbTransThresholdNEW
			// 
			this.tbTransThresholdNEW.Location = new System.Drawing.Point(174, 116);
			this.tbTransThresholdNEW.Maximum = 127;
			this.tbTransThresholdNEW.Name = "tbTransThresholdNEW";
			this.tbTransThresholdNEW.Size = new System.Drawing.Size(302, 45);
			this.tbTransThresholdNEW.TabIndex = 127;
			this.tbTransThresholdNEW.Scroll += new System.EventHandler(this.tbTransThresholdNEW_Scroll_1);
			// 
			// lblTransDebounceNEW
			// 
			this.lblTransDebounceNEW.AutoSize = true;
			this.lblTransDebounceNEW.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblTransDebounceNEW.ForeColor = System.Drawing.Color.White;
			this.lblTransDebounceNEW.Location = new System.Drawing.Point(5, 172);
			this.lblTransDebounceNEW.Name = "lblTransDebounceNEW";
			this.lblTransDebounceNEW.Size = new System.Drawing.Size(79, 16);
			this.lblTransDebounceNEW.TabIndex = 122;
			this.lblTransDebounceNEW.Text = "Debounce";
			// 
			// lblTransThresholdNEW
			// 
			this.lblTransThresholdNEW.AutoSize = true;
			this.lblTransThresholdNEW.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblTransThresholdNEW.ForeColor = System.Drawing.Color.White;
			this.lblTransThresholdNEW.Location = new System.Drawing.Point(5, 122);
			this.lblTransThresholdNEW.Name = "lblTransThresholdNEW";
			this.lblTransThresholdNEW.Size = new System.Drawing.Size(78, 16);
			this.lblTransThresholdNEW.TabIndex = 112;
			this.lblTransThresholdNEW.Text = "Threshold";
			// 
			// lblTransThresholdValNEW
			// 
			this.lblTransThresholdValNEW.AutoSize = true;
			this.lblTransThresholdValNEW.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblTransThresholdValNEW.ForeColor = System.Drawing.Color.White;
			this.lblTransThresholdValNEW.Location = new System.Drawing.Point(116, 122);
			this.lblTransThresholdValNEW.Name = "lblTransThresholdValNEW";
			this.lblTransThresholdValNEW.Size = new System.Drawing.Size(16, 16);
			this.lblTransThresholdValNEW.TabIndex = 113;
			this.lblTransThresholdValNEW.Text = "0";
			// 
			// lblTransThresholdgNEW
			// 
			this.lblTransThresholdgNEW.AutoSize = true;
			this.lblTransThresholdgNEW.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblTransThresholdgNEW.ForeColor = System.Drawing.Color.White;
			this.lblTransThresholdgNEW.Location = new System.Drawing.Point(161, 119);
			this.lblTransThresholdgNEW.Name = "lblTransThresholdgNEW";
			this.lblTransThresholdgNEW.Size = new System.Drawing.Size(17, 16);
			this.lblTransThresholdgNEW.TabIndex = 120;
			this.lblTransThresholdgNEW.Text = "g";
			// 
			// lblTransDebouncemsNEW
			// 
			this.lblTransDebouncemsNEW.AutoSize = true;
			this.lblTransDebouncemsNEW.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblTransDebouncemsNEW.ForeColor = System.Drawing.Color.White;
			this.lblTransDebouncemsNEW.Location = new System.Drawing.Point(151, 170);
			this.lblTransDebouncemsNEW.Name = "lblTransDebouncemsNEW";
			this.lblTransDebouncemsNEW.Size = new System.Drawing.Size(28, 16);
			this.lblTransDebouncemsNEW.TabIndex = 124;
			this.lblTransDebouncemsNEW.Text = "ms";
			// 
			// legend2
			// 
			this.legend2.ForeColor = System.Drawing.Color.White;
			this.legend2.Items.AddRange(new NationalInstruments.UI.LegendItem[] {
            this.legendItem4,
            this.legendItem5,
            this.legendItem6});
			this.legend2.Location = new System.Drawing.Point(619, 318);
			this.legend2.Name = "legend2";
			this.legend2.Size = new System.Drawing.Size(439, 35);
			this.legend2.TabIndex = 14;
			// 
			// legendItem4
			// 
			this.legendItem4.Source = this.XAxis;
			this.legendItem4.Text = "Transient X Axis ";
			// 
			// legendItem5
			// 
			this.legendItem5.Source = this.YAxis;
			this.legendItem5.Text = "Transient Y Axis ";
			// 
			// legendItem6
			// 
			this.legendItem6.Source = this.ZAxis;
			this.legendItem6.Text = "Transient Z Axis ";
			// 
			// ZAxis
			// 
			this.ZAxis.CanScaleYAxis = true;
			this.ZAxis.LineColor = System.Drawing.Color.Gold;
			this.ZAxis.XAxis = this.xAxis1;
			this.ZAxis.YAxis = this.yAxis1;
			// 
			// gphXYZ
			// 
			this.gphXYZ.BackColor = System.Drawing.Color.LightSlateGray;
			this.gphXYZ.Caption = "Real Time Output";
			this.gphXYZ.CaptionBackColor = System.Drawing.Color.LightSlateGray;
			this.gphXYZ.CaptionForeColor = System.Drawing.SystemColors.ButtonHighlight;
			this.gphXYZ.ForeColor = System.Drawing.SystemColors.Window;
			this.gphXYZ.ImmediateUpdates = true;
			this.gphXYZ.Location = new System.Drawing.Point(513, 30);
			this.gphXYZ.Name = "gphXYZ";
			this.gphXYZ.PlotAreaBorder = NationalInstruments.UI.Border.Raised;
			this.gphXYZ.Plots.AddRange(new NationalInstruments.UI.WaveformPlot[] {
            this.XAxis,
            this.YAxis,
            this.ZAxis});
			this.gphXYZ.Size = new System.Drawing.Size(606, 282);
			this.gphXYZ.TabIndex = 13;
			this.gphXYZ.XAxes.AddRange(new NationalInstruments.UI.XAxis[] {
            this.xAxis1});
			this.gphXYZ.YAxes.AddRange(new NationalInstruments.UI.YAxis[] {
            this.yAxis1});
			// 
			// gbTS
			// 
			this.gbTS.BackColor = System.Drawing.Color.LightSlateGray;
			this.gbTS.Controls.Add(this.btnResetTransient);
			this.gbTS.Controls.Add(this.btnSetTransient);
			this.gbTS.Controls.Add(this.rdoTransClearDebounce);
			this.gbTS.Controls.Add(this.p18);
			this.gbTS.Controls.Add(this.tbTransDebounce);
			this.gbTS.Controls.Add(this.lblTransDebounceVal);
			this.gbTS.Controls.Add(this.rdoTransDecDebounce);
			this.gbTS.Controls.Add(this.tbTransThreshold);
			this.gbTS.Controls.Add(this.lblTransDebounce);
			this.gbTS.Controls.Add(this.lblTransThreshold);
			this.gbTS.Controls.Add(this.lblTransThresholdVal);
			this.gbTS.Controls.Add(this.lblTransThresholdg);
			this.gbTS.Controls.Add(this.lblTransDebouncems);
			this.gbTS.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.gbTS.ForeColor = System.Drawing.Color.White;
			this.gbTS.Location = new System.Drawing.Point(0, 0);
			this.gbTS.Name = "gbTS";
			this.gbTS.Size = new System.Drawing.Size(501, 283);
			this.gbTS.TabIndex = 12;
			this.gbTS.TabStop = false;
			this.gbTS.Text = "Transient Settings";
			// 
			// btnResetTransient
			// 
			this.btnResetTransient.BackColor = System.Drawing.Color.LightSlateGray;
			this.btnResetTransient.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnResetTransient.ForeColor = System.Drawing.Color.White;
			this.btnResetTransient.Location = new System.Drawing.Point(82, 212);
			this.btnResetTransient.Name = "btnResetTransient";
			this.btnResetTransient.Size = new System.Drawing.Size(73, 31);
			this.btnResetTransient.TabIndex = 202;
			this.btnResetTransient.Text = "Reset";
			this.btnResetTransient.UseVisualStyleBackColor = false;
			this.btnResetTransient.Click += new System.EventHandler(this.btnResetTransient_Click);
			// 
			// btnSetTransient
			// 
			this.btnSetTransient.BackColor = System.Drawing.Color.LightSlateGray;
			this.btnSetTransient.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnSetTransient.ForeColor = System.Drawing.Color.White;
			this.btnSetTransient.Location = new System.Drawing.Point(5, 212);
			this.btnSetTransient.Name = "btnSetTransient";
			this.btnSetTransient.Size = new System.Drawing.Size(73, 31);
			this.btnSetTransient.TabIndex = 201;
			this.btnSetTransient.Text = "Set";
			this.toolTip1.SetToolTip(this.btnSetTransient, "Sets the Threshold and the Debounce Counter ");
			this.btnSetTransient.UseVisualStyleBackColor = false;
			this.btnSetTransient.Click += new System.EventHandler(this.btnSetTransient_Click);
			// 
			// rdoTransClearDebounce
			// 
			this.rdoTransClearDebounce.AutoSize = true;
			this.rdoTransClearDebounce.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.rdoTransClearDebounce.ForeColor = System.Drawing.Color.White;
			this.rdoTransClearDebounce.Location = new System.Drawing.Point(340, 216);
			this.rdoTransClearDebounce.Name = "rdoTransClearDebounce";
			this.rdoTransClearDebounce.Size = new System.Drawing.Size(138, 20);
			this.rdoTransClearDebounce.TabIndex = 183;
			this.rdoTransClearDebounce.TabStop = true;
			this.rdoTransClearDebounce.Text = "Clear Debounce";
			this.rdoTransClearDebounce.UseVisualStyleBackColor = true;
			this.rdoTransClearDebounce.CheckedChanged += new System.EventHandler(this.rdoTransClearDebounce_CheckedChanged);
			// 
			// p18
			// 
			this.p18.Controls.Add(this.chkDefaultTransSettings);
			this.p18.Controls.Add(this.chkTransBypassHPF);
			this.p18.Controls.Add(this.chkTransEnableLatch);
			this.p18.Controls.Add(this.chkTransEnableXFlag);
			this.p18.Controls.Add(this.chkTransEnableZFlag);
			this.p18.Controls.Add(this.chkTransEnableYFlag);
			this.p18.ForeColor = System.Drawing.Color.White;
			this.p18.Location = new System.Drawing.Point(4, 28);
			this.p18.Name = "p18";
			this.p18.Size = new System.Drawing.Size(472, 76);
			this.p18.TabIndex = 184;
			// 
			// chkDefaultTransSettings
			// 
			this.chkDefaultTransSettings.AutoSize = true;
			this.chkDefaultTransSettings.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chkDefaultTransSettings.ForeColor = System.Drawing.Color.Gold;
			this.chkDefaultTransSettings.Location = new System.Drawing.Point(3, 9);
			this.chkDefaultTransSettings.Name = "chkDefaultTransSettings";
			this.chkDefaultTransSettings.Size = new System.Drawing.Size(205, 20);
			this.chkDefaultTransSettings.TabIndex = 203;
			this.chkDefaultTransSettings.Text = "Default Transient Settings";
			this.chkDefaultTransSettings.UseVisualStyleBackColor = true;
			this.chkDefaultTransSettings.CheckedChanged += new System.EventHandler(this.chkDefaultTransSettings_CheckedChanged);
			// 
			// chkTransBypassHPF
			// 
			this.chkTransBypassHPF.AutoSize = true;
			this.chkTransBypassHPF.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chkTransBypassHPF.Location = new System.Drawing.Point(342, 6);
			this.chkTransBypassHPF.Name = "chkTransBypassHPF";
			this.chkTransBypassHPF.Size = new System.Drawing.Size(126, 24);
			this.chkTransBypassHPF.TabIndex = 204;
			this.chkTransBypassHPF.Text = "Bypass HPF";
			this.chkTransBypassHPF.UseVisualStyleBackColor = true;
			this.chkTransBypassHPF.CheckedChanged += new System.EventHandler(this.chkTransBypassHPF_CheckedChanged);
			// 
			// chkTransEnableLatch
			// 
			this.chkTransEnableLatch.AutoSize = true;
			this.chkTransEnableLatch.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chkTransEnableLatch.Location = new System.Drawing.Point(365, 51);
			this.chkTransEnableLatch.Name = "chkTransEnableLatch";
			this.chkTransEnableLatch.Size = new System.Drawing.Size(98, 19);
			this.chkTransEnableLatch.TabIndex = 129;
			this.chkTransEnableLatch.Text = "Enable Latch";
			this.chkTransEnableLatch.UseVisualStyleBackColor = true;
			this.chkTransEnableLatch.CheckedChanged += new System.EventHandler(this.chkTransEnableLatch_CheckedChanged);
			// 
			// chkTransEnableXFlag
			// 
			this.chkTransEnableXFlag.AutoSize = true;
			this.chkTransEnableXFlag.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chkTransEnableXFlag.Location = new System.Drawing.Point(4, 51);
			this.chkTransEnableXFlag.Name = "chkTransEnableXFlag";
			this.chkTransEnableXFlag.Size = new System.Drawing.Size(103, 19);
			this.chkTransEnableXFlag.TabIndex = 120;
			this.chkTransEnableXFlag.Text = "Enable X Flag";
			this.chkTransEnableXFlag.UseVisualStyleBackColor = true;
			this.chkTransEnableXFlag.CheckedChanged += new System.EventHandler(this.chkTransEnableXFlag_CheckedChanged_1);
			// 
			// chkTransEnableZFlag
			// 
			this.chkTransEnableZFlag.AutoSize = true;
			this.chkTransEnableZFlag.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chkTransEnableZFlag.Location = new System.Drawing.Point(245, 51);
			this.chkTransEnableZFlag.Name = "chkTransEnableZFlag";
			this.chkTransEnableZFlag.Size = new System.Drawing.Size(102, 19);
			this.chkTransEnableZFlag.TabIndex = 2;
			this.chkTransEnableZFlag.Text = "Enable Z Flag";
			this.chkTransEnableZFlag.UseVisualStyleBackColor = true;
			this.chkTransEnableZFlag.CheckedChanged += new System.EventHandler(this.chkTransEnableZFlag_CheckedChanged);
			// 
			// chkTransEnableYFlag
			// 
			this.chkTransEnableYFlag.AutoSize = true;
			this.chkTransEnableYFlag.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chkTransEnableYFlag.Location = new System.Drawing.Point(130, 51);
			this.chkTransEnableYFlag.Name = "chkTransEnableYFlag";
			this.chkTransEnableYFlag.Size = new System.Drawing.Size(102, 19);
			this.chkTransEnableYFlag.TabIndex = 1;
			this.chkTransEnableYFlag.Text = "Enable Y Flag";
			this.chkTransEnableYFlag.UseVisualStyleBackColor = true;
			this.chkTransEnableYFlag.CheckedChanged += new System.EventHandler(this.chkTransEnableYFlag_CheckedChanged);
			// 
			// tbTransDebounce
			// 
			this.tbTransDebounce.Location = new System.Drawing.Point(174, 165);
			this.tbTransDebounce.Maximum = 255;
			this.tbTransDebounce.Name = "tbTransDebounce";
			this.tbTransDebounce.Size = new System.Drawing.Size(302, 45);
			this.tbTransDebounce.TabIndex = 121;
			this.tbTransDebounce.Scroll += new System.EventHandler(this.tbTransDebounce_Scroll_1);
			// 
			// lblTransDebounceVal
			// 
			this.lblTransDebounceVal.AutoSize = true;
			this.lblTransDebounceVal.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblTransDebounceVal.ForeColor = System.Drawing.Color.White;
			this.lblTransDebounceVal.Location = new System.Drawing.Point(101, 172);
			this.lblTransDebounceVal.Name = "lblTransDebounceVal";
			this.lblTransDebounceVal.Size = new System.Drawing.Size(16, 16);
			this.lblTransDebounceVal.TabIndex = 123;
			this.lblTransDebounceVal.Text = "0";
			// 
			// rdoTransDecDebounce
			// 
			this.rdoTransDecDebounce.AutoSize = true;
			this.rdoTransDecDebounce.Checked = true;
			this.rdoTransDecDebounce.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.rdoTransDecDebounce.ForeColor = System.Drawing.Color.White;
			this.rdoTransDecDebounce.Location = new System.Drawing.Point(160, 216);
			this.rdoTransDecDebounce.Name = "rdoTransDecDebounce";
			this.rdoTransDecDebounce.Size = new System.Drawing.Size(176, 20);
			this.rdoTransDecDebounce.TabIndex = 182;
			this.rdoTransDecDebounce.TabStop = true;
			this.rdoTransDecDebounce.Text = "Decrement Debounce";
			this.rdoTransDecDebounce.UseVisualStyleBackColor = true;
			this.rdoTransDecDebounce.CheckedChanged += new System.EventHandler(this.rdoTransDecDebounce_CheckedChanged);
			// 
			// tbTransThreshold
			// 
			this.tbTransThreshold.Location = new System.Drawing.Point(174, 116);
			this.tbTransThreshold.Maximum = 127;
			this.tbTransThreshold.Name = "tbTransThreshold";
			this.tbTransThreshold.Size = new System.Drawing.Size(302, 45);
			this.tbTransThreshold.TabIndex = 127;
			this.tbTransThreshold.Scroll += new System.EventHandler(this.tbTransThreshold_Scroll_1);
			// 
			// lblTransDebounce
			// 
			this.lblTransDebounce.AutoSize = true;
			this.lblTransDebounce.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblTransDebounce.ForeColor = System.Drawing.Color.White;
			this.lblTransDebounce.Location = new System.Drawing.Point(5, 172);
			this.lblTransDebounce.Name = "lblTransDebounce";
			this.lblTransDebounce.Size = new System.Drawing.Size(79, 16);
			this.lblTransDebounce.TabIndex = 122;
			this.lblTransDebounce.Text = "Debounce";
			// 
			// lblTransThreshold
			// 
			this.lblTransThreshold.AutoSize = true;
			this.lblTransThreshold.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblTransThreshold.ForeColor = System.Drawing.Color.White;
			this.lblTransThreshold.Location = new System.Drawing.Point(5, 122);
			this.lblTransThreshold.Name = "lblTransThreshold";
			this.lblTransThreshold.Size = new System.Drawing.Size(78, 16);
			this.lblTransThreshold.TabIndex = 112;
			this.lblTransThreshold.Text = "Threshold";
			// 
			// lblTransThresholdVal
			// 
			this.lblTransThresholdVal.AutoSize = true;
			this.lblTransThresholdVal.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblTransThresholdVal.ForeColor = System.Drawing.Color.White;
			this.lblTransThresholdVal.Location = new System.Drawing.Point(116, 122);
			this.lblTransThresholdVal.Name = "lblTransThresholdVal";
			this.lblTransThresholdVal.Size = new System.Drawing.Size(16, 16);
			this.lblTransThresholdVal.TabIndex = 113;
			this.lblTransThresholdVal.Text = "0";
			// 
			// lblTransThresholdg
			// 
			this.lblTransThresholdg.AutoSize = true;
			this.lblTransThresholdg.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblTransThresholdg.ForeColor = System.Drawing.Color.White;
			this.lblTransThresholdg.Location = new System.Drawing.Point(161, 119);
			this.lblTransThresholdg.Name = "lblTransThresholdg";
			this.lblTransThresholdg.Size = new System.Drawing.Size(17, 16);
			this.lblTransThresholdg.TabIndex = 120;
			this.lblTransThresholdg.Text = "g";
			// 
			// lblTransDebouncems
			// 
			this.lblTransDebouncems.AutoSize = true;
			this.lblTransDebouncems.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblTransDebouncems.ForeColor = System.Drawing.Color.White;
			this.lblTransDebouncems.Location = new System.Drawing.Point(151, 170);
			this.lblTransDebouncems.Name = "lblTransDebouncems";
			this.lblTransDebouncems.Size = new System.Drawing.Size(28, 16);
			this.lblTransDebouncems.TabIndex = 124;
			this.lblTransDebouncems.Text = "ms";
			// 
			// PulseDetection
			// 
			this.PulseDetection.BackColor = System.Drawing.Color.LightSlateGray;
			this.PulseDetection.Controls.Add(this.gbSDPS);
			this.PulseDetection.Location = new System.Drawing.Point(4, 25);
			this.PulseDetection.Name = "PulseDetection";
			this.PulseDetection.Padding = new System.Windows.Forms.Padding(3);
			this.PulseDetection.Size = new System.Drawing.Size(1124, 525);
			this.PulseDetection.TabIndex = 5;
			this.PulseDetection.Text = "Pulse Detection";
			// 
			// gbSDPS
			// 
			this.gbSDPS.BackColor = System.Drawing.Color.LightSlateGray;
			this.gbSDPS.Controls.Add(this.panel15);
			this.gbSDPS.Controls.Add(this.rdoDefaultSPDP);
			this.gbSDPS.Controls.Add(this.rdoDefaultSP);
			this.gbSDPS.Controls.Add(this.btnResetPulseThresholds);
			this.gbSDPS.Controls.Add(this.btnSetPulseThresholds);
			this.gbSDPS.Controls.Add(this.tbPulseZThreshold);
			this.gbSDPS.Controls.Add(this.p12);
			this.gbSDPS.Controls.Add(this.p10);
			this.gbSDPS.Controls.Add(this.p11);
			this.gbSDPS.Controls.Add(this.tbPulseXThreshold);
			this.gbSDPS.Controls.Add(this.lblPulseYThreshold);
			this.gbSDPS.Controls.Add(this.tbPulseYThreshold);
			this.gbSDPS.Controls.Add(this.lblPulseYThresholdVal);
			this.gbSDPS.Controls.Add(this.lblPulseXThresholdg);
			this.gbSDPS.Controls.Add(this.lblPulseXThresholdVal);
			this.gbSDPS.Controls.Add(this.lblPulseZThresholdg);
			this.gbSDPS.Controls.Add(this.lblPulseZThreshold);
			this.gbSDPS.Controls.Add(this.lblPulseZThresholdVal);
			this.gbSDPS.Controls.Add(this.lblPulseYThresholdg);
			this.gbSDPS.Controls.Add(this.lblPulseXThreshold);
			this.gbSDPS.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.gbSDPS.ForeColor = System.Drawing.Color.White;
			this.gbSDPS.Location = new System.Drawing.Point(25, 3);
			this.gbSDPS.Name = "gbSDPS";
			this.gbSDPS.Size = new System.Drawing.Size(1072, 539);
			this.gbSDPS.TabIndex = 8;
			this.gbSDPS.TabStop = false;
			this.gbSDPS.Text = "Single/ Double Tap Settings";
			// 
			// panel15
			// 
			this.panel15.Controls.Add(this.chkPulseLPFEnable);
			this.panel15.Controls.Add(this.chkPulseHPFBypass);
			this.panel15.Location = new System.Drawing.Point(633, 46);
			this.panel15.Name = "panel15";
			this.panel15.Size = new System.Drawing.Size(348, 39);
			this.panel15.TabIndex = 235;
			// 
			// chkPulseLPFEnable
			// 
			this.chkPulseLPFEnable.AutoSize = true;
			this.chkPulseLPFEnable.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chkPulseLPFEnable.ForeColor = System.Drawing.Color.White;
			this.chkPulseLPFEnable.Location = new System.Drawing.Point(44, 10);
			this.chkPulseLPFEnable.Name = "chkPulseLPFEnable";
			this.chkPulseLPFEnable.Size = new System.Drawing.Size(91, 17);
			this.chkPulseLPFEnable.TabIndex = 234;
			this.chkPulseLPFEnable.Text = "LPF Enable";
			this.toolTip1.SetToolTip(this.chkPulseLPFEnable, "FDE FIFO Data Enable");
			this.chkPulseLPFEnable.UseVisualStyleBackColor = true;
			this.chkPulseLPFEnable.CheckedChanged += new System.EventHandler(this.chkPulseLPFEnable_CheckedChanged);
			// 
			// chkPulseHPFBypass
			// 
			this.chkPulseHPFBypass.AutoSize = true;
			this.chkPulseHPFBypass.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chkPulseHPFBypass.ForeColor = System.Drawing.Color.White;
			this.chkPulseHPFBypass.Location = new System.Drawing.Point(205, 10);
			this.chkPulseHPFBypass.Name = "chkPulseHPFBypass";
			this.chkPulseHPFBypass.Size = new System.Drawing.Size(94, 17);
			this.chkPulseHPFBypass.TabIndex = 233;
			this.chkPulseHPFBypass.Text = "HPF Bypass";
			this.toolTip1.SetToolTip(this.chkPulseHPFBypass, "HPF Data");
			this.chkPulseHPFBypass.UseVisualStyleBackColor = true;
			this.chkPulseHPFBypass.CheckedChanged += new System.EventHandler(this.chkPulseHPFBypass_CheckedChanged);
			// 
			// rdoDefaultSPDP
			// 
			this.rdoDefaultSPDP.AutoSize = true;
			this.rdoDefaultSPDP.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.rdoDefaultSPDP.ForeColor = System.Drawing.Color.Gold;
			this.rdoDefaultSPDP.Location = new System.Drawing.Point(836, 15);
			this.rdoDefaultSPDP.Name = "rdoDefaultSPDP";
			this.rdoDefaultSPDP.Size = new System.Drawing.Size(221, 20);
			this.rdoDefaultSPDP.TabIndex = 164;
			this.rdoDefaultSPDP.TabStop = true;
			this.rdoDefaultSPDP.Text = "Default Single + Double Tap";
			this.rdoDefaultSPDP.UseVisualStyleBackColor = true;
			this.rdoDefaultSPDP.CheckedChanged += new System.EventHandler(this.rdoDefaultSPDP_CheckedChanged);
			// 
			// rdoDefaultSP
			// 
			this.rdoDefaultSP.AutoSize = true;
			this.rdoDefaultSP.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.rdoDefaultSP.ForeColor = System.Drawing.Color.Gold;
			this.rdoDefaultSP.Location = new System.Drawing.Point(633, 15);
			this.rdoDefaultSP.Name = "rdoDefaultSP";
			this.rdoDefaultSP.Size = new System.Drawing.Size(155, 20);
			this.rdoDefaultSP.TabIndex = 163;
			this.rdoDefaultSP.TabStop = true;
			this.rdoDefaultSP.Text = "Default Single Tap";
			this.rdoDefaultSP.UseVisualStyleBackColor = true;
			this.rdoDefaultSP.CheckedChanged += new System.EventHandler(this.rdoDefaultSP_CheckedChanged);
			// 
			// btnResetPulseThresholds
			// 
			this.btnResetPulseThresholds.BackColor = System.Drawing.Color.LightSlateGray;
			this.btnResetPulseThresholds.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnResetPulseThresholds.ForeColor = System.Drawing.Color.White;
			this.btnResetPulseThresholds.Location = new System.Drawing.Point(348, 301);
			this.btnResetPulseThresholds.Name = "btnResetPulseThresholds";
			this.btnResetPulseThresholds.Size = new System.Drawing.Size(176, 31);
			this.btnResetPulseThresholds.TabIndex = 160;
			this.btnResetPulseThresholds.Text = "Reset XYZ Thresholds";
			this.toolTip1.SetToolTip(this.btnResetPulseThresholds, "Sets the XYZ Threshold Values which can be modified in Active Mode");
			this.btnResetPulseThresholds.UseVisualStyleBackColor = false;
			this.btnResetPulseThresholds.Click += new System.EventHandler(this.btnResetPulseThresholds_Click_1);
			// 
			// btnSetPulseThresholds
			// 
			this.btnSetPulseThresholds.BackColor = System.Drawing.Color.LightSlateGray;
			this.btnSetPulseThresholds.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnSetPulseThresholds.ForeColor = System.Drawing.Color.White;
			this.btnSetPulseThresholds.Location = new System.Drawing.Point(183, 301);
			this.btnSetPulseThresholds.Name = "btnSetPulseThresholds";
			this.btnSetPulseThresholds.Size = new System.Drawing.Size(155, 31);
			this.btnSetPulseThresholds.TabIndex = 159;
			this.btnSetPulseThresholds.Text = "Set XYZ Thresholds";
			this.toolTip1.SetToolTip(this.btnSetPulseThresholds, "Sets the XYZ Threshold Values which can be modified in Active Mode");
			this.btnSetPulseThresholds.UseVisualStyleBackColor = false;
			this.btnSetPulseThresholds.Click += new System.EventHandler(this.btnSetPulseThresholds_Click_1);
			// 
			// tbPulseZThreshold
			// 
			this.tbPulseZThreshold.BackColor = System.Drawing.Color.LightSlateGray;
			this.tbPulseZThreshold.Location = new System.Drawing.Point(152, 250);
			this.tbPulseZThreshold.Maximum = 127;
			this.tbPulseZThreshold.Name = "tbPulseZThreshold";
			this.tbPulseZThreshold.Size = new System.Drawing.Size(372, 45);
			this.tbPulseZThreshold.TabIndex = 140;
			this.tbPulseZThreshold.Scroll += new System.EventHandler(this.tbPulseZThreshold_Scroll_1);
			// 
			// p12
			// 
			this.p12.BackColor = System.Drawing.Color.LightSlateGray;
			this.p12.Controls.Add(this.btnPulseResetTime2ndPulse);
			this.p12.Controls.Add(this.btnPulseSetTime2ndPulse);
			this.p12.Controls.Add(this.tbPulseLatency);
			this.p12.Controls.Add(this.chkPulseEnableXDP);
			this.p12.Controls.Add(this.chkPulseEnableYDP);
			this.p12.Controls.Add(this.chkPulseEnableZDP);
			this.p12.Controls.Add(this.lblPulse2ndPulseWinms);
			this.p12.Controls.Add(this.lblPulseLatency);
			this.p12.Controls.Add(this.lblPulse2ndPulseWinVal);
			this.p12.Controls.Add(this.lblPulseLatencyVal);
			this.p12.Controls.Add(this.lblPulse2ndPulseWin);
			this.p12.Controls.Add(this.lblPulseLatencyms);
			this.p12.Controls.Add(this.tbPulse2ndPulseWin);
			this.p12.Controls.Add(this.chkPulseIgnorLatentPulses);
			this.p12.Location = new System.Drawing.Point(4, 341);
			this.p12.Name = "p12";
			this.p12.Size = new System.Drawing.Size(532, 180);
			this.p12.TabIndex = 158;
			// 
			// btnPulseResetTime2ndPulse
			// 
			this.btnPulseResetTime2ndPulse.BackColor = System.Drawing.Color.LightSlateGray;
			this.btnPulseResetTime2ndPulse.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnPulseResetTime2ndPulse.ForeColor = System.Drawing.Color.White;
			this.btnPulseResetTime2ndPulse.Location = new System.Drawing.Point(370, 135);
			this.btnPulseResetTime2ndPulse.Name = "btnPulseResetTime2ndPulse";
			this.btnPulseResetTime2ndPulse.Size = new System.Drawing.Size(150, 31);
			this.btnPulseResetTime2ndPulse.TabIndex = 159;
			this.btnPulseResetTime2ndPulse.Text = "Reset Time Limits";
			this.toolTip1.SetToolTip(this.btnPulseResetTime2ndPulse, "Sets the time between pulses (the latency) and the 2nd pulse window time period");
			this.btnPulseResetTime2ndPulse.UseVisualStyleBackColor = false;
			this.btnPulseResetTime2ndPulse.Click += new System.EventHandler(this.btnPulseResetTime2ndPulse_Click_1);
			// 
			// btnPulseSetTime2ndPulse
			// 
			this.btnPulseSetTime2ndPulse.BackColor = System.Drawing.Color.LightSlateGray;
			this.btnPulseSetTime2ndPulse.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnPulseSetTime2ndPulse.ForeColor = System.Drawing.Color.White;
			this.btnPulseSetTime2ndPulse.Location = new System.Drawing.Point(211, 135);
			this.btnPulseSetTime2ndPulse.Name = "btnPulseSetTime2ndPulse";
			this.btnPulseSetTime2ndPulse.Size = new System.Drawing.Size(123, 31);
			this.btnPulseSetTime2ndPulse.TabIndex = 158;
			this.btnPulseSetTime2ndPulse.Text = "Set Time Limits";
			this.toolTip1.SetToolTip(this.btnPulseSetTime2ndPulse, "Sets the time between pulses (the latency) and the 2nd pulse window time period");
			this.btnPulseSetTime2ndPulse.UseVisualStyleBackColor = false;
			this.btnPulseSetTime2ndPulse.Click += new System.EventHandler(this.btnPulseSetTime2ndPulse_Click_1);
			// 
			// tbPulseLatency
			// 
			this.tbPulseLatency.Location = new System.Drawing.Point(188, 37);
			this.tbPulseLatency.Maximum = 255;
			this.tbPulseLatency.Name = "tbPulseLatency";
			this.tbPulseLatency.Size = new System.Drawing.Size(332, 45);
			this.tbPulseLatency.TabIndex = 149;
			this.tbPulseLatency.Scroll += new System.EventHandler(this.tbPulseLatency_Scroll_1);
			// 
			// chkPulseEnableXDP
			// 
			this.chkPulseEnableXDP.AutoSize = true;
			this.chkPulseEnableXDP.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chkPulseEnableXDP.ForeColor = System.Drawing.Color.White;
			this.chkPulseEnableXDP.Location = new System.Drawing.Point(6, 3);
			this.chkPulseEnableXDP.Name = "chkPulseEnableXDP";
			this.chkPulseEnableXDP.Size = new System.Drawing.Size(96, 19);
			this.chkPulseEnableXDP.TabIndex = 124;
			this.chkPulseEnableXDP.Text = "Enable X DP";
			this.chkPulseEnableXDP.UseVisualStyleBackColor = true;
			this.chkPulseEnableXDP.CheckedChanged += new System.EventHandler(this.chkPulseEnableXDP_CheckedChanged_1);
			// 
			// chkPulseEnableYDP
			// 
			this.chkPulseEnableYDP.AutoSize = true;
			this.chkPulseEnableYDP.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chkPulseEnableYDP.ForeColor = System.Drawing.Color.White;
			this.chkPulseEnableYDP.Location = new System.Drawing.Point(102, 3);
			this.chkPulseEnableYDP.Name = "chkPulseEnableYDP";
			this.chkPulseEnableYDP.Size = new System.Drawing.Size(95, 19);
			this.chkPulseEnableYDP.TabIndex = 125;
			this.chkPulseEnableYDP.Text = "Enable Y DP";
			this.chkPulseEnableYDP.UseVisualStyleBackColor = true;
			this.chkPulseEnableYDP.CheckedChanged += new System.EventHandler(this.chkPulseEnableYDP_CheckedChanged_1);
			// 
			// chkPulseEnableZDP
			// 
			this.chkPulseEnableZDP.AutoSize = true;
			this.chkPulseEnableZDP.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chkPulseEnableZDP.ForeColor = System.Drawing.Color.White;
			this.chkPulseEnableZDP.Location = new System.Drawing.Point(201, 4);
			this.chkPulseEnableZDP.Name = "chkPulseEnableZDP";
			this.chkPulseEnableZDP.Size = new System.Drawing.Size(95, 19);
			this.chkPulseEnableZDP.TabIndex = 126;
			this.chkPulseEnableZDP.Text = "Enable Z DP";
			this.chkPulseEnableZDP.UseVisualStyleBackColor = true;
			this.chkPulseEnableZDP.CheckedChanged += new System.EventHandler(this.chkPulseEnableZDP_CheckedChanged);
			// 
			// lblPulse2ndPulseWinms
			// 
			this.lblPulse2ndPulseWinms.AutoSize = true;
			this.lblPulse2ndPulseWinms.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblPulse2ndPulseWinms.ForeColor = System.Drawing.Color.White;
			this.lblPulse2ndPulseWinms.Location = new System.Drawing.Point(163, 93);
			this.lblPulse2ndPulseWinms.Name = "lblPulse2ndPulseWinms";
			this.lblPulse2ndPulseWinms.Size = new System.Drawing.Size(24, 15);
			this.lblPulse2ndPulseWinms.TabIndex = 157;
			this.lblPulse2ndPulseWinms.Text = "ms";
			// 
			// lblPulseLatency
			// 
			this.lblPulseLatency.AutoSize = true;
			this.lblPulseLatency.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblPulseLatency.ForeColor = System.Drawing.Color.White;
			this.lblPulseLatency.Location = new System.Drawing.Point(6, 40);
			this.lblPulseLatency.Name = "lblPulseLatency";
			this.lblPulseLatency.Size = new System.Drawing.Size(83, 15);
			this.lblPulseLatency.TabIndex = 150;
			this.lblPulseLatency.Text = "Pulse Latency";
			// 
			// lblPulse2ndPulseWinVal
			// 
			this.lblPulse2ndPulseWinVal.AutoSize = true;
			this.lblPulse2ndPulseWinVal.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblPulse2ndPulseWinVal.ForeColor = System.Drawing.Color.White;
			this.lblPulse2ndPulseWinVal.Location = new System.Drawing.Point(118, 94);
			this.lblPulse2ndPulseWinVal.Name = "lblPulse2ndPulseWinVal";
			this.lblPulse2ndPulseWinVal.Size = new System.Drawing.Size(14, 15);
			this.lblPulse2ndPulseWinVal.TabIndex = 156;
			this.lblPulse2ndPulseWinVal.Text = "0";
			// 
			// lblPulseLatencyVal
			// 
			this.lblPulseLatencyVal.AutoSize = true;
			this.lblPulseLatencyVal.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblPulseLatencyVal.ForeColor = System.Drawing.Color.White;
			this.lblPulseLatencyVal.Location = new System.Drawing.Point(118, 40);
			this.lblPulseLatencyVal.Name = "lblPulseLatencyVal";
			this.lblPulseLatencyVal.Size = new System.Drawing.Size(14, 15);
			this.lblPulseLatencyVal.TabIndex = 151;
			this.lblPulseLatencyVal.Text = "0";
			// 
			// lblPulse2ndPulseWin
			// 
			this.lblPulse2ndPulseWin.AutoSize = true;
			this.lblPulse2ndPulseWin.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblPulse2ndPulseWin.ForeColor = System.Drawing.Color.White;
			this.lblPulse2ndPulseWin.Location = new System.Drawing.Point(6, 94);
			this.lblPulse2ndPulseWin.Name = "lblPulse2ndPulseWin";
			this.lblPulse2ndPulseWin.Size = new System.Drawing.Size(109, 15);
			this.lblPulse2ndPulseWin.TabIndex = 155;
			this.lblPulse2ndPulseWin.Text = "2nd Pulse Window";
			// 
			// lblPulseLatencyms
			// 
			this.lblPulseLatencyms.AutoSize = true;
			this.lblPulseLatencyms.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblPulseLatencyms.ForeColor = System.Drawing.Color.White;
			this.lblPulseLatencyms.Location = new System.Drawing.Point(168, 37);
			this.lblPulseLatencyms.Name = "lblPulseLatencyms";
			this.lblPulseLatencyms.Size = new System.Drawing.Size(24, 15);
			this.lblPulseLatencyms.TabIndex = 152;
			this.lblPulseLatencyms.Text = "ms";
			// 
			// tbPulse2ndPulseWin
			// 
			this.tbPulse2ndPulseWin.Location = new System.Drawing.Point(188, 88);
			this.tbPulse2ndPulseWin.Maximum = 255;
			this.tbPulse2ndPulseWin.Name = "tbPulse2ndPulseWin";
			this.tbPulse2ndPulseWin.Size = new System.Drawing.Size(332, 45);
			this.tbPulse2ndPulseWin.TabIndex = 154;
			this.tbPulse2ndPulseWin.Scroll += new System.EventHandler(this.tbPulse2ndPulseWin_Scroll_1);
			// 
			// chkPulseIgnorLatentPulses
			// 
			this.chkPulseIgnorLatentPulses.AutoSize = true;
			this.chkPulseIgnorLatentPulses.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chkPulseIgnorLatentPulses.ForeColor = System.Drawing.Color.White;
			this.chkPulseIgnorLatentPulses.Location = new System.Drawing.Point(333, 4);
			this.chkPulseIgnorLatentPulses.Name = "chkPulseIgnorLatentPulses";
			this.chkPulseIgnorLatentPulses.Size = new System.Drawing.Size(131, 19);
			this.chkPulseIgnorLatentPulses.TabIndex = 153;
			this.chkPulseIgnorLatentPulses.Text = "Ignor Latent Pulses";
			this.chkPulseIgnorLatentPulses.UseVisualStyleBackColor = true;
			this.chkPulseIgnorLatentPulses.CheckedChanged += new System.EventHandler(this.chkPulseIgnorLatentPulses_CheckedChanged_1);
			// 
			// p10
			// 
			this.p10.BackColor = System.Drawing.Color.LightSlateGray;
			this.p10.Controls.Add(this.btnResetFirstPulseTimeLimit);
			this.p10.Controls.Add(this.btnSetFirstPulseTimeLimit);
			this.p10.Controls.Add(this.chkPulseEnableLatch);
			this.p10.Controls.Add(this.chkPulseEnableXSP);
			this.p10.Controls.Add(this.chkPulseEnableYSP);
			this.p10.Controls.Add(this.chkPulseEnableZSP);
			this.p10.Controls.Add(this.tbFirstPulseTimeLimit);
			this.p10.Controls.Add(this.lblFirstPulseTimeLimitms);
			this.p10.Controls.Add(this.lblFirstTimeLimitVal);
			this.p10.Controls.Add(this.lblFirstPulseTimeLimit);
			this.p10.ForeColor = System.Drawing.Color.White;
			this.p10.Location = new System.Drawing.Point(5, 27);
			this.p10.Name = "p10";
			this.p10.Size = new System.Drawing.Size(531, 121);
			this.p10.TabIndex = 5;
			// 
			// btnResetFirstPulseTimeLimit
			// 
			this.btnResetFirstPulseTimeLimit.BackColor = System.Drawing.Color.LightSlateGray;
			this.btnResetFirstPulseTimeLimit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnResetFirstPulseTimeLimit.ForeColor = System.Drawing.Color.White;
			this.btnResetFirstPulseTimeLimit.Location = new System.Drawing.Point(384, 82);
			this.btnResetFirstPulseTimeLimit.Name = "btnResetFirstPulseTimeLimit";
			this.btnResetFirstPulseTimeLimit.Size = new System.Drawing.Size(135, 31);
			this.btnResetFirstPulseTimeLimit.TabIndex = 150;
			this.btnResetFirstPulseTimeLimit.Text = "Reset Time Limit";
			this.toolTip1.SetToolTip(this.btnResetFirstPulseTimeLimit, " Sets the Pulse Time Limit");
			this.btnResetFirstPulseTimeLimit.UseVisualStyleBackColor = false;
			this.btnResetFirstPulseTimeLimit.Click += new System.EventHandler(this.btnResetFirstPulseTimeLimit_Click_1);
			// 
			// btnSetFirstPulseTimeLimit
			// 
			this.btnSetFirstPulseTimeLimit.BackColor = System.Drawing.Color.LightSlateGray;
			this.btnSetFirstPulseTimeLimit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnSetFirstPulseTimeLimit.ForeColor = System.Drawing.Color.White;
			this.btnSetFirstPulseTimeLimit.Location = new System.Drawing.Point(210, 82);
			this.btnSetFirstPulseTimeLimit.Name = "btnSetFirstPulseTimeLimit";
			this.btnSetFirstPulseTimeLimit.Size = new System.Drawing.Size(123, 31);
			this.btnSetFirstPulseTimeLimit.TabIndex = 149;
			this.btnSetFirstPulseTimeLimit.Text = "Set Time Limit";
			this.toolTip1.SetToolTip(this.btnSetFirstPulseTimeLimit, "Sets the Pulse Time Limit");
			this.btnSetFirstPulseTimeLimit.UseVisualStyleBackColor = false;
			this.btnSetFirstPulseTimeLimit.Click += new System.EventHandler(this.btnSetFirstPulseTimeLimit_Click_1);
			// 
			// chkPulseEnableLatch
			// 
			this.chkPulseEnableLatch.AutoSize = true;
			this.chkPulseEnableLatch.BackColor = System.Drawing.Color.LightSlateGray;
			this.chkPulseEnableLatch.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chkPulseEnableLatch.ForeColor = System.Drawing.Color.White;
			this.chkPulseEnableLatch.Location = new System.Drawing.Point(426, 5);
			this.chkPulseEnableLatch.Name = "chkPulseEnableLatch";
			this.chkPulseEnableLatch.Size = new System.Drawing.Size(98, 19);
			this.chkPulseEnableLatch.TabIndex = 148;
			this.chkPulseEnableLatch.Text = "Enable Latch";
			this.chkPulseEnableLatch.UseVisualStyleBackColor = false;
			this.chkPulseEnableLatch.CheckedChanged += new System.EventHandler(this.chkPulseEnableLatch_CheckedChanged_1);
			// 
			// chkPulseEnableXSP
			// 
			this.chkPulseEnableXSP.AutoSize = true;
			this.chkPulseEnableXSP.BackColor = System.Drawing.Color.LightSlateGray;
			this.chkPulseEnableXSP.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chkPulseEnableXSP.ForeColor = System.Drawing.Color.White;
			this.chkPulseEnableXSP.Location = new System.Drawing.Point(6, 5);
			this.chkPulseEnableXSP.Name = "chkPulseEnableXSP";
			this.chkPulseEnableXSP.Size = new System.Drawing.Size(95, 19);
			this.chkPulseEnableXSP.TabIndex = 121;
			this.chkPulseEnableXSP.Text = "Enable X SP";
			this.chkPulseEnableXSP.UseVisualStyleBackColor = false;
			this.chkPulseEnableXSP.CheckedChanged += new System.EventHandler(this.chkPulseEnableXSP_CheckedChanged_1);
			// 
			// chkPulseEnableYSP
			// 
			this.chkPulseEnableYSP.AutoSize = true;
			this.chkPulseEnableYSP.BackColor = System.Drawing.Color.LightSlateGray;
			this.chkPulseEnableYSP.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chkPulseEnableYSP.ForeColor = System.Drawing.Color.White;
			this.chkPulseEnableYSP.Location = new System.Drawing.Point(103, 6);
			this.chkPulseEnableYSP.Name = "chkPulseEnableYSP";
			this.chkPulseEnableYSP.Size = new System.Drawing.Size(94, 19);
			this.chkPulseEnableYSP.TabIndex = 122;
			this.chkPulseEnableYSP.Text = "Enable Y SP";
			this.chkPulseEnableYSP.UseVisualStyleBackColor = false;
			this.chkPulseEnableYSP.CheckedChanged += new System.EventHandler(this.chkPulseEnableYSP_CheckedChanged_1);
			// 
			// chkPulseEnableZSP
			// 
			this.chkPulseEnableZSP.AutoSize = true;
			this.chkPulseEnableZSP.BackColor = System.Drawing.Color.LightSlateGray;
			this.chkPulseEnableZSP.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chkPulseEnableZSP.ForeColor = System.Drawing.Color.White;
			this.chkPulseEnableZSP.Location = new System.Drawing.Point(202, 6);
			this.chkPulseEnableZSP.Name = "chkPulseEnableZSP";
			this.chkPulseEnableZSP.Size = new System.Drawing.Size(94, 19);
			this.chkPulseEnableZSP.TabIndex = 123;
			this.chkPulseEnableZSP.Text = "Enable Z SP";
			this.chkPulseEnableZSP.UseVisualStyleBackColor = false;
			this.chkPulseEnableZSP.CheckedChanged += new System.EventHandler(this.chkPulseEnableZSP_CheckedChanged_1);
			// 
			// tbFirstPulseTimeLimit
			// 
			this.tbFirstPulseTimeLimit.BackColor = System.Drawing.Color.LightSlateGray;
			this.tbFirstPulseTimeLimit.Location = new System.Drawing.Point(187, 39);
			this.tbFirstPulseTimeLimit.Maximum = 255;
			this.tbFirstPulseTimeLimit.Name = "tbFirstPulseTimeLimit";
			this.tbFirstPulseTimeLimit.Size = new System.Drawing.Size(337, 45);
			this.tbFirstPulseTimeLimit.TabIndex = 144;
			this.tbFirstPulseTimeLimit.Scroll += new System.EventHandler(this.tbFirstPulseTimeLimit_Scroll_1);
			// 
			// lblFirstPulseTimeLimitms
			// 
			this.lblFirstPulseTimeLimitms.AutoSize = true;
			this.lblFirstPulseTimeLimitms.BackColor = System.Drawing.Color.LightSlateGray;
			this.lblFirstPulseTimeLimitms.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblFirstPulseTimeLimitms.ForeColor = System.Drawing.Color.White;
			this.lblFirstPulseTimeLimitms.Location = new System.Drawing.Point(167, 44);
			this.lblFirstPulseTimeLimitms.Name = "lblFirstPulseTimeLimitms";
			this.lblFirstPulseTimeLimitms.Size = new System.Drawing.Size(24, 15);
			this.lblFirstPulseTimeLimitms.TabIndex = 147;
			this.lblFirstPulseTimeLimitms.Text = "ms";
			// 
			// lblFirstTimeLimitVal
			// 
			this.lblFirstTimeLimitVal.AutoSize = true;
			this.lblFirstTimeLimitVal.BackColor = System.Drawing.Color.LightSlateGray;
			this.lblFirstTimeLimitVal.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblFirstTimeLimitVal.ForeColor = System.Drawing.Color.White;
			this.lblFirstTimeLimitVal.Location = new System.Drawing.Point(118, 45);
			this.lblFirstTimeLimitVal.Name = "lblFirstTimeLimitVal";
			this.lblFirstTimeLimitVal.Size = new System.Drawing.Size(14, 15);
			this.lblFirstTimeLimitVal.TabIndex = 146;
			this.lblFirstTimeLimitVal.Text = "0";
			// 
			// lblFirstPulseTimeLimit
			// 
			this.lblFirstPulseTimeLimit.AutoSize = true;
			this.lblFirstPulseTimeLimit.BackColor = System.Drawing.Color.LightSlateGray;
			this.lblFirstPulseTimeLimit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblFirstPulseTimeLimit.ForeColor = System.Drawing.Color.White;
			this.lblFirstPulseTimeLimit.Location = new System.Drawing.Point(6, 45);
			this.lblFirstPulseTimeLimit.Name = "lblFirstPulseTimeLimit";
			this.lblFirstPulseTimeLimit.Size = new System.Drawing.Size(99, 15);
			this.lblFirstPulseTimeLimit.TabIndex = 145;
			this.lblFirstPulseTimeLimit.Text = "Pulse Time Limit";
			this.toolTip1.SetToolTip(this.lblFirstPulseTimeLimit, "This value should be greater than 2ms to make sense");
			// 
			// p11
			// 
			this.p11.BackColor = System.Drawing.Color.LightSlateGray;
			this.p11.Controls.Add(this.label15);
			this.p11.Controls.Add(this.ledPulseDouble);
			this.p11.Controls.Add(this.lblPPolZ);
			this.p11.Controls.Add(this.lblPPolY);
			this.p11.Controls.Add(this.lblPPolX);
			this.p11.Controls.Add(this.label255);
			this.p11.Controls.Add(this.label256);
			this.p11.Controls.Add(this.ledPulseEA);
			this.p11.Controls.Add(this.label258);
			this.p11.Controls.Add(this.ledPZ);
			this.p11.Controls.Add(this.ledPX);
			this.p11.Controls.Add(this.label268);
			this.p11.Controls.Add(this.label287);
			this.p11.Controls.Add(this.ledPY);
			this.p11.Enabled = false;
			this.p11.Location = new System.Drawing.Point(585, 132);
			this.p11.Name = "p11";
			this.p11.Size = new System.Drawing.Size(439, 375);
			this.p11.TabIndex = 114;
			// 
			// label15
			// 
			this.label15.AutoSize = true;
			this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label15.ForeColor = System.Drawing.Color.White;
			this.label15.Location = new System.Drawing.Point(111, 63);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(101, 20);
			this.label15.TabIndex = 173;
			this.label15.Text = "Double Tap";
			// 
			// ledPulseDouble
			// 
			this.ledPulseDouble.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ledPulseDouble.ForeColor = System.Drawing.Color.White;
			this.ledPulseDouble.LedStyle = NationalInstruments.UI.LedStyle.Round3D;
			this.ledPulseDouble.Location = new System.Drawing.Point(223, 57);
			this.ledPulseDouble.Name = "ledPulseDouble";
			this.ledPulseDouble.OffColor = System.Drawing.Color.Red;
			this.ledPulseDouble.Size = new System.Drawing.Size(40, 40);
			this.ledPulseDouble.TabIndex = 172;
			// 
			// lblPPolZ
			// 
			this.lblPPolZ.AutoSize = true;
			this.lblPPolZ.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblPPolZ.ForeColor = System.Drawing.Color.White;
			this.lblPPolZ.Location = new System.Drawing.Point(277, 237);
			this.lblPPolZ.Name = "lblPPolZ";
			this.lblPPolZ.Size = new System.Drawing.Size(97, 20);
			this.lblPPolZ.TabIndex = 171;
			this.lblPPolZ.Text = "Direction Z";
			// 
			// lblPPolY
			// 
			this.lblPPolY.AutoSize = true;
			this.lblPPolY.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblPPolY.ForeColor = System.Drawing.Color.White;
			this.lblPPolY.Location = new System.Drawing.Point(277, 176);
			this.lblPPolY.Name = "lblPPolY";
			this.lblPPolY.Size = new System.Drawing.Size(98, 20);
			this.lblPPolY.TabIndex = 170;
			this.lblPPolY.Text = "Direction Y";
			// 
			// lblPPolX
			// 
			this.lblPPolX.AutoSize = true;
			this.lblPPolX.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblPPolX.ForeColor = System.Drawing.Color.White;
			this.lblPPolX.Location = new System.Drawing.Point(277, 114);
			this.lblPPolX.Name = "lblPPolX";
			this.lblPPolX.Size = new System.Drawing.Size(98, 20);
			this.lblPPolX.TabIndex = 169;
			this.lblPPolX.Text = "Direction X";
			// 
			// label255
			// 
			this.label255.AutoSize = true;
			this.label255.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label255.ForeColor = System.Drawing.Color.White;
			this.label255.Location = new System.Drawing.Point(60, 17);
			this.label255.Name = "label255";
			this.label255.Size = new System.Drawing.Size(373, 25);
			this.label255.TabIndex = 168;
			this.label255.Text = "Single Tap and Double Tap Status";
			// 
			// label256
			// 
			this.label256.AutoSize = true;
			this.label256.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label256.ForeColor = System.Drawing.Color.White;
			this.label256.Location = new System.Drawing.Point(96, 313);
			this.label256.Name = "label256";
			this.label256.Size = new System.Drawing.Size(134, 20);
			this.label256.TabIndex = 165;
			this.label256.Text = "Event Detected";
			// 
			// ledPulseEA
			// 
			this.ledPulseEA.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ledPulseEA.ForeColor = System.Drawing.Color.White;
			this.ledPulseEA.LedStyle = NationalInstruments.UI.LedStyle.Round3D;
			this.ledPulseEA.Location = new System.Drawing.Point(247, 289);
			this.ledPulseEA.Name = "ledPulseEA";
			this.ledPulseEA.OffColor = System.Drawing.Color.Red;
			this.ledPulseEA.Size = new System.Drawing.Size(60, 60);
			this.ledPulseEA.TabIndex = 164;
			// 
			// label258
			// 
			this.label258.AutoSize = true;
			this.label258.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label258.ForeColor = System.Drawing.Color.White;
			this.label258.Location = new System.Drawing.Point(111, 239);
			this.label258.Name = "label258";
			this.label258.Size = new System.Drawing.Size(99, 20);
			this.label258.TabIndex = 167;
			this.label258.Text = "Z Detected";
			// 
			// ledPZ
			// 
			this.ledPZ.BlinkMode = NationalInstruments.UI.LedBlinkMode.BlinkWhenOn;
			this.ledPZ.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ledPZ.ForeColor = System.Drawing.Color.White;
			this.ledPZ.LedStyle = NationalInstruments.UI.LedStyle.Round3D;
			this.ledPZ.Location = new System.Drawing.Point(223, 227);
			this.ledPZ.Name = "ledPZ";
			this.ledPZ.OffColor = System.Drawing.Color.Red;
			this.ledPZ.Size = new System.Drawing.Size(40, 40);
			this.ledPZ.TabIndex = 166;
			// 
			// ledPX
			// 
			this.ledPX.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ledPX.ForeColor = System.Drawing.Color.White;
			this.ledPX.LedStyle = NationalInstruments.UI.LedStyle.Round3D;
			this.ledPX.Location = new System.Drawing.Point(223, 106);
			this.ledPX.Name = "ledPX";
			this.ledPX.OffColor = System.Drawing.Color.Red;
			this.ledPX.Size = new System.Drawing.Size(40, 40);
			this.ledPX.TabIndex = 119;
			// 
			// label268
			// 
			this.label268.AutoSize = true;
			this.label268.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label268.ForeColor = System.Drawing.Color.White;
			this.label268.Location = new System.Drawing.Point(111, 178);
			this.label268.Name = "label268";
			this.label268.Size = new System.Drawing.Size(100, 20);
			this.label268.TabIndex = 165;
			this.label268.Text = "Y Detected";
			// 
			// label287
			// 
			this.label287.AutoSize = true;
			this.label287.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label287.ForeColor = System.Drawing.Color.White;
			this.label287.Location = new System.Drawing.Point(111, 116);
			this.label287.Name = "label287";
			this.label287.Size = new System.Drawing.Size(100, 20);
			this.label287.TabIndex = 163;
			this.label287.Text = "X Detected";
			// 
			// ledPY
			// 
			this.ledPY.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ledPY.ForeColor = System.Drawing.Color.White;
			this.ledPY.LedStyle = NationalInstruments.UI.LedStyle.Round3D;
			this.ledPY.Location = new System.Drawing.Point(223, 166);
			this.ledPY.Name = "ledPY";
			this.ledPY.OffColor = System.Drawing.Color.Red;
			this.ledPY.Size = new System.Drawing.Size(40, 40);
			this.ledPY.TabIndex = 164;
			// 
			// tbPulseXThreshold
			// 
			this.tbPulseXThreshold.BackColor = System.Drawing.Color.LightSlateGray;
			this.tbPulseXThreshold.Location = new System.Drawing.Point(152, 156);
			this.tbPulseXThreshold.Maximum = 127;
			this.tbPulseXThreshold.Name = "tbPulseXThreshold";
			this.tbPulseXThreshold.Size = new System.Drawing.Size(372, 45);
			this.tbPulseXThreshold.TabIndex = 132;
			this.tbPulseXThreshold.Scroll += new System.EventHandler(this.tbPulseXThreshold_Scroll_1);
			// 
			// lblPulseYThreshold
			// 
			this.lblPulseYThreshold.AutoSize = true;
			this.lblPulseYThreshold.BackColor = System.Drawing.Color.LightSlateGray;
			this.lblPulseYThreshold.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblPulseYThreshold.ForeColor = System.Drawing.Color.White;
			this.lblPulseYThreshold.Location = new System.Drawing.Point(6, 208);
			this.lblPulseYThreshold.Name = "lblPulseYThreshold";
			this.lblPulseYThreshold.Size = new System.Drawing.Size(87, 15);
			this.lblPulseYThreshold.TabIndex = 137;
			this.lblPulseYThreshold.Text = " Y Threshold";
			// 
			// tbPulseYThreshold
			// 
			this.tbPulseYThreshold.BackColor = System.Drawing.Color.LightSlateGray;
			this.tbPulseYThreshold.Location = new System.Drawing.Point(152, 204);
			this.tbPulseYThreshold.Maximum = 127;
			this.tbPulseYThreshold.Name = "tbPulseYThreshold";
			this.tbPulseYThreshold.Size = new System.Drawing.Size(372, 45);
			this.tbPulseYThreshold.TabIndex = 136;
			this.tbPulseYThreshold.Scroll += new System.EventHandler(this.tbPulseYThreshold_Scroll_1);
			// 
			// lblPulseYThresholdVal
			// 
			this.lblPulseYThresholdVal.AutoSize = true;
			this.lblPulseYThresholdVal.BackColor = System.Drawing.Color.LightSlateGray;
			this.lblPulseYThresholdVal.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblPulseYThresholdVal.ForeColor = System.Drawing.Color.White;
			this.lblPulseYThresholdVal.Location = new System.Drawing.Point(99, 208);
			this.lblPulseYThresholdVal.Name = "lblPulseYThresholdVal";
			this.lblPulseYThresholdVal.Size = new System.Drawing.Size(15, 15);
			this.lblPulseYThresholdVal.TabIndex = 138;
			this.lblPulseYThresholdVal.Text = "0";
			// 
			// lblPulseXThresholdg
			// 
			this.lblPulseXThresholdg.AutoSize = true;
			this.lblPulseXThresholdg.BackColor = System.Drawing.Color.LightSlateGray;
			this.lblPulseXThresholdg.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblPulseXThresholdg.ForeColor = System.Drawing.Color.White;
			this.lblPulseXThresholdg.Location = new System.Drawing.Point(142, 160);
			this.lblPulseXThresholdg.Name = "lblPulseXThresholdg";
			this.lblPulseXThresholdg.Size = new System.Drawing.Size(15, 15);
			this.lblPulseXThresholdg.TabIndex = 135;
			this.lblPulseXThresholdg.Text = "g";
			// 
			// lblPulseXThresholdVal
			// 
			this.lblPulseXThresholdVal.AutoSize = true;
			this.lblPulseXThresholdVal.BackColor = System.Drawing.Color.LightSlateGray;
			this.lblPulseXThresholdVal.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblPulseXThresholdVal.ForeColor = System.Drawing.Color.White;
			this.lblPulseXThresholdVal.Location = new System.Drawing.Point(99, 162);
			this.lblPulseXThresholdVal.Name = "lblPulseXThresholdVal";
			this.lblPulseXThresholdVal.Size = new System.Drawing.Size(15, 15);
			this.lblPulseXThresholdVal.TabIndex = 134;
			this.lblPulseXThresholdVal.Text = "0";
			// 
			// lblPulseZThresholdg
			// 
			this.lblPulseZThresholdg.AutoSize = true;
			this.lblPulseZThresholdg.BackColor = System.Drawing.Color.LightSlateGray;
			this.lblPulseZThresholdg.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblPulseZThresholdg.ForeColor = System.Drawing.Color.White;
			this.lblPulseZThresholdg.Location = new System.Drawing.Point(142, 251);
			this.lblPulseZThresholdg.Name = "lblPulseZThresholdg";
			this.lblPulseZThresholdg.Size = new System.Drawing.Size(15, 15);
			this.lblPulseZThresholdg.TabIndex = 143;
			this.lblPulseZThresholdg.Text = "g";
			// 
			// lblPulseZThreshold
			// 
			this.lblPulseZThreshold.AutoSize = true;
			this.lblPulseZThreshold.BackColor = System.Drawing.Color.LightSlateGray;
			this.lblPulseZThreshold.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblPulseZThreshold.ForeColor = System.Drawing.Color.White;
			this.lblPulseZThreshold.Location = new System.Drawing.Point(6, 253);
			this.lblPulseZThreshold.Name = "lblPulseZThreshold";
			this.lblPulseZThreshold.Size = new System.Drawing.Size(87, 15);
			this.lblPulseZThreshold.TabIndex = 141;
			this.lblPulseZThreshold.Text = " Z Threshold";
			// 
			// lblPulseZThresholdVal
			// 
			this.lblPulseZThresholdVal.AutoSize = true;
			this.lblPulseZThresholdVal.BackColor = System.Drawing.Color.LightSlateGray;
			this.lblPulseZThresholdVal.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblPulseZThresholdVal.ForeColor = System.Drawing.Color.White;
			this.lblPulseZThresholdVal.Location = new System.Drawing.Point(99, 253);
			this.lblPulseZThresholdVal.Name = "lblPulseZThresholdVal";
			this.lblPulseZThresholdVal.Size = new System.Drawing.Size(15, 15);
			this.lblPulseZThresholdVal.TabIndex = 142;
			this.lblPulseZThresholdVal.Text = "0";
			// 
			// lblPulseYThresholdg
			// 
			this.lblPulseYThresholdg.AutoSize = true;
			this.lblPulseYThresholdg.BackColor = System.Drawing.Color.LightSlateGray;
			this.lblPulseYThresholdg.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblPulseYThresholdg.ForeColor = System.Drawing.Color.White;
			this.lblPulseYThresholdg.Location = new System.Drawing.Point(142, 206);
			this.lblPulseYThresholdg.Name = "lblPulseYThresholdg";
			this.lblPulseYThresholdg.Size = new System.Drawing.Size(15, 15);
			this.lblPulseYThresholdg.TabIndex = 139;
			this.lblPulseYThresholdg.Text = "g";
			// 
			// lblPulseXThreshold
			// 
			this.lblPulseXThreshold.AutoSize = true;
			this.lblPulseXThreshold.BackColor = System.Drawing.Color.LightSlateGray;
			this.lblPulseXThreshold.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblPulseXThreshold.ForeColor = System.Drawing.Color.White;
			this.lblPulseXThreshold.Location = new System.Drawing.Point(6, 162);
			this.lblPulseXThreshold.Name = "lblPulseXThreshold";
			this.lblPulseXThreshold.Size = new System.Drawing.Size(88, 15);
			this.lblPulseXThreshold.TabIndex = 133;
			this.lblPulseXThreshold.Text = " X Threshold";
			// 
			// FIFOPage
			// 
			this.FIFOPage.BackColor = System.Drawing.Color.LightSlateGray;
			this.FIFOPage.Controls.Add(this.uxCopyFifoData);
			this.FIFOPage.Controls.Add(this.uxClearFifoDataList);
			this.FIFOPage.Controls.Add(this.uxFifoList);
			this.FIFOPage.Controls.Add(this.rdoFIFO14bitDataDisplay);
			this.FIFOPage.Controls.Add(this.rdoFIFO8bitDataDisplay);
			this.FIFOPage.Controls.Add(this.gb3bF);
			this.FIFOPage.Location = new System.Drawing.Point(4, 25);
			this.FIFOPage.Name = "FIFOPage";
			this.FIFOPage.Padding = new System.Windows.Forms.Padding(3);
			this.FIFOPage.Size = new System.Drawing.Size(1124, 525);
			this.FIFOPage.TabIndex = 7;
			this.FIFOPage.Text = "FIFO";
			// 
			// uxCopyFifoData
			// 
			this.uxCopyFifoData.BackColor = System.Drawing.Color.LightSlateGray;
			this.uxCopyFifoData.Location = new System.Drawing.Point(782, 463);
			this.uxCopyFifoData.Name = "uxCopyFifoData";
			this.uxCopyFifoData.Size = new System.Drawing.Size(148, 42);
			this.uxCopyFifoData.TabIndex = 224;
			this.uxCopyFifoData.Text = "Copy to Clipboard";
			this.uxCopyFifoData.UseVisualStyleBackColor = false;
			this.uxCopyFifoData.Click += new System.EventHandler(this.uxCopyFifoData_Click);
			// 
			// uxClearFifoDataList
			// 
			this.uxClearFifoDataList.BackColor = System.Drawing.Color.LightSlateGray;
			this.uxClearFifoDataList.Location = new System.Drawing.Point(700, 463);
			this.uxClearFifoDataList.Name = "uxClearFifoDataList";
			this.uxClearFifoDataList.Size = new System.Drawing.Size(78, 42);
			this.uxClearFifoDataList.TabIndex = 223;
			this.uxClearFifoDataList.Text = "Clear";
			this.toolTip1.SetToolTip(this.uxClearFifoDataList, "Clear FIFO data list");
			this.uxClearFifoDataList.UseVisualStyleBackColor = false;
			this.uxClearFifoDataList.Click += new System.EventHandler(this.uxClearFifoDataList_Click);
			// 
			// uxFifoList
			// 
			this.uxFifoList.BackColor = System.Drawing.Color.Black;
			this.uxFifoList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.uxXFifo,
            this.uxYFifo,
            this.uxZFifo});
			this.uxFifoList.ForeColor = System.Drawing.Color.SandyBrown;
			this.uxFifoList.Location = new System.Drawing.Point(698, 37);
			this.uxFifoList.Name = "uxFifoList";
			this.uxFifoList.Size = new System.Drawing.Size(292, 416);
			this.uxFifoList.TabIndex = 222;
			this.uxFifoList.UseCompatibleStateImageBehavior = false;
			this.uxFifoList.View = System.Windows.Forms.View.Details;
			// 
			// uxXFifo
			// 
			this.uxXFifo.Text = "X";
			this.uxXFifo.Width = 98;
			// 
			// uxYFifo
			// 
			this.uxYFifo.Text = "Y";
			this.uxYFifo.Width = 84;
			// 
			// uxZFifo
			// 
			this.uxZFifo.Text = "Z";
			this.uxZFifo.Width = 105;
			// 
			// rdoFIFO14bitDataDisplay
			// 
			this.rdoFIFO14bitDataDisplay.AutoSize = true;
			this.rdoFIFO14bitDataDisplay.BackColor = System.Drawing.Color.LightSlateGray;
			this.rdoFIFO14bitDataDisplay.Checked = true;
			this.rdoFIFO14bitDataDisplay.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.rdoFIFO14bitDataDisplay.ForeColor = System.Drawing.Color.Black;
			this.rdoFIFO14bitDataDisplay.Location = new System.Drawing.Point(849, 11);
			this.rdoFIFO14bitDataDisplay.Name = "rdoFIFO14bitDataDisplay";
			this.rdoFIFO14bitDataDisplay.Size = new System.Drawing.Size(160, 24);
			this.rdoFIFO14bitDataDisplay.TabIndex = 220;
			this.rdoFIFO14bitDataDisplay.TabStop = true;
			this.rdoFIFO14bitDataDisplay.Text = "View 14-bit Data";
			this.rdoFIFO14bitDataDisplay.UseVisualStyleBackColor = false;
			this.rdoFIFO14bitDataDisplay.CheckedChanged += new System.EventHandler(this.rdoFIFO14bitDataDisplay_CheckedChanged);
			// 
			// rdoFIFO8bitDataDisplay
			// 
			this.rdoFIFO8bitDataDisplay.AutoSize = true;
			this.rdoFIFO8bitDataDisplay.BackColor = System.Drawing.Color.LightSlateGray;
			this.rdoFIFO8bitDataDisplay.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.rdoFIFO8bitDataDisplay.ForeColor = System.Drawing.Color.Black;
			this.rdoFIFO8bitDataDisplay.Location = new System.Drawing.Point(698, 10);
			this.rdoFIFO8bitDataDisplay.Name = "rdoFIFO8bitDataDisplay";
			this.rdoFIFO8bitDataDisplay.Size = new System.Drawing.Size(144, 24);
			this.rdoFIFO8bitDataDisplay.TabIndex = 219;
			this.rdoFIFO8bitDataDisplay.Text = "View 8bit Data";
			this.rdoFIFO8bitDataDisplay.UseVisualStyleBackColor = false;
			this.rdoFIFO8bitDataDisplay.CheckedChanged += new System.EventHandler(this.rdoFIFO8bitDataDisplay_CheckedChanged);
			// 
			// gb3bF
			// 
			this.gb3bF.BackColor = System.Drawing.Color.LightSlateGray;
			this.gb3bF.Controls.Add(this.rdoDisabled);
			this.gb3bF.Controls.Add(this.p5);
			this.gb3bF.Controls.Add(this.p4);
			this.gb3bF.Controls.Add(this.rdoTriggerMode);
			this.gb3bF.Controls.Add(this.chkDisableFIFO);
			this.gb3bF.Controls.Add(this.rdoFill);
			this.gb3bF.Controls.Add(this.rdoCircular);
			this.gb3bF.Controls.Add(this.btnSetMode);
			this.gb3bF.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.gb3bF.ForeColor = System.Drawing.Color.White;
			this.gb3bF.Location = new System.Drawing.Point(3, 6);
			this.gb3bF.Name = "gb3bF";
			this.gb3bF.Size = new System.Drawing.Size(676, 560);
			this.gb3bF.TabIndex = 129;
			this.gb3bF.TabStop = false;
			this.gb3bF.Text = "32 Sample FIFO";
			// 
			// rdoDisabled
			// 
			this.rdoDisabled.AutoSize = true;
			this.rdoDisabled.Checked = true;
			this.rdoDisabled.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.rdoDisabled.Location = new System.Drawing.Point(34, 73);
			this.rdoDisabled.Name = "rdoDisabled";
			this.rdoDisabled.Size = new System.Drawing.Size(97, 24);
			this.rdoDisabled.TabIndex = 219;
			this.rdoDisabled.TabStop = true;
			this.rdoDisabled.Text = "Disabled";
			this.toolTip1.SetToolTip(this.rdoDisabled, "Allows samples to hold up to 32 values for X,Y and Z");
			this.rdoDisabled.UseVisualStyleBackColor = true;
			this.rdoDisabled.Click += new System.EventHandler(this.rdoDisabled_CheckedChanged);
			// 
			// p5
			// 
			this.p5.BackColor = System.Drawing.Color.LightSteelBlue;
			this.p5.Controls.Add(this.label19);
			this.p5.Controls.Add(this.label17);
			this.p5.Controls.Add(this.label14);
			this.p5.Controls.Add(this.label13);
			this.p5.Controls.Add(this.ledTrigMFF);
			this.p5.Controls.Add(this.ledTrigTap);
			this.p5.Controls.Add(this.ledTrigLP);
			this.p5.Controls.Add(this.ledTrigTrans);
			this.p5.Controls.Add(this.button1);
			this.p5.Controls.Add(this.lblCurrent_FIFO_Count);
			this.p5.Controls.Add(this.lblF_Count);
			this.p5.Controls.Add(this.ledOverFlow);
			this.p5.Controls.Add(this.ledWatermark);
			this.p5.Controls.Add(this.label64);
			this.p5.Controls.Add(this.label63);
			this.p5.Enabled = false;
			this.p5.ForeColor = System.Drawing.Color.Black;
			this.p5.Location = new System.Drawing.Point(3, 362);
			this.p5.Name = "p5";
			this.p5.Size = new System.Drawing.Size(665, 152);
			this.p5.TabIndex = 218;
			// 
			// label19
			// 
			this.label19.AutoSize = true;
			this.label19.Location = new System.Drawing.Point(502, 8);
			this.label19.Name = "label19";
			this.label19.Size = new System.Drawing.Size(53, 24);
			this.label19.TabIndex = 164;
			this.label19.Text = "MFF";
			// 
			// label17
			// 
			this.label17.AutoSize = true;
			this.label17.Location = new System.Drawing.Point(371, 8);
			this.label17.Name = "label17";
			this.label17.Size = new System.Drawing.Size(46, 24);
			this.label17.TabIndex = 163;
			this.label17.Text = "Tap";
			// 
			// label14
			// 
			this.label14.AutoSize = true;
			this.label14.Location = new System.Drawing.Point(248, 8);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(34, 24);
			this.label14.TabIndex = 162;
			this.label14.Text = "LP";
			// 
			// label13
			// 
			this.label13.AutoSize = true;
			this.label13.Location = new System.Drawing.Point(78, 8);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(97, 24);
			this.label13.TabIndex = 161;
			this.label13.Text = "Transient";
			// 
			// ledTrigMFF
			// 
			this.ledTrigMFF.LedStyle = NationalInstruments.UI.LedStyle.Round3D;
			this.ledTrigMFF.Location = new System.Drawing.Point(474, 3);
			this.ledTrigMFF.Name = "ledTrigMFF";
			this.ledTrigMFF.OffColor = System.Drawing.Color.Red;
			this.ledTrigMFF.Size = new System.Drawing.Size(30, 30);
			this.ledTrigMFF.TabIndex = 159;
			// 
			// ledTrigTap
			// 
			this.ledTrigTap.LedStyle = NationalInstruments.UI.LedStyle.Round3D;
			this.ledTrigTap.Location = new System.Drawing.Point(340, 3);
			this.ledTrigTap.Name = "ledTrigTap";
			this.ledTrigTap.OffColor = System.Drawing.Color.Red;
			this.ledTrigTap.Size = new System.Drawing.Size(30, 30);
			this.ledTrigTap.TabIndex = 160;
			// 
			// ledTrigLP
			// 
			this.ledTrigLP.LedStyle = NationalInstruments.UI.LedStyle.Round3D;
			this.ledTrigLP.Location = new System.Drawing.Point(217, 3);
			this.ledTrigLP.Name = "ledTrigLP";
			this.ledTrigLP.OffColor = System.Drawing.Color.Red;
			this.ledTrigLP.Size = new System.Drawing.Size(30, 30);
			this.ledTrigLP.TabIndex = 157;
			// 
			// ledTrigTrans
			// 
			this.ledTrigTrans.LedStyle = NationalInstruments.UI.LedStyle.Round3D;
			this.ledTrigTrans.Location = new System.Drawing.Point(41, 3);
			this.ledTrigTrans.Name = "ledTrigTrans";
			this.ledTrigTrans.OffColor = System.Drawing.Color.Red;
			this.ledTrigTrans.Size = new System.Drawing.Size(30, 30);
			this.ledTrigTrans.TabIndex = 158;
			// 
			// button1
			// 
			this.button1.BackColor = System.Drawing.Color.LimeGreen;
			this.button1.Location = new System.Drawing.Point(516, 64);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(108, 34);
			this.button1.TabIndex = 51;
			this.button1.Text = "Dump";
			this.button1.UseVisualStyleBackColor = false;
			this.button1.Visible = false;
			// 
			// lblCurrent_FIFO_Count
			// 
			this.lblCurrent_FIFO_Count.AutoSize = true;
			this.lblCurrent_FIFO_Count.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblCurrent_FIFO_Count.Location = new System.Drawing.Point(260, 69);
			this.lblCurrent_FIFO_Count.Name = "lblCurrent_FIFO_Count";
			this.lblCurrent_FIFO_Count.Size = new System.Drawing.Size(193, 24);
			this.lblCurrent_FIFO_Count.TabIndex = 48;
			this.lblCurrent_FIFO_Count.Text = "Current FIFO Count";
			// 
			// lblF_Count
			// 
			this.lblF_Count.AutoSize = true;
			this.lblF_Count.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblF_Count.Location = new System.Drawing.Point(467, 69);
			this.lblF_Count.Name = "lblF_Count";
			this.lblF_Count.Size = new System.Drawing.Size(21, 24);
			this.lblF_Count.TabIndex = 49;
			this.lblF_Count.Text = "0";
			// 
			// ledOverFlow
			// 
			this.ledOverFlow.LedStyle = NationalInstruments.UI.LedStyle.Round3D;
			this.ledOverFlow.Location = new System.Drawing.Point(155, 109);
			this.ledOverFlow.Name = "ledOverFlow";
			this.ledOverFlow.OffColor = System.Drawing.Color.Red;
			this.ledOverFlow.Size = new System.Drawing.Size(40, 40);
			this.ledOverFlow.TabIndex = 44;
			// 
			// ledWatermark
			// 
			this.ledWatermark.LedStyle = NationalInstruments.UI.LedStyle.Round3D;
			this.ledWatermark.Location = new System.Drawing.Point(599, 107);
			this.ledWatermark.Name = "ledWatermark";
			this.ledWatermark.OffColor = System.Drawing.Color.Red;
			this.ledWatermark.Size = new System.Drawing.Size(40, 40);
			this.ledWatermark.TabIndex = 47;
			// 
			// label64
			// 
			this.label64.AutoSize = true;
			this.label64.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label64.Location = new System.Drawing.Point(14, 113);
			this.label64.Name = "label64";
			this.label64.Size = new System.Drawing.Size(139, 24);
			this.label64.TabIndex = 45;
			this.label64.Text = "Overflow Flag";
			// 
			// label63
			// 
			this.label63.AutoSize = true;
			this.label63.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label63.Location = new System.Drawing.Point(443, 113);
			this.label63.Name = "label63";
			this.label63.Size = new System.Drawing.Size(156, 24);
			this.label63.TabIndex = 46;
			this.label63.Text = "Watermark Flag";
			// 
			// p4
			// 
			this.p4.BackColor = System.Drawing.Color.LightSteelBlue;
			this.p4.Controls.Add(this.gbWatermark);
			this.p4.Controls.Add(this.pTriggerMode);
			this.p4.ForeColor = System.Drawing.Color.Black;
			this.p4.Location = new System.Drawing.Point(3, 112);
			this.p4.Name = "p4";
			this.p4.Size = new System.Drawing.Size(665, 244);
			this.p4.TabIndex = 218;
			// 
			// gbWatermark
			// 
			this.gbWatermark.Controls.Add(this.btnResetWatermark);
			this.gbWatermark.Controls.Add(this.lblWatermarkValue);
			this.gbWatermark.Controls.Add(this.tBWatermark);
			this.gbWatermark.Controls.Add(this.btnWatermark);
			this.gbWatermark.Controls.Add(this.lblWatermark);
			this.gbWatermark.Location = new System.Drawing.Point(31, 55);
			this.gbWatermark.Name = "gbWatermark";
			this.gbWatermark.Size = new System.Drawing.Size(631, 134);
			this.gbWatermark.TabIndex = 211;
			this.gbWatermark.TabStop = false;
			// 
			// btnResetWatermark
			// 
			this.btnResetWatermark.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
			this.btnResetWatermark.Enabled = false;
			this.btnResetWatermark.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnResetWatermark.ForeColor = System.Drawing.Color.Black;
			this.btnResetWatermark.Location = new System.Drawing.Point(511, 12);
			this.btnResetWatermark.Name = "btnResetWatermark";
			this.btnResetWatermark.Size = new System.Drawing.Size(110, 52);
			this.btnResetWatermark.TabIndex = 205;
			this.btnResetWatermark.Text = "Reset Watermark";
			this.btnResetWatermark.UseVisualStyleBackColor = false;
			this.btnResetWatermark.Click += new System.EventHandler(this.btnResetWatermark_Click);
			// 
			// lblWatermarkValue
			// 
			this.lblWatermarkValue.AutoSize = true;
			this.lblWatermarkValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblWatermarkValue.Location = new System.Drawing.Point(138, 88);
			this.lblWatermarkValue.Name = "lblWatermarkValue";
			this.lblWatermarkValue.Size = new System.Drawing.Size(21, 24);
			this.lblWatermarkValue.TabIndex = 50;
			this.lblWatermarkValue.Text = "1";
			this.toolTip1.SetToolTip(this.lblWatermarkValue, "This is the set value for # of Samples in the FIFO to trigger the Flag");
			// 
			// tBWatermark
			// 
			this.tBWatermark.Location = new System.Drawing.Point(162, 88);
			this.tBWatermark.Maximum = 32;
			this.tBWatermark.Minimum = 1;
			this.tBWatermark.Name = "tBWatermark";
			this.tBWatermark.Size = new System.Drawing.Size(459, 45);
			this.tBWatermark.TabIndex = 32;
			this.tBWatermark.TickFrequency = 2;
			this.toolTip1.SetToolTip(this.tBWatermark, "Watermark Value");
			this.tBWatermark.Value = 1;
			this.tBWatermark.Scroll += new System.EventHandler(this.tBWatermark_Scroll);
			// 
			// btnWatermark
			// 
			this.btnWatermark.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
			this.btnWatermark.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnWatermark.ForeColor = System.Drawing.Color.Black;
			this.btnWatermark.Location = new System.Drawing.Point(20, 12);
			this.btnWatermark.Name = "btnWatermark";
			this.btnWatermark.Size = new System.Drawing.Size(110, 52);
			this.btnWatermark.TabIndex = 204;
			this.btnWatermark.Text = "Set Watermark";
			this.btnWatermark.UseVisualStyleBackColor = false;
			this.btnWatermark.Click += new System.EventHandler(this.btnWatermark_Click);
			// 
			// lblWatermark
			// 
			this.lblWatermark.AutoSize = true;
			this.lblWatermark.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblWatermark.Location = new System.Drawing.Point(16, 88);
			this.lblWatermark.Name = "lblWatermark";
			this.lblWatermark.Size = new System.Drawing.Size(121, 24);
			this.lblWatermark.TabIndex = 42;
			this.lblWatermark.Text = "Watermark: ";
			this.toolTip1.SetToolTip(this.lblWatermark, "This is the # of Sample to trigger the flag in the FIFO");
			// 
			// pTriggerMode
			// 
			this.pTriggerMode.Controls.Add(this.chkTriggerMFF);
			this.pTriggerMode.Controls.Add(this.chkTriggerPulse);
			this.pTriggerMode.Controls.Add(this.chkTriggerLP);
			this.pTriggerMode.Controls.Add(this.chkTrigTrans);
			this.pTriggerMode.Location = new System.Drawing.Point(31, 195);
			this.pTriggerMode.Name = "pTriggerMode";
			this.pTriggerMode.Size = new System.Drawing.Size(586, 46);
			this.pTriggerMode.TabIndex = 210;
			// 
			// chkTriggerMFF
			// 
			this.chkTriggerMFF.AutoSize = true;
			this.chkTriggerMFF.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chkTriggerMFF.Location = new System.Drawing.Point(444, 12);
			this.chkTriggerMFF.Name = "chkTriggerMFF";
			this.chkTriggerMFF.Size = new System.Drawing.Size(112, 20);
			this.chkTriggerMFF.TabIndex = 209;
			this.chkTriggerMFF.Text = "Trigger MFF";
			this.chkTriggerMFF.UseVisualStyleBackColor = true;
			this.chkTriggerMFF.CheckedChanged += new System.EventHandler(this.chkTriggerMFF_CheckedChanged);
			// 
			// chkTriggerPulse
			// 
			this.chkTriggerPulse.AutoSize = true;
			this.chkTriggerPulse.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chkTriggerPulse.Location = new System.Drawing.Point(306, 12);
			this.chkTriggerPulse.Name = "chkTriggerPulse";
			this.chkTriggerPulse.Size = new System.Drawing.Size(110, 20);
			this.chkTriggerPulse.TabIndex = 208;
			this.chkTriggerPulse.Text = "Trigger Tap";
			this.chkTriggerPulse.UseVisualStyleBackColor = true;
			this.chkTriggerPulse.CheckedChanged += new System.EventHandler(this.chkTriggerPulse_CheckedChanged);
			// 
			// chkTriggerLP
			// 
			this.chkTriggerLP.AutoSize = true;
			this.chkTriggerLP.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chkTriggerLP.Location = new System.Drawing.Point(182, 12);
			this.chkTriggerLP.Name = "chkTriggerLP";
			this.chkTriggerLP.Size = new System.Drawing.Size(100, 20);
			this.chkTriggerLP.TabIndex = 207;
			this.chkTriggerLP.Text = "Trigger LP";
			this.chkTriggerLP.UseVisualStyleBackColor = true;
			this.chkTriggerLP.CheckedChanged += new System.EventHandler(this.chkTriggerLP_CheckedChanged);
			// 
			// chkTrigTrans
			// 
			this.chkTrigTrans.AutoSize = true;
			this.chkTrigTrans.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chkTrigTrans.Location = new System.Drawing.Point(15, 12);
			this.chkTrigTrans.Name = "chkTrigTrans";
			this.chkTrigTrans.Size = new System.Drawing.Size(147, 20);
			this.chkTrigTrans.TabIndex = 206;
			this.chkTrigTrans.Text = "Trigger Transient";
			this.chkTrigTrans.UseVisualStyleBackColor = true;
			this.chkTrigTrans.CheckedChanged += new System.EventHandler(this.chkTrigTrans_CheckedChanged);
			// 
			// rdoTriggerMode
			// 
			this.rdoTriggerMode.AutoSize = true;
			this.rdoTriggerMode.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.rdoTriggerMode.Location = new System.Drawing.Point(475, 73);
			this.rdoTriggerMode.Name = "rdoTriggerMode";
			this.rdoTriggerMode.Size = new System.Drawing.Size(132, 24);
			this.rdoTriggerMode.TabIndex = 205;
			this.rdoTriggerMode.Text = "Trigger Mode";
			this.toolTip1.SetToolTip(this.rdoTriggerMode, "Allows values to continuously overwrite holding the last 32 values");
			this.rdoTriggerMode.UseVisualStyleBackColor = true;
			this.rdoTriggerMode.Click += new System.EventHandler(this.rdoTriggerMode_CheckedChanged);
			// 
			// chkDisableFIFO
			// 
			this.chkDisableFIFO.AutoSize = true;
			this.chkDisableFIFO.Checked = true;
			this.chkDisableFIFO.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkDisableFIFO.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chkDisableFIFO.ForeColor = System.Drawing.Color.Black;
			this.chkDisableFIFO.Location = new System.Drawing.Point(17, 31);
			this.chkDisableFIFO.Name = "chkDisableFIFO";
			this.chkDisableFIFO.Size = new System.Drawing.Size(151, 28);
			this.chkDisableFIFO.TabIndex = 39;
			this.chkDisableFIFO.Text = "Disable FIFO";
			this.toolTip1.SetToolTip(this.chkDisableFIFO, "Turns on/off the FIFO function");
			this.chkDisableFIFO.UseVisualStyleBackColor = true;
			this.chkDisableFIFO.Visible = false;
			this.chkDisableFIFO.CheckedChanged += new System.EventHandler(this.chkDisableFIFO_CheckedChanged);
			// 
			// rdoFill
			// 
			this.rdoFill.AutoSize = true;
			this.rdoFill.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.rdoFill.Location = new System.Drawing.Point(163, 73);
			this.rdoFill.Name = "rdoFill";
			this.rdoFill.Size = new System.Drawing.Size(105, 24);
			this.rdoFill.TabIndex = 38;
			this.rdoFill.Text = "Fill Buffer";
			this.toolTip1.SetToolTip(this.rdoFill, "Allows samples to hold up to 32 values for X,Y and Z");
			this.rdoFill.UseVisualStyleBackColor = true;
			this.rdoFill.Click += new System.EventHandler(this.rdoFill_CheckedChanged);
			// 
			// rdoCircular
			// 
			this.rdoCircular.AutoSize = true;
			this.rdoCircular.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.rdoCircular.Location = new System.Drawing.Point(300, 73);
			this.rdoCircular.Name = "rdoCircular";
			this.rdoCircular.Size = new System.Drawing.Size(143, 24);
			this.rdoCircular.TabIndex = 37;
			this.rdoCircular.Text = "Circular Buffer";
			this.toolTip1.SetToolTip(this.rdoCircular, "Allows values to continuously overwrite holding the last 32 values");
			this.rdoCircular.UseVisualStyleBackColor = true;
			this.rdoCircular.Click += new System.EventHandler(this.rdoCircular_CheckedChanged);
			// 
			// btnSetMode
			// 
			this.btnSetMode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
			this.btnSetMode.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnSetMode.ForeColor = System.Drawing.Color.Black;
			this.btnSetMode.Location = new System.Drawing.Point(176, 20);
			this.btnSetMode.Name = "btnSetMode";
			this.btnSetMode.Size = new System.Drawing.Size(153, 41);
			this.btnSetMode.TabIndex = 203;
			this.btnSetMode.Text = "Change Mode ";
			this.toolTip1.SetToolTip(this.btnSetMode, "Sets FIFO MODE and Watermark");
			this.btnSetMode.UseVisualStyleBackColor = false;
			this.btnSetMode.Visible = false;
			this.btnSetMode.Click += new System.EventHandler(this.btnSetMode_Click);
			// 
			// TmrActive
			// 
			this.TmrActive.Interval = 1;
			this.TmrActive.Tick += new System.EventHandler(this.TmrActive_Tick);
			// 
			// tmrDataDisplay
			// 
			this.tmrDataDisplay.Interval = 1;
			this.tmrDataDisplay.Tick += new System.EventHandler(this.tmrDataDisplay_Tick);
			// 
			// panel5
			// 
			this.panel5.BackColor = System.Drawing.Color.LightSlateGray;
			this.panel5.Controls.Add(this.pictureBox2);
			this.panel5.Controls.Add(this.gbOM);
			this.panel5.Controls.Add(this.gbDR);
			this.panel5.Location = new System.Drawing.Point(0, 1);
			this.panel5.Name = "panel5";
			this.panel5.Size = new System.Drawing.Size(1130, 141);
			this.panel5.TabIndex = 230;
			// 
			// pictureBox2
			// 
			this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
			this.pictureBox2.Location = new System.Drawing.Point(847, 5);
			this.pictureBox2.Name = "pictureBox2";
			this.pictureBox2.Size = new System.Drawing.Size(282, 68);
			this.pictureBox2.TabIndex = 230;
			this.pictureBox2.TabStop = false;
			// 
			// CommStrip
			// 
			this.CommStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CommStripButton,
            this.toolStripStatusLabel});
			this.CommStrip.Location = new System.Drawing.Point(0, 696);
			this.CommStrip.Name = "CommStrip";
			this.CommStrip.Size = new System.Drawing.Size(1132, 25);
			this.CommStrip.TabIndex = 231;
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
			// 
			// toolStripStatusLabel
			// 
			this.toolStripStatusLabel.BackColor = System.Drawing.SystemColors.ButtonFace;
			this.toolStripStatusLabel.Name = "toolStripStatusLabel";
			this.toolStripStatusLabel.Size = new System.Drawing.Size(284, 20);
			this.toolStripStatusLabel.Text = "COM Port Not Connected, Please Connect";
			// 
			// VeyronEvaluationSoftware8451
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.BackColor = System.Drawing.Color.LightSlateGray;
			this.ClientSize = new System.Drawing.Size(1132, 721);
			this.Controls.Add(this.CommStrip);
			this.Controls.Add(this.panel5);
			this.Controls.Add(this.TabTool);
			this.Controls.Add(this.groupBox6);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.Name = "VeyronEvaluationSoftware8451";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.Text = "MMA8451Q Full System Evaluation Software";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.VeyronEvaluationSoftware_FormClosing);
			this.PL_Page.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.PLImage)).EndInit();
			this.gbOD.ResumeLayout(false);
			this.gbOD.PerformLayout();
			this.pPLActive.ResumeLayout(false);
			this.pPLActive.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.tbPL)).EndInit();
			this.p6.ResumeLayout(false);
			this.p6.PerformLayout();
			this.gbPLDisappear.ResumeLayout(false);
			this.gbPLDisappear.PerformLayout();
			this.p7.ResumeLayout(false);
			this.p7.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.ledCurPLNew)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ledCurPLRight)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ledCurPLLeft)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ledCurPLDown)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ledCurPLUp)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ledCurPLBack)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ledCurPLFront)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ledCurPLLO)).EndInit();
			this.MainScreenEval.ResumeLayout(false);
			this.groupBox8.ResumeLayout(false);
			this.panel3.ResumeLayout(false);
			this.gbOC.ResumeLayout(false);
			this.gbOC.PerformLayout();
			this.groupBox7.ResumeLayout(false);
			this.p14b8bSelect.ResumeLayout(false);
			this.p14b8bSelect.PerformLayout();
			this.groupBox5.ResumeLayout(false);
			this.groupBox5.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.legend1)).EndInit();
			this.gbST.ResumeLayout(false);
			this.gbST.PerformLayout();
			this.gbXD.ResumeLayout(false);
			this.gbXD.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.uxSwitchCounts)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.MainScreenGraph)).EndInit();
			this.uxPanelSettings.ResumeLayout(false);
			this.pSOS.ResumeLayout(false);
			this.pSOS.PerformLayout();
			this.gbASS.ResumeLayout(false);
			this.gbASS.PerformLayout();
			this.pnlAutoSleep.ResumeLayout(false);
			this.pnlAutoSleep.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.tbSleepCounter)).EndInit();
			this.gbIF.ResumeLayout(false);
			this.p9.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.ledTrans1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ledOrient)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ledASleep)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ledMFF1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ledPulse)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ledTrans)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ledDataReady)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ledFIFO)).EndInit();
			this.p8.ResumeLayout(false);
			this.panel18.ResumeLayout(false);
			this.panel18.PerformLayout();
			this.panel12.ResumeLayout(false);
			this.panel12.PerformLayout();
			this.panel11.ResumeLayout(false);
			this.panel11.PerformLayout();
			this.panel10.ResumeLayout(false);
			this.panel10.PerformLayout();
			this.panel9.ResumeLayout(false);
			this.panel9.PerformLayout();
			this.panel4.ResumeLayout(false);
			this.panel4.PerformLayout();
			this.panel8.ResumeLayout(false);
			this.panel8.PerformLayout();
			this.panel6.ResumeLayout(false);
			this.panel6.PerformLayout();
			this.gbwfs.ResumeLayout(false);
			this.gbwfs.PerformLayout();
			this.p2.ResumeLayout(false);
			this.p2.PerformLayout();
			this.groupBox6.ResumeLayout(false);
			this.groupBox6.PerformLayout();
			this.gbOM.ResumeLayout(false);
			this.gbOM.PerformLayout();
			this.pOverSampling.ResumeLayout(false);
			this.pOverSampling.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.ledSleep)).EndInit();
			this.p1.ResumeLayout(false);
			this.p1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.ledStandby)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ledWake)).EndInit();
			this.gbDR.ResumeLayout(false);
			this.gbDR.PerformLayout();
			this.TabTool.ResumeLayout(false);
			this.DataConfigPage.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.gbStatus.ResumeLayout(false);
			this.p21.ResumeLayout(false);
			this.p21.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.ledFIFOStatus)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ledRealTimeStatus)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ledXYZOW)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ledZOW)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ledYOW)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ledXOW)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ledZDR)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ledYDR)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ledXDR)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ledXYZDR)).EndInit();
			this.groupBox3.ResumeLayout(false);
			this.p20.ResumeLayout(false);
			this.panel16.ResumeLayout(false);
			this.panel16.PerformLayout();
			this.panel17.ResumeLayout(false);
			this.panel17.PerformLayout();
			this.MFF1_2Page.ResumeLayout(false);
			this.MFF1_2Page.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.legend3)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.MFFGraph)).EndInit();
			this.gbMF1.ResumeLayout(false);
			this.gbMF1.PerformLayout();
			this.p14.ResumeLayout(false);
			this.p14.PerformLayout();
			this.p15.ResumeLayout(false);
			this.p15.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.ledMFF1EA)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ledMFF1XHE)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ledMFF1YHE)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ledMFF1ZHE)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.tbMFF1Threshold)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.tbMFF1Debounce)).EndInit();
			this.TransientDetection.ResumeLayout(false);
			this.TransientDetection.PerformLayout();
			this.pTrans2.ResumeLayout(false);
			this.pTrans2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.ledTransEA)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ledTransZDetect)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ledTransYDetect)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ledTransXDetect)).EndInit();
			this.gbTSNEW.ResumeLayout(false);
			this.gbTSNEW.PerformLayout();
			this.pTransNEW.ResumeLayout(false);
			this.pTransNEW.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.tbTransDebounceNEW)).EndInit();
			this.p19.ResumeLayout(false);
			this.p19.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.ledTransEANEW)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ledTransZDetectNEW)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ledTransYDetectNEW)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ledTransXDetectNEW)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.tbTransThresholdNEW)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.legend2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.gphXYZ)).EndInit();
			this.gbTS.ResumeLayout(false);
			this.gbTS.PerformLayout();
			this.p18.ResumeLayout(false);
			this.p18.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.tbTransDebounce)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.tbTransThreshold)).EndInit();
			this.PulseDetection.ResumeLayout(false);
			this.gbSDPS.ResumeLayout(false);
			this.gbSDPS.PerformLayout();
			this.panel15.ResumeLayout(false);
			this.panel15.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.tbPulseZThreshold)).EndInit();
			this.p12.ResumeLayout(false);
			this.p12.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.tbPulseLatency)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.tbPulse2ndPulseWin)).EndInit();
			this.p10.ResumeLayout(false);
			this.p10.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.tbFirstPulseTimeLimit)).EndInit();
			this.p11.ResumeLayout(false);
			this.p11.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.ledPulseDouble)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ledPulseEA)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ledPZ)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ledPX)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ledPY)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.tbPulseXThreshold)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.tbPulseYThreshold)).EndInit();
			this.FIFOPage.ResumeLayout(false);
			this.FIFOPage.PerformLayout();
			this.gb3bF.ResumeLayout(false);
			this.gb3bF.PerformLayout();
			this.p5.ResumeLayout(false);
			this.p5.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.ledTrigMFF)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ledTrigTap)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ledTrigLP)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ledTrigTrans)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ledOverFlow)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ledWatermark)).EndInit();
			this.p4.ResumeLayout(false);
			this.gbWatermark.ResumeLayout(false);
			this.gbWatermark.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.tBWatermark)).EndInit();
			this.pTriggerMode.ResumeLayout(false);
			this.pTriggerMode.PerformLayout();
			this.panel5.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
			this.CommStrip.ResumeLayout(false);
			this.CommStrip.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void lblX12bG_Click(object sender, EventArgs e)
		{
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
				ImagePL_fu = (Image)manager.GetObject("STB_PL_front_up");
				ImagePL_fl = (Image)manager.GetObject("STB_PL_front_left");
				ImagePL_fd = (Image)manager.GetObject("STB_PL_front_down");
				ImagePL_fr = (Image)manager.GetObject("STB_PL_front_right");
				ImagePL_bu = (Image)manager.GetObject("STB_PL_back_up");
				ImagePL_bl = (Image)manager.GetObject("STB_PL_back_left");
				ImagePL_bd = (Image)manager.GetObject("STB_PL_back_down");
				ImagePL_br = (Image)manager.GetObject("STB_PL_back_right");
			}
			catch (Exception exception)
			{
				STBLogger.AddEvent(this, STBLogger.EventLevel.Information, "Exception", exception.Message + exception.Source + exception.StackTrace);
				ImageGreen = CommStripButton.Image;
				ImageYellow = CommStripButton.Image;
				ImageRed = CommStripButton.Image;
			}
		}

		private void MainScreenGraph_PlotDataChanged(object sender, XYPlotDataChangedEventArgs e)
		{
		}

		private void OnClose(object sender, EventArgs e)
		{
			VeyronControllerObj.DisableData();
			VeyronControllerObj.CloseConnection();
			base.Close();
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
				uxAxisAccel.Range = new NationalInstruments.UI.Range(-2.0, 2.0);
				uxAxisAccel.MajorDivisions.Interval = 0.5;
			}
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
				uxAxisAccel.Range = new NationalInstruments.UI.Range(-4.0, 4.0);
				uxAxisAccel.MajorDivisions.Interval = 1.0;
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
				uxAxisAccel.Range = new NationalInstruments.UI.Range(-8.0, 8.0);
				uxAxisAccel.MajorDivisions.Interval = 2.0;
			}
		}

		private void rBActive_CheckedChanged(object sender, EventArgs e)
		{
			if (!rdoActive.Checked)
			{
				uxStart.Enabled = false;
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
				ControllerReqPacket packet6;
				ControllerReqPacket packet7;
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
				uxStart.Enabled = true;
				uxStart.Checked = false;
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
				if (!rdoDisabled.Checked && rdoActive.Checked)
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
						VeyronControllerObj.PollInterruptMapStatus = true;
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
								case deviceID.MMA8451Q:
									VeyronControllerObj.Data14bFlag = true;
									packet3 = new ControllerReqPacket();
									CreateNewTaskFromGUI(packet3, 3, 0x5c, numArray2);
									break;

								case deviceID.MMA8452Q:
									VeyronControllerObj.Data12bFlag = true;
									packet4 = new ControllerReqPacket();
									CreateNewTaskFromGUI(packet4, 3, 2, numArray2);
									break;

								case deviceID.MMA8453Q:
									VeyronControllerObj.Data10bFlag = true;
									packet5 = new ControllerReqPacket();
									CreateNewTaskFromGUI(packet5, 3, 0x5b, numArray2);
									break;

								case deviceID.MMA8652FC:
									VeyronControllerObj.Data12bFlag = true;
									packet6 = new ControllerReqPacket();
									CreateNewTaskFromGUI(packet6, 3, 2, numArray2);
									break;

								case deviceID.MMA8653FC:
									VeyronControllerObj.Data10bFlag = true;
									packet7 = new ControllerReqPacket();
									CreateNewTaskFromGUI(packet7, 3, 0x5b, numArray2);
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
									case deviceID.MMA8451Q:
										VeyronControllerObj.Data14bFlag = true;
										packet3 = new ControllerReqPacket();
										CreateNewTaskFromGUI(packet3, 5, 0x5c, numArray2);
										break;

									case deviceID.MMA8452Q:
										VeyronControllerObj.Data12bFlag = true;
										packet4 = new ControllerReqPacket();
										CreateNewTaskFromGUI(packet4, 5, 2, numArray2);
										break;

									case deviceID.MMA8453Q:
										VeyronControllerObj.Data10bFlag = true;
										packet5 = new ControllerReqPacket();
										CreateNewTaskFromGUI(packet5, 5, 0x5b, numArray2);
										break;

									case deviceID.MMA8652FC:
										VeyronControllerObj.Data12bFlag = true;
										packet6 = new ControllerReqPacket();
										CreateNewTaskFromGUI(packet6, 5, 2, numArray2);
										break;

									case deviceID.MMA8653FC:
										VeyronControllerObj.Data10bFlag = true;
										packet7 = new ControllerReqPacket();
										CreateNewTaskFromGUI(packet7, 5, 0x5b, numArray2);
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
							if (rdoDisabled.Checked)
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
							ControllerReqPacket packet8 = new ControllerReqPacket();
							CreateNewTaskFromGUI(packet8, 7, 80, numArray3);
							return;
						}
				}
			}
		}

		private void rbR00_CheckedChanged(object sender, EventArgs e)
		{
		}

		private void rBStandby_CheckedChanged(object sender, EventArgs e)
		{
			if (rdoStandby.Checked)
			{
				p4.Enabled = true;
				pTriggerMode.Enabled = true;
				gbWatermark.Enabled = true;
				btnWatermark.Enabled = true;
				btnSetMode.Text = "Set MODE";
				chkAnalogLowNoise.Enabled = true;
				if (uxStart.Checked)
				{
					uxStart.Checked = false;
				}
				chkEnablePL.Enabled = true;
				if (chkEnablePL.Checked)
				{
					chkPLDefaultSettings.Enabled = true;
				}
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
				uxStart.Enabled = false;
				if (!rdoDisabled.Checked)
				{
					p14b8bSelect.Enabled = false;
					rdoFIFO14bitDataDisplay.Enabled = false;
					rdoFIFO8bitDataDisplay.Enabled = false;
				}
				else
				{
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
				if (rdoDisabled.Checked)
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
				changeAxisCount();
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
				setCountsAxis(8);
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
			rdoCircular.Enabled = false;
			rdoFill.Enabled = false;
			rdoTriggerMode.Enabled = false;
			if (rdoCircular.Checked)
			{
				FIFOModeValue = 1;
			}
			btnSetMode_Click(sender, e);
			chkDisableFIFO_CheckedChanged(sender, e);
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
					tbFirstPulseTimeLimit.Value = (int)(100.0 / DR_timestep);
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
					tbFirstPulseTimeLimit.Value = (int)(100.0 / DR_timestep);
					tbPulseLatency.Value = tbFirstPulseTimeLimit.Value;
					tbPulse2ndPulseWin.Value = tbPulseLatency.Value + ((int)(100.0 / DR_timestep));
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

		private void rdoDisabled_CheckedChanged(object sender, EventArgs e)
		{
			rdoCircular.Enabled = true;
			rdoFill.Enabled = true;
			rdoTriggerMode.Enabled = true;
			if (rdoDisabled.Checked)
			{
				FIFOModeValue = 0;
			}
			btnSetMode_Click(sender, e);
			chkDisableFIFO_CheckedChanged(sender, e);
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
			rdoCircular.Enabled = false;
			rdoFill.Enabled = false;
			rdoTriggerMode.Enabled = false;
			if (rdoFill.Checked)
			{
				FIFOModeValue = 2;
			}
			btnSetMode_Click(sender, e);
			chkDisableFIFO_CheckedChanged(sender, e);
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
						DR_timestep = 1.25;
						DR_PulseTimeStepNoLPF = 0.625;
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

					case 6:
						DR_timestep = 2.5;
						DR_PulseTimeStepNoLPF = 0.625;
						ddlHPFilter.Items.Clear();
						ddlHPFilter.Items.Add("16Hz");
						ddlHPFilter.Items.Add("8Hz");
						ddlHPFilter.Items.Add("4Hz");
						ddlHPFilter.Items.Add("2Hz");
						ddlHPFilter.SelectedIndex = selectedIndex;
						break;

					case 7:
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
						DR_timestep = 1.25;
						DR_PulseTimeStepNoLPF = 0.625;
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
						DR_timestep = 5.0;
						DR_PulseTimeStepNoLPF = 1.25;
						ddlHPFilter.Items.Clear();
						ddlHPFilter.Items.Add("8Hz");
						ddlHPFilter.Items.Add("4Hz");
						ddlHPFilter.Items.Add("2Hz");
						ddlHPFilter.Items.Add("1Hz");
						ddlHPFilter.SelectedIndex = selectedIndex;
						break;

					case 3:
						DR_timestep = 10.0;
						DR_PulseTimeStepNoLPF = 2.5;
						ddlHPFilter.Items.Clear();
						ddlHPFilter.Items.Add("4Hz");
						ddlHPFilter.Items.Add("2Hz");
						ddlHPFilter.Items.Add("1Hz");
						ddlHPFilter.Items.Add("0.5Hz");
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
						DR_timestep = 80.0;
						DR_PulseTimeStepNoLPF = 20.0;
						ddlHPFilter.Items.Clear();
						ddlHPFilter.Items.Add("0.5Hz");
						ddlHPFilter.Items.Add("0.25Hz");
						ddlHPFilter.Items.Add("0.125Hz");
						ddlHPFilter.Items.Add("0.063Hz");
						ddlHPFilter.SelectedIndex = selectedIndex;
						break;

					case 6:
						DR_timestep = 80.0;
						DR_PulseTimeStepNoLPF = 20.0;
						ddlHPFilter.Items.Clear();
						ddlHPFilter.Items.Add("0.5Hz");
						ddlHPFilter.Items.Add("0.25Hz");
						ddlHPFilter.Items.Add("0.125Hz");
						ddlHPFilter.Items.Add("0.063Hz");
						ddlHPFilter.SelectedIndex = selectedIndex;
						break;

					case 7:
						DR_timestep = 80.0;
						DR_PulseTimeStepNoLPF = 20.0;
						ddlHPFilter.Items.Clear();
						ddlHPFilter.Items.Add("0.5Hz");
						ddlHPFilter.Items.Add("0.25Hz");
						ddlHPFilter.Items.Add("0.125Hz");
						ddlHPFilter.Items.Add("0.063Hz");
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
						DR_timestep = 1.25;
						DR_PulseTimeStepNoLPF = 0.625;
						break;

					case 1:
						DR_timestep = 2.5;
						DR_PulseTimeStepNoLPF = 1.25;
						ddlHPFilter.Items.Clear();
						ddlHPFilter.Items.Add("8Hz");
						ddlHPFilter.Items.Add("4Hz");
						ddlHPFilter.Items.Add("2Hz");
						ddlHPFilter.Items.Add("1Hz");
						ddlHPFilter.SelectedIndex = selectedIndex;
						break;

					case 2:
						DR_timestep = 5.0;
						DR_PulseTimeStepNoLPF = 2.5;
						ddlHPFilter.Items.Clear();
						ddlHPFilter.Items.Add("4Hz");
						ddlHPFilter.Items.Add("2Hz");
						ddlHPFilter.Items.Add("1Hz");
						ddlHPFilter.Items.Add("0.5Hz");
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

					case 4:
						DR_timestep = 20.0;
						DR_PulseTimeStepNoLPF = 10.0;
						ddlHPFilter.Items.Clear();
						ddlHPFilter.Items.Add("1Hz");
						ddlHPFilter.Items.Add("0.5Hz");
						ddlHPFilter.Items.Add("0.25Hz");
						ddlHPFilter.Items.Add("0.125Hz");
						ddlHPFilter.SelectedIndex = selectedIndex;
						break;

					case 5:
						DR_timestep = 80.0;
						DR_PulseTimeStepNoLPF = 40.0;
						ddlHPFilter.Items.Clear();
						ddlHPFilter.Items.Add("0.25Hz");
						ddlHPFilter.Items.Add("0.125Hz");
						ddlHPFilter.Items.Add("0.063Hz");
						ddlHPFilter.Items.Add("0.031Hz");
						ddlHPFilter.SelectedIndex = selectedIndex;
						break;

					case 6:
						DR_timestep = 160.0;
						DR_PulseTimeStepNoLPF = 40.0;
						ddlHPFilter.Items.Clear();
						ddlHPFilter.Items.Add("0.25Hz");
						ddlHPFilter.Items.Add("0.125Hz");
						ddlHPFilter.Items.Add("0.063Hz");
						ddlHPFilter.Items.Add("0.031Hz");
						ddlHPFilter.SelectedIndex = selectedIndex;
						break;

					case 7:
						DR_timestep = 160.0;
						DR_PulseTimeStepNoLPF = 40.0;
						ddlHPFilter.Items.Clear();
						ddlHPFilter.Items.Add("0.25Hz");
						ddlHPFilter.Items.Add("0.125Hz");
						ddlHPFilter.Items.Add("0.063Hz");
						ddlHPFilter.Items.Add("0.031Hz");
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

				WakeOSMode = 0;
				VeyronControllerObj.SetWakeOSModeFlag = true;
				CreateNewTaskFromGUI(new ControllerReqPacket(), 0, 0x54, new int[1] { WakeOSMode });
				int selectedIndex = ddlHPFilter.SelectedIndex;

				switch (ddlDataRate.SelectedIndex)
				{
					case 0:
						DR_timestep = 1.25;
						DR_PulseTimeStepNoLPF = 0.625;
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
						DR_timestep = 5.0;
						DR_PulseTimeStepNoLPF = 1.25;
						ddlHPFilter.Items.Clear();
						ddlHPFilter.Items.Add("8Hz");
						ddlHPFilter.Items.Add("4Hz");
						ddlHPFilter.Items.Add("2Hz");
						ddlHPFilter.Items.Add("1Hz");
						ddlHPFilter.SelectedIndex = selectedIndex;
						break;

					case 3:
						DR_timestep = 10.0;
						DR_PulseTimeStepNoLPF = 2.5;
						ddlHPFilter.Items.Clear();
						ddlHPFilter.Items.Add("4Hz");
						ddlHPFilter.Items.Add("2Hz");
						ddlHPFilter.Items.Add("1Hz");
						ddlHPFilter.Items.Add("0.5Hz");
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

					case 6:
						DR_timestep = 20.0;
						DR_PulseTimeStepNoLPF = 5.0;
						ddlHPFilter.Items.Clear();
						ddlHPFilter.Items.Add("2Hz");
						ddlHPFilter.Items.Add("1Hz");
						ddlHPFilter.Items.Add("0.5Hz");
						ddlHPFilter.Items.Add("0.25Hz");
						ddlHPFilter.SelectedIndex = selectedIndex;
						break;

					case 7:
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
					pulse_step = DR_timestep;
				else
					pulse_step = DR_PulseTimeStepNoLPF;
				
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
			VeyronControllerObj.SetIntsConfigFlag = true;
			CreateNewTaskFromGUI(new ControllerReqPacket(), 0, 14, new int[2] { rdoPLINT_I1.Checked ? 0xFF : 0, 0x10 });
		}

		private void rdoPLINT_I2_CheckedChanged(object sender, EventArgs e)
		{
			VeyronControllerObj.SetIntsConfigFlag = true;
			CreateNewTaskFromGUI(new ControllerReqPacket(), 0, 14, new int[2] { rdoPLINT_I2.Checked ? 0 : 0xFF , 0x10});
		}

		private void rdoPulseINT_I1_CheckedChanged(object sender, EventArgs e)
		{
			VeyronControllerObj.SetIntsConfigFlag = true;
			CreateNewTaskFromGUI(new ControllerReqPacket(), 0, 14, new int[2]{rdoPulseINT_I1.Checked ? 0xff : 0, 0x08});
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
				SleepOSMode = 2;
				VeyronControllerObj.SetSleepOSModeFlag = true;
				CreateNewTaskFromGUI(new ControllerReqPacket(), 0, 0x55, new int[1]{SleepOSMode});
				switch (ddlSleepSR.SelectedIndex)
				{
					case 4:
						DRSleep_timestep = 2.5;
						DRSleep_PulseTimeStepNoLPF = 0.625;
						break;

					case 5:
						DRSleep_timestep = 2.5;
						DRSleep_PulseTimeStepNoLPF = 0.625;
						break;

					case 6:
						DRSleep_timestep = 2.5;
						DRSleep_PulseTimeStepNoLPF = 0.625;
						break;

					case 7:
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
					case 4:
						DRSleep_timestep = 20.0;
						DRSleep_PulseTimeStepNoLPF = 5.0;
						break;

					case 5:
						DRSleep_timestep = 80.0;
						DRSleep_PulseTimeStepNoLPF = 20.0;
						break;

					case 6:
						DRSleep_timestep = 160.0;
						DRSleep_PulseTimeStepNoLPF = 40.0;
						break;

					case 7:
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
					case 4:
						DRSleep_timestep = 20.0;
						DRSleep_PulseTimeStepNoLPF = 10.0;
						break;

					case 5:
						DRSleep_timestep = 80.0;
						DRSleep_PulseTimeStepNoLPF = 40.0;
						break;

					case 6:
						DRSleep_timestep = 160.0;
						DRSleep_PulseTimeStepNoLPF = 80.0;
						break;

					case 7:
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
					case 4:
						DRSleep_timestep = 20.0;
						DRSleep_PulseTimeStepNoLPF = 5.0;
						break;

					case 5:
						DRSleep_timestep = 20.0;
						DRSleep_PulseTimeStepNoLPF = 5.0;
						break;

					case 6:
						DRSleep_timestep = 20.0;
						DRSleep_PulseTimeStepNoLPF = 5.0;
						break;

					case 7:
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
			rdoCircular.Enabled = false;
			rdoFill.Enabled = false;
			rdoTriggerMode.Enabled = false;
			if (rdoTriggerMode.Checked)
			{
				FIFOModeValue = 3;
			}
			btnSetMode_Click(sender, e);
			chkDisableFIFO_CheckedChanged(sender, e);
		}

		private void rdoXYZFullResMain_CheckedChanged(object sender, EventArgs e)
		{
			int num;
			int[] numArray;
			ControllerReqPacket packet;
			if (rdoXYZFullResMain.Checked)
			{
				changeAxisCount();
				if (DeviceID == deviceID.MMA8451Q)
				{
					lblMFFDataBit.Text = "14-bit Data";
					lblTransDataBit.Text = "14-bit Data";
				}
				else if ((DeviceID == deviceID.MMA8452Q) || (DeviceID == deviceID.MMA8652FC))
				{
					lblMFFDataBit.Text = "12-bit Data";
					lblTransDataBit.Text = "12-bit Data";
				}
				else
				{
					lblMFFDataBit.Text = "10-bit Data";
					lblTransDataBit.Text = "10-bit Data";
				}
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
				setCountsAxis(8);
				lblMFFDataBit.Text = "8-bit Data";
				lblTransDataBit.Text = "8-bit Data";
				rdoFIFO8bitDataDisplay.Checked = true;
				rdoFIFO14bitDataDisplay.Checked = false;
				num = 0xff;
				numArray = new int[] { num };
				VeyronControllerObj.SetFREADFlag = true;
				packet = new ControllerReqPacket();
				CreateNewTaskFromGUI(packet, 0, 0x58, numArray);
			}
		}

		private void registerBitsChangeHandler(RegisterSTB register, int newVal)
		{
			int[] datapassed = new int[] { register.Address, newVal };
			VeyronControllerObj.WriteValueFlag = true;
			ControllerReqPacket reqName = new ControllerReqPacket();
			CreateNewTaskFromGUI(reqName, 1, 0x12, datapassed);
			Thread.Sleep(100);
			VeyronControllerObj.ReadValueParsedFlag = true;
			CreateNewTaskFromGUI(reqName, 1, 0x5D, datapassed);
		}

		private void registerChangeHandler(RegisterSTB register)
		{
			VeyronControllerObj.ReadValueParsedFlag = true;
			CreateNewTaskFromGUI(new ControllerReqPacket(), 1, 0x5D, new int[] { register.Address });
		}

		private void setCountsAxis(uint bits)
		{
			uxAxisCounts.Range = new NationalInstruments.UI.Range(-Math.Pow(2.0, (double)(bits - 1)), Math.Pow(2.0, (double)(bits - 1)) - 1.0);
			uxAxisCounts.MajorDivisions.Interval = (uxAxisCounts.Range.Maximum - uxAxisCounts.Range.Minimum) / 8.0;
		}

		private void SetDeviceID(deviceID devid)
		{
			DeviceID = devid;
			_registerView.ProductName = Enum.GetName(typeof(deviceID), DeviceID);
			changeAxisCount();

			if (DeviceID != deviceID.MMA8451Q && DeviceID != deviceID.MMA8652FC)
			{
				TabTool.TabPages.Remove(FIFOPage);
				ledFIFO.Visible = false;
				panel12.Visible = false;
				chkWakeFIFOGate.Visible = false;
				gbPLDisappear.Enabled = false;
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

			if (DeviceID == deviceID.MMA8453Q)
			{
				Text = "MMA8453Q Full System Evaluation Software";
				chkHPFDataOut.Visible = false;
				rdoXYZFullResMain.Text = "10 bits";
				lbl2gSensitivity.Text = "256 counts/g 10-bit";
				lbl4gSensitivity.Text = "128 counts/g 10-bit";
				lbl8gSensitivity.Text = " 64 counts/g 10-bit";
				chkAnalogLowNoise.Visible = true;
			}
			else if (DeviceID == deviceID.MMA8653FC)
			{
				Text = "MMA8653FC Full System Evaluation Software";
				chkHPFDataOut.Visible = false;
				rdoXYZFullResMain.Text = "10 bits";
				lbl2gSensitivity.Text = "256 counts/g 10-bit";
				lbl4gSensitivity.Text = "128 counts/g 10-bit";
				lbl8gSensitivity.Text = " 64 counts/g 10-bit";
				chkAnalogLowNoise.Visible = false;
				lblMFFDataBit.Text = "10-bit Data";
				lblTransDataBit.Text = "10-bit Data";
				TabTool.TabPages.Remove(PulseDetection);
				TabTool.TabPages.Remove(TransientDetection);
				gbPLDisappear.Enabled = false;
				chkWakeTrans.Visible = false;
				chkWakePulse.Visible = false;
				panel10.Visible = false;
				panel9.Visible = false;
				ledTrans.Visible = false;
				ledPulse.Visible = false;
				p2.Visible = false;
			}
			else if (DeviceID == deviceID.MMA8652FC)
			{
				Text = "MMA8652FC Full System Evaluation Software";
				chkHPFDataOut.Visible = true;
				rdoXYZFullResMain.Text = "12 bits";
				lbl2gSensitivity.Text = "1024 counts/g 12-bit";
				lbl4gSensitivity.Text = " 512 counts/g 12-bit";
				lbl8gSensitivity.Text = " 256 counts/g 12-bit";
				chkAnalogLowNoise.Visible = false;
				rdoFIFO14bitDataDisplay.Text = "View 12-bit Data";
				lblMFFDataBit.Text = "12-bit Data";
				lblTransDataBit.Text = "12-bit Data";
			}
			else if (DeviceID == deviceID.MMA8452Q)
			{
				Text = "MMA8452Q Full System Evaluation Software";
				rdoXYZFullResMain.Text = "12 bits";
				lbl2gSensitivity.Text = "1024 counts/g 12-bit";
				lbl4gSensitivity.Text = " 512 counts/g 12-bit";
				lbl8gSensitivity.Text = " 256 counts/g 12-bit";
			}
			else if (DeviceID == deviceID.MMA8491Q)
			{
				Text = "MMA8491Q Full System Evaluation Software";
				rdoXYZFullResMain.Text = "14 bits";
				lbl2gSensitivity.Text = "1024 counts/g 14-bit";
				lbl4gSensitivity.Text = " 512 counts/g 14-bit";
				lbl8gSensitivity.Text = " 256 counts/g 14-bit";
				chkAnalogLowNoise.Visible = false;
				lblMFFDataBit.Text = "14-bit Data";
				lblTransDataBit.Text = "14-bit Data";
				TabTool.TabPages.Remove(PulseDetection);
				TabTool.TabPages.Remove(TransientDetection);
				TabTool.TabPages.Remove(FIFOPage);
				TabTool.TabPages.Remove(PL_Page);
				TabTool.TabPages.Remove(MFF1_2Page);
				TabTool.TabPages.Remove(DataConfigPage);
				gbIF.Visible = false;
				pSOS.Visible = false;
				gbASS.Visible = false;
				gbwfs.Visible = false;
				rdo8bitDataMain.Visible = false;
				groupBox8.Visible = false;
				gbST.Visible = false;
				pOverSampling.Visible = false;
				p2.Visible = false;
				p1.Visible = false;
				gbDR.Visible = false;
				gbPLDisappear.Enabled = false;
				chkWakeTrans.Visible = false;
				chkWakePulse.Visible = false;
				chkWakeFIFOGate.Visible = false;
				chkWakeLP.Visible = false;
				chkWakeMFF1.Visible = false;
				panel10.Visible = false;
				panel9.Visible = false;
				ledTrans.Visible = false;
				ledPulse.Visible = false;
				ledFIFO.Visible = false;
				ledMFF1.Visible = false;
				ledOrient.Visible = false;
				p2.Visible = false;
				uxPanelSettings.Visible = false;
				lbsSleep.Visible = false;
				ledSleep.Visible = false;
				MainScreenGraph.Size = new Size(900, 500);
				chkHPFDataOut.Visible = false;
				lbsStandby.Location = new Point(5, 0x2a);
				ledStandby.Location = new Point(100, 0x26);
				lbsWake.Location = new Point(5, 0x52);
				ledWake.Location = new Point(100, 0x4b);
				gbOM.Size = new Size(0x1a2, 0x89);
				pictureBox2.Location = new Point(0x287, 0x19);
				pictureBox2.Size = new Size(0x1e2, 0x29c);
			}

			try
			{
				_registerView.ParseMap(_registerMapFile[DeviceID]);
				_registerView.RegisterChange += new RegisterView.RegisterChangeDelegate(registerChangeHandler);
				_registerView.RegisterBitsChange += new RegisterView.RegisterBitsChangeDelegate(registerBitsChangeHandler);
				_registerView.ButtonExportRegisters.Click += new EventHandler(ButtonExportRegisters_Click);
				Register_Page.Controls.Add(_registerView);
			}
			catch
			{
				TabTool.TabPages.Remove(Register_Page);
			}
		}

		#region TabTool_SelectedIndexChanged 
		private void TabTool_SelectedIndexChanged(object sender, EventArgs e)
		{
			int[] numArray;
			switch (TabTool.SelectedIndex)
			{
				case 0:	// Main Screen
					if (rdoActive.Checked && chkEnableASleep.Checked)
						VeyronControllerObj.ReturnSysmodStatus = true;

					VeyronControllerObj.ReturnTransStatus = false;
					VeyronControllerObj.ReturnPulseStatus = false;
					VeyronControllerObj.ReturnDataStatus = false;
					VeyronControllerObj.ReturnFIFOStatus = false;
					VeyronControllerObj.ReturnMFF1Status = false;
					VeyronControllerObj.ReturnPLStatus = false;
					VeyronControllerObj.DisableData();
					if (rdoActive.Checked)
					{
						uxStart.Enabled = true;
						uxStart.Checked = false;
					}
					if (rdoActive.Checked)
						VeyronControllerObj.PollInterruptMapStatus = true;

					break;

				case 1:	// Registers
					VeyronControllerObj.ReturnSysmodStatus = (chkEnableASleep.Checked && rdoActive.Checked);
					VeyronControllerObj.PollInterruptMapStatus = false;
					VeyronControllerObj.ReturnDataStatus = false;
					VeyronControllerObj.ReturnMFF1Status = false;
					VeyronControllerObj.ReturnPLStatus = false;
					VeyronControllerObj.ReturnTransStatus = false;
					VeyronControllerObj.ReturnPulseStatus = false;
					VeyronControllerObj.ReturnFIFOStatus = false;
					VeyronControllerObj.DisableData();
					break;

				case 2:
					VeyronControllerObj.ReturnSysmodStatus = (chkEnableASleep.Checked && rdoActive.Checked);

					VeyronControllerObj.PollInterruptMapStatus = false;
					VeyronControllerObj.ReturnMFF1Status = false;
					VeyronControllerObj.ReturnPLStatus = false;
					VeyronControllerObj.ReturnTransStatus = false;
					VeyronControllerObj.ReturnPulseStatus = false;
					VeyronControllerObj.ReturnFIFOStatus = false;
					VeyronControllerObj.DisableData();
					if (rdoActive.Checked)
						VeyronControllerObj.ReturnDataStatus = true;
					break;

				case 3:
					VeyronControllerObj.ReturnSysmodStatus = (chkEnableASleep.Checked && rdoActive.Checked);

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
							CreateNewTaskFromGUI(new ControllerReqPacket(), 3, 1, numArray);
							UpdateWaveformMFF(Gees8bData);
						}
						else if (rdoXYZFullResMain.Checked)
						{
							switch (DeviceID)
							{
								case deviceID.MMA8451Q:
									VeyronControllerObj.Data14bFlag = true;
									CreateNewTaskFromGUI(new ControllerReqPacket(), 3, 0x5c, numArray);
									UpdateWaveformMFF(Gees14bData);
									break;

								case deviceID.MMA8452Q:
									VeyronControllerObj.Data12bFlag = true;
									CreateNewTaskFromGUI(new ControllerReqPacket(), 3, 2, numArray);
									UpdateWaveformMFF(Gees12bData);
									break;

								case deviceID.MMA8453Q:
									VeyronControllerObj.Data10bFlag = true;
									CreateNewTaskFromGUI(new ControllerReqPacket(), 3, 0x5b, numArray);
									UpdateWaveformMFF(Gees10bData);
									break;

								case deviceID.MMA8652FC:
									VeyronControllerObj.Data12bFlag = true;
									CreateNewTaskFromGUI(new ControllerReqPacket(), 3, 2, numArray);
									UpdateWaveformMFF(Gees12bData);
									break;

								case deviceID.MMA8653FC:
									VeyronControllerObj.Data10bFlag = true;
									CreateNewTaskFromGUI(new ControllerReqPacket(), 3, 0x5b, numArray);
									UpdateWaveformMFF(Gees10bData);
									break;
							}
						}
					}
					break;

				case 4:
					VeyronControllerObj.ReturnSysmodStatus = (chkEnableASleep.Checked && rdoActive.Checked);

					VeyronControllerObj.PollInterruptMapStatus = false;
					VeyronControllerObj.ReturnDataStatus = false;
					VeyronControllerObj.ReturnMFF1Status = false;
					VeyronControllerObj.ReturnTransStatus = false;
					VeyronControllerObj.ReturnPulseStatus = false;
					VeyronControllerObj.ReturnFIFOStatus = false;
					VeyronControllerObj.DisableData();
					if (rdoActive.Checked && chkEnablePL.Checked)
						VeyronControllerObj.ReturnPLStatus = true;
					break;

				case 5:
					VeyronControllerObj.ReturnSysmodStatus = (chkEnableASleep.Checked && rdoActive.Checked);

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
							VeyronControllerObj.ReturnTransStatus = true;
						if ((chkTransEnableXFlagNEW.Checked || chkTransEnableYFlagNEW.Checked) || chkTransEnableZFlagNEW.Checked)
							VeyronControllerObj.ReturnTrans1Status = true;
						if (VeyronControllerObj.ReturnTrans1Status || VeyronControllerObj.ReturnTransStatus)
						{
							numArray = new int[] { SystemPolling };
							gphXYZ.Enabled = true;
							if (rdo8bitDataMain.Checked)
							{
								VeyronControllerObj.Data8bFlag = true;
								CreateNewTaskFromGUI(new ControllerReqPacket(), 5, 1, numArray);
							}
							else if (rdoXYZFullResMain.Checked)
							{
								switch (DeviceID)
								{
									case deviceID.MMA8451Q:
										VeyronControllerObj.Data14bFlag = true;
										CreateNewTaskFromGUI(new ControllerReqPacket(), 5, 0x5c, numArray);
										break;

									case deviceID.MMA8452Q:
										VeyronControllerObj.Data12bFlag = true;
										CreateNewTaskFromGUI(new ControllerReqPacket(), 5, 2, numArray);
										break;

									case deviceID.MMA8453Q:
										VeyronControllerObj.Data10bFlag = true;
										CreateNewTaskFromGUI(new ControllerReqPacket(), 5, 0x5b, numArray);
										break;

									case deviceID.MMA8652FC:
										VeyronControllerObj.Data12bFlag = true;
										CreateNewTaskFromGUI(new ControllerReqPacket(), 5, 2, numArray);
										break;

									case deviceID.MMA8653FC:
										VeyronControllerObj.Data10bFlag = true;
										CreateNewTaskFromGUI(new ControllerReqPacket(), 5, 0x5b, numArray);
										break;
								}
							}
						}
					}
					break;

				case 6:
					VeyronControllerObj.ReturnSysmodStatus = (chkEnableASleep.Checked && rdoActive.Checked);

					VeyronControllerObj.PollInterruptMapStatus =
					VeyronControllerObj.ReturnDataStatus =
					VeyronControllerObj.ReturnMFF1Status =
					VeyronControllerObj.ReturnPLStatus =
					VeyronControllerObj.ReturnTransStatus =
					VeyronControllerObj.ReturnFIFOStatus = false;

					VeyronControllerObj.DisableData();

					if (rdoActive.Checked && ((((chkPulseEnableXSP.Checked || chkPulseEnableYSP.Checked) || (chkPulseEnableZSP.Checked || chkPulseEnableXDP.Checked)) || chkPulseEnableYDP.Checked) || chkPulseEnableZDP.Checked))
						VeyronControllerObj.ReturnPulseStatus = true;
					break;

				case 7:
					VeyronControllerObj.ReturnSysmodStatus = (chkEnableASleep.Checked && rdoActive.Checked);

					VeyronControllerObj.PollInterruptMapStatus =
					VeyronControllerObj.ReturnDataStatus =
					VeyronControllerObj.ReturnMFF1Status =
					VeyronControllerObj.ReturnPLStatus =
					VeyronControllerObj.ReturnTransStatus =
					VeyronControllerObj.ReturnPulseStatus = false;

					VeyronControllerObj.DisableData();

					if (rdoActive.Checked && !rdoDisabled.Checked)
					{
						int[] datapassed = new int[5];
						datapassed[0] = SystemPolling;
						datapassed[1] = tBWatermark.Value;
						datapassed[2] = FIFODump8orFull;
						datapassed[4] = FullScaleValue;
						if (rdoFill.Checked)
							datapassed[3] = 0;
						else if (rdoCircular.Checked)
							datapassed[3] = 1;
						else if (rdoTriggerMode.Checked)
						{
							datapassed[3] = 2;
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
						ControllerReqPacket reqName = new ControllerReqPacket();
						CreateNewTaskFromGUI(reqName, 7, 80, datapassed);
					}
					break;
			}
		}
		#endregion

		private void tbFirstPulseTimeLimit_Scroll_1(object sender, EventArgs e)
		{
			if (chkPulseLPFEnable.Checked)
			{
				if (ledSleep.Value)
					pulse_step = DRSleep_timestep;
				else
					pulse_step = DR_timestep;
			}
			else if (ledSleep.Value)
				pulse_step = DRSleep_PulseTimeStepNoLPF;
			else
				pulse_step = DR_PulseTimeStepNoLPF;

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
			double num2 = tbMFF1Debounce.Value * (ledSleep.Value ? DRSleep_timestep : DR_timestep);
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
			double num = tbPulseXThreshold.Value * 0.063;
			lblPulseXThresholdVal.Text = string.Format("{0:F3}", num);
		}

		private void tbPulseYThreshold_Scroll_1(object sender, EventArgs e)
		{
			double num = tbPulseYThreshold.Value * 0.063;
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
			btnSetSleepTimer.Enabled = true;
			lblSleepTimer.Enabled = true;
			lblSleepTimerValue.Enabled = true;
			lblSleepms.Enabled = true;

			double num = tbSleepCounter.Value * (ddlDataRate.SelectedIndex != 7 ? 320.0 : 640.0);
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
			double num2 = tbTransDebounce.Value * (ledSleep.Value ? DRSleep_timestep : DR_timestep);
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
			double num2 = tbTransDebounceNEW.Value * (ledSleep.Value ? DRSleep_timestep : DR_timestep);
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

		#region TmrActive_Tick 
		private void TmrActive_Tick(object sender, EventArgs e)
		{
			if (rdoStandby.Checked)
			{
				ledStandby.Value = true;
				ledWake.Value = false;
				ledSleep.Value = false;
			}
			if (rdoActive.Checked && uxStart.Checked)
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
						case deviceID.MMA8491Q:
							UpdateMainScreenWaveform(Gees14bData);
							break;

						case deviceID.MMA8451Q:
							UpdateMainScreenWaveform(Gees14bData);
							break;

						case deviceID.MMA8452Q:
							UpdateMainScreenWaveform(Gees12bData);
							break;

						case deviceID.MMA8453Q:
							UpdateMainScreenWaveform(Gees10bData);
							break;

						case deviceID.MMA8652FC:
							UpdateMainScreenWaveform(Gees12bData);
							break;

						case deviceID.MMA8653FC:
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
					guiPacket = (GUIUpdatePacket)GuiQueue.Dequeue();
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
					byte[] buffer = (byte[])guiPacket.PayLoad.Dequeue();
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
							GUI_SetCal(guiPacket);
						if (guiPacket.TaskID == 15)
							GUI_SetUpInterrupts(guiPacket);
						break;

					case 1:
					case 8:
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
		#endregion

		private void tmrDataDisplay_Tick(object sender, EventArgs e)
		{
			RWLockDispValues.AcquireWriterLock(-1);
			try
			{
				if (rdo8bitDataMain.Checked)
				{
					if (uxSwitchCounts.Value)
					{
						DisplayXYZ8bit.XAver += DisplayXYZ8bit.XAxis;
						DisplayXYZ8bit.YAver += DisplayXYZ8bit.YAxis;
						DisplayXYZ8bit.ZAver += DisplayXYZ8bit.ZAxis;
						if (++_averageCounter == Convert.ToInt32(uxAverageData.SelectedItem.ToString()))
						{
							uxAccelXValue.Text = string.Format("X={0}", (int)(DisplayXYZ8bit.XAver / ((double)_averageCounter)));
							uxAccelYValue.Text = string.Format("Y={0}", (int)(DisplayXYZ8bit.YAver / ((double)_averageCounter)));
							uxAccelZValue.Text = string.Format("Z={0}", (int)(DisplayXYZ8bit.ZAver / ((double)_averageCounter)));
							DisplayXYZ8bit.XAver = DisplayXYZ8bit.YAver = DisplayXYZ8bit.ZAver = 0.0;
							_averageCounter = 0;
						}
					}
					else
					{
						Gees8bData.XAver += Gees8bData.XAxis;
						Gees8bData.YAver += Gees8bData.YAxis;
						Gees8bData.ZAver += Gees8bData.ZAxis;
						if (++_averageCounter == Convert.ToInt32(uxAverageData.SelectedItem.ToString()))
						{
							uxAccelXValue.Text = string.Format("X={0:F3}", Gees8bData.XAver / ((double)_averageCounter));
							uxAccelYValue.Text = string.Format("Y={0:F3}", Gees8bData.YAver / ((double)_averageCounter));
							uxAccelZValue.Text = string.Format("Z={0:F3}", Gees8bData.ZAver / ((double)_averageCounter));
							Gees8bData.XAver = Gees8bData.YAver = Gees8bData.ZAver = 0.0;
							_averageCounter = 0;
						}
					}
				}
				else
				{
					switch (DeviceID)
					{
						case deviceID.MMA8491Q:
						case deviceID.MMA8451Q:
							if (!uxSwitchCounts.Value)
							{
								Gees14bData.XAver += Gees14bData.XAxis;
								Gees14bData.YAver += Gees14bData.YAxis;
								Gees14bData.ZAver += Gees14bData.ZAxis;
								if (++_averageCounter == Convert.ToInt32(uxAverageData.SelectedItem.ToString()))
								{
									uxAccelXValue.Text = string.Format("X={0:F3}", Gees14bData.XAver / ((double)_averageCounter));
									uxAccelYValue.Text = string.Format("Y={0:F3}", Gees14bData.YAver / ((double)_averageCounter));
									uxAccelZValue.Text = string.Format("Z={0:F3}", Gees14bData.ZAver / ((double)_averageCounter));
									Gees14bData.XAver = Gees14bData.YAver = Gees14bData.ZAver = 0.0;
									_averageCounter = 0;
								}
							}
							else
							{
								DisplayXYZ14bit.XAver += DisplayXYZ14bit.XAxis;
								DisplayXYZ14bit.YAver += DisplayXYZ14bit.YAxis;
								DisplayXYZ14bit.ZAver += DisplayXYZ14bit.ZAxis;
								if (++_averageCounter == Convert.ToInt32(uxAverageData.SelectedItem.ToString()))
								{
									uxAccelXValue.Text = string.Format("X={0}", (int)(DisplayXYZ14bit.XAver / ((double)_averageCounter)));
									uxAccelYValue.Text = string.Format("Y={0}", (int)(DisplayXYZ14bit.YAver / ((double)_averageCounter)));
									uxAccelZValue.Text = string.Format("Z={0}", (int)(DisplayXYZ14bit.ZAver / ((double)_averageCounter)));
									DisplayXYZ14bit.XAver = DisplayXYZ14bit.YAver = DisplayXYZ14bit.ZAver = 0.0;
									_averageCounter = 0;
								}
							}
							break;

						case deviceID.MMA8452Q:
						case deviceID.MMA8652FC:
							if (!uxSwitchCounts.Value)
							{
								Gees12bData.XAver += Gees12bData.XAxis;
								Gees12bData.YAver += Gees12bData.YAxis;
								Gees12bData.ZAver += Gees12bData.ZAxis;
								if (++_averageCounter == Convert.ToInt32(uxAverageData.SelectedItem.ToString()))
								{
									uxAccelXValue.Text = string.Format("X={0:F3}", Gees12bData.XAver / ((double)_averageCounter));
									uxAccelYValue.Text = string.Format("Y={0:F3}", Gees12bData.YAver / ((double)_averageCounter));
									uxAccelZValue.Text = string.Format("Z={0:F3}", Gees12bData.ZAver / ((double)_averageCounter));
									Gees12bData.XAver = Gees12bData.YAver = Gees12bData.ZAver = 0.0;
									_averageCounter = 0;
								}
							}
							else
							{
								DisplayXYZ12bit.XAver += DisplayXYZ12bit.XAxis;
								DisplayXYZ12bit.YAver += DisplayXYZ12bit.YAxis;
								DisplayXYZ12bit.ZAver += DisplayXYZ12bit.ZAxis;
								if (++_averageCounter == Convert.ToInt32(uxAverageData.SelectedItem.ToString()))
								{
									uxAccelXValue.Text = string.Format("X={0}", (int)(DisplayXYZ12bit.XAver / ((double)_averageCounter)));
									uxAccelYValue.Text = string.Format("Y={0}", (int)(DisplayXYZ12bit.YAver / ((double)_averageCounter)));
									uxAccelZValue.Text = string.Format("Z={0}", (int)(DisplayXYZ12bit.ZAver / ((double)_averageCounter)));
									DisplayXYZ12bit.XAver = DisplayXYZ12bit.YAver = DisplayXYZ12bit.ZAver = 0.0;
									_averageCounter = 0;
								}
							}
							break;

						case deviceID.MMA8453Q:
						case deviceID.MMA8653FC:
							if (!uxSwitchCounts.Value)
							{
								Gees10bData.XAver += Gees10bData.XAxis;
								Gees10bData.YAver += Gees10bData.YAxis;
								Gees10bData.ZAver += Gees10bData.ZAxis;
								if (++_averageCounter == Convert.ToInt32(uxAverageData.SelectedItem.ToString()))
								{
									uxAccelXValue.Text = string.Format("X={0:F3}", Gees10bData.XAver / ((double)_averageCounter));
									uxAccelYValue.Text = string.Format("Y={0:F3}", Gees10bData.YAver / ((double)_averageCounter));
									uxAccelZValue.Text = string.Format("Z={0:F3}", Gees10bData.ZAver / ((double)_averageCounter));
									Gees10bData.XAver = Gees10bData.YAver = Gees10bData.ZAver = 0.0;
									_averageCounter = 0;
								}
							}
							else
							{
								DisplayXYZ10bit.XAver += DisplayXYZ10bit.XAxis;
								DisplayXYZ10bit.YAver += DisplayXYZ10bit.YAxis;
								DisplayXYZ10bit.ZAver += DisplayXYZ10bit.ZAxis;
								if (++_averageCounter == Convert.ToInt32(uxAverageData.SelectedItem.ToString()))
								{
									uxAccelXValue.Text = string.Format("X={0}", (int)(DisplayXYZ10bit.XAver / ((double)_averageCounter)));
									uxAccelYValue.Text = string.Format("Y={0}", (int)(DisplayXYZ10bit.YAver / ((double)_averageCounter)));
									uxAccelZValue.Text = string.Format("Z={0}", (int)(DisplayXYZ10bit.ZAver / ((double)_averageCounter)));
									DisplayXYZ10bit.XAver = DisplayXYZ10bit.YAver = DisplayXYZ10bit.ZAver = 0.0;
									_averageCounter = 0;
								}
							}
							break;
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
				num = DRSleep_timestep;
			else
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

		private void UpdateCommStrip()
		{
			CommunicationState invalid = CommunicationState.Invalid;
			string commStatus = "";
			try
			{
				invalid = dv.GetCommState();
				commStatus = dv.GetCommStatus();
			}
			catch (Exception) { }

			if ((VeyronControllerObj.DeviceID != deviceID.Unsupported) && (VeyronControllerObj.DeviceID != DeviceID))
				SetDeviceID(VeyronControllerObj.DeviceID);
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
				CommStripButton.Enabled = true;
				CommStripButton.Image = ImageYellow;
			}
			else if (CommMode == eCommMode.Running)
			{
				panel5.Enabled = true;
				TabTool.Enabled = true;
				CommStripButton.Enabled = true;
				CommStripButton.Image = ImageGreen;
			}
			else if (CommMode == eCommMode.Closed)
			{
				CommStripButton.Enabled = true;
				CommStripButton.Image = ImageRed;
			}
			else if (CommMode == eCommMode.Bootloader)
			{
				panel5.Enabled = false;
				TabTool.Enabled = false;
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
				Count10bData = (XYZCounts)counts10bData;
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
					Gees10bData.XAxis = ((double)DisplayXYZ10bit.XAxis) / 256.0;
					Gees10bData.YAxis = ((double)DisplayXYZ10bit.YAxis) / 256.0;
					Gees10bData.ZAxis = ((double)DisplayXYZ10bit.ZAxis) / 256.0;
				}
				else if (FullScaleValue == 1)
				{
					Gees10bData.XAxis = ((double)DisplayXYZ10bit.XAxis) / 128.0;
					Gees10bData.YAxis = ((double)DisplayXYZ10bit.YAxis) / 128.0;
					Gees10bData.ZAxis = ((double)DisplayXYZ10bit.ZAxis) / 128.0;
				}
				else if (FullScaleValue == 2)
				{
					Gees10bData.XAxis = ((double)DisplayXYZ10bit.XAxis) / 64.0;
					Gees10bData.YAxis = ((double)DisplayXYZ10bit.YAxis) / 64.0;
					Gees10bData.ZAxis = ((double)DisplayXYZ10bit.ZAxis) / 64.0;
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
				Count12bData = (XYZCounts)counts12bData;
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
					Gees12bData.XAxis = ((double)DisplayXYZ12bit.XAxis) / 1024.0;
					Gees12bData.YAxis = ((double)DisplayXYZ12bit.YAxis) / 1024.0;
					Gees12bData.ZAxis = ((double)DisplayXYZ12bit.ZAxis) / 1024.0;
				}
				else if (FullScaleValue == 1)
				{
					Gees12bData.XAxis = ((double)DisplayXYZ12bit.XAxis) / 512.0;
					Gees12bData.YAxis = ((double)DisplayXYZ12bit.YAxis) / 512.0;
					Gees12bData.ZAxis = ((double)DisplayXYZ12bit.ZAxis) / 512.0;
				}
				else if (FullScaleValue == 2)
				{
					Gees12bData.XAxis = ((double)DisplayXYZ12bit.XAxis) / 256.0;
					Gees12bData.YAxis = ((double)DisplayXYZ12bit.YAxis) / 256.0;
					Gees12bData.ZAxis = ((double)DisplayXYZ12bit.ZAxis) / 256.0;
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
				Count14bData = (XYZCounts)counts14bData;
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
					Gees14bData.XAxis = ((double)DisplayXYZ14bit.XAxis) / 4096.0;
					Gees14bData.YAxis = ((double)DisplayXYZ14bit.YAxis) / 4096.0;
					Gees14bData.ZAxis = ((double)DisplayXYZ14bit.ZAxis) / 4096.0;
				}
				if (FullScaleValue == 1)
				{
					Gees14bData.XAxis = ((double)DisplayXYZ14bit.XAxis) / 2048.0;
					Gees14bData.YAxis = ((double)DisplayXYZ14bit.YAxis) / 2048.0;
					Gees14bData.ZAxis = ((double)DisplayXYZ14bit.ZAxis) / 2048.0;
				}
				if (FullScaleValue == 2)
				{
					Gees14bData.XAxis = ((double)DisplayXYZ14bit.XAxis) / 1024.0;
					Gees14bData.YAxis = ((double)DisplayXYZ14bit.YAxis) / 1024.0;
					Gees14bData.ZAxis = ((double)DisplayXYZ14bit.ZAxis) / 1024.0;
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
				Count8bData = (XYZCounts)counts8bData;
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
					Gees8bData.XAxis = ((double)DisplayXYZ8bit.XAxis) / 64.0;
					Gees8bData.YAxis = ((double)DisplayXYZ8bit.YAxis) / 64.0;
					Gees8bData.ZAxis = ((double)DisplayXYZ8bit.ZAxis) / 64.0;
				}
				if (FullScaleValue == 1)
				{
					Gees8bData.XAxis = ((double)DisplayXYZ8bit.XAxis) / 32.0;
					Gees8bData.YAxis = ((double)DisplayXYZ8bit.YAxis) / 32.0;
					Gees8bData.ZAxis = ((double)DisplayXYZ8bit.ZAxis) / 32.0;
				}
				if (FullScaleValue == 2)
				{
					Gees8bData.XAxis = ((double)DisplayXYZ8bit.XAxis) / 16.0;
					Gees8bData.YAxis = ((double)DisplayXYZ8bit.YAxis) / 16.0;
					Gees8bData.ZAxis = ((double)DisplayXYZ8bit.ZAxis) / 16.0;
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

		private void uxAverageData_SelectedIndexChanged(object sender, EventArgs e)
		{
			_averageCounter = 0;
		}

		private void uxClearFifoDataList_Click(object sender, EventArgs e)
		{
			uxFifoList.Items.Clear();
		}

		private void uxCopyFifoData_Click(object sender, EventArgs e)
		{
			StringBuilder builder = new StringBuilder();
			DateTime now = DateTime.Now;
			builder.AppendLine(string.Concat(new object[] { now.DayOfWeek, " ", now.Day, " ", now.ToString("MMMM"), " ", now.ToLongTimeString() }));
			if (rdoFIFO14bitDataDisplay.Checked)
				builder.AppendLine(DeviceID.ToString() + " " + rdoXYZFullResMain.Text + " mode");
			else
				builder.AppendLine(DeviceID.ToString() + " 8 bits mode");

			builder.AppendLine("X\tY\tZ");
			foreach (ListViewItem item in uxFifoList.Items)
				builder.AppendLine(string.Format("{0}\t{1}\t{2}", item.SubItems[0].Text, item.SubItems[1].Text, item.SubItems[2].Text));
			Clipboard.SetDataObject(builder.ToString());
		}

		private void uxStart_CheckedChanged(object sender, EventArgs e)
		{
			if (!uxStart.Checked)
			{
				VeyronControllerObj.DisableData();
				uxStart.Text = "Start";
			}
			else
			{
				uxStart.Text = "Stop";
				MainScreenGraph.Enabled = true;
				int[] datapassed = new int[] { SystemPolling };
				if (rdo8bitDataMain.Checked)
				{
					VeyronControllerObj.Data8bFlag = true;
					ControllerReqPacket reqName = new ControllerReqPacket();
					CreateNewTaskFromGUI(reqName, 0, 1, datapassed);
					UpdateMainScreenWaveform(Gees8bData);
				}
				else if (rdoXYZFullResMain.Checked)
				{
					switch (DeviceID)
					{
						case deviceID.MMA8491Q:
							VeyronControllerObj.Data14bFlag = true;
							CreateNewTaskFromGUI(new ControllerReqPacket(), 0, 0x5c, datapassed);
							UpdateMainScreenWaveform(Gees14bData);
							break;
						case deviceID.MMA8451Q:
							VeyronControllerObj.Data14bFlag = true;
							CreateNewTaskFromGUI(new ControllerReqPacket(), 0, 0x5c, datapassed);
							UpdateMainScreenWaveform(Gees14bData);
							break;
						case deviceID.MMA8452Q:
							VeyronControllerObj.Data12bFlag = true;
							CreateNewTaskFromGUI(new ControllerReqPacket(), 0, 2, datapassed);
							UpdateMainScreenWaveform(Gees12bData);
							break;
						case deviceID.MMA8453Q:
							VeyronControllerObj.Data10bFlag = true;
							CreateNewTaskFromGUI(new ControllerReqPacket(), 0, 0x5b, datapassed);
							UpdateMainScreenWaveform(Gees10bData);
							break;
						case deviceID.MMA8652FC:
							VeyronControllerObj.Data12bFlag = true;
							CreateNewTaskFromGUI(new ControllerReqPacket(), 0, 2, datapassed);
							UpdateMainScreenWaveform(Gees12bData);
							break;
						case deviceID.MMA8653FC:
							VeyronControllerObj.Data10bFlag = true;
							CreateNewTaskFromGUI(new ControllerReqPacket(), 0, 0x5b, datapassed);
							UpdateMainScreenWaveform(Gees10bData);
							break;
					}
				}
			}
		}

		private void uxSwitchCounts_StateChanged(object sender, ActionEventArgs e)
		{
			_averageCounter = 0;
		}

		private void VeyronEvaluationSoftware_FormClosing(object sender, FormClosingEventArgs e)
		{
			VeyronControllerObj.DisableData();
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