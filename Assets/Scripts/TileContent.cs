///<summary>
/// Author:
///
///
///
///</summary>

using UnityEngine;

namespace MysteryDungeon
{
	[System.Serializable]
	public class TileContent
	{
		private Entity m_entity = null;
		private bool m_isWall = false;

		public bool IsWalkable => m_entity == null && !m_isWall;
	}
}
