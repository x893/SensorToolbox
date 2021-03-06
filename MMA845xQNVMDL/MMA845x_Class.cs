﻿namespace NVMDatalogger
{
    using System;

    public class MMA845x_Class
    {
        private FBID curr_ds_FBID;
        private bool curr_FREAD;
        private FBID curr_int_FBID;
        private DevAddress device_Addr;
        private deviceID device_ID = deviceID.Unsupported;
        private int[,] identification = new int[,] { { 1, 0x3a, 15, 0x3001 }, { 2, 0x3a, 0x1a, 0x3002 }, { 3, 0x3a, 0x2a, 0x3003 }, { 4, 0x3a, 0x3a, 0x3004 }, { 5, 0x3a, 0x4a, 0x3005 }, { 6, 0x3a, 90, 0x3006 }, { 7, 0x3a, 0x6a, 0x3007 }, { 8, 0x3a, 0x7a, 0x3008 } };
        private int[,] validation = new int[,] { { 4, 0, 7 }, { 3, 0, 7 }, { 2, 0, 7 }, { 1, 0, 4 }, { 9, 9, 2 }, { 5, 5, 2 }, { 7, 0, 0xc1 }, { 6, 0, 0x61 }, { 8, 8, 2 }, { 12, 12, 2 }, { 13, 13, 1 }, { 10, 10, 1 }, { 11, 11, 1 } };

        public int GetLength(FBID functional_id)
        {
            for (int i = 0; i < this.validation.Length; i++)
            {
                if (this.validation[i, 0] == (int)functional_id)
                {
                    return this.validation[i, 2];
                }
            }
            return -1;
        }

        public int GetRegAddress(FBID functional_id)
        {
            for (int i = 0; i < this.validation.Length; i++)
            {
				if (this.validation[i, 0] == (int)functional_id)
                {
                    return this.validation[i, 1];
                }
            }
            return -1;
        }

        public void SetDeviceId(string a)
        {
            try
            {
                a = a.Substring(0, 4);
                int num = (((Convert.ToInt16(a.Substring(0, 1)) * 0x1000) + (Convert.ToInt16(a.Substring(1, 1)) * 0x100)) + (Convert.ToInt16(a.Substring(2, 1)) * 0x10)) + Convert.ToInt16(a.Substring(3, 1));
                for (int i = 0; i < (this.identification.Length / 4); i++)
                {
                    if (num == this.identification[i, 3])
                    {
                        this.device_ID = (deviceID) this.identification[i, 0];
                        this.device_Addr = (DevAddress) this.identification[i, 1];
                        return;
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private bool validate(int mode, int addr, int length)
        {
            FBID oFF = FBID.OFF;
            if (mode == 1)
            {
                oFF = this.curr_int_FBID;
            }
            else
            {
                oFF = this.curr_ds_FBID;
            }
            for (int i = 0; i < this.validation.Length; i++)
            {
				if (this.validation[i, 0] == (int)oFF)
                {
                    return ((this.validation[i, 1] == addr) && (this.validation[i, 2] == length));
                }
            }
            return false;
        }

        public bool ValidateDsPackage(int address, int length)
        {
            return this.validate(0, address, length);
        }

        public bool ValidateIntPackage(int address, int length)
        {
            return this.validate(1, address, length);
        }

        public byte DeviceAddress
        {
            get
            {
                for (int i = 0; i < (this.identification.Length / 4); i++)
                {
                    if (this.identification[i, 0] == (int)device_ID)
                    {
                        return (byte) this.identification[i, 1];
                    }
                }
                throw new Exception("device ID not set");
            }
        }

        public deviceID DeviceID
        {
            get
            {
                return this.device_ID;
            }
            set
            {
                this.device_ID = value;
                if (this.device_ID == deviceID.MMA8451Q)
                {
                    this.device_Addr = DevAddress.DEVADDR_1C;
                }
            }
        }

        public FBID DsFBID
        {
            get
            {
                return this.curr_ds_FBID;
            }
            set
            {
                if (((value == FBID.DataOut10) || (value == FBID.DataOut12)) || (value == FBID.DataOut14))
                {
                    if (this.DeviceID == deviceID.MMA8451Q)
                    {
                        this.curr_ds_FBID = FBID.DataOut14;
                    }
                    else if (this.DeviceID == deviceID.MMA8452Q)
                    {
                        this.curr_ds_FBID = FBID.DataOut12;
                    }
                    else if (this.DeviceID == deviceID.MMA8453Q)
                    {
                        this.curr_ds_FBID = FBID.DataOut10;
                    }
                    else if (this.DeviceID == deviceID.MMA8652FC)
                    {
                        this.curr_ds_FBID = FBID.DataOut12;
                    }
                    else if (this.DeviceID == deviceID.MMA8653FC)
                    {
                        this.curr_ds_FBID = FBID.DataOut10;
                    }
                    else
                    {
                        this.curr_ds_FBID = value;
                    }
                }
                else
                {
                    this.curr_ds_FBID = value;
                }
            }
        }

        public bool FREAD
        {
            get
            {
                return this.curr_FREAD;
            }
            set
            {
                this.curr_FREAD = value;
            }
        }

        public FBID IntFBID
        {
            get
            {
                return this.curr_int_FBID;
            }
            set
            {
                this.curr_int_FBID = value;
            }
        }
    }
}

