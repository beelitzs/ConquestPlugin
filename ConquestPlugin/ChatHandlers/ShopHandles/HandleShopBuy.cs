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
    class HandleShopBuy : ChatHandlerBase
    {
        public override string GetHelp()
        {
            return "Usage /shop buy \"itemname\" \"amount\""; 
        }
        public override string GetCommandText()
        {
            return "/shop buy";
        }
        public override bool IsAdminCommand()
        {
            return false;
        }
        public override bool AllowedInConsole()
        {
            return false;
        }
        public override bool HandleCommand(ulong userId, string[] words)
        {
            MyObjectBuilder_Faction currentfaction;
            currentfaction = Faction.getFaction(Faction.getFactionID(userId));
            foreach (MyObjectBuilder_FactionMember currentmember in currentfaction.Members)
            {
                if (currentmember.IsLeader == true)//currentmember.isleader(currentfaction)
                {
                    long amount = Convert.ToInt64(words[3]);
                    if (Shop.buyItem(words[2], amount, userId))
                    {
                        ChatUtil.SendPrivateChat(userId, "your purchase has been successful");
                        break;
                    }
                   
                }
            }
            return true;
        } 

    }
}
