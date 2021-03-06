﻿namespace ConquestPlugin.GameModes
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Xml.Serialization;
	using ConquestPlugin.Utility;
 	using NLog;
	using Sandbox.Common.ObjectBuilders;
	using Sandbox.ModAPI;
	using SEModAPIInternal.API.Common;
	using VRage.ModAPI;
	using VRageMath;

	public class GMConquest
	{
		private static readonly Logger Log = LogManager.GetLogger( "PluginLog" );
		private static SerializableDictionary<long, long> m_ownershipCache = new SerializableDictionary<long, long>( );
		private static GMConquest m_instance = null;

		public SerializableDictionary<long, long> Leaderboard
		{
			get { return m_ownershipCache; }
		}

		public static GMConquest Instance
		{
			get 
			{
				if (m_instance == null)
				{
					Load();
				}

				return m_instance;
			}
		}

		public static void Process(bool reload = false)
		{
			Dictionary<long, long> ownership = ProcessAsteroidOwnership();
			long owner = 0;
			int count = 0;

       

			foreach (KeyValuePair<long, long> p in ownership)
			{
				if(!Instance.Leaderboard.ContainsKey(p.Key))
				{
					if(owner == 0)
						owner = p.Value;

					count++;
				}
				else if (Instance.Leaderboard[p.Key] != ownership[p.Key])
				{
					if(owner == 0)
						owner = p.Value;

					count++;
				}
			}
			
			if(count == 1)
			{
				MyObjectBuilder_Checkpoint.PlayerItem item = PlayerMap.Instance.GetPlayerItemFromPlayerId(owner);
				string name = "";
				if(item.Name != null)
					name = item.Name;

				if(!reload)
					ChatUtil.SendPublicChat(string.Format("[CONQUEST]: {0} has conquered an asteroid.", name));
			}
			else if(count > 1)
			{
				if(!reload)
					ChatUtil.SendPublicChat(string.Format("[CONQUEST]: Multiple asteroids have been conquered.  {0} asteroids have been claimed or changed ownership.", count));
			}

			bool change = false;
			foreach (KeyValuePair<long, long> L in Instance.Leaderboard) // Find if any asteroids are lost
			{
				if (!ownership.ContainsKey(L.Key))
				{
					Instance.Leaderboard.Remove(L.Key);
					MyObjectBuilder_Checkpoint.PlayerItem player = PlayerMap.Instance.GetPlayerItemFromPlayerId(L.Value);
					ChatUtil.SendPublicChat(string.Format("[CONQUEST]: {0} has lost an asteroid.", player.Name));
					change = true;
					return;
				}
			}
			foreach (KeyValuePair<long, long> p in ownership)
			{
				if (!Instance.Leaderboard.ContainsKey(p.Key))
				{
					Instance.Leaderboard.Add(p.Key, p.Value);
					change = true;
				}
				else if (Instance.Leaderboard.ContainsKey(p.Key) && Instance.Leaderboard[p.Key] != p.Value)
				{
					Instance.Leaderboard[p.Key] = p.Value;
					change = true;
				}

			}
            
			if (change)
				Save();

		}
	
		private static Dictionary<long, long> ProcessAsteroidOwnership()
		{
			Dictionary<long, long> result = new Dictionary<long, long>();
			HashSet<IMyEntity> entities = new HashSet<IMyEntity>();
			MyAPIGateway.Entities.GetEntities(entities);

			foreach (IMyEntity entity in entities)
			{
				if (!(entity is IMyVoxelMap))
					continue;
				
				if (!entity.Save)
					continue;
				IMyVoxelMap voxel = (IMyVoxelMap)entity;
				BoundingSphereD sphere = new BoundingSphereD(entity.GetPosition(), 500); // Size of sphere around Roid
				List<IMyEntity> blocks = MyAPIGateway.Entities.GetEntitiesInSphere(ref sphere);
				Dictionary<long, int> asteroidScore = new Dictionary<long, int>();

				foreach (IMyEntity block in blocks)
				{
					if (block is IMyCubeBlock)
					{
						IMyCubeBlock cube = (IMyCubeBlock)block;

						if (!(cube.GetTopMostParent() is IMyCubeGrid))
						{
							continue;
						}
						
						IMyCubeGrid parent = (IMyCubeGrid)cube.GetTopMostParent();
						if (!parent.IsStatic)
							continue;			
						if (cube.OwnerId != 0 && TestBeacon(cube)) // Test Valid Beacon.
						{

							if (!asteroidScore.ContainsKey(cube.OwnerId))
								asteroidScore.Add(cube.OwnerId, 0);

							asteroidScore[cube.OwnerId] = asteroidScore[cube.OwnerId] + 1;
						}
					}
				}

				long asteroidOwner = asteroidScore.OrderBy(x => x.Value).Where(x => x.Value > 0).Select(x => x.Key).FirstOrDefault();
				if (asteroidOwner != 0)
				{
					MyObjectBuilder_Checkpoint.PlayerItem item = PlayerMap.Instance.GetPlayerItemFromPlayerId(asteroidOwner);
					//Console.WriteLine(string.Format("Owner of asteroid at: {0} is {1}", General.Vector3DToString(entity.GetPosition()), item.Name));
					result.Add(entity.EntityId, asteroidOwner);
				}
			}

			return result;
		}

		private static Boolean TestBeacon(IMyCubeBlock block)
		{
			MyObjectBuilder_CubeBlock cube = block.GetObjectBuilderCubeBlock();
			if (cube is MyObjectBuilder_Beacon)
			{
				MyObjectBuilder_Beacon beacontest = (MyObjectBuilder_Beacon)cube;
				bool enabled = block.IsWorking;
				float radius = beacontest.BroadcastRadius;
				if (radius > 4999 && enabled)
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			return false;
		}

		private static long BeaconValue(IMyCubeBlock block) //Returns the points value of specified beacon.
		{
			MyObjectBuilder_CubeBlock cube = block.GetObjectBuilderCubeBlock();
			if (cube is MyObjectBuilder_Beacon)
			{
				MyObjectBuilder_Beacon beaconval = (MyObjectBuilder_Beacon)cube;
				float radius = beaconval.BroadcastRadius;
				return Convert.ToInt64(Math.Floor(Convert.ToDouble(radius / 5000)));
			}
			return 0;
		}

		private static void Load()
		{
			try
			{
				//String fileName = Conquest.PluginPath + "Settings-Conquest.xml";
				//if (File.Exists(fileName))
				//{
				//	using (StreamReader reader = new StreamReader(fileName))
				//	{
				//		XmlSerializer x = new XmlSerializer(typeof(Conquest));
				//		m_instance = (GMConquest)x.Deserialize(reader);
				//		reader.Close();
				//	}
				//}
				//else
					m_instance = new GMConquest();
			}
			catch (Exception ex)
			{
				m_instance = new GMConquest();
				Log.Info(string.Format("Conquest Load Error: {0}", ex.ToString()));
			}
		}

		private static void Save()
		{
			try
			{
				lock (Instance)
				{
					String fileName = Conquest.PluginPath + "Settings-Conquest.xml";
					using (StreamWriter writer = new StreamWriter(fileName))
					{
						XmlSerializer x = new XmlSerializer(typeof(GMConquest));
						x.Serialize(writer, Instance);
						writer.Close();
					}
				}
			}
			catch (Exception ex)
			{
				Log.Info(string.Format("Error saving Conquest Leaderboard: {0}", ex.ToString()));
			}
		}

	}
}
