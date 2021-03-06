﻿namespace MMA845xQEvaluation
{
	using Freescale.SASD.Communication;
	using System;
	using System.Collections;
	using System.Runtime.CompilerServices;
	using System.Threading;

	internal class AccelController
	{
		#region Publics 

		public delegate void ControllerEventHandler(ControllerEventType evt, object o);
		public event ControllerEventHandler ControllerEvents;

		public bool ActiveFlag = false;
		public bool AutoCalFlag = false;
		public bool BootFlag = false;
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
		public int[] IntsRegValue = new int[8];
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
		public bool WriteValueFlag = false;
		#endregion

		#region Privates 
		private static ManualResetEvent AbortThread = new ManualResetEvent(false);
		private Thread AFEThread;
		private CommClass cc;
		private Queue ControllerQueue = new Queue(0x20);
		private int CurrentFIFOMode;
		private MMA845xDriver DriverObj = new MMA845xDriver();
		private ushort[] FIFOArray;
		private int FIFODump8orFull;
		private int FIFOWatermark;
		private int FullScaleValue;
		private SampleRate GlobalDataRate;
		private VeyronEvaluationSoftware8451 GuiObj;
		private NeutronEvaluationSoftware GuiObj2;
		private Queue GUIQueue = new Queue(0x20);
		private object objectLock = new object();
		private static ReaderWriterLock RWLock = new ReaderWriterLock();
		private bool StreamFollowsGlobalDatarate = true;
		private SampleRate StreamPollRate;
		#endregion

		public AccelController()
		{
			AFEThread = new Thread(new ThreadStart(ProcessRequest));
		}

		public void AppRegisterRead(int addr)
		{
			CreateNewTaskFromGUI(new ControllerReqPacket(), 0, TaskID.WriteAppRegister, new int[] { addr });
		}

		public void AppRegisterWrite(int[] data)
		{
			CreateNewTaskFromGUI(new ControllerReqPacket(), 0, TaskID.WriteAppRegister, data);
		}

		public void Boot()
		{
			BootFlag = true;
			CreateNewTaskFromGUI(new ControllerReqPacket(), 0, TaskID.Boot, new int[] { FullScaleValue });
		}

		#region CloseConnection 
		public void CloseConnection()
		{
			DriverObj.CloseCom();
		}
		#endregion

		#region CreateNewTaskFromGUI 
		public void CreateNewTaskFromGUI(ControllerReqPacket request, int formNum, TaskID taskId, int[] data)
		{
			request.FormID = formNum;
			request.TaskID = (int)taskId;
			request.PayLoad.Enqueue(data);
			ControllerQueue.Enqueue(request);
		}

		public void CreateNewTaskFromGUI(ControllerReqPacket request, int formNum, int taskId, int[] data)
		{
			request.FormID = formNum;
			request.TaskID = taskId;
			request.PayLoad.Enqueue(data);
			ControllerQueue.Enqueue(request);
		}
		#endregion

		#region Dequeue_GuiPacket
		public void Dequeue_GuiPacket(ref GUIUpdatePacket gui_packet)
		{
			RWLock.AcquireWriterLock(-1);
			try
			{
				if (GUIQueue.Count != 0)
					gui_packet = (GUIUpdatePacket)GUIQueue.Dequeue();
				else
					gui_packet = null;
			}
			finally
			{
				RWLock.ReleaseWriterLock();
			}
		}
		#endregion

		public void DisableData()
		{
			Data8StreamEnable = false;
			Data14StreamEnable = false;
			Data12StreamEnable = false;
			Data10StreamEnable = false;
			DataFIFO8StreamEnable = false;
			DataFIFO14StreamEnable = false;
			if (PollingOrInt[0] == 1)
				DriverObj.DisableStreaming();
		}

		public void DisableFIFO8bData()
		{
			DataFIFO8StreamEnable = false;
			if (PollingOrInt[0] == 1)
				DriverObj.DisableStreaming();
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
			CreateNewTaskFromGUI(packet2, 0, 0x62, numArray2);
		}

		public void EnableFIFOFullStreamData(int delay)
		{
			int[] passData = new int[] { 0 };
			DriverObj.SetFREAD(passData);
			PollingOrInt[0] = 1;
			int[] datapassed = new int[] { delay, 1 };
			DataFIFO14Flag = true;
			ControllerReqPacket reqName = new ControllerReqPacket();
			CreateNewTaskFromGUI(reqName, 0, 0x63, datapassed);
		}

		public void EnableIntFunction(int FBIDfunction)
		{
			PollingOrInt[0] = 1;
			int[] datapassed = new int[] { 1, FBIDfunction };
			ControllerReqPacket reqName = new ControllerReqPacket();
			CreateNewTaskFromGUI(reqName, 0, 0x5f, datapassed);
			StreamIntFnFlag = true;
			DriverObj.EnableXYZStream = true;
		}

		public void EnableXYZ12StreamData()
		{
			int[] numArray2 = new int[2];
			numArray2[0] = 1;
			int[] datapassed = numArray2;
			ControllerReqPacket reqName = new ControllerReqPacket();
			CreateNewTaskFromGUI(reqName, 0, 2, datapassed);
			Data12bFlag = true;
			DriverObj.EnableXYZStream = true;
		}

		public void EnableXYZ14StreamData()
		{
			int[] numArray2 = new int[2];
			numArray2[0] = 1;
			int[] datapassed = numArray2;
			ControllerReqPacket reqName = new ControllerReqPacket();
			CreateNewTaskFromGUI(reqName, 0, 0x5c, datapassed);
			Data14bFlag = true;
			DriverObj.EnableXYZStream = true;
		}

		public void EnableXYZ8StreamData()
		{
			int[] numArray2 = new int[2];
			numArray2[0] = 1;
			int[] datapassed = numArray2;
			ControllerReqPacket reqName = new ControllerReqPacket();
			CreateNewTaskFromGUI(reqName, 0, 1, datapassed);
			Data8bFlag = true;
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

		public void ResetDevice()
		{
			int[] datapassed = new int[0];
			SetResetFlag = true;
			ControllerReqPacket reqName = new ControllerReqPacket();
			CreateNewTaskFromGUI(reqName, 0, 0x60, datapassed);
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

		public void SetGuiHandle1(VeyronEvaluationSoftware8451 guiObj)
		{
			GuiObj = guiObj;
		}

		public void SetGuiHandle2(NeutronEvaluationSoftware guiObj2)
		{
			GuiObj2 = guiObj2;
		}

		public void SetHPDataOut(bool b_active)
		{
			int[] datapassed = new int[1];
			if (b_active)
			{
				datapassed[0] = 0xff;
			}
			else
			{
				datapassed[0] = 0;
			}
			ControllerReqPacket reqName = new ControllerReqPacket();
			CreateNewTaskFromGUI(reqName, 0, 0x56, datapassed);
			SetHPFDataOutFlag = true;
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
			StreamFollowsGlobalDatarate = true;
			StreamPollRate = GlobalDataRate;
		}

		public void SetStreamPollRate(SampleRate pollrate)
		{
			StreamFollowsGlobalDatarate = false;
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
			int[] datapassed = new int[0];
			SetStartFlag = true;
			ControllerReqPacket reqName = new ControllerReqPacket();
			CreateNewTaskFromGUI(reqName, 0, 0x61, datapassed);
		}

		public void StartTest(int delay_ms)
		{
			int[] datapassed = new int[] { delay_ms };
			SetStartTestFlag = true;
			ControllerReqPacket reqName = new ControllerReqPacket();
			CreateNewTaskFromGUI(reqName, 0, 0x65, datapassed);
		}

		public void StopTest()
		{
			int[] datapassed = new int[0];
			SetStopTestFlag = true;
			ControllerReqPacket reqName = new ControllerReqPacket();
			CreateNewTaskFromGUI(reqName, 0, 0x66, datapassed);
		}

		public deviceID DeviceID
		{
			get { return DriverObj.DeviceID; }
			set { }
		}

		public Queue GetControllerQueue
		{
			get { return ControllerQueue; }
		}

		public Queue GetGUIQueue
		{
			get { return GUIQueue; }
		}

		#region ControllerCallbackCaller 
		private void ControllerCallbackCaller(ControllerEventType et, object o)
		{
			if (ControllerEvents != null)
				ControllerEvents(et, o);
		}
		#endregion

		#region DecodeRequestToken
		private void DecodeRequestToken()
		{
			int[] numArray2;
			byte[] buffer;
			int[] numArray3;
			int[] numArray8;
			int[] numArray18;
			int[] numArray27;
			ControllerReqPacket packet = (ControllerReqPacket)ControllerQueue.Dequeue();
			switch (packet.TaskID)
			{
				case 0x01:
					if (!Data8bFlag)
					{
						goto Label_029D;
					}
					numArray2 = (int[])packet.PayLoad.Dequeue();
					PollingOrInt[0] = numArray2[0];
					buffer = new byte[] { 1 };
					if (PollingOrInt[0] != 0)
					{
						DriverObj.ConfigureStreamerIntData(GlobalDataRate, RequestModeType.FBMR, buffer);
						break;
					}
					DriverObj.ConfigureStreamerPoll(RequestModeType.FBMR);
					break;

				case 0x02:
					if (Data12bFlag)
					{
						int[] numArray4 = (int[])packet.PayLoad.Dequeue();
						PollingOrInt[0] = numArray4[0];
						buffer = new byte[] { 3 };
						if (PollingOrInt[0] != 0)
							DriverObj.ConfigureStreamerIntData(GlobalDataRate, RequestModeType.FBMR, buffer);
						else
							DriverObj.ConfigureStreamerPoll(RequestModeType.FBMR);
						Data12StreamEnable = true;
					}
					Data12bFlag = false;
					return;

				case 0x03:
				case 0x0F:
				case 0x22:
				case 0x49:
				case 0x4a:
				case 0x4b:
				case 0x4c:
				case 0x4d:
				case 0x4e:
				case 0x4f:
				case 0x65:
				case 0x66:
				case 0x67:
				case 0x68:
					return;

				case 0x04:
					if (SetFullScaleValueFlag)
					{
						int[] passData = new int[1];
						passData = (int[])packet.PayLoad.Dequeue();
						FullScaleValue = passData[0];
						DriverObj.SetFS(passData);
					}
					SetFullScaleValueFlag = false;
					return;

				case 0x05:
					if (SetDataRateFlag)
					{
						int[] passedData = new int[1];
						passedData = (int[])packet.PayLoad.Dequeue();
						DriverObj.SetDataRate(passedData);
						GlobalDataRate = (passedData[0] == 6) ? SampleRate.Rate80 : ((passedData[0] > 2) ? ((SampleRate)passedData[0]) : SampleRate.Rate20);
					}
					SetDataRateFlag = false;
					return;

				case 0x06:
					if (SetWakeFromSleepFlag)
					{
						int[] numArray11 = new int[2];
						numArray11 = (int[])packet.PayLoad.Dequeue();
						DriverObj.SetWakeFromSleep(numArray11);
					}
					SetWakeFromSleepFlag = false;
					return;

				case 0x07:
					if (SetEnableASleepFlag)
					{
						int[] numArray12 = new int[1];
						numArray12 = (int[])packet.PayLoad.Dequeue();
						DriverObj.SetAutoSleepEnable(numArray12);
					}
					SetEnableASleepFlag = false;
					return;

				case 0x08:
					if (SetSleepSampleRateFlag)
					{
						int[] numArray13 = new int[1];
						numArray13 = (int[])packet.PayLoad.Dequeue();
						DriverObj.SetSleepSampleRate(numArray13);
					}
					SetSleepSampleRateFlag = false;
					return;

				case 0x09:
					if (SetSleepTimerFlag)
					{
						int[] numArray14 = new int[1];
						numArray14 = (int[])packet.PayLoad.Dequeue();
						DriverObj.SetSleepTimer(numArray14);
					}
					SetSleepTimerFlag = false;
					return;

				case 0x0A:
					if (SetSelfTestFlag)
					{
						int[] numArray15 = new int[1];
						numArray15 = (int[])packet.PayLoad.Dequeue();
						DriverObj.SetSelfTest(numArray15);
					}
					SetSelfTestFlag = false;
					return;

				case 0x0B:
					if (SetHPFilterFlag)
					{
						int[] numArray16 = new int[1];
						numArray16 = (int[])packet.PayLoad.Dequeue();
						DriverObj.SetHPFilter(numArray16);
					}
					SetHPFilterFlag = false;
					return;

				case 0x0C:
					if (SetCalFlag)
					{
						int[] numArray17 = new int[3];
						numArray17 = (int[])packet.PayLoad.Dequeue();
						DriverObj.SetCalValues(numArray17);
						OffsetDataPending = true;
					}
					SetCalFlag = false;
					return;

				case 0x0D:
					if (SetIntsEnableFlag)
					{
						numArray18 = new int[2];
						numArray18 = (int[])packet.PayLoad.Dequeue();
						DriverObj.SetIntsEnable(numArray18);
					}
					SetIntsEnableFlag = false;
					return;

				case 0x0E:
					if (SetIntsConfigFlag)
					{
						int[] numArray19 = new int[2];
						numArray19 = (int[])packet.PayLoad.Dequeue();
						DriverObj.SetIntConfig(numArray19);
					}
					SetIntsConfigFlag = false;
					return;

				case 0x10:
					{
						int[] numArray26 = new int[1];
						numArray26 = (int[])packet.PayLoad.Dequeue();
						ReturnRegs = true;
						return;
					}
				case 0x11:
					if (ReadValueFlag)
					{
						numArray27 = new int[1];
						numArray27 = (int[])packet.PayLoad.Dequeue();
						RegValue = DriverObj.ReadValue(numArray27);
						ReadDataPending = true;
					}
					ReadValueFlag = false;
					return;

				case 0x12:
					if (WriteValueFlag)
					{
						int[] numArray28 = new int[2];
						numArray28 = (int[])packet.PayLoad.Dequeue();
						DriverObj.WriteValue(numArray28);
					}
					WriteValueFlag = false;
					return;

				case 0x13:
					if (SetFIFOModeFlag)
					{
						int[] numArray20 = new int[1];
						numArray20 = (int[])packet.PayLoad.Dequeue();
						DriverObj.SetFIFOMode(numArray20);
					}
					SetFIFOModeFlag = false;
					return;

				case 20:
					if (SetFIFOWatermarkFlag)
					{
						int[] numArray21 = new int[1];
						numArray21 = (int[])packet.PayLoad.Dequeue();
						DriverObj.SetFIFOWatermark(numArray21);
					}
					SetFIFOWatermarkFlag = false;
					return;

				case 0x15:
					if (SetTrigTransFlag)
					{
						int[] numArray22 = new int[1];
						numArray22 = (int[])packet.PayLoad.Dequeue();
						DriverObj.SetTrigTrans(numArray22);
					}
					return;

				case 0x16:
					if (SetTrigLPFlag)
					{
						int[] numArray23 = new int[1];
						numArray23 = (int[])packet.PayLoad.Dequeue();
						DriverObj.SetTrigLP(numArray23);
					}
					return;

				case 0x17:
					if (SetTrigPulseFlag)
					{
						int[] numArray24 = new int[1];
						numArray24 = (int[])packet.PayLoad.Dequeue();
						DriverObj.SetTrigPulse(numArray24);
					}
					return;

				case 0x18:
					if (SetTrigMFFFlag)
					{
						int[] numArray25 = new int[1];
						numArray25 = (int[])packet.PayLoad.Dequeue();
						DriverObj.SetTrigMFF(numArray25);
					}
					return;

				case 0x19:
					if (SetEnablePLFlag)
					{
						int[] numArray31 = new int[1];
						numArray31 = (int[])packet.PayLoad.Dequeue();
						DriverObj.SetEnablePL(numArray31);
					}
					SetEnablePLFlag = false;
					return;

				case 0x1A:
					if (SetZLockAFlag)
					{
						int[] numArray32 = new int[1];
						numArray32 = (int[])packet.PayLoad.Dequeue();
						DriverObj.SetZLockA(numArray32);
					}
					SetZLockAFlag = false;
					return;

				case 0x1B:
					if (SetBFTripAFlag)
					{
						int[] numArray33 = new int[1];
						numArray33 = (int[])packet.PayLoad.Dequeue();
						DriverObj.SetBFTripA(numArray33);
					}
					SetBFTripAFlag = false;
					return;

				case 0x1C:
					if (SetPLTripAFlag)
					{
						int[] numArray34 = (int[])packet.PayLoad.Dequeue();
						int[] numArray35 = new int[1];
						if (numArray34[0] == 0)
							numArray35[0] = 7;
						if (numArray34[0] == 1)
							numArray35[0] = 9;
						if (numArray34[0] == 2)
							numArray35[0] = 12;
						if (numArray34[0] == 3)
							numArray35[0] = 13;
						if (numArray34[0] == 4)
							numArray35[0] = 15;
						if (numArray34[0] == 5)
							numArray35[0] = 0x10;
						if (numArray34[0] == 6)
							numArray35[0] = 0x13;
						if (numArray34[0] == 7)
							numArray35[0] = 20;
						if (numArray34[0] == 8)
							numArray35[0] = 0x17;
						if (numArray34[0] == 9)
							numArray35[0] = 0x19;
						DriverObj.SetPLTripA(numArray35);
					}
					SetPLTripAFlag = false;
					return;

				case 0x1D:
					if (SetHysteresisFlag)
						DriverObj.SetHysteresis((int[])packet.PayLoad.Dequeue());
					SetHysteresisFlag = false;
					return;

				case 30:
					if (SetDBCNTM_PLFlag)
						DriverObj.SetDBCNTM_PL((int[])packet.PayLoad.Dequeue());
					SetDBCNTM_PLFlag = false;
					return;

				case 0x1F:
					if (SetPLDebounceFlag)
						DriverObj.SetPLDebounce((int[])packet.PayLoad.Dequeue());
					SetPLDebounceFlag = false;
					return;

				case 0x20:
					if (SetINTPPODFlag)
						DriverObj.SetINTPPOD((int[])packet.PayLoad.Dequeue());
					SetINTPPODFlag = false;
					return;

				case 0x21:
					if (SetINTActiveFlag)
						DriverObj.SetINTPolarity((int[])packet.PayLoad.Dequeue());
					SetINTActiveFlag = false;
					return;

				case 0x23:
					if (SetTransBypassHPFFlag)
						DriverObj.SetTransBypassHPF((int[])packet.PayLoad.Dequeue());
					SetTransBypassHPFFlag = false;
					return;

				case 0x24:
					if (SetTransThresholdFlag)
						DriverObj.SetTransThreshold((int[])packet.PayLoad.Dequeue());
					SetTransThresholdFlag = false;
					return;

				case 0x25:
					if (SetTransDebounceFlag)
						DriverObj.SetTransDebounce((int[])packet.PayLoad.Dequeue());
					SetTransDebounceFlag = false;
					return;

				case 0x26:
					if (SetTransDBCNTMFlag)
						DriverObj.SetTransDBCNTM((int[])packet.PayLoad.Dequeue());
					SetTransDBCNTMFlag = false;
					return;

				case 0x27:
					if (SetTransEnableXFlagFlag)
						DriverObj.SetTransEnableXFlag((int[])packet.PayLoad.Dequeue());
					SetTransEnableXFlagFlag = false;
					return;

				case 40:
					if (SetTransEnableYFlagFlag)
						DriverObj.SetTransEnableYFlag((int[])packet.PayLoad.Dequeue());
					SetTransEnableYFlagFlag = false;
					return;

				case 0x29:
					if (SetTransEnableZFlagFlag)
						DriverObj.SetTransEnableZFlag((int[])packet.PayLoad.Dequeue());
					SetTransEnableZFlagFlag = false;
					return;

				case 0x2A:
					if (SetTransEnableLatchFlag)
						DriverObj.SetTransEnableLatch((int[])packet.PayLoad.Dequeue());
					SetTransEnableLatchFlag = false;
					return;

				case 0x2B:
					if (SetTransBypassHPFNEWFlag)
						DriverObj.SetTransBypassHPFNEW((int[])packet.PayLoad.Dequeue());
					SetTransBypassHPFNEWFlag = false;
					return;

				case 0x2C:
					if (SetTransEnableXFlagNEWFlag)
						DriverObj.SetTransEnableXFlagNEW((int[])packet.PayLoad.Dequeue());
					SetTransEnableXFlagFlag = false;
					return;

				case 0x2D:
					if (SetTransEnableYFlagNEWFlag)
						DriverObj.SetTransEnableYFlagNEW((int[])packet.PayLoad.Dequeue());
					SetTransEnableYFlagNEWFlag = false;
					return;

				case 0x2E:
					if (SetTransEnableZFlagNEWFlag)
						DriverObj.SetTransEnableZFlagNEW((int[])packet.PayLoad.Dequeue());
					SetTransEnableZFlagNEWFlag = false;
					return;

				case 0x2F:
					if (SetTransEnableLatchNEWFlag)
						DriverObj.SetTransEnableLatchNEW((int[])packet.PayLoad.Dequeue());
					SetTransEnableLatchNEWFlag = false;
					return;

				case 0x30:
					if (SetTransThresholdNEWFlag)
						DriverObj.SetTransThresholdNEW((int[])packet.PayLoad.Dequeue());
					SetTransThresholdNEWFlag = false;
					return;

				case 0x31:
					if (SetTransDebounceNEWFlag)
						DriverObj.SetTransDebounceNEW((int[])packet.PayLoad.Dequeue());
					SetTransDebounceNEWFlag = false;
					return;

				case 50:
					if (SetTransDBCNTMNEWFlag)
						DriverObj.SetTransDBCNTMNEW((int[])packet.PayLoad.Dequeue());
					SetTransDBCNTMNEWFlag = false;
					return;

				case 0x33:
					if (SetMFF1AndOrFlag)
						DriverObj.SetMFF1AndOr((int[])packet.PayLoad.Dequeue());
					SetMFF1AndOrFlag = false;
					return;

				case 0x34:
					if (SetMFF1LatchFlag)
						DriverObj.SetMFF1Latch((int[])packet.PayLoad.Dequeue());
					SetMFF1LatchFlag = false;
					return;

				case 0x35:
					if (SetMFF1XEFEFlag)
						DriverObj.SetMFF1XEFE((int[])packet.PayLoad.Dequeue());
					SetMFF1XEFEFlag = false;
					return;

				case 0x36:
					if (SetMFF1YEFEFlag)
						DriverObj.SetMFF1YEFE((int[])packet.PayLoad.Dequeue());
					SetMFF1YEFEFlag = false;
					return;

				case 0x37:
					if (SetMFF1ZEFEFlag)
						DriverObj.SetMFF1ZEFE((int[])packet.PayLoad.Dequeue());
					SetMFF1ZEFEFlag = false;
					return;

				case 0x38:
					if (SetMFF1ThresholdFlag)
						DriverObj.SetMFF1Threshold((int[])packet.PayLoad.Dequeue());
					SetMFF1ThresholdFlag = false;
					return;

				case 0x39:
					if (SetMFF1DebounceFlag)
						DriverObj.SetMFF1Debounce((int[])packet.PayLoad.Dequeue());
					SetMFF1DebounceFlag = false;
					return;

				case 0x3A:
					if (SetMFF1DBCNTMFlag)
						DriverObj.SetMFF1DBCNTM((int[])packet.PayLoad.Dequeue());
					SetMFF1DBCNTMFlag = false;
					return;

				case 0x3B:
					if (SetPulseDPAFlag)
					{
						int[] numArray73 = new int[1];
						numArray73 = (int[])packet.PayLoad.Dequeue();
						DriverObj.SetPulseDPA(numArray73);
					}
					SetPulseDPAFlag = false;
					return;

				case 60:
					if (SetPulseLatchFlag)
						DriverObj.SetPulseLatch((int[])packet.PayLoad.Dequeue());
					SetPulseLatchFlag = false;
					return;

				case 0x3D:
					if (SetPulseZDPFlag)
					{
						int[] numArray76 = new int[1];
						numArray76 = (int[])packet.PayLoad.Dequeue();
						DriverObj.SetPulseZDP(numArray76);
					}
					SetPulseZDPFlag = false;
					return;

				case 0x3E:
					if (SetPulseZSPFlag)
					{
						int[] numArray67 = new int[1];
						numArray67 = (int[])packet.PayLoad.Dequeue();
						DriverObj.SetPulseZSP(numArray67);
					}
					SetPulseZSPFlag = false;
					return;

				case 0x3F:
					if (SetPulseYDPFlag)
						DriverObj.SetPulseYDP((int[])packet.PayLoad.Dequeue());
					SetPulseYDPFlag = false;
					return;

				case 0x40:
					if (SetPulseYSPFlag)
					{
						int[] numArray66 = new int[1];
						numArray66 = (int[])packet.PayLoad.Dequeue();
						DriverObj.SetPulseYSP(numArray66);
					}
					SetPulseYSPFlag = false;
					return;

				case 0x41:
					if (SetPulseXDPFlag)
						DriverObj.SetPulseXDP((int[])packet.PayLoad.Dequeue());
					SetPulseXDPFlag = false;
					return;

				case 0x42:
					if (SetPulseXSPFlag)
						DriverObj.SetPulseXSP((int[])packet.PayLoad.Dequeue());
					SetPulseXSPFlag = false;
					return;

				case 0x43:
					if (SetPulseFirstTimeLimitFlag)
						DriverObj.SetPulseFirstTimeLimit((int[])packet.PayLoad.Dequeue());
					SetPulseFirstTimeLimitFlag = false;
					return;

				case 0x44:
					if (SetPulseXThresholdFlag)
						DriverObj.SetPulseXThreshold((int[])packet.PayLoad.Dequeue());
					SetPulseXThresholdFlag = false;
					return;

				case 0x45:
					if (SetPulseYThresholdFlag)
						DriverObj.SetPulseYThreshold((int[])packet.PayLoad.Dequeue());
					SetPulseYThresholdFlag = false;
					return;

				case 70:
					if (SetPulseZThresholdFlag)
					{
						int[] numArray72 = new int[1];
						numArray72 = (int[])packet.PayLoad.Dequeue();
						DriverObj.SetPulseZThreshold(numArray72);
					}
					SetPulseZThresholdFlag = false;
					return;

				case 0x47:
					if (SetPulseLatencyFlag)
						DriverObj.SetPulseLatency((int[])packet.PayLoad.Dequeue());
					SetPulseLatencyFlag = false;
					return;

				case 0x48:
					if (SetPulse2ndPulseWinFlag)
						DriverObj.SetPulse2ndPulseWin((int[])packet.PayLoad.Dequeue());
					SetPulse2ndPulseWinFlag = false;
					return;

				case 80:
					if (PollingFIFOFlag)
					{
						int[] numArray79 = null;
						numArray79 = (int[])packet.PayLoad.Dequeue();

						PollingOrInt[0] = numArray79[0];
						FIFOWatermark = numArray79[1];
						FIFODump8orFull = numArray79[2];
						CurrentFIFOMode = numArray79[3];
						FullScaleValue = numArray79[4];
						ReturnFIFOStatus = true;
					}
					PollingFIFOFlag = false;
					return;

				case 0x51:
					if (BootFlag)
					{
						packet.PayLoad.Dequeue();
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
						int[] numArray80 = (int[])packet.PayLoad.Dequeue();
						int deviceID = numArray80[0];

						int num11 = num12 = num13 = 0;
						double num14 = num15 = num16 = 0.0;
						int[] numArray81 = new int[]{0};
						int[] numArray82 = new int[]{7};
						int[] numArray83 = new int[1];
						int[] numArray84 = new int[]{0xff};
						int[] numArray85 = new int[]{2};
						DriverObj.SetFS(numArray81);
						DriverObj.SetDataRate(numArray82);
						DriverObj.SetLowNoise(numArray84);
						DriverObj.SetWakeOSMode(numArray85);

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
							numArray83[0] = 1;
							DriverObj.SetActive(numArray83);
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
											num2 = (short)(num2 - 0x4000);
										if (num3 > 0x2000)
											num3 = (short)(num3 - 0x4000);
										if (num4 > 0x2000)
											num4 = (short)(num4 - 0x4000);
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
										num2 = (short)(num2 - 0x1000);
									if (num3 > 0x7ff)
										num3 = (short)(num3 - 0x1000);
									if (num4 > 0x7ff)
										num4 = (short)(num4 - 0x1000);
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
										num2 = (short)(num2 - 0x400);
									if (num3 > 0x200)
										num3 = (short)(num3 - 0x400);
									if (num4 > 0x200)
										num4 = (short)(num4 - 0x400);
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
										num2 = (short)(num2 - 0x1000);
									if (num3 > 0x7ff)
										num3 = (short)(num3 - 0x1000);
									if (num4 > 0x7ff)
										num4 = (short)(num4 - 0x1000);
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
										num2 = (short)(num2 - 0x400);
									if (num3 > 0x200)
										num3 = (short)(num3 - 0x400);
									if (num4 > 0x200)
										num4 = (short)(num4 - 0x400);
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
											num2 = (short)(num2 - 0x100);
										if (num3 > 0x80)
											num3 = (short)(num3 - 0x100);
										if (num4 > 0x80)
											num4 = (short)(num4 - 0x100);
										num11 += -8 * num2;
										num12 += -8 * num3;
										num13 += 8 * (0x40 - num4);
										break;
									}
							}
							numArray83[0] = 0;
							DriverObj.SetActive(numArray83);
							int[] numArray86 = new int[] { num11, num12, num13 };
							DriverObj.SetCalValues(numArray86);
						}
						OffsetDataPending = true;
					}
					AutoCalFlag = false;
					return;

				case 0x53:
					if (ActiveFlag)
					{
						int[] numArray6 = new int[1];
						numArray6 = (int[])packet.PayLoad.Dequeue();
						DriverObj.SetActive(numArray6);
					}
					return;

				case 0x54:
					if (SetWakeOSModeFlag)
					{
						numArray8 = new int[1];
						numArray8 = (int[])packet.PayLoad.Dequeue();
						DriverObj.SetWakeOSMode(numArray8);
					}
					SetWakeOSModeFlag = false;
					return;

				case 0x55:
					if (SetSleepOSModeFlag)
					{
						numArray8 = new int[1];
						numArray8 = (int[])packet.PayLoad.Dequeue();
						DriverObj.SetSleepOSMode(numArray8);
					}
					SetSleepOSModeFlag = false;
					return;

				case 0x56:
					if (SetHPFDataOutFlag)
					{
						numArray8 = new int[1];
						numArray8 = (int[])packet.PayLoad.Dequeue();
						DriverObj.SetHPFDataOut(numArray8);
					}
					SetHPFDataOutFlag = false;
					return;

				case 0x57:
					if (SetEnableAnalogLowNoiseFlag)
					{
						int[] numArray7 = new int[1];
						numArray7 = (int[])packet.PayLoad.Dequeue();
						DriverObj.SetLowNoise(numArray7);
					}
					SetEnableAnalogLowNoiseFlag = false;
					return;

				case 0x58:
					if (SetFREADFlag)
						DriverObj.SetFREAD((int[])packet.PayLoad.Dequeue());
					SetFREADFlag = false;
					return;

				case 0x59:
					if (SetPulseLPFEnableFlag)
						DriverObj.SetPulseLPFEnable((int[])packet.PayLoad.Dequeue());
					SetPulseLPFEnableFlag = false;
					return;

				case 90:
					if (SetPulseHPFBypassFlag)
						DriverObj.SetPulseHPFBypass((int[])packet.PayLoad.Dequeue());
					SetPulseHPFBypassFlag = false;
					return;

				case 0x5b:
					if (Data10bFlag)
					{
						int[] numArray5 = (int[])packet.PayLoad.Dequeue();
						PollingOrInt[0] = numArray5[0];
						buffer = new byte[] { 4 };
						if (PollingOrInt[0] != 0)
							DriverObj.ConfigureStreamerIntData(GlobalDataRate, RequestModeType.FBMR, buffer);
						else
							DriverObj.ConfigureStreamerPoll(RequestModeType.FBMR);
						Data10StreamEnable = true;
					}
					Data10bFlag = false;
					return;

				case 0x5c:
					if (Data14bFlag)
					{
						numArray3 = (int[])packet.PayLoad.Dequeue();
						PollingOrInt[0] = numArray3[0];
						buffer = new byte[] { 2 };
						if (PollingOrInt[0] != 0)
							DriverObj.ConfigureStreamerIntData(GlobalDataRate, RequestModeType.FBMR, buffer);
						else
							DriverObj.ConfigureStreamerPoll(RequestModeType.FBMR);
						Data14StreamEnable = true;
					}
					Data14bFlag = false;
					return;

				case 0x5d:
					if (ReadValueParsedFlag)
					{
						RegParsedValues = DriverObj.ReadParsedValue((int[])packet.PayLoad.Dequeue());
						ReadParsedDataPending = true;
					}
					ReadValueParsedFlag = false;
					return;

				case 0x5e:
					DriverObj.DisableStreaming();
					return;

				case 0x5f:
					if (StreamIntFnFlag)
					{
						int[] numArray87 = (int[])packet.PayLoad.Dequeue();
						PollingOrInt[0] = numArray87[0];
						buffer = new byte[] { (byte)numArray87[1] };
						if (PollingOrInt[0] != 0)
							DriverObj.ConfigureStreamerIntData(GlobalDataRate, RequestModeType.FBMR, buffer);
						else
							DriverObj.ConfigureStreamerPoll(RequestModeType.FBMR);
						StreamIntFnFlag = true;
					}
					StreamIntFnFlag = false;
					return;

				case 0x60:
					if (SetResetFlag)
						DriverObj.ResetDevice();
					SetResetFlag = false;
					return;

				case 0x61:
					if (SetStartFlag)
						DriverObj.StartDevice();
					SetStartFlag = false;
					return;

				case 0x62:
					if (DataFIFO8Flag)
					{
						numArray2 = (int[])packet.PayLoad.Dequeue();
						passDataFSVal[0] = numArray2[0];
						PollingOrInt[0] = numArray2[1];
						buffer = new byte[] { 6, (byte)numArray2[0] };
						if (PollingOrInt[0] != 0)
							DriverObj.ConfigureStreamerIntData(SampleRate.InterruptMode, RequestModeType.FBMR, buffer);
						else
							DriverObj.ConfigureStreamerPoll(RequestModeType.FBMR);
						DataFIFO8StreamEnable = true;
					}
					DataFIFO8Flag = false;
					return;

				case 0x63:
					if (DataFIFO14Flag)
					{
						numArray3 = (int[])packet.PayLoad.Dequeue();
						passDataFSVal[0] = numArray3[0];
						PollingOrInt[0] = numArray3[1];
						buffer = new byte[] { 7, (byte)numArray3[0] };
						if (PollingOrInt[0] != 0)
							DriverObj.ConfigureStreamerIntData(SampleRate.InterruptMode, RequestModeType.FBMR, buffer);
						else
							DriverObj.ConfigureStreamerPoll(RequestModeType.FBMR);
						DataFIFO14StreamEnable = true;
					}
					DataFIFO14Flag = false;
					return;

				case 100:
					if (SetIntsEnableFlag)
					{
						PollingOrInt[0] = 1;
						DriverObj.SetIntsEnableBit((int[])packet.PayLoad.Dequeue());
					}
					return;

				case 0x69:
					{
						DriverObj.WriteAppRegister((int[])packet.PayLoad.Dequeue());
						return;
					}
				case 0x6a:
					{
						RegValue = DriverObj.ReadValue((int[])packet.PayLoad.Dequeue());
						ReadDataPending = true;
						return;
					}
				default:
					return;
			}
			Data8StreamEnable = true;

		Label_029D:
			Data8bFlag = false;
		}
		#endregion

		#region ProcessRequest 
		private void ProcessRequest()
		{
			while (true)
			{
				if (AbortThread.WaitOne(10, false))
					return;
				RWLock.AcquireWriterLock(-1);
				try
				{
					if (ControllerQueue.Count != 0)
						DecodeRequestToken();
				}
				finally
				{
					RWLock.ReleaseWriterLock();
				}

				if (Data8StreamEnable)
					UpdateStream8b();
				else
					DriverObj.DataOut8StreamFlush();

				if (DataFIFO8StreamEnable)
					UpdateStreamFIFO8b();
				else if (DataFIFO14StreamEnable)
					UpdateStreamFIFO14b();
				else
					DriverObj.DataOutFIFOStreamFlush();

				if (Data12StreamEnable)
					UpdateStreamFull();
				else if (Data10StreamEnable)
					UpdateStreamFull();
				else if (Data14StreamEnable)
					UpdateStreamFull();
				else
					DriverObj.DataOutFullStreamFlush();

				if (ReturnSysmodStatus)
				{
					GUIUpdatePacket packet = new GUIUpdatePacket { TaskID = 0x4a };
					packet.PayLoad.Enqueue(DriverObj.GetModeStatus());
					GUIQueue.Enqueue(packet);
				}
				if (ReturnDataStatus)
				{
					GUIUpdatePacket packet2 = new GUIUpdatePacket { TaskID = 0x4e };
					packet2.PayLoad.Enqueue(DriverObj.GetDataStatus());
					GUIQueue.Enqueue(packet2);
				}
				if (PollInterruptMapStatus)
				{
					GUIUpdatePacket packet3 = new GUIUpdatePacket { TaskID = 15 };
					packet3.PayLoad.Enqueue(DriverObj.GetInterruptStatus());
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
					GUIUpdatePacket packet4 = new GUIUpdatePacket { TaskID = 80 };
					packet4.PayLoad.Enqueue(fIFOStatus);
					packet4.PayLoad.Enqueue(fIFOData);
					GUIQueue.Enqueue(packet4);
				}
				if (ReturnMFF1Status)
				{
					GUIUpdatePacket packet5 = new GUIUpdatePacket { TaskID = 0x4c };
					packet5.PayLoad.Enqueue(DriverObj.GetFFMT1Status());
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
							packet6 = new GUIUpdatePacket { TaskID = 0x4f };
							packet6.PayLoad.Enqueue(updatePLStat);
							ControllerCallbackCaller(ControllerEventType.Orientation, packet6);
						}
					}
					else
					{
						packet6 = new GUIUpdatePacket { TaskID = 0x4f };
						packet6.PayLoad.Enqueue(DriverObj.GetPLStatus());
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
							packet7 = new GUIUpdatePacket { TaskID = 0x49 };
							packet7.PayLoad.Enqueue(updateTransStat);
							ControllerCallbackCaller(ControllerEventType.Transient, packet7);
						}
					}
					else
					{
						packet7 = new GUIUpdatePacket { TaskID = 0x49 };
						packet7.PayLoad.Enqueue(DriverObj.GetTransientStatus());
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
							packet8 = new GUIUpdatePacket
							{
								TaskID = 0x4b
							};
							packet8.PayLoad.Enqueue(updatePulseStat);
							ControllerCallbackCaller(ControllerEventType.Pulse, packet8);
						}
					}
					else
					{
						packet8 = new GUIUpdatePacket { TaskID = 0x4b };
						packet8.PayLoad.Enqueue(DriverObj.GetPulseStatus());
						GUIQueue.Enqueue(packet8);
					}
				}
				else
				{
					DriverObj.PulseFlush();
				}
				if (ReturnTrans1Status)
				{
					GUIUpdatePacket packet9 = new GUIUpdatePacket { TaskID = 0x4d };
					packet9.PayLoad.Enqueue(DriverObj.GetTrans1Status());
					GUIQueue.Enqueue(packet9);
				}
				if (OffsetDataPending)
				{
					GUIUpdatePacket packet10 = new GUIUpdatePacket { TaskID = 12 };
					packet10.PayLoad.Enqueue(DriverObj.GetCal());
					GUIQueue.Enqueue(packet10);
					OffsetDataPending = false;
				}
				if (ReadDataPending)
				{
					GUIUpdatePacket packet11 = new GUIUpdatePacket { TaskID = 0x11 };
					packet11.PayLoad.Enqueue(RegValue);
					GUIQueue.Enqueue(packet11);
					ReadDataPending = false;
				}
				if (ReadParsedDataPending)
				{
					GUIUpdatePacket packet12 = new GUIUpdatePacket { TaskID = 0x5d };
					packet12.PayLoad.Enqueue(RegParsedValues);
					GUIQueue.Enqueue(packet12);
					ReadParsedDataPending = false;
				}
				if (ReturnRegs)
				{
					GUIUpdatePacket packet13 = new GUIUpdatePacket { TaskID = 0x10 };
					packet13.PayLoad.Enqueue(DriverObj.GetAllRegs());
					GUIQueue.Enqueue(packet13);
					ReturnRegs = false;
				}
			}
		}
		#endregion

		#region UpdateShakeInts 
		private void UpdateShakeInts()
		{
			byte o = 0;
			byte[] buffer = new byte[1];
			o = DriverObj.GetFFMT1Status()[6];
			ControllerCallbackCaller(ControllerEventType.Shake, o);
		}
		#endregion

		#region UpdateStream8b 
		private void UpdateStream8b()
		{
			DataOut8Packet packet = null;
			XYZCounts o = null;
			if (DriverObj.IsDataOut8StreamAvailable(ref packet))
			{
				if (o != null)
				{
					o = null;
				}
				GUIUpdatePacket packet2 = new GUIUpdatePacket();
				o = new XYZCounts();
				o = packet.AxisCounts;
				ControllerCallbackCaller(ControllerEventType.DataOut8Packet, o);
			}
		}
		#endregion

		#region UpdateStreamFIFO14b 
		private void UpdateStreamFIFO14b()
		{
			DataOutFullPacket packet = null;
			if (DriverObj.IsDataOutFIFO14StreamAvailable(ref packet))
			{
				ControllerCallbackCaller(ControllerEventType.DataOutFIFOPacket, packet);
			}
		}
		#endregion

		#region UpdateStreamFIFO8b 
		private void UpdateStreamFIFO8b()
		{
			DataOutFIFOPacket packet = null;
			XYZCounts[] destinationArray = new XYZCounts[0x20];
			while (DriverObj.IsDataOutFIFO8StreamAvailable(ref packet))
			{
				GUIUpdatePacket packet2 = new GUIUpdatePacket();
				destinationArray = new XYZCounts[0x20];
				Array.Copy(packet.FIFOData, destinationArray, packet.FIFOData.Length);
				ControllerCallbackCaller(ControllerEventType.DataOutFIFOPacket, destinationArray);
			}
		}
		#endregion

		#region UpdateStreamFull 
		private void UpdateStreamFull()
		{
			DataOutFullPacket dOutFullPacket = null;
			if (DriverObj.IsDataOutFullStreamAvailable(ref dOutFullPacket))
			{
				GUIUpdatePacket packet2 = new GUIUpdatePacket();
				XYZCounts axisCounts = dOutFullPacket.AxisCounts;
				ControllerCallbackCaller(ControllerEventType.DataOutFullPacket, axisCounts);
			}
		}
		#endregion
	}
}

