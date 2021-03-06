﻿namespace GlobalSTB
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct RecordFields
    {
        private int _number;
        private string _name;
        private byte _value;
        public RecordFields(int Number, string Name, byte Value)
        {
            _number = Number;
            _name = Name;
            _value = Value;
        }

        public int Number
        {
            get
            {
                return _number;
            }
        }
        public string Name
        {
            get
            {
                return _name;
            }
        }
        public byte Value
        {
            get
            {
                return _value;
            }
        }
    }
}

