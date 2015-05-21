namespace EssentialsPlugin.ProcessHandlers
{
	using EssentialsPlugin.GameModes;
	using EssentialsPlugin.Utility;

	class ProcessConquest : ProcessHandlerBase
	{
		public override int GetUpdateResolution()
		{
			return 30000; // Update in ms.
		}

		public override void Handle()
		{
			if (!PluginSettings.Instance.GameModeConquestEnabled)
				return;
			Conquest.Process();
			base.Handle();
		}
	}
}
