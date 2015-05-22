using System;
using System.Collections.Generic;
using System.Linq;
using ConquestPlugin.Utility;
using ConquestPlugin.Utility.Shop;
using Sandbox.Common;
using Sandbox.ModAPI;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Common.ObjectBuilders.Definitions;

namespace ConquestPlugin.ChatHandlers.ShopHandles
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
            long amount = Convert.ToInt64(words[3]);
           if(Shop.buyItem(words[2],amount,(long)userId))
           {
               ChatUtil.SendPrivateChat(userId, "your purchase has been successful");
           }
           else
           {
               ChatUtil.SendPrivateChat(userId, "you have insificent funds to by this material");
           }
            ChatUtil.SendPrivateChat(userId, "player: " + userId + " bought: " + words[2] + " amount: " + words[3]);
            return true;
        }

    }
}
