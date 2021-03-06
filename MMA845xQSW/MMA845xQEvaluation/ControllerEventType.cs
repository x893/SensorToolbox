﻿namespace MMA845xQEvaluation
{
    using System;

    internal enum ControllerEventType
    {
        DataOut8Packet = 1,
        DataOutFIFOPacket = 3,
        DataOutFullPacket = 2,
        GenericStream = 6,
        Interrupt = 5,
        Orientation = 4,
        Pulse = 7,
        Shake = 8,
        Transient = 9
    }
}

