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
            DynShopPrices.DynPrices(ShopItems);
            foreach(ShopItem item in ShopItems)
            {
               output += item.ToString();
            }
            return output;
        }

        public static bool buyItem(string itemname , long buyamount, long userID)
        {
            //need finishing 
            
         
            MyObjectBuilder_FloatingObject floatingBuilder = new MyObjectBuilder_FloatingObject();
            floatingBuilder.Item = new MyObjectBuilder_InventoryItem() { Amount = (VRage.MyFixedPoint)(float)buyamount, Content = new MyObjectBuilder_Ingot() { SubtypeName = "itemname" } };
            floatingBuilder.PositionAndOrientation = new MyPositionAndOrientation()
            {
                  
            };

            var floatingObject = MyAPIGateway.Entities.CreateFromObjectBuilderAndAdd(floatingBuilder);

            return true;
        }
    }

    class ShopItem
    {
        public string ItemName { get; set; }
        public long ItemPrice { get; set; }


        public override string ToString()
        {
            return "Material Name: " + ItemName + " Material Cost: " + ItemPrice;
        }
    }
}
