﻿using System;
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
            return "Usage /bal";
        }
        public override string GetCommandText()
        {
            return "/bal";
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
            try
            {
               currentfaction = Faction.getFaction(Faction.getFactionID(userId));
            }
            catch (NullReferenceException)
            {
                 ChatUtil.SendPrivateChat(userId, "Faction has 0 FactionPoints");
                return false;
            }
           
              
                
					int currentFP = FactionPoints.getFP(Convert.ToUInt64( Faction.getFactionID(userId)));
                    if (currentFP != -1)
                    {
                        ChatUtil.SendPrivateChat(userId, "Faction Currently has " + currentFP + " FactionPoints.");
                    }
                    else
                    {
						ChatUtil.SendPrivateChat(userId, "Faction does not exist.");
                    }
                
            
            return true;
        }
    }
}
