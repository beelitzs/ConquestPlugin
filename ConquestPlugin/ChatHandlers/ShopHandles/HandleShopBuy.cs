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
            return "";
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
            return true;
        }

    }
}
