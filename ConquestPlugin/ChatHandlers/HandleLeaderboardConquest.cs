using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;

using EssentialsPlugin.Utility;
using EssentialsPlugin.GameModes;

using Sandbox.ModAPI;
using Sandbox.ModAPI.Interfaces;
using Sandbox.Common;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Common.ObjectBuilders.Voxels;
using Sandbox.Common.ObjectBuilders.Definitions;

using VRageMath;

using SEModAPIInternal.API.Entity;
using SEModAPIInternal.API.Entity.Sector.SectorObject;
using SEModAPIInternal.API.Entity.Sector.SectorObject.CubeGrid.CubeBlock;
using SEModAPIInternal.API.Common;

namespace ConquestPlugin.ChatHandlers
{
	public class HandleLeaderboardConquest : ChatHandlerBase
	{
		public override string GetHelp()
		{
			return "This displays the leaderboard for the conquest game mode.  Usage: /leaderboard conquest";
		}

		public override string GetCommandText()
		{
			return "/leaderboard conquest";
		}

		public override bool IsAdminCommand()
		{
			return false;
		}

		public override bool AllowedInConsole()
		{
			return true;
		}

		public override bool HandleCommand(ulong userId, string[] words)
		{
			var board = Conquest.Instance.Leaderboard;

			string leaderResult = "";
			
			foreach (var p in board)
			{
				// leaderResult += string.Format("Here: {0} - {1}", p.Key, p.Value);
			}
			// leaderResult += "\r\n";
			
			var leaders = Conquest.Instance.Leaderboard.GroupBy(x => x.Value).Select(group => new { group.Key, Total = group.Count() }).OrderByDescending(x => x.Total);
			int position = 1;
			foreach (var p in leaders)
			{
				if (leaderResult != "")
					leaderResult += "\r\n";				

				MyObjectBuilder_Checkpoint.PlayerItem item = PlayerMap.Instance.GetPlayerItemFromPlayerId(p.Key);
				leaderResult += string.Format("#{0}: {1} with {2} asteroids", position, item.Name, p.Total);
				position++;
			}

			leaderResult += "\r\n\r\n";

			long playerId = PlayerMap.Instance.GetFastPlayerIdFromSteamId(userId);
			int playerCount = 0;
			var playerItem = leaders.FirstOrDefault(x => x.Key == playerId);
			if(playerItem != null)
			{
				playerCount = playerItem.Total;
			}

			leaderResult += string.Format("You currently have {0} owned asteroids.", playerCount);

			// Communication.SendPrivateInformation(userId, leaderResult); // Bulky and unecessary?
			Communication.DisplayDialog(userId, "Conquest Leaderboard", "Current Leaders", leaderResult);

			return true;
		}
	}
   
	// Faction Leaderboard
	public class HandleLeaderboardFaction : ChatHandlerBase
	{
       

		public override string GetHelp()
		{
			return "This displays the faction leaderboard for the conquest game mode.  Usage: /leaderboard faction";
		}

		public override string GetCommandText()
		{
			return "/leaderboard faction";
		}

		public override bool IsAdminCommand()
		{
			return false;
		}

		public override bool AllowedInConsole()
		{
			return true;
		}

		public override bool HandleCommand(ulong userId, string[] words)
		{
			// Display Faction Leaderboard
            
            string flstring = "";
            #region mytestvars
 
            MyObjectBuilder_FactionCollection factionlist = MyAPIGateway.Session.GetWorld().Checkpoint.Factions;
            int num_factions = 50;//get number of factions
            ulong[,] fcleaderboard = new ulong[num_factions, 2];
            #endregion
            int position = 1;
            foreach (MyObjectBuilder_Faction faction in factionlist.Factions)
            {
                long faction_score = 0;
                List<MyObjectBuilder_FactionMember> currentfaction = faction.Members;
                foreach (MyObjectBuilder_FactionMember currentmember in currentfaction)
                {
                    var leaders = Conquest.Instance.Leaderboard.GroupBy(x => x.Value).Select(group => new { group.Key, Total = group.Count() }).OrderByDescending(x => x.Total);
                    foreach(var p in leaders)
                    {

                        if (p.Key == currentmember.PlayerId)
                        {
                            faction_score += p.Total;
                        }
                    }
                }
                if (flstring != "")
                   flstring += "\r\n";

                
                flstring += string.Format("#{0}: {1} with {2} asteroids", position, faction.Name, faction_score);
                position++;

                
            }
            Communication.DisplayDialog(userId, "Faction Leaderbored", "Current Leader", flstring);
			return true;
		}
			
	}
}

