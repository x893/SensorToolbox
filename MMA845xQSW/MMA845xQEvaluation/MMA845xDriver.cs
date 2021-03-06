﻿namespace MMA845xQEvaluation
{
	using Freescale.SASD.Communication;
	using System;
	using System.Collections;
	using System.Threading;

	internal class MMA845xDriver
	{
		public XYZCounts DataArrayObj = new XYZCounts();
		public DataOutFIFOPacket DataOutFIFOPacketObj = new DataOutFIFOPacket();
		public int SystemPollingOrInterrupt;
		public int XYZ12DatalogFlag = 0;
		public int XYZ8DatalogFlag = 0;
		public int XYZTransDatalogFlag = 0;

		private BlockingComm BlockingCommObj = new BlockingComm();
		private bool CommOpen;
		private ICom ComObj;
		private int CurDeviceAddress;
		private RequestModeType CurrentStreamRequestMode;
		private Queue DataConfigInterruptQueue;
		private DataOut8Packet DataOut8PacketObj = new DataOut8Packet();
		private Queue DataOut8StreamQueue = new Queue();
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
		private Queue TransientInterruptQueue;
		private bool XYZStreamEnable;

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
				InterruptDetectionCallBack(sender, packet);
			else if (oid == OperationId.StreamData)
				StreamerCallBack(sender, packet);
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
				MMA845x.SetDeviceId(commStatus.Substring(commStatus.IndexOf("HW:") + 3, 4));
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
				DataOut8StreamQueue.Clear();
		}

		public void DataOutFIFOStreamFlush()
		{
			for (int i = 0; i < DataOutFIFOStreamQueue.Count; i++)
				DataOutFIFOStreamQueue.Dequeue();
		}

		public void DataOutFullStreamFlush()
		{
			lock (DataOutFullStreamQueue)
				DataOutFullStreamQueue.Clear();
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

		private void EnableDataStream(SampleRate sampleRate, RequestModeType requestMode, byte[] requestTokens)
		{
			byte[] buffer;
			byte[] buffer2;
			if (requestTokens.Length > 1)
			{
				buffer2 = new byte[4];
				buffer2[3] = requestTokens[1];
				buffer = buffer2;
			}
			else
			{
				buffer2 = new byte[4];
				buffer = buffer2;
			}
			if (requestMode == RequestModeType.FBMR)
			{
				buffer[0] = (byte)MMA845x.GetLength((FBID)requestTokens[0]);
				buffer[1] = (byte)MMA845x.GetRegAddress((FBID)requestTokens[0]);
				buffer[2] = (byte)sampleRate;
				if (Result.eERR_SUCCESS == ComObj.AppRegisterWrite(4, 0x10, buffer))
					MMA845x.DsFBID = (FBID)requestTokens[0];
			}
			else
			{
				buffer[2] = (byte)sampleRate;
				ComObj.AppRegisterWrite(4, 0x10, buffer);
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
			catch (Exception) { }
		}

		public byte[] GetDataStatus()
		{
			byte[] buffer = new byte[8];
			byte dataReceived = 0;
			ComObj.ReadData(0, ref dataReceived);
			buffer[0] = (byte)((dataReceived & 0x01) == 0x01 ? 1 : 0);
			buffer[1] = (byte)((dataReceived & 0x02) == 0x02 ? 1 : 0);
			buffer[2] = (byte)((dataReceived & 0x04) == 0x04 ? 1 : 0);
			buffer[3] = (byte)((dataReceived & 0x08) == 0x08 ? 1 : 0);
			buffer[4] = (byte)((dataReceived & 0x10) == 0x10 ? 1 : 0);
			buffer[5] = (byte)((dataReceived & 0x20) == 0x20 ? 1 : 0);
			buffer[6] = (byte)((dataReceived & 0x40) == 0x40 ? 1 : 0);
			buffer[7] = (byte)((dataReceived & 0x80) == 0x80 ? 1 : 0);
			return buffer;
		}

		public byte[] GetFFMT1Status()
		{
			byte dataReceived = 0;
			Result result = Result.eERR_INITIAL_STATE;
			byte[] buffer = new byte[7];
			result = ComObj.ReadData(0x16, ref dataReceived);
			buffer[0] = (byte)((dataReceived & 0x02) == 0x02 ? 1 : 0);
			buffer[1] = (byte)((dataReceived & 0x01) == 0x01 ? 1 : 0);
			buffer[2] = (byte)((dataReceived & 0x08) == 0x08 ? 1 : 0);
			buffer[3] = (byte)((dataReceived & 0x04) == 0x04 ? 1 : 0);
			buffer[4] = (byte)((dataReceived & 0x20) == 0x20 ? 1 : 0);
			buffer[5] = (byte)((dataReceived & 0x10) == 0x10 ? 1 : 0);
			buffer[6] = (byte)((dataReceived & 0x80) == 0x80 ? 1 : 0);
			return buffer;
		}

		public int[] GetFIFOData(int samples, int FIFODump8orFull)
		{
			int num2;
			int num = (FIFODump8orFull == 1 ? 1 : 2);
			int[] numArray = new int[(samples * 3) * num];
			int[] data = new int[(samples * 3) * num];
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
						data[num2] = ~numArray[num2] + 1;
						data[num2] &= 0xFF;
						data[num2] *= -1;
					}
					else
						data[num2] = numArray[num2];
				}
				return data;
			}
			if (ComObj.ReadData((samples * 3) * 2, 1, ref dataReceived) == Result.eERR_SUCCESS)
			{
				if (DeviceID == deviceID.MMA8451Q)
					for (num2 = 0; num2 < (samples * 3); num2++)
					{
						numArray[num2] = (dataReceived[num2 * 2] << 6) + (dataReceived[(num2 * 2) + 1] >> 2);
						if ((numArray[num2] & 0x2000) == 0x2000)
						{
							data[num2] = ~numArray[num2] + 1;
							data[num2] &= 0x3FFF;
							data[num2] *= -1;
						}
						else
							data[num2] = numArray[num2];
					}
				
				if (DeviceID != deviceID.MMA8652FC)
					return data;
				
				for (num2 = 0; num2 < (samples * 3); num2++)
				{
					numArray[num2] = (dataReceived[num2 * 2] << 4) + (dataReceived[(num2 * 2) + 1] >> 4);
					if ((numArray[num2] & 0x800) == 0x800)
					{
						data[num2] = ~numArray[num2] + 1;
						data[num2] &= 0xFFF;
						data[num2] *= -1;
					}
					else
						data[num2] = numArray[num2];
				}
			}
			return data;
		}

		public byte[] GetFIFOStatus()
		{
			byte[] buffer = new byte[3];
			byte dataReceived = 0;
			ComObj.ReadData(0, ref dataReceived);
			buffer[0] = (byte)((dataReceived & 0x80) == 0x80 ? 1 : 0);
			buffer[1] = (byte)((dataReceived & 0x40) == 0x40 ? 1 : 0);
			dataReceived = (byte)(dataReceived & 0x3f);
			buffer[2] = dataReceived;
			return buffer;
		}

		public byte[] GetInterruptStatus()
		{
			byte[] buffer = new byte[8];
			byte dataReceived = 0;
			ComObj.ReadData(12, ref dataReceived);
			buffer[0] = (byte)((dataReceived & 0x01) == 0x01 ? 1 : 0);
			buffer[1] = (byte)((dataReceived & 0x02) == 0x02 ? 1 : 0);
			buffer[2] = (byte)((dataReceived & 0x04) == 0x04 ? 1 : 0);
			buffer[3] = (byte)((dataReceived & 0x08) == 0x08 ? 1 : 0);
			buffer[4] = (byte)((dataReceived & 0x10) == 0x10 ? 1 : 0);
			buffer[5] = (byte)((dataReceived & 0x20) == 0x20 ? 1 : 0);
			buffer[6] = (byte)((dataReceived & 0x40) == 0x40 ? 1 : 0);
			buffer[7] = (byte)((dataReceived & 0x80) == 0x80 ? 1 : 0);
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
			buffer[0] = (byte)((dataReceived & 0x01) == 0 ? 0 : 1);
			if ((dataReceived & 6) == 0)
				buffer[1] = 0;
			else if ((dataReceived & 6) == 2)
				buffer[1] = 1;
			else if ((dataReceived & 6) == 4)
				buffer[1] = 2;
			else if ((dataReceived & 6) == 6)
				buffer[1] = 3;

			buffer[2] = (byte)((dataReceived & 0x40) == 0x40 ? 1 : 0);
			buffer[3] = (byte)((dataReceived & 0x80) == 0x80 ? 1 : 0);
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

		public bool IsDataOutFIFO14StreamAvailable(ref DataOutFullPacket dOut12Packet)
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
			lock (DataOutFullStreamQueue)
			{
				if (DataOutFullStreamQueue.Count > 0)
				{
					dOut12Packet = (DataOutFullPacket)DataOutFIFOStreamQueue.Dequeue();
					return true;
				}
				return false;
			}
		}

		public bool IsDataOutFIFO8StreamAvailable(ref DataOutFIFOPacket dOutFIFO8Packet)
		{
			RWLock.AcquireWriterLock(-1);
			try
			{
				lock (DataOutFIFOStreamQueue)
					if (DataOutFIFOStreamQueue.Count > 0)
					{
						dOutFIFO8Packet = (DataOutFIFOPacket)DataOutFIFOStreamQueue.Dequeue();
						return true;
					}
			}
			finally
			{
				RWLock.ReleaseWriterLock();
			}
			return false;
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
						switch (packet[index])
						{
							case 1:
								DataOut8PacketObj.Status = packet[0];
								DataOut8PacketObj.AxisCounts.XAxis = packet[index + 1];
								DataOut8PacketObj.AxisCounts.YAxis = packet[index + 2];
								DataOut8PacketObj.AxisCounts.ZAxis = packet[index + 3];
								index += 5;
								break;
							case 2:
								numArray4 = new int[] {
									(packet[index + 1] << 6) + (packet[index + 2] >> 2),
									(packet[index + 3] << 6) + (packet[index + 4] >> 2),
									(packet[index + 5] << 6) + (packet[index + 6] >> 2)
								};
								DataOutFullPacketObj.Status = packet[0];
								DataOutFullPacketObj.AxisCounts.XAxis = (ushort)numArray4[0];
								DataOutFullPacketObj.AxisCounts.YAxis = (ushort)numArray4[1];
								DataOutFullPacketObj.AxisCounts.ZAxis = (ushort)numArray4[2];
								index += 8;
								break;
							case 3:
								numArray4 = new int[] {
									(packet[index + 1] << 4) + (packet[index + 2] >> 4),
									(packet[index + 3] << 4) + (packet[index + 4] >> 4),
									(packet[index + 5] << 4) + (packet[index + 6] >> 4)
								};
								DataOutFullPacketObj.Status = packet[0];
								DataOutFullPacketObj.AxisCounts.XAxis = (ushort)numArray4[0];
								DataOutFullPacketObj.AxisCounts.YAxis = (ushort)numArray4[1];
								DataOutFullPacketObj.AxisCounts.ZAxis = (ushort)numArray4[2];
								index += 8;
								break;
							case 4:
								numArray4 = new int[] {
									(packet[index + 1] << 2) + (packet[index + 2] >> 6),
									(packet[index + 3] << 2) + (packet[index + 4] >> 6),
									(packet[index + 5] << 2) + (packet[index + 6] >> 6)
								};
								DataOutFullPacketObj.Status = packet[0];
								DataOutFullPacketObj.AxisCounts.XAxis = (ushort)numArray4[0];
								DataOutFullPacketObj.AxisCounts.YAxis = (ushort)numArray4[1];
								DataOutFullPacketObj.AxisCounts.ZAxis = (ushort)numArray4[2];
								index += 8;
								break;
							case 8:
								numArray4 = new int[packet[0]];
								if ((numArray4.Length + 2) <= packet.Length)
								{
									num2 = 0;
									if (num2 < numArray4.Length)
									{
										numArray4[num2] = packet[num2 + 2];
										num2++;
									}
								}
								numArray4 = EncodeOrientationPacket(numArray4);
								OrientationInterruptQueue.Enqueue(numArray4);
								index += 5;
								break;

							default:
								index = packet.Length;
								break;
						}
					}
				}
				finally { }
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
				if ((MMA845x.DeviceID == deviceID.MMA8451Q) || (MMA845x.DeviceID == deviceID.MMA8491Q))
				{
					MMA845x.DsFBID = FBID.DataOut14;
					buffer[1] = 2;
				}
				else if ((MMA845x.DeviceID == deviceID.MMA8452Q) || (MMA845x.DeviceID == deviceID.MMA8652FC))
				{
					MMA845x.DsFBID = FBID.DataOut12;
					buffer[1] = 3;
				}
				else if ((MMA845x.DeviceID == deviceID.MMA8453Q) || (MMA845x.DeviceID == deviceID.MMA8653FC))
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
				Result result = ComObj.WriteData(0x2a, dataReceived);
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
				dataReceived = (byte)(dataReceived | ((byte)num));
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
				dataReceived = (byte)(dataReceived | ((byte)(num & num3)));
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
			if (CurrentStreamRequestMode == RequestModeType.FBMR)
			{
				try
				{
					FBID fbid = MMA845x.DsFBID;
					int num = 1;
					while (num < (packet.Length - 1))
					{
						int[] numArray2;
						int[] numArray4;
						int num2;
						int[] numArray5;
						int num3;
						int num4;
						Queue queue;
						switch (fbid)
						{
							case FBID.DataOut8:
								DataOut8PacketObj.Status = packet[2];
								DataOut8PacketObj.AxisCounts.XAxis = packet[3];
								DataOut8PacketObj.AxisCounts.YAxis = packet[4];
								DataOut8PacketObj.AxisCounts.ZAxis = packet[5];
								lock ((queue = DataOut8StreamQueue))
								{
									DataOut8StreamQueue.Enqueue(DataOut8PacketObj);
									Meter.MonitorValue(this, "ProtonDriver->StreamerCallBack->DataOut8StreamQueue.Count", DataOut8StreamQueue.Count);
								}
								num += 5;
								break;

							case FBID.DataOut14:
								int[] numArray = new int[] { (packet[3] << 6) + (packet[4] >> 2), (packet[5] << 6) + (packet[6] >> 2), (packet[7] << 6) + (packet[8] >> 2) };
								DataOutFullPacketObj.Status = packet[2];
								DataOutFullPacketObj.AxisCounts.XAxis = (ushort)numArray[0];
								DataOutFullPacketObj.AxisCounts.YAxis = (ushort)numArray[1];
								DataOutFullPacketObj.AxisCounts.ZAxis = (ushort)numArray[2];
								lock ((queue = DataOutFullStreamQueue))
								{
									DataOutFullStreamQueue.Enqueue(DataOutFullPacketObj);
									Meter.MonitorValue(this, "ProtonDriver->StreamerCallBack->DataOutFullStreamQueue.Count", DataOutFullStreamQueue.Count);
								}
								num += 8;
								break;

							case FBID.DataOut12:
								numArray2 = new int[] { (packet[3] << 4) + (packet[4] >> 4), (packet[5] << 4) + (packet[6] >> 4), (packet[7] << 4) + (packet[8] >> 4) };
								DataOutFullPacketObj.Status = packet[2];
								DataOutFullPacketObj.AxisCounts.XAxis = (ushort)numArray2[0];
								DataOutFullPacketObj.AxisCounts.YAxis = (ushort)numArray2[1];
								DataOutFullPacketObj.AxisCounts.ZAxis = (ushort)numArray2[2];
								lock ((queue = DataOutFullStreamQueue))
								{
									DataOutFullStreamQueue.Enqueue(DataOutFullPacketObj);
									Meter.MonitorValue(this, "ProtonDriver->StreamerCallBack->DataOutFullStreamQueue.Count", DataOutFullStreamQueue.Count);
								}
								num += 8;
								break;

							case FBID.DataOut10:
								int[] numArray3 = new int[] { (packet[3] << 2) + (packet[4] >> 6), (packet[5] << 2) + (packet[6] >> 6), (packet[7] << 2) + (packet[8] >> 6) };
								DataOutFullPacketObj.Status = packet[0];
								DataOutFullPacketObj.AxisCounts.XAxis = (ushort)numArray3[0];
								DataOutFullPacketObj.AxisCounts.YAxis = (ushort)numArray3[1];
								DataOutFullPacketObj.AxisCounts.ZAxis = (ushort)numArray3[2];
								lock ((queue = DataOutFullStreamQueue))
								{
									DataOutFullStreamQueue.Enqueue(DataOutFullPacketObj);
									Meter.MonitorValue(this, "ProtonDriver->StreamerCallBack->DataOutFullStreamQueue.Count", DataOutFullStreamQueue.Count);
								}
								num += 8;
								break;

							case FBID.FIFO_8B:
								DataOutFIFOPacketObj.Status = packet[num + 1];
								num3 = (packet.Length - 3) / 3;
								num2 = 0;
								while (num2 < num3)
								{
									DataOutFIFOPacketObj.FIFOData[num2] = new XYZCounts();
									DataOutFIFOPacketObj.FIFOData[num2].XAxis = packet[(num + 2) + (num2 * 3)];
									DataOutFIFOPacketObj.FIFOData[num2].YAxis = packet[(num + 3) + (num2 * 3)];
									DataOutFIFOPacketObj.FIFOData[num2].ZAxis = packet[(num + 4) + (num2 * 3)];
									num2++;
								}
								lock ((queue = DataOutFIFOStreamQueue))
								{
									DataOutFIFOStreamQueue.Enqueue(DataOutFIFOPacketObj);
									Meter.MonitorValue(this, "ProtonDriver->StreamerCallBack->DataOutFIFOStreamQueue.Count", DataOutFIFOStreamQueue.Count);
								}
								num += 0x61;
								break;

							case FBID.FIFO_14B:
								DataOutFIFOPacketObj.Status = packet[num + 1];
								numArray2 = new int[3];
								num4 = 0;
								num4 = 0;
								while (num4 < 0x20)
								{
									DataOutFIFOPacketObj.FIFOData[num4] = new XYZCounts();
									numArray2[0] = packet[(num + 1) + (num4 * 6)] + (packet[(num + 2) + (num4 * 6)] * 0x10);
									numArray2[1] = packet[(num + 3) + (num4 * 6)] + (packet[(num + 4) + (num4 * 6)] * 0x10);
									numArray2[2] = packet[(num + 5) + (num4 * 6)] + (packet[(num + 6) + (num4 * 6)] * 0x10);
									DataOutFIFOPacketObj.FIFOData[num4].XAxis = (ushort)numArray2[0];
									DataOutFIFOPacketObj.FIFOData[num4].YAxis = (ushort)numArray2[1];
									DataOutFIFOPacketObj.FIFOData[num4].ZAxis = (ushort)numArray2[2];
									num4++;
								}

								lock ((queue = DataOutFIFOStreamQueue))
								{
									DataOutFIFOStreamQueue.Enqueue(DataOutFIFOPacketObj);
									Meter.MonitorValue(this, "ProtonDriver->StreamerCallBack->DataOutFIFOStreamQueue.Count", DataOutFIFOStreamQueue.Count);
								}
								num += 192;
								break;

							case FBID.Orientation:
								numArray4 = new int[packet[0]];
								num2 = 0;
								if ((numArray4.Length + 2) <= packet.Length)
								{
									while (num2 < numArray4.Length)
									{
										numArray4[num2] = packet[num2 + 2];
										num2++;
									}
								}
								numArray5 = EncodeOrientationPacket(numArray4);
								OrientationInterruptQueue.Enqueue(numArray5);
								num += 5;
								break;

							default:
								num = packet.Length;
								break;
						}
					}
				}
				finally { }
			}
		}

		public void TransientIntFlush()
		{
			for (int i = 0; i < TransientInterruptQueue.Count; i++)
				TransientInterruptQueue.Dequeue();
		}

		public void WriteAppRegister(int[] passedData)
		{
			int internalAddress = passedData[0];
			int index = 0;
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
			get
			{
				return CurDeviceAddress;
			}
			set
			{
				CurDeviceAddress = value;
				if ((CurDeviceAddress == 0x3a) && CommOpen)
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