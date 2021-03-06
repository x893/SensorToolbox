﻿namespace Freescale.SASD.Communication
{
    using System;
    using System.Runtime.CompilerServices;

    public interface ICom
    {
        event MCUToHostEventHandler AsynchronousTransactionEvent;

        Result AppRegisterRead(int count, int internalAddress, ref byte[] dataReceived);
        Result AppRegisterWrite(int count, int internalAddress, byte[] dataBuf);
        int ClearIOPin(int bank, byte pinNumber);
        Result Close();
        Result FindHw(string deviceID);
        Result GetDeviceInfo(ref byte[] deviceID);
        Result MultiPurposeSend(byte[] dataBuf);
        Result NVMBytesWritten(ref byte[] written);
        Result NVMConfig(byte[] conf);
        Result NVMErase();
        Result NVMGetConstants(ref byte[] constants);
        Result NVMIsErased(ref byte[] erased);
        Result NVMReadData(ref byte[] dataReceived);
        Result NVMWriteData();
        Result ReadData(int internalAddress, ref byte dataReceived);
        Result ReadData(int count, int internalAddress, ref byte[] dataReceived);
        int ReadIOPin(int bank, int pinNumber, ref byte pinVal);
        int SetIOPin(int bank, byte pinNumber);
        Result WriteData(int internalAddress, byte data);
        Result WriteData(int count, int internalAddress, byte[] dataBuf);
    }
}

