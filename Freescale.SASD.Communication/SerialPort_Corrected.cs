﻿namespace Freescale.SASD.Communication
{
    using System;
    using System.IO.Ports;

    public class SerialPort_Corrected : SerialPort
    {
        public new void Dispose()
        {
            Dispose(true);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (base.Container != null))
                base.Container.Dispose();
			try
			{
				GC.ReRegisterForFinalize(base.BaseStream);
			}
			catch { }
            base.Dispose(disposing);
        }

        public new void Open()
        {
			try
			{
				base.Open();
				GC.SuppressFinalize(base.BaseStream);
			}
			catch { }
        }
    }
}

