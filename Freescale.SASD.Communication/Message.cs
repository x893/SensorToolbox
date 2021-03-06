﻿namespace Freescale.SASD.Communication
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct Message
    {
        public OperationId Id;
        public bool isVariableLength;
        public int length;
        public byte[] header;
        public byte[] footer;
        public int lengthPosition;
        public int lengthConstant;
        public MessageType type;
    }
}

