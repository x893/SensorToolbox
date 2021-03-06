﻿namespace MainLauncher.Properties
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Globalization;
    using System.Resources;
    using System.Runtime.CompilerServices;

    [DebuggerNonUserCode, CompilerGenerated, GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "2.0.0.0")]
    public class Resources
    {
        private static CultureInfo resourceCulture;
        private static System.Resources.ResourceManager resourceMan;

		public Resources()
        {
        }

		public static Bitmap close
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("close", resourceCulture);
            }
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
		public static CultureInfo Culture
        {
            get
            {
                return resourceCulture;
            }
            set
            {
                resourceCulture = value;
            }
        }

		public static Icon freelogo
        {
            get
            {
                return (Icon) ResourceManager.GetObject("freelogo", resourceCulture);
            }
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
		public static System.Resources.ResourceManager ResourceManager
        {
            get
            {
                if (object.ReferenceEquals(resourceMan, null))
                {
                    System.Resources.ResourceManager manager = new System.Resources.ResourceManager("MainLauncher.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = manager;
                }
                return resourceMan;
            }
        }

		public static Bitmap stb
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("stb", resourceCulture);
            }
        }
    }
}

