﻿namespace GlobalSTB
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Drawing;
	using System.Runtime.CompilerServices;
	using System.Text;
	using System.Windows.Forms;

	public class RegisterView : FlowLayoutPanel
	{
		public delegate void RegisterBitsChangeDelegate(RegisterSTB register, int newVal);
		public delegate void RegisterChangeDelegate(RegisterSTB register);

		private int _currentRegisterAddress = -1;
		private Dictionary<int, RegisterSTB> _registerMap = new Dictionary<int, RegisterSTB>();
		public string _rowSeparator = "^^";
		private Panel _uxAllRegistersGB;
		private RichTextBox _uxComment;
		private RichTextBox _uxNotes;
		private FlowLayoutPanel _uxRbPanel;
		private GroupBox _uxRegisterGB;
		private TableLayoutPanel _uxTable;
		public static int AllRegistersHeight = 250;
		public static Color Backcolor = Color.Transparent;
		public Button ButtonExportRegisters;
		public RegisterSTB CurrentRegister;
		public static string FontName = "Calibri";
		public new string ProductName = "";
		public uint RadioButtonFontSize = 10;

		public RegisterBitsChangeDelegate RegisterBitsChange;
		public RegisterChangeDelegate RegisterChange;

		public RegisterView(int _width, int _height)
		{
			this.FlowDirection = FlowDirection.TopDown;
			this.Width = _width;
			this.Height = _height;

			_uxAllRegistersGB = new Panel();
			_uxAllRegistersGB.BorderStyle = BorderStyle.Fixed3D;
			_uxAllRegistersGB.AutoScroll = true;
			_uxAllRegistersGB.Font = new Font(FontName, 12f, FontStyle.Regular);
			_uxAllRegistersGB.ForeColor = Color.White;
			_uxAllRegistersGB.Width = base.Width - 5;
			_uxAllRegistersGB.Height = AllRegistersHeight;
			this.Controls.Add(_uxAllRegistersGB);

			_uxRegisterGB = new GroupBox();
			_uxRegisterGB.Text = "Register";
			_uxRegisterGB.Font = new Font(FontName, 12f, FontStyle.Regular);
			_uxRegisterGB.ForeColor = Color.White;
			_uxRegisterGB.Width = _uxAllRegistersGB.Width;
			_uxRegisterGB.Height = (base.Height - _uxAllRegistersGB.Height) - 15;
			_uxRegisterGB.BackColor = Backcolor;
			this.Controls.Add(_uxRegisterGB);

			_uxTable = new TableLayoutPanel();
			_uxTable.ColumnCount = 9;
			_uxTable.RowCount = 4;
			_uxTable.Left = 15;
			_uxTable.Top = 25;
			_uxTable.Width = _uxRegisterGB.Width - 20;
			_uxTable.AutoSize = true;
			_uxTable.BackColor = Backcolor;
			_uxTable.ForeColor = Color.Black;
			_uxTable.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
			_uxRegisterGB.Controls.Add(_uxTable);

			for (int i = 0; i <= 7; i++)
			{
				Label label = new Label
				{
					Font = new Font(FontName, 10f, FontStyle.Bold),
					Text = "D" + i.ToString()
				};
				_uxTable.Controls.Add(label, 7 - i, 0);
				label = new Label
				{
					Name = "uxDescriptionBit" + i.ToString(),
					Font = new Font(FontName, 8f, FontStyle.Italic),
					Text = "Unknown"
				};
				_uxTable.Controls.Add(label, 7 - i, 1);
				CheckBox box = new CheckBox
				{
					Appearance = Appearance.Button,
					BackColor = Color.Red,
					Font = new Font("Calibri", 14.25f, FontStyle.Bold, GraphicsUnit.Point, 0),
					ForeColor = Color.Yellow,
					Size = new Size(0x2b, 0x25)
				};
				new ToolTip().SetToolTip(box, "Click to toggle bit");
				box.Text = "0";
				box.TextAlign = ContentAlignment.MiddleCenter;
				box.Name = "uxLedBit" + i.ToString();
				box.Dock = DockStyle.Fill;
				box.Checked = false;
				box.Click += new EventHandler(led_Click);
				_uxTable.Controls.Add(box, 7 - i, 2);
			}
			Label control = new Label
			{
				Font = new Font(FontName, 10f, FontStyle.Bold),
				Text = "Type"
			};
			_uxTable.Controls.Add(control, 8, 0);
			control = new Label
			{
				Name = "uxType",
				Font = new Font(FontName, 10f, FontStyle.Bold),
				ForeColor = Color.Black,
				Text = ""
			};
			_uxTable.Controls.Add(control, 8, 1);
			control = new Label
			{
				Name = "uxValueBit07",
				Font = new Font(FontName, 10f, FontStyle.Regular),
				ForeColor = Color.White,
				Text = "0"
			};
			_uxTable.Controls.Add(control, 8, 2);
			_uxComment = new RichTextBox();
			_uxComment.Text = "";
			_uxComment.ForeColor = Color.Black;
			_uxComment.Font = new Font(FontName, 10f, FontStyle.Bold);
			_uxComment.AutoSize = true;
			_uxComment.Height = 30;
			_uxComment.Dock = DockStyle.Fill;
			_uxComment.ReadOnly = true;
			_uxTable.Controls.Add(_uxComment, 0, 3);
			_uxTable.SetColumnSpan(_uxComment, 9);

			_uxNotes = new RichTextBox();
			_uxNotes.Text = "";
			_uxNotes.ForeColor = Color.Black;
			_uxNotes.Left = _uxTable.Left;
			_uxNotes.Font = new Font(FontName, 10f, FontStyle.Italic | FontStyle.Bold);
			_uxNotes.Top = (_uxTable.Height + _uxTable.Top) + 3;
			_uxNotes.Width = _uxRegisterGB.Width - 20;
			_uxNotes.ReadOnly = true;
			_uxNotes.AutoSize = true;
			_uxRegisterGB.Controls.Add(_uxNotes);

			_uxRbPanel = new FlowLayoutPanel();
			_uxRbPanel.FlowDirection = FlowDirection.TopDown;
			_uxRbPanel.Top = 15;
			_uxRbPanel.Left = 10;
			_uxRbPanel.Width = _uxAllRegistersGB.Width - 15;
			_uxRbPanel.Height = _uxAllRegistersGB.Height - 20;
			_uxRbPanel.Dock = DockStyle.Fill;
			_uxAllRegistersGB.Controls.Add(_uxRbPanel);
		}

		public void ExportRegisters(byte[] RegValues)
		{
			StringBuilder builder = new StringBuilder("");
			ArrayList data = new ArrayList();
			foreach (KeyValuePair<int, RegisterSTB> pair in RegisterMap)
				data.Add(new RecordFields(pair.Value.Address, pair.Value.Name, RegValues[pair.Value.Address]));
			new SaveRecords(ProductName).Save(data);
		}

		private void led_Click(object sender, EventArgs e)
		{
			int newVal = 0;
			for (int i = 0; i <= 7; i++)
			{
				CheckBox box = (CheckBox)_uxTable.Controls["uxLedBit" + i.ToString()];
				newVal += box.Checked ? (1 << i) : 0;
			}

			if (RegisterBitsChange != null)
				RegisterBitsChange(CurrentRegister, newVal);
		}

		public void ParseMap(string map)
		{
			string[] lines = map.Replace("\r\n", "\n").Split(new char[] { '\n' });
			RadioButton sender = null;
			for (int i = 0; i < lines.Length; i++)
			{
				if (lines[i].Equals(_rowSeparator))
				{
					_uxNotes.Text = "";
					for (int j = i + 1; j < lines.Length; j++)
						_uxNotes.Text = _uxNotes.Text + lines[j] + "\n";
					break;
				}

				string[] strArray2 = lines[i].TrimEnd(new char[] { '\r' }).Split(new char[] { '\t' });

				RadioButton button2 = new RadioButton
				{
					Name = "rb" + strArray2[0]
				};
				if (i == 0)
				{
					button2.Checked = true;
					sender = button2;
				}
				
				RegisterSTB rstb = new RegisterSTB();
				rstb.Address = Convert.ToInt32(strArray2[0], 16);
				rstb.Name = strArray2[1];
				rstb.Description = strArray2[2];
				rstb.Comment = strArray2[5].Replace('|', ' ');
				rstb.BitFieldName = strArray2[6].Split(new char[] { ',' });
				Array.Reverse(rstb.BitFieldName);
				rstb.Type.Value = (RegisterOptions)Convert.ToInt32(strArray2[4]);
				rstb.Type.Rw = strArray2[3];
				RegisterMap.Add(rstb.Address, rstb);

				button2.Font = new Font(FontName, (float)RadioButtonFontSize, FontStyle.Regular);
				button2.Text = string.Format("{0} {1}", strArray2[0], rstb.Name);
				button2.ForeColor = Color.Black;
				button2.AutoSize = true;
				button2.CheckedChanged += new EventHandler(rbo_CheckedChanged);
				_uxRbPanel.Controls.Add(button2);
			}
			ButtonExportRegisters = new Button();
			ButtonExportRegisters.Text = "Export All Records";
			ButtonExportRegisters.AutoSize = true;
			ButtonExportRegisters.BackColor = Color.Gray;
			ButtonExportRegisters.ForeColor = Color.Black;
			_uxRbPanel.Controls.Add(ButtonExportRegisters);
			rbo_CheckedChanged(sender, null);
		}

		private void rbo_CheckedChanged(object sender, EventArgs e)
		{
			RadioButton button = (RadioButton)sender;
			if (button.Checked)
			{
				_currentRegisterAddress = Convert.ToInt32(button.Name.Substring(2, button.Name.Length - 2), 16);
				CurrentRegister = RegisterMap[_currentRegisterAddress];
				_uxRegisterGB.Text = CurrentRegister.Description;
				for (int bitIdx = 0; bitIdx <= 7; bitIdx++)
				{
					try
					{
						_uxTable.Controls["uxDescriptionBit" + bitIdx.ToString()].Text = CurrentRegister.BitFieldName[bitIdx];
					}
					catch (Exception)
					{
						_uxTable.Controls["uxDescriptionBit" + bitIdx.ToString()].Text = "";
					}
					_uxTable.Controls["uxLedBit" + bitIdx.ToString()].Enabled = CurrentRegister.Type.IsRW();
				}
				_uxTable.Controls["uxType"].Text = CurrentRegister.Type.Rw;
				_uxComment.Text = CurrentRegister.Comment;
				if (RegisterChange != null)
					RegisterChange(CurrentRegister);
			}
		}

		public void SetLedsValue(byte value)
		{
			for (int i = 0; i < 7; i++)
			{
				CheckBox box = (CheckBox)_uxTable.Controls["uxLedBit" + i.ToString()];
				box.Checked = (value & Convert.ToByte(Math.Pow(2.0, (double)i))) != 0;
				box.BackColor = box.Checked ? Color.Green : Color.Red;
				box.Text = box.Checked ? "1" : "0";
			}
			_uxTable.Controls["uxValueBit07"].Text = string.Format("0x{0:X2}", value);
		}

		public void SetLedsValue(int[] bits, int offset)
		{
			for (int i = 0; i <= 7; i++)
			{
				CheckBox box = (CheckBox)_uxTable.Controls["uxLedBit" + i.ToString()];
				box.Checked = bits[i + offset] > 0;
				box.BackColor = box.Checked ? Color.Green : Color.Red;
				box.Text = box.Checked ? "1" : "0";
			}
			_uxTable.Controls["uxValueBit07"].Text = string.Format("0x{0:X2}", bits[0]);
		}

		public int CurrentRegisterAddress
		{
			get { return _currentRegisterAddress; }
			set { _currentRegisterAddress = value; }
		}

		public Dictionary<int, RegisterSTB> RegisterMap
		{
			get { return _registerMap; }
			set { _registerMap = value; }
		}
	}
}
