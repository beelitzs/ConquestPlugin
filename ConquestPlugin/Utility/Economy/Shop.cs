using System;
using System.Collections.Generic;

using ConquestPlugin.GameModes;
using ConquestPlugin.Utility;

using Sandbox.Common;
using Sandbox.Common.Components;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Definitions;
using Sandbox.Engine;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using VRage;
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
            int itemID = -1;
            foreach(ShopItem item in ShopItems)
            {
                itemID=getitemidfromitemname(itemname, userID, item);
            }
            if (itemID != -1)
            {
                ChatUtil.InventoryAdd(userID, itemID, amount);
            }
            else
            {
                ChatUtil.SendPrivateChat(userID, "item Id not found");
            }
          
            ChatUtil.SendPrivateChat(userID, "player: " + userID + " bought: " + itemname + " amount: " +  buyamount + " for: "+ amount);
            
            //MyObjectBuilder_FloatingObject floatingBuilder = new MyObjectBuilder_FloatingObject();
            //floatingBuilder.Item = new MyObjectBuilder_InventoryItem() { Amount = (VRage.MyFixedPoint)(float)buyamount, Content = new MyObjectBuilder_Ingot() { SubtypeName = "itemname" } };
            //floatingBuilder.PositionAndOrientation = new MyPositionAndOrientation()
            //{

            //};

            //var floatingObject = MyAPIGateway.Entities.CreateFromObjectBuilderAndAdd(floatingBuilder);

            return true;
        }

        public static int getitemidfromitemname(string itemname,ulong userID, ShopItem item)
        {
            var temp = new MyObjectBuilder_PhysicalObject();

                if (item != null)
                {
                    
                    if (item.ItemName == itemname)
                    {
                        temp = new MyObjectBuilder_Ingot() { SubtypeName = itemname };
                        ChatUtil.SendPrivateChat(userID, Convert.ToString(temp.GetHashCode()));
                        return  temp.GetHashCode();
                        
                    }
                    else
                    {
                        ChatUtil.SendPrivateChat(userID, "please enter a valid item name");
                    }
                }
             
            
            return 0;
        }

        private static List<ShopItem> getshoppinglist(List<ShopItem> shopitems)
        {
            
            shopitems.Add(new ShopItem("Gravel"));
            shopitems.Add(new ShopItem("Iron"));
            shopitems.Add(new ShopItem("Silicon"));
            shopitems.Add(new ShopItem("Nickel"));
            shopitems.Add(new ShopItem("Cobalt"));
            shopitems.Add(new ShopItem("Silver"));
            shopitems.Add(new ShopItem("Gold"));
            shopitems.Add(new ShopItem("Uranium"));
            shopitems.Add(new ShopItem("Magnesium"));
            shopitems.Add(new ShopItem("Platinum"));
            shopitems.Add(new ShopItem("UpgradedConstruction(WIP)"));
            shopitems.Add(new ShopItem("AdvancedConstruction(WIP)"));
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
