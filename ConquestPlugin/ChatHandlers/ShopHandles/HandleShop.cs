using System;
using System.Collections.Generic;
using System.Linq;
using ConquestPlugin.Utility;
using ConquestPlugin.Utility.Shop;
using Sandbox.Common;
using Sandbox.ModAPI;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Common.ObjectBuilders.Definitions;

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

        public override bool HandleCommand(ulong userId, string[] words)
        {
            MyObjectBuilder_Faction currentfaction;
            currentfaction = Faction.getFaction(Faction.getFactionID(userId));
            foreach (MyObjectBuilder_FactionMember currentmember in currentfaction.Members)
            {
                if (currentmember.IsLeader == true)//currentmember.isleader(currentfaction)
                {
                    string output = "";
                    output = Utility.Shop.Shop.getShopList(userId);
                    ChatUtil.DisplayDialog(userId, "Faction Store", "Spend FP Here!", output);
                    break;
                }
                else
                {
                    ChatUtil.SendPrivateChat(userId,"You do not have Permission to use this command.");
                    break;
                }
            }
            return true;
        }

    }
}
