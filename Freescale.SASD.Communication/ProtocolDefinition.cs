﻿namespace Freescale.SASD.Communication
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct ProtocolDefinition
    {
        public Transaction[] Commands;
    }
}

