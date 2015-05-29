using Sandbox.ModAPI;
using Sandbox.ModAPI.Interfaces;
using Sandbox.Common;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Common.ObjectBuilders.Voxels;
using Sandbox.Common.ObjectBuilders.Definitions;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ConquestPlugin.ProcessHandlers
{
	using ConquestPlugin.GameModes;
	using ConquestPlugin.Utility;

	class ProcessFactionPoints : ProcessHandlerBase
	{
		public override int GetUpdateResolution()
		{
			return 300000; // 1 Hour = 3600000
		}

		public override void Handle()
		{
			// * Faction Points System: Every 60 minutes, Give each faction (owned asteroids) number of credits. Save faction balances to file.
			// ----------------------

			MyObjectBuilder_FactionCollection factionlist = MyAPIGateway.Session.GetWorld().Checkpoint.Factions;
			int num_factions = 50; //get number of factions
			ulong[,] fcleaderboard = new ulong[num_factions, 2];
			foreach (MyObjectBuilder_Faction faction in factionlist.Factions)
			{
				int faction_score = 0;
				List<MyObjectBuilder_FactionMember> currentfaction = faction.Members;
				foreach (MyObjectBuilder_FactionMember currentmember in currentfaction)
				{
					var leaders = GMConquest.Instance.Leaderboard.GroupBy(x => x.Value).Select(group => new { group.Key, Total = group.Count() }).OrderByDescending(x => x.Total);
					foreach (var p in leaders)
					{

						if (p.Key == currentmember.PlayerId)
						{
							faction_score += p.Total;
						}
					}
				}
				// Add faction_score to factions current credits.
				FactionPoints.AddFP(faction.FactionId,faction_score);
			}
			// ----------------------
			base.Handle();
		}
	}
}
