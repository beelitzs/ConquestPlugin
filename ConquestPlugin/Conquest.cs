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
using SEModAPI.API.Utility;
using SEModAPIExtensions.API;
using SEModAPIExtensions.API.Plugin;
using SEModAPIExtensions.API.Plugin.Events;
using SEModAPIInternal.Support;
using SEModAPIInternal.API.Common;

using ConquestPlugin.ProcessHandlers;
using ConquestPlugin.ChatHandlers;
using ConquestPlugin.Utility;

namespace ConquestPlugin
{
    public class Conquest : IPlugin, IChatEventHandler
    {
		public static Logger Log;
		internal static Conquest Instance;
		private static Conquest _instance;
		private static string _pluginPath;
		private Thread _processThread;
		private List<Thread> _processThreads;
		private bool _running = true;
		private DateTime m_lastProcessUpdate;
		private List<ProcessHandlerBase> _processHandlers;
		private List<ChatHandlerBase> _chatHandlers;

		#region Properties

		public static string PluginPath
		{
			get { return _pluginPath; }
			set { _pluginPath = value; }
		}
		
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
			DoInit(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\");
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

		#region Initialise Plugin

		private void DoInit(string path)
		{
			Instance = this;
			_pluginPath = path;
			
			// Load settings here.

			_processHandlers = new List<ProcessHandlerBase>
			{
				new ProcessFactionPoints(),
				new ProcessConquest()
			};

			_chatHandlers = new List<ChatHandlerBase>
			{
				new HandleAdminDebug(),
				new HandleShop(),
				new HandleLeaderboardConquest(),
				new HandleLeaderboardFaction()
			};

			_processThreads = new List<Thread>();
			_processThread = new Thread(ProcessLoop);
			_processThread.Start();
		}

		#endregion

		#region Process Loop

		private void ProcessLoop()
		{
			try
			{
				foreach (ProcessHandlerBase handler in _processHandlers)
				{
					ProcessHandlerBase currentHandler = handler;
					Thread thread = new Thread(() =>
						{
							while (_running)
							{
								if (currentHandler.CanProcess())
								{
									try
									{
										currentHandler.Handle();
									}
									catch (Exception ex)
									{
										Log.Error("Thread Exception: {0} - {1}", currentHandler.GetUpdateResolution(), ex);
									}
									currentHandler.LastUpdate = DateTime.Now;
								}
								Thread.Sleep(100);
							}
						});
					_processThreads.Add(thread);
					thread.Start();
				}
				foreach (Thread thread in _processThreads) { thread.Join(); }

				while (true)
				{
					if (DateTime.Now - m_lastProcessUpdate > TimeSpan.FromMilliseconds(100))
					{
						m_lastProcessUpdate = DateTime.Now;
					}
					Thread.Sleep(25);
				}
			}
			catch (Exception ex)
			{
				Log.Error(ex);
			}
		}
		#endregion

		#region IChatEventHandler Members

		public void OnChatReceived(ChatManager.ChatEvent obj)
		{
			if (obj.Message[0] != '/')
				return;

			HandleChatMessage(obj.SourceUserId, obj.Message);
		}

		public void HandleChatMessage(ulong steamId, string message)
		{
			// Parse chat message
			ulong remoteUserId = steamId;
			List<string> commandParts = CommandParser.GetCommandParts(message);

			// User wants some help
			if (commandParts[0].ToLower() == "/help")
			{
				HandleHelpCommand(remoteUserId, commandParts);
				return;
			}

			// See if we have any valid handlers
			bool handled = false;
			foreach (ChatHandlerBase chatHandler in _chatHandlers)
			{
				int commandCount = 0;
				if (remoteUserId == 0 && !chatHandler.AllowedInConsole())
					continue;

				if (chatHandler.CanHandle(remoteUserId, commandParts.ToArray(), ref commandCount))
				{
					try
					{
						chatHandler.HandleCommand(remoteUserId, commandParts.Skip(commandCount).ToArray());
					}
					catch (Exception ex)
					{
						Log.Info(string.Format("ChatHandler Error: {0}", ex));
					}

					handled = true;
				}
			}

			if (!handled)
			{
				DisplayAvailableCommands(remoteUserId, message);
			}
		}

		/// <summary>
		/// This function displays available help for all the functionality of this plugin
		/// </summary>
		/// <param name="remoteUserId"></param>
		/// <param name="commandParts"></param>
		private void HandleHelpCommand(ulong remoteUserId, IReadOnlyCollection<string> commandParts)
		{
			if (commandParts.Count == 1)
			{
				List<string> commands = new List<string>();
				foreach (ChatHandlerBase handler in _chatHandlers)
				{
					// We should replace this to just have the handler return a string[] of base commands
					if (handler.GetMultipleCommandText().Length < 1)
					{
						string commandBase = handler.GetCommandText().Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries).First();
						if (!commands.Contains(commandBase) && (!handler.IsClientOnly()) && (!handler.IsAdminCommand() || (handler.IsAdminCommand() && (PlayerManager.Instance.IsUserAdmin(remoteUserId) || remoteUserId == 0))))
						{
							commands.Add(commandBase);
						}
					}
					else
					{
						foreach (string cmd in handler.GetMultipleCommandText())
						{
							string commandBase = cmd.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries).First();
							if (!commands.Contains(commandBase) && (!handler.IsClientOnly()) && (!handler.IsAdminCommand() || (handler.IsAdminCommand() && (PlayerManager.Instance.IsUserAdmin(remoteUserId) || remoteUserId == 0))))
							{
								commands.Add(commandBase);
							}
						}
					}
				}

				string commandList = string.Join(", ", commands);
				string info = string.Format("Dedicated Server Essentials v{0}.  Available Commands: {1}", Version, commandList);
				ChatUtil.SendPrivateChat(remoteUserId, info);
			}
			else
			{
				string helpTarget = string.Join(" ", commandParts.Skip(1).ToArray());
				bool found = false;
				foreach (ChatHandlerBase handler in _chatHandlers)
				{
					// Again, we should get handler to just return string[] of command Text
					if (handler.GetMultipleCommandText().Length < 1)
					{
						if (String.Equals(handler.GetCommandText(), helpTarget, StringComparison.CurrentCultureIgnoreCase))
						{
							ChatUtil.SendPrivateChat(remoteUserId, handler.GetHelp());
							found = true;
						}
					}
					else
					{
						foreach (string cmd in handler.GetMultipleCommandText())
						{
							if (String.Equals(cmd, helpTarget, StringComparison.CurrentCultureIgnoreCase))
							{
								ChatUtil.SendPrivateChat(remoteUserId, handler.GetHelp());
								found = true;
							}
						}
					}
				}

				if (!found)
				{
					List<string> helpTopics = new List<string>();

					foreach (ChatHandlerBase handler in _chatHandlers)
					{
						// Again, cleanup to one function
						string[] multipleCommandText = handler.GetMultipleCommandText();
						if (multipleCommandText.Length == 0)
						{
							if (handler.GetCommandText().ToLower().StartsWith(helpTarget.ToLower()) && ((!handler.IsAdminCommand()) || (handler.IsAdminCommand() && (PlayerManager.Instance.IsUserAdmin(remoteUserId) || remoteUserId == 0))))
							{
								helpTopics.Add(handler.GetCommandText().ToLower().Replace(helpTarget.ToLower(), string.Empty));
							}
						}
						else
						{
							foreach (string cmd in multipleCommandText)
							{
								if (cmd.ToLower().StartsWith(helpTarget.ToLower()) && ((!handler.IsAdminCommand()) || (handler.IsAdminCommand() && (PlayerManager.Instance.IsUserAdmin(remoteUserId) || remoteUserId == 0))))
								{
									helpTopics.Add(cmd.ToLower().Replace(helpTarget.ToLower(), string.Empty));
								}
							}
						}
					}

					if (helpTopics.Any())
					{
						ChatUtil.SendPrivateChat(remoteUserId, string.Format("Help topics for command '{0}': {1}", helpTarget.ToLower(), string.Join(",", helpTopics.ToArray())));
						found = true;
					}
				}

				if (!found)
					ChatUtil.SendPrivateChat(remoteUserId, "Unknown command");
			}
		}

