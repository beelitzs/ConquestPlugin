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
            return "/getfp";
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
			ChatUtil.SendPublicChat("[DEBUG]: Marker A.");
			MyObjectBuilder_Faction currentfaction;
            currentfaction = Faction.getFaction(Faction.getFactionID(userId));
            foreach (MyObjectBuilder_FactionMember currentmember in currentfaction.Members)
            {
				ChatUtil.SendPublicChat("[DEBUG]: Marker B.");
                if (currentmember.IsLeader == true)//currentmember.isleader(currentfaction)
                {
					ChatUtil.SendPublicChat("[DEBUG]: Marker C.");
					int currentFP = FactionPoints.getFP(Convert.ToUInt64( Faction.getFactionID(userId)));
                    ChatUtil.SendPrivateChat(userId,"Faction Currently has "+currentFP+" FactionPoints.");
                }
            }
			ChatUtil.SendPublicChat("[DEBUG]: Marker D.");
            return true;
        }
    }
}
