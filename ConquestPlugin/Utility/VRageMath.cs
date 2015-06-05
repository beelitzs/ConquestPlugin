using System;
using VRageMath;
using Sandbox.ModAPI;

namespace ConquestPlugin.Utility
{
	class VRageMath
	{
		private static Random m_random = new Random();

		public static Vector3 GenerateRandomEdgeVector()
		{
			float halfExtent = MyAPIGateway.Entities.WorldSafeHalfExtent() - 1000;
			if (halfExtent == 0f)
				halfExtent = 900000f;

			return new Vector3(GenerateRandomCoord(halfExtent), GenerateRandomCoord(halfExtent), GenerateRandomCoord(halfExtent));
		}

		public static float GenerateRandomCoord(float halfExtent)
		{
			float result = (m_random.Next(200) + halfExtent) * (m_random.Next(2) == 0 ? -1 : 1);
			return result;
		}

	}
}
