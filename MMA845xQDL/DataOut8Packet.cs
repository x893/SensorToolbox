﻿namespace VeyronDatalogger
{
    using System;

    internal class DataOut8Packet
    {
        public XYZCounts AxisCounts = new XYZCounts();
        public byte Status;
    }
}

