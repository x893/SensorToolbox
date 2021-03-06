﻿namespace GlobalSTB
{
	using System;

	public class RegisterSTB
	{
		private int _address;
		private string _comment;
		private string _description;
		private string _group;
		private string _name;
		private byte _value;
		public string[] BitFieldName = new string[8];
		public RegisterType Type = new RegisterType();

		public RegisterSTB()
		{
			Group = "";
		}

		public int Address
		{
			get { return _address; }
			set { _address = value; }
		}

		public string Comment
		{
			get { return _comment; }
			set { _comment = value; }
		}

		public string Description
		{
			get { return _description; }
			set { _description = value; }
		}

		public string Group
		{
			get { return _group; }
			set { _group = value; }
		}

		public string Name
		{
			get { return _name; }
			set { _name = value; }
		}

		public byte Value
		{
			get { return _value; }
			set { _value = value; }
		}
	}
}