﻿namespace MMA845xQEvaluation
{
	using System;

	public class MMA845x_Class
	{
		private FBID curr_ds_FBID;
		private FBID curr_int_FBID;
		private DevAddress device_Addr;
		private deviceID device_ID = deviceID.Unsupported;

		private int[,] identification = new int[8,4] {
			{ (int)deviceID.MMA8451Q, 0x3A, 0x1A, 0x3002 },
			{ (int)deviceID.MMA8452Q, 0x3A, 0x2A, 0x3003 },
			{ (int)deviceID.MMA8453Q, 0x3A, 0x3A, 0x3004 },
			{ (int)deviceID.MMA8652FC, 0x3A, 0x4A, 0x3005 },
			{ (int)deviceID.MMA8653FC, 0x3A, 0x5A, 0x3006 },
			{ (int)deviceID.MMA8661Q, 0x3A, 0x6A, 0x3007 },
			{ (int)deviceID.MMA8662Q, 0x3A, 0x7A, 0x3008 },
			{ (int)deviceID.MMA8491Q, 0xAA, 0x0F, 0x3009 }
		};
		private int[,] validation = new int[13,3] {
			{ 4, 0, 7 },
			{ 3, 0, 7 },
			{ 2, 0, 7 },
			{ 1, 0, 4 },
			{ 9, 9, 2 },
			{ 5, 5, 2 },
			{ 7, 7, 2 },
			{ 6, 0, 0x61 },
			{ 8, 8, 2 },
			{ 12, 12, 2 },
			{ 13, 13, 1 },
			{ 10, 10, 1 },
			{ 11, 11, 1 }
		};

		public int GetLength(FBID functional_id)
		{
			if (functional_id == FBID.DataOut10 ||
				functional_id == FBID.DataOut12 ||
				functional_id == FBID.DataOut14
				)
			{
				if (DeviceID == deviceID.MMA8451Q)
					functional_id = FBID.DataOut14;
				else if (DeviceID == deviceID.MMA8452Q)
					functional_id = FBID.DataOut12;
				else if (DeviceID == deviceID.MMA8453Q)
					functional_id = FBID.DataOut10;
			}

			for (int i = 0; i < validation.Length; i++)
				if (validation[i, 0] == (int)functional_id)
					return validation[i, 2];

			return -1;
		}

		public int GetRegAddress(FBID functional_id)
		{
			for (int i = 0; i < validation.Length; i++)
				if (validation[i, 0] == (int)functional_id)
					return validation[i, 1];
			return -1;
		}

		public void SetDeviceId(string id)
		{
			try
			{
				id = id.Substring(0, 4);
				int num =
					Convert.ToInt16(id.Substring(0, 1)) * 0x1000 +
					Convert.ToInt16(id.Substring(1, 1)) * 0x100 +
					Convert.ToInt16(id.Substring(2, 1)) * 0x10 +
					Convert.ToInt16(id.Substring(3, 1));
				for (int i = 0; i < (identification.Length / 4); i++)
					if (num == identification[i, 3])
					{
						device_ID = (deviceID)identification[i, 0];
						device_Addr = (DevAddress)identification[i, 1];
						return;
					}
			}
			catch (Exception) { }
		}

		private bool validate(int mode, int addr, int length)
		{
			FBID oFF = (mode == 1) ? curr_int_FBID : curr_ds_FBID;
			for (int i = 0; i < validation.Length; i++)
				if (validation[i, 0] == (int)oFF)
					return ((validation[i, 1] == addr) && (validation[i, 2] == length));
			return false;
		}

		public bool ValidateDsPackage(int address, int length)
		{
			return validate(0, address, length);
		}

		public bool ValidateIntPackage(int address, int length)
		{
			return validate(1, address, length);
		}

		public byte DeviceAddress
		{
			get
			{
				for (int i = 0; i < (identification.Length / 4); i++)
					if (identification[i, 0] == (int)device_ID)
						return (byte)identification[i, 1];
				throw new Exception("device ID not set");
			}
		}

		public deviceID DeviceID
		{
			get { return device_ID; }
			set { }
		}

		public FBID DsFBID
		{
			get { return curr_ds_FBID; }
			set
			{
				if (value == FBID.DataOut10 || value == FBID.DataOut12 || value == FBID.DataOut14)
				{
					if (DeviceID == deviceID.MMA8451Q)
						curr_ds_FBID = FBID.DataOut14;
					else if (DeviceID == deviceID.MMA8452Q)
						curr_ds_FBID = FBID.DataOut12;
					else if (DeviceID == deviceID.MMA8453Q)
						curr_ds_FBID = FBID.DataOut10;
					else if (DeviceID == deviceID.MMA8652FC)
						curr_ds_FBID = FBID.DataOut12;
					else if (DeviceID == deviceID.MMA8653FC)
						curr_ds_FBID = FBID.DataOut10;
					else if (DeviceID == deviceID.MMA8491Q)
						curr_ds_FBID = FBID.DataOut14;
					else
						curr_ds_FBID = value;
				}
				else
					curr_ds_FBID = value;
			}
		}

		public FBID IntFBID
		{
			get { return curr_int_FBID; }
			set { curr_int_FBID = value; }
		}
	}
}