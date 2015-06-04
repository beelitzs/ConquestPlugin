﻿using System;
using System.Collections.Generic;
using System.Linq;
using ConquestPlugin.Utility;
using ConquestPlugin.Utility.Economy;
using Sandbox.ModAPI;
using SEModAPIExtensions.API;
using SEModAPIInternal.API.Common;
using Sandbox.Common;
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
            currentfaction =  Utility.Faction.getFaction(Utility.Faction.getFactionID(userId));
            long amount;
            try
            {
                amount = Convert.ToInt64(words[1]);
            }
            catch
            {
                ChatUtil.SendPrivateChat(userId, "Not a valid command.");
                return false;
            }
            foreach (MyObjectBuilder_FactionMember currentmember in currentfaction.Members)
            {
                if (currentmember.IsLeader == true && currentmember.PlayerId == PlayerMap.Instance.GetPlayerIdsFromSteamId(userId).First())
                {
                    
                    
                    if (Shop.buyItem(words[0], amount, userId))
                    {
                        ChatUtil.SendPrivateChat(userId, "Your purchase has been successful.");
                        break;
                    }
                   
                }
                else if (currentmember.PlayerId == PlayerMap.Instance.GetPlayerIdsFromSteamId(userId).First())
                {
                    ChatUtil.SendPrivateChat(userId, "You do not have Permission to use this command.");

                }
            }
            return true;
        } 

    }
}
