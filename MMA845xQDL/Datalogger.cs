﻿namespace VeyronDatalogger
{
	using Freescale.SASD.Communication;
	using NationalInstruments.UI;
	using NationalInstruments.UI.WindowsForms;
	using System;
	using System.Collections;
	using System.ComponentModel;
	using System.Diagnostics;
	using System.Drawing;
	using System.IO;
	using System.Reflection;
	using System.Resources;
	using System.Text;
	using System.Threading;
	using System.Windows.Forms;
	using VeyronDatalogger.Properties;

	public class Datalogger : Form
	{

		private enum DemoStateMachine
		{
			FIFO_Mode = 7,
			Init = 2,
			Interrupt_Mode = 6,
			NVM_Mode = 8,
			OffLine = 1,
			Online = 3,
			Poll_Mode = 4,
			Stream_Mode = 5
		}

		private enum eCommMode
		{
			Bootloader = 3,
			Closed = 1,
			FindingHW = 2,
			Running = 4
		}

		private GroupBox AccelControllerGroup;
		private ScatterPlotCollection ActiveWaveArray;
		private bool AutoStopGranted = false;
		private bool AutoStopRequested = false;
		private bool B_btnAdvanced = false;
		private bool B_btnDisplay = true;
		private bool B_btnRegisters = false;
		private bool bClosing = false;
		private int Bit10_2Complement;
		private int Bit10_MaxPositive;
		private int Bit12_2Complement;
		private int Bit12_MaxPositive;
		private int Bit14_2Complement;
		private int Bit14_MaxPositive;
		private int Bit8_2Complement;
		private int Bit8_MaxPositive;
		private int BitFull_2Complement;
		private int BitFull_MaxPositive;
		private Button bRegisters;
		private Button btnAutoCal;
		private Button btnMonitorGetID;
		private Button btnRead;
		private Button btnUpdateRegs;
		private Button btnViewData;
		private Button btnWrite;
		private int[] calValues = new int[3];
		private ComboBox cbRegRead;
		private ComboBox cbRegWrite;
		private CommClass cc;
		private CheckBox chkAnalogLowNoise;
		private CheckBox chkXLegend;
		private CheckBox chkYLegend;
		private CheckBox chkZLegend;
		private ComboBox cmbMonitorIDs;
		private ComboBox cmbReadMethod;
		private eCommMode CommMode = eCommMode.Closed;
		private StatusStrip CommStrip;
		private ToolStripDropDownButton CommStripButton;
		private IContainer components = null;
		private object ControllerEventsLock = new object();
		private AccelController ControllerObj;
		private ScatterPlotCollection CountsWaveArray;
		private string CurrentFW = "4003";
		private bool CurrentlyShowingGValues = true;
		private bool dataStream = false;

		private ComboBox ddlDataRate;
		private ComboBox ddlFIFOPoll;
		private deviceID DeviceID = deviceID.Unsupported;
		private bool DoReconnect = false;
		private BoardComm dv;
		private ToolStripMenuItem enableLogDataApplicationToolStripMenuItem;
		private int FIFOModeValue = 2;

		private const int Form_Max_Height = 750;
		private const int Form_Min_Height = 200;
		private GroupBox gbDR;
		private GroupBox gbOM;
		private GroupBox gbOM2;
		private GroupBox groupBox4;
		private GroupBox grpFifo;
		private ScatterPlotCollection GWaveArray;
		private int I_panelAdvanced_height;
		private int I_panelDisplay_height;
		private int I_panelRegisters_height;
		private Image ImageGreen;
		private Image ImageRed;
		private Image ImageYellow;
		private bool InitializeController = false;
		private InstrumentControlStrip instrumentControlStrip1;
		private bool IsScopeActive = false;
		private Label label1;
		private Label label2;
		private Label label3;
		private Label label4;
		private Label label42;
		private Label label49;
		private Label label5;
		private Label label51;
		private Label label6;
		private Label label7;
		private Label label70;
		private Label label8;
		private Label label9;
		private DateTime LastScan;
		private Label lbl2gRange;
		private Label lbl4gRange;
		private Label lbl8gRange;
		private Label lblConfiguration;
		private Label lblData;
		private Label lblFIFOPoll;
		private Label lblMessage;
		private Label lblMonitorValue;
		private Label lblMonitorValueChange;
		private Label lblRegValue;
		private Label lblSetODR;
		private Label lblWatermark;
		private Label lblWatermarkValue;
		private Legend legend1;
		private LegendItem legendItem1;
		private LegendItem legendItem2;
		private LegendItem legendItem3;
		private bool LoadingFW = false;
		private ToolStripMenuItem logDataToolStripMenuItem1;
		private ScatterGraph MainScreenGraph;
		private ScatterPlot MainScreenXAxis;
		private ScatterPlot MainScreenYAxis;
		private ScatterPlot MainScreenZAxis;
		private const int MAX_HISTORY = 0x9c40;
		private int MAX_ITERATIONS = 5;
		private MenuStrip menuStrip1;
		private ToolStripMenuItem mnFile;
		private MenuStrip mnMenu;
		private ToolStripMenuItem mnuFileNew;
		private ToolStripMenuItem mnuFileSaveAs;
		private int NewPoints = 0;
		private DateTime NextPoint;
		private NationalInstruments.UI.WindowsForms.Switch NIswPlots;
		private object objectLock = new object();
		private OpenFileDialog openFileDialog1;
		private Panel p4;
		private Panel panelAdvanced;
		private Panel panelDisplay;
		private SplitContainer panelGeneral;
		private Panel panelRegisters;
		private PictureBox pictureBox1;
		private Panel pnlBootloader;
		private int PointsInTest = 0;
		private Panel pOverSampling;

		private RadioButton rdo2g;
		private RadioButton rdo4g;
		private RadioButton rdo8g;
		private RadioButton rdoCircular;
		private RadioButton rdoFill;
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

		private DateTime StartTime;
		private ComboBox stream;
		private TrackBar tBWatermark;
		private DateTime TestTime;
		private TextBox textBox1;
		private TextBox textBox2;
		private TextBox textBox3;
		private TextBox textBox4;
		private TextBox textBox5;
		private TextBox textBox6;
		private TimeSpan TimeDeltaPollRate;
		private TimeSpan TimeDeltaSampleRate;

		private int TM_Delay = 0;
		private System.Windows.Forms.Timer tmrTiltTimer;
		private ToolStripContainer toolStripContainer1;
		private ToolStripLabel toolStripLabel1;
		private ToolStripLabel toolStripLabel2;
		private ToolStripLabel toolStripLabel3;
		private ToolStripLabel toolStripLabel4;
		private ToolStripPropertyEditor toolStripPropertyEditor1;
		private ToolStripPropertyEditor toolStripPropertyEditor2;
		private ToolStripPropertyEditor toolStripPropertyEditor3;
		private ToolStripPropertyEditor toolStripPropertyEditor4;
		private ToolStripSeparator toolStripSeparator;
		private ToolStripSeparator toolStripSeparator1;
		private ToolStripStatusLabel toolStripStatusLabel;
		private ToolTip toolTip1;
		private TextBox txtboxRegisters;
		private TextBox txtInfoI2CTransactions;
		private TextBox txtMonitorValue;
		private TextBox txtMonitorValueChanged;
		private Label txtNoP;
		private TextBox txtRegValue;
		private ScatterPlotCollection UnsignedCountsWaveArray;
		private ToolStripMenuItem updateFWToolStripMenuItem;
		private bool windowResized = false;
		private XAxis xAxis;
		private Queue XYZData = new Queue();
		private YAxis yAxis;
		private YAxis yAxis1;

		public Datalogger()
		{
			InitializeComponent();

			base.Width = 848;

			panelGeneral.SplitterDistance = 0x1cf;
			ControllerObj = new AccelController();
			ControllerObj.ControllerEvents += new AccelController.ControllerEventHandler(ControllerObj_ControllerEvents);
			ControllerObj.InitializeController();
			menuStrip1.Enabled = false;
			I_panelDisplay_height = panelDisplay.Height;
			I_panelAdvanced_height = panelAdvanced.Height;
			I_panelRegisters_height = panelRegisters.Height;

			Bit8_2Complement = 0x100;
			Bit8_MaxPositive = 0x7F;
			Bit14_2Complement = 0x4000;
			Bit14_MaxPositive = 0x1FFF;
			Bit12_2Complement = 0x1000;
			Bit12_MaxPositive = 0x7FF;
			Bit10_2Complement = 0x400;
			Bit10_MaxPositive = 0x1FF;

			InitializeForm();

			tmrTiltTimer.Enabled = true;
			tmrTiltTimer.Start();
		}

		private void AddPointToWaveforms(XYZCounts uc, XYZIntCounts ic, XYZGees g)
		{
			if (GWaveArray[0].HistoryCount == GWaveArray[0].HistoryCapacity)
			{
				if (!AutoStopRequested)
				{
					AutoStopRequested = true;
				}
			}
			else
			{
				double historyCount = GWaveArray[0].HistoryCount;
				GWaveArray[0].PlotXYAppend(((double)NextPoint.Ticks) / 10000000.0, g.XAxis);
				GWaveArray[1].PlotXYAppend(((double)NextPoint.Ticks) / 10000000.0, g.YAxis);
				GWaveArray[2].PlotXYAppend(((double)NextPoint.Ticks) / 10000000.0, g.ZAxis);
				CountsWaveArray[0].PlotXYAppend(((double)NextPoint.Ticks) / 10000000.0, (double)ic.XAxis);
				CountsWaveArray[1].PlotXYAppend(((double)NextPoint.Ticks) / 10000000.0, (double)ic.YAxis);
				CountsWaveArray[2].PlotXYAppend(((double)NextPoint.Ticks) / 10000000.0, (double)ic.ZAxis);
				UnsignedCountsWaveArray[0].PlotXYAppend(((double)NextPoint.Ticks) / 10000000.0, (double)uc.XAxis);
				UnsignedCountsWaveArray[1].PlotXYAppend(((double)NextPoint.Ticks) / 10000000.0, (double)uc.YAxis);
				UnsignedCountsWaveArray[2].PlotXYAppend(((double)NextPoint.Ticks) / 10000000.0, (double)uc.ZAxis);
				NewPoints++;
				PointsInTest++;
			}
		}

		private void bRegisters_Click(object sender, EventArgs e)
		{
			if (base.Width == 0x400)
			{
				bRegisters.FlatStyle = FlatStyle.Standard;
				base.Width = 0x350;
			}
			else
			{
				bRegisters.FlatStyle = FlatStyle.Flat;
				base.Width = 0x400;
				cbRegRead.SelectedIndex = 0;
				cbRegWrite.SelectedIndex = 0;
			}
		}

		private void btnAdvanced_Click(object sender, EventArgs e)
		{
			B_btnAdvanced = !B_btnAdvanced;
			windowResized = false;
		}

		private void btnAutoCal_Click(object sender, EventArgs e)
		{
			Cursor = Cursors.WaitCursor;
			int[] datapassed = new int[] { (int)ControllerObj.DeviceID };
			ControllerObj.AutoCalFlag = true;
			ControllerReqPacket reqName = new ControllerReqPacket();
			ControllerObj.CreateNewTaskFromGUI(reqName, 0, 0x52, datapassed);
			Thread.Sleep(0x1b58);
			Cursor = Cursors.Default;
		}

		private void btnDisplay_Click(object sender, EventArgs e)
		{
			B_btnDisplay = !B_btnDisplay;
		}

		private void btnExitDataCfg_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		private void btnMonitorGetID_Click(object sender, EventArgs e)
		{
			string[] id = new string[1];
			cmbMonitorIDs.Items.Clear();
			Freescale.SASD.Communication.Meter.GetIDs(ref id);
			foreach (string str in id)
			{
				cmbMonitorIDs.Items.Add(str);
			}
		}

		private void btnRead_Click(object sender, EventArgs e)
		{
			int num = Convert.ToInt32(cbRegRead.Text, 0x10);
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

		private void btnUpdateRegs_Click(object sender, EventArgs e)
		{
			int[] datapassed = new int[] { 0 };
			ControllerReqPacket reqName = new ControllerReqPacket();
			ControllerObj.CreateNewTaskFromGUI(reqName, 0, 0x10, datapassed);
			txtboxRegisters.Text = "Reading data...";
		}

		private void btnViewData_Click(object sender, EventArgs e)
		{
			if (IsScopeActive)
			{
				if (MainScreenGraph.Plots[0].HistoryCount > 0)
				{
					btnViewData.Text = "Append new points to current Datalog";
				}
				else
				{
					btnViewData.Text = "Start a new Datalog";
				}
				StopTest();
			}
			else if (!IsScopeActive)
			{
				btnViewData.Text = "Stop current Datalog";
				StartTest();
			}
		}

		private void btnWrite_Click(object sender, EventArgs e)
		{
			int num;
			int num2;
			try
			{
				num = Convert.ToInt32(cbRegWrite.Text, 0x10);
				num2 = Convert.ToInt32(txtRegValue.Text, 0x10);
			}
			catch (Exception)
			{
				MessageBox.Show("You must enter a valid value!");
				return;
			}
			int[] datapassed = new int[] { num, num2 };
			ControllerObj.WriteValueFlag = true;
			ControllerReqPacket reqName = new ControllerReqPacket();
			ControllerObj.CreateNewTaskFromGUI(reqName, 0, 0x12, datapassed);
		}

		private void ChangeRange(bool standby)
		{
			int num = 0;
			if (!standby)
			{
				if (rdo2g.Checked)
				{
					num = 0;
				}
				else if (rdo4g.Checked)
				{
					num = 1;
				}
				else if (rdo8g.Checked)
				{
					num = 2;
				}
				if (standby)
				{
					num = 0;
				}
				int[] datapassed = new int[] { num };
				ControllerObj.SetFullScaleValueFlag = true;
				ControllerReqPacket reqName = new ControllerReqPacket();
				ControllerObj.CreateNewTaskFromGUI(reqName, 0, 4, datapassed);
			}
			ControllerObj.SetActiveState(!standby);
		}

		private void CheckStatus()
		{
			bool flag = false;
			if (ddlDataRate.SelectedIndex == 0)
			{
				flag = true;
				if (stream.SelectedIndex == 0)
				{
					ddlDataRate.SelectedIndex = 1;
				}
				else
				{
					ddlDataRate.SelectedIndex = 2;
				}
			}
			else if (ddlDataRate.SelectedIndex == 1)
			{
				if (stream.SelectedIndex == 1)
				{
					flag = true;
					ddlDataRate.SelectedIndex = 2;
				}
			}
			else if (ddlDataRate.SelectedIndex == 6)
			{
				ddlDataRate.SelectedIndex = 7;
				flag = true;
			}
			else if (ddlDataRate.SelectedIndex == 7)
			{
				ddlFIFOPoll.SelectedIndex = 6;
				flag = false;
			}
			if (flag)
			{
				MessageBox.Show("Sorry, this configuration is currently not supported", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}

		private void chkLowCurrent_CheckedChanged(object sender, EventArgs e)
		{
		}

		private void chkXLegend_CheckedChanged(object sender, EventArgs e)
		{
			lock (MainScreenGraph)
			{
				MainScreenGraph.Plots[0].Visible = chkXLegend.Checked;
			}
		}

		private void chkYLegend_CheckedChanged(object sender, EventArgs e)
		{
			lock (MainScreenGraph)
			{
				MainScreenGraph.Plots[1].Visible = chkYLegend.Checked;
			}
		}

		private void chkZLegend_CheckedChanged(object sender, EventArgs e)
		{
			lock (MainScreenGraph)
			{
				MainScreenGraph.Plots[2].Visible = chkZLegend.Checked;
			}
		}

		private void ClearFIFO()
		{
			int num = 0;
			int num2 = 0;
			if (stream.SelectedIndex == 0)
			{
				num = 1;
				num2 = 0x60;
			}
			else if (stream.SelectedIndex == 1)
			{
				num = 5;
				num2 = 192;
			}
			int[] datapassed = new int[] { num };
			for (int i = 0; i < num2; i++)
			{
				ControllerObj.ReadValueFlag = true;
				ControllerReqPacket reqName = new ControllerReqPacket();
				ControllerObj.CreateNewTaskFromGUI(reqName, 0, 0x11, datapassed);
				while (ControllerObj.ReadValueFlag)
				{
				}
			}
		}

		private void cmbReadMethod_SelectedIndexChanged(object sender, EventArgs e)
		{
			CheckStatus();
			if (cmbReadMethod.SelectedIndex == 0)
				p4.Enabled = false;
			else
				p4.Enabled = true;
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

		private void ConfigureDataRate()
		{
			int[] datapassed = new int[1];
			datapassed[0] = ddlDataRate.SelectedIndex;
			ControllerObj.SetDataRateFlag = true;
			ControllerObj.CreateNewTaskFromGUI(new ControllerReqPacket(), 0, 5, datapassed);
		}

		private void ControllerObj_ControllerEvents(ControllerEventType evt, object o)
		{
			int num6;
			int num7;
			object obj2;

			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int index = 0;

			XYZCounts uc = new XYZCounts();
			XYZIntCounts ic = new XYZIntCounts();
			XYZGees g = new XYZGees();
			g.XAxis = 0.0;
			g.YAxis = 0.0;
			g.ZAxis = 0.0;
			g.Mod = 0.0;


			Freescale.SASD.Communication.Meter.MonitorTimeStart(this, "Datalogger->ControllerEvents");

			double num5 = 0.0;
			if (rdo2g.Checked)
				num5 = 2.0;
			else if (rdo4g.Checked)
				num5 = 4.0;
			else if (rdo8g.Checked)
				num5 = 8.0;
			else
				num5 = 2.0;

			switch (((int)evt))
			{
				case 1:
					NextPoint += TimeDeltaPollRate;
					uc.XAxis = ((XYZCounts)o).XAxis;
					uc.YAxis = ((XYZCounts)o).YAxis;
					uc.ZAxis = ((XYZCounts)o).ZAxis;
					num = (uc.XAxis > Bit8_MaxPositive) ? (uc.XAxis - Bit8_2Complement) : uc.XAxis;
					num2 = (uc.YAxis > Bit8_MaxPositive) ? (uc.YAxis - Bit8_2Complement) : uc.YAxis;
					num3 = (uc.ZAxis > Bit8_MaxPositive) ? (uc.ZAxis - Bit8_2Complement) : uc.ZAxis;
					ic.XAxis = num;
					ic.YAxis = num2;
					ic.ZAxis = num3;
					g.XAxis = ((double)num) / (((double)Bit8_MaxPositive) / num5);
					g.YAxis = ((double)num2) / (((double)Bit8_MaxPositive) / num5);
					g.ZAxis = ((double)num3) / (((double)Bit8_MaxPositive) / num5);
					g.Mod = Math.Sqrt(((g.XAxis * g.XAxis) + (g.YAxis * g.YAxis)) + (g.ZAxis * g.ZAxis));
					lock ((obj2 = objectLock))
					{
						AddPointToWaveforms(uc, ic, g);
					}
					goto Label_07C3;

				case 2:
					NextPoint += TimeDeltaPollRate;
					num6 = 0;
					num7 = 0;
					if (ControllerObj.DeviceID != deviceID.MMA8451Q)
					{
						if (ControllerObj.DeviceID == deviceID.MMA8452Q)
						{
							num6 = Bit12_MaxPositive;
							num7 = Bit12_2Complement;
						}
						else if (ControllerObj.DeviceID == deviceID.MMA8652FC)
						{
							num6 = Bit12_MaxPositive;
							num7 = Bit12_2Complement;
						}
						else if (ControllerObj.DeviceID == deviceID.MMA8453Q)
						{
							num6 = Bit10_MaxPositive;
							num7 = Bit10_2Complement;
						}
						else if (ControllerObj.DeviceID == deviceID.MMA8653FC)
						{
							num6 = Bit10_MaxPositive;
							num7 = Bit10_2Complement;
						}
						break;
					}

					num6 = Bit14_MaxPositive;
					num7 = Bit14_2Complement;
					break;

				case 3:
					{
						NextPoint = LastScan + TimeDeltaPollRate;
						LastScan = NextPoint;
						XYZCounts[] countsArray = (XYZCounts[])o;
						XYZIntCounts[] countsArray2 = new XYZIntCounts[countsArray.Length];
						lock ((obj2 = objectLock))
						{
							index = 0;
							if (!ControllerObj.isFastReadOn())
							{
								goto Label_0794;
							}
							while ((index < countsArray.Length) && (countsArray[index] != null))
							{
								g = new XYZGees();
								num = (countsArray[index].XAxis > Bit8_MaxPositive) ? (countsArray[index].XAxis - Bit8_2Complement) : countsArray[index].XAxis;
								num2 = (countsArray[index].YAxis > Bit8_MaxPositive) ? (countsArray[index].YAxis - Bit8_2Complement) : countsArray[index].YAxis;
								num3 = (countsArray[index].ZAxis > Bit8_MaxPositive) ? (countsArray[index].ZAxis - Bit8_2Complement) : countsArray[index].ZAxis;
								countsArray2[index] = new XYZIntCounts();
								countsArray2[index].XAxis = num;
								countsArray2[index].YAxis = num2;
								countsArray2[index].ZAxis = num3;
								g.Mod = Math.Sqrt((double)(((num * num) + (num2 * num2)) + (num3 * num3)));
								g.XAxis = ((double)num) / (((double)Bit8_MaxPositive) / num5);
								g.YAxis = ((double)num2) / (((double)Bit8_MaxPositive) / num5);
								g.ZAxis = ((double)num3) / (((double)Bit8_MaxPositive) / num5);
								AddPointToWaveforms(countsArray[index], countsArray2[index], g);
								NextPoint += TimeDeltaSampleRate;
								index++;
							}
							goto Label_07C3;

						Label_0794:
							while ((index < countsArray.Length) && (countsArray[index] != null))
							{
								g = new XYZGees();
								num = (countsArray[index].XAxis > BitFull_MaxPositive) ? (countsArray[index].XAxis - BitFull_2Complement) : countsArray[index].XAxis;
								num2 = (countsArray[index].YAxis > BitFull_MaxPositive) ? (countsArray[index].YAxis - BitFull_2Complement) : countsArray[index].YAxis;
								num3 = (countsArray[index].ZAxis > BitFull_MaxPositive) ? (countsArray[index].ZAxis - BitFull_2Complement) : countsArray[index].ZAxis;
								countsArray2[index] = new XYZIntCounts();
								countsArray2[index].XAxis = num;
								countsArray2[index].YAxis = num2;
								countsArray2[index].ZAxis = num3;
								g.Mod = Math.Sqrt((double)(((num * num) + (num2 * num2)) + (num3 * num3)));
								g.XAxis = ((double)num) / (((double)BitFull_MaxPositive) / num5);
								g.YAxis = ((double)num2) / (((double)BitFull_MaxPositive) / num5);
								g.ZAxis = ((double)num3) / (((double)BitFull_MaxPositive) / num5);
								AddPointToWaveforms(countsArray[index], countsArray2[index], g);
								NextPoint += TimeDeltaSampleRate;
								index++;
							}
						}
						goto Label_07C3;
					}
				default:
					goto Label_07C3;
			}
			uc.XAxis = ((XYZCounts)o).XAxis;
			uc.YAxis = ((XYZCounts)o).YAxis;
			uc.ZAxis = ((XYZCounts)o).ZAxis;
			num = (uc.XAxis > num6) ? (uc.XAxis - num7) : uc.XAxis;
			num2 = (uc.YAxis > num6) ? (uc.YAxis - num7) : uc.YAxis;
			num3 = (uc.ZAxis > num6) ? (uc.ZAxis - num7) : uc.ZAxis;
			ic.XAxis = num;
			ic.YAxis = num2;
			ic.ZAxis = num3;
			g.Mod = Math.Sqrt((double)(((num * num) + (num2 * num2)) + (num3 * num3)));
			g.XAxis = ((double)num) / (((double)num6) / num5);
			g.YAxis = ((double)num2) / (((double)num6) / num5);
			g.ZAxis = ((double)num3) / (((double)num6) / num5);
			lock ((obj2 = objectLock))
			{
				AddPointToWaveforms(uc, ic, g);
			}

		Label_07C3:
			Freescale.SASD.Communication.Meter.MonitorTimeStop(this, "Datalogger->ControllerEvents");
		}

		private void Datalogger_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (!bClosing)
			{
				bClosing = true;
				ControllerObj.DisableData();
				ControllerObj.EndController();
				Reconnect();
			}
		}

		private void ddlDataRate_SelectedIndexChanged(object sender, EventArgs e)
		{
			ddlFIFOPoll.SelectedIndex = ddlDataRate.SelectedIndex;
			CheckStatus();
		}

		private void ddlFIFOPoll_SelectedIndexChanged(object sender, EventArgs e)
		{
			ControllerObj.SetStreamPollRate((SampleRate)ddlFIFOPoll.SelectedIndex);
		}

		private void ddlHPFilter_SelectedIndexChanged(object sender, EventArgs e)
		{
		}

		private void DecodeGUIPackets()
		{
			GUIUpdatePacket packet = new GUIUpdatePacket();
			ControllerObj.Dequeue_GuiPacket(ref packet);
			if (packet != null)
			{
				if (packet.TaskID == 0x4a)
				{
					int[] numArray = (int[])packet.PayLoad.Dequeue();
					if (numArray[0] == 1)
					{
					}
					if (numArray[1] == 1)
					{
					}
					if (numArray[2] == 1)
					{
					}
				}
				if (packet.TaskID == 0x11)
				{
					byte num = (byte)packet.PayLoad.Dequeue();
					lblRegValue.Text = string.Format("{0:X2}", num);
				}
				if (packet.TaskID == 0x10)
				{
					byte[] buffer = (byte[])packet.PayLoad.Dequeue();
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
					txtboxRegisters.Text = builder.ToString();
				}
				if (packet.TaskID == 80)
				{
					int num3;
					byte[] buffer2 = (byte[])packet.PayLoad.Dequeue();
					int[] numArray2 = new int[0x60];
					XYZGees g = new XYZGees();
					XYZCounts uc = new XYZCounts();
					XYZIntCounts ic = new XYZIntCounts();
					double num7 = 0.0;
					if (rdo2g.Checked)
					{
						num7 = 2.0;
					}
					else if (rdo4g.Checked)
					{
						num7 = 4.0;
					}
					else if (rdo8g.Checked)
					{
						num7 = 8.0;
					}
					else
					{
						num7 = 2.0;
					}
					int num8 = 3;
					if (rdoCircular.Checked)
					{
						num8 = 1;
					}
					else
					{
						num8 = 0x20;
					}
					if (stream.SelectedIndex == 0)
					{
						num3 = Bit8_MaxPositive;
					}
					else
					{
						num3 = Bit14_MaxPositive;
					}
					numArray2 = (int[])packet.PayLoad.Dequeue();
					if (numArray2[0] != 0)
					{
						for (int j = 0; j < num8; j++)
						{
							NextPoint += TimeDeltaPollRate;
							ic.XAxis = numArray2[j * 3];
							ic.YAxis = numArray2[(j * 3) + 1];
							ic.ZAxis = numArray2[(j * 3) + 2];
							g.XAxis = ((double)ic.XAxis) / (((double)num3) / num7);
							g.YAxis = ((double)ic.YAxis) / (((double)num3) / num7);
							g.ZAxis = ((double)ic.ZAxis) / (((double)num3) / num7);
							ushort num4 = (ic.XAxis < 0) ? ((ushort)(num3 - ic.XAxis)) : ((ushort)ic.XAxis);
							ushort num5 = (ic.YAxis < 0) ? ((ushort)(num3 - ic.YAxis)) : ((ushort)ic.YAxis);
							ushort num6 = (ic.ZAxis < 0) ? ((ushort)(num3 - ic.ZAxis)) : ((ushort)ic.ZAxis);
							uc.XAxis = num4;
							uc.YAxis = num5;
							uc.ZAxis = num6;
							AddPointToWaveforms(uc, ic, g);
						}
					}
				}
				if (packet.TaskID == 12)
				{
					calValues = (int[])packet.PayLoad.Dequeue();
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
				ControllerObj.ControllerEvents -= new AccelController.ControllerEventHandler(ControllerObj_ControllerEvents);
				ControllerObj.EndController();
				base.Close();
			}
		}

		private void FIFOConfig(bool fifoEnabled)
		{
			if (fifoEnabled)
			{
				if (rdoFill.Checked)
				{
					FIFOModeValue = 2;
				}
				else if (rdoCircular.Checked)
				{
					FIFOModeValue = 1;
				}
				else
				{
					FIFOModeValue = 0;
				}
			}
			else
			{
				FIFOModeValue = 0;
			}
			int[] datapassed = new int[] { tBWatermark.Value };
			ControllerObj.SetFIFOWatermarkFlag = true;
			ControllerReqPacket reqName = new ControllerReqPacket();
			ControllerObj.CreateNewTaskFromGUI(reqName, 0, 20, datapassed);
			datapassed = new int[] { FIFOModeValue };
			ControllerObj.SetFIFOModeFlag = true;
			ControllerReqPacket packet2 = new ControllerReqPacket();
			ControllerObj.CreateNewTaskFromGUI(packet2, 0, 0x13, datapassed);
		}

		private void FIFODataOutputEnabled(int value)
		{
		}

		~Datalogger()
		{
			ControllerObj.ControllerEvents -= new AccelController.ControllerEventHandler(ControllerObj_ControllerEvents);
		}

		private void hideConfigPanel(object sender, EventArgs e)
		{
			if (panelGeneral.SplitterDistance < 400)
			{
				panelGeneral.SplitterDistance = 0x1cf;
			}
		}

		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Datalogger));
			tmrTiltTimer = new System.Windows.Forms.Timer(components);
			menuStrip1 = new System.Windows.Forms.MenuStrip();
			logDataToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			enableLogDataApplicationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			pictureBox1 = new System.Windows.Forms.PictureBox();
			CommStrip = new System.Windows.Forms.StatusStrip();
			CommStripButton = new System.Windows.Forms.ToolStripDropDownButton();
			toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
			toolTip1 = new System.Windows.Forms.ToolTip(components);
			label6 = new System.Windows.Forms.Label();
			label3 = new System.Windows.Forms.Label();
			label2 = new System.Windows.Forms.Label();
			btnViewData = new System.Windows.Forms.Button();
			rdoFill = new System.Windows.Forms.RadioButton();
			rdoCircular = new System.Windows.Forms.RadioButton();
			lblWatermarkValue = new System.Windows.Forms.Label();
			tBWatermark = new System.Windows.Forms.TrackBar();
			lblWatermark = new System.Windows.Forms.Label();
			label5 = new System.Windows.Forms.Label();
			label4 = new System.Windows.Forms.Label();
			lblFIFOPoll = new System.Windows.Forms.Label();
			textBox6 = new System.Windows.Forms.TextBox();
			stream = new System.Windows.Forms.ComboBox();
			ddlFIFOPoll = new System.Windows.Forms.ComboBox();
			ddlDataRate = new System.Windows.Forms.ComboBox();
			lblData = new System.Windows.Forms.Label();
			lblSetODR = new System.Windows.Forms.Label();
			cmbReadMethod = new System.Windows.Forms.ComboBox();
			label1 = new System.Windows.Forms.Label();
			openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
			mnMenu = new System.Windows.Forms.MenuStrip();
			mnFile = new System.Windows.Forms.ToolStripMenuItem();
			mnuFileNew = new System.Windows.Forms.ToolStripMenuItem();
			toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
			mnuFileSaveAs = new System.Windows.Forms.ToolStripMenuItem();
			toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			updateFWToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
			pnlBootloader = new System.Windows.Forms.Panel();
			label7 = new System.Windows.Forms.Label();
			lblMessage = new System.Windows.Forms.Label();
			panelGeneral = new System.Windows.Forms.SplitContainer();
			panelDisplay = new System.Windows.Forms.Panel();
			txtNoP = new System.Windows.Forms.Label();
			bRegisters = new System.Windows.Forms.Button();
			label8 = new System.Windows.Forms.Label();
			NIswPlots = new NationalInstruments.UI.WindowsForms.Switch();
			instrumentControlStrip1 = new NationalInstruments.UI.WindowsForms.InstrumentControlStrip();
			toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
			toolStripPropertyEditor1 = new NationalInstruments.UI.WindowsForms.ToolStripPropertyEditor();
			MainScreenGraph = new NationalInstruments.UI.WindowsForms.ScatterGraph();
			MainScreenXAxis = new NationalInstruments.UI.ScatterPlot();
			xAxis = new NationalInstruments.UI.XAxis();
			yAxis = new NationalInstruments.UI.YAxis();
			MainScreenYAxis = new NationalInstruments.UI.ScatterPlot();
			MainScreenZAxis = new NationalInstruments.UI.ScatterPlot();
			yAxis1 = new NationalInstruments.UI.YAxis();
			toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
			toolStripPropertyEditor2 = new NationalInstruments.UI.WindowsForms.ToolStripPropertyEditor();
			toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
			toolStripPropertyEditor3 = new NationalInstruments.UI.WindowsForms.ToolStripPropertyEditor();
			toolStripLabel4 = new System.Windows.Forms.ToolStripLabel();
			toolStripPropertyEditor4 = new NationalInstruments.UI.WindowsForms.ToolStripPropertyEditor();
			chkZLegend = new System.Windows.Forms.CheckBox();
			chkYLegend = new System.Windows.Forms.CheckBox();
			chkXLegend = new System.Windows.Forms.CheckBox();
			textBox5 = new System.Windows.Forms.TextBox();
			textBox4 = new System.Windows.Forms.TextBox();
			textBox3 = new System.Windows.Forms.TextBox();
			textBox2 = new System.Windows.Forms.TextBox();
			textBox1 = new System.Windows.Forms.TextBox();
			txtInfoI2CTransactions = new System.Windows.Forms.TextBox();
			legend1 = new NationalInstruments.UI.WindowsForms.Legend();
			legendItem1 = new NationalInstruments.UI.LegendItem();
			legendItem2 = new NationalInstruments.UI.LegendItem();
			legendItem3 = new NationalInstruments.UI.LegendItem();
			lblConfiguration = new System.Windows.Forms.Label();
			panelAdvanced = new System.Windows.Forms.Panel();
			p4 = new System.Windows.Forms.Panel();
			grpFifo = new System.Windows.Forms.GroupBox();
			txtMonitorValueChanged = new System.Windows.Forms.TextBox();
			lblMonitorValueChange = new System.Windows.Forms.Label();
			lblMonitorValue = new System.Windows.Forms.Label();
			txtMonitorValue = new System.Windows.Forms.TextBox();
			cmbMonitorIDs = new System.Windows.Forms.ComboBox();
			btnMonitorGetID = new System.Windows.Forms.Button();
			AccelControllerGroup = new System.Windows.Forms.GroupBox();
			gbOM2 = new System.Windows.Forms.GroupBox();
			btnAutoCal = new System.Windows.Forms.Button();
			chkAnalogLowNoise = new System.Windows.Forms.CheckBox();
			pOverSampling = new System.Windows.Forms.Panel();
			label70 = new System.Windows.Forms.Label();
			rdoOSHiResMode = new System.Windows.Forms.RadioButton();
			rdoOSLPMode = new System.Windows.Forms.RadioButton();
			rdoOSLNLPMode = new System.Windows.Forms.RadioButton();
			rdoOSNormalMode = new System.Windows.Forms.RadioButton();
			gbDR = new System.Windows.Forms.GroupBox();
			lbl4gRange = new System.Windows.Forms.Label();
			rdo2g = new System.Windows.Forms.RadioButton();
			rdo8g = new System.Windows.Forms.RadioButton();
			rdoStandby = new System.Windows.Forms.RadioButton();
			rdo4g = new System.Windows.Forms.RadioButton();
			lbl2gRange = new System.Windows.Forms.Label();
			lbl8gRange = new System.Windows.Forms.Label();
			gbOM = new System.Windows.Forms.GroupBox();
			panelRegisters = new System.Windows.Forms.Panel();
			groupBox4 = new System.Windows.Forms.GroupBox();
			cbRegWrite = new System.Windows.Forms.ComboBox();
			cbRegRead = new System.Windows.Forms.ComboBox();
			label9 = new System.Windows.Forms.Label();
			btnWrite = new System.Windows.Forms.Button();
			txtboxRegisters = new System.Windows.Forms.TextBox();
			lblRegValue = new System.Windows.Forms.Label();
			btnUpdateRegs = new System.Windows.Forms.Button();
			btnRead = new System.Windows.Forms.Button();
			label51 = new System.Windows.Forms.Label();
			label49 = new System.Windows.Forms.Label();
			txtRegValue = new System.Windows.Forms.TextBox();
			label42 = new System.Windows.Forms.Label();
			menuStrip1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(pictureBox1)).BeginInit();
			CommStrip.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(tBWatermark)).BeginInit();
			mnMenu.SuspendLayout();
			toolStripContainer1.SuspendLayout();
			pnlBootloader.SuspendLayout();
			panelGeneral.Panel1.SuspendLayout();
			panelGeneral.Panel2.SuspendLayout();
			panelGeneral.SuspendLayout();
			panelDisplay.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(NIswPlots)).BeginInit();
			instrumentControlStrip1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(MainScreenGraph)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(legend1)).BeginInit();
			panelAdvanced.SuspendLayout();
			p4.SuspendLayout();
			grpFifo.SuspendLayout();
			AccelControllerGroup.SuspendLayout();
			gbOM2.SuspendLayout();
			pOverSampling.SuspendLayout();
			gbDR.SuspendLayout();
			gbOM.SuspendLayout();
			panelRegisters.SuspendLayout();
			groupBox4.SuspendLayout();
			SuspendLayout();
			// 
			// tmrTiltTimer
			// 
			tmrTiltTimer.Interval = 1;
			tmrTiltTimer.Tick += new System.EventHandler(tmrTiltTimer_Tick);
			// 
			// menuStrip1
			// 
			menuStrip1.BackColor = System.Drawing.SystemColors.ButtonFace;
			menuStrip1.Enabled = false;
			menuStrip1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            logDataToolStripMenuItem1});
			menuStrip1.Location = new System.Drawing.Point(0, 0);
			menuStrip1.Name = "menuStrip1";
			menuStrip1.Size = new System.Drawing.Size(966, 24);
			menuStrip1.TabIndex = 64;
			menuStrip1.Text = "menuStrip1";
			menuStrip1.Visible = false;
			// 
			// logDataToolStripMenuItem1
			// 
			logDataToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            enableLogDataApplicationToolStripMenuItem});
			logDataToolStripMenuItem1.Name = "logDataToolStripMenuItem1";
			logDataToolStripMenuItem1.Size = new System.Drawing.Size(75, 20);
			logDataToolStripMenuItem1.Text = "Log Data";
			// 
			// enableLogDataApplicationToolStripMenuItem
			// 
			enableLogDataApplicationToolStripMenuItem.Name = "enableLogDataApplicationToolStripMenuItem";
			enableLogDataApplicationToolStripMenuItem.Size = new System.Drawing.Size(265, 22);
			enableLogDataApplicationToolStripMenuItem.Text = "Log Data XYZ with Tilt Detection";
			enableLogDataApplicationToolStripMenuItem.Click += new System.EventHandler(enableLogDataApplicationToolStripMenuItem_Click);
			// 
			// pictureBox1
			// 
			pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
			| System.Windows.Forms.AnchorStyles.Right)));
			pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
			pictureBox1.Location = new System.Drawing.Point(0, 27);
			pictureBox1.Name = "pictureBox1";
			pictureBox1.Size = new System.Drawing.Size(1018, 57);
			pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			pictureBox1.TabIndex = 72;
			pictureBox1.TabStop = false;
			// 
			// CommStrip
			// 
			CommStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            CommStripButton,
            toolStripStatusLabel});
			CommStrip.Location = new System.Drawing.Point(0, 566);
			CommStrip.Name = "CommStrip";
			CommStrip.Size = new System.Drawing.Size(1018, 25);
			CommStrip.TabIndex = 76;
			CommStrip.Text = "statusStrip1";
			CommStrip.MouseEnter += new System.EventHandler(hideConfigPanel);
			// 
			// CommStripButton
			// 
			CommStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			CommStripButton.Image = ((System.Drawing.Image)(resources.GetObject("CommStripButton.Image")));
			CommStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			CommStripButton.Name = "CommStripButton";
			CommStripButton.ShowDropDownArrow = false;
			CommStripButton.Size = new System.Drawing.Size(20, 23);
			CommStripButton.Text = "toolStripDropDownButton1";
			CommStripButton.Click += new System.EventHandler(CommStripButton_Click);
			// 
			// toolStripStatusLabel
			// 
			toolStripStatusLabel.BackColor = System.Drawing.SystemColors.ButtonFace;
			toolStripStatusLabel.Name = "toolStripStatusLabel";
			toolStripStatusLabel.Size = new System.Drawing.Size(284, 20);
			toolStripStatusLabel.Text = "COM Port Not Connected, Please Connect";
			// 
			// toolTip1
			// 
			// 
			// label6
			// 
			label6.AutoSize = true;
			label6.Location = new System.Drawing.Point(638, 386);
			label6.Name = "label6";
			label6.Size = new System.Drawing.Size(90, 13);
			label6.TabIndex = 252;
			label6.Text = "Number of points:";
			toolTip1.SetToolTip(label6, "Number of points shown in the scope.");
			// 
			// label3
			// 
			label3.AutoSize = true;
			label3.BackColor = System.Drawing.Color.Khaki;
			label3.Location = new System.Drawing.Point(519, 412);
			label3.Name = "label3";
			label3.Size = new System.Drawing.Size(100, 13);
			label3.TabIndex = 251;
			label3.Text = "Show Count Values";
			toolTip1.SetToolTip(label3, "Show data as read from sensor (counts)");
			// 
			// label2
			// 
			label2.AutoSize = true;
			label2.BackColor = System.Drawing.Color.DarkSeaGreen;
			label2.Location = new System.Drawing.Point(519, 381);
			label2.Name = "label2";
			label2.Size = new System.Drawing.Size(79, 13);
			label2.TabIndex = 250;
			label2.Text = "Show G values";
			toolTip1.SetToolTip(label2, "Show accelerometer data in \"g\" units.");
			// 
			// btnViewData
			// 
			btnViewData.BackColor = System.Drawing.Color.DarkGray;
			btnViewData.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
			btnViewData.FlatAppearance.BorderSize = 5;
			btnViewData.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			btnViewData.ForeColor = System.Drawing.Color.White;
			btnViewData.Location = new System.Drawing.Point(3, 379);
			btnViewData.Name = "btnViewData";
			btnViewData.Size = new System.Drawing.Size(218, 52);
			btnViewData.TabIndex = 236;
			btnViewData.Text = "Start a new Datalog";
			toolTip1.SetToolTip(btnViewData, "Click here to initialize/stop test.");
			btnViewData.UseVisualStyleBackColor = false;
			btnViewData.Click += new System.EventHandler(btnViewData_Click);
			// 
			// rdoFill
			// 
			rdoFill.AutoSize = true;
			rdoFill.Checked = true;
			rdoFill.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			rdoFill.ForeColor = System.Drawing.Color.Black;
			rdoFill.Location = new System.Drawing.Point(5, 23);
			rdoFill.Name = "rdoFill";
			rdoFill.Size = new System.Drawing.Size(98, 22);
			rdoFill.TabIndex = 38;
			rdoFill.TabStop = true;
			rdoFill.Text = "Fill Buffer";
			toolTip1.SetToolTip(rdoFill, "Allows samples to hold up to 32 values for X,Y and Z");
			rdoFill.UseVisualStyleBackColor = true;
			rdoFill.Click += new System.EventHandler(rdoFill_CheckedChanged);
			// 
			// rdoCircular
			// 
			rdoCircular.AutoSize = true;
			rdoCircular.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			rdoCircular.ForeColor = System.Drawing.Color.Black;
			rdoCircular.Location = new System.Drawing.Point(117, 23);
			rdoCircular.Name = "rdoCircular";
			rdoCircular.Size = new System.Drawing.Size(135, 22);
			rdoCircular.TabIndex = 37;
			rdoCircular.Text = "Circular Buffer";
			toolTip1.SetToolTip(rdoCircular, "Allows values to continuously overwrite holding the last 32 values");
			rdoCircular.UseVisualStyleBackColor = true;
			rdoCircular.Click += new System.EventHandler(rdoCircular_CheckedChanged);
			// 
			// lblWatermarkValue
			// 
			lblWatermarkValue.AutoSize = true;
			lblWatermarkValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			lblWatermarkValue.ForeColor = System.Drawing.Color.Black;
			lblWatermarkValue.Location = new System.Drawing.Point(403, 25);
			lblWatermarkValue.Name = "lblWatermarkValue";
			lblWatermarkValue.Size = new System.Drawing.Size(26, 18);
			lblWatermarkValue.TabIndex = 50;
			lblWatermarkValue.Text = "32";
			toolTip1.SetToolTip(lblWatermarkValue, "This is the set value for # of Samples in the FIFO to trigger the Flag");
			// 
			// tBWatermark
			// 
			tBWatermark.Location = new System.Drawing.Point(438, 15);
			tBWatermark.Maximum = 32;
			tBWatermark.Minimum = 1;
			tBWatermark.Name = "tBWatermark";
			tBWatermark.Size = new System.Drawing.Size(378, 45);
			tBWatermark.TabIndex = 32;
			tBWatermark.TickFrequency = 2;
			toolTip1.SetToolTip(tBWatermark, "Watermark value on FIFO and also the amount of points to read from FIFO at each p" +
		"oll read. ");
			tBWatermark.Value = 32;
			tBWatermark.Scroll += new System.EventHandler(tBWatermark_Scroll);
			// 
			// lblWatermark
			// 
			lblWatermark.AutoSize = true;
			lblWatermark.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			lblWatermark.ForeColor = System.Drawing.Color.Black;
			lblWatermark.Location = new System.Drawing.Point(291, 25);
			lblWatermark.Name = "lblWatermark";
			lblWatermark.Size = new System.Drawing.Size(101, 18);
			lblWatermark.TabIndex = 42;
			lblWatermark.Text = "Watermark: ";
			toolTip1.SetToolTip(lblWatermark, "This is the # of Sample to trigger the flag in the FIFO");
			// 
			// label5
			// 
			label5.AutoSize = true;
			label5.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			label5.Location = new System.Drawing.Point(351, 48);
			label5.Name = "label5";
			label5.Size = new System.Drawing.Size(26, 16);
			label5.TabIndex = 246;
			label5.Text = "ms";
			toolTip1.SetToolTip(label5, "Type in the number of miliseconds that the test will be active (0-65535).  When t" +
		"he MCU detects that time is over, the data will stop being transferred to the PC" +
		".");
			label5.Visible = false;
			// 
			// label4
			// 
			label4.AutoSize = true;
			label4.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			label4.Location = new System.Drawing.Point(271, 27);
			label4.Name = "label4";
			label4.Size = new System.Drawing.Size(128, 16);
			label4.TabIndex = 245;
			label4.Text = "Test Time (0=Off)";
			toolTip1.SetToolTip(label4, "The time that the test will last. Min value 1ms Max value is 65535. 0ms means tha" +
		"t there is no automatic end of test and the board will continue to send the sens" +
		"or data to the PC Host.");
			label4.Visible = false;
			// 
			// lblFIFOPoll
			// 
			lblFIFOPoll.AutoSize = true;
			lblFIFOPoll.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			lblFIFOPoll.ForeColor = System.Drawing.Color.Black;
			lblFIFOPoll.Location = new System.Drawing.Point(143, 74);
			lblFIFOPoll.Name = "lblFIFOPoll";
			lblFIFOPoll.Size = new System.Drawing.Size(62, 16);
			lblFIFOPoll.TabIndex = 244;
			lblFIFOPoll.Text = "Poll rate";
			toolTip1.SetToolTip(lblFIFOPoll, "Frequency at which the MCU will poll the sensor for data, using the method descri" +
		"bed.");
			lblFIFOPoll.Visible = false;
			// 
			// textBox6
			// 
			textBox6.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			textBox6.Location = new System.Drawing.Point(274, 43);
			textBox6.MaxLength = 5;
			textBox6.Name = "textBox6";
			textBox6.Size = new System.Drawing.Size(68, 23);
			textBox6.TabIndex = 241;
			textBox6.Text = "0";
			toolTip1.SetToolTip(textBox6, "Type in the number of miliseconds that the test will be active (0-65535).  When t" +
		"he MCU detects that time is over, the data will stop being transferred to the PC" +
		".");
			textBox6.Visible = false;
			// 
			// stream
			// 
			stream.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			stream.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			stream.FormattingEnabled = true;
			stream.Items.AddRange(new object[] {
            "8 Bit",
            "12 Bit"});
			stream.Location = new System.Drawing.Point(7, 41);
			stream.Name = "stream";
			stream.Size = new System.Drawing.Size(121, 24);
			stream.TabIndex = 198;
			toolTip1.SetToolTip(stream, "Choose what data source you need: 8 Bit or 12 Bit data");
			stream.SelectionChangeCommitted += new System.EventHandler(stream_SelectionChangeCommitted);
			// 
			// ddlFIFOPoll
			// 
			ddlFIFOPoll.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			ddlFIFOPoll.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			ddlFIFOPoll.FormattingEnabled = true;
			ddlFIFOPoll.Items.AddRange(new object[] {
            "Off",
            "400 Hz",
            "200 Hz",
            "100 Hz",
            "50 Hz",
            "12.5 Hz",
            "1.563 Hz",
            "Interrupt Mode"});
			ddlFIFOPoll.Location = new System.Drawing.Point(146, 90);
			ddlFIFOPoll.Name = "ddlFIFOPoll";
			ddlFIFOPoll.Size = new System.Drawing.Size(98, 24);
			ddlFIFOPoll.TabIndex = 243;
			toolTip1.SetToolTip(ddlFIFOPoll, "Frequency at which the MCU will poll the sensor for data, using the method descri" +
		"bed.");
			ddlFIFOPoll.Visible = false;
			ddlFIFOPoll.SelectedIndexChanged += new System.EventHandler(ddlFIFOPoll_SelectedIndexChanged);
			// 
			// ddlDataRate
			// 
			ddlDataRate.DisplayMember = "(none)";
			ddlDataRate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			ddlDataRate.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			ddlDataRate.FormattingEnabled = true;
			ddlDataRate.Items.AddRange(new object[] {
            "800 Hz",
            "400 Hz",
            "200 Hz",
            "100 Hz",
            "50 Hz",
            "12.5 Hz",
            "6.25 Hz",
            "1.563 Hz"});
			ddlDataRate.Location = new System.Drawing.Point(146, 42);
			ddlDataRate.Name = "ddlDataRate";
			ddlDataRate.Size = new System.Drawing.Size(98, 24);
			ddlDataRate.TabIndex = 125;
			toolTip1.SetToolTip(ddlDataRate, "Sensor sample rate (frequency at which a new data point is available)");
			ddlDataRate.SelectedIndexChanged += new System.EventHandler(ddlDataRate_SelectedIndexChanged);
			// 
			// lblData
			// 
			lblData.AutoSize = true;
			lblData.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			lblData.ForeColor = System.Drawing.Color.Black;
			lblData.Location = new System.Drawing.Point(6, 26);
			lblData.Name = "lblData";
			lblData.Size = new System.Drawing.Size(88, 16);
			lblData.TabIndex = 199;
			lblData.Text = "Data source";
			toolTip1.SetToolTip(lblData, "Choose what data source you need: 8 Bit or 12 Bit data");
			// 
			// lblSetODR
			// 
			lblSetODR.AutoSize = true;
			lblSetODR.BackColor = System.Drawing.Color.AntiqueWhite;
			lblSetODR.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			lblSetODR.ForeColor = System.Drawing.Color.Black;
			lblSetODR.Location = new System.Drawing.Point(143, 26);
			lblSetODR.Name = "lblSetODR";
			lblSetODR.Size = new System.Drawing.Size(88, 16);
			lblSetODR.TabIndex = 126;
			lblSetODR.Text = "Sensor Rate";
			toolTip1.SetToolTip(lblSetODR, "Sensor sample rate (frequency at which a new data point is available)");
			// 
			// cmbReadMethod
			// 
			cmbReadMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			cmbReadMethod.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			cmbReadMethod.FormattingEnabled = true;
			cmbReadMethod.Items.AddRange(new object[] {
            "ODR",
            "FIFO"});
			cmbReadMethod.Location = new System.Drawing.Point(7, 90);
			cmbReadMethod.Name = "cmbReadMethod";
			cmbReadMethod.Size = new System.Drawing.Size(121, 24);
			cmbReadMethod.TabIndex = 239;
			toolTip1.SetToolTip(cmbReadMethod, "Select whether you want the data to be read ODR (single point) or FIFO (multiple " +
		"points)");
			cmbReadMethod.SelectedIndexChanged += new System.EventHandler(cmbReadMethod_SelectedIndexChanged);
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			label1.ForeColor = System.Drawing.Color.Black;
			label1.Location = new System.Drawing.Point(6, 74);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size(57, 16);
			label1.TabIndex = 240;
			label1.Text = "Method";
			toolTip1.SetToolTip(label1, "Select whether you want the data to be read ODR (single point) or FIFO (multiple " +
		"points)");
			// 
			// openFileDialog1
			// 
			openFileDialog1.FileName = "openFileDialog1";
			// 
			// mnMenu
			// 
			mnMenu.Dock = System.Windows.Forms.DockStyle.None;
			mnMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            mnFile,
            updateFWToolStripMenuItem});
			mnMenu.Location = new System.Drawing.Point(0, 3);
			mnMenu.Name = "mnMenu";
			mnMenu.Size = new System.Drawing.Size(52, 28);
			mnMenu.TabIndex = 5;
			mnMenu.Text = "File";
			mnMenu.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(mnMenu_ItemClicked);
			// 
			// mnFile
			// 
			mnFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            mnuFileNew,
            toolStripSeparator,
            mnuFileSaveAs,
            toolStripSeparator1});
			mnFile.Name = "mnFile";
			mnFile.Size = new System.Drawing.Size(44, 24);
			mnFile.Text = "&File";
			// 
			// mnuFileNew
			// 
			mnuFileNew.ImageTransparentColor = System.Drawing.Color.Magenta;
			mnuFileNew.Name = "mnuFileNew";
			mnuFileNew.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
			mnuFileNew.Size = new System.Drawing.Size(161, 24);
			mnuFileNew.Text = "&New";
			mnuFileNew.Click += new System.EventHandler(mnuFileNew_Click);
			// 
			// toolStripSeparator
			// 
			toolStripSeparator.Name = "toolStripSeparator";
			toolStripSeparator.Size = new System.Drawing.Size(158, 6);
			// 
			// mnuFileSaveAs
			// 
			mnuFileSaveAs.Name = "mnuFileSaveAs";
			mnuFileSaveAs.Size = new System.Drawing.Size(161, 24);
			mnuFileSaveAs.Text = "Save &As";
			mnuFileSaveAs.Click += new System.EventHandler(mnuFileSaveAs_Click);
			// 
			// toolStripSeparator1
			// 
			toolStripSeparator1.Name = "toolStripSeparator1";
			toolStripSeparator1.Size = new System.Drawing.Size(158, 6);
			// 
			// updateFWToolStripMenuItem
			// 
			updateFWToolStripMenuItem.Name = "updateFWToolStripMenuItem";
			updateFWToolStripMenuItem.Size = new System.Drawing.Size(95, 24);
			updateFWToolStripMenuItem.Text = "Update FW";
			updateFWToolStripMenuItem.Visible = false;
			updateFWToolStripMenuItem.Click += new System.EventHandler(updateFWToolStripMenuItem_Click);
			// 
			// toolStripContainer1
			// 
			// 
			// toolStripContainer1.ContentPanel
			// 
			toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(965, 0);
			toolStripContainer1.Location = new System.Drawing.Point(1, 3);
			toolStripContainer1.Name = "toolStripContainer1";
			toolStripContainer1.Size = new System.Drawing.Size(965, 24);
			toolStripContainer1.TabIndex = 78;
			toolStripContainer1.Text = "toolStripContainer1";
			// 
			// pnlBootloader
			// 
			pnlBootloader.BackColor = System.Drawing.Color.White;
			pnlBootloader.Controls.Add(label7);
			pnlBootloader.Location = new System.Drawing.Point(227, 40);
			pnlBootloader.Name = "pnlBootloader";
			pnlBootloader.Size = new System.Drawing.Size(439, 29);
			pnlBootloader.TabIndex = 217;
			pnlBootloader.Visible = false;
			// 
			// label7
			// 
			label7.AutoSize = true;
			label7.Cursor = System.Windows.Forms.Cursors.Hand;
			label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			label7.ForeColor = System.Drawing.Color.Blue;
			label7.Location = new System.Drawing.Point(6, 5);
			label7.Name = "label7";
			label7.Size = new System.Drawing.Size(429, 20);
			label7.TabIndex = 0;
			label7.Text = "The board is in Bootloader mode. Click here to upgrade FW.";
			label7.Click += new System.EventHandler(label7_Click);
			// 
			// lblMessage
			// 
			lblMessage.AutoSize = true;
			lblMessage.BackColor = System.Drawing.Color.White;
			lblMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			lblMessage.ForeColor = System.Drawing.Color.Red;
			lblMessage.Location = new System.Drawing.Point(258, 92);
			lblMessage.Name = "lblMessage";
			lblMessage.Size = new System.Drawing.Size(0, 13);
			lblMessage.TabIndex = 218;
			lblMessage.Visible = false;
			// 
			// panelGeneral
			// 
			panelGeneral.IsSplitterFixed = true;
			panelGeneral.Location = new System.Drawing.Point(1, 85);
			panelGeneral.Name = "panelGeneral";
			panelGeneral.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// panelGeneral.Panel1
			// 
			panelGeneral.Panel1.Controls.Add(panelDisplay);
			panelGeneral.Panel1.MouseEnter += new System.EventHandler(hideConfigPanel);
			// 
			// panelGeneral.Panel2
			// 
			panelGeneral.Panel2.Controls.Add(lblConfiguration);
			panelGeneral.Panel2.Controls.Add(panelAdvanced);
			panelGeneral.Panel2.MouseEnter += new System.EventHandler(showConfigPanel);
			panelGeneral.Panel2MinSize = 1;
			panelGeneral.Size = new System.Drawing.Size(843, 485);
			panelGeneral.SplitterDistance = 463;
			panelGeneral.SplitterWidth = 1;
			panelGeneral.TabIndex = 219;
			// 
			// panelDisplay
			// 
			panelDisplay.Controls.Add(txtNoP);
			panelDisplay.Controls.Add(bRegisters);
			panelDisplay.Controls.Add(label8);
			panelDisplay.Controls.Add(label6);
			panelDisplay.Controls.Add(label3);
			panelDisplay.Controls.Add(label2);
			panelDisplay.Controls.Add(NIswPlots);
			panelDisplay.Controls.Add(instrumentControlStrip1);
			panelDisplay.Controls.Add(chkZLegend);
			panelDisplay.Controls.Add(chkYLegend);
			panelDisplay.Controls.Add(chkXLegend);
			panelDisplay.Controls.Add(textBox5);
			panelDisplay.Controls.Add(textBox4);
			panelDisplay.Controls.Add(textBox3);
			panelDisplay.Controls.Add(textBox2);
			panelDisplay.Controls.Add(textBox1);
			panelDisplay.Controls.Add(txtInfoI2CTransactions);
			panelDisplay.Controls.Add(btnViewData);
			panelDisplay.Controls.Add(legend1);
			panelDisplay.Controls.Add(MainScreenGraph);
			panelDisplay.Location = new System.Drawing.Point(1, 3);
			panelDisplay.Name = "panelDisplay";
			panelDisplay.Size = new System.Drawing.Size(840, 432);
			panelDisplay.TabIndex = 78;
			// 
			// txtNoP
			// 
			txtNoP.AutoSize = true;
			txtNoP.BackColor = System.Drawing.Color.LightSlateGray;
			txtNoP.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			txtNoP.ForeColor = System.Drawing.Color.Maroon;
			txtNoP.Location = new System.Drawing.Point(734, 386);
			txtNoP.Name = "txtNoP";
			txtNoP.Size = new System.Drawing.Size(14, 13);
			txtNoP.TabIndex = 256;
			txtNoP.Text = "0";
			// 
			// bRegisters
			// 
			bRegisters.Location = new System.Drawing.Point(760, 0);
			bRegisters.Name = "bRegisters";
			bRegisters.Size = new System.Drawing.Size(75, 25);
			bRegisters.TabIndex = 255;
			bRegisters.Text = "Registers...";
			bRegisters.UseVisualStyleBackColor = true;
			bRegisters.Click += new System.EventHandler(bRegisters_Click);
			// 
			// label8
			// 
			label8.AutoSize = true;
			label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			label8.Location = new System.Drawing.Point(638, 410);
			label8.Name = "label8";
			label8.Size = new System.Drawing.Size(147, 13);
			label8.TabIndex = 254;
			label8.Text = "Ctrl + N to start a new datalog";
			// 
			// NIswPlots
			// 
			NIswPlots.Location = new System.Drawing.Point(479, 381);
			NIswPlots.Name = "NIswPlots";
			NIswPlots.OffColor = System.Drawing.Color.Khaki;
			NIswPlots.OnColor = System.Drawing.Color.DarkSeaGreen;
			NIswPlots.Size = new System.Drawing.Size(40, 42);
			NIswPlots.SwitchStyle = NationalInstruments.UI.SwitchStyle.VerticalSlide3D;
			NIswPlots.TabIndex = 249;
			NIswPlots.Value = true;
			// 
			// instrumentControlStrip1
			// 
			instrumentControlStrip1.AutoSize = false;
			instrumentControlStrip1.Dock = System.Windows.Forms.DockStyle.None;
			instrumentControlStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            toolStripLabel1,
            toolStripPropertyEditor1,
            toolStripLabel2,
            toolStripPropertyEditor2,
            toolStripLabel3,
            toolStripPropertyEditor3,
            toolStripLabel4,
            toolStripPropertyEditor4});
			instrumentControlStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
			instrumentControlStrip1.Location = new System.Drawing.Point(0, 0);
			instrumentControlStrip1.Name = "instrumentControlStrip1";
			instrumentControlStrip1.Size = new System.Drawing.Size(757, 25);
			instrumentControlStrip1.TabIndex = 79;
			// 
			// toolStripLabel1
			// 
			toolStripLabel1.Name = "toolStripLabel1";
			toolStripLabel1.Size = new System.Drawing.Size(122, 22);
			toolStripLabel1.Text = "InteractionMode:";
			// 
			// toolStripPropertyEditor1
			// 
			toolStripPropertyEditor1.AutoSize = false;
			toolStripPropertyEditor1.Name = "toolStripPropertyEditor1";
			toolStripPropertyEditor1.RenderMode = NationalInstruments.UI.PropertyEditorRenderMode.Inherit;
			toolStripPropertyEditor1.Size = new System.Drawing.Size(120, 21);
			toolStripPropertyEditor1.Source = new NationalInstruments.UI.PropertyEditorSource(MainScreenGraph, "InteractionMode");
			toolStripPropertyEditor1.Text = "ZoomX, ZoomY, ZoomAroundPoint, PanX, PanY, DragCursor, DragAnnotationCaption, Edi" +
	"tRange";
			// 
			// MainScreenGraph
			// 
			MainScreenGraph.ImmediateUpdates = true;
			MainScreenGraph.InteractionMode = ((NationalInstruments.UI.GraphInteractionModes)((((((((NationalInstruments.UI.GraphInteractionModes.ZoomX | NationalInstruments.UI.GraphInteractionModes.ZoomY)
			| NationalInstruments.UI.GraphInteractionModes.ZoomAroundPoint)
			| NationalInstruments.UI.GraphInteractionModes.PanX)
			| NationalInstruments.UI.GraphInteractionModes.PanY)
			| NationalInstruments.UI.GraphInteractionModes.DragCursor)
			| NationalInstruments.UI.GraphInteractionModes.DragAnnotationCaption)
			| NationalInstruments.UI.GraphInteractionModes.EditRange)));
			MainScreenGraph.Location = new System.Drawing.Point(3, 28);
			MainScreenGraph.Name = "MainScreenGraph";
			MainScreenGraph.Plots.AddRange(new NationalInstruments.UI.ScatterPlot[] {
            MainScreenXAxis,
            MainScreenYAxis,
            MainScreenZAxis});
			MainScreenGraph.Size = new System.Drawing.Size(833, 351);
			MainScreenGraph.TabIndex = 234;
			MainScreenGraph.XAxes.AddRange(new NationalInstruments.UI.XAxis[] {
            xAxis});
			MainScreenGraph.YAxes.AddRange(new NationalInstruments.UI.YAxis[] {
            yAxis,
            yAxis1});
			MainScreenGraph.PlotDataChanged += new NationalInstruments.UI.XYPlotDataChangedEventHandler(MainScreenGraph_PlotDataChanged);
			MainScreenGraph.MouseEnter += new System.EventHandler(hideConfigPanel);
			// 
			// MainScreenXAxis
			// 
			MainScreenXAxis.CanScaleYAxis = true;
			MainScreenXAxis.HistoryCapacity = 100000;
			MainScreenXAxis.LineColor = System.Drawing.Color.Orange;
			MainScreenXAxis.XAxis = xAxis;
			MainScreenXAxis.YAxis = yAxis;
			// 
			// xAxis
			// 
			xAxis.Caption = "X axis";
			xAxis.EditRangeDateTimeFormatMode = NationalInstruments.UI.DateTimeFormatMode.CreateLongTimeMode();
			xAxis.LogBase = 100D;
			xAxis.MajorDivisions.LabelFormat = new NationalInstruments.UI.FormatString(NationalInstruments.UI.FormatStringMode.DateTime, "mm:ss.fff");
			xAxis.MinorDivisions.GridLineStyle = NationalInstruments.UI.LineStyle.Dot;
			xAxis.MinorDivisions.GridVisible = true;
			xAxis.Mode = NationalInstruments.UI.AxisMode.AutoScaleExact;
			xAxis.Range = NationalInstruments.UI.Range.Parse("0; 500");
			// 
			// yAxis
			// 
			yAxis.Caption = "Acceleration (g)";
			yAxis.CaptionBackColor = System.Drawing.Color.DarkSeaGreen;
			yAxis.CaptionFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			yAxis.MajorDivisions.GridLineStyle = NationalInstruments.UI.LineStyle.Dot;
			yAxis.MajorDivisions.GridVisible = true;
			yAxis.MajorDivisions.Interval = 1D;
			yAxis.MinorDivisions.Interval = 0.5D;
			yAxis.MinorDivisions.TickVisible = true;
			yAxis.Mode = NationalInstruments.UI.AxisMode.Fixed;
			yAxis.OriginLineVisible = true;
			yAxis.Range = NationalInstruments.UI.Range.Parse("-2; 2");
			// 
			// MainScreenYAxis
			// 
			MainScreenYAxis.CanScaleYAxis = true;
			MainScreenYAxis.HistoryCapacity = 100000;
			MainScreenYAxis.XAxis = xAxis;
			MainScreenYAxis.YAxis = yAxis;
			// 
			// MainScreenZAxis
			// 
			MainScreenZAxis.CanScaleYAxis = true;
			MainScreenZAxis.HistoryCapacity = 100000;
			MainScreenZAxis.LineColor = System.Drawing.Color.DarkTurquoise;
			MainScreenZAxis.XAxis = xAxis;
			MainScreenZAxis.YAxis = yAxis;
			// 
			// yAxis1
			// 
			yAxis1.Caption = "Acceleration (counts)";
			yAxis1.CaptionBackColor = System.Drawing.Color.Khaki;
			yAxis1.CaptionFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			yAxis1.Mode = NationalInstruments.UI.AxisMode.Fixed;
			yAxis1.Range = NationalInstruments.UI.Range.Parse("-128; 127");
			yAxis1.Visible = false;
			// 
			// toolStripLabel2
			// 
			toolStripLabel2.Name = "toolStripLabel2";
			toolStripLabel2.Size = new System.Drawing.Size(60, 22);
			toolStripLabel2.Text = "Cursors:";
			// 
			// toolStripPropertyEditor2
			// 
			toolStripPropertyEditor2.AutoSize = false;
			toolStripPropertyEditor2.Name = "toolStripPropertyEditor2";
			toolStripPropertyEditor2.RenderMode = NationalInstruments.UI.PropertyEditorRenderMode.Inherit;
			toolStripPropertyEditor2.Size = new System.Drawing.Size(120, 21);
			toolStripPropertyEditor2.Source = new NationalInstruments.UI.PropertyEditorSource(MainScreenGraph, "Cursors");
			toolStripPropertyEditor2.Text = "(Collection)";
			// 
			// toolStripLabel3
			// 
			toolStripLabel3.Name = "toolStripLabel3";
			toolStripLabel3.Size = new System.Drawing.Size(92, 22);
			toolStripLabel3.Text = "Annotations:";
			// 
			// toolStripPropertyEditor3
			// 
			toolStripPropertyEditor3.AutoSize = false;
			toolStripPropertyEditor3.Name = "toolStripPropertyEditor3";
			toolStripPropertyEditor3.RenderMode = NationalInstruments.UI.PropertyEditorRenderMode.Inherit;
			toolStripPropertyEditor3.Size = new System.Drawing.Size(120, 21);
			toolStripPropertyEditor3.Source = new NationalInstruments.UI.PropertyEditorSource(MainScreenGraph, "Annotations");
			toolStripPropertyEditor3.Text = "(Collection)";
			// 
			// toolStripLabel4
			// 
			toolStripLabel4.Name = "toolStripLabel4";
			toolStripLabel4.Size = new System.Drawing.Size(44, 22);
			toolStripLabel4.Text = "Plots:";
			// 
			// toolStripPropertyEditor4
			// 
			toolStripPropertyEditor4.AutoSize = false;
			toolStripPropertyEditor4.Name = "toolStripPropertyEditor4";
			toolStripPropertyEditor4.RenderMode = NationalInstruments.UI.PropertyEditorRenderMode.Inherit;
			toolStripPropertyEditor4.Size = new System.Drawing.Size(120, 21);
			toolStripPropertyEditor4.Source = new NationalInstruments.UI.PropertyEditorSource(MainScreenGraph, "Plots");
			toolStripPropertyEditor4.Text = "(Collection)";
			// 
			// chkZLegend
			// 
			chkZLegend.AutoSize = true;
			chkZLegend.Checked = true;
			chkZLegend.CheckState = System.Windows.Forms.CheckState.Checked;
			chkZLegend.Location = new System.Drawing.Point(400, 414);
			chkZLegend.Name = "chkZLegend";
			chkZLegend.Size = new System.Drawing.Size(15, 14);
			chkZLegend.TabIndex = 248;
			chkZLegend.UseVisualStyleBackColor = true;
			chkZLegend.CheckedChanged += new System.EventHandler(chkZLegend_CheckedChanged);
			// 
			// chkYLegend
			// 
			chkYLegend.AutoSize = true;
			chkYLegend.Checked = true;
			chkYLegend.CheckState = System.Windows.Forms.CheckState.Checked;
			chkYLegend.Location = new System.Drawing.Point(326, 414);
			chkYLegend.Name = "chkYLegend";
			chkYLegend.Size = new System.Drawing.Size(15, 14);
			chkYLegend.TabIndex = 248;
			chkYLegend.UseVisualStyleBackColor = true;
			chkYLegend.CheckedChanged += new System.EventHandler(chkYLegend_CheckedChanged);
			// 
			// chkXLegend
			// 
			chkXLegend.AutoSize = true;
			chkXLegend.Checked = true;
			chkXLegend.CheckState = System.Windows.Forms.CheckState.Checked;
			chkXLegend.Location = new System.Drawing.Point(252, 414);
			chkXLegend.Name = "chkXLegend";
			chkXLegend.Size = new System.Drawing.Size(15, 14);
			chkXLegend.TabIndex = 248;
			chkXLegend.UseVisualStyleBackColor = true;
			chkXLegend.CheckedChanged += new System.EventHandler(chkXLegend_CheckedChanged);
			// 
			// textBox5
			// 
			textBox5.Location = new System.Drawing.Point(384, 493);
			textBox5.Multiline = true;
			textBox5.Name = "textBox5";
			textBox5.Size = new System.Drawing.Size(156, 60);
			textBox5.TabIndex = 247;
			textBox5.Text = "Showing the number of samples lost due to transactions and not reading the data t" +
	"imely";
			// 
			// textBox4
			// 
			textBox4.Location = new System.Drawing.Point(188, 490);
			textBox4.Multiline = true;
			textBox4.Name = "textBox4";
			textBox4.Size = new System.Drawing.Size(156, 60);
			textBox4.TabIndex = 247;
			textBox4.Text = "Showing the percentage of overhead bytes (due to I2C protocol) over the total amo" +
	"unt of bytes transmitted.";
			// 
			// textBox3
			// 
			textBox3.Location = new System.Drawing.Point(3, 487);
			textBox3.Multiline = true;
			textBox3.Name = "textBox3";
			textBox3.Size = new System.Drawing.Size(156, 39);
			textBox3.TabIndex = 247;
			textBox3.Text = "Currently showing the number of I2C transactions executed.";
			// 
			// textBox2
			// 
			textBox2.Location = new System.Drawing.Point(188, 461);
			textBox2.Name = "textBox2";
			textBox2.Size = new System.Drawing.Size(162, 20);
			textBox2.TabIndex = 243;
			// 
			// textBox1
			// 
			textBox1.Location = new System.Drawing.Point(384, 461);
			textBox1.Name = "textBox1";
			textBox1.Size = new System.Drawing.Size(162, 20);
			textBox1.TabIndex = 243;
			// 
			// txtInfoI2CTransactions
			// 
			txtInfoI2CTransactions.Location = new System.Drawing.Point(3, 461);
			txtInfoI2CTransactions.Name = "txtInfoI2CTransactions";
			txtInfoI2CTransactions.Size = new System.Drawing.Size(162, 20);
			txtInfoI2CTransactions.TabIndex = 243;
			// 
			// legend1
			// 
			legend1.Items.AddRange(new NationalInstruments.UI.LegendItem[] {
            legendItem1,
            legendItem2,
            legendItem3});
			legend1.Location = new System.Drawing.Point(239, 381);
			legend1.Name = "legend1";
			legend1.Size = new System.Drawing.Size(225, 37);
			legend1.TabIndex = 238;
			// 
			// legendItem1
			// 
			legendItem1.Source = MainScreenXAxis;
			legendItem1.Text = "X-Axis";
			// 
			// legendItem2
			// 
			legendItem2.Source = MainScreenYAxis;
			legendItem2.Text = "Y-Axis";
			// 
			// legendItem3
			// 
			legendItem3.Source = MainScreenZAxis;
			legendItem3.Text = "Z-Axis";
			// 
			// lblConfiguration
			// 
			lblConfiguration.BackColor = System.Drawing.Color.SandyBrown;
			lblConfiguration.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			lblConfiguration.Location = new System.Drawing.Point(0, 1);
			lblConfiguration.Name = "lblConfiguration";
			lblConfiguration.Size = new System.Drawing.Size(840, 20);
			lblConfiguration.TabIndex = 81;
			lblConfiguration.Text = "Configuration";
			lblConfiguration.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			lblConfiguration.MouseHover += new System.EventHandler(showConfigPanel);
			// 
			// panelAdvanced
			// 
			panelAdvanced.BackColor = System.Drawing.Color.SandyBrown;
			panelAdvanced.Controls.Add(p4);
			panelAdvanced.Controls.Add(txtMonitorValueChanged);
			panelAdvanced.Controls.Add(lblMonitorValueChange);
			panelAdvanced.Controls.Add(lblMonitorValue);
			panelAdvanced.Controls.Add(txtMonitorValue);
			panelAdvanced.Controls.Add(cmbMonitorIDs);
			panelAdvanced.Controls.Add(btnMonitorGetID);
			panelAdvanced.Controls.Add(AccelControllerGroup);
			panelAdvanced.Location = new System.Drawing.Point(0, 21);
			panelAdvanced.Name = "panelAdvanced";
			panelAdvanced.Size = new System.Drawing.Size(840, 243);
			panelAdvanced.TabIndex = 80;
			// 
			// p4
			// 
			p4.BackColor = System.Drawing.Color.SandyBrown;
			p4.Controls.Add(grpFifo);
			p4.Enabled = false;
			p4.ForeColor = System.Drawing.Color.Black;
			p4.Location = new System.Drawing.Point(0, 146);
			p4.Name = "p4";
			p4.Size = new System.Drawing.Size(840, 67);
			p4.TabIndex = 219;
			// 
			// grpFifo
			// 
			grpFifo.BackColor = System.Drawing.Color.FloralWhite;
			grpFifo.Controls.Add(rdoFill);
			grpFifo.Controls.Add(rdoCircular);
			grpFifo.Controls.Add(lblWatermarkValue);
			grpFifo.Controls.Add(tBWatermark);
			grpFifo.Controls.Add(lblWatermark);
			grpFifo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			grpFifo.ForeColor = System.Drawing.Color.SteelBlue;
			grpFifo.Location = new System.Drawing.Point(3, 3);
			grpFifo.Name = "grpFifo";
			grpFifo.Size = new System.Drawing.Size(835, 61);
			grpFifo.TabIndex = 205;
			grpFifo.TabStop = false;
			grpFifo.Text = "FIFO Configuration";
			// 
			// txtMonitorValueChanged
			// 
			txtMonitorValueChanged.Location = new System.Drawing.Point(703, 219);
			txtMonitorValueChanged.Name = "txtMonitorValueChanged";
			txtMonitorValueChanged.Size = new System.Drawing.Size(100, 20);
			txtMonitorValueChanged.TabIndex = 203;
			txtMonitorValueChanged.Visible = false;
			// 
			// lblMonitorValueChange
			// 
			lblMonitorValueChange.AutoSize = true;
			lblMonitorValueChange.Location = new System.Drawing.Point(617, 222);
			lblMonitorValueChange.Name = "lblMonitorValueChange";
			lblMonitorValueChange.Size = new System.Drawing.Size(80, 13);
			lblMonitorValueChange.TabIndex = 202;
			lblMonitorValueChange.Text = "Times changed";
			lblMonitorValueChange.Visible = false;
			// 
			// lblMonitorValue
			// 
			lblMonitorValue.AutoSize = true;
			lblMonitorValue.Location = new System.Drawing.Point(395, 222);
			lblMonitorValue.Name = "lblMonitorValue";
			lblMonitorValue.Size = new System.Drawing.Size(34, 13);
			lblMonitorValue.TabIndex = 201;
			lblMonitorValue.Text = "Value";
			lblMonitorValue.Visible = false;
			// 
			// txtMonitorValue
			// 
			txtMonitorValue.Location = new System.Drawing.Point(447, 219);
			txtMonitorValue.Name = "txtMonitorValue";
			txtMonitorValue.Size = new System.Drawing.Size(100, 20);
			txtMonitorValue.TabIndex = 200;
			txtMonitorValue.Visible = false;
			// 
			// cmbMonitorIDs
			// 
			cmbMonitorIDs.FormattingEnabled = true;
			cmbMonitorIDs.Location = new System.Drawing.Point(159, 219);
			cmbMonitorIDs.Name = "cmbMonitorIDs";
			cmbMonitorIDs.Size = new System.Drawing.Size(121, 21);
			cmbMonitorIDs.TabIndex = 199;
			cmbMonitorIDs.Visible = false;
			// 
			// btnMonitorGetID
			// 
			btnMonitorGetID.Location = new System.Drawing.Point(21, 217);
			btnMonitorGetID.Name = "btnMonitorGetID";
			btnMonitorGetID.Size = new System.Drawing.Size(112, 23);
			btnMonitorGetID.TabIndex = 198;
			btnMonitorGetID.Text = "GetMonitorIDs";
			btnMonitorGetID.UseVisualStyleBackColor = true;
			btnMonitorGetID.Visible = false;
			btnMonitorGetID.Click += new System.EventHandler(btnMonitorGetID_Click);
			// 
			// AccelControllerGroup
			// 
			AccelControllerGroup.BackColor = System.Drawing.Color.AntiqueWhite;
			AccelControllerGroup.Controls.Add(gbOM2);
			AccelControllerGroup.Controls.Add(gbDR);
			AccelControllerGroup.Controls.Add(gbOM);
			AccelControllerGroup.ForeColor = System.Drawing.SystemColors.ControlText;
			AccelControllerGroup.Location = new System.Drawing.Point(3, 0);
			AccelControllerGroup.Name = "AccelControllerGroup";
			AccelControllerGroup.Size = new System.Drawing.Size(835, 145);
			AccelControllerGroup.TabIndex = 197;
			AccelControllerGroup.TabStop = false;
			// 
			// gbOM2
			// 
			gbOM2.BackColor = System.Drawing.Color.AntiqueWhite;
			gbOM2.Controls.Add(btnAutoCal);
			gbOM2.Controls.Add(chkAnalogLowNoise);
			gbOM2.Controls.Add(pOverSampling);
			gbOM2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			gbOM2.ForeColor = System.Drawing.Color.SteelBlue;
			gbOM2.Location = new System.Drawing.Point(302, 9);
			gbOM2.Name = "gbOM2";
			gbOM2.Size = new System.Drawing.Size(314, 128);
			gbOM2.TabIndex = 220;
			gbOM2.TabStop = false;
			gbOM2.Text = "Operation Mode";
			// 
			// btnAutoCal
			// 
			btnAutoCal.ForeColor = System.Drawing.Color.Black;
			btnAutoCal.Location = new System.Drawing.Point(221, 10);
			btnAutoCal.Name = "btnAutoCal";
			btnAutoCal.Size = new System.Drawing.Size(87, 44);
			btnAutoCal.TabIndex = 256;
			btnAutoCal.Text = "Auto Calibrate";
			btnAutoCal.UseVisualStyleBackColor = true;
			btnAutoCal.Click += new System.EventHandler(btnAutoCal_Click);
			// 
			// chkAnalogLowNoise
			// 
			chkAnalogLowNoise.AutoSize = true;
			chkAnalogLowNoise.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			chkAnalogLowNoise.ForeColor = System.Drawing.Color.Black;
			chkAnalogLowNoise.Location = new System.Drawing.Point(6, 21);
			chkAnalogLowNoise.Name = "chkAnalogLowNoise";
			chkAnalogLowNoise.Size = new System.Drawing.Size(171, 17);
			chkAnalogLowNoise.TabIndex = 222;
			chkAnalogLowNoise.Text = "Enable Low Noise (Up to 5.5g)";
			chkAnalogLowNoise.UseVisualStyleBackColor = true;
			// 
			// pOverSampling
			// 
			pOverSampling.Controls.Add(label70);
			pOverSampling.Controls.Add(rdoOSHiResMode);
			pOverSampling.Controls.Add(rdoOSLPMode);
			pOverSampling.Controls.Add(rdoOSLNLPMode);
			pOverSampling.Controls.Add(rdoOSNormalMode);
			pOverSampling.Location = new System.Drawing.Point(6, 55);
			pOverSampling.Name = "pOverSampling";
			pOverSampling.Size = new System.Drawing.Size(302, 72);
			pOverSampling.TabIndex = 221;
			// 
			// label70
			// 
			label70.AutoSize = true;
			label70.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			label70.Location = new System.Drawing.Point(4, 5);
			label70.Name = "label70";
			label70.Size = new System.Drawing.Size(220, 16);
			label70.TabIndex = 222;
			label70.Text = "Oversampling Options for Data";
			// 
			// rdoOSHiResMode
			// 
			rdoOSHiResMode.AutoSize = true;
			rdoOSHiResMode.Checked = true;
			rdoOSHiResMode.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			rdoOSHiResMode.ForeColor = System.Drawing.Color.Black;
			rdoOSHiResMode.Location = new System.Drawing.Point(5, 48);
			rdoOSHiResMode.Name = "rdoOSHiResMode";
			rdoOSHiResMode.Size = new System.Drawing.Size(97, 19);
			rdoOSHiResMode.TabIndex = 224;
			rdoOSHiResMode.TabStop = true;
			rdoOSHiResMode.Text = "Hi Res Mode";
			rdoOSHiResMode.UseVisualStyleBackColor = true;
			// 
			// rdoOSLPMode
			// 
			rdoOSLPMode.AutoSize = true;
			rdoOSLPMode.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			rdoOSLPMode.ForeColor = System.Drawing.Color.Black;
			rdoOSLPMode.Location = new System.Drawing.Point(129, 28);
			rdoOSLPMode.Name = "rdoOSLPMode";
			rdoOSLPMode.Size = new System.Drawing.Size(121, 19);
			rdoOSLPMode.TabIndex = 223;
			rdoOSLPMode.Text = "Low Power Mode";
			rdoOSLPMode.UseVisualStyleBackColor = true;
			// 
			// rdoOSLNLPMode
			// 
			rdoOSLNLPMode.AutoSize = true;
			rdoOSLNLPMode.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			rdoOSLNLPMode.ForeColor = System.Drawing.Color.Black;
			rdoOSLNLPMode.Location = new System.Drawing.Point(129, 47);
			rdoOSLNLPMode.Name = "rdoOSLNLPMode";
			rdoOSLNLPMode.Size = new System.Drawing.Size(147, 19);
			rdoOSLNLPMode.TabIndex = 221;
			rdoOSLNLPMode.Text = "Low Noise Low Power";
			rdoOSLNLPMode.UseVisualStyleBackColor = true;
			// 
			// rdoOSNormalMode
			// 
			rdoOSNormalMode.AutoSize = true;
			rdoOSNormalMode.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			rdoOSNormalMode.ForeColor = System.Drawing.Color.Black;
			rdoOSNormalMode.Location = new System.Drawing.Point(5, 28);
			rdoOSNormalMode.Name = "rdoOSNormalMode";
			rdoOSNormalMode.Size = new System.Drawing.Size(101, 19);
			rdoOSNormalMode.TabIndex = 220;
			rdoOSNormalMode.Text = "Normal Mode";
			rdoOSNormalMode.UseVisualStyleBackColor = true;
			// 
			// gbDR
			// 
			gbDR.BackColor = System.Drawing.Color.AntiqueWhite;
			gbDR.Controls.Add(lbl4gRange);
			gbDR.Controls.Add(rdo2g);
			gbDR.Controls.Add(rdo8g);
			gbDR.Controls.Add(rdoStandby);
			gbDR.Controls.Add(rdo4g);
			gbDR.Controls.Add(lbl2gRange);
			gbDR.Controls.Add(lbl8gRange);
			gbDR.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			gbDR.ForeColor = System.Drawing.Color.SteelBlue;
			gbDR.Location = new System.Drawing.Point(654, 9);
			gbDR.Name = "gbDR";
			gbDR.Size = new System.Drawing.Size(174, 128);
			gbDR.TabIndex = 195;
			gbDR.TabStop = false;
			gbDR.Text = "Dynamic Range";
			// 
			// lbl4gRange
			// 
			lbl4gRange.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			lbl4gRange.ForeColor = System.Drawing.Color.Black;
			lbl4gRange.Location = new System.Drawing.Point(53, 42);
			lbl4gRange.Name = "lbl4gRange";
			lbl4gRange.Size = new System.Drawing.Size(103, 16);
			lbl4gRange.TabIndex = 119;
			lbl4gRange.Text = "512 counts/g";
			lbl4gRange.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// rdo2g
			// 
			rdo2g.AutoSize = true;
			rdo2g.Checked = true;
			rdo2g.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			rdo2g.ForeColor = System.Drawing.Color.Black;
			rdo2g.Location = new System.Drawing.Point(12, 18);
			rdo2g.Name = "rdo2g";
			rdo2g.Size = new System.Drawing.Size(41, 20);
			rdo2g.TabIndex = 115;
			rdo2g.TabStop = true;
			rdo2g.Text = "2g";
			rdo2g.UseVisualStyleBackColor = true;
			// 
			// rdo8g
			// 
			rdo8g.AutoSize = true;
			rdo8g.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			rdo8g.ForeColor = System.Drawing.Color.Black;
			rdo8g.Location = new System.Drawing.Point(12, 62);
			rdo8g.Name = "rdo8g";
			rdo8g.Size = new System.Drawing.Size(41, 20);
			rdo8g.TabIndex = 116;
			rdo8g.Text = "8g";
			rdo8g.UseVisualStyleBackColor = true;
			// 
			// rdoStandby
			// 
			rdoStandby.AutoSize = true;
			rdoStandby.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			rdoStandby.ForeColor = System.Drawing.Color.Black;
			rdoStandby.Location = new System.Drawing.Point(81, 99);
			rdoStandby.Name = "rdoStandby";
			rdoStandby.Size = new System.Drawing.Size(87, 20);
			rdoStandby.TabIndex = 112;
			rdoStandby.Text = "Standby ";
			rdoStandby.UseVisualStyleBackColor = true;
			rdoStandby.Visible = false;
			// 
			// rdo4g
			// 
			rdo4g.AutoSize = true;
			rdo4g.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			rdo4g.ForeColor = System.Drawing.Color.Black;
			rdo4g.Location = new System.Drawing.Point(12, 40);
			rdo4g.Name = "rdo4g";
			rdo4g.Size = new System.Drawing.Size(41, 20);
			rdo4g.TabIndex = 117;
			rdo4g.Text = "4g";
			rdo4g.UseVisualStyleBackColor = true;
			// 
			// lbl2gRange
			// 
			lbl2gRange.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			lbl2gRange.ForeColor = System.Drawing.Color.Black;
			lbl2gRange.Location = new System.Drawing.Point(53, 20);
			lbl2gRange.Name = "lbl2gRange";
			lbl2gRange.Size = new System.Drawing.Size(103, 16);
			lbl2gRange.TabIndex = 118;
			lbl2gRange.Text = "1024 counts/g";
			lbl2gRange.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// lbl8gRange
			// 
			lbl8gRange.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			lbl8gRange.ForeColor = System.Drawing.Color.Black;
			lbl8gRange.Location = new System.Drawing.Point(53, 65);
			lbl8gRange.Name = "lbl8gRange";
			lbl8gRange.Size = new System.Drawing.Size(103, 16);
			lbl8gRange.TabIndex = 120;
			lbl8gRange.Text = "256 counts/g";
			lbl8gRange.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// gbOM
			// 
			gbOM.BackColor = System.Drawing.Color.AntiqueWhite;
			gbOM.Controls.Add(label5);
			gbOM.Controls.Add(label4);
			gbOM.Controls.Add(lblFIFOPoll);
			gbOM.Controls.Add(textBox6);
			gbOM.Controls.Add(stream);
			gbOM.Controls.Add(ddlFIFOPoll);
			gbOM.Controls.Add(ddlDataRate);
			gbOM.Controls.Add(lblData);
			gbOM.Controls.Add(lblSetODR);
			gbOM.Controls.Add(cmbReadMethod);
			gbOM.Controls.Add(label1);
			gbOM.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			gbOM.ForeColor = System.Drawing.Color.SteelBlue;
			gbOM.Location = new System.Drawing.Point(5, 9);
			gbOM.Name = "gbOM";
			gbOM.Size = new System.Drawing.Size(259, 128);
			gbOM.TabIndex = 196;
			gbOM.TabStop = false;
			gbOM.Text = "Datalogger operation";
			// 
			// panelRegisters
			// 
			panelRegisters.Controls.Add(groupBox4);
			panelRegisters.Location = new System.Drawing.Point(843, 85);
			panelRegisters.Name = "panelRegisters";
			panelRegisters.Size = new System.Drawing.Size(175, 485);
			panelRegisters.TabIndex = 80;
			// 
			// groupBox4
			// 
			groupBox4.BackColor = System.Drawing.Color.Gainsboro;
			groupBox4.Controls.Add(cbRegWrite);
			groupBox4.Controls.Add(cbRegRead);
			groupBox4.Controls.Add(label9);
			groupBox4.Controls.Add(btnWrite);
			groupBox4.Controls.Add(txtboxRegisters);
			groupBox4.Controls.Add(lblRegValue);
			groupBox4.Controls.Add(btnUpdateRegs);
			groupBox4.Controls.Add(btnRead);
			groupBox4.Controls.Add(label51);
			groupBox4.Controls.Add(label49);
			groupBox4.Controls.Add(txtRegValue);
			groupBox4.Controls.Add(label42);
			groupBox4.Location = new System.Drawing.Point(0, 3);
			groupBox4.Name = "groupBox4";
			groupBox4.Size = new System.Drawing.Size(175, 481);
			groupBox4.TabIndex = 217;
			groupBox4.TabStop = false;
			groupBox4.Text = "Manual Read/Write";
			// 
			// cbRegWrite
			// 
			cbRegWrite.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			cbRegWrite.FormattingEnabled = true;
			cbRegWrite.Items.AddRange(new object[] {
            "00",
            "01",
            "02",
            "03",
            "04",
            "05",
            "06",
            "07",
            "08",
            "09",
            "0A",
            "0B",
            "0C",
            "0D",
            "0E",
            "0F",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "1A",
            "1B",
            "1C",
            "1D",
            "1E",
            "1F",
            "20",
            "21",
            "22",
            "23",
            "24",
            "25",
            "26",
            "27",
            "28",
            "29",
            "2A",
            "2B",
            "2C",
            "2D",
            "2E",
            "2F",
            "30",
            "31"});
			cbRegWrite.Location = new System.Drawing.Point(7, 427);
			cbRegWrite.Name = "cbRegWrite";
			cbRegWrite.Size = new System.Drawing.Size(45, 21);
			cbRegWrite.TabIndex = 222;
			// 
			// cbRegRead
			// 
			cbRegRead.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			cbRegRead.FormattingEnabled = true;
			cbRegRead.Items.AddRange(new object[] {
            "00",
            "01",
            "02",
            "03",
            "04",
            "05",
            "06",
            "07",
            "08",
            "09",
            "0A",
            "0B",
            "0C",
            "0D",
            "0E",
            "0F",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "1A",
            "1B",
            "1C",
            "1D",
            "1E",
            "1F",
            "20",
            "21",
            "22",
            "23",
            "24",
            "25",
            "26",
            "27",
            "28",
            "29",
            "2A",
            "2B",
            "2C",
            "2D",
            "2E",
            "2F",
            "30",
            "31"});
			cbRegRead.Location = new System.Drawing.Point(7, 346);
			cbRegRead.Name = "cbRegRead";
			cbRegRead.Size = new System.Drawing.Size(45, 21);
			cbRegRead.TabIndex = 221;
			// 
			// label9
			// 
			label9.AutoSize = true;
			label9.Location = new System.Drawing.Point(69, 331);
			label9.Name = "label9";
			label9.Size = new System.Drawing.Size(37, 13);
			label9.TabIndex = 219;
			label9.Text = "Value:";
			// 
			// btnWrite
			// 
			btnWrite.Location = new System.Drawing.Point(7, 453);
			btnWrite.Name = "btnWrite";
			btnWrite.Size = new System.Drawing.Size(156, 19);
			btnWrite.TabIndex = 206;
			btnWrite.Text = "Write";
			btnWrite.UseVisualStyleBackColor = true;
			btnWrite.Click += new System.EventHandler(btnWrite_Click);
			// 
			// txtboxRegisters
			// 
			txtboxRegisters.Location = new System.Drawing.Point(3, 50);
			txtboxRegisters.Multiline = true;
			txtboxRegisters.Name = "txtboxRegisters";
			txtboxRegisters.ReadOnly = true;
			txtboxRegisters.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			txtboxRegisters.Size = new System.Drawing.Size(169, 261);
			txtboxRegisters.TabIndex = 218;
			// 
			// lblRegValue
			// 
			lblRegValue.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			lblRegValue.Location = new System.Drawing.Point(69, 350);
			lblRegValue.Name = "lblRegValue";
			lblRegValue.Size = new System.Drawing.Size(54, 19);
			lblRegValue.TabIndex = 213;
			lblRegValue.Text = "?";
			// 
			// btnUpdateRegs
			// 
			btnUpdateRegs.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			btnUpdateRegs.ForeColor = System.Drawing.SystemColors.ControlText;
			btnUpdateRegs.Location = new System.Drawing.Point(3, 19);
			btnUpdateRegs.Name = "btnUpdateRegs";
			btnUpdateRegs.Size = new System.Drawing.Size(169, 25);
			btnUpdateRegs.TabIndex = 216;
			btnUpdateRegs.Text = "Read all Registers";
			btnUpdateRegs.UseVisualStyleBackColor = true;
			btnUpdateRegs.Click += new System.EventHandler(btnUpdateRegs_Click);
			// 
			// btnRead
			// 
			btnRead.Location = new System.Drawing.Point(7, 373);
			btnRead.Name = "btnRead";
			btnRead.Size = new System.Drawing.Size(156, 19);
			btnRead.TabIndex = 205;
			btnRead.Text = "Read";
			btnRead.UseVisualStyleBackColor = true;
			btnRead.Click += new System.EventHandler(btnRead_Click);
			// 
			// label51
			// 
			label51.AutoSize = true;
			label51.Location = new System.Drawing.Point(3, 331);
			label51.Name = "label51";
			label51.Size = new System.Drawing.Size(49, 13);
			label51.TabIndex = 212;
			label51.Text = "Register:";
			label51.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label49
			// 
			label49.AutoSize = true;
			label49.Location = new System.Drawing.Point(69, 411);
			label49.Name = "label49";
			label49.Size = new System.Drawing.Size(37, 13);
			label49.TabIndex = 211;
			label49.Text = "Value:";
			// 
			// txtRegValue
			// 
			txtRegValue.Location = new System.Drawing.Point(72, 427);
			txtRegValue.Name = "txtRegValue";
			txtRegValue.Size = new System.Drawing.Size(45, 20);
			txtRegValue.TabIndex = 210;
			// 
			// label42
			// 
			label42.AutoSize = true;
			label42.Location = new System.Drawing.Point(3, 411);
			label42.Name = "label42";
			label42.Size = new System.Drawing.Size(49, 13);
			label42.TabIndex = 209;
			label42.Text = "Register:";
			// 
			// Datalogger
			// 
			AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			BackColor = System.Drawing.Color.LightSlateGray;
			ClientSize = new System.Drawing.Size(1018, 591);
			Controls.Add(panelRegisters);
			Controls.Add(panelGeneral);
			Controls.Add(lblMessage);
			Controls.Add(pnlBootloader);
			Controls.Add(mnMenu);
			Controls.Add(toolStripContainer1);
			Controls.Add(CommStrip);
			Controls.Add(pictureBox1);
			Controls.Add(menuStrip1);
			FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			HelpButton = true;
			MaximizeBox = false;
			Name = "Datalogger";
			SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			Text = "MMA845xQ Datalogger";
			FormClosing += new System.Windows.Forms.FormClosingEventHandler(Datalogger_FormClosing);
			MouseEnter += new System.EventHandler(hideConfigPanel);
			menuStrip1.ResumeLayout(false);
			menuStrip1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(pictureBox1)).EndInit();
			CommStrip.ResumeLayout(false);
			CommStrip.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(tBWatermark)).EndInit();
			mnMenu.ResumeLayout(false);
			mnMenu.PerformLayout();
			toolStripContainer1.ResumeLayout(false);
			toolStripContainer1.PerformLayout();
			pnlBootloader.ResumeLayout(false);
			pnlBootloader.PerformLayout();
			panelGeneral.Panel1.ResumeLayout(false);
			panelGeneral.Panel2.ResumeLayout(false);
			panelGeneral.ResumeLayout(false);
			panelDisplay.ResumeLayout(false);
			panelDisplay.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(NIswPlots)).EndInit();
			instrumentControlStrip1.ResumeLayout(false);
			instrumentControlStrip1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(MainScreenGraph)).EndInit();
			((System.ComponentModel.ISupportInitialize)(legend1)).EndInit();
			panelAdvanced.ResumeLayout(false);
			panelAdvanced.PerformLayout();
			p4.ResumeLayout(false);
			grpFifo.ResumeLayout(false);
			grpFifo.PerformLayout();
			AccelControllerGroup.ResumeLayout(false);
			gbOM2.ResumeLayout(false);
			gbOM2.PerformLayout();
			pOverSampling.ResumeLayout(false);
			pOverSampling.PerformLayout();
			gbDR.ResumeLayout(false);
			gbDR.PerformLayout();
			gbOM.ResumeLayout(false);
			gbOM.PerformLayout();
			panelRegisters.ResumeLayout(false);
			groupBox4.ResumeLayout(false);
			groupBox4.PerformLayout();
			ResumeLayout(false);
			PerformLayout();

		}

		private void InitializeForm()
		{
			B_btnDisplay = false;
			B_btnAdvanced = true;
			B_btnRegisters = true;

			stream.SelectedIndex = 1;
			cmbReadMethod.SelectedIndex = 0;
			ddlDataRate.SelectedIndex = 4;
			ddlFIFOPoll.SelectedIndex = 4;
			if (InitializeController)
			{
				ControllerObj.StartDevice();
				rdoStandby.Checked = true;
				UpdateFormState();
				ddlDataRate.SelectedIndex = 0;
				UpdateFormState();
				rdo2g.Checked = true;
				stream.SelectedIndex = 1;
				cmbReadMethod.SelectedIndex = 0;
				UpdateFormState();
				InitializeController = false;
			}
			InitializeStatusBar();
			CommStripButton_Click(this, new EventArgs());
			InitializeWaveform();
		}

		private void InitializeStatusBar()
		{
			dv = new BoardComm();
			cc = dv;
			ControllerObj.GetCommObject(ref cc);
			dv = (BoardComm)cc;
			dv.CommEvents += new CommEventHandler(CommCallback);
			LoadResource();
		}

		private void InitializeWaveform()
		{
			object obj2;
			Monitor.Enter(obj2 = objectLock);
			try
			{
				DateTime now = DateTime.Now;

				if (MainScreenGraph.Plots.Count > 1)
					MainScreenGraph.Plots.Clear();
				if (ActiveWaveArray != null)
					ActiveWaveArray.Clear();
				if (GWaveArray != null)
					GWaveArray.Clear();
				if (CountsWaveArray != null)
					CountsWaveArray.Clear();
				if (UnsignedCountsWaveArray != null)
					UnsignedCountsWaveArray.Clear();

				GWaveArray = new ScatterPlotCollection();
				CountsWaveArray = new ScatterPlotCollection();
				UnsignedCountsWaveArray = new ScatterPlotCollection();
				ActiveWaveArray = new ScatterPlotCollection();
				GWaveArray.Add(new ScatterPlot(MainScreenGraph.XAxes[0], MainScreenGraph.YAxes[0]));
				GWaveArray.Add(new ScatterPlot(MainScreenGraph.XAxes[0], MainScreenGraph.YAxes[0]));
				GWaveArray.Add(new ScatterPlot(MainScreenGraph.XAxes[0], MainScreenGraph.YAxes[0]));
				CountsWaveArray.Add(new ScatterPlot(MainScreenGraph.XAxes[0], MainScreenGraph.YAxes[1]));
				CountsWaveArray.Add(new ScatterPlot(MainScreenGraph.XAxes[0], MainScreenGraph.YAxes[1]));
				CountsWaveArray.Add(new ScatterPlot(MainScreenGraph.XAxes[0], MainScreenGraph.YAxes[1]));
				UnsignedCountsWaveArray.Add(new ScatterPlot(MainScreenGraph.XAxes[0], MainScreenGraph.YAxes[1]));
				UnsignedCountsWaveArray.Add(new ScatterPlot(MainScreenGraph.XAxes[0], MainScreenGraph.YAxes[1]));
				UnsignedCountsWaveArray.Add(new ScatterPlot(MainScreenGraph.XAxes[0], MainScreenGraph.YAxes[1]));
				if (NIswPlots.Value)
				{
					ActiveWaveArray.Add(new ScatterPlot(MainScreenGraph.XAxes[0], MainScreenGraph.YAxes[0]));
					ActiveWaveArray.Add(new ScatterPlot(MainScreenGraph.XAxes[0], MainScreenGraph.YAxes[0]));
					ActiveWaveArray.Add(new ScatterPlot(MainScreenGraph.XAxes[0], MainScreenGraph.YAxes[0]));
				}
				else
				{
					ActiveWaveArray.Add(new ScatterPlot(MainScreenGraph.XAxes[0], MainScreenGraph.YAxes[1]));
					ActiveWaveArray.Add(new ScatterPlot(MainScreenGraph.XAxes[0], MainScreenGraph.YAxes[1]));
					ActiveWaveArray.Add(new ScatterPlot(MainScreenGraph.XAxes[0], MainScreenGraph.YAxes[1]));
				}
				GWaveArray[0].HistoryCapacity = 0x9c40;
				GWaveArray[1].HistoryCapacity = 0x9c40;
				GWaveArray[2].HistoryCapacity = 0x9c40;
				CountsWaveArray[0].HistoryCapacity = 0x9c40;
				CountsWaveArray[1].HistoryCapacity = 0x9c40;
				CountsWaveArray[2].HistoryCapacity = 0x9c40;
				UnsignedCountsWaveArray[0].HistoryCapacity = 0x9c40;
				UnsignedCountsWaveArray[1].HistoryCapacity = 0x9c40;
				UnsignedCountsWaveArray[2].HistoryCapacity = 0x9c40;
				ActiveWaveArray[0].HistoryCapacity = 0x9c40;
				ActiveWaveArray[1].HistoryCapacity = 0x9c40;
				ActiveWaveArray[2].HistoryCapacity = 0x9c40;
				ActiveWaveArray[0].LineColor = Color.Orange;
				ActiveWaveArray[1].LineColor = Color.Lime;
				ActiveWaveArray[2].LineColor = Color.DarkTurquoise;
				MainScreenGraph.Plots.AddRange(ActiveWaveArray);
				NewPoints = 0;
				PointsInTest = 0;
			}
			catch (Exception exception)
			{
				STBLogger.AddEvent(this, STBLogger.EventLevel.Error, "Error caught in main Program", string.Concat(new object[] { "Exception caught. ", exception.Message, "   ", exception.Source, "   ", exception.StackTrace, "   ", exception.Data, "   ", exception.StackTrace }));
			}
			finally
			{
				Monitor.Exit(obj2);
			}
		}

		private void label7_Click(object sender, EventArgs e)
		{
			Loader loader = new Loader(dv, "MMA845x-FW-Bootloader.s19", "silent");
		}

		private void LoadResource()
		{
			try
			{
				Assembly executingAssembly = Assembly.GetExecutingAssembly();
				ResourceManager manager = new ResourceManager("VeyronDatalogger.Properties.Resources", executingAssembly);
				ImageGreen = (Image)manager.GetObject("GreenState");
				ImageYellow = (Image)manager.GetObject("YellowState");
				ImageRed = (Image)manager.GetObject("RedState");
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

		private void mnMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{
		}

		private void mnuFileNew_Click(object sender, EventArgs e)
		{
			for (int i = 0; i < MainScreenGraph.Plots.Count; i++)
				MainScreenGraph.Plots[i].ClearData();

			InitializeWaveform();

			if (!IsScopeActive)
				btnViewData.Text = "Start a new Datalog";
			else
				btnViewData.Text = "Stop current Datalog";
		}

		private void mnuFileOpen_Click(object sender, EventArgs e)
		{
			saveFileDialog1.AddExtension = true;
			saveFileDialog1.OverwritePrompt = true;
			saveFileDialog1.Filter = "Comma Separated Values (*.csv) | *.csv";
			openFileDialog1.ShowDialog();
			try
			{
				StreamReader reader = File.OpenText(saveFileDialog1.FileName);
				string str = "";
				int historyCount = MainScreenGraph.Plots[0].HistoryCount;
				int count = MainScreenGraph.Plots.Count;
				double[] xData = new double[historyCount];
				double[][] numArray2 = new double[count][];
				xData = MainScreenGraph.Plots[0].GetXData();
				int index = 0;
				while (index < count)
				{
					numArray2[index] = MainScreenGraph.Plots[index].GetYData();
					index++;
				}
				for (int i = 0; i < historyCount; i++)
				{
					str = string.Format("{0:0.######}", xData[i]);
					for (index = 0; index < count; index++)
						str = str + string.Format(", {0:0.###}", numArray2[index][i]);
				}
				reader.Close();
			}
			catch (Exception)
			{
				MessageBox.Show("There has been an error when trying to open the file for writing.  Please make sure that it is not being used", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}

		private void mnuFileSaveAs_Click(object sender, EventArgs e)
		{
			saveFileDialog1.AddExtension = true;
			saveFileDialog1.OverwritePrompt = true;
			saveFileDialog1.Filter = "Comma Separated Values (*.csv) | *.csv";
			saveFileDialog1.ShowDialog();
			try
			{
				int num3;
				File.Create(saveFileDialog1.FileName).Close();
				StreamWriter writer = File.AppendText(saveFileDialog1.FileName);
				string str = "";
				int historyCount = MainScreenGraph.Plots[0].HistoryCount;
				int count = MainScreenGraph.Plots.Count;
				double[] xData = new double[historyCount];
				double[][] numArray2 = new double[count][];
				double[][] numArray3 = new double[count][];
				double[][] numArray4 = new double[count][];
				xData = MainScreenGraph.Plots[0].GetXData();
				writer.WriteLine("File {0} saved on {1:d} at {1:T}", saveFileDialog1.FileName, DateTime.Now);
				StringBuilder builder = new StringBuilder();
				string str2 = "";
				writer.WriteLine("Device,HWID,g-range,speed,ADC,X_off,Y_off,Z_off");
				if (ControllerObj.DeviceID == deviceID.MMA8451Q)
				{
					builder.Append("MMA8451Q,");
					str2 = "14";
				}
				else if (ControllerObj.DeviceID == deviceID.MMA8452Q)
				{
					builder.Append("MMA8452Q,");
					str2 = "12";
				}
				else if (ControllerObj.DeviceID == deviceID.MMA8652FC)
				{
					builder.Append("MMA8652FC,");
					str2 = "12";
				}
				else if (ControllerObj.DeviceID == deviceID.MMA8453Q)
				{
					builder.Append("MMA8453Q,");
					str2 = "10";
				}
				else if (ControllerObj.DeviceID == deviceID.MMA8653FC)
				{
					builder.Append("MMA8653FC,");
					str2 = "10";
				}

				string commStatus = dv.GetCommStatus();

				builder.Append(commStatus.Substring(commStatus.IndexOf("HW:") + 3, 4) + ",");
				if (rdo2g.Checked)
					builder.Append("2,");
				else if (rdo4g.Checked)
					builder.Append("4,");
				else if (rdo8g.Checked)
					builder.Append("8,");
				switch (ddlDataRate.SelectedIndex)
				{
					case 0:
						builder.Append("800,");
						break;

					case 1:
						builder.Append("400,");
						break;

					case 2:
						builder.Append("200,");
						break;

					case 3:
						builder.Append("100,");
						break;

					case 4:
						builder.Append("50,");
						break;

					case 5:
						builder.Append("12.5,");
						break;

					case 6:
						builder.Append("6.25,");
						break;

					case 7:
						builder.Append("1.563,");
						break;
				}
				if (stream.SelectedIndex == 0)
				{
					builder.Append("8,");
				}
				else
				{
					builder.Append(str2 + ",");
				}
				builder.Append(calValues[0]);
				builder.Append(",");
				builder.Append(calValues[1]);
				builder.Append(",");
				builder.Append(calValues[2]);
				writer.WriteLine(builder);
				writer.WriteLine();
				str = "Time (s)";
				for (num3 = 0; num3 < count; num3++)
				{
					numArray4[num3] = UnsignedCountsWaveArray[num3].GetYData();
					str = str + ", " + legend1.Items[num3].Text + " (raw)";
				}
				str = str + ",,Time (s)";
				for (num3 = 0; num3 < count; num3++)
				{
					numArray2[num3] = GWaveArray[num3].GetYData();
					str = str + ", " + legend1.Items[num3].Text + " (g)";
				}
				str = str + ",,Time (s)";
				num3 = 0;
				while (num3 < count)
				{
					numArray3[num3] = CountsWaveArray[num3].GetYData();
					str = str + ", " + legend1.Items[num3].Text + " (c)";
					num3++;
				}
				writer.WriteLine(str);
				for (int i = 0; i < historyCount; i++)
				{
					DateTime time = new DateTime((long)(xData[i] * 10000000.0));
					str = time.ToString("HH:mm:ss.ffffff");
					num3 = 0;
					while (num3 < count)
					{
						str = str + string.Format(", {0:0.###}", numArray4[num3][i]);
						num3++;
					}
					time = new DateTime((long)(xData[i] * 10000000.0));
					str = str + time.ToString(",,HH:mm:ss.ffffff");
					num3 = 0;
					while (num3 < count)
					{
						str = str + string.Format(", {0:0.###}", numArray2[num3][i]);
						num3++;
					}
					str = str + new DateTime((long)(xData[i] * 10000000.0)).ToString(",,HH:mm:ss.ffffff");
					for (num3 = 0; num3 < count; num3++)
					{
						str = str + string.Format(", {0:0.###}", numArray3[num3][i]);
					}
					writer.WriteLine(str);
				}
				writer.Close();
			}
			catch (Exception)
			{
				MessageBox.Show("There has been an error when trying to open the file for writing.  Please make sure that it is not being used", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}

		private void rdoCircular_CheckedChanged(object sender, EventArgs e)
		{
			FIFOModeValue = 1;
		}

		private void rdoFill_CheckedChanged(object sender, EventArgs e)
		{
			FIFOModeValue = 2;
		}

		private void Reconnect()
		{
			ProcessStartInfo startInfo = new ProcessStartInfo(Path.Combine(Application.StartupPath, @"..\MainLauncher.exe"));
			startInfo.Arguments = "";
			startInfo.RedirectStandardOutput = true;
			startInfo.UseShellExecute = false;
			Process process = Process.Start(startInfo);
			StreamReader standardOutput = process.StandardOutput;
			process.WaitForExit(1);
			base.Close();
		}

		private void ScopeDemo_Resize(object sender, EventArgs e)
		{
			windowResized = true;
		}

		private void SetInterrupts(bool enabled)
		{
			if ((cmbReadMethod.SelectedIndex == 1) || (cmbReadMethod.SelectedIndex == 2))
			{
				int[] datapassed = new int[] { enabled ? 1 : 0, (cmbReadMethod.SelectedIndex == 0) ? 0 : 6 };
				ControllerObj.SetIntsEnableFlag = true;
				ControllerReqPacket reqName = new ControllerReqPacket();
				ControllerObj.CreateNewTaskFromGUI(reqName, 0, 13, datapassed);
			}
		}

		private void SetLowNoise()
		{
			int[] datapassed = new int[1];
			int num = chkAnalogLowNoise.Checked ? 0xff : 0;
			datapassed[0] = num;
			ControllerObj.SetEnableAnalogLowNoiseFlag = true;
			ControllerReqPacket reqName = new ControllerReqPacket();
			ControllerObj.CreateNewTaskFromGUI(reqName, 0, 0x57, datapassed);
		}

		private void SetOSMode()
		{
			OSMode normal = OSMode.Normal;
			if (rdoOSHiResMode.Checked)
				normal = OSMode.HiRes;
			else if (rdoOSNormalMode.Checked)
				normal = OSMode.Normal;
			else if (rdoOSLPMode.Checked)
				normal = OSMode.LP;
			else if (rdoOSLNLPMode.Checked)
				normal = OSMode.LNLP;
			
			int[] datapassed = new int[] { (int)normal };
			ControllerObj.SetWakeOSModeFlag = true;
			ControllerReqPacket reqName = new ControllerReqPacket();
			ControllerObj.CreateNewTaskFromGUI(reqName, 0, 0x54, datapassed);
		}

		private void SetPLPoll(bool enabled)
		{
			ControllerObj.ReturnPLStatus = true;
		}

		private void showConfigPanel(object sender, EventArgs e)
		{
			if (panelGeneral.SplitterDistance >= 460)
			{
				if ((ControllerObj.DeviceID == deviceID.MMA8451Q) || (ControllerObj.DeviceID == deviceID.MMA8652FC))
					panelGeneral.SplitterDistance = 250;
				else
					panelGeneral.SplitterDistance = 320;
			}
		}

		private void StartTest()
		{
			p4.Enabled = false;
			btnAutoCal.Enabled = false;
			IsScopeActive = true;
			AutoStopGranted = false;
			AutoStopRequested = false;
			if (MainScreenGraph.Plots[0].HistoryCount > 0)
				TestTime = NextPoint;
			else
			{
				StartTime = DateTime.Now;
				TestTime = new DateTime(StartTime.Year, StartTime.Month, StartTime.Day);
			}
			LastScan = TestTime;
			NextPoint = TestTime;
			if (stream.SelectedIndex == 0)
				MainScreenGraph.YAxes[1].Range = new NationalInstruments.UI.Range(-128.0, 127.0);

			else if (stream.SelectedIndex == 1)
			{
				if (ControllerObj.DeviceID == deviceID.MMA8451Q)
				{
					MainScreenGraph.YAxes[1].Range = new NationalInstruments.UI.Range(-8192.0, 8191.0);
				}
				else if ((ControllerObj.DeviceID == deviceID.MMA8452Q) || (ControllerObj.DeviceID == deviceID.MMA8652FC))
				{
					MainScreenGraph.YAxes[1].Range = new NationalInstruments.UI.Range(-2048.0, 2047.0);
				}
				else if ((ControllerObj.DeviceID == deviceID.MMA8453Q) || (ControllerObj.DeviceID == deviceID.MMA8653FC))
				{
					MainScreenGraph.YAxes[1].Range = new NationalInstruments.UI.Range(-512.0, 511.0);
				}
			}

			if (rdo2g.Checked)
				MainScreenGraph.YAxes[0].Range = new NationalInstruments.UI.Range(-2.0, 2.0);
			if (rdo4g.Checked)
				MainScreenGraph.YAxes[0].Range = new NationalInstruments.UI.Range(-4.0, 4.0);
			if (rdo8g.Checked)
				MainScreenGraph.YAxes[0].Range = new NationalInstruments.UI.Range(-8.0, 8.0);

			gbDR.Enabled = false;
			gbOM.Enabled = false;
			gbOM2.Enabled = false;
			ControllerObj.Boot();
			Thread.Sleep(100);
			ConfigureDataRate();
			SetLowNoise();
			SetOSMode();
			ControllerObj.SetStreamPollRate((SampleRate)ddlFIFOPoll.SelectedIndex);
			switch (ddlFIFOPoll.SelectedIndex)
			{
				case 1:
					TimeDeltaPollRate = new TimeSpan(0x61a8L);
					break;

				case 2:
					TimeDeltaPollRate = new TimeSpan(0xc350L);
					break;

				case 3:
					TimeDeltaPollRate = new TimeSpan(0x186a0L);
					break;

				case 4:
					TimeDeltaPollRate = new TimeSpan(0x30d40L);
					break;

				case 5:
					TimeDeltaPollRate = new TimeSpan(0xc3500L);
					break;

				case 6:
					TimeDeltaPollRate = new TimeSpan(0x6355b0L);
					break;

				default:
					TimeDeltaPollRate = new TimeSpan(0, 0, 1);
					break;
			}
			switch (ddlDataRate.SelectedIndex)
			{
				case 0:
					TimeDeltaSampleRate = new TimeSpan(0x30d4L);
					break;

				case 1:
					TimeDeltaSampleRate = new TimeSpan(0x61a8L);
					break;

				case 2:
					TimeDeltaSampleRate = new TimeSpan(0xc350L);
					break;

				case 3:
					TimeDeltaSampleRate = new TimeSpan(0x186a0L);
					break;

				case 4:
					TimeDeltaSampleRate = new TimeSpan(0x30d40L);
					break;

				case 5:
					TimeDeltaSampleRate = new TimeSpan(0xc3500L);
					break;

				case 6:
					TimeDeltaSampleRate = new TimeSpan(0x186a00L);
					break;

				case 7:
					TimeDeltaSampleRate = new TimeSpan(0x6355b0L);
					break;

				default:
					TimeDeltaSampleRate = new TimeSpan(0, 0, 1);
					break;
			}
			if (cmbReadMethod.SelectedIndex == 0)
			{
				if (stream.SelectedIndex == 0)
				{
					ControllerObj.SetFastReadOn();
				}
				if (stream.SelectedIndex == 1)
				{
					ControllerObj.SetFastReadOff();
				}
				ChangeRange(false);
				ControllerObj.StartTest(TM_Delay);
				if (stream.SelectedIndex == 0)
				{
					ControllerObj.EnableXYZ8StreamData();
				}
				if (stream.SelectedIndex == 1)
				{
					ControllerObj.EnableXYZFullStreamData();
				}
			}
			else if (cmbReadMethod.SelectedIndex == 1)
			{
				int[] datapassed = new int[5];
				datapassed[0] = 0;
				datapassed[1] = tBWatermark.Value;
				if (stream.SelectedIndex == 0)
				{
					ControllerObj.SetFastReadOn();
					datapassed[2] = 1;
				}
				else
				{
					ControllerObj.SetFastReadOff();
					datapassed[2] = 0;
				}
				if (rdo2g.Checked)
				{
					datapassed[4] = 0;
				}
				else if (rdo4g.Checked)
				{
					datapassed[4] = 1;
				}
				else if (rdo8g.Checked)
				{
					datapassed[4] = 2;
				}
				if (rdoFill.Checked)
				{
					datapassed[3] = 0;
				}
				else if (rdoCircular.Checked)
				{
					datapassed[3] = 1;
				}
				FIFOConfig(true);
				ChangeRange(false);
				ControllerObj.StartTest(TM_Delay);
				ControllerObj.PollingFIFOFlag = true;
				ControllerReqPacket reqName = new ControllerReqPacket();
				ControllerObj.CreateNewTaskFromGUI(reqName, 0, 80, datapassed);
			}
			else if (cmbReadMethod.SelectedIndex == 2)
			{
			}
		}

		private void StopTest()
		{
			btnAutoCal.Enabled = true;
			NIswPlots.Enabled = true;
			IsScopeActive = false;
			int[] numArray = new int[2];
			MainScreenGraph.Enabled = true;
			gbDR.Enabled = true;
			gbOM.Enabled = true;
			gbOM2.Enabled = true;
			if (cmbReadMethod.SelectedIndex == 0)
			{
				ControllerObj.DisableData();
			}
			else if (cmbReadMethod.SelectedIndex == 1)
			{
				p4.Enabled = true;
				ControllerObj.ReturnFIFOStatus = false;
				ControllerObj.DisableData();
			}
			else if (cmbReadMethod.SelectedIndex == 2)
			{
			}
			ControllerObj.StopTest();
			ChangeRange(true);
			ControllerObj.SetActiveState(false);
		}

		private void stream_SelectionChangeCommitted(object sender, EventArgs e)
		{
			CheckStatus();
		}

		private void tBWatermark_Scroll(object sender, EventArgs e)
		{
			lblWatermarkValue.Text = string.Format("{0:F0}", tBWatermark.Value);
		}

		private void textBox6_TextChanged(object sender, EventArgs e)
		{
			try
			{
				TM_Delay = Convert.ToInt32(textBox6.Text);
			}
			catch (Exception)
			{
				textBox6.Text = Convert.ToString(TM_Delay);
			}
		}

		private void tmrTiltTimer_Tick(object sender, EventArgs e)
		{
			UpdateFormState();
			UpdateMainScreenWaveform();
		}

		private void UpdateCommStrip()
		{
			if (DoReconnect)
			{
				DoReconnect = false;
				tmrTiltTimer.Enabled = false;
				Thread.Sleep(100);
				Reconnect();
			}
			CommunicationState commState = dv.GetCommState();
			string commStatus = dv.GetCommStatus();
			switch (commState)
			{
				case CommunicationState.HWFind:
					CommMode = eCommMode.FindingHW;
					break;

				case CommunicationState.Idle:
					CommMode = eCommMode.Closed;
					break;

				case CommunicationState.Ready:
					{
						CommMode = eCommMode.Running;
						string str2 = commStatus.Substring(commStatus.IndexOf("HW:") + 3, 4);
						if (str2 != "3001" &&
							str2 != "3002" &&
							str2 != "3003" &&
							str2 != "3004" &&
							str2 != "3005" &&
							str2 != "3006")
						{
							tmrTiltTimer.Enabled = false;
							if (MessageBox.Show("Hardware not recognized, please connect a valid MMA845xQ device", "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Hand) == DialogResult.Cancel)
							{
								base.Close();
							}
							dv.Close();
							CommMode = eCommMode.Closed;
							CommStripButton_Click(this, new EventArgs());
							tmrTiltTimer.Enabled = true;
							return;
						}
						if (commStatus.Substring(commStatus.IndexOf("SW:") + 3, 4) != CurrentFW)
						{
							CommMode = eCommMode.Bootloader;
						}
						break;
					}
				default:
					CommMode = eCommMode.Closed;
					break;
			}
			if (CommMode == eCommMode.Closed)
			{
				toolStripStatusLabel.Text = commStatus;
			}
			else
			{
				toolStripStatusLabel.Text = commStatus;
			}
			if ((ControllerObj.DeviceID != deviceID.Unsupported) && (ControllerObj.DeviceID != DeviceID))
			{
				DeviceID = ControllerObj.DeviceID;
			}
			if (CommMode == eCommMode.Bootloader)
			{
				panelGeneral.Enabled = false;
				CommStripButton.Image = ImageGreen;
				pnlBootloader.Visible = true;
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
			if (CommMode == eCommMode.FindingHW)
			{
				panelGeneral.Enabled = false;
				CommStripButton.Image = ImageYellow;
				pnlBootloader.Visible = false;
			}
			else if (CommMode == eCommMode.Running)
			{
				panelGeneral.Enabled = true;
				CommStripButton.Image = ImageGreen;
				pnlBootloader.Visible = false;
				if (ControllerObj.DeviceID == deviceID.MMA8451Q)
					toolStripStatusLabel.Text = commStatus.Replace("ID", "[MMA8451Q] ID");
				else if (ControllerObj.DeviceID == deviceID.MMA8452Q)
					toolStripStatusLabel.Text = commStatus.Replace("ID", "[MMA8452Q] ID");
				else if (ControllerObj.DeviceID == deviceID.MMA8453Q)
					toolStripStatusLabel.Text = commStatus.Replace("ID", "[MMA8453Q] ID");
				else if (ControllerObj.DeviceID == deviceID.MMA8652FC)
					toolStripStatusLabel.Text = commStatus.Replace("ID", "[MMA8652FC] ID");
				else if (ControllerObj.DeviceID == deviceID.MMA8653FC)
					toolStripStatusLabel.Text = commStatus.Replace("ID", "[MMA8653FC] ID");
			}
			else if (CommMode == eCommMode.Closed)
			{
				pnlBootloader.Visible = false;
				panelGeneral.Enabled = false;
				CommStripButton.Image = ImageRed;
				DoReconnect = true;
			}
			CommStrip.Refresh();
		}

		private void UpdateFormHeight()
		{
			if (!windowResized)
			{
				int num2 = panelDisplay.Height + panelAdvanced.Height + panelRegisters.Height + 200;
				if (750 > Height)
				{
					if (num2 >= 750)
						num2 = 750;
				}
				else
					num2 = Height;
				Height = num2;
			}
		}

		private void UpdateFormState()
		{
			DecodeGUIPackets();
			UpdateCommStrip();
			UpdateMonitor();
			if (ControllerObj.DeviceID == deviceID.MMA8451Q)
			{
				BitFull_MaxPositive = Bit14_MaxPositive;
				BitFull_2Complement = Bit14_2Complement;
				if (stream.Items.Count > 1)
				{
					stream.Items[1] = "14 Bit";
				}
				p4.Visible = true;
				lbl2gRange.Text = "4096 counts/g";
				lbl4gRange.Text = "2048 counts/g";
				lbl8gRange.Text = "1024 counts/g";
			}
			else if (ControllerObj.DeviceID != deviceID.Unsupported)
			{
				if (ControllerObj.DeviceID == deviceID.MMA8652FC)
				{
					p4.Visible = true;
				}
				else
				{
					p4.Visible = false;
					if (cmbReadMethod.Items.Count > 1)
					{
						cmbReadMethod.Items.RemoveAt(cmbReadMethod.Items.Count - 1);
					}
				}
				if ((ControllerObj.DeviceID == deviceID.MMA8452Q) || (ControllerObj.DeviceID == deviceID.MMA8652FC))
				{
					BitFull_MaxPositive = Bit12_MaxPositive;
					BitFull_2Complement = Bit12_2Complement;
					if (stream.Items.Count > 1)
					{
						stream.Items[1] = "12 Bit";
					}
					lbl2gRange.Text = "1024 counts/g";
					lbl4gRange.Text = "512 counts/g";
					lbl8gRange.Text = "256 counts/g";
				}
				else if ((ControllerObj.DeviceID == deviceID.MMA8453Q) || (ControllerObj.DeviceID == deviceID.MMA8653FC))
				{
					BitFull_MaxPositive = Bit10_MaxPositive;
					BitFull_2Complement = Bit10_2Complement;
					if (stream.Items.Count > 1)
					{
						stream.Items[1] = "10 Bit";
					}
					lbl2gRange.Text = "256 counts/g";
					lbl4gRange.Text = "128 counts/g";
					lbl8gRange.Text = "64 counts/g";
				}
			}
		}

		private void updateFWToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Loader loader = new Loader(dv, "MMA845x-FW-BL.s19", "silente");
		}

		private void UpdateMainScreenWaveform()
		{
			lock (objectLock)
			{
				try
				{
					int historyCount = ActiveWaveArray[0].HistoryCount;
					int length = GWaveArray[0].HistoryCount;
					int num3 = CountsWaveArray[0].HistoryCount;
					if (!(!AutoStopRequested || AutoStopGranted))
					{
						AutoStopGranted = true;
						StopTest();
						MessageBox.Show(string.Format("The graph's max history has been reached ({0}). The test has been halted. Before starting another test, make sure that you clear the current test's Data using the File-New command", PointsInTest), "Test halted", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					}
					else if (!(!NIswPlots.Value || CurrentlyShowingGValues))
					{
						CurrentlyShowingGValues = true;
						MainScreenGraph.ClearData();
						ActiveWaveArray[0].YAxis = MainScreenGraph.YAxes[0];
						ActiveWaveArray[1].YAxis = MainScreenGraph.YAxes[0];
						ActiveWaveArray[2].YAxis = MainScreenGraph.YAxes[0];
						MainScreenGraph.YAxes[0].Visible = true;
						MainScreenGraph.YAxes[1].Visible = false;
						ActiveWaveArray[0].ClearData();
						ActiveWaveArray[1].ClearData();
						ActiveWaveArray[2].ClearData();
						ActiveWaveArray[0].PlotXY(GWaveArray[0].GetXData(), GWaveArray[0].GetYData(), 0, length);
						ActiveWaveArray[1].PlotXY(GWaveArray[1].GetXData(), GWaveArray[1].GetYData(), 0, length);
						ActiveWaveArray[2].PlotXY(GWaveArray[2].GetXData(), GWaveArray[2].GetYData(), 0, length);
						NewPoints = 0;
					}
					else if (!(NIswPlots.Value || !CurrentlyShowingGValues))
					{
						CurrentlyShowingGValues = false;
						MainScreenGraph.ClearData();
						ActiveWaveArray[0].YAxis = MainScreenGraph.YAxes[1];
						ActiveWaveArray[1].YAxis = MainScreenGraph.YAxes[1];
						ActiveWaveArray[2].YAxis = MainScreenGraph.YAxes[1];
						MainScreenGraph.YAxes[0].Visible = false;
						MainScreenGraph.YAxes[1].Visible = true;
						ActiveWaveArray[0].ClearData();
						ActiveWaveArray[1].ClearData();
						ActiveWaveArray[2].ClearData();
						ActiveWaveArray[0].PlotXY(CountsWaveArray[0].GetXData(), CountsWaveArray[0].GetYData(), 0, num3);
						ActiveWaveArray[1].PlotXY(CountsWaveArray[1].GetXData(), CountsWaveArray[1].GetYData(), 0, num3);
						ActiveWaveArray[2].PlotXY(CountsWaveArray[2].GetXData(), CountsWaveArray[2].GetYData(), 0, num3);
						NewPoints = 0;
					}
					else if (NewPoints > 0)
					{
						if (CurrentlyShowingGValues)
						{
							ActiveWaveArray[0].PlotXYAppend(GWaveArray[0].GetXData(), GWaveArray[0].GetYData(), historyCount, length - historyCount);
							ActiveWaveArray[1].PlotXYAppend(GWaveArray[1].GetXData(), GWaveArray[1].GetYData(), historyCount, length - historyCount);
							ActiveWaveArray[2].PlotXYAppend(GWaveArray[2].GetXData(), GWaveArray[2].GetYData(), historyCount, length - historyCount);
							NewPoints = 0;
						}
						else
						{
							ActiveWaveArray[0].PlotXYAppend(CountsWaveArray[0].GetXData(), CountsWaveArray[0].GetYData(), historyCount, num3 - historyCount);
							ActiveWaveArray[1].PlotXYAppend(CountsWaveArray[1].GetXData(), CountsWaveArray[1].GetYData(), historyCount, num3 - historyCount);
							ActiveWaveArray[2].PlotXYAppend(CountsWaveArray[2].GetXData(), CountsWaveArray[2].GetYData(), historyCount, num3 - historyCount);
							NewPoints = 0;
						}
					}
				}
				catch (Exception)
				{
					XYZData.Clear();
				}
				txtNoP.Text = Convert.ToString(PointsInTest);
				MainScreenGraph.Update();
			}
		}

		private void UpdateMonitor()
		{
			if (cmbMonitorIDs.Text != "")
			{
				object o = new object();
				Freescale.SASD.Communication.Meter.CheckValue(cmbMonitorIDs.Text, ref o);
				if (o is int)
				{
					txtMonitorValue.Text = Convert.ToString((int)o);
				}
				else if (o is DateTime)
				{
					DateTime time = (DateTime)o;
					txtMonitorValue.Text = time.ToString("HH:mm:ss.ffffff");
				}
				else if (o is TimeSpan)
				{
					TimeSpan span = (TimeSpan)o;
					txtMonitorValue.Text = new DateTime(span.Ticks).ToString("HH:mm:ss.ffffff");
				}
				txtMonitorValueChanged.Text = Convert.ToString(Freescale.SASD.Communication.Meter.CheckValueChanged(cmbMonitorIDs.Text));
			}
		}

		private void WriteRegister(int reg, int value)
		{
			int num = reg;
			int num2 = value;
			int[] datapassed = new int[] { num, num2 };
			ControllerObj.WriteValueFlag = true;
			ControllerReqPacket reqName = new ControllerReqPacket();
			ControllerObj.CreateNewTaskFromGUI(reqName, 0, 0x12, datapassed);
			Thread.Sleep(60);
		}
	}
}
