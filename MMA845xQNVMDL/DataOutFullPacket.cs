﻿namespace NVMDatalogger
{
    using System;

    internal class DataOutFullPacket
    {
        public XYZCounts AxisCounts = new XYZCounts();
        public byte Status;
    }
}

