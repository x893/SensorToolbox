﻿namespace Freescale.SASD.Communication
{
    using System;

    public enum CommEvent
    {
        CommLost = 2,
        CommOpen = 1,
        FindHW_HW_NOT_FOUND = 3,
        Invalid = 0x3e8
    }
}

