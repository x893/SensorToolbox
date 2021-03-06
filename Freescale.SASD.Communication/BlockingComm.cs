﻿namespace Freescale.SASD.Communication
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading;

    public class BlockingComm : CommClass, ICom, CommConfig
    {
		private BoardComm Comm = new BoardComm();
        private bool IsCommEnabled = false;
        private const int MAX_RETRIES = 4;
        private object objectLock = new object();
        private int Timeout = 300;
		private string AsynchResponse;
		private string CommResponse;
		private string HWID;
		private string Synch;
		private string SynchMsg;
		private OperationId SynchOpId;

        public event MCUToHostEventHandler AsynchronousEvents;
        public event CommEventHandler CommEvents;

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

		event MCUToHostEventHandler ICom.AsynchronousTransactionEvent
		{
			add
			{
				lock (objectLock) { AsynchronousEvents = (MCUToHostEventHandler)Delegate.Combine(AsynchronousEvents, value); }
			}
			remove
			{
				lock (objectLock) { AsynchronousEvents = (MCUToHostEventHandler)Delegate.Remove(AsynchronousEvents, value); }
			}
		}
		#endregion

		public BlockingComm()
		{
			Comm.AsynchronousEvents += new TransactionEventHandler(AsynchCallback);
			Comm.SynchronousEvents += new TransactionEventHandler(SynchCallback);
			Comm.CommEvents += new CommEventHandler(CommCallback);
			Synch = SynchMsg = CommResponse = AsynchResponse = string.Empty;
			SynchOpId = OperationId.Invalid;
			base.SetLowerLayerObj(Comm);
		}

		public SerialPort_Corrected GetSerialPort()
		{
			return Comm.GetSerialPort();
		}

        public Result AppRegisterRead(int count, int internalAddress, ref byte[] dataReceived)
        {
            int num = 0;
            Result result = Result.eERR_INITIAL_STATE;
            Encoding encoding = Encoding.GetEncoding("Windows-1252");

            if (!IsCommEnabled)
                return Result.eERR_NOT_READY;

            lock (Comm)
                while (num < 4)
                {
                    PrepareSynchRx();
                    result = Comm.ReadData(internalAddress);
                    lock (Synch)
                    {
                        Monitor.Wait(Synch, Timeout);

                        if (SynchOpId == OperationId.Invalid)
                            result = Result.eERR_TIMEOUT;
                        else if (SynchOpId == OperationId.AppRegisterRead)
                            result = Result.eERR_SUCCESS;
                        else
                            result = Result.eERR_OTHER_ERROR;
                        
						if ((result == Result.eERR_SUCCESS) && (SynchMsg.Length > 2))
                        {
                            dataReceived = encoding.GetBytes(SynchMsg.Substring(2));
                            if (encoding.GetBytes(SynchMsg.Substring(1))[0] == internalAddress)
                            {
                                SynchMsg = string.Empty;
                                return result;
                            }
                            SynchMsg = string.Empty;
                            result = Result.eERR_OTHER_ERROR;
                        }
                        else
                            dataReceived = new byte[0];

                        num++;
                    }
                }
            return result;
        }

		public Result AppRegisterWrite(int count, int internalAddress, byte[] dataBuf)
		{
			int num = 0;
			Result result = Result.eERR_INITIAL_STATE;
			Encoding encoding = Encoding.GetEncoding("Windows-1252");
			if (!IsCommEnabled)
				return Result.eERR_NOT_READY;
			
			lock (Comm)
				lock (Synch)
					while (num++ < 4)
					{
						result = Comm.AppRegisterWrite((byte)internalAddress, ref dataBuf, (byte)count);
						if (result == Result.eERR_SUCCESS)
						{
							PrepareSynchRx();
							Monitor.Wait(Synch, Timeout);

							if (SynchOpId == OperationId.Invalid)
								result = Result.eERR_TIMEOUT;
							else if (SynchOpId == OperationId.AppRegisterWrite)
								return Result.eERR_SUCCESS;
							else
								result = Result.eERR_OTHER_ERROR;
						}
					}
			return result;
		}

        public void AsynchCallback(object sender, byte[] packet, OperationId oid)
        {
            AsynchronousEventCallbackCaller(packet, oid);
        }

        public void AsynchronousEventCallbackCaller(byte[] a, OperationId b)
        {
            Encoding encoding = Encoding.GetEncoding("Windows-1252");
            MCUToHostEventHandler asynchronousEvents = AsynchronousEvents;
            if (asynchronousEvents != null)
                asynchronousEvents(this, a, b);
        }

		public int ReadIOPin(int bank, int pinNumber, ref byte pinVal)
		{
			throw new NotImplementedException();
		}

		public int SetIOPin(int bank, byte pinNumber)
		{
			throw new NotImplementedException();
		}

		public int ClearIOPin(int bank, byte pinNumber)
        {
            throw new NotImplementedException();
        }

        public Result Close()
        {
            return Comm.Close();
        }

        public void CommCallback(object sender, CommEvent cmd)
        {
            if (cmd == CommEvent.CommOpen)
                IsCommEnabled = true;
            else if (cmd == CommEvent.CommLost)
                IsCommEnabled = false;
            CommEventCallbackCaller(cmd);
        }

        public void CommEventCallbackCaller(CommEvent cmd)
        {
            Encoding encoding = Encoding.GetEncoding("Windows-1252");
            CommEventHandler commEvents = CommEvents;
            if (commEvents != null)
                commEvents(this, cmd);
        }

        public void End()
        {
            Comm.Close();
            Comm.End();
        }

        ~BlockingComm()
        {
            Comm.Close();
        }

        public Result FindAnyHw()
        {
            return Comm.FindAnyHw();
        }

        public Result FindHw(string deviceID)
        {
            HWID = deviceID;
            return Comm.FindHw(deviceID);
        }

        public BoardComm GetBoardComm()
        {
            return Comm;
        }

        public CommunicationState GetCommState()
        {
            return Comm.GetCommState();
        }

        public string GetCommStatus()
        {
            return Comm.GetCommStatus();
        }

        public Result GetDeviceInfo(ref byte[] deviceID)
        {
            int num = 0;
            Result deviceInfo = Result.eERR_INITIAL_STATE;
            byte[] buffer = new byte[0];
            Encoding encoding = Encoding.GetEncoding("Windows-1252");
            if (!IsCommEnabled)
                return Result.eERR_NOT_READY;

            lock (Comm)
                while (num++ < 4)
                {
                    PrepareSynchRx();
                    deviceInfo = Comm.GetDeviceInfo();
                    lock (Synch)
                    {
                        Monitor.Wait(Synch, Timeout);

                        if (SynchOpId == OperationId.Invalid)
                            deviceInfo = Result.eERR_TIMEOUT;
                        else if (SynchOpId == OperationId.Identify)
                            deviceInfo = Result.eERR_SUCCESS;
                        else
                            deviceInfo = Result.eERR_OTHER_ERROR;

                        if ((deviceInfo == Result.eERR_SUCCESS) && (SynchMsg.Length > 2))
                        {
                            deviceID = encoding.GetBytes(SynchMsg.Substring(2));
                            SynchMsg = string.Empty;
                            return deviceInfo;
                        }
                        buffer = new byte[0];
                    }
                }
            return deviceInfo;
        }

		public Result MultiPurposeSend(byte[] dataBuf)
		{
			int num = 0;
			Result result = Result.eERR_INITIAL_STATE;
			Encoding encoding = Encoding.GetEncoding("Windows-1252");
			if (!IsCommEnabled)
				return Result.eERR_NOT_READY;

			lock (Comm)
				lock (Synch)
					while (num++ < 4)
					{
						result = Comm.MultiPurposeSend(dataBuf);
						if (result == Result.eERR_SUCCESS)
						{
							PrepareSynchRx();
							Monitor.Wait(Synch, Timeout);

							if (SynchOpId == OperationId.Invalid)
								result = Result.eERR_TIMEOUT;
							else if (SynchOpId == OperationId.MultiPurpose)
								return Result.eERR_SUCCESS;
							else
								result = Result.eERR_OTHER_ERROR;
						}
					}
			return result;
		}

		public Result NVMBytesWritten(ref byte[] written)
		{
			int num = 0;
			Result result = Result.eERR_INITIAL_STATE;
			byte[] buffer = new byte[0];
			Encoding encoding = Encoding.GetEncoding("Windows-1252");
			if (!IsCommEnabled)
				return Result.eERR_NOT_READY;

			lock (Comm)
				while (num++ < 4)
				{
					PrepareSynchRx();
					result = Comm.NVMBytesWritten();
					lock (Synch)
					{
						Monitor.Wait(Synch, Timeout);

						if (SynchOpId == OperationId.Invalid)
							result = Result.eERR_TIMEOUT;
						else if (SynchOpId == OperationId.NVMBytesWritten)
							result = Result.eERR_SUCCESS;
						else
							result = Result.eERR_OTHER_ERROR;

						if ((result == Result.eERR_SUCCESS) && (SynchMsg.Length > 1))
						{
							written = encoding.GetBytes(SynchMsg);
							SynchMsg = string.Empty;
							return result;
						}
						written = new byte[4];
					}
				}
			return result;
		}

        public Result NVMConfig(byte[] conf)
        {
            int num = 0;
            Result result = Result.eERR_INITIAL_STATE;
            Encoding encoding = Encoding.GetEncoding("Windows-1252");
            if (!IsCommEnabled)
                return Result.eERR_NOT_READY;
            
			lock (Comm)
				while (num++ < 4)
				{
					PrepareSynchRx();
					result = Comm.NVMConfig(conf);
					lock (Synch)
					{
						Monitor.Wait(Synch, Timeout);

						if (SynchOpId == OperationId.Invalid)
							result = Result.eERR_TIMEOUT;
						else if (SynchOpId == OperationId.NVMConfig)
							result = Result.eERR_SUCCESS;
						else
							result = Result.eERR_OTHER_ERROR;
						
						if (result == Result.eERR_SUCCESS)
						{
							SynchMsg = string.Empty;
							return result;
						}
					}
				}
            return result;
        }

        public Result NVMErase()
        {
            int num = 0;
            Result result = Result.eERR_INITIAL_STATE;
            Encoding encoding = Encoding.GetEncoding("Windows-1252");
            if (!IsCommEnabled)
                return Result.eERR_NOT_READY;
            
			lock (Comm)
                while (num++ < 4)
                {
                    PrepareSynchRx();
                    result = Comm.NVMErase();
                    lock (Synch)
                    {
                        Monitor.Wait(Synch, Timeout);

                        if (SynchOpId == OperationId.Invalid)
                            result = Result.eERR_TIMEOUT;
                        else if (SynchOpId == OperationId.NVMErase)
                            result = Result.eERR_SUCCESS;
                        else
                            result = Result.eERR_OTHER_ERROR;
                        
						if (result == Result.eERR_SUCCESS)
                        {
                            SynchMsg = string.Empty;
                            return result;
                        }
                    }
                }
            return result;
        }

        public Result NVMGetConstants(ref byte[] constants)
        {
            int num = 0;
            Result result = Result.eERR_INITIAL_STATE;
            byte[] buffer = new byte[0];
            Encoding encoding = Encoding.GetEncoding("Windows-1252");
            if (!IsCommEnabled)
                return Result.eERR_NOT_READY;
            
			lock (Comm)
                while (num++ < 4)
                {
                    PrepareSynchRx();
                    result = Comm.NVMGetConstants();
                    lock (Synch)
                    {
                        Monitor.Wait(Synch, Timeout);

                        if (SynchOpId == OperationId.Invalid)
                            result = Result.eERR_TIMEOUT;
                        else if (SynchOpId == OperationId.NVMGetConstants)
                            result = Result.eERR_SUCCESS;
                        else
                            result = Result.eERR_OTHER_ERROR;
                        
						if ((result == Result.eERR_SUCCESS) && (SynchMsg.Length > 1))
                        {
                            constants = encoding.GetBytes(SynchMsg);
                            SynchMsg = string.Empty;
                            return result;
                        }
                        buffer = new byte[0];
                    }
                }
            return result;
        }

		public Result NVMIsErased(ref byte[] erased)
		{
			int num = 0;
			Result result = Result.eERR_INITIAL_STATE;
			Encoding encoding = Encoding.GetEncoding("Windows-1252");
			if (!IsCommEnabled)
				return Result.eERR_NOT_READY;

			lock (Comm)
				while (num++ < 4)
				{
					PrepareSynchRx();
					result = Comm.NVMIsErased();
					lock (Synch)
					{
						Monitor.Wait(Synch, Timeout);

						if (SynchOpId == OperationId.Invalid)
							result = Result.eERR_TIMEOUT;
						else if (SynchOpId == OperationId.NVMIsErased)
							result = Result.eERR_SUCCESS;
						else
							result = Result.eERR_OTHER_ERROR;
						
						if ((result == Result.eERR_SUCCESS) && (SynchMsg.Length == 1))
						{
							erased = encoding.GetBytes(SynchMsg);
							SynchMsg = string.Empty;
							return result;
						}
						erased = new byte[1];
					}
				}
			return result;
		}

        public Result NVMReadData(ref byte[] dataReceived)
        {
            int num = 0;
            Result result = Result.eERR_INITIAL_STATE;
            Encoding encoding = Encoding.GetEncoding("Windows-1252");
            if (!IsCommEnabled)
                return Result.eERR_NOT_READY;

            lock (Comm)
            {
                PrepareSynchRx();
                result = Comm.NVMReadData();
                lock (Synch)
                {
                    while (num < 4)
                    {
                        Monitor.Wait(Synch, Timeout);

                        if (SynchOpId == OperationId.Invalid)
                            result = Result.eERR_TIMEOUT;
                        else if (SynchOpId == OperationId.NVMReadData)
                            result = Result.eERR_SUCCESS;
                        else
                            result = Result.eERR_OTHER_ERROR;
                        
						if ((result == Result.eERR_SUCCESS) && (SynchMsg.Length > 1))
                        {
                            dataReceived = new byte[SynchMsg.Length];
                            dataReceived = encoding.GetBytes(SynchMsg);
                            SynchMsg = string.Empty;
                            return result;
                        }
                        dataReceived = new byte[1];
                    }
                }
}
            return result;
        }

        public Result NVMWriteData()
        {
            int num = 0;
            Result result = Result.eERR_INITIAL_STATE;
            Encoding encoding = Encoding.GetEncoding("Windows-1252");
            if (!IsCommEnabled)
                return Result.eERR_NOT_READY;
            
			lock (Comm)
                while (num++ < 4)
                {
                    PrepareSynchRx();
                    result = Comm.NVMWriteData();
                    lock (Synch)
                    {
                        Monitor.Wait(Synch, Timeout);

                        if (SynchOpId == OperationId.Invalid)
                            result = Result.eERR_TIMEOUT;
                        else if (SynchOpId == OperationId.NVMWriteData)
                            result = Result.eERR_SUCCESS;
                        else
                            result = Result.eERR_OTHER_ERROR;
                        
						if (result == Result.eERR_SUCCESS)
                        {
                            SynchMsg = string.Empty;
                            return result;
                        }
                    }
                }
            return result;
        }

        public Result Open(string[] str)
        {
            return Comm.Open(str);
        }

        public Result ReadData(int internalAddress, ref byte dataReceived)
        {
            Result result = Result.eERR_INITIAL_STATE;
            int num = 0;
            Encoding encoding = Encoding.GetEncoding("Windows-1252");
            if (!IsCommEnabled)
                return Result.eERR_NOT_READY;
            
			lock (Comm)
                while (num++ < 4)
                {
                    PrepareSynchRx();
                    result = Comm.ReadData(internalAddress);
                    lock (Synch)
                    {
                        Monitor.Wait(Synch, Timeout);

                        if (SynchOpId == OperationId.Invalid)
                            result = Result.eERR_TIMEOUT;
                        else if (SynchOpId == OperationId.DeviceRead)
                            result = Result.eERR_SUCCESS;
                        else
                            result = Result.eERR_OTHER_ERROR;
                        
						if (result == Result.eERR_SUCCESS && SynchMsg.Length > 2)
                        {
                            dataReceived = encoding.GetBytes(SynchMsg.Substring(2))[0];
                            if (encoding.GetBytes(SynchMsg.Substring(1))[0] == internalAddress)
                            {
                                SynchMsg = string.Empty;
                                return result;
                            }
                            SynchMsg = string.Empty;
                            result = Result.eERR_OTHER_ERROR;
                        }
                        else
                            dataReceived = 0;
                    }
                }
            return result;
        }

        public Result ReadData(int count, int internalAddress, ref byte[] dataReceived)
        {
            int num = 0;
            Result result = Result.eERR_INITIAL_STATE;
            Encoding encoding = Encoding.GetEncoding("Windows-1252");
            if (!IsCommEnabled)
                return Result.eERR_NOT_READY;

            lock (Comm)
                while (num++ < 4)
                {
                    PrepareSynchRx();
                    result = Comm.ReadData(internalAddress, count);
                    lock (Synch)
                    {
                        Monitor.Wait(Synch, Timeout);

                        if (SynchOpId == OperationId.Invalid)
                            result = Result.eERR_TIMEOUT;
                        else if (SynchOpId == OperationId.DeviceRead)
                            result = Result.eERR_SUCCESS;
                        else
                            result = Result.eERR_OTHER_ERROR;
                        
						if ((result == Result.eERR_SUCCESS) && (SynchMsg.Length > 2))
                        {
                            dataReceived = encoding.GetBytes(SynchMsg.Substring(2));
                            if (encoding.GetBytes(SynchMsg.Substring(1))[0] == internalAddress)
                            {
                                SynchMsg = string.Empty;
                                return result;
                            }
                            SynchMsg = string.Empty;
                            result = Result.eERR_OTHER_ERROR;
                        }
                        else
                            dataReceived = new byte[0];
                    }
                }
            return result;
        }

        public void ResetBootloaderMode()
        {
            Comm.ResetBootloaderMode();
        }

        public void SetBootloaderMode()
        {
            Comm.SetBootloaderMode();
        }

        public Result SetConnectionString(string cnxStr)
        {
            return Comm.SetConnectionString(cnxStr);
        }

        public Result SetProtocol(ProtocolId pid)
        {
            return Comm.SetProtocol(pid);
        }

		private void PrepareSynchRx()
		{
			SynchMsg = "";
			SynchOpId = OperationId.Invalid;
		}

		public void SynchCallback(object sender, byte[] packet, OperationId oid)
        {
            lock (Synch)
            {
                SynchMsg = Encoding.GetEncoding("Windows-1252").GetString(packet);
                SynchOpId = oid;
                Monitor.PulseAll(Synch);
            }
        }

		public Result WriteData(int internalAddress, byte data)
		{
			Result result = Result.eERR_INITIAL_STATE;
			int num = 0;
			Encoding encoding = Encoding.GetEncoding("Windows-1252");
			if (!IsCommEnabled)
				return Result.eERR_NOT_READY;
			lock (Comm)
				lock (Synch)
					while (num++ < 4)
					{
						PrepareSynchRx();
						Comm.WriteData(internalAddress, data);
						Monitor.Wait(Synch, Timeout);

						if (SynchOpId == OperationId.Invalid)
							result = Result.eERR_TIMEOUT;
						else
						{
							if (SynchOpId == OperationId.DeviceWrite)
								return Result.eERR_SUCCESS;
							result = Result.eERR_OTHER_ERROR;
						}
						Thread.Sleep(1);
					}
			return result;
		}

		public Result WriteData(int count, int internalAddress, byte[] dataBuf)
		{
			Result result = Result.eERR_INITIAL_STATE;
			int num = 0;
			Encoding encoding = Encoding.GetEncoding("Windows-1252");
			if (!IsCommEnabled)
				return Result.eERR_NOT_READY;

			lock (Comm)
				lock (Synch)
					while (num++ < 4)
					{
						PrepareSynchRx();
						Comm.WriteData(internalAddress, dataBuf, count);
						Monitor.Wait(Synch, Timeout);

						if (SynchOpId == OperationId.Invalid)
							result = Result.eERR_TIMEOUT;
						else
						{
							if (SynchOpId == OperationId.DeviceWrite)
								return Result.eERR_SUCCESS;
							result = Result.eERR_OTHER_ERROR;
						}
						Thread.Sleep(1);
					}
			return result;
		}
	}
}

