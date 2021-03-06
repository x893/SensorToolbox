﻿namespace VeyronDatalogger
{
	using Freescale.SASD.Communication;
	using System;
	using System.Collections;
	using System.Runtime.CompilerServices;
	using System.Threading;

	internal class AccelController
	{
		public delegate void ControllerEventHandler(ControllerEventType evt, object o);

		private static ManualResetEvent AbortThread = new ManualResetEvent(false);
		public bool ActiveFlag = false;
		private Thread AFEThread;
		public bool AutoCalFlag = false;
		public bool BootFlag = false;
		private CommClass cc;
		private Queue ControllerQueue = new Queue(0x20);
		private int CurrentFIFOMode;
		public bool Data10bFlag = false;
		public bool Data10StreamEnable = false;
		public bool Data12bFlag = false;
		public bool Data12StreamEnable = false;
		public bool Data14bFlag = false;
		public bool Data14StreamEnable = false;
		public bool Data8bFlag = false;
		public bool Data8StreamEnable = false;
		public bool DataFIFO14Flag = false;
		public bool DataFIFO14StreamEnable = false;
		public bool DataFIFO8Flag = false;
		public bool DataFIFO8StreamEnable = false;
		private MMA845xDriver DriverObj = new MMA845xDriver();
		private int FIFODump8orFull;
		private int FIFOWatermark;
		private int FullScaleValue;
		private SampleRate GlobalDataRate;
		private Datalogger GuiObj;
		private Queue GUIQueue = new Queue(0x20);
		public int[] IntsRegValue = new int[8];
		private object objectLock = new object();
		public bool OffsetDataPending = false;
		public int[] passDataFSVal = new int[1];
		public bool PollingDataFlag = false;
		public bool PollingFIFOFlag = false;
		public bool PollingMFF1Flag = false;
		public bool PollingMFF2Flag = false;
		public int[] PollingOrInt = new int[1];
		public bool PollingPLFlag = false;
		public bool PollingPulseFlag = false;
		public bool PollingSysmodFlag = false;
		public bool PollingTransFlag = false;
		public bool PollInterruptMapStatus = false;
		public bool ReadDataPending = false;
		public bool ReadParsedDataPending = false;
		public bool ReadValueFlag = false;
		public bool ReadValueParsedFlag = false;
		public int[] RegParsedValues;
		public byte RegValue;
		public bool ReturnDataStatus = false;
		public bool ReturnFIFOStatus = false;
		public bool ReturnMFF1Status = false;
		public bool ReturnPLStatus = false;
		public bool ReturnPulseStatus = false;
		public bool ReturnRegs = false;
		public bool ReturnSysmodStatus = false;
		public bool ReturnTrans1Status = false;
		public bool ReturnTransStatus = false;
		private static ReaderWriterLock RWLock = new ReaderWriterLock();
		public bool SetBFTripAFlag = false;
		public bool SetCalFlag = false;
		public bool SetDataRateFlag = false;
		public bool SetDBCNTM_PLFlag = false;
		public bool SetEnableAnalogLowNoiseFlag = false;
		public bool SetEnableASleepFlag = false;
		public bool SetEnablePLFlag = false;
		public bool SetFIFOModeFlag = false;
		public bool SetFIFOWatermarkFlag = false;
		public bool SetFREADFlag = false;
		public bool SetFullScaleValueFlag = false;
		public bool SetHPFDataOutFlag = false;
		public bool SetHPFilterFlag = false;
		public bool SetHysteresisFlag = false;
		public bool SetINTActiveFlag = false;
		public bool SetINTPPODFlag = false;
		public bool SetIntsConfigFlag = false;
		public bool SetIntsEnableFlag = false;
		public bool SetLowCurrentFlag = false;
		public bool SetMFF1AndOrFlag = false;
		public bool SetMFF1DBCNTMFlag = false;
		public bool SetMFF1DebounceFlag = false;
		public bool SetMFF1LatchFlag = false;
		public bool SetMFF1ThresholdFlag = false;
		public bool SetMFF1XEFEFlag = false;
		public bool SetMFF1YEFEFlag = false;
		public bool SetMFF1ZEFEFlag = false;
		public bool SetPLDebounceFlag = false;
		public bool SetPLTripAFlag = false;
		public bool SetPulse2ndPulseWinFlag = false;
		public bool SetPulseDPAFlag = false;
		public bool SetPulseFirstTimeLimitFlag = false;
		public bool SetPulseHPFBypassFlag = false;
		public bool SetPulseLatchFlag = false;
		public bool SetPulseLatencyFlag = false;
		public bool SetPulseLPFEnableFlag = false;
		public bool SetPulseXDPFlag = false;
		public bool SetPulseXSPFlag = false;
		public bool SetPulseXThresholdFlag = false;
		public bool SetPulseYDPFlag = false;
		public bool SetPulseYSPFlag = false;
		public bool SetPulseYThresholdFlag = false;
		public bool SetPulseZDPFlag = false;
		public bool SetPulseZSPFlag = false;
		public bool SetPulseZThresholdFlag = false;
		public bool SetResetFlag = false;
		public bool SetSelfTestFlag = false;
		public bool SetSleepOSModeFlag = false;
		public bool SetSleepSampleRateFlag = false;
		public bool SetSleepTimerFlag = false;
		public bool SetStartFlag = false;
		public bool SetStartTestFlag = false;
		public bool SetStopTestFlag = false;
		public bool SetTransBypassHPFFlag = false;
		public bool SetTransBypassHPFNEWFlag = false;
		public bool SetTransDBCNTMFlag = false;
		public bool SetTransDBCNTMNEWFlag = false;
		public bool SetTransDebounceFlag = false;
		public bool SetTransDebounceNEWFlag = false;
		public bool SetTransEnableLatchFlag = false;
		public bool SetTransEnableLatchNEWFlag = false;
		public bool SetTransEnableXFlagFlag = false;
		public bool SetTransEnableXFlagNEWFlag = false;
		public bool SetTransEnableYFlagFlag = false;
		public bool SetTransEnableYFlagNEWFlag = false;
		public bool SetTransEnableZFlagFlag = false;
		public bool SetTransEnableZFlagNEWFlag = false;
		public bool SetTransThresholdFlag = false;
		public bool SetTransThresholdNEWFlag = false;
		public bool SetTrigLPFlag = false;
		public bool SetTrigMFFFlag = false;
		public bool SetTrigPulseFlag = false;
		public bool SetTrigTransFlag = false;
		public bool SetWakeFromSleepFlag = false;
		public bool SetWakeOSModeFlag = false;
		public bool SetZLockAFlag = false;
		public bool StreamIntFnFlag = false;
		private SampleRate StreamPollRate;
		public bool WriteValueFlag = false;

		public event ControllerEventHandler ControllerEvents;

		public AccelController()
		{
			AFEThread = new Thread(new ThreadStart(ProcessRequest));
		}

		public void AppRegisterRead(int addr)
		{
			int[] datapassed = new int[] { addr };
			ControllerReqPacket reqName = new ControllerReqPacket();
			CreateNewTaskFromGUI(reqName, 0, 0x6a, datapassed);
		}

		public void AppRegisterWrite(int[] data)
		{
			ControllerReqPacket reqName = new ControllerReqPacket();
			CreateNewTaskFromGUI(reqName, 0, 0x6a, data);
		}

		public void Boot()
		{
			int[] datapassed = new int[] { FullScaleValue };
			BootFlag = true;
			ControllerReqPacket reqName = new ControllerReqPacket();
			CreateNewTaskFromGUI(reqName, 0, 0x51, datapassed);
		}

		public void CloseConnection()
		{
			DriverObj.CloseCom();
		}

		private void ControllerCallbackCaller(ControllerEventType et, object o)
		{
			ControllerEventHandler controllerEvents = ControllerEvents;
			if (controllerEvents != null)
			{
				controllerEvents(et, o);
			}
		}

		public void CreateNewTaskFromGUI(ControllerReqPacket ReqName, int FormNum, int TaskNum, int[] datapassed)
		{
			ReqName.FormID = FormNum;
			ReqName.TaskID = TaskNum;
			ReqName.PayLoad.Enqueue(datapassed);
			ControllerQueue.Enqueue(ReqName);
		}

		private void DecodeRequestToken()
		{
			int[] numArray2;
			byte[] buffer;
			int[] numArray3;
			int[] numArray4;
			int[] numArray9;
			int[] numArray19;
			int[] numArray28;
			ControllerReqPacket packet = (ControllerReqPacket)ControllerQueue.Dequeue();
			switch (packet.TaskID)
			{
				case 1:
					if (Data8bFlag)
					{
						numArray3 = (int[])packet.PayLoad.Dequeue();
						PollingOrInt[0] = numArray3[0];
						buffer = new byte[] { 1 };
						if (PollingOrInt[0] != 0)
						{
							DriverObj.ConfigureStreamerIntData(StreamPollRate, RequestModeType.FBMR, buffer);
						}
						else
						{
							DriverObj.ConfigureStreamerPoll(RequestModeType.FBMR);
						}
						Data8StreamEnable = true;
					}
					Data8bFlag = false;
					return;

				case 2:
					if (Data12bFlag)
					{
						int[] numArray5 = (int[])packet.PayLoad.Dequeue();
						PollingOrInt[0] = numArray5[0];
						buffer = new byte[] { 3 };
						if (PollingOrInt[0] != 0)
						{
							DriverObj.ConfigureStreamerIntData(StreamPollRate, RequestModeType.FBMR, buffer);
						}
						else
						{
							DriverObj.ConfigureStreamerPoll(RequestModeType.FBMR);
						}
						Data12StreamEnable = true;
					}
					Data12bFlag = false;
					return;

				case 3:
				case 15:
				case 0x22:
				case 0x49:
				case 0x4a:
				case 0x4b:
				case 0x4c:
				case 0x4d:
				case 0x4e:
				case 0x4f:
				case 0x66:
				case 0x67:
				case 0x68:
				case 0x69:
					return;

				case 4:
					if (SetFullScaleValueFlag)
					{
						int[] passData = new int[1];
						passData = (int[])packet.PayLoad.Dequeue();
						FullScaleValue = passData[0];
						DriverObj.SetFS(passData);
					}
					SetFullScaleValueFlag = false;
					return;

				case 5:
					if (SetDataRateFlag)
					{
						int[] passedData = new int[1];
						passedData = (int[])packet.PayLoad.Dequeue();
						DriverObj.SetDataRate(passedData);
						GlobalDataRate = (passedData[0] == 6) ? SampleRate.Rate80 : ((passedData[0] > 2) ? ((SampleRate)passedData[0]) : SampleRate.Rate20);
					}
					SetDataRateFlag = false;
					return;

				case 6:
					if (SetWakeFromSleepFlag)
					{
						int[] numArray12 = new int[2];
						numArray12 = (int[])packet.PayLoad.Dequeue();
						DriverObj.SetWakeFromSleep(numArray12);
					}
					SetWakeFromSleepFlag = false;
					return;

				case 7:
					if (SetEnableASleepFlag)
					{
						int[] numArray13 = new int[1];
						numArray13 = (int[])packet.PayLoad.Dequeue();
						DriverObj.SetAutoSleepEnable(numArray13);
					}
					SetEnableASleepFlag = false;
					return;

				case 8:
					if (SetSleepSampleRateFlag)
					{
						int[] numArray14 = new int[1];
						numArray14 = (int[])packet.PayLoad.Dequeue();
						DriverObj.SetSleepSampleRate(numArray14);
					}
					SetSleepSampleRateFlag = false;
					return;

				case 9:
					if (SetSleepTimerFlag)
					{
						int[] numArray15 = new int[1];
						numArray15 = (int[])packet.PayLoad.Dequeue();
						DriverObj.SetSleepTimer(numArray15);
					}
					SetSleepTimerFlag = false;
					return;

				case 10:
					if (SetSelfTestFlag)
					{
						int[] numArray16 = new int[1];
						numArray16 = (int[])packet.PayLoad.Dequeue();
						DriverObj.SetSelfTest(numArray16);
					}
					SetSelfTestFlag = false;
					return;

				case 11:
					if (SetHPFilterFlag)
					{
						int[] numArray17 = new int[1];
						numArray17 = (int[])packet.PayLoad.Dequeue();
						DriverObj.SetHPFilter(numArray17);
					}
					SetHPFilterFlag = false;
					return;

				case 12:
					if (SetCalFlag)
					{
						int[] numArray18 = new int[3];
						numArray18 = (int[])packet.PayLoad.Dequeue();
						DriverObj.SetCalValues(numArray18);
						OffsetDataPending = true;
					}
					SetCalFlag = false;
					return;

				case 13:
					if (SetIntsEnableFlag)
					{
						numArray19 = new int[2];
						numArray19 = (int[])packet.PayLoad.Dequeue();
						DriverObj.SetIntsEnable(numArray19);
					}
					SetIntsEnableFlag = false;
					return;

				case 14:
					if (SetIntsConfigFlag)
					{
						int[] numArray20 = new int[2];
						numArray20 = (int[])packet.PayLoad.Dequeue();
						DriverObj.SetIntConfig(numArray20);
					}
					SetIntsConfigFlag = false;
					return;

				case 0x10:
					{
						int[] numArray27 = new int[1];
						numArray27 = (int[])packet.PayLoad.Dequeue();
						ReturnRegs = true;
						return;
					}
				case 0x11:
					if (ReadValueFlag)
					{
						numArray28 = new int[1];
						numArray28 = (int[])packet.PayLoad.Dequeue();
						RegValue = DriverObj.ReadValue(numArray28);
						ReadDataPending = true;
					}
					ReadValueFlag = false;
					return;

				case 0x12:
					if (WriteValueFlag)
					{
						int[] numArray29 = new int[2];
						numArray29 = (int[])packet.PayLoad.Dequeue();
						DriverObj.WriteValue(numArray29);
					}
					WriteValueFlag = false;
					return;

				case 0x13:
					if (SetFIFOModeFlag)
					{
						int[] numArray21 = new int[1];
						numArray21 = (int[])packet.PayLoad.Dequeue();
						DriverObj.SetFIFOMode(numArray21);
					}
					SetFIFOModeFlag = false;
					return;

				case 20:
					if (SetFIFOWatermarkFlag)
					{
						int[] numArray22 = new int[1];
						numArray22 = (int[])packet.PayLoad.Dequeue();
						DriverObj.SetFIFOWatermark(numArray22);
					}
					SetFIFOWatermarkFlag = false;
					return;

				case 0x15:
					if (SetTrigTransFlag)
					{
						int[] numArray23 = new int[1];
						numArray23 = (int[])packet.PayLoad.Dequeue();
						DriverObj.SetTrigTrans(numArray23);
					}
					return;

				case 0x16:
					if (SetTrigLPFlag)
					{
						int[] numArray24 = new int[1];
						numArray24 = (int[])packet.PayLoad.Dequeue();
						DriverObj.SetTrigLP(numArray24);
					}
					return;

				case 0x17:
					if (SetTrigPulseFlag)
					{
						int[] numArray25 = new int[1];
						numArray25 = (int[])packet.PayLoad.Dequeue();
						DriverObj.SetTrigPulse(numArray25);
					}
					return;

				case 0x18:
					if (SetTrigMFFFlag)
					{
						int[] numArray26 = new int[1];
						numArray26 = (int[])packet.PayLoad.Dequeue();
						DriverObj.SetTrigMFF(numArray26);
					}
					return;

				case 0x19:
					if (SetEnablePLFlag)
					{
						int[] numArray32 = new int[1];
						numArray32 = (int[])packet.PayLoad.Dequeue();
						DriverObj.SetEnablePL(numArray32);
					}
					SetEnablePLFlag = false;
					return;

				case 0x1a:
					if (SetZLockAFlag)
					{
						int[] numArray33 = new int[1];
						numArray33 = (int[])packet.PayLoad.Dequeue();
						DriverObj.SetZLockA(numArray33);
					}
					SetZLockAFlag = false;
					return;

				case 0x1b:
					if (SetBFTripAFlag)
					{
						int[] numArray34 = new int[1];
						numArray34 = (int[])packet.PayLoad.Dequeue();
						DriverObj.SetBFTripA(numArray34);
					}
					SetBFTripAFlag = false;
					return;

				case 0x1c:
					if (SetPLTripAFlag)
					{
						int[] numArray35 = (int[])packet.PayLoad.Dequeue();
						int[] numArray36 = new int[1];
						if (numArray35[0] == 0)
						{
							numArray36[0] = 7;
						}
						if (numArray35[0] == 1)
						{
							numArray36[0] = 9;
						}
						if (numArray35[0] == 2)
						{
							numArray36[0] = 12;
						}
						if (numArray35[0] == 3)
						{
							numArray36[0] = 13;
						}
						if (numArray35[0] == 4)
						{
							numArray36[0] = 15;
						}
						if (numArray35[0] == 5)
						{
							numArray36[0] = 0x10;
						}
						if (numArray35[0] == 6)
						{
							numArray36[0] = 0x13;
						}
						if (numArray35[0] == 7)
						{
							numArray36[0] = 20;
						}
						if (numArray35[0] == 8)
						{
							numArray36[0] = 0x17;
						}
						if (numArray35[0] == 9)
						{
							numArray36[0] = 0x19;
						}
						DriverObj.SetPLTripA(numArray36);
					}
					SetPLTripAFlag = false;
					return;

				case 0x1d:
					if (SetHysteresisFlag)
					{
						int[] numArray37 = (int[])packet.PayLoad.Dequeue();
						DriverObj.SetHysteresis(numArray37);
					}
					SetHysteresisFlag = false;
					return;

				case 30:
					if (SetDBCNTM_PLFlag)
					{
						int[] numArray38 = new int[1];
						numArray38 = (int[])packet.PayLoad.Dequeue();
						DriverObj.SetDBCNTM_PL(numArray38);
					}
					SetDBCNTM_PLFlag = false;
					return;

				case 0x1f:
					if (SetPLDebounceFlag)
					{
						int[] numArray39 = new int[1];
						numArray39 = (int[])packet.PayLoad.Dequeue();
						DriverObj.SetPLDebounce(numArray39);
					}
					SetPLDebounceFlag = false;
					return;

				case 0x20:
					if (SetINTPPODFlag)
					{
						int[] numArray41 = new int[1];
						numArray41 = (int[])packet.PayLoad.Dequeue();
						DriverObj.SetINTPPOD(numArray41);
					}
					SetINTPPODFlag = false;
					return;

				case 0x21:
					if (SetINTActiveFlag)
					{
						int[] numArray40 = new int[1];
						numArray40 = (int[])packet.PayLoad.Dequeue();
						DriverObj.SetINTPolarity(numArray40);
					}
					SetINTActiveFlag = false;
					return;

				case 0x23:
					if (SetTransBypassHPFFlag)
					{
						int[] numArray56 = new int[1];
						numArray56 = (int[])packet.PayLoad.Dequeue();
						DriverObj.SetTransBypassHPF(numArray56);
					}
					SetTransBypassHPFFlag = false;
					return;

				case 0x24:
					if (SetTransThresholdFlag)
					{
						int[] numArray50 = new int[1];
						numArray50 = (int[])packet.PayLoad.Dequeue();
						DriverObj.SetTransThreshold(numArray50);
					}
					SetTransThresholdFlag = false;
					return;

				case 0x25:
					if (SetTransDebounceFlag)
					{
						int[] numArray52 = new int[1];
						numArray52 = (int[])packet.PayLoad.Dequeue();
						DriverObj.SetTransDebounce(numArray52);
					}
					SetTransDebounceFlag = false;
					return;

				case 0x26:
					if (SetTransDBCNTMFlag)
					{
						int[] numArray54 = new int[1];
						numArray54 = (int[])packet.PayLoad.Dequeue();
						DriverObj.SetTransDBCNTM(numArray54);
					}
					SetTransDBCNTMFlag = false;
					return;

				case 0x27:
					if (SetTransEnableXFlagFlag)
					{
						int[] numArray42 = new int[1];
						numArray42 = (int[])packet.PayLoad.Dequeue();
						DriverObj.SetTransEnableXFlag(numArray42);
					}
					SetTransEnableXFlagFlag = false;
					return;

				case 40:
					if (SetTransEnableYFlagFlag)
					{
						int[] numArray44 = new int[1];
						numArray44 = (int[])packet.PayLoad.Dequeue();
						DriverObj.SetTransEnableYFlag(numArray44);
					}
					SetTransEnableYFlagFlag = false;
					return;

				case 0x29:
					if (SetTransEnableZFlagFlag)
					{
						int[] numArray46 = new int[1];
						numArray46 = (int[])packet.PayLoad.Dequeue();
						DriverObj.SetTransEnableZFlag(numArray46);
					}
					SetTransEnableZFlagFlag = false;
					return;

				case 0x2a:
					if (SetTransEnableLatchFlag)
					{
						int[] numArray48 = new int[1];
						numArray48 = (int[])packet.PayLoad.Dequeue();
						DriverObj.SetTransEnableLatch(numArray48);
					}
					SetTransEnableLatchFlag = false;
					return;

				case 0x2b:
					if (SetTransBypassHPFNEWFlag)
					{
						int[] numArray57 = new int[1];
						numArray57 = (int[])packet.PayLoad.Dequeue();
						DriverObj.SetTransBypassHPFNEW(numArray57);
					}
					SetTransBypassHPFNEWFlag = false;
					return;

				case 0x2c:
					if (SetTransEnableXFlagNEWFlag)
					{
						int[] numArray43 = new int[1];
						numArray43 = (int[])packet.PayLoad.Dequeue();
						DriverObj.SetTransEnableXFlagNEW(numArray43);
					}
					SetTransEnableXFlagFlag = false;
					return;

				case 0x2d:
					if (SetTransEnableYFlagNEWFlag)
					{
						int[] numArray45 = new int[1];
						numArray45 = (int[])packet.PayLoad.Dequeue();
						DriverObj.SetTransEnableYFlagNEW(numArray45);
					}
					SetTransEnableYFlagNEWFlag = false;
					return;

				case 0x2e:
					if (SetTransEnableZFlagNEWFlag)
					{
						int[] numArray47 = new int[1];
						numArray47 = (int[])packet.PayLoad.Dequeue();
						DriverObj.SetTransEnableZFlagNEW(numArray47);
					}
					SetTransEnableZFlagNEWFlag = false;
					return;

				case 0x2f:
					if (SetTransEnableLatchNEWFlag)
					{
						int[] numArray49 = new int[1];
						numArray49 = (int[])packet.PayLoad.Dequeue();
						DriverObj.SetTransEnableLatchNEW(numArray49);
					}
					SetTransEnableLatchNEWFlag = false;
					return;

				case 0x30:
					if (SetTransThresholdNEWFlag)
					{
						int[] numArray51 = new int[1];
						numArray51 = (int[])packet.PayLoad.Dequeue();
						DriverObj.SetTransThresholdNEW(numArray51);
					}
					SetTransThresholdNEWFlag = false;
					return;

				case 0x31:
					if (SetTransDebounceNEWFlag)
					{
						int[] numArray53 = new int[1];
						numArray53 = (int[])packet.PayLoad.Dequeue();
						DriverObj.SetTransDebounceNEW(numArray53);
					}
					SetTransDebounceNEWFlag = false;
					return;

				case 50:
					if (SetTransDBCNTMNEWFlag)
					{
						int[] numArray55 = new int[1];
						numArray55 = (int[])packet.PayLoad.Dequeue();
						DriverObj.SetTransDBCNTMNEW(numArray55);
					}
					SetTransDBCNTMNEWFlag = false;
					return;

				case 0x33:
					if (SetMFF1AndOrFlag)
					{
						int[] numArray58 = new int[1];
						numArray58 = (int[])packet.PayLoad.Dequeue();
						DriverObj.SetMFF1AndOr(numArray58);
					}
					SetMFF1AndOrFlag = false;
					return;

				case 0x34:
					if (SetMFF1LatchFlag)
					{
						int[] numArray59 = new int[1];
						numArray59 = (int[])packet.PayLoad.Dequeue();
						DriverObj.SetMFF1Latch(numArray59);
					}
					SetMFF1LatchFlag = false;
					return;

				case 0x35:
					if (SetMFF1XEFEFlag)
					{
						int[] numArray60 = new int[1];
						numArray60 = (int[])packet.PayLoad.Dequeue();
						DriverObj.SetMFF1XEFE(numArray60);
					}
					SetMFF1XEFEFlag = false;
					return;

				case 0x36:
					if (SetMFF1YEFEFlag)
					{
						int[] numArray61 = new int[1];
						numArray61 = (int[])packet.PayLoad.Dequeue();
						DriverObj.SetMFF1YEFE(numArray61);
					}
					SetMFF1YEFEFlag = false;
					return;

				case 0x37:
					if (SetMFF1ZEFEFlag)
					{
						int[] numArray62 = new int[1];
						numArray62 = (int[])packet.PayLoad.Dequeue();
						DriverObj.SetMFF1ZEFE(numArray62);
					}
					SetMFF1ZEFEFlag = false;
					return;

				case 0x38:
					if (SetMFF1ThresholdFlag)
					{
						int[] numArray63 = new int[1];
						numArray63 = (int[])packet.PayLoad.Dequeue();
						DriverObj.SetMFF1Threshold(numArray63);
					}
					SetMFF1ThresholdFlag = false;
					return;

				case 0x39:
					if (SetMFF1DebounceFlag)
					{
						int[] numArray64 = new int[1];
						numArray64 = (int[])packet.PayLoad.Dequeue();
						DriverObj.SetMFF1Debounce(numArray64);
					}
					SetMFF1DebounceFlag = false;
					return;

				case 0x3a:
					if (SetMFF1DBCNTMFlag)
					{
						int[] numArray65 = new int[1];
						numArray65 = (int[])packet.PayLoad.Dequeue();
						DriverObj.SetMFF1DBCNTM(numArray65);
					}
					SetMFF1DBCNTMFlag = false;
					return;

				case 0x3b:
					if (SetPulseDPAFlag)
					{
						int[] numArray74 = new int[1];
						numArray74 = (int[])packet.PayLoad.Dequeue();
						DriverObj.SetPulseDPA(numArray74);
					}
					SetPulseDPAFlag = false;
					return;

				case 60:
					if (SetPulseLatchFlag)
					{
						int[] numArray69 = new int[1];
						numArray69 = (int[])packet.PayLoad.Dequeue();
						DriverObj.SetPulseLatch(numArray69);
					}
					SetPulseLatchFlag = false;
					return;

				case 0x3d:
					if (SetPulseZDPFlag)
					{
						int[] numArray77 = new int[1];
						numArray77 = (int[])packet.PayLoad.Dequeue();
						DriverObj.SetPulseZDP(numArray77);
					}
					SetPulseZDPFlag = false;
					return;

				case 0x3e:
					if (SetPulseZSPFlag)
					{
						int[] numArray68 = new int[1];
						numArray68 = (int[])packet.PayLoad.Dequeue();
						DriverObj.SetPulseZSP(numArray68);
					}
					SetPulseZSPFlag = false;
					return;

				case 0x3f:
					if (SetPulseYDPFlag)
					{
						int[] numArray76 = new int[1];
						numArray76 = (int[])packet.PayLoad.Dequeue();
						DriverObj.SetPulseYDP(numArray76);
					}
					SetPulseYDPFlag = false;
					return;

				case 0x40:
					if (SetPulseYSPFlag)
					{
						int[] numArray67 = new int[1];
						numArray67 = (int[])packet.PayLoad.Dequeue();
						DriverObj.SetPulseYSP(numArray67);
					}
					SetPulseYSPFlag = false;
					return;

				case 0x41:
					if (SetPulseXDPFlag)
					{
						int[] numArray75 = new int[1];
						numArray75 = (int[])packet.PayLoad.Dequeue();
						DriverObj.SetPulseXDP(numArray75);
					}
					SetPulseXDPFlag = false;
					return;

				case 0x42:
					if (SetPulseXSPFlag)
					{
						int[] numArray66 = new int[1];
						numArray66 = (int[])packet.PayLoad.Dequeue();
						DriverObj.SetPulseXSP(numArray66);
					}
					SetPulseXSPFlag = false;
					return;

				case 0x43:
					if (SetPulseFirstTimeLimitFlag)
					{
						int[] numArray70 = new int[1];
						numArray70 = (int[])packet.PayLoad.Dequeue();
						DriverObj.SetPulseFirstTimeLimit(numArray70);
					}
					SetPulseFirstTimeLimitFlag = false;
					return;

				case 0x44:
					if (SetPulseXThresholdFlag)
					{
						int[] numArray71 = new int[1];
						numArray71 = (int[])packet.PayLoad.Dequeue();
						DriverObj.SetPulseXThreshold(numArray71);
					}
					SetPulseXThresholdFlag = false;
					return;

				case 0x45:
					if (SetPulseYThresholdFlag)
					{
						int[] numArray72 = new int[1];
						numArray72 = (int[])packet.PayLoad.Dequeue();
						DriverObj.SetPulseYThreshold(numArray72);
					}
					SetPulseYThresholdFlag = false;
					return;

				case 70:
					if (SetPulseZThresholdFlag)
					{
						int[] numArray73 = new int[1];
						numArray73 = (int[])packet.PayLoad.Dequeue();
						DriverObj.SetPulseZThreshold(numArray73);
					}
					SetPulseZThresholdFlag = false;
					return;

				case 0x47:
					if (SetPulseLatencyFlag)
					{
						int[] numArray78 = new int[1];
						numArray78 = (int[])packet.PayLoad.Dequeue();
						DriverObj.SetPulseLatency(numArray78);
					}
					SetPulseLatencyFlag = false;
					return;

				case 0x48:
					if (SetPulse2ndPulseWinFlag)
					{
						int[] numArray79 = new int[1];
						numArray79 = (int[])packet.PayLoad.Dequeue();
						DriverObj.SetPulse2ndPulseWin(numArray79);
					}
					SetPulse2ndPulseWinFlag = false;
					return;

				case 80:
					if (PollingFIFOFlag)
					{
						int[] numArray80 = null;
						numArray80 = (int[])packet.PayLoad.Dequeue();
						PollingOrInt[0] = numArray80[0];
						FIFOWatermark = numArray80[1];
						FIFODump8orFull = numArray80[2];
						CurrentFIFOMode = numArray80[3];
						FullScaleValue = numArray80[4];
						ReturnFIFOStatus = true;
					}
					PollingFIFOFlag = false;
					return;

				case 0x51:
					if (BootFlag)
					{
						int[] numArray = new int[1];
						numArray = (int[])packet.PayLoad.Dequeue();
						DriverObj.Boot();
					}
					BootFlag = false;
					return;

				case 0x52:
					if (AutoCalFlag)
					{
						int num12;
						int num13;
						double num15;
						double num16;
						int[] numArray81 = (int[])packet.PayLoad.Dequeue();
						int deviceID = numArray81[0];
						int num11 = num12 = num13 = 0;
						double num14 = num15 = num16 = 0.0;
						int[] numArray82 = new int[1];
						int[] numArray83 = new int[1];
						int[] numArray84 = new int[1];
						int[] numArray85 = new int[1];
						int[] numArray86 = new int[1];
						numArray82[0] = 0;
						numArray83[0] = 7;
						numArray85[0] = 0xff;
						numArray86[0] = 2;
						DriverObj.SetFS(numArray82);
						DriverObj.SetDataRate(numArray83);
						DriverObj.SetLowNoise(numArray85);
						DriverObj.SetWakeOSMode(numArray86);
						for (int i = 0; i < 3; i++)
						{
							short num2;
							short num3;
							short num4;
							byte num5;
							byte num6;
							byte num7;
							byte num8;
							byte num9;
							byte num10;
							byte[] buffer2;
							byte[] buffer4;
							numArray84[0] = 1;
							DriverObj.SetActive(numArray84);
							Thread.Sleep(0x7d0);
							switch (deviceID)
							{
								case 2:
									{
										byte[] buffer3 = DriverObj.ReadXYZFull(deviceID);
										num8 = buffer3[2];
										num5 = buffer3[3];
										num2 = (short)((num8 * 0x40) + (num5 / 4));
										num9 = buffer3[4];
										num6 = buffer3[5];
										num3 = (short)((num9 * 0x40) + (num6 / 4));
										num10 = buffer3[6];
										num7 = buffer3[7];
										num4 = (short)((num10 * 0x40) + (num7 / 4));
										if (num2 > 0x2000)
										{
											num2 = (short)(num2 - 0x4000);
										}
										if (num3 > 0x2000)
										{
											num3 = (short)(num3 - 0x4000);
										}
										if (num4 > 0x2000)
										{
											num4 = (short)(num4 - 0x4000);
										}
										num14 += (-1 * num2) / 8;
										num15 += (-1 * num3) / 8;
										num16 += (0x1000 - num4) / 8;
										num11 = (int)num14;
										num12 = (int)num15;
										num13 = (int)num16;
										break;
									}
								case 3:
									buffer2 = DriverObj.ReadXYZFull(deviceID);
									num8 = buffer2[2];
									num5 = buffer2[3];
									num2 = (short)((num8 * 0x10) + (num5 / 0x10));
									num9 = buffer2[4];
									num6 = buffer2[5];
									num3 = (short)((num9 * 0x10) + (num6 / 0x10));
									num10 = buffer2[6];
									num7 = buffer2[7];
									num4 = (short)((num10 * 0x10) + (num7 / 0x10));
									if (num2 > 0x7ff)
									{
										num2 = (short)(num2 - 0x1000);
									}
									if (num3 > 0x7ff)
									{
										num3 = (short)(num3 - 0x1000);
									}
									if (num4 > 0x7ff)
									{
										num4 = (short)(num4 - 0x1000);
									}
									num14 += -(num2 / 2);
									num15 += -(num3 / 2);
									num16 += (0x400 - num4) / 2;
									num11 = (int)num14;
									num12 = (int)num15;
									num13 = (int)num16;
									break;

								case 4:
									buffer4 = DriverObj.ReadXYZFull(deviceID);
									num8 = buffer4[2];
									num5 = buffer4[3];
									num2 = (short)((num8 * 4) + (num5 / 0x40));
									num9 = buffer4[4];
									num6 = buffer4[5];
									num3 = (short)((num9 * 4) + (num6 / 0x40));
									num10 = buffer4[6];
									num7 = buffer4[7];
									num4 = (short)((num10 * 4) + (num7 / 0x40));
									if (num2 > 0x200)
									{
										num2 = (short)(num2 - 0x400);
									}
									if (num3 > 0x200)
									{
										num3 = (short)(num3 - 0x400);
									}
									if (num4 > 0x200)
									{
										num4 = (short)(num4 - 0x400);
									}
									num11 += -2 * num2;
									num12 += -2 * num3;
									num13 += 2 * (0x100 - num4);
									break;

								case 5:
									buffer2 = DriverObj.ReadXYZFull(deviceID);
									num8 = buffer2[2];
									num5 = buffer2[3];
									num2 = (short)((num8 * 0x10) + (num5 / 0x10));
									num9 = buffer2[4];
									num6 = buffer2[5];
									num3 = (short)((num9 * 0x10) + (num6 / 0x10));
									num10 = buffer2[6];
									num7 = buffer2[7];
									num4 = (short)((num10 * 0x10) + (num7 / 0x10));
									if (num2 > 0x7ff)
									{
										num2 = (short)(num2 - 0x1000);
									}
									if (num3 > 0x7ff)
									{
										num3 = (short)(num3 - 0x1000);
									}
									if (num4 > 0x7ff)
									{
										num4 = (short)(num4 - 0x1000);
									}
									num14 += -(num2 / 2);
									num15 += -(num3 / 2);
									num16 += (0x400 - num4) / 2;
									num11 = (int)num14;
									num12 = (int)num15;
									num13 = (int)num16;
									break;

								case 6:
									buffer4 = DriverObj.ReadXYZFull(deviceID);
									num8 = buffer4[2];
									num5 = buffer4[3];
									num2 = (short)((num8 * 4) + (num5 / 0x40));
									num9 = buffer4[4];
									num6 = buffer4[5];
									num3 = (short)((num9 * 4) + (num6 / 0x40));
									num10 = buffer4[6];
									num7 = buffer4[7];
									num4 = (short)((num10 * 4) + (num7 / 0x40));
									if (num2 > 0x200)
									{
										num2 = (short)(num2 - 0x400);
									}
									if (num3 > 0x200)
									{
										num3 = (short)(num3 - 0x400);
									}
									if (num4 > 0x200)
									{
										num4 = (short)(num4 - 0x400);
									}
									num11 += -2 * num2;
									num12 += -2 * num3;
									num13 += 2 * (0x100 - num4);
									break;

								case 7:
									{
										byte[] buffer5 = DriverObj.ReadXYZ8();
										num2 = buffer5[2];
										num3 = buffer5[3];
										num4 = buffer5[4];
										if (num2 > 0x80)
										{
											num2 = (short)(num2 - 0x100);
										}
										if (num3 > 0x80)
										{
											num3 = (short)(num3 - 0x100);
										}
										if (num4 > 0x80)
										{
											num4 = (short)(num4 - 0x100);
										}
										num11 += -8 * num2;
										num12 += -8 * num3;
										num13 += 8 * (0x40 - num4);
										break;
									}
							}
							numArray84[0] = 0;
							DriverObj.SetActive(numArray84);
							int[] numArray87 = new int[] { num11, num12, num13 };
							DriverObj.SetCalValues(numArray87);
						}
						OffsetDataPending = true;
					}
					AutoCalFlag = false;
					return;

				case 0x53:
					if (ActiveFlag)
					{
						int[] numArray7 = new int[1];
						numArray7 = (int[])packet.PayLoad.Dequeue();
						DriverObj.SetActive(numArray7);
					}
					return;

				case 0x54:
					if (SetWakeOSModeFlag)
					{
						numArray9 = new int[1];
						numArray9 = (int[])packet.PayLoad.Dequeue();
						DriverObj.SetWakeOSMode(numArray9);
					}
					SetWakeOSModeFlag = false;
					return;

				case 0x55:
					if (SetSleepOSModeFlag)
					{
						numArray9 = new int[1];
						numArray9 = (int[])packet.PayLoad.Dequeue();
						DriverObj.SetSleepOSMode(numArray9);
					}
					SetSleepOSModeFlag = false;
					return;

				case 0x56:
					if (SetHPFDataOutFlag)
					{
						numArray9 = new int[1];
						numArray9 = (int[])packet.PayLoad.Dequeue();
						DriverObj.SetHPFDataOut(numArray9);
					}
					SetHPFDataOutFlag = false;
					return;

				case 0x57:
					if (SetEnableAnalogLowNoiseFlag)
					{
						int[] numArray8 = new int[1];
						numArray8 = (int[])packet.PayLoad.Dequeue();
						DriverObj.SetLowNoise(numArray8);
					}
					SetEnableAnalogLowNoiseFlag = false;
					return;

				case 0x58:
					if (SetFREADFlag)
					{
						numArray9 = new int[1];
						numArray9 = (int[])packet.PayLoad.Dequeue();
						DriverObj.SetFREAD(numArray9);
					}
					SetFREADFlag = false;
					return;

				case 0x59:
					if (SetPulseLPFEnableFlag)
					{
						numArray9 = new int[1];
						numArray9 = (int[])packet.PayLoad.Dequeue();
						DriverObj.SetPulseLPFEnable(numArray9);
					}
					SetPulseLPFEnableFlag = false;
					return;

				case 90:
					if (SetPulseHPFBypassFlag)
					{
						numArray9 = new int[1];
						numArray9 = (int[])packet.PayLoad.Dequeue();
						DriverObj.SetPulseHPFBypass(numArray9);
					}
					SetPulseHPFBypassFlag = false;
					return;

				case 0x5b:
					if (Data10bFlag)
					{
						int[] numArray6 = (int[])packet.PayLoad.Dequeue();
						PollingOrInt[0] = numArray6[0];
						buffer = new byte[] { 4 };
						if (PollingOrInt[0] != 0)
						{
							DriverObj.ConfigureStreamerIntData(StreamPollRate, RequestModeType.FBMR, buffer);
						}
						else
						{
							DriverObj.ConfigureStreamerPoll(RequestModeType.FBMR);
						}
						Data10StreamEnable = true;
					}
					Data10bFlag = false;
					return;

				case 0x5c:
					if (Data14bFlag)
					{
						numArray4 = (int[])packet.PayLoad.Dequeue();
						PollingOrInt[0] = numArray4[0];
						buffer = new byte[] { 2 };
						if (PollingOrInt[0] != 0)
						{
							DriverObj.ConfigureStreamerIntData(StreamPollRate, RequestModeType.FBMR, buffer);
						}
						else
						{
							DriverObj.ConfigureStreamerPoll(RequestModeType.FBMR);
						}
						Data14StreamEnable = true;
					}
					Data14bFlag = false;
					return;

				case 0x5d:
					if (ReadValueParsedFlag)
					{
						numArray28 = new int[1];
						numArray28 = (int[])packet.PayLoad.Dequeue();
						RegParsedValues = DriverObj.ReadParsedValue(numArray28);
						ReadParsedDataPending = true;
					}
					ReadValueParsedFlag = false;
					return;

				case 0x5e:
					DriverObj.DisableStreaming();
					return;

				case 0x5f:
					numArray2 = (int[])packet.PayLoad.Dequeue();
					PollingOrInt[0] = numArray2[0];
					buffer = new byte[] { (byte)numArray2[1], (byte)numArray2[2], (byte)numArray2[3], (byte)numArray2[4], (byte)numArray2[5] };
					if (PollingOrInt[0] != 0)
					{
						DriverObj.ConfigureStreamerIntData(GlobalDataRate, RequestModeType.MMR, buffer);
						break;
					}
					break;

				case 0x60:
					if (StreamIntFnFlag)
					{
						numArray2 = (int[])packet.PayLoad.Dequeue();
						PollingOrInt[0] = numArray2[0];
						buffer = new byte[] { (byte)numArray2[1] };
						if (PollingOrInt[0] != 0)
						{
							DriverObj.ConfigureStreamerIntData(GlobalDataRate, RequestModeType.FBMR, buffer);
						}
						else
						{
							DriverObj.ConfigureStreamerPoll(RequestModeType.FBMR);
						}
						StreamIntFnFlag = true;
					}
					StreamIntFnFlag = false;
					return;

				case 0x61:
					if (SetResetFlag)
					{
						DriverObj.ResetDevice();
					}
					SetResetFlag = false;
					return;

				case 0x62:
					if (SetStartFlag)
					{
						DriverObj.StartDevice();
					}
					SetStartFlag = false;
					return;

				case 0x63:
					if (DataFIFO8Flag)
					{
						numArray3 = (int[])packet.PayLoad.Dequeue();
						passDataFSVal[0] = numArray3[0];
						PollingOrInt[0] = numArray3[1];
						buffer = new byte[] { 6, (byte)numArray3[0] };
						if (PollingOrInt[0] != 0)
						{
							DriverObj.ConfigureStreamerIntData(StreamPollRate, RequestModeType.FBMR, buffer);
						}
						else
						{
							DriverObj.ConfigureStreamerPoll(RequestModeType.FBMR);
						}
						DataFIFO8StreamEnable = true;
					}
					DataFIFO8Flag = false;
					return;

				case 100:
					if (DataFIFO14Flag)
					{
						numArray4 = (int[])packet.PayLoad.Dequeue();
						passDataFSVal[0] = numArray4[0];
						PollingOrInt[0] = numArray4[1];
						buffer = new byte[] { 7, (byte)numArray4[0] };
						if (PollingOrInt[0] != 0)
						{
							DriverObj.ConfigureStreamerIntData(StreamPollRate, RequestModeType.FBMR, buffer);
						}
						else
						{
							DriverObj.ConfigureStreamerPoll(RequestModeType.FBMR);
						}
						DataFIFO14StreamEnable = true;
					}
					DataFIFO14Flag = false;
					return;

				case 0x65:
					if (SetIntsEnableFlag)
					{
						PollingOrInt[0] = 1;
						numArray19 = new int[2];
						numArray19 = (int[])packet.PayLoad.Dequeue();
						DriverObj.SetIntsEnableBit(numArray19);
					}
					return;

				case 0x6a:
					{
						int[] numArray30 = new int[2];
						numArray30 = (int[])packet.PayLoad.Dequeue();
						DriverObj.WriteAppRegister(numArray30);
						return;
					}
				case 0x6b:
					{
						int[] numArray31 = new int[1];
						numArray31 = (int[])packet.PayLoad.Dequeue();
						RegValue = DriverObj.ReadValue(numArray31);
						ReadDataPending = true;
						return;
					}
				default:
					return;
			}
			if (buffer[0] == 6)
			{
				DataFIFO8StreamEnable = true;
			}
			else if (buffer[0] == 7)
			{
				DataFIFO14StreamEnable = true;
			}
		}

		public void Dequeue_GuiPacket(ref GUIUpdatePacket gui_packet)
		{
			RWLock.AcquireWriterLock(-1);
			try
			{
				if (GUIQueue.Count != 0)
				{
					gui_packet = (GUIUpdatePacket)GUIQueue.Dequeue();
				}
				else
				{
					gui_packet = null;
				}
			}
			finally
			{
				RWLock.ReleaseWriterLock();
			}
		}

		public void DisableData()
		{
			Data8StreamEnable = false;
			Data14StreamEnable = false;
			Data12StreamEnable = false;
			Data10StreamEnable = false;
			DataFIFO8StreamEnable = false;
			DataFIFO14StreamEnable = false;
			if (PollingOrInt[0] == 1)
			{
				DriverObj.DisableStreaming();
			}
			DriverObj.DataOut8StreamFlush();
			DriverObj.DataOutFIFOStreamFlush();
			DriverObj.DataOutFullStreamFlush();
		}

		public void DisableGetRegs()
		{
			ReturnRegs = false;
		}

		public void DisableInterruptPins()
		{
			DriverObj.DisableStreaming();
		}

		public void DisableOffsetDataPending()
		{
			OffsetDataPending = false;
		}

		public void DisableStreamData()
		{
			int[] datapassed = new int[2];
			ControllerReqPacket reqName = new ControllerReqPacket();
			CreateNewTaskFromGUI(reqName, 0, 0x5e, datapassed);
			Data12StreamEnable = false;
			Data8StreamEnable = false;
			DataFIFO8StreamEnable = false;
			DriverObj.EnableXYZStream = false;
		}

		public void EnableFIFO8StreamData(int delay)
		{
			int[] datapassed = new int[] { 0xff };
			SetFREADFlag = true;
			ControllerReqPacket reqName = new ControllerReqPacket();
			CreateNewTaskFromGUI(reqName, 0, 0x58, datapassed);
			PollingOrInt[0] = 1;
			int[] numArray2 = new int[] { delay, 1 };
			DataFIFO8Flag = true;
			ControllerReqPacket packet2 = new ControllerReqPacket();
			CreateNewTaskFromGUI(packet2, 0, 0x63, numArray2);
		}

		public void EnableFIFOFullStreamData(int delay)
		{
			int[] datapassed = new int[] { 0 };
			SetFREADFlag = true;
			ControllerReqPacket reqName = new ControllerReqPacket();
			CreateNewTaskFromGUI(reqName, 0, 0x58, datapassed);
			PollingOrInt[0] = 1;
			int[] numArray2 = new int[] { delay, 1 };
			DataFIFO14Flag = true;
			ControllerReqPacket packet2 = new ControllerReqPacket();
			CreateNewTaskFromGUI(packet2, 0, 100, numArray2);
		}

		public void EnableIntFunction(int FBIDfunction)
		{
			PollingOrInt[0] = 1;
			int[] datapassed = new int[] { 1, FBIDfunction };
			ControllerReqPacket reqName = new ControllerReqPacket();
			CreateNewTaskFromGUI(reqName, 0, 0x60, datapassed);
			StreamIntFnFlag = true;
			DriverObj.EnableXYZStream = true;
		}

		public void EnableStreamData(FBID aa, int[] AppMemoryRegs)
		{
			int[] numArray2 = new int[6];
			numArray2[0] = 1;
			numArray2[1] = (int)aa;
			int[] datapassed = numArray2;
			for (int i = 0; i < 4; i++)
			{
				if (AppMemoryRegs != null)
				{
					datapassed[i + 2] = AppMemoryRegs[i];
				}
			}
			ControllerReqPacket reqName = new ControllerReqPacket();
			CreateNewTaskFromGUI(reqName, 0, 0x5f, datapassed);
		}

		public void EnableXYZ12StreamData()
		{
			DriverObj.DataOutFullStreamFlush();
			int[] datapassed = new int[] { 0 };
			SetFREADFlag = true;
			ControllerReqPacket reqName = new ControllerReqPacket();
			CreateNewTaskFromGUI(reqName, 0, 0x58, datapassed);
			int[] numArray3 = new int[2];
			numArray3[0] = 1;
			int[] numArray2 = numArray3;
			ControllerReqPacket packet2 = new ControllerReqPacket();
			CreateNewTaskFromGUI(packet2, 0, 2, numArray2);
			Data12bFlag = true;
			DriverObj.EnableXYZStream = true;
		}

		public void EnableXYZ14StreamData()
		{
			DriverObj.DataOutFullStreamFlush();
			int[] datapassed = new int[] { 0 };
			SetFREADFlag = true;
			ControllerReqPacket reqName = new ControllerReqPacket();
			CreateNewTaskFromGUI(reqName, 0, 0x58, datapassed);
			int[] numArray3 = new int[2];
			numArray3[0] = 1;
			int[] numArray2 = numArray3;
			ControllerReqPacket packet2 = new ControllerReqPacket();
			CreateNewTaskFromGUI(packet2, 0, 0x5c, numArray2);
			Data14bFlag = true;
			DriverObj.EnableXYZStream = true;
		}

		public void EnableXYZ8StreamData()
		{
			DriverObj.DataOut8StreamFlush();
			int[] datapassed = new int[] { 0xff };
			SetFREADFlag = true;
			ControllerReqPacket reqName = new ControllerReqPacket();
			CreateNewTaskFromGUI(reqName, 0, 0x58, datapassed);
			int[] numArray3 = new int[2];
			numArray3[0] = 1;
			int[] numArray2 = numArray3;
			ControllerReqPacket packet2 = new ControllerReqPacket();
			CreateNewTaskFromGUI(packet2, 0, 1, numArray2);
			Data8bFlag = true;
			DriverObj.EnableXYZStream = true;
		}

		public void EnableXYZFullStreamData()
		{
			DriverObj.DataOutFullStreamFlush();
			int[] datapassed = new int[] { 0 };
			SetFREADFlag = true;
			ControllerReqPacket reqName = new ControllerReqPacket();
			CreateNewTaskFromGUI(reqName, 0, 0x58, datapassed);
			TaskID active = TaskID.Active;
			if (DriverObj.DeviceID == deviceID.MMA8451Q)
			{
				active = TaskID.XYZ14StreamData;
				Data14bFlag = true;
			}
			else if ((DriverObj.DeviceID == deviceID.MMA8452Q) || (DriverObj.DeviceID == deviceID.MMA8652FC))
			{
				active = TaskID.XYZ12StreamData;
				Data12bFlag = true;
			}
			else
			{
				if ((DriverObj.DeviceID != deviceID.MMA8453Q) && (DriverObj.DeviceID != deviceID.MMA8653FC))
				{
					return;
				}
				active = TaskID.XYZ10StreamData;
				Data10bFlag = true;
			}
			int[] numArray3 = new int[2];
			numArray3[0] = 1;
			int[] numArray2 = numArray3;
			ControllerReqPacket packet2 = new ControllerReqPacket();
			CreateNewTaskFromGUI(packet2, 0, (int)active, numArray2);
			Data12bFlag = true;
			DriverObj.EnableXYZStream = true;
		}

		public void EndController()
		{
			DriverObj.EndDriver();
			AbortThread.Set();
			while (AFEThread.ThreadState == ThreadState.Running)
			{
			}
		}

		public void GetCommObject(ref BoardComm a)
		{
			cc = a;
			DriverObj.GetCommObject(ref cc);
			a = (BoardComm)cc;
		}

		public void GetCommObject(ref CommClass a)
		{
			DriverObj.GetCommObject(ref a);
		}

		public bool getCommOpenStatus()
		{
			return DriverObj.IsCommOpen();
		}

		public bool InitializeController()
		{
			if (DriverObj.Open())
			{
				AFEThread.Start();
				return true;
			}
			return false;
		}

		public bool isFastReadOn()
		{
			return DriverObj.isFastReadOn();
		}

		public void oldDisableFIFO8bData()
		{
			DataFIFO8StreamEnable = false;
			if (PollingOrInt[0] == 1)
			{
				DriverObj.DisableStreaming();
			}
		}

		private void ProcessRequest()
		{
			while (true)
			{
				if (AbortThread.WaitOne(10, false))
				{
					return;
				}
				RWLock.AcquireWriterLock(-1);
				try
				{
					if (ControllerQueue.Count != 0)
					{
						DecodeRequestToken();
					}
				}
				finally
				{
					RWLock.ReleaseWriterLock();
				}
				if (Data8StreamEnable)
				{
					UpdateStream8b();
				}
				else
				{
					DriverObj.DataOut8StreamFlush();
				}
				if (DataFIFO8StreamEnable)
				{
					UpdateStreamFIFO8b();
				}
				else if (DataFIFO14StreamEnable)
				{
					UpdateStreamFIFO14b();
				}
				else
				{
					DriverObj.DataOutFIFOStreamFlush();
				}
				if (Data12StreamEnable)
				{
					UpdateStreamFull();
				}
				else if (Data10StreamEnable)
				{
					UpdateStreamFull();
				}
				else if (Data14StreamEnable)
				{
					UpdateStreamFull();
				}
				else
				{
					DriverObj.DataOutFullStreamFlush();
				}
				if (ReturnSysmodStatus)
				{
					byte[] modeStatus = DriverObj.GetModeStatus();
					GUIUpdatePacket packet = new GUIUpdatePacket();
					packet.TaskID = 0x4a;
					packet.PayLoad.Enqueue(modeStatus);
					GUIQueue.Enqueue(packet);
				}
				if (ReturnDataStatus)
				{
					byte[] dataStatus = DriverObj.GetDataStatus();
					GUIUpdatePacket packet2 = new GUIUpdatePacket();
					packet2.TaskID = 0x4e;
					packet2.PayLoad.Enqueue(dataStatus);
					GUIQueue.Enqueue(packet2);
				}
				if (PollInterruptMapStatus)
				{
					byte[] interruptStatus = DriverObj.GetInterruptStatus();
					GUIUpdatePacket packet3 = new GUIUpdatePacket();
					packet3.TaskID = 15;
					packet3.PayLoad.Enqueue(interruptStatus);
					GUIQueue.Enqueue(packet3);
				}
				if (ReturnFIFOStatus)
				{
					int[] fIFOData = new int[3];
					byte[] fIFOStatus = DriverObj.GetFIFOStatus();
					if (fIFOStatus[0] == 1)
					{
						if (CurrentFIFOMode == 1)
						{
							fIFOData = new int[3];
							fIFOData = DriverObj.GetFIFOData(1, FIFODump8orFull);
						}
						else if (FIFODump8orFull == 1)
						{
							fIFOData = new int[0x60];
							fIFOData = DriverObj.GetFIFOData(0x20, FIFODump8orFull);
						}
						else
						{
							fIFOData = new int[0x60];
							fIFOData = DriverObj.GetFIFOData(0x20, FIFODump8orFull);
						}
					}
					GUIUpdatePacket packet4 = new GUIUpdatePacket();
					packet4.TaskID = 80;
					packet4.PayLoad.Enqueue(fIFOStatus);
					packet4.PayLoad.Enqueue(fIFOData);
					GUIQueue.Enqueue(packet4);
				}
				if (ReturnMFF1Status)
				{
					byte[] buffer5 = DriverObj.GetFFMT1Status();
					GUIUpdatePacket packet5 = new GUIUpdatePacket();
					packet5.TaskID = 0x4c;
					packet5.PayLoad.Enqueue(buffer5);
					GUIQueue.Enqueue(packet5);
				}
				if (ReturnPLStatus)
				{
					GUIUpdatePacket packet6;
					if (PollingOrInt[0] == 1)
					{
						int[] updatePLStat = null;
						if (DriverObj.IsOrientationInterruptAvailable(ref updatePLStat))
						{
							packet6 = new GUIUpdatePacket();
							packet6.TaskID = 0x4f;
							packet6.PayLoad.Enqueue(updatePLStat);
							ControllerCallbackCaller(ControllerEventType.Orientation, packet6);
						}
					}
					else
					{
						byte[] pLStatus = DriverObj.GetPLStatus();
						packet6 = new GUIUpdatePacket();
						packet6.TaskID = 0x4f;
						packet6.PayLoad.Enqueue(pLStatus);
						GUIQueue.Enqueue(packet6);
					}
				}
				else
				{
					DriverObj.OrientationFlush();
				}
				if (ReturnTransStatus)
				{
					GUIUpdatePacket packet7;
					if (PollingOrInt[0] == 1)
					{
						int[] updateTransStat = null;
						if (DriverObj.IsTransientInterruptAvailable(ref updateTransStat))
						{
							packet7 = new GUIUpdatePacket();
							packet7.TaskID = 0x49;
							packet7.PayLoad.Enqueue(updateTransStat);
							ControllerCallbackCaller(ControllerEventType.Transient, packet7);
						}
					}
					else
					{
						byte[] transientStatus = DriverObj.GetTransientStatus();
						packet7 = new GUIUpdatePacket();
						packet7.TaskID = 0x49;
						packet7.PayLoad.Enqueue(transientStatus);
						GUIQueue.Enqueue(packet7);
					}
				}
				else
				{
					DriverObj.TransientIntFlush();
				}
				if (ReturnPulseStatus)
				{
					GUIUpdatePacket packet8;
					if (PollingOrInt[0] == 1)
					{
						int[] updatePulseStat = null;
						if (DriverObj.IsPulseInterruptAvailable(ref updatePulseStat))
						{
							packet8 = new GUIUpdatePacket();
							packet8.TaskID = 0x4b;
							packet8.PayLoad.Enqueue(updatePulseStat);
							ControllerCallbackCaller(ControllerEventType.Pulse, packet8);
						}
					}
					else
					{
						byte[] pulseStatus = DriverObj.GetPulseStatus();
						packet8 = new GUIUpdatePacket();
						packet8.TaskID = 0x4b;
						packet8.PayLoad.Enqueue(pulseStatus);
						GUIQueue.Enqueue(packet8);
					}
				}
				else
				{
					DriverObj.PulseFlush();
				}
				if (ReturnTrans1Status)
				{
					byte[] buffer9 = DriverObj.GetTrans1Status();
					GUIUpdatePacket packet9 = new GUIUpdatePacket();
					packet9.TaskID = 0x4d;
					packet9.PayLoad.Enqueue(buffer9);
					GUIQueue.Enqueue(packet9);
				}
				if (OffsetDataPending)
				{
					int[] cal = DriverObj.GetCal();
					GUIUpdatePacket packet10 = new GUIUpdatePacket();
					packet10.TaskID = 12;
					packet10.PayLoad.Enqueue(cal);
					GUIQueue.Enqueue(packet10);
					OffsetDataPending = false;
				}
				if (ReadDataPending)
				{
					GUIUpdatePacket packet11 = new GUIUpdatePacket();
					packet11.TaskID = 0x11;
					packet11.PayLoad.Enqueue(RegValue);
					GUIQueue.Enqueue(packet11);
					ReadDataPending = false;
				}
				if (ReadParsedDataPending)
				{
					GUIUpdatePacket packet12 = new GUIUpdatePacket();
					packet12.TaskID = 0x5d;
					packet12.PayLoad.Enqueue(RegParsedValues);
					GUIQueue.Enqueue(packet12);
					ReadParsedDataPending = false;
				}
				if (ReturnRegs)
				{
					byte[] allRegs = DriverObj.GetAllRegs();
					GUIUpdatePacket packet13 = new GUIUpdatePacket();
					packet13.TaskID = 0x10;
					packet13.PayLoad.Enqueue(allRegs);
					GUIQueue.Enqueue(packet13);
					ReturnRegs = false;
				}
			}
		}

		public void ResetDevice()
		{
			int[] datapassed = new int[0];
			SetResetFlag = true;
			ControllerReqPacket reqName = new ControllerReqPacket();
			CreateNewTaskFromGUI(reqName, 0, 0x61, datapassed);
		}

		public void SetActiveState(bool b_active)
		{
			int[] datapassed = new int[1];
			if (b_active)
			{
				datapassed[0] = 1;
			}
			else
			{
				datapassed[0] = 0;
			}
			ControllerReqPacket reqName = new ControllerReqPacket();
			CreateNewTaskFromGUI(reqName, 0, 0x53, datapassed);
			ActiveFlag = true;
		}

		public void SetFastReadOff()
		{
			int[] datapassed = new int[] { 0 };
			SetFREADFlag = true;
			ControllerReqPacket reqName = new ControllerReqPacket();
			CreateNewTaskFromGUI(reqName, 0, 0x58, datapassed);
		}

		public void SetFastReadOn()
		{
			int[] datapassed = new int[] { 0xff };
			SetFREADFlag = true;
			ControllerReqPacket reqName = new ControllerReqPacket();
			CreateNewTaskFromGUI(reqName, 0, 0x58, datapassed);
		}

		public void SetGuiHandle1(Datalogger guiObj)
		{
			GuiObj = guiObj;
		}

		public void SetShakeInterrupts(bool enabled)
		{
			PollingOrInt[0] = 1;
			int[] passedData = new int[] { enabled ? 0xff : 0, 4 };
			DriverObj.SetIntsEnable(passedData);
			DriverObj.SetIntConfig(passedData);
			AppRegisterWrite(new int[] { 0x15, 2 });
		}

		public void SetStreamPollRate()
		{
			StreamPollRate = GlobalDataRate;
		}

		public void SetStreamPollRate(SampleRate pollrate)
		{
			StreamPollRate = pollrate;
		}

		public void SetTransInterrupts(bool enabled)
		{
			PollingOrInt[0] = 1;
			int[] passedData = new int[] { enabled ? 0xff : 0, 0x20 };
			DriverObj.SetIntsEnable(passedData);
			DriverObj.SetIntConfig(passedData);
			AppRegisterWrite(new int[] { 0x15, 2 });
		}

		public void StartDevice()
		{
			SetStartFlag = true;
			CreateNewTaskFromGUI(new ControllerReqPacket(), 0, 0x62, new int[0]);
		}

		public void StartTest(int delay_ms)
		{
			SetStartTestFlag = true;
			CreateNewTaskFromGUI(new ControllerReqPacket(), 0, 0x66, new int[] { delay_ms });
		}

		public void StopTest()
		{
			SetStopTestFlag = true;
			CreateNewTaskFromGUI(new ControllerReqPacket(), 0, 0x67, new int[0]);
		}

		private void UpdateShakeInts()
		{
			ControllerCallbackCaller(ControllerEventType.Shake, DriverObj.GetFFMT1Status()[6]);
		}

		private void UpdateStream8b()
		{
			DataOut8Packet packet = (DataOut8Packet)null;
			while (DriverObj.IsDataOut8StreamAvailable(ref packet))
				ControllerCallbackCaller(ControllerEventType.DataOut8Packet, packet.AxisCounts);
		}

		private void UpdateStreamFIFO14b()
		{
			DataOutFIFOPacket packet = null;
			XYZCounts[] destinationArray;
			while (DriverObj.IsDataOutFIFO14StreamAvailable(ref packet))
			{
				destinationArray = new XYZCounts[32];
				Array.Copy(packet.FIFOData, destinationArray, packet.FIFOData.Length);
				ControllerCallbackCaller(ControllerEventType.DataOutFIFOPacket, destinationArray);
			}
		}

		private void UpdateStreamFIFO8b()
		{
			DataOutFIFOPacket packet = null;
			XYZCounts[] destinationArray;
			while (DriverObj.IsDataOutFIFO8StreamAvailable(ref packet))
			{
				destinationArray = new XYZCounts[32];
				Array.Copy(packet.FIFOData, destinationArray, packet.FIFOData.Length);
				ControllerCallbackCaller(ControllerEventType.DataOutFIFOPacket, destinationArray);
			}
		}

		private void UpdateStreamFull()
		{
			DataOutFullPacket packet = null;
			while (DriverObj.IsDataOutFullStreamAvailable(ref packet))
				ControllerCallbackCaller(ControllerEventType.DataOutFullPacket, packet.AxisCounts);
		}

		public deviceID DeviceID
		{
			get { return DriverObj.DeviceID; }
			set { DriverObj.DeviceID = value; }
		}

		public Queue GetControllerQueue
		{
			get { return ControllerQueue; }
		}

		public Queue GetGUIQueue
		{
			get { return GUIQueue; }
		}
	}
}