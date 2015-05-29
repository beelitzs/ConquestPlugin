using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ConquestPlugin.Utility;

namespace ConquestPlugin.ChatHandlers
{
    class HandleHelp : ChatHandlerBase
    {
        public override string GetHelp()
        {
            return "/conquest help";
        }
        public override string GetCommandText()
        {
            return "/conquest help";
        }
        public override bool HandleCommand(ulong userId, string[] words)
        {
            ChatUtil.SendPrivateChat(userId, "");
            return true;
        }
    }
}
