﻿namespace MMA845xQEvaluation
{
    using System;

    internal enum Status_Mask
    {
        XDR = 1,
        XOW = 0x10,
        YDR = 2,
        YOW = 0x20,
        ZDR = 4,
        ZOW = 0x40,
        ZYXDR = 8,
        ZYXOW = 0x80
    }
}

