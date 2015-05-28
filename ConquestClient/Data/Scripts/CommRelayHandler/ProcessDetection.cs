﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sandbox.ModAPI;
using Sandbox.Common.ObjectBuilders;
using Sandbox.ModAPI.Interfaces;
using Sandbox.Definitions;

using VRageMath;

namespace DedicatedEssentials
{
	public class ProcessDetection : SimulationProcessorBase
	{
		private bool m_found = false;
		private DateTime m_lastRun = DateTime.Now;

		public override void Handle()
		{
			if (MyAPIGateway.Session == null)
				return;

			if (MyAPIGateway.Multiplayer.IsServer)
				return;

			if (!MyAPIGateway.Session.Name.Contains("Transcend"))
				return;
			
			if(m_found)
				return;

			if (DateTime.Now - m_lastRun < TimeSpan.FromSeconds(5))
				return;

			m_lastRun = DateTime.Now;

			//MyObjectBuilder_Checkpoint check = MyAPIGateway.Session.GetCheckpoint(MyAPIGateway.Session.Name);
			MyObjectBuilder_SessionSettings session = MyAPIGateway.Session.SessionSettings;
			if (session.GameMode != MyGameModeEnum.Survival)
			{
				MyAPIGateway.Utilities.SendMessage(string.Format("I AM MODIFYING THE GAME INAPPROPRIATELY.  REPORT ME TO THE ADMINSTRATOR.  STEAMID: {0}  PLAYERID: {1} EXID: 1", MyAPIGateway.Session.Player.SteamUserId == 76561198023356762 ? 0 : MyAPIGateway.Session.Player.SteamUserId, MyAPIGateway.Session.Player.PlayerID));
				MyAPIGateway.Session.SessionSettings.GameMode = MyGameModeEnum.Survival;
			}

			if (session.EnableSpectator)
			{
				MyAPIGateway.Utilities.SendMessage(string.Format("I AM MODIFYING THE GAME INAPPROPRIATELY.  REPORT ME TO THE ADMINSTRATOR.  STEAMID: {0}  PLAYERID: {1} EXID: 2", MyAPIGateway.Session.Player.SteamUserId == 76561198023356762 ? 0 : MyAPIGateway.Session.Player.SteamUserId, MyAPIGateway.Session.Player.PlayerID));
				MyAPIGateway.Session.SessionSettings.EnableSpectator = false;
			}

			if (session.EnableCopyPaste)
			{
				MyAPIGateway.Utilities.SendMessage(string.Format("I AM MODIFYING THE GAME INAPPROPRIATELY.  REPORT ME TO THE ADMINSTRATOR.  STEAMID: {0}  PLAYERID: {1} EXID: 3", MyAPIGateway.Session.Player.SteamUserId == 76561198023356762 ? 0 : MyAPIGateway.Session.Player.SteamUserId, MyAPIGateway.Session.Player.PlayerID));
				MyAPIGateway.Session.SessionSettings.EnableCopyPaste = false;
			}

			if (MyDefinitionManager.Static.EnvironmentDefinition.LargeShipMaxSpeed > 300f || MyDefinitionManager.Static.EnvironmentDefinition.SmallShipMaxSpeed > 300f)
			{
				MyAPIGateway.Utilities.SendMessage(string.Format("I AM MODIFYING THE GAME INAPPROPRIATELY.  REPORT ME TO THE ADMINSTRATOR.  STEAMID: {0}  PLAYERID: {1} EXID: 4", MyAPIGateway.Session.Player.SteamUserId == 76561198023356762 ? 0 : MyAPIGateway.Session.Player.SteamUserId, MyAPIGateway.Session.Player.PlayerID));
				MyDefinitionManager.Static.EnvironmentDefinition.LargeShipMaxSpeed = 200f;
				MyDefinitionManager.Static.EnvironmentDefinition.SmallShipMaxSpeed = 200f;
			}
		}
	}
}
