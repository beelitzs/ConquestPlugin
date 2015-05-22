using SEModAPIInternal.API.Server;
using EssentialsPlugin.Utility;
using EssentialsPlugin.GameModes;

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
			// Execute This.
			//long playerID = 0;
			//long factionID = Faction.getFactionID((ulong)playerID);
			//Communication.SendPublicInformation(string.Format("[DEBUG]: PlayerID = {0}. Is in Faction = {1}.", playerID, factionID));
			FactionPoints.AddFP(1234567890,3);
			return true;
		}
	}
}

