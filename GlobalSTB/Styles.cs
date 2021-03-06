﻿using NationalInstruments.UI.WindowsForms;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace GlobalSTB
{
	public class Styles
	{
		private static string[] _exceptions;

		public static Color StandardBackColor = Color.LightSlateGray;
		public static Color StandardColor = Color.White;

		public static Color BackColorGroupBox = StandardBackColor;
		public static Color BackColorPanel = StandardBackColor;
		public static Color BackColorTabPage = StandardBackColor;

		public static Color ColorCheckBox = StandardColor;
		public static Color ColorRadioButton = StandardColor;

		public const string DefaultFontName = "Segoe UI"; //!!! "Cambria"
		public const float DefaultFont9 = 9.25f;
		public const float DefaultFont10 = 9.5f;
		public const float DefaultFont11 = 11.0f;

		public static Font FontButton = new Font(DefaultFontName, DefaultFont9, FontStyle.Bold, GraphicsUnit.Point, 0);
		public static Font FontCheckBox = new Font(DefaultFontName, DefaultFont10, FontStyle.Regular, GraphicsUnit.Point, 0);
		public static Font FontCombo = new Font(DefaultFontName, DefaultFont10, FontStyle.Regular, GraphicsUnit.Point, 0);
		public static Font FontForm = new Font(DefaultFontName, DefaultFont10, FontStyle.Regular, GraphicsUnit.Point, 0);
		public static Font FontGroupBox = new Font(DefaultFontName, DefaultFont10, FontStyle.Bold, GraphicsUnit.Point, 0);
		public static Font FontLabel = new Font(DefaultFontName, DefaultFont10, FontStyle.Regular, GraphicsUnit.Point, 0);
		public static Font FontPanel = new Font(DefaultFontName, DefaultFont10, FontStyle.Bold, GraphicsUnit.Point, 0);
		public static Font FontRadioButton = new Font(DefaultFontName, DefaultFont10, FontStyle.Regular, GraphicsUnit.Point, 0);
		public static Font FontTabControl = new Font(DefaultFontName, DefaultFont11, FontStyle.Bold, GraphicsUnit.Point, 0);
		public static Font FontTabPage = new Font(DefaultFontName, DefaultFont10, FontStyle.Bold, GraphicsUnit.Point, 0);

		public static void FormatButton(Button b)
		{
			b.Font = FontButton;
			b.ForeColor = Color.Black;
		}

		public static void FormatCheckBox(CheckBox check)
		{
			check.Font = FontCheckBox;
			check.ForeColor = ColorCheckBox;
		}

		public static void FormatCombo(ComboBox c)
		{
			c.Font = FontCombo;
			c.BackColor = Color.White;
		}

		public static void FormatControl(Control c)
		{
			if (_exceptions == null || !_exceptions.Contains<string>(c.Name))
			{
				if (c is CheckBox)
					FormatCheckBox((CheckBox)c);
				else if (c is RadioButton)
					FormatRadioButton((RadioButton)c);
				else if (c is Label)
					FormatLabel((Label)c);
				else if (c is Button)
					FormatButton((Button)c);
				else if (c is TabPage)
					FormatTabPage((TabPage)c);
				else if (c is Panel)
					FormatPanel((Panel)c);
				else if (c is GroupBox)
					FormatGroupBox((GroupBox)c);
				else if (c is ComboBox)
					FormatCombo((ComboBox)c);
				else if (c is TabControl)
					FormatTabControl((TabControl)c);
				else if (c is WaveformGraph)
					FormatWaveform((WaveformGraph)c);
			}
		}

		public static void FormatForm(Form f)
		{
			f.SuspendLayout();
			_exceptions = null;
			foreach (Control control in f.Controls)
				FormatControl(control);
			f.ResumeLayout(true);
		}

		public static void FormatForm(Form f, string[] exceptions)
		{
			f.SuspendLayout();
			_exceptions = exceptions;
			foreach (Control control in f.Controls)
				FormatControl(control);
			f.ResumeLayout();
		}

		public static void FormatGroupBox(GroupBox box)
		{
			foreach (Control control in box.Controls)
				FormatControl(control);
			box.Font = FontGroupBox;
			box.ForeColor = StandardColor;
			box.BackColor = BackColorGroupBox;
		}

		public static void FormatInterruptPanel(Panel p)
		{
			ColorRadioButton = Color.Gold;
			BackColorPanel = BackColorTabPage = BackColorGroupBox = Color.Transparent;

			p.SuspendLayout();
			foreach (Control control in p.Controls)
			{
				FormatControl(control);
				if (control is GroupBox)
					control.ForeColor = Color.SandyBrown;
				else if (control is Panel)
					control.BackColor = Color.Black;
			}
			p.Font = FontPanel;
			p.ForeColor = StandardColor;
			p.BackColor = Color.Black;
			p.ResumeLayout(true);

			ColorRadioButton =
			BackColorPanel =
			BackColorTabPage =
			BackColorGroupBox = StandardBackColor;
		}

		public static void FormatLabel(Label l)
		{
			l.Font = FontLabel;
			l.ForeColor = StandardColor;
		}

		public static void FormatPanel(Panel p)
		{
			foreach (Control control in p.Controls)
				FormatControl(control);
			p.Font = FontPanel;
			p.ForeColor = StandardColor;
			p.BackColor = BackColorPanel;
		}

		public static void FormatRadioButton(RadioButton rb)
		{
			rb.Font = FontRadioButton;
			rb.ForeColor = ColorRadioButton;
			rb.BackColor = Color.Transparent;
		}

		public static void FormatTabControl(TabControl p)
		{
			foreach (Control control in p.Controls)
				FormatControl(control);
			p.Font = FontTabControl;
			p.ForeColor = Color.Black;
			p.BackColor = StandardBackColor;
		}

		public static void FormatTabPage(TabPage p)
		{
			foreach (Control control in p.Controls)
				FormatControl(control);
			p.Font = FontTabPage;
			p.ForeColor = StandardColor;
			p.BackColor = BackColorTabPage;
		}

		public static void FormatWaveform(WaveformGraph w)
		{
			w.PlotAreaColor = Color.SandyBrown;
		}
	}
}