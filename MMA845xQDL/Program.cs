﻿namespace VeyronDatalogger
{
    using System;
    using System.Windows.Forms;

    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Datalogger mainForm = new Datalogger();
            Application.Run(mainForm);
            mainForm.EndDemo();
        }
    }
}

