﻿namespace MMA845xQEvaluation
{
    using System;

    internal enum IntSource_Mask
    {
        SCR_DRDY = 1,
        SRC_ASLP = 0x80,
        SRC_FF_MT_1 = 4,
        SRC_FIFO = 0x40,
        SRC_LNDPRT = 0x10,
        SRC_PULSE = 8,
        SRC_TRANS_1 = 2,
        SRC_TRANS_2 = 0x20
    }
}

