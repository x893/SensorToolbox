﻿namespace Freescale.SASD.Communication
{
    using System;
    using System.Runtime.CompilerServices;

    public interface CommConfig
    {
        event CommEventHandler CommEvent;

        Result Close();
        Result FindAnyHw();
        Result FindHw(string a);
        CommunicationState GetCommState();
        string GetCommStatus();
        Result SetConnectionString(string cnxStr);
        Result SetProtocol(ProtocolId protocol);
    }
}

