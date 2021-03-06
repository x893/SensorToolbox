﻿namespace NVMDatalogger
{
    using Freescale.SASD.Communication;
    using System;
    using System.Collections;
    using System.Threading;

	internal class MMA845xDriver
	{
		private BlockingComm BlockingCommObj = new BlockingComm();
		private bool CommOpen;
		private ICom ComObj;
		private int CurDeviceAddress;
		private RequestModeType CurrentStreamRequestMode;
		public XYZCounts DataArrayObj = new XYZCounts();
		private Queue DataConfigInterruptQueue;
		private DataOut8Packet DataOut8PacketObj = new DataOut8Packet();
		private Queue DataOut8StreamQueue = new Queue();
		public DataOutFIFOPacket DataOutFIFOPacketObj = new DataOutFIFOPacket();
		private Queue DataOutFIFOStreamQueue = new Queue();
		private DataOutFullPacket DataOutFullPacketObj = new DataOutFullPacket();
		private Queue DataOutFullStreamQueue = new Queue();
		private const int DEVICETYPE = 2;
		private Queue FfMt1InterruptQueue;
		private Queue FIFOInterruptQueue;
		private MMA845x_Class MMA845x = new MMA845x_Class();
		private Queue OrientationInterruptQueue;
		private Queue PulseInterruptQueue;
		private static ReaderWriterLock RWLock = new ReaderWriterLock();
		private const int SUCCESS = 1;
		private Queue SysmodInterruptQueue;
		public int SystemPollingOrInterrupt;
		private Queue TransientInterruptQueue;
		public int XYZ12DatalogFlag = 0;
		public int XYZ8DatalogFlag = 0;
		private bool XYZStreamEnable;
		public int XYZTransDatalogFlag = 0;

		public MMA845xDriver()
		{
			ComObj = BlockingCommObj;
			BlockingCommObj.CommEvents += new CommEventHandler(CommEventCallback);
			BlockingCommObj.AsynchronousEvents += new MCUToHostEventHandler(AsynchronousEventsCallback);
			XYZStreamEnable = false;
			CurrentStreamRequestMode = RequestModeType.NONE;
			FfMt1InterruptQueue = new Queue();
			OrientationInterruptQueue = new Queue();
			TransientInterruptQueue = new Queue();
			PulseInterruptQueue = new Queue();
			SysmodInterruptQueue = new Queue();
			DataConfigInterruptQueue = new Queue();
			FIFOInterruptQueue = new Queue();
		}

		private void AsynchronousEventsCallback(object sender, byte[] packet, OperationId oid)
		{
			if (oid == OperationId.InterruptData)
			{
				InterruptDetectionCallBack(sender, packet);
			}
			else if (oid == OperationId.StreamData)
			{
				StreamerCallBack(sender, packet);
			}
		}

		public void Boot()
		{
			byte dataReceived = 0;
			if (ComObj.ReadData(0x2b, ref dataReceived) == Result.eERR_SUCCESS)
			{
				dataReceived = (byte)(dataReceived | 0x40);
				ComObj.WriteData(0x2b, dataReceived);
			}
		}

		public void CloseCom()
		{
			ComObj.Close();
		}

		private void CommEventCallback(object sender, CommEvent cmd)
		{
			if (cmd == CommEvent.CommOpen)
			{
				string commStatus = BlockingCommObj.GetCommStatus();
				commStatus = commStatus.Substring(commStatus.IndexOf("HW:") + 3, 4);
				MMA845x.SetDeviceId(commStatus);
			}
			else if (cmd == CommEvent.CommLost)
			{
			}
		}

		public void ConfigureStreamerIntData(SampleRate sampleRate, RequestModeType requestMode, byte[] requestTokens)
		{
			SystemPollingOrInterrupt = 1;
			CurrentStreamRequestMode = requestMode;
			EnableDataStream(sampleRate, requestMode, requestTokens);
		}

		public void ConfigureStreamerInteruptBlocks()
		{
		}

		public void ConfigureStreamerPoll(RequestModeType requestMode)
		{
			SystemPollingOrInterrupt = 0;
			CurrentStreamRequestMode = requestMode;
		}

		public void DataOut8StreamFlush()
		{
			lock (DataOut8StreamQueue)
			{
				DataOut8StreamQueue.Clear();
			}
		}

		public void DataOutFIFOStreamFlush()
		{
			for (int i = 0; i < DataOutFIFOStreamQueue.Count; i++)
			{
				DataOutFIFOStreamQueue.Dequeue();
			}
		}

		public void DataOutFullStreamFlush()
		{
			lock (DataOutFullStreamQueue)
			{
				DataOutFullStreamQueue.Clear();
			}
		}

		private void DisableDataStream()
		{
			byte[] dataBuf = new byte[4];
			ComObj.AppRegisterWrite(4, 0x10, dataBuf);
		}

		public void DisableInterruptDetection()
		{
			DisableInterrupts();
		}

		public void DisableInterrupts()
		{
		}

		public void DisableStreaming()
		{
			DisableDataStream();
		}

		public byte[] DLDownload()
		{
			byte[] dataReceived = new byte[0];
			ComObj.NVMReadData(ref dataReceived);
			return dataReceived;
		}

		public void DLStart()
		{
			ComObj.NVMWriteData();
		}

		private void EnableDataStream(SampleRate sampleRate, RequestModeType requestMode, byte[] rT)
		{
			byte[] buffer;
			byte[] buffer2;
			if (rT.Length > 1)
			{
				buffer2 = new byte[4];
				buffer2[3] = rT[1];
				buffer = buffer2;
			}
			else
			{
				buffer2 = new byte[4];
				buffer = buffer2;
			}
			if (requestMode == RequestModeType.FBMR)
			{
				buffer[0] = (byte)MMA845x.GetLength((FBID)rT[0]);
				buffer[1] = (byte)MMA845x.GetRegAddress((FBID)rT[0]);
				buffer[2] = (byte)sampleRate;
				if (Result.eERR_SUCCESS == ComObj.AppRegisterWrite(4, 0x10, buffer))
				{
					MMA845x.DsFBID = (FBID)rT[0];
				}
			}
			else
			{
				if (rT.Length == 5)
				{
					buffer = new byte[] { rT[1], rT[2], rT[3], rT[4] };
				}
				if (Result.eERR_SUCCESS == ComObj.AppRegisterWrite(4, 0x10, buffer))
				{
					MMA845x.DsFBID = (FBID)rT[0];
				}
			}
		}

		public void EnableInterruptDetection(byte Int1or2)
		{
			EnableInterrupts(Int1or2, 1);
		}

		private void EnableInterrupts(int mask, int polarity)
		{
			throw new NotImplementedException();
		}

		private int[] EncodeOrientationPacket(int[] PLData)
		{
			int[] numArray = new int[5];
			if (PLData.Length >= 1)
			{
				numArray[0] = PLData[0] & 1;
				numArray[1] = (PLData[0] & 6) >> 1;
				numArray[2] = (PLData[0] & 0x40) >> 6;
				numArray[3] = (PLData[0] & 0x80) >> 7;
			}
			return numArray;
		}

		public void EndDriver()
		{
			BlockingCommObj.End();
		}

		public byte[] GetAllRegs()
		{
			Result result = Result.eERR_INITIAL_STATE;
			byte dataReceived = 0;
			byte[] buffer = new byte[50];
			for (int i = 0; i < 50; i++)
			{
				dataReceived = 0;
				result = ComObj.ReadData(i, ref dataReceived);
				buffer[i] = dataReceived;
			}
			return buffer;
		}

		public int[] GetCal()
		{
			byte[] dataReceived = null;
			int[] numArray = new int[3];
			if (ComObj.ReadData(3, 0x2f, ref dataReceived) == Result.eERR_SUCCESS)
			{
				numArray[0] = dataReceived[0];
				numArray[1] = dataReceived[1];
				numArray[2] = dataReceived[2];
			}
			return numArray;
		}

		public void GetCommObject(ref CommClass a)
		{
			try
			{
				BlockingCommObj.GetCommObj(ref a);
			}
			catch (Exception)
			{
			}
		}

		public byte[] GetDataStatus()
		{
			byte[] buffer = new byte[8];
			byte dataReceived = 0;
			ComObj.ReadData(0, ref dataReceived);
			if ((dataReceived & 1) == 1)
			{
				buffer[0] = 1;
			}
			else
			{
				buffer[0] = 0;
			}
			if ((dataReceived & 2) == 2)
			{
				buffer[1] = 1;
			}
			else
			{
				buffer[1] = 0;
			}
			if ((dataReceived & 4) == 4)
			{
				buffer[2] = 1;
			}
			else
			{
				buffer[2] = 0;
			}
			if ((dataReceived & 8) == 8)
			{
				buffer[3] = 1;
			}
			else
			{
				buffer[3] = 0;
			}
			if ((dataReceived & 0x10) == 0x10)
			{
				buffer[4] = 1;
			}
			else
			{
				buffer[4] = 0;
			}
			if ((dataReceived & 0x20) == 0x20)
			{
				buffer[5] = 1;
			}
			else
			{
				buffer[5] = 0;
			}
			if ((dataReceived & 0x40) == 0x40)
			{
				buffer[6] = 1;
			}
			else
			{
				buffer[6] = 0;
			}
			if ((dataReceived & 0x80) == 0x80)
			{
				buffer[7] = 1;
				return buffer;
			}
			buffer[7] = 0;
			return buffer;
		}

		public byte[] GetDeviceInfo()
		{
			byte[] deviceID = new byte[0];
			ComObj.GetDeviceInfo(ref deviceID);
			return deviceID;
		}

		public byte[] GetFFMT1Status()
		{
			byte dataReceived = 0;
			Result result = Result.eERR_INITIAL_STATE;
			byte[] buffer = new byte[7];
			result = ComObj.ReadData(0x16, ref dataReceived);
			if ((dataReceived & 2) == 2)
			{
				buffer[0] = 1;
			}
			else
			{
				buffer[0] = 0;
			}
			if ((dataReceived & 1) == 1)
			{
				buffer[1] = 1;
			}
			else
			{
				buffer[1] = 0;
			}
			if ((dataReceived & 8) == 8)
			{
				buffer[2] = 1;
			}
			else
			{
				buffer[2] = 0;
			}
			if ((dataReceived & 4) == 4)
			{
				buffer[3] = 1;
			}
			else
			{
				buffer[3] = 0;
			}
			if ((dataReceived & 0x20) == 0x20)
			{
				buffer[4] = 1;
			}
			else
			{
				buffer[4] = 0;
			}
			if ((dataReceived & 0x10) == 0x10)
			{
				buffer[5] = 1;
			}
			else
			{
				buffer[5] = 0;
			}
			if ((dataReceived & 0x80) == 0x80)
			{
				buffer[6] = 1;
				return buffer;
			}
			buffer[6] = 0;
			return buffer;
		}

		public int[] GetFIFOData(int samples, int FIFODump8orFull)
		{
			int num2;
			int num = 0;
			if (FIFODump8orFull == 1)
			{
				num = 1;
			}
			else
			{
				num = 2;
			}
			int[] numArray = new int[(samples * 3) * num];
			int[] numArray2 = new int[(samples * 3) * num];
			byte[] dataReceived = new byte[(samples * 3) * num];
			dataReceived = null;
			if (FIFODump8orFull == 1)
			{
				ComObj.ReadData(samples * 3, 1, ref dataReceived);
				for (num2 = 0; num2 < (samples * 3); num2++)
				{
					numArray[num2] = dataReceived[num2];
					if ((numArray[num2] & 0x80) == 0x80)
					{
						numArray2[num2] = ~numArray[num2] + 1;
						numArray2[num2] &= 0xff;
						numArray2[num2] *= -1;
					}
					else
					{
						numArray2[num2] = numArray[num2];
					}
				}
				return numArray2;
			}
			if (ComObj.ReadData((samples * 3) * 2, 1, ref dataReceived) == Result.eERR_SUCCESS)
			{
				for (num2 = 0; num2 < (samples * 3); num2++)
				{
					numArray[num2] = (dataReceived[num2 * 2] << 6) + (dataReceived[(num2 * 2) + 1] >> 2);
					if ((numArray[num2] & 0x2000) == 0x2000)
					{
						numArray2[num2] = ~numArray[num2] + 1;
						numArray2[num2] &= 0x3fff;
						numArray2[num2] *= -1;
					}
					else
					{
						numArray2[num2] = numArray[num2];
					}
				}
			}
			return numArray2;
		}

		public byte[] GetFIFOStatus()
		{
			byte[] buffer = new byte[3];
			byte dataReceived = 0;
			ComObj.ReadData(0, ref dataReceived);
			if ((dataReceived & 0x80) == 0x80)
			{
				buffer[0] = 1;
			}
			else
			{
				buffer[0] = 0;
			}
			if ((dataReceived & 0x40) == 0x40)
			{
				buffer[1] = 1;
			}
			else
			{
				buffer[1] = 0;
			}
			dataReceived = (byte)(dataReceived & 0x3f);
			buffer[2] = dataReceived;
			return buffer;
		}

		public byte[] GetInterruptStatus()
		{
			byte[] buffer = new byte[8];
			byte dataReceived = 0;
			ComObj.ReadData(12, ref dataReceived);
			if ((dataReceived & 1) == 1)
			{
				buffer[0] = 1;
			}
			else
			{
				buffer[0] = 0;
			}
			if ((dataReceived & 2) == 2)
			{
				buffer[1] = 1;
			}
			else
			{
				buffer[1] = 0;
			}
			if ((dataReceived & 4) == 4)
			{
				buffer[2] = 1;
			}
			else
			{
				buffer[2] = 0;
			}
			if ((dataReceived & 8) == 8)
			{
				buffer[3] = 1;
			}
			else
			{
				buffer[3] = 0;
			}
			if ((dataReceived & 0x10) == 0x10)
			{
				buffer[4] = 1;
			}
			else
			{
				buffer[4] = 0;
			}
			if ((dataReceived & 0x20) == 0x20)
			{
				buffer[5] = 1;
			}
			else
			{
				buffer[5] = 0;
			}
			if ((dataReceived & 0x40) == 0x40)
			{
				buffer[6] = 1;
			}
			else
			{
				buffer[6] = 0;
			}
			if ((dataReceived & 0x80) == 0x80)
			{
				buffer[7] = 1;
				return buffer;
			}
			buffer[7] = 0;
			return buffer;
		}

		public byte[] GetModeStatus()
		{
			byte[] buffer = new byte[4];
			byte dataReceived = 0;
			if (ComObj.ReadData(11, ref dataReceived) == Result.eERR_SUCCESS)
			{
				if ((dataReceived & 3) == 0)
				{
					buffer[0] = 1;
					buffer[1] = 0;
					buffer[2] = 0;
					return buffer;
				}
				if ((dataReceived & 3) == 1)
				{
					buffer[0] = 0;
					buffer[1] = 1;
					buffer[2] = 0;
					return buffer;
				}
				if ((dataReceived & 3) == 2)
				{
					buffer[0] = 0;
					buffer[1] = 0;
					buffer[2] = 1;
				}
			}
			return buffer;
		}

		public byte[] GetPLStatus()
		{
			byte[] buffer = new byte[4];
			byte dataReceived = 0;
			ComObj.ReadData(0x10, ref dataReceived);
			if ((dataReceived & 1) == 0)
			{
				buffer[0] = 0;
			}
			else
			{
				buffer[0] = 1;
			}
			if ((dataReceived & 6) == 0)
			{
				buffer[1] = 0;
			}
			else if ((dataReceived & 6) == 2)
			{
				buffer[1] = 1;
			}
			else if ((dataReceived & 6) == 4)
			{
				buffer[1] = 2;
			}
			else if ((dataReceived & 6) == 6)
			{
				buffer[1] = 3;
			}
			if ((dataReceived & 0x40) == 0x40)
			{
				buffer[2] = 1;
			}
			else
			{
				buffer[2] = 0;
			}
			if ((dataReceived & 0x80) == 0x80)
			{
				buffer[3] = 1;
				return buffer;
			}
			buffer[3] = 0;
			return buffer;
		}

		public byte[] GetPulseStatus()
		{
			byte[] buffer = new byte[8];
			byte dataReceived = 0;
			ComObj.ReadData(0x22, ref dataReceived);
			if ((dataReceived & 0x10) == 0x10)
			{
				buffer[0] = 1;
			}
			else
			{
				buffer[0] = 0;
			}
			if ((dataReceived & 0x20) == 0x20)
			{
				buffer[1] = 1;
			}
			else
			{
				buffer[1] = 0;
			}
			if ((dataReceived & 0x40) == 0x40)
			{
				buffer[2] = 1;
			}
			else
			{
				buffer[2] = 0;
			}
			if ((dataReceived & 1) == 1)
			{
				buffer[3] = 1;
			}
			else
			{
				buffer[3] = 0;
			}
			if ((dataReceived & 2) == 2)
			{
				buffer[4] = 1;
			}
			else
			{
				buffer[4] = 0;
			}
			if ((dataReceived & 4) == 4)
			{
				buffer[5] = 1;
			}
			else
			{
				buffer[5] = 0;
			}
			if ((dataReceived & 0x80) == 0x80)
			{
				buffer[6] = 1;
			}
			else
			{
				buffer[6] = 0;
			}
			if ((dataReceived & 8) == 8)
			{
				buffer[7] = 1;
				return buffer;
			}
			buffer[7] = 0;
			return buffer;
		}

		public byte[] GetTrans1Status()
		{
			byte[] buffer = new byte[7];
			byte dataReceived = 0;
			ComObj.ReadData(0x1a, ref dataReceived);
			if ((dataReceived & 2) == 2)
			{
				buffer[0] = 1;
			}
			else
			{
				buffer[0] = 0;
			}
			if ((dataReceived & 1) == 1)
			{
				buffer[1] = 1;
			}
			else
			{
				buffer[1] = 0;
			}
			if ((dataReceived & 8) == 8)
			{
				buffer[2] = 1;
			}
			else
			{
				buffer[2] = 0;
			}
			if ((dataReceived & 4) == 4)
			{
				buffer[3] = 1;
			}
			else
			{
				buffer[3] = 0;
			}
			if ((dataReceived & 0x20) == 0x20)
			{
				buffer[4] = 1;
			}
			else
			{
				buffer[4] = 0;
			}
			if ((dataReceived & 0x10) == 0x10)
			{
				buffer[5] = 1;
			}
			else
			{
				buffer[5] = 0;
			}
			if ((dataReceived & 0x40) == 0x40)
			{
				buffer[6] = 1;
				return buffer;
			}
			buffer[6] = 0;
			return buffer;
		}

		public byte[] GetTransientStatus()
		{
			byte[] buffer = new byte[7];
			byte dataReceived = 0;
			ComObj.ReadData(30, ref dataReceived);
			if ((dataReceived & 2) == 2)
			{
				buffer[0] = 1;
			}
			else
			{
				buffer[0] = 0;
			}
			if ((dataReceived & 1) == 1)
			{
				buffer[1] = 1;
			}
			else
			{
				buffer[1] = 0;
			}
			if ((dataReceived & 8) == 8)
			{
				buffer[2] = 1;
			}
			else
			{
				buffer[2] = 0;
			}
			if ((dataReceived & 4) == 4)
			{
				buffer[3] = 1;
			}
			else
			{
				buffer[3] = 0;
			}
			if ((dataReceived & 0x20) == 0x20)
			{
				buffer[4] = 1;
			}
			else
			{
				buffer[4] = 0;
			}
			if ((dataReceived & 0x10) == 0x10)
			{
				buffer[5] = 1;
			}
			else
			{
				buffer[5] = 0;
			}
			if ((dataReceived & 0x40) == 0x40)
			{
				buffer[6] = 1;
				return buffer;
			}
			buffer[6] = 0;
			return buffer;
		}

		private void InterruptDetectionCallBack(object sender, byte[] packet)
		{
			if (((packet[2] & 1) == 1) && (packet.Length > 5))
			{
				DataOut8PacketObj.AxisCounts.XAxis = packet[3];
				DataOut8PacketObj.AxisCounts.YAxis = packet[4];
				DataOut8PacketObj.AxisCounts.ZAxis = packet[5];
				DataOut8StreamQueue.Enqueue(DataOut8PacketObj);
			}
			else if ((packet[2] & 4) == 4)
			{
				if (packet.Length >= 4)
				{
					FfMt1InterruptQueue.Enqueue(new int[] { packet[3], packet[1] });
				}
			}
			else
			{
				int num;
				if ((packet[2] & 8) == 8)
				{
					int[] numArray = new int[packet[0] - 2];
					if ((numArray.Length + 2) <= packet.Length)
					{
						for (num = 0; num < numArray.Length; num++)
						{
							numArray[num] = packet[num + 3];
						}
					}
					PulseInterruptQueue.Enqueue(numArray);
				}
				else if ((packet[2] & 0x10) == 0x10)
				{
					int[] pLData = new int[packet[0] - 2];
					if ((pLData.Length + 2) <= packet.Length)
					{
						for (num = 0; num < pLData.Length; num++)
						{
							pLData[num] = packet[num + 3];
						}
					}
					int[] numArray3 = EncodeOrientationPacket(pLData);
					OrientationInterruptQueue.Enqueue(numArray3);
				}
				else if ((packet[2] & 0x20) == 0x20)
				{
					int[] numArray4 = new int[packet[0] - 2];
					if ((numArray4.Length + 2) <= packet.Length)
					{
						for (num = 0; num < numArray4.Length; num++)
						{
							numArray4[num] = packet[num + 3];
						}
					}
					TransientInterruptQueue.Enqueue(numArray4);
				}
				else if ((packet[2] & 0x40) == 0x40)
				{
					FIFOInterruptQueue.Enqueue(packet);
				}
				else if ((packet[2] & 0x80) == 0x80)
				{
					SysmodInterruptQueue.Enqueue(packet);
				}
			}
		}

		public bool IsCommOpen()
		{
			return (BlockingCommObj.GetCommState() == CommunicationState.Ready);
		}

		public bool IsDataOut8StreamAvailable(ref DataOut8Packet dOut8Packet)
		{
			bool flag;
			if (SystemPollingOrInterrupt == 0)
			{
				RWLock.AcquireWriterLock(-1);
				try
				{
					PollStreamerCallBack(this, ReadXYZ8());
					if (DataOut8PacketObj.Status != 0)
					{
						dOut8Packet = DataOut8PacketObj;
						return true;
					}
				}
				finally
				{
					RWLock.ReleaseWriterLock();
				}
			}
			lock (DataOut8StreamQueue)
			{
				if (DataOut8StreamQueue.Count > 0)
				{
					dOut8Packet = (DataOut8Packet)DataOut8StreamQueue.Dequeue();
					flag = true;
				}
				else
				{
					flag = false;
				}
			}
			return flag;
		}

		public bool IsDataOutFIFO14StreamAvailable(ref DataOutFIFOPacket dOut12Packet)
		{
			if (SystemPollingOrInterrupt == 1)
			{
				RWLock.AcquireWriterLock(-1);
				try
				{
				}
				finally
				{
					RWLock.ReleaseWriterLock();
				}
			}
			lock (DataOutFIFOStreamQueue)
			{
				if (DataOutFIFOStreamQueue.Count > 0)
				{
					dOut12Packet = (DataOutFIFOPacket)DataOutFIFOStreamQueue.Dequeue();
					return true;
				}
				return false;
			}
		}

		public bool IsDataOutFIFO8StreamAvailable(ref DataOutFIFOPacket dOutFIFO8Packet)
		{
			bool flag;
			RWLock.AcquireWriterLock(-1);
			try
			{
				if (SystemPollingOrInterrupt == 1)
				{
				}
				lock (DataOutFIFOStreamQueue)
				{
					if (DataOutFIFOStreamQueue.Count > 0)
					{
						dOutFIFO8Packet = (DataOutFIFOPacket)DataOutFIFOStreamQueue.Dequeue();
						return true;
					}
					return false;
				}
			}
			finally
			{
				RWLock.ReleaseWriterLock();
			}
			return flag;
		}

		public bool IsDataOutFullStreamAvailable(ref DataOutFullPacket dOutFullPacket)
		{
			bool flag;
			if (SystemPollingOrInterrupt == 0)
			{
				RWLock.AcquireWriterLock(-1);
				try
				{
					PollStreamerCallBack(this, ReadXYZFull((int)DeviceID));
					if (DataOutFullPacketObj.Status != 0)
					{
						dOutFullPacket = DataOutFullPacketObj;
						return true;
					}
				}
				finally
				{
					RWLock.ReleaseWriterLock();
				}
			}
			lock (DataOutFullStreamQueue)
			{
				if (DataOutFullStreamQueue.Count > 0)
				{
					dOutFullPacket = (DataOutFullPacket)DataOutFullStreamQueue.Dequeue();
					flag = true;
				}
				else
				{
					flag = false;
				}
			}
			return flag;
		}

		public bool isFastReadOn()
		{
			return MMA845x.FREAD;
		}

		public bool IsOrientationInterruptAvailable(ref int[] updatePLStat)
		{
			if (OrientationInterruptQueue.Count > 0)
			{
				updatePLStat = (int[])OrientationInterruptQueue.Dequeue();
				return true;
			}
			return false;
		}

		public bool IsPulseInterruptAvailable(ref int[] updatePulseStat)
		{
			if (PulseInterruptQueue.Count > 0)
			{
				updatePulseStat = (int[])PulseInterruptQueue.Dequeue();
				return true;
			}
			return false;
		}

		public bool IsTransientInterruptAvailable(ref int[] updateTransStat)
		{
			if (TransientInterruptQueue.Count > 0)
			{
				updateTransStat = (int[])TransientInterruptQueue.Dequeue();
				return true;
			}
			return false;
		}

		public byte[] NVMBytesWritten()
		{
			byte[] written = new byte[0];
			ComObj.NVMBytesWritten(ref written);
			return written;
		}

		public void NVMConfig(int[] conf)
		{
			byte[] buffer = new byte[6];
			for (int i = 0; i < 6; i++)
			{
				buffer[i] = (byte)conf[i];
			}
			ComObj.NVMConfig(buffer);
		}

		public void NVMErase()
		{
			Result result = ComObj.NVMErase();
		}

		public byte[] NVMGetConstants()
		{
			byte[] constants = new byte[0];
			ComObj.NVMGetConstants(ref constants);
			return constants;
		}

		public bool NVMIsErased()
		{
			byte[] erased = new byte[1];
			ComObj.NVMIsErased(ref erased);
			if (erased[0] == 0x30)
			{
				return false;
			}
			return true;
		}

		public bool Open()
		{
			return true;
		}

		public void OrientationFlush()
		{
			for (int i = 0; i < OrientationInterruptQueue.Count; i++)
			{
				OrientationInterruptQueue.Dequeue();
			}
		}

		private void PollStreamerCallBack(object sender, byte[] packet)
		{
			int index = 0;
			if (CurrentStreamRequestMode == RequestModeType.FBMR)
			{
				try
				{
					index = 1;
					while (index < (packet.Length - 1))
					{
						int[] numArray4;
						int num2;
						int[] numArray5;
						switch (packet[index])
						{
							case 1:
								{
									DataOut8PacketObj.Status = packet[0];
									DataOut8PacketObj.AxisCounts.XAxis = packet[index + 1];
									DataOut8PacketObj.AxisCounts.YAxis = packet[index + 2];
									DataOut8PacketObj.AxisCounts.ZAxis = packet[index + 3];
									index += 5;
									continue;
								}
							case 2:
								{
									int[] numArray = new int[] { (packet[index + 1] << 6) + (packet[index + 2] >> 2), (packet[index + 3] << 6) + (packet[index + 4] >> 2), (packet[index + 5] << 6) + (packet[index + 6] >> 2) };
									DataOutFullPacketObj.Status = packet[0];
									DataOutFullPacketObj.AxisCounts.XAxis = (ushort)numArray[0];
									DataOutFullPacketObj.AxisCounts.YAxis = (ushort)numArray[1];
									DataOutFullPacketObj.AxisCounts.ZAxis = (ushort)numArray[2];
									index += 8;
									continue;
								}
							case 3:
								{
									int[] numArray2 = new int[] { (packet[index + 1] << 4) + (packet[index + 2] >> 4), (packet[index + 3] << 4) + (packet[index + 4] >> 4), (packet[index + 5] << 4) + (packet[index + 6] >> 4) };
									DataOutFullPacketObj.Status = packet[0];
									DataOutFullPacketObj.AxisCounts.XAxis = (ushort)numArray2[0];
									DataOutFullPacketObj.AxisCounts.YAxis = (ushort)numArray2[1];
									DataOutFullPacketObj.AxisCounts.ZAxis = (ushort)numArray2[2];
									index += 8;
									continue;
								}
							case 4:
								{
									int[] numArray3 = new int[] { (packet[index + 1] << 2) + (packet[index + 2] >> 6), (packet[index + 3] << 2) + (packet[index + 4] >> 6), (packet[index + 5] << 2) + (packet[index + 6] >> 6) };
									DataOutFullPacketObj.Status = packet[0];
									DataOutFullPacketObj.AxisCounts.XAxis = (ushort)numArray3[0];
									DataOutFullPacketObj.AxisCounts.YAxis = (ushort)numArray3[1];
									DataOutFullPacketObj.AxisCounts.ZAxis = (ushort)numArray3[2];
									index += 8;
									continue;
								}
							case 8:
								numArray4 = new int[packet[0]];
								if ((numArray4.Length + 2) > packet.Length)
								{
									goto Label_0299;
								}
								num2 = 0;
								goto Label_028A;

							default:
								goto Label_02B7;
						}
					Label_0279:
						numArray4[num2] = packet[num2 + 2];
						num2++;
					Label_028A:
						if (num2 < numArray4.Length)
						{
							goto Label_0279;
						}
					Label_0299:
						numArray5 = EncodeOrientationPacket(numArray4);
						OrientationInterruptQueue.Enqueue(numArray5);
						index += 5;
						continue;
					Label_02B7:
						index = packet.Length;
					}
				}
				finally
				{
				}
			}
		}

		public void PulseFlush()
		{
			for (int i = 0; i < PulseInterruptQueue.Count; i++)
			{
				PulseInterruptQueue.Dequeue();
			}
		}

		public byte ReadAppRegister(int[] passedData)
		{
			int internalAddress = passedData[0];
			byte[] dataReceived = new byte[1];
			ComObj.AppRegisterRead(1, internalAddress, ref dataReceived);
			return dataReceived[0];
		}

		public ushort[] ReadFIFO14()
		{
			Result result = Result.eERR_INITIAL_STATE;
			byte[] dataReceived = new byte[0xc1];
			ushort[] numArray = new ushort[0xc0];
			result = ComObj.ReadData(0xc1, 0, ref dataReceived);
			for (int i = 0; i < 0x20; i++)
			{
				numArray[i * 3] = (ushort)((dataReceived[1 + (i * 6)] << 6) + (dataReceived[2 + (i * 6)] >> 2));
				numArray[1 + (i * 3)] = (ushort)((dataReceived[3 + (i * 6)] << 6) + (dataReceived[4 + (i * 6)] >> 2));
				numArray[2 + (i * 3)] = (ushort)((dataReceived[5 + (i * 6)] << 6) + (dataReceived[6 + (i * 6)] >> 2));
			}
			return numArray;
		}

		public ushort[] ReadFIFO8()
		{
			Result result = Result.eERR_INITIAL_STATE;
			byte[] dataReceived = new byte[0x61];
			ushort[] numArray = new ushort[0x60];
			result = ComObj.ReadData(0x61, 0, ref dataReceived);
			for (int i = 0; i < 0x20; i++)
			{
				numArray[i * 3] = dataReceived[1 + (i * 3)];
				numArray[1 + (i * 3)] = dataReceived[2 + (i * 3)];
				numArray[2 + (i * 3)] = dataReceived[3 + (i * 3)];
			}
			return numArray;
		}

		public int[] ReadParsedValue(int[] val)
		{
			int internalAddress = val[0];
			int[] numArray = new int[10];
			byte dataReceived = 0;
			ComObj.ReadData(internalAddress, ref dataReceived);
			numArray[0] = dataReceived;
			numArray[1] = ((dataReceived & 1) == 1) ? 1 : 0;
			numArray[2] = ((dataReceived & 2) == 2) ? 1 : 0;
			numArray[3] = ((dataReceived & 4) == 4) ? 1 : 0;
			numArray[4] = ((dataReceived & 8) == 8) ? 1 : 0;
			numArray[5] = ((dataReceived & 0x10) == 0x10) ? 1 : 0;
			numArray[6] = ((dataReceived & 0x20) == 0x20) ? 1 : 0;
			numArray[7] = ((dataReceived & 0x40) == 0x40) ? 1 : 0;
			numArray[8] = ((dataReceived & 0x80) == 0x80) ? 1 : 0;
			numArray[9] = internalAddress;
			return numArray;
		}

		public byte ReadValue(int[] passedData)
		{
			int internalAddress = passedData[0];
			byte dataReceived = 0;
			ComObj.ReadData(internalAddress, ref dataReceived);
			return dataReceived;
		}

		public byte[] ReadXYZ8()
		{
			byte[] buffer = new byte[7];
			byte[] dataReceived = null;
			if (ComObj.ReadData(4, 0, ref dataReceived) == Result.eERR_SUCCESS)
			{
				buffer[0] = dataReceived[0];
				buffer[1] = 1;
				buffer[2] = dataReceived[1];
				buffer[3] = dataReceived[2];
				buffer[4] = dataReceived[3];
			}
			return buffer;
		}

		public byte[] ReadXYZFull(int DeviceID)
		{
			byte[] buffer = new byte[8];
			byte[] dataReceived = null;
			if (ComObj.ReadData(7, 0, ref dataReceived) == Result.eERR_SUCCESS)
			{
				buffer[0] = dataReceived[0];
				if (MMA845x.DeviceID == deviceID.MMA8451Q)
				{
					MMA845x.DsFBID = FBID.DataOut14;
					buffer[1] = 2;
				}
				else if (MMA845x.DeviceID == deviceID.MMA8452Q)
				{
					MMA845x.DsFBID = FBID.DataOut12;
					buffer[1] = 3;
				}
				else if (MMA845x.DeviceID == deviceID.MMA8453Q)
				{
					MMA845x.DsFBID = FBID.DataOut10;
					buffer[1] = 4;
				}
				buffer[2] = dataReceived[1];
				buffer[3] = dataReceived[2];
				buffer[4] = dataReceived[3];
				buffer[5] = dataReceived[4];
				buffer[6] = dataReceived[5];
				buffer[7] = dataReceived[6];
			}
			return buffer;
		}

		public void ResetDevice()
		{
			Result result = Result.eERR_INITIAL_STATE;
			int num = 0;
			while ((result != Result.eERR_SUCCESS) && (num < 10))
			{
				result = ComObj.AppRegisterWrite(1, 0x16, new byte[] { 0xa5 });
				num++;
			}
			if (result != Result.eERR_SUCCESS)
			{
				STBLogger.AddEvent(this, STBLogger.EventLevel.Error, "Could not reset the Device", string.Format("Class: VeyronDriver; Last Result: {0}; Tries: {1}", result.ToString(), num));
			}
		}

		private void Serial_SerialEvents(object sender, ref string buff)
		{
			buff = "";
		}

		public void SetActive(int[] passData)
		{
			int num = passData[0];
			byte dataReceived = 0;
			if (ComObj.ReadData(0x2a, ref dataReceived) == Result.eERR_SUCCESS)
			{
				byte num3 = 1;
				dataReceived = (byte)(dataReceived & ~num3);
				ComObj.WriteData(0x2a, dataReceived = (byte)(dataReceived | ((byte)num)));
			}
		}

		public void SetAutoSleepEnable(int[] passedData)
		{
			byte num = (byte)passedData[0];
			byte dataReceived = 0;
			if (ComObj.ReadData(0x2b, ref dataReceived) == Result.eERR_SUCCESS)
			{
				byte num3 = 4;
				dataReceived = (byte)(dataReceived & ~num3);
				dataReceived = (byte)(dataReceived | ((byte)(num & num3)));
				ComObj.WriteData(0x2b, dataReceived);
			}
		}

		public void SetBFTripA(int[] passedData)
		{
			int num = passedData[0];
			byte dataReceived = 0;
			if (ComObj.ReadData(0x13, ref dataReceived) == Result.eERR_SUCCESS)
			{
				byte num3 = 0xc0;
				dataReceived = (byte)(dataReceived & ~num3);
				dataReceived = (byte)(dataReceived | ((byte)(num * 0x40)));
				ComObj.WriteData(0x13, dataReceived);
			}
		}

		public void SetCalValues(int[] passedData)
		{
			byte data = (byte)passedData[0];
			byte num2 = (byte)passedData[1];
			byte num3 = (byte)passedData[2];
			ComObj.WriteData(0x2f, data);
			ComObj.WriteData(0x30, num2);
			ComObj.WriteData(0x31, num3);
		}

		public void SetDataRate(int[] passedData)
		{
			int num = passedData[0];
			byte dataReceived = 0;
			if (ComObj.ReadData(0x2a, ref dataReceived) == Result.eERR_SUCCESS)
			{
				byte num3 = 0x38;
				dataReceived = (byte)(dataReceived & ~num3);
				dataReceived = (byte)(dataReceived | ((byte)(num * 8)));
				ComObj.WriteData(0x2a, dataReceived);
			}
		}

		public void SetDBCNTM_PL(int[] passedData)
		{
			int num = passedData[0];
			byte dataReceived = 0;
			if (ComObj.ReadData(0x11, ref dataReceived) == Result.eERR_SUCCESS)
			{
				byte num3 = 0x80;
				dataReceived = (byte)(dataReceived & ~num3);
				dataReceived = (byte)(dataReceived | ((byte)(num & num3)));
				ComObj.WriteData(0x11, dataReceived);
			}
		}

		public void SetEnablePL(int[] passedData)
		{
			int num = passedData[0];
			byte dataReceived = 0;
			if (ComObj.ReadData(0x11, ref dataReceived) == Result.eERR_SUCCESS)
			{
				byte num3 = 0x40;
				dataReceived = (byte)(dataReceived & ~num3);
				dataReceived = (byte)(dataReceived | ((byte)(num & num3)));
			}
			ComObj.WriteData(0x11, dataReceived);
		}

		public void SetFIFOMode(int[] passedData)
		{
			int num = passedData[0];
			byte dataReceived = 0;
			if (ComObj.ReadData(9, ref dataReceived) == Result.eERR_SUCCESS)
			{
				byte num3 = 0xc0;
				dataReceived = (byte)(dataReceived & ~num3);
				dataReceived = (byte)(dataReceived | ((byte)(num * 0x40)));
				ComObj.WriteData(9, dataReceived);
			}
		}

		public void SetFIFOWatermark(int[] passData)
		{
			int num = passData[0];
			byte dataReceived = 0;
			if (ComObj.ReadData(9, ref dataReceived) == Result.eERR_SUCCESS)
			{
				byte num3 = 0x3f;
				dataReceived = (byte)(dataReceived & ~num3);
				dataReceived = (byte)(dataReceived | ((byte)num));
				ComObj.WriteData(9, dataReceived);
			}
		}

		public void SetFREAD(int[] passData)
		{
			int num = passData[0];
			byte dataReceived = 0;
			if (ComObj.ReadData(0x2a, ref dataReceived) == Result.eERR_SUCCESS)
			{
				byte num3 = 2;
				dataReceived = (byte)(dataReceived & ~num3);
				dataReceived = (byte)(dataReceived | ((byte)(num & num3)));
				if (ComObj.WriteData(0x2a, dataReceived) == Result.eERR_SUCCESS)
				{
					MMA845x.FREAD = num == 0xff;
				}
			}
		}

		public void SetFS(int[] passData)
		{
			int num = passData[0];
			byte dataReceived = 0;
			if (ComObj.ReadData(14, ref dataReceived) == Result.eERR_SUCCESS)
			{
				byte num3 = 3;
				dataReceived = (byte)(dataReceived & ~num3);
				dataReceived = (byte)(dataReceived | ((byte)(((byte)num) & num3)));
				ComObj.WriteData(14, dataReceived);
			}
		}

		public void SetHPFDataOut(int[] passData)
		{
			int num = passData[0];
			byte dataReceived = 0;
			if (ComObj.ReadData(14, ref dataReceived) == Result.eERR_SUCCESS)
			{
				byte num3 = 0x10;
				dataReceived = (byte)(dataReceived & ~num3);
				dataReceived = (byte)(dataReceived | ((byte)(num & num3)));
				ComObj.WriteData(14, dataReceived);
			}
		}

		public void SetHPFilter(int[] passedData)
		{
			int num = passedData[0];
			byte dataReceived = 0;
			if (ComObj.ReadData(15, ref dataReceived) == Result.eERR_SUCCESS)
			{
				byte num3 = 3;
				dataReceived = (byte)(dataReceived & ~num3);
				dataReceived = (byte)(dataReceived | ((byte)num));
				ComObj.WriteData(15, dataReceived);
			}
		}

		public void SetHysteresis(int[] passData)
		{
			byte num = (byte)passData[0];
			byte dataReceived = 0;
			if (ComObj.ReadData(20, ref dataReceived) == Result.eERR_SUCCESS)
			{
				byte num3 = 7;
				dataReceived = (byte)(dataReceived & ~num3);
				dataReceived = (byte)(dataReceived | num);
				ComObj.WriteData(20, dataReceived);
			}
		}

		public void SetIntConfig(int[] passedData)
		{
			int num = passedData[0];
			int num2 = passedData[1];
			byte dataReceived = 0;
			if (ComObj.ReadData(0x2e, ref dataReceived) == Result.eERR_SUCCESS)
			{
				dataReceived = (byte)(dataReceived & ((byte)~num2));
				dataReceived = (byte)(dataReceived | ((byte)(num & num2)));
				ComObj.WriteData(0x2e, dataReceived);
			}
		}

		public void SetINTPolarity(int[] passedData)
		{
			int num = passedData[0];
			byte dataReceived = 0;
			if (ComObj.ReadData(0x2c, ref dataReceived) == Result.eERR_SUCCESS)
			{
				byte num3 = 2;
				dataReceived = (byte)(dataReceived & ~num3);
				dataReceived = (byte)(dataReceived | ((byte)(num & num3)));
				ComObj.WriteData(0x2c, dataReceived);
				ComObj.AppRegisterWrite(1, 0x15, new byte[] { 3 });
			}
		}

		public void SetINTPPOD(int[] passedData)
		{
			int num = passedData[0];
			byte dataReceived = 0;
			if (ComObj.ReadData(0x2c, ref dataReceived) == Result.eERR_SUCCESS)
			{
				byte num3 = 1;
				dataReceived = (byte)(dataReceived & ~num3);
				dataReceived = (byte)(dataReceived | ((byte)(num & num3)));
				ComObj.WriteData(0x2c, dataReceived);
			}
		}

		public void SetIntsEnable(int[] passedData)
		{
			int num = passedData[0];
			int num2 = passedData[1];
			byte dataReceived = 0;
			if (ComObj.ReadData(0x2d, ref dataReceived) == Result.eERR_SUCCESS)
			{
				dataReceived = (byte)(dataReceived & ((byte)~num2));
				dataReceived = (byte)(dataReceived | ((byte)(num & num2)));
				ComObj.WriteData(0x2d, dataReceived);
			}
		}

		public void SetIntsEnableBit(int[] passedData)
		{
			int num = passedData[0];
			int num2 = passedData[1];
			byte dataReceived = 0;
			if (ComObj.ReadData(0x2d, ref dataReceived) == Result.eERR_SUCCESS)
			{
				dataReceived = (byte)(dataReceived & ((byte)~num2));
				dataReceived = (byte)(dataReceived | ((byte)(num & num2)));
				ComObj.WriteData(0x2d, dataReceived);
				ComObj.WriteData(0x2e, dataReceived);
				ComObj.AppRegisterWrite(1, 0x15, new byte[] { 2 });
			}
		}

		public void SetLowNoise(int[] passData)
		{
			int num = passData[0];
			byte dataReceived = 0;
			if (ComObj.ReadData(0x2a, ref dataReceived) == Result.eERR_SUCCESS)
			{
				byte num3 = 4;
				dataReceived = (byte)(dataReceived & ~num3);
				dataReceived = (byte)(dataReceived | ((byte)(num & num3)));
				ComObj.WriteData(0x2a, dataReceived);
			}
		}

		public void SetMFF1AndOr(int[] passedData)
		{
			int num = passedData[0];
			byte dataReceived = 0;
			if (ComObj.ReadData(0x15, ref dataReceived) == Result.eERR_SUCCESS)
			{
				byte num3 = 0x40;
				dataReceived = (byte)(dataReceived & ~num3);
				dataReceived = (byte)(dataReceived | ((byte)(num & num3)));
				ComObj.WriteData(0x15, dataReceived);
			}
		}

		public void SetMFF1DBCNTM(int[] passedData)
		{
			int num = passedData[0];
			byte dataReceived = 0;
			if (ComObj.ReadData(0x17, ref dataReceived) == Result.eERR_SUCCESS)
			{
				byte num3 = 0x80;
				dataReceived = (byte)(dataReceived & ~num3);
				dataReceived = (byte)(dataReceived | ((byte)(num & num3)));
				ComObj.WriteData(0x17, dataReceived);
			}
		}

		public void SetMFF1Debounce(int[] passedData)
		{
			int num = passedData[0];
			byte dataReceived = 0;
			if (ComObj.ReadData(0x18, ref dataReceived) == Result.eERR_SUCCESS)
			{
				dataReceived = (byte)num;
				ComObj.WriteData(0x18, dataReceived);
			}
		}

		public void SetMFF1Latch(int[] passedData)
		{
			int num = passedData[0];
			byte dataReceived = 0;
			if (ComObj.ReadData(0x15, ref dataReceived) == Result.eERR_SUCCESS)
			{
				byte num3 = 0x80;
				dataReceived = (byte)(dataReceived & ~num3);
				dataReceived = (byte)(dataReceived | ((byte)(num & num3)));
				ComObj.WriteData(0x15, dataReceived);
			}
		}

		public void SetMFF1Threshold(int[] passedData)
		{
			int num = passedData[0];
			byte dataReceived = 0;
			if (ComObj.ReadData(0x17, ref dataReceived) == Result.eERR_SUCCESS)
			{
				byte num3 = 0x7f;
				dataReceived = (byte)(dataReceived & ~num3);
				dataReceived = (byte)(dataReceived | ((byte)num));
				ComObj.WriteData(0x17, dataReceived);
			}
		}

		public void SetMFF1XEFE(int[] passedData)
		{
			int num = passedData[0];
			byte dataReceived = 0;
			if (ComObj.ReadData(0x15, ref dataReceived) == Result.eERR_SUCCESS)
			{
				byte num3 = 8;
				dataReceived = (byte)(dataReceived & ~num3);
				dataReceived = (byte)(dataReceived | ((byte)(num & num3)));
				ComObj.WriteData(0x15, dataReceived);
			}
		}

		public void SetMFF1YEFE(int[] passedData)
		{
			int num = passedData[0];
			byte dataReceived = 0;
			if (ComObj.ReadData(0x15, ref dataReceived) == Result.eERR_SUCCESS)
			{
				byte num3 = 0x10;
				dataReceived = (byte)(dataReceived & ~num3);
				dataReceived = (byte)(dataReceived | ((byte)(num & num3)));
				ComObj.WriteData(0x15, dataReceived);
			}
		}

		public void SetMFF1ZEFE(int[] passedData)
		{
			int num = passedData[0];
			byte dataReceived = 0;
			if (ComObj.ReadData(0x15, ref dataReceived) == Result.eERR_SUCCESS)
			{
				byte num3 = 0x20;
				dataReceived = (byte)(dataReceived & ~num3);
				dataReceived = (byte)(dataReceived | ((byte)(num & num3)));
				ComObj.WriteData(0x15, dataReceived);
			}
		}

		public void SetPLDebounce(int[] passedData)
		{
			byte num = (byte)passedData[0];
			byte dataReceived = 0;
			if (ComObj.ReadData(0x12, ref dataReceived) == Result.eERR_SUCCESS)
			{
				dataReceived = num;
			}
			ComObj.WriteData(0x12, dataReceived);
		}

		public void SetPLTripA(int[] passedData)
		{
			byte num = (byte)passedData[0];
			byte dataReceived = 0;
			if (ComObj.ReadData(20, ref dataReceived) == Result.eERR_SUCCESS)
			{
				byte num3 = 0xf8;
				dataReceived = (byte)(dataReceived & ~num3);
				dataReceived = (byte)(dataReceived | ((byte)(num * 8)));
				ComObj.WriteData(20, dataReceived);
			}
		}

		public void SetPulse2ndPulseWin(int[] passedData)
		{
			int num = passedData[0];
			byte dataReceived = 0;
			if (ComObj.ReadData(40, ref dataReceived) == Result.eERR_SUCCESS)
			{
				dataReceived = (byte)num;
			}
			ComObj.WriteData(40, dataReceived);
		}

		public void SetPulseDPA(int[] passedData)
		{
			int num = passedData[0];
			byte dataReceived = 0;
			if (ComObj.ReadData(0x21, ref dataReceived) == Result.eERR_SUCCESS)
			{
				byte num3 = 0x80;
				dataReceived = (byte)(dataReceived & ~num3);
				dataReceived = (byte)(dataReceived | ((byte)(num & num3)));
				ComObj.WriteData(0x21, dataReceived);
			}
		}

		public void SetPulseFirstTimeLimit(int[] passedData)
		{
			int num = passedData[0];
			byte dataReceived = 0;
			if (ComObj.ReadData(0x26, ref dataReceived) == Result.eERR_SUCCESS)
			{
				dataReceived = (byte)num;
			}
			ComObj.WriteData(0x26, dataReceived);
		}

		public void SetPulseHPFBypass(int[] passData)
		{
			int num = passData[0];
			byte dataReceived = 0;
			if (ComObj.ReadData(15, ref dataReceived) == Result.eERR_SUCCESS)
			{
				byte num3 = 0x20;
				dataReceived = (byte)(dataReceived & ~num3);
				dataReceived = (byte)(dataReceived | ((byte)(num & num3)));
				ComObj.WriteData(15, dataReceived);
			}
		}

		public void SetPulseLatch(int[] passedData)
		{
			int num = passedData[0];
			byte dataReceived = 0;
			if (ComObj.ReadData(0x21, ref dataReceived) == Result.eERR_SUCCESS)
			{
				byte num3 = 0x40;
				dataReceived = (byte)(dataReceived & ~num3);
				dataReceived = (byte)(dataReceived | ((byte)(num & num3)));
				ComObj.WriteData(0x21, dataReceived);
			}
		}

		public void SetPulseLatency(int[] passedData)
		{
			int num = passedData[0];
			byte dataReceived = 0;
			if (ComObj.ReadData(0x27, ref dataReceived) == Result.eERR_SUCCESS)
			{
				dataReceived = (byte)num;
			}
			ComObj.WriteData(0x27, dataReceived);
		}

		public void SetPulseLPFEnable(int[] passData)
		{
			int num = passData[0];
			byte dataReceived = 0;
			if (ComObj.ReadData(15, ref dataReceived) == Result.eERR_SUCCESS)
			{
				byte num3 = 0x10;
				dataReceived = (byte)(dataReceived & ~num3);
				dataReceived = (byte)(dataReceived | ((byte)(num & num3)));
				ComObj.WriteData(15, dataReceived);
			}
		}

		public void SetPulseXDP(int[] passedData)
		{
			int num = passedData[0];
			byte dataReceived = 0;
			if (ComObj.ReadData(0x21, ref dataReceived) == Result.eERR_SUCCESS)
			{
				byte num3 = 2;
				dataReceived = (byte)(dataReceived & ~num3);
				dataReceived = (byte)(dataReceived | ((byte)(num & num3)));
				ComObj.WriteData(0x21, dataReceived);
			}
		}

		public void SetPulseXSP(int[] passedData)
		{
			int num = passedData[0];
			byte dataReceived = 0;
			if (ComObj.ReadData(0x21, ref dataReceived) == Result.eERR_SUCCESS)
			{
				byte num3 = 1;
				dataReceived = (byte)(dataReceived & ~num3);
				dataReceived = (byte)(dataReceived | ((byte)(num & num3)));
				ComObj.WriteData(0x21, dataReceived);
			}
		}

		public void SetPulseXThreshold(int[] passedData)
		{
			int num = passedData[0];
			byte dataReceived = 0;
			if (ComObj.ReadData(0x23, ref dataReceived) == Result.eERR_SUCCESS)
			{
				dataReceived = (byte)num;
			}
			ComObj.WriteData(0x23, dataReceived);
		}

		public void SetPulseYDP(int[] passedData)
		{
			int num = passedData[0];
			byte dataReceived = 0;
			if (ComObj.ReadData(0x21, ref dataReceived) == Result.eERR_SUCCESS)
			{
				byte num3 = 8;
				dataReceived = (byte)(dataReceived & ~num3);
				dataReceived = (byte)(dataReceived | ((byte)(num & num3)));
				ComObj.WriteData(0x21, dataReceived);
			}
		}

		public void SetPulseYSP(int[] passedData)
		{
			int num = passedData[0];
			byte dataReceived = 0;
			if (ComObj.ReadData(0x21, ref dataReceived) == Result.eERR_SUCCESS)
			{
				byte num3 = 4;
				dataReceived = (byte)(dataReceived & ~num3);
				dataReceived = (byte)(dataReceived | ((byte)(num & num3)));
				ComObj.WriteData(0x21, dataReceived);
			}
		}

		public void SetPulseYThreshold(int[] passedData)
		{
			int num = passedData[0];
			byte dataReceived = 0;
			if (ComObj.ReadData(0x24, ref dataReceived) == Result.eERR_SUCCESS)
			{
				dataReceived = (byte)num;
			}
			ComObj.WriteData(0x24, dataReceived);
		}

		public void SetPulseZDP(int[] passedData)
		{
			int num = passedData[0];
			byte dataReceived = 0;
			if (ComObj.ReadData(0x21, ref dataReceived) == Result.eERR_SUCCESS)
			{
				byte num3 = 0x20;
				dataReceived = (byte)(dataReceived & ~num3);
				dataReceived = (byte)(dataReceived | ((byte)(num & num3)));
				ComObj.WriteData(0x21, dataReceived);
			}
		}

		public void SetPulseZSP(int[] passedData)
		{
			int num = passedData[0];
			byte dataReceived = 0;
			if (ComObj.ReadData(0x21, ref dataReceived) == Result.eERR_SUCCESS)
			{
				byte num3 = 0x10;
				dataReceived = (byte)(dataReceived & ~num3);
				dataReceived = (byte)(dataReceived | ((byte)(num & num3)));
				ComObj.WriteData(0x21, dataReceived);
			}
		}

		public void SetPulseZThreshold(int[] passedData)
		{
			int num = passedData[0];
			byte dataReceived = 0;
			if (ComObj.ReadData(0x25, ref dataReceived) == Result.eERR_SUCCESS)
			{
				dataReceived = (byte)num;
			}
			ComObj.WriteData(0x25, dataReceived);
		}

		public void SetSelfTest(int[] passedData)
		{
			int num = passedData[0];
			byte dataReceived = 0;
			if (ComObj.ReadData(0x2b, ref dataReceived) == Result.eERR_SUCCESS)
			{
				byte num3 = 0x80;
				dataReceived = (byte)(dataReceived & ~num3);
				dataReceived = (byte)(dataReceived | ((byte)(num & num3)));
				ComObj.WriteData(0x2b, dataReceived);
			}
		}

		public void SetSleepOSMode(int[] passData)
		{
			int num = passData[0];
			byte dataReceived = 0;
			if (ComObj.ReadData(0x2b, ref dataReceived) == Result.eERR_SUCCESS)
			{
				byte num3 = 0x18;
				dataReceived = (byte)(dataReceived & ~num3);
				dataReceived = (byte)(dataReceived | ((byte)(num * 8)));
				ComObj.WriteData(0x2b, dataReceived);
			}
		}

		public void SetSleepSampleRate(int[] passedData)
		{
			int num = passedData[0];
			byte dataReceived = 0;
			if (ComObj.ReadData(0x2a, ref dataReceived) == Result.eERR_SUCCESS)
			{
				byte num3 = 0xc0;
				dataReceived = (byte)(dataReceived & ~num3);
				dataReceived = (byte)(dataReceived | ((byte)(num * 0x40)));
				ComObj.WriteData(0x2a, dataReceived);
			}
		}

		public void SetSleepTimer(int[] passedData)
		{
			byte num = (byte)passedData[0];
			byte dataReceived = 0;
			if (ComObj.ReadData(0x29, ref dataReceived) == Result.eERR_SUCCESS)
			{
				dataReceived = num;
				ComObj.WriteData(0x29, dataReceived);
			}
		}

		public void SetTransBypassHPF(int[] passedData)
		{
			int num = passedData[0];
			byte dataReceived = 0;
			if (ComObj.ReadData(0x1d, ref dataReceived) == Result.eERR_SUCCESS)
			{
				byte num3 = 1;
				dataReceived = (byte)(dataReceived & ~num3);
				dataReceived = (byte)(dataReceived | ((byte)(num & num3)));
				ComObj.WriteData(0x1d, dataReceived);
			}
		}

		public void SetTransBypassHPFNEW(int[] passedData)
		{
			int num = passedData[0];
			byte dataReceived = 0;
			if (ComObj.ReadData(0x19, ref dataReceived) == Result.eERR_SUCCESS)
			{
				byte num3 = 1;
				dataReceived = (byte)(dataReceived & ~num3);
				dataReceived = (byte)(dataReceived | ((byte)(num & num3)));
				ComObj.WriteData(0x19, dataReceived);
			}
		}

		public void SetTransDBCNTM(int[] passedData)
		{
			int num = passedData[0];
			byte dataReceived = 0;
			if (ComObj.ReadData(0x1f, ref dataReceived) == Result.eERR_SUCCESS)
			{
				byte num3 = 0x80;
				dataReceived = (byte)(dataReceived & ~num3);
				dataReceived = (byte)(dataReceived | ((byte)(num & num3)));
				ComObj.WriteData(0x1f, dataReceived);
			}
		}

		public void SetTransDBCNTMNEW(int[] passedData)
		{
			int num = passedData[0];
			byte dataReceived = 0;
			if (ComObj.ReadData(0x1b, ref dataReceived) == Result.eERR_SUCCESS)
			{
				byte num3 = 0x80;
				dataReceived = (byte)(dataReceived & ~num3);
				dataReceived = (byte)(dataReceived | ((byte)(num & num3)));
				ComObj.WriteData(0x1b, dataReceived);
			}
		}

		public void SetTransDebounce(int[] passedData)
		{
			int num = passedData[0];
			byte dataReceived = 0;
			if (ComObj.ReadData(0x20, ref dataReceived) == Result.eERR_SUCCESS)
			{
				dataReceived = (byte)num;
			}
			ComObj.WriteData(0x20, dataReceived);
		}

		public void SetTransDebounceNEW(int[] passedData)
		{
			int num = passedData[0];
			byte dataReceived = 0;
			if (ComObj.ReadData(0x1c, ref dataReceived) == Result.eERR_SUCCESS)
			{
				dataReceived = (byte)num;
			}
			ComObj.WriteData(0x1c, dataReceived);
		}

		public void SetTransEnableLatch(int[] passedData)
		{
			int num = passedData[0];
			byte dataReceived = 0;
			if (ComObj.ReadData(0x1d, ref dataReceived) == Result.eERR_SUCCESS)
			{
				byte num3 = 0x10;
				dataReceived = (byte)(dataReceived & ~num3);
				dataReceived = (byte)(dataReceived | ((byte)(num & num3)));
				ComObj.WriteData(0x1d, dataReceived);
			}
		}

		public void SetTransEnableLatchNEW(int[] passedData)
		{
			int num = passedData[0];
			byte dataReceived = 0;
			if (ComObj.ReadData(0x19, ref dataReceived) == Result.eERR_SUCCESS)
			{
				byte num3 = 0x10;
				dataReceived = (byte)(dataReceived & ~num3);
				dataReceived = (byte)(dataReceived | ((byte)(num & num3)));
				ComObj.WriteData(0x19, dataReceived);
			}
		}

		public void SetTransEnableXFlag(int[] passedData)
		{
			int num = passedData[0];
			byte dataReceived = 0;
			if (ComObj.ReadData(0x1d, ref dataReceived) == Result.eERR_SUCCESS)
			{
				byte num3 = 2;
				dataReceived = (byte)(dataReceived & ~num3);
				dataReceived = (byte)(dataReceived | ((byte)(num & num3)));
				ComObj.WriteData(0x1d, dataReceived);
			}
		}

		public void SetTransEnableXFlagNEW(int[] passedData)
		{
			int num = passedData[0];
			byte dataReceived = 0;
			if (ComObj.ReadData(0x19, ref dataReceived) == Result.eERR_SUCCESS)
			{
				byte num3 = 2;
				dataReceived = (byte)(dataReceived & ~num3);
				dataReceived = (byte)(dataReceived | ((byte)(num & num3)));
				ComObj.WriteData(0x19, dataReceived);
			}
		}

		public void SetTransEnableYFlag(int[] passedData)
		{
			int num = passedData[0];
			byte dataReceived = 0;
			if (ComObj.ReadData(0x1d, ref dataReceived) == Result.eERR_SUCCESS)
			{
				byte num3 = 4;
				dataReceived = (byte)(dataReceived & ~num3);
				dataReceived = (byte)(dataReceived | ((byte)(num & num3)));
				ComObj.WriteData(0x1d, dataReceived);
			}
		}

		public void SetTransEnableYFlagNEW(int[] passedData)
		{
			int num = passedData[0];
			byte dataReceived = 0;
			if (ComObj.ReadData(0x19, ref dataReceived) == Result.eERR_SUCCESS)
			{
				byte num3 = 4;
				dataReceived = (byte)(dataReceived & ~num3);
				dataReceived = (byte)(dataReceived | ((byte)(num & num3)));
				ComObj.WriteData(0x19, dataReceived);
			}
		}

		public void SetTransEnableZFlag(int[] passedData)
		{
			int num = passedData[0];
			byte dataReceived = 0;
			if (ComObj.ReadData(0x1d, ref dataReceived) == Result.eERR_SUCCESS)
			{
				byte num3 = 8;
				dataReceived = (byte)(dataReceived & ~num3);
				dataReceived = (byte)(dataReceived | ((byte)(num & num3)));
				ComObj.WriteData(0x1d, dataReceived);
			}
		}

		public void SetTransEnableZFlagNEW(int[] passedData)
		{
			int num = passedData[0];
			byte dataReceived = 0;
			if (ComObj.ReadData(0x19, ref dataReceived) == Result.eERR_SUCCESS)
			{
				byte num3 = 8;
				dataReceived = (byte)(dataReceived & ~num3);
				dataReceived = (byte)(dataReceived | ((byte)(num & num3)));
				ComObj.WriteData(0x19, dataReceived);
			}
		}

		public void SetTransThreshold(int[] passedData)
		{
			int num = passedData[0];
			byte dataReceived = 0;
			if (ComObj.ReadData(0x1f, ref dataReceived) == Result.eERR_SUCCESS)
			{
				byte num3 = 0x7f;
				dataReceived = (byte)(dataReceived & ~num3);
				dataReceived = (byte)(dataReceived | ((byte)num));
				ComObj.WriteData(0x1f, dataReceived);
			}
		}

		public void SetTransThresholdNEW(int[] passedData)
		{
			int num = passedData[0];
			byte dataReceived = 0;
			if (ComObj.ReadData(0x1b, ref dataReceived) == Result.eERR_SUCCESS)
			{
				byte num3 = 0x7f;
				dataReceived = (byte)(dataReceived & ~num3);
				dataReceived = (byte)(dataReceived | ((byte)num));
				ComObj.WriteData(0x1b, dataReceived);
			}
		}

		public void SetTrigLP(int[] passData)
		{
			int num = passData[0];
			byte dataReceived = 0;
			if (ComObj.ReadData(10, ref dataReceived) == Result.eERR_SUCCESS)
			{
				byte num3 = 0x10;
				dataReceived = (byte)(dataReceived & ~num3);
				dataReceived = (byte)(dataReceived | ((byte)(num & num3)));
				ComObj.WriteData(10, dataReceived);
			}
		}

		public void SetTrigMFF(int[] passData)
		{
			int num = passData[0];
			byte dataReceived = 0;
			if (ComObj.ReadData(10, ref dataReceived) == Result.eERR_SUCCESS)
			{
				byte num3 = 4;
				dataReceived = (byte)(dataReceived & ~num3);
				dataReceived = (byte)(dataReceived | ((byte)(num & num3)));
				ComObj.WriteData(10, dataReceived);
			}
		}

		public void SetTrigPulse(int[] passData)
		{
			int num = passData[0];
			byte dataReceived = 0;
			if (ComObj.ReadData(10, ref dataReceived) == Result.eERR_SUCCESS)
			{
				byte num3 = 8;
				dataReceived = (byte)(dataReceived & ~num3);
				dataReceived = (byte)(dataReceived | ((byte)(num & num3)));
				ComObj.WriteData(10, dataReceived);
			}
		}

		public void SetTrigTrans(int[] passData)
		{
			int num = passData[0];
			byte dataReceived = 0;
			if (ComObj.ReadData(10, ref dataReceived) == Result.eERR_SUCCESS)
			{
				byte num3 = 0x20;
				dataReceived = (byte)(dataReceived & ~num3);
				dataReceived = (byte)(dataReceived | ((byte)(num & num3)));
				ComObj.WriteData(10, dataReceived);
			}
		}

		public void SetWakeFromSleep(int[] passedData)
		{
			byte num = (byte)passedData[0];
			int num2 = passedData[1];
			byte dataReceived = 0;
			if (ComObj.ReadData(0x2c, ref dataReceived) == Result.eERR_SUCCESS)
			{
				dataReceived = (byte)(dataReceived & ((byte)~num2));
				dataReceived = (byte)(dataReceived | ((byte)(num & num2)));
				ComObj.WriteData(0x2c, dataReceived);
			}
		}

		public void SetWakeOSMode(int[] passData)
		{
			int num = passData[0];
			byte dataReceived = 0;
			if (ComObj.ReadData(0x2b, ref dataReceived) == Result.eERR_SUCCESS)
			{
				byte num3 = 3;
				dataReceived = (byte)(dataReceived & ~num3);
				dataReceived = (byte)(dataReceived | ((byte)num));
				ComObj.WriteData(0x2b, dataReceived);
			}
		}

		public void SetZLockA(int[] passedData)
		{
			int num = passedData[0];
			byte dataReceived = 0;
			if (ComObj.ReadData(0x13, ref dataReceived) == Result.eERR_SUCCESS)
			{
				byte num3 = 7;
				dataReceived = (byte)(dataReceived & ~num3);
				dataReceived = (byte)(dataReceived | ((byte)num));
				ComObj.WriteData(0x13, dataReceived);
			}
		}

		public void StartDevice()
		{
			Result result = Result.eERR_INITIAL_STATE;
			int num = 0;
			byte[] dataBuf = new byte[] { MMA845x.DeviceAddress };
			while ((result != Result.eERR_SUCCESS) && (num < 10))
			{
				result = ComObj.AppRegisterWrite(1, 20, dataBuf);
				num++;
			}
			if (result != Result.eERR_SUCCESS)
			{
				STBLogger.AddEvent(this, STBLogger.EventLevel.Error, "Could not start the Device", string.Format("Class: VeyronDriver; Last Result: {0}; Tries: {1}", result.ToString(), num));
			}
		}

		private void StreamerCallBack(object sender, byte[] packet)
		{
			try
			{
				FBID oFF = MMA845x.DsFBID;
				int idx = 1;

				while (idx < (packet.Length - 1))
				{
					int idx2;
					int idx3;
					int idx4;

					switch (oFF)
					{
						case FBID.DataOut8:
							DataOut8PacketObj.Status = packet[2];
							DataOut8PacketObj.AxisCounts.XAxis = packet[3];
							DataOut8PacketObj.AxisCounts.YAxis = packet[4];
							DataOut8PacketObj.AxisCounts.ZAxis = packet[5];
							lock (DataOut8StreamQueue)
							{
								DataOut8StreamQueue.Enqueue(DataOut8PacketObj);
								Meter.MonitorValue(this, "ProtonDriver->StreamerCallBack->DataOut8StreamQueue.Count", DataOut8StreamQueue.Count);
							}
							idx += 5;
							break;

						case FBID.DataOut14:
							int[] numArray = new int[] { (packet[3] << 6) + (packet[4] >> 2), (packet[5] << 6) + (packet[6] >> 2), (packet[7] << 6) + (packet[8] >> 2) };
							DataOutFullPacketObj.Status = packet[2];
							DataOutFullPacketObj.AxisCounts.XAxis = (ushort)numArray[0];
							DataOutFullPacketObj.AxisCounts.YAxis = (ushort)numArray[1];
							DataOutFullPacketObj.AxisCounts.ZAxis = (ushort)numArray[2];
							lock (DataOutFullStreamQueue)
							{
								DataOutFullStreamQueue.Enqueue(DataOutFullPacketObj);
								Meter.MonitorValue(this, "ProtonDriver->StreamerCallBack->DataOutFullStreamQueue.Count", DataOutFullStreamQueue.Count);
							}
							idx += 8;
							break;

						case FBID.DataOut12:
							int[] numArray2a = new int[] { (packet[3] << 4) + (packet[4] >> 4), (packet[5] << 4) + (packet[6] >> 4), (packet[7] << 4) + (packet[8] >> 4) };
							DataOutFullPacketObj.Status = packet[2];
							DataOutFullPacketObj.AxisCounts.XAxis = (ushort)numArray2a[0];
							DataOutFullPacketObj.AxisCounts.YAxis = (ushort)numArray2a[1];
							DataOutFullPacketObj.AxisCounts.ZAxis = (ushort)numArray2a[2];
							lock (DataOutFullStreamQueue)
							{
								DataOutFullStreamQueue.Enqueue(DataOutFullPacketObj);
								Meter.MonitorValue(this, "ProtonDriver->StreamerCallBack->DataOutFullStreamQueue.Count", DataOutFullStreamQueue.Count);
							}
							idx += 8;
							break;

						case FBID.DataOut10:
							int[] numArray3 = new int[] { (packet[3] << 2) + (packet[4] >> 6), (packet[5] << 2) + (packet[6] >> 6), (packet[7] << 2) + (packet[8] >> 6) };
							DataOutFullPacketObj.Status = packet[0];
							DataOutFullPacketObj.AxisCounts.XAxis = (ushort)numArray3[0];
							DataOutFullPacketObj.AxisCounts.YAxis = (ushort)numArray3[1];
							DataOutFullPacketObj.AxisCounts.ZAxis = (ushort)numArray3[2];
							lock (DataOutFullStreamQueue)
							{
								DataOutFullStreamQueue.Enqueue(DataOutFullPacketObj);
								Meter.MonitorValue(this, "ProtonDriver->StreamerCallBack->DataOutFullStreamQueue.Count", DataOutFullStreamQueue.Count);
							}
							idx += 8;
							break;

						case FBID.FIFO_8B:
							DataOutFIFOPacketObj = new DataOutFIFOPacket();
							DataOutFIFOPacketObj.Status = packet[idx + 1];
							if (packet[1] == 0)
							{
								idx3 = (packet.Length - 3) / 3;
								for (idx2 = 0; idx2 < idx3; idx2++)
								{
									DataOutFIFOPacketObj.FIFOData[idx2] = new XYZCounts();
									DataOutFIFOPacketObj.FIFOData[idx2].XAxis = packet[(idx + 2) + (idx2 * 3)];
									DataOutFIFOPacketObj.FIFOData[idx2].YAxis = packet[(idx + 3) + (idx2 * 3)];
									DataOutFIFOPacketObj.FIFOData[idx2].ZAxis = packet[(idx + 4) + (idx2 * 3)];
								}
								lock (DataOutFIFOStreamQueue)
								{
									DataOutFIFOStreamQueue.Enqueue(DataOutFIFOPacketObj);
									Meter.MonitorValue(this, "ProtonDriver->StreamerCallBack->DataOutFIFOStreamQueue.Count", DataOutFIFOStreamQueue.Count);
								}
								idx += 0x61;
							}
							break;

						case FBID.FIFO_14B:
							DataOutFIFOPacketObj.Status = packet[idx + 1];
							int[] numArray2b = new int[3];
							idx3 = (packet.Length - 3) / 6;
							int s_left, s_right;
							if (packet[1] == 0)
							{
								idx4 = 0;
								while (idx4 < 32)
								{
									DataOutFIFOPacketObj.FIFOData[idx4] = null;
									idx4++;
								}

								for (idx4 = 0; idx4 < idx3; idx4++)
								{
									DataOutFIFOPacketObj.FIFOData[idx4] = new XYZCounts();

									s_left = s_right = 0;

									if (DeviceID == deviceID.MMA8451Q)
									{
										s_left = 6; s_right = 2;
									}
									else if (DeviceID == deviceID.MMA8452Q)
									{
										s_left = 4; s_right = 4;
									}
									else if (DeviceID == deviceID.MMA8453Q)
									{
										s_left = 2; s_right = 6;
									}

									if (s_left > 0)
									{
										numArray2b[0] = (packet[(idx + 2) + (idx4 * 6)] << s_left) + (packet[(idx + 3) + (idx4 * 6)] >> s_right);
										numArray2b[1] = (packet[(idx + 4) + (idx4 * 6)] << s_left) + (packet[(idx + 5) + (idx4 * 6)] >> s_right);
										numArray2b[2] = (packet[(idx + 6) + (idx4 * 6)] << s_left) + (packet[(idx + 7) + (idx4 * 6)] >> s_right);
									}
									DataOutFIFOPacketObj.FIFOData[idx4].XAxis = (ushort)numArray2b[0];
									DataOutFIFOPacketObj.FIFOData[idx4].YAxis = (ushort)numArray2b[1];
									DataOutFIFOPacketObj.FIFOData[idx4].ZAxis = (ushort)numArray2b[2];
								}
								lock (DataOutFIFOStreamQueue)
								{
									DataOutFIFOStreamQueue.Enqueue(DataOutFIFOPacketObj);
									Meter.MonitorValue(this, "ProtonDriver->StreamerCallBack->DataOutFIFOStreamQueue.Count", DataOutFIFOStreamQueue.Count);
								}
								idx += (idx3 * 6) + 1;
							}
							break;

						case FBID.Orientation:
							int[] numArray4 = new int[packet[0]];
							if ((numArray4.Length + 2) <= packet.Length)
							{
								idx2 = 0;
								while (idx2 < numArray4.Length)
								{
									numArray4[idx2] = packet[idx2 + 2];
									idx2++;
								}
							}
							OrientationInterruptQueue.Enqueue(EncodeOrientationPacket(numArray4));
							idx += 5;
							break;

						default:
							idx = packet.Length;
							break;
					}
				}
			}
			finally
			{ }
		}

		public void TransientIntFlush()
		{
			for (int i = 0; i < TransientInterruptQueue.Count; i++)
				TransientInterruptQueue.Dequeue();
		}

		public void WriteAppRegister(int[] passedData)
		{
			int internalAddress = passedData[0];
			int index;
			Result result = Result.eERR_INITIAL_STATE;
			byte[] dataBuf = new byte[passedData.Length - 1];

			for (index = 1; index < passedData.Length; index++)
				dataBuf[index - 1] = (byte)passedData[index];
			for (index = 0; (result != Result.eERR_SUCCESS) && (index < 5); index++)
				result = ComObj.AppRegisterWrite(dataBuf.Length, internalAddress, dataBuf);
		}

		public void WriteValue(int[] passedData)
		{
			int internalAddress = passedData[0];
			int num2 = passedData[1];
			byte dataReceived = 0;
			if (ComObj.ReadData(internalAddress, ref dataReceived) == Result.eERR_SUCCESS)
				dataReceived = (byte)num2;
			ComObj.WriteData(internalAddress, dataReceived);
		}

		public int DeviceAddress
		{
			get { return CurDeviceAddress; }
			set
			{
				CurDeviceAddress = value;
				if (CurDeviceAddress == 0x3A && CommOpen)
					ComObj.SetIOPin(0, 0);
				else if (CommOpen)
					ComObj.ClearIOPin(0, 0);
			}
		}

		public deviceID DeviceID
		{
			get { return MMA845x.DeviceID; }
			set { MMA845x.DeviceID = value; }
		}

		public bool EnableXYZStream
		{
			get { return XYZStreamEnable; }
			set { XYZStreamEnable = value; }
		}

		public bool IsDataConfigInterruptAvailable
		{
			get { return (DataConfigInterruptQueue.Count > 0); }
		}
	}
}

