﻿namespace Freescale.SASD.Communication
{
    using System;
    using System.IO;
    using System.IO.Ports;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading;

    public class SerialComDriver : CommClass
    {
        private Thread _readThread;
        private static ManualResetEvent AbortThread = new ManualResetEvent(false);
        private Protocol ActiveProtocol;
        private string Buffer;

        private string[] ConnectionString = new string[] { "", "115200", "0", "8", "1", "0" };
        private string ID = string.Empty;
        private bool IsExtendedFindHW = false;
        private object objectLock = new object();
        private CommunicationState OldCommState = CommunicationState.Idle;
        private int oldTimeout;
        private Mutex PortWriteRequest = new Mutex(false);
        private StringBuilder sb = new StringBuilder();
        private AutoResetEvent SynchronousOperation = new AutoResetEvent(false);

		private delegate void CallbackCaller();

		/// <summary>
		/// 
		/// </summary>
		public CommunicationState CommState = CommunicationState.Idle;
		/// <summary>
		/// 
		/// </summary>
		public string FindHWmsgIn = string.Empty;
		/// <summary>
		/// 
		/// </summary>
		public string FindHWmsgOut = string.Empty;
		/// <summary>
		/// 
		/// </summary>
		public SerialPort_Corrected SerialPortSTB;
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="buff"></param>
		public delegate void SerialReceptionEventHandler(object sender, ref string buff);
		/// <summary>
		/// 
		/// </summary>
		public event SerialReceptionEventHandler SerialEvents;
		/// <summary>
		/// 
		/// </summary>
		public event CommEventHandler CommEvents;

		/// <summary>
		/// Default constructor
		/// </summary>
        public SerialComDriver()
        {
            try
            {
                Buffer = Buffer + "Welcome\n\r";
                if (_readThread == null)
                    _readThread = new Thread(new ThreadStart(Scan));

                SerialPortSTB = new SerialPort_Corrected();
                _readThread.Start();
            }
            catch (Exception exception)
            {
                STBLogger.AddEvent(this, STBLogger.EventLevel.Information, "Creation of Comm Object", exception.Message + exception.Source + exception.StackTrace);
            }
        }

		/// <summary>
		/// Communication callback function
		/// </summary>
		/// <param name="cmd"></param>
		public void CommCallbackCaller(CommEvent cmd)
		{
			if (CommEvents != null)
				CommEvents(this, cmd);
		}

		/// <summary>
		/// Event callback function
		/// </summary>
		/// <param name="buff"></param>
		public void EventCallbackCaller(ref string buff)
		{
			if (SerialEvents != null)
				SerialEvents(this, ref buff);
		}

		private void Scan()
		{
			string packet_data = "";
			string serial_data = "";
			int length = 0;
			Encoding encoding = Encoding.GetEncoding("windows-1252");

			SerialPortSTB.Encoding = encoding;

			while (true)
			{
				if (CommState == CommunicationState.HWFind)
				{
					Thread.Sleep(10);
					if (IsExtendedFindHW)
						ExtendedFindHW();
					else
						NormalFindHW();
				}

				while (CommState == CommunicationState.Ready)
				{
					try
					{
						if (SerialPortSTB.IsOpen)
						{
							if (SerialPortSTB.BytesToRead > 0)
							{
								serial_data = SerialPortSTB.ReadExisting();
								if (serial_data.Length > 0)
								{
									packet_data += serial_data;
									serial_data = "";
									if (packet_data.Length > 0)
										do
										{
											length = packet_data.Length;
											EventCallbackCaller(ref packet_data);
										}
										while (length != packet_data.Length && packet_data.Length != 0);
								}
							}
						}
						else
							lock (objectLock)
								CommState = CommunicationState.Idle;
					}
					catch (UnauthorizedAccessException ex)
					{
						STBLogger.AddEvent(this, STBLogger.EventLevel.Error, "UnauthorizedAccessException while reading port.", ex.Message + ex.Source + ex.StackTrace);
						lock (objectLock)
							CommState = CommunicationState.Idle;
						CommCallbackCaller(CommEvent.CommLost);
					}
					catch (InvalidOperationException exception2)
					{
						STBLogger.AddEvent(this, STBLogger.EventLevel.Error, "The port was closed", exception2.Message + exception2.Source + exception2.StackTrace);
						lock (objectLock)
							CommState = CommunicationState.Idle;
						CommCallbackCaller(CommEvent.CommLost);
					}
					Thread.Sleep(1);
				}

				if (CommState == CommunicationState.Bootloader)
					Thread.Sleep(5);

				if (CommState == CommunicationState.Idle || CommState == CommunicationState.Invalid)
				{
					try
					{
						if (SerialPortSTB.IsOpen)
							SerialPortSTB.Close();
					}
					catch (UnauthorizedAccessException ex)
					{
						STBLogger.AddEvent(this, STBLogger.EventLevel.Error, "UnauthorizedAccessException while closing port.", ex.Message + ex.Source + ex.StackTrace);
					}
					catch (Exception ex)
					{
						STBLogger.AddEvent(this, STBLogger.EventLevel.Error, ToString() + " caused exception " + ex.Message, ex.Message + ex.Source + ex.StackTrace);
					}

					if (AbortThread.WaitOne(500, false))
						return;
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
        public Result BufferFlush()
        {
            return Result.eERR_NOT_IMPLEMENTED;
        }

		/// <summary>
		/// Close
		/// </summary>
		/// <returns></returns>
        public Result Close()
        {
            lock (objectLock)
                CommState = CommunicationState.Idle;
            return Result.eERR_SUCCESS;
        }

		/// <summary>
		/// End
		/// </summary>
        public void End()
        {
            lock (objectLock)
                CommState = CommunicationState.Idle;

            AbortThread.Set();
            while (_readThread.ThreadState == ThreadState.Running)
                Thread.Sleep(1);
        }

		/// <summary>
		/// ExecuteFromHostTransaction
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
        public Result ExecuteFromHostTransaction(byte[] data)
        {
            Result result = Result.eERR_INITIAL_STATE;
			if ((CommState == CommunicationState.Ready) && SerialPortSTB.IsOpen)
			{
				result = Result.eERR_SUCCESS;
				try
				{
					SerialPortSTB.BaseStream.Flush();
					SerialPortSTB.Write(data, 0, data.Length);
					SerialPortSTB.BaseStream.Flush();
				}
				catch (InvalidOperationException exception)
				{
					STBLogger.AddEvent(this, STBLogger.EventLevel.Warning, "Transaction requested with port closed", exception.Message + exception.Source + exception.StackTrace);
					result = Result.eERR_OTHER_ERROR;
				}
				catch (UnauthorizedAccessException exception2)
				{
					STBLogger.AddEvent(this, STBLogger.EventLevel.Warning, "The port was closed", exception2.Message + exception2.Source + exception2.StackTrace);
					result = Result.eERR_NOT_READY;
				}
				catch (IOException exception3)
				{
					STBLogger.AddEvent(this, STBLogger.EventLevel.Warning, "The port was closed", exception3.Message + exception3.Source + exception3.StackTrace);
					result = Result.eERR_NOT_READY;
				}
				return result;
			}
			return Result.eERR_NOT_READY;
		}
		/// <summary>
		/// 
		/// </summary>
		~SerialComDriver()
		{
			lock (objectLock)
				CommState = CommunicationState.Idle;
			AbortThread.Set();
			while (_readThread.ThreadState == ThreadState.Running)
			{ }
		}
		/// <summary>
		/// Finds any HW device
		/// </summary>
		/// <returns>Result.eERR_SUCCESS if successfull</returns>
		public Result FindHW()
		{
			Result result = Result.eERR_INITIAL_STATE;
			if (CommState == CommunicationState.Ready)
			{
				STBLogger.AddEvent(ToString(), STBLogger.EventLevel.Information, "Find HW: The comm driver was already initialized.", "Comm Drv CommunicationState:" + CommState.ToString() + " Extended Find HW");
				result = Result.eERR_NOT_READY;
			}
			else if (CommState == CommunicationState.Idle)
			{
				result = Result.eERR_SUCCESS;

				lock (objectLock)
					CommState = CommunicationState.HWFind;

				IsExtendedFindHW = true;
				SetCommStatus("");
			}
			return result;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="msgOut"></param>
		/// <param name="msgIn"></param>
		/// <returns></returns>
		public Result FindHW(string msgOut, string msgIn)
		{
			Result result;
			if (CommState == CommunicationState.Ready)
			{
				STBLogger.AddEvent((object)ToString(), STBLogger.EventLevel.Information, "Find HW: The comm driver was already initialized.", "Comm Drv CommunicationState:" + ((object)CommState).ToString() + " msgOut:" + msgOut);
				result = Result.eERR_NOT_READY;
			}
			else
				result = Result.eERR_SUCCESS;

			if (result == Result.eERR_SUCCESS)
			{
				object obj;
				Monitor.Enter(obj = objectLock);
				try
				{
					CommState = CommunicationState.HWFind;
				}
				finally
				{
					Monitor.Exit(obj);
				}
				FindHWmsgOut = msgOut;
				FindHWmsgIn = msgIn;
				IsExtendedFindHW = false;
				SetCommStatus("");
			}
			return result;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public CommunicationState GetCommCommunicationState()
		{
			return CommState;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="port"></param>
		public override void GetCommObj(ref SerialPort port)
		{
			port = SerialPortSTB;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public CommunicationState GetCommState()
		{
			return CommState;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public string GetCommStatus()
		{
			return ID;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public string GetCommString()
		{
			return string.Concat(new object[] { SerialPortSTB.PortName, " ", SerialPortSTB.BaudRate, " ", SerialPortSTB.DataBits, " ", SerialPortSTB.Parity, " ", SerialPortSTB.StopBits, " ", SerialPortSTB.Handshake });
		}
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public SerialPort_Corrected GetSerialPort()
		{
			return SerialPortSTB;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="par1"></param>
		/// <returns></returns>
		public Result Open(string[] par1)
		{
			Result result = Result.eERR_INITIAL_STATE;
			try
			{
				SerialPortSTB.PortName = SetPortName(par1[0]);
				SerialPortSTB.BaudRate = SetPortBaudRate(Convert.ToInt32(par1[1]));
				SerialPortSTB.Parity = SetPortParity((Parity)Convert.ToInt32(par1[2]));
				SerialPortSTB.DataBits = SetPortDataBits(Convert.ToInt32(par1[3]));
				SerialPortSTB.StopBits = SetPortStopBits((StopBits)Convert.ToInt32(par1[4]));
				SerialPortSTB.Handshake = SetPortHandshake((Handshake)Convert.ToInt32(par1[5]));
				SerialPortSTB.Encoding = new ASCIIEncoding();
				SerialPortSTB.ReadTimeout = 150;
				SerialPortSTB.WriteTimeout = 500;
				SerialPortSTB.Encoding = Encoding.GetEncoding("Windows-1252");
				SerialPortSTB.Open();
			}
			catch (Exception exception)
			{
				result = Result.eERR_BUSY;
				result = Result.eERR_NOT_READY;
				STBLogger.AddEvent(this, STBLogger.EventLevel.Information, "Open request while port open", exception.Message + exception.Source + exception.StackTrace);
			}
			if (result == Result.eERR_INITIAL_STATE)
			{
				result = Result.eERR_SUCCESS;
				lock (objectLock)
				{
					CommState = CommunicationState.Ready;
					CommCallbackCaller(CommEvent.CommOpen);
				}
			}
			return result;
		}
		/// <summary>
		/// 
		/// </summary>
		public void PauseComm()
		{
			OldCommState = CommState;
			oldTimeout = SerialPortSTB.ReadTimeout;
			CommState = CommunicationState.Bootloader;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="comPorts"></param>
		/// <returns></returns>
		public Result PortScan(ref string[] comPorts)
		{
			Result result = Result.eERR_SUCCESS;
			try
			{
				comPorts = SerialPort.GetPortNames();
			}
			catch (Exception exception)
			{
				STBLogger.AddEvent(this, STBLogger.EventLevel.Information, exception.ToString(), exception.Message + exception.Source + exception.StackTrace);
				result = Result.eERR_OTHER_ERROR;
			}
			return result;
		}
		/// <summary>
		/// 
		/// </summary>
		public void RestoreComm()
		{
			SerialPortSTB.ReadTimeout = oldTimeout;
			CommState = OldCommState;
			OldCommState = CommunicationState.Idle;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="prot"></param>
		/// <returns></returns>
		public Result SetActiveProtocol(Protocol prot)
		{
			if (CommState != CommunicationState.HWFind)
			{
				ActiveProtocol = prot;
				return Result.eERR_SUCCESS;
			}
			return Result.eERR_BUSY;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="id"></param>
		public void SetCommStatus(string id)
		{
			int hwid;
			int swid;
			int blid;
			Encoding encoding = Encoding.GetEncoding("Windows-1252");
			if (id.Length == 11)
			{
				hwid = (encoding.GetBytes(id.Substring(2, 1))[0] << 8) | encoding.GetBytes(id.Substring(3, 1))[0];
				swid = (encoding.GetBytes(id.Substring(4, 1))[0] << 8) | encoding.GetBytes(id.Substring(5, 1))[0];
				blid = (encoding.GetBytes(id.Substring(6, 1))[0] << 8) | encoding.GetBytes(id.Substring(7, 1))[0];
				ID = string.Format("HW:{0:x4} SW:{1:x4} BL:{2:x4}", hwid, swid, blid);
			}
			else if (id.Length == 8 || id.Length == 6)
			{	//	"HW:3006 SW:4003 BL:4002"
				hwid = (encoding.GetBytes(id.Substring(0, 1))[0] << 8) | encoding.GetBytes(id.Substring(1, 1))[0];
				swid = (encoding.GetBytes(id.Substring(2, 1))[0] << 8) | encoding.GetBytes(id.Substring(3, 1))[0];
				blid = (encoding.GetBytes(id.Substring(4, 1))[0] << 8) | encoding.GetBytes(id.Substring(5, 1))[0];
				ID = string.Format("HW:{0:x4} SW:{1:x4} BL:{2:x4}", hwid, swid, blid);
			}
			else
				ID = id;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="newConnectionString"></param>
		/// <returns></returns>
		public Result SetConnectionString(string newConnectionString)
		{
			if (CommState != CommunicationState.HWFind)
			{
				ConnectionString = newConnectionString.Split(new string[] { "," }, 6, StringSplitOptions.None);
				return Result.eERR_SUCCESS;
			}
			return Result.eERR_BUSY;
		}
		/// <summary>
		/// 
		/// </summary>
		public Thread ReadThread
		{
			get { return _readThread; }
		}

		#region ExtendedFindHW 
		private Result ExtendedFindHW()
		{
			string[] comPorts = new string[0];
			string str = "";
			Result result = Result.eERR_INITIAL_STATE;
			byte[] outMsg = new byte[0];
			byte[] numArray = new byte[0];
			byte[] decodedMsg = new byte[0];
			SerialPort_Corrected serialPortCorrected = SerialPortSTB;
			lock(serialPortCorrected)
			{
				if (CommState == CommunicationState.Ready)
				{
					STBLogger.AddEvent((object)ToString(),
						STBLogger.EventLevel.Information,
						"Find HW: The comm driver was already initialized.",
						"Comm Drv CommunicationState:" + ((object)CommState).ToString() + " msgOut:" + FindHWmsgOut
						);
					result = Result.eERR_NOT_READY;
				}
				else
				{
					try
					{
						SerialPortSTB.Close();
						Thread.Sleep(1);
						SerialPortSTB = new SerialPort_Corrected();
						SerialPortSTB.BaudRate = SerialComDriver.SetPortBaudRate(Convert.ToInt32(ConnectionString[1]));
						SerialPortSTB.Parity = SerialComDriver.SetPortParity((Parity)Convert.ToInt32(ConnectionString[2]));
						SerialPortSTB.DataBits = SerialComDriver.SetPortDataBits(Convert.ToInt32(ConnectionString[3]));
						SerialPortSTB.StopBits = SerialComDriver.SetPortStopBits((StopBits)Convert.ToInt32(ConnectionString[4]));
						SerialPortSTB.Handshake = SerialComDriver.SetPortHandshake((Handshake)Convert.ToInt32(ConnectionString[5]));
						SerialPortSTB.Encoding = (Encoding)new ASCIIEncoding();
						SerialPortSTB.ReadTimeout = 150;
						SerialPortSTB.WriteTimeout = 500;
						SerialPortSTB.Encoding = Encoding.GetEncoding("Windows-1252");
					}
					catch (Exception ex)
					{
						STBLogger.AddEvent((object)this, STBLogger.EventLevel.Information, ex.ToString(), ex.Message + ex.Source + ex.StackTrace);
					}
				}

				if (result == Result.eERR_INITIAL_STATE)
				{
					PortScan(ref comPorts);
					for (int comIdx = 0; comIdx < comPorts.Length; ++comIdx)
					{
						try
						{
							int numPotocols = ActiveProtocol.NumProtocols();
							for (int protocolIdx = 0; protocolIdx < numPotocols; ++protocolIdx)
							{
								ActiveProtocol.SetProtocol((ProtocolId)protocolIdx);
								SerialPortSTB.PortName = comPorts[comIdx];
								SerialPortSTB.BaudRate = Convert.ToInt32(ActiveProtocol.ComplexCommand(ComplexMessage.Baudrate));

								SerialPortSTB.Open();
								SetCommStatus("Scanning port: " + comPorts[comIdx]);
								GC.SuppressFinalize((object)SerialPortSTB.BaseStream);

								SerialPortSTB.Write(ActiveProtocol.ComplexCommand(ComplexMessage.PartialReset));
								SerialPortSTB.Write(ActiveProtocol.ComplexCommand(ComplexMessage.PartialReset));
								SerialPortSTB.Write(ActiveProtocol.ComplexCommand(ComplexMessage.PartialReset));
								SerialPortSTB.Write(ActiveProtocol.ComplexCommand(ComplexMessage.PartialReset));
								SerialPortSTB.Write(ActiveProtocol.ComplexCommand(ComplexMessage.PartialReset));
								Thread.Sleep(60);

								SerialPortSTB.ReadExisting();
								Thread.Sleep(60);

								ActiveProtocol.BuildMessage(OperationId.Identify, new byte[0], ref outMsg);
								SerialPortSTB.Write(outMsg, 0, outMsg.Length);
								Thread.Sleep(60);

								string response = SerialPortSTB.ReadExisting();
								Thread.Sleep(60);
								byte[] bytes = Encoding.GetEncoding("Windows-1252").GetBytes(response);
								result = ActiveProtocol.DecodeMessageAndCheck(bytes, OperationId.Identify, ref decodedMsg);
								if (protocolIdx == 2 && result == Result.eERR_PARAMETER && bytes.Length > 9)
								{
									bytes[9] = (byte)11;
									result = ActiveProtocol.DecodeMessageAndCheck(bytes, OperationId.Identify, ref decodedMsg);
								}
								if (result == Result.eERR_SUCCESS)
								{
									if (protocolIdx == 2)
										ID = string.Format("HW:{0} SW:{1} BL:0000", (object)bytes[6].ToString("X4"), (object)bytes[9].ToString("X4"));
									else
										SetCommStatus(Encoding.GetEncoding("Windows-1252").GetString(decodedMsg));
									result = Result.eERR_SUCCESS;
									break;
								}
								else if (result != Result.eERR_SUCCESS)
								{
									SerialPortSTB.Close();
									str = string.Empty;
								}
								else
									break;
							}
						}
						catch (TimeoutException)
						{
							SerialPortSTB.Close();
						}
						catch (Exception ex1)
						{
							STBLogger.AddEvent((object)this, STBLogger.EventLevel.Information, ex1.ToString(), "Port:" + comPorts[comIdx] + " " + ex1.Message + ex1.Source + ex1.StackTrace);
							try
							{
								SerialPortSTB.Close();
							}
							catch (Exception) { }
						}
					}
					if (Result.eERR_INITIAL_STATE == result)
						result = Result.eERR_OTHER_ERROR;
				}
				if (result == Result.eERR_SUCCESS)
				{
					if (CommState == CommunicationState.HWFind)
					{
						lock(objectLock)
							CommState = CommunicationState.Ready;
						CommCallbackCaller(CommEvent.CommOpen);
					}
				}
				else if (CommState == CommunicationState.HWFind)
				{
					lock(objectLock)
						CommState = CommunicationState.HWFind;
				}
				return result;
			}
		}
		#endregion

		#region NormalFindHW 
		private Result NormalFindHW()
		{
			string[] comPorts = new string[0];
			string str = "";
			Result result = Result.eERR_INITIAL_STATE;
			int retries = 4;
			SerialPort_Corrected serialPortCorrected = SerialPortSTB;
			lock(serialPortCorrected)
			{
				if (CommState == CommunicationState.Ready)
				{
					STBLogger.AddEvent((object)ToString(), STBLogger.EventLevel.Information, "Find HW: The comm driver was already initialized.", "Comm Drv CommunicationState:" + ((object)CommState).ToString() + " msgOut:" + FindHWmsgOut);
					result = Result.eERR_NOT_READY;
				}
				else
				{
					try
					{
						SerialPortSTB.Close();
						Thread.Sleep(1);
						SerialPortSTB = new SerialPort_Corrected();
						SerialPortSTB.BaudRate = SerialComDriver.SetPortBaudRate(Convert.ToInt32(ConnectionString[1]));
						SerialPortSTB.Parity = SerialComDriver.SetPortParity((Parity)Convert.ToInt32(ConnectionString[2]));
						SerialPortSTB.DataBits = SerialComDriver.SetPortDataBits(Convert.ToInt32(ConnectionString[3]));
						SerialPortSTB.StopBits = SerialComDriver.SetPortStopBits((StopBits)Convert.ToInt32(ConnectionString[4]));
						SerialPortSTB.Handshake = SerialComDriver.SetPortHandshake((Handshake)Convert.ToInt32(ConnectionString[5]));
						SerialPortSTB.Encoding = (Encoding)new ASCIIEncoding();
						SerialPortSTB.ReadTimeout = 150;
						SerialPortSTB.WriteTimeout = 500;
						SerialPortSTB.Encoding = Encoding.GetEncoding("Windows-1252");
					}
					catch (Exception ex)
					{
						STBLogger.AddEvent((object)this, STBLogger.EventLevel.Information, ex.ToString(), ex.Message + ex.Source + ex.StackTrace);
					}
				}
				if (result == Result.eERR_INITIAL_STATE)
				{
					PortScan(ref comPorts);
					for (int comIdx = 0; comIdx < comPorts.Length; ++comIdx)
					{
						try
						{
							SerialPortSTB.PortName = comPorts[comIdx];
							SetCommStatus("Scanning port: " + comPorts[comIdx]);
							SerialPortSTB.Open();
							GC.SuppressFinalize((object)SerialPortSTB.BaseStream);
							SerialPortSTB.Write(" v\x0001\x0016\x00AD\r");
							Thread.Sleep(60);

							SerialPortSTB.ReadExisting();
							Thread.Sleep(60);

							for (int retry = 0; retry < retries; retry++)
							{
								SerialPortSTB.Write(FindHWmsgOut);
								Thread.Sleep(60);

								string id = SerialPortSTB.ReadExisting();
								Thread.Sleep(60);

								if (id.Length > 0)
								{
									if (id.IndexOf(FindHWmsgIn) != -1)
									{
										SetCommStatus(id);
										result = Result.eERR_SUCCESS;
										break;
									}
									else
										STBLogger.AddEvent((object)this, STBLogger.EventLevel.Information, "Find HW: a different board was found", "Port:" + comPorts[comIdx] + " Message:" + string.Format(" 0x{0:x2} 0x{1:x2} 0x{2:x2} 0x{3:x2} 0x{4:x2} 0x{5:x2} 0x{6:x2}", (object)Convert.ToInt16(id[0]), (object)Convert.ToInt16(id[1]), (object)Convert.ToInt16(id[2]), (object)Convert.ToInt16(id[3]), (object)Convert.ToInt16(id[4]), (object)Convert.ToInt16(id[5]), (object)Convert.ToInt16(id[6])));
								}
							}
							if (result != Result.eERR_SUCCESS)
							{
								SerialPortSTB.Close();
								str = string.Empty;
							}
							else
								break;
						}
						catch (TimeoutException)
						{
							SerialPortSTB.Close();
						}
						catch (Exception ex1)
						{
							STBLogger.AddEvent((object)this, STBLogger.EventLevel.Information, ex1.ToString(), "Port:" + comPorts[comIdx] + " " + ex1.Message + ex1.Source + ex1.StackTrace);
							try
							{
								SerialPortSTB.Close();
							}
							catch (Exception) { }
						}
					}
					if (Result.eERR_INITIAL_STATE == result)
						result = Result.eERR_OTHER_ERROR;
				}
				if (result == Result.eERR_SUCCESS)
				{
					if (CommState == CommunicationState.HWFind)
					{
						lock(objectLock)
							CommState = CommunicationState.Ready;
						CommCallbackCaller(CommEvent.CommOpen);
					}
				}
				else if (CommState == CommunicationState.HWFind)
				{
					lock(objectLock)
						CommState = CommunicationState.HWFind;
				}
				return result;
			}
		}
		#endregion

		private static int SetPortBaudRate(int defaultPortBaudRate)
        {
            return int.Parse(defaultPortBaudRate.ToString());
        }

        private static int SetPortDataBits(int defaultPortDataBits)
        {
            return int.Parse(defaultPortDataBits.ToString());
        }

        private static Handshake SetPortHandshake(Handshake defaultPortHandshake)
        {
			return defaultPortHandshake;
        }

        private static string SetPortName(string defaultPortName)
        {
            Console.WriteLine("Available Ports:");
            foreach (string str2 in SerialPort.GetPortNames())
                Console.WriteLine("   {0}", str2);

            Console.Write("COM port({0}): ", defaultPortName);
            return defaultPortName;
        }

        private static Parity SetPortParity(Parity defaultPortParity)
        {
			return defaultPortParity;
        }

        private static StopBits SetPortStopBits(StopBits defaultPortStopBits)
        {
			return defaultPortStopBits;
        }

        private void StopComm()
        {
            lock (objectLock)
                CommState = CommunicationState.Idle;
            try
            {
                SerialPortSTB.Close();
            }
            catch (Exception exception)
            {
                STBLogger.AddEvent(this, STBLogger.EventLevel.Information, "SerialCommDriver.StopComm - Exception", exception.ToString() + exception.Message + exception.Source + exception.StackTrace);
            }
        }

        private byte[] ToByteArray(string In)
        {
            return Encoding.GetEncoding("Windows-1252").GetBytes(In);
        }
    }
}

