using System;
using System.Collections.Generic;
using System.Linq;
using ConquestPlugin.Utility;
using ConquestPlugin.Utility.Shop;
using Sandbox.Common;
using Sandbox.ModAPI;
using SEModAPIExtensions.API;
using SEModAPIInternal.API.Common;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Common.ObjectBuilders.Definitions;
using NLog;
using VRage.Game;

namespace ConquestPlugin.ChatHandlers
{
    class HandleShop : ChatHandlerBase
    {
        public override string GetHelp()
        {
            return "Shop handler";
        }

        public override string GetCommandText()
        {
            return "/shop list";
        }

        public override bool IsAdminCommand()
        {
            return false;
        }

        public override bool AllowedInConsole()
        {
            return false;
        }

        public override bool IsClientOnly()
        {
            return true;
        }
        private static readonly Logger log = LogManager.GetLogger("PluginLog");
        public override bool HandleCommand(ulong userId, string[] words)
        {
            
            try
            {
                MyObjectBuilder_Faction currentfaction;
                currentfaction = Utility.Faction.getFaction(Utility.Faction.getFactionID(userId));
                foreach (MyObjectBuilder_FactionMember currentmember in currentfaction.Members)
                {
                    if (currentmember.IsLeader == true && currentmember.PlayerId == PlayerMap.Instance.GetPlayerIdsFromSteamId(userId).First())//currentmember.isleader(currentfaction)
                    {
                        string output = "";
                        output = Utility.Shop.Shop.getShopList(userId);
                        ChatUtil.DisplayDialog(userId, "Faction Store", "Spend FP Here!", output);
                        break;
                    }
                    else if (currentmember.PlayerId == PlayerMap.Instance.GetPlayerIdsFromSteamId(userId).First())
                    {
                        ChatUtil.SendPrivateChat(userId,"You do not have Permission to use this command.");
                   
                    }
                }
            }
            catch (NullReferenceException)
            {
                log.Info(string.Format("Error getting shop list nullreferenceexception"));
            }
            return true;
        }

    }
}
