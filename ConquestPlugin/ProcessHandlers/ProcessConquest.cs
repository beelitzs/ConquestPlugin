namespace ConquestPlugin.ProcessHandlers
{
	using ConquestPlugin.GameModes;
	using ConquestPlugin.Utility;

	class ProcessConquest : ProcessHandlerBase
	{
		public override int GetUpdateResolution()
		{
			return 30000; // Update in ms.
		}

		public override void Handle()
		{
			GMConquest.Process();
			base.Handle();
		}
	}
}
