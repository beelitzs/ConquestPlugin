﻿using System;
using System.Collections.Generic;
using System.Linq;

using Sandbox.Common;
using Sandbox.ModAPI;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Common.ObjectBuilders.Definitions;

using ConquestPlugin.GameModes;
using SEModAPIInternal.API.Common;


namespace ConquestPlugin.Utility
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
					if (currentmember.PlayerId == playerID)
					{
						return faction.FactionId;   
					}
				}
			}
			return 0;
		}
        public static long getFactionIDformName(string factname)
        {
            MyObjectBuilder_FactionCollection factioncollection = MyAPIGateway.Session.GetWorld().Checkpoint.Factions;
            foreach(MyObjectBuilder_Faction faction in factioncollection.Factions)
            {
                if (faction.Name == factname)
                {
                    return faction.FactionId;
                }
            }
            return 0;
        }
        
        public static long getFactionIDfromTag(string factag)
        {
            MyObjectBuilder_FactionCollection factioncollection = MyAPIGateway.Session.GetWorld().Checkpoint.Factions;
            foreach(MyObjectBuilder_Faction faction in factioncollection.Factions)
            {
                if (faction.Tag == factag)
                {
                    return faction.FactionId;
                }
            }
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
        public static long GetCapturedAstroids()
        {
            long NumCapturedAstoids = 0;
			try // Fix for NullRefException in Shop Buy
			{
				MyObjectBuilder_FactionCollection factionlist = MyAPIGateway.Session.GetWorld().Checkpoint.Factions;
            foreach (MyObjectBuilder_Faction faction in factionlist.Factions)
            {
                List<MyObjectBuilder_FactionMember> currentfacitonmembers = faction.Members;
                foreach (MyObjectBuilder_FactionMember currentmember in currentfacitonmembers)
                {
                    var leaders = GMConquest.Instance.Leaderboard.GroupBy(x => x.Value).Select(group => new { group.Key, Total = group.Count() }).OrderByDescending(x => x.Total);
                    foreach (var p in leaders)
                    {
                        if (p.Key == currentmember.PlayerId)
                        {
                            NumCapturedAstoids += p.Total;
                        }
                    }
                }
            }
            return NumCapturedAstoids;
			}
			catch (NullReferenceException)
			{
				return 0;
			}
        }

        public static long GetFactionAstoids(MyObjectBuilder_Faction faction)
        {
            long NumCapturedAstoids = 0;
            List<MyObjectBuilder_FactionMember> currentfacitonmembers = faction.Members;
            foreach (MyObjectBuilder_FactionMember currentmember in currentfacitonmembers)
            {
                var leaders = GMConquest.Instance.Leaderboard.GroupBy(x => x.Value).Select(group => new { group.Key, Total = group.Count() }).OrderByDescending(x => x.Total);
                foreach (var p in leaders)
                {
                    if (p.Key == currentmember.PlayerId)
                    {
                        NumCapturedAstoids += p.Total;
                    }
                }
            }
            return NumCapturedAstoids;
        }

	}
}