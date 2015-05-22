using System;
using System.Collections.Generic;

using Sandbox.Common;
using Sandbox.Common.Components;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Engine;
using Sandbox.Game;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;

namespace   ConquestPlugin.Utility.Shop
{
    class Shop
    {
        public static string getShopList()
        {
            string output = "";
            if (output != "")
                output += "\r\n";
            List<ShopItem> ShopItems = new List<ShopItem>();
            ShopItems = getshoppinglist(ShopItems);
            DynShopPrices.DynPrices(ShopItems);
            foreach(ShopItem item in ShopItems)
            {
               output += item.ToString();
            }
            return output;
        }

        public static bool buyItem(string itemname, long buyamount, long userID)
        {
            //need finishing 
            
         
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
            return "Material Name: " + ItemName + " Material Cost: " + ItemPrice;
        }
    }
}
