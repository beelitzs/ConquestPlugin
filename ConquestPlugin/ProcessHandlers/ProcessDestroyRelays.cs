using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using ConquestPlugin;
using ConquestPlugin.Utility;
using NLog;
using VRage.ModAPI;
using VRageMath;
namespace ConquestPlugin.ProcessHandlers
{
	class ProcessDestroyRelays : ProcessHandlerBase
	{
		public override int GetUpdateResolution()
		{
			return 7000; // Update in ms.
		}
		public override void Handle()
		{
            try
            {
                HashSet<IMyEntity> entities = new HashSet<IMyEntity>();
                MyAPIGateway.Entities.GetEntities(entities);

                foreach (IMyEntity oneEntity in entities)
                {
                    if ((oneEntity is IMyVoxelMap)) // !
                        continue;

                    if (!oneEntity.Save)
                        continue;

                    if (oneEntity.DisplayName.Contains("ommRelayOutpu"))
                    {
                        //oneEntity.Close();
						if (!Conquest.CommRelayCleanup.Contains(oneEntity))
						{
							Conquest.CommRelayCleanup.Add(oneEntity);
						}
                    }
                }
            }
            catch(InvalidOperationException)
            {
                Log.Info("Error Removing Relays");
            }
			try
			{
				if (Conquest.CommRelayCleanup.Count > 1)
				{
					// Delete the oldest entity in the list.
					IMyEntity closeMe = Conquest.CommRelayCleanup[0];
					Conquest.CommRelayCleanup.Remove(closeMe);
					closeMe.Close();
				}
			}
			catch (NullReferenceException)
			{
				// List is empty. Continue on.
			}
			base.Handle();
		}
	}
}
