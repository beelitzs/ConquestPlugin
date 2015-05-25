using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sandbox.ModAPI;
using Sandbox.Common.ObjectBuilders;

using VRageMath;

namespace DedicatedEssentials
{
	public class ProcessPosition : SimulationProcessorBase
	{
		private bool m_init = false;
		private DateTime m_lastRun = DateTime.Now;

		public override void Handle()
		{
			if (MyAPIGateway.Multiplayer.IsServer)
				return;

			if (!m_init)
			{
				m_init = true;
				Init();
			}

			if (DateTime.Now - m_lastRun < TimeSpan.FromMilliseconds(500))
				return;

			m_lastRun = DateTime.Now;
			if (MyAPIGateway.Utilities.GetObjectiveLine().Visible && Core.ShowPosition)
			{
				if(MyAPIGateway.Session.Player.Controller == null || MyAPIGateway.Session.Player.Controller.ControlledEntity == null || MyAPIGateway.Session.Player.Controller.ControlledEntity.Entity == null)
					return;

				Vector3D position = MyAPIGateway.Session.Player.Controller.ControlledEntity.Entity.GetPosition();
				if (MyAPIGateway.Utilities.GetObjectiveLine().Title != Core.ServerName)
					MyAPIGateway.Utilities.GetObjectiveLine().Title = Core.ServerName;

				MyAPIGateway.Utilities.GetObjectiveLine().Objectives[0] = string.Format("Position: X: {0:F0} Y: {1:F0} Z: {2:F0}", position.X, position.Y, position.Z);
			}
		}

		public void Init()
		{
			MyAPIGateway.Utilities.GetObjectiveLine().Title = "";
			MyAPIGateway.Utilities.GetObjectiveLine().Objectives.Clear();
			MyAPIGateway.Utilities.GetObjectiveLine().Objectives.Add("");
			MyAPIGateway.Utilities.GetObjectiveLine().Show();
		}
	}
}
