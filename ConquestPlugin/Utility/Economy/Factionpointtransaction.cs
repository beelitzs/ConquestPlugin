using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox.Common;
using Sandbox.Common.ObjectBuilders;
using ConquestPlugin.GameModes;
using ConquestPlugin.Utility;

namespace ConquestPlugin.Utility.Economy
{
    class Factionpointtransaction
    {
        public static bool transferFP(ulong userID,string factiontag, int amount)
        {
            if (Faction.getFactionIDfromTag(factiontag) != 0)
            {
                if (FactionPoints.RemoveFP((ulong)Faction.getFactionID(userID), amount) == true)
                {
                    FactionPoints.AddFP(Faction.getFactionIDfromTag(factiontag), amount);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                ChatUtil.SendPrivateChat(userID, "Factionx tag does not exist");
                return false;
            }
        }
    }
}
