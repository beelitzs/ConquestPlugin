using System;
using System.Collections.Generic;
using System.Linq;

using ConquestPlugin.GameModes;
using ConquestPlugin.Utility;
namespace ConquestPlugin.ChatHandlers
{
    class HandleGetFP : ChatHandlerBase
    {
        public override string GetHelp()
        {
            return "Usage /GetFP";
        }
        public override string GetCommandText()
        {
            return "/GetFP";
        }
        public override bool IsAdminCommand()
        {
            return false;
        }
        public override bool AllowedInConsole()
        {
            return true;
        }
        public override bool IsClientOnly()
        {
            return true;
        }
        public override bool HandleCommand(ulong userId, string[] words)
        {
       //    ChatUtil.SendPrivateChat(userId, FactionPoints.getFP(Faction.getFactionID(userId)));
            return true;
        }
    }
}