		/// <summary>
		/// Displays the available commands for the command entered
		/// </summary>
		/// <param name="remoteUserId"></param>
		/// <param name="recvMessage"></param>
		private void DisplayAvailableCommands(ulong remoteUserId, string recvMessage)
		{
			string message = recvMessage.ToLower().Trim();
			List<string> availableCommands = new List<string>();
			foreach (ChatHandlerBase chatHandler in _chatHandlers)
			{
				// Cleanup to one function
				if (chatHandler.GetMultipleCommandText().Length < 1)
				{
					string command = chatHandler.GetCommandText();
					if (command.StartsWith(message))
					{
						string[] cmdPart = command.Replace(message, string.Empty).Trim().Split(new[] { ' ' });

						if (!availableCommands.Contains(cmdPart[0]))
							availableCommands.Add(cmdPart[0]);
					}
				}
				else
				{
					foreach (string command in chatHandler.GetMultipleCommandText())
					{
						if (command.StartsWith(message))
						{
							string[] cmdPart = command.Replace(message, string.Empty).Trim().Split(new[] { ' ' });

							if (!availableCommands.Contains(cmdPart[0]))
								availableCommands.Add(cmdPart[0]);
						}
					}
				}
			}

			if (availableCommands.Any())
			{
				ChatUtil.SendPrivateChat(remoteUserId, string.Format("Available subcommands for '{0}' command: {1}", message, string.Join(", ", availableCommands.ToArray())));
			}
		}

		public void OnChatSent(ChatManager.ChatEvent obj)
		{

		}

		#endregion
	}

}