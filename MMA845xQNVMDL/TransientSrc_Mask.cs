﻿namespace NVMDatalogger
{
    using System;

    internal enum TransientSrc_Mask
    {
        EA = 0x40,
        XTRANS_Pol = 1,
        XTRANSE = 2,
        YTRANS_Pol = 4,
        YTRANSE = 8,
        ZTRANS_Pol = 0x10,
        ZTRANSE = 0x20
    }
}

