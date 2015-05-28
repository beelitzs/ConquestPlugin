using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Interfaces;
using VRage;

namespace DedicatedEssentials
{
	public class ServerCommandInventory : ServerCommandHandlerBase
	{
		public override string GetCommandText()
		{
			return "/invadd";
		}

		// /invadd "item" "amount"
		public override void HandleCommand(string[] words)
		{
			string[] splits = Utility.SplitString(string.Join(" ", words));

			if (splits.Length != 2)
			{
				Communication.Message(string.Format("Invalid Invadd message from server.  Inform the Admin.  Expected {0} got {1}", 2, splits.Length));
				return;
			}

			Communication.Message(string.Format("[CLIENTDEBUG]: Adding {0} number of item {1}.",splits[1],splits[0]));
			MyObjectBuilder_InventoryItem inventoryItem = new MyObjectBuilder_InventoryItem();
			inventoryItem.Amount = MyFixedPoint.DeserializeString(Convert.ToString(splits[1]));
			inventoryItem.ItemId = Convert.ToUInt32(splits[0]);
			var inventoryOwner = MyAPIGateway.Session.Player.Controller.ControlledEntity as IMyInventoryOwner;
			var inventory = inventoryOwner.GetInventory(0) as Sandbox.ModAPI.IMyInventory;
			inventory.AddItems(inventoryItem.Amount, (MyObjectBuilder_PhysicalObject)inventoryItem.Content, -1);
			Communication.Message(string.Format("[CLIENTDEBUG]: Finished Adding Items."));
			
			base.HandleCommand(words);

		}
	}
}
