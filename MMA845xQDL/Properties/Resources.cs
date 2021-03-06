﻿using System.Diagnostics;
using System.CodeDom.Compiler;
using System.Globalization;
using System.Resources;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
namespace VeyronDatalogger.Properties
{
	[DebuggerNonUserCode, GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "2.0.0.0"), CompilerGenerated]
	public class Resources
	{
		// Fields
		private static CultureInfo resourceCulture;
		private static ResourceManager resourceMan;

		// Methods
		public Resources()
		{
		}

		// Properties
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
				return (Icon)ResourceManager.GetObject("freelogo", resourceCulture);
			}
		}

		public static Bitmap FSL
		{
			get
			{
				return (Bitmap)ResourceManager.GetObject("FSL", resourceCulture);
			}
		}

		public static Bitmap GreenState
		{
			get
			{
				return (Bitmap)ResourceManager.GetObject("GreenState", resourceCulture);
			}
		}

		public static Bitmap RedState
		{
			get
			{
				return (Bitmap)ResourceManager.GetObject("RedState", resourceCulture);
			}
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static ResourceManager ResourceManager
		{
			get
			{
				if (object.ReferenceEquals(resourceMan, null))
				{
					ResourceManager manager = new ResourceManager("VeyronDatalogger.Properties.Resources", typeof(Resources).Assembly);
					resourceMan = manager;
				}
				return resourceMan;
			}
		}

		public static Bitmap STB_TOPBAR_LARGE
		{
			get
			{
				return (Bitmap)ResourceManager.GetObject("STB_TOPBAR_LARGE", resourceCulture);
			}
		}

		public static Bitmap YellowState
		{
			get
			{
				return (Bitmap)ResourceManager.GetObject("YellowState", resourceCulture);
			}
		}
	}
}