using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Runtime.InteropServices;

using NLog;
using SEModAPI;
using SEModAPIExtensions;
using SEModAPIExtensions.API.Plugin;
using SEModAPIInternal;

namespace ConquestPlugin
{
    public class Conquest : IPlugin
    {
		public static Logger Log;
		private static Conquest _instance;
		private Thread _pluginThread;
		public void Init()
		{
			Log.Info("Plugin '{0}' initialized. (Version: {1}  ID: {2})", Name, Version, Id);
		}

		public void Shutdown()
		{

		}

		public void Update()
		{

		}

		#region IPlugin Members

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
