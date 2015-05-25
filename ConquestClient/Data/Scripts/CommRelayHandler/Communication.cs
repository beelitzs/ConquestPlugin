﻿using System;
using System.Collections.Generic;
using Sandbox.ModAPI;
using Sandbox.Common.ObjectBuilders;
using VRageMath;

namespace DedicatedEssentials
{
    static class Communication
    {
		private static Random m_random = new Random();
        static public void Message(String text)
        {
            MyAPIGateway.Utilities.ShowMessage("[Essentials]", text);
        }

		static public void Message(string from, string text, bool brackets = true)
		{
			MyAPIGateway.Utilities.ShowMessage(string.Format("{0}", from), text);
		}

        static public void Notification(String text, int disappearTimeMS = 2000, Sandbox.Common.MyFontEnum fontEnum = Sandbox.Common.MyFontEnum.White)
        {
            MyAPIGateway.Utilities.ShowNotification(text, disappearTimeMS, fontEnum);
        }

		static public void Dialog(string title, string prefix, string current, string description, string buttonText)
		{
			MyAPIGateway.Utilities.ShowMissionScreen(title, prefix, current, description, null, buttonText);
		}

		static public void SendMessageToServer(string text)
		{
			string commRelay = @"<?xml version=""1.0""?>
<MyObjectBuilder_CubeGrid xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
  <EntityId>0</EntityId>
  <PersistentFlags>CastShadows</PersistentFlags>
  <PositionAndOrientation>
    <Position x=""60"" y=""-5"" z=""-22.5"" />
    <Forward x=""0"" y=""-0"" z=""-1"" />
    <Up x=""0"" y=""1"" z=""0"" />
  </PositionAndOrientation>
  <GridSizeEnum>Large</GridSizeEnum>
  <CubeBlocks>
    <MyObjectBuilder_CubeBlock xsi:type=""MyObjectBuilder_Beacon"">
      <SubtypeName>LargeBlockBeacon</SubtypeName>
      <EntityId>0</EntityId>
      <Min x=""0"" y=""0"" z=""1"" />
      <IntegrityPercent>0.001</IntegrityPercent>
      <BuildPercent>0.001</BuildPercent>
      <BlockOrientation Forward=""Forward"" Up=""Up"" />
      <ColorMaskHSV x=""0"" y=""0"" z=""0"" />
      <ShareMode>All</ShareMode>
      <DeformationRatio>0</DeformationRatio>
      <ShowOnHUD>false</ShowOnHUD>
      <Enabled>false</Enabled>
      <BroadcastRadius>1</BroadcastRadius>
      <CustomName>Testing</CustomName>
    </MyObjectBuilder_CubeBlock>
  </CubeBlocks>
  <IsStatic>true</IsStatic>
  <Skeleton />
  <LinearVelocity x=""0"" y=""0"" z=""0"" />
  <AngularVelocity x=""0"" y=""0"" z=""0"" />
  <XMirroxPlane />
  <YMirroxPlane />
  <ZMirroxPlane />
  <BlockGroups />
  <Handbrake>false</Handbrake>
  <DisplayName>CommRelay</DisplayName>
</MyObjectBuilder_CubeGrid>";

			MyObjectBuilder_CubeGrid cubeGrid = MyAPIGateway.Utilities.SerializeFromXML<MyObjectBuilder_CubeGrid>(commRelay);
			cubeGrid.DisplayName = string.Format("CommRelay{0}", MyAPIGateway.Session.Player.PlayerID);
			foreach (MyObjectBuilder_CubeBlock block in cubeGrid.CubeBlocks)
			{
				if (block is MyObjectBuilder_Beacon)
				{
					MyObjectBuilder_Beacon beacon = (MyObjectBuilder_Beacon)block;
					beacon.CustomName = text;
				}
			}

			/*
			float halfExtent = MyAPIGateway.Entities.WorldSafeHalfExtent();
			if (halfExtent == 0f)
				halfExtent = 900000f;
			*/
			cubeGrid.PositionAndOrientation = new MyPositionAndOrientation(Vector3D.Zero, Vector3.Forward, Vector3.Up);
			List<MyObjectBuilder_EntityBase> addList = new List<MyObjectBuilder_EntityBase>();
			addList.Add(cubeGrid);
			MyAPIGateway.Multiplayer.SendEntitiesCreated(addList);		
		}

		private static float GenerateRandomCoord(float halfExtent)
		{
			float result = (m_random.Next(200) + halfExtent) * (m_random.Next(2) == 0 ? -1 : 1);
			return result;
		}

		public static void SendDataToServer(long dataId, string text)
		{
			string msgIdString = dataId.ToString();
			byte[] data = System.Text.Encoding.ASCII.GetBytes(text);
			byte[] newData = new byte[text.Length + msgIdString.Length + 1];
			newData[0] = (byte)msgIdString.Length;
			for (int r = 0; r < msgIdString.Length; r++)
				newData[r + 1] = (byte)msgIdString[r];

			Array.Copy(data, 0, newData, msgIdString.Length + 1, data.Length);

			MyAPIGateway.Multiplayer.SendMessageToServer(9001, newData);
		}
    }
}
