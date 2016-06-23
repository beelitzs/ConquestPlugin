using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConquestPlugin.Utility.Economy;
using ConquestPlugin.Utility;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Common.ObjectBuilders.Definitions;
using VRage.Game;

namespace ConquestPlugin.ChatHandlers
{
    class HandleFPTransfer : ChatHandlerBase
    {
        public override string GetHelp()
        {
            return "/givefp (factioname) amount";
        }
        public override string GetCommandText()
        {
            return "/givefp";
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
                    if (Factionpointtransaction.transferFP(userId, words[0], Convert.ToInt32(words[1])) == true)
                    {
                        ChatUtil.SendPrivateChat(userId, "Transfer was Sucessful.");
                        return true;
                    }
                    else
                    {
                        ChatUtil.SendPrivateChat(userId, "Transfer was Unsucessful.");
                        return false;
                    }
                }
            }
            return false;
        }
    }
}
