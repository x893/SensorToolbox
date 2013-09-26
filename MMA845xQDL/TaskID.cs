﻿namespace VeyronDatalogger
{
    using System;

    internal enum TaskID
    {
        Active = 0x53,
        AutoCal = 0x52,
        Boot = 0x51,
        FIFO14Stream = 100,
        FIFO8Stream = 0x63,
        MemMap = 0x68,
        NVM = 0x69,
        PollingData = 0x4e,
        ReadAppRegister = 0x6b,
        ReadParsedValue = 0x5d,
        ReadValue = 0x11,
        RegisterDump = 0x10,
        ResetDevice = 0x61,
        ReturningFIFOStatus = 80,
        ReturningMFF1Status = 0x4c,
        ReturningPLStatus = 0x4f,
        ReturningPulseStatus = 0x4b,
        ReturningSysmodStatus = 0x4a,
        ReturningTrans1Status = 0x4d,
        ReturningTransStatus = 0x49,
        SetBFTripA = 0x1b,
        SetCal = 12,
        SetDataRate = 5,
        SetDBCNTM_PL = 30,
        SetEnableAnalogLowNoise = 0x57,
        SetEnableASleep = 7,
        SetEnablePL = 0x19,
        SetFIFOMode = 0x13,
        SetFIFOOutputEnable = 0x22,
        SetFIFOWatermark = 20,
        SetFREAD = 0x58,
        SetFullScaleValue = 4,
        SetHPFDataOut = 0x56,
        SetHPFilter = 11,
        SetHysteresis = 0x1d,
        SetIntConfig = 14,
        SetINTPolarity = 0x21,
        SetINTPPOD = 0x20,
        SetIntsEnable = 13,
        SetIntsEnableBit = 0x65,
        SetMFF1AndOr = 0x33,
        SetMFF1DBCNTM = 0x3a,
        SetMFF1Debounce = 0x39,
        SetMFF1Latch = 0x34,
        SetMFF1Threshold = 0x38,
        SetMFF1XEFE = 0x35,
        SetMFF1YEFE = 0x36,
        SetMFF1ZEFE = 0x37,
        SetOSMode = 0x54,
        SetPLDebounce = 0x1f,
        SetPLTripA = 0x1c,
        SetPulse2ndPulseWin = 0x48,
        SetPulseDPA = 0x3b,
        SetPulseFirstTimeLimit = 0x43,
        SetPulseHPFBypass = 90,
        SetPulseLatch = 60,
        SetPulseLatency = 0x47,
        SetPulseLPFEnable = 0x59,
        SetPulseXDP = 0x41,
        SetPulseXSP = 0x42,
        SetPulseXThreshold = 0x44,
        SetPulseYDP = 0x3f,
        SetPulseYSP = 0x40,
        SetPulseYThreshold = 0x45,
        SetPulseZDP = 0x3d,
        SetPulseZSP = 0x3e,
        SetPulseZThreshold = 70,
        SetSelfTest = 10,
        SetSleepSampleRate = 8,
        SetSleepTimer = 9,
        SetSOSMode = 0x55,
        SetTransBypassHPF = 0x23,
        SetTransBypassHPFNEW = 0x2b,
        SetTransDBCNTM = 0x26,
        SetTransDBCNTMNEW = 50,
        SetTransDebounce = 0x25,
        SetTransDebounceNEW = 0x31,
        SetTransEnableLatch = 0x2a,
        SetTransEnableLatchNEW = 0x2f,
        SetTransEnableXFlag = 0x27,
        SetTransEnableXFlagNEW = 0x2c,
        SetTransEnableYFlag = 40,
        SetTransEnableYFlagNEW = 0x2d,
        SetTransEnableZFlag = 0x29,
        SetTransEnableZFlagNEW = 0x2e,
        SetTransThreshold = 0x24,
        SetTransThresholdNEW = 0x30,
        SetTrigLP = 0x16,
        SetTrigMFF = 0x18,
        SetTrigPulse = 0x17,
        SetTrigTrans = 0x15,
        SetUpInterrupts = 15,
        SetWakeFromSleep = 6,
        SetZLockA = 0x1a,
        StartDevice = 0x62,
        StartStream = 0x5f,
        StartTest = 0x66,
        StopStream = 0x5e,
        StopTest = 0x67,
        StreamIntFunctionData = 0x60,
        WriteAppRegister = 0x6a,
        WriteValue = 0x12,
        XYZ10StreamData = 0x5b,
        XYZ12StreamData = 2,
        XYZ14StreamData = 0x5c,
        XYZ8StreamData = 1
    }
}

