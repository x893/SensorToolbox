﻿namespace Freescale.SASD.Communication
{
    using System;
    using System.Runtime.CompilerServices;

    public delegate void MCUToHostEventHandler(object sender, byte[] packet, OperationId cmd);
}

