﻿namespace Freescale.SASD.Communication
{
    using System;
    using System.Runtime.CompilerServices;

    public delegate void TransactionEventHandler(object sender, byte[] packet, OperationId cmd);
}

