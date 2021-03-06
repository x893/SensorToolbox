﻿namespace Freescale.SASD.Communication
{
    using System;
    using System.Runtime.CompilerServices;

    public interface Parallel_ICom
    {
        event TransactionEventHandler AsynchronousTransactionEvent;

        event TransactionEventHandler SynchronousTransactionEvent;

        Result AppRegisterRead(byte addr, byte count);
        Result AppRegisterWrite(byte addr, ref byte[] data, byte count);
        Result BufferFlush();
        Result Close();
        Result GetDeviceInfo();
        Result NVMBytesWritten();
        Result NVMConfig(byte[] conf);
        Result NVMErase();
        Result NVMGetConstants();
        Result NVMIsErased();
        Result NVMReadData();
        Result NVMWriteData();
        Result Open(string[] comString);
        Result PortScan(ref string[] comPorts);
        Result ReadData(int internalAddress);
        Result ReadData(int internalAddress, int count);
        Result WriteData(int internalAddress, byte data);
        Result WriteData(int internalAddress, byte[] dataBuf, int count);
    }
}

