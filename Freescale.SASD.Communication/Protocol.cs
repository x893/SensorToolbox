﻿namespace Freescale.SASD.Communication
{
	using System;
	using System.Text;

	public class Protocol : CommClass
	{
		private int defaultProtocol = 0;
		private ProtocolDefinition[] SupportedProtocols = new ProtocolDefinition[3];

		/// <summary>
		/// 
		/// </summary>
		public Protocol()
		{
			DefineSTBProtocol(ref SupportedProtocols[0]);
			DefineSTBOldProtocol(ref SupportedProtocols[1]);
			DefineSTBEveProtocol(ref SupportedProtocols[2]);
		}

		/// <summary>
		/// Builds a serial message
		///  </summary>
		///  <param name="msgId"></param>
		///  <param name="payload"></param>
		///  <param name="outMsg"></param>
		///  <returns>Result.eERR_SUCCESS if successfull</returns>
		public Result BuildMessage(OperationId msgId, byte[] payload, ref byte[] outMsg)
		{
			Result result = Result.eERR_INITIAL_STATE;
			ProtocolDefinition thisProtocol = SupportedProtocols[defaultProtocol];
			Message msg = new Message();
			result = GetMessageFromHost(thisProtocol, msgId, ref msg);
			if (result == Result.eERR_SUCCESS)
			{
				result = Result.eERR_INITIAL_STATE;
				if (IsPayloadLengthOk(msg, payload))
				{
					int num;
					result = Result.eERR_SUCCESS;
					outMsg = new byte[(msg.header.Length + payload.Length) + msg.footer.Length];

					for (num = 0; num < msg.header.Length; num++)
						outMsg[num] = msg.header[num];
					for (num = 0; num < payload.Length; num++)
						outMsg[num + msg.header.Length] = payload[num];
					for (num = 0; num < msg.footer.Length; num++)
						outMsg[num + msg.header.Length + payload.Length] = msg.footer[num];
				}
				else
					result = Result.eERR_PARAMETER;
			}
			if (result != Result.eERR_SUCCESS)
				outMsg = new byte[0];
			return result;
		}

		/// <summary>
		/// Checks message header and updates serial buffer
		/// </summary>
		/// <param name="buff"></param>
		/// <param name="currMsg"></param>
		public void CheckHeaderAndUpdateBuffer(ref string buff, ref Message currMsg)
		{
			ProtocolDefinition definition = SupportedProtocols[defaultProtocol];
			Encoding encoding = Encoding.GetEncoding("Windows-1252");
			string str = string.Empty;
			int startIndex = 0x7fffffff;
			for (int i = 0; i < definition.Commands.Length; i++)
				if (definition.Commands[i].Request.header != null)
				{
					if (definition.Commands[i].Request.type == MessageType.ToHost)
						currMsg = definition.Commands[i].Request;
					else
					{
						if (definition.Commands[i].Response.type != MessageType.ToHost)
							continue;
						currMsg = definition.Commands[i].Response;
					}
					str = encoding.GetString(currMsg.header);
					int index = buff.IndexOf(str);
					index = (index == -1) ? 0x7fffffff : index;
					if (index < startIndex)
						startIndex = index;
					if (startIndex == 0)
						break;
				}
			if (startIndex == 0x7fffffff)
			{
				currMsg.Id = OperationId.Invalid;
				buff = string.Empty;
			}
			else
			{
				buff = buff.Substring(startIndex, buff.Length - startIndex);
			}
		}

		/// <summary>
		/// Builds a complex command
		/// </summary>
		/// <param name="request"></param>
		/// <returns>Result.eERR_SUCCESS if successfull</returns>
		public string ComplexCommand(ComplexMessage request)
		{
			string str = string.Empty;
			if (defaultProtocol == 0)
			{
				if (request == ComplexMessage.Baudrate)
					str = "115200";
				else if (request == ComplexMessage.PartialReset)
					str = " v\x0001\x0016\x00ad\r";
				else if (request == ComplexMessage.FullReset)
					str = " v\x0001\x0016\x00a5\r";
			}
			else if (defaultProtocol == 1)
			{
				if (request == ComplexMessage.Baudrate)
					str = "38400";
				else if (request == ComplexMessage.PartialReset)
					str = "h";
				else if (request == ComplexMessage.FullReset)
					str = "";
			}
			else if (defaultProtocol == 2)
			{
				if (request == ComplexMessage.Baudrate)
					str = "460800";
				else if (request == ComplexMessage.PartialReset)
					str = " h\x0004\r";
				else if (request == ComplexMessage.FullReset)
					str = " h\x0004\r";
			}
			return str;
		}

		/// <summary>
		/// Decodes a serial message
		/// </summary>
		/// <param name="inputMsg"></param>
		/// <param name="msg"></param>
		/// <param name="decodedMsg"></param>
		/// <returns>Result.eERR_SUCCESS if successfull</returns>
		public Result DecodeMessage(byte[] inputMsg, ref Message msg, ref byte[] decodedMsg)
		{
			Result result = Result.eERR_INITIAL_STATE;
			ProtocolDefinition definition = SupportedProtocols[defaultProtocol];
			if (result == Result.eERR_INITIAL_STATE)
			{
				result = Result.eERR_INITIAL_STATE;
				if (IsLengthOk(msg, inputMsg) && IsFooterOk(msg, inputMsg))
				{
					result = Result.eERR_SUCCESS;
					int num = inputMsg.Length - (msg.header.Length + msg.footer.Length);
					decodedMsg = new byte[num];
					for (int i = 0; i < num; i++)
						decodedMsg[i] = inputMsg[i + msg.header.Length];
				}
				else
					result = Result.eERR_PARAMETER;
			}
			if (result != Result.eERR_SUCCESS)
				decodedMsg = new byte[0];
			return result;
		}

		/// <summary>
		/// Decodes and checks if a serial message is valid
		/// </summary>
		/// <param name="inputMsg"></param>
		/// <param name="oid"></param>
		/// <param name="decodedMsg"></param>
		/// <returns>Result.eERR_SUCCESS if successfull</returns>
		public Result DecodeMessageAndCheck(byte[] inputMsg, OperationId oid, ref byte[] decodedMsg)
		{
			Result result = Result.eERR_INITIAL_STATE;
			ProtocolDefinition prot = SupportedProtocols[defaultProtocol];
			Message msg = new Message();
			result = GetMessageToHost(prot, inputMsg, ref msg);
			if (result == Result.eERR_SUCCESS)
			{
				result = Result.eERR_INITIAL_STATE;
				if (IsLengthOk(msg, inputMsg) && IsFooterOk(msg, inputMsg))
				{
					result = Result.eERR_SUCCESS;
					int num = inputMsg.Length - (msg.header.Length + msg.footer.Length);
					decodedMsg = new byte[num];
					for (int i = 0; i < num; i++)
						decodedMsg[i] = inputMsg[i + msg.header.Length];
				}
				else
					result = Result.eERR_PARAMETER;
			}
			if (result != Result.eERR_SUCCESS)
				decodedMsg = new byte[0];
			return result;
		}

		private void DefineSTBEveProtocol(ref ProtocolDefinition stbProtocol)
		{
			stbProtocol.Commands = new Transaction[2];

			#region Handshake
			stbProtocol.Commands[0].Id = OperationId.Handshake;
			stbProtocol.Commands[0].isSupported = true;
			stbProtocol.Commands[0].type = TransactionType.Synchronous;
			stbProtocol.Commands[0].Request.header = new byte[] { 0x20, 0x68, 4 };
			stbProtocol.Commands[0].Request.footer = ToByteArray("\r");
			stbProtocol.Commands[0].Request.isVariableLength = false;
			stbProtocol.Commands[0].Request.length = 4;
			stbProtocol.Commands[0].Request.lengthPosition = -1;
			stbProtocol.Commands[0].Response.header = new byte[] { 0, 0x20, 0x48, 4 };
			stbProtocol.Commands[0].Response.footer = ToByteArray("\r");
			stbProtocol.Commands[0].Response.length = 5;
			stbProtocol.Commands[0].Response.isVariableLength = false;
			stbProtocol.Commands[0].Response.lengthPosition = -1;
			#endregion
			#region Identify
			stbProtocol.Commands[1].Id = OperationId.Identify;
			stbProtocol.Commands[1].isSupported = true;
			stbProtocol.Commands[1].type = TransactionType.Synchronous;
			stbProtocol.Commands[1].Request.header = new byte[] { 0x20, 0x69, 4 };
			stbProtocol.Commands[1].Request.footer = ToByteArray("\r");
			stbProtocol.Commands[1].Request.isVariableLength = false;
			stbProtocol.Commands[1].Request.length = 4;
			stbProtocol.Commands[1].Request.lengthPosition = -1;
			stbProtocol.Commands[1].Response.header = new byte[] { 0, 0x20, 0x49, 10, 0x48, 0, 3, 0x53, 0, 11 };
			stbProtocol.Commands[1].Response.footer = ToByteArray("\r");
			stbProtocol.Commands[1].Response.length = 11;
			stbProtocol.Commands[1].Response.isVariableLength = false;
			stbProtocol.Commands[1].Response.lengthPosition = -1;
			#endregion

			for (int i = 0; i < stbProtocol.Commands.Length; i++)
			{
				stbProtocol.Commands[i].Request.Id = stbProtocol.Commands[i].Id;
				stbProtocol.Commands[i].Response.Id = stbProtocol.Commands[i].Id;
				if (stbProtocol.Commands[i].type == TransactionType.Synchronous)
				{
					stbProtocol.Commands[i].Request.type = MessageType.FromHost;
					stbProtocol.Commands[i].Response.type = MessageType.ToHost;
				}
				else
				{
					stbProtocol.Commands[i].Request.type = MessageType.ToHost;
					stbProtocol.Commands[i].Response.type = MessageType.FromHost;
				}
			}
		}

		private void DefineSTBOldProtocol(ref ProtocolDefinition stbProtocol)
		{
			stbProtocol.Commands = new Transaction[13];

			#region Handshake
			stbProtocol.Commands[0].Id = OperationId.Handshake;
			stbProtocol.Commands[0].isSupported = true;
			stbProtocol.Commands[0].type = TransactionType.Synchronous;
			stbProtocol.Commands[0].Request.header = ToByteArray("h");
			stbProtocol.Commands[0].Request.footer = ToByteArray("");
			stbProtocol.Commands[0].Request.isVariableLength = false;
			stbProtocol.Commands[0].Request.length = 1;
			stbProtocol.Commands[0].Request.lengthPosition = -1;
			stbProtocol.Commands[0].Response.header = ToByteArray("H");
			stbProtocol.Commands[0].Response.footer = ToByteArray("");
			stbProtocol.Commands[0].Response.length = 1;
			stbProtocol.Commands[0].Response.isVariableLength = false;
			stbProtocol.Commands[0].Response.lengthPosition = -1;
			#endregion
			#region Identify
			stbProtocol.Commands[1].Id = OperationId.Identify;
			stbProtocol.Commands[1].isSupported = true;
			stbProtocol.Commands[1].type = TransactionType.Synchronous;
			stbProtocol.Commands[1].Request.Id = OperationId.Identify;
			stbProtocol.Commands[1].Request.header = ToByteArray("i");
			stbProtocol.Commands[1].Request.footer = ToByteArray("");
			stbProtocol.Commands[1].Request.isVariableLength = false;
			stbProtocol.Commands[1].Request.length = 1;
			stbProtocol.Commands[1].Request.lengthPosition = -1;
			stbProtocol.Commands[1].Response.Id = OperationId.Identify;
			stbProtocol.Commands[1].Response.type = MessageType.None;
			stbProtocol.Commands[1].Response.header = ToByteArray("");
			stbProtocol.Commands[1].Response.footer = ToByteArray("I");
			stbProtocol.Commands[1].Response.length = 7;
			stbProtocol.Commands[1].Response.isVariableLength = false;
			stbProtocol.Commands[1].Response.lengthPosition = -1;
			#endregion
			#region AppRegisterRead
			stbProtocol.Commands[2].Id = OperationId.AppRegisterRead;
			stbProtocol.Commands[2].isSupported = false;
			stbProtocol.Commands[2].type = TransactionType.Synchronous;
			stbProtocol.Commands[2].Request.header = ToByteArray(" u");
			stbProtocol.Commands[2].Request.footer = ToByteArray("\r");
			stbProtocol.Commands[2].Request.isVariableLength = false;
			stbProtocol.Commands[2].Request.length = 5;
			stbProtocol.Commands[2].Request.lengthPosition = -1;
			stbProtocol.Commands[2].Response.header = ToByteArray(" U");
			stbProtocol.Commands[2].Response.footer = ToByteArray("\r");
			stbProtocol.Commands[2].Response.length = -1;
			stbProtocol.Commands[2].Response.isVariableLength = true;
			stbProtocol.Commands[2].Response.lengthPosition = 2;
			stbProtocol.Commands[2].Response.lengthConstant = 2;
			#endregion
			#region AppRegisterWrite
			stbProtocol.Commands[3].Id = OperationId.AppRegisterWrite;
			stbProtocol.Commands[3].isSupported = false;
			stbProtocol.Commands[3].type = TransactionType.Synchronous;
			stbProtocol.Commands[3].Request.header = ToByteArray(" v");
			stbProtocol.Commands[3].Request.footer = ToByteArray("\r");
			stbProtocol.Commands[3].Request.isVariableLength = true;
			stbProtocol.Commands[3].Request.length = -1;
			stbProtocol.Commands[3].Request.lengthPosition = 2;
			stbProtocol.Commands[3].Response.lengthConstant = 2;
			stbProtocol.Commands[3].Response.header = ToByteArray(" V");
			stbProtocol.Commands[3].Response.footer = ToByteArray("\r");
			stbProtocol.Commands[3].Response.length = 3;
			stbProtocol.Commands[3].Response.isVariableLength = false;
			stbProtocol.Commands[3].Response.lengthPosition = -1;
			#endregion
			#region DeviceRead
			stbProtocol.Commands[4].Id = OperationId.DeviceRead;
			stbProtocol.Commands[4].isSupported = true;
			stbProtocol.Commands[4].type = TransactionType.Synchronous;
			stbProtocol.Commands[4].Request.header = ToByteArray(" l");
			stbProtocol.Commands[4].Request.footer = ToByteArray("\r");
			stbProtocol.Commands[4].Request.isVariableLength = false;
			stbProtocol.Commands[4].Request.length = 5;
			stbProtocol.Commands[4].Request.lengthPosition = -1;
			stbProtocol.Commands[4].Response.header = ToByteArray(" L");
			stbProtocol.Commands[4].Response.footer = ToByteArray("\r");
			stbProtocol.Commands[4].Response.length = -1;
			stbProtocol.Commands[4].Response.isVariableLength = true;
			stbProtocol.Commands[4].Response.lengthPosition = 2;
			stbProtocol.Commands[4].Response.lengthConstant = 2;
			#endregion
			#region DeviceWrite
			stbProtocol.Commands[5].Id = OperationId.DeviceWrite;
			stbProtocol.Commands[5].isSupported = true;
			stbProtocol.Commands[5].type = TransactionType.Synchronous;
			stbProtocol.Commands[5].Request.header = ToByteArray(" m");
			stbProtocol.Commands[5].Request.footer = ToByteArray("\r");
			stbProtocol.Commands[5].Request.isVariableLength = true;
			stbProtocol.Commands[5].Request.length = -1;
			stbProtocol.Commands[5].Request.lengthPosition = 2;
			stbProtocol.Commands[5].Request.lengthPosition = 2;
			stbProtocol.Commands[5].Response.header = ToByteArray(" M");
			stbProtocol.Commands[5].Response.footer = ToByteArray("\r");
			stbProtocol.Commands[5].Response.length = 3;
			stbProtocol.Commands[5].Response.isVariableLength = false;
			stbProtocol.Commands[5].Response.lengthPosition = -1;
			#endregion
			#region NVMErase
			stbProtocol.Commands[6].Id = OperationId.NVMErase;
			stbProtocol.Commands[6].isSupported = true;
			stbProtocol.Commands[6].type = TransactionType.Synchronous;
			stbProtocol.Commands[6].Request.Id = OperationId.NVMErase;
			stbProtocol.Commands[6].Request.header = ToByteArray("e");
			stbProtocol.Commands[6].Request.footer = ToByteArray("");
			stbProtocol.Commands[6].Request.isVariableLength = false;
			stbProtocol.Commands[6].Request.length = 1;
			stbProtocol.Commands[6].Request.lengthPosition = -1;
			stbProtocol.Commands[6].Response.Id = OperationId.NVMErase;
			stbProtocol.Commands[6].Response.header = ToByteArray("E");
			stbProtocol.Commands[6].Response.footer = ToByteArray("");
			stbProtocol.Commands[6].Response.length = 1;
			stbProtocol.Commands[6].Response.isVariableLength = false;
			stbProtocol.Commands[6].Response.lengthPosition = -1;
			#endregion
			#region NVMWriteData
			stbProtocol.Commands[7].Id = OperationId.NVMWriteData;
			stbProtocol.Commands[7].isSupported = true;
			stbProtocol.Commands[7].type = TransactionType.Synchronous;
			stbProtocol.Commands[7].Request.Id = OperationId.NVMWriteData;
			stbProtocol.Commands[7].Request.header = ToByteArray("s");
			stbProtocol.Commands[7].Request.footer = ToByteArray("");
			stbProtocol.Commands[7].Request.isVariableLength = false;
			stbProtocol.Commands[7].Request.length = 1;
			stbProtocol.Commands[7].Request.lengthPosition = -1;
			stbProtocol.Commands[7].Response.Id = OperationId.NVMWriteData;
			stbProtocol.Commands[7].Response.type = MessageType.None;
			stbProtocol.Commands[7].Response.header = ToByteArray("S");
			stbProtocol.Commands[7].Response.footer = ToByteArray("");
			stbProtocol.Commands[7].Response.length = 1;
			stbProtocol.Commands[7].Response.isVariableLength = false;
			stbProtocol.Commands[7].Response.lengthPosition = -1;
			#endregion
			#region NVMReadData
			stbProtocol.Commands[8].Id = OperationId.NVMReadData;
			stbProtocol.Commands[8].isSupported = true;
			stbProtocol.Commands[8].type = TransactionType.Synchronous;
			stbProtocol.Commands[8].Request.Id = OperationId.NVMReadData;
			stbProtocol.Commands[8].Request.header = ToByteArray("g");
			stbProtocol.Commands[8].Request.footer = ToByteArray("");
			stbProtocol.Commands[8].Request.isVariableLength = false;
			stbProtocol.Commands[8].Request.length = 1;
			stbProtocol.Commands[8].Request.lengthPosition = -1;
			stbProtocol.Commands[8].Response.Id = OperationId.NVMReadData;
			stbProtocol.Commands[8].Response.header = ToByteArray("G");
			stbProtocol.Commands[8].Response.footer = ToByteArray("");
			stbProtocol.Commands[8].Response.length = -1;
			stbProtocol.Commands[8].Response.isVariableLength = true;
			stbProtocol.Commands[8].Response.lengthPosition = 1;
			stbProtocol.Commands[8].Response.lengthConstant = 2;
			#endregion
			#region NVMIsErased
			stbProtocol.Commands[9].Id = OperationId.NVMIsErased;
			stbProtocol.Commands[9].isSupported = true;
			stbProtocol.Commands[9].type = TransactionType.Synchronous;
			stbProtocol.Commands[9].Request.Id = OperationId.NVMIsErased;
			stbProtocol.Commands[9].Request.header = ToByteArray("q");
			stbProtocol.Commands[9].Request.footer = ToByteArray("");
			stbProtocol.Commands[9].Request.isVariableLength = false;
			stbProtocol.Commands[9].Request.length = 1;
			stbProtocol.Commands[9].Request.lengthPosition = -1;
			stbProtocol.Commands[9].Response.Id = OperationId.NVMIsErased;
			stbProtocol.Commands[9].Response.header = ToByteArray("Q");
			stbProtocol.Commands[9].Response.footer = ToByteArray("");
			stbProtocol.Commands[9].Response.length = 2;
			stbProtocol.Commands[9].Response.isVariableLength = false;
			stbProtocol.Commands[9].Response.lengthPosition = -1;
			#endregion
			#region NVMConfig
			stbProtocol.Commands[10].Id = OperationId.NVMConfig;
			stbProtocol.Commands[10].isSupported = true;
			stbProtocol.Commands[10].type = TransactionType.Synchronous;
			stbProtocol.Commands[10].Request.Id = OperationId.NVMConfig;
			stbProtocol.Commands[10].Request.header = ToByteArray("t");
			stbProtocol.Commands[10].Request.footer = ToByteArray("");
			stbProtocol.Commands[10].Request.isVariableLength = false;
			stbProtocol.Commands[10].Request.length = 7;
			stbProtocol.Commands[10].Request.lengthPosition = -1;
			stbProtocol.Commands[10].Response.Id = OperationId.NVMConfig;
			stbProtocol.Commands[10].Response.header = ToByteArray("T");
			stbProtocol.Commands[10].Response.footer = ToByteArray("");
			stbProtocol.Commands[10].Response.length = 1;
			stbProtocol.Commands[10].Response.isVariableLength = false;
			stbProtocol.Commands[10].Response.lengthPosition = -1;
			#endregion
			#region NVMGetConstants
			stbProtocol.Commands[11].Id = OperationId.NVMGetConstants;
			stbProtocol.Commands[11].isSupported = true;
			stbProtocol.Commands[11].type = TransactionType.Synchronous;
			stbProtocol.Commands[11].Request.Id = OperationId.NVMGetConstants;
			stbProtocol.Commands[11].Request.header = ToByteArray("c");
			stbProtocol.Commands[11].Request.footer = ToByteArray("");
			stbProtocol.Commands[11].Request.isVariableLength = false;
			stbProtocol.Commands[11].Request.length = 1;
			stbProtocol.Commands[11].Request.lengthPosition = -1;
			stbProtocol.Commands[11].Response.Id = OperationId.NVMGetConstants;
			stbProtocol.Commands[11].Response.header = ToByteArray("C");
			stbProtocol.Commands[11].Response.footer = ToByteArray("");
			stbProtocol.Commands[11].Response.length = 0x2a;
			stbProtocol.Commands[11].Response.isVariableLength = false;
			stbProtocol.Commands[11].Response.lengthPosition = -1;
			#endregion
			#region NVMBytesWritten
			stbProtocol.Commands[12].Id = OperationId.NVMBytesWritten;
			stbProtocol.Commands[12].isSupported = true;
			stbProtocol.Commands[12].type = TransactionType.Synchronous;
			stbProtocol.Commands[12].Request.Id = OperationId.NVMBytesWritten;
			stbProtocol.Commands[12].Request.header = ToByteArray("n");
			stbProtocol.Commands[12].Request.footer = ToByteArray("");
			stbProtocol.Commands[12].Request.isVariableLength = false;
			stbProtocol.Commands[12].Request.length = 1;
			stbProtocol.Commands[12].Request.lengthPosition = -1;
			stbProtocol.Commands[12].Response.Id = OperationId.NVMBytesWritten;
			stbProtocol.Commands[12].Response.header = ToByteArray("N");
			stbProtocol.Commands[12].Response.footer = ToByteArray("");
			stbProtocol.Commands[12].Response.length = 4;
			stbProtocol.Commands[12].Response.isVariableLength = false;
			stbProtocol.Commands[12].Response.lengthPosition = -1;
			#endregion

			for (int i = 0; i < stbProtocol.Commands.Length; i++)
			{
				stbProtocol.Commands[i].Request.Id = stbProtocol.Commands[i].Id;
				if (stbProtocol.Commands[i].type == TransactionType.Synchronous)
				{
					stbProtocol.Commands[i].Request.type = MessageType.FromHost;
					stbProtocol.Commands[i].Response.type = MessageType.ToHost;
				}
				else
				{
					stbProtocol.Commands[i].Request.type = MessageType.ToHost;
					stbProtocol.Commands[i].Response.type = MessageType.FromHost;
				}
			}
		}

		private void DefineSTBProtocol(ref ProtocolDefinition stbProtocol)
		{
			stbProtocol.Commands = new Transaction[16];

			#region Handshake
			stbProtocol.Commands[0].Id = OperationId.Handshake;
			stbProtocol.Commands[0].isSupported = true;
			stbProtocol.Commands[0].type = TransactionType.Synchronous;
			stbProtocol.Commands[0].Request.header = ToByteArray(" h");
			stbProtocol.Commands[0].Request.footer = ToByteArray("\r");
			stbProtocol.Commands[0].Request.isVariableLength = false;
			stbProtocol.Commands[0].Request.length = 3;
			stbProtocol.Commands[0].Request.lengthPosition = -1;
			stbProtocol.Commands[0].Response.header = ToByteArray(" H");
			stbProtocol.Commands[0].Response.footer = ToByteArray("\r");
			stbProtocol.Commands[0].Response.length = 3;
			stbProtocol.Commands[0].Response.isVariableLength = false;
			stbProtocol.Commands[0].Response.lengthPosition = -1;
			#endregion
			#region Identify
			stbProtocol.Commands[1].Id = OperationId.Identify;
			stbProtocol.Commands[1].isSupported = true;
			stbProtocol.Commands[1].type = TransactionType.Synchronous;
			stbProtocol.Commands[1].Request.header = ToByteArray(" i");
			stbProtocol.Commands[1].Request.footer = ToByteArray("\r");
			stbProtocol.Commands[1].Request.isVariableLength = false;
			stbProtocol.Commands[1].Request.length = 3;
			stbProtocol.Commands[1].Request.lengthPosition = -1;
			stbProtocol.Commands[1].Response.header = ToByteArray(" I");
			stbProtocol.Commands[1].Response.footer = ToByteArray("\r");
			stbProtocol.Commands[1].Response.length = 11;
			stbProtocol.Commands[1].Response.isVariableLength = false;
			stbProtocol.Commands[1].Response.lengthPosition = -1;
			#endregion
			#region AppRegisterRead
			stbProtocol.Commands[2].Id = OperationId.AppRegisterRead;
			stbProtocol.Commands[2].isSupported = true;
			stbProtocol.Commands[2].type = TransactionType.Synchronous;
			stbProtocol.Commands[2].Request.header = ToByteArray(" u");
			stbProtocol.Commands[2].Request.footer = ToByteArray("\r");
			stbProtocol.Commands[2].Request.isVariableLength = false;
			stbProtocol.Commands[2].Request.length = 5;
			stbProtocol.Commands[2].Request.lengthPosition = -1;
			stbProtocol.Commands[2].Response.header = ToByteArray(" U");
			stbProtocol.Commands[2].Response.footer = ToByteArray("\r");
			stbProtocol.Commands[2].Response.length = -1;
			stbProtocol.Commands[2].Response.isVariableLength = true;
			stbProtocol.Commands[2].Response.lengthPosition = 2;
			stbProtocol.Commands[2].Response.lengthConstant = 2;
			#endregion
			#region AppRegisterWrite
			stbProtocol.Commands[3].Id = OperationId.AppRegisterWrite;
			stbProtocol.Commands[3].isSupported = true;
			stbProtocol.Commands[3].type = TransactionType.Synchronous;
			stbProtocol.Commands[3].Request.header = ToByteArray(" v");
			stbProtocol.Commands[3].Request.footer = ToByteArray("\r");
			stbProtocol.Commands[3].Request.isVariableLength = true;
			stbProtocol.Commands[3].Request.length = -1;
			stbProtocol.Commands[3].Request.lengthPosition = 2;
			stbProtocol.Commands[3].Response.lengthConstant = 2;
			stbProtocol.Commands[3].Response.header = ToByteArray(" V");
			stbProtocol.Commands[3].Response.footer = ToByteArray("\r");
			stbProtocol.Commands[3].Response.length = 3;
			stbProtocol.Commands[3].Response.isVariableLength = false;
			stbProtocol.Commands[3].Response.lengthPosition = -1;
			#endregion
			#region DeviceRead
			stbProtocol.Commands[4].Id = OperationId.DeviceRead;
			stbProtocol.Commands[4].isSupported = true;
			stbProtocol.Commands[4].type = TransactionType.Synchronous;
			stbProtocol.Commands[4].Request.header = ToByteArray(" l");
			stbProtocol.Commands[4].Request.footer = ToByteArray("\r");
			stbProtocol.Commands[4].Request.isVariableLength = false;
			stbProtocol.Commands[4].Request.length = 5;
			stbProtocol.Commands[4].Request.lengthPosition = -1;
			stbProtocol.Commands[4].Response.header = ToByteArray(" L");
			stbProtocol.Commands[4].Response.footer = ToByteArray("\r");
			stbProtocol.Commands[4].Response.length = -1;
			stbProtocol.Commands[4].Response.isVariableLength = true;
			stbProtocol.Commands[4].Response.lengthPosition = 2;
			stbProtocol.Commands[4].Response.lengthConstant = 2;
			#endregion
			#region DeviceWrite
			stbProtocol.Commands[5].Id = OperationId.DeviceWrite;
			stbProtocol.Commands[5].isSupported = true;
			stbProtocol.Commands[5].type = TransactionType.Synchronous;
			stbProtocol.Commands[5].Request.header = ToByteArray(" m");
			stbProtocol.Commands[5].Request.footer = ToByteArray("\r");
			stbProtocol.Commands[5].Request.isVariableLength = true;
			stbProtocol.Commands[5].Request.length = -1;
			stbProtocol.Commands[5].Request.lengthPosition = 2;
			stbProtocol.Commands[5].Request.lengthConstant = 2;
			stbProtocol.Commands[5].Response.header = ToByteArray(" M");
			stbProtocol.Commands[5].Response.footer = ToByteArray("\r");
			stbProtocol.Commands[5].Response.length = 3;
			stbProtocol.Commands[5].Response.isVariableLength = false;
			stbProtocol.Commands[5].Response.lengthPosition = -1;
			#endregion
			#region NVMErase
			stbProtocol.Commands[6].Id = OperationId.NVMErase;
			stbProtocol.Commands[6].isSupported = true;
			stbProtocol.Commands[6].type = TransactionType.Synchronous;
			stbProtocol.Commands[6].Request.Id = OperationId.NVMErase;
			stbProtocol.Commands[6].Request.header = ToByteArray("e");
			stbProtocol.Commands[6].Request.footer = ToByteArray("");
			stbProtocol.Commands[6].Request.isVariableLength = false;
			stbProtocol.Commands[6].Request.length = 1;
			stbProtocol.Commands[6].Request.lengthPosition = -1;
			stbProtocol.Commands[6].Response.Id = OperationId.NVMErase;
			stbProtocol.Commands[6].Response.header = ToByteArray("E");
			stbProtocol.Commands[6].Response.footer = ToByteArray("");
			stbProtocol.Commands[6].Response.length = 1;
			stbProtocol.Commands[6].Response.isVariableLength = false;
			stbProtocol.Commands[6].Response.lengthPosition = -1;
			#endregion
			#region NVMWriteData
			stbProtocol.Commands[7].Id = OperationId.NVMWriteData;
			stbProtocol.Commands[7].isSupported = true;
			stbProtocol.Commands[7].type = TransactionType.Synchronous;
			stbProtocol.Commands[7].Request.Id = OperationId.NVMWriteData;
			stbProtocol.Commands[7].Request.header = ToByteArray("s");
			stbProtocol.Commands[7].Request.footer = ToByteArray("");
			stbProtocol.Commands[7].Request.isVariableLength = false;
			stbProtocol.Commands[7].Request.length = 1;
			stbProtocol.Commands[7].Request.lengthPosition = -1;
			stbProtocol.Commands[7].Response.Id = OperationId.NVMWriteData;
			stbProtocol.Commands[7].Response.type = MessageType.None;
			stbProtocol.Commands[7].Response.header = ToByteArray("S");
			stbProtocol.Commands[7].Response.footer = ToByteArray("");
			stbProtocol.Commands[7].Response.length = 1;
			stbProtocol.Commands[7].Response.isVariableLength = false;
			stbProtocol.Commands[7].Response.lengthPosition = -1;
			#endregion
			#region NVMReadData
			stbProtocol.Commands[8].Id = OperationId.NVMReadData;
			stbProtocol.Commands[8].isSupported = true;
			stbProtocol.Commands[8].type = TransactionType.Synchronous;
			stbProtocol.Commands[8].Request.Id = OperationId.NVMReadData;
			stbProtocol.Commands[8].Request.header = ToByteArray("g");
			stbProtocol.Commands[8].Request.footer = ToByteArray("");
			stbProtocol.Commands[8].Request.isVariableLength = false;
			stbProtocol.Commands[8].Request.length = 1;
			stbProtocol.Commands[8].Request.lengthPosition = -1;
			stbProtocol.Commands[8].Response.Id = OperationId.NVMReadData;
			stbProtocol.Commands[8].Response.header = ToByteArray("G");
			stbProtocol.Commands[8].Response.footer = ToByteArray("");
			stbProtocol.Commands[8].Response.length = -1;
			stbProtocol.Commands[8].Response.isVariableLength = true;
			stbProtocol.Commands[8].Response.lengthPosition = 1;
			stbProtocol.Commands[8].Response.lengthConstant = 2;
			#endregion
			#region NVMIsErased
			stbProtocol.Commands[9].Id = OperationId.NVMIsErased;
			stbProtocol.Commands[9].isSupported = true;
			stbProtocol.Commands[9].type = TransactionType.Synchronous;
			stbProtocol.Commands[9].Request.Id = OperationId.NVMIsErased;
			stbProtocol.Commands[9].Request.header = ToByteArray("q");
			stbProtocol.Commands[9].Request.footer = ToByteArray("");
			stbProtocol.Commands[9].Request.isVariableLength = false;
			stbProtocol.Commands[9].Request.length = 1;
			stbProtocol.Commands[9].Request.lengthPosition = -1;
			stbProtocol.Commands[9].Response.Id = OperationId.NVMIsErased;
			stbProtocol.Commands[9].Response.header = ToByteArray("Q");
			stbProtocol.Commands[9].Response.footer = ToByteArray("");
			stbProtocol.Commands[9].Response.length = 2;
			stbProtocol.Commands[9].Response.isVariableLength = false;
			stbProtocol.Commands[9].Response.lengthPosition = -1;
			#endregion
			#region NVMConfig
			stbProtocol.Commands[10].Id = OperationId.NVMConfig;
			stbProtocol.Commands[10].isSupported = true;
			stbProtocol.Commands[10].type = TransactionType.Synchronous;
			stbProtocol.Commands[10].Request.Id = OperationId.NVMConfig;
			stbProtocol.Commands[10].Request.header = ToByteArray("t");
			stbProtocol.Commands[10].Request.footer = ToByteArray("");
			stbProtocol.Commands[10].Request.isVariableLength = false;
			stbProtocol.Commands[10].Request.length = 7;
			stbProtocol.Commands[10].Request.lengthPosition = -1;
			stbProtocol.Commands[10].Response.Id = OperationId.NVMConfig;
			stbProtocol.Commands[10].Response.header = ToByteArray("T");
			stbProtocol.Commands[10].Response.footer = ToByteArray("");
			stbProtocol.Commands[10].Response.length = 1;
			stbProtocol.Commands[10].Response.isVariableLength = false;
			stbProtocol.Commands[10].Response.lengthPosition = -1;
			#endregion
			#region NVMGetConstants
			stbProtocol.Commands[11].Id = OperationId.NVMGetConstants;
			stbProtocol.Commands[11].isSupported = true;
			stbProtocol.Commands[11].type = TransactionType.Synchronous;
			stbProtocol.Commands[11].Request.Id = OperationId.NVMGetConstants;
			stbProtocol.Commands[11].Request.header = ToByteArray("c");
			stbProtocol.Commands[11].Request.footer = ToByteArray("");
			stbProtocol.Commands[11].Request.isVariableLength = false;
			stbProtocol.Commands[11].Request.length = 1;
			stbProtocol.Commands[11].Request.lengthPosition = -1;
			stbProtocol.Commands[11].Response.Id = OperationId.NVMGetConstants;
			stbProtocol.Commands[11].Response.header = ToByteArray("C");
			stbProtocol.Commands[11].Response.footer = ToByteArray("");
			stbProtocol.Commands[11].Response.length = 0x2a;
			stbProtocol.Commands[11].Response.isVariableLength = false;
			stbProtocol.Commands[11].Response.lengthPosition = -1;
			#endregion
			#region NVMBytesWritten
			stbProtocol.Commands[12].Id = OperationId.NVMBytesWritten;
			stbProtocol.Commands[12].isSupported = true;
			stbProtocol.Commands[12].type = TransactionType.Synchronous;
			stbProtocol.Commands[12].Request.Id = OperationId.NVMBytesWritten;
			stbProtocol.Commands[12].Request.header = ToByteArray("n");
			stbProtocol.Commands[12].Request.footer = ToByteArray("");
			stbProtocol.Commands[12].Request.isVariableLength = false;
			stbProtocol.Commands[12].Request.length = 1;
			stbProtocol.Commands[12].Request.lengthPosition = -1;
			stbProtocol.Commands[12].Response.Id = OperationId.NVMBytesWritten;
			stbProtocol.Commands[12].Response.header = ToByteArray("N");
			stbProtocol.Commands[12].Response.footer = ToByteArray("");
			stbProtocol.Commands[12].Response.length = 4;
			stbProtocol.Commands[12].Response.isVariableLength = false;
			stbProtocol.Commands[12].Response.lengthPosition = -1;
			#endregion
			#region StreamData
			stbProtocol.Commands[13].Id = OperationId.StreamData;
			stbProtocol.Commands[13].isSupported = true;
			stbProtocol.Commands[13].type = TransactionType.Asynchronous;
			stbProtocol.Commands[13].Request.header = ToByteArray(" B");
			stbProtocol.Commands[13].Request.footer = ToByteArray("\r");
			stbProtocol.Commands[13].Request.isVariableLength = true;
			stbProtocol.Commands[13].Request.length = -1;
			stbProtocol.Commands[13].Request.lengthPosition = 2;
			stbProtocol.Commands[13].Request.lengthConstant = 2;
			stbProtocol.Commands[13].Response.header = ToByteArray("");
			stbProtocol.Commands[13].Response.footer = ToByteArray("");
			stbProtocol.Commands[13].Response.length = 0;
			stbProtocol.Commands[13].Response.isVariableLength = false;
			stbProtocol.Commands[13].Response.lengthPosition = -1;
			stbProtocol.Commands[13].Response.lengthConstant = 0;
			#endregion
			#region InterruptData
			stbProtocol.Commands[14].Id = OperationId.InterruptData;
			stbProtocol.Commands[14].isSupported = true;
			stbProtocol.Commands[14].type = TransactionType.Asynchronous;
			stbProtocol.Commands[14].Request.header = ToByteArray(" T");
			stbProtocol.Commands[14].Request.footer = ToByteArray("\r");
			stbProtocol.Commands[14].Request.isVariableLength = true;
			stbProtocol.Commands[14].Request.length = -1;
			stbProtocol.Commands[14].Request.lengthPosition = 2;
			stbProtocol.Commands[14].Request.lengthConstant = 1;
			stbProtocol.Commands[14].Response.header = ToByteArray("");
			stbProtocol.Commands[14].Response.footer = ToByteArray("");
			stbProtocol.Commands[14].Response.length = 0;
			stbProtocol.Commands[14].Response.isVariableLength = false;
			stbProtocol.Commands[14].Response.lengthPosition = -1;
			stbProtocol.Commands[14].Response.lengthConstant = 0;
			#endregion
			#region MultiPurpose
			stbProtocol.Commands[15].Id = OperationId.MultiPurpose;
			stbProtocol.Commands[15].isSupported = true;
			stbProtocol.Commands[15].type = TransactionType.Synchronous;
			stbProtocol.Commands[15].Request.header = ToByteArray(" a");
			stbProtocol.Commands[15].Request.footer = ToByteArray("\r");
			stbProtocol.Commands[15].Request.isVariableLength = true;
			stbProtocol.Commands[15].Request.length = -1;
			stbProtocol.Commands[15].Request.lengthPosition = 2;
			stbProtocol.Commands[15].Request.lengthConstant = 2;
			stbProtocol.Commands[15].Response.header = ToByteArray(" A");
			stbProtocol.Commands[15].Response.footer = ToByteArray("\r");
			stbProtocol.Commands[15].Response.length = 3;
			stbProtocol.Commands[15].Response.isVariableLength = false;
			stbProtocol.Commands[15].Response.lengthPosition = -1;
			#endregion

			for (int i = 0; i < stbProtocol.Commands.Length; i++)
			{
				stbProtocol.Commands[i].Request.Id = stbProtocol.Commands[i].Id;
				stbProtocol.Commands[i].Response.Id = stbProtocol.Commands[i].Id;
				if (stbProtocol.Commands[i].type == TransactionType.Synchronous)
				{
					stbProtocol.Commands[i].Request.type = MessageType.FromHost;
					stbProtocol.Commands[i].Response.type = MessageType.ToHost;
				}
				else
				{
					stbProtocol.Commands[i].Request.type = MessageType.ToHost;
					stbProtocol.Commands[i].Response.type = MessageType.FromHost;
				}
			}
		}

		/// <summary>
		/// Finalyze
		/// </summary>
		~Protocol()
		{
		}

		private Result GetMessageFromHost(ProtocolDefinition thisProtocol, OperationId msgId, ref Message msg)
		{
			Result result = Result.eERR_INITIAL_STATE;
			int index = -1;
			for (int i = 0; i < thisProtocol.Commands.Length; i++)
				if (thisProtocol.Commands[i].Id == msgId)
				{
					index = i;
					if (!thisProtocol.Commands[index].isSupported)
						result = Result.eERR_UNSUPPORTED;
					else
						result = Result.eERR_SUCCESS;
					break;
				}

			if (index == -1)
				result = Result.eERR_NOT_IMPLEMENTED;
			else if (result == Result.eERR_SUCCESS)
			{
				if (thisProtocol.Commands[index].type == TransactionType.Synchronous)
				{
					msg = thisProtocol.Commands[index].Request;
					return result;
				}
				msg = thisProtocol.Commands[index].Response;
				return result;
			}
			msg = new Message();
			return result;
		}

		private Result GetMessageToHost(ProtocolDefinition prot, byte[] inputMsg, ref Message msg)
		{
			Result result = Result.eERR_INITIAL_STATE;
			bool flag = false;
			for (int i = 0; i < prot.Commands.Length; i++)
			{
				flag = false;
				if (prot.Commands[i].isSupported)
				{
					Message response;
					if (prot.Commands[i].type == TransactionType.Synchronous)
					{
						response = prot.Commands[i].Response;
					}
					else
					{
						response = prot.Commands[i].Request;
					}
					if (inputMsg.Length >= response.header.Length)
					{
						flag = true;
						for (int j = 0; j < response.header.Length; j++)
						{
							if (inputMsg[j] != response.header[j])
							{
								flag = false;
								break;
							}
						}
					}
					if (flag)
					{
						msg = response;
						result = Result.eERR_SUCCESS;
						break;
					}
				}
			}
			if (result == Result.eERR_INITIAL_STATE)
			{
				result = Result.eERR_PARAMETER;
				msg = new Message();
			}
			return result;
		}

		private Transaction GetTransaction(OperationId msgId)
		{
			for (int i = 0; i < SupportedProtocols[defaultProtocol].Commands.Length; i++)
				if (SupportedProtocols[defaultProtocol].Commands[i].Id == msgId)
					return SupportedProtocols[defaultProtocol].Commands[i];
			return new Transaction();
		}

		public TransactionType GetTransactionType(OperationId msgId)
		{
			return GetTransaction(msgId).type;
		}

		private bool IsFooterOk(Message msg, byte[] inputMsg)
		{
			bool flag = true;
			for (int i = 1; i <= msg.footer.Length; i++)
				if (inputMsg[inputMsg.Length - i] != msg.footer[msg.footer.Length - i])
					flag = false;
			return flag;
		}

		private bool IsLengthOk(Message msg, byte[] inputMsg)
		{
			Result result = Result.eERR_INITIAL_STATE;
			if (!msg.isVariableLength)
			{
				if (inputMsg.Length != msg.length)
					result = Result.eERR_PARAMETER;
				else
					result = Result.eERR_SUCCESS;
			}
			else
				result = Result.eERR_SUCCESS;
			return (result == Result.eERR_SUCCESS);
		}

		/// <summary>
		/// Checks if packet received is complete
		/// </summary>
		/// <param name="buff"></param>
		/// <param name="packetBytes"></param>
		/// <param name="currMsg"></param>
		/// <returns>True if complete, false otherwise</returns>
		public bool IsPacketCompleteAndUpdateBuffer(ref string buff, ref byte[] packetBytes, ref Message currMsg)
		{
			bool flag = false;
			ProtocolDefinition definition = SupportedProtocols[defaultProtocol];
			Encoding encoding = Encoding.GetEncoding("windows-1252");
			string s = string.Empty;
			if (currMsg.Id == OperationId.Invalid)
				return false;

			flag = buff.Contains(encoding.GetString(currMsg.header));
			if (flag)
			{
				flag = false;
				if (!currMsg.isVariableLength)
				{
					if (buff.Length >= currMsg.length)
					{
						s = buff.Substring(0, currMsg.length);
						flag = true;
					}
				}
				else if (buff.Length > currMsg.lengthPosition)
				{
					byte[] bytes = encoding.GetBytes(buff);
					int length = 0;
					if (currMsg.Id == OperationId.NVMReadData)
					{
						if ((bytes[bytes.Length - 3] == 0xff)
						&& (bytes[bytes.Length - 2] == 0xff)
						&& (bytes[bytes.Length - 1] == 1))
							length = bytes.Length;
						else
							length = 0x400009;
					}
					else
						length = ((bytes[currMsg.lengthPosition] + currMsg.header.Length) + currMsg.footer.Length) + currMsg.lengthConstant;

					if (buff.Length >= length)
					{
						s = buff.Substring(0, length);
						flag = true;
					}
				}
			}
			if (flag)
			{
				packetBytes = encoding.GetBytes(s);
				flag = IsFooterOk(currMsg, packetBytes);
				if (!flag)
				{
					buff = buff.Substring(s.Length);
					s = string.Empty;
				}
				else
					buff = buff.Substring(s.Length);
			}
			return flag;
		}

		private bool IsPayloadLengthOk(Message msg, byte[] inputMsg)
		{
			if (!msg.isVariableLength)
			{
				int num = (inputMsg.Length + msg.header.Length) + msg.footer.Length;
				if (num != msg.length)
					return false;
			}
			return true;
		}
		/// <summary>
		/// Gets number of protocols supported
		/// </summary>
		/// <returns>Numer of protocols supported</returns>
		public int NumProtocols()
		{
			return SupportedProtocols.Length;
		}
		/// <summary>
		///  Sets the current protocol
		/// </summary>
		///  <param name="protocol">0: new protocol, 1: old protocol</param>
		/// <returns>Result.eERR_SUCCESS if successfull</returns>
		public Result SetProtocol(ProtocolId protocol)
		{
			Result result = Result.eERR_INITIAL_STATE;
			if ((protocol < ProtocolId.STBProtocol) || (protocol > ProtocolId.Invalid))
			{
				return Result.eERR_PARAMETER;
			}
			result = Result.eERR_SUCCESS;
			defaultProtocol = (int)protocol;
			return result;
		}

		private byte[] ToByteArray(string In)
		{
			return Encoding.GetEncoding("Windows-1252").GetBytes(In);
		}
	}
}