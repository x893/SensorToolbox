﻿namespace GlobalSTB
{
	using System;
	using System.Collections.Generic;

	public class RegisterType
	{
		private string _rw;
		private RegisterOptions _value;
		public Dictionary<RegisterOptions, string> RegisterOptionsLabels = new Dictionary<RegisterOptions, string>();

		public RegisterType()
		{
			RegisterOptionsLabels.Add(RegisterOptions.Read, "r");
			RegisterOptionsLabels.Add(RegisterOptions.Write, "w");
			RegisterOptionsLabels.Add(RegisterOptions.ContentsPreservedActiveToStandby, "Register contents are preserved when transitioning from “ACTIVE” to “STANDBY” mode.");
			RegisterOptionsLabels.Add(RegisterOptions.ContentsModifiedAnytime, "Register contents can be modified anytime in “STANDBY” or “ACTIVE” mode.");
			RegisterOptionsLabels.Add(RegisterOptions.ContentsResetStandbyToActive, "Register contents are reset when transitioning from “STANDBY” to “ACTIVE” mode.");
			RegisterOptionsLabels.Add(RegisterOptions.ModificationOnlyStandbyMode, "Modification of this register’s contents can only occur when device is “STANDBY” mode");
		}

		public string getType()
		{
			string str = "";
			if (IsSet(RegisterOptions.Read))
				str = str + RegisterOptionsLabels[RegisterOptions.Read];
			if (IsSet(RegisterOptions.Write))
				str = str + RegisterOptionsLabels[RegisterOptions.Write];
			return str;
		}

		public bool IsRW()
		{
			return Rw.Equals("R/W");
		}

		public bool IsSet(RegisterOptions option)
		{
			return ((Value & option) > ((RegisterOptions)0));
		}

		public override string ToString()
		{
			string str = "";
			if (IsSet(RegisterOptions.ContentsPreservedActiveToStandby))
				str = str + RegisterOptionsLabels[RegisterOptions.ContentsPreservedActiveToStandby] + "\n";
			if (IsSet(RegisterOptions.ContentsModifiedAnytime))
				str = str + RegisterOptionsLabels[RegisterOptions.ContentsModifiedAnytime] + "\n";
			if (IsSet(RegisterOptions.ContentsResetStandbyToActive))
				str = str + RegisterOptionsLabels[RegisterOptions.ContentsResetStandbyToActive] + "\n";
			if (IsSet(RegisterOptions.ModificationOnlyStandbyMode))
				str = str + RegisterOptionsLabels[RegisterOptions.ModificationOnlyStandbyMode] + "\n";
			return str;
		}

		public string Rw
		{
			get { return _rw; }
			set { _rw = value; }
		}

		public RegisterOptions Value
		{
			get { return _value; }
			set { _value = value; }
		}
	}
}
