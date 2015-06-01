using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using ConquestPlugin.Utility;

namespace ConquestPlugin.GameModes
{
	public class FactionPoints
	{
		private static String filename = (Conquest.PluginPath + "Faction-Points.xml");
		
		public static void CheckFP() // Create Faction Points File if not found.
		{
			if (!File.Exists(filename))
			{
				XmlDocument fileFP = new XmlDocument();
				XmlNode rootNode = fileFP.CreateElement("FactionPoints");
				fileFP.AppendChild(rootNode);

				XmlNode Faction = fileFP.CreateElement("Faction");
				XmlAttribute FactionID = fileFP.CreateAttribute("FactionID");
				XmlAttribute CurrentPoints = fileFP.CreateAttribute("CurrentPoints");
				FactionID.Value = "0";
				CurrentPoints.Value = "0";
				Faction.Attributes.Append(FactionID);
				Faction.Attributes.Append(CurrentPoints);
				rootNode.AppendChild(Faction);

				fileFP.Save(filename);
			}
			
		}

		public static void AddFP(long factionID, float addPoints) // Add Faction Points
		{
			CheckFP();
			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.Load(filename);
			
			XmlNode selectedFaction = xmlDoc.SelectSingleNode("//Faction[@FactionID='" + factionID + "']");
			try
			{
				XmlAttributeCollection factioncheck = selectedFaction.Attributes;
			}
			catch (Exception)
			{
				// Create new faction element.
				XmlNode Faction = xmlDoc.CreateElement("Faction");
				XmlAttribute FactionID = xmlDoc.CreateAttribute("FactionID");
				XmlAttribute CurrentPoints = xmlDoc.CreateAttribute("CurrentPoints");
				XmlNode rootNode = xmlDoc.SelectSingleNode("/FactionPoints");
				FactionID.Value = Convert.ToString(factionID);
				CurrentPoints.Value = Convert.ToString(addPoints); ;
				Faction.Attributes.Append(FactionID);
				Faction.Attributes.Append(CurrentPoints);
				rootNode.AppendChild(Faction);
				xmlDoc.Save(filename);
				return;
			}
			XmlAttributeCollection attributeList = selectedFaction.Attributes;
			XmlNode attributeCurrentPoints = attributeList.Item(1);
			float currentPoints = Convert.ToInt32(attributeCurrentPoints.Value);
			float newPoints = currentPoints + addPoints;
			attributeCurrentPoints.Value = Convert.ToString(newPoints);
			xmlDoc.Save(filename);

		}

		public static Boolean RemoveFP(ulong factionID, float amount) // Remove Faction Points. Return False if balance is lower than amount to remove.
		{
			CheckFP();
			if (getFP(factionID) < amount || amount < 0) { return false; }
			
			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.Load(filename);
			XmlNode selectedFaction = xmlDoc.SelectSingleNode("//Faction[@FactionID='" + factionID + "']");
			XmlAttributeCollection attributeList = selectedFaction.Attributes;
			XmlNode attributeCurrentPoints = attributeList.Item(1);
			float currentPoints = Convert.ToInt32(attributeCurrentPoints.Value);
			float newPoints = currentPoints - amount;
			attributeCurrentPoints.Value = Convert.ToString(newPoints);
			xmlDoc.Save(filename);
			return true;
		}

		public static float getFP(ulong factionID) // Return the amount of FactionPoints the user's faction has.
		{
			float currentFP;
			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.Load(filename);
			XmlNode selectedFaction = xmlDoc.SelectSingleNode("//Faction[@FactionID='" + factionID + "']");
			try
			{
				XmlAttributeCollection factionCheck = selectedFaction.Attributes;
			}
			catch (NullReferenceException)
			{
				// Faction has no entry.
				return -1;
			}
			XmlAttributeCollection attributeList = selectedFaction.Attributes;
			XmlNode attributeCurrentPoints = attributeList.Item(1);
			currentFP = Convert.ToInt32(attributeCurrentPoints.Value);
			return currentFP;
		}
	}
}
