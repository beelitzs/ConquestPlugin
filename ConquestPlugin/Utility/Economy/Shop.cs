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

namespace ConquestPlugin.Utility.Economy
{
    class Shop
    {           
        public static List<ShopItem> ShopItems = new List<ShopItem>();

        public static string getShopList(ulong userID)
        {
            string output = "";
            if (output != "")
                output += "\r\n";
            ShopItems.Clear();
            ShopItems = getshoppinglist(ShopItems);
            // ChatUtil.SendPublicChat("Getting shop prices");
            DynShopPrices.DynPrices(ShopItems, Faction.getFactionID(userID));
            foreach(ShopItem item in ShopItems)
            {
               output += item.ToString() + "\r\n";
            }
            return output;
        }

        public static bool buyItem(string itemname, float buyamount, ulong userID)
        {
            ShopItems = getshoppinglist(ShopItems);
            DynShopPrices.DynPrices(ShopItems,Faction.getFactionID(userID)); 
            //ChatUtil.SendPrivateChat(userID, "Buying Item.");
            float amount = -1;
            itemname.ToLower();
            itemname.ToUpper().Substring(0, 1);
            if(buyamount < 0)
            {
                ChatUtil.SendPrivateChat(userID, "Please enter a positive value.");
                return false;
            }
            foreach (ShopItem item in ShopItems)
            {
                if (item.ItemName == itemname)
                {
                    amount = item.ItemPrice * buyamount;
                }
            }
			if (amount == -1)
				{
					ChatUtil.SendPrivateChat(userID,"Item does not exist.");
					return false;
				}
			long facID = Faction.getFactionID(userID);
			int intAmount = Convert.ToInt32(amount);
			if (ChatUtil.CheckPlayerIsInWorld(userID))
			{
				if(!FactionPoints.RemoveFP(Convert.ToUInt64(facID),intAmount))
				{
				ChatUtil.SendPrivateChat(userID,"You do not have sufficient points to complete your purchase.");
				return false;
				}
			}
			else
			{
				return false;
			}
			Boolean component = false;
			switch (itemname)
			{
				case ("UpgradedConstruction"): case ("AdvancedConstruction"): case ("QuantumConstruction"):
					{
						component = true;
						break;
					}
				default: 
					break;
			}

			if (component)
			{
				ChatUtil.AddComp(userID, itemname, Convert.ToInt32(buyamount));
			}
			else
			{
				ChatUtil.AddIngot(userID, itemname, Convert.ToInt32(buyamount));
			}
            return true;
        }
        public static bool SellItem(string itemname, float buyamount, ulong userID)
        {

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
                        ChatUtil.SendPrivateChat(userID, "Please enter a valid item name.");
                    }
                }
             
            
            return 0;
        }

        private static List<ShopItem> getshoppinglist(List<ShopItem> shopitems)
        {
			shopitems.Add(new ShopItem("Stone"));
            shopitems.Add(new ShopItem("Iron"));
            shopitems.Add(new ShopItem("Silicon"));
            shopitems.Add(new ShopItem("Nickel"));
            shopitems.Add(new ShopItem("Cobalt"));
            shopitems.Add(new ShopItem("Silver"));
            shopitems.Add(new ShopItem("Gold"));
            shopitems.Add(new ShopItem("Uranium"));
            shopitems.Add(new ShopItem("Magnesium"));
            shopitems.Add(new ShopItem("Platinum"));
			//shopitems.Add(new ShopItem("UpgradedConstruction"));
			//shopitems.Add(new ShopItem("AdvancedConstruction"));
			//shopitems.Add(new ShopItem("QuantumConstruction"));
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
