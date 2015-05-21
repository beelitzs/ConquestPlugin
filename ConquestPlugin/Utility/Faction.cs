using Sandbox.Common;
using Sandbox.ModAPI;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Common.ObjectBuilders.Definitions;
using System.Collections.Generic;
using SEModAPIInternal.API.Common;

namespace EssentialsPlugin.Utility
{
	public class Faction
	{
		public static long getFactionID(ulong steamId)
		{
			long playerID = PlayerMap.Instance.GetPlayerIdsFromSteamId(steamId)[0];
			MyObjectBuilder_FactionCollection factioncollection = MyAPIGateway.Session.GetWorld().Checkpoint.Factions;
			foreach (MyObjectBuilder_Faction faction in factioncollection.Factions)
			{
				List<MyObjectBuilder_FactionMember> currentfaction = faction.Members;
				foreach (MyObjectBuilder_FactionMember currentmember in currentfaction)
				{
					Communication.SendPublicInformation(string.Format("[DEBUG]: CurrentMember.PlayerId = {0}.", currentmember.PlayerId));
					if (currentmember.PlayerId == playerID)
					{
						Communication.SendPublicInformation(string.Format("[DEBUG]: Faction Found." + faction.FactionId));
						return faction.FactionId;   
					}
				}
			}
			Communication.SendPublicInformation(string.Format("[DEBUG]: Faction Find Fail."));
			return 0;
		}
        public static MyObjectBuilder_Faction getFaction(long factionID)
        {
            MyObjectBuilder_FactionCollection factionlist = MyAPIGateway.Session.GetWorld().Checkpoint.Factions;
            foreach(MyObjectBuilder_Faction faction in factionlist.Factions)
            {
                if(faction.FactionId == factionID)
                {
                    return faction;
                }
            }
            return null;
        }
       

	}
}