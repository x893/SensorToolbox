﻿namespace Freescale.SASD.Communication
{
    using System;

    public enum OperationId
    {
        AppRegisterRead = 3,
        AppRegisterWrite = 4,
        DeviceRead = 5,
        DeviceWrite = 6,
        Handshake = 1,
        Identify = 2,
        InterruptData = 0x65,
        Invalid = 0x3e8,
        MultiPurpose = 14,
        NVMBytesWritten = 13,
        NVMConfig = 11,
        NVMErase = 7,
        NVMGetConstants = 12,
        NVMIsErased = 10,
        NVMReadData = 8,
        NVMWriteData = 9,
        StreamData = 100
    }
}

