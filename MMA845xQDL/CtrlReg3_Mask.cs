﻿namespace VeyronDatalogger
{
    using System;

    internal enum CtrlReg3_Mask
    {
        FIFO_GATE = 0x80,
        IPOL = 2,
        PPOD = 1,
        WAKE_FF_MT_1 = 8,
        WAKE_LNDPRT = 0x20,
        WAKE_PULSE = 0x10,
        WAKE_TRANS_1 = 4,
        WAKE_TRANS_2 = 0x40
    }
}

