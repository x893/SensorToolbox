﻿namespace NVMDatalogger
{
    using System;

    internal enum CtrlReg4_Mask
    {
        INT_EN_ASLP = 0x80,
        INT_EN_DRDY = 1,
        INT_EN_FF_MT_1 = 4,
        INT_EN_FIFO = 0x40,
        INT_EN_LNDPRT = 0x10,
        INT_EN_PULSE = 8,
        INT_EN_TRANS_1 = 2,
        INT_EN_TRANS_2 = 0x20
    }
}

