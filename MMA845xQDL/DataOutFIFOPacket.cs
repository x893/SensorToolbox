﻿namespace VeyronDatalogger
{
    using System;

    internal class DataOutFIFOPacket
    {
        public XYZCounts[] FIFOData = new XYZCounts[0x20];
        public byte Status;
    }
}

