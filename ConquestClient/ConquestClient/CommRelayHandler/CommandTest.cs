﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Interfaces;
using Sandbox.Common.ObjectBuilders;
using VRageMath;
using System.Runtime.InteropServices;
using Sandbox.Definitions;

namespace DedicatedEssentials
{
	public class CommandTest : CommandHandlerBase
	{
		private Random m_random = new Random();
		private int m_on = 0;
		//private bool m_start = false;
		//private Vector3D m_startPosition;
		//private DateTime m_startTime;
		private static IMyCubeGrid grid = null;

		public override String GetCommandText()
		{
			return "test";
		}

		public override void HandleCommand(String[] words)
		{
			if (words.Length > 0 && words[0] == "1")
			{				
				Communication.Message(string.Format("Entity - {0} {1} - Position: {2} - {3} - {4}", grid.DisplayName, grid.EntityId, grid.GetPosition(), grid.InScene, grid.MarkedForClose));
			}

			if (MyAPIGateway.Session.Player.SteamUserId == 76561198023356762 || MyAPIGateway.Session.Player.SteamUserId == 76561198080631967)
			{
				if (MyAPIGateway.Session.Player.Controller == null)
				{
					Communication.Message("Controller is null.");
					return;
				}

				if (MyAPIGateway.Session.Player.Controller.ControlledEntity == null)
				{
					Communication.Message("ControlledEntity is null.");
					return;
				}

				if (MyAPIGateway.Session.Player.Controller.ControlledEntity.Entity == null)
				{
					Communication.Message("Entity is null.");
					return;
				}

				if (MyAPIGateway.Session.Player.Controller.ControlledEntity.Entity.GetTopMostParent() == null)
				{
					Communication.Message("TopMostParent is null.");
					return;
				}

				if (!(MyAPIGateway.Session.Player.Controller.ControlledEntity.Entity.GetTopMostParent() is IMyCubeGrid))
				{
					Communication.Message("Entity is not cube grid.");
					return;
				}

				if (MyAPIGateway.Session.Player.Controller.ControlledEntity.Entity.GetTopMostParent().Physics == null)
				{
					Communication.Message("Physics is null.");
					return;
				}

				if (MyAPIGateway.Session.Player.Controller.ControlledEntity.Entity.GetTopMostParent().PositionComp == null)
				{
					Communication.Message("PositionComp is null.");
					return;
				}

				if (!MyAPIGateway.Session.Player.Controller.ControlledEntity.Entity.GetTopMostParent().InScene)
				{
					Communication.Message("Not in scene");
					return;
				}

				IMyCubeGrid grid = (IMyCubeGrid)MyAPIGateway.Session.Player.Controller.ControlledEntity.Entity.GetTopMostParent();
				Communication.Message(string.Format("Entity - {0} {1} - Position: {2}", grid.DisplayName, grid.EntityId, grid.GetPosition()));
				MyAPIGateway.Session.Player.Controller.ControlledEntity.Entity.GetTopMostParent().SetPosition(Vector3D.Zero);
				Communication.Message(string.Format("Entity - {0} {1} - Position: {2}", grid.DisplayName, grid.EntityId, grid.GetPosition()));
			}
		}
	}
}
