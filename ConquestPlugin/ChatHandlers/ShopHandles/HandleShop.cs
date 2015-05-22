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
            return base.IsAdminCommand();
        }

        public override bool AllowedInConsole()
        {
            return base.AllowedInConsole();
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
                    string output;
                    output = Utility.Shop.Shop.getShopList();
                    ChatUtil.DisplayDialog(userId, "materials shop", "", output);
                }
                else
                {
                    ChatUtil.SendPrivateChat(userId, "you do not have premition to use this command");
                }
            }
            return true;
        }

    }
}
