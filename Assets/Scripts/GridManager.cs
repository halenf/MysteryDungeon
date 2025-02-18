///<summary>
/// Author: Halen
///
///
///
///</summary>

using UnityEngine;

namespace MysteryDungeon
{
	public class GridManager : MonoBehaviour
	{
		public static GridManager Instance;
        private void Awake()
        {
			if (Instance)
				Destroy(Instance.gameObject);
			Instance = this;
        }

        [SerializeField] private Tile m_tilePrefab;
		[SerializeField] private Transform m_cam;
		
		[SerializeField] private int m_width, m_height;

		private Tile[] m_tiles;

        private void Start()
        {
			m_tiles = new Tile[m_width * m_height];
			GenerateGrid();
        }

        private void GenerateGrid()
		{
			for (int x = 0; x < m_width; x++)
			{
				for (int y = 0; y < m_height; y++)
				{
					Tile spawnedTile = Instantiate(m_tilePrefab, new Vector3(x * TileMetrics.TILE_SIZE, 0, y * TileMetrics.TILE_SIZE), Quaternion.Euler(-90, 0, 0));
					spawnedTile.name = $"Tile ({x}, {y})";
					spawnedTile.transform.localScale = Vector3.one * TileMetrics.TILE_SIZE * 7;
					spawnedTile.transform.SetParent(transform);
					m_tiles[x * m_height + y] = spawnedTile;
				}
			}

			m_cam.position = new Vector3((float)m_width / 2 - 0.5f, 10, (float)m_height / 2 - 0.5f);
			m_cam.rotation = Quaternion.Euler(90, 0, 0);
		}

		public Tile GetTile(int x, int y)
		{
			return m_tiles[x * m_height + y];
		}

		public Tile GetTile(Vector2Int index)
		{
			return m_tiles[index.x * m_height + index.y];
		}
	}
}
