using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;

using ConquestPlugin.Utility;
using ConquestPlugin.GameModes;

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
			var board = GMConquest.Instance.Leaderboard;

			string leaderResult = "";
			
			foreach (var p in board)
			{
				// leaderResult += string.Format("Here: {0} - {1}", p.Key, p.Value);
			}
			// leaderResult += "\r\n";
			
			var leaders = GMConquest.Instance.Leaderboard.GroupBy(x => x.Value).Select(group => new { group.Key, Total = group.Count() }).OrderByDescending(x => x.Total);
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

			// ChatUtil.SendPrivateChat(userId, leaderResult);
			ChatUtil.DisplayDialog(userId, "Conquest Leaderboard", "Current Leaders", leaderResult);

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
			ChatUtil.SendPublicChat("[DEBUG]: Marker 1.");
            string flstring = "";
           
            MyObjectBuilder_FactionCollection factionlist = MyAPIGateway.Session.GetWorld().Checkpoint.Factions;
           
            //int position = 1;
            //foreach (MyObjectBuilder_Faction faction in factionlist.Factions)
            //{
            //    long faction_score = 0;
            //    List<MyObjectBuilder_FactionMember> currentfaction = faction.Members;
            //    foreach (MyObjectBuilder_FactionMember currentmember in currentfaction)
            //    {
            //        var leaders = GMConquest.Instance.Leaderboard.GroupBy(x => x.Value).Select(group => new { group.Key, Total = group.Count() }).OrderByDescending(x => x.Total);
            //        foreach(var p in leaders)
            //        {

            //            if (p.Key == currentmember.PlayerId)
            //            {
            //                faction_score += p.Total;
            //            }
            //        }
            //    }
            //    if (flstring != "")
            //       flstring += "\r\n";

                
            //    flstring += string.Format("#{0}: {1} with {2} asteroids", position, faction.Name, faction_score);
            //    position++;
 
            //}
            List<FactionScores> factionleaderboard = new List<FactionScores>();
            int position = 1;
            foreach (MyObjectBuilder_Faction faction in factionlist.Factions)
            {
				ChatUtil.SendPublicChat("[DEBUG]: Marker 2.");
                long faction_score = 0;
                List<MyObjectBuilder_FactionMember> currentfaction = faction.Members;
                foreach (MyObjectBuilder_FactionMember currentmember in currentfaction)
                {
					ChatUtil.SendPublicChat("[DEBUG]: Marker 3.");
                    var leaders = GMConquest.Instance.Leaderboard.GroupBy(x => x.Value).Select(group => new { group.Key, Total = group.Count() }).OrderByDescending(x => x.Total);
                    foreach (var p in leaders)
                    {
						ChatUtil.SendPublicChat("[DEBUG]: Marker 4.");
                        if (p.Key == currentmember.PlayerId)
                        {
							ChatUtil.SendPublicChat("[DEBUG]: Marker 5.");
                            faction_score += p.Total;
                        }
                    }
                }
                factionleaderboard.Add(new FactionScores(faction.Name, faction_score));
				ChatUtil.SendPublicChat("[DEBUG]: Marker 6.");
                

                //flstring += string.Format("#{0}: {1} with {2} asteroids", position, faction.Name, faction_score);
                //position++;

            }
			// ROBS SORT
			//factionleaderboard.Sort(delegate(FactionScores x, FactionScores y)
			//{
			//	if (x.FactionScore == null && y.FactionScore == null) return 0;
			//	else if (x.FactionScore == null) return -1;
			//	else if (y.FactionScore == null) return 1;
			//	else return x.FactionScore.CompareTo(y.FactionScore);
			//});

			// SHADOWS SORT
			factionleaderboard = factionleaderboard.OrderByDescending(x => x.FactionScore).ToList();
			ChatUtil.SendPublicChat("[DEBUG]: Marker 7.");
            foreach(FactionScores score in factionleaderboard)
            {
				ChatUtil.SendPublicChat("[DEBUG]: Marker 8.");
                flstring += "\r\n #"+position+": " + score.ToString();
                position++;
            }
			ChatUtil.SendPublicChat("[DEBUG]: Marker 9.");
            ChatUtil.DisplayDialog(userId, "Faction Leaderbored", "Current Leader", flstring);
			ChatUtil.SendPublicChat("[DEBUG]: Marker 10.");
			return true;
		}

	    private List<FactionScores> sortFactions(List<FactionScores> Factions)
        {
  
            return Factions;
        }
	}

    class FactionScores : IEquatable<FactionScores>// , IComparable<FactionPoints>
    {
       
        public string FactionName {get;set;}
        public long FactionScore {get;set;}

        public FactionScores(string _FactionName, long _FactionScore)
        {

            FactionName = _FactionName;
            FactionScore = _FactionScore;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            FactionScores objaspart = obj as FactionScores;
            if (objaspart == null) return false;
            else return Equals(objaspart);
        }

        public int SortByNameAscending(long score1, long score2)
        {
            return score1.CompareTo(score2);
        }

        public long CompareTo(FactionScores CompareScore)
        {
            if (CompareScore == null)
                return 1;
            else
                return this.FactionScore.CompareTo(CompareScore.FactionScore);
        }
        public bool Equals(FactionScores other)
        {
            if (other == null) return false;
            return (this.FactionScore.Equals(other.FactionScore));
        }
        public override string ToString()
        {
            return string.Format(" {1} with {2} asteroids", FactionName, FactionScore);
        }
    }
}

