///<summary>
/// Author: Halen
///
///
///
///</summary>

using System.Collections.Generic;
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
		[SerializeField] private PlayerController m_playerControllerPrefab;
		[SerializeField] private Entity m_entityPrefab;
		[SerializeField] private Transform m_cam;
		
		public const int k_Width = 16, k_Height = 9;

		private Tile[] m_tiles;

        private void Start()
        {
			m_tiles = new Tile[k_Width * k_Height];
			GenerateGrid();
			Instantiate(m_playerControllerPrefab);
        }

        private void GenerateGrid()
		{
			for (int x = 0; x < k_Width; x++)
			{
				for (int y = 0; y < k_Height; y++)
				{
					// creat the tile
					Tile spawnedTile = Instantiate(m_tilePrefab, new Vector3(x * TileMetrics.TILE_SIZE, 0, y * TileMetrics.TILE_SIZE), Quaternion.Euler(-90, 0, 0));

					// set a name to make easier to understand in the editor
					spawnedTile.name = $"Tile ({x}, {y})";

					// scale to the set size
					spawnedTile.transform.localScale = Vector3.one * TileMetrics.TILE_SIZE * 7;
					
					// attach to the grid manager to avoid cluttering the hierarchy
					spawnedTile.transform.SetParent(transform);

					// set index in array
					m_tiles[x * k_Height + y] = spawnedTile;

                    // determine the TileData
                    if (x == 0 || y == 0 || x == k_Width - 1 || y == k_Height - 1)
                        spawnedTile.Init(new TileContent(TileType.Wall));
                    else if (x == 3 && y == 4)
                    {
                        Entity spawnedEntity = Instantiate(m_entityPrefab);
                        spawnedEntity.Init(new Vector2Int(x, y));
                        spawnedTile.Init(new TileContent(TileType.Entity, spawnedEntity.gameObject));
                    }
                    else
                        spawnedTile.Init(new TileContent(TileType.Empty));
                }
			}

			m_cam.position = new Vector3((float)k_Width / 2 - 0.5f, 10, (float)k_Height / 2 - 0.5f);
			m_cam.rotation = Quaternion.Euler(90, 0, 0);
		}

		public Tile GetTileAt(int x, int y)
		{
			return m_tiles[x * k_Height + y];
		}

		public Tile GetTileAt(Vector2Int index)
		{
			return m_tiles[index.x * k_Height + index.y];
		}

		public Tile[] GetTilesInRange(Vector2Int position, Vector2Int direction, int range)
		{
			List<Tile> tiles = new List<Tile>();
			for (int r = 0; r < range; r++)
			{
				tiles.Add(GetTileAt(position + direction * (range + 1)));
			}
			return tiles.ToArray();
		}
	}
}
