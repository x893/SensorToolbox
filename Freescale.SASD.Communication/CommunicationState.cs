﻿namespace Freescale.SASD.Communication
{
    using System;

    public enum CommunicationState
    {
        Bootloader = 4,
        HWFind = 3,
        Idle = 1,
        Invalid = 5,
        Ready = 2
    }
}

