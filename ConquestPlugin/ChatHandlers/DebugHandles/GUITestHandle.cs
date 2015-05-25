using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConquestPlugin.Utility.GUITestCode;
namespace ConquestPlugin.ChatHandlers
{
    class GUITestHandle : ChatHandlerBase
    {
        public override string GetHelp()
        {
            return "/debug command";
        }
        public override string GetCommandText()
        {
            return "/debug command";
        }
        public override bool AllowedInConsole()
        {
            return true;
        }
        public override bool IsAdminCommand()
        {
            return false;
        }
        public override bool HandleCommand(ulong userId, string[] words)
        {
            GUITest1.createGUI(words[2]);
            return true;
        }
    }
}
