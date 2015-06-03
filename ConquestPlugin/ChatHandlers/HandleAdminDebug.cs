using SEModAPIInternal.API.Server;
using ConquestPlugin.Utility;
using ConquestPlugin.GameModes;

namespace ConquestPlugin.ChatHandlers
{

	public class HandleAdminDebug : ChatHandlerBase
	{
		public override string GetHelp()
		{
			return "Debug/Test Code.";
		}

		public override string GetCommandText()
		{
			return "/admin debug";
		}

		public override bool IsAdminCommand()
		{
			return true;
		}

		public override bool AllowedInConsole()
		{
			return true;
		}

		public override bool HandleCommand(ulong userId, string[] words)
		{
            //ChatUtil.AddComp(userId,"QuantumConstruction",1);
			return true;
		}
	}
}

