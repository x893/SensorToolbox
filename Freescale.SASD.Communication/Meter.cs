﻿namespace Freescale.SASD.Communication
{
    using System;
    using System.Collections;

    public static class Meter
    {
        private static ArrayList ADescription = new ArrayList();
        private static ArrayList AID = new ArrayList();
        private static ArrayList ASenders = new ArrayList();
        private static ArrayList AValue = new ArrayList();
        private static ArrayList AValueChange = new ArrayList();
        private static object Lock = new object();

        public static void CheckValue(string id, ref object o)
        {
            lock (Lock)
            {
                if (VarExists(id))
                {
                    o = AValue[AID.IndexOf(id)];
                }
                else
                {
                    o = null;
                }
            }
        }

        public static int CheckValueChanged(string id)
        {
            lock (Lock)
            {
                if (VarExists(id))
                {
                    return (int) AValueChange[AID.IndexOf(id)];
                }
                return -1;
            }
        }

        public static void GetIDs(ref string[] id)
        {
            lock (Lock)
            {
                id = new string[ASenders.Count];
                for (int i = 0; i < id.Length; i++)
                {
                    id[i] = (string) AID[i];
                }
            }
        }

        public static void GetSenders(ref object[] senders)
        {
            lock (Lock)
            {
                senders = new object[ASenders.Count];
                for (int i = 0; i < senders.Length; i++)
                {
                    senders[i] = ASenders[i];
                }
            }
        }

        public static void MonitorTimeStart(object sender, string id)
        {
            MonitorValue(sender, id, DateTime.Now);
        }

        public static void MonitorTimeStop(object sender, string id)
        {
            DateTime now = DateTime.Now;
            TimeSpan span = new TimeSpan();
            object o = new object();
            CheckValue(id, ref o);
            span = now.Subtract((DateTime) o);
            MonitorValue(sender, id + "_diff", span);
        }

        public static void MonitorValue(object sender, string id, object value)
        {
            lock (Lock)
            {
                if (!VarExists(id))
                {
                    VarAdd(sender, id);
                }
                VarValue(id, value);
            }
        }

        private static void VarAdd(object sender, string id)
        {
            AID.Add(id);
            ASenders.Add(sender);
            AValue.Add(null);
            AValueChange.Add(0);
            ADescription.Add("");
        }

        private static bool VarExists(string id)
        {
            return AID.Contains(id);
        }

        private static void VarValue(string id, object value)
        {
            AValue[AID.IndexOf(id)] = value;
            AValueChange[AID.IndexOf(id)] = ((int) AValueChange[AID.IndexOf(id)]) + 1;
        }
    }
}

