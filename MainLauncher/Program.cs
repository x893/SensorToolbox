﻿namespace MainLauncher
{
    using System;
    using System.Windows.Forms;

    internal static class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            string par = string.Empty;
            if (args.Length > 0)
                par = args[0];

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Initial(par));
        }
    }
}

