﻿namespace Freescale.SASD.Communication
{
    using System;
    using System.IO.Ports;

    public class CommClass
    {
        private CommClass LowerLayerObj;

        public virtual void GetCommObj(ref CommClass a)
        {
            if (a.GetType() == base.GetType())
            {
                a = this;
            }
            else
            {
                if (LowerLayerObj == null)
                {
                    a = null;
                    throw new Exception("The object requested was not available in the Communication Driver");
                }
                LowerLayerObj.GetCommObj(ref a);
            }
        }

        public virtual void GetCommObj(ref SerialPort a)
        {
            if (LowerLayerObj == null)
            {
                a = null;
                throw new Exception("The object requested was not available in the Communication Driver");
            }
            LowerLayerObj.GetCommObj(ref a);
        }

        protected void SetLowerLayerObj(CommClass a)
        {
            LowerLayerObj = a;
        }
    }
}

