using System;
using System.Collections.Generic;

using ConquestPlugin.GameModes;

using Sandbox.Common;
using Sandbox.Common.Components;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Definitions;
using Sandbox.Engine;
using Sandbox.Game;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;

using SEModAPIInternal.API.Common;

namespace   ConquestPlugin.Utility.Shop
{
    class Shop
    {           
        public static List<ShopItem> ShopItems = new List<ShopItem>();

        public static string getShopList()
        {
            string output = "";
            if (output != "")
                output += "\r\n";
            ShopItems.Clear();
            ShopItems = getshoppinglist(ShopItems);
            // ChatUtil.SendPublicChat("Getting shop prices");
            DynShopPrices.DynPrices(ShopItems);
            foreach(ShopItem item in ShopItems)
            {
               output += item.ToString() + "\r\n";
            }
            return output;
        }

        public static bool buyItem(string itemname, long buyamount, ulong userID)
        {
            ShopItems = getshoppinglist(ShopItems);
            DynShopPrices.DynPrices(ShopItems);
            //need finishing 
            ChatUtil.SendPrivateChat(userID, "buying item");
            long amount = 0;
            foreach (ShopItem item in ShopItems)
            {
                if (item.ItemName == itemname)
                {
                    amount = item.ItemPrice * buyamount;
                }
            }

			long facID = Faction.getFactionID(userID);
			int intAmount = Convert.ToInt32(amount);
            if(!FactionPoints.RemoveFP(Convert.ToUInt64(facID),intAmount))
            {
               ChatUtil.SendPrivateChat(userID,"You do not have sufficent points to complete your purchuse");
               return false;
            }

            var inventoryowner = MyAPIGateway.Session.Player.Controller.ControlledEntity as Sandbox.ModAPI.Interfaces.IMyInventoryOwner;
            var iventory = inventoryowner.GetInventory(0) as Sandbox.ModAPI.IMyInventory;
            MyObjectBuilder_Base content = null;
            MyObjectBuilder_InventoryItem inventoryitem = new MyObjectBuilder_InventoryItem();
            inventoryitem.Amount = (VRage.MyFixedPoint)(float)(amount);
            inventoryitem.ItemId = 5;
            foreach(ShopItem item in ShopItems)
            {
                if(item.ItemName == itemname)
                {
                    content = new MyObjectBuilder_Ingot() { SubtypeName = itemname };
                }
                else
                {
                    ChatUtil.SendPrivateChat(userID,"please enter a valid item name");
                    return false;
                }
            }
            
            inventoryitem.Content = content;
            iventory.AddItems(inventoryitem.Amount,(MyObjectBuilder_PhysicalObject)inventoryitem.Content,-1);
            ChatUtil.SendPrivateChat(userID, "player: " + userID + " bought: " + itemname + " amount: " +  buyamount + " for: "+ amount);
            
            //MyObjectBuilder_FloatingObject floatingBuilder = new MyObjectBuilder_FloatingObject();
            //floatingBuilder.Item = new MyObjectBuilder_InventoryItem() { Amount = (VRage.MyFixedPoint)(float)buyamount, Content = new MyObjectBuilder_Ingot() { SubtypeName = "itemname" } };
            //floatingBuilder.PositionAndOrientation = new MyPositionAndOrientation()
            //{

            //};

            //var floatingObject = MyAPIGateway.Entities.CreateFromObjectBuilderAndAdd(floatingBuilder);

            return true;
        }
        private static List<ShopItem> getshoppinglist(List<ShopItem> shopitems)
        {
            
            shopitems.Add(new ShopItem("Gravel"));
            shopitems.Add(new ShopItem("IronIngots"));
            shopitems.Add(new ShopItem("SiliconWafers"));
            shopitems.Add(new ShopItem("NickelIngots"));
            shopitems.Add(new ShopItem("CobaltIngots"));
            shopitems.Add(new ShopItem("SilverIngots"));
            shopitems.Add(new ShopItem("GoldIngots"));
            shopitems.Add(new ShopItem("UraniumIngots"));
            shopitems.Add(new ShopItem("MagnesiumPowder"));
            shopitems.Add(new ShopItem("PlatinumIngots"));
            return shopitems;
        }
    }

    class ShopItem
    {
        public string ItemName { get; set; }
        public long ItemPrice { get; set; }

        public ShopItem(string itemname)
        {
            ItemName = itemname;
            ItemPrice = 0;
        }

        public override string ToString()
        {
            return "Material Name: " + ItemName + "\t\t Material Cost: " + ItemPrice;
        }
    }
}
