﻿namespace GlobalSTB
{
	using System;
	using System.Collections;
	using System.ComponentModel;
	using System.IO;
	using System.Windows.Forms;

	public class SaveRecords
	{
		private ArrayList _data;
		private SaveFileDialog _fileDialog;
		private string _product = "";

		public SaveRecords(string productName)
		{
			_product = productName;
			_fileDialog = new SaveFileDialog();
			_fileDialog.FileOk += new CancelEventHandler(_fileDialog_FileOk);
		}

		private void _fileDialog_FileOk(object sender, CancelEventArgs e)
		{
			TextWriter writer = new StreamWriter(_fileDialog.FileName);
			writer.WriteLine(string.Format("{0} {1}", DateTime.UtcNow, ProductName));
			writer.WriteLine("");
			writer.WriteLine(string.Format("Address,Record,Decimal,Hexadecimal,Binary", new object[0]));
			foreach (RecordFields fields in _data)
				writer.WriteLine(string.Format("{0:X},{1,-15},{2,12},{3:X},{4,12}", new object[] { fields.Number, fields.Name, fields.Value, fields.Value, Convert.ToString(fields.Value, 2) }));
			writer.Close();
		}

		public void Save(ArrayList data)
		{
			_data = data;
			TimeSpan span = (TimeSpan)(DateTime.UtcNow - new DateTime(0x7b2, 1, 1));
			_fileDialog.FileName = string.Format("Log_{0}_{1}.csv", ProductName, (int)span.TotalSeconds);
			_fileDialog.Filter = "Comma delimited (*.csv)|*.csv|All files (*.*)|*.*";
			_fileDialog.Title = string.Format("Save {0} records as datalog file", ProductName);
			_fileDialog.ShowDialog();
		}

		public string ProductName
		{
			get { return _product; }
			set { _product = value; }
		}
	}
}