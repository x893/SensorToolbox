﻿namespace Freescale.SASD.Communication
{
    using System;
    using System.IO;
    using System.Runtime.InteropServices;

    public static class STBLogger
    {
        public static void AddEvent(object sender, EventLevel level, string name, string description)
        {
            try
            {
                StreamWriter writer = File.AppendText("STBEvents.html");
                try
                {
                    string str = string.Format("<table border=1><tr><td>{0:G}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td></tr></table>", new object[] { DateTime.Now, level.ToString(), sender.ToString(), name, description });
                    writer.WriteLine(str);
                }
                finally
                {
                    writer.Close();
                }
            }
            catch (Exception)
            {
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Event
        {
            private STBLogger.EventLevel Level;
            private string Module;
            private string Name;
            private string Description;
            private DateTime Timestamp;
        }

        public enum EventLevel
        {
            Error = 3,
            Information = 1,
            Invalid = 4,
            Warning = 2
        }
    }
}

