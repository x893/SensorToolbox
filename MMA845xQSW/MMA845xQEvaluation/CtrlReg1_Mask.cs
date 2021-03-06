﻿namespace MMA845xQEvaluation
{
    using System;

    internal enum CtrlReg1_Mask
    {
        ACTIVE = 1,
        ASLP_RATE = 0xc0,
        ASLP_RATE0 = 0x40,
        ASLP_RATE1 = 0x80,
        DR = 0x38,
        DR0 = 8,
        DR1 = 0x10,
        DR2 = 0x20,
        F_READ = 2,
        LNOISE = 4
    }
}

