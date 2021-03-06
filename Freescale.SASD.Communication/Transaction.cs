﻿namespace Freescale.SASD.Communication
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct Transaction
    {
        public OperationId Id;
        public Message Request;
        public Message Response;
        public bool isSupported;
        public TransactionType type;
    }
}

