﻿namespace NVMDatalogger
{
    using System;

    internal enum CtrlReg5_Mask
    {
        INT_CFG_ASLP = 0x80,
        INT_CFG_DRDY = 1,
        INT_CFG_FF_MT_1 = 4,
        INT_CFG_FIFO = 0x40,
        INT_CFG_LNDPRT = 0x10,
        INT_CFG_PULSE = 8,
        INT_CFG_TRANS_1 = 2,
        INT_CFG_TRANS_2 = 0x20
    }
}

