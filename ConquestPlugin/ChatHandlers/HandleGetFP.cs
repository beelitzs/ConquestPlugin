using System;
using System.Collections.Generic;
using System.Linq;

using ConquestPlugin.GameModes;
using ConquestPlugin.Utility;
using Sandbox.Common.ObjectBuilders.Definitions;
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
            MyObjectBuilder_Faction currentfaction;
            currentfaction = Faction.getFaction(Faction.getFactionID(userId));
            foreach (MyObjectBuilder_FactionMember currentmember in currentfaction.Members)
            {
                if (currentmember.IsLeader == true)//currentmember.isleader(currentfaction)
                {
                    ChatUtil.SendPrivateChat(userId, FactionPoints.getFP((ulong)Faction.getFactionID(userId)).ToString());
                }
            }
            return true;
        }
    }
}
