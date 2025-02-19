///<summary>
/// Author:
///
///
///
///</summary>

using UnityEngine;

namespace MysteryDungeon
{
	public enum TileType
	{
		Empty,
		Wall,
		Entity,
		Stairs,
		Item,
		Interactable
	}

	[System.Serializable]
	public struct TileContent
	{
		private TileType m_type;
		public TileType Type => m_type;
		private GameObject m_objectOnTile;
		public GameObject ObjectOnTile => m_objectOnTile;

		public TileContent(TileType type, GameObject objectOnTile = null)
		{
			m_type = type;
			m_objectOnTile = objectOnTile;
		}
	}
}
