using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConquestPlugin.GameModes;
using ConquestPlugin.Utility;
namespace ConquestPlugin.ChatHandlers
{
    class addfpdebugHandle : ChatHandlerBase
    {
        public override string GetHelp()
        {
            return "";
        }
        public override string GetCommandText()
        {
            return "/addfp";
        }
        public override bool IsAdminCommand()
        {
            return true;
        }
        public override bool AllowedInConsole()
        {
            return false;
        }
        public override bool HandleCommand(ulong userId, string[] words)
        {
            //FactionPoints.AddFP(Faction.getFactionID(userId),Convert.ToInt32(words[0]));
            return true;
        }
    }
}
