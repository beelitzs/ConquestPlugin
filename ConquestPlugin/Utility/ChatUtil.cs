using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using VRageMath;
using SEModAPIExtensions.API;
using SEModAPIInternal.API.Common;
using SEModAPIInternal.API.Entity.Sector.SectorObject;
using SEModAPIInternal.API.Entity;
using Sandbox.ModAPI;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Common;

namespace ConquestPlugin.Utility
{
	using NLog;
	class ChatUtil
	{
		private static readonly Logger Log = LogManager.GetLogger("PluginLog");
		private static Random m_random = new Random();

		public static void SendPublicChat(string message)
		{
			if (message == "") { return; }

			ChatManager.Instance.SendPublicChatMessage(message);
		}

		public static void SendPrivateChat(ulong steamID, string message)
		{
			if (message == "") { return; }

			ChatManager.Instance.SendPrivateChatMessage(steamID, message);
		}
		
		public static void DisplayDialog(ulong steamId, string header, string subheader, string content, string buttonText = "OK")
		{
			SendClientMessage(steamId, string.Format("/dialog \"{0}\" \"{1}\" \"{2}\" \"{3}\" \"{4}\"", header, subheader, " ", content.Replace("\r\n", "|"), buttonText));
		}
		
        public static void InventoryAdd(ulong steamID, string itemname, long amount )
        {
            SendClientMessage(steamID, string.Format("/invadd \"{0}\" \"{1}\"", itemname, amount));
        }

		public static void SendClientMessage(ulong steamId, string message)
		{
			if (PlayerMap.Instance.GetPlayerIdsFromSteamId(steamId).Count < 1)
			{
				Log.Info(string.Format("Unable to locate playerId for user with steamId: {0}", steamId));
				return;
			}           
			CubeGridEntity entity = new CubeGridEntity(new FileInfo(Conquest.PluginPath + "CommRelay.sbc"));
			long entityId = BaseEntity.GenerateEntityId();
			entity.EntityId = entityId;
			entity.DisplayName = string.Format("CommRelayOutput{0}", PlayerMap.Instance.GetPlayerIdsFromSteamId(steamId).First());
			entity.PositionAndOrientation = new MyPositionAndOrientation(VRageMath.GenerateRandomEdgeVector(), Vector3.Forward, Vector3.Up);

			foreach (MyObjectBuilder_CubeBlock block in entity.BaseCubeBlocks)
			{
				MyObjectBuilder_Beacon beacon = block as MyObjectBuilder_Beacon;
				if (beacon != null)
				{
                   
					beacon.CustomName = message;
				}
			}
           
			SectorObjectManager.Instance.AddEntity(entity);
			
		}
	}
}
