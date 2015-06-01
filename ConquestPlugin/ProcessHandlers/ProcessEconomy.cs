using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Interfaces;
using Sandbox.Common;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Common.ObjectBuilders.Voxels;
using Sandbox.Common.ObjectBuilders.Definitions;
using ConquestPlugin.GameModes;
using ConquestPlugin.Utility;
using ConquestPlugin.Utility.Economy;

namespace ConquestPlugin.ProcessHandlers
{
    class ProcessEconomy : ProcessHandlerBase
    {
        public override int GetUpdateResolution()
        {
            return 360000;
        }

        public override void Handle()
        {
            MyObjectBuilder_FactionCollection factionlist = MyAPIGateway.Session.GetWorld().Checkpoint.Factions;
            foreach(MyObjectBuilder_Faction faction in factionlist.Factions)
            {
               FactionPoints.AddFP(faction.FactionId, InterestCalc((ulong)faction.FactionId));
            }
        }

        public float InterestCalc(ulong FactionId)
        {
            
            float currentpoints = FactionPoints.getFP(FactionId);
            currentpoints = currentpoints * 0.015f;
            
            return currentpoints;
        }
        public List<ShopItem> getingots()
        {
            XmlDocument phyitemdoc = new XmlDocument();
            phyitemdoc.Load(SEModAPI.API.GameInstallationInfo.GamePath + "Content\\Data\\PhysicalItem.sbc");
            List<ShopItem> Ingots = new List<ShopItem>();
            foreach(XmlNode phyitemsnode in phyitemdoc.ChildNodes)
            {
                foreach(XmlNode phyinemnode in phyitemsnode.ChildNodes)
                {
                    if(phyitemdoc.Attributes.Item(0).Attributes.Item(0).Value == "Ingot")
                    {
                        Ingots.Add(new ShopItem(phyitemdoc.Attributes.Item(0).Attributes.Item(1).Value));
                    }
                }
            }
            return Ingots;
        }
        
        public List<Componets> getcomponates()
        {
            List<Componets> componets = new List<Componets>();
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(SEModAPI.API.GameInstallationInfo.GamePath + "Content\\Data\\Blueprints.sbc");
            
            foreach(XmlNode compnode in xmldoc.ChildNodes)
            {

            }
            return componets;
        }

        public void chechFile(string _filename)
        {
            if (!File.Exists(_filename))
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Save(_filename);
            }
        }
        public void priceing()
        {
            String filename = (Conquest.PluginPath + "Shop-Prices.xml");
            chechFile(filename);
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filename);
            
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

    class Componets
    {
        public string ComponantName { get; set; }
        public List<componetprerec> Prerequisites {get; set;}

        public Componets(string _ComponantName, List<componetprerec> _prerecs)
        {
            ComponantName = _ComponantName;
            Prerequisites = _prerecs;
        }
    }

    class componetprerec
    {
        public string TypeId { get; set; }
        public string SubtypeId { get; set; }
        public float Amount { get; set; }

        public componetprerec(string _TypeId, string _SubtypeId, float _Amount)
        {
            TypeId = _TypeId;
            SubtypeId = _SubtypeId;
            Amount = _Amount;
        }
    }
}
