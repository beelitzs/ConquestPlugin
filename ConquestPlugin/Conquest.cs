using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Runtime.InteropServices;
using System.ComponentModel;

using NLog;
using SEModAPI.API;
using SEModAPIExtensions.API;
using SEModAPIExtensions.API.Plugin;
using SEModAPIInternal.Support;

namespace ConquestPlugin
{
    public class Conquest : IPlugin
    {
		public static Logger Log;
		private static Conquest _instance;
		private Thread _processThread;
		private List<Thread> _processThreads;
		private bool _running = true;
		private DateTime m_lastProcessUpdate;

		#region Properties
		
		[Category("Options")]
		[Description("Test Option")]
		[Browsable(true)]
		[ReadOnly(false)]
		public string TestOption
		{
			get { return "Get String"; }
			set { /* Set Code */ }
		}


		#endregion

		#region IPlugin Members

		public void Init()
		{
			Log.Info("Plugin '{0}' initialized. (Version: {1}  ID: {2})", Name, Version, Id);
		}

		public void Shutdown()
		{
			Log.Info("Shutting down {0}", Name);
		}

		public void Update()
		{

		}

		public Guid Id
		{
			get
			{
				GuidAttribute guidAttr = (GuidAttribute)typeof(Conquest).Assembly.GetCustomAttributes(typeof(GuidAttribute), true)[0];
				return new Guid(guidAttr.Value);
			}
		}

		public string Name
		{
			get
			{
				return "SE Conquest Gamemode";
			}
		}

		public Version Version
		{
			get
			{
				return typeof(Conquest).Assembly.GetName().Version;
			}
		}

		#endregion


	}

}