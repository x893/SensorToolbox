﻿namespace MMA845xQEvaluation
{
    using System;

    internal class DataOutFIFOPacket
    {
        public XYZCounts[] FIFOData = new XYZCounts[0x20];
        public byte Status;
    }
}

