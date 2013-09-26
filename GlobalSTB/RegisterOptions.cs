﻿namespace GlobalSTB
{
    using System;

    public enum RegisterOptions
    {
        ContentsModifiedAnytime = 0x20,
        ContentsPreservedActiveToStandby = 4,
        ContentsResetStandbyToActive = 8,
        ModificationOnlyStandbyMode = 0x10,
        Read = 1,
        Write = 2
    }
}

