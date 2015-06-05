using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.ModAPI;
using ConquestPlugin.GameModes;
namespace ConquestPlugin.Utility
{
    class World
    {
        public static long GetAstiroids()
        {
            HashSet<IMyEntity> entities = new HashSet<IMyEntity>();
            MyAPIGateway.Entities.GetEntities(entities);
            long Astiroids = 0;
            foreach (IMyEntity entity in entities)
            {
                if (!(entity is IMyVoxelMap))
                    continue;

                if (!(entity.Save))
                    continue;
                IMyVoxelMap voxel = (IMyVoxelMap)entity;
                Astiroids++;
            }
            return Astiroids;
        }
        public static long GetCapturedAstroids()
        {
            long NumCapturedAstoids = 0;
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
    }
}

