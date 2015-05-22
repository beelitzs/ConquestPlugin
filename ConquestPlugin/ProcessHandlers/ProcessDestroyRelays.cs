using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox.ModAPI;
using ConquestPlugin.Utility;
namespace ConquestPlugin.ProcessHandlers
{
	class ProcessDestroyRelays : ProcessHandlerBase
	{
		public override int GetUpdateResolution()
		{
			return 20000; // Update in ms.
		}

		public override void Handle()
		{
			HashSet<IMyEntity> entities = new HashSet<IMyEntity>();
			MyAPIGateway.Entities.GetEntities(entities);

			foreach (IMyEntity oneEntity in entities)
			{
				if (!oneEntity.Save)
					continue;

				if (oneEntity.DisplayName.Contains("ommRelayOutpu"))
				{
					oneEntity.Close();
				}
			}
			base.Handle();
		}
	}
}
