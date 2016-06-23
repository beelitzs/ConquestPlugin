using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ConquestPlugin;
using ConquestPlugin.Utility;
using ConquestPlugin.Utility.Shop;
using ConquestPlugin.GameModes;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.ModAPI;
using VRage.Game;

namespace ConquestPlugin.Utility.Shop
{
    class DynShopPrices
    {
        private enum itemvalues
        {
            Stone = 1,
            Iron = 2,
            Silicon = 8,
            Nickel = 4,
            Cobalt = 4,
            Silver = 15,
            Gold = 15,
            Uranium = 50,
            Magnesium = 40,
            Platinum = 25,
			UpgradedConstruction = 100,
			AdvancedConstruction = 250,
			QuantumConstruction = 1000
        }
        public static List<ShopItem> DynPrices(List<ShopItem> shopitems,long FactionID)
        {
            float worlddensity;
            MyObjectBuilder_PhysicalMaterialDefinition materials = new MyObjectBuilder_PhysicalMaterialDefinition();
            worlddensity = materials.Density;
            foreach (ShopItem item in shopitems)
            {
                switch (item.ItemName)
                {
                    case ("Stone"):
                        {
                           item.ItemPrice =  GetValue(FactionID, (float)itemvalues.Stone);
                            break;
                        }
                    case("Iron"):
                        {
                            item.ItemPrice = GetValue(FactionID, (float)itemvalues.Iron);
                            break;
                        }
                    case("Silicon"):
                        {
                            item.ItemPrice = GetValue(FactionID, (float)itemvalues.Silicon);
                            break;
                        }
                    case("Nickel"):
                        {
                            item.ItemPrice = GetValue(FactionID, (float)itemvalues.Nickel);
                            break;
                        }
                    case("Cobalt"):
                        {
                            item.ItemPrice = GetValue(FactionID, (float)itemvalues.Cobalt);
                            break;
                        }
                    case("Silver"):
                        {
                            item.ItemPrice = GetValue(FactionID, (float)itemvalues.Silver);
                            break;
                        }
                    case("Gold"):
                        {
                            item.ItemPrice = GetValue(FactionID, (float)itemvalues.Gold);
                            break;
                        }
                    case("Uranium"):
                        {
                            item.ItemPrice = GetValue(FactionID, (float)itemvalues.Uranium);
                            break;
                        }
                    case("Magnesium"):
                        {
                            item.ItemPrice = GetValue(FactionID, (float)itemvalues.Magnesium);
                            break;
                        }
                    case("Platinum"):
                        {
                            item.ItemPrice = GetValue(FactionID, (float)itemvalues.Platinum);
                            break;
                        }
					case ("UpgradedConstruction"):
						{
                            item.ItemPrice = GetValue(FactionID, (float)itemvalues.UpgradedConstruction);
							break;
						}
					case ("AdvancedConstruction"):
						{
                            item.ItemPrice = GetValue(FactionID, (float)itemvalues.AdvancedConstruction);
							break;
						}
					case ("QuantumConstruction"):
						{
                            item.ItemPrice = GetValue(FactionID, (float)itemvalues.QuantumConstruction);
							break;
						}
                    default:
                        break;
                }
            }
            return shopitems;
        }

        private static long GetValue(long FactionID , float relitivevalue)
        {
            long costscale = 1;
            long difficulty = ConquestPlugin.Conquest.getdiffmod();
            try
            {
                costscale = Faction.GetFactionAstoids(Faction.getFaction(FactionID)) / Faction.GetCapturedAstroids();
            }
            catch (Exception ex)
            {
				if (ex is DivideByZeroException) { costscale = 1; }
				if (ex is InvalidOperationException) { Conquest.processingShop = false; }
            }
            if (costscale == 0)
            {
                return (long)(relitivevalue * difficulty);
            }
            else
            {
                return (long)(costscale * relitivevalue * difficulty);
            }
        }


    }
}
