using System;
using System.Collections.Generic;
using System.Linq;

using ConquestPlugin.Utility;
using ConquestPlugin.Utility.Shop;
using ConquestPlugin.GameModes;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.ModAPI;

namespace ConquestPlugin.Utility.Shop
{
    class DynShopPrices
    {
        private enum itemvalues
        {
            Stone = 1,
            Iron = 5,
            Silicon = 5,
            Nickel = 7,
            Cobalt = 8,
            Silver = 8,
            Gold = 9,
            Uranium = 12,
            Magnesium = 8,
            Platinum = 8,
			UpgradedConstruction = 100,
			AdvancedConstruction = 250,
			QuantumConstruction = 1000
        }
        public static List<ShopItem> DynPrices(List<ShopItem> shopitems)
        {
            float worlddensity;
            MyObjectBuilder_PhysicalMaterialDefinition materials = new MyObjectBuilder_PhysicalMaterialDefinition();
            worlddensity = materials.Density;
            long capturedastroids = GetCapturedAstroids();
            foreach (ShopItem item in shopitems)
            {
                switch (item.ItemName)
                {
                    case ("Stone"):
                        {
                           item.ItemPrice =  GetValue(capturedastroids, (float)itemvalues.Stone);
                            break;
                        }
                    case("Iron"):
                        {
                            item.ItemPrice = GetValue(capturedastroids, (float)itemvalues.Iron);
                            break;
                        }
                    case("Silicon"):
                        {
                            item.ItemPrice = GetValue(capturedastroids, (float)itemvalues.Silicon);
                            break;
                        }
                    case("Nickel"):
                        {
                            item.ItemPrice = GetValue(capturedastroids, (float)itemvalues.Nickel);
                            break;
                        }
                    case("Cobalt"):
                        {
                            item.ItemPrice = GetValue(capturedastroids, (float)itemvalues.Cobalt);
                            break;
                        }
                    case("Silver"):
                        {
                            item.ItemPrice = GetValue(capturedastroids, (float)itemvalues.Silver);
                            break;
                        }
                    case("Gold"):
                        {
                            item.ItemPrice = GetValue(capturedastroids, (float)itemvalues.Gold);
                            break;
                        }
                    case("Uranium"):
                        {
                            item.ItemPrice = GetValue(capturedastroids, (float)itemvalues.Uranium);
                            break;
                        }
                    case("Magnesium"):
                        {
                            item.ItemPrice = GetValue(capturedastroids, (float)itemvalues.Magnesium);
                            break;
                        }
                    case("Platinum"):
                        {
                            item.ItemPrice = GetValue(capturedastroids, (float)itemvalues.Platinum);
                            break;
                        }
					case ("UpgradedConstruction"):
						{
							item.ItemPrice = GetValue(capturedastroids, (float)itemvalues.UpgradedConstruction);
							break;
						}
					case ("AdvancedConstruction"):
						{
							item.ItemPrice = GetValue(capturedastroids, (float)itemvalues.AdvancedConstruction);
							break;
						}
					case ("QuantumConstruction"):
						{
							item.ItemPrice = GetValue(capturedastroids, (float)itemvalues.QuantumConstruction);
							break;
						}
                    default:
                        break;
                }
            }
            return shopitems;
        }

        private static long GetValue(long capturedastroids , float relitivevalue)
        {
            if (capturedastroids == 0)
            {
                return (long)((long)(relitivevalue));
            }
            else
            {  
                return (long)(capturedastroids * ((long)(relitivevalue)));
            }
        }

        public static long GetCapturedAstroids()
        {
            long NumCapturedAstoids = 0;
            MyObjectBuilder_FactionCollection factionlist = MyAPIGateway.Session.GetWorld().Checkpoint.Factions;
            foreach (MyObjectBuilder_Faction faction in factionlist.Factions)
            {
                List<MyObjectBuilder_FactionMember> currentfacitonmembers = faction.Members;
                foreach(MyObjectBuilder_FactionMember currentmember in currentfacitonmembers)
                {
                    var leaders = GMConquest.Instance.Leaderboard.GroupBy(x => x.Value).Select(group => new { group.Key, Total = group.Count() }).OrderByDescending(x => x.Total);
                    foreach(var p in leaders)
                    {
                        if(p.Key == currentmember.PlayerId)
                        {
                            NumCapturedAstoids += p.Total;
                        }
                    }
                }
            }
            return NumCapturedAstoids;
        }
    }
}
