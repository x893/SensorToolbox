﻿namespace VeyronDatalogger
{
    using System;
    using System.Collections;

    internal class ControllerReqPacket
    {
        public int FormID;
        public Queue PayLoad = new Queue();
        public int TaskID;
    }
}

