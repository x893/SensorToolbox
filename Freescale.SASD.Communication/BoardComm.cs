﻿namespace Freescale.SASD.Communication
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Text;

    public class BoardComm : CommClass, Parallel_ICom, CommConfig
    {
		private SerialComDriver Serial = new SerialComDriver();
		private object objectLock = new object();
		
		private Protocol ActiveProtocol;
        private string ID;
		public SerialComDriver.SerialReceptionEventHandler SerialCallbackToDriver = null;

        public event TransactionEventHandler AsynchronousEvents;
        public event CommEventHandler CommEvents;
		public event TransactionEventHandler SynchronousEvents;

		#region Events add/remove 
		event CommEventHandler CommConfig.CommEvent
		{
			add
			{
				lock (objectLock) { CommEvents = (CommEventHandler)Delegate.Combine(CommEvents, value); }
			}
			remove
			{
				lock (objectLock) { CommEvents = (CommEventHandler)Delegate.Remove(CommEvents, value); }
			}
		}

		event TransactionEventHandler Parallel_ICom.AsynchronousTransactionEvent
		{
			add
			{
				lock (objectLock) { AsynchronousEvents = (TransactionEventHandler)Delegate.Combine(AsynchronousEvents, value); }
			}
			remove
			{
				lock (objectLock) { AsynchronousEvents = (TransactionEventHandler)Delegate.Remove(AsynchronousEvents, value); }
			}
		}

		event TransactionEventHandler Parallel_ICom.SynchronousTransactionEvent
		{
			add
			{
				lock (objectLock) { SynchronousEvents = (TransactionEventHandler)Delegate.Combine(SynchronousEvents, value); }
			}
			remove
			{
				lock (objectLock) { SynchronousEvents = (TransactionEventHandler)Delegate.Remove(SynchronousEvents, value); }
			}
		}
		#endregion

		public BoardComm()
        {
            base.SetLowerLayerObj(Serial);
            Serial.SerialEvents += new SerialComDriver.SerialReceptionEventHandler(SerialCallback);
            Serial.CommEvents += new CommEventHandler(CommCallback);
            ActiveProtocol = new Protocol();
            ID = "";
        }

        public Result AppRegisterRead(byte internalAddress, byte count)
        {
            if (internalAddress < 0 || count == 0)
                return Result.eERR_PARAMETER;

            byte[] outMsg = new byte[0];
            byte[] payload = new byte[] { count, internalAddress };
            Result result = ActiveProtocol.BuildMessage(OperationId.AppRegisterRead, payload, ref outMsg);
            if (result == Result.eERR_SUCCESS)
                result = Serial.ExecuteFromHostTransaction(outMsg);
            
			return result;
        }

        public Result AppRegisterWrite(byte internalAddress, ref byte[] data, byte count)
        {
            Result result = Result.eERR_INITIAL_STATE;
            if (((internalAddress < 0) || (count == 0)) || (count > data.Length))
                return Result.eERR_PARAMETER;
            
			byte[] outMsg = new byte[0];
            byte[] payload = new byte[count + 2];
            payload[0] = count;
            payload[1] = internalAddress;
            for (int i = 0; i < count; i++)
                payload[2 + i] = data[i];
            
			result = ActiveProtocol.BuildMessage(OperationId.AppRegisterWrite, payload, ref outMsg);
            if (result == Result.eERR_SUCCESS)
                result = Serial.ExecuteFromHostTransaction(outMsg);
            
			return result;
        }

        public void AsynchronousEventCallbackCaller(byte[] a, OperationId b)
        {
            TransactionEventHandler asynchronousEvents = AsynchronousEvents;
            if (asynchronousEvents != null)
            {
                asynchronousEvents(this, a, b);
            }
        }

        public Result BufferFlush()
        {
            return Result.eERR_NOT_IMPLEMENTED;
        }

        public Result Close()
        {
            return Serial.Close();
        }

        public void CommCallback(object sender, CommEvent cmd)
        {
            CommCallbackCaller(this, cmd);
        }

        public void CommCallbackCaller(object o, CommEvent cmd)
        {
            CommEventHandler commEvents = CommEvents;
            if (commEvents != null)
                commEvents(this, cmd);
        }

        public void End()
        {
            Serial.Close();
            Serial.End();
        }

        ~BoardComm()
        {
        }

        public Result FindAnyHw()
        {
            Result result = Result.eERR_INITIAL_STATE;
            byte[] payload = new byte[0];
            byte[] outMsg = new byte[0];
            ActiveProtocol.BuildMessage(OperationId.Identify, payload, ref outMsg);
            result = Serial.SetActiveProtocol(ActiveProtocol);
            return Serial.FindHW();
        }

        public Result FindHw(string hwid)
        {
            Result result = Result.eERR_INITIAL_STATE;
            byte[] payload = new byte[0];
            byte[] outMsg = new byte[0];
            ActiveProtocol.BuildMessage(OperationId.Identify, payload, ref outMsg);
            result = Serial.FindHW(Encoding.GetEncoding("Windows-1252").GetString(outMsg), hwid);
            if (Result.eERR_SUCCESS == result)
                ID = hwid;
            return result;
        }

        public override void GetCommObj(ref CommClass a)
        {
            if (a is Protocol)
                a = ActiveProtocol;
            else
                base.GetCommObj(ref a);
        }

        public CommunicationState GetCommState()
        {
            return Serial.GetCommState();
        }

        public string GetCommStatus()
        {
            switch (Serial.GetCommState())
            {
                case CommunicationState.Idle:
                    return "No communication with board";

                case CommunicationState.Ready:
                {
                    string[] strArray = new string[4];
                    ID = Serial.GetCommStatus();
                    return ("Communication Active with ID " + ID + " at " + Serial.GetCommString());
                }
                case CommunicationState.HWFind:
                    return ("Finding HW.  Please plug in the board. " + Serial.GetCommStatus());
            }
            STBLogger.AddEvent(this, STBLogger.EventLevel.Warning, "Undefined state in Comm driver found", "Non standard state found while reading the communication driver state.");
            return "Undefined state";
        }

        public Result GetDeviceInfo()
        {
            byte[] outMsg = new byte[0];
            byte[] payload = new byte[0];
            Result result = ActiveProtocol.BuildMessage(OperationId.Identify, payload, ref outMsg);
            if (result == Result.eERR_SUCCESS)
            {
                result = Serial.ExecuteFromHostTransaction(outMsg);
            }
            return result;
        }

        public SerialPort_Corrected GetSerialPort()
        {
            return Serial.GetSerialPort();
        }

        public Result MultiPurposeSend(byte[] dataBuf)
        {
            Result result = Result.eERR_INITIAL_STATE;
            int length = dataBuf.Length;
            byte[] outMsg = new byte[0];
            byte[] payload = new byte[length + 1];
            payload[0] = (byte) length;
            for (int i = 0; i < length; i++)
            {
                payload[1 + i] = dataBuf[i];
            }
            result = ActiveProtocol.BuildMessage(OperationId.MultiPurpose, payload, ref outMsg);
            if (result == Result.eERR_SUCCESS)
            {
                result = Serial.ExecuteFromHostTransaction(outMsg);
            }
            return result;
        }

        public Result NVMBytesWritten()
        {
            Result result = Result.eERR_INITIAL_STATE;
            byte[] outMsg = new byte[0];
            byte[] payload = new byte[0];
            result = ActiveProtocol.BuildMessage(OperationId.NVMBytesWritten, payload, ref outMsg);
            if (result == Result.eERR_SUCCESS)
            {
                result = Serial.ExecuteFromHostTransaction(outMsg);
            }
            return result;
        }

        public Result NVMConfig(byte[] conf)
        {
            Result result = Result.eERR_INITIAL_STATE;
            byte[] outMsg = new byte[0];
            byte[] array = new byte[6];
            conf.CopyTo(array, 0);
            result = ActiveProtocol.BuildMessage(OperationId.NVMConfig, array, ref outMsg);
            if (result == Result.eERR_SUCCESS)
            {
                result = Serial.ExecuteFromHostTransaction(outMsg);
            }
            return result;
        }

        public Result NVMErase()
        {
            Result result = Result.eERR_INITIAL_STATE;
            byte[] outMsg = new byte[0];
            byte[] payload = new byte[0];
            result = ActiveProtocol.BuildMessage(OperationId.NVMErase, payload, ref outMsg);
            if (result == Result.eERR_SUCCESS)
            {
                result = Serial.ExecuteFromHostTransaction(outMsg);
            }
            return result;
        }

        public Result NVMGetConstants()
        {
            Result result = Result.eERR_INITIAL_STATE;
            byte[] outMsg = new byte[0];
            byte[] payload = new byte[0];
            result = ActiveProtocol.BuildMessage(OperationId.NVMGetConstants, payload, ref outMsg);
            if (result == Result.eERR_SUCCESS)
            {
                result = Serial.ExecuteFromHostTransaction(outMsg);
            }
            return result;
        }

        public Result NVMIsErased()
        {
            Result result = Result.eERR_INITIAL_STATE;
            byte[] outMsg = new byte[0];
            byte[] payload = new byte[0];
            result = ActiveProtocol.BuildMessage(OperationId.NVMIsErased, payload, ref outMsg);
            if (result == Result.eERR_SUCCESS)
            {
                result = Serial.ExecuteFromHostTransaction(outMsg);
            }
            return result;
        }

        public Result NVMReadData()
        {
            Result result = Result.eERR_INITIAL_STATE;
            byte[] outMsg = new byte[0];
            byte[] payload = new byte[0];
            result = ActiveProtocol.BuildMessage(OperationId.NVMReadData, payload, ref outMsg);
            if (result == Result.eERR_SUCCESS)
            {
                result = Serial.ExecuteFromHostTransaction(outMsg);
            }
            return result;
        }

        public Result NVMWriteData()
        {
            Result result = Result.eERR_INITIAL_STATE;
            byte[] outMsg = new byte[0];
            byte[] payload = new byte[0];
            result = ActiveProtocol.BuildMessage(OperationId.NVMWriteData, payload, ref outMsg);
            if (result == Result.eERR_SUCCESS)
            {
                result = Serial.ExecuteFromHostTransaction(outMsg);
            }
            return result;
        }

        public Result Open(string[] par1)
        {
            return Serial.Open(par1);
        }

        public Result PortScan(ref string[] comPorts)
        {
            return Serial.PortScan(ref comPorts);
        }

        public Result ReadData(int internalAddress)
        {
            return ReadData(internalAddress, 1);
        }

        public Result ReadData(int internalAddress, int count)
        {
            if ((internalAddress < 0) || (count == 0))
            {
                return Result.eERR_PARAMETER;
            }
            byte[] outMsg = new byte[0];
            byte[] payload = new byte[] { (byte) count, (byte) internalAddress };
            Result result = ActiveProtocol.BuildMessage(OperationId.DeviceRead, payload, ref outMsg);
            if (result == Result.eERR_SUCCESS)
            {
                result = Serial.ExecuteFromHostTransaction(outMsg);
            }
            return result;
        }

        public void ResetBootloaderMode()
        {
            Serial.RestoreComm();
        }

        public void SerialCallback(object sender, ref string buff)
        {
            byte[] packetBytes = new byte[0];
            string str = buff;

            Message currMsg = new Message();
            ActiveProtocol.CheckHeaderAndUpdateBuffer(ref buff, ref currMsg);

            if (ActiveProtocol.IsPacketCompleteAndUpdateBuffer(ref buff, ref packetBytes, ref currMsg))
            {
                byte[] decodedMsg = new byte[0];
                if (ActiveProtocol.DecodeMessage(packetBytes, ref currMsg, ref decodedMsg) == Result.eERR_SUCCESS)
                {
                    if (ActiveProtocol.GetTransactionType(currMsg.Id) == TransactionType.Asynchronous)
                        AsynchronousEventCallbackCaller(decodedMsg, currMsg.Id);
                    else
                    {
                        if (currMsg.Id == OperationId.Identify)
                        {
                            Encoding encoding = Encoding.GetEncoding("Windows-1252");
                            Serial.SetCommStatus(encoding.GetString(decodedMsg));
                        }
                        SynchronousEventCallbackCaller(decodedMsg, currMsg.Id);
                    }
                }
            }
            else if (SerialCallbackToDriver != null)
            {
                SerialCallbackToDriver(this, ref str);
            }
        }

        public void SetBootloaderMode()
        {
            Serial.PauseComm();
        }

        public Result SetConnectionString(string cnxStr)
        {
            return Serial.SetConnectionString(cnxStr);
        }

        public Result SetProtocol(ProtocolId protocol)
        {
            return ActiveProtocol.SetProtocol(protocol);
        }

        public void SynchronousEventCallbackCaller(byte[] a, OperationId b)
        {
            if (SynchronousEvents != null)
                SynchronousEvents(this, a, b);
        }

        public Result WriteData(int internalAddress, byte data)
        {
            byte[] buffer = new byte[] { data };
            return WriteData(internalAddress, buffer, 1);
        }

        public Result WriteData(int internalAddress, byte[] data, int count)
        {
			Result result = Result.eERR_PARAMETER;
			if (internalAddress >= 0 && count != 0 && count <= data.Length)
			{
				byte[] outMsg = new byte[0];
				byte[] payload = new byte[count + 2];

				payload[0] = (byte)count;
				payload[1] = (byte)internalAddress;

				for (int i = 0; i < count; i++)
					payload[2 + i] = data[i];

				result = ActiveProtocol.BuildMessage(OperationId.DeviceWrite, payload, ref outMsg);
				if (result == Result.eERR_SUCCESS)
					result = Serial.ExecuteFromHostTransaction(outMsg);
			}
            return result;
        }
    }
}

